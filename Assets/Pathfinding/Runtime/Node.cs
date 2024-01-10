using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    public readonly struct Node : IEquatable<Node>
    {
        public static readonly Node Invalid = new(NodeIdentity.Invalid, null, Vector3.zero);

        public readonly NodeIdentity[] Neighbors;
        public readonly Vector3 Position;

        private readonly NodeIdentity identity;

        public Node(NodeIdentity nodeIdentity, NodeIdentity[] neighbors, Vector3 position)
        {
            identity = nodeIdentity;
            Neighbors = neighbors;
            Position = position;
        }

        public bool Equals(Node other)
        {
            return identity.Context == other.identity.Context;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && Equals(node);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(identity.Context);
        }
    }
}