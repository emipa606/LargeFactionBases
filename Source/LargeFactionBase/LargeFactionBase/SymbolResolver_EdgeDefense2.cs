using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace LargeFactionBase;

public class SymbolResolver_EdgeDefense2 : SymbolResolver
{
    private const int DefaultCellsPerTurret = 20;

    private const int DefaultCellsPerMortar = 200;

    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction();
        var edgeDefenseGuardsCount = rp.edgeDefenseGuardsCount;
        var num = edgeDefenseGuardsCount ?? 0;
        int width;
        if (rp.edgeDefenseWidth.HasValue)
        {
            width = rp.edgeDefenseWidth.Value;
        }
        else if (rp.edgeDefenseMortarsCount is > 0)
        {
            width = 4;
        }
        else
        {
            width = !Rand.Bool ? 4 : 2;
        }

        width = Mathf.Clamp(width, 1, Mathf.Min(rp.rect.Width, rp.rect.Height) / 2);
        int num2;
        int num3;
        bool flag;
        bool flag2;
        bool flag3;
        switch (width)
        {
            case 1:
            {
                var edgeDefenseTurretsCount2 = rp.edgeDefenseTurretsCount;
                num2 = edgeDefenseTurretsCount2 ?? 0;
                num3 = 0;
                flag = false;
                flag2 = true;
                flag3 = true;
                break;
            }
            case 2:
            {
                var edgeDefenseTurretsCount4 = rp.edgeDefenseTurretsCount;
                num2 = edgeDefenseTurretsCount4 ?? rp.rect.EdgeCellsCount / 30;
                num3 = 0;
                flag = false;
                flag2 = false;
                flag3 = true;
                break;
            }
            case 3:
            {
                var edgeDefenseTurretsCount3 = rp.edgeDefenseTurretsCount;
                num2 = edgeDefenseTurretsCount3 ?? rp.rect.EdgeCellsCount / 30;
                var edgeDefenseMortarsCount2 = rp.edgeDefenseMortarsCount;
                num3 = edgeDefenseMortarsCount2 ?? rp.rect.EdgeCellsCount / 75;
                flag = num3 == 0;
                flag2 = false;
                flag3 = true;
                break;
            }
            default:
            {
                var edgeDefenseTurretsCount = rp.edgeDefenseTurretsCount;
                num2 = edgeDefenseTurretsCount ?? rp.rect.EdgeCellsCount / 30;
                var edgeDefenseMortarsCount = rp.edgeDefenseMortarsCount;
                num3 = edgeDefenseMortarsCount ?? rp.rect.EdgeCellsCount / 75;
                flag = true;
                flag2 = false;
                flag3 = false;
                break;
            }
        }

        if (faction != null && (int)faction.def.techLevel < 4)
        {
            num2 = 0;
            num3 = 0;
        }

        if (num > 0)
        {
            var singlePawnLord = rp.singlePawnLord ??
                                 LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell, 0),
                                     map);
            for (var i = 0; i < num; i++)
            {
                var value = new PawnGenerationRequest(faction.RandomPawnKind(), faction,
                    PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, 1f, true, true, true, true,
                    false, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null,
                    null, null, null, null, false, false, false, true, null, null, null, null, null, 1f);
                var resolveParams = rp;
                resolveParams.faction = faction;
                resolveParams.singlePawnLord = singlePawnLord;
                resolveParams.singlePawnGenerationRequest = value;
                resolveParams.singlePawnSpawnCellExtraPredicate ??= delegate(IntVec3 x)
                {
                    var cellRect = rp.rect;
                    for (var m = 0; m < width; m++)
                    {
                        if (cellRect.IsOnEdge(x))
                        {
                            return true;
                        }

                        cellRect = cellRect.ContractedBy(1);
                    }

                    return true;
                };
                BaseGen.symbolStack.Push("pawn", resolveParams);
            }
        }

        var rect = rp.rect;
        for (var j = 0; j < width; j++)
        {
            if (j % 2 == 0)
            {
                var resolveParams2 = rp;
                resolveParams2.faction = faction;
                resolveParams2.rect = rect;
                BaseGen.symbolStack.Push("edgeSandbags", resolveParams2);
                if (!flag)
                {
                    break;
                }
            }

            rect = rect.ContractedBy(1);
        }

        var rect2 = !flag3 ? rp.rect.ContractedBy(1) : rp.rect;
        for (var k = 0; k < num3; k++)
        {
            var resolveParams3 = rp;
            resolveParams3.faction = faction;
            resolveParams3.rect = rect2;
            BaseGen.symbolStack.Push("edgeMannedMortar", resolveParams3);
        }

        var rect3 = !flag2 ? rp.rect.ContractedBy(1) : rp.rect;
        for (var l = 0; l < num2 * 2; l++)
        {
            var resolveParams4 = rp;
            resolveParams4.faction = faction;
            resolveParams4.singleThingDef = ThingDefOf.Turret_MiniTurret;
            if (Rand.Chance(0.1f))
            {
                resolveParams4.singleThingDef = Large_DefOf.Turret_Sniper;
            }

            if (Rand.Chance(0.2f))
            {
                resolveParams4.singleThingDef = Large_DefOf.Turret_Autocannon;
            }

            resolveParams4.rect = rect3;
            var edgeThingAvoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings;
            resolveParams4.edgeThingAvoidOtherEdgeThings =
                !edgeThingAvoidOtherEdgeThings.HasValue || edgeThingAvoidOtherEdgeThings.Value;
            BaseGen.symbolStack.Push("edgeThing", resolveParams4);
        }
    }
}