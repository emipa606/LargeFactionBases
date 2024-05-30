namespace RimWorld.BaseGen;

public class SymbolResolver_BasePart_Indoors_Leaf_Storage3 : SymbolResolver
{
    public override bool CanResolve(ResolveParams rp)
    {
        if (!base.CanResolve(rp))
        {
            return false;
        }

        if (rp.faction == null || rp.faction != Faction.OfEmpire)
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

        return BaseGen.globalSettings.basePart_worshippedTerminalsResolved >=
               BaseGen.globalSettings.requiredWorshippedTerminalRooms ||
               !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp);
    }

    public override void Resolve(ResolveParams rp)
    {
        BaseGen.symbolStack.Push("storage3", rp);
    }
}