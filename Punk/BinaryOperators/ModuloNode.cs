using Punk.TypeNodes;


namespace Punk.BinaryOperators
{
    public class ModuloNode : BinaryOperatorNode
    {
        public NumberType? Result { get; set; }
        private Operation modulo;
        public ModuloNode(TreeNode A, TreeNode B)
        {
            this.Left = A;
            this.Right = B;
            this.modulo = (NumberType n1, NumberType n2) => { return new NumberType(n1.Value % n2.Value); };
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
            var node1 = (NumberNode)a;
            var node2 = (NumberNode)b;
            var n1 = node1.Value;
            var n2 = node2.Value;
            if(n1 == null || n2 == null)
            {
                throw new Punk.Exceptions.PunkModuloException("Evaluation in Modulo failed. Check syntax");
            }

            if (n1.Value is long && n2.Value is long)
            {
                Result = new NumberType((long)n1.Value % (long)n2.Value);
            }
            else
            {
                //error
                throw new Punk.Exceptions.PunkModuloException("Left or Right operator for modulo is Empty");
            }
            var token = new Token(TokenType.NumberType, Result.ToString());
            return new NumberNode(Result);

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

        public override Operation GetOperationDelegate()
        {
            return this.modulo;
        }

    }
}
