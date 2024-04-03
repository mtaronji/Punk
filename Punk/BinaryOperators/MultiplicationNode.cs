using Punk.TypeNodes;



namespace Punk.BinaryOperators
{
    public class MultiplicationNode : BinaryOperatorNode
    {
        public dynamic? Result { get; set; }
        private Operation multiplication;
        public MultiplicationNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;
            this.multiplication = (NumberType n1, NumberType n2) => { return new NumberType(n1.Value * n2.Value); };
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
            if(a is NumberNode && b is NumberNode)
            {
                var node1 = (NumberNode)a;
                var node2 = (NumberNode)b;
                var n1 = node1.Value;
                var n2 = node2.Value;

                if ((n1.Value is long && n2.Value is long) || (n1.Value is int && n2.Value is int))
                {
                    Result = new NumberType((long)n1.Value * (long)n2.Value);
                }
                else
                {
                    Result = new NumberType((double)n1.Value * (double)n2.Value);
                }
                var token = new Token(TokenType.NumberType, Result.ToString());
                return new NumberNode(Result);
            }
            else if(a is MatrixNode && b is MatrixNode)
            {
                var node1 = (MatrixNode)a;
                var node2 = (MatrixNode)b;
                var n1 = node1.matrix;
                var n2 = node2.matrix;

                Result = n1.Value * n2.Value;

                var token = new Token(TokenType.MatrixType, Result.ToString()); 
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

        public override Operation GetOperationDelegate()
        {
            return this.multiplication;
        }

    }
}
