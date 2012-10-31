using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems.SystemTopologicalSort
{
    public class DiGraph<T>
    {
        public List<Node> nodes;

        public DiGraph()
        {
            nodes = new List<Node>();
        }

        public class Node
        {
            public T data;

            //A list of ancestors of this Node<T>
            public List<Node> neighbors;

            public Node()
            {
                neighbors = new List<Node>();
            }

            public Node(T data)
            {
                this.data = data;
                neighbors = new List<Node>();
            }

            public void AddNeighbor(Node toAdd)
            {
                neighbors.Add(toAdd);
            }
        }
    }
}
