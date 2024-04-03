
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Punk.TypeNodes
{
    //register nodes should parse the language for the datastore as well as the particular item you want to search for in the datastore
    //these items are all contained in IDataregister
    public class RegisterNode:TreeNode
    {
        public string RegisterItem { get; set; }
        public string Register { get; set; }
        public List<dynamic> Transforms { get; set; }
        private Regex registeritemregex = new Regex(@"\([A-Za-z]+\)");
        private Regex registerregex = new Regex(@"[A-Za-z]+[^\(]");
        public RegisterNode(string register)
        {
            Transforms = new List<dynamic>();

            registerregex.Match(register);
            registeritemregex.Match(register);
           
            var matchesregister = registerregex.Match(register);
            var matchesregisteritem = registeritemregex.Match(register);

            if (matchesregister.Success)
            {
                this.Register = matchesregister.Value;
                this.RegisterItem = matchesregisteritem.Value;
            }
            else 
            {
                throw new Exceptions.PunkRegisterException("Unable to parse the register and/or register item. Please check your spelling");
            }
        }
        public void AddLambda(dynamic fn)
        {
            this.Transforms.Add(fn);
        }
        public void SetRegisterItem(StringNode node)
        {
            this.RegisterItem = node.Value;
        }
        public override string Print()
        {
            return $"({this.Register}Register)";
        }
        public override TreeNode Eval()
        {
            return this;
        }
    }
}

//syntax 
///
/// "SPY" | ##stock get data on SPY  from the stock data register
/// "10Y2" | ##FRED get data on 10 year vs 2 year from FRED data register
///NKE220624C00099000 | ##Option  get data on NKE call option from option data register
