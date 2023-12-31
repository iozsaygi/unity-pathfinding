using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class NodeMapController : MonoBehaviour
    {
        // The target plane that we aim to generate nodes on.
        [SerializeField] private MeshFilter meshFilter;

        // Caching corner points of plane for debugging.
        private Vector3 topLeftCornerInWorldCoordinates;
        private Vector3 topRightCornerInWorldCoordinates;
        private Vector3 bottomLeftCornerInWorldCoordinates;

        private void Start()
        {
            GenerateNodes();
        }

        private void OnDrawGizmosSelected()
        {
            // Render cached corner points of plane.
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(topLeftCornerInWorldCoordinates, Vector3.one);
            Gizmos.DrawWireCube(topRightCornerInWorldCoordinates, Vector3.one);
            Gizmos.DrawWireCube(bottomLeftCornerInWorldCoordinates, Vector3.one);
        }

        private void GenerateNodes()
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
        }
    }
}