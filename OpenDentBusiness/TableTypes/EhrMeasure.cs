﻿using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>For EHR module, automate measure calculation.</summary>
	[Serializable]
	public class EhrMeasure:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long EhrMeasureNum;
		///<summary>Enum:EhrMeasureType</summary>
		public EhrMeasureType MeasureType;
		///<summary>0-100, -1 indicates not entered yet.</summary>
		public int Numerator;
		///<summary>0-100, -1 indicates not entered yet.</summary>
		public int Denominator;
		///<summary></summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string Objective;
		///<summary></summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string Measure;

		///<summary></summary>
		public EhrMeasure Copy() {
			return (EhrMeasure)MemberwiseClone();
		}

	}

	public enum EhrMeasureType {
		///<summary>0</summary>
		ProblemList,
		///<summary>1</summary>
		MedicationList,
		///<summary>2</summary>
		AllergyList,
		///<summary>3</summary>
		Demographics,
		///<summary>4</summary>
		Education,
		///<summary>5</summary>
		TimelyAccess,
		///<summary>6</summary>
		ProvOrderEntry,
		///<summary>7</summary>
		Rx,
		///<summary>8</summary>
		VitalSigns,
		///<summary>9</summary>
		Smoking,
		///<summary>10</summary>
		Lab,
		///<summary>11</summary>
		ElectronicCopy,
		///<summary>12</summary>
		ClinicalSummaries,
		///<summary>13</summary>
		Reminders,
		///<summary>14</summary>
		MedReconcile,
		///<summary>15- Summary of care record for transition or referral.</summary>
		Exchange

	}
}