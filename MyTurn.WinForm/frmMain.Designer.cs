namespace MyTurn.WinForm
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pcMinimize = new System.Windows.Forms.PictureBox();
            this.pcBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlToiletContainer = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pcMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcMinimize
            // 
            this.pcMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pcMinimize.Image = global::MyTurn.WinForm.Properties.Resources._1466763958_arrow_minimise;
            this.pcMinimize.Location = new System.Drawing.Point(507, 3);
            this.pcMinimize.Name = "pcMinimize";
            this.pcMinimize.Size = new System.Drawing.Size(16, 21);
            this.pcMinimize.TabIndex = 8;
            this.pcMinimize.TabStop = false;
            this.pcMinimize.Click += new System.EventHandler(this.pcMinimize_Click);
            // 
            // pcBox
            // 
            this.pcBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pcBox.Image = global::MyTurn.WinForm.Properties.Resources._1466763871_cross_button;
            this.pcBox.Location = new System.Drawing.Point(529, 3);
            this.pcBox.Name = "pcBox";
            this.pcBox.Size = new System.Drawing.Size(16, 21);
            this.pcBox.TabIndex = 7;
            this.pcBox.TabStop = false;
            this.pcBox.Click += new System.EventHandler(this.pcBox_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pcBox);
            this.panel1.Controls.Add(this.pcMinimize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(557, 30);
            this.panel1.TabIndex = 7;
            // 
            // pnlToiletContainer
            // 
            this.pnlToiletContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlToiletContainer.Location = new System.Drawing.Point(0, 30);
            this.pnlToiletContainer.Name = "pnlToiletContainer";
            this.pnlToiletContainer.Size = new System.Drawing.Size(557, 231);
            this.pnlToiletContainer.TabIndex = 8;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 261);
            this.Controls.Add(this.pnlToiletContainer);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Turn";
            ((System.ComponentModel.ISupportInitialize)(this.pcMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pcMinimize;
        private System.Windows.Forms.PictureBox pcBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel pnlToiletContainer;


    }
}

