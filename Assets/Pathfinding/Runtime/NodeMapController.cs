using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class NodeMapController : MonoBehaviour
    {
        // The target plane that we aim to generate nodes on.
        [SerializeField] private MeshFilter meshFilter;

        // Editor debugging properties.
        [SerializeField, Min(0.1f)] private float gizmosSphereRenderRadius;

        // Caching corner points of plane for debugging.
        private Vector3 topLeftCornerInWorldCoordinates;
        private Vector3 topRightCornerInWorldCoordinates;
        private Vector3 bottomLeftCornerInWorldCoordinates;

        // Origin point to start node generation.
        private Vector3 nodeGenerationOrigin;

        // Calculated count values for node generation.
        private int horizontalNodeCount;
        private int verticalNodeCount;

        // Node array to store generated nodes.
        private Node[] nodes;

        private void Start()
        {
            // Calculates required values for node generation.
            Warmup();

            GenerateNodes();
        }

        private void OnDrawGizmosSelected()
        {
            // Render nodes.
            if (nodes == null) return;
            Gizmos.color = Color.magenta;
            for (byte i = 0; i < nodes.Length; i++)
            {
                Gizmos.DrawSphere(nodes[i].PositionInWorldCoordinates, gizmosSphereRenderRadius);
            }
        }

        private void Warmup()
        {
            // The bounds of the plane, will be used to calculate corner points of the plane.
            var meshFilterBounds = meshFilter.sharedMesh.bounds;

            // Cache the top left corner of the plane.
            topLeftCornerInWorldCoordinates = meshFilter.transform.TransformPoint(meshFilterBounds.min +
                new Vector3(0.0f, 0.0f, meshFilterBounds.size.z));

            // Cache the top right corner of the plane.
            topRightCornerInWorldCoordinates =
                meshFilter.transform.TransformPoint(meshFilterBounds.min +
                                                    new Vector3(meshFilterBounds.size.x, 0.0f,
                                                        meshFilterBounds.size.z));

            // Cache the bottom left corner of the plane.
            bottomLeftCornerInWorldCoordinates = meshFilter.transform.TransformPoint(meshFilterBounds.min);

            // Calculate how many nodes we need to generate horizontally.
            horizontalNodeCount =
                Mathf.FloorToInt(Vector3.Distance(topLeftCornerInWorldCoordinates, topRightCornerInWorldCoordinates) /
                                 Node.Size.x);

            // Calculate how many nodes we need to generate vertically. (For 'Z' axis)
            verticalNodeCount =
                Mathf.FloorToInt(Vector3.Distance(topLeftCornerInWorldCoordinates, bottomLeftCornerInWorldCoordinates) /
                                 Node.Size.y);
        }

        private void GenerateNodes()
        {
            nodes = new Node[horizontalNodeCount * verticalNodeCount];

            var generationStep = new Vector3(Node.Size.x / 2.0f, 0.0f, -(Node.Size.y / 2.0f));
            nodeGenerationOrigin = topLeftCornerInWorldCoordinates + generationStep;

            byte iterator = 0;
            byte verticalOffset = 0;

            for (byte i = 0; i < horizontalNodeCount * verticalNodeCount; i++)
            {
                var placement = new Vector3(nodeGenerationOrigin.x + iterator, 0.0f,
                    nodeGenerationOrigin.z - verticalOffset);

                nodes[i] = new Node(new NodeIdentity(i), null, placement);

                iterator++;
                if (iterator != horizontalNodeCount) continue;
                verticalOffset += 1;
                iterator = 0;
            }

            Debug.Log($"Generated {horizontalNodeCount} nodes horizontally and {verticalNodeCount} nodes vertically");
        }
    }
}