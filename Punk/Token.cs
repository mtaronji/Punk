
namespace Punk
{
    /*////
    For any new operators make sure you also add the new operator to newoperatortoken types in the Token class at the bottom

    */
    public enum TokenCategory
    {
        KeyWordTokenType = 1,
        OperatorTokenType = 2,
        SeperatorTokenType = 3,
        UnknownType = 4
    }
    //unknown type must come last
    public enum TokenType
    {
        PickType = 1,
        SurfType = 2,
        NumberType = 3,
        StringType = 4,
        DataSetType = 5,
        DataType = 6,
        FunctionType = 7,
        ReturnType = 8,
        ReadType = 9,
        OpenType = 10,
        GetType = 11,
        WhileType = 12,
        WalkType = 13,
        PipeType = 14,
        AddType = 15,
        SubtractType = 16,
        DivideType = 17,
        MultiplicationType = 18,
        ModuloType = 19,
        AssignType = 20,
        IsEqualType = 21,
        IsGreaterThanType = 22,
        IsGreaterThanEqualType = 23,
        IsLessThanType = 24,
        IsLessThanEqualType = 25,
        LBracketType = 26,
        RBracketType = 27,
        LParenthesisType = 28,
        RParenthesisType = 29,
        LCurlybraceType = 30,
        RcurlybraceType = 31,
        CommaType = 32,
        IdentityfierType = 33,
        MapType = 34,
        PlotType = 35,
        SequenceType = 36,
        SuchThatType = 37,
        PeriodType = 38,
        TrailingDotType = 39,
        ExponentialType = 40,
        MatrixType = 41,
        RegisterType = 42,
        QueryType = 43,
        UnknownType = 44
    }

    public sealed class Token
    {
        //public TokenCategory TokenCategory { get; private set; }
        public TokenType TokenType { get; private set; }
        public string Value { get; private set; }

        public Token(TokenType type, string value) 
        {
            //this.TokenCategory = category;
            this.TokenType = type;
            this.Value = value;
        }

        public static TokenType[] OperatorTokenTypes = {
            TokenType.MultiplicationType,TokenType.AddType, TokenType.SubtractType, TokenType.DivideType,
            TokenType.IsGreaterThanType, TokenType.IsGreaterThanEqualType, TokenType.IsLessThanType, TokenType.IsLessThanEqualType, TokenType.IsEqualType,
            TokenType.AssignType, TokenType.ModuloType, TokenType.PipeType, TokenType.MapType, TokenType.PlotType,TokenType.ExponentialType
        };

        public static string[] OperatorsStringForm = { "^", "*", "+", "-", "/", ">", ">=", "<", "<=", "==", "=", "%", "|", "->", "->=" };
        public static string[] GetOperatorStrings() { return OperatorsStringForm; }
        public static int GetOperatorCount() { return OperatorTokenTypes.Length; }


        public static bool IsOperatorTokenType(TokenType token)
        {
            return Token.OperatorTokenTypes.Any(t => t == token);
        }
    }

}
