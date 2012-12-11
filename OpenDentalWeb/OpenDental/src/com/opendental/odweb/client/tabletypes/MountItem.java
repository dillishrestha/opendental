package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;

public class MountItem {
		/** Primary key. */
		public int MountItemNum;
		/** FK to mount.MountNum. */
		public int MountNum;
		/** The x position, in pixels, of the item on the mount. */
		public int Xpos;
		/** The y position, in pixels, of the item on the mount. */
		public int Ypos;
		/** The ordinal position of the item on the mount. */
		public int OrdinalPos;
		/** The scaled or unscaled width of the mount item in pixels. */
		public int Width;
		/** The scaled or unscaled height of the mount item in pixels. */
		public int Height;

		/** Deep copy of object. */
		public MountItem Copy() {
			MountItem mountitem=new MountItem();
			mountitem.MountItemNum=this.MountItemNum;
			mountitem.MountNum=this.MountNum;
			mountitem.Xpos=this.Xpos;
			mountitem.Ypos=this.Ypos;
			mountitem.OrdinalPos=this.OrdinalPos;
			mountitem.Width=this.Width;
			mountitem.Height=this.Height;
			return mountitem;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<MountItem>");
			sb.append("<MountItemNum>").append(MountItemNum).append("</MountItemNum>");
			sb.append("<MountNum>").append(MountNum).append("</MountNum>");
			sb.append("<Xpos>").append(Xpos).append("</Xpos>");
			sb.append("<Ypos>").append(Ypos).append("</Ypos>");
			sb.append("<OrdinalPos>").append(OrdinalPos).append("</OrdinalPos>");
			sb.append("<Width>").append(Width).append("</Width>");
			sb.append("<Height>").append(Height).append("</Height>");
			sb.append("</MountItem>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void DeserializeFromXml(Document doc) throws Exception {
			try {
				if(Serializing.GetXmlNodeValue(doc,"MountItemNum")!=null) {
					MountItemNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"MountItemNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"MountNum")!=null) {
					MountNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"MountNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Xpos")!=null) {
					Xpos=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"Xpos"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Ypos")!=null) {
					Ypos=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"Ypos"));
				}
				if(Serializing.GetXmlNodeValue(doc,"OrdinalPos")!=null) {
					OrdinalPos=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"OrdinalPos"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Width")!=null) {
					Width=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"Width"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Height")!=null) {
					Height=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"Height"));
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}