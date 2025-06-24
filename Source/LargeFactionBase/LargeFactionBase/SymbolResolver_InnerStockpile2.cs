using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace LargeFactionBase;

public class SymbolResolver_InnerStockpile2 : SymbolResolver
{
    private const int DefaultSize = 3;

    public override void Resolve(ResolveParams rp)
    {
        var innerStockpileSize = rp.innerStockpileSize;
        CellRect rect;
        if (innerStockpileSize.HasValue)
        {
            if (!TryFindPerfectPlaceThenBest(rp.rect, rp.innerStockpileSize.Value, out rect))
            {
                return;
            }
        }
        else if (rp.stockpileConcreteContents != null)
        {
            var num = Mathf.CeilToInt(Mathf.Sqrt(rp.stockpileConcreteContents.Count));
            if (!TryFindRandomInnerRect(rp.rect, num, out rect, num * num, out _))
            {
                rect = rp.rect;
            }
        }
        else if (!TryFindPerfectPlaceThenBest(rp.rect, 3, out rect))
        {
            return;
        }

        var resolveParams = rp;
        resolveParams.rect = rect;
        BaseGen.symbolStack.Push("stockpile2", resolveParams);
    }

    private bool TryFindPerfectPlaceThenBest(CellRect outerRect, int size, out CellRect rect)
    {
        if (TryFindRandomInnerRect(outerRect, size, out rect, size * size, out var maxValidCellsFound))
        {
            return true;
        }

        return maxValidCellsFound != 0 &&
               TryFindRandomInnerRect(outerRect, size, out rect, maxValidCellsFound, out _);
    }

    private bool TryFindRandomInnerRect(CellRect outerRect, int size, out CellRect rect, int minValidCells,
        out int maxValidCellsFound)
    {
        var map = BaseGen.globalSettings.map;
        size = Mathf.Min(size, Mathf.Min(outerRect.Width, outerRect.Height));
        var maxValidCellsFoundLocal = 0;
        var result = outerRect.TryFindRandomInnerRect(new IntVec2(size, size), out rect, delegate(CellRect x)
        {
            var num = 0;
            foreach (var intVec3 in x)
            {
                if (intVec3.Standable(map) && intVec3.GetFirstItem(map) == null &&
                    intVec3.GetFirstBuilding(map) == null)
                {
                    num++;
                }
            }

            maxValidCellsFoundLocal = Mathf.Max(maxValidCellsFoundLocal, num);
            return num >= minValidCells;
        });
        maxValidCellsFound = maxValidCellsFoundLocal;
        return result;
    }
}