using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class TextInterface2 : MonoBehaviour
{
    public Text TextField;

    public void OnEnable()
    {
        var comp = this.GetComponentInParent<IMinigameManager>();
        
        this.TextField.text = $"SCORE: {comp.Score}";
    }
}