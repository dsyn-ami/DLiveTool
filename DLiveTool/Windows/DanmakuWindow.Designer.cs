namespace DLiveTool.Windows
{
    partial class DanmakuWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanmakuWindow));
            this.pictureBg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBg)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBg
            // 
            this.pictureBg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBg.Image = ((System.Drawing.Image)(resources.GetObject("pictureBg.Image")));
            this.pictureBg.Location = new System.Drawing.Point(0, 0);
            this.pictureBg.Name = "pictureBg";
            this.pictureBg.Size = new System.Drawing.Size(800, 450);
            this.pictureBg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBg.TabIndex = 0;
            this.pictureBg.TabStop = false;
            // 
            // DanmakuWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBg);
            this.Name = "DanmakuWindow";
            this.Text = "DanmakuWindow";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBg;
    }
}