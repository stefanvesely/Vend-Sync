
namespace Vend_Sync
{
    partial class frmSyncServer
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tbPCName = new System.Windows.Forms.TextBox();
            this.btnPurge = new System.Windows.Forms.Button();
            this.btnFullUpload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(220, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(180, 25);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Reload From Server";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tbPCName
            // 
            this.tbPCName.Enabled = false;
            this.tbPCName.Location = new System.Drawing.Point(13, 76);
            this.tbPCName.Name = "tbPCName";
            this.tbPCName.Size = new System.Drawing.Size(387, 20);
            this.tbPCName.TabIndex = 6;
            // 
            // btnPurge
            // 
            this.btnPurge.Location = new System.Drawing.Point(220, 43);
            this.btnPurge.Name = "btnPurge";
            this.btnPurge.Size = new System.Drawing.Size(180, 25);
            this.btnPurge.TabIndex = 7;
            this.btnPurge.Text = "Purge Logs";
            this.btnPurge.UseVisualStyleBackColor = true;
            // 
            // btnFullUpload
            // 
            this.btnFullUpload.Location = new System.Drawing.Point(13, 43);
            this.btnFullUpload.Name = "btnFullUpload";
            this.btnFullUpload.Size = new System.Drawing.Size(180, 25);
            this.btnFullUpload.TabIndex = 8;
            this.btnFullUpload.Text = "Full Sync Local";
            this.btnFullUpload.UseVisualStyleBackColor = true;
            this.btnFullUpload.Click += new System.EventHandler(this.btnFullUpload_Click);
            // 
            // frmSyncServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 115);
            this.Controls.Add(this.btnFullUpload);
            this.Controls.Add(this.btnPurge);
            this.Controls.Add(this.tbPCName);
            this.Controls.Add(this.btnRefresh);
            this.Name = "frmSyncServer";
            this.Text = "Sync";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox tbPCName;
        private System.Windows.Forms.Button btnPurge;
        private System.Windows.Forms.Button btnFullUpload;
    }
}

