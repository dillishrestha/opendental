using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormRxAlertEdit:Form {
		public FormRxAlertEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			
			//Code for deletion.
			//if(listAlerts.SelectedIndex==-1){
			//  MsgBox.Show(this,"Please select an items first.");
			//  return;
			//}
			//RxAlerts.Delete(RxAlertList[listAlerts.SelectedIndex]);
			//FillAlerts();
		}
	}
}