using MathNet.Numerics.LinearAlgebra;
using Punk.TypeNodes;
using Punk.Types;


namespace Punk.UnaryOperators
{
    public class QueryNode : TreeNode, IResultTreeNode
    {
        public RegisterNode Bottom;
        public Query query {  get; private set; }
        public QueryNode(RegisterNode node, string Syntax) 
        {
            this.query = new Query(Syntax, node.Register);
            this.Bottom = node;
        }
        public override TreeNode Eval()
        {
            if (query.EvaulatedQuery is Matrix<double>)
            {
                var m = query.EvaulatedQuery as Matrix<double>;
                if(m == null) { throw new Exceptions.PunkQueryException("Evaluation of query failed"); }
                MatrixType mt = new MatrixType(m);
                return new MatrixNode(mt);
            }
            else
            {
                return this;
            }
        }

        public override string Print()
        {
            return $"({this.query.QueryStr})"; 
        }

        public object GetResult()
        {
            return query.EvaulatedQuery;
        }
    }
}
