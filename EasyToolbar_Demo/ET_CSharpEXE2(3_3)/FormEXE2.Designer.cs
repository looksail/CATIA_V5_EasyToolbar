namespace ET_CSharpEXE
{
    partial class FormEXE2
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
            this.Test8 = new System.Windows.Forms.Button();
            this.Test7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Test8
            // 
            this.Test8.Location = new System.Drawing.Point(46, 110);
            this.Test8.Name = "Test8";
            this.Test8.Size = new System.Drawing.Size(290, 39);
            this.Test8.TabIndex = 9;
            this.Test8.Text = "Test8:MultiSelectEdgeFillet";
            this.Test8.UseVisualStyleBackColor = true;
            this.Test8.Click += new System.EventHandler(this.Test8_Click);
            // 
            // Test7
            // 
            this.Test7.Location = new System.Drawing.Point(46, 42);
            this.Test7.Name = "Test7";
            this.Test7.Size = new System.Drawing.Size(290, 39);
            this.Test7.TabIndex = 8;
            this.Test7.Text = "Test7:SingleSelectAndFillet";
            this.Test7.UseVisualStyleBackColor = true;
            this.Test7.Click += new System.EventHandler(this.Test7_Click);
            // 
            // FormEXE2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 187);
            this.Controls.Add(this.Test8);
            this.Controls.Add(this.Test7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEXE2";
            this.ShowIcon = false;
            this.Text = "FormEXE2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Test8;
        private System.Windows.Forms.Button Test7;
    }
}

