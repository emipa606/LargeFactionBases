using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace LargeFactionBase;

public class GenStep_LargePower : GenStep
{
    private const int MaxDistToExistingNetForTurrets = 13;

    private const int RoofPadding = 2;

    private static readonly IntRange MaxDistanceBetweenBatteryAndTransmitter = new IntRange(20, 50);

    private static readonly List<IntVec3> tmpTransmitterCells = [];
    public readonly bool canSpawnBatteries = true;

    public readonly bool canSpawnPowerGenerators = true;

    public readonly bool spawnRoofOverNewBatteries = true;

    private readonly List<IntVec3> tmpCells = [];

    private readonly Dictionary<PowerNet, bool> tmpPowerNetPredicateResults = new Dictionary<PowerNet, bool>();

    private readonly List<Thing> tmpThings = [];

    public FloatRange newBatteriesInitialStoredEnergyPctRange = new FloatRange(0.9f, 0.1f);

    public override int SeedPart => 1186199651;

    public override void Generate(Map map, GenStepParams parms)
    {
        map.skyManager.ForceSetCurSkyGlow(1f);
        map.powerNetManager.UpdatePowerNetsAndConnections_First();
        UpdateDesiredPowerOutputForAllGenerators(map);
        EnsureBatteriesConnectedAndMakeSense(map);
        EnsurePowerUsersConnected(map);
        EnsureGeneratorsConnectedAndMakeSense(map);
        tmpThings.Clear();
    }

    private void UpdateDesiredPowerOutputForAllGenerators(Map map)
    {
        tmpThings.Clear();
        tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
        foreach (var thing in tmpThings)
        {
            if (IsPowerGenerator(thing))
            {
                thing.TryGetComp<CompPowerPlant>()?.UpdateDesiredPowerOutput();
            }
        }
    }

