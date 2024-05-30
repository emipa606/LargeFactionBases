using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace LargeFactionBase;

public class SymbolResolver_ThingSet2 : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var thingSetMakerDef = rp.thingSetMakerDef ?? LargeFactionBase_ThingSetMakerDefOf.MapGen_DefaultStockpile2;
        var thingSetMakerParams = rp.thingSetMakerParams;
        ThingSetMakerParams parms;
        if (thingSetMakerParams.HasValue)
        {
            parms = rp.thingSetMakerParams.Value;
        }
        else
        {
            var num = rp.rect.Cells.Count(x => x.Standable(map) && x.GetFirstItem(map) == null);
            parms = default;
            parms.countRange = new IntRange(num, num);
            parms.techLevel = rp.faction != null ? rp.faction.def.techLevel : TechLevel.Undefined;
        }

        var list = thingSetMakerDef.root.Generate(parms);
        foreach (var thingToSpawn in list)
        {
            var resolveParams = rp;
            resolveParams.singleThingToSpawn = thingToSpawn;
            BaseGen.symbolStack.Push("thing", resolveParams);
        }
    }
}