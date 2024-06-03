
using Punk.Types;

namespace Punk.TypeNodes
{
    public class NumberNode : TreeNode, IResultTreeNode
    {
        public NumberType NumberTypeValue { get; set; }
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
            NumberTypeValue = n1;
        }

        public NumberNode(NumberType value)
        {
            this.Right = null;
            this.Left = null;
            this.NumberTypeValue = value;
            this.token = new Token(TokenType.NumberType, value.NumberValue.ToString());
        }

        public NumberNode(dynamic number)
        {
            this.Right = null;
            this.Left = null;
            this.NumberTypeValue = new NumberType(number);
            this.token = new Token(TokenType.NumberType, number.ToString());
        }
        public override TreeNode Eval()
        {
          
            return this;
        }

        public override string Print()
        {
            if(this.token != null)
            {
                return $"{this.token.Value}";
            }
            else { return ""; }
        }

        public object GetResult()
        {

            return new List<object> { new { x = NumberTypeValue.NumberValue } };
        }
    }
}
