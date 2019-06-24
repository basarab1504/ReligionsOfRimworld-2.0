using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_JoiningRestriction : ReligionSettings
    {
        private List<ReligionPermission> permissions = new List<ReligionPermission>();

        public IEnumerable<ReligionPermission> Permissions => permissions;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach(ReligionPermission permission in permissions)
                yield return new ReligionInfoEntry("ReligionInfo_Permission".Translate(), permission.Reason);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ReligionPermission>(ref this.permissions, "permissions", LookMode.Deep);
        }
    }
}
