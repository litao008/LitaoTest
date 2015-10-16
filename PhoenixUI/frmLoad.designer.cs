namespace BinHong.PhoenixUI
{
    partial class FrmLoad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLoad));
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.flgView = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnDel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flgView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(477, 322);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(99, 23);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "登    陆";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(355, 322);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存配置";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(233, 322);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(99, 23);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "载入配置";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // flgView
            // 
            this.flgView.AllowDelete = true;
            this.flgView.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.ColumnsUniform;
            this.flgView.ColumnInfo = "1,1,0,0,0,100,Columns:0{Visible:False;}\t";
            this.flgView.Location = new System.Drawing.Point(13, 6);
            this.flgView.Name = "flgView";
            this.flgView.Rows.Count = 1;
            this.flgView.Rows.DefaultSize = 20;
            this.flgView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.flgView.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.AlwaysVisible;
            this.flgView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.flgView.Size = new System.Drawing.Size(695, 306);
            this.flgView.TabIndex = 4;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(13, 322);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(99, 23);
            this.btnDel.TabIndex = 8;
            this.btnDel.Text = "删    除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // FrmLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 357);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.flgView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmLoad";
            ((System.ComponentModel.ISupportInitialize)(this.flgView)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private C1.Win.C1FlexGrid.C1FlexGrid flgView;
        private System.Windows.Forms.Button btnDel;
    }
}