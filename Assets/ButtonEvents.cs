using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets
{
    public class ButtonEvents : MonoBehaviour
    {
        public event Action OnActionButtonPressed;
        
        public event Action OnUpButtonPressed;
        public event Action OnDownButtonPressed;
        public event Action OnLeftButtonPressed;
        public event Action OnRightButtonPressed;

        public event Action<InputValue> OnHorizontalPressed;
        public event Action<InputValue> OnVerticalPressed;

        public void VerticalButtonPressed(InputValue inputValue)
        {
            this.OnVerticalPressed?.Invoke(inputValue);
        }

        public void HorizontalButtonPressed(InputValue inputValue)
        {
            this.OnHorizontalPressed?.Invoke(inputValue);
        }

        public void ActionButtonPressed()
        {
            OnActionButtonPressed?.Invoke();
        }

        public void UpButtonPressed()
        {
            OnUpButtonPressed?.Invoke();
        }

        public void DownButtonPressed()
        {
            OnDownButtonPressed?.Invoke();
        }

        public void LeftButtonPressed()
        {
            OnLeftButtonPressed?.Invoke();
        }

        public void RightButtonPressed()
        {
            OnRightButtonPressed?.Invoke();
        }
    }
}
