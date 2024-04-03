using MathNet.Numerics.LinearAlgebra;
using Punk.TypeNodes;
using System;


namespace Punk
{
    public class InstanceFnFactory
    {
        public InstanceFnFactory()
        {

        }

        public TreeNode Invoke(TreeNode A, TreeNode B)
        {
            if (B is IdentifierNode)
            {
                if (A is MatrixNode)
                {
                    return MatrixInstanceFnFactory((MatrixNode)A, (IdentifierNode)B);
                }
                else if (A is RegisterNode)
                {
                    return RegisterInstanceFnFactory((RegisterNode)A, (IdentifierNode)B);
                }
                else if (A is DataNode)
                {
                    return DataInstanceFnFactory((DataNode)A, (IdentifierNode)B);
                }
                else
                {
                    return null;
                }
            }     
            else
            {
                return null;
            }
        }

        TreeNode MatrixInstanceFnFactory(MatrixNode A, IdentifierNode B)
        {
            switch (B.token.Value.ToLower())
            {
                case ("transpose"):
                    A.matrix.Value = A.matrix.Value.Transpose();
                    break;
                case ("inverse"):
                    A.matrix.Value = A.matrix.Value.Inverse();
                    break;
                case ("determinant"):
                    A.matrix.CalculateDeterminant();
                    return new NumberNode(new NumberType(A.matrix.Determinant));
                
                case ("kernel"):
                    A.matrix.Value = Matrix<double>.Build.DenseOfColumnVectors(A.matrix.Value.Kernel());
                    break;
                default:
                    return null;
            }
            return A;
        }
        TreeNode DataInstanceFnFactory(DataNode A, TreeNode B)
        {
            if (B is IdentifierNode)
            {
                throw new NotImplementedException();
            }

            else
            {
                throw new Exceptions.PunkInstanceMethodException($"Unable to Pipe {A.token.Value} with {B.token.Value} ");
            }
        }
        TreeNode RegisterInstanceFnFactory(RegisterNode A, TreeNode B)
        {
            if (B is IdentifierNode)
            {
                throw new NotImplementedException();
            }

            else
            {
                throw new Exceptions.PunkInstanceMethodException($"Unable to Pipe {A.token.Value} with {B.token.Value} ");
            }
        }

    }
}
