using Components;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Preparation {
public class Preparation : AddMinigameManager2 {
    public GameObject Background;
    public Text InformationText;
    public GameObject BoxPrefab;
    public GameObject Overlay;

    public float BoxMovementSpeed;
    
    private Color playerColor;
    
    private void Start() {

        playerColor = MinigameManager.CommunicationBus.PlayerColor;

        Background.GetComponent<SpriteRenderer>().color = playerColor;
        BoxPrefab.GetComponent<SpriteRenderer>().color = playerColor;
        Overlay.GetComponent<Image>().color = playerColor;

        InformationText.text = "Press Action button when you are ready";

        MinigameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
    }

    private void OnDisable() {
        MinigameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
    }

    private void HandleAction() {
      MinigameManager.Events.EventDeath();
      InformationText.text = "WAIT FOR OTHERS";  
    } 

    private void FixedUpdate() {
        BoxPrefab.transform.position += new Vector3 (
            BoxMovementSpeed * Time.fixedDeltaTime * (int)MinigameManager.Controls.HorizontalState,
            BoxMovementSpeed * Time.fixedDeltaTime * (int)MinigameManager.Controls.VerticalState,
            0);
    }
}
}