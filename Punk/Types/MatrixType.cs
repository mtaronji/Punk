
using MathNet.Numerics.LinearAlgebra;
using System.Text.RegularExpressions;


namespace Punk.Types
{
    //can be a vector or a matrix or vector []
    
    public class MatrixType
    {

        public double Determinant { get; private set; }

        //matrix or Vector
        public dynamic Value { get; set; }
        

        public string Syntax { get; set; }
        public static Regex SplitonWhitespace = new Regex(@"\s+");
        public MatrixType(string syntax)
        {
            Syntax = syntax;
            Value = TryParseMatrixSyntax();

        }
        public void CalculateDeterminant()
        {
           if(!(this.Value is Matrix<double>)) { throw new Exceptions.PunkSyntaxErrorException("You can only calculate a determinant for a matrix"); }
           var matrix = (Matrix<double>)Value;
           Determinant = matrix.Determinant();
              
        }
        public MatrixType(Matrix<double> m)
        {
            Value = m;
            this.Syntax = string.Empty;
        }

        public MatrixType(Vector<double> m)
        {
            Value = m;
            this.Syntax = string.Empty;
        }
        public MatrixType(Vector<double>[] vectors)
        {
            Value = vectors;
            this.Syntax = string.Empty;
        }
        private Matrix<double> TryParseMatrixSyntax()
        {

            string BarsRemoved = Syntax.Replace("|", "").Trim();
            var rows = BarsRemoved.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            double[][] matrixdata = new double[rows.Length][];
            for (int i = 0; i < rows.Length; i++)
            {

                string[] colvalues = SplitonWhitespace.Split(rows[i].Trim()).Where(c => c != string.Empty).ToArray();

                matrixdata[i] = new double[colvalues.Length];
                for (int j = 0; j < colvalues.Length; j++)
                {
                    var success = double.TryParse(colvalues[j].Trim(), out double value);
                    if (!success) { throw new Exceptions.PunkMatrixParseException(); }
                    matrixdata[i][j] = value;
                }

            }
            return Matrix<double>.Build.DenseOfRowArrays(matrixdata);
        }

    }
}
