

namespace Punk.Plotly
{
    public class PlotlyEnumerations
    {
        public HashSet<string> ExponentFormats = new HashSet<string>
        {
           "none","e","E","power","SI","B"
        };
        public HashSet<string> LenModeFormats = new HashSet<string>
        {
           "fraction","pixels"
        };
        public HashSet<string> OrientationTypes = new HashSet<string>
        {
           "h","v"
        };
        public HashSet<string> TickprefixTypes = new HashSet<string>
        {
           "all","first","last","none"
        };
        public HashSet<string> TickLabelOverflowTypes = new HashSet<string>
        {
           "allow","hide past div","hide past domain"
        };
        public HashSet<string> TickLabelPosition = new HashSet<string>
        {
           "outside","inside","outside top","inside top","outside left","inside left","outside right","inside right","outside bottom","inside bottom"
        };
        public HashSet<string> TickModeTypes = new HashSet<string>
        {
           "auto","linear","array"
        };
        public HashSet<string> Legends = new HashSet<string>
        {
           "legend", "legend2", "legend3", "legend4","legend5",
            "legend6", "legend7", "legend8", "legend9","legend10",
             "legend11", "legend12", "legend13", "legend14","legend15",
            "legend16", "legend17", "legend18", "legend19","legend20"
        };
        public HashSet<string> coloraxis = new HashSet<string>
        {
            "coloraxis","coloraxis2", "coloraxis3","coloraxis4", "coloraxis5","coloraxis6", "coloraxis7","coloraxis8", "coloraxis9","coloraxis10", "coloraxis11","coloraxis12"
        };
        public HashSet<string> TextPositions = new HashSet<string>
        {
            "top left","top center","top right","middle left","middle center","middle right","bottom left","bottom center","bottom right"
        };
        public HashSet<string> TopThreeSides = new HashSet<string>
        {
            "right","top","bottom"
        };
        public HashSet<string> OutsideOrInsideOrDefault = new HashSet<string>
        {
            "outside","inside",""
        };
        public HashSet<string> ColorbarXAnchorTypes = new HashSet<string>
        {
            "left","center","right"
        };
        public HashSet<string> ColorbarYAnchorTypes = new HashSet<string>
        {
            "top","middle","bottom"
        };
        public HashSet<string> ColorbarAxisRef = new HashSet<string>
        {
            "container","paper"
        };
        public HashSet<string> SizemodeTypes = new HashSet<string>
        {
            "diameter", "area"
        };
        public HashSet<string> SymbolTypes = new HashSet<string>()
        {
            "circle", "circle-open", "circle-dot",  "circle-open-dot",  "square" , "square-open" , "square-dot" ,"square-open-dot" , "diamond"  ,"diamond-open"  ,"diamond-dot",
            "diamond-open-dot",  "cross", "cross-open", "cross-dot",  "cross-open-dot" , "x" , "x-open" ,"x-dot" , "x-open-dot",  "triangle-up" , "triangle-up-open" , "triangle-up-dot" ,
            "triangle-up-open-dot", "triangle-down" ,"triangle-down-open", "triangle-down-dot" , "triangle-down-open-dot" , "triangle-left",  "triangle-left-open", "triangle-left-dot",
            "triangle-left-open-dot", "triangle-right", "triangle-right-open", "triangle-right-dot",  "triangle-right-open-dot" , "triangle-ne" , "triangle-ne-open" ,"triangle-ne-dot",
            "triangle-ne-open-dot", "triangle-se" , "triangle-se-open",  "triangle-se-dot"  ,"triangle-se-open-dot" , "triangle-sw",  "triangle-sw-open",  "triangle-sw-dot"  ,
            "triangle-sw-open-dot" ,"triangle-nw", "triangle-nw-open",  "triangle-nw-dot", "triangle-nw-open-dot", "pentagon" , "pentagon-open",  "pentagon-dot" , "pentagon-open-dot" ,
            "hexagon" , "hexagon-open" ,"hexagon-dot",  "hexagon-open-dot",  "hexagon2",  "hexagon2-open", "hexagon2-dot" , "hexagon2-open-dot" , "octagon" , "octagon-open" ,"octagon-dot" ,
             "octagon-open-dot",  "star" , "star-open",  "star-dot",  "star-open-dot" ,"hexagram", "hexagram-open", "hexagram-dot" , "hexagram-open-dot",  "star-triangle-up" , "star-triangle-up-open" ,
            "star-triangle-up-dot",  "star-triangle-up-open-dot"  ,"star-triangle-down",  "star-triangle-down-open" , "star-triangle-down-dot",  "star-triangle-down-open-dot" , "star-square",
            "star-square-open",  "star-square-dot" , "star-square-open-dot"  ,"star-diamond", "star-diamond-open" , "star-diamond-dot" , "star-diamond-open-dot" , "diamond-tall", "diamond-tall-open" ,
            "diamond-tall-dot" , "diamond-tall-open-dot" ,"diamond-wide" , "diamond-wide-open" , "diamond-wide-dot",  "diamond-wide-open-dot"  ,"hourglass" , "hourglass-open" , "bowtie" , "bowtie-open",
            "circle-cross" ,"circle-cross-open",  "circle-x" , "circle-x-open" ,"square-cross" , "square-cross-open" ,"square-x",  "square-x-open" ,"diamond-cross" , "diamond-cross-open"  ,"diamond-x" , "diamond-x-open" ,
            "cross-thin", "cross-thin-open" , "x-thin" , "x-thin-open" ,"asterisk" , "asterisk-open",  "hash",  "hash-open" , "hash-dot",  "hash-open-dot" , "y-up" , "y-up-open",  "y-down" , "y-down-open",
            "y-left",  "y-left-open" , "y-right" , "y-right-open" , "line-ew" , "line-ew-open" , "line-ns" , "line-ns-open",  "line-ne",  "line-ne-open" , "line-nw" , "line-nw-open" , "arrow-up" ,"arrow-up-open" ,"arrow-down",
            "arrow-down-open",  "arrow-left", "arrow-left-open"  ,"arrow-right"  ,"arrow-right-open" , "arrow-bar-up", "arrow-bar-up-open" , "arrow-bar-down" , "arrow-bar-down-open",  "arrow-bar-left" , "arrow-bar-left-open",
            "arrow-bar-right",  "arrow-bar-right-open" ,"arrow" , "arrow-open" , "arrow-wide", "arrow-wide-open"
        };

        public HashSet<string> ScatterFillTypes = new HashSet<string>()
        {
            "none","tozeroy","tozerox","tonexty","tonextx", "toself","tonext"
        };
        public HashSet<string> CalenderTypes = new HashSet<string>()
        {
            "chinese","coptic","discworld","ethiopian","gregorian","hebrew","islamic","jalali","julian","mayan",
            "nanakshahi","nepali","persian","taiwan","thai","ummalqura"
        };
        public HashSet<string> angleref = new HashSet<string> 
        {
            "previous",
            "up"
        };
        public HashSet<string> PeriodAlignmentTypes = new HashSet<string> //xperiodalignment, yperiodalignment
        {
            "start","middle","end"
        };
    }
}
