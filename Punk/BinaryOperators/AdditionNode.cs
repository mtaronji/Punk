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
            this.addition = (NumberType n1, NumberType n2) => { return new NumberType(n1.Value + n2.Value); };
         
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
            if (b is IdentifierNode)
            {
                IdentifierNode i = (IdentifierNode)b;
                b = i.Value;
            }

            if (a is NumberNode && b is NumberNode)
            {
                var node1 = (NumberNode)a;
                var node2 = (NumberNode)b;
                var n1 = node1.Value;
                var n2 = node2.Value;

                if ((n1.Value is long && n2.Value is long) || (n1.Value is int && n2.Value is int))
                {
                    Result = new NumberType((long)n1.Value + (long)n2.Value);
                }
                else
                {
                    Result = new NumberType((double)n1.Value + (double)n2.Value);
                }

                return new NumberNode(Result);
            }
            if (a is DataNode && b is DataNode)
            {
                var node1 = (DataNode)a;
                var node2 = (DataNode)b;
                var d1 = node1.Value;
                var d2 = node2.Value;
                Token t = new Token(TokenType.DataType, $"{a.token.Value} + {b.token.Value}");
                var d3 = d1 + d2;
                return new DataNode(d3, t);

            }
            throw new Exceptions.PunkAdditionException("Datatype not supported for addition");
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
