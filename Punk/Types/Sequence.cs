
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;



namespace Punk.Types
{


    public class Sequence
    {
        public int Dimension { get; private set; }
        public string SequenceSyntax;
        public string TransformString;
        public string VariableString;
        public string LambdaExpressionString;
        public string? LambdaArgs_str { get; private set; }
        public string? Delegate_args_str { get; private set; }
        public string? Delegate_Type_Str { get; private set; }
        public dynamic? SequenceTransformation { get; private set; }
        private string RosalynScript;



        public Sequence(string Syntax)
        {
            SequenceSyntax = Syntax;
            SequenceSyntax = Syntax.Replace("{", string.Empty).Replace("}", string.Empty);

            var chunks = SequenceSyntax.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            VariableString = chunks[0].Trim();
            TransformString = chunks[1].Trim();
            LambdaExpressionString = string.Empty;
            RosalynScript = string.Empty;
        }


        public async Task CreateDelegateAsync()
        {
            ParseSyntax();
            await CreateDataTransformAsync();
        }

        private void ParseSyntax()
        {
            var chunks = VariableString.Split(',');
            Dimension = chunks.Length;
            string delegate_args = string.Empty;
            string delegate_argstypes = string.Empty;
            if (Regex.IsMatch(TransformString, @"\s*return\s*"))
            {
                delegate_argstypes = "dynamic,"; //start with return arg
                Delegate_Type_Str = "Func";
            }
            else
            {
                delegate_argstypes = string.Empty;
                Delegate_Type_Str = "Action";
            }

            var lambdaArgs_str = string.Empty;  //arguments to the lambda expression
            for (int i = 0; i < chunks.Length; i++)
            {
                delegate_argstypes += $"dynamic,";
                lambdaArgs_str += $"{chunks[i]},";

            }
            delegate_argstypes = delegate_argstypes.Remove(delegate_argstypes.Length - 1, 1); //remove last comma
            lambdaArgs_str = lambdaArgs_str.Remove(lambdaArgs_str.Length - 1, 1); //remove last comma
            LambdaArgs_str = lambdaArgs_str;
            Delegate_args_str = delegate_argstypes;
        }


        public async Task CreateDataTransformAsync()
        {

            LambdaExpressionString = $"({LambdaArgs_str}) => {{{TransformString}}}";
            RosalynScript = $@"var l = new {Delegate_Type_Str}<{Delegate_args_str}>({LambdaExpressionString});";


            try
            {
                var result = await CSharpScript.RunAsync(RosalynScript,
                                        ScriptOptions.Default.WithImports("Punk", "System", "System.Math", "MathNet.Numerics.Integration").WithReferences("Punk.dll"));
                var value = result.GetVariable("l").Value;
                SequenceTransformation = value;
            }
            catch (Exception e)
            {

                throw new Exceptions.PunkSequenceException($"Sequence Syntax is incorrect {e.Message}");

            }

        }


    }
}
