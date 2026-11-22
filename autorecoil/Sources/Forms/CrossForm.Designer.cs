
namespace autorecoil.Sources.Forms
{
    partial class CrossForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lbl_cross;

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lbl_cross = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_cross
            // 
            this.lbl_cross.BackColor = System.Drawing.Color.Transparent;
            this.lbl_cross.Font = new System.Drawing.Font("Courier New", 9F);
            this.lbl_cross.ForeColor = System.Drawing.Color.Red;
            this.lbl_cross.Location = new System.Drawing.Point(32, 32);
            this.lbl_cross.Name = "lbl_cross";
            this.lbl_cross.Size = new System.Drawing.Size(64, 64);
            this.lbl_cross.TabIndex = 0;
            this.lbl_cross.Text = "*";
            this.lbl_cross.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CrossForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Magenta;
            this.ClientSize = new System.Drawing.Size(128, 128);
            this.Controls.Add(this.lbl_cross);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "CrossForm";
            this.Text = "Cross Window";
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.ResumeLayout(false);

        }
    }
}
