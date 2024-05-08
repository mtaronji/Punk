﻿using MathNet.Numerics.Distributions;
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
            if(B.token == null) { throw new Exceptions.PunkInstanceMethodException("Null information on either side of . operator"); }
            switch (B.FNId.ToLower())
            {
                case ("transpose"):
                    return new MatrixNode(new MatrixType(A.matrix.Value.Transpose()) );
                case ("inverse"):
                    return new MatrixNode(new MatrixType(A.matrix.Value.Inverse()));
                case ("determinant"):
                    return new NumberNode(new NumberType(A.matrix.Value.Determinant()));           
                case ("kernel"):
                    var kernelvalue = Matrix<double>.Build.DenseOfColumnVectors(A.matrix.Value.Kernel());
                    return new MatrixNode(new MatrixType(kernelvalue));
                case ("column"):
                    if(B.Args == null) { throw new Exceptions.PunkInstanceMethodException("Column requires 1 argument"); }
                    if(B.Args.Length != 1) { throw new Exceptions.PunkInstanceMethodException("Column requires 1 argument"); }
                    if(!int.TryParse(B.Args[0], out var index)) { throw new Exceptions.PunkInstanceMethodException("Column Argument Should be an integer"); }

                    var columnmatrix =  Matrix<double>.Build.DenseOfColumnVectors(A.matrix.Value.Column(index));
                    return new MatrixNode(new MatrixType(columnmatrix));
                case ("transposethismultiply"):
                    if (B.Args == null) { throw new Exceptions.PunkInstanceMethodException("Transpose and Multiply missing multiplication argument"); }
                    if (B.Args.Length != 1) { throw new Exceptions.PunkInstanceMethodException("Transpose and Multiply missing multiplication argument"); }
                    if (!B.ParserIdentifiers.ContainsKey(B.Args[0])) { throw new Exceptions.PunkInstanceMethodException($"Matrix {B.Args[0]} does not exist"); }
                    if (!(B.ParserIdentifiers[B.Args[0]].Value is MatrixNode)) { throw new Exceptions.PunkInstanceMethodException($"Identifier is not a matrix"); }
                    var matrixnode = B.ParserIdentifiers[B.Args[0]].Value as MatrixNode;
                    if(matrixnode == null) { throw new Exceptions.PunkInstanceMethodException($"Matrix Argument Value is empty for identifier"); }
                    return new MatrixNode(new MatrixType(A.matrix.Value.TransposeThisAndMultiply(matrixnode.matrix.Value)));
                case ("transposemultiply"):
                    if (B.Args == null) { throw new Exceptions.PunkInstanceMethodException("Transpose and Multiply missing multiplication argument"); }
                    if (B.Args.Length != 1) { throw new Exceptions.PunkInstanceMethodException("Transpose and Multiply missing multiplication argument"); }
                    if (!B.ParserIdentifiers.ContainsKey(B.Args[0])) { throw new Exceptions.PunkInstanceMethodException($"Matrix {B.Args[0]} does not exist"); }
                    if (!(B.ParserIdentifiers[B.Args[0]].Value is MatrixNode)) { throw new Exceptions.PunkInstanceMethodException($"Identifier is not a matrix"); }
                    matrixnode = B.ParserIdentifiers[B.Args[0]].Value as MatrixNode;
                    if (matrixnode == null) { throw new Exceptions.PunkInstanceMethodException($"Matrix Argument Value is empty for identifier"); }
                    return new MatrixNode(new MatrixType(A.matrix.Value.TransposeAndMultiply(matrixnode.matrix.Value)));
                case ("choleskysolve"):
                    if (B.Args == null) { throw new Exceptions.PunkInstanceMethodException("Cholesky missing solve variable"); }
                    if (B.Args.Length != 1) { throw new Exceptions.PunkInstanceMethodException("Cholesky missing solve variable"); }
                    if (!B.ParserIdentifiers.ContainsKey(B.Args[0])) { throw new Exceptions.PunkInstanceMethodException($"Cholesky Solve for variable {B.Args[0]} does not exist"); }
                    if (!(B.ParserIdentifiers[B.Args[0]].Value is MatrixNode)) { throw new Exceptions.PunkInstanceMethodException($"Identifier is not a matrix"); }
                    matrixnode = B.ParserIdentifiers[B.Args[0]].Value as MatrixNode;
                    if (matrixnode == null) { throw new Exceptions.PunkInstanceMethodException($"Matrix Argument is empty for identifier"); }
                    var result = A.matrix.Value.Cholesky().Solve(matrixnode.matrix.Value);        
                    return new MatrixNode(new MatrixType(result));

                default:
                    throw new Exceptions.PunkInstanceMethodException("That matrix Instance function seems to not Exist");
            }
        }
        TreeNode ProbabilityInstanceFnFactory(ProbabilityNode A, FnNode B)
        {
            if (B.token == null) { throw new Exceptions.PunkInstanceMethodException("Null information on either side of . operator"); }
         
            
            if(A.distributionIntervalType == DistributionIntervalType.Discrete)
            {
                switch (B.FNId.ToLower())
                {
                    case ("cdf"):
                        if (B.Args == null) { throw new Exceptions.PunkInstanceMethodException("Expecting 1 input for CumulativeDistribution"); }
                        if (B.Args.Length != 1) { throw new Exceptions.PunkInstanceMethodException("Expecting 1 input for CumulativeDistribution"); }
                        if (A.Distribution == null) { throw new Exceptions.PunkInstanceMethodException("Probability Distribution Empty"); }
                        if (double.TryParse(B.Args[0], out var doubleval)) { return new NumberNode(A.Distribution.CumulativeDistribution(doubleval)); }
                        else { throw new Exception("Argument for cdf must be a real number"); } 
                    case ("probability"):
                        if (B.Args == null) { throw new Exceptions.PunkInstanceMethodException("Expecting 1 input for CumulativeDistribution"); }
                        if (B.Args.Length != 1) { throw new Exceptions.PunkInstanceMethodException("Expecting 1 input for CumulativeDistribution"); }
                        if (A.Distribution == null) { throw new Exceptions.PunkInstanceMethodException("Probability Distribution Empty"); }
                        if (int.TryParse(B.Args[0], out var intval)) { return new NumberNode(A.Distribution.Probability(intval)); }
                        else { throw new Exception("Argument for probability must be an integer"); }
                    default:
                        throw new NotImplementedException("Probability Instance Function does not exist");
                }
            }
            if (A.distributionIntervalType == DistributionIntervalType.Continous)
            {
                switch (B.FNId.ToLower())
                {
                    case ("cdf"):
                        if (B.Args == null) { throw new Exceptions.PunkInstanceMethodException("Expecting 1 input for CumulativeDistribution"); }
                        if (B.Args.Length != 1) { throw new Exceptions.PunkInstanceMethodException("Expecting 1 input for CumulativeDistribution"); }
                        if (A.Distribution == null) { throw new Exceptions.PunkInstanceMethodException("Probability Distribution Empty"); }
                        if (double.TryParse(B.Args[0], out var doubleval)) { return new NumberNode(A.Distribution.CumulativeDistribution(doubleval)); }
                        else { throw new Exception("Argument for cdf must be a real number"); }
                    default:
                        throw new NotImplementedException("Probability Instance Function does not exist");
                }
            }

            throw new NotImplementedException("Probability Distribution interval type not implemented");


        }
        TreeNode DataInstanceFnFactory(DataNode A, FnNode B)
        {           
            throw new NotImplementedException($"No Data instance function for Data type");          
        }
        //TreeNode SequenceInstanceFnFactory(SequenceNode A, IdentifierNode B)
        //{      
        //    switch (B.token.Value.ToLower())
        //    {
        //        case ("SimpsonRuleIntegrateComposite"):
        //            if(A.sequence.Dimension == 1) { }
        //            else if(A.sequence.Dimension == 2) { }
        //            else { }
        //            break;
                
        //        case ("NewtonCotesTrapeziumRuleIntegrateAdaptive"):
        //            break;

        //        case ("DoubleExponentialTransformationIntegrate"):
        //            break;
        //        case ("GaussLegendreRuleIntegrate"):

        //        default:
        //            return null;
        //    }

            
        //}

    }
}
