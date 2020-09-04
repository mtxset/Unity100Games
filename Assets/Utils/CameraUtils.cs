using UnityEngine;
using static UnityEngine.GeometryUtility;

namespace Utils
{
    public static class CameraUtils
    {
        public static bool IsObjectInCamera(Camera camera, Vector3 objectsPosition)
        {
            var viewPosition = camera.WorldToViewportPoint(objectsPosition);

            return viewPosition.x >= 0 &&
                   viewPosition.x <= 1 &&
                   viewPosition.y >= 0 &&
                   viewPosition.y <= 1 &&
                   viewPosition.z > 0;
        }
        public static bool IsObjectInCamera(Camera camera, Renderer objectsRenderer)
        {
            return TestPlanesAABB(CalculateFrustumPlanes(camera), objectsRenderer.bounds);
        }
    }
}