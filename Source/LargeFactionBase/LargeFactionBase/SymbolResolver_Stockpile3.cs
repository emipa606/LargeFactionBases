using System.Collections.Generic;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace LargeFactionBase;

public class SymbolResolver_Stockpile3 : SymbolResolver
{
    private const float FreeCellsFraction = 0.5f;
    private readonly List<IntVec3> cells = [];

    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        if (rp.stockpileConcreteContents != null)
        {
            CalculateFreeCells(rp.rect, 0f);
            var num = 0;
            var num2 = rp.stockpileConcreteContents.Count - 1;
            while (num2 >= 0 && num < cells.Count)
            {
                GenSpawn.Spawn(rp.stockpileConcreteContents[num2], cells[num], map);
                num++;
                num2--;
            }

            for (var num3 = rp.stockpileConcreteContents.Count - 1; num3 >= 0; num3--)
            {
                if (!rp.stockpileConcreteContents[num3].Spawned)
                {
                    rp.stockpileConcreteContents[num3].Destroy();
                }
            }

            rp.stockpileConcreteContents.Clear();
            return;
        }

        CalculateFreeCells(rp.rect, FreeCellsFraction);
        var thingSetMakerDef = rp.thingSetMakerDef ?? LargeFactionBase_ThingSetMakerDefOf.MapGen_DefaultStockpile3;
        var thingSetMakerParams = rp.thingSetMakerParams;
        ThingSetMakerParams value;
        if (thingSetMakerParams.HasValue)
        {
            value = rp.thingSetMakerParams.Value;
        }
        else
        {
            value = default;
            value.techLevel = rp.faction != null ? rp.faction.def.techLevel : TechLevel.Undefined;
            value.validator = x =>
                rp.faction == null || (int)x.techLevel >= (int)rp.faction.def.techLevel || !x.IsWeapon ||
                x.GetStatValueAbstract(StatDefOf.MarketValue, GenStuff.DefaultStuffFor(x)) >= 100f;
            var stockpileMarketValue = rp.stockpileMarketValue;
            var num4 = stockpileMarketValue ?? Mathf.Min(cells.Count * 130f, 1800f);
            value.totalMarketValueRange = new FloatRange(num4, num4);
        }

        var countRange = value.countRange;
        if (!countRange.HasValue)
        {
            value.countRange = new IntRange(cells.Count, cells.Count);
        }

        var resolveParams = rp;
        resolveParams.thingSetMakerDef = thingSetMakerDef;
        resolveParams.thingSetMakerParams = value;
        BaseGen.symbolStack.Push("thingSet2", resolveParams);
    }

    private void CalculateFreeCells(CellRect rect, float freeCellsFraction)
    {
        var map = BaseGen.globalSettings.map;
        cells.Clear();
        foreach (var item in rect)
        {
            if (item.Standable(map) && item.GetFirstItem(map) == null)
            {
                cells.Add(item);
            }
        }

        var num = (int)(freeCellsFraction * cells.Count);
        for (var i = 0; i < num; i++)
        {
            cells.RemoveAt(Rand.Range(0, cells.Count));
        }

        cells.Shuffle();
    }
}