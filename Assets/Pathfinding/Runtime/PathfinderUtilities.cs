using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Runtime
{
    public static class PathfinderUtilities
    {
        public static void CalculateHeuristicDistance(Node source, Node destination, out float heuristicDistance)
        {
            // Using 'Manhattan' distance for heuristics.
            heuristicDistance = Mathf.Abs(source.Position.x - destination.Position.x) +
                                Mathf.Abs(source.Position.z - destination.Position.z);
        }

        public static void FindReliableNode(List<Node> toSearch, IReadOnlyDictionary<Node, float> fCostCalculations,
            Node destination, out Node reliableNode)
        {
            reliableNode = toSearch[0];
            foreach (var node in toSearch)
            {
                CalculateHeuristicDistance(reliableNode, destination, out var reliableNodeHCost);
                CalculateHeuristicDistance(node, destination, out var currentNodeHCost);

                var reliableNodeFCost = fCostCalculations[reliableNode];
                var currentNodeFCost = fCostCalculations[node];

                // Try to find the node with lowest 'F Cost' but also inspect 'H Cost' values if 'F Costs' are same.
                if (currentNodeFCost < reliableNodeFCost || (int)currentNodeFCost == (int)reliableNodeFCost &&
                    currentNodeHCost < reliableNodeHCost)
                {
                    reliableNode = node;
                }
            }
        }

        public static void TraversePath(IReadOnlyDictionary<Node, Node> explorationChain, Node current,
            out List<Node> traversedPath)
        {
            traversedPath = new List<Node> { current };
            while (explorationChain.ContainsKey(current))
            {
                current = explorationChain[current];
                traversedPath.Insert(0, current);
            }
        }
    }
}