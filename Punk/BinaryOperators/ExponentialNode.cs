using Punk.TypeNodes;
using Punk.Types;
using System;

namespace Punk.BinaryOperators
{
    //for operations that are using ^ operator 
    public class ExponentialNode : TreeNode
    {

        public ExponentialNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;

        }

        public override TreeNode Eval()
        {
            if (this.Left == null || this.Right == null) { throw new Exceptions.PunkSyntaxErrorException("Exponential operation missing arguments"); }
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
            if (a == null || b == null) { throw new Exceptions.PunkSyntaxErrorException("Exponential operation missing arguments"); }
            var node1 = (NumberNode)a;
            var node2 = (NumberNode)b;
            var n1 = node1.NumberTypeValue;
            var n2 = node2.NumberTypeValue;

            //we will do all ex with double
            var result = Math.Pow((double)n1.NumberValue, (double)n2.NumberValue);


            if ((n1.NumberValue is long && n2.NumberValue is long) || (n1.NumberValue is int && n2.NumberValue is int))
            {
                return new NumberNode(new NumberType((long)result));
            }
            else
            {
                return new NumberNode(new NumberType(result));
            }

          
        }

        public override string Print()
        {
            if (this.Left == null || this.Right == null) { return ""; }
            return $"(Pow({this.Left.Print()},{this.Right.Print()}))";
        }


    }
    
}