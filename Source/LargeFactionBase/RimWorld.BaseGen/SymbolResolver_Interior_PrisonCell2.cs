using LargeFactionBase;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Interior_PrisonCell2 : SymbolResolver
{
    private const int FoodStockpileSize = 3;

    public override void Resolve(ResolveParams rp)
    {
        var value = default(ThingSetMakerParams);
        value.techLevel = rp.faction == null ? TechLevel.Spacer : rp.faction.def.techLevel;
        var resolveParams = rp;
        resolveParams.thingSetMakerDef = LargeFactionBase_ThingSetMakerDefOf.MapGen_PrisonCellStockpile2;
        resolveParams.thingSetMakerParams = value;
        resolveParams.innerStockpileSize = FoodStockpileSize;
        BaseGen.symbolStack.Push("innerStockpile", resolveParams);
        BaseGen.symbolStack.Push("indoorLighting", rp);
        InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
        BaseGen.symbolStack.Push("medicalPrisonerBed", rp);
        if (Rand.Value > 0.5f)
        {
            BaseGen.symbolStack.Push("medicalPrisonerBed", rp);
        }

        if (Rand.Value > 0.5f)
        {
            BaseGen.symbolStack.Push("medicalPrisonerBed", rp);
        }

        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
    }
}