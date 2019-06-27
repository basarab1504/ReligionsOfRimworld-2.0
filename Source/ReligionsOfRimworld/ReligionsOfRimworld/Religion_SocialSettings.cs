using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_Social : ReligionSettings
    {
        private ReligionProperty defaultPropety;
        private List<ReligionProperty> properties;

        public ReligionSettings_Social()
        {
            properties = new List<ReligionProperty>();
        }

        public ReligionProperty DefaultPropety => defaultPropety;
        public IEnumerable<ReligionProperty> Properties => properties;

        public ReligionProperty GetPropertyBySubject(Def def)
        {
            ReligionProperty property = properties.FirstOrDefault(x => x.GetObject() == def);
            if (property != null)
                return property;
            return defaultPropety;
        }

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            if (defaultPropety != null)
                foreach (ReligionInfoEntry entry in defaultPropety.GetInfoEntries())
                    yield return entry;

            foreach (ReligionProperty property in properties)
                foreach (ReligionInfoEntry entry in property.GetInfoEntries())
                    yield return entry;
        }
    }
}
