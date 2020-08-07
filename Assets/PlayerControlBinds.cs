using Assets;
using Assets.GameManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlBinds : MonoBehaviour
{
    private ButtonEvents buttonEvents;
    private void Start()
    {
        this.buttonEvents = GameManager.Instance.AddNewPlayer(gameObject);
    }
    private void OnActionButton()
    {
        buttonEvents?.ActionButtonPressed();
    }

    private void OnUp()
    {
        buttonEvents?.UpButtonPressed();
    }

    private void OnDown()
    {
        buttonEvents?.DownButtonPressed();
    }

    private void OnLeft()
    {
        buttonEvents?.LeftButtonPressed();
    }

    private void OnRight()
    {
        buttonEvents?.RightButtonPressed();
    }

    private void OnHorizontal(InputValue inputValue)
    {
        buttonEvents?.HorizontalButtonPressed(inputValue);
    }

    private void OnVertical(InputValue inputValue)
    {
        buttonEvents?.VerticalButtonPressed(inputValue);
    }
}
