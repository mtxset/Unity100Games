using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.DefaultImplementations
{
    class BasicControls : MonoBehaviour
    {
        public enum AxisState
        {
            Idle = 0,
            Negative = -1,
            Positive = 1
        }

        public AxisState HorizontalState = AxisState.Idle;
        public AxisState VerticalState = AxisState.Idle;

        public void HandleHorizontalStateChange(InputValue inputValue)
        {
            this.HorizontalState = (AxisState)inputValue.Get<float>();
        }

        public void HandleVerticalStateChange(InputValue inputValue)
        {
            this.VerticalState = (AxisState)inputValue.Get<float>();
        }
    }
}
