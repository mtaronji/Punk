
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;



namespace Punk
{
    
    
    public class Sequence
    {
        public List<Type>? Args { get; private set; }
        public string SequenceSyntax;
        public string TransformString;
        public string VariableString;
        public string LambdaExpressionString;
        public string? LambdaArgs_str { get; private set; }
        public string? FnArgs_str { get; private set; }
        public dynamic? SequenceTransformation { get; private set; }
        private string RosalynScript;



        public Sequence(string Syntax)
        {
            this.SequenceSyntax = Syntax;
            this.SequenceSyntax = Syntax.Replace("{", String.Empty).Replace("}", String.Empty);

            var chunks = SequenceSyntax.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            this.VariableString = chunks[0].Trim();
            this.TransformString = chunks[1].Trim();
            this.LambdaExpressionString = string.Empty;
            this.RosalynScript = string.Empty;
        }


        public async Task CreateDelegateAsync()
        {
            ParseSyntax();
            await CreateDataTransformAsync();
        }

        private void ParseSyntax()
        {
            var chunks = this.VariableString.Split(',');
            var fn_args = String.Empty;//all arguments to a func delegate
            var lambdaArgs_str = String.Empty;  //arguments to the lambda expression
            for (int i = 0; i < chunks.Length; i++)
            {
                fn_args += $"{chunks[i]},";
                //all 1 argument syntax automatically have 1 argument. For argument syntax with more than 1 arg, the last one is a return and shouldn't be counted
                if (i == 0 || i < chunks.Length - 1)
                {
                    lambdaArgs_str += $"x{i},";
                }
            }
            fn_args = fn_args.Remove(fn_args.Length - 1, 1); //remove last comma
            lambdaArgs_str = lambdaArgs_str.Remove(lambdaArgs_str.Length - 1, 1); //remove last comma
            this.LambdaArgs_str = lambdaArgs_str;
            this.FnArgs_str = fn_args;
        }

        private void ParseRegisterSyntax()
        {

            var chunks = this.VariableString.Split(',');
            var fn_args = String.Empty;//all arguments to a func delegate
            var lambdaArgs_str = String.Empty;  //arguments to the lambda expression
            for (int i = 0; i < chunks.Length; i++)
            {
                fn_args += $"{chunks[i]},";
                //all 1 argument syntax automatically have 1 argument. For argument syntax with more than 1 arg, the last one is a return and shouldn't be counted
                if (i == 0 || i < chunks.Length - 1)
                {
                    lambdaArgs_str += $"x{i},";
                }
            }
            fn_args = fn_args.Remove(fn_args.Length - 1, 1); //remove last comma
            lambdaArgs_str = lambdaArgs_str.Remove(lambdaArgs_str.Length - 1, 1); //remove last comma
            this.LambdaArgs_str = lambdaArgs_str;
            this.FnArgs_str = fn_args;
        }

        public async Task CreateDataTransformAsync()
        {

            this.LambdaExpressionString = $"({this.LambdaArgs_str}) => {{{this.TransformString}}}";
            this.RosalynScript = $@"var l = new Func<{FnArgs_str}>({this.LambdaExpressionString});";
            var result = await CSharpScript.RunAsync(this.RosalynScript,
                                        ScriptOptions.Default.WithImports("Punk", "System", "System.Math").WithReferences("Punk.dll"));
            var value = result.GetVariable("l").Value;
            this.SequenceTransformation = value;

        }

     
    }
}
