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
            if(this.Left == null) { throw new Exceptions.PunkSyntaxErrorException("Negating an empty value"); }
            var node = (NumberNode)this.Left.Eval();
            node.NumberTypeValue.NumberValue = -node.NumberTypeValue.NumberValue;
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
