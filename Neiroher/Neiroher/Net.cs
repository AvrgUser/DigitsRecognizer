using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal class Net
    {
        public Node[] nodes;

        public int[] input, output;

        public List<int> ready, done;

        public static float step = 0.0003f, avgEr = 0;

        private static Net instance;

        public static Net Instance { get { return instance; } }

        public Net(params int[][] nodeAms)
        {
            instance = this;
            int amount = 0;
            for (int i = 0; i < nodeAms.Length; i++)
            {
                for (int j = 0; j < nodeAms[i].Length; j++)
                {
                    amount++;
                }
            }
            nodes = new Node[amount];
            for (int i = 0; i < nodeAms.Length;i++)
            {
                var layer = nodeAms[i];
                for(int j = 0; j < layer.Length;j++)
                {
                    var node = layer[j];
                    
                }
            }
            
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new Node[nodeAms[i]];
                for (int j = 0; j < nodes[i].Length; j++)
                {
                    nodes[i][j] = new Node();
                }
            }
            input = new int[nodeAms[0].Length];
            output = new int[nodes[nodes.Length-1].Length];
            for (int i = 0;i < nodes[0].Length; i++)
            {
                input[i] = nodes[0][i];
            }
        }

        public void Transform(float[] inputActs)
        {
            ready = new List<int>();
            done = new List<int>();
        }

        public void BackProp(float[] a)
        {
        }

        public void RecDerivatives(float[] a)
        {
        }

        public void AddReady(int number)
        {
            ready.Add(number);
        }
    }
}
