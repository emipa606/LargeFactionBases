using RimWorld;
using RimWorld.BaseGen;

public class SymbolResolver_SleepSpot : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var sleepingSpot = ThingDefOf.SleepingSpot;
        var resolveParams = rp;
        resolveParams.singleThingDef = sleepingSpot;
        resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit =
            rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit ?? true;
        BaseGen.symbolStack.Push("thing", resolveParams);
    }
}