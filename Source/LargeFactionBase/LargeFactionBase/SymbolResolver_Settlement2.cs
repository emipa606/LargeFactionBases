using RimWorld;
using RimWorld.BaseGen;
using Verse;
using Verse.AI.Group;

namespace LargeFactionBase;

public class SymbolResolver_Settlement2 : SymbolResolver
{
    public static readonly FloatRange DefaultPawnsPoints = new(1150f, 1600f);

    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction();
        var num = 0;
        var edgeDefenseWidth = rp.edgeDefenseWidth;
        if (edgeDefenseWidth.HasValue)
        {
            num = rp.edgeDefenseWidth.Value;
        }
        else if (rp.rect is { Width: >= 20, Height: >= 20 } && ((int)faction.def.techLevel >= 4 || Rand.Bool))
        {
            num = !Rand.Bool ? 4 : 2;
        }

        var num2 = rp.rect.Area / 144f * 0.17f;
        BaseGen.globalSettings.minEmptyNodes = num2 >= 1f ? GenMath.RoundRandom(num2) : 0;
        var singlePawnLord = rp.singlePawnLord ??
                             LordMaker.MakeNewLord(faction, new LordJob_DefendBase2(faction, rp.rect.CenterCell), map);
        var singlePawnLord2 = rp.singlePawnLord ??
                              LordMaker.MakeNewLord(faction, new LordJob_DefendBase3(faction, rp.rect.CenterCell), map);
        var traverseParms = TraverseParms.For(TraverseMode.PassDoors);
        if (Rand.Value > 0.5f)
        {
            var resolveParams = rp;
            resolveParams.rect = rp.rect.ExpandedBy(12);
            resolveParams.faction = faction;
            resolveParams.thingSetMakerDef = LargeFactionBase_ThingSetMakerDefOf.MapGen_DefaultStockpile3;
            resolveParams.singlePawnLord = singlePawnLord2;
            resolveParams.pawnGroupKindDef = rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Settlement;
            resolveParams.singlePawnSpawnCellExtraPredicate = rp.singlePawnSpawnCellExtraPredicate ??
                                                              (x => map.reachability.CanReachMapEdge(x, traverseParms));
            if (resolveParams.pawnGroupMakerParams == null)
            {
                resolveParams.pawnGroupMakerParams = new PawnGroupMakerParms();
                resolveParams.pawnGroupMakerParams.tile = map.Tile;
                resolveParams.pawnGroupMakerParams.faction = faction;
                var settlementPawnGroupPoints = rp.settlementPawnGroupPoints;
                resolveParams.pawnGroupMakerParams.points = (settlementPawnGroupPoints ??
                                                             SymbolResolver_Settlement.DefaultPawnsPoints
                                                                 .RandomInRange) / 2f;
                resolveParams.pawnGroupMakerParams.inhabitants = true;
                resolveParams.pawnGroupMakerParams.seed = rp.settlementPawnGroupSeed;
            }

            BaseGen.symbolStack.Push("pawnGroup", resolveParams);
            var resolveParams2 = rp;
            resolveParams2.rect = rp.rect.ExpandedBy(12);
            resolveParams2.faction = faction;
            resolveParams2.thingSetMakerDef = LargeFactionBase_ThingSetMakerDefOf.MapGen_DefaultStockpile3;
            resolveParams2.singlePawnLord = singlePawnLord;
            resolveParams2.pawnGroupKindDef = rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Combat;
            resolveParams2.singlePawnSpawnCellExtraPredicate = rp.singlePawnSpawnCellExtraPredicate ??
                                                               (x => map.reachability.CanReachMapEdge(x,
                                                                   traverseParms));
            if (resolveParams2.pawnGroupMakerParams == null)
            {
                resolveParams2.pawnGroupMakerParams = new PawnGroupMakerParms();
                resolveParams2.pawnGroupMakerParams.tile = map.Tile;
                resolveParams2.pawnGroupMakerParams.faction = faction;
                var settlementPawnGroupPoints2 = rp.settlementPawnGroupPoints;
                resolveParams2.pawnGroupMakerParams.points = (settlementPawnGroupPoints2 ??
                                                              SymbolResolver_Settlement.DefaultPawnsPoints
                                                                  .RandomInRange) / 2f;
                resolveParams2.pawnGroupMakerParams.inhabitants = true;
                resolveParams2.pawnGroupMakerParams.seed = rp.settlementPawnGroupSeed;
            }

            BaseGen.symbolStack.Push("pawnGroup", resolveParams2);
        }
        else
        {
            var resolveParams3 = rp;
            resolveParams3.rect = rp.rect.ExpandedBy(12);
            resolveParams3.faction = faction;
            resolveParams3.thingSetMakerDef = LargeFactionBase_ThingSetMakerDefOf.MapGen_DefaultStockpile3;
            resolveParams3.singlePawnLord = singlePawnLord;
            resolveParams3.pawnGroupKindDef = rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Settlement;
            resolveParams3.singlePawnSpawnCellExtraPredicate = rp.singlePawnSpawnCellExtraPredicate ??
                                                               (x => map.reachability.CanReachMapEdge(x,
                                                                   traverseParms));
            if (resolveParams3.pawnGroupMakerParams == null)
            {
                resolveParams3.pawnGroupMakerParams = new PawnGroupMakerParms();
                resolveParams3.pawnGroupMakerParams.tile = map.Tile;
                resolveParams3.pawnGroupMakerParams.faction = faction;
                var settlementPawnGroupPoints3 = rp.settlementPawnGroupPoints;
                resolveParams3.pawnGroupMakerParams.points = settlementPawnGroupPoints3 ??
                                                             SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange;
                resolveParams3.pawnGroupMakerParams.inhabitants = true;
                resolveParams3.pawnGroupMakerParams.seed = rp.settlementPawnGroupSeed;
            }

            BaseGen.symbolStack.Push("pawnGroup", resolveParams3);
        }

        BaseGen.symbolStack.Push("outdoorLighting", rp);
        if ((int)faction.def.techLevel >= 4)
        {
            var num3 = Rand.Chance(0.75f) ? GenMath.RoundRandom(rp.rect.Area / 400f) : 0;
            for (var i = 0; i < num3; i++)
            {
                var resolveParams4 = rp;
                resolveParams4.faction = faction;
                BaseGen.symbolStack.Push("firefoamPopper", resolveParams4);
            }
        }

        if (num > 0)
        {
            var resolveParams5 = rp;
            resolveParams5.faction = faction;
            resolveParams5.edgeDefenseWidth = num;
            BaseGen.symbolStack.Push("edgeDefense2", resolveParams5);
        }

        var resolveParams6 = rp;
        resolveParams6.rect = rp.rect.ContractedBy(num);
        resolveParams6.faction = faction;
        BaseGen.symbolStack.Push("ensureCanReachMapEdge", resolveParams6);
        var resolveParams7 = rp;
        resolveParams7.rect = rp.rect.ContractedBy(num);
        resolveParams7.faction = faction;
        BaseGen.symbolStack.Push("basePart_outdoors", resolveParams7);
        var resolveParams8 = rp;
        resolveParams8.floorDef = TerrainDefOf.Bridge;
        var floorOnlyIfTerrainSupports = rp.floorOnlyIfTerrainSupports;
        resolveParams8.floorOnlyIfTerrainSupports =
            !floorOnlyIfTerrainSupports.HasValue || floorOnlyIfTerrainSupports.Value;
        BaseGen.symbolStack.Push("floor", resolveParams8);
    }
}