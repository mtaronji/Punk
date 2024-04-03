
using System.Text.RegularExpressions;


namespace Punk
{
    internal class PunkLexerRuleSet
    {
        public Dictionary<TokenType, Regex> RuleSet { get; private set; }
        public PunkLexerRuleSet()
        {
            //init rule set for the language
            RuleSet = new Dictionary<TokenType, Regex>();

            RuleSet[TokenType.PickType] = new Regex(@"^pick$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.SurfType] = new Regex(@"^surf$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.NumberType] = new Regex(@"^-?[0-9]+\.?[0-9]*$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.StringType] = new Regex(@"^""{1}.*""{1}$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.DataSetType] = new Regex(@"^dataset$", RegexOptions.IgnoreCase);
            //RuleSet[TokenType.CodeType] = new Regex(@"^##\w+(#\w+\d*)*$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.RegisterType] = new Regex(@"^##[A-Za-z]+$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.MatrixType] = new Regex(@"^\|\|(\s*[\-]?\d+(\.\d+)?(\s*[\-]?\d+(\.\d+)?)*\s*;\s*)+\|\|$", RegexOptions.IgnoreCase);


            //                                               -------========Group 1 ------------------------- ------- group 2------------------------
            RuleSet[TokenType.DataType] = new Regex(@"^\[\s*((([\-]?\d+\s*\.{3}\s*\d+\s*,?\s*)+)|(([\-]?\d+(\.\d+)?)(\s*,\s*([\-]?\d+(\.\d+)?))*))\s*\]$", RegexOptions.Compiled);
            RuleSet[TokenType.SequenceType] = new Regex(@"^\{\s*([A-Za-z0-9\.])+\s*(,\s*[A-Za-z0-9\.]+)*\s*:[;\s\(\)\+\-\*\^/=<>a-zA-Z0-9\.,]+\}$", RegexOptions.Compiled);
            RuleSet[TokenType.QueryType] = new Regex(@"^\{[A-Za-z=>,\{\}\.\[\]<&\|0-9\*\+\-/""'\\\(\)\s;]+\}$", RegexOptions.Compiled);

            RuleSet[TokenType.FunctionType] = new Regex(@"^function$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.ReturnType] = new Regex(@"^return$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.ReadType] = new Regex(@"^read$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.OpenType] = new Regex(@"^open$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.GetType] = new Regex(@"^get$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.WhileType] = new Regex(@"^while$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.WalkType] = new Regex(@"^walk$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.PipeType] = new Regex(@"^\|$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.AddType] = new Regex(@"^\+$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.SubtractType] = new Regex(@"^-$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.DivideType] = new Regex(@"^/$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.MultiplicationType] = new Regex(@"^\*$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.ExponentialType] = new Regex(@"^\^$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.ModuloType] = new Regex(@"^%$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.AssignType] = new Regex(@"^=$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.IsEqualType] = new Regex(@"^==$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.IsGreaterThanType] = new Regex(@"^>$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.IsGreaterThanEqualType] = new Regex(@"^>=$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.IsLessThanType] = new Regex(@"^<$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.IsLessThanEqualType] = new Regex(@"^<=$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.MapType] = new Regex(@"^->$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.PlotType] = new Regex(@"^->=$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.LBracketType] = new Regex(@"^\[$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.RBracketType] = new Regex(@"^]$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.LParenthesisType] = new Regex(@"^\($", RegexOptions.IgnoreCase);
            RuleSet[TokenType.RParenthesisType] = new Regex(@"^\)$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.LCurlybraceType] = new Regex(@"^{$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.RcurlybraceType] = new Regex(@"^}$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.CommaType] = new Regex(@"^,$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.SuchThatType] = new Regex(@"^:$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.PeriodType] = new Regex(@"^\.$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.TrailingDotType] = new Regex(@"^\.\.\.$", RegexOptions.IgnoreCase);
            RuleSet[TokenType.UnknownType] = new Regex(@".", RegexOptions.Singleline);
            RuleSet[TokenType.IdentityfierType] = new Regex(@"^[a-zA-Z]+[a-zA-Z0-9]*()$", RegexOptions.Singleline);
        }
    }
}

