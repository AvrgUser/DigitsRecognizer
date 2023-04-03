using System;
using System.Collections.Generic;
using System.IO;

namespace Neiroher
{
    internal static class Manager
    {
        static Net net;
        
        static int iteration = 0, interval = 1;

        public static Net Network => net;

        public static void Create(params int[] nodeAms)
        {
            net = new Net(nodeAms);
        }

        public static float[] Get(float[] input)
        {
            return net.Transform(input);
        }

        static public void Teach(float[] example, float[] correct)
        {
            var a = net.Transform(example);
            var e = new float[net.output.nodes.Length];
            float error = 0;
            for (int j = 0; j < e.Length; j++)
            {
                e[j] = a[j] - correct[j];
                error += e[j]*e[j];
            }
            net.avgEr = (net.avgEr * 1000 + error)/1001;

            
            if (iteration == 0)
            {
                iteration = interval;
                net.RecDerivatives(e);
            }
            iteration--;
            net.BackProp(e);
        }

        public static void LoadSave(string name)
        {
            string[] texts = File.ReadAllText(name+".txt").Split(';');
            float[] floats = new float[texts.Length-1];
            for(int i = 0; i < floats.Length-1; i++)
            {
                floats[i] = Convert.ToSingle(texts[i]);
            }

            int ind = (int)(floats[0]+0.1) + 1;
            for (int i = 0; i < net.layers.Length; i++)
            {
                for (int j = 0; j < net.layers[i].nodes.Length; j++)
                {
                    var node = net.layers[i].nodes[j];
                    node.activation=0;
                    if (i > 0)
                    {
                        node.bias = floats[ind];
                        ind++;
                    }
                    if (i > 0)
                    {
                        for (int k = 0; k < net.layers[i - 1].nodes.Length; k++)
                        {
                            net.layers[i][k, j] = floats[ind];
                            ind++;
                        }
                    }
                }
            }
            Console.WriteLine(ind);
        }

        public static void DoSave(string name)
        {
            Console.WriteLine("saving");
            string save = $"{net.layers.Length};";
            List<float> list = new List<float>();
            for (int i = 0; i < net.layers.Length; i++)
            {
                list.Add(net.layers[i].nodes.Length);
            }
            for (int i = 0; i < net.layers.Length; i++)
            {
                for(int j = 0;j < net.layers[i].nodes.Length; j++)
                {
                    var node = net.layers[i].nodes[j];
                    if(i>0)list.Add(node.bias);
                    if (i > 0)
                    {
                        for (int k = 0; k < net.layers[i - 1].nodes.Length; k++)
                        {
                            list.Add(net.layers[i][k, j]);
                        }
                    }
                }
            }
            Console.WriteLine("listed "+list.Count);
            for(int i = 0; i < list.Count; i++)
            {
                save+=list[i].ToString()+';';
                if (i % 10000==0) Console.WriteLine("ff");
            }
            Console.WriteLine("saved");
            File.WriteAllText(name+".txt", save);
        }
    }
}