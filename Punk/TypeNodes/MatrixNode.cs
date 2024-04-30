using System;
using MathNet.Numerics.LinearAlgebra;
using Punk.Types;

namespace Punk.TypeNodes
{
    public class MatrixNode :TreeNode, IResultTreeNode
    {
        public MatrixType matrix {  get; set; }
        public MatrixNode(MatrixType matrixType)
        {
            this.matrix = matrixType;
            this.token = new Token(TokenType.MatrixType, "");
        }
        
        public override string Print()
        {
            return $"(Matrix {this.matrix.Value.RowCount} x {this.matrix.Value.ColumnCount} )";
        }
        public override TreeNode Eval()
        {
            return this;
        }

        public object GetResult()
        {
            return matrix.Value.ToRowArrays();
        }
    }
}
