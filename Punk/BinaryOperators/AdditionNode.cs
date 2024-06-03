using Punk.TypeNodes;
using Punk.Types;
using System.ComponentModel.Design.Serialization;


namespace Punk.BinaryOperators
{
    public class AdditionNode : BinaryOperatorNode
    {
        public NumberType? Result { get; set; }
        private Operation addition;
        public AdditionNode(TreeNode A, TreeNode B)
        {

            this.Left = A;
            this.Right = B;
            this.addition = (NumberType n1, NumberType n2) => { return new NumberType(n1.NumberValue + n2.NumberValue); };
         
        }

        public override TreeNode Eval()
        {
            if (this.Left == null || this.Right == null) { throw new Exceptions.PunkAdditionException("Addition operator missing arguments from left and/or right operands"); }
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

                if ((n1.NumberValue is long && n2.NumberValue is long) || (n1.NumberValue is int && n2.NumberValue is int))
                {
                    Result = new NumberType((long)n1.NumberValue + (long)n2.NumberValue);
                }
                else
                {
                    Result = new NumberType((double)n1.NumberValue + (double)n2.NumberValue);
                }

                return new NumberNode(Result);
            }
            else if (a is DataNode && b is DataNode)
            {
                var node1 = (DataNode)a;
                var node2 = (DataNode)b;
                var d1 = node1.Value;
                var d2 = node2.Value;
                Token t = new Token(TokenType.DataType, $"{a.Print()} + {b.Print()}");
                var d3 = d1 + d2;
                return new DataNode(d3, t);
            }
            else if (a is MatrixNode && b is MatrixNode)
            {
                var node1 = (MatrixNode)a;
                var node2 = (MatrixNode)b;
                var n1 = node1.matrix;
                var n2 = node2.matrix;
                
                var Result = n1.Value + n2.Value;

                return new MatrixNode(new MatrixType(Result));
            }
            else
            {
                throw new Exceptions.PunkAdditionException("Datatype not supported for addition");
            }
        }

        public override string Print()
        {
            if(Left != null && Right != null)
            {
                return $"({this.Left.Print()} + {this.Right.Print()})";
            }
            return $"";
        }

        public override Operation GetOperationDelegate()
        {
            return this.addition;
        }
    }
}
