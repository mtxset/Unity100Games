using Components.UnityComponents;
using Interfaces;
using UnityEngine;

namespace Components
{
    public class AddMinigameManager2 : MonoBehaviour
    {
        [HideInInspector]
        public MinigameManager2 MinigameManager;

        public void Awake()
        {
            this.MinigameManager = this.GetComponentInParent<MinigameManager2>();
        }
    }
}