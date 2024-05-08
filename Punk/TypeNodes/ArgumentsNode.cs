

namespace Punk.TypeNodes
{
    public class ArgumentsNode : TreeNode
    {
        public List<TreeNode> Arguments = new List<TreeNode>();
        public TreeNode? Bottom;
        public override TreeNode Eval()
        {
            if(Bottom == null)
            {
                throw new Exceptions.PunkArgumentException("Can't find target to apply arguments to");
            }
            else if(Bottom != null)
            {
                IdentifierNode? id = Bottom.Eval() as IdentifierNode; if(id == null) { throw new Exceptions.PunkArgumentException("Arguments should only be applied to identifiers"); }
                IArgumentsNode? iarg = id.Value as IArgumentsNode; if(iarg == null) { throw new Exceptions.PunkArgumentException("Arguments Must be applied to types that support them"); }
                iarg.SetArgument(this);
                return (TreeNode)iarg;
            }
 
            throw new Exceptions.PunkArgumentException();
            
        }
        public void AddArgument(NumberNode arg)
        {
            this.Arguments.Add(arg);
        }
        public override string Print()
        {
            if(Bottom == null) { return ""; }
            string printStatement = "Args -> (";
            foreach(var node in Arguments)
            {
                printStatement += $"{node.Print()},";
            }
            printStatement.Remove(printStatement.Length - 1);
            printStatement += ")";
            return $"({Bottom.Print()} ({printStatement}))";
        }
    }
}
