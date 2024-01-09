using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

namespace Pathfinding.Runtime
{
    public class Grid
    {
        public readonly Node[] Nodes;
        public readonly Vector2 NodeSize;
        public readonly Vector2Int MapSize;
        public readonly float SpacingBetweenNodes;

        public Grid(Vector2Int mapSize, Vector2 nodeSize, float spacingBetweenNodes)
        {
            Debug.Assert(!mapSize.Equals(Vector2Int.zero));
            Debug.Assert(!nodeSize.Equals(Vector2Int.zero));
            Debug.Assert(spacingBetweenNodes >= 0.0f);

            MapSize = mapSize;
            NodeSize = nodeSize;
            Nodes = new Node[mapSize.x * mapSize.y];
            SpacingBetweenNodes = spacingBetweenNodes;

            Populate();
        }

        private void Populate()
        {
            // Calculate offset for the node.
            var nodePositionOffset =
                new Vector3(NodeSize.x + SpacingBetweenNodes, 0.0f, NodeSize.y + SpacingBetweenNodes);

            // Figure out the center.
            var gridCenter =
                new Vector3((MapSize.x - 1) * 0.5f * nodePositionOffset.x, 0.0f,
                    (MapSize.y - 1) * 0.5f * nodePositionOffset.z);

            for (var row = 0; row < MapSize.y; row++)
            {
                for (var col = 0; col < MapSize.x; col++)
                {
                    var index = row * MapSize.x + col;
                    var nodeIdentity = new NodeIdentity(index);

                    var nodePosition =
                        new Vector3(col * nodePositionOffset.x, 0.0f, row * nodePositionOffset.z) - gridCenter;

                    Nodes[index] = new Node(nodeIdentity, null, nodePosition);
                }
            }
        }
    }
}