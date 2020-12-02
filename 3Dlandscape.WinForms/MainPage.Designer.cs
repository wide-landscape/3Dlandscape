namespace _3Dlandscape.WinForms
{
    partial class MainPage
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
            this.urhoSurfacePlaceholder = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // urhoSurfacePlaceholder
            // 
            this.urhoSurfacePlaceholder.Location = new System.Drawing.Point(0, 0);
            this.urhoSurfacePlaceholder.Name = "urhoSurfacePlaceholder";
            this.urhoSurfacePlaceholder.Size = new System.Drawing.Size(989, 592);
            this.urhoSurfacePlaceholder.TabIndex = 0;
            this.urhoSurfacePlaceholder.Paint += new System.Windows.Forms.PaintEventHandler(this.urhoSurfacePlaceholder_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 589);
            this.Controls.Add(this.urhoSurfacePlaceholder);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel urhoSurfacePlaceholder;
    }
}

