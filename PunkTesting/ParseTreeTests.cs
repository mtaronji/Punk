using Punk;
using Punk.BinaryOperators;
using Punk.UnaryOperators;
using Punk2.UnaryOperators;



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
            string expression = @"##stocks{stocks.Prices.Where((x) => x.Close > 300.00 && x.Ticker == ""SPY"").Select(x => x)} | ->=";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var treeprint = tree[0].Print();
            Assert.True(treeprint == "((Lambda(stocks.Prices.Where((x) => x.Close > 300.00 && x.Ticker == \"SPY\").Select(x => x))) | (Plot User Defined Data))");
            
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
            string expression = @"x = ||1 3 4 6;||.Transpose";
            var lexicon = this._lexer.Read(expression);
            Assert.True(lexicon[3].TokenType == TokenType.PeriodType);
            var expressionTrees = await this._parser.ParseAsync(lexicon);         
            Assert.True(expressionTrees[0].Right is InstanceFnNode);

            expression = @"x = (||1 3 4 6;|| * ||1;
                                                 1;
                                                 1;
                                                 1;||).Transpose";
            lexicon = this._lexer.Read(expression);
            Assert.True(lexicon[7].TokenType == TokenType.PeriodType);
            expressionTrees = await this._parser.ParseAsync(lexicon);
            Assert.True(expressionTrees[0].Right is InstanceFnNode);
        }

        [Fact]
        public async Task Linq_Sequence_Parses_and_pipes_For_User_Types()
        {
            //string expression = @"""SPY"" | ##stock
            //{               
            //    StockPrice, bool:
            //    return x0.Close > 17.1;
            //}";
            //var lexicon = this._lexer.Read(expression);
            //List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            //Assert.True(tree[0] is PipeNode);
            //var a = (PipeNode)tree[0];
        }
        [Fact]
        public async Task PlotType_ParseTree_Works()
        {
            //string expression = @"""SPY"" |##stock | ->=";

            //var lexicon = this._lexer.Read(expression);
            //List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            //Assert.True(tree[0] is PipeNode);
            //Assert.True(tree[0].Left is PipeNode);
            //Assert.True(tree[0].Left.Left is StringNode);
            //Assert.True(tree[0].Left.Right is RegisterNode);
            //Assert.True(tree[0].Right is PlotNode);

        }
        [Fact]
        public async Task Lambda_Type_ParseTree_Works()
        {
            string expression = @"##stocks{stocks.Prices.Where(x => x.Ticker == ""QQQ"").Select(x => x.Date.ToString())}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            Assert.True(tree[0] is QueryNode);
            

        }


    }
}
