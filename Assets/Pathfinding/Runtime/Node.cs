using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    public readonly struct Node : IEquatable<Node>
    {
        public static readonly Node Invalid = new(NodeIdentity.Invalid, null, Vector3.zero);

        public readonly NodeIdentity Identity;
        public readonly NodeIdentity[] Neighbors;
        public readonly Vector3 Position;

        public Node(NodeIdentity nodeIdentity, NodeIdentity[] neighbors, Vector3 position)
        {
            Identity = nodeIdentity;
            Neighbors = neighbors;
            Position = position;
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