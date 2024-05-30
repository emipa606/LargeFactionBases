using LargeFactionBase;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_FillWithMedBeds : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var thingDef = rp.singleThingDef ?? (rp.faction == null || (int)rp.faction.def.techLevel < 4
            ? Rand.Element(ThingDefOf.Bed, ThingDefOf.Bedroll)
            : Large_DefOf.HospitalBed);
        var singleThingStuff = rp.singleThingStuff == null || !rp.singleThingStuff.stuffProps.CanMake(thingDef)
            ? GenStuff.RandomStuffInexpensiveFor(thingDef, rp.faction)
            : rp.singleThingStuff;
        var @bool = Rand.Bool;
        foreach (var item in rp.rect)
        {
            if (@bool)
            {
                if (item.x % 3 != 0 || item.z % 2 != 0)
                {
                    continue;
                }
            }
            else if (item.x % 2 != 0 || item.z % 3 != 0)
            {
                continue;
            }

            var rot = !@bool ? Rot4.North : Rot4.West;
            if (GenSpawn.WouldWipeAnythingWith(item, rot, thingDef, map,
                    x => x.def.category == ThingCategory.Building) ||
                BaseGenUtility.AnyDoorAdjacentCardinalTo(GenAdj.OccupiedRect(item, rot, thingDef.Size), map))
            {
                continue;
            }

            var resolveParams = rp;
            resolveParams.rect = GenAdj.OccupiedRect(item, rot, thingDef.size);
            resolveParams.singleThingDef = thingDef;
            resolveParams.singleThingStuff = singleThingStuff;
            resolveParams.thingRot = rot;
            BaseGen.symbolStack.Push("medBed", resolveParams);
        }
    }
}