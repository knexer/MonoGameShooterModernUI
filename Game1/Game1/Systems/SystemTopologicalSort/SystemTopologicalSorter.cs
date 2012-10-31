using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems.SystemTopologicalSort
{
    public class SystemTopologicalSorter
    {
        DiGraph<ASystem> graph;
        Dictionary<Type, DiGraph<ASystem>.Node> nodeLookup;

        List<ASystem> unsorted;
        List<ASystem> sorted;

        public SystemTopologicalSorter(List<ASystem> toSort)
        {
            graph = new DiGraph<ASystem>();
            nodeLookup = new Dictionary<Type, DiGraph<ASystem>.Node>();

            unsorted = new List<ASystem>(toSort);
            sorted = new List<ASystem>();
        }

        public List<ASystem> getSortedOrdering()
        {
            if (sorted.Count != unsorted.Count)
            {
                doTopologicalSort();
            }

            return sorted;
        }

        private void doTopologicalSort()
        {
            buildGraph();

            HashSet<DiGraph<ASystem>.Node> visited = new HashSet<DiGraph<ASystem>.Node>();

            //for each node
            foreach ( DiGraph<ASystem>.Node startNode in graph.nodes )
            {
                DFS(startNode, visited, new HashSet<DiGraph<ASystem>.Node>());
            }
        }

        private void buildGraph()
        {
            //create the nodes
            foreach (ASystem sys in unsorted)
            {
                //create the node
                DiGraph<ASystem>.Node node = new DiGraph<ASystem>.Node(sys);

                //add it to the data structures
                graph.nodes.Add(node);
                nodeLookup[sys.GetType()] = node;
            }

            //create the edges
            foreach (DiGraph<ASystem>.Node node in graph.nodes)
            {
                //for each system that must run after this one
                foreach (Type childSystem in node.data.GetChildren())
                {
                    //create an edge from that system to this one to indicate that dependency
                    DiGraph<ASystem>.Node childNode = nodeLookup[childSystem];
                    childNode.AddNeighbor(node);
                }

                //for each system that must run before this one
                foreach (Type parentSystem in node.data.GetParents())
                {
                    //create an edge from this system to that one to indicate that dependency
                    DiGraph<ASystem>.Node parentNode = nodeLookup[parentSystem];
                    node.AddNeighbor(parentNode);
                }
            }
        }

        private void DFS(DiGraph<ASystem>.Node cur, HashSet<DiGraph<ASystem>.Node> visited, HashSet<DiGraph<ASystem>.Node> visitedThisTime)
        {
            if (visitedThisTime.Contains(cur))
            {
                throw new ArgumentException("A cycle exists in the set of systems!");
            }

            if (!visited.Contains(cur))
            {
                visited.Add(cur);
                //visit every ancestor of cur (every system that must be executed first)
                foreach (DiGraph<ASystem>.Node neighbor in cur.neighbors)
                {
                    DFS(neighbor, visited, visitedThisTime);
                }
                //Since every preceding system has been added to the execution ordering, this one can be safely added now
                sorted.Add(cur.data);
            }
        }
    }
}
