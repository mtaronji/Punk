
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;


namespace Punk.Types
{
    //punk class for manipulating data
    //expects a string with the correct syntax
    //can init using [integerA...integerB] where integer b is greater than integer A
    //or you can just set the list with the data values
    //this list is meant to handle number data
    public class DataType
    {
        public List<List<object>> DataVectors { get; set; }
        public static Regex DataRegex = new Regex(@"([\-]?\d+(\.\d+)?)", RegexOptions.Compiled);
        public DataType(string Syntax)
        {
            DataVectors = new();

            ParseData(Syntax);
        }
        public DataType(DataType d)
        {
            DataVectors = d.DataVectors;

        }
        public static DataType operator +(DataType a, DataType b)
        {
            if (a.DataVectors.Count != 1 || b.DataVectors.Count != 1)
            {
                throw new Exceptions.PunkDataNodeException("Data vectors for addition must be degree 1");
            }

            List<List<object>> domain = new();
            domain.Add(a.DataVectors[0]); domain.Add(b.DataVectors[0]);      
            return new DataType(domain);

        }
        public int? GetDimension()
        {
            return DataVectors.Count;
        }

        public DataType(List<List<object>> domain)
        {
            DataVectors = domain;

        }


        public DataType ApplySequence(dynamic Sequence)
        {
            List<List<object>> newdomain = new();
            var dim = GetDimension();
            if (dim == 1)
            {
                List<object> transformed = new();
                transformed = DataVectors[0].Select(x =>
                {
                    return Sequence(x);
                }).ToList<object>();

                newdomain.Add(transformed);      
                return new DataType(newdomain);
            }
            else if (dim == 2)
            {     
                List<object> surface = new List<object>();
                for (int i = 0; i < DataVectors[0].Count; i++)
                {
                    var row = new List<object>();
                    for (int j = 0; j < DataVectors[1].Count; j++)
                    {
                        var eval = Sequence(DataVectors[0][i], DataVectors[1][j]);
                        row.Add(eval);
                    }
                    surface.Add(row);
                }
                newdomain.Add(surface);
                return new DataType(newdomain);
            }
            else
            {

            }

            throw new NotImplementedException("Only support R3 -> R maps");

        }
        public void ParseData(string syntax)
        {
            List<object> numbers = new List<object>();

            var matches = DataRegex.Matches(syntax);

            var domainregex = new Regex(@"\.\.\.");
            var chunks = domainregex.Split(syntax);
            if (chunks.Length > 1)
            {
                for (int i = 0; i < matches.Count; i += 2)
                {
                    long x1 = default;
                    long x2 = default;
                    string a = matches[i].Value;
                    string b = matches[i + 1].Value;
                    bool success = long.TryParse(a, out x1); if (!success) { throw new Exceptions.PunkDataTypeException("Incorrect syntax for 3 dot Data Initializer(are you using integers?"); }
                    success = long.TryParse(b, out x2); if (!success) { throw new Exceptions.PunkDataTypeException("Incorrect syntax for 3 dot Data Initializer(are you using integers?"); }
                    long span = x2 - x1;
                    if (span < 0) { throw new Exceptions.PunkDataTypeException("Start number of 3 dot initializer for Data should be less than the end number"); }
                    var iterator = x1;
                    var domain = new object[span + 1];
                    for (long j = 0; j <= span; j++)
                    {
                        domain[j] = iterator;
                        iterator++;
                    }

                    DataVectors.Add(domain.ToList());
                }
            }
            else
            {
                if (matches.Count == 0)
                {
                    //empty data 
                    //leave as is
                }
                else
                {
                    long x1;
                    double x2;
                    foreach (var match in matches)
                    {
                        var success = long.TryParse(match.ToString(), out x1);
                        if (success) { numbers.Add(x1); continue; }
                        success = double.TryParse(match.ToString(), out x2);
                        if (success) { numbers.Add(x2); continue; }
                        throw new Exceptions.PunkNumberParseException("Unable to parse value");

                    }
                    DataVectors.Add(numbers);
                }

            }
        }


    }

    public static partial class LinqExtensions
    {
        public static IEnumerable<TResult> ZipThree<T1, T2, T3, TResult>(
            this IEnumerable<T1> source,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            Func<T1, T2, T3, TResult> func)
        {
            using (var e1 = source.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            using (var e3 = third.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                    yield return func(e1.Current, e2.Current, e3.Current);
            }
        }
    }
}
