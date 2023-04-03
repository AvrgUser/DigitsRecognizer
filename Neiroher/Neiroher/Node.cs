using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal struct Node
    {
        public float activation, bias, d, cw;

        List<Connection> inputConnections, outputConnections;

        ActivationFunction activationFunction = ActivationFunction.Sigmoid;

        public bool DataPushed {
            get
            {
                foreach (var connection in inputConnections)
                {
                    if(!connection.dataPushed) return false;
                }
                return true;
            }
        }

        public Node(int number)
        {
            activation = 0;
            bias = 0;
            d = 0;
            cw = 0;
            inputConnections = new List<Connection>();
            outputConnections = new List<Connection>();
        }

        public void Connect(Node with)
        {
            var con = new Connection(with, this);
            inputConnections.Add(con);
            with.outputConnections.Add(con);
        }

        public void Activate()
        {
            float sum = 0;
            foreach(var connection in inputConnections)
            {
                connection.DataPushed(false);
                sum += connection.value;
            }
            switch (activationFunction)
            {
                case ActivationFunction.Sigmoid:
                    activation = MS.Sigmoid(sum);
                    break;
                case ActivationFunction.LRelu:
                    activation = MS.LRelu(sum);
                    break;
            }
        }

        public void BackPropagate()
        {
            foreach (var connection in inputConnections)
            {
                connection.SetWeight(connection.weight - d * Net.step * connection.value);
            }
        }

        public void RecalcDerivatives()
        {
            d *= MS.SigmoidDerS(activation);
            foreach (var connection in inputConnections)
            {
                connection.from.d += d * connection.weight;
            }
        }
    }

    internal struct Connection
    {
        public Node from;
        public Node to;
        public bool dataPushed = false;
        public float value = 0, weight = 0;
        public Connection self => this;

        public Connection(Node from, Node to)
        {
            this.from = from;
            this.to = to;
        }

        public void Push(float value)
        {
            dataPushed = true;
            this.value = value;
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
        }

        public void SetValue(float value)
        {
            this.value = value;
        }

        public void DataPushed(bool state) => dataPushed = state;
    }

    public enum ActivationFunction {
        Sigmoid,
        Relu,
        LRelu,
        Squared,
        lg,
        ln,
    }
}
