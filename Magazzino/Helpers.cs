using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazzino
{
    public class Helpers
    {
        public static int CheckInt()
        {
            bool isInt = true;
            int num;
            do
            {
                isInt = int.TryParse(Console.ReadLine(), out num);
            } while (!isInt);
            return num;
        }

        public static decimal CheckDecimal()
        {
            bool isDecimal = true;
            decimal num;
            do
            {
                isDecimal = decimal.TryParse(Console.ReadLine(), out num);
            } while (!isDecimal);
            return num;
        }
    }
}
