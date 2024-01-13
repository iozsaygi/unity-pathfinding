using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Runtime
{
    public static class Pathfinder
    {
        public static List<Node> Execute(Node source, Node destination, Grid grid)
        {
            Debug.Assert(!source.Equals(Node.Invalid));
            Debug.Assert(!destination.Equals(Node.Invalid));
            Debug.Assert(grid != null);

            // Nodes to be searched.
            var toSearch = new List<Node>();

            // Map to store relative 'G Cost' values for each node.
            // Starting with the source node.
            var calculatedGCosts = new Dictionary<Node, float>
            {
                { source, 0.0f }
            };

            // Map to store 'F Cost' values for each calculated node.
            PathfinderUtilities.CalculateHeuristicDistance(source, destination, out var heuristicDistanceForStartNode);
            var calculatedFCosts = new Dictionary<Node, float>
            {
                // Adding start node by only calculating 'H Cost' as 'F Cost' because we already know that 'G Cost' is 0 for the start node.
                { source, heuristicDistanceForStartNode }
            };

            // Keeping track of parent and child nodes. Will be used during traversal.
            var explorationChain = new Dictionary<Node, Node>();

            // Add the starting node to the search list.
            toSearch.Add(source);

            while (toSearch.Count > 0)
            {
                // Get the most reliable node.
                // TODO: Implement min-heap to avoid search to find reliable node.
                PathfinderUtilities.FindReliableNode(toSearch, calculatedFCosts, destination, out var reliableNode);

                // Check if the current reliable node is the destination.
                if (reliableNode.Equals(destination))
                {
                    // We found the path.
                    PathfinderUtilities.TraversePath(explorationChain, reliableNode, out var traversedPath);
                    traversedPath.Add(reliableNode);
                    return traversedPath;
                }

                // Remove the queried node.
                toSearch.Remove(reliableNode);

                // Explore the neighbors.
                foreach (var nodeIdentity in reliableNode.Neighbors)
                {
                    // Ensure neighbor is valid.
                    if (nodeIdentity.Equals(NodeIdentity.Invalid))
                    {
                        continue;
                    }

                    grid.NodeFromNodeIdentity(nodeIdentity, out var neighbor);
                    if (neighbor.Equals(Node.Invalid))
                    {
                        continue;
                    }

                    // Check if node registered with pathfinding blocker object.
                    if (grid.GridGameObjectRegistry.IsNodeRegisteredWithPathfindingBlocker(neighbor))
                    {
                        continue;
                    }

                    // Calculate the heuristic distance for neighbor.
                    PathfinderUtilities.CalculateHeuristicDistance(reliableNode, neighbor, out var heuristicDistance);
                    var tentativeGCostForNeighbor = calculatedGCosts[reliableNode] + heuristicDistance;

                    if (calculatedGCosts.ContainsKey(neighbor) &&
                        tentativeGCostForNeighbor >= calculatedGCosts[neighbor])
                    {
                        continue;
                    }


                    // Update the exploration chain for future traversals.
                    explorationChain[neighbor] = reliableNode;

                    // Update the 'G Cost' value of the neighbor.
                    calculatedGCosts[neighbor] = tentativeGCostForNeighbor;

                    // Update the 'F Cost' value of the neighbor.
                    PathfinderUtilities.CalculateHeuristicDistance(neighbor, destination,
                        out var heuristicDistanceForNeighbor);
                    calculatedFCosts[neighbor] = calculatedGCosts[neighbor] + heuristicDistanceForNeighbor;

                    if (!toSearch.Contains(neighbor))
                    {
                        toSearch.Add(neighbor);
                    }
                }
            }

            // Just allocate the list if no path found.
            return new List<Node>();
        }
    }
}