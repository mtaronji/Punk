using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punk.BinaryOperators
{
    public class MapOperator : TreeNode
    {
        public MapOperator(TreeNode A, TreeNode B)
        {
            this.Right = B;
            this.Left = A;
        }

        public override TreeNode Eval()
        {
            throw new NotImplementedException();
        }

        public override string Print()
        {
            throw new NotImplementedException();
        }
    }
}
