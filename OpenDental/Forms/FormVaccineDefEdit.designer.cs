namespace OpenDental{
	partial class FormVaccineDefEdit {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.comboManufacturer = new System.Windows.Forms.ComboBox();
			this.textVaccineName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textCVXCode = new System.Windows.Forms.TextBox();
			this.labelCVXCode = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(174,130);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(255,130);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(1,83);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(98,18);
			this.label3.TabIndex = 22;
			this.label3.Text = "Manufacturer";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboManufacturer
			// 
			this.comboManufacturer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboManufacturer.FormattingEnabled = true;
			this.comboManufacturer.Location = new System.Drawing.Point(101,83);
			this.comboManufacturer.Name = "comboManufacturer";
			this.comboManufacturer.Size = new System.Drawing.Size(173,21);
			this.comboManufacturer.TabIndex = 21;
			// 
			// textVaccineName
			// 
			this.textVaccineName.Location = new System.Drawing.Point(101,57);
			this.textVaccineName.Name = "textVaccineName";
			this.textVaccineName.Size = new System.Drawing.Size(229,20);
			this.textVaccineName.TabIndex = 20;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11,56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88,20);
			this.label1.TabIndex = 19;
			this.label1.Text = "Vaccine Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCVXCode
			// 
			this.textCVXCode.Location = new System.Drawing.Point(101,31);
			this.textCVXCode.Name = "textCVXCode";
			this.textCVXCode.Size = new System.Drawing.Size(77,20);
			this.textCVXCode.TabIndex = 18;
			// 
			// labelCVXCode
			// 
			this.labelCVXCode.Location = new System.Drawing.Point(11,30);
			this.labelCVXCode.Name = "labelCVXCode";
			this.labelCVXCode.Size = new System.Drawing.Size(88,20);
			this.labelCVXCode.TabIndex = 17;
			this.labelCVXCode.Text = "CVX Code";
			this.labelCVXCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormVaccineDefEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(355,181);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboManufacturer);
			this.Controls.Add(this.textVaccineName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textCVXCode);
			this.Controls.Add(this.labelCVXCode);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Name = "FormVaccineDefEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Vaccine Definition Edit";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboManufacturer;
		private System.Windows.Forms.TextBox textVaccineName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textCVXCode;
		private System.Windows.Forms.Label labelCVXCode;
	}
}