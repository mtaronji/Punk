using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Punk2.UnaryOperators;

namespace Punk.TypeNodes
{
    public class PlotNode : TreeNode
    {


        public IEnumerable<object>? data { get; private set; }

        public PlotNode()
        {

        }

        public PlotNode(string trace)
        {

        }
        public void SetData( IEnumerable<object> data)
        {
            this.data = data;
        }
        


        public override TreeNode Eval()
        {
            
            return this;
        }

        public override string Print()
        {
            
           return $"(Plot User Defined Data)";
                    
        }
    }
}

//plot node example syntax
/// "SPY" | ##stock | ##Plot
///
/// "Spy" | ##stock | ## plot 
/// {
///     ***linq based number or string query****
/// }
///
///
