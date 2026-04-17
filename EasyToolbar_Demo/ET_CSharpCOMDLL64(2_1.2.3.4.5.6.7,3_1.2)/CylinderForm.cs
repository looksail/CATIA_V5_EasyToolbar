using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETCSharpCOMDLL
{
    public partial class CylinderForm : Form
    {
        public double diameter { get; private set; }
        public double height { get; private set; }

        public CylinderForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!IsNumberByTryCatch(textBox_diameter.Text) ||
                !IsNumberByTryCatch(textBox_height.Text) )
            {
                MessageBox.Show("Input Error");
                return;
            }

            diameter = double.Parse(textBox_diameter.Text);
            height = double.Parse(textBox_height.Text);

            // 3. 设置窗体返回结果，关闭弹窗
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public static bool IsNumberByTryCatch(string input)
        {
            try
            {
                double.Parse(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }
    }
}
