using Punk.BinaryOperators;
using Punk.TypeNodes;
using Punk.UnaryOperators;
using Punk.Types;
using Punk;
using System.Text.RegularExpressions;

namespace Punk
{
    //create the parse tree
    public class Parser
    {
        private void InitDiscreteProbabilityIdentifiers()
        {
            var discreteuniform = new ProbabilityNode(DistributionType.DiscreteUniform); this.Keywords["discreteuniform"] = new IdentifierNode("discreteuniform", discreteuniform);
            var bernouli = new ProbabilityNode(DistributionType.Bernoulli); this.Keywords["bernouli"] = new IdentifierNode("bernouli", bernouli);
            var binomial = new ProbabilityNode(DistributionType.Binomial); this.Keywords["binomial"] = new IdentifierNode("binomial", binomial);
            var negativebinomial = new ProbabilityNode(DistributionType.NegativeBinomial); this.Keywords["negativebinomial"] = new IdentifierNode("negativebinomial", negativebinomial);
            var geometric = new ProbabilityNode(DistributionType.Geometric); this.Keywords["geometric"] = new IdentifierNode("geometric", geometric);
            var hypergeometric = new ProbabilityNode(DistributionType.Hypergeometric); this.Keywords["hypergeometric"] = new IdentifierNode("hypergeometric", hypergeometric);
            var poisson = new ProbabilityNode(DistributionType.Poisson); this.Keywords["poisson"] = new IdentifierNode("poisson", poisson);
            var cateogorical = new ProbabilityNode(DistributionType.Categorical); this.Keywords["cateogorical"] = new IdentifierNode("cateogorical", cateogorical);
            var conwaympoisson = new ProbabilityNode(DistributionType.ConwayMaxwellPoisson); this.Keywords["conwaympoisson"] = new IdentifierNode("conwaympoisson", conwaympoisson);
            var zipf = new ProbabilityNode(DistributionType.Zipf); this.Keywords["zipf"] = new IdentifierNode("zipf", zipf);
        }

        private void InitContinuousProbabilityIdentifiers()
        {
            var continuousuniform = new ProbabilityNode(DistributionType.ContinuousUniform); this.Keywords["continuousuniform"] = new IdentifierNode("continuousuniform", continuousuniform);
            var normal = new ProbabilityNode(DistributionType.Normal); this.Keywords["normal"] = new IdentifierNode("normal", normal);
            var lognormal = new ProbabilityNode(DistributionType.LogNormal); this.Keywords["lognormal"] = new IdentifierNode("lognormal", lognormal);
            var beta = new ProbabilityNode(DistributionType.Beta); this.Keywords["beta"] = new IdentifierNode("beta", beta);
            var cauchy = new ProbabilityNode(DistributionType.Cauchy); this.Keywords["cauchy"] = new IdentifierNode("cauchy", cauchy);
            var chi = new ProbabilityNode(DistributionType.Chi); this.Keywords["chi"] = new IdentifierNode("chi", chi);
            var erlang = new ProbabilityNode(DistributionType.Erlang); this.Keywords["erlang"] = new IdentifierNode("erlang", erlang);
            var fdistribution = new ProbabilityNode(DistributionType.FDistribution); this.Keywords["fdistribution"] = new IdentifierNode("fdistribution", fdistribution);
            var gamma = new ProbabilityNode(DistributionType.Gamma); this.Keywords["gamma"] = new IdentifierNode("gamma", gamma);
            var inversegamma = new ProbabilityNode(DistributionType.InverseGamma); this.Keywords["inversegamma"] = new IdentifierNode("inversegamma", inversegamma);
            var laplace = new ProbabilityNode(DistributionType.Laplace); this.Keywords["laplace"] = new IdentifierNode("laplace", laplace);
            var pareto = new ProbabilityNode(DistributionType.Pareto); this.Keywords["pareto"] = new IdentifierNode("pareto", pareto);
            var rayleigh = new ProbabilityNode(DistributionType.Rayleigh); this.Keywords["rayleigh"] = new IdentifierNode("rayleigh", rayleigh);
            var stable = new ProbabilityNode(DistributionType.Stable); this.Keywords["stable"] = new IdentifierNode("stable", stable);
            var tstudent = new ProbabilityNode(DistributionType.StudentT); this.Keywords["tstudent"] = new IdentifierNode("tstudent", tstudent);
            var weibull = new ProbabilityNode(DistributionType.Weibull); this.Keywords["weibull"] = new IdentifierNode("weibull", weibull);
            var triangle = new ProbabilityNode(DistributionType.Triangular); this.Keywords["triangle"] = new IdentifierNode("triangle", triangle);
        }
        public Dictionary<string,IdentifierNode> Identifiers { get; set; }
        public Dictionary<string, IdentifierNode> Keywords { get; set; }


