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
    public class LordJob_ReligionActivity : LordJob_VoluntarilyJoinable
    {
        private ReligionActivityData data;
        private int activityCurrentStage;
        private Dictionary<Pawn, bool> signalsCounted;

        //for scribe only
        List<Pawn> pawnsKeysWorkingList;
        List<bool> signalsValuesWorkingList;

        public LordJob_ReligionActivity()
        { }

        public LordJob_ReligionActivity(ReligionActivityData data)
        {
            this.data = data;
            if(Scribe.mode == LoadSaveMode.Inactive)
                signalsCounted = new Dictionary<Pawn, bool>();
        }

        public void RecieveStageEndedSignal(Pawn pawn)
        {
            if (!signalsCounted[pawn])
            {
                //Log.Message(pawn.ToString() + " counted");
                signalsCounted[pawn] = true;
            }
        }

        public override void Notify_PawnAdded(Pawn p)
        {
            base.Notify_PawnAdded(p);
            //Log.Message(p.ToString() + " added");
            signalsCounted.Add(p, false);
        }

        public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
        {
            base.Notify_PawnLost(p, condition);
            //Log.Message(p.ToString() + " lost");
            signalsCounted.Remove(p);
        }

        public override float VoluntaryJoinPriorityFor(Pawn p)
        {
            if (p.GetReligionComponent().Religion == data.Religion)
            {
                if (p == data.Organizer)
                    return 100f;
                else
                    return p.GetReligionComponent().PietyTracker.Piety.CurCategoryInt * 19f;
            }
            return 0.0f;
        }

        public override StateGraph CreateGraph()
        {
            StateGraph stateGraph = new StateGraph();

            LordToil_ActivityStage stageToil = new LordToil_ActivityStage(data);
            stateGraph.AddToil(stageToil);

            LordToil_End endToil = new LordToil_End();
            stateGraph.AddToil(endToil);

            Transition transition = new Transition(stageToil, stageToil, true, true);
            transition.AddTrigger((Trigger)new Trigger_TickCondition((Func<bool>)(() => AllSignalsReceived && ShouldMoveNext)));
            transition.AddPreAction((TransitionAction)new TransitionAction_Custom((Action)(() => MoveNext())));    

            stateGraph.AddTransition(transition);

            Transition transition2 = new Transition(stageToil, endToil, false, true);
            transition2.AddTrigger((Trigger)new Trigger_TickCondition((Func<bool>)(() => AllSignalsReceived && ShouldEndActivity)));
            transition2.AddPreAction((TransitionAction)new TransitionAction_Custom((Action)(() => ActivityEnd())));
            stateGraph.AddTransition(transition2);

            Transition transition3 = new Transition(stageToil, endToil, false, true);
            transition3.AddTrigger((Trigger)new Trigger_TickCondition((Func<bool>)(() => ShouldActivityBeCalledOff)));
            stateGraph.AddTransition(transition3);

            return stateGraph;
        }

        private bool AllSignalsReceived => signalsCounted.All(x => x.Value == true);

        private bool ShouldMoveNext => activityCurrentStage + 1 < data.ActivityJobs.Count();

        private bool ShouldEndActivity => activityCurrentStage + 1 == data.ActivityJobs.Count();

        private void MoveNext()
        {
            activityCurrentStage++;
            //Log.Message("CURRENT " + activityCurrentStage.ToString());
            foreach (Pawn pawn in lord.ownedPawns)
                signalsCounted[pawn] = false;
        }

        private bool ShouldActivityBeCalledOff => !lord.ownedPawns.Contains(data.Organizer) || lord.ownedPawns.NullOrEmpty();

        public Job GetCurrentJob(Pawn pawn)
        {
            if (activityCurrentStage >= data.ActivityJobs.Count())
                return null;

            PawnDuty duty = pawn.mindState.duty;
            if (duty == null)
                return (Job)null;

            if (signalsCounted[pawn] == true)
                return (Job)null;

            JobDef def;

            if (pawn == data.Organizer)
            {
                def = data.ActivityJobs.ElementAt(activityCurrentStage).OrganizerJob;
            }
            else
                def = data.ActivityJobs.ElementAt(activityCurrentStage).CongregationJob;

            Job job = (Job)null;

            if (def != null)
            {
                if (pawn == data.Organizer)
                {
                    job = new Job(data.ActivityJobs.ElementAt(activityCurrentStage).OrganizerJob, duty.focus, duty.focusSecond)
                    {
                        bill = data.Bill
                    };
                    pawn.Reserve(duty.focus, job);
                }
                else
                    job = new Job(data.ActivityJobs.ElementAt(activityCurrentStage).CongregationJob, duty.focus);
            }

            return job;
        }

        private void ActivityEnd()
        {
            foreach (Pawn pawn in lord.ownedPawns)
                if (pawn == data.Organizer)
                    PietyUtility.TryApplyOnPawn(data.OrganizerProperty, pawn);
                else
                    PietyUtility.TryApplyOnPawn(data.СongregationProperty, pawn);

            data.Bill.Notify_IterationCompleted(data.Organizer, null);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ReligionActivityData>(ref this.data, "activityData", null, null, null, null);
            Scribe_Values.Look<int>(ref this.activityCurrentStage, "currentStage");
            Scribe_Collections.Look<Pawn, bool> (ref this.signalsCounted, "countedSignals", LookMode.Reference, LookMode.Value, ref pawnsKeysWorkingList, ref signalsValuesWorkingList);
        }

        //private class SignalCounter
        //{
        //    private List<Signal> signals;

        //    public SignalCounter()
        //    {
        //        if (Scribe.mode == LoadSaveMode.Inactive)
        //            signals = new List<Signal>();
        //    }

        //    public Signal this[Pawn pawn]
        //    {
        //        get
        //        {
        //            return signals.Where(x => x.Pawn == pawn)
        //        }
        //        set
        //        {
        //            signals[key] = value;
        //        }
        //    }

        //    public void Add(Pawn pawn) => signals.Add(new Signal(pawn));
        //    public void Remove(Pawn pawn) => signals.Remove(pawn);
        //    public void ChangeSignal(Pawn pawn, bool signal) => signal[]

        //    private class Signal
        //    {
        //        private Pawn pawn;
        //        private bool signal;

        //        public Signal(Pawn pawn) => this.pawn = pawn;
        //        public Pawn Pawn => pawn;
        //        public bool CurrentSignal => signal;
        //        public void ChangeSignal(bool signal) => this.signal = signal;
        //    }
        //}
    }
}
