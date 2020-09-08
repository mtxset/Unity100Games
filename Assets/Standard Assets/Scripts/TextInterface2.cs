using Components.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class TextInterface2 : MonoBehaviour
{
    public Text TextField;

    public void OnEnable()
    {
        var comp = GetComponentInParent<IMinigameManager>();
        
        TextField.text = $"SCORE: {comp.Score}";
    }
}