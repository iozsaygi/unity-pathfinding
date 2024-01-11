using System.Collections.Generic;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    /// <summary>
    /// Keeps track of registered game objects to each node. Uses dictionary for constant look-up times.
    /// </summary>
    public class GridGameObjectRegistry
    {
        private readonly Dictionary<Node, GameObject> registry;
        private readonly Vector2 nodeSize;

        public GridGameObjectRegistry(Vector2 nodeSize)
        {
            registry = new Dictionary<Node, GameObject>();
            this.nodeSize = nodeSize;
        }

        public void Register(Node node, GameObject pathfindingBlockerPrefab)
        {
            // Check if there is an already assigned game object for given node in registry.
            if (registry.TryGetValue(node, out var registeredGameObject))
            {
                // Destroy the existing game object from that node.
                Object.Destroy(registeredGameObject);

                // Register the new game object.
                registry.Remove(node);
            }
            else
            {
                // Create a new entry for the game object and node pair.
                var pathfindingBlockerPrefabInstance =
                    Object.Instantiate(pathfindingBlockerPrefab, node.Position, Quaternion.identity);

                pathfindingBlockerPrefabInstance.transform.localScale = nodeSize;
                pathfindingBlockerPrefabInstance.transform.eulerAngles = Vector3.right * 90.0f;

                registry.TryAdd(node, pathfindingBlockerPrefabInstance);
            }
        }
    }
}