using RimWorld;
using RimWorld.BaseGen;

namespace LargeFactionBase;

public class SymbolResolver_Interior_Storage2 : SymbolResolver
{
    private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;

    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        BaseGen.symbolStack.Push("stockpile2", rp);
        if (!(map.mapTemperature.OutdoorTemp > SpawnPassiveCoolerIfTemperatureAbove))
        {
            return;
        }

        var resolveParams = rp;
        resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
        BaseGen.symbolStack.Push("edgeThing", resolveParams);
    }
}