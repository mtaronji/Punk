
namespace Punk.TypeNodes
{
    public class CodeNode : TreeNode
    {
        public string Code {  get; set; }
        public CodeNode(Token value)
        {
            this.Code = value.Value;
            this.Right = null;
            this.Left = null;
        }
        public override TreeNode Eval()
        {
            return this;
        }

        public override string Print()
        {

            return $"({this.Code})";
        }
    }
}
