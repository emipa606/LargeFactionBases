using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Corpse : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var num = rp.hivesCount ?? Rand.Range(1, 3);
        for (var i = 0; i < num; i++)
        {
            var pawnKindDef = !(Rand.Value > 0.7f) ? PawnKindDefOf.Slave :
                Rand.Value > 0.6f ? PawnKindDefOf.AncientSoldier : PawnKindDefOf.WellEquippedTraveler;
            var faction = rp.faction;
            var request = new PawnGenerationRequest(pawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, false,
                false, false, false, false, 1f, true, true, true, true, false, false, false, false, false, 0f, 0f, null,
                1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true,
                null, null, null, null, null, 1f);
            var pawn = PawnGenerator.GeneratePawn(request);
            var map = BaseGen.globalSettings?.map ?? Find.CurrentMap;
            CellFinderLoose.TryGetRandomCellWith(
                x => x.IsValid && rp.rect.Contains(x) && x.GetEdifice(map) == null && x.GetFirstItem(map) == null, map,
                100000, out var result);
            GenSpawn.Spawn(pawn, result, map);
            pawn.Kill(null);
            Corpse corpse;
            CompRottable compRottable;
            if ((corpse = pawn.Corpse) == null || (compRottable = corpse.TryGetComp<CompRottable>()) == null)
            {
                continue;
            }

            var num2 = Rand.Range(0, 45);
            var num3 = Rand.Range(0.75f, 1.25f);
            corpse.Age += 60000 * num2;
            compRottable.RotProgress += Rand.Range(8000, 12000) * num2 * num3;
        }
    }

    private bool IsWallOrRock(Building b)
    {
        return b != null && (b.def == ThingDefOf.Wall || b.def.building.isNaturalRock);
    }
}