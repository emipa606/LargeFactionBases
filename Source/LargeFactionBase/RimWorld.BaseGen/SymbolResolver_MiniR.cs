using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_MiniR : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var singleThingDef =
            Rand.Element(ThingDefOf.Filth_Blood, ThingDefOf.Filth_CorpseBile, ThingDefOf.Filth_DriedBlood);
        var resolveParams = rp;
        resolveParams.singleThingDef = singleThingDef;
        var skipSingleThingIfHasToWipeBuildingOrDoesntFit = rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit;
        resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit =
            !skipSingleThingIfHasToWipeBuildingOrDoesntFit.HasValue ||
            skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value;
        BaseGen.symbolStack.Push("thing", resolveParams);
    }
}