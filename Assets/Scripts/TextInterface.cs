using Assets.GameManager.UnityInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameManager
{
    public class TextInterface : MonoBehaviour
    {
        public Text TextField;

        public void OnEnable()
        {
            var comp = this.GetComponentInParent<IMinigameManagerOLD>();
            this.TextField.text = $"SCORE: {comp.GetScore()}";
        }
    }
}
