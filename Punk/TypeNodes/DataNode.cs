


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

        public object? GetResult()
        {
            List<Object> DataTraces = new List<Object>();
            if (this.Value.TransformedSequence == null)
            {
                if (this.Value.GetDimension() == 1)
                {
                    DataTraces = this.Value.DataVectors[0].Select(x =>
                    {
                        return new { x = x };
                    }).ToList<object>();
                }
                else if (this.Value.GetDimension() == 2)
                {

                    DataTraces = this.Value.DataVectors[0].Zip(this.Value.DataVectors[1], (x,y) =>
                    {
                        return new { x = x, y = y };
                    }).ToList<object>();
                }
                return DataTraces;
                
            }
           
            if (this.Value.GetDimension() == 1)
            {
                DataTraces = this.Value.DataVectors[0].Zip(this.Value.TransformedSequence, (x, y) =>
                {
                    return new { x = x, y = y };
                }).ToList<object>();
            }
            else if (this.Value.GetDimension() == 2)
            {
                var Surface = this.Value.DataVectors[0].ZipThree(this.Value.DataVectors[1], this.Value.TransformedSequence, (x,y,z) =>
                {
                    return new { x = x, y = y, z = z };
                }).ToList<object>();
                DataTraces = Surface;

            }
            else if (this.Value.GetDimension() == 3)
            {
                var range = (List<object>)this.Value.TransformedSequence;
                DataTraces = range.Select(x =>
                {
                    return new { z = x };
                }).ToList<object>();
            }
            else
            {
                throw new NotImplementedException("No support for dimensional domains greater than 3");
            }
            
            return DataTraces;
        }
    }

}