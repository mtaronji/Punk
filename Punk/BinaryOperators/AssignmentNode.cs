using Punk.TypeNodes;
using Punk;
using Punk.UnaryOperators;

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
            if(this.Left == null || this.Right == null) { throw new Exceptions.PunkAssignmentException("Left assigment or right assigment is empty"); }
            IdentifierNode identifierNode = (IdentifierNode)this.Left;
            
            var b = this.Right.Eval();
            identifierNode.Value = b;

            if(b is IdentifierNode)
            {
                var b_cast = (IdentifierNode)b.Eval();
                identifierNode.Value = b_cast.Value;
            }
            else
            {
                identifierNode.Value = b;
            }
            if(!(identifierNode.Value is DataNode || identifierNode.Value is ProbabilityNode || identifierNode.Value is NumberNode
                || identifierNode.Value is QueryNode || identifierNode.Value is MatrixNode)) { throw new Exceptions.PunkArgumentException("Identifiers Can only be set to certain types of values. Please check syntax"); }

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
