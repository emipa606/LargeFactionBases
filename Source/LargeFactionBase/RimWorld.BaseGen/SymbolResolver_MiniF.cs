using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_MiniF : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var singleThingDef = rp.faction != null && rp.faction.def.techLevel.IsNeolithicOrWorse()
            ? rp.singleThingDef ?? Rand.Element(ThingDefOf.Filth_Blood, ThingDefOf.Filth_CorpseBile,
                ThingDefOf.Filth_Vomit, ThingDefOf.Filth_DriedBlood)
            : rp.singleThingDef ?? Rand.Element(ThingDefOf.Filth_Blood, ThingDefOf.Filth_CorpseBile,
                ThingDefOf.Filth_Vomit, ThingDefOf.Filth_DriedBlood, ThingDefOf.Filth_AmnioticFluid);
        var resolveParams = rp;
        resolveParams.singleThingDef = singleThingDef;
        var skipSingleThingIfHasToWipeBuildingOrDoesntFit = rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit;
        resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit =
            !skipSingleThingIfHasToWipeBuildingOrDoesntFit.HasValue ||
            skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value;
        BaseGen.symbolStack.Push("thing", resolveParams);
    }
}