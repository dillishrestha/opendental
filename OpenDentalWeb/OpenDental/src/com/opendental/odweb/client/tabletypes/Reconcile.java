package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;
import com.google.gwt.i18n.client.DateTimeFormat;
import java.util.Date;

//DO NOT MAKE CHANGES TO THIS FILE.  THEY WILL GET OVERWRITTEN BY THE CRUD.
public class Reconcile {
		/** Primary key. */
		public int ReconcileNum;
		/** FK to account.AccountNum */
		public int AccountNum;
		/** User enters starting balance here. */
		public double StartingBal;
		/** User enters ending balance here. */
		public double EndingBal;
		/** The date that the reconcile was performed. */
		public Date DateReconcile;
		/** If StartingBal + sum of entries selected = EndingBal, then user can lock.  Unlock requires special permission, which nobody will have by default. */
		public boolean IsLocked;

		/** Deep copy of object. */
		public Reconcile deepCopy() {
			Reconcile reconcile=new Reconcile();
			reconcile.ReconcileNum=this.ReconcileNum;
			reconcile.AccountNum=this.AccountNum;
			reconcile.StartingBal=this.StartingBal;
			reconcile.EndingBal=this.EndingBal;
			reconcile.DateReconcile=this.DateReconcile;
			reconcile.IsLocked=this.IsLocked;
			return reconcile;
		}

		/** Serialize the object into XML. */
		public String serialize() {
			StringBuilder sb=new StringBuilder();
			sb.append("<Reconcile>");
			sb.append("<ReconcileNum>").append(ReconcileNum).append("</ReconcileNum>");
			sb.append("<AccountNum>").append(AccountNum).append("</AccountNum>");
			sb.append("<StartingBal>").append(StartingBal).append("</StartingBal>");
			sb.append("<EndingBal>").append(EndingBal).append("</EndingBal>");
			sb.append("<DateReconcile>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateReconcile)).append("</DateReconcile>");
			sb.append("<IsLocked>").append((IsLocked)?1:0).append("</IsLocked>");
			sb.append("</Reconcile>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void deserialize(Document doc) throws Exception {
			try {
				if(Serializing.getXmlNodeValue(doc,"ReconcileNum")!=null) {
					ReconcileNum=Integer.valueOf(Serializing.getXmlNodeValue(doc,"ReconcileNum"));
				}
				if(Serializing.getXmlNodeValue(doc,"AccountNum")!=null) {
					AccountNum=Integer.valueOf(Serializing.getXmlNodeValue(doc,"AccountNum"));
				}
				if(Serializing.getXmlNodeValue(doc,"StartingBal")!=null) {
					StartingBal=Double.valueOf(Serializing.getXmlNodeValue(doc,"StartingBal"));
				}
				if(Serializing.getXmlNodeValue(doc,"EndingBal")!=null) {
					EndingBal=Double.valueOf(Serializing.getXmlNodeValue(doc,"EndingBal"));
				}
				if(Serializing.getXmlNodeValue(doc,"DateReconcile")!=null) {
					DateReconcile=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.getXmlNodeValue(doc,"DateReconcile"));
				}
				if(Serializing.getXmlNodeValue(doc,"IsLocked")!=null) {
					IsLocked=(Serializing.getXmlNodeValue(doc,"IsLocked")=="0")?false:true;
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}
