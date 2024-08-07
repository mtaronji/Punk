namespace Punk.SP500StockModels;

public partial class SectorComponent
{
    public virtual SectorSymbol SectorSymbol { get; } = null!;
    public string SectorSymbolId { get; } = null!;
    public virtual Stock Stock { get; } = null!;
    public string StockTicker { get; } = null!;
}
