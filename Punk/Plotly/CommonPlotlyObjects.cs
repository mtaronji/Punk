

namespace Punk.Plotly
{
    public class TickFormatStops
    {
        public dynamic[] dtickrange { get; set; } //array of strings or float
        public bool enabled { get; set; }
        public string name { get; set; }
        public string templateitemname { get; set; }
        public string value { get; set; }
        public float ticklen { get; set; }
    }
    public class Font
    {
        public string color { get; set; }
        public string family { get; set; }
        public int size { get; set; }
    }
    public class Title
    {
        public Font font { get; set; }
        public string side { get; set; } //right top bottom (use TopThreeSides)
        public string text { get; set; }
    }
    public class RGBAColor
    {
        private int R;
        private int G;
        private int B;
        private int A;
        public RGBAColor(int R, int G, int B, int A)
        {
            this.R = R; this.R = G; this.G = B; this.B = A;
        }
        public override string ToString()
        {
            return $"rgba({R},{G},{B},{A})";
        }
    }

    public class HoverLabel
    {
        public dynamic align { get; set; } //Parent: data[type = scattergl].hoverlabel
                                           //Type: enumerated or array of enumerateds , one of( "left" | "right" | "auto" )
                                           //Default: "auto"Sets the horizontal alignment of the text content within hover label box.Has an effect only if the hover label text spans more two or more lines

        public dynamic bgcolor { get; set; } //Parent: data[type = scattergl].hoverlabel
                                             //Type: color or array of colors Sets the background color of the hover labels for this trace bordercolor
        public dynamic bordercolor { get; set; } //Type: color or array of colors
                                                 //Sets the border color of the hover labels for this trace.
        public Font font { get; set; }
        public dynamic namelength { get; set; } //Sets the default length (in number of characters) of the trace name in the hover labels for all traces.
                                                //-1 shows the whole name regardless of length. 0-3 shows the first 0-3 characters, and an integer >3
                                                //will show the whole name if it is less than that many characters, but if it is longer, will truncate
                                                //to `namelength - 3` characters and add an ellipsis.

    }

  

    public class ScatterLine
    {
        public dynamic backoff { get; set; }

        public string color { get; set; }

        public string dash { get; set; } //Sets the dash style of lines. Set to a dash type string ("solid", "dot", "dash", "longdash", "dashdot", or "longdashdot") or a dash
        public string shape { get; set; } //"linear" | "spline" | "hv" | "vh" | "hvh" | "vhv"
        public bool simplify { get; set; }
        public float smoothing { get; set; } //Type: number between or equal to 0 and 1.3
                                             //Default: 1 Has an effect only if `shape` is set to "spline" Sets the amount of smoothing.
                                             //"0" corresponds to no smoothing(equivalent to a "linear" shape)
        public float width { get; set; } //Sets the line width (in px).
    }
    public class Error_X
    {

    }
    public class Error_Y
    {

    }

    public class Gradient
    {
        public dynamic color { get; set; }
        public string type { get; set; }
    }
}
