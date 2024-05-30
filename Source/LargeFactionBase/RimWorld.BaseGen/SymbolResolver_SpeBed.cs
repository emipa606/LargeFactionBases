using LargeFactionBase;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_SpeBed : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var singleThingDef = rp.faction != null && rp.faction.def.techLevel.IsNeolithicOrWorse()
            ? rp.singleThingDef ?? Rand.Element(ThingDefOf.Bedroll, ThingDefOf.SleepingSpot)
            : rp.singleThingDef ?? Rand.Element(Large_DefOf.HospitalBed, ThingDefOf.Bed, ThingDefOf.Bedroll,
                ThingDefOf.SleepingSpot);
        var resolveParams = rp;
        resolveParams.singleThingDef = singleThingDef;
        var skipSingleThingIfHasToWipeBuildingOrDoesntFit = rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit;
        resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit =
            !skipSingleThingIfHasToWipeBuildingOrDoesntFit.HasValue ||
            skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value;
        BaseGen.symbolStack.Push("thing", resolveParams);
    }
}