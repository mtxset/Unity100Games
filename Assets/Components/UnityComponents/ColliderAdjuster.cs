using System.Collections.Generic;
using UnityEngine;

namespace Components.UnityComponents
{
    public class ColliderAdjuster : MonoBehaviour
    {
        [Tooltip("It will load from ../Resources/<PathToSprites>")]
        public string PathToSprites;
        
        private PolygonCollider2D currentCollider;
        private Dictionary<Sprite, Vector2[]> spriteColliderVectors;
        private SpriteRenderer spriteRenderer;
        private void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.currentCollider = this.GetComponent<PolygonCollider2D>();
            this.spriteColliderVectors = new Dictionary<Sprite, Vector2[]>();
            
            foreach (var item in Resources.LoadAll<Sprite>(PathToSprites))
            {
                var temp = new GameObject();
                temp.AddComponent<SpriteRenderer>().sprite = item;
                spriteColliderVectors.Add(item, temp.AddComponent<PolygonCollider2D>().points);
                Destroy(temp);
            }
        }
        private void Update()
        {
            this.currentCollider.points = this.spriteColliderVectors[this.spriteRenderer.sprite];
        }
    }
}