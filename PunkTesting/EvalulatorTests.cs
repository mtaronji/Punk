using Punk;
using Punk.TypeNodes;
using Punk.SP500StockModels;
using Xunit.Sdk;

namespace EvaluatorTests
{
    public class Evaluators_Should_Work
    {
        private Lexer _lexer;
        private Parser _parser;
        public Evaluators_Should_Work()
        {
            this._lexer = new Lexer();
            this._parser = new Parser();
        }

        [Fact]
        public async Task Addition_Evaluator_Should_Work()
        {
            string expression = @"5 + 7 + 10";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            Assert.True(node is NumberNode);
            var n1 = node.Value;
            Assert.True(n1.Value == 22);
        }
        [Fact]
        public async Task Subtraction_Evaluator_Should_Work()
        {
            string expression = @"10 - 7 + 10*2";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            Assert.True(node is NumberNode);
            var n1 = node.Value;
            Assert.True(n1.Value == 23);

            expression = @"-(10 - 7 + 10*2)*2";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            Assert.True(node is NumberNode);
            n1 = node.Value;
            Assert.True(n1.Value == -46);
        }
        [Fact]
        public async Task Negate_Evaluator_Should_Work()
        {
            string expression = @"-10 - 7 + 10*2 + -6";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            Assert.True(node is NumberNode);
            var n1 = node.Value;
            Assert.True(n1.Value == -3);
        }

        [Fact]
        public async Task Multiplication_Evaluator_Should_Work()
        {
            string expression = @"5*7*10";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.Value;
            Assert.True(n1.Value == 350);
        }

        [Fact]
        public async Task Modulo_Evaluator_Should_Work()
        {
            string expression = @"(5*7)%(5+2)";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.Value;
            Assert.True(n1.Value == 0);

            expression = @"(5*7)%(5 + 1)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.Value;
            Assert.True(n1.Value == 5);
        }

        [Fact]
        public async Task Evaluator_Works_With_Combinations()
        {
            string expression = @"5*7*10 + 5";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.Value;
            Assert.True(n1.Value == 355);
        }

        [Fact]
        public async Task Evaluator_Works_With_Exponents()
        {
            string expression = @"5*7*10 + 5^2";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.Value;
            Assert.True(n1.Value == 375);

            expression = @"25^(1/2)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.Value;
            Assert.True(n1.Value == 1);

            expression = @"25^(1.0/2)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.Value;
            Assert.True(n1.Value == 5);

            expression = @"25^(1/2.0)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.Value;
            Assert.True(n1.Value == 5);

            expression = @"7 + 2^2 + 25.0^(1.0/2.0)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.Value;
            Assert.True(n1.Value == 16);
        }

        [Fact]
        public async Task Evaluator_Works_With_Data_Type()
        {
            string expression = @"x = [14,0.5,3,6]";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is IdentifierNode);

            expression = @"y = [14,0.5,3,-6,-2,23.4]";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            Assert.True(node is IdentifierNode);

