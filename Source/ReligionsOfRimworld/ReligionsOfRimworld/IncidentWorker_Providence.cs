using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class IncidentWorker_Providence : IncidentWorker
    {
        public IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
        {
            Map map = target as Map;
            if (map != null)
                return map.mapPawns.FreeColonistsAndPrisoners.Where<Pawn>((Func<Pawn, bool>)(x =>
                {
                    if (x.ParentHolder is Building_CryptosleepCasket)
                        return false;
                    return x.RaceProps.IsFlesh;
                }));
            return ((Caravan)target).PawnsListForReading.Where<Pawn>((Func<Pawn, bool>)(x =>
            {
                if (x.ParentHolder is Building_CryptosleepCasket)
                    return false;
                return x.RaceProps.IsFlesh;
            }));
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return this.PotentialVictimCandidates(parms.target).Any<Pawn>();
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Religion religion = ReligionManager.GetReligionManager().AllReligions.RandomElement();
            if (religion.IncidentsSettings != null)
            {
                ReligionSettings_Incidents settings = religion.IncidentsSettings;
                if (settings.Incidents.Any(x => x == this.def))
                {
                    Pawn pawn = PotentialVictimCandidates(parms.target).RandomElement();
                    if (pawn == null)
                        return false;
                    pawn.GetReligionComponent().ChangeReligion(religion);
                    Find.LetterStack.ReceiveLetter(this.def.letterLabel, pawn.LabelCap + " " + def.letterText + " " + religion.Label, this.def.letterDef, (LookTargets)pawn, (Faction)null, (string)null);
                    return true;
                }
            } 
            return false;
        }
    }
}
