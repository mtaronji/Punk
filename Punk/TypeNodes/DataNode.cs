


using Punk.Types;

namespace Punk.TypeNodes
{
    public class DataNode : TreeNode, IResultTreeNode
    {
        public DataType Value { get; set; }
        public DataNode(Token value)
        {
            this.token = value;
            this.Right = null;
            this.Left = null;
            this.Value = new DataType(this.token.Value);
        }

        public DataNode(DataType d, Token t)
        {
            this.token = t;
            this.Value = d;
            this.Right = null;
            this.Left = null;
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

        public object? GetResult()
        {
            List<Object> DataTraces = new List<Object>();
            
           
            if (this.Value.GetDimension() == 1)
            {

                foreach(var e in this.Value.DataVectors[0])
                {
                    DataTraces.Add(new { x = e });
                }
            }
            else if (this.Value.GetDimension() == 2)
            {
                var plane = this.Value.DataVectors[0].Zip(this.Value.DataVectors[1], (x,y) =>
                {
                    return new { x = x, y = y };
                }).ToList<object>();
                DataTraces = plane;

            }
            else if (this.Value.GetDimension() == 3)
            {
                var surface = this.Value.DataVectors[0].ZipThree(this.Value.DataVectors[1],this.Value.DataVectors[2],(x, y, z) =>
                {
                    return new { x = x, y = y, z =z };
                }).ToList<object>();
                DataTraces = surface;
            }
            else
            {
                throw new NotImplementedException("No support for dimensional domains greater than 3");
            }
            
            return DataTraces;
        }
    }

}