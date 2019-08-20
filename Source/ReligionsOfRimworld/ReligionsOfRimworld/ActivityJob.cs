using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReligionsOfRimworld
{
    public class ActivityJob : Job
    {
        public ActivityTask activityTask;

        public new void ExposeData()
        {
            ILoadReferenceable commTarget = (ILoadReferenceable)this.commTarget;
            Scribe_References.Look<ActivityTask>(ref this.activityTask, "activityTask");
            Scribe_References.Look<ILoadReferenceable>(ref commTarget, "commTarget", false);
            this.commTarget = (ICommunicable)commTarget;
            Scribe_References.Look<Verb>(ref this.verbToUse, "verbToUse", false);
            Scribe_References.Look<Bill>(ref this.bill, "bill", false);
            Scribe_References.Look<Lord>(ref this.lord, "lord", false);
            Scribe_Defs.Look<JobDef>(ref this.def, "def");
            Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
            Scribe_TargetInfo.Look(ref this.targetA, "targetA");
            Scribe_TargetInfo.Look(ref this.targetB, "targetB");
            Scribe_TargetInfo.Look(ref this.targetC, "targetC");
            Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueA, "targetQueueA", LookMode.Undefined, new object[0]);
            Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueB, "targetQueueB", LookMode.Undefined, new object[0]);
            Scribe_Values.Look<int>(ref this.count, "count", -1, false);
            Scribe_Collections.Look<int>(ref this.countQueue, "countQueue", LookMode.Undefined, new object[0]);
            Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
            Scribe_Values.Look<int>(ref this.expiryInterval, "expiryInterval", -1, false);
            Scribe_Values.Look<bool>(ref this.checkOverrideOnExpire, "checkOverrideOnExpire", false, false);
            Scribe_Values.Look<bool>(ref this.playerForced, "playerForced", false, false);
            Scribe_Collections.Look<ThingCountClass>(ref this.placedThings, "placedThings", LookMode.Undefined, new object[0]);
            Scribe_Values.Look<int>(ref this.maxNumMeleeAttacks, "maxNumMeleeAttacks", int.MaxValue, false);
            Scribe_Values.Look<int>(ref this.maxNumStaticAttacks, "maxNumStaticAttacks", int.MaxValue, false);
            Scribe_Values.Look<bool>(ref this.exitMapOnArrival, "exitMapOnArrival", false, false);
            Scribe_Values.Look<bool>(ref this.failIfCantJoinOrCreateCaravan, "failIfCantJoinOrCreateCaravan", false, false);
            Scribe_Values.Look<bool>(ref this.killIncappedTarget, "killIncappedTarget", false, false);
            Scribe_Values.Look<bool>(ref this.haulOpportunisticDuplicates, "haulOpportunisticDuplicates", false, false);
            Scribe_Values.Look<HaulMode>(ref this.haulMode, "haulMode", HaulMode.Undefined, false);
            Scribe_Defs.Look<ThingDef>(ref this.plantDefToSow, "plantDefToSow");
            Scribe_Values.Look<LocomotionUrgency>(ref this.locomotionUrgency, "locomotionUrgency", LocomotionUrgency.Jog, false);
            Scribe_Values.Look<bool>(ref this.ignoreDesignations, "ignoreDesignations", false, false);
            Scribe_Values.Look<bool>(ref this.canBash, "canBash", false, false);
            Scribe_Values.Look<bool>(ref this.haulDroppedApparel, "haulDroppedApparel", false, false);
            Scribe_Values.Look<bool>(ref this.restUntilHealed, "restUntilHealed", false, false);
            Scribe_Values.Look<bool>(ref this.ignoreJoyTimeAssignment, "ignoreJoyTimeAssignment", false, false);
            Scribe_Values.Look<bool>(ref this.overeat, "overeat", false, false);
            Scribe_Values.Look<bool>(ref this.attackDoorIfTargetLost, "attackDoorIfTargetLost", false, false);
            Scribe_Values.Look<int>(ref this.takeExtraIngestibles, "takeExtraIngestibles", 0, false);
            Scribe_Values.Look<bool>(ref this.expireRequiresEnemiesNearby, "expireRequiresEnemiesNearby", false, false);
            Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
            Scribe_Values.Look<bool>(ref this.forceSleep, "forceSleep", false, false);
            Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
            Scribe_Values.Look<bool>(ref this.endIfCantShootTargetFromCurPos, "endIfCantShootTargetFromCurPos", false, false);
            Scribe_Values.Look<bool>(ref this.endIfCantShootInMelee, "endIfCantShootInMelee", false, false);
            Scribe_Values.Look<bool>(ref this.checkEncumbrance, "checkEncumbrance", false, false);
            Scribe_Values.Look<float>(ref this.followRadius, "followRadius", 0.0f, false);
            Scribe_Values.Look<bool>(ref this.endAfterTendedOnce, "endAfterTendedOnce", false, false);
            if (Scribe.mode != LoadSaveMode.PostLoadInit || this.verbToUse == null || !this.verbToUse.BuggedAfterLoading)
                return;
            this.verbToUse = (Verb)null;
            Log.Warning(this.GetType().ToString() + " had a bugged verbToUse after loading.", false);
        }
    }
}
