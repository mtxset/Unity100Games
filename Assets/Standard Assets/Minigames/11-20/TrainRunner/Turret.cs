using System.Collections;
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
        private bool canSpawn = true;
        private static readonly int Shoot = Animator.StringToHash("shoot");
        private bool noEnemies = true;

        private void Start()
        {
            GetComponent<SpriteRenderer>();
            gameManager = GetComponentInParent<MinigameManager>();
            setRandomDirection();

            subscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnActionButtonStateChanged += HandleActionButton;
            gameManager.Events.OnHit += HandleHit;
            gameManager.Events.OnScored += HandleScored;
            gameManager.SlowMotionEvents.OnNoEnemies += HandleNoEnemies;
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnActionButtonStateChanged -= HandleActionButton;
            gameManager.Events.OnHit -= HandleHit;
            gameManager.Events.OnScored -= HandleScored;
            gameManager.SlowMotionEvents.OnNoEnemies -= HandleNoEnemies;
            
        }

        private void HandleNoEnemies()
        {
            noEnemies = true;
        }

        private void HandleScored(int obj)
        {
            canSpawn = true;
        }

        private void HandleHit()
        {
            canSpawn = true;
        }

        private void HandleActionButton(InputValue inputValue)
        {
            toggleShootingState();
        }

        private void toggleShootingState()
        {
            if (!canShoot)
            {
                return;
            }
            
            if (!startedShooting)
            {
                gameManager.SlowMotionEvents.EventStartShooting();
                startedShooting = true;
            }
            else
            {
                TurretAnimator.SetTrigger(Shoot);
                shoot();
                gameManager.SlowMotionEvents.EventEndShooting();
                canShoot = false;
                doWeRotate = false;
                startedShooting = false;
                StartCoroutine(cooldown());
            }
        }
        
        private void setRandomDirection()
        {
            var randomDirection = (Random.Range(0, 1) == 1) ? 1 : -1;
            currentDirection = Vector3.back * randomDirection;
        }
        
        private void FixedUpdate()
        {
            rotateCycle();
            
            if (canSpawn && noEnemies)
            {
                gameManager.SlowMotionEvents.EventReloaded();
                StartCoroutine(spawnCooldown());
                noEnemies = false;
            }
        }
        
        private IEnumerator cooldown()
        {
            yield return new WaitForSeconds(ShootAfter);
            
            canShoot = true;
            doWeRotate = true;
        }

        private IEnumerator spawnCooldown()
        {
            yield return new WaitForSeconds(ShootAfter);

            canSpawn = true;
        }

        private void rotateCycle()
        {
            if (!doWeRotate || gameManager.GameOver)
            {
                return;
            }
            
            transform.RotateAround(
                Target.transform.position,
                currentDirection,
                RotationSpeed * Time.deltaTime);

            var angleOffset = AngleDegree / 2 / 360;

            if (transform.rotation.z <= -angleOffset ||
                transform.rotation.z >= angleOffset)
            {
                currentDirection *= -1;
            }
        }

        private void shoot()
        {
            if (!canShoot)
            {
                return;
            }
            
            var bullet = Instantiate(BulletPrefab, GunPoint.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.up * BulletSpeed;
            
            Destroy(bullet, 5.0f);
        }
    }
}
