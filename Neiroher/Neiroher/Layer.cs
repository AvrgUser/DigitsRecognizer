using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal class Layer
    {
        public Node[] nodes;
        public float[,] weights;
        public Layer prev;
        public int number = 0, all = 0;
        public bool bias = true;

        public Layer(int nodeAm, Layer prev = null)
        {
            nodes = new Node[nodeAm];
            this.prev = prev;
            if (prev != null)
            {
                weights = new float[prev.nodes.Length, nodeAm];
                for (int x = 0, y = 0; x < nodeAm; y++)
                {
                    this[y, x] = new Random().NextSingle() - 0.5f;
                    if (y >= prev.nodes.Length-1)
                    {
                        this[x] = new Node(number);
                        y = -1;
                        x++;
                    }
                }
            }
            else for(int i = 0; i < nodes.Length; i++)
                {
                    this[i] = new Node(number);
                }
        }

        public void Activate()
        {
            for(int i = 0;i < nodes.Length; i++)
            {
                float sum = 0;
                for (int j = 0; j < prev.nodes.Length; j++)
                {
                    sum += prev[j].activation * this[j, i];
                }
                if (bias) sum += this[i].bias;
                this[i].activation = MS.Sigmoid(sum);
            }
        }

        public void BackProp(float s)
        {
            for (int i = 0; i < nodes.Length & number != 0; i++)
            {
                float d = this[i].d;
                for (int j = 0; j < prev.nodes.Length; j++)
                {
                    this[j, i] -= s * prev[j].activation * d;
                }
                if (bias) this[i].bias -= s * d;
            }
        }

        public void RecDerivatives()
        {
            for (int i = 0; i < nodes.Length & number != 0; i++)
            {
                float d = this[i].d;
                if (number != all - 1)
                    d *= MS.SigmoidDerS(this[i].activation);

                this[i].d = d;
                for (int j = 0; j < prev.nodes.Length; j++)
                {
                    prev[j].d += d * this[j, i];
                }
            }
        }

        public float this[int from, int to]
        {
            get { return weights[from, to]; }
            set { weights[from, to] = value; }
        }

        public Node this[int key]
        {
            get {
                return nodes[key];
            }
            set
            {
                nodes[key] = value;
            }
        }
    }
}
