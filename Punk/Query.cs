using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace Punk
{
    public class Query
    {
        public string Syntax { get; private set; }
        public string QueryStr { get; private set; }
        private string Register;
        private string Script;
        public IEnumerable<dynamic> EvaulatedLambda { get; private set; }
        Dictionary<string, string> RegisterDBContexts = new Dictionary<string, string>()
        {
            ["stocks"] = "SP500Context",
            ["options"] = "Sp500oContext"
        };
        Dictionary<string, string> RegisterConnections = new Dictionary<string, string>()
        {
            ["stocks"] = "DataContexts/SP500.db;Read Only=True;",
            ["options"] = "DataContexts/SP500O.db;Read Only=True;"
        };

        Dictionary<string, string> Includes = new Dictionary<string, string>()
        {
            ["stocks"] = "Punk.SP500StockModels",
            ["options"] = "Punk.SP500OptionModels"
        };
        public Query(string Syntax, string Register) 
        { 
            this.Syntax = Syntax;
            this.QueryStr = Syntax.Substring(1, Syntax.Length - 2); //remove starting and ending brackets
            this.Register = Register;
            this.Script = String.Empty;
            this.EvaulatedLambda = new List<object>();
        }

        public async Task EvaluateQueryAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SP500Context>();
            optionsBuilder.UseSqlite(connectionString: @"Data Source = DataContexts/SP500.db;");
            var stocks = new SP500Context(optionsBuilder.Options);
            

            this.Script =
                @$"
                  var optionsBuilder = new DbContextOptionsBuilder<{RegisterDBContexts[this.Register]}>();
                  optionsBuilder.UseSqlite(connectionString: @""Data Source = {RegisterConnections[this.Register]};"");
                  var {Register} = new {RegisterDBContexts[this.Register]}(optionsBuilder.Options);            
                  var results = await {this.QueryStr}.ToListAsync();
                    
                  ";

            var result = await CSharpScript.RunAsync(this.Script,
                                        ScriptOptions.Default.WithImports("Punk", "System", Includes[this.Register], "System.Math", "Microsoft.EntityFrameworkCore", "System.Linq").WithReferences("Punk.dll"));
            var value = result.GetVariable("results").Value;
            this.EvaulatedLambda = (IEnumerable<dynamic>)value ;

        }
    }
}

