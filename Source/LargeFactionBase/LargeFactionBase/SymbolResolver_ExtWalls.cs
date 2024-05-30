using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace LargeFactionBase;

public class SymbolResolver_ExtWalls : SymbolResolver
{
    public ThingSetMakerDef thingSetMakerDef;

    public override void Resolve(ResolveParams rp)
    {
        rp.floorDef = BaseGenUtility.CorrespondingTerrainDef(
            rp.wallStuff = DefDatabase<ThingDef>.AllDefs
                .Where(d => d.IsStuff && d.stuffProps.CanMake(ThingDefOf.Wall) &&
                            d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony) && d != ThingDef.Named("Jade") &&
                            d.BaseMarketValue < 1f).ToList().RandomElementByWeight(x => 3f + (1f / x.BaseMarketValue)),
            true);
        ResolveParams resolveParams5;
        ResolveParams resolveParams4;
        ResolveParams resolveParams3;
        var resolveParams6 = resolveParams5 = resolveParams4 = resolveParams3 = rp;
        var cellRect = new CellRect(rp.rect.maxX, rp.rect.maxZ, 1, 1);
        resolveParams5.rect = cellRect.ExpandedBy(6);
        resolveParams6.rect = new CellRect(rp.rect.minX, rp.rect.maxZ, 1, 1).ExpandedBy(6);
        resolveParams4.rect = new CellRect(rp.rect.maxX, rp.rect.minZ, 1, 1).ExpandedBy(6);
        resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ, 1, 1).ExpandedBy(6);
        var resolveParams7 = rp;
        resolveParams7.rect = resolveParams7.rect.ContractedBy(2);
        BaseGen.symbolStack.Push("edgeWalls3", resolveParams7);
        var resolveParams8 = rp;
        resolveParams8.rect = resolveParams7.rect.ContractedBy(1);
        BaseGen.symbolStack.Push("edgeWalls3", resolveParams8);
        var resolveParams9 = rp;
        BaseGen.symbolStack.Push("edgeWalls3", resolveParams9);
        var resolveParams10 = rp;
        resolveParams10.rect = resolveParams9.rect.ExpandedBy(2);
        BaseGen.symbolStack.Push("edgeWalls3", resolveParams10);
        var resolveParams11 = rp;
        resolveParams11.rect = resolveParams10.rect.ExpandedBy(2);
        BaseGen.symbolStack.Push("edgeWalls3", resolveParams11);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams5);
        var resolveParams12 = resolveParams5;
        resolveParams12.rect = resolveParams5.rect.ContractedBy(1);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams12);
        var resolveParams13 = resolveParams5;
        resolveParams13.rect = resolveParams5.rect.ContractedBy(2);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams13);
        var value = Rand.Value;
        if (value < 0.25f)
        {
            BaseGen.symbolStack.Push("emptyRoom", resolveParams13);
        }
        else if (value is >= 0.25f and < 0.35f)
        {
            BaseGen.symbolStack.Push("prisonCell4", resolveParams13);
        }
        else if (value is >= 0.35f and < 0.65f)
        {
            BaseGen.symbolStack.Push("prisonCell2", resolveParams13);
        }
        else if (value is >= 0.65f and < 0.85f)
        {
            BaseGen.symbolStack.Push("prisonCell3", resolveParams13);
        }
        else if (value is >= 0.85f and < 0.95f)
        {
            BaseGen.symbolStack.Push("storage", resolveParams13);
        }
        else
        {
            BaseGen.symbolStack.Push("storage", resolveParams13);
        }

        BaseGen.symbolStack.Push("edgeWalls", resolveParams6);
        var resolveParams14 = resolveParams6;
        resolveParams14.rect = resolveParams6.rect.ContractedBy(1);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams14);
        var resolveParams15 = resolveParams6;
        resolveParams15.rect = resolveParams6.rect.ContractedBy(2);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams15);
        var value2 = Rand.Value;
        if (value2 < 0.25f)
        {
            BaseGen.symbolStack.Push("emptyRoom", resolveParams15);
        }
        else if (value2 is >= 0.25f and < 0.35f)
        {
            BaseGen.symbolStack.Push("prisonCell4", resolveParams15);
        }
        else if (value2 is >= 0.35f and < 0.65f)
        {
            BaseGen.symbolStack.Push("prisonCell2", resolveParams15);
        }
        else if (value2 is >= 0.65f and < 0.85f)
        {
            BaseGen.symbolStack.Push("prisonCell3", resolveParams15);
        }
        else if (value2 is >= 0.85f and < 0.95f)
        {
            BaseGen.symbolStack.Push("storage", resolveParams15);
        }
        else
        {
            BaseGen.symbolStack.Push("storage", resolveParams15);
        }

        BaseGen.symbolStack.Push("edgeWalls", resolveParams4);
        var resolveParams16 = resolveParams4;
        resolveParams16.rect = resolveParams4.rect.ContractedBy(1);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams16);
        var resolveParams17 = resolveParams4;
        resolveParams17.rect = resolveParams4.rect.ContractedBy(2);
        var value3 = Rand.Value;
        if (value3 < 0.25f)
        {
            BaseGen.symbolStack.Push("emptyRoom", resolveParams17);
        }
        else if (value3 is >= 0.25f and < 0.35f)
        {
            BaseGen.symbolStack.Push("prisonCell4", resolveParams17);
        }
        else if (value3 is >= 0.35f and < 0.65f)
        {
            BaseGen.symbolStack.Push("prisonCell2", resolveParams17);
        }
        else if (value3 is >= 0.65f and < 0.85f)
        {
            BaseGen.symbolStack.Push("prisonCell3", resolveParams17);
        }
        else if (value3 is >= 0.85f and < 0.95f)
        {
            BaseGen.symbolStack.Push("storage", resolveParams17);
        }
        else
        {
            BaseGen.symbolStack.Push("storage", resolveParams17);
        }

        BaseGen.symbolStack.Push("edgeWalls", resolveParams3);
        var resolveParams18 = resolveParams3;
        resolveParams18.rect = resolveParams3.rect.ContractedBy(1);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams18);
        var resolveParams19 = resolveParams3;
        resolveParams19.rect = resolveParams3.rect.ContractedBy(2);
        var value4 = Rand.Value;
        if (value4 < 0.25f)
        {
            BaseGen.symbolStack.Push("emptyRoom", resolveParams19);
        }
        else if (value4 is >= 0.25f and < 0.35f)
        {
            BaseGen.symbolStack.Push("prisonCell4", resolveParams19);
        }
        else if (value4 is >= 0.35f and < 0.65f)
        {
            BaseGen.symbolStack.Push("prisonCell2", resolveParams19);
        }
        else if (value4 is >= 0.65f and < 0.85f)
        {
            BaseGen.symbolStack.Push("prisonCell3", resolveParams19);
        }
        else if (value4 is >= 0.85f and < 0.95f)
        {
            BaseGen.symbolStack.Push("storage", resolveParams19);
        }
        else
        {
            BaseGen.symbolStack.Push("storage", resolveParams19);
        }

        var resolveParams20 = rp;
        resolveParams20.rect = cellRect.ExpandedBy(4);
        var rect = new CellRect(rp.rect.minX - 4, rp.rect.minZ - 4, rp.rect.Width + 8, rp.rect.Height + 8);
        resolveParams20.rect = rect;
        resolveParams20.floorDef = TerrainDefOf.Bridge;
        var floorOnlyIfTerrainSupports = rp.floorOnlyIfTerrainSupports;
        resolveParams20.floorOnlyIfTerrainSupports =
            !floorOnlyIfTerrainSupports.HasValue || floorOnlyIfTerrainSupports.Value;
        BaseGen.symbolStack.Push("floor", resolveParams20);
    }
}