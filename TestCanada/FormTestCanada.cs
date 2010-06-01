﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace TestCanada {
	public partial class FormTestCanada:Form {
		public FormTestCanada() {
			InitializeComponent();
		}

		private void butNewDb_Click(object sender,EventArgs e) {
			textResults.Text="";
			Application.DoEvents();
			Cursor=Cursors.WaitCursor;
			if(!DatabaseTools.SetDbConnection("")){
				MessageBox.Show("Could not connect");
				return;
			}
			DatabaseTools.FreshFromDump();
			textResults.Text+="Fresh database loaded from sql dump.";
			Cursor=Cursors.Default;
		}

		private void butClear_Click(object sender,EventArgs e) {
			textResults.Text="";
			Application.DoEvents();
			Cursor=Cursors.WaitCursor;
			if(!DatabaseTools.SetDbConnection("canadatest")) {//if database doesn't exist
				//MessageBox.Show("Database canadatest does not exist.");
				DatabaseTools.SetDbConnection("");
				textResults.Text+=DatabaseTools.FreshFromDump();//this also sets database to be unittest.
			}
			else {
				textResults.Text+=DatabaseTools.ClearDb();
			}
			Cursor=Cursors.Default;
		}

		private void butObjects_Click(object sender,EventArgs e) {
			FillObjects();
			Cursor=Cursors.Default;
		}

		private void FillObjects(){
			textResults.Text="";
			Application.DoEvents();
			Cursor=Cursors.WaitCursor;
			if(!DatabaseTools.SetDbConnection("canadatest")) {//if database doesn't exist
				//MessageBox.Show("Database canadatest does not exist.");
				DatabaseTools.SetDbConnection("");
				textResults.Text+=DatabaseTools.FreshFromDump();//this also sets database to be unittest.
			}
			else {
				textResults.Text+=DatabaseTools.ClearDb();
			}
			Prefs.RefreshCache();
			textResults.Text+=ProviderTC.SetInitialProviders();
			Application.DoEvents();
			textResults.Text+=CarrierTC.SetInitialCarriers();
			Application.DoEvents();
			textResults.Text+=PatientTC.SetInitialPatients();			
		}

		private void butProcedures_Click(object sender,EventArgs e) {
			FillObjects();
			textResults.Text+="---------------------------------------\r\n";
			Application.DoEvents();
			textResults.Text+="(procs not implemented yet)\r\n";
			Cursor=Cursors.Default;
		}

		private void butScripts_Click(object sender,EventArgs e) {
			FillObjects();
			textResults.Text+="---------------------------------------\r\n";
			Application.DoEvents();
			textResults.Text+="(procs not implemented yet)\r\n";
			Application.DoEvents();
			textResults.Text+="---------------------------------------\r\n";
			Application.DoEvents();
			textResults.Text+=Eligibility.RunOne();
			Cursor=Cursors.Default;
		}
	}
}
