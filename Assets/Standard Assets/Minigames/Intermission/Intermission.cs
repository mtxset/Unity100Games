using Components;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.Intermission
{
  public class Intermission : AddMinigameManager2
  {
    public GameObject IntermissionPage;
    public GameObject MenuUI;

    private BasicMenu basicMenu;

    private void Start()
    {
      basicMenu = GetComponent<BasicMenu>();

      basicMenu.Menu.CreateMenuEntries(MinigameManager.CommunicationBus.MenuEntries);

      subscribe();

      IntermissionPage.SetActive(true);
      MenuUI.SetActive(false);

      StartCoroutine(Delay.StartDelay(2, disableIntermissionPage, null));
    }

    private void disableIntermissionPage()
    {
      IntermissionPage.SetActive(false);
      MenuUI.SetActive(true);
    }

    private void subscribe()
    {
      basicMenu.Menu.OnSelected += HandleSelection;
      MinigameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
      MinigameManager.ButtonEvents.OnLeftButtonPressed += HandleLeft;
      MinigameManager.ButtonEvents.OnRightButtonPressed += HandleRight;
      MinigameManager.ButtonEvents.OnUpButtonPressed += HandleUp;
      MinigameManager.ButtonEvents.OnDownButtonPressed += HandleDown;

      MinigameManager.CommunicationBus.OnNewVote += HandleNewVote;
    }

    private void unsubscribe()
    {
      basicMenu.Menu.OnSelected -= HandleSelection;
      MinigameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
      MinigameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeft;
      MinigameManager.ButtonEvents.OnRightButtonPressed -= HandleRight;
      MinigameManager.ButtonEvents.OnUpButtonPressed -= HandleUp;
      MinigameManager.ButtonEvents.OnDownButtonPressed -= HandleDown;

      MinigameManager.CommunicationBus.OnNewVote -= HandleNewVote;
    }

    private void HandleAction() => this.basicMenu.Menu.Select();
    private void HandleLeft() => this.basicMenu.Menu.PreviousPage();
    private void HandleRight() => this.basicMenu.Menu.NextPage();
    private void HandleUp() => this.basicMenu.Menu.PreviousMenuItem();
    private void HandleDown() => this.basicMenu.Menu.NextMenuItem();

    private void HandleNewVote()
    {
      basicMenu.Menu.CreateMenuEntries(MinigameManager.CommunicationBus.MenuEntries);
    }

    private void OnDisable()
    {
      unsubscribe();
    }

    private void HandleSelection(string obj)
    {
      MinigameManager.CommunicationBus.PlayerVoted(obj);
    }
  }
}
