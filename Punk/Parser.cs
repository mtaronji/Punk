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
        
        public Dictionary<string,IdentifierNode> Identifiers { get; set; }
        public Dictionary<string, IdentifierNode> csvfiles { get; set; }

        private Stack<Token> _stack;
        private List<TreeNode>? ParseExpressions;

        public Parser()
        {
            //add special identifiers
            this._stack = new Stack<Token>();
            Identifiers = new Dictionary<string,IdentifierNode>();
            csvfiles = new Dictionary<string, IdentifierNode>();
        }
        public Parser(Dictionary<string, IdentifierNode> files)
        {
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


                    var b = await ParseExpressionAsync();
                    if (b == null) { throw new Exceptions.PunkAssignmentException("Incorrect Assignment syntax"); }
                    if (b is PipeNode) { throw new Exceptions.PunkAssignmentException("You cannot assign an identifier to a pipe sequence"); }

                    //set the value in the identifier and create an assignment node
                    IdentifierNode i = (IdentifierNode)a;
                    i.Value = b;
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

                //if the identifier isn't in our books, we will check to see if it's gonna be assigned. If it isn't, that's an error unless we are parsing a lambda expression
                if (this.Identifiers.ContainsKey(nextToken.Value))
                {
                    return Identifiers[nextToken.Value];
                }
                else
                {               
                    if (this._stack.Count > 0)
                    {
                        if (this._stack.Peek().TokenType != TokenType.AssignType)
                        {
                            throw new Exceptions.PunkIdentifierUninitializedException();
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
    }
}
