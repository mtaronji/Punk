using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punk.BinaryOperators
{
    public class InstanceFnNode : TreeNode
    {
        public TreeNode? Result { get; private set; }
        private InstanceFnFactory _instanceFnFactory;
        public TreeNode InstanceBase { get; private set; }
        public List<TreeNode> InstanceFnChain { get; private set; }

        //period base represents the object that initiates the chain
        public InstanceFnNode(TreeNode PeriodBase)
        {
            this.InstanceFnChain = new List<TreeNode>();
            this.Left = null; this.Right = null;
            this.InstanceBase = PeriodBase;
            this._instanceFnFactory = new InstanceFnFactory();
        }
        public void AddInstanceFnToChain(TreeNode n)
        {
            this.InstanceFnChain.Add(n);
        }
        public override TreeNode Eval()
        {
            TreeNode leftside = this.InstanceBase;
            foreach (TreeNode n in this.InstanceFnChain)
            {
                leftside = this._instanceFnFactory.Invoke(leftside, n);
            }
            return leftside;

        }

        public override string Print()
        {
            string InstanceFNString = "";
            foreach (var fn in this.InstanceFnChain)
            {
                InstanceFNString += fn.Print();
            }
            return $"({InstanceBase.Print()}{InstanceFNString} )";
        }
    }
}
