using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


namespace ET_CSharpEXE
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormEXE2 f = new FormEXE2();
            if (args.Length > 0)
            {
                string rotKey = args[0];
                f.rotKey = rotKey;
            }

            Application.Run(f);
        }
    }

    public static class GetEasyToolbarCATIA
    {
        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        [DllImport("ole32.dll")]
        private static extern int CreateItemMoniker(string lpszDelim, string lpszItem, out IMoniker ppmk);

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        public static INFITF.Application GetCATIAFromROT(string rotKey)
        {
            IRunningObjectTable rot = null;
            IMoniker moniker = null;
            IBindCtx bindCtx = null;

            try
            {
                List<string> rotEntries = GetAllROTEntries();
                //MessageBox.Show($"All objects in ROT table:\n{string.Join("\n", rotEntries)}", "ROT Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!rotEntries.Contains(rotKey))
                {
                    MessageBox.Show($"Identifier not found in ROT: {rotKey}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                int hr = CreateBindCtx(0, out bindCtx);
                if (hr != 0 || bindCtx == null)
                {
                    MessageBox.Show($"Failed to create BindCtx, error code: {hr}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                hr = GetRunningObjectTable(0, out rot);
                if (hr != 0 || rot == null)
                {
                    MessageBox.Show($"Failed to open ROT, error code: {hr}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                hr = CreateItemMoniker(null, rotKey, out moniker);
                if (hr != 0 || moniker == null)
                {
                    MessageBox.Show($"Failed to create Moniker, error code: {hr}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                object catiaObj = null;
                try
                {
                    moniker.BindToObject(bindCtx, null, typeof(INFITF.Application).GUID, out catiaObj);
                }
                catch
                {
                    IEnumMoniker enumMoniker = null;
                    IMoniker[] monikerArr = new IMoniker[1];
                    IntPtr fetched = IntPtr.Zero;
                    try
                    {
                        rot.EnumRunning(out enumMoniker);
                        enumMoniker.Reset();

                        while (enumMoniker.Next(1, monikerArr, fetched) == 0)
                        {
                            string displayName = null;
                            monikerArr[0].GetDisplayName(bindCtx, null, out displayName);

                            if (displayName == rotKey)
                            {
                                rot.GetObject(monikerArr[0], out catiaObj);
                                break;
                            }

                            Marshal.ReleaseComObject(monikerArr[0]);
                        }
                    }
                    finally
                    {
                        if (enumMoniker != null) Marshal.ReleaseComObject(enumMoniker);
                    }
                }

                if (catiaObj == null)
                {
                    MessageBox.Show($"Found rotKey[{rotKey}], but the object is null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                if (catiaObj is INFITF.Application catiaApp)
                {
                    try
                    {
                        //MessageBox.Show($"Successfully obtained CATIA object!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return catiaApp;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"CATIA object is invalid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show($"Object type error, actual type: {catiaObj.GetType().FullName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General exception: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (moniker != null) Marshal.ReleaseComObject(moniker);
                if (rot != null) Marshal.ReleaseComObject(rot);
                if (bindCtx != null) Marshal.ReleaseComObject(bindCtx);
            }
        }
        private static List<string> GetAllROTEntries()
        {
            List<string> entries = new List<string>();
            IRunningObjectTable rot = null;
            IEnumMoniker enumMoniker = null;
            IMoniker[] moniker = new IMoniker[1];
            IntPtr fetched = IntPtr.Zero;
            IBindCtx bindCtx = null;

            try
            {
                if (GetRunningObjectTable(0, out rot) == 0 && rot != null)
                {
                    rot.EnumRunning(out enumMoniker);
                    enumMoniker.Reset();

                    if (CreateBindCtx(0, out bindCtx) == 0)
                    {
                        while (enumMoniker.Next(1, moniker, fetched) == 0)
                        {
                            string displayName = null;
                            moniker[0].GetDisplayName(bindCtx, null, out displayName);
                            if (!string.IsNullOrEmpty(displayName))
                            {
                                entries.Add(displayName);
                            }
                            Marshal.ReleaseComObject(moniker[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to traverse ROT: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (enumMoniker != null) Marshal.ReleaseComObject(enumMoniker);
                if (rot != null) Marshal.ReleaseComObject(rot);
                if (bindCtx != null) Marshal.ReleaseComObject(bindCtx);
            }

            return entries;
        }
    }
}