        public Dictionary<string, IdentifierNode> csvfiles { get; set; }

        private Stack<Token> _stack;
        private List<TreeNode>? ParseExpressions;

     
        public Parser()
        {

            this.Keywords = new();
            InitContinuousProbabilityIdentifiers();
            InitDiscreteProbabilityIdentifiers();
            this._stack = new Stack<Token>();
            Identifiers = new Dictionary<string,IdentifierNode>();
            csvfiles = new Dictionary<string, IdentifierNode>();
        }
        public Parser(Dictionary<string, IdentifierNode> files)
        {
            this.Keywords = new();
            InitContinuousProbabilityIdentifiers();
            InitDiscreteProbabilityIdentifiers();
            //add special identifiers
            this._stack = new Stack<Token>();
            Identifiers = new Dictionary<string, IdentifierNode>();
            csvfiles = files;
        }

        public async Task<List<TreeNode>> ParseAsync(Token[] Lexicon)
        {
            this._stack = new Stack<Token>(Lexicon.Reverse());
            this.ParseExpressions = new List<TreeNode>();
            while(this._stack.Count > 0)
            {
                var e = await ParseExpressionAsync();
                if(e == null) { throw new Exceptions.PunkExpressionParseException("Expression parse result null. Error"); }
                ParseExpressions.Add(e);                         
            }
            return ParseExpressions;
        }
        public async Task<TreeNode?> ParseExpressionAsync()
        {
            var a = await ParseTermAsync();
            if(a == null) { return null; }

            while (true)
            {
                if (this._stack.Count == 0)
                {
                    return a;
                }
                Token nextToken = _stack.Peek();

                if (nextToken.TokenType == TokenType.AddType)
                {
                    _stack.Pop();
                    var b = await ParseTermAsync();
                    if (b == null) { throw new Exceptions.PunkAdditionException("Right side of Addition has wrong syntax"); }
                    a = new AdditionNode(a, b);
                }
                else if (nextToken.TokenType == TokenType.SubtractType)
                {
                    _stack.Pop();
                    var b = await ParseTermAsync();
                    if (b == null) { throw new Exceptions.PunkAdditionException("Right side of Subtraction has wrong syntax"); }
                    //if (!(a is IdentifierNode || a is NumberNode)) { return null; }
                    a = new SubstractNode(a, b);
                }

                else if (nextToken.TokenType == TokenType.AssignType)
                {
                    if(!(a is IdentifierNode)) {throw new Exceptions.PunkAssignmentException("Assignments can only be done on Identifiers"); }
                    _stack.Pop();

                    //make sure identifier isn't a keyword


                    var b = await ParseExpressionAsync();
                    if (b == null) { throw new Exceptions.PunkAssignmentException("Incorrect Assignment syntax"); }
                    //if (b is PipeNode) { throw new Exceptions.PunkAssignmentException("You cannot assign an identifier to a pipe sequence"); }

                    //set the value in the identifier and create an assignment node
                    IdentifierNode i = (IdentifierNode)a;
                    i.Value = b;
                    if(i.token == null) { throw new Exceptions.PunkAssignmentException("Identifier missing"); }
                    this.Identifiers[i.token.Value] = i;
                    a = new AssignmentNode(i, b);               
                }
                else
                {
                    return a;
                }
            }
        }

