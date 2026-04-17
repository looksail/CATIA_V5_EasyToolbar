using INFITF;
using MECMOD;
using PARTITF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ET_CSharpEXE
{
    public partial class FormEXE2 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public string rotKey;

        public INFITF.Application catia;
        public bool bCatiaConnected = false;

        public FormEXE2()
        {
            InitializeComponent();
        }
        private bool ConnectCatia()
        {
            if (bCatiaConnected) return true;

            try
            {
                if (!string.IsNullOrEmpty(rotKey))
                {
                    catia = GetEasyToolbarCATIA.GetCATIAFromROT(rotKey);
                }
                if (catia == null)
                {
                    //catia = (INFITF.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Catia.Application");
                    //if (catia != null)
                    //{
                    //    bCatiaConnected = true;
                    //    return true;
                    //}
                    MessageBox.Show("Connect CATIA Failed");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ConnectCatia \r\n\r\n" + ex.ToString());
            }
            return false;
        }
        private void DisConnectCatia()
        {
            if (!bCatiaConnected) return;

            Marshal.ReleaseComObject(catia);
            bCatiaConnected = false;
        }
        public void SingleSelectEdgeFillet()
        {
            if (catia == null)
            {
                return;
            }

            SingleSelectEdgeFilletFunction(ref catia);
        }
        public void MultiSelectEdgeFillet()
        {
            if (catia == null)
            {
                return;
            }

            MultiSelectEdgeFilletFunction(ref catia);
        }
        private void SingleSelectEdgeFilletFunction(ref INFITF.Application catia)
        {
            PartDocument partDoc = null;
            Part part = null;
            Selection sel = null;
            ShapeFactory shapeFactory = null;
            double Radius = 5.0;

            try
            {
                string extension = Path.GetExtension(catia.ActiveDocument.FullName).ToLower();
                if (extension != ".catpart")
                {
                    MessageBox.Show("Please switch to the Part document to fillet");
                    return;
                }

                partDoc = (PartDocument)catia.ActiveDocument;
                part = partDoc.Part;
                sel = partDoc.Selection;
                sel.Clear();

                object[] filter = new object[] { "Edge" };
                string status = sel.SelectElement2(filter, "Please select one edge(press Esc to cancel).", true);

                if (status == "Normal")
                {
                    Reference edgeRef = sel.Item(1).Reference;

                    ShapeFactory shapefactory = (ShapeFactory)part.ShapeFactory;
                    EdgeFillet edgeFillet = shapefactory.AddNewEdgeFilletWithConstantRadius(
                                       edgeRef, CatFilletEdgePropagation.catTangencyFilletEdgePropagation, Radius);

                    part.Update();
                    MessageBox.Show($"Success! Created fillet for 1 edge(s) (Radius: {Radius}).");
                }
                else
                {
                    MessageBox.Show("No edge selected or selection canceled.");
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("C# COMDLL64 SelectEdgeFilletFunction \r\n\r\n COM Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("C# COMDLL64 SelectEdgeFilletFunction \r\n\r\n Error：" + ex.Message);
            }
            finally
            {
                if (shapeFactory != null) Marshal.ReleaseComObject(shapeFactory);
                if (sel != null) Marshal.ReleaseComObject(sel);
                if (part != null) Marshal.ReleaseComObject(part);
                if (partDoc != null) Marshal.ReleaseComObject(partDoc);
            }
        }
        private void MultiSelectEdgeFilletFunction(ref INFITF.Application catia)
        {
            PartDocument partDoc = null;
            Part part = null;
            Selection sel = null;
            ShapeFactory shapeFactory = null;
            double Radius = 5.0;

            try
            {
                string extension = Path.GetExtension(catia.ActiveDocument.FullName).ToLower();
                if (extension != ".catpart")
                {
                    MessageBox.Show("Please switch to the Part document to fillet");
                    return;
                }

                partDoc = (PartDocument)catia.ActiveDocument;
                part = partDoc.Part;
                sel = partDoc.Selection;
                sel.Clear();

                object[] filter = new object[] { "Edge" };
                string status = sel.SelectElement3(filter, "Please select one or more edges (hold Ctrl to multi-select).",
                                                   true, CATMultiSelectionMode.CATMultiSelTriggWhenUserValidatesSelection, true);
                if (status == "Normal")
                {
                    int selectedCount = sel.Count;
                    if (selectedCount == 0)
                    {
                        MessageBox.Show("No edge selected or selection canceled.");
                        return;
                    }

                    shapeFactory = (ShapeFactory)part.ShapeFactory;

                    for (int i = 1; i <= selectedCount; i++)
                    {
                        Reference edgeRef = sel.Item(i).Reference;

                        EdgeFillet edgeFillet = shapeFactory.AddNewEdgeFilletWithConstantRadius(
                            edgeRef,
                            CatFilletEdgePropagation.catTangencyFilletEdgePropagation,
                            Radius);
                    }

                    part.Update();
                    MessageBox.Show($"Success! Created fillet for {selectedCount} edge(s) (Radius: {Radius}).");
                }
                else
                {
                    MessageBox.Show("No edge selected or selection canceled.");
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("C# COMDLL64 MultiSelectEdgeFilletFunction \r\n\r\n COM Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("C# COMDLL64 MultiSelectEdgeFilletFunction \r\n\r\n Error：" + ex.Message);
            }
            finally
            {
                if (shapeFactory != null) Marshal.ReleaseComObject(shapeFactory);
                if (sel != null) Marshal.ReleaseComObject(sel);
                if (part != null) Marshal.ReleaseComObject(part);
                if (partDoc != null) Marshal.ReleaseComObject(partDoc);
            }
        }

        private void Test7_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                SingleSelectEdgeFillet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test8_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                MultiSelectEdgeFillet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }
    }
}
