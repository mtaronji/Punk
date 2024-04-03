

namespace Punk.Plotly
{
    public class ScatterTrace2d<T1, T2> : Trace2d<T1, T2>
    {
       
        public string mode { get; set; } //Type: flaglist string. Any combination of "lines", "markers", "text" joined with a "+" OR "none".
                                         //Examples: "lines", "markers", "lines+markers", "lines+markers+text", "none"
                                         //Determines the drawing mode for this scatter trace.

        public ScatterLine line { get; set; }
        public bool cliponaxis { get; set; }
        public bool connectgaps { get; set; }
        public string fill { get; set; } //one of ( "none" | "tozeroy" | "tozerox" | "tonexty" | "tonextx" | "toself" | "tonext" )

    }
}
