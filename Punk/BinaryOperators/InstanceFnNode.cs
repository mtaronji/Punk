using Punk.TypeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
  
        public override TreeNode? Eval()
        {

            var left = this.Left.Eval();
            var right = this.Right.Eval();
            if(!(right is FnNode)) { throw new Exceptions.PunkInstanceMethodException("Right argument of instance doesn't evaluate to a function and or members are null"); }
            if(left == null || right == null) { throw new Exceptions.PunkInstanceMethodException("Instead method null argument. Check syntax"); }
            return this._instanceFnFactory.Invoke(left, (FnNode)right);

        }

        public override string Print()
        {
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
