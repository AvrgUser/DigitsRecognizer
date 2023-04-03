using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal class Net
    {
        public Node[][] nodes;

        public Node[] input, output;

        public static float step = 0.0003f, avgEr = 0;

        public Net(params int[] nodeAms)
        {
            nodes = new Node[nodeAms.Length][];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new Node[nodeAms[i]];
                for (int j = 0; j < nodes[i].Length; j++)
                {
                    nodes[i][j] = new Node();
                }
            }
            input = new Node[nodes[0].Length];
            output = new Node[nodes[nodes.Length-1].Length];
            for (int i = 0;i < nodes[0].Length; i++)
            {
                input[i] = nodes[0][i];
            }
        }

        public void Transform(float[] inputActs)
        {
            foreach (var node in nodes)
            {
                
            }
        }

        public void BackProp(float[] a)
        {
            //RecDerivatives(a);
            for (int i = layers.Length - 1; i > 0; i--)
            {
                layers[i].BackProp(step*(MS.Sigmoid(avgEr)+0.3f));
            }
        }

        public void RecDerivatives(float[] a)
        {
            for (int i = 0; i < output.nodes.Length; i++)
            {
                output[i].d = a[i]*MS.SigmoidDerS(output[i].activation);
                //Console.WriteLine(output[i].d);
            }
            for (int i = layers.Length - 1; i > 0; i--)
            {
                layers[i].RecDerivatives();
            }
        }
    }
}
