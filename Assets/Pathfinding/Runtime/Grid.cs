using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    public class Grid
    {
        public readonly Node[] Nodes;

        private readonly Vector2 nodeSize;
        private readonly Vector2Int mapSize;
        private readonly float spacingBetweenNodes;

        public Grid(Vector2Int mapSize, Vector2 nodeSize, float spacingBetweenNodes)
        {
            Debug.Assert(!mapSize.Equals(Vector2Int.zero));
            Debug.Assert(!nodeSize.Equals(Vector2Int.zero));
            Debug.Assert(spacingBetweenNodes >= 0.0f);

            Nodes = new Node[mapSize.x * mapSize.y];

            this.mapSize = mapSize;
            this.nodeSize = nodeSize;
            this.spacingBetweenNodes = spacingBetweenNodes;

            Populate();
        }

        private void Populate()
        {
            // Calculate offset for the node.
            var nodePositionOffset =
                new Vector3(nodeSize.x + spacingBetweenNodes, 0.0f, nodeSize.y + spacingBetweenNodes);

            // Figure out the center.
            var gridCenter =
                new Vector3((mapSize.x - 1) * 0.5f * nodePositionOffset.x, 0.0f,
                    (mapSize.y - 1) * 0.5f * nodePositionOffset.z);

            for (var row = 0; row < mapSize.y; row++)
            {
                for (var col = 0; col < mapSize.x; col++)
                {
                    var index = row * mapSize.x + col;
                    var nodeIdentity = new NodeIdentity(index);

                    var nodePosition =
                        new Vector3(col * nodePositionOffset.x, 0.0f, row * nodePositionOffset.z) - gridCenter;

                    Nodes[index] = new Node(nodeIdentity, null, nodePosition);
                }
            }
        }
    }
}