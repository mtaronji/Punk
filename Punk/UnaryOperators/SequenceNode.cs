using Microsoft.CodeAnalysis.CSharp.Syntax;
using Punk.TypeNodes;


namespace Punk.UnaryOperators
{
    //sequences are actions
    public class SequenceNode:TreeNode
    {
        TreeNode? Bottom;
        public enum SequenceOn
        {
            SequenceOnDataType = 1,
            SequenceOnSequence = 2
        }
        SequenceOn on;
        public DataNode? BaseData { get; private set; }
        public Sequence sequence { get; private set; }
        public SequenceNode(TreeNode SequenceOf, string Syntax)
        {

            if(SequenceOf is DataNode)
            {
                this.on = SequenceOn.SequenceOnDataType;
            }
            else if(SequenceOf is SequenceNode)
            {
                this.on = SequenceOn.SequenceOnSequence;
            }
            else
            {
                throw new NotImplementedException("Sequences can be made of registers, user data, or other sequences");            
            }
            this.Bottom = SequenceOf;
            this.Left = null; this.Right = null;
            var Sequence = new Sequence(Syntax);
            this.sequence = Sequence;
        }


        //sequence will create tree with a value
        public override TreeNode Eval()
        {
            //for sequences that are compositions of other sequences, we must evaluate the root sequence first then evaluate upwards
            //The numbers field is the calculation
            if(this.Bottom is DataNode)
            {
                var node = (DataNode)this.Bottom;
                node.Value.ApplySequence(this.sequence.SequenceTransformation);
                return node;
            }
            else if(this.Bottom is SequenceNode)
            {
                var eval = (DataNode)this.Bottom.Eval(); 
                eval.Value.ApplySequence(this.sequence.SequenceTransformation);
                return eval;
            }
            else
            {

            }
            
            throw new NotImplementedException("Can't evaluate a sequence on unknown type");

        }
        public override string Print()
        {
            if(this.Bottom != null)
            {
                return $"({this.on.ToString()} {this.Bottom.Print()})";
            }
            else
            {
                return "";
            }
            
        }
    }
}


//       [a,b,c]{x0: return x + 2;}{x0: return x - 2}
//                               
//                                  seq
//                                   |
//                                  seq
//                                   |
//                                  data
//                       
//                       
//      ##stock(spy){select, args1,arg2,...,argn : statement; }{where, select, args1,arg2,...,argn : statement; }{Groupby, select, args1,arg2,...,argn: statement;}