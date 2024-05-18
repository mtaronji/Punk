using Punk;
using Punk.BinaryOperators;
using Punk.TypeNodes;
using Punk.UnaryOperators;



namespace ParseTreeTests
{
    public class Parser_Should_Create_Tree
    {
        private Lexer _lexer;
        private Parser _parser;

        public Parser_Should_Create_Tree()
        {
            this._lexer = new Lexer();
            this._parser = new Parser();
        }
        [Fact]
        public async Task Addition_Expression_Should_Create_Tree()
        {
            string expression = @"5 + 7 + 10";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var temp = tree[0].Print();
            Assert.True(temp == "((5 + 7) + 10)");
        }

        [Fact]
        public async Task Multiplication_Expression_Should_Create_Tree()
        {
            string expression = @"7*2/4";
            var lexicon2 = this._lexer.Read(expression);
            var tree2 = await this._parser.ParseAsync(lexicon2);
            var temp2 = tree2[0].Print();
            Assert.True(temp2 == "((7 * 2) / 4)");
        }

        [Fact]
        public async Task Exponential_Expression_Should_Create_Tree()
        {
            string expression = @"7*2/4^5";
            var lexicon2 = this._lexer.Read(expression);
            var tree2 = await this._parser.ParseAsync(lexicon2);
            var temp2 = tree2[0].Print();

        }
        [Fact]
        public async Task Modulo_Expression_Should_Create_Tree()
        {
            string expression = @"7%3";
            var lexicon2 = this._lexer.Read(expression);
            var tree2 = await this._parser.ParseAsync(lexicon2);
            var temp2 = tree2[0].Print();
            Assert.True(temp2 == "(7 % 3)");

            expression = @"(7*2)%(3+4)";
            lexicon2 = this._lexer.Read(expression);
            tree2 = await this._parser.ParseAsync(lexicon2);
            temp2 = tree2[0].Print();
            Assert.True(temp2 == "((7 * 2) % (3 + 4))");
        }


        [Fact]
        public async Task Tree_Should_Have_Correct_Order_Of_Operations()
        {
            string expression = @"6 + 2 *7*2/4^5";
            var lexicon2 = this._lexer.Read(expression);
            var tree2 = await this._parser.ParseAsync(lexicon2);
            var temp2 = tree2[0].Print();


            expression = @"6 + 2 *7*2/4^5";
            lexicon2 = this._lexer.Read(expression);
            tree2 = await this._parser.ParseAsync(lexicon2);
            temp2 = tree2[0].Print();

        }

        [Fact]
        public async Task Assignment_Operation_Should_Create_Tree()
        {
            string expression = @"x = 2 *7*2/4^5";
            var lexicon2 = this._lexer.Read(expression);
            var tree2 = await this._parser.ParseAsync(lexicon2);
            var temp2 = tree2[0].Print();

        }

        [Fact]
        public async Task Negate_Operator_Should_Create_Tree()
        {
            string expression = @"-(2 + 5) + 3";
            var lexicon2 = this._lexer.Read(expression);
            var tree2 = await this._parser.ParseAsync(lexicon2);
            var temp2 = tree2[0].Print();
            Assert.True(temp2 == "(-(2 + 5) + 3)");

            expression = @"x = -(7 * 2 ^ 3)";
            lexicon2 = this._lexer.Read(expression);
            tree2 = await this._parser.ParseAsync(lexicon2);
            temp2 = tree2[0].Print();
        }

        [Fact]
        public async Task Should_Have_Syntax_Error()
        {
            string expression = @"x = [10,11,12,13.5])";
            var lexicon = this._lexer.Read(expression);
            await Assert.ThrowsAsync<Punk.Exceptions.PunkParenthesisException>(async () => await this._parser.ParseAsync(lexicon));

            expression = @"y = [10,11,12,13.5)";
            lexicon = this._lexer.Read(expression);
            await Assert.ThrowsAsync<Punk.Exceptions.PunkUnknownCharactersException>(async () => await this._parser.ParseAsync(lexicon));

            expression = @"z = [10,11,12,13.5]) 
                                  z1 = (6 * 7 ^   5)";
            lexicon = this._lexer.Read(expression);
            await Assert.ThrowsAsync<Punk.Exceptions.PunkParenthesisException>(async () => await this._parser.ParseAsync(lexicon));
        }

        [Fact]
        public async Task Seperate_Expressions_Should_Create_Seperate_Trees()
        {
            string expression = @"x = [10,11,12,13.5] y = +(6 * 7 ^   5)";
            var lexicon = this._lexer.Read(expression);
            await Assert.ThrowsAsync<Punk.Exceptions.PunkUnknownCharactersException>(async () => await this._parser.ParseAsync(lexicon));

            expression = @"z1 = [10,11,12,13.5] z2 = -(6 * 7 ^   5)";
            lexicon = this._lexer.Read(expression);
    
            var trees = await this._parser.ParseAsync(lexicon);
            Assert.True(trees.Count == 2);
                      
        }
        [Fact]
        public async Task Identifiers_Should_Work_When_Initialized()
        {
            string expression = @"x + 7";
            var lexicon = this._lexer.Read(expression);
            await Assert.ThrowsAsync<Punk.Exceptions.PunkIdentifierUninitializedException>( async () =>  await this._parser.ParseAsync(lexicon));

        }

