using Punk.BinaryOperators;
using Punk.TypeNodes;
using Punk.UnaryOperators;
using Punk.Types;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;

namespace Punk
{
    //create the parse tree
    public class Parser
    {
        private void InitDiscreteProbabilityIdentifiers()
        {
            var discreteuniform = new ProbabilityNode(DistributionType.DiscreteUniform); this.ProbabilityIdentifiers["discreteuniform"] = discreteuniform;
            var bernouli = new ProbabilityNode(DistributionType.Bernoulli); this.ProbabilityIdentifiers["bernouli"] = bernouli;
            var binomial = new ProbabilityNode(DistributionType.Binomial); this.ProbabilityIdentifiers["binomial"] = binomial;
            var negativebinomial = new ProbabilityNode(DistributionType.NegativeBinomial); this.ProbabilityIdentifiers["negativebinomial"] = negativebinomial;
            var geometric = new ProbabilityNode(DistributionType.Geometric); this.ProbabilityIdentifiers["geometric"] = geometric;
            var hypergeometric = new ProbabilityNode(DistributionType.Hypergeometric); this.ProbabilityIdentifiers["hypergeometric"] = hypergeometric;
            var poisson = new ProbabilityNode(DistributionType.Poisson); this.ProbabilityIdentifiers["poisson"] = poisson;
            var cateogorical = new ProbabilityNode(DistributionType.Categorical); this.ProbabilityIdentifiers["cateogorical"] = cateogorical;
            var conwaympoisson = new ProbabilityNode(DistributionType.ConwayMaxwellPoisson); this.ProbabilityIdentifiers["conwaympoisson"] = conwaympoisson;
            var zipf = new ProbabilityNode(DistributionType.Zipf); this.ProbabilityIdentifiers["zipf"] = zipf;
        }

