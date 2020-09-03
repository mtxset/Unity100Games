using System;
using UnityEngine;

namespace Components
{
    public partial class SimpleEntityLifecycle
    {
        public enum UpdateType
        {
            FixedUpdate,
            Update
        }

        /**
         * Returns vector with postion outside of camera
         */
        public static Vector2 PositionOutsideCamera(
            Camera currentCamera,
            Vector2 cameraAxis,
            Vector3 objectScale)
        {
            if (!currentCamera.orthographic)
            {
                throw new Exception("Function is only for orthographic camera");
            }

            return new Vector2(
                (currentCamera.orthographicSize * currentCamera.aspect + objectScale.x) * cameraAxis.x,
                (currentCamera.orthographicSize + objectScale.y) * cameraAxis.y);
        }
        
        /**
         * Moves object with linearly from current position
         * to direction in provided axes by offset
         */
        public static Vector3 MoveEntityLinearly(
            Vector3 currentPosition, 
            Vector3 direction,
            Vector3 axes,
            float offsetBy,
            UpdateType updateType)
        {
            var time = (updateType == UpdateType.Update) ? 
                Time.deltaTime : 
                Time.fixedDeltaTime;

            var newOffset = new Vector3(
                offsetBy * direction.x * axes.x * time,
                offsetBy * direction.y * axes.y * time,
                offsetBy * direction.z * axes.z * time);

            var newPosition = currentPosition + newOffset;
            return newPosition;
        }
    }
}