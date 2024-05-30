using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_PrisonDefense : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var resolveParams = rp;
        var prevPostThingSpawn = resolveParams.postThingSpawn;
        resolveParams.postThingSpawn = delegate(Thing x)
        {
            prevPostThingSpawn?.Invoke(x);

            if (x is not Building_Bed building_Bed)
            {
                return;
            }

            building_Bed.ForPrisoners = true;
            building_Bed.Medical = true;
        };
        BaseGen.symbolStack.Push("miniT", resolveParams);
    }
}