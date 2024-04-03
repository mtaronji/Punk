
namespace Punk.TypeNodes
{
    //identifier should have a value that is determined before parsing it
    //in this case, it's a data node or an identifier or a function
    public class IdentifierNode : TreeNode
    {
        //this value should be a type. As of 2024 Feb it is a Sequence, number or data type, Matrix
        public TreeNode? Value { get; set; }

        //pass an identifier a value
        public IdentifierNode(TreeNode Value)
        {
            this.Value = Value;
        }

        public IdentifierNode(Token token)
        {
            this.token = token;
        }
        public override TreeNode Eval()
        {
            var node = Value;
            while(node is IdentifierNode)
            {
                IdentifierNode idnode = (IdentifierNode)node;
                node = idnode.Value;
            }
            this.Value = node;
            return this;
        }

        public override string Print()
        {
            if(this.token != null)
            {
                return $"{this.token.Value}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
