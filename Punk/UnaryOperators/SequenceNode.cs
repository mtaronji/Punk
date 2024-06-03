using Microsoft.CodeAnalysis.CSharp.Syntax;
using Punk.TypeNodes;
using Punk.Types;
using System.Xml.Linq;


namespace Punk.UnaryOperators
{
    //sequences are actions
    public class SequenceNode:TreeNode
    {
        TreeNode? Bottom;
        public enum SequenceOn
        {
            SequenceOnDataType = 1,
            SequenceOnSequence = 2,
            SequenceOnIdentifier = 3,
            SequenceOnNothing = 4
        }
        SequenceOn on;
        public DataNode? BaseData { get; private set; }
        public Sequence sequence { get; private set; }
        public SequenceNode(TreeNode SequenceOf, string Syntax)
        {
            if (SequenceOf is IdentifierNode)
            {
                this.on = SequenceOn.SequenceOnIdentifier;
            }
            else if (SequenceOf is DataNode)
            {
                var node = (DataNode)SequenceOf;
                
                this.on = SequenceOn.SequenceOnDataType;
            }
            else if (SequenceOf is SequenceNode)
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
            
            if(this.Bottom is IdentifierNode)
            {
                var id = (IdentifierNode)this.Bottom;
                if(id.Value != null) 
                {
                    this.Bottom = id.Value.Eval();
                }
                else
                {
                    throw new Exceptions.PunkSequenceException("Identifier node on the sequence has an empty value");
                }
            }
            if(this.Bottom is DataNode)
            {
                var node = (DataNode)this.Bottom;
                if(node.Value.DataVectors.Count == 0)
                {
                    //empty data so just return the sequence
                    return this;
                }
                else
                {
                    var dtype = node.Value.ApplySequence(this.sequence.SequenceTransformation);
                    Token token = new Token(TokenType.DataType, $"Transform");
                    return new DataNode(dtype,token);
                }
            }
            else if(this.Bottom is SequenceNode)
            {
                var eval = (DataNode)this.Bottom.Eval();
                var dtype = eval.Value.ApplySequence(this.sequence.SequenceTransformation);
                Token token = new Token(TokenType.DataType, $"Transform");
                return new DataNode(dtype, token);

            }
            else
            {

            }
            
            throw new NotImplementedException("Evaluation for Sequence has failed. Transformation is incorrect or unknown sequence type");

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