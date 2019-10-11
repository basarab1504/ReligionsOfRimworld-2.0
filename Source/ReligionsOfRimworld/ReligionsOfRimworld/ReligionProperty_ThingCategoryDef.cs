using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionProperty_ThingCategoryDef : ReligionProperty
    {
        private ThingCategoryDef propertyObject;

        public override bool Contains(Def def)
        {
            if(def is ThingDef)
            {
                IList<ThingCategoryDef> thingCategories = (def as ThingDef).thingCategories;

                if (!thingCategories.NullOrEmpty() && thingCategories.Contains(propertyObject))
                    return true;
            }
            return false;
        }

        public override Def GetObject()
        {
            return propertyObject;
        }

        protected override string ObjectLabel => propertyObject.LabelCap;
        protected override string Description => propertyObject.description;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ThingCategoryDef>(ref this.propertyObject, "thingCategoryDef");
        }
    }
}