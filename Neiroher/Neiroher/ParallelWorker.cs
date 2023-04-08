using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal class ParallelWorker
    {
        int[][] nodes;

        private bool done = false;

        public bool Done => done;

        Task task;

        private Node this[int x, int y]
        {
            get { return Net.Instance.nodes[x][y]; }
            set { Net.Instance.nodes[x][y] = value; }
        }

        public ParallelWorker(int[][] nodes)
        {
            this.nodes = nodes;
            task = Task.Run(() => { Job(); });
        }

        private async void Job()
        {
            for(int i = 0; i < nodes.Length; i++)
            {
                foreach(int node in nodes[i])
                {
                    this[i, node].Activate();
                }
            }
        }
    }
}
