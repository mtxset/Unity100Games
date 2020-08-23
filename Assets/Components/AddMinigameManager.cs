﻿using Components.UnityComponents;
using UnityEngine;

namespace Components
{
    public class AddMinigameManager : MonoBehaviour
    {
        public MinigameManagerDefault MinigameManager;

        public void Awake()
        {
            this.MinigameManager = this.GetComponentInParent<MinigameManagerDefault>();
        }
    }
}