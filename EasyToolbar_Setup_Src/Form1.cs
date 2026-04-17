
using CATIA_V5_EasyToolbar_Setup;
using CATIA_V5_EasyToolbar_Setup.Properties;
using IWshRuntimeLibrary; //Windows Script Host Object Model
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CATIA_V5_EasyToolbar_Setup
{
    public partial class Form_CATIA_V5_EasyToolbar_Setup : Form
    {
        private List<CATIAInfo> _catiaInfos = new List<CATIAInfo>();
        private VSCompatibilityResult compatibility;
        public Form_CATIA_V5_EasyToolbar_Setup()
        {
            InitializeComponent();
            InitListView();
        }
        private void InitListView()
        {
            listViewCATIA.View = View.Details;
            listViewCATIA.CheckBoxes = true;
            listViewCATIA.FullRowSelect = true;
            listViewCATIA.GridLines = true;

            listViewCATIA.Columns.Add("Version", 120);
            listViewCATIA.Columns.Add("CATIAInstallPath", 320);

            //VC Redist
            listViewVCRedist.View = View.Details;
            listViewVCRedist.FullRowSelect = true;
            listViewVCRedist.GridLines = true;

            listViewVCRedist.Columns.Add("Product Name", 320);
            listViewVCRedist.Columns.Add("Version", 120);

            List<VCRedistInfo> installedVCRedists = GetInstalledVCRedists();
            installedVCRedists = SortVCRedistByVersion(installedVCRedists);
            if (installedVCRedists.Count > 0)
            {
                foreach (var vcredist in installedVCRedists)
                {
                    ListViewItem item = new ListViewItem(vcredist.ProductName);
                    item.SubItems.Add(vcredist.Version);
                    listViewVCRedist.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No installed VC++ Redistributables found");
            }

            compatibility = CheckVSCompatibility(installedVCRedists);
        }
        private void Form_CATIA_V5_EasyToolbar_Setup_Load(object sender, EventArgs e)
        {
            LoadCATIAInfo();

            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void LoadCATIAInfo()
        {
            try
            {
                string registryPath = @"SOFTWARE\Dassault Systemes";
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(registryPath))
                {
                    if (baseKey == null)
                    {
                        return;
                    }

                    _catiaInfos.Clear();
                    listViewCATIA.Items.Clear();

                    foreach (string subKeyName in baseKey.GetSubKeyNames())
                    {
                        if (subKeyName.StartsWith("B"))
                        {
                            string fullSubKeyPath = $"{subKeyName}\\0";//The path may not only be directory 0, but also 1, 2, 3, etc. Further improvements are needed.
                            using (RegistryKey subKey = baseKey.OpenSubKey(fullSubKeyPath))
                            {
                                if (subKey != null)
                                {
                                    object CATIAInstallPathObj = subKey.GetValue("DEST_FOLDER_OSDS");
                                    string CATIAInstallPath = CATIAInstallPathObj?.ToString() ?? "Path not found.";

                                    CATIAInfo catiaInfo = new CATIAInfo
                                    {
                                        Version = subKeyName,
                                        CATIAInstallPath = CATIAInstallPath
                                    };

                                    _catiaInfos.Add(catiaInfo);

                                    ListViewItem item = new ListViewItem(catiaInfo.Version);
                                    item.SubItems.Add(catiaInfo.CATIAInstallPath);
                                    item.Tag = catiaInfo;

                                    listViewCATIA.Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Insufficient permissions! Please run this program as an administrator.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read the registry: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonSetup_Click(object sender, EventArgs e)
        {
            //Setup Path
            string easytoolbarSetupPath = textBoxSetupPath.Text.Trim();
            if (string.IsNullOrEmpty(easytoolbarSetupPath) || !Directory.Exists(easytoolbarSetupPath))
            {
                MessageBox.Show("Please select a valid EasyToolbar setup path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            easytoolbarSetupPath = Path.Combine(easytoolbarSetupPath, "CATIA_V5_EasyToolbar");
            if (!Directory.Exists(easytoolbarSetupPath))
            {
                Directory.CreateDirectory(easytoolbarSetupPath);
            }

            //CATIA Version
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string catenvPath = Path.Combine(appdataPath, "DassaultSystemes", "CATEnv");
            if (!Directory.Exists(catenvPath))
            {
                Directory.CreateDirectory(catenvPath);
            }
            string catenvFilename = Path.Combine(catenvPath, "CATIA_V5_EasyToolbar");
            List<CATIAInfo> selectedInfos = GetSelectedCATIAInfos();
            if (selectedInfos.Count <= 0)
            {
                MessageBox.Show("Please select CATIA version.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int nVersion = -1;
            for (int i = 0; i < selectedInfos.Count; i++)
            {
                nVersion = ExtractTrailingNumbers(selectedInfos[i].Version);
                if (nVersion < 18)
                {
                    MessageBox.Show("Unsupported version : " + selectedInfos[i].Version, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Copy file
            string exeFullPath = Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exeFullPath);
            if (string.IsNullOrEmpty(exeDir))
            {
                exeDir = Environment.CurrentDirectory;
            }

            CopyFolder(Path.Combine(exeDir, "1"), Path.Combine(easytoolbarSetupPath, "1"));
            CopyFolder(Path.Combine(exeDir, "2"), Path.Combine(easytoolbarSetupPath, "2"));
            CopyFolder(Path.Combine(exeDir, "3"), Path.Combine(easytoolbarSetupPath, "3"));

            //Write Settting File & Create Shortcut
            //C:\Users\Administrator\AppData\Roaming\DassaultSystemes\CATEnv
            for (int i = 0; i < selectedInfos.Count; i++)
            {
                string easytoolbarSetupPathTemp = easytoolbarSetupPath;
                nVersion = ExtractTrailingNumbers(selectedInfos[i].Version);
                if (nVersion < 28)
                {
                    if (!compatibility.SupportVS2005)
                    {
                        MessageBox.Show("Please install the MSVC Redistributable that supports VC2005 : " + selectedInfos[i].Version, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    easytoolbarSetupPathTemp = Path.Combine(easytoolbarSetupPathTemp, "1", "win_b64");
                }
                else if (nVersion < 33)
                {
                    easytoolbarSetupPathTemp = Path.Combine(easytoolbarSetupPathTemp, "2", "win_b64");
                    if (!compatibility.SupportVS2015)
                    {
                        MessageBox.Show("Please install the MSVC Redistributable that supports VC2015 : " + selectedInfos[i].Version, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    easytoolbarSetupPathTemp = Path.Combine(easytoolbarSetupPathTemp, "3", "win_b64");
                    if (!compatibility.SupportVS2019)
                    {
                        MessageBox.Show("Please install the MSVC Redistributable that supports VC2019 : " + selectedInfos[i].Version, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string catenvFilenameTemp = catenvFilename + "_" + selectedInfos[i].Version + ".txt";
                System.IO.File.Delete(catenvFilenameTemp);

                string templateFilename = Path.Combine(Application.StartupPath, "CATEnv_Easytoolbar.txt");
                System.IO.File.Copy(templateFilename, catenvFilenameTemp, true);

                string fileContent = System.IO.File.ReadAllText(catenvFilenameTemp, new System.Text.UTF8Encoding(false));
                string newContent = fileContent.Replace("CATIADir", selectedInfos[i].CATIAInstallPath);
                newContent = newContent.Replace("EasytoolbarDir", easytoolbarSetupPathTemp);
                System.IO.File.WriteAllText(catenvFilenameTemp, newContent, new System.Text.UTF8Encoding(false));

                //lnk
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string desktopShortcutPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(catenvFilenameTemp) + ".lnk");

                string arguments = $"- run \"CNEXT.exe\" -env {Path.GetFileNameWithoutExtension(catenvFilenameTemp)} -direnv \"{catenvPath}\" -nowindow";

                CreateShortcutWithArguments(desktopShortcutPath, arguments, selectedInfos[i].CATIAInstallPath);
            }

            //COMDLL COMDLL64 need regasm/regsvr32
            InstallReg(easytoolbarSetupPath);

            //Finish
            DialogResult result = MessageBox.Show(
                "Installed successfully. Do you want to exit the installer?",
                "Installation Complete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void InstallReg(string easytoolbarSetupPath)
        {
            string exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            string configPath = Path.Combine(exeDir, "Setup_RegDllConfig.txt");
            if (!System.IO.File.Exists(configPath))
            {
                MessageBox.Show($"Configuration file does not exist: {configPath}");
                return;
            }
            System.IO.File.Copy(configPath, Path.Combine(easytoolbarSetupPath, "Setup_RegDllConfig.txt"),true);

            string[] lines = System.IO.File.ReadAllLines(configPath, Encoding.UTF8);

            foreach (string line in lines)
            {
                string trimLine = line.Trim();
                if (string.IsNullOrEmpty(trimLine) || trimLine.StartsWith("#"))
                    continue;

                string[] parts = trimLine.Split('|');
                if (parts.Length != 3)
                {
                    MessageBox.Show($"Invalid configuration format, skip this line: {line}");
                    continue;
                }

                string regType = parts[0].Trim().ToLower();
                string regFlagStr = parts[1].Trim();
                string dllPath = parts[2].Trim();

                int regFlag;
                if (!int.TryParse(regFlagStr, out regFlag) || (regFlag != 1 && regFlag != -1))
                {
                    MessageBox.Show($"Invalid registration flag (only 1/-1 supported), skip: {line}");
                    continue;
                }
                bool unregister = regFlag == -1;

                if (!System.IO.File.Exists(dllPath))
                {
                    MessageBox.Show($"DLL file does not exist, skip: {dllPath}");
                    continue;
                }

                try
                {
                    if (regType == "regsvr32")
                    {
                        Regsvr32ComDll(dllPath, true, true);
                    }
                    else if (regType == "regasm")
                    {
                        RegAsmComDll(dllPath, true, true);
                    }
                    else
                    {
                        MessageBox.Show($"Invalid registration type, skip: {dllPath} (Type: {regType})");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Process failed: {dllPath} - {ex.Message}");
                }
            }
        }
        private bool RegAsmComDll(string dllPath, bool is64bit = true,bool isRegister = true)
        {
            if (!System.IO.File.Exists(dllPath))
            {
                return false;
            }

            string frameworkDir = is64bit
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.NET", "Framework64", "v4.0.30319")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.NET", "Framework", "v4.0.30319");

            string regAsmPath = Path.Combine(frameworkDir, "regasm.exe");
            if (!System.IO.File.Exists(regAsmPath))
            {
                return false;
            }

            StringBuilder argsBuilder = new StringBuilder();
            if (isRegister)
            {
                argsBuilder.Append("/codebase ");
                argsBuilder.Append("/tlb ");
            }
            else
            {
                argsBuilder.Append("/unregister ");
            }
            argsBuilder.Append($"\"{dllPath}\"");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = regAsmPath,
                Arguments = argsBuilder.ToString(),
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            try
            {
                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Regasm error ：{error}\r\n\r\n：{output}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception during COM component regasm ：{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private bool Regsvr32ComDll(string dllPath, bool is64Bit, bool isRegister)
        {
            if (!System.IO.File.Exists(dllPath))
            {
                return false;
            }

            string systemDir = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string regsvr32Path = is64Bit
                ? Path.Combine(systemDir, "regsvr32.exe") 
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "regsvr32.exe");

            if (!System.IO.File.Exists(regsvr32Path))
            {
                return false;
            }

            string args = isRegister
                ? $"/s \"{dllPath}\""
                : $"/s /u \"{dllPath}\"";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = regsvr32Path,
                Arguments = args,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,         
                RedirectStandardOutput = true, 
                RedirectStandardError = true,  
                UseShellExecute = false,       
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            try
            {
                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Regsvr32 error ：{error}\r\n\r\n：{output}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception during COM component regsvr32 ：{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void CopyFolder(string sourceDirPath, string targetDirPath, bool overwriteExisting = true, bool copyEmptyDirs = true)
        {
            if (!Directory.Exists(sourceDirPath))
            {
                MessageBox.Show($"The source folder does not exist：{sourceDirPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(targetDirPath))
            {
                Directory.CreateDirectory(targetDirPath);
            }

            try
            {
                foreach (string filePath in Directory.GetFiles(sourceDirPath))
                {
                    try
                    {
                        string fileName = Path.GetFileName(filePath);
                        string targetFilePath = Path.Combine(targetDirPath, fileName);

                        System.IO.File.Copy(filePath, targetFilePath, overwriteExisting);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to copy file：{filePath}\r\n\r\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                foreach (string subDirPath in Directory.GetDirectories(sourceDirPath))
                {
                    string subDirName = Path.GetFileName(subDirPath);
                    string targetSubDirPath = Path.Combine(targetDirPath, subDirName);

                    CopyFolder(subDirPath, targetSubDirPath, overwriteExisting, copyEmptyDirs);

                    if (copyEmptyDirs && !Directory.Exists(targetSubDirPath))
                    {
                        Directory.CreateDirectory(targetSubDirPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy folder：{sourceDirPath}\r\n\r\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private int ExtractTrailingNumbers(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return -1;
            }
            int lastDigitIndex = input.Length - 1;
            while (lastDigitIndex >= 0 && char.IsDigit(input[lastDigitIndex]))
            {
                lastDigitIndex--;
            }
            string numberString = input.Substring(lastDigitIndex + 1);
            if (string.IsNullOrEmpty(numberString))
            {
                return -1;
            }
            return int.Parse(numberString);
        }
        private void CreateShortcutWithArguments(string shortcutPath, string arguments,string CATIAInstallPath)
        {
            try
            {
                string catstartExePath = $@"{CATIAInstallPath}\code\bin\CATSTART.exe";
                if (!System.IO.File.Exists(catstartExePath))
                {
                    return;
                }
                string cnextExePath = $@"{CATIAInstallPath}\code\bin\CNEXT.exe";

                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = catstartExePath;
                shortcut.Arguments = arguments;
                shortcut.WorkingDirectory = "";
                shortcut.Description = "";
                shortcut.IconLocation = $"{cnextExePath},0";
                shortcut.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private List<CATIAInfo> GetSelectedCATIAInfos()
        {
            List<CATIAInfo> selectedItems = new List<CATIAInfo>();

            foreach (ListViewItem item in listViewCATIA.Items)
            {
                if (item.Checked)
                {
                    if (item.Tag is CATIAInfo catiaInfo)
                    {
                        selectedItems.Add(catiaInfo);
                    }
                }
            }

            return selectedItems;
        }
        private void buttonSelSetupPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Dir";
                folderDialog.ShowNewFolderButton = true;
                folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolderPath = folderDialog.SelectedPath;
                    textBoxSetupPath.Text = selectedFolderPath;
                }
            }
        }
        private List<VCRedistInfo> GetInstalledVCRedists()
        {
            List<VCRedistInfo> vcredistList = new List<VCRedistInfo>();

            string[] registryPaths = new[]
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (string regPath in registryPaths)
            {
                try
                {
                    RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey uninstallKey = baseKey.OpenSubKey(regPath);

                    if (uninstallKey == null) continue;

                    foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                    {
                        using (RegistryKey subKey = uninstallKey.OpenSubKey(subKeyName))
                        {
                            if (subKey == null) continue;

                            try
                            {
                                object productNameObj = subKey.GetValue("DisplayName");
                                if (productNameObj == null) continue;

                                string productName = productNameObj.ToString().Trim();

                                if (productName.Contains("Visual C++"))
                                {
                                    VCRedistInfo info = new VCRedistInfo();
                                    info.ProductName = productName;
                                    info.Version = subKey.GetValue("DisplayVersion")?.ToString() ?? "Unknown";
                                    info.Publisher = subKey.GetValue("Publisher")?.ToString() ?? "";
                                    info.InstallLocation = subKey.GetValue("InstallLocation")?.ToString() ?? "";

                                    if (productName.Contains("x64") || productName.Contains("64-bit"))
                                        info.Architecture = "x64";
                                    else if (productName.Contains("x86") || productName.Contains("32-bit"))
                                        info.Architecture = "x86";
                                    else
                                        info.Architecture = regPath.Contains("WOW6432Node") ? "x86" : "x64";

                                    if (!vcredistList.Exists(x => x.ProductName == info.ProductName && x.Architecture == info.Architecture))
                                        vcredistList.Add(info);
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            }

            return vcredistList;
        }
        private List<VCRedistInfo> SortVCRedistByVersion(List<VCRedistInfo> list)
        {
            return list.OrderBy(v =>
            {
                if (Version.TryParse(v.Version, out Version ver))
                {
                    return ver;
                }
                return new Version(int.MaxValue, int.MaxValue);
            }).ToList();
        }
        private VSCompatibilityResult CheckVSCompatibility(List<VCRedistInfo> vcredistList)
        {
            VSCompatibilityResult result = new VSCompatibilityResult();

            foreach (var vcredist in vcredistList)
            {
                if (vcredist.Architecture != "x64")
                {
                    continue;
                }

                if (vcredist.Version == "Unknown Version" || !Version.TryParse(vcredist.Version, out Version ver))
                {
                    continue;
                }

                if (ver.Major >= 9)
                {
                    result.SupportVS2005 = true;
                }

                if (ver.Major == 14)
                {
                    result.SupportVS2015 = true;
                    if (ver.Minor >= 20)
                    {
                        result.SupportVS2019 = true;
                    }
                }
            }
            return result;
        }
    }

    public class CATIAInfo
    {
        public string Version { get; set; }
        public string CATIAInstallPath { get; set; }
    }

    public class VCRedistInfo
    {
        public string ProductName { get; set; }
        public string Version { get; set; }
        public string Architecture { get; set; }
        public string Publisher { get; set; }
        public string InstallLocation { get; set; }
    }

    public class VSCompatibilityResult
    {
        public bool SupportVS2005 { get; set; }
        public bool SupportVS2015 { get; set; }
        public bool SupportVS2019 { get; set; }
    }
}
