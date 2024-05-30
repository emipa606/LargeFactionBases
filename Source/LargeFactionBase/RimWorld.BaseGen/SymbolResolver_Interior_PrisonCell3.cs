using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Interior_PrisonCell3 : SymbolResolver
{
    private const int FoodStockpileSize = 3;

    public override void Resolve(ResolveParams rp)
    {
        var value = default(ThingSetMakerParams);
        value.techLevel = rp.faction == null ? TechLevel.Spacer : rp.faction.def.techLevel;
        var resolveParams = rp;
        resolveParams.thingSetMakerDef = ThingSetMakerDefOf.MapGen_PrisonCellStockpile;
        resolveParams.thingSetMakerParams = value;
        resolveParams.innerStockpileSize = FoodStockpileSize;
        BaseGen.symbolStack.Push("innerStockpile", resolveParams);
        InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
        BaseGen.symbolStack.Push("prisonerSpot", rp);
        if (Rand.Value > 0.5f)
        {
            BaseGen.symbolStack.Push("prisonerSpot", rp);
        }

        BaseGen.symbolStack.Push("prisonDefense", rp);
        if (Rand.Value > 0.5f)
        {
            BaseGen.symbolStack.Push("prisonerSpot", rp);
        }

        BaseGen.symbolStack.Push("prisonDefense", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
    }
}