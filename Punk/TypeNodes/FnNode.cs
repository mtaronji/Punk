using MathNet.Numerics.LinearAlgebra;
using Punk.Types;

namespace Punk.TypeNodes
{
    //lexer doesn't create these. These are library nodes created my us
    public class FnNode : TreeNode
    {      
        public string FNId { get; private set; }
        public dynamic FN { get; set; }
        public List<dynamic> Args { get; set; }
        public FnNode(string ID, dynamic FN, IEnumerable<TreeNode> Args) 
        {
            this.Args = new();
            this.FNId = ID;
            this.FN = FN;
            SetArgs(Args);
        }
  
        public override TreeNode Eval()
        {
            return this;
        }

        public TreeNode Invoke()
        {
            object? result;
            result = FN(Args);
            if (result is double || result is long)
            {
                return new NumberNode(result);
            }
            else if (result is Matrix<double>) { return new MatrixNode(new MatrixType((Matrix<double>)result)); }
            else if (result is Vector<double>) { return new MatrixNode(new MatrixType((Vector<double>)result)); }
            else if (result is Vector<double>[]) { return new MatrixNode(new MatrixType((Vector<double>[])result)); }
            else
            {
                throw new NotImplementedException("Current implemented FN return types are double and the Matrix set");
            }
        }

        public override string Print()
        {
           return $"({this.FNId}";       
        }

        public void SetArgs(IEnumerable<TreeNode> args)
        {
            if (args == null) { return; }
            var argvs = new List<dynamic>();
            foreach(var node in args)
            {
                if(node is NumberNode) { NumberNode n = (NumberNode)node; argvs.Add(n.NumberTypeValue.NumberValue); }
                if(node is MatrixNode) { MatrixNode m = (MatrixNode)node; argvs.Add(m.matrix.Value); }
              
            }
            this.Args = argvs;
        }
    }
}
