using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_ChargeBatteries2 : SymbolResolver
{
    private static readonly List<CompPowerBattery> batteries = [];

    public override void Resolve(ResolveParams rp)
    {
        var map = BaseGen.globalSettings.map;
        batteries.Clear();
        foreach (var intVec3 in rp.rect)
        {
            var thingList = intVec3.GetThingList(map);
            foreach (var thing in thingList)
            {
                var compPowerBattery = thing.TryGetComp<CompPowerBattery>();
                if (compPowerBattery != null && !batteries.Contains(compPowerBattery))
                {
                    batteries.Add(compPowerBattery);
                }
            }
        }

        foreach (var battery in batteries)
        {
            var num = Rand.Range(0.6f, 0.95f);
            battery.SetStoredEnergyPct(Mathf.Min(battery.StoredEnergyPct + num, 1f));
        }

        batteries.Clear();
    }
}