using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class LordJob_DefendBase3(Faction faction, IntVec3 baseCenter) : LordJob
{
    public override StateGraph CreateGraph()
    {
        var stateGraph = new StateGraph();
        var lordToil_DefendBase =
            (LordToil_DefendBase3)(stateGraph.StartingToil = new LordToil_DefendBase3(baseCenter));
        var lordToil_DefendBase2 = new LordToil_DefendBase3(baseCenter);
        stateGraph.AddToil(lordToil_DefendBase2);
        var lordToil_AssaultColony = new LordToil_AssaultColony(true)
        {
            useAvoidGrid = true
        };
        stateGraph.AddToil(lordToil_AssaultColony);
        var transition = new Transition(lordToil_DefendBase, lordToil_DefendBase2);
        transition.AddSource(lordToil_AssaultColony);
        transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
        stateGraph.AddTransition(transition);
        var transition2 = new Transition(lordToil_DefendBase2, lordToil_DefendBase);
        transition2.AddTrigger(new Trigger_BecamePlayerEnemy());
        stateGraph.AddTransition(transition2);
        var transition3 = new Transition(lordToil_DefendBase, lordToil_AssaultColony);
        transition3.AddTrigger(new Trigger_FractionPawnsLost(0.1f));
        transition3.AddTrigger(new Trigger_PawnHarmed(0.01f));
        transition3.AddTrigger(new Trigger_ChanceOnPlayerHarmNPCBuilding(0.01f));
        transition3.AddPostAction(new TransitionAction_WakeAll());
        string message = "MessageDefendersAttacking"
            .Translate(faction.def.pawnsPlural, faction.Name, Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst();
        transition3.AddPreAction(new TransitionAction_Message(message, MessageTypeDefOf.ThreatBig));
        stateGraph.AddTransition(transition3);
        return stateGraph;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref faction, "faction");
        Scribe_Values.Look(ref baseCenter, "baseCenter");
    }
}