        public async Task<TreeNode?> ParseTermAsync()
        {
            var a = await ParsePowerFactorAsync();
            if(a == null) { return a; }
            while (true)
            {
                if (this._stack.Count == 0)
                {
                    return a;
                }

                Token? nextToken = null;
                nextToken = _stack.Peek();

                if (nextToken.TokenType == TokenType.MultiplicationType)
                {
                    _stack.Pop();
                    var b = await ParsePowerFactorAsync();
                    if (b == null) { throw new Exceptions.PunkMulitiplicationException("Right side of Multiplication has wrong syntax"); }
                    //if (!(a is IdentifierNode || a is NumberNode)) { return null; }
                    a = new MultiplicationNode(a, b);
                }

                else if (nextToken.TokenType == TokenType.PointwiseMultiplicationType)
                {
                    _stack.Pop();
                    var b = await ParsePowerFactorAsync();
                    if (b == null) { throw new Exceptions.PunkMulitiplicationException("Right side of Multiplication has wrong syntax"); }
                    //if (!(a is IdentifierNode || a is NumberNode)) { return null; }
                    a = new PointWiseMultiplicationNode(a, b);
                }
                else if (nextToken.TokenType == TokenType.DivideType)
                {
                    _stack.Pop();
                    var b = await ParsePowerFactorAsync();
                    if (b == null) { throw new Exceptions.PunkDivisionException("Right side of Division has wrong syntax"); }
                    //if (!(a is IdentifierNode || a is NumberNode)) { return null; }
                    a =  new DivisionNode(a, b);
                }
                else if (nextToken.TokenType == TokenType.ModuloType)
                {
                    _stack.Pop();
                    var b = await ParsePowerFactorAsync();
                    if (b == null) { throw new Exceptions.PunkModuloException("Right side of Modulo has wrong syntax"); }
                    a = new ModuloNode(a, b);
                }
                else if (nextToken.TokenType == TokenType.PipeType)
                {
                    //this operator works sideways of the usual. Switch operator
                    _stack.Pop();
                    var b = await ParsePowerFactorAsync();
                    if (b == null) { throw new Exceptions.PunkPipeException("Input pipe is missing"); }
                    a = new PipeNode(a,b);
                                 
                }
             

                else
                {
                    return a;
                }
            }
        }
        public  async Task<TreeNode?> ParsePowerFactorAsync()
        {
            var a = await ParseFactorAsync();
            if (a == null) { return null; }
            while (true)
            {
                if (this._stack.Count == 0)
                {
                    return a;
                }
                Token? nextToken = null;

                nextToken = _stack.Peek();

                if (nextToken.TokenType == TokenType.ExponentialType)
                {
                    this._stack.Pop();
                    var b = await ParseFactorAsync(); if(b == null) { throw new Exceptions.PunkSyntaxErrorException("Unable to find argument for exponential operation"); }
                    a = new ExponentialNode(a, b);

                }
                else if (nextToken.TokenType == TokenType.SequenceType)
                {
                    this._stack.Pop();
                    SequenceNode b = new SequenceNode(a, nextToken.Value);
                    await b.sequence.CreateDelegateAsync();
                    a = b;
                }
                else if (nextToken.TokenType == TokenType.QueryType)
                {
                    this._stack.Pop();
                    if(!(a is RegisterNode)){ throw new Exceptions.PunkQueryException("Query must be on a registers"); }

                    QueryNode b = new QueryNode((RegisterNode)a, nextToken.Value);
                    await b.query.EvaluateQueryAsync();
                    a = b;
                }
                else if (nextToken.TokenType == TokenType.PeriodType)
                {
                    //this operator works sideways of the usual. Switch operator
                    _stack.Pop();
                    var b = await ParseFactorAsync();
                    var fnnode = b as FnNode; if(fnnode == null) { throw new Exceptions.PunkInstanceMethodException("Argument after '.' should be an Instance Function. Please check syntax"); }
                    a = new InstanceFnNode(a, fnnode);

                }
                else
                {
                    return a;
                }
            }
        }
        
        public async Task<TreeNode?> ParseFactorAsync()
        {
            if (this._stack.Count == 0)
            {
                return null;
            }
            Token? nextToken = _stack.Peek();

            if (nextToken.TokenType == TokenType.DataType)
            {
                this._stack.Pop();
                return new DataNode(nextToken);

            }
            else if (nextToken.TokenType == TokenType.MatrixType)
            {
                this._stack.Pop();
                var matrix = new MatrixType(nextToken.Value);
                return new MatrixNode(matrix);
            }
            else if (nextToken.TokenType == TokenType.NumberType)
            {
                this._stack.Pop();
                return new NumberNode(nextToken);
            }
            else if (nextToken.TokenType == TokenType.StringType)
            {
                this._stack.Pop();
                return new StringNode(nextToken.Value);
            }
            else if (nextToken.TokenType == TokenType.FunctionType)
            {
                this._stack.Pop();
                return new FnNode(nextToken, this.Identifiers);
            }
            else if (nextToken.TokenType == TokenType.RegisterType)
            {
                this._stack.Pop();
                return new RegisterNode(nextToken.Value);
            }
            else if (nextToken.TokenType == TokenType.IdentityfierType)
            {              
                this._stack.Pop();
                if (this.Identifiers.ContainsKey(nextToken.Value))
                {
                    return Identifiers[nextToken.Value];
                }
                else if (this.Keywords.ContainsKey(nextToken.Value))
                {
                    string keyword = nextToken.Value;
                    if (this._stack.Count > 0)
                    {
                        nextToken = _stack.Peek();
                        if (nextToken.TokenType == TokenType.LParenthesisType)
                        {
                            this._stack.Pop();
                            var argsnode = await ParseArgsAsync();
                            if(argsnode != null){argsnode.Bottom = Keywords[keyword]; }
                            else { return null; }
                            return argsnode;
                        }
                    }
                    return this.Keywords[keyword];                  
                }
                else
                {
                    if (this._stack.Count > 0)
                    {
                        if (this._stack.Peek().TokenType != TokenType.AssignType)
                        {
                            throw new Exceptions.PunkIdentifierUninitializedException($"{nextToken.Value} is not assigned");
                        }
                    }
                    return new IdentifierNode(nextToken);                   
                }                     
            }
            else if (nextToken.TokenType == TokenType.PlotType)
            {
                this._stack.Pop();
                return new PlotNode();
            }
          
            else if (nextToken.TokenType == TokenType.SubtractType)
            {
                this._stack.Pop();
                var a = await ParseTermAsync();
                if(a == null) { throw new Exceptions.PunkSubtractionException("incorrect argument for subtraction operator"); }
                return new NegateNode(a);
            }

            else if (nextToken.TokenType == TokenType.LParenthesisType)
            {
                this._stack.Pop();
                var a = await ParseExpressionAsync();
                if(this._stack.Count > 0 && this._stack.Peek().TokenType == TokenType.RParenthesisType)
                {
                    this._stack.Pop();
                    return a;
                }
                else
                {
                    throw new Exceptions.PunkParenthesisException("Missing Matching Parenthesis");                  
                }
            }
 
            else
            {
                if(nextToken.TokenType == TokenType.RParenthesisType)
                {
                    throw new   Exceptions.PunkParenthesisException("Missing Matching Parenthesis");
                }
                else
                {
                    throw new Exceptions.PunkUnknownCharactersException("Unknown Character in Syntax");
                }
            }
        }

        public async Task<ArgumentsNode?> ParseArgsAsync()
        {
            if (this._stack.Count == 0)
            {
                return null;
            }
            ArgumentsNode argnode = new ArgumentsNode();
            while (true)
            {
                var node = await ParseFactorAsync();
                Token? nextToken = _stack.Peek();
                if (node is IdentifierNode)
                {
                    node = node.Eval();
                    node = ((IdentifierNode)node).Value;                                     
                }
                if(node is NumberNode)
                {
                    argnode.AddArgument((NumberNode)node);
                }
                else
                {
                    return null;
                }

                if(nextToken.TokenType == TokenType.CommaType)
                {
                    this._stack.Pop();
                }
                else if(nextToken.TokenType == TokenType.RParenthesisType)
                {
                    this._stack.Pop();
                    return argnode;
                }
                else
                {
                    this._stack.Pop();
                    return null;
                }
            }
        }
    }
}
