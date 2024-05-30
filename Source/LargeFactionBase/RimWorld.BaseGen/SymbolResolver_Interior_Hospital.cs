using LargeFactionBase;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Interior_Hospital : SymbolResolver
{
    private const int FoodStockpileSize = 3;

    public override void Resolve(ResolveParams rp)
    {
        var list = rp.faction != null && rp.faction.def.techLevel.IsNeolithicOrWorse()
            ? LargeFactionBase_ThingSetMakerDefOf.MapGen_TribalHospitalStockpile.root.Generate()
            : LargeFactionBase_ThingSetMakerDefOf.MapGen_HospitalStockpile.root.Generate();
        foreach (var thing in list)
        {
            var resolveParams = rp;
            resolveParams.singleThingToSpawn = thing;
            BaseGen.symbolStack.Push("thing", resolveParams);
        }

        BaseGen.symbolStack.Push("indoorLighting", rp);
        BaseGen.symbolStack.Push("indoorLighting", rp);
        InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
        BaseGen.symbolStack.Push("medicalBed", rp);
        if (Rand.Value > 0.5f)
        {
            BaseGen.symbolStack.Push("medicalBed", rp);
        }

        if (Rand.Value > 0.5f)
        {
            BaseGen.symbolStack.Push("medicalBed", rp);
        }

        rp.floorDef = rp.faction != null && (int)rp.faction.def.techLevel <= 3
            ? TerrainDefOf.MetalTile
            : Large_DefOf.SterileTile;

        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("prisonFilth", rp);
        BaseGen.symbolStack.Push("floor", rp);
    }
}