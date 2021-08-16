
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
            this.brnAddNew = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnRetrieveData = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // brnAddNew
            // 
            this.brnAddNew.Location = new System.Drawing.Point(250, 10);
            this.brnAddNew.Name = "brnAddNew";
            this.brnAddNew.Size = new System.Drawing.Size(75, 23);
            this.brnAddNew.TabIndex = 0;
            this.brnAddNew.Text = "Add New";
            this.brnAddNew.UseVisualStyleBackColor = true;
            this.brnAddNew.Click += new System.EventHandler(this.brnAddNew_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(232, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(12, 126);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(202, 26);
            this.btnSendData.TabIndex = 2;
            this.btnSendData.Text = "Send";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnRetrieveData
            // 
            this.btnRetrieveData.Location = new System.Drawing.Point(12, 95);
            this.btnRetrieveData.Name = "btnRetrieveData";
            this.btnRetrieveData.Size = new System.Drawing.Size(202, 25);
            this.btnRetrieveData.TabIndex = 3;
            this.btnRetrieveData.Text = "Get";
            this.btnRetrieveData.UseVisualStyleBackColor = true;
            this.btnRetrieveData.Click += new System.EventHandler(this.btnRetrieveData_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(220, 126);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(180, 25);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Get All Tables";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmSyncServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 210);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnRetrieveData);
            this.Controls.Add(this.btnSendData);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.brnAddNew);
            this.Name = "frmSyncServer";
            this.Text = "Sync";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button brnAddNew;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnRetrieveData;
        private System.Windows.Forms.Button btnRefresh;
    }
}

