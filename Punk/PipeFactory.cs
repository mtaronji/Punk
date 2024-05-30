using Punk.UnaryOperators;
using Punk.Exceptions;
using Punk.TypeNodes;
using MathNet.Numerics.Distributions;

namespace Punk
{
    public class PipeFactory
    {
        public PipeFactory()
        {

        }

        public TreeNode Combine(TreeNode A, TreeNode B )
        {
            //this node should always be first
            if(B is IdentifierNode)
            {
                IdentifierNode idnode = (IdentifierNode)B;
                if(idnode == null || idnode.Value == null) { throw new Exceptions.PunkPipeException("Identifier node is not pointing to a type"); }
                B = idnode.Value;
            }

            if (B is MatrixNode)
            {
                return MatrixPipeFactory(A, (MatrixNode)B);
            }
            else if (B is RegisterNode)
            {
                return RegisterPipeFactory(A,(RegisterNode)B);
            }
            else if (B is DataNode)
            {
                return DataPipeFactory(A, (DataNode)B);
            }
            else if (B is PlotNode)
            {
                return PlotPipeFactory(A, (PlotNode)B);
            }
            else if (B is ProbabilityNode)
            {
                return ProbabilityPipeFactory(A, (ProbabilityNode)B);
            }
            else if (B is SequenceNode)
            {
                return SequencePipeFactory(A, (SequenceNode)B);
            }
 
            else
            {
                throw new PunkPipeException($"Unable to establish a pipe to to node");
            }        

        }

        TreeNode MatrixPipeFactory(TreeNode A, MatrixNode B)
        {
            throw new NotImplementedException();
        }
        TreeNode DataPipeFactory(TreeNode A, DataNode B)
        {
            
            throw new Exceptions.PunkPipeException($"Unable to establish a pipe to node");
            
        }
        TreeNode RegisterPipeFactory(TreeNode A, RegisterNode B)
        {
            //if(A is StringNode)
            //{
            //    var a = (StringNode)A;
            //    B.SetRegisterItem(a);
            //}
            //else
            //{
            //    throw new NotImplementedException("Only Except string pipes into register");
            //}
            throw new NotImplementedException("Only Except string pipes into register");
            //return B;         
        }

        TreeNode SequencePipeFactory(TreeNode A, SequenceNode B)
        {
            if (A is SequenceNode)
            {
                var sequencenode = (SequenceNode)A;
                var func = sequencenode.sequence.SequenceTransformation;
                var val = B.sequence.SequenceTransformation(func);
                if(val is Double ||  val is long || val is float || val is int)
                {
                    return new NumberNode(val);
                }
                else
                {
                    throw new NotImplementedException("No Value for ");
                }

            }
            else
            {
                throw new NotImplementedException("Pipe operator not applicable for type");
            }
        }
        TreeNode ProbabilityPipeFactory(TreeNode A, ProbabilityNode B)
        {
            if (A is DataNode)
            {
                DataNode data = (DataNode)A;
                if(data.Value.GetDimension() != 1)
                {
                    throw new NotImplementedException("Only Accept 1 dimensional array for pipes into Probability Distributions");
                }
                if(B.Distribution == null) { throw new Exceptions.PunkPipeException("Probability distribution is Empty"); }
                for(int i = 0; i < data.Value.DataVectors[0].Count; i++)
                {
                    data.Value.DataVectors[0][i] = B.Distribution.Sample();
                }
                return A;
                
            }
            else
            {
                throw new NotImplementedException("Pipes to Probability Nodes only accept a Data type");
            }
        }
        TreeNode PlotPipeFactory(TreeNode A, PlotNode B)
        {
            if(A is DataNode)
            {
                var a = (DataNode)A;
                B.SetData(a.GetData());
            }
            else if(A is QueryNode)
            {
                var a = (QueryNode)A;
                B.SetData(a.query.EvaulatedQuery);
            }
           
            else
            {
                throw new NotImplementedException("input to plot must be data or a register/register item");
            }
            return B;
        }

    }
}
