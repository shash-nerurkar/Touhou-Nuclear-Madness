//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/Input/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerFly"",
            ""id"": ""33fbde88-b2d1-426e-9392-1e84cb497089"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""29c272c4-7064-4969-91e1-947ee1d671ec"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""f3927974-320a-46a2-8c55-1a2dde1e0396"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability1"",
                    ""type"": ""Button"",
                    ""id"": ""813d9f76-9c49-452b-b0aa-3d741b20060f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability2"",
                    ""type"": ""Button"",
                    ""id"": ""1975c4bf-b947-4e56-b373-b3092b69647b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrow keys"",
                    ""id"": ""55fbe004-1223-4d49-8416-ca21b6ced1cf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6b2dbb6a-e86c-4dca-aa6b-427edac1c2cb"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""88afedc4-f43f-4664-b81b-4e14cc16e71f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f92482c9-25dc-4bef-a50d-e9b13504efe7"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""304831e3-75d7-4527-afe3-6349a119befe"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""80ce192b-7430-4a38-a9a0-dae79d579df2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0566c6b4-8a81-47c1-8a1a-72e023da521b"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ccf6cb24-d70b-4c2c-ab48-ab09c58fdf8f"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerUI"",
            ""id"": ""f79ce020-496c-42ca-ab8a-f6b829a0a8ed"",
            ""actions"": [
                {
                    ""name"": ""PopDialogue"",
                    ""type"": ""Button"",
                    ""id"": ""fa2e8a69-64ab-441f-8df1-705d9109b020"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""094a3211-b60f-4a53-a54e-1fdec2867a93"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PopDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerFly
        m_PlayerFly = asset.FindActionMap("PlayerFly", throwIfNotFound: true);
        m_PlayerFly_Movement = m_PlayerFly.FindAction("Movement", throwIfNotFound: true);
        m_PlayerFly_Shoot = m_PlayerFly.FindAction("Shoot", throwIfNotFound: true);
        m_PlayerFly_Ability1 = m_PlayerFly.FindAction("Ability1", throwIfNotFound: true);
        m_PlayerFly_Ability2 = m_PlayerFly.FindAction("Ability2", throwIfNotFound: true);
        // PlayerUI
        m_PlayerUI = asset.FindActionMap("PlayerUI", throwIfNotFound: true);
        m_PlayerUI_PopDialogue = m_PlayerUI.FindAction("PopDialogue", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerFly
    private readonly InputActionMap m_PlayerFly;
    private IPlayerFlyActions m_PlayerFlyActionsCallbackInterface;
    private readonly InputAction m_PlayerFly_Movement;
    private readonly InputAction m_PlayerFly_Shoot;
    private readonly InputAction m_PlayerFly_Ability1;
    private readonly InputAction m_PlayerFly_Ability2;
    public struct PlayerFlyActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerFlyActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerFly_Movement;
        public InputAction @Shoot => m_Wrapper.m_PlayerFly_Shoot;
        public InputAction @Ability1 => m_Wrapper.m_PlayerFly_Ability1;
        public InputAction @Ability2 => m_Wrapper.m_PlayerFly_Ability2;
        public InputActionMap Get() { return m_Wrapper.m_PlayerFly; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerFlyActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerFlyActions instance)
        {
            if (m_Wrapper.m_PlayerFlyActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnMovement;
                @Shoot.started -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnShoot;
                @Ability1.started -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnAbility1;
                @Ability1.performed -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnAbility1;
                @Ability1.canceled -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnAbility1;
                @Ability2.started -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnAbility2;
                @Ability2.performed -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnAbility2;
                @Ability2.canceled -= m_Wrapper.m_PlayerFlyActionsCallbackInterface.OnAbility2;
            }
            m_Wrapper.m_PlayerFlyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Ability1.started += instance.OnAbility1;
                @Ability1.performed += instance.OnAbility1;
                @Ability1.canceled += instance.OnAbility1;
                @Ability2.started += instance.OnAbility2;
                @Ability2.performed += instance.OnAbility2;
                @Ability2.canceled += instance.OnAbility2;
            }
        }
    }
    public PlayerFlyActions @PlayerFly => new PlayerFlyActions(this);

    // PlayerUI
    private readonly InputActionMap m_PlayerUI;
    private IPlayerUIActions m_PlayerUIActionsCallbackInterface;
    private readonly InputAction m_PlayerUI_PopDialogue;
    public struct PlayerUIActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerUIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PopDialogue => m_Wrapper.m_PlayerUI_PopDialogue;
        public InputActionMap Get() { return m_Wrapper.m_PlayerUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerUIActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerUIActions instance)
        {
            if (m_Wrapper.m_PlayerUIActionsCallbackInterface != null)
            {
                @PopDialogue.started -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnPopDialogue;
                @PopDialogue.performed -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnPopDialogue;
                @PopDialogue.canceled -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnPopDialogue;
            }
            m_Wrapper.m_PlayerUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PopDialogue.started += instance.OnPopDialogue;
                @PopDialogue.performed += instance.OnPopDialogue;
                @PopDialogue.canceled += instance.OnPopDialogue;
            }
        }
    }
    public PlayerUIActions @PlayerUI => new PlayerUIActions(this);
    public interface IPlayerFlyActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnAbility1(InputAction.CallbackContext context);
        void OnAbility2(InputAction.CallbackContext context);
    }
    public interface IPlayerUIActions
    {
        void OnPopDialogue(InputAction.CallbackContext context);
    }
}