        [Fact]
        public async Task Pipes_Should_Work()
        {
            string expression = @"##stocks{stocks.Query(stock => stock.Close > 300.00 && stock.Ticker == ""SPY"")} | ->=";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var treeprint = tree[0].Print();
            Assert.True(treeprint == "((Query(stocks.Query(stock => stock.Close > 300.00 && stock.Ticker == \"SPY\"))) | (Plot User Defined Data))");
            
        }

        [Fact]
        public async Task Parenthesis_Should_work()
        {
            string expression = @"x = (7 - 1";
            var lexicon = this._lexer.Read(expression);
            await Assert.ThrowsAsync<Punk.Exceptions.PunkParenthesisException>(async () => await this._parser.ParseAsync(lexicon));

        }

        [Fact]
        public async Task Period_Seperator_Should_work()
        {
            string expression = @"x = ||1 3 4 6;||.Transpose()";
            var lexicon = this._lexer.Read(expression);
            Assert.True(lexicon[3].TokenType == TokenType.PeriodType);
            var expressionTrees = await this._parser.ParseAsync(lexicon);
            Assert.True(expressionTrees[0].Right is InstanceFnNode);

            expression = @"x = (||1 3 4 6;|| * ||1;
                                                 1;
                                                 1;
                                                 1;||).Transpose()";
            lexicon = this._lexer.Read(expression);
            Assert.True(lexicon[7].TokenType == TokenType.PeriodType);
            expressionTrees = await this._parser.ParseAsync(lexicon);
            Assert.True(expressionTrees[0].Right is InstanceFnNode);
        }

        [Fact]
        public async Task Query_Type_ParseTree_Works()
        {
            string expression = @"##stocks{stocks.GetPrices(""QQQ"")}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            Assert.True(tree[0] is QueryNode);

            expression = @"##stocks{stocks.SMA(20,""SPY"", ""2020-01-01"",""2021-01-01"")}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            Assert.True(tree[0] is QueryNode);

            expression = @"##stocks{stocks.GetPrices(""^VIX"", ""2024-01-01"")}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            Assert.True(tree[0] is QueryNode);


        }

        [Fact]
        public async Task InstanceFN_Should_Parse()
        {
            string expression = @"||1 2 4;
                                    2 7 17;
                                    0 0 1;                                      
                                         ||. transpose() .Inverse()";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);


        }

        [Fact]
        public async Task Sequence_Pipes_Should_Parse()
        {
            string expression = @"x = [-10...10]
                                  y = x                               
                                  []{x0 : return Pow(x0,2);} | []{fn: return SimpsonRule.IntegrateComposite(x => fn(x), 0.0, 10.0, 4);}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var print3 = tree[2].Print();
            Assert.True(tree.Count == 3);
            Assert.True(print3 == "((SequenceOnDataType []) | (SequenceOnDataType []))");

        }

        [Fact]
        public async Task Matrix_Identifier_Operations_Should_Work()
        {
            string expression = @"x = || 1 2 5;
                                         3 5 6;
                                         4 2 2;
                                              ||
                                  y = || 3 4 5;
                                         4 5 5;
                                         4 5 6;
                                              ||

                                  z = x.transpose() * y";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var print3 = tree[2].Print();
            Assert.True(tree.Count == 3);

            expression =        @"x = || 1 2 5;
                                         3 5 6;
                                         4 2 2;
                                              ||
                                  y = || 3 4 5;
                                         4 5 5;
                                         4 5 6;
                                              ||

                                  z = x * y.transpose()";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            print3 = tree[2].Print();
            Assert.True(tree.Count == 3);

        }
        [Fact]
        public async Task Pointwise_Multiplication_Should_Parse_For_Matrices()
        {

            var teststring = @"x = || 1 2 5;
                                      3 5 6;
                                      4 2 2;
                                           ||

                               y = || 3 4 5;
                                      4 5 5;
                                      4 5 6;
                                           ||

                               z = x .* y.transpose()";

            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[2] is AssignmentNode);
            Assert.True(expressionTree[2].Right is PointWiseMultiplicationNode);
        }
        [Fact]
        public async Task ProbabilityFnParses()
        {
            var teststring = @"gamma(7, 10)";
            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[0] is ArgumentsNode);
            var print = expressionTree[0].Print();

            teststring = @"gamma(7, 10).cdf(0.7)";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[0] is InstanceFnNode);
            print = expressionTree[0].Print();

            teststring = @"x = gamma(7, 10)
                           x.cdf(0.7)";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[1] is InstanceFnNode);
            print = expressionTree[1].Print();

        }


    }
}
