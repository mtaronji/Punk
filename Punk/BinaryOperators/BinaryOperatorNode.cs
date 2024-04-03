namespace Punk.BinaryOperators
{
    public delegate NumberType Operation(NumberType n1, NumberType n2);
    public abstract class BinaryOperatorNode:TreeNode
    {
        public BinaryOperatorNode()
        {

        }

        public abstract Operation GetOperationDelegate();

    }

}
