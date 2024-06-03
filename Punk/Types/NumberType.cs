using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punk.Types
{
    //All numbertypes should inherit from number

    public class NumberType
    {
        public dynamic NumberValue { get; set; }
        public NumberType(dynamic value)
        {
            NumberValue = value;
        }
        public NumberType(string stringnumber)
        {
            long x1 = default;
            double x2 = default;
            if (long.TryParse(stringnumber, out x1))
            {
                NumberValue = x1;
            }
            else if (double.TryParse(stringnumber, out x2))
            {
                NumberValue = x2;
            }
            else
            {
                throw new ArgumentException("Type of number is not supported");
            }
        }
        public static NumberType operator +(NumberType x1, NumberType x2)
        {
            return new NumberType(x1.NumberValue + x2.NumberValue);
        }
        public static NumberType operator *(NumberType x1, NumberType x2)
        {
            return new NumberType(x1.NumberValue * x2.NumberValue);
        }
        public static NumberType operator /(NumberType x1, NumberType x2)
        {
            return new NumberType(x1.NumberValue / x2.NumberValue);
        }
        public static NumberType operator -(NumberType x1, NumberType x2)
        {
            return new NumberType(x1.NumberValue - x2.NumberValue);
        }
        public override string ToString()
        {
            return NumberValue.ToString();
        }
    }

}

//A pair of references exhibit referential equality when both references point to the same object. By default, the == and != operators will compare two reference-type variables by reference.
//However, it is occasionally more natural for the == and != operators to exhibit value equality, whereby the comparison is based on the value of the objects that the references point to.

//Whenever overloading the == and != operators, you should always override the virtual Equals method to route its functionality to the == operator. This allows a class to be used polymorphically
//(which is essential if you want to take advantage of functionality such as the collection classes). It also provides compatibility with other .NET languages that don’t overload operators.
