namespace RimWorld.BaseGen;

public class SymbolResolver_BasePart_Indoors_Leaf_PrisonCell4 : SymbolResolver
{
    private const float MaxCoverage = 0.08f;

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

        return BaseGen.globalSettings.basePart_worshippedTerminalsResolved >=
               BaseGen.globalSettings.requiredWorshippedTerminalRooms ||
               !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp);
    }

    public override void Resolve(ResolveParams rp)
    {
        BaseGen.symbolStack.Push("prisonCell4", rp);
    }
}