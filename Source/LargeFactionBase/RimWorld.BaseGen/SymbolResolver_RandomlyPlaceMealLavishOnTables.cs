using LargeFactionBase;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_RandomlyPlaceMealLavishOnTables : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var mealLavish = Large_DefOf.MealLavish;
        foreach (var intVec3 in rp.rect)
        {
            var thingList = intVec3.GetThingList(map);
            foreach (var thing in thingList)
            {
                if (!thing.def.IsTable || !Rand.Chance(0.15f) || intVec3.GetItemStackSpaceLeftFor(map, mealLavish) == 0)
                {
                    continue;
                }

                var resolveParams = rp;
                resolveParams.rect = CellRect.SingleCell(intVec3);
                resolveParams.singleThingDef = mealLavish;
                resolveParams.singleThingStackCount = Rand.Range(1, Mathf.Min(2, mealLavish.stackLimit));
                BaseGen.symbolStack.Push("thing", resolveParams);
            }
        }
    }
}