

namespace Punk
{
    abstract public class TreeNode
    {
        public Token? token { get; set; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }

        public abstract TreeNode Eval();

        public abstract string Print();
    }

}
