﻿using Punk.TypeNodes;
using Punk.Types;



namespace Punk.BinaryOperators
{
    public class MultiplicationNode:TreeNode
    {
        public MultiplicationNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;
        }

        public override TreeNode Eval()
        {
            if(this.Left == null || this.Right == null) { throw new Exceptions.PunkMulitiplicationException("Operands for multiplication operator missing"); }
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
            if(a is NumberNode && b is NumberNode)
            {
                var node1 = (NumberNode)a;
                var node2 = (NumberNode)b;
                var n1 = node1.NumberTypeValue;
                var n2 = node2.NumberTypeValue;

                if ((n1.NumberValue is long && n2.NumberValue is long) || (n1.NumberValue is int && n2.NumberValue is int))
                {
                    return new NumberNode(new NumberType((long)n1.NumberValue * (long)n2.NumberValue));
                }
                else
                {
                    return new NumberNode(new NumberType((double)n1.NumberValue * (double)n2.NumberValue));
                }
            }
            else if(a is MatrixNode && b is MatrixNode)
            {
                var node1 = (MatrixNode)a;
                var node2 = (MatrixNode)b;
                var n1 = node1.matrix;
                var n2 = node2.matrix;

                var Result = n1.Value * n2.Value;

                return new MatrixNode(new MatrixType(Result));
            }
            else
            {
                throw new Exceptions.PunkSyntaxErrorException();
            }
   

        }

        public override string Print()
        {
            if(this.Left != null && this.Right != null) 
            {
                return $"({this.Left.Print()} * {this.Right.Print()})";
            }
            else
            {
                return "";
            }
            
        }

    }
}
