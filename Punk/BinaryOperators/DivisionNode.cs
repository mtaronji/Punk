using Punk.TypeNodes;
using Punk.Types;


namespace Punk.BinaryOperators
{
    public class DivisionNode : TreeNode
    {
        public DivisionNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;
        }

        public override TreeNode Eval()
        {
            if (this.Left == null || this.Right == null) { throw new Exceptions.PunkDivisionException("Division operands are empty. Please check values"); }
            var a = this.Left.Eval();
            var b = this.Right.Eval();
 

            if (a is IdentifierNode)
            {
                IdentifierNode i = (IdentifierNode)a;
                a = i.Value;
            }
            if (b is IdentifierNode)
            {
                IdentifierNode i = (IdentifierNode)b;
                b = i.Value;
            }
            if (a == null || b == null) { throw new Exceptions.PunkDivisionException("Division operands are empty. Please check values"); }
            var node1 = (NumberNode)a;
            var node2 = (NumberNode)b;
            var n1 = node1.Value;
            var n2 = node2.Value;

            if ((n1.Value is long && n2.Value is long)||(n1.Value is int && n2.Value is int))
            {
                return new NumberNode( new NumberType((long)n1.Value / (long)n2.Value));
            }
            else
            {
                return new NumberNode(new NumberType((double)n1.Value / (double)n2.Value));
            }
        }

        public override string Print()
        {
            if(Left != null && Right != null)
            {
                return $"({this.Left.Print()} / {this.Right.Print()})";
            }
            else
            {
                return "";
            }
        }

    }
}
