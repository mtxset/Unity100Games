using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Minigames.TrainRunner
{
    public class Turret : MonoBehaviour
    {
        public GameObject Target;
        public GameObject BulletPrefab;
        public Transform GunPoint;
        public Animator TurretAnimator;

        public float BulletSpeed = 20;
        public float AngleDegree;
        public float ShootAfter = 2;
        public float RotationSpeed = 30;

        private Vector3 currentDirection;
        private MinigameManager gameManager;
        private bool doWeRotate = true;
        private bool canShoot = true;
        private bool startedShooting;
        private static readonly int Shoot = Animator.StringToHash("shoot");

        private void Start()
        {
            this.GetComponent<SpriteRenderer>();
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.setRandomDirection();

            this.subscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonStateChanged += HandleActionButton;
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonStateChanged -= HandleActionButton;
        }

        private void HandleActionButton(InputValue inputValue)
        {
            this.toggleShootingState();
        }

        private void toggleShootingState()
        {
            if (!canShoot)
            {
                return;
            }
            
            if (!startedShooting && doWeRotate)
            {
                this.gameManager.SlowMotionEvents.EventStartShooting();
                this.startedShooting = true;
            }
            else
            {
                this.TurretAnimator.SetTrigger(Shoot);
                this.shoot();
                this.gameManager.SlowMotionEvents.EventEndShooting();
                this.canShoot = false;
                this.doWeRotate = false;
                this.startedShooting = false;
                this.cooldownAsync();
            }
        }
        
        private void setRandomDirection()
        {
            var randomDirection = (Random.Range(0, 1) == 1) ? 1 : -1;
            this.currentDirection = Vector3.back * randomDirection;
        }
        
        private void FixedUpdate()
        {
            this.rotateCycle();
        }
        
        private async void cooldownAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(this.ShootAfter));
            
            this.canShoot = true;
            this.doWeRotate = true;
        }

        private void rotateCycle()
        {
            if (!this.doWeRotate)
            {
                return;
            }
            
            transform.RotateAround(
                Target.transform.position,
                this.currentDirection,
                RotationSpeed * Time.deltaTime);

            var angleOffset = AngleDegree / 2 / 360;

            if (transform.rotation.z <= -angleOffset ||
                transform.rotation.z >= angleOffset)
            {
                this.currentDirection *= -1;
            }
        }

        private void shoot()
        {
            var bullet = Instantiate(BulletPrefab, this.GunPoint.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.up * this.BulletSpeed;
        }
    }
}
