using UnityEngine;
using static UnityEngine.GeometryUtility;

namespace Utils
{
    public static class CameraUtils
    {
        public static bool IsObjectInCamera(UnityEngine.Camera camera, Renderer objectsRenderer)
        {
            return TestPlanesAABB(CalculateFrustumPlanes(camera), objectsRenderer.bounds);
        }
    }
}