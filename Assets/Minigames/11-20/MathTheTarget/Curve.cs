using System.Collections.Generic;
using UnityEngine;
using Utils.Curves;

namespace Minigames.MathTheTarget
{
    public class Curve : MonoBehaviour
    {
        public GameObject ObjectToMove;
        public GameObject Target;
        public float A = 1;
        public float MovementSpeed = 5f;
        public float TargetMovementSpeed = 5f;
        public float TargetRotationSpeed = 10f;
        public float DistanceThreshold = 0.3f;
        public float TargetScale = 1.0f;
        
        private List<Vector2> points;
        private Vector2 targetPosition;
        private int currentPoint;
        
        private List<Vector2> targetPoints;
        private Vector2 targetTargetPosition;
        private int targetCurrentPoint;
        
        private float lastA;
        private MinigameManager gameManager;
        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            
            points = newPoints();
            targetPoints = newTargetPoints();

            lastA = A;
            targetPosition = points[0];
            targetTargetPosition = targetPoints[0];
            
            ObjectToMove.transform.position = targetPosition;
            Target.transform.position = targetTargetPosition;

            gameManager.DartEvents.OnShoot += HandleShoot;
            gameManager.DartEvents.OnDartReset += HandleReset;
        }

        private void OnDisable()
        {
            gameManager.DartEvents.OnShoot -= HandleShoot;
            gameManager.DartEvents.OnDartReset -= HandleReset;
        }

        private void HandleShoot()
        {
            ObjectToMove.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void HandleReset()
        {
            ObjectToMove.GetComponent<SpriteRenderer>().enabled = true;
        }

        private List<Vector2> newPoints()
        {
            return Curves.LemniscateOfBernoulli(
                transform.position,
                0.1f,
                0,
                Mathf.PI * 2,
                A);
        }
        
        private List<Vector2> newTargetPoints()
        {
            return Curves.LemniscateOfBernoulli(
                gameManager.transform.position, 
                0.01f,
                0, 
                Mathf.PI*2,
                6, 1, 3);
        }

        public void FixedUpdate()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            moveCrosshair();
            moveTarget();

            Target.transform.localScale = Vector3.Lerp(
                Target.transform.localScale, Vector3.one * TargetScale,
                30 * Time.fixedDeltaTime);
        }

        private void moveTarget()
        {
            Target.transform.position = Vector2.Lerp(
                Target.transform.position,
                targetTargetPosition,
                TargetMovementSpeed * Time.fixedDeltaTime);
            
            Target.transform.Rotate(
                0,0, TargetRotationSpeed * Time.fixedDeltaTime);
            
            if (Vector2.Distance(
                Target.transform.position,
                targetTargetPosition) <= DistanceThreshold)
            {
                targetCurrentPoint++;

                if (targetCurrentPoint == targetPoints.Count)
                {
                    targetCurrentPoint = 0;
                }

                targetTargetPosition = targetPoints[targetCurrentPoint];
            }
        }

        private void moveCrosshair()
        {
            if (Mathf.Abs(lastA - A) > 0.001f)
            {
                points = newPoints();
                lastA = A;
            }

            ObjectToMove.transform.position = Vector2.Lerp(
                ObjectToMove.transform.position,
                targetPosition,
                MovementSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(
                ObjectToMove.transform.position,
                targetPosition) <= DistanceThreshold)
            {
                currentPoint++;

                if (currentPoint == points.Count)
                {
                    currentPoint = 0;
                }

                targetPosition = points[currentPoint];
            }
        }
    }
}