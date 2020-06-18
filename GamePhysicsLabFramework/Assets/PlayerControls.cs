// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Starship"",
            ""id"": ""4f2be53b-2ac7-4c94-8c43-e028518a9dcd"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""b4088d21-eb2b-4bc4-bd87-8c42737a0339"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Vertical Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b0cfedce-8723-44a5-9c23-3dfa7c35c305"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Horizontal Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a8d08423-aecf-4be7-89af-9c9c4b538967"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Thrusters"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ae4a7615-ecb0-4bfe-8559-ba4b14fd0c06"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5ffdb0de-71e5-43c2-bb60-96e4a8a90b94"",
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
                    ""id"": ""ba84496e-38c3-4320-87a5-d86e5c88f1a1"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD Vertical"",
                    ""id"": ""590392f4-30f8-4635-9812-3c1ec5f39a84"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e240a520-e079-4ce3-83f7-f4b6fad2254c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""029792b5-01f6-4c21-a2cb-4bbeb6779878"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keyboards"",
                    ""id"": ""319c73dc-0aa0-4ee2-8825-8320924b4281"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a814f4d5-a7f6-4749-8721-9b2ad95b2fed"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d2d107a7-1857-4f9c-b3bd-125b9c411acf"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""0b541f2c-8020-4454-b210-b29426d92758"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""561e6b9f-bcc9-441b-8d37-a4d18962fcd1"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c98aefe3-ff48-4dfa-8311-084a1ff46a2f"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD Horizontal"",
                    ""id"": ""127fad42-9972-4fc2-91db-1f28283b40ee"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""228d9922-feda-4960-91c4-c7065cb84a2b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d3d15f92-4eef-45e7-8613-2f6871b2e772"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""47ff5525-4e03-453d-8164-fa4e30fc1631"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6317da78-5dce-44ba-9d9f-8dd3f9bcd725"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""31a6d653-f5de-483f-81c1-cf8c92a3d889"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""a95724f5-d4b9-49a0-91f3-a068e3d9bc9d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""37203a61-a86c-409e-bdc4-831a9d07d6b7"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f988dca3-9920-40c3-8787-a4d9ea33fa5d"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""571fbddd-4a0c-4a83-8bdf-601303d8c9b3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrusters"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""abff2795-e8a9-482b-939f-1c998e79d4db"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5231c97c-388a-4d27-964e-63c96f458ef0"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""65cfc6a9-4fea-4398-95e6-12d94e87569e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrusters"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b7eef10c-a337-4182-830d-0d43b322e283"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""987299c6-8336-4194-84d2-cbb922b64d4d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Starship
        m_Starship = asset.FindActionMap("Starship", throwIfNotFound: true);
        m_Starship_Shoot = m_Starship.FindAction("Shoot", throwIfNotFound: true);
        m_Starship_VerticalMovement = m_Starship.FindAction("Vertical Movement", throwIfNotFound: true);
        m_Starship_HorizontalMovement = m_Starship.FindAction("Horizontal Movement", throwIfNotFound: true);
        m_Starship_Thrusters = m_Starship.FindAction("Thrusters", throwIfNotFound: true);
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

    // Starship
    private readonly InputActionMap m_Starship;
    private IStarshipActions m_StarshipActionsCallbackInterface;
    private readonly InputAction m_Starship_Shoot;
    private readonly InputAction m_Starship_VerticalMovement;
    private readonly InputAction m_Starship_HorizontalMovement;
    private readonly InputAction m_Starship_Thrusters;
    public struct StarshipActions
    {
        private @PlayerControls m_Wrapper;
        public StarshipActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Starship_Shoot;
        public InputAction @VerticalMovement => m_Wrapper.m_Starship_VerticalMovement;
        public InputAction @HorizontalMovement => m_Wrapper.m_Starship_HorizontalMovement;
        public InputAction @Thrusters => m_Wrapper.m_Starship_Thrusters;
        public InputActionMap Get() { return m_Wrapper.m_Starship; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(StarshipActions set) { return set.Get(); }
        public void SetCallbacks(IStarshipActions instance)
        {
            if (m_Wrapper.m_StarshipActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_StarshipActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_StarshipActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_StarshipActionsCallbackInterface.OnShoot;
                @VerticalMovement.started -= m_Wrapper.m_StarshipActionsCallbackInterface.OnVerticalMovement;
                @VerticalMovement.performed -= m_Wrapper.m_StarshipActionsCallbackInterface.OnVerticalMovement;
                @VerticalMovement.canceled -= m_Wrapper.m_StarshipActionsCallbackInterface.OnVerticalMovement;
                @HorizontalMovement.started -= m_Wrapper.m_StarshipActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.performed -= m_Wrapper.m_StarshipActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.canceled -= m_Wrapper.m_StarshipActionsCallbackInterface.OnHorizontalMovement;
                @Thrusters.started -= m_Wrapper.m_StarshipActionsCallbackInterface.OnThrusters;
                @Thrusters.performed -= m_Wrapper.m_StarshipActionsCallbackInterface.OnThrusters;
                @Thrusters.canceled -= m_Wrapper.m_StarshipActionsCallbackInterface.OnThrusters;
            }
            m_Wrapper.m_StarshipActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @VerticalMovement.started += instance.OnVerticalMovement;
                @VerticalMovement.performed += instance.OnVerticalMovement;
                @VerticalMovement.canceled += instance.OnVerticalMovement;
                @HorizontalMovement.started += instance.OnHorizontalMovement;
                @HorizontalMovement.performed += instance.OnHorizontalMovement;
                @HorizontalMovement.canceled += instance.OnHorizontalMovement;
                @Thrusters.started += instance.OnThrusters;
                @Thrusters.performed += instance.OnThrusters;
                @Thrusters.canceled += instance.OnThrusters;
            }
        }
    }
    public StarshipActions @Starship => new StarshipActions(this);
    public interface IStarshipActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnVerticalMovement(InputAction.CallbackContext context);
        void OnHorizontalMovement(InputAction.CallbackContext context);
        void OnThrusters(InputAction.CallbackContext context);
    }
}
