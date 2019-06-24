using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReligionsOfRimworld
{
    public static class MakeReligionFromDefUtility
    {
        private static List<ReligionSettings> listOfSettings = new List<ReligionSettings>();

        public static Religion MakeReligionFromDef(ReligionDef def)
        {
            listOfSettings.Clear();
            foreach (ReligionSettingsDef d in def.SettingsDefs)
                listOfSettings.Add(d.Settings);
            return new Religion(new ReligionConfiguration(def.label, def.description, listOfSettings));
        }
    }
}
