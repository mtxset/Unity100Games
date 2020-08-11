using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.TrainRunner
{
    public class Barrel : MonoBehaviour
    {
        public GameObject Target;
        public GameObject BulletPrefab;
        public Transform GunPoint;

        public float BulletSpeed = 20;
        public float AngleDegree;
        public float ShootAfter = 2;
        public float RotationSpeed = 30;

        private Vector3 currentDirection;
        private float timer;

        private void Start()
        {
            setRandomDirection();
        }

        private void setRandomDirection()
        {
            var randomDirection = (Random.Range(0, 1) == 1) ? 1 : -1;
            this.currentDirection = Vector3.back * randomDirection;
        }
        
        private void FixedUpdate()
        {
            this.timer += Time.deltaTime;

            if (this.timer >= this.ShootAfter)
            {
                this.shoot();
                this.timer = 0;
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
