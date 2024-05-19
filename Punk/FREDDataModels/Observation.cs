
namespace Punk.FREDDataModels;

public partial class Observation
{
    public string SeriesId { get; set; } = null!;

    public DateOnly Date { get; set; }

    public double ObservedValue { get; set; }

    public virtual Series Series { get; set; } = null!;
}
