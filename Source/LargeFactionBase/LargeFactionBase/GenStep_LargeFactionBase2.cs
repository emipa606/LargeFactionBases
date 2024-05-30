using System.Collections.Generic;
using RimWar.Planet;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace LargeFactionBase;

public class GenStep_LargeFactionBase2 : GenStep
{
    private static readonly List<CellRect> possibleRects = [];
    public FloatRange defaultPawnGroupPointsRange = SymbolResolver_Settlement2.DefaultPawnsPoints;

    private bool flag;

    private bool flag2;

    public override int SeedPart => 1806208471;

    public override void Generate(Map map, GenStepParams parms)
    {
        if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out var var))
        {
            var = CellRect.SingleCell(map.Center);
        }

        var faction = map.ParentFaction != null && map.ParentFaction != Faction.OfPlayer
            ? map.ParentFaction
            : Find.FactionManager.RandomEnemyFaction();
        var resolveParams = default(ResolveParams);
        resolveParams.rect = GetOutpostRect(var, map);
        resolveParams.faction = faction;
        resolveParams.edgeDefenseWidth = 4;
        resolveParams.thingSetMakerDef = LargeFactionBase_ThingSetMakerDefOf.MapGen_DefaultStockpile3;
        float num = GenDate.DaysPassed;
        var num2 = Mathf.Pow(num / 2f, 0.45f);
        var maxInclusive = Mathf.FloorToInt(num2 * 1.5f);
        var maxInclusive2 = Mathf.Min(Mathf.FloorToInt(num2 / 1.5f), 5);
        var minInclusive = Mathf.Max(1, Mathf.FloorToInt(num2 / 1.5f));
        var minInclusive2 = Mathf.Max(1, Mathf.FloorToInt(num2 / 2.5f));
        resolveParams.edgeDefenseTurretsCount = Rand.RangeInclusive(minInclusive, maxInclusive);
        resolveParams.edgeDefenseMortarsCount = Rand.RangeInclusive(minInclusive2, maxInclusive2);
        var maxInclusive3 = Mathf.Pow(num / 12f, 0.3f) * 0.6f;
        var minInclusive3 = Mathf.Pow(num / 12f, 0.36f);
        var num3 = Mathf.Max(Rand.Range(minInclusive3, maxInclusive3), 1f);
        var num4 = Mathf.Min(Find.Storyteller.difficulty.threatScale / 2.2f, 1f);
        var f = Find.StoryWatcher.statsRecord.numThreatBigs / 20f;
        var num5 = 0.4f * (Mathf.Pow(f, 4f) / (1f + Mathf.Pow(f, 4f)));
        float num6 = GenDate.DaysPassed;
        var persistentRandomValue = Find.World.info.persistentRandomValue;
        var num7 = Mathf.Sin((num6 / 10f) + (persistentRandomValue / 2f));
        var num8 = num7 + 1f;
        float num9 = Find.StoryWatcher.statsRecord.numThreatBigs;
        var num10 = Mathf.Sin((num6 / 10f * (num6 / num9) / 21.2f) + persistentRandomValue);
        var a = Mathf.Abs(num10 * num8);
        var num11 = Mathf.Min(a, 0.8f);
        var num12 = (num5 * num11) + 0.85f;
        BaseGen.globalSettings.map = map;
        var map2 = BaseGen.globalSettings?.map ?? Find.CurrentMap;
        var tile = map2.info.Tile;
        float num13 = WorldUtility.GetRimWarSettlementAtTile(tile).RimWarPoints;
        _ = WorldUtility.GetRimWarDataForFaction(faction).PointsFromSettlements;
        _ = WorldUtility.GetRimWarDataForFaction(faction).WorldSettlements.Count;
        var tile2 = WorldUtility.GetRimWarDataForFaction(faction).StrongestSettlement.Tile;
        float num16 = WorldUtility.GetRimWarSettlementAtTile(tile2).RimWarPoints;
        var tile3 = WorldUtility.GetRimWarDataForFaction(faction).GetCapitol.Tile;
        _ = WorldUtility.GetRimWarSettlementAtTile(tile3).RimWarPoints;
        var num18 = num13 / num16 * 1.5f;
        resolveParams.settlementPawnGroupPoints =
            defaultPawnGroupPointsRange.RandomInRange * num4 * num3 * num12 * num18;
        BaseGen.symbolStack.Push("settlement2", resolveParams);
        var resolveParams2 = default(ResolveParams);
        resolveParams2.rect = resolveParams.rect.ExpandedBy(20);
        var persistentRandomValue2 = Find.World.info.persistentRandomValue;
        var map3 = BaseGen.globalSettings?.map ?? Find.CurrentMap;
        var tile4 = map3.info.Tile;
        var num19 = Mathf.FloorToInt(tile4 * persistentRandomValue2 / 10f);
        var num20 = (tile4 * persistentRandomValue2) - (num19 * 10);
        var num21 = num20 / 10f;
        if ((num21 < 0.7f) & ((int)resolveParams.faction.def.techLevel > 3))
        {
            BaseGen.symbolStack.Push("extWalls", resolveParams2);
            var rect = resolveParams2.rect.ContractedBy(4);
            Mathf.Clamp(rect.Width, 1, Mathf.Min(rect.Width, rect.Height) / 2);
            var resolveParams3 = resolveParams2;
            resolveParams3.faction = faction;
            resolveParams3.rect = rect;
            BaseGen.symbolStack.Push("edgeGrain", resolveParams3);
            for (var i = 0; i < num2 * 2f; i++)
            {
                var resolveParams4 = resolveParams2;
                resolveParams4.rect = resolveParams2.rect.ContractedBy(20);
                resolveParams4.faction = faction;
                resolveParams4.singleThingDef = ThingDefOf.Turret_MiniTurret;
                if (Rand.Chance(0.1f))
                {
                    resolveParams4.singleThingDef = Large_DefOf.Turret_Sniper;
                }

                if (Rand.Chance(0.1f))
                {
                    resolveParams4.singleThingDef = Large_DefOf.Turret_Autocannon;
                }

                resolveParams4.rect = resolveParams4.rect.ExpandedBy(16);
                var edgeThingAvoidOtherEdgeThings = resolveParams2.edgeThingAvoidOtherEdgeThings;
                resolveParams4.edgeThingAvoidOtherEdgeThings =
                    !edgeThingAvoidOtherEdgeThings.HasValue || edgeThingAvoidOtherEdgeThings.Value;
                BaseGen.symbolStack.Push("edgeThing", resolveParams4);
            }
        }

        if ((num21 >= 0.7f) & ((int)resolveParams.faction.def.techLevel > 3))
        {
            var cellRect = resolveParams2.rect.ExpandedBy(1);
            var num23 = Mathf.Clamp(cellRect.Width, 1, Mathf.Min(cellRect.Width, cellRect.Height) / 2);
            var resolveParams5 = resolveParams2;
            resolveParams5.faction = faction;
            resolveParams5.rect = cellRect;
            BaseGen.symbolStack.Push("edgeIED", resolveParams5);
            for (var j = 0; j < num23; j++)
            {
                if (j % 2 != 0)
                {
                    continue;
                }

                var resolveParams6 = resolveParams2;
                resolveParams6.faction = faction;
                resolveParams6.rect = cellRect;
                BaseGen.symbolStack.Push("edgeWalls2", resolveParams6);
                if (!flag)
                {
                    break;
                }
            }

            var rect2 = resolveParams2.rect.ContractedBy(1);
            Mathf.Clamp(rect2.Width, 1, Mathf.Min(rect2.Width, rect2.Height) / 2);
            var resolveParams7 = resolveParams2;
            resolveParams7.faction = faction;
            resolveParams7.rect = rect2;
            BaseGen.symbolStack.Push("edgeIED", resolveParams7);
            for (var k = 0; k < num23; k++)
            {
                if (k % 2 != 0)
                {
                    continue;
                }

                var resolveParams8 = resolveParams2;
                resolveParams8.faction = faction;
                resolveParams8.rect = rect2;
                BaseGen.symbolStack.Push("edgeWalls2", resolveParams8);
                if (!flag)
                {
                    break;
                }
            }

            var rect3 = resolveParams2.rect.ContractedBy(3);
            Mathf.Clamp(rect3.Width, 1, Mathf.Min(rect3.Width, rect3.Height) / 2);
            var resolveParams9 = resolveParams2;
            resolveParams9.faction = faction;
            resolveParams9.rect = rect3;
            BaseGen.symbolStack.Push("edgeIED", resolveParams9);
            for (var l = 0; l < num23; l++)
            {
                if (l % 2 != 0)
                {
                    continue;
                }

                var resolveParams10 = resolveParams2;
                resolveParams10.faction = faction;
                resolveParams10.rect = rect3;
                BaseGen.symbolStack.Push("edgeWalls2", resolveParams10);
                if (!flag)
                {
                    break;
                }
            }

            var rect4 = resolveParams2.rect.ContractedBy(17);
            var resolveParams11 = resolveParams2;
            resolveParams11.faction = faction;
            resolveParams11.rect = rect4;
            BaseGen.symbolStack.Push("edgeSandbags", resolveParams11);
            _ = !flag2 ? cellRect.ContractedBy(19) : cellRect;
            for (var m = 0; m < num2 * 2f; m++)
            {
                resolveParams2.rect.ContractedBy(24);
            }
        }

        if ((num21 < 0.7f) & ((int)resolveParams.faction.def.techLevel <= 3))
        {
            var rect5 = resolveParams2.rect.ExpandedBy(1);
            var num26 = Mathf.Clamp(rect5.Width, 1, Mathf.Min(rect5.Width, rect5.Height) / 2);
            var resolveParams13 = resolveParams2;
            resolveParams13.faction = faction;
            resolveParams13.rect = rect5;
            BaseGen.symbolStack.Push("edgeSpikes", resolveParams13);
            for (var n = 0; n < num26; n++)
            {
                if (n % 2 != 0)
                {
                    continue;
                }

                var resolveParams14 = resolveParams2;
                resolveParams14.faction = faction;
                resolveParams14.rect = rect5;
                BaseGen.symbolStack.Push("edgeWalls2", resolveParams14);
                if (!flag)
                {
                    break;
                }
            }

            var rect6 = resolveParams2.rect.ContractedBy(1);
            Mathf.Clamp(rect6.Width, 1, Mathf.Min(rect6.Width, rect6.Height) / 2);
            var resolveParams15 = resolveParams2;
            resolveParams15.faction = faction;
            resolveParams15.rect = rect6;
            BaseGen.symbolStack.Push("edgeSpikes", resolveParams15);
            for (var num28 = 0; num28 < num26; num28++)
            {
                if (num28 % 2 != 0)
                {
                    continue;
                }

                var resolveParams16 = resolveParams2;
                resolveParams16.faction = faction;
                resolveParams16.rect = rect6;
                BaseGen.symbolStack.Push("edgeWalls2", resolveParams16);
                if (!flag)
                {
                    break;
                }
            }

            var rect7 = resolveParams2.rect.ContractedBy(3);
            Mathf.Clamp(rect7.Width, 1, Mathf.Min(rect7.Width, rect7.Height) / 2);
            var resolveParams17 = resolveParams2;
            resolveParams17.faction = faction;
            resolveParams17.rect = rect7;
            BaseGen.symbolStack.Push("edgeSpikes", resolveParams17);
            for (var num30 = 0; num30 < num26; num30++)
            {
                if (num30 % 2 != 0)
                {
                    continue;
                }

                var resolveParams18 = resolveParams2;
                resolveParams18.faction = faction;
                resolveParams18.rect = rect7;
                BaseGen.symbolStack.Push("edgeWalls2", resolveParams18);
                if (!flag)
                {
                    break;
                }
            }
        }

        if ((num21 >= 0.7f) & ((int)resolveParams.faction.def.techLevel <= 3))
        {
            var rect8 = resolveParams2.rect.ExpandedBy(1);
            Mathf.Clamp(rect8.Width, 1, Mathf.Min(rect8.Width, rect8.Height) / 2);
            var resolveParams19 = resolveParams2;
            resolveParams19.faction = faction;
            resolveParams19.rect = rect8;
            BaseGen.symbolStack.Push("edgeSpikes2", resolveParams19);
            var rect9 = resolveParams2.rect.ContractedBy(1);
            Mathf.Clamp(rect9.Width, 1, Mathf.Min(rect9.Width, rect9.Height) / 2);
            var resolveParams20 = resolveParams2;
            resolveParams20.faction = faction;
            resolveParams20.rect = rect9;
            BaseGen.symbolStack.Push("edgeSpikes2", resolveParams20);
            var rect10 = resolveParams2.rect.ContractedBy(3);
            Mathf.Clamp(rect10.Width, 1, Mathf.Min(rect10.Width, rect10.Height) / 2);
            var resolveParams21 = resolveParams2;
            resolveParams21.faction = faction;
            resolveParams21.rect = rect10;
            BaseGen.symbolStack.Push("edgeSpikes2", resolveParams21);
            var rect11 = resolveParams2.rect.ContractedBy(5);
            Mathf.Clamp(rect11.Width, 1, Mathf.Min(rect11.Width, rect11.Height) / 2);
            var resolveParams22 = resolveParams2;
            resolveParams22.faction = faction;
            resolveParams22.rect = rect11;
            BaseGen.symbolStack.Push("edgeSpikes2", resolveParams22);
            var rect12 = resolveParams2.rect.ContractedBy(7);
            Mathf.Clamp(rect12.Width, 1, Mathf.Min(rect12.Width, rect12.Height) / 2);
            var resolveParams23 = resolveParams2;
            resolveParams23.faction = faction;
            resolveParams23.rect = rect12;
            BaseGen.symbolStack.Push("edgeSpikes2", resolveParams23);
            var rect13 = resolveParams2.rect.ContractedBy(9);
            Mathf.Clamp(rect13.Width, 1, Mathf.Min(rect13.Width, rect13.Height) / 2);
            var resolveParams24 = resolveParams2;
            resolveParams24.faction = faction;
            resolveParams24.rect = rect13;
            BaseGen.symbolStack.Push("edgeSpikes2", resolveParams24);
            var rect14 = resolveParams2.rect.ContractedBy(11);
            Mathf.Clamp(rect14.Width, 1, Mathf.Min(rect14.Width, rect14.Height) / 2);
            var resolveParams25 = resolveParams2;
            resolveParams25.faction = faction;
            resolveParams25.rect = rect14;
            BaseGen.symbolStack.Push("edgeSpikes2", resolveParams25);
        }

        BaseGen.Generate();
    }

    private CellRect GetOutpostRect(CellRect rectToDefend, Map map)
    {
        var num = 1f;
        float num2 = Mathf.Min(GenDate.DaysPassed, 120);
        var b = (1f + (num2 / 300f)) * num;
        var num3 = Mathf.Min(2f, b);
        var num4 = num3 * 48f;
        var num5 = Mathf.FloorToInt(num4);
        var num6 = Mathf.FloorToInt(num4 / 2f);
        possibleRects.Add(new CellRect(rectToDefend.minX - 1 - num5, rectToDefend.CenterCell.z - num6, num5, num5));
        possibleRects.Add(new CellRect(rectToDefend.maxX + 1, rectToDefend.CenterCell.z - num6, num5, num5));
        possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - num6, rectToDefend.minZ - 1 - num6, num5, num5));
        possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - num6, rectToDefend.maxZ + 1, num5, num5));
        var mapRect = new CellRect(0, 0, map.Size.x, map.Size.z);
        possibleRects.RemoveAll(x => !x.FullyContainedWithin(mapRect));
        return possibleRects.Any() ? possibleRects.RandomElement() : rectToDefend;
    }
}