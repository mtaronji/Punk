

namespace Punk.Plotly
{
    public class ColorBar
    {
        public string bgcolor { get; set; }
        public string bordercolor { get; set; }
        public int borderwidth { get; set; }
        public string dtick { get; set; }
        public string exponentformat { get; set; }
        public int len { get; set; }
        public string lenmode { get; set; }
        public int minexponent { get; set; }
        public int nticks { get; set; } //integer greater than or equal to 0
        public string orientation { get; set; }
        public string outlinecolor { get; set; }
        public int outlinewidth { get; set; } //pixels
        public bool separatethousands { get; set; } //If "true", even 4-digit integers are separated
        public string showexponent { get; set; } //Type: enumerated , one of ( "all" | "first" | "last" | "none" )
        public bool showticklabels { get; set; }
        public bool showtickprefix { get; set; } //Type: enumerated , one of( "all" | "first" | "last" | "none" )
        public float thickness { get; set; } //Default: 30 Sets the thickness of the color bar This measure excludes the size of the padding, ticks and labels.
        public string thicknessmode { get; set; } //Type: enumerated , one of ( "fraction" | "pixels" ) Default: "pixels" Determines whether this color bar's thickness (i.e. the measure in the constant color direction) is set in units of plot "fraction" or in "pixels". Use `thickness` to set the value.
        public dynamic tick0 { get; set; } //Sets the placement of the first tick on this axis. Use with `dtick`.
                                           //If the axis `type` is "log", then you must take the log of your starting tick (e.g. to set the starting tick to 100, set the `tick0` to 2)
                                           //except when `dtick`="L<f>" (see `dtick` for more info). If the axis `type` is "date", it should be a date string, like date data.
                                           //If the axis `type` is "category", it should be a number, using the scale where each category is assigned a serial number from zero in the order it appears.
        public float tickangle { get; set; }
        public string tickcolor { get; set; }
        public Font font { get; set; }
        public string tickformat { get; set; }
        public TickFormatStops[] tickformatstops { get; set; }
        public string ticklabeloverflow { get; set; }
        public string ticklabelposition { get; set; }
        public int ticklabelstep { get; set; }
        public string tickmode { get; set; }
        public string tickprefix { get; set; }
        public string ticks { get; set; } //"outside" | "inside" | ""
        public string ticksuffix { get; set; }
        public dynamic[] ticktext { get; set; } //Type: data array
                                                //Sets the text displayed at the ticks position via `tickvals`. Only has an effect if `tickmode` is set to "array". Used with `tickvals`.
        public dynamic[] tickvals { get; set; }
        public float tickwidth { get; set; }
        public float x { get; set; }
        public string xanchor { get; set; } //ColorbaranchorTypes
        public float xpad { get; set; }
        public string xref { get; set; } //"container" | "paper"
        public float y { get; set; } //Sets the y position with respect to `yref` of the
        public string yanchor { get; set; } //"top" | "middle" | "bottom"
        public float ypad { get; set; }
        public string yref { get; set; }// "container","paper"
    }
}
