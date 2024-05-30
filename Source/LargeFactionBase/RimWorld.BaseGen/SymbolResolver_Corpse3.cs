using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Corpse3 : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var num = rp.hivesCount ?? Rand.Range(1, 3);
        for (var i = 0; i < num; i++)
        {
            var slave = PawnKindDefOf.Slave;
            var faction = rp.faction;
            var request = new PawnGenerationRequest(slave, faction, PawnGenerationContext.NonPlayer, -1, false, false,
                false, false, false, 1f, true, true, true, true, false, false, false, false, false, 0f, 0f, null, 1f,
                null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null,
                null, null, null, null, 1f);
            var pawn = PawnGenerator.GeneratePawn(request);
            var map = BaseGen.globalSettings?.map ?? Find.CurrentMap;
            CellFinderLoose.TryGetRandomCellWith(
                x => x.IsValid && rp.rect.Contains(x) && x.GetEdifice(map) == null && x.GetFirstItem(map) == null, map,
                100000, out var result);
            GenSpawn.Spawn(pawn, result, map);
            if (Rand.Value > 0.25f)
            {
                HealthUtility.DamageUntilDowned(pawn, false);
            }
            else
            {
                HealthUtility.DamageLegsUntilIncapableOfMoving(pawn);
            }
        }
    }
}