namespace CATIA_V5_EasyToolbar_Setup
{
    partial class Form_CATIA_V5_EasyToolbar_Setup
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listViewCATIA = new System.Windows.Forms.ListView();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSetupPath = new System.Windows.Forms.TextBox();
            this.buttonSelSetupPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewVCRedist = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewCATIA
            // 
            this.listViewCATIA.HideSelection = false;
            this.listViewCATIA.Location = new System.Drawing.Point(-1, 63);
            this.listViewCATIA.Name = "listViewCATIA";
            this.listViewCATIA.Size = new System.Drawing.Size(626, 260);
            this.listViewCATIA.TabIndex = 0;
            this.listViewCATIA.UseCompatibleStateImageBehavior = false;
            // 
            // buttonSetup
            // 
            this.buttonSetup.Location = new System.Drawing.Point(260, 580);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(105, 33);
            this.buttonSetup.TabIndex = 1;
            this.buttonSetup.Text = "Setup";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 517);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Easytoolbar Setup Path:";
            // 
            // textBoxSetupPath
            // 
            this.textBoxSetupPath.Location = new System.Drawing.Point(8, 544);
            this.textBoxSetupPath.Name = "textBoxSetupPath";
            this.textBoxSetupPath.Size = new System.Drawing.Size(558, 25);
            this.textBoxSetupPath.TabIndex = 3;
            // 
            // buttonSelSetupPath
            // 
            this.buttonSelSetupPath.Location = new System.Drawing.Point(574, 544);
            this.buttonSelSetupPath.Name = "buttonSelSetupPath";
            this.buttonSelSetupPath.Size = new System.Drawing.Size(50, 25);
            this.buttonSelSetupPath.TabIndex = 4;
            this.buttonSelSetupPath.Text = "...";
            this.buttonSelSetupPath.UseVisualStyleBackColor = true;
            this.buttonSelSetupPath.Click += new System.EventHandler(this.buttonSelSetupPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 342);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Installed MSVC Redistributable";
            // 
            // listViewVCRedist
            // 
            this.listViewVCRedist.HideSelection = false;
            this.listViewVCRedist.Location = new System.Drawing.Point(-1, 360);
            this.listViewVCRedist.Name = "listViewVCRedist";
            this.listViewVCRedist.Size = new System.Drawing.Size(626, 143);
            this.listViewVCRedist.TabIndex = 5;
            this.listViewVCRedist.UseCompatibleStateImageBehavior = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(415, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Check the CATIA version to install the demo plug-in";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(343, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Copy the EasyToolbar_Demo directory to C:\\";
            // 
            // Form_CATIA_V5_EasyToolbar_Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 619);
            this.Controls.Add(this.listViewVCRedist);
            this.Controls.Add(this.buttonSelSetupPath);
            this.Controls.Add(this.textBoxSetupPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSetup);
            this.Controls.Add(this.listViewCATIA);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_CATIA_V5_EasyToolbar_Setup";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CATIA_V5_EasyToolbar_Setup";
            this.Load += new System.EventHandler(this.Form_CATIA_V5_EasyToolbar_Setup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewCATIA;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSetupPath;
        private System.Windows.Forms.Button buttonSelSetupPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listViewVCRedist;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

