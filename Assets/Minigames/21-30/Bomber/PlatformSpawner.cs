using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Bomber
{
    internal class Platform
    {
        public GameObject Holder;
        public List<GameObject> Blocks;
        public GameObject LastBlock;
    }
    public class PlatformSpawner : MonoBehaviour
    {
        public GameObject StartPart;
        public GameObject MiddlePart;
        public GameObject EndPart;
        public GameObject Bomb;

        public Camera CurrentCamera;
        public Vector2 PlatformLengthMinMax;
        public float PlatformSlideSpeed;
        public float SpawnAfter = 1f;
        public float Force = 4;

        private List<Platform> liveEntities;
        private List<Platform> deadEntities;

        private float spawnTimer;
        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            
            this.liveEntities = new List<Platform>();
            this.deadEntities = new List<Platform>();

            this.gameManager.OnPlatformHit += HandlePlatformHit;
        }

        private void OnDisable()
        {
            this.gameManager.OnPlatformHit -= HandlePlatformHit;
        }

        private void HandlePlatformHit(GameObject obj)
        {
            foreach (var item in liveEntities)
            {
                if (item.Blocks.Contains(obj))
                {
                    smashPlatform(item);
                }
            }
        }

        private void FixedUpdate()
        {
            if ((this.spawnTimer += Time.fixedDeltaTime) >= this.SpawnAfter)
            {
                this.liveEntities.Add(this.createPlatform());
                this.spawnTimer = 0;
            }
            
            foreach (var item in liveEntities)
            {
                item.Holder.transform.position += new Vector3(
                    -1 * this.PlatformSlideSpeed * Time.fixedDeltaTime,
                    0, 0);

                if (item.LastBlock.transform.position.x + item.LastBlock.transform.localScale.x <
                    -this.CurrentCamera.orthographicSize * this.CurrentCamera.aspect)
                {
                    deadEntities.Add(item);
                }
            }

            foreach (var item in deadEntities)
            {
                liveEntities.Remove(item);
                Destroy(item.Holder, 5f);
            }

            deadEntities.Clear();
        }

        private void smashPlatform(Platform platform)
        {
            deadEntities.Add(platform);
            foreach (var item in platform.Blocks)
            {
                var rigidBody = item.GetComponent<Rigidbody2D>();
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                item.transform.Rotate(Vector3.forward, Random.Range(0, 360));

                var direction = item.transform.position -
                                this.Bomb.transform.position;
                
                rigidBody.AddForce(
                    direction * this.Force, ForceMode2D.Impulse);

                rigidBody.angularVelocity = 30f;
                Destroy(item, 5f);
            }

            Destroy(platform.Holder, 5f);
        }
        private Platform createPlatform()
        {
            var holder = new Platform
            {
                Holder = new GameObject(),
                Blocks = new List<GameObject>()
            };

            holder.Holder.transform.SetParent(this.transform);
            var start = Instantiate(
                this.StartPart, 
                this.transform.position,
                Quaternion.identity,
                holder.Holder.transform);
            
            holder.Blocks.Add(start);

            var randomLength = Random.Range(
                this.PlatformLengthMinMax.x, this.PlatformLengthMinMax.y);

            float lastXOffset = 0;
            for (var i = 1; i < randomLength; i++)
            {
                var temp = Instantiate(this.MiddlePart, holder.Holder.transform);
                temp.transform.position = new Vector3(
                    start.transform.position.x + temp.transform.localScale.x * i,
                    start.transform.position.y);

                lastXOffset = temp.transform.position.x;
                holder.Blocks.Add(temp);
            }
            
            var end = Instantiate(this.EndPart, holder.Holder.transform);
            
            end.transform.position = new Vector3(
                lastXOffset + end.transform.localScale.x,
                start.transform.position.y);
            
            holder.Blocks.Add(end);
            holder.LastBlock = end;
            
            return holder;
        }
    }
}