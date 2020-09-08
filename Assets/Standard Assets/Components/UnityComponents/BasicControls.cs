using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.UnityComponents
{
    public class BasicControls : MonoBehaviour
    {
        public enum AxisState
        {
            Idle = 0,
            Negative = -1,
            Positive = 1
        }
        
        public AxisState HorizontalState = AxisState.Idle;
        public AxisState VerticalState = AxisState.Idle;

        protected void HandleHorizontalStateChange(InputValue inputValue)
        {
            HorizontalState = (AxisState)inputValue.Get<float>();
        }

        protected void HandleVerticalStateChange(InputValue inputValue)
        {
            VerticalState = (AxisState)inputValue.Get<float>();
        }
    }
}
