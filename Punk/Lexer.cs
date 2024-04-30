
using System.Text;
using System.Text.RegularExpressions;

namespace Punk
{
    public class Lexer
    {
        private PunkLexerRuleSet _ruleset;
        private List<TokenType> _tokenTypes;
        private List<string> _tokenTypesString;
        private Regex _hasOperator;
        //                                                                         
        private Regex SemanticChunksRegex =
        //            string  register      query                                                instance fn                 identifier                            matrix  data  sequence       operators   
        new Regex(@"("".*""|##[A-Za-z]+|\{[A-Za-z=>,\{\}\.\[\]<&\|0-9\*\+\-/""'\\\(\)\s;]+\}|[a-zA-Z]+[0-9]*\s*\([^\(\)\{\}]*\)|[A-Za-z]+[0-9]*|\|\|\s*[\s\d\.;-]+\s*\|\||\[.*\]|\{[^{}]*\}|\[|\]|\.\*|->=|->|\+|\-|<=|>=|>|<|\*|/|==|=|%|\)|:|,|\^|\(|\)|[0-9]+\.[0-9]+|\.|\|)");

        //do not change order
        private List<Regex> SemanticExpressions = new List<Regex>()
        {
            new Regex(@""".*"""), new Regex(@"\|\|[\s\d\.;-]+\|\|"),new Regex(@"\[.*\]"),new Regex(@"\{[^{}]*\}|\[|\]"), new Regex(@"\w+\d*"), new Regex(@"[0-9]+\.[0-9]+"),new Regex(@"[0-9]+"), new Regex(@""),
            new Regex(@"->="),new Regex(@"->"), new Regex(@"\+"), new Regex(@"\-"),new Regex(@"<="),new Regex(@">="),new Regex(@">"),new Regex(@"<"),new Regex(@"\*"),new Regex(@"/"),new Regex(@"=="),new Regex(@"="),new Regex(@"%"),
            new Regex(@"\|"),new Regex(@":"),new Regex(@","),new Regex(@"\^"),new Regex(@"\("),new Regex(@"\)"),new Regex(@"\.*")      
        };
        public Lexer()
        {
            this._ruleset = new PunkLexerRuleSet();
            this._tokenTypes = Enum.GetValues(typeof(TokenType)).Cast<TokenType>().ToList<TokenType>();
            this._tokenTypesString = this._tokenTypes.ConvertAll(t => t.ToString());
            string regexString = @"(";
            foreach (var tokenstring in this._tokenTypesString)
            {
                regexString += $@"{tokenstring}|";
            }

        }

        public Token[] Read(string language)
        {
            var tokens = new List<Token>();
            var chars = new StringReader(language);
            var sbuilder = new StringBuilder();
            char lastchar;
            bool incompleteDoubleQuote = false;
            bool incompleteBracket = false;
            //bool incompleteCurlyBrace = false;
            bool incompleteMatrix = false;
            Stack<bool> CurlyBracket = new Stack<bool>();
            //Stack<bool> Parenthesis = new Stack<bool>();
            while (chars.Peek() > -1)
            {

                //We will build only words that aren't whitespace and while we have leftover characters in the stream
                //we will keep building words from the stream if we are in the process of a quotation
                while ((!char.IsWhiteSpace((char)chars.Peek())) && (chars.Peek() > -1) || incompleteDoubleQuote || incompleteBracket || CurlyBracket.Count > 0 || incompleteMatrix)
                {
                    //if incomplete quote isn't set, check if the current character is a quote. If it is, set incomplete quote to true
                    //do this for both single quotes and double quotes and curley brackets and square brackets
                    lastchar = (char)chars.Read();
                    if (incompleteDoubleQuote == false) { incompleteDoubleQuote = (lastchar == '"'); }
                    else { if (lastchar == '"') { incompleteDoubleQuote = false; } }
                    if (incompleteBracket == false) { incompleteBracket = (lastchar == '['); }
                    else { if (lastchar == ']') { incompleteBracket = false; } }
                    if (lastchar == '{') { CurlyBracket.Push(true); } 
                    else { if (lastchar == '}') { CurlyBracket.Pop(); } }

                    //if (lastchar == '(') { Parenthesis.Push(true); }
                    //else { if (lastchar == ')') { Parenthesis.Pop(); } }

                    if (incompleteMatrix == false)
                    {
                        incompleteMatrix = (lastchar == '|') && ((char)chars.Peek() == '|');
                        if (incompleteMatrix) 
                        {
                            sbuilder.Append(lastchar);//since the matrix operator is two in the row we have to read the char from the stream
                            lastchar = (char)chars.Read();
                        }
                    }
                    else 
                    { 
                        if ((lastchar == '|') && ((char)chars.Peek() == '|')) 
                        {
                            sbuilder.Append(lastchar);//since the matrix operator is two in the row we have to read the char from the stream
                            lastchar = (char)chars.Read();
                            incompleteMatrix = false;
                        } 
                    }

                    sbuilder.Append(lastchar);

                    //if we got to the end of the stream and have incompleted unclosed groupings break;
                    if ((chars.Peek() < 0) && (incompleteDoubleQuote || incompleteBracket || CurlyBracket.Count > 0 || incompleteMatrix))
                    {
                        break;
                    }
                }
                if (sbuilder.Length > 0)
                {
                    var splitresults = CreateSemanticChunks(sbuilder.ToString());
                    foreach (var splitresult in splitresults)
                    {
                        var token = ReadBlock(splitresult);
                        tokens.Add(token);
                    }
                    sbuilder.Length = 0;
                }
                //pull the whitespace block from the stream
                chars.Read();
            }
            chars.Close();
            return tokens.ToArray();
        }
        private Token ReadBlock(string Block)
        {

            foreach (var tokentype in this._tokenTypes)
            {
                if (_ruleset.RuleSet[tokentype].IsMatch(Block))
                {
                    return new Token(tokentype, Block.Trim('"').Trim('\''));
                }
            }

            return new Token(TokenType.UnknownType, Block);
        }

        //Check if an operator is wedged in the block
        //if there is no operator we will return a string array with one element
        private string[] CreateSemanticChunks(string block)
        {
            //order for this regex matters. Check bigger expressions that contain smaller expressions first
            //check for matrix, string, data, sequence, brackets(curly and square) recursive operator, map operator, and other common operators and seperators

            return Regex.Split(block, this.SemanticChunksRegex.ToString()).Where(sc => sc != String.Empty).ToArray();
            
        }

    }
}
