using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReligionsOfRimworld
{
	public static class PawnCategoryHelper
	{
		public static string Label(this PropertyPawnCategory p)
		{
			switch (p)
			{
				case PropertyPawnCategory.Everyone:
					return "PawnCategoryEveryone".Translate();
				case PropertyPawnCategory.Hostile:
					return "PawnCategoryHostile".Translate();
				case PropertyPawnCategory.Peaceful:
					return "PawnCategoryPeaceful".Translate();
				case PropertyPawnCategory.SameFaction:
					return "PawnCategorySameFaction".Translate();
				case PropertyPawnCategory.SameReligion:
					return "PawnCategorySameReligion".Translate();
				case PropertyPawnCategory.SameReligionGroup:
					return "PawnCategorySameReligionGroup".Translate();
				default:
					return "Unknown";
			}
		}
	}
}
