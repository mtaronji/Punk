﻿using Punk;
using Punk.TypeNodes;
using Punk.BinaryOperators;
using Punk.UnaryOperators;
using System.Collections;

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
            string expression = @"[14,0.5,3,6]{ x0 : return Pow(x0,2.0);}";
            var lexicon = this._lexer.Read(expression);
            var tree = await this._parser.ParseAsync(lexicon);
            var node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            var data = node.Value;
            Assert.True(node.Value.DataVectors.Count == 1);
            Assert.True(node.Value.TransformedSequence != null);
            Assert.True(node.Value.TransformedSequence.Count == 4);

            Assert.True(data.TransformedSequence != null);
            Assert.True((double)data.TransformedSequence[0] == 196);
            Assert.True((double)data.TransformedSequence[1] == 0.25);
            Assert.True((double)data.TransformedSequence[2] == 9);
            Assert.True((double)data.TransformedSequence[3] == 36);

            expression = @"[-3140...3140]{x : return x/1000.0;}{theta : return Tan(theta);}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(node.Value.DataVectors.Count == 1);
            Assert.True(node.Value.TransformedSequence != null);
            Assert.True((double)node.Value.DataVectors[0][0] == -3.14);
            Assert.True((double)node.Value.DataVectors[0][node.Value.DataVectors[0].Count - 1] == 3.14);



            expression = @"[14,0.5,3,6]{x0 : return Pow(x0, 2.0);}{x0: return x0/2.0;}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(data.TransformedSequence != null);
            Assert.True((double)data.TransformedSequence[0] == 98);
            Assert.True((double)data.TransformedSequence[1] == 0.125);
            Assert.True((double)data.TransformedSequence[2] == 4.5);
            Assert.True((double)data.TransformedSequence[3] == 18);

            expression = @"[13,4,5,7]{x0 : return x0 / 2;}";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);
            data = node.Value;
            Assert.True(data.TransformedSequence != null);
            Assert.True((long)data.TransformedSequence[0] == 6);
            Assert.True((long)data.TransformedSequence[1] == 2);
            Assert.True((long)data.TransformedSequence[2] == 2);
            Assert.True((long)data.TransformedSequence[3] == 3);


            expression = @"[0...100000]{x0 : return x0*2.0;}{x0 : return x0/3.0;}{x0: return Pow(x0,(1.0/3.0));}";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = (DataNode)tree[0].Eval();
            Assert.True(node is DataNode);

            data = node.Value;
            int i0 = 0;
            Assert.True(data.TransformedSequence != null);
            foreach (var d in data.TransformedSequence)
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
            Assert.True(data.TransformedSequence != null);
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
            Assert.True(dnode.Value.TransformedSequence != null);
            Assert.True((double)dnode.Value.TransformedSequence[0] == -0.4);

            idnode = (IdentifierNode)evaluations[1];
            dnode = idnode.Value as DataNode;
            Assert.True(dnode is DataNode);
            Assert.True(dnode.Value.TransformedSequence != null);
            Assert.True((double)dnode.Value.TransformedSequence[0] == -0.4);

            idnode = (IdentifierNode)evaluations[2];
            dnode = idnode.Value as DataNode;
            Assert.True(dnode is DataNode);
            Assert.True(dnode.Value.TransformedSequence != null);
            Assert.True(dnode.Value.TransformedSequence.Count == 201);
            List<object> row = (List<object>)dnode.Value.TransformedSequence[0];
            Assert.True(Math.Round((double)row[0], 3) == 0.532);


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
            Assert.True(data.TransformedSequence != null);
            Assert.True(data.TransformedSequence.Count == 21);

            
           
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
                                         ||. transpose() .Inverse()";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is MatrixNode);
            var matrixnode = (MatrixNode)node;
            var matrixvaluesarr = matrixnode.matrix.Value.ToArray();
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
            matrixvaluesarr = matrixnode.matrix.Value.ToArray();
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

            expression = @"##stocks{stocks.Query(p => p.Close < 2.00)} | ->=";

            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            Assert.True(node is PlotNode); 

            expression = @"##stocks{stocks.Query(x => x.Ticker == ""XLY"" && x.Close > 120.0)} | ->=";
            //expression = @"##options{options.Prices.Where(p => p.Code == ""QQQ211217C00330000"").Select(p => p)} | ->=";
            lexicon = this._lexer.Read(expression);
            tree = await this._parser.ParseAsync(lexicon);
            node = tree[0].Eval();
            Assert.True(node is PlotNode);

        }

        [Fact]
        public async Task Join_Query_Works()
        {
            string expression = @"##stocks{stocks.Join(""SPY"",""XLY"")}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var node = tree[0].Eval();
            var qnode = (QueryNode)node;
            Assert.NotNull(qnode); 
            Assert.NotNull(qnode.query); 
            Assert.NotNull(qnode.query.EvaulatedQuery);
            Assert.True(qnode.query.EvaulatedQuery.Count() > 0);

        }
        [Fact]
        public async Task Pipes_Should_Evaluate_Correctly()
        {
            //expression = @"##stock(SPY){StockPrice,StockPrice:x0.Close += 10; return x0;}{StockPrice,StockPrice: x0.Open += 10; return x0;}{StockPrice, StockPrice: x0.Low += 10; return x0;} | ->=";
            var expression = @"##stocks{stocks.GetPrices(""XLV"", ""2023-09-01"", ""2023-12-01"")}| ->=";
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
            var expression = @"##stocks{stocks.GetPrices(""XLV"",""2020-01-01"", ""2023-11-01"")}| ->=";
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
                                  []{x0 : return Pow(x0,2);} | []{fn: return SimpsonRule.IntegrateComposite(x => fn(x), 0.0, 10.0, 4);}";

            var lexicon = this._lexer.Read(expression);
            List<TreeNode> tree = await this._parser.ParseAsync(lexicon);
            var print3 = tree[2].Print();
            Assert.True(tree.Count == 3);
            Assert.True(tree[2] is PipeNode);
            var eval = tree[2].Eval();
            Assert.True(eval is NumberNode);
            var node = (NumberNode)eval;
            var val = Math.Round(node.Value.Value, 2);
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
            Assert.True(matrixnode.matrix.Value[0,0] == 3);
            Assert.True(matrixnode.matrix.Value[1, 0] == 4);
            Assert.True(matrixnode.matrix.Value[2, 0] == 4);

        }
    }
}