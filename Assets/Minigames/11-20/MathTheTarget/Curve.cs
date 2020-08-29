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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            
            this.points = newPoints();
            this.targetPoints = newTargetPoints();

            this.lastA = A;
            this.targetPosition = this.points[0];
            this.targetTargetPosition = this.targetPoints[0];
            
            this.ObjectToMove.transform.position = this.targetPosition;
            this.Target.transform.position = this.targetTargetPosition;

            this.gameManager.DartEvents.OnShoot += HandleShoot;
            this.gameManager.DartEvents.OnDartReset += HandleReset;
        }

        private void OnDisable()
        {
            this.gameManager.DartEvents.OnShoot -= HandleShoot;
            this.gameManager.DartEvents.OnDartReset -= HandleReset;
        }

        private void HandleShoot()
        {
            this.ObjectToMove.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void HandleReset()
        {
            this.ObjectToMove.GetComponent<SpriteRenderer>().enabled = true;
        }

        private List<Vector2> newPoints()
        {
            return Curves.LemniscateOfBernoulli(
                this.transform.position,
                0.1f,
                0,
                Mathf.PI * 2,
                A);
        }
        
        private List<Vector2> newTargetPoints()
        {
            return Curves.LemniscateOfBernoulli(
                this.gameManager.transform.position, 
                0.01f,
                0, 
                Mathf.PI*2,
                6, 1, 3);
        }

        public void FixedUpdate()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            moveCrosshair();
            moveTarget();

            this.Target.transform.localScale = Vector3.Lerp(
                this.Target.transform.localScale, Vector3.one * this.TargetScale,
                30 * Time.fixedDeltaTime);
        }

        private void moveTarget()
        {
            this.Target.transform.position = Vector2.Lerp(
                this.Target.transform.position,
                this.targetTargetPosition,
                this.TargetMovementSpeed * Time.fixedDeltaTime);
            
            this.Target.transform.Rotate(
                0,0, this.TargetRotationSpeed * Time.fixedDeltaTime);
            
            if (Vector2.Distance(
                this.Target.transform.position,
                this.targetTargetPosition) <= this.DistanceThreshold)
            {
                this.targetCurrentPoint++;

                if (this.targetCurrentPoint == this.targetPoints.Count)
                {
                    this.targetCurrentPoint = 0;
                }

                targetTargetPosition = this.targetPoints[targetCurrentPoint];
            }
        }

        private void moveCrosshair()
        {
            if (Mathf.Abs(this.lastA - this.A) > 0.001f)
            {
                this.points = this.newPoints();
                this.lastA = this.A;
            }

            this.ObjectToMove.transform.position = Vector2.Lerp(
                this.ObjectToMove.transform.position,
                this.targetPosition,
                this.MovementSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(
                this.ObjectToMove.transform.position,
                this.targetPosition) <= this.DistanceThreshold)
            {
                this.currentPoint++;

                if (this.currentPoint == this.points.Count)
                {
                    this.currentPoint = 0;
                }

                targetPosition = this.points[currentPoint];
            }
        }
    }
}