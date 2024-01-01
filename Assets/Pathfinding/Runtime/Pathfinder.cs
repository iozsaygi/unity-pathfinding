using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Runtime
{
    public class Pathfinder
    {
        public static List<Node> Execute(Node start, Node destination, NodeMapController nodeMapController)
        {
            // Create open list to consider nodes for visiting.
            var toSearch = new List<Node>();

            // Map to store relative 'G Cost' values for each node.
            var gCostMap = new Dictionary<Node, float>
            {
                { start, 0.0f }
            };

            // Map to store 'F Cost' values for each calculated node.
            CalculateHeuristicDistance(start, destination, out var heuristicDistanceForStartNode);
            var fCostMap = new Dictionary<Node, float>
            {
                // Adding start node by only calculating 'H Cost' as 'F Cost' because we already know that 'G Cost' is 0 for the start node.
                { start, heuristicDistanceForStartNode }
            };

            // Keeping track of parent and child nodes.
            var explorationMap = new Dictionary<Node, Node>();

            // Add starting node to open nodes.
            toSearch.Add(start);

            while (toSearch.Count > 0)
            {
                FindReliableNode(toSearch, fCostMap, destination, out var currentReliableNode);

                // Check if the reliable node is our destination.
                if (currentReliableNode.Equals(destination))
                {
                    TraversePath(explorationMap, currentReliableNode, out var path);
                    path.Add(currentReliableNode);
                    return path;
                }

                toSearch.Remove(currentReliableNode);

                foreach (var neighborIdentity in currentReliableNode.Neighbors)
                {
                    nodeMapController.NodeFromIdentity(neighborIdentity, out var neighbor);
                    if (neighbor.Identity.Equals(NodeIdentity.InvalidIdentity)) continue;

                    // TODO: Check if neighbor is blocked.

                    CalculateHeuristicDistance(currentReliableNode, neighbor, out var heuristicDistance);
                    var tentativeGCost = gCostMap[currentReliableNode] + heuristicDistance;

                    if (gCostMap.ContainsKey(neighbor) && !(tentativeGCost < gCostMap[neighbor])) continue;

                    explorationMap[neighbor] = currentReliableNode;
                    gCostMap[neighbor] = tentativeGCost;
                    CalculateHeuristicDistance(neighbor, destination, out var heuristicDistanceForNeighbor);
                    fCostMap[neighbor] = gCostMap[neighbor] + heuristicDistanceForNeighbor;

                    if (!toSearch.Contains(neighbor)) toSearch.Add(neighbor);
                }
            }

            return null;
        }

        private static void CalculateHeuristicDistance(Node current, Node destination, out float heuristicDistance)
        {
            // Using manhattan distance for heuristics.
            heuristicDistance =
                Mathf.Abs(current.PositionInWorldCoordinates.x - destination.PositionInWorldCoordinates.x) +
                Mathf.Abs(current.PositionInWorldCoordinates.z - destination.PositionInWorldCoordinates.z);
        }

        // Finds the most reliable node for pathfinding, priorities node with lowest 'F Cost' first.
        // Then prioritizes node with lowest 'H Cost'.
        private static void FindReliableNode(List<Node> nodes, IReadOnlyDictionary<Node, float> fCostMap,
            Node destination, out Node reliableNode)
        {
            reliableNode = nodes[0];
            foreach (var node in nodes)
            {
                CalculateHeuristicDistance(reliableNode, destination, out var reliableNodeHCost);
                CalculateHeuristicDistance(node, destination, out var currentNodeHCost);

                var reliableNodeFCost = fCostMap[reliableNode];
                var currentNodeFCost = fCostMap[node];

                // Try to find the node with lowest 'F Cost' but also inspect 'H Cost' values if 'F Costs' are same.
                if (currentNodeFCost < reliableNodeFCost ||
                    (int)currentNodeFCost == (int)reliableNodeFCost && currentNodeHCost < reliableNodeHCost)
                {
                    reliableNode = node;
                }
            }
        }

        private static void TraversePath(IReadOnlyDictionary<Node, Node> explorationMap, Node current,
            out List<Node> path)
        {
            path = new List<Node> { current };
            while (explorationMap.ContainsKey(current))
            {
                current = explorationMap[current];
                path.Insert(0, current);
            }
        }
    }
}