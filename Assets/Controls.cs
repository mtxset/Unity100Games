// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""player1"",
            ""id"": ""8fe86831-8f27-4d53-bd8a-de36fa7712dd"",
            ""actions"": [
                {
                    ""name"": ""ActionButton"",
                    ""type"": ""Button"",
                    ""id"": ""c9344213-2a3a-44b6-ab0f-4bfaa26fc8a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""30037012-6cf7-4d64-90e9-b93d3bd4b2e8"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ActionButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Global"",
            ""id"": ""4fb39401-72fe-4c4a-8780-9b54e54cd3ba"",
            ""actions"": [
                {
                    ""name"": ""Add New Player"",
                    ""type"": ""Button"",
                    ""id"": ""3df11245-9cad-4a0a-ba83-88bc3e85b7ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4b98633c-a934-4651-94da-4eaf08b288d0"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Add New Player"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""TEST1"",
            ""bindingGroup"": ""TEST1"",
            ""devices"": []
        }
    ]
}");
        // player1
        m_player1 = asset.FindActionMap("player1", throwIfNotFound: true);
        m_player1_ActionButton = m_player1.FindAction("ActionButton", throwIfNotFound: true);
        // Global
        m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
        m_Global_AddNewPlayer = m_Global.FindAction("Add New Player", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // player1
    private readonly InputActionMap m_player1;
    private IPlayer1Actions m_Player1ActionsCallbackInterface;
    private readonly InputAction m_player1_ActionButton;
    public struct Player1Actions
    {
        private @Controls m_Wrapper;
        public Player1Actions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ActionButton => m_Wrapper.m_player1_ActionButton;
        public InputActionMap Get() { return m_Wrapper.m_player1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player1Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayer1Actions instance)
        {
            if (m_Wrapper.m_Player1ActionsCallbackInterface != null)
            {
                @ActionButton.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnActionButton;
                @ActionButton.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnActionButton;
                @ActionButton.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnActionButton;
            }
            m_Wrapper.m_Player1ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ActionButton.started += instance.OnActionButton;
                @ActionButton.performed += instance.OnActionButton;
                @ActionButton.canceled += instance.OnActionButton;
            }
        }
    }
    public Player1Actions @player1 => new Player1Actions(this);

    // Global
    private readonly InputActionMap m_Global;
    private IGlobalActions m_GlobalActionsCallbackInterface;
    private readonly InputAction m_Global_AddNewPlayer;
    public struct GlobalActions
    {
        private @Controls m_Wrapper;
        public GlobalActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @AddNewPlayer => m_Wrapper.m_Global_AddNewPlayer;
        public InputActionMap Get() { return m_Wrapper.m_Global; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
        public void SetCallbacks(IGlobalActions instance)
        {
            if (m_Wrapper.m_GlobalActionsCallbackInterface != null)
            {
                @AddNewPlayer.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnAddNewPlayer;
                @AddNewPlayer.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnAddNewPlayer;
                @AddNewPlayer.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnAddNewPlayer;
            }
            m_Wrapper.m_GlobalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AddNewPlayer.started += instance.OnAddNewPlayer;
                @AddNewPlayer.performed += instance.OnAddNewPlayer;
                @AddNewPlayer.canceled += instance.OnAddNewPlayer;
            }
        }
    }
    public GlobalActions @Global => new GlobalActions(this);
    private int m_TEST1SchemeIndex = -1;
    public InputControlScheme TEST1Scheme
    {
        get
        {
            if (m_TEST1SchemeIndex == -1) m_TEST1SchemeIndex = asset.FindControlSchemeIndex("TEST1");
            return asset.controlSchemes[m_TEST1SchemeIndex];
        }
    }
    public interface IPlayer1Actions
    {
        void OnActionButton(InputAction.CallbackContext context);
    }
    public interface IGlobalActions
    {
        void OnAddNewPlayer(InputAction.CallbackContext context);
    }
}
