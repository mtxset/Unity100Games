using Components.UnityComponents.v1;
using UnityEngine;

namespace Components
{
    public class AddMinigameManager : MonoBehaviour
    {
        [HideInInspector]
        public MinigameManagerDefault MinigameManager;

        public void Awake()
        {
            MinigameManager = GetComponentInParent<MinigameManagerDefault>();
        }
    }
}