using LargeFactionBase;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_RandomlyPlaceHumanMeat : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var meat_Human = Large_DefOf.Meat_Human;
        foreach (var intVec3 in rp.rect)
        {
            var thingList = intVec3.GetThingList(map);
            foreach (var thing in thingList)
            {
                if (!thing.def.IsTable || !Rand.Chance(0.15f) || intVec3.GetItemStackSpaceLeftFor(map, meat_Human) == 0)
                {
                    continue;
                }

                var resolveParams = rp;
                resolveParams.rect = CellRect.SingleCell(intVec3);
                resolveParams.singleThingDef = meat_Human;
                resolveParams.singleThingStackCount = Rand.Range(10, Mathf.Min(30, meat_Human.stackLimit));
                BaseGen.symbolStack.Push("thing", resolveParams);
            }
        }
    }
}