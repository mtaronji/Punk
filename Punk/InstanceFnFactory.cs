using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Providers.LinearAlgebra;
using Punk.TypeNodes;
using Punk.Types;
using System.Reflection;

namespace Punk
{
    public class InstanceFnFactory
    {
        public InstanceFnFactory()
        {

        }

        public TreeNode Invoke(TreeNode A, FnNode B)
        {         
            if(A is IdentifierNode)
            {
                var idnode = (IdentifierNode)A;
                if(idnode.Value != null) { A = idnode.Value; }
            }
            if (A is DataNode)
            {
                return DataInstanceFnFactory((DataNode)A, B);
            }
            else if(A is MatrixNode)
            {

                return MatrixInstanceFnFactory((MatrixNode)A, B);
            }
            else if (A is ProbabilityNode)
            {

                return ProbabilityInstanceFnFactory((ProbabilityNode)A, B);
            }

            else
            {
                throw new Exceptions.PunkInstanceMethodException("Unknown Instance object. Check Syntax");
            }
            
        }

        TreeNode MatrixInstanceFnFactory(MatrixNode A, FnNode B)
        {
            if (B.FNId == null) { throw new Exceptions.PunkInstanceMethodException("Null information on either side of . operator"); }
            Matrix<double>? Leftside = A.matrix.Value as Matrix<double>;
            if(Leftside == null) { throw new Exceptions.PunkInstanceMethodException($"Left side ({B.FNId}) of instance operator is not a matrix"); }
            B.Args.Insert(0, Leftside);
            return B.Invoke();
        }
        TreeNode ProbabilityInstanceFnFactory(ProbabilityNode A, FnNode B)
        {
            if (B.FN == null) { throw new Exceptions.PunkInstanceMethodException("Can't Find the probability FN"); }
            B.Args.Insert(0, A.Distribution);
            return B.Invoke();

            throw new NotImplementedException("Probability Distribution interval type not implemented");


        }
        TreeNode DataInstanceFnFactory(DataNode A, FnNode B)
        {
            if (B.FNId == null) { throw new Exceptions.PunkInstanceMethodException("Null information on either side of . operator"); }
            if(A.Value.DataVectors.Count < 1) { throw new Exceptions.PunkInstanceMethodException("Instance methods don't work on empty data");}
            if (A.Value.DataVectors[0].Count < 1) { throw new Exceptions.PunkInstanceMethodException("Instance methods don't work on empty data"); }
            List<object>? Leftside = A.Value.DataVectors[0] as List<object>;
            if (Leftside == null) { throw new Exceptions.PunkInstanceMethodException($"Left side ({B.FNId}) of instance operator is not a matrix"); }
            B.Args.Insert(0, Leftside); 
            return B.Invoke();
        }

        TreeNode QueryInstanceFnFactory(Query A, FnNode B)
        {
            if (B.FNId == null) { throw new Exceptions.PunkInstanceMethodException("Null information on either side of . operator"); }
            
            //B.Args.Insert(0, Leftside);
            return B.Invoke();
        }
    }
}
