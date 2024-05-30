namespace RimWorld.BaseGen;

public class SymbolResolver_BasePart_Indoors_Leaf_BatteryRoom2 : SymbolResolver
{
    private const float MaxCoverage = 0.1f;

    public override bool CanResolve(ResolveParams rp)
    {
        if (!base.CanResolve(rp))
        {
            return false;
        }

        if (BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms)
        {
            return false;
        }

        if (BaseGen.globalSettings.basePart_barracksResolved < BaseGen.globalSettings.minBarracks)
        {
            return false;
        }

        if (BaseGen.globalSettings.basePart_worshippedTerminalsResolved <
            BaseGen.globalSettings.requiredWorshippedTerminalRooms &&
            SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp))
        {
            return false;
        }

        if (BaseGen.globalSettings.basePart_batteriesCoverage +
            (rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area) >= MaxCoverage)
        {
            return false;
        }

        if (rp.faction != null)
        {
            return (int)rp.faction.def.techLevel >= 4;
        }

        return true;
    }

    public override void Resolve(ResolveParams rp)
    {
        BaseGen.symbolStack.Push("batteryRoom2", rp);
        BaseGen.globalSettings.basePart_batteriesCoverage += rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
    }
}