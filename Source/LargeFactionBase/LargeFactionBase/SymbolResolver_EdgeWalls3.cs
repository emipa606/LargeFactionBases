using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace LargeFactionBase;

public class SymbolResolver_EdgeWalls3 : SymbolResolver
{
    private static readonly IntRange LineLengthRange = new(20, 30);

    private static readonly IntRange GapLengthRange = new(0, 1);

    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var num = 0;
        var num2 = 0;
        var num3 = -1;
        if (rp.rect.EdgeCellsCount < (LineLengthRange.max + GapLengthRange.max) * 2)
        {
            num = rp.rect.EdgeCellsCount;
        }
        else if (Rand.Bool)
        {
            num = LineLengthRange.RandomInRange;
        }
        else
        {
            num2 = GapLengthRange.RandomInRange;
        }

        foreach (var edgeCell in rp.rect.EdgeCells)
        {
            num3++;
            if (num2 > 0)
            {
                num2--;
                if (num2 != 0)
                {
                    continue;
                }

                if (num3 == rp.rect.EdgeCellsCount - 2)
                {
                    num2 = 1;
                }
                else
                {
                    num = LineLengthRange.RandomInRange;
                }
            }
            else
            {
                if (!edgeCell.Standable(map) || edgeCell.Roofed(map) ||
                    !edgeCell.SupportsStructureType(map, ThingDefOf.Sandbags.terrainAffordanceNeeded) ||
                    GenSpawn.WouldWipeAnythingWith(edgeCell, Rot4.North, ThingDefOf.Sandbags, map,
                        x => x.def.category == ThingCategory.Building || x.def.category == ThingCategory.Item))
                {
                    continue;
                }

                if (num > 0)
                {
                    num--;
                    if (num == 0)
                    {
                        num2 = GapLengthRange.RandomInRange;
                    }
                }

                var thing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.WoodLog);
                thing.SetFaction(rp.faction);
                GenSpawn.Spawn(thing, edgeCell, map);
            }
        }
    }
}