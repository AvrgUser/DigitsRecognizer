using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal class Node
    {
        public float activation, bias, d, cw;


        public Node(int number)
        {
            activation = 0;
            bias = 0;
        }
    }
}
