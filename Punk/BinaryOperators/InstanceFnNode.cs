using Punk.TypeNodes;


namespace Punk.BinaryOperators
{
    public class InstanceFnNode : TreeNode
    {
        public TreeNode? Result { get; private set; }


        private InstanceFnFactory _instanceFnFactory;

        //period base represents the object that initiates the chain
        public InstanceFnNode(TreeNode A, FnNode B)
        {
            if (A == null || B == null) { throw new Exceptions.PunkInstanceMethodException("Instance method aruguemts null"); }
            this.Left = A; this.Right = B;
 
            this._instanceFnFactory = new InstanceFnFactory();
        }
        

        public override TreeNode Eval()
        {
            if (this.Left == null || this.Right == null) { throw new Exceptions.PunkInstanceMethodException("Left or right side of operand is missing"); }
            var left = this.Left.Eval();
            var right = this.Right.Eval();
            if(left == null || right == null) { throw new Exceptions.PunkInstanceMethodException("Instead method null argument. Check syntax"); }
            if(Right is FnNode)
            {
                return this._instanceFnFactory.Invoke(left, (FnNode)right);
            }
            else
            {
                throw new Exceptions.PunkInstanceMethodException("Expect a FN after '.' seperator");
            }
         

        }

        public override string Print()
        {
            if (this.Left == null || this.Right == null) { return ""; }
            return $"({this.Left.Print()} . {this.Right.Print()})";

        }
    }
}

////Matrix.Transpose.Nullspace 
///                     
///                         InstanceFn  
///                        /          \  
///                     Matrix     Transpose
///                                 /    \  
///                          
