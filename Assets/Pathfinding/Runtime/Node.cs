using UnityEngine;

namespace Pathfinding.Runtime
{
    public readonly struct Node
    {
        // Size of a single node. Will not change at runtime.
        public static readonly Vector3 Size = Vector3.one;

        // Unique identity value for this node.
        public readonly NodeIdentity Identity;

        // The identities of the neighbor nodes to this node.
        public readonly NodeIdentity[] Neighbors;

        public Node(NodeIdentity identity, NodeIdentity[] neighbors)
        {
            Identity = identity;
            Neighbors = neighbors;
        }
    }
}