using Components;
using UnityEngine;

namespace Minigames.DoubleTrouble
{
    class DimmerController: AddMinigameManager2
    {
        public Transform SoundAttacker;

        private SpriteRenderer dimmerBox;
        private float maxXOffset;

        private void Start() {
            dimmerBox = GetComponent<SpriteRenderer>();

            maxXOffset = MinigameManager.CurrentCamera.orthographicSize * MinigameManager.CurrentCamera.aspect;
        }

        private void Update() {
           var x = SoundAttacker.position.x;

           dimmerBox.color = new Color(dimmerBox.color.r, dimmerBox.color.g, dimmerBox.color.b, 1f - (x / maxXOffset));
        }
    }
}