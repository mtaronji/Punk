using Punk;

namespace LexerRuleTests
{
    public class Lexer_Rules_Should_Work
    {
        private Lexer _lexer;
        public Lexer_Rules_Should_Work()
        {
            this._lexer = new Lexer();
        }

        [Fact]
        public void string_type_should_work()
        {
            string teststring = @"""var1""";
            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.StringType);
            Assert.True(tokens[0].Value == "var1");

            teststring = @"""var1 """;
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.StringType);
            Assert.True(tokens[0].Value == "var1 ");

            teststring = @"string stockname = ""SPY""  \n";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.StringType);
            Assert.True(tokens[3].Value == "SPY");

            teststring = @"string stockname = ""^&SPY""  \n";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.StringType);

            teststring = @"string stockname = ""^1673SPY""  \n";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.StringType);

            //Strings cannot contain specialsymbols anywhere
            teststring = @"string stockname = ""SPY&&""  \n";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.StringType);

            teststring = @"string stockname = 'SPY&&'  \n";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.UnknownType);

            teststring = @"string stockname = 'SPY&& this is an ""Interested"" thing  '";
            tokens = this._lexer.Read(teststring);
            System.Diagnostics.Debug.WriteLine("SPY&& this is an \"Interested\" thing  ");
            Assert.True(tokens[3].TokenType == TokenType.UnknownType);
        }

        [Fact]
        public void Unknown_type_Should_Work()
        {
            string teststring = @"&&&F";
            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.UnknownType);

            teststring = "string stockname = %$###@  \n";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.ModuloType);
            Assert.True(tokens[4].TokenType == TokenType.UnknownType);
            Assert.True(tokens.Length == 5);
        }

        [Fact]
        public void Number_Type_Should_Work()
        {
            string teststring = @"10";
            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.NumberType);

            teststring = @"10.2";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.NumberType);

            teststring = @"x = ""SPY"" 10.2";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.NumberType);
        }

        [Fact]
        public void Operator_Types_Should_Work()
        {
            string teststring = @"x = 40
                                  x > 20
                                  x >= 40 
                                  y = 20.1
                                  y <= x
                                  y < x
                                  y == x
                                  z = (5*x)
                                  z = (5/x)";

            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.IdentityfierType);
            Assert.True(tokens[1].TokenType == TokenType.AssignType);
            Assert.True(tokens[2].TokenType == TokenType.NumberType);
            Assert.True(tokens[4].TokenType == TokenType.IsGreaterThanType);
            Assert.True(tokens[7].TokenType == TokenType.IsGreaterThanEqualType);
            Assert.True(tokens[10].TokenType == TokenType.AssignType);
            Assert.True(tokens[11].TokenType == TokenType.NumberType);
            Assert.True(tokens[13].TokenType == TokenType.IsLessThanEqualType);
            Assert.True(tokens[16].TokenType == TokenType.IsLessThanType);
            Assert.True(tokens[19].TokenType == TokenType.IsEqualType);
            Assert.True(tokens[23].TokenType == TokenType.LParenthesisType);
            Assert.True(tokens[24].TokenType == TokenType.NumberType);
            Assert.True(tokens[25].TokenType == TokenType.MultiplicationType);
            Assert.True(tokens[26].TokenType == TokenType.IdentityfierType);
            Assert.True(tokens[27].TokenType == TokenType.RParenthesisType);

            teststring = @"x = (50*10)";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.LParenthesisType);
            Assert.True(tokens[3].TokenType == TokenType.NumberType);
            Assert.True(tokens[4].TokenType == TokenType.MultiplicationType);
            Assert.True(tokens[5].TokenType == TokenType.NumberType);
            Assert.True(tokens[6].TokenType == TokenType.RParenthesisType);

            teststring = @"z=(4%5*10)";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.IdentityfierType);
            Assert.True(tokens[1].TokenType == TokenType.AssignType);
            Assert.True(tokens[2].TokenType == TokenType.LParenthesisType);
            Assert.True(tokens[3].TokenType == TokenType.NumberType);
            Assert.True(tokens[4].TokenType == TokenType.ModuloType);
            Assert.True(tokens[5].TokenType == TokenType.NumberType);
            Assert.True(tokens[6].TokenType == TokenType.MultiplicationType);
            Assert.True(tokens[7].TokenType == TokenType.NumberType);
            Assert.True(tokens[8].TokenType == TokenType.RParenthesisType);

            teststring = @"z=(4==5/10+50.5-4.5% <=ty>=&&T| |)";        
            tokens = this._lexer.Read(teststring);
            
            Assert.True(tokens[0].TokenType == TokenType.IdentityfierType);
            Assert.True(tokens[1].TokenType == TokenType.AssignType);
            Assert.True(tokens[2].TokenType == TokenType.LParenthesisType);
            Assert.True(tokens[3].TokenType == TokenType.NumberType);
            Assert.True(tokens[4].TokenType == TokenType.IsEqualType);
            Assert.True(tokens[5].TokenType == TokenType.NumberType);
            Assert.True(tokens[6].TokenType == TokenType.DivideType);
            Assert.True(tokens[7].TokenType == TokenType.NumberType);
            Assert.True(tokens[8].TokenType == TokenType.AddType);
            Assert.True(tokens[9].TokenType == TokenType.NumberType);
            Assert.True(tokens[10].TokenType == TokenType.SubtractType);
            Assert.True(tokens[11].TokenType == TokenType.NumberType);
            Assert.True(tokens[12].TokenType == TokenType.ModuloType);
            Assert.True(tokens[13].TokenType == TokenType.IsLessThanEqualType);
            Assert.True(tokens[14].TokenType == TokenType.IdentityfierType);
            Assert.True(tokens[15].TokenType == TokenType.IsGreaterThanEqualType);
            Assert.True(tokens[16].TokenType == TokenType.UnknownType);
            Assert.True(tokens[17].TokenType == TokenType.IdentityfierType);
            Assert.True(tokens[18].TokenType == TokenType.PipeType);
            Assert.True(tokens[19].TokenType == TokenType.PipeType);

        }

        [Fact]
        public void Data_Type_Should_Work()
        {
            string teststring = @"y = [65,a,590]";

            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.UnknownType);

            teststring = @"x = [50,10,-0.7]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.DataType);

            teststring = @"x = [ 50 , 10, -0.7 ]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.DataType);

            teststring = @"x = [-20...20]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.DataType);

            teststring = @"x = [20...20.0]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.UnknownType);

            teststring = @"x = [20.0...20]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.UnknownType);

            teststring = @"x = [20.0...30.0]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.UnknownType);

            teststring = @"x = [-10...10,-10...10]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.DataType);

            teststring = @"x = [-20...10, -5...5 ]";
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.DataType);

        }

        [Fact]
        public void Sequence_Type_Should_Work()
        {
            string teststring = @"y = [65,17.6,590]{number,number : x * 2}";

            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.DataType);
            Assert.True(tokens[3].TokenType == TokenType.SequenceType);

            teststring = @"y = [65,17.6,590]{stock1,stock1 : x * 2}"; 

            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[3].TokenType == TokenType.SequenceType);

        }

        [Fact]
        public void Matrix_Type_Should_Work()
        {
            string teststring = @"y = ||65 17.6 590;||";

            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.MatrixType);          

            teststring = @"y = || -65 17.6 -590;
                                                ||";

            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.MatrixType);

            teststring = @"y = || -65 17.6 -590;
                                    1    3    5;
                                              ||";

            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[2].TokenType == TokenType.MatrixType);

            teststring = @"y3 =  || -65 17.6 -590;
                                      1    3    5
                                                ||";

            tokens = this._lexer.Read(teststring);
            Assert.False(tokens[2].TokenType == TokenType.MatrixType);

        }

        [Fact]
        public void Register_Type_Should_Work()
        {
            string teststring = @"#stock()";

            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.UnknownType);

            teststring = @"##option";
                                              
            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.RegisterType);

        }

        [Fact]
        public void Plot_Type_Should_Work()
        {
            string teststring = @"##stock | ->= ";

            var tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.RegisterType);
            Assert.True(tokens[1].TokenType == TokenType.PipeType);
            Assert.True(tokens[2].TokenType == TokenType.PlotType);

            teststring = @"##fred ##stock ""SPY"" ->=
            {x : x * 2}";


            tokens = this._lexer.Read(teststring);
            Assert.True(tokens[0].TokenType == TokenType.RegisterType);
            Assert.True(tokens[1].TokenType == TokenType.RegisterType);
            Assert.True(tokens[2].TokenType == TokenType.StringType);
            Assert.True(tokens[3].TokenType == TokenType.PlotType);
            Assert.True(tokens[4].TokenType == TokenType.SequenceType);
        }

    }
}