using UnityEngine;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_RandomlyPlacePemmicanOnTables : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var pemmican = ThingDefOf.Pemmican;
        foreach (var intVec3 in rp.rect)
        {
            var thingList = intVec3.GetThingList(map);
            foreach (var thing in thingList)
            {
                if (!thing.def.IsTable || !Rand.Chance(0.15f) || intVec3.GetItemStackSpaceLeftFor(map, pemmican) == 0)
                {
                    continue;
                }

                var resolveParams = rp;
                resolveParams.rect = CellRect.SingleCell(intVec3);
                resolveParams.singleThingDef = pemmican;
                resolveParams.singleThingStackCount = Rand.Range(10, Mathf.Min(30, pemmican.stackLimit));
                BaseGen.symbolStack.Push("thing", resolveParams);
            }
        }
    }
}