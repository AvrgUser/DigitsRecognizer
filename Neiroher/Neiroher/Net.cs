using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal class Net
    {
        public Layer[] layers;

        public Layer input, output;

        public float step = 0.02f, avgEr = 0;

        public Net(params int[] nodeAms)
        {
            layers = new Layer[nodeAms.Length];
            for (int i = 0; i < nodeAms.Length; i++)
            {
                Layer newOne;
                newOne = new Layer(nodeAms[i], i > 0 ? layers[i - 1] : null);
                layers[i] = newOne;
                newOne.number = i;
                newOne.all = layers.Length;
            }
            input = layers[0];
            output = layers[^1];
            output.bias = false;
        }

        public float[] Transform(float[] inputActs)
        {
            if (inputActs.Length != input.nodes.Length)
            {
                throw new ArgumentException("wrong input");
            }
            for (int i = 0; i < input.nodes.Length; i++)
            {
                input[i].activation = inputActs[i];
            }
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i].Activate();
            }
            float[] outputActs = new float[output.nodes.Length];
            for (int i = 0; i < output.nodes.Length; i++)
            {
                outputActs[i] = output[i].activation;
            }
            return outputActs;
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
