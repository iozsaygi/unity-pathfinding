using System;

namespace Pathfinding.Runtime
{
    public readonly struct Node : IEquatable<Node>
    {
        public readonly NodeIdentity Identity;
        public readonly NodeIdentity[] Neighbors;

        public Node(NodeIdentity nodeIdentity, NodeIdentity[] neighbors)
        {
            Identity = nodeIdentity;
            Neighbors = neighbors;
        }

        public bool Equals(Node other)
        {
            return Identity.Context == other.Identity.Context;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && Equals(node);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identity.Context);
        }
    }
}