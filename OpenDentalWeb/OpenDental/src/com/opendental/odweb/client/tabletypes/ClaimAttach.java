package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;

public class ClaimAttach {
		/** Primary key. */
		public int ClaimAttachNum;
		/** FK to claim.ClaimNum */
		public int ClaimNum;
		/** The name of the file that shows on the claim.  For example: tooth2.jpg. */
		public String DisplayedFileName;
		/** The actual file is stored in the A-Z folder in EmailAttachments.  (yes, even though it's not actually an email attachment)  The files are named automatically based on Date/time along with a random number.  This ensures that they will be sequential as well as unique. */
		public String ActualFileName;

		/** Deep copy of object. */
		public ClaimAttach Copy() {
			ClaimAttach claimattach=new ClaimAttach();
			claimattach.ClaimAttachNum=this.ClaimAttachNum;
			claimattach.ClaimNum=this.ClaimNum;
			claimattach.DisplayedFileName=this.DisplayedFileName;
			claimattach.ActualFileName=this.ActualFileName;
			return claimattach;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<ClaimAttach>");
			sb.append("<ClaimAttachNum>").append(ClaimAttachNum).append("</ClaimAttachNum>");
			sb.append("<ClaimNum>").append(ClaimNum).append("</ClaimNum>");
			sb.append("<DisplayedFileName>").append(Serializing.EscapeForXml(DisplayedFileName)).append("</DisplayedFileName>");
			sb.append("<ActualFileName>").append(Serializing.EscapeForXml(ActualFileName)).append("</ActualFileName>");
			sb.append("</ClaimAttach>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void DeserializeFromXml(Document doc) throws Exception {
			try {
				if(Serializing.GetXmlNodeValue(doc,"ClaimAttachNum")!=null) {
					ClaimAttachNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"ClaimAttachNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"ClaimNum")!=null) {
					ClaimNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"ClaimNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DisplayedFileName")!=null) {
					DisplayedFileName=Serializing.GetXmlNodeValue(doc,"DisplayedFileName");
				}
				if(Serializing.GetXmlNodeValue(doc,"ActualFileName")!=null) {
					ActualFileName=Serializing.GetXmlNodeValue(doc,"ActualFileName");
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}