            expression = @"x";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            Assert.True(node is IdentifierNode);
            var inode = (IdentifierNode)node;
            Assert.True(inode.Value is DataNode);

        }
        [Fact]
        public async Task Sequence_Operator_Works()
        {
            string expression = @"[14,0.5,3,6]{ dynamic, dynamic : return Pow(x0,2.0);}";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            var data = node.Value;
            Assert.True(node.Value.DataVectors.Count == 1);
            Assert.True(node.Value.TransformedSequence != null);
            Assert.True(node.Value.TransformedSequence.Count == 4);

            Assert.True((double)data.TransformedSequence[0] == 196);
            Assert.True((double)data.TransformedSequence[1] == 0.25);
            Assert.True((double)data.TransformedSequence[2] == 9);
            Assert.True((double)data.TransformedSequence[3] == 36);

            expression = @"[14,0.5,3,6]{dynamic, dynamic : return Pow(x0, 2.0);}{dynamic,dynamic: return x0/2.0;}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True((double)data.TransformedSequence[0] == 98);
            Assert.True((double)data.TransformedSequence[1] == 0.125);
            Assert.True((double)data.TransformedSequence[2] == 4.5);
            Assert.True((double)data.TransformedSequence[3] == 18);

            expression = @"[13,4,5,7]{dynamic, dynamic : return x0 / 2;}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True((long)data.TransformedSequence[0] == 6);
            Assert.True((long)data.TransformedSequence[1] == 2);
            Assert.True((long)data.TransformedSequence[2] == 2);
            Assert.True((long)data.TransformedSequence[3] == 3);


            expression = @"[0...100000]{dynamic, dynamic : return x0*2.0;}{dynamic, dynamic : return x0/3.0;}{dynamic, dynamic: return Pow(x0,(1.0/3.0));}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);

            data = node.Value;
            int i0 = 0;
            foreach (var d in data.TransformedSequence)
            {

                var a = i0 * 2.0 / 3.0;
                var b = 1.0 / 3.0;
                var trueValue = Math.Pow(a, b);
                Assert.True((double)d == trueValue);
                i0++;
            }

            expression = @"[0...100000]{dynamic,dynamic : return x0 - 50000;}{dynamic, dynamic : return x0/5000.0;}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);

            data = node.Value;
            i0 = 0;
            foreach (var d in data.TransformedSequence)
            {
                var a = (i0 - 50000) / 5000.0;
                Assert.True((double)d == a);
                i0++;
            }

            var list = new List<double>();
            for (int i = 0; i < 100000; i++)
            {
                list.Add(i);
            }
            Func<double, double> transform1 = d => d - 50000.00;
            Func<double, double> transform2 = d => d / 50000.00;
            for (int i = 0; i < 100000; i++)
            {
                var t1 = transform1(i);
                list[i] = transform2(t1);
            }

            expression = @"[-10...10,-10...10]{dynamic,dynamic,dynamic : return Pow(x0,2) + Pow(x1,2);}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True((double)data.TransformedSequence[0] == 200);
            Assert.True((double)data.TransformedSequence[3] == 98);
            Assert.True((double)data.TransformedSequence[20] == 200);

            expression = @"[-100 ... 10 ,   -10 ...   10]{ dynamic, dynamic,  dynamic : return Pow(x0,2) + Pow(x1,2);}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(data.TransformedSequence.Count ==21);
            Assert.True((double)data.TransformedSequence[0] == 10100);
            Assert.True((double)data.TransformedSequence[3] == 9458);
            Assert.True((double)data.TransformedSequence[20] == 6500);

            expression = @"[-10 ... 10 ,   -10 ... 10, -10...10]{ dynamic, dynamic,  dynamic, dynamic : return Pow(x0,2) + Pow(x1,2) + Pow(x2, 2);}{dynamic,dynamic : return x0 + 1;}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(data.TransformedSequence.Count == 21);
            Assert.True((double)data.TransformedSequence[1] == 244);
            Assert.True((double)data.TransformedSequence[3] == 148);
            Assert.True((double)data.TransformedSequence[20] == 301);
        }
        [Fact]
        public async Task Identifiers_Work()
        {
            string expression = @"x = 7
                                  y = x + 3
                                  z = x + y";
                                                    
            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is IdentifierNode);
            var idnode = (IdentifierNode)node;
            var numnode = (NumberNode)idnode.Value;
            Assert.True(numnode.Value.Value == 7);
            var node2 = tree[1].Eval();
            Assert.True(node2 is IdentifierNode);
            idnode = (IdentifierNode)node2;
            numnode = (NumberNode)idnode.Value;
            Assert.True(numnode.Value.Value == 10);
            node = tree[2].Eval();
            Assert.True(node is IdentifierNode);
            idnode = (IdentifierNode)node;
            numnode = (NumberNode)idnode.Value;
            Assert.True(numnode.Value.Value == 17);
        }

        [Fact]
        public async Task MatrixType_Works()
        {
            string expression = @"x = ||15 15 3;
                                         4  4 2;
                                         77 5 1;
                                              ||";
                                                    
            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is IdentifierNode);
            var idnode = (IdentifierNode)node;
            var matrixnode = (MatrixNode)idnode.Value;
            Assert.True(matrixnode.matrix.Value != null);

            expression = @"x = ||15 15 3;
                                  4  4 2;
                                 77  5 1;
                                        || *

                                || 1 0 0;
                                   0 1 0;
                                   0 0 1;||";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            Assert.True(node is IdentifierNode);
            idnode = (IdentifierNode)node;
            matrixnode = (MatrixNode)idnode.Value;
            Assert.True(matrixnode.matrix.Value != null);

        }

        [Fact]
        public async Task Seperator_Evaluator_Works()
        {
            string expression = @"||1 2 4;
                                    2 7 17;
                                    0 0 1;                                      
                                         ||. transpose .Inverse";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is MatrixNode);
            var matrixnode = (MatrixNode)node;
            var matrixvaluesarr = matrixnode.matrix.Value.ToArray();
            Assert.True(matrixvaluesarr[0,0] == 2.333333333333333);
            Assert.True(matrixvaluesarr[0,1] == -0.66666666666666641);
            Assert.True(matrixvaluesarr[0, 2] == 0);
            Assert.True(matrixvaluesarr[1, 0] == -0.66666666666666663);
            Assert.True(matrixvaluesarr[1, 1] == 0.33333333333333326);
            Assert.True(matrixvaluesarr[1, 2] == 0);
            Assert.True(matrixvaluesarr[2, 0] == 1.9999999999999998);
            Assert.True(matrixvaluesarr[2, 1] == -2.9999999999999996);
            Assert.True(matrixvaluesarr[2, 2] == 1);

        }

        [Fact]
        public async Task Register_Evaluator_Works()
        {
            string expression = @"##stock | ->=";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            Assert.Throws<NotImplementedException>( () => tree[0].Eval());

            expression = @"##stocks{stocks.Prices.Where(p => p.Close < 2.00).Select(p => p)} | ->=()";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is PlotNode);

            expression = @"##options{options.Prices.Where(p => p.Code == ""QQQ211217C00330000"").Select(p => p)} | ->=";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            Assert.True(node is PlotNode);
        }

        [Fact]
        public async Task Linq_Sequence_Works_For_Register_Type()
        {
            //string expression = @"##stock(SPY)
            //{               
            //    StockPrice, bool:
            //    return x0.Close > 17.1;
            //}";
            //var stock_prices = TestData();
            //var lexicon = this._lexer.Read(expression);
            //List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            //var node = tree[0].Eval();
            //Assert.True(node is RegisterNode);
            //var register = (RegisterNode)node;
            //var filteredPrices = stock_prices.FindAll(x => { return register.Transforms[0](x); });

            //Assert.True(filteredPrices.Count == 1);

        }

        [Fact]
        public async Task Pipes_Should_Evaluate_Correctly()
        {
            //expression = @"##stock(SPY){StockPrice,StockPrice:x0.Close += 10; return x0;}{StockPrice,StockPrice: x0.Open += 10; return x0;}{StockPrice, StockPrice: x0.Low += 10; return x0;} | ->=";
            var expression = @"##stocks{stocks.Prices.Where(x => x.Ticker == ""XLV"" && x.Date > new DateOnly(2023,11,1))}| ->=";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var treeprint = tree[0].Print();
            var evaluation = tree[0].Eval();
            Assert.True(evaluation is PlotNode);
            var plotnode = (PlotNode)evaluation;
          
            
        }

        [Fact]
        public async Task Plot_Should_Evaluate_Correctly()
        {
            var expression = @"##stocks{stocks.Prices.Where(x => x.Ticker == ""XLV"" && x.Date > new DateOnly(2023,11,1))}| ->=";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var treeprint = tree[0].Print();
            var evaluation = tree[0].Eval();
            Assert.True(evaluation is PlotNode);
            var plotnode = (PlotNode)evaluation;
            Assert.True(plotnode.data != null);


        }

    }
}