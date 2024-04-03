using Punk.TypeNodes;
using Punk;

namespace Punk.BinaryOperators
{
    //assignment should make both a and b equal to the same value. 
    //then it should return the identifier
    public class AssignmentNode : TreeNode
    {
        public AssignmentNode(TreeNode A, TreeNode B)
        {
            //Give the Identifier the value

            this.Left = A;
            this.Right = B;

            
        }

        public override TreeNode Eval()
        {
            var a = this.Left.Eval(); 
            var b = this.Right.Eval();
            if (!(a is IdentifierNode))
            {
                throw new Exceptions.PunkTreeNodeException("You could only assign values to an identifier");
            }

            //if you have an id node for b, you should set Value property of A to the value of id node B
            IdentifierNode identifierNode = (IdentifierNode)a;
            if(b is IdentifierNode)
            {
                var b_cast = (IdentifierNode)b.Eval();
                identifierNode.Value = b_cast.Value;
            }
            else
            {
                identifierNode.Value = b;
            }
            return identifierNode;        
        }

        public override string Print()
        {
            if(Left != null && Right != null) 
            {
                return $"({this.Left.Print()} = {this.Right.Print()})";
            }
            else
            {
                return "";
            }
        }
    }
}
