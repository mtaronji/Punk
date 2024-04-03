using Punk;

namespace TokenizeTests
{
    public class Lexer_Should_Tokenize
    {
       
        public Lexer_Should_Tokenize()
        {

        }
        [Fact]
        public void Punk_Should_Have_Twelve_Operators()
        {
            Assert.True(Token.GetOperatorCount() == 15);
            Assert.True(Token.GetOperatorStrings().Length == 15);
            Assert.True(Token.GetOperatorStrings().Length == Token.GetOperatorCount());
        }
       
    }
}