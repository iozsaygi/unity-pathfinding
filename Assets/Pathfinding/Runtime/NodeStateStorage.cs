using System.Collections.Generic;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    /// <summary>
    /// Contains which node is walkable or not walkable for runtime.
    /// </summary>
    public class NodeStateStorage
    {
        private readonly Dictionary<Node, GameObject> unWalkableNodes = new();

        public void UpdateNodeState(Node node, GameObject unWalkableMarkerPrefab)
        {
            if (unWalkableNodes.ContainsKey(node))
            {
                Object.Destroy(unWalkableNodes[node]);
                unWalkableNodes.Remove(node);
            }
            else
            {
                var unWalkableMarkerInstance = Object.Instantiate(unWalkableMarkerPrefab);
                unWalkableMarkerInstance.transform.position = node.PositionInWorldCoordinates;
                unWalkableNodes[node] = unWalkableMarkerInstance;
            }
        }

        public void IsNodeWalkable(Node node, out bool isWalkable)
        {
            isWalkable = !unWalkableNodes.ContainsKey(node) || unWalkableNodes[node] == null;
        }
    }
}