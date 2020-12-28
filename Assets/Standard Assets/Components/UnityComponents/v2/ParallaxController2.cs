using UnityEngine;

namespace Components.UnityComponents.v2 {
    public class ParallaxController2 : MonoBehaviour {
        public GameObject ObjectToParallax;
        public Parallaxer.Direction SelectMovementPostion;
        public float ParallaxSpeed;
        public Camera CurrentCamera;
        
        private Parallaxer parallaxer;
        private MinigameManager2 gameManager;

        private void Start() {
            gameManager = GetComponentInParent<MinigameManager2>();
            
            parallaxer = new Parallaxer(
                ObjectToParallax,
                SelectMovementPostion,
                ParallaxSpeed,
                CurrentCamera,
                gameManager.transform.position,
                transform);
        }

        private void FixedUpdate() {
            parallaxer.FixedUpdateRoutine();
        }
    }
}