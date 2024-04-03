using Punk;
using Punk.TypeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punk2.UnaryOperators
{
    public class QueryNode : TreeNode
    {
        RegisterNode? Bottom;
        public Query query {  get; private set; }
        public QueryNode(RegisterNode node, string Syntax) 
        {
            this.query = new Query(Syntax, node.Register);
            this.Bottom = node;
        }
        public override TreeNode Eval()
        {
            return this;
        }

        public override string Print()
        {
            return $"(Lambda({this.query.QueryStr}))"; 
        }
    }
}
