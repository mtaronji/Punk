using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Punk.Types;

namespace Punk.Extensions
{
    public static class NumberExtensions
    {
        //returns a null if unsuccessful
        public static NumberType TryParse (string value)
        {
           long x1 = default(long);
           if(long.TryParse(value, out x1))
            {
                return new NumberType(x1);
            }
           double x2 = default(double);
            if (double.TryParse(value, out x2))
            {
                return new NumberType(x2);
            }

            throw new Exceptions.PunkNumberParseException($"unable to parse value {value}");
        }

        public static NumberType Power(this NumberType n, dynamic power)
        {
            double result = Math.Pow((double)n.NumberValue, (double)power);

            if(n.NumberValue is double)
            {
                return new NumberType(result);
            }
            else if(n.NumberValue is long)
            {
                return new NumberType((int)result);
            }
            else
            {
                
                throw new NotImplementedException();
            }
        }
    }
}
