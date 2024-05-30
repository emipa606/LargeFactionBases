using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Interior_PrisonCell4 : SymbolResolver
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
        for (var i = 0; i < Rand.Range(3, 9); i++)
        {
            BaseGen.symbolStack.Push("prisonerSpot", rp);
        }

        for (var j = 0; j < Rand.Range(24, 48); j++)
        {
            BaseGen.symbolStack.Push("prisonBile", rp);
        }

        for (var k = 0; k < Rand.Range(4, 8); k++)
        {
            BaseGen.symbolStack.Push("prisonFilth", rp);
        }
    }
}