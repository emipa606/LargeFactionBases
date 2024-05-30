using LargeFactionBase;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_RandomlyPlaceWakeUpOnTables : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var singleThingDef = rp.faction != null && rp.faction.def.techLevel.IsNeolithicOrWorse()
            ? Large_DefOf.PsychiteTea
            : Large_DefOf.WakeUp;
        foreach (var intVec3 in rp.rect)
        {
            var thingList = intVec3.GetThingList(map);
            foreach (var thing in thingList)
            {
                if (!thing.def.IsTable || !Rand.Chance(0.15f) ||
                    intVec3.GetItemStackSpaceLeftFor(map, singleThingDef) == 0)
                {
                    continue;
                }

                var resolveParams = rp;
                resolveParams.rect = CellRect.SingleCell(intVec3);
                resolveParams.singleThingDef = singleThingDef;
                resolveParams.singleThingStackCount = Rand.Range(1, Mathf.Min(3, singleThingDef.stackLimit));
                BaseGen.symbolStack.Push("thing", resolveParams);
            }
        }
    }
}