
namespace Punk.TypeNodes
{
    public class NumberNode : TreeNode
    {
        public NumberType? Value { get; set; }
        public NumberNode()
        {
            this.Right = null;
            this.Left = null;
        }
        public NumberNode(Token value)
        {
            this.Right = null;
            this.Left = null;
            this.token = value;

            var n1 = new NumberType(this.token.Value);
            if (n1 == null)
            {
                throw new Exceptions.PunkTreeNodeException("Unable to evaluate number of the number node.");
            }
            Value = n1;
        }

        public NumberNode(NumberType value)
        {
            this.Right = null;
            this.Left = null;
            this.Value = value;
        }

        public override TreeNode Eval()
        {
          
            return this;
        }

        public override string Print()
        {
            return $"{this.token.Value}";
        }
    }
}
