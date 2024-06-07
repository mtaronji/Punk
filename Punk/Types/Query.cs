using MathNet.Numerics.LinearAlgebra;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Punk.Types
{
    public class Query
    {
        public string Syntax { get; private set; }
        public string QueryStr { get; private set; }
        private string Register;
        private string Script;
        public dynamic? EvaulatedQuery { get; private set; }
        Dictionary<string, string> RegisterDBContexts = new Dictionary<string, string>()
        {
            ["stocks"] = "SP500Context",
            ["fred"] = "FredContext"
        };
        Dictionary<string, string> RegisterConnections = new Dictionary<string, string>()
        {
            ["stocks"] = "DataContexts/SP500.db;Read Only=True;",
            ["fred"] = "DataContexts/FRED.db;Read Only=True;"
        };

        Dictionary<string, string> Includes = new Dictionary<string, string>()
        {
            ["stocks"] = "Punk.SP500StockModels",
            ["fred"] = "Punk.FREDDataModels"
        };
        public Query(string Syntax, string Register)
        {
            this.Syntax = Syntax;
            QueryStr = Syntax.Substring(1, Syntax.Length - 2); //remove starting and ending brackets
            this.Register = Register;
            Script = string.Empty;
            EvaulatedQuery = null;
        }

        public async Task EvaluateQueryAsync()
        {

            Script =
                @$"
                  var optionsBuilder = new DbContextOptionsBuilder<{RegisterDBContexts[Register]}>();
                  optionsBuilder.UseSqlite(connectionString: @""Data Source = {RegisterConnections[Register]};"");
                  var {Register} = new {RegisterDBContexts[Register]}(optionsBuilder.Options);            
                  var results = await {this.Register}.{QueryStr};
                    
                  ";

            try
            {
                var result = await CSharpScript.RunAsync(Script,
                                            ScriptOptions.Default.WithImports("Punk", "System", Includes[Register], "System.Math", "Microsoft.EntityFrameworkCore", "System.Linq", "MathNet.Numerics.LinearAlgebra").WithReferences("Punk.dll"));
                var value = result.GetVariable("results").Value;
                
                EvaulatedQuery = value as List<object>; if(EvaulatedQuery != null) { return; }
                EvaulatedQuery = value as Matrix<double>; if (EvaulatedQuery != null) { return; }
                throw new Exceptions.PunkQueryException("Unable to parse Query. Please check syntax");
            }

            catch (Exception ex)
            {
                throw new Exceptions.PunkQueryException($"Error in Query Syntax {ex.Message}");
            }

        }
    }
}