    private void EnsureBatteriesConnectedAndMakeSense(Map map)
    {
        tmpThings.Clear();
        tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
        foreach (var thing in tmpThings)
        {
            var compPowerBattery = thing.TryGetComp<CompPowerBattery>();
            if (compPowerBattery == null)
            {
                continue;
            }

            var powerNet = compPowerBattery.PowerNet;
            if (powerNet != null && HasAnyPowerGenerator(powerNet))
            {
                continue;
            }

            map.powerNetManager.UpdatePowerNetsAndConnections_First();
            if (TryFindClosestReachableNet(compPowerBattery.parent.Position, HasAnyPowerGenerator, map,
                    out var foundNet, out var closestTransmitter))
            {
                map.floodFiller.ReconstructLastFloodFillPath(closestTransmitter, tmpCells);
                if (canSpawnPowerGenerators)
                {
                    var count = tmpCells.Count;
                    var chance = Mathf.InverseLerp(MaxDistanceBetweenBatteryAndTransmitter.min,
                        MaxDistanceBetweenBatteryAndTransmitter.max, count);
                    if (Rand.Chance(chance) && TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map,
                            compPowerBattery.parent.Faction, out var newPowerGenerator))
                    {
                        SpawnTransmitters(compPowerBattery.parent.Position, newPowerGenerator.Position, map,
                            compPowerBattery.parent.Faction);
                        foundNet = null;
                    }
                }

                if (foundNet != null)
                {
                    SpawnTransmitters(tmpCells, map, compPowerBattery.parent.Faction);
                }
            }
            else if (canSpawnPowerGenerators && TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map,
                         compPowerBattery.parent.Faction, out var newPowerGenerator2))
            {
                SpawnTransmitters(compPowerBattery.parent.Position, newPowerGenerator2.Position, map,
                    compPowerBattery.parent.Faction);
            }
        }
    }

    private void EnsurePowerUsersConnected(Map map)
    {
        tmpThings.Clear();
        tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
        foreach (var thing in tmpThings)
        {
            if (!IsPowerUser(thing))
            {
                continue;
            }

            var powerComp = thing.TryGetComp<CompPowerTrader>();
            var powerNet = powerComp.PowerNet;
            if (powerNet is { hasPowerSource: true })
            {
                TryTurnOnImmediately(powerComp, map);
                continue;
            }

            map.powerNetManager.UpdatePowerNetsAndConnections_First();
            if (TryFindClosestReachableNet(powerComp.parent.Position,
                    x => x.CurrentEnergyGainRate() -
                        (powerComp.Props.PowerConsumption * CompPower.WattsToWattDaysPerTick) > 1E-07f, map,
                    out _, out var closestTransmitter))
            {
                map.floodFiller.ReconstructLastFloodFillPath(closestTransmitter, tmpCells);
                var canAndConnect = false;
                if (canSpawnPowerGenerators && thing is Building_Turret && tmpCells.Count > 13)
                {
                    canAndConnect = TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(thing, map);
                }

                if (!canAndConnect)
                {
                    SpawnTransmitters(tmpCells, map, thing.Faction);
                }

                TryTurnOnImmediately(powerComp, map);
            }
            else if (canSpawnPowerGenerators && TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(thing, map))
            {
                TryTurnOnImmediately(powerComp, map);
            }
            else if (TryFindClosestReachableNet(powerComp.parent.Position, x => x.CurrentStoredEnergy() > 1E-07f, map,
                         out _, out closestTransmitter))
            {
                map.floodFiller.ReconstructLastFloodFillPath(closestTransmitter, tmpCells);
                SpawnTransmitters(tmpCells, map, thing.Faction);
            }
            else if (canSpawnBatteries &&
                     TrySpawnBatteryNear(thing.Position, map, thing.Faction, out var newBattery))
            {
                SpawnTransmitters(thing.Position, newBattery.Position, map, thing.Faction);
                if (newBattery.GetComp<CompPowerBattery>().StoredEnergy > 0f)
                {
                    TryTurnOnImmediately(powerComp, map);
                }
            }
        }
    }

    private void EnsureGeneratorsConnectedAndMakeSense(Map map)
    {
        tmpThings.Clear();
        tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
        foreach (var thing in tmpThings)
        {
            if (!IsPowerGenerator(thing))
            {
                continue;
            }

            var powerNet = thing.TryGetComp<CompPower>().PowerNet;
            if (powerNet != null && HasAnyPowerUser(powerNet))
            {
                continue;
            }

            map.powerNetManager.UpdatePowerNetsAndConnections_First();
            if (!TryFindClosestReachableNet(thing.Position, HasAnyPowerUser, map, out var _,
                    out var closestTransmitter))
            {
                continue;
            }

            map.floodFiller.ReconstructLastFloodFillPath(closestTransmitter, tmpCells);
            SpawnTransmitters(tmpCells, map, thing.Faction);
        }
    }

    private bool IsPowerUser(Thing thing)
    {
        var compPowerTrader = thing.TryGetComp<CompPowerTrader>();
        return compPowerTrader != null && (compPowerTrader.PowerOutput < 0f ||
                                           !compPowerTrader.PowerOn && compPowerTrader.Props.PowerConsumption > 0f);
    }

    private bool IsPowerGenerator(Thing thing)
    {
        if (thing.TryGetComp<CompPowerPlant>() != null)
        {
            return true;
        }

        var compPowerTrader = thing.TryGetComp<CompPowerTrader>();
        return compPowerTrader != null && (compPowerTrader.PowerOutput > 0f ||
                                           !compPowerTrader.PowerOn && compPowerTrader.Props.PowerConsumption < 0f);
    }

    private bool HasAnyPowerGenerator(PowerNet net)
    {
        var powerComps = net.powerComps;
        foreach (var powerTrader in powerComps)
        {
            if (IsPowerGenerator(powerTrader.parent))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasAnyPowerUser(PowerNet net)
    {
        var powerComps = net.powerComps;
        foreach (var powerTrader in powerComps)
        {
            if (IsPowerUser(powerTrader.parent))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryFindClosestReachableNet(IntVec3 root, Predicate<PowerNet> predicate, Map map, out PowerNet foundNet,
        out IntVec3 closestTransmitter)
    {
        tmpPowerNetPredicateResults.Clear();
        PowerNet foundNetLocal = null;
        var closestTransmitterLocal = IntVec3.Invalid;
        map.floodFiller.FloodFill(root, x => EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
        {
            var powerNet = x.GetTransmitter(map)?.GetComp<CompPower>().PowerNet;
            if (powerNet == null)
            {
                return false;
            }

            if (!tmpPowerNetPredicateResults.TryGetValue(powerNet, out var value))
            {
                value = predicate(powerNet);
                tmpPowerNetPredicateResults.Add(powerNet, value);
            }

            if (!value)
            {
                return false;
            }

            foundNetLocal = powerNet;
            closestTransmitterLocal = x;
            return true;
        }, rememberParents: true);
        tmpPowerNetPredicateResults.Clear();
        if (foundNetLocal != null)
        {
            foundNet = foundNetLocal;
            closestTransmitter = closestTransmitterLocal;
            return true;
        }

        foundNet = null;
        closestTransmitter = IntVec3.Invalid;
        return false;
    }

    private void SpawnTransmitters(List<IntVec3> cells, Map map, Faction faction)
    {
        for (var i = 0; i < cells.Count; i++)
        {
            if (cells[i].GetTransmitter(map) != null)
            {
                continue;
            }

            var thing = GenSpawn.Spawn(ThingDefOf.PowerConduit, cells[i], map);
            thing.SetFaction(faction);
        }
    }

    private void SpawnTransmitters(IntVec3 start, IntVec3 end, Map map, Faction faction)
    {
        var foundPath = false;
        map.floodFiller.FloodFill(start, x => EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
        {
            if (x != end)
            {
                return false;
            }

            foundPath = true;
            return true;
        }, rememberParents: true);
        if (!foundPath)
        {
            return;
        }

        map.floodFiller.ReconstructLastFloodFillPath(end, tmpTransmitterCells);
        SpawnTransmitters(tmpTransmitterCells, map, faction);
    }

    private bool TrySpawnPowerTransmittingBuildingNear(IntVec3 position, Map map, Faction faction, ThingDef thingDef,
        out Building newBuilding, Predicate<IntVec3> extraValidator = null)
    {
        var traverseParams = TraverseParms.For(TraverseMode.PassAllDestroyableThings);
        if (RCellFinder.TryFindRandomCellNearWith(position, delegate(IntVec3 x)
            {
                if (!x.Standable(map) || x.Roofed(map) || !EverPossibleToTransmitPowerAt(x, map))
                {
                    return false;
                }

                if (!map.reachability.CanReach(position, x, PathEndMode.OnCell, traverseParams))
                {
                    return false;
                }

                foreach (var item in GenAdj.OccupiedRect(x, Rot4.North, thingDef.size))
                {
                    if (!item.InBounds(map) || item.Roofed(map) || item.GetEdifice(map) != null ||
                        item.GetFirstItem(map) != null || item.GetTransmitter(map) != null)
                    {
                        return false;
                    }
                }

                return extraValidator == null || extraValidator(x);
            }, map, out var result, 8))
        {
            newBuilding = (Building)GenSpawn.Spawn(ThingMaker.MakeThing(thingDef), result, map, Rot4.North);
            newBuilding.SetFaction(faction);
            return true;
        }

        newBuilding = null;
        return false;
    }

    private bool TrySpawnPowerGeneratorNear(IntVec3 position, Map map, Faction faction, out Building newPowerGenerator)
    {
        if (!TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.SolarGenerator,
                out newPowerGenerator))
        {
            return false;
        }

        map.powerNetManager.UpdatePowerNetsAndConnections_First();
        newPowerGenerator.GetComp<CompPowerPlant>().UpdateDesiredPowerOutput();
        return true;
    }

    private bool TrySpawnBatteryNear(IntVec3 position, Map map, Faction faction, out Building newBattery)
    {
        Predicate<IntVec3> extraValidator = null;
        if (spawnRoofOverNewBatteries)
        {
            extraValidator = delegate(IntVec3 x)
            {
                foreach (var intVec3 in GenAdj.OccupiedRect(x, Rot4.North, ThingDefOf.Battery.size).ExpandedBy(3))
                {
                    if (!intVec3.InBounds(map))
                    {
                        continue;
                    }

                    var thingList = intVec3.GetThingList(map);
                    foreach (var thing in thingList)
                    {
                        if (thing.def.PlaceWorkers != null &&
                            thing.def.PlaceWorkers.Any(y => y is PlaceWorker_NotUnderRoof))
                        {
                            return false;
                        }
                    }
                }

                return true;
            };
        }

        if (!TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.Battery, out newBattery,
                extraValidator))
        {
            return false;
        }

        var randomInRange = newBatteriesInitialStoredEnergyPctRange.RandomInRange;
        newBattery.GetComp<CompPowerBattery>().SetStoredEnergyPct(randomInRange);
        if (spawnRoofOverNewBatteries)
        {
            SpawnRoofOver(newBattery);
        }

        return true;
    }

    private bool TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(Thing forThing, Map map)
    {
        if (!canSpawnPowerGenerators)
        {
            return false;
        }

        var position = forThing.Position;
        if (canSpawnBatteries)
        {
            var chance = forThing is not Building_Turret ? 0.1f : 1f;
            if (Rand.Chance(chance) &&
                TrySpawnBatteryNear(forThing.Position, map, forThing.Faction, out var newBattery))
            {
                SpawnTransmitters(forThing.Position, newBattery.Position, map, forThing.Faction);
                position = newBattery.Position;
            }
        }

        if (!TrySpawnPowerGeneratorNear(position, map, forThing.Faction, out var newPowerGenerator))
        {
            return false;
        }

        SpawnTransmitters(position, newPowerGenerator.Position, map, forThing.Faction);
        return true;
    }

    private bool EverPossibleToTransmitPowerAt(IntVec3 c, Map map)
    {
        return c.GetTransmitter(map) != null ||
               GenConstruct.CanBuildOnTerrain(ThingDefOf.PowerConduit, c, map, Rot4.North);
    }

    private void TryTurnOnImmediately(CompPowerTrader powerComp, Map map)
    {
        if (powerComp.PowerOn)
        {
            return;
        }

        map.powerNetManager.UpdatePowerNetsAndConnections_First();
        if (powerComp.PowerNet != null && powerComp.PowerNet.CurrentEnergyGainRate() > 1E-07f)
        {
            powerComp.PowerOn = true;
        }
    }

    private void SpawnRoofOver(Thing thing)
    {
        var cellRect = thing.OccupiedRect();
        var roofed = true;
        foreach (var intVec3 in cellRect)
        {
            if (intVec3.Roofed(thing.Map))
            {
                continue;
            }

            roofed = false;
            break;
        }

        if (roofed)
        {
            return;
        }

        var num = 0;
        var cellRect2 = cellRect.ExpandedBy(2);
        foreach (var intVec3 in cellRect2)
        {
            if (intVec3.InBounds(thing.Map) && intVec3.GetRoofHolderOrImpassable(thing.Map) != null)
            {
                num++;
            }
        }

        if (num < 2)
        {
            var stuff = Rand.Element(ThingDefOf.WoodLog, ThingDefOf.Steel);
            foreach (var corner in cellRect2.Corners)
            {
                if (!corner.InBounds(thing.Map) || !corner.Standable(thing.Map) ||
                    corner.GetFirstItem(thing.Map) != null || corner.GetFirstBuilding(thing.Map) != null ||
                    corner.GetFirstPawn(thing.Map) != null ||
                    GenAdj.CellsAdjacent8Way(new TargetInfo(corner, thing.Map))
                        .Any(x => !x.InBounds(thing.Map) || !x.Walkable(thing.Map)) ||
                    !corner.SupportsStructureType(thing.Map, ThingDefOf.Wall.terrainAffordanceNeeded))
                {
                    continue;
                }

                var thing2 = ThingMaker.MakeThing(ThingDefOf.Wall, stuff);
                GenSpawn.Spawn(thing2, corner, thing.Map);
                thing2.SetFaction(thing.Faction);
                num++;
            }
        }

        if (num <= 0)
        {
            return;
        }

        foreach (var intVec3 in cellRect2)
        {
            if (intVec3.InBounds(thing.Map) && !intVec3.Roofed(thing.Map))
            {
                thing.Map.roofGrid.SetRoof(intVec3, RoofDefOf.RoofConstructed);
            }
        }
    }
}