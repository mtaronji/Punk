using Punk.Extensions;
using Punk2.UnaryOperators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Punk.TypeNodes
{
    public class DataNode : TreeNode
    {
        public DataType Value { get; set; }
        public DataNode(Token value)
        {
            this.token = value;
            this.Right = null;
            this.Left = null;
            this.Value = new DataType(this.token.Value);
        }

        public override TreeNode Eval()
        {
            if (this.Value == null) { throw new Exceptions.PunkDataNodeException("Unable to Parse Data. Fatal Error"); }
            
            return this;
        }

        public List<object> GetData()
        {
            List<object> data = new List<object>();
            foreach(var d in this.Value.DataVectors)
            {
                data.Add(d);
            }
            if(this.Value.TransformedSequence != null)
            {
                data.Add(this.Value.TransformedSequence);
            }
            return data;          
        }
        public override string Print()
        {
            if (this.token != null)
            {
                return $"{this.token.Value}";
            }
            else
            {
                return "";
            }
        }
    }

}