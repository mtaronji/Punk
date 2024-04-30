using Punk.TypeNodes;
using Punk.Types;
using System;

namespace Punk.BinaryOperators
{
    //for operations that are using ^ operator 
    public class ExponentialNode : BinaryOperatorNode
    {
        public NumberType Result { get; set; }
        private Operation exponential;
        public ExponentialNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;
            this.exponential = (NumberType n1, NumberType n2) => { return new NumberType(Math.Pow((double)n1.Value, (double)n2.Value)); };

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
            var node1 = (NumberNode)a;
            var node2 = (NumberNode)b;
            var n1 = node1.Value;
            var n2 = node2.Value;

            //we will do all ex with double
            var result = Math.Pow((double)n1.Value, (double)n2.Value);

            if ((n1.Value is long && n2.Value is long) || (n1.Value is int && n2.Value is int))
            {
                Result = new NumberType((long)result);
            }
            else
            {
                Result = new NumberType(result);
            }

            return new NumberNode(Result);
        }

        public override string Print()
        {
            return $"(Pow({this.Left.Print()},{this.Right.Print()}))";
        }

        public override Operation GetOperationDelegate()
        {
            return this.exponential;
        }

    }
    
}