        private void InitContinuousProbabilityIdentifiers()
        {
            var continuousuniform = new ProbabilityNode(DistributionType.ContinuousUniform); this.ProbabilityIdentifiers["continuousuniform"] = continuousuniform;
            var normal = new ProbabilityNode(DistributionType.Normal); this.ProbabilityIdentifiers["normal"] = normal;
            var lognormal = new ProbabilityNode(DistributionType.LogNormal); this.ProbabilityIdentifiers["lognormal"] = lognormal;
            var beta = new ProbabilityNode(DistributionType.Beta); this.ProbabilityIdentifiers["beta"] = beta;
            var cauchy = new ProbabilityNode(DistributionType.Cauchy); this.ProbabilityIdentifiers["cauchy"] = cauchy;
            var chi = new ProbabilityNode(DistributionType.Chi); this.ProbabilityIdentifiers["chi"] = chi;
            var erlang = new ProbabilityNode(DistributionType.Erlang); this.ProbabilityIdentifiers["erlang"] = erlang;
            var fdistribution = new ProbabilityNode(DistributionType.FDistribution); this.ProbabilityIdentifiers["fdistribution"] = fdistribution;
            var gamma = new ProbabilityNode(DistributionType.Gamma); this.ProbabilityIdentifiers["gamma"] = gamma;
            var inversegamma = new ProbabilityNode(DistributionType.InverseGamma); this.ProbabilityIdentifiers["inversegamma"] = inversegamma;
            var laplace = new ProbabilityNode(DistributionType.Laplace); this.ProbabilityIdentifiers["laplace"] = laplace;
            var pareto = new ProbabilityNode(DistributionType.Pareto); this.ProbabilityIdentifiers["pareto"] = pareto;
            var rayleigh = new ProbabilityNode(DistributionType.Rayleigh); this.ProbabilityIdentifiers["rayleigh"] = rayleigh;
            var stable = new ProbabilityNode(DistributionType.Stable); this.ProbabilityIdentifiers["stable"] = stable;
            var tstudent = new ProbabilityNode(DistributionType.StudentT); this.ProbabilityIdentifiers["tstudent"] = tstudent;
            var weibull = new ProbabilityNode(DistributionType.Weibull); this.ProbabilityIdentifiers["weibull"] = weibull;
            var triangle = new ProbabilityNode(DistributionType.Triangular); this.ProbabilityIdentifiers["triangle"] = triangle;
        }
        private void InitMatrixInstanceFNKeywords()
        {
         
            Func<IEnumerable<dynamic>, Matrix<double>> transpose = (IEnumerable<dynamic> args) => { return args.ElementAt(0).Transpose(); }; MatrixInstanceFns.Add("transpose", transpose);
            Func<IEnumerable<dynamic>, Matrix<double>> transposethismultiply = (IEnumerable<dynamic> args) => { return args.ElementAt(0).TransposeThisAndMultiply(args.ElementAt(1)); }; MatrixInstanceFns.Add("transposethismultiply",transposethismultiply);
            Func<IEnumerable<dynamic>, Matrix<double>> transposemultiply = (IEnumerable<dynamic> args) => { return args.ElementAt(0).TransposeAndMultiply(args.ElementAt(1)); }; MatrixInstanceFns.Add("transposemultiply",transposemultiply);
            Func<IEnumerable<dynamic>, double> determinant = (IEnumerable<dynamic> args) => { return args.ElementAt(0).Determinant(); }; MatrixInstanceFns.Add("determinant",determinant);
            Func<IEnumerable<dynamic>, Matrix<double>> inverse = (IEnumerable<dynamic> args) => { return args.ElementAt(0).Inverse(); }; MatrixInstanceFns.Add("inverse",inverse);
            Func<IEnumerable<dynamic>, Vector<double>[]> kernel = (IEnumerable<dynamic> args) => { return args.ElementAt(0).Kernel(); }; MatrixInstanceFns.Add("kernel",kernel);
            Func<IEnumerable<dynamic>, Vector<double>> column = (IEnumerable<dynamic> args) => { return args.ElementAt(0).Column((int)args.ElementAt(1)); }; MatrixInstanceFns.Add("column", column);
            Func<IEnumerable<dynamic>, Matrix<double>> choleskysolve = (IEnumerable<dynamic> args) => { return args.ElementAt(0).Cholesky().Solve(args.ElementAt(1)); }; MatrixInstanceFns.Add("solve",choleskysolve); //AX = B  X and B matrices 
        }
        private void InitProbabilityInstanceFNKeywords()
        {
            
            Func<IEnumerable<dynamic>, double> cdf = (IEnumerable<dynamic> args) => { return args.ElementAt(0).CumulativeDistribution(args.ElementAt(1)); }; ProbabilityInstanceFns.Add("cdf", cdf);
            Func<IEnumerable<dynamic>, double> discreteprobability = (IEnumerable<dynamic> args) => { return args.ElementAt(0).Probability((int)args.ElementAt(1)); }; ProbabilityInstanceFns.Add("probability", discreteprobability);
        }
        private void InitDataInstanceFNKeywords()
        {

            Func<IEnumerable<dynamic>, Vector<double>> ToVector = (IEnumerable<dynamic> args) => {
                List<object>? data = args.ElementAt(0) as List<object>;
                if(data != null)
                {
                    double[] vectordata = data.Select(e => Convert.ToDouble(e)).ToArray();
                    return Vector<double>.Build.Dense(vectordata);
                }
                else
                {
                    throw new Exceptions.PunkDataNodeException("Unable to turn data into array. Check args");
                }
            }; 
            
            DataInstanceFns.Add("vector", ToVector);
        }
        
        public Dictionary<string,IdentifierNode> Identifiers { get; set; }
        public Dictionary<string, ProbabilityNode> ProbabilityIdentifiers { get; set; }
        public Dictionary<string, dynamic> ProbabilityInstanceFns { get; set; }
        public Dictionary<string,dynamic> MatrixInstanceFns { get; set; }
        public Dictionary<string, dynamic> DataInstanceFns { get; set; }


