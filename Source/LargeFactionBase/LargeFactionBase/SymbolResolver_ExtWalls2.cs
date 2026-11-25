using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace LargeFactionBase;

public class SymbolResolver_ExtWalls2 : SymbolResolver
{
    public ThingSetMakerDef thingSetMakerDef;

    public override void Resolve(ResolveParams rp)
    {
        var wallStuff = DefDatabase<ThingDef>.AllDefs
            .Where(d => d.IsStuff && d.stuffProps.CanMake(ThingDefOf.Wall) &&
                        d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony) && d != ThingDef.Named("Jade"))
            .RandomElementByWeight(x => 3f + (1f / x.BaseMarketValue));
        rp.wallStuff = wallStuff;
        rp.floorDef = BaseGenUtility.CorrespondingTerrainDef(wallStuff, true);
        var resolveParams = rp;
        resolveParams.rect = rp.rect.ExpandedBy(9);
        BaseGen.symbolStack.Push("edgeWalls", resolveParams);
    }
}