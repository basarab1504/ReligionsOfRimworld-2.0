using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReligionsOfRimworld
{
	public static class CriteriaImportanceHelper
	{
		public static string Label(this ReligionCriteriaImportance p)
		{
			switch (p)
			{
				case ReligionCriteriaImportance.Low:
					return "CriteriaImportanceLow".Translate();
				case ReligionCriteriaImportance.Regular:
					return "CriteriaImportanceRegular".Translate();
				case ReligionCriteriaImportance.Important:
					return "CriteriaImportanceImportant".Translate();
				case ReligionCriteriaImportance.Critical:
					return "CriteriaImportanceCritical".Translate();
				default:
					return "Unknown";
			}
		}
	}
}
