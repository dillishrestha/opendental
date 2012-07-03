namespace OpenDental {
	partial class FormSupplyInventory {
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuItemSuppliers = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCategories = new System.Windows.Forms.ToolStripMenuItem();
			this.butAddNeeded = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.butSupplies = new OpenDental.UI.Button();
			this.butOrders = new OpenDental.UI.Button();
			this.butEquipment = new OpenDental.UI.Button();
			this.gridNeeded = new OpenDental.UI.ODGrid();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSuppliers,
            this.menuItemCategories});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(520, 24);
			this.menuStrip1.TabIndex = 12;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuItemSuppliers
			// 
			this.menuItemSuppliers.Name = "menuItemSuppliers";
			this.menuItemSuppliers.Size = new System.Drawing.Size(67, 20);
			this.menuItemSuppliers.Text = "Suppliers";
			this.menuItemSuppliers.Click += new System.EventHandler(this.menuItemSuppliers_Click);
			// 
			// menuItemCategories
			// 
			this.menuItemCategories.Name = "menuItemCategories";
			this.menuItemCategories.Size = new System.Drawing.Size(75, 20);
			this.menuItemCategories.Text = "Categories";
			this.menuItemCategories.Click += new System.EventHandler(this.menuItemCategories_Click);
			// 
			// butAddNeeded
			// 
			this.butAddNeeded.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddNeeded.Autosize = true;
			this.butAddNeeded.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddNeeded.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddNeeded.CornerRadius = 4F;
			this.butAddNeeded.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddNeeded.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddNeeded.Location = new System.Drawing.Point(12, 555);
			this.butAddNeeded.Name = "butAddNeeded";
			this.butAddNeeded.Size = new System.Drawing.Size(80, 26);
			this.butAddNeeded.TabIndex = 5;
			this.butAddNeeded.Text = "Add";
			this.butAddNeeded.Click += new System.EventHandler(this.butAddNeeded_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(158, 555);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(80, 26);
			this.butPrint.TabIndex = 24;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(433, 555);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(482, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(92, 18);
			this.label1.TabIndex = 10;
			this.label1.Text = "Supplier";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(576, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(173, 21);
			this.comboBox1.TabIndex = 8;
			// 
			// butSupplies
			// 
			this.butSupplies.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSupplies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSupplies.Autosize = true;
			this.butSupplies.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSupplies.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSupplies.CornerRadius = 4F;
			this.butSupplies.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSupplies.Location = new System.Drawing.Point(390, 87);
			this.butSupplies.Name = "butSupplies";
			this.butSupplies.Size = new System.Drawing.Size(124, 24);
			this.butSupplies.TabIndex = 13;
			this.butSupplies.Text = "Supplies";
			this.butSupplies.Click += new System.EventHandler(this.butSupplies_Click);
			// 
			// butOrders
			// 
			this.butOrders.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butOrders.Autosize = true;
			this.butOrders.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOrders.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOrders.CornerRadius = 4F;
			this.butOrders.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butOrders.Location = new System.Drawing.Point(390, 57);
			this.butOrders.Name = "butOrders";
			this.butOrders.Size = new System.Drawing.Size(124, 24);
			this.butOrders.TabIndex = 14;
			this.butOrders.Text = "Orders";
			this.butOrders.Click += new System.EventHandler(this.butOrders_Click);
			// 
			// butEquipment
			// 
			this.butEquipment.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEquipment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butEquipment.Autosize = true;
			this.butEquipment.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEquipment.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEquipment.CornerRadius = 4F;
			this.butEquipment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEquipment.Location = new System.Drawing.Point(390, 27);
			this.butEquipment.Name = "butEquipment";
			this.butEquipment.Size = new System.Drawing.Size(124, 24);
			this.butEquipment.TabIndex = 15;
			this.butEquipment.Text = "Equipment";
			this.butEquipment.Click += new System.EventHandler(this.butEquipment_Click);
			// 
			// gridNeeded
			// 
			this.gridNeeded.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridNeeded.HScrollVisible = false;
			this.gridNeeded.Location = new System.Drawing.Point(12, 27);
			this.gridNeeded.Name = "gridNeeded";
			this.gridNeeded.ScrollValue = 0;
			this.gridNeeded.Size = new System.Drawing.Size(372, 522);
			this.gridNeeded.TabIndex = 4;
			this.gridNeeded.Title = "Supplies Needed";
			this.gridNeeded.TranslationName = null;
			this.gridNeeded.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridNeeded_CellDoubleClick);
			// 
			// FormSupplyInventory
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(520, 593);
			this.Controls.Add(this.gridNeeded);
			this.Controls.Add(this.butAddNeeded);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butEquipment);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.butOrders);
			this.Controls.Add(this.butSupplies);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FormSupplyInventory";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Supply Inventory";
			this.Load += new System.EventHandler(this.FormInventory_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAddNeeded;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuItemSuppliers;
		private System.Windows.Forms.ToolStripMenuItem menuItemCategories;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		private OpenDental.UI.Button butPrint;
		private UI.Button butSupplies;
		private UI.Button butOrders;
		private UI.Button butEquipment;
		private UI.ODGrid gridNeeded;
	}
}