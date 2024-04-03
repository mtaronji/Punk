
using System.Globalization;


namespace Punk.Plotly
{
    public abstract class Trace3d<T1, T2,T3>
    {
        public List<T1> x { get; set; }
        public List<T2> y { get; set; }
        public List<T3> z { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public bool visible { get; set; }
        public string legend { get; set; }                      //Default: legend
                                                                //Sets the reference to a legend to show this trace in. References to these legends are "legend", "legend2", "legend3", etc.
                                                                //Settings for these legends are set in the layout, under `layout.legend`, `layout.legend2`, etc.
        public int legendrank { get; set; }
        public int legendwidth { get; set; }
        public float opacity { get; set; }                      //Type: number between or equal to 0 and 1
                                                                //Default: 1 -> Sets the opacity of the trace.
        public float dx { get; set; }
        public float x0 { get; set; }

        public float dy { get; set; }
        public float y0 { get; set; }
        public string xaxis { get; set; }                       //default "x"
        public string yaxis { get; set; }                       //default "y"
        public dynamic xperiod { get; set; }                    //Type: number or categorical coordinate string
                                                                //Default: 0
                                                                //Only relevant when the axis `type` is "date". Sets the period positioning in milliseconds or "M<n>" on the x axis.
                                                                //Special values in the form of "M<n>" could be used to declare the number of months.In this case `n` must be a positive integer.

        public string xperiodalignment { get; set; }            //Type: enumerated , one of ( "start" | "middle" | "end" )
                                                                //Default: "middle"
                                                                //Only relevant when the axis `type` is "date". Sets the alignment of data points on the x axis.
        public dynamic xperiod0 { get; set; }                   //Only relevant when the axis `type` is "date". Sets the base for period positioning in milliseconds or date string on the x0 axis.
                                                                //When `x0period` is round number of weeks, the `x0period0` by default would be on a Sunday i.e. 2000-01-02, otherwise it would be at 2000-01-01.


        public dynamic yperiod { get; set; }                    //Type: number or categorical coordinate string
                                                                //Default: 0
                                                                //Only relevant when the axis `type` is "date". Sets the period positioning in milliseconds or "M<n>" on the x axis.
                                                                //Special values in the form of "M<n>" could be used to declare the number of months.In this case `n` must be a positive integer.

        public string yperiodalignment { get; set; }            //Type: enumerated , one of ( "start" | "middle" | "end" )
                                                                //Default: "middle"
                                                                //Only relevant when the axis `type` is "date". Sets the alignment of data points on the x axis.
        public dynamic yperiod0 { get; set; }                   //Only relevant when the axis `type` is "date". Sets the base for period positioning in milliseconds or date string on the x0 axis.
                                                                //When `x0period` is round number of weeks, the `x0period0` by default would be on a Sunday i.e. 2000-01-02, otherwise it would be at 2000-01-01.
        public string[] text { get; set; }                      //Sets text elements associated with each (x,y) pair. 
        public string textposition { get; set; }                //Type: enumerated or array of enumerateds ,
                                                                //one of ( "top left" | "top center" | "top right" | "middle left" |
                                                                //"middle center" | "middle right" | "bottom left" | "bottom center" | "bottom right" )
                                                                //Default: "middle center"

        public dynamic texttemplate { get; set; }
        public dynamic hovertext { get; set; }
        public string hoverinfo { get; set; }
        public dynamic hovertemplate { get; set; }             //Type: string or array of strings
                                                               //Default: ""
                                                               //Template string used for rendering the information that appear on hover box.Note that this will override `hoverinfo`. Variables are inserted using %{variable},
                                                               //for example "y: %{y}" as well as %{ xother}, {% _xother}, {% _xother_}, {% xother_}. When showing info for several points, "xother" will be added to those with
                                                               //different x positions from the first point. An underscore before or after "(x|y)other" will add a space on that side, only when this field is shown.
                                                               //Numbers are formatted using d3-format's syntax %{variable:d3-format}, for example "Price: %{y:$.2f}". https://github.com/d3/d3-format/tree/v1.4.5#d3-format for details
                                                               //on the formatting syntax. Dates are formatted using d3-time-format's syntax %{variable|d3-time-format}, for example
                                                               //"Day: %{2019-01-01|%A}".https://github.com/d3/d3-time-format/tree/v2.2.3#locale_format for details on the date formatting syntax. The variables available in `hovertemplate`
                                                               //are the ones emitted as event data described at this link https://plotly.com/javascript/plotlyjs-events/#event-data. Additionally, every attributes that can be specified
                                                               //per-point (the ones that are `arrayOk: true`) are available. Anything contained in tag `<extra>` is displayed in the secondary box, for example "<extra>{fullData.name}</extra>".
                                                               //To hide the secondary box completely, use an empty tag `<extra></extra>`.

        public string xhoverformat { get; set; }
        public string yhoverformat { get; set; }
        public dynamic meta { get; set; }
        public dynamic[] customdata { get; set; }

        public Font textfont { get; set; }

        public Marker marker { get; set; }
        public Error_X error_x { get; set; }
        public Error_Y error_y { get; set; }
        public Calendar xcalendar { get; set; }
        public Calendar ycalender { get; set; }
    }
}
