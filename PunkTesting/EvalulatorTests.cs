using Punk;
using Punk.TypeNodes;
using Punk.BinaryOperators;
using Punk.UnaryOperators;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
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
            var n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 22);
        }
        [Fact]
        public async Task Subtraction_Evaluator_Should_Work()
        {
            string expression = @"10 - 7 + 10*2";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            Assert.True(node is NumberNode);
            var n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 23);

            expression = @"-(10 - 7 + 10*2)*2";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            Assert.True(node is NumberNode);
            n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == -46);
        }
        [Fact]
        public async Task Negate_Evaluator_Should_Work()
        {
            string expression = @"-10 - 7 + 10*2 + -6";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            Assert.True(node is NumberNode);
            var n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == -3);
        }

        [Fact]
        public async Task Multiplication_Evaluator_Should_Work()
        {
            string expression = @"5*7*10";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 350);
        }

        [Fact]
        public async Task Modulo_Evaluator_Should_Work()
        {
            string expression = @"(5*7)%(5+2)";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 0);

            expression = @"(5*7)%(5 + 1)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 5);
        }

        [Fact]
        public async Task Evaluator_Works_With_Combinations()
        {
            string expression = @"5*7*10 + 5";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 355);
        }

        [Fact]
        public async Task Evaluator_Works_With_Exponents()
        {
            string expression = @"5*7*10 + 5^2";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (NumberNode)tree[0].Eval();
            var n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 375);

            expression = @"25^(1/2)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 1);

            expression = @"25^(1.0/2)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 5);

            expression = @"25^(1/2.0)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 5);

            expression = @"7 + 2^2 + 25.0^(1.0/2.0)";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (NumberNode)tree[0].Eval();
            n1 = node.NumberTypeValue;
            Assert.True(n1.NumberValue == 16);
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

        }
        [Fact]
        public async Task Sequence_Operator_Works()
        {
            string expression = @"[14,0.5,3,6]{ x0 : return Pow(x0,2.0);}";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            var data = node.Value;
            Assert.True(node.Value.DataVectors.Count == 1);
            Assert.True(node.Value.DataVectors[0].Count == 4);
            Assert.True(data.DataVectors[0].Count > 0);
            Assert.True((double)data.DataVectors[0][0] == 196);
            Assert.True((double)data.DataVectors[0][1] == 0.25);
            Assert.True((double)data.DataVectors[0][2] == 9);
            Assert.True((double)data.DataVectors[0][3] == 36);

            expression = @"[-3140...3140]{x : return x/1000.0;}{theta : return Tan(theta);}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(node.Value.DataVectors.Count == 1);
            Assert.True(node.Value.DataVectors[0].Count > 0);

            expression = @"[14,0.5,3,6]{x0 : return Pow(x0, 2.0);}{x0: return x0/2.0;}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(data.DataVectors[0].Count > 0);
            Assert.True((double)data.DataVectors[0][0] == 98);
            Assert.True((double)data.DataVectors[0][1] == 0.125);
            Assert.True((double)data.DataVectors[0][2] == 4.5);
            Assert.True((double)data.DataVectors[0][3] == 18);

            expression = @"[13,4,5,7]{x0 : return x0 / 2;}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(data.DataVectors != null);
            Assert.True((long)data.DataVectors[0][0] == 6);
            Assert.True((long)data.DataVectors[0][1] == 2);
            Assert.True((long)data.DataVectors[0][2] == 2);
            Assert.True((long)data.DataVectors[0][3] == 3);


            expression = @"[0...100000]{x0 : return x0*2.0;}{x0 : return x0/3.0;}{x0: return Pow(x0,(1.0/3.0));}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);

            data = node.Value;
            int i0 = 0;
            Assert.True(data.DataVectors[0] != null);
            foreach (var d in data.DataVectors[0])
            {

                var a = i0 * 2.0 / 3.0;
                var b = 1.0 / 3.0;
                var trueValue = Math.Pow(a, b);
                Assert.True((double)d == trueValue);
                i0++;
            }

            expression = @"[0...100000]{x0 : return x0 - 50000;}{x0 : return x0/5000.0;}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);

            data = node.Value;
            i0 = 0;
            Assert.True(data.DataVectors[0].Count > 0);
            foreach (var d in data.DataVectors[0])
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

            expression = @"x = [-100...100]{x0 : return x0 / 250.0;}
                           y = [-100...100]{x0 : return x0 / 250.0;} 
                           z = x + y
                           z{x0, x1 : return Sin(x0) + Cos(x1);}
                           x2 = [-100...100]
                           x3 = x2{x0 : return x0 / 250.0;}";


            lexicon = this._lexer.Read(expression);
            var trees = await this._parser.ParseAsync(lexicon);
            List<TreeNode> evaluations = new();
            foreach (var t in trees)
            {
                evaluations.Add(t.Eval());
            }
            var idnode = (IdentifierNode)evaluations[0];
            DataNode? dnode = idnode.Value as DataNode;
            Assert.True(dnode is DataNode);
            Assert.True(dnode.Value.DataVectors[0].Count > 0);
            Assert.True((double)dnode.Value.DataVectors[0][0] == -0.4);

            idnode = (IdentifierNode)evaluations[1];
            dnode = idnode.Value as DataNode;
            Assert.True(dnode is DataNode);
            Assert.True(dnode.Value.DataVectors[0].Count > 0);
            Assert.True((double)dnode.Value.DataVectors[0][0] == -0.4);

            idnode = (IdentifierNode)evaluations[2];
            dnode = idnode.Value as DataNode;
            Assert.True(dnode is DataNode);
            Assert.True(dnode.Value.DataVectors[0].Count == 201);
            Assert.True(dnode.Value.DataVectors[1].Count == 201);

            dnode = evaluations[3] as DataNode;
            Assert.True(dnode is DataNode);
            Assert.True(dnode.Value.DataVectors.Count == 1);
            //Assert.True(dnode.Value.DataVectors[1].Count == 201);

            idnode = (IdentifierNode)evaluations[1];
            Assert.True(node is DataNode);

            idnode = (IdentifierNode)evaluations[2];
            Assert.True(node is DataNode);

            node = (DataNode)evaluations[3];
            Assert.True(node is DataNode);

            expression = @"[-10 ... 10,  -10 ... 10]{ x0,x1 : return Sin(x0) + Cos(x1);}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(data.DataVectors[0].Count > 0);
            Assert.True(data.DataVectors[0].Count == 21);
            var row = data.DataVectors[0][0] as List<object>;
            Assert.True(row != null);
            Assert.True(row.Count == 21);        
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
            Assert.NotNull(idnode.Value);
            var numnode = (NumberNode)idnode.Value;
            Assert.True(numnode.NumberTypeValue.NumberValue == 7);
            var node2 = tree[1].Eval();
            Assert.True(node2 is IdentifierNode);
            idnode = (IdentifierNode)node2;
            Assert.NotNull(idnode.Value);
            numnode = (NumberNode)idnode.Value;
            Assert.True(numnode.NumberTypeValue.NumberValue == 10);
            node = tree[2].Eval();
            Assert.True(node is IdentifierNode);
            idnode = (IdentifierNode)node;
            Assert.NotNull(idnode.Value);
            numnode = (NumberNode)idnode.Value;
            Assert.True(numnode.NumberTypeValue.NumberValue == 17);
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
            Assert.NotNull(idnode.Value);
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
            Assert.NotNull(idnode.Value);
            matrixnode = (MatrixNode)idnode.Value;
            Assert.True(matrixnode.matrix.Value != null);

        }

        [Fact]
        public async Task Seperator_Evaluator_Works()
        {
            string expression = @"||1 2 4;
                                    2 7 17;
                                    0 0 1;                                      
                                         ||. transpose() .inverse()";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is MatrixNode);
            var matrixnode = (MatrixNode)node;
            var matrix = matrixnode.matrix.Value as Matrix<double>;
            Assert.True(matrix != null);
            var matrixvaluesarr = matrix.ToArray();
            Assert.True(matrixvaluesarr[0, 0] == 2.333333333333333);
            Assert.True(matrixvaluesarr[0, 1] == -0.66666666666666641);
            Assert.True(matrixvaluesarr[0, 2] == 0);
            Assert.True(matrixvaluesarr[1, 0] == -0.66666666666666663);
            Assert.True(matrixvaluesarr[1, 1] == 0.33333333333333326);
            Assert.True(matrixvaluesarr[1, 2] == 0);
            Assert.True(matrixvaluesarr[2, 0] == 1.9999999999999998);
            Assert.True(matrixvaluesarr[2, 1] == -2.9999999999999996);
            Assert.True(matrixvaluesarr[2, 2] == 1);

            expression =  @"x = || 1 2 5;
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
            Assert.True(tree.Count == 3);
            node = tree[2].Eval();
            Assert.True(node is IdentifierNode);
            var idnode = (IdentifierNode)node;
            Assert.True(idnode != null && idnode.Value != null);
            matrixnode = (MatrixNode)idnode.Value;
            matrix = matrixnode.matrix.Value as Matrix<double>;
            Assert.True(matrix != null);
            matrixvaluesarr = matrix.ToArray();
            Assert.True(matrixvaluesarr[0, 0] == 36);
            Assert.True(matrixvaluesarr[0, 1] == 39);
            Assert.True(matrixvaluesarr[0, 2] == 44);
            Assert.True(matrixvaluesarr[1, 0] == 59);
            



        }

        [Fact]
         public async Task Register_And_Query_Evaluator_Works()
        {
            string expression = @"##stock | ->=";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            Assert.Throws<NotImplementedException>( () => tree[0].Eval());

            expression = @"##stocks{Query(p => p.Close < 2.00 && p.Ticker == ""XLK"")} | ->=";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is PlotNode); 

            expression = @"##stocks{Query(x => x.Ticker == ""XLY"" && x.Close > 120.0)} | ->=";
            //expression = @"##options{options.Prices.Where(p => p.Code == ""QQQ211217C00330000"").Select(p => p)} | ->=";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            Assert.True(node is PlotNode);

            expression = @"##stocks{GetPrices(""^VIX"", ""2024-01-01"")}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            Assert.True(tree[0] is QueryNode);
            node = tree[0].Eval();
            

        }

        [Fact]
        public async Task Join_Query_Works()
        {
            string expression = @"##stocks{Join(""SPY"",""XLY"")}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            var qnode = (QueryNode)node;
            Assert.NotNull(qnode); 
            Assert.NotNull(qnode.query); 
            Assert.NotNull(qnode.query.EvaulatedQuery);
            Assert.True(qnode.query.EvaulatedQuery.Count() > 0);

            expression = @"##stocks{Join(""SPY"",""XLK"",""2020-05-01"")}";
            //expression = @"x = ##stocks{stocks.EMA(8, ""2022-01-01"")}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            qnode = (QueryNode)node;
            Assert.NotNull(qnode);
            Assert.NotNull(qnode.query);
            Assert.NotNull(qnode.query.EvaulatedQuery);
            Assert.True(qnode.query.EvaulatedQuery.Count() > 0);

        }

        [Fact]
        public async Task Lead_Query_Works()
        {
            string expression = @"##stocks{Lead(""SPY"",5, ""2021-01-01"")}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            var qnode = (QueryNode)node;
            Assert.NotNull(qnode);
            Assert.NotNull(qnode.query);
            Assert.NotNull(qnode.query.EvaulatedQuery);
            Assert.True(qnode.query.EvaulatedQuery.Count() > 0);

            expression = @"##stocks{Lead(""XLK"",20, ""2021-01-04"")}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            qnode = (QueryNode)node;
            Assert.NotNull(qnode);
            Assert.NotNull(qnode.query);
            Assert.NotNull(qnode.query.EvaulatedQuery);
            Assert.True(qnode.query.EvaulatedQuery.Count() > 0);

        }

        [Fact]
        public async Task Lag_Query_Works()
        {
            string expression = @"##stocks{Lag(""SPY"",5, ""2021-01-01"")}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            var qnode = (QueryNode)node;
            Assert.NotNull(qnode);
            Assert.NotNull(qnode.query);
            Assert.NotNull(qnode.query.EvaulatedQuery);
            Assert.True(qnode.query.EvaulatedQuery.Count() > 0);

            expression = @"##stocks{Lag(""XLK"",20, ""2021-01-04"")}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            qnode = (QueryNode)node;
            Assert.NotNull(qnode);
            Assert.NotNull(qnode.query);
            Assert.NotNull(qnode.query.EvaulatedQuery);
            Assert.True(qnode.query.EvaulatedQuery.Count() > 0);

        }
        [Fact]
        public async Task Pipes_Should_Evaluate_Correctly()
        {
            //expression = @"##stock(SPY){StockPrice,StockPrice:x0.Close += 10; return x0;}{StockPrice,StockPrice: x0.Open += 10; return x0;}{StockPrice, StockPrice: x0.Low += 10; return x0;} | ->=";
            var expression = @"##stocks{GetPrices(""XLV"", ""2023-09-01"", ""2023-12-01"")}| ->=";
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
            var expression = @"##stocks{GetPrices(""XLV"",""2020-01-01"", ""2023-11-01"")}| ->=";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var treeprint = tree[0].Print();
            var evaluation = tree[0].Eval();
            Assert.True(evaluation is PlotNode);
            var plotnode = (PlotNode)evaluation;
            Assert.True(plotnode.data != null);


        }

        [Fact]
        public async Task Sequence_Pipes_Should_Evaluate()
        {
    
            string expression = @"x = [-10...10]
                                  y = x                               
                                  []{x0 : return Pow(x0,2);} | []{fn: return SimpsonRule.IntegrateComposite(x => fn(x), 0.0, 10.0, 4);}
                                  []{x0 : return Pow(x0,2);} | []{fn: return SimpsonRule.IntegrateThreePoint(x => fn(x), 0.0, 10.0);}";

            

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var print3 = tree[2].Print();
            Assert.True(tree[2] is PipeNode);
            var eval = tree[2].Eval();
            Assert.True(eval is NumberNode);
            var node = (NumberNode)eval;
            var val = Math.Round(node.NumberTypeValue.NumberValue, 2);
            Assert.True(val == 333.33);

            eval = tree[3].Eval();
            Assert.True(eval is NumberNode);
            node = (NumberNode)eval;
            val = Math.Round(node.NumberTypeValue.NumberValue, 2);
            Assert.True(val == 333.33);

        }

        [Fact]
        public async Task Column_Instance_Should_Evaluate_For_Matrices()
        {

            var teststring = @"x = || 1 2 5;
                                      3 5 6;
                                      4 2 2;
                                           ||

                               y = || 3 4 5;
                                      4 5 5;
                                      4 5 6;
                                           ||

                               z = y.column(0)";

            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            var eval = expressionTree[2].Eval();
            IdentifierNode idnode = (IdentifierNode)eval;
            var matrixnode = idnode.Value as MatrixNode;
            Assert.True(matrixnode != null);
            var vector = matrixnode.matrix.Value as Vector<double>;
            Assert.True(vector is Vector<double> && vector != null);
            Assert.True(vector[0] == 3);
            Assert.True(vector[1] == 4);
            Assert.True(vector[2] == 4);

        }

        [Fact]
        public async Task ProbabilityFnParses()
        {
            var teststring = @"gamma(7, 10)";
            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[0] is ProbabilityNode);
            var eval = expressionTree[0].Eval();
            Assert.True(eval is ProbabilityNode);

            teststring = @"gamma(7, 10).cdf(0.7)";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[0] is InstanceFnNode);
            eval = expressionTree[0].Eval();
            Assert.True(eval is NumberNode);

            teststring = @"x = gamma(7, 10)
                           x.cdf(0.7)";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[1] is InstanceFnNode);
            eval = expressionTree[1].Eval();
            Assert.True(eval is NumberNode);

            teststring = @"binomial(0.5, 10).probability(0)";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[0] is InstanceFnNode);
            eval = expressionTree[0].Eval();
            Assert.True(eval is NumberNode);

            teststring = @"x = binomial(0.5, 10)
                           x.cdf(1)";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[1] is InstanceFnNode);
            eval = expressionTree[1].Eval();
            Assert.True(eval is NumberNode);

            teststring = @"d = binomial(0.5, 10)
                           [1...10] | d";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[1] is PipeNode);
            eval = expressionTree[1].Eval();
            Assert.True(eval is DataNode);

            teststring = @"d = binomial(0.5, 10)
                           s = [1...10] | d";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[1] is AssignmentNode);
            Assert.True(expressionTree[1].Right is PipeNode);
            eval = expressionTree[1].Eval();
            Assert.True(eval is IdentifierNode);

            teststring = @"d = normal()
                           s = [1...10] | d";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            Assert.True(expressionTree[1] is AssignmentNode);
            Assert.True(expressionTree[1].Right is PipeNode);
            eval = expressionTree[1].Eval();
            Assert.True(eval is IdentifierNode);

            teststring = @"d = normal()
                           x = [1...10]
                           samples = x | d";

            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            eval = expressionTree[2].Eval();
            Assert.True(eval is IdentifierNode);
        }
        [Fact]
        public async Task DataToMatrixPipeEvaluates()
        {
            string teststring = @"x = [1...10]
                                  v = x.vector()";
            ;

            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            var eval = expressionTree[1].Eval();
            Assert.True(eval is IdentifierNode);
            var idnode = eval as IdentifierNode;
            Assert.NotNull(idnode);
            Assert.NotNull(idnode.Value);
            Assert.True(idnode.Value is MatrixNode);
        }
        [Fact]
        public async Task Simpson_Integration_Evaluates()
        {
            var teststring = @"[] {x0 : return Pow(x0,2);} | [ ] {fn : return SimpsonRule.IntegrateComposite(x => fn(x), 0.0,  10.0, 4);}";

            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            //Assert.True(expressionTree[0] is ArgumentsNode);
            var eval = expressionTree[0].Eval();
            Assert.True(eval is NumberNode);
        }

        [Fact]
        public async Task FRED_GetObservations_Evaluates()
        {
            var teststring = @"##fred{GetObservations(""T10Y2Y"",""2023-01-01"")}";
            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            //Assert.True(expressionTree[0] is ArgumentsNode);
            var eval = expressionTree[0].Eval();
            Assert.True(eval is QueryNode);
            QueryNode queryNode = (QueryNode)eval;
            Assert.True(queryNode.query.EvaulatedQuery.Count() > 0);
        }

        [Fact]
        public async Task FRED_JoinObservations_Evaluates()
        {
            var teststring = @"##fred{Join(""T10Y2Y"",""MORTGAGE30US"",""2023-01-01"")}";
            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            //Assert.True(expressionTree[0] is ArgumentsNode);
            var eval = expressionTree[0].Eval();
            Assert.True(eval is QueryNode);
            QueryNode queryNode = (QueryNode)eval;
            Assert.True(queryNode.query.EvaulatedQuery.Count() > 0);
        }

        [Fact]
        public async Task FRED_LeadLag_Evaluates()
        {
            var teststring = @"##fred{Lag(""T10Y2Y"",5,""2023-01-01"")}";
            var tokens = this._lexer.Read(teststring);
            var expressionTree = await this._parser.ParseAsync(tokens);
            //Assert.True(expressionTree[0] is ArgumentsNode);
            var eval = expressionTree[0].Eval();
            Assert.True(eval is QueryNode);
            QueryNode queryNode = (QueryNode)eval;
            Assert.True(queryNode.query.EvaulatedQuery.Count() > 0);

            teststring = @"##fred{Lead(""T10Y2Y"",5,""2023-01-01"")}";
            tokens = this._lexer.Read(teststring);
            expressionTree = await this._parser.ParseAsync(tokens);
            //Assert.True(expressionTree[0] is ArgumentsNode);
            eval = expressionTree[0].Eval();
            Assert.True(eval is QueryNode);
            queryNode = (QueryNode)eval;
            Assert.True(queryNode.query.EvaulatedQuery.Count() > 0);
        }

    }
}