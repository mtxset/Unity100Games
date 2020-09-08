using UnityEngine;
using UnityEngine.UI;
using UnityInterfaces;

public class TextInterface : MonoBehaviour
{
    public Text TextField;

    public void OnEnable()
    {
        var comp = GetComponentInParent<IMinigameManagerOld>();
        TextField.text = $"SCORE: {comp.GetScore()}";
    }
}