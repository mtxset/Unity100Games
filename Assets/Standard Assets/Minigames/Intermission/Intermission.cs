using System;
using Components;
using Components.UnityComponents;

namespace Minigames.Intermission {
public class Intermission : AddMinigameManager2 {

    private BasicMenu basicMenu;

    private void Start() {

        basicMenu = GetComponent<BasicMenu>();

        basicMenu.Menu.UpdateMenuEntries(
            MinigameManager.CommunicationBus.MenuEntries);

        basicMenu.Menu.OnSelected += HandleSelection;    
    }

    private void OnDisable() {
        basicMenu.Menu.OnSelected -= HandleSelection;  
    }

    private void HandleSelection(string obj) {
        MinigameManager.CommunicationBus.PlayerVoted(obj);
    }
}
}