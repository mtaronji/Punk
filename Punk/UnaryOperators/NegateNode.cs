using Punk.TypeNodes;

namespace Punk.UnaryOperators
{
  
    internal class NegateNode : TreeNode
    {
        public NegateNode(TreeNode A)
        {
            this.Left = A;
        }

        public override TreeNode Eval()
        {
            var node = (NumberNode)this.Left.Eval();
            node.Value.Value = -node.Value.Value;
            return node;
        }

        public override string Print()
        {
            if (this.Left != null)
            {
                return $"-{this.Left.Print()}";
            }
            else return "";
        }
    }
}
