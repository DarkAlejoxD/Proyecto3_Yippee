//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Data/Inputs/Menus.inputactions
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

namespace InputController
{
    public partial class @Menus: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Menus()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Menus"",
    ""maps"": [
        {
            ""name"": ""Pause"",
            ""id"": ""6bb9ab99-9f11-4ef1-bb37-ae5e562e464d"",
            ""actions"": [
                {
                    ""name"": ""Pause/Unpause"",
                    ""type"": ""Button"",
                    ""id"": ""469b36d1-9fa2-465e-9c6c-88a57ec5ffb9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""26fbf014-1e61-411c-9a74-1e7211ab59cb"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Pause/Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""842f5609-8bb6-463b-a401-59ad0bd41b99"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause/Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": []
        },
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": []
        }
    ]
}");
            // Pause
            m_Pause = asset.FindActionMap("Pause", throwIfNotFound: true);
            m_Pause_PauseUnpause = m_Pause.FindAction("Pause/Unpause", throwIfNotFound: true);
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

        // Pause
        private readonly InputActionMap m_Pause;
        private List<IPauseActions> m_PauseActionsCallbackInterfaces = new List<IPauseActions>();
        private readonly InputAction m_Pause_PauseUnpause;
        public struct PauseActions
        {
            private @Menus m_Wrapper;
            public PauseActions(@Menus wrapper) { m_Wrapper = wrapper; }
            public InputAction @PauseUnpause => m_Wrapper.m_Pause_PauseUnpause;
            public InputActionMap Get() { return m_Wrapper.m_Pause; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PauseActions set) { return set.Get(); }
            public void AddCallbacks(IPauseActions instance)
            {
                if (instance == null || m_Wrapper.m_PauseActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PauseActionsCallbackInterfaces.Add(instance);
                @PauseUnpause.started += instance.OnPauseUnpause;
                @PauseUnpause.performed += instance.OnPauseUnpause;
                @PauseUnpause.canceled += instance.OnPauseUnpause;
            }

            private void UnregisterCallbacks(IPauseActions instance)
            {
                @PauseUnpause.started -= instance.OnPauseUnpause;
                @PauseUnpause.performed -= instance.OnPauseUnpause;
                @PauseUnpause.canceled -= instance.OnPauseUnpause;
            }

            public void RemoveCallbacks(IPauseActions instance)
            {
                if (m_Wrapper.m_PauseActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPauseActions instance)
            {
                foreach (var item in m_Wrapper.m_PauseActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PauseActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PauseActions @Pause => new PauseActions(this);
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IPauseActions
        {
            void OnPauseUnpause(InputAction.CallbackContext context);
        }
    }
}
