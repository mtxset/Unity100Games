using Components.UnityComponents.v2;
using UnityEngine;

namespace Components
{
    public class AddMinigameManager2 : MonoBehaviour
    {
        [HideInInspector]
        public MinigameManager2 MinigameManager;

        public void Awake()
        {
            MinigameManager = GetComponentInParent<MinigameManager2>();
        }
    }
}