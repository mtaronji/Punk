using Punk.TypeNodes;
using Punk.Types;


namespace Punk.UnaryOperators
{
    public class QueryNode : TreeNode, IResultTreeNode
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
            return $"(Query({this.query.QueryStr}))"; 
        }

        public object GetResult()
        {
            return query.EvaulatedQuery;
        }
    }
}