        public Dictionary<string, IdentifierNode> csvfiles { get; set; }

        private Stack<Token> _stack;
        private List<TreeNode>? ParseExpressions;

        public void Init()
        {

        }
     
        public Parser()
        {
            List<List<object>> ex1 = new();
            ex1.Add(new());
            ex1[0].Add(10); ex1[0].Add(0.5); ex1[0].Add(1);
            double[] e = ex1[0].Select(e => Convert.ToDouble(e)).ToArray();
            Vector<double> vector = Vector<double>.Build.Dense(e);
            this.DataInstanceFns = new();
            this.MatrixInstanceFns = new();
            this.ProbabilityInstanceFns = new();
            this.ProbabilityIdentifiers = new();
            this.ProbabilityInstanceFns = new();
            InitContinuousProbabilityIdentifiers();
            InitDiscreteProbabilityIdentifiers();
            InitMatrixInstanceFNKeywords();
            InitProbabilityInstanceFNKeywords();
            InitDataInstanceFNKeywords();
            this._stack = new Stack<Token>();
            Identifiers = new Dictionary<string,IdentifierNode>();
            csvfiles = new Dictionary<string, IdentifierNode>();
        }
        public Parser(Dictionary<string, IdentifierNode> files)
        {
            this.DataInstanceFns = new();
            this.MatrixInstanceFns = new();
            this.ProbabilityInstanceFns = new();
            this.ProbabilityIdentifiers = new();
            this.ProbabilityInstanceFns = new();
            InitContinuousProbabilityIdentifiers();
            InitDiscreteProbabilityIdentifiers();
            InitMatrixInstanceFNKeywords();
            InitProbabilityInstanceFNKeywords();
            InitDataInstanceFNKeywords();
            //add special identifiers
            this._stack = new Stack<Token>();
            Identifiers = new Dictionary<string, IdentifierNode>();
            csvfiles = files;
        }

