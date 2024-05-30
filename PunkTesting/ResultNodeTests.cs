using Punk;
using Punk.TypeNodes;
using Punk.SP500StockModels;
using Xunit.Sdk;
using Punk.BinaryOperators;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace EvaluatorTests
{
    public class ResultNodes_Should_Work
    {
        private Lexer _lexer;
        private Parser _parser;
        public ResultNodes_Should_Work()
        {
            this._lexer = new Lexer();
            this._parser = new Parser();
        }

        [Fact]
        public async Task DataNodeResults_Should_Work()
        {
            string expression = @"x = [-10...10]
                                  y = x        
                                  z = x{ x0: return Sin(x0);}
                                  []{x0 : return Pow(x0,2);} | []{fn: return SimpsonRule.IntegrateComposite(x => fn(x), 0.0, 10.0, 4);}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);

            var eval = tree[0].Eval();
            var resultnode = eval as IResultTreeNode;
            Assert.NotNull(resultnode);
            var result = resultnode.GetResult();
            Assert.NotNull(result);

            eval = tree[1].Eval();
            resultnode = eval as IResultTreeNode;
            Assert.NotNull(resultnode);
            result = resultnode.GetResult();
            Assert.NotNull(result);

            eval = tree[2].Eval();
            resultnode = eval as IResultTreeNode;
            Assert.NotNull(resultnode);
            result = resultnode.GetResult();
            Assert.NotNull(result);

        }

        [Fact]
        public async Task QueryNodeResults_Should_Work()
        {
            string expression = @"##stocks{Join(""SPY"", ""XLV"")}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);

            var eval = tree[0].Eval();
            var resultnode = eval as IResultTreeNode;
            Assert.NotNull(resultnode);
            var result = resultnode.GetResult();

        }


    }
}