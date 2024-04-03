
namespace Punk.TypeNodes
{
    public class StringNode : TreeNode
    {
        public string Value { get; private set; }
        public StringNode(string value)
        {
            this.Value = value;
            this.Left = null; this.Right = null;
        }
        public override TreeNode Eval()
        {
            return this;
        }

        public override string Print()
        {
            return Value;
        }
    }
}
