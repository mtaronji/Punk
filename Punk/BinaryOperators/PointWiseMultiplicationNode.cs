using Punk.TypeNodes;
using Punk.Types;



namespace Punk.BinaryOperators
{
    public class PointWiseMultiplicationNode : TreeNode
    {
        public dynamic? Result { get; set; }

        public PointWiseMultiplicationNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;
        }

        public override TreeNode Eval()
        {
            var a = this.Left.Eval();
            var b = this.Right.Eval();

            if (a is IdentifierNode)
            {
                IdentifierNode i = (IdentifierNode)a;
                a = i.Value;
            }
            if (a is MatrixNode && b is MatrixNode)
            {
                var node1 = (MatrixNode)a;
                var node2 = (MatrixNode)b;
                var n1 = node1.matrix;
                var n2 = node2.matrix;

                Result = n1.Value.PointwiseMultiply(n2.Value);

                var token = new Token(TokenType.MatrixType, Result.ToString());
                return new MatrixNode(new MatrixType(Result));
            }
            else
            {
                throw new Exceptions.PunkSyntaxErrorException("As of yet, point wise multiplication isn't supported with that type");
            }


        }

        public override string Print()
        {
            if (this.Left != null && this.Right != null)
            {
                return $"({this.Left.Print()} .* {this.Right.Print()})";
            }
            else
            {
                return "";
            }

        }

    }
}
