using System;
using MathNet.Numerics.LinearAlgebra;

namespace Punk.TypeNodes
{
    public class MatrixNode :TreeNode
    {
        public MatrixType matrix {  get; set; }
        public MatrixNode(MatrixType matrixType)
        {
            this.matrix = matrixType;
        }
        public override string Print()
        {
            return $"({matrix.Value})";
        }
        public override TreeNode Eval()
        {
            return this;
        }
    }
}