        public async Task<List<TreeNode>> ParseAsync(Token[] Lexicon)
        {
            Identifiers = new Dictionary<string, IdentifierNode>(); //identifiers are memoryless
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
                else if (this.ProbabilityIdentifiers.ContainsKey(nextToken.Value))
                {
                    string keyword = nextToken.Value;
                    if (this._stack.Count > 0)
                    {
                        nextToken = _stack.Peek();
                        if (nextToken.TokenType == TokenType.LParenthesisType)
                        {
                            this._stack.Pop();
                            var args = await ParseArgsAsync();
                            if (args == null) { return ProbabilityIdentifiers[keyword]; } //no args
                            else
                            {
                                var pnode = ProbabilityIdentifiers[keyword]; pnode.SetArgs(args);
                                return pnode;
                            } //add args to the node
                        }
                        else
                        {
                            //expect parenthesis.Error
                            throw new NotImplementedException("Probability Identifiers should have open and close parenthesis");
                        }
                    }
                    else { throw new NotImplementedException("Probability Function should have open and close parenthesis"); }
                }
                else if (this.MatrixInstanceFns.ContainsKey(nextToken.Value))
                {
                    string keyword = nextToken.Value;
                    if (this._stack.Count > 0)
                    {
                        nextToken = _stack.Peek();
                        if (nextToken.TokenType == TokenType.LParenthesisType)
                        {
                            this._stack.Pop();
                            var args = await ParseArgsAsync();
                            if (args == null) { return new FnNode(keyword, MatrixInstanceFns[keyword], null); } //no args
                            else
                            {
                                return new FnNode(keyword, MatrixInstanceFns[keyword], args);
                            }
                        }
                        else
                        {
                            //expect parenthesis.Error
                            throw new NotImplementedException("Library Function should have open and close parenthesis");
                        }
                    }
                    else { throw new NotImplementedException("Library Function should have open and close parenthesis"); }
                }
                else if (this.DataInstanceFns.ContainsKey(nextToken.Value))
                {
                    string keyword = nextToken.Value;
                    if (this._stack.Count > 0)
                    {
                        nextToken = _stack.Peek();
                        if (nextToken.TokenType == TokenType.LParenthesisType)
                        {
                            this._stack.Pop();
                            var args = await ParseArgsAsync();
                            if (args == null) { return new FnNode(keyword, DataInstanceFns[keyword], null); } //no args
                            else
                            {
                                return new FnNode(keyword, DataInstanceFns[keyword], args);
                            }
                        }
                        else
                        {
                            //expect parenthesis.Error
                            throw new NotImplementedException("Data Instance Function should have open and close parenthesis");
                        }
                    }
                    else { throw new NotImplementedException("Data Instance Function should have open and close parenthesis"); }
                }
                else if (this.ProbabilityInstanceFns.ContainsKey(nextToken.Value))
                {
                    string keyword = nextToken.Value;
                    if (this._stack.Count > 0)
                    {
                        nextToken = _stack.Peek();
                        if (nextToken.TokenType == TokenType.LParenthesisType)
                        {
                            this._stack.Pop();
                            var args = await ParseArgsAsync();
                            if (args == null) { return new FnNode(keyword, ProbabilityInstanceFns[keyword], null); } //no args
                            else
                            {
                                return new FnNode(keyword, ProbabilityInstanceFns[keyword], args);
                            }
                        }
                        else
                        {
                            //expect parenthesis.Error
                            throw new NotImplementedException("Library Function should have open and close parenthesis");
                        }
                    }
                    else { throw new NotImplementedException("Library Function should have open and close parenthesis"); }
                }
                else  //identifier doesn't exist
                {
                    if (this._stack.Count > 0)
                    {
                        if (this._stack.Peek().TokenType != TokenType.AssignType)
                        {
                            throw new Exceptions.PunkIdentifierUninitializedException($"{nextToken.Value} is not assigned");
                        }
                    }
                    else { throw new Exceptions.PunkIdentifierUninitializedException($"{nextToken.Value} is not assigned"); }
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
                if (a == null) { throw new Exceptions.PunkSubtractionException("incorrect argument for subtraction operator"); }
                return new NegateNode(a);
            }

            else if (nextToken.TokenType == TokenType.LParenthesisType)
            {
                this._stack.Pop();
                var a = await ParseExpressionAsync();
                if (this._stack.Count > 0 && this._stack.Peek().TokenType == TokenType.RParenthesisType)
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
                if (nextToken.TokenType == TokenType.RParenthesisType)
                {
                    throw new Exceptions.PunkParenthesisException("Missing Matching Parenthesis");
                }
                else
                {
                    throw new Exceptions.PunkUnknownCharactersException("Unknown Character in Syntax");
                }
            }
            
        }

        public async Task<List<TreeNode>?> ParseArgsAsync()
        {
            if (this._stack.Count == 0)
            {
                return null;
            }
            if(_stack.Peek().TokenType == TokenType.RParenthesisType) { this._stack.Pop(); return null; }
            List<TreeNode> arguments = new List<TreeNode>();
            while (true)
            {
                var node = await ParseFactorAsync();
                Token? nextToken = _stack.Peek();
                if (node is IdentifierNode)
                {
                    node = node.Eval();
                    node = ((IdentifierNode)node).Value;                                     
                }
                if(node!= null)
                {
                    arguments.Add(node);
                }
                else
                {
                    throw new Exceptions.PunkArgumentException("No null arguments allowed");
                }


                if(nextToken.TokenType == TokenType.CommaType)
                {
                    this._stack.Pop();
                }
                else if(nextToken.TokenType == TokenType.RParenthesisType)
                {
                    this._stack.Pop();
                    return arguments;
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
