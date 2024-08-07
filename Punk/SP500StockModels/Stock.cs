
namespace Punk.SP500StockModels;

public partial class Stock
{
    public string Ticker { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? Country { get; set; }
    
    public virtual ICollection<SectorComponent>? Components { get; }
}


