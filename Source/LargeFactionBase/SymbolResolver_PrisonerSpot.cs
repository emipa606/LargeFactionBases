using RimWorld;
using RimWorld.BaseGen;
using Verse;

public class SymbolResolver_PrisonerSpot : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        var resolveParams = rp;
        var prevPostThingSpawn = resolveParams.postThingSpawn;
        resolveParams.postThingSpawn = delegate(Thing x)
        {
            prevPostThingSpawn?.Invoke(x);

            if (x is Building_Bed building_Bed)
            {
                building_Bed.ForPrisoners = true;
            }
        };
        BaseGen.symbolStack.Push("sleepSpot", resolveParams);
    }
}