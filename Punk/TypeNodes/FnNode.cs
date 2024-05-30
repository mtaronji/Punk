using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Punk.TypeNodes
{
    public class FnNode : TreeNode
    {      
        public string FNId { get; private set; }
        public string[]? Args { get; private set; }
        public Dictionary<string, IdentifierNode> ParserIdentifiers { get; private set; }
        public FnNode(Token value, Dictionary<string, IdentifierNode> Identifiers) 
        { 
            this.ParserIdentifiers = Identifiers;
            this.token = value;
            this.Left = null; this.Right = null;

            var chunks = value.Value.Split(new char[] { '(', ')' },StringSplitOptions.RemoveEmptyEntries);
            FNId = chunks[0];
            
            if (chunks.Length > 1 )
            {
                this.Args = chunks[1].Split(",");
            }
        }
        public override TreeNode Eval()
        {
            return this;
        }

        public override string Print()
        {
            if(this.token != null)
            {
                return $"{this.token.Value}";
            }
            else { return ""; }
        }
    }
}
