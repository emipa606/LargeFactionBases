using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_MedicalBed : SymbolResolver
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
                building_Bed.Medical = true;
            }
        };
        BaseGen.symbolStack.Push("medBed", resolveParams);
    }
}