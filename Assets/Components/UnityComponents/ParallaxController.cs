using Components.UnityComponents.v1;
using UnityEngine;

namespace Components.UnityComponents
{
    public class ParallaxController : MonoBehaviour
    {
        public GameObject ObjectToParallax;
        public Parallaxer.Direction SelectMovementPostion;
        public float ParallaxSpeed;
        public Camera CurrentCamera;
        
        private Parallaxer parallaxer;
        private MinigameManagerDefault gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManagerDefault>();
            
            parallaxer = new Parallaxer(
                ObjectToParallax,
                SelectMovementPostion,
                ParallaxSpeed,
                CurrentCamera,
                gameManager.transform.position,
                transform);
        }

        private void FixedUpdate()
        {
            parallaxer.FixedUpdateRoutine();
        }
    }
}