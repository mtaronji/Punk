
namespace Punk.TypeNodes
{
    //identifier should have a value that is determined before parsing it
    //in this case, it's a data node or an identifier or a function
    public class IdentifierNode : TreeNode
    {
        //this value should be a type. As of 2024 Feb it is a Sequence, number or data type, Matrix
        public TreeNode? Value { get; set; }
        //public ArgumentsNode? Args { get; set; }

        //pass an identifier a value
        public IdentifierNode(TreeNode Value, Token token)
        {
            this.token = token;
            this.Value = Value;
        }

        public IdentifierNode(Token token)
        {
            this.token = token;
        }
        public IdentifierNode(string token, TreeNode Value)
        {
            this.Value = Value;
            this.token = new Token(TokenType.IdentityfierType, token);
        }
        public override TreeNode Eval()
        {
            var node = Value;
            if (node != null)
            {             
                node = node.Eval();         
                Value = node;
                return this;
                
            }
            throw new Exceptions.PunkIdentifierUninitializedException("Identifier is empty. Check syntax");                  
        }

        public override string Print()
        {
            if(this.token != null && this.Value != null)
            {
                return $"{this.token.Value} -> {this.Value.Print()}";
            }
            else
            {
                return string.Empty;
            }
        }

        //public object? GetResult()
        //{
        //    if (this.Value == null) { return null; }
        //    if(!(this.Value is IResultTreeNode))
        //    {

        //        throw new Punk.Exceptions.PunkSyntaxErrorException($"Unable to get result from Identifier");
        //    }
        //    var node = (IResultTreeNode)this.Value;
        //    return node.GetResult();
        //}
    }
}
