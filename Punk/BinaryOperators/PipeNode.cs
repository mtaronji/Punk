using Punk.TypeNodes;
using System.Collections.Generic;


namespace Punk.BinaryOperators
{
    public class PipeNode : TreeNode
    {
        public TreeNode? Result { get; private set; }
        private PipeFactory _pipefactory;

        public PipeNode(TreeNode A, TreeNode B)
        {
            this.Left = A; this.Right = B;
            this._pipefactory = new PipeFactory();
        }
        public override TreeNode Eval()
        {
            if(this.Left != null && this.Right != null)
            {
                return this._pipefactory.Combine(this.Left.Eval(), this.Right.Eval());
            }
            else
            {
                throw new Exceptions.PunkPipeException("Left or right operand to pipe is empty");
            }        
        }

        public override string Print()
        {
            if (this.Left != null && this.Right != null)
            {
                return $"({this.Left.Print()} | {this.Right.Print()})";
            }
            else { return ""; }
        }
    }
}



///                     
////#stock#spy | plot#scatter
///                     
///                         Pipe  
///                        /    \  
///                     Mtrx     Pipe
///                             /    \  
///                          Trpse    Pipe
///                                  /    \
///                               Invrs   Kernel