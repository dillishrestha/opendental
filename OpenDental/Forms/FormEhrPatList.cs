﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormEhrPatList:Form {
		public List<EhrPatListElement> ElementList;

		public FormEhrPatList() {
			InitializeComponent();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Restriction",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Compare string",120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Operand",120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Lab value",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Order",120,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ElementList.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(ElementList[i].Restriction.ToString());
				row.Cells.Add(ElementList[i].CompareString);
				if(ElementList[i].Restriction==EhrRestrictionType.Gender
					|| ElementList[i].Restriction==EhrRestrictionType.Problem
					|| ElementList[i].Restriction==EhrRestrictionType.Medication) 
				{
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(ElementList[i].Operand.ToString());
				}
				row.Cells.Add(ElementList[i].LabValue);
				if(ElementList[i].OrderBy) {
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void AddElement(EhrRestrictionType restriction) {
			FormEhrPatListElementEdit FormPLEE=new FormEhrPatListElementEdit();
			FormPLEE.Element=new EhrPatListElement();
			FormPLEE.Element.Restriction=restriction;
			FormPLEE.IsNew=true;
			FormPLEE.ShowDialog();
			if(FormPLEE.DialogResult==DialogResult.OK) {
				ElementList.Add(FormPLEE.Element);
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int index=gridMain.GetSelectedIndex();
			if(index==-1) {
				MessageBox.Show("Please select a data element first.");
				return;
			}
			FormEhrPatListElementEdit FormPLEE=new FormEhrPatListElementEdit();
			FormPLEE.Element=ElementList[index];
			FormPLEE.ShowDialog();
			if(FormPLEE.DialogResult==DialogResult.Cancel && FormPLEE.Delete) {
				ElementList.Remove(ElementList[index]);
			}
			FillGrid();
		}

		private void butResults_Click(object sender,EventArgs e) {
			if(gridMain.Rows.Count<1) {
				MessageBox.Show(Lans.g(this,"Please add a data element."));
				return;
			}
			bool hasOrder=false;
			for(int i=0;i<ElementList.Count;i++) {
				if(hasOrder && ElementList[i].OrderBy) {
					MessageBox.Show(Lans.g(this,"You can only 'Order By' exactly one data element."));
					return;
				}
				if(ElementList[i].OrderBy) {
					hasOrder=true;
				}
			}
			FormEhrPatListResults FormPLR=new FormEhrPatListResults(ElementList);
			FormPLR.ShowDialog();
		}

		private void butBirthdate_Click(object sender,EventArgs e) {
			AddElement(EhrRestrictionType.Birthdate);
		}

		private void butDisease_Click(object sender,EventArgs e) {
			AddElement(EhrRestrictionType.Problem);
		}

		private void butMedication_Click(object sender,EventArgs e) {
			AddElement(EhrRestrictionType.Medication);
		}

		private void butLabResult_Click(object sender,EventArgs e) {
			AddElement(EhrRestrictionType.LabResult);
		}

		private void butGender_Click(object sender,EventArgs e) {
			AddElement(EhrRestrictionType.Gender);
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
