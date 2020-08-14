using UnityEngine;
using UnityEngine.InputSystem;

namespace Components
{
    public class PlayerControlBinds : MonoBehaviour
    {
        private ButtonEvents buttonEvents;
        private void Start()
        {
            this.buttonEvents = GameManager.GameManager.Instance.AddNewPlayer(gameObject);
        }
        private void OnActionButton()
        {
            if (buttonEvents != null) buttonEvents.ActionButtonPressed();
        }

        private void OnUp()
        {
            if (buttonEvents != null) buttonEvents.UpButtonPressed();
        }

        private void OnDown()
        {
            if (buttonEvents != null) buttonEvents.DownButtonPressed();
        }

        private void OnLeft()
        {
            if (buttonEvents != null) buttonEvents.LeftButtonPressed();
        }

        private void OnRight()
        {
            if (buttonEvents != null) buttonEvents.RightButtonPressed();
        }

        private void OnHorizontal(InputValue inputValue)
        {
            if (buttonEvents != null) buttonEvents.HorizontalButtonPressed(inputValue);
        }

        private void OnVertical(InputValue inputValue)
        {
            if (buttonEvents != null) buttonEvents.VerticalButtonPressed(inputValue);
        }
    }
}
