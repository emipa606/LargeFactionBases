using System.Collections.Generic;
using LargeFactionBase;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_PlaceHumanLeatherArmchair : SymbolResolver
{
    private static readonly List<Thing> tables = [];

    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        tables.Clear();
        foreach (var intVec3 in rp.rect)
        {
            var thingList = intVec3.GetThingList(map);
            foreach (var thing in thingList)
            {
                if (thing.def.IsTable && !tables.Contains(thing))
                {
                    tables.Add(thing);
                }
            }
        }

        foreach (var thing in tables)
        {
            var cellRect = thing.OccupiedRect().ExpandedBy(1);
            var itemPLaced = false;
            foreach (var item in cellRect.EdgeCells.InRandomOrder())
            {
                if (cellRect.IsCorner(item) || !rp.rect.Contains(item) || !item.Standable(map) ||
                    item.GetEdifice(map) != null || itemPLaced && Rand.Bool)
                {
                    continue;
                }

                var value = item.x == cellRect.minX ? Rot4.East :
                    item.x == cellRect.maxX ? Rot4.West :
                    item.z != cellRect.minZ ? Rot4.South : Rot4.North;
                var resolveParams = rp;
                resolveParams.rect = CellRect.SingleCell(item);
                resolveParams.singleThingDef = ThingDef.Named("Armchair");
                resolveParams.singleThingStuff = Large_DefOf.Leather_Human;
                resolveParams.thingRot = value;
                BaseGen.symbolStack.Push("thing", resolveParams);
                itemPLaced = true;
            }
        }

        tables.Clear();
    }
}