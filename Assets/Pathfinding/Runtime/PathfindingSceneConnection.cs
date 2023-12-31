using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class PathfindingSceneConnection : MonoBehaviour
    {
        [SerializeField] private Vector2Int mapSize;
        [SerializeField] private Vector2 nodeSize;
        [SerializeField, Min(0.0f)] private float spacingBetweenNodes;

        private Grid grid;

        private void Start()
        {
            grid = new Grid(mapSize, nodeSize, spacingBetweenNodes);
        }

        // TODO: This executes draw call for each node we have in the array. Find a better way to represents nodes during gizmos.
        private void OnDrawGizmosSelected()
        {
            if (grid?.Nodes == null)
            {
                return;
            }

            const float nodeSphereRadius = 0.1f;

            Gizmos.color = Color.white;
            foreach (var node in grid.Nodes)
            {
                Gizmos.DrawWireSphere(node.Position, nodeSphereRadius);
            }
        }
    }
}