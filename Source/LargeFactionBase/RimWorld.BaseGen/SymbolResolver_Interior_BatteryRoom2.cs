using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Interior_BatteryRoom2 : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        BaseGen.symbolStack.Push("indoorLighting", rp);
        BaseGen.symbolStack.Push("chargeBatteries2", rp);
        var resolveParams = rp;
        resolveParams.singleThingDef = ThingDefOf.Battery;
        resolveParams.thingRot = !Rand.Bool ? Rot4.East : Rot4.North;
        var fillWithThingsPadding = rp.fillWithThingsPadding;
        resolveParams.fillWithThingsPadding = fillWithThingsPadding ?? 1;
        BaseGen.symbolStack.Push("fillWithThings", resolveParams);
    }
}