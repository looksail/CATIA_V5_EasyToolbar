namespace ET_CSharpCOMDLL_Test
{
    partial class Form1
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
            this.Test1 = new System.Windows.Forms.Button();
            this.Test2 = new System.Windows.Forms.Button();
            this.Test3 = new System.Windows.Forms.Button();
            this.Test4 = new System.Windows.Forms.Button();
            this.Test5 = new System.Windows.Forms.Button();
            this.Test6 = new System.Windows.Forms.Button();
            this.Test10 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Test1
            // 
            this.Test1.Location = new System.Drawing.Point(51, 40);
            this.Test1.Name = "Test1";
            this.Test1.Size = new System.Drawing.Size(290, 39);
            this.Test1.TabIndex = 0;
            this.Test1.Text = "Test1:ShowActiveDocName";
            this.Test1.UseVisualStyleBackColor = true;
            this.Test1.Click += new System.EventHandler(this.Test1_Click);
            // 
            // Test2
            // 
            this.Test2.Location = new System.Drawing.Point(51, 98);
            this.Test2.Name = "Test2";
            this.Test2.Size = new System.Drawing.Size(290, 39);
            this.Test2.TabIndex = 1;
            this.Test2.Text = "Test2:CreateSimpleCube";
            this.Test2.UseVisualStyleBackColor = true;
            this.Test2.Click += new System.EventHandler(this.Test2_Click);
            // 
            // Test3
            // 
            this.Test3.Location = new System.Drawing.Point(51, 156);
            this.Test3.Name = "Test3";
            this.Test3.Size = new System.Drawing.Size(290, 39);
            this.Test3.TabIndex = 2;
            this.Test3.Text = "Test3:CreateCylinder";
            this.Test3.UseVisualStyleBackColor = true;
            this.Test3.Click += new System.EventHandler(this.Test3_Click);
            // 
            // Test4
            // 
            this.Test4.Location = new System.Drawing.Point(51, 214);
            this.Test4.Name = "Test4";
            this.Test4.Size = new System.Drawing.Size(290, 39);
            this.Test4.TabIndex = 3;
            this.Test4.Text = "Test4:ShowAsmTree";
            this.Test4.UseVisualStyleBackColor = true;
            this.Test4.Click += new System.EventHandler(this.Test4_Click);
            // 
            // Test5
            // 
            this.Test5.Location = new System.Drawing.Point(51, 272);
            this.Test5.Name = "Test5";
            this.Test5.Size = new System.Drawing.Size(290, 39);
            this.Test5.TabIndex = 4;
            this.Test5.Text = "Test5:AddUserProperty";
            this.Test5.UseVisualStyleBackColor = true;
            this.Test5.Click += new System.EventHandler(this.Test5_Click);
            // 
            // Test6
            // 
            this.Test6.Location = new System.Drawing.Point(51, 330);
            this.Test6.Name = "Test6";
            this.Test6.Size = new System.Drawing.Size(290, 39);
            this.Test6.TabIndex = 5;
            this.Test6.Text = "Test6:RenamePartNumber";
            this.Test6.UseVisualStyleBackColor = true;
            this.Test6.Click += new System.EventHandler(this.Test6_Click);
            // 
            // Test10
            // 
            this.Test10.Location = new System.Drawing.Point(51, 388);
            this.Test10.Name = "Test10";
            this.Test10.Size = new System.Drawing.Size(290, 37);
            this.Test10.TabIndex = 9;
            this.Test10.Text = "Test10:OpenAndCLoseFile";
            this.Test10.UseVisualStyleBackColor = true;
            this.Test10.Click += new System.EventHandler(this.Test10_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 441);
            this.Controls.Add(this.Test10);
            this.Controls.Add(this.Test6);
            this.Controls.Add(this.Test5);
            this.Controls.Add(this.Test4);
            this.Controls.Add(this.Test3);
            this.Controls.Add(this.Test2);
            this.Controls.Add(this.Test1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "ET_CSharpCOMDLL64_Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Test1;
        private System.Windows.Forms.Button Test2;
        private System.Windows.Forms.Button Test3;
        private System.Windows.Forms.Button Test4;
        private System.Windows.Forms.Button Test5;
        private System.Windows.Forms.Button Test6;
        private System.Windows.Forms.Button Test10;
    }
}

