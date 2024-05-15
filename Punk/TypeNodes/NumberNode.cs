
using Punk.Types;

namespace Punk.TypeNodes
{
    public class NumberNode : TreeNode, IResultTreeNode
    {
        public NumberType Value { get; set; }
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
            this.token = new Token(TokenType.NumberType, value.Value.ToString());
        }

        public NumberNode(dynamic number)
        {
            this.Right = null;
            this.Left = null;
            this.Value = new NumberType(number);
            this.token = new Token(TokenType.NumberType, number.ToString());
        }
        public override TreeNode Eval()
        {
          
            return this;
        }

        public override string Print()
        {
            return $"{this.token.Value}";
        }

        public object GetResult()
        {

            return new List<object> { new { x = Value.Value } };
        }
    }
}
