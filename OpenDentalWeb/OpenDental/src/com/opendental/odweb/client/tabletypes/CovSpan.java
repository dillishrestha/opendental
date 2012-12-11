package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;

public class CovSpan {
		/** Primary key. */
		public int CovSpanNum;
		/** FK to covcat.CovCatNum. */
		public int CovCatNum;
		/** Lower range of the span.  Does not need to be a valid code. */
		public String FromCode;
		/** Upper range of the span.  Does not need to be a valid code. */
		public String ToCode;

		/** Deep copy of object. */
		public CovSpan Copy() {
			CovSpan covspan=new CovSpan();
			covspan.CovSpanNum=this.CovSpanNum;
			covspan.CovCatNum=this.CovCatNum;
			covspan.FromCode=this.FromCode;
			covspan.ToCode=this.ToCode;
			return covspan;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<CovSpan>");
			sb.append("<CovSpanNum>").append(CovSpanNum).append("</CovSpanNum>");
			sb.append("<CovCatNum>").append(CovCatNum).append("</CovCatNum>");
			sb.append("<FromCode>").append(Serializing.EscapeForXml(FromCode)).append("</FromCode>");
			sb.append("<ToCode>").append(Serializing.EscapeForXml(ToCode)).append("</ToCode>");
			sb.append("</CovSpan>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void DeserializeFromXml(Document doc) throws Exception {
			try {
				if(Serializing.GetXmlNodeValue(doc,"CovSpanNum")!=null) {
					CovSpanNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"CovSpanNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"CovCatNum")!=null) {
					CovCatNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"CovCatNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"FromCode")!=null) {
					FromCode=Serializing.GetXmlNodeValue(doc,"FromCode");
				}
				if(Serializing.GetXmlNodeValue(doc,"ToCode")!=null) {
					ToCode=Serializing.GetXmlNodeValue(doc,"ToCode");
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}