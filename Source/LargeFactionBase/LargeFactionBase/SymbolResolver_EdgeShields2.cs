using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace LargeFactionBase;

public class SymbolResolver_EdgeShields2 : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        var rect = rp.rect;
        if (rp.wallStuff == null)
        {
            rp.wallStuff = BaseGenUtility.RandomCheapWallStuff(Faction.OfPlayer);
        }

        var num = 1;
        foreach (var edgeCell in rect.EdgeCells)
        {
            var wall = ThingDefOf.Wall;
            var newThing = ThingMaker.MakeThing(wall, rp.wallStuff);
            if (num % 3 == 0)
            {
                wall = ThingDefOf.Sandbags;
                newThing = ThingMaker.MakeThing(wall);
            }

            num++;
            GenSpawn.Spawn(newThing, edgeCell, map);
        }
    }
}