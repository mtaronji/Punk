using MathNet.Numerics.Distributions;
using Punk;
using System.Dynamic;

namespace Punk.TypeNodes
{
    public class ProbabilityNode : TreeNode, IArgumentsNode
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

        }
        public override TreeNode Eval()
        {
            return this;
        }

        public override string Print()
        {
            return $"";
        }

        public void SetArgument(ArgumentsNode node)
        {
            //continuous
            dynamic? arg1 = null, arg2 = null, arg3 = null;
            NumberNode? node1; if(node.Arguments.Count > 0) { node1 = node.Arguments[0] as NumberNode; if (node1 != null) { arg1 = node1.Value.Value; } }
            NumberNode ? node2; if (node.Arguments.Count > 1) { node2 = node.Arguments[1] as NumberNode; if (node2 != null) { arg2 = node2.Value.Value; } }
            NumberNode ? node3; if (node.Arguments.Count > 2) { node3 = node.Arguments[2] as NumberNode; if (node3 != null) { arg3 = node3.Value.Value; } }



            if (this.distributiontype == DistributionType.ContinuousUniform) { this.Distribution = new ContinuousUniform(arg1, arg2); }
            if (this.distributiontype == DistributionType.Normal) { this.Distribution = new Normal(arg1, arg2); }
            if (this.distributiontype == DistributionType.LogNormal) { this.Distribution = new LogNormal(arg1, arg2); }
            if (this.distributiontype == DistributionType.Beta) { this.Distribution = new Beta(arg1, arg2); }
            if (this.distributiontype == DistributionType.Cauchy) { this.Distribution = new Cauchy(arg1, arg2); }
            if (this.distributiontype == DistributionType.Chi) { this.Distribution = new Chi(arg1); }
            if (this.distributiontype == DistributionType.ChiSquared) { this.Distribution = new ChiSquared(arg1); }
            if (this.distributiontype == DistributionType.Erlang) { this.Distribution = new Erlang((int)arg1, arg2); }
            if (this.distributiontype == DistributionType.Exponential) { this.Distribution = new Exponential(arg1); }
            if (this.distributiontype == DistributionType.FDistribution) { this.Distribution = new FisherSnedecor(arg1, arg2); }
            if (this.distributiontype == DistributionType.Gamma) { this.Distribution = new Gamma(arg1, arg2); }
            if (this.distributiontype == DistributionType.InverseGamma) { this.Distribution = new InverseGamma(arg1, arg2); }
            if (this.distributiontype == DistributionType.Laplace) { this.Distribution = new Laplace(arg1, arg2); }
            if (this.distributiontype == DistributionType.Pareto) { this.Distribution = new Pareto(arg1, arg2); }
            if (this.distributiontype == DistributionType.Rayleigh) { this.Distribution = new Rayleigh(arg1); }
            if (this.distributiontype == DistributionType.Stable) { this.Distribution = new Stable(arg1, arg2, arg3, arg3); }
            if (this.distributiontype == DistributionType.StudentT) { this.Distribution = new StudentT(arg1, arg2, arg3); }
            if (this.distributiontype == DistributionType.Weibull) { this.Distribution = new Weibull(arg1, arg2); }
            if (this.distributiontype == DistributionType.Triangular) { this.Distribution = new Triangular(arg1, arg2, arg3); }

            // discrete
            if (this.distributiontype == DistributionType.DiscreteUniform) { this.Distribution = new DiscreteUniform((int)arg1, (int)arg2); }
            if (this.distributiontype == DistributionType.Bernoulli) { this.Distribution = new Bernoulli(arg1); }
            if (this.distributiontype == DistributionType.Binomial) { this.Distribution = new Binomial(arg1, (int)arg2); }
            if (this.distributiontype == DistributionType.NegativeBinomial) { this.Distribution = new NegativeBinomial(arg1, arg2); }
            if (this.distributiontype == DistributionType.Geometric) { this.Distribution = new Geometric(arg1); }
            if (this.distributiontype == DistributionType.Hypergeometric) { this.Distribution = new Hypergeometric((int)arg1, (int)arg2, (int)arg3); }
            if (this.distributiontype == DistributionType.Poisson) { this.Distribution = new Poisson(arg1); }
            if (this.distributiontype == DistributionType.Categorical) { this.Distribution = new Categorical(arg1); }
            if (this.distributiontype == DistributionType.ConwayMaxwellPoisson) { this.Distribution = new ConwayMaxwellPoisson(arg1, arg2); }
            if (this.distributiontype == DistributionType.Zipf) { this.Distribution = new Zipf(arg1, (int)arg2); }

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
