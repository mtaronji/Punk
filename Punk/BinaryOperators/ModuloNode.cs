using Punk.TypeNodes;
using Punk.Types;


namespace Punk.BinaryOperators
{
    public class ModuloNode : TreeNode
    {


        public ModuloNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;
        }

        public override TreeNode Eval()
        {
            if(this.Left == null || this.Right == null)
            {
                throw new Punk.Exceptions.PunkModuloException("Operand for modulo operation is incorrect");
            }
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

            if (a is NumberNode && b is NumberNode)
            {
                var node1 = (NumberNode)a;
                var node2 = (NumberNode)b;
                var n1 = node1.NumberTypeValue;
                var n2 = node2.NumberTypeValue;
                if (n1 == null || n2 == null)
                {
                    throw new Punk.Exceptions.PunkModuloException("Evaluation in Modulo failed. Check syntax");
                }

                NumberType Result;
                if (n1.NumberValue is long && n2.NumberValue is long)
                {
                    Result = new NumberType((long)n1.NumberValue % (long)n2.NumberValue);
                }
                else
                {
                    Result = new NumberType((double)n1.NumberValue % (double)n2.NumberValue);
                }
                var token = new Token(TokenType.NumberType, Result.ToString());
                return new NumberNode(Result);
            }
            else if (a is MatrixNode && b is MatrixNode)
            {
                var node1 = (MatrixNode)a;
                var node2 = (MatrixNode)b;
                var n1 = node1.matrix.Value;
                var n2 = node2.matrix.Value;
                if (n1 == null || n2 == null)
                {
                    throw new Punk.Exceptions.PunkModuloException("Evaluation in Modulo failed for Matrix Modulo. Check syntax");
                }
                MatrixType Result;
                Result = new MatrixType(n1 % n2);
                  
                var token = new Token(TokenType.MatrixType, Result.Value.ToString());
                return new MatrixNode(Result);
            }
            else
            {
                throw new Exceptions.PunkModuloException("Modulo not supported for that type");
            }


        }

        public override string Print()
        {
            if(this.Left != null && this.Right != null) 
            {
                return $"({this.Left.Print()} % {this.Right.Print()})";
            }
            else
            {
                return "";
            }
        }

    }
}
