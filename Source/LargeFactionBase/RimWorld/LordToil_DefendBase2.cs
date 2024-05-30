using LargeFactionBase;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class LordToil_DefendBase2(IntVec3 baseCenter) : LordToil
{
    public IntVec3 baseCenter = baseCenter;

    public override IntVec3 FlagLoc => baseCenter;

    public override void UpdateAllDuties()
    {
        foreach (var pawn in lord.ownedPawns)
        {
            pawn.mindState.duty = new PawnDuty(Large_DutyDefOf.DefendBase2, baseCenter);
        }
    }
}