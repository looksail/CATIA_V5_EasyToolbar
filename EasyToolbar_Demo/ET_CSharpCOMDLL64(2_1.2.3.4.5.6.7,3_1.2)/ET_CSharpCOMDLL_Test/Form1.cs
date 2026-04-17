
using ETCSharpCOMDLL64;
using INFITF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ET_CSharpCOMDLL_Test
{
    public partial class Form1 : Form
    {
        INFITF.Application CATIA;
        bool bCatiaConnected = false;

        public Form1()
        {
            InitializeComponent();
        }

        private bool ConnectCatia()
        {
            if (bCatiaConnected) return true;

            try
            {
                CATIA = (INFITF.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Catia.Application");
                if (CATIA == null)
                {
                    return false;
                }
                bCatiaConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
            return false;
        }

        private void DisConnectCatia()
        {
            if (!bCatiaConnected) return;

            Marshal.ReleaseComObject(CATIA);

            bCatiaConnected = false;
        }

        private void Test1_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.ShowActiveDocName(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test2_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.CreateSimpleCube(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test3_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.CreateCylinder(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test4_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.ShowAsmTree(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test5_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.AddUserProperty(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test6_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.RenamePartNumber(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test10_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.OpenAndCLoseFile(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        /*
        private void Test7_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.SingleSelectEdgeFillet(ref catia);//MultiSelectEdgeFillet
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
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.MultiSelectEdgeFillet(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }

        private void Test9_Click(object sender, EventArgs e)
        {
            if (!ConnectCatia())
            {
                return;
            }

            try
            {
                ETCSharpCOMDLL64Class comObj = new ETCSharpCOMDLL64Class();

                object catia = CATIA;
                comObj.CheckConflict(ref catia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }

            DisConnectCatia();
        }
        */
    }
}

