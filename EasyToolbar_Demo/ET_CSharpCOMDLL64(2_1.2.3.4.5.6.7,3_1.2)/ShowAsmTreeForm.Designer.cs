namespace ETCSharpCOMDLL
{
    partial class ShowAsmTreeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeViewAssembly = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewAssembly
            // 
            this.treeViewAssembly.Location = new System.Drawing.Point(12, 12);
            this.treeViewAssembly.Name = "treeViewAssembly";
            this.treeViewAssembly.Size = new System.Drawing.Size(812, 585);
            this.treeViewAssembly.TabIndex = 0;
            // 
            // ShowAsmTreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 609);
            this.Controls.Add(this.treeViewAssembly);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShowAsmTreeForm";
            this.Text = "ShowAsmTreeForm";
            this.Load += new System.EventHandler(this.ShowAsmTreeForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewAssembly;
    }
}