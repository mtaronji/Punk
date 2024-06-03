using MathNet.Numerics.Distributions;
using Punk;
using System.Dynamic;

namespace Punk.TypeNodes
{
    public class ProbabilityNode : TreeNode
    {
        public dynamic? Distribution { get; private set; }
        public DistributionType distributiontype { get;private set; }
        public DistributionIntervalType distributionIntervalType { get; private set; }
        public ProbabilityNode(DistributionType d)
        {
            this.distributiontype = d;
            if(d == DistributionType.ContinuousUniform ||
                d == DistributionType.Normal ||
                 d == DistributionType.LogNormal ||
                  d == DistributionType.Beta ||
                   d == DistributionType.Cauchy ||
                    d == DistributionType.Chi ||
                     d == DistributionType.ChiSquared ||
                      d == DistributionType.Erlang ||
                       d == DistributionType.Exponential ||
                        d == DistributionType.FDistribution ||
                         d == DistributionType.Gamma ||
                          d == DistributionType.InverseGamma ||
                           d == DistributionType.Laplace ||
                            d == DistributionType.Pareto ||
                             d == DistributionType.Rayleigh ||
                              d == DistributionType.Stable ||
                               d == DistributionType.StudentT ||
                                d == DistributionType.Weibull ||
                                 d == DistributionType.Triangular
                                 )
            {
                this.distributionIntervalType = DistributionIntervalType.Continous;
            }
            else
            {
                this.distributionIntervalType = DistributionIntervalType.Discrete;
            }

            //if a normal distribution is provided with no args we will give a default 0 mean 1 variance version
            if(distributiontype == DistributionType.Normal)
            {
                this.Distribution = new Normal();
            }

        }
        public override TreeNode Eval()
        {
            return this;
        }

        public override string Print()
        {
            return $"";
        }

        public void SetArgs(IEnumerable<TreeNode> Args)
        {
            
            if (Args.Count() == 1)
            {
                NumberNode? arg1;
                arg1 = Args.ElementAt(0) as NumberNode;
                if (arg1 == null) { throw new Exceptions.PunkArgumentException("Some arguments are Empty"); }
                dynamic argv1;
                argv1 = arg1.NumberTypeValue.NumberValue;

                if (this.distributiontype == DistributionType.Chi) { this.Distribution = new Chi(argv1); }
                if (this.distributiontype == DistributionType.ChiSquared) { this.Distribution = new ChiSquared(argv1); }
                if (this.distributiontype == DistributionType.Exponential) { this.Distribution = new Exponential(argv1); }
                if (this.distributiontype == DistributionType.Categorical) { this.Distribution = new Categorical(argv1); }
                if (this.distributiontype == DistributionType.Poisson) { this.Distribution = new Poisson(argv1); }
                if (this.distributiontype == DistributionType.Geometric) { this.Distribution = new Geometric(argv1); }
                if (this.distributiontype == DistributionType.Bernoulli) { this.Distribution = new Bernoulli(argv1); }
                if (this.distributiontype == DistributionType.Rayleigh) { this.Distribution = new Rayleigh(argv1); }
            }

            if (Args.Count() == 2)
            {
                NumberNode? arg1, arg2;
                arg1 = Args.ElementAt(0) as NumberNode; arg2 = Args.ElementAt(1) as NumberNode;
                if (arg1 == null || arg2 == null) { throw new Exceptions.PunkArgumentException("Some arguments are Empty"); }
                dynamic argv1, argv2; 
                argv1 = arg1.NumberTypeValue.NumberValue; argv2 = arg2.NumberTypeValue.NumberValue;
 
                if (this.distributiontype == DistributionType.ContinuousUniform) { this.Distribution = new ContinuousUniform(argv1,argv2); }
                if (this.distributiontype == DistributionType.Normal) { this.Distribution = new Normal(argv1, argv2); }
                if (this.distributiontype == DistributionType.LogNormal) { this.Distribution = new LogNormal(argv1, argv2); }
                if (this.distributiontype == DistributionType.Beta) { this.Distribution = new Beta(argv1, argv2); }
                if (this.distributiontype == DistributionType.Cauchy) { this.Distribution = new Cauchy(argv1, argv2); }
                if (this.distributiontype == DistributionType.Erlang) { this.Distribution = new Erlang((int)argv1, argv2); }
                if (this.distributiontype == DistributionType.FDistribution) { this.Distribution = new FisherSnedecor(argv1, argv2); }
                if (this.distributiontype == DistributionType.Gamma) { this.Distribution = new Gamma(argv1, argv2); }
                if (this.distributiontype == DistributionType.InverseGamma) { this.Distribution = new InverseGamma(argv1, argv2); }
                if (this.distributiontype == DistributionType.Laplace) { this.Distribution = new Laplace(argv1, argv2); }
                if (this.distributiontype == DistributionType.Pareto) { this.Distribution = new Pareto(argv1, argv2); }
                if (this.distributiontype == DistributionType.Weibull) { this.Distribution = new Weibull(argv1, argv2); }
                if (this.distributiontype == DistributionType.ConwayMaxwellPoisson) { this.Distribution = new ConwayMaxwellPoisson(argv1, argv2); }
                if (this.distributiontype == DistributionType.Zipf) { this.Distribution = new Zipf(argv1, (int)argv2); }
                if (this.distributiontype == DistributionType.Binomial) { this.Distribution = new Binomial(argv1, (int)argv2); }
                if (this.distributiontype == DistributionType.NegativeBinomial) { this.Distribution = new NegativeBinomial(argv1, argv2); }
                if (this.distributiontype == DistributionType.DiscreteUniform) { this.Distribution = new DiscreteUniform((int)argv1, (int)argv2); }
            }
            if (Args.Count() == 3)
            {
                NumberNode? arg1, arg2, arg3;
                arg1 = Args.ElementAt(0) as NumberNode; arg2 = Args.ElementAt(1) as NumberNode; arg3 = Args.ElementAt(2) as NumberNode; 
                if (arg1 == null || arg2 == null || arg3 == null) { throw new Exceptions.PunkArgumentException("Some arguments are Empty"); }
                dynamic argv1, argv2, argv3; argv1 = arg1.NumberTypeValue.NumberValue; argv2 = arg2.NumberTypeValue.NumberValue;
                argv3 = arg3.NumberTypeValue.NumberValue; 
                if (this.distributiontype == DistributionType.StudentT) { this.Distribution = new StudentT(argv1,argv2,argv3); }
                if (this.distributiontype == DistributionType.Triangular) { this.Distribution = new Triangular(argv1,argv2,argv3); }
                if (this.distributiontype == DistributionType.Hypergeometric) { this.Distribution = new Hypergeometric((int)argv1, (int)argv2, (int)argv3); }
            }

            if (Args.Count() == 4)
            {
                NumberNode? arg1, arg2, arg3, arg4;
                arg1 = Args.ElementAt(0) as NumberNode; arg2 = Args.ElementAt(1) as NumberNode; arg3 = Args.ElementAt(2) as NumberNode; arg4 = Args.ElementAt(3) as NumberNode;
                if (arg1 == null ||  arg2 == null || arg3 == null || arg4 == null){ throw new Exceptions.PunkArgumentException("Some arguments are Empty"); }
                dynamic argv1, argv2, argv3, argv4; argv1 = arg1.NumberTypeValue.NumberValue; argv2 = arg2.NumberTypeValue.NumberValue;
                argv3 = arg3.NumberTypeValue.NumberValue; argv4 = arg4.NumberTypeValue.NumberValue;
                if (this.distributiontype == DistributionType.Stable) { this.Distribution = new Stable(argv1,argv2,argv3,argv4); }
            }       
                
        }

    }

    public enum DistributionType
    {
        ContinuousUniform,
        Normal,
        LogNormal,
        Beta,
        Cauchy,
        Chi,
        ChiSquared,
        Erlang,
        Exponential,
        FDistribution,
        Gamma,
        InverseGamma,
        Laplace,
        Pareto,
        Rayleigh,
        Stable,
        StudentT,
        Weibull,
        Triangular,
        DiscreteUniform,
        Bernoulli,
        Binomial,
        NegativeBinomial,
        Geometric,
        Hypergeometric,
        Poisson,
        Categorical,
        ConwayMaxwellPoisson,
        Zipf
    }
    public enum DistributionIntervalType
    {
        Continous,
        Discrete
    }

}
