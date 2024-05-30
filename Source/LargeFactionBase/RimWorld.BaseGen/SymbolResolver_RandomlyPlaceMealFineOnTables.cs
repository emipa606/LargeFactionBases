using LargeFactionBase;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_RandomlyPlaceMealFineOnTables : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var mealFine = Large_DefOf.MealFine;
        foreach (var intVec3 in rp.rect)
        {
            var thingList = intVec3.GetThingList(map);
            foreach (var thing in thingList)
            {
                if (!thing.def.IsTable || !Rand.Chance(0.15f) || intVec3.GetItemStackSpaceLeftFor(map, mealFine) == 0)
                {
                    continue;
                }

                var resolveParams = rp;
                resolveParams.rect = CellRect.SingleCell(intVec3);
                resolveParams.singleThingDef = mealFine;
                resolveParams.singleThingStackCount = Rand.Range(1, Mathf.Min(2, mealFine.stackLimit));
                BaseGen.symbolStack.Push("thing", resolveParams);
            }
        }
    }
}