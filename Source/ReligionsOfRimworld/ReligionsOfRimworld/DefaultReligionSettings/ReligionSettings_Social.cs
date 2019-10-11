using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_Social : ReligionSettings
    {
        private ReligionProperty_Default defaultPropety;
        private List<ReligionProperty> properties;

        public ReligionSettings_Social()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                properties = new List<ReligionProperty>();
        }

        public ReligionProperty DefaultPropety => defaultPropety;
        public IEnumerable<ReligionProperty> Properties => properties;

        public ReligionProperty GetPropertyByObject(Pawn pawn, Def def, Pawn otherPawn = null)
        {
            IEnumerable<ReligionProperty> props = properties.FindAll(x => x.Contains(def));

            if (otherPawn != null)
            {
                foreach (ReligionProperty prop in props)
                    if (PropertyPawnCategoryUtility.IsSubjectFromRightCategory(pawn, otherPawn, prop.PawnCategory))
                        return prop;
            }
            else
                return props.FirstOrDefault();

            return defaultPropety;
        }

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            if (defaultPropety != null)
                yield return defaultPropety.GetReligionInfoEntry();

            foreach (ReligionProperty property in properties)
                    yield return property.GetReligionInfoEntry();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ReligionProperty>(ref this.properties, "properties", LookMode.Deep);
            Scribe_Deep.Look<ReligionProperty_Default>(ref this.defaultPropety, "defaultProperty");
        }
    }
}
