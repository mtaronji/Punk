using System;
using System.Collections.Generic;

namespace Punk.FREDDataModels;

public partial class Series
{
    public string SeriesId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Frequency { get; set; } = null!;

    public string Units { get; set; } = null!;

    public string SeasonalAdj { get; set; } = null!;

    public int Popularity { get; set; }

    public string Notes { get; set; } = null!;
}
