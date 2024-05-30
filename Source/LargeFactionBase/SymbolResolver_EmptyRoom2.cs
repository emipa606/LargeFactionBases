using RimWorld.BaseGen;
using Verse;

public class SymbolResolver_EmptyRoom2 : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction);
        var floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef, false, rp.faction);
        if (!rp.noRoof.HasValue || !rp.noRoof.Value)
        {
            BaseGen.symbolStack.Push("roof", rp);
        }

        var resolveParams = rp;
        resolveParams.wallStuff = thingDef;
        BaseGen.symbolStack.Push("edgeWalls", resolveParams);
        var resolveParams2 = rp;
        resolveParams2.floorDef = floorDef;
        BaseGen.symbolStack.Push("floor", resolveParams2);
        for (var i = 0; i < Rand.Range(24, 48); i++)
        {
            BaseGen.symbolStack.Push("prisonBile", rp);
        }

        for (var j = 0; j < Rand.Range(4, 8); j++)
        {
            BaseGen.symbolStack.Push("prisonFilth", rp);
        }

        if (rp.addRoomCenterToRootsToUnfog.HasValue && rp.addRoomCenterToRootsToUnfog.Value &&
            Current.ProgramState == ProgramState.MapInitializing)
        {
            MapGenerator.rootsToUnfog.Add(rp.rect.CenterCell);
        }
    }
}