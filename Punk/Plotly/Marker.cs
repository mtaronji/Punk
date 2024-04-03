

namespace Punk.Plotly
{
    public class Marker
    {
        public float angle { get; set; }
        public string angleref { get; set; }            //"previous" "up"
                                                        //Default: "up"
                                                        //Sets the reference for marker angle.With "previous", angle 0 points along the line from the previous point to this one.
                                                        //With "up", angle 0 points toward the top of the screen.

        public bool autocolorscale { get; set; }        //default true

        public bool cauto { get; set; }                 //default true
                                                        //Determines whether or not the color domain is computed with respect to the input data (here in `marker.color`) or the bounds set in `marker.cmin`
                                                        //and `marker.cmax` Has an effect only if in `marker.color` is set to a numerical array. Defaults to `false` when `marker.cmin` and `marker.cmax`
                                                        //are set by the user.
        public float cmax { get; set; }                 //Sets the upper bound of the color domain. Has an effect only if in `marker.color` is set to a numerical array.
                                                        //Value should have the same units as in `marker.color` and if set, `marker.cmin` must be set as well.
        public float cmid { get; set; }                 //Sets the mid-point of the color domain by scaling `marker.cmin` and/or `marker.cmax` to be equidistant to this point.
                                                        //Has an effect only if in `marker.color` is set to a numerical array. Value should have the same units as in `marker.color`.
                                                        //Has no effect when `marker.cauto` is `false`.
        public float cmin { get; set; }                 //Sets the lower bound of the color domain. Has an effect only if in `marker.color` is set to a numerical array.
                                                        //Value should have the same units as in `marker.color` and if set, `marker.cmax` must be set as well.
        public string[] color { get; set; }             //Type: color or array of colors  Sets the marker color.It accepts either a specific color or an array of numbers that
                                                        //are mapped to the colorscale relative to the max and min values of the array or relative to `marker.cmin` and `marker.cmax` if set.
        public string coloraxis { get; set; }           //Sets a reference to a shared color axis. References to these shared color axes are "coloraxis", "coloraxis2", "coloraxis3", etc.
                                                        //Settings for these shared color axes are set in the layout, under `layout.coloraxis`, `layout.coloraxis2`, etc.
                                                        //Note that multiple color scales can be linked to the same color axis.

        public ColorBar colorbar { get; set; }
        public dynamic colorscale { get; set; }
        public MarkerLine line { get; set; }
        public float maxdisplayed { get; set; }
        public dynamic opacity { get; set; }
        public bool reversescale { get; set; }
        public bool showscale { get; set; }
        public dynamic size { get; set; }               //Type: number or array of numbers greater than or equal to 0
        public float sizemin { get; set; }
        public string sizemode { get; set; }            //Type: enumerated , one of ( "diameter" | "area" )
        public float sizeref { get; set; }              //Has an effect only if `marker.size` is set to a numerical array. Sets the scale factor used to determine the rendered size of marker points.

        public dynamic standoff { get; set; }
        public string symbol { get; set; }

    }

    public class MarkerLine
    {
        public bool autocolorscale { get; set; }

        public bool cauto { get; set; }

        public float cmax { get; set; }
        public float cmid { get; set; }
        public float cmin { get; set; }
        public string coloraxis { get; set; } //Sets a reference to a shared color axis. References to these shared color axes are "coloraxis", "coloraxis2", "coloraxis3", etc.
        public dynamic color { get; set; } //Sets the marker.line color. It accepts either a specific color or an array of numbers that are mapped to the colorscale relative
                                           //to the max and min values of the array or relative to `marker.line.cmin` and `marker.line.cmax` if set.

        public dynamic colorscale { get; set; }  //Sets the colorscale. Has an effect only if in `marker.line.color` is set to a numerical array
        public bool reversescale { get; set; }
        public dynamic width { get; set; }
    }
}
