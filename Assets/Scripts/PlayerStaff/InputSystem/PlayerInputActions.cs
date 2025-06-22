// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerStaff/InputSystem/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset Asset { get; }
    public @PlayerInputActions()
    {
        Asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""e518dcb8-2772-472a-9694-b69b5a8f0bde"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""8629dd06-913d-45f5-bcba-0b9463def9a7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Castbar1"",
                    ""type"": ""Button"",
                    ""id"": ""2815da3e-05cb-4b94-b0b0-c856a5d559f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Castbar2"",
                    ""type"": ""Button"",
                    ""id"": ""963004c2-8fe3-455b-84e7-d38c11c7765c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Castbar3"",
                    ""type"": ""Button"",
                    ""id"": ""2dae4d38-d59f-49bf-ae11-9bf8ef186b6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Castbar4"",
                    ""type"": ""Button"",
                    ""id"": ""337fc86a-6b14-49b1-ba19-997e146b4434"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Castbar5"",
                    ""type"": ""Button"",
                    ""id"": ""06734a59-c880-4b76-a7d1-debba38675ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Castbar6"",
                    ""type"": ""Button"",
                    ""id"": ""13c60747-c488-4ac9-a698-d033f356a703"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Castbar7"",
                    ""type"": ""Button"",
                    ""id"": ""eda686a5-5e9e-4dbe-8809-b9ade0081653"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CastInterrupt "",
                    ""type"": ""Button"",
                    ""id"": ""43e81e99-9af4-4736-a013-014e7efa8471"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Blink"",
                    ""type"": ""Button"",
                    ""id"": ""41b4edd3-27c6-4037-81d9-930476b4f6ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FastTarget"",
                    ""type"": ""Button"",
                    ""id"": ""e8a23113-af3a-4c03-aaed-c930aabf075e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""1b36dbeb-71f7-40a2-a69c-d79ed4d6f166"",
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
                    ""id"": ""b79d59af-27e5-4867-a9f0-76009805a91a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d7d55691-d50b-4d27-911a-e6ca57a80a0b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e0c25346-918d-4357-9d75-57b06263532e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ae29c673-d8a0-4aa6-99af-0d384d280871"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bac359f8-3637-41db-a9fa-f4b6f7675c3a"",
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
                    ""id"": ""01f5a336-87b4-4d81-9bc0-cfe4d2221981"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""272eaee2-d7a3-47c5-8590-57c8cbe6c1ed"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c7db8f35-09d3-416c-b1e9-b92f0e32cc90"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7d13f29b-e6fc-4cae-8024-6ae4a6626b59"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3c521a28-6842-4301-aeac-9e8b1b00cc66"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Castbar1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6df9dc4a-42f3-496e-97ee-981d2edcbe74"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Castbar1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9ae8173-1703-406e-bcd1-329edba859ce"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Castbar2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90d44dc8-1011-4db6-9d71-e9fab5464190"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Castbar2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d04538c-3b41-415b-afcd-833b51d6c6f5"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Castbar3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49ad4b0d-3441-46eb-ac0e-abedb3d7ce5c"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Castbar3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01c30dab-9858-4713-91dd-1a819794d4d2"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Castbar4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a703b68b-18e8-471f-b1a7-62bf3294599c"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Castbar4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12bb4e95-c4be-4e72-87b1-c30c6f61d8c8"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Castbar5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""754937b8-e46a-4497-b9fb-e08547aa4bdb"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Castbar5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""604549f9-1bca-4630-ad21-822442dfc38d"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Castbar6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5d91f84-5dcd-4d26-bc04-e31bf16a8c2b"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Castbar6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18a26a4e-bd9e-4866-abfb-f60f5bbfa572"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardALT"",
                    ""action"": ""Castbar7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe417bf5-e6f6-4350-bd79-5933256b754c"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Castbar7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e054ea47-10b4-460e-ac3e-2684714f2b95"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""CastInterrupt "",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""704f24cb-26a4-45f1-b1de-a6af1929ab42"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Blink"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63f76da0-7ea0-4468-9182-7361c1b58fac"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""FastTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""4924de2e-c911-4a2d-bfc5-1ed29b9783a4"",
            ""actions"": [
                {
                    ""name"": ""OpenSpellBook"",
                    ""type"": ""Button"",
                    ""id"": ""9b966065-ac0a-4be3-8b14-25f976dfe2dc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""9796cd40-8537-4092-ae58-9d8cc96846c3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LBM"",
                    ""type"": ""Button"",
                    ""id"": ""f307f5df-fc51-4ccb-956a-143357949088"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RBM"",
                    ""type"": ""Button"",
                    ""id"": ""f13bca49-3213-4bb2-acc2-153cb72219ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d83235f3-bd93-49a7-97b4-9ff754b92f9a"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""OpenSpellBook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9037e43a-b377-4c27-9347-57183000c9f4"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;KeyboardALT"",
                    ""action"": ""OpenSpellBook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d571ab6-083b-48a2-98a9-89bc55e134ff"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;KeyboardALT"",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06419a91-3465-45d4-bc80-6bd9d4e756a9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;KeyboardALT"",
                    ""action"": ""LBM"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0406669-0229-4215-8c4d-9b2c6f408496"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RBM"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""KeyboardALT"",
            ""bindingGroup"": ""KeyboardALT"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        _mPlayer = Asset.FindActionMap("Player", throwIfNotFound: true);
        _mPlayerMovement = _mPlayer.FindAction("Movement", throwIfNotFound: true);
        _mPlayerCastbar1 = _mPlayer.FindAction("Castbar1", throwIfNotFound: true);
        _mPlayerCastbar2 = _mPlayer.FindAction("Castbar2", throwIfNotFound: true);
        _mPlayerCastbar3 = _mPlayer.FindAction("Castbar3", throwIfNotFound: true);
        _mPlayerCastbar4 = _mPlayer.FindAction("Castbar4", throwIfNotFound: true);
        _mPlayerCastbar5 = _mPlayer.FindAction("Castbar5", throwIfNotFound: true);
        _mPlayerCastbar6 = _mPlayer.FindAction("Castbar6", throwIfNotFound: true);
        _mPlayerCastbar7 = _mPlayer.FindAction("Castbar7", throwIfNotFound: true);
        _mPlayerCastInterrupt = _mPlayer.FindAction("CastInterrupt ", throwIfNotFound: true);
        _mPlayerBlink = _mPlayer.FindAction("Blink", throwIfNotFound: true);
        _mPlayerFastTarget = _mPlayer.FindAction("FastTarget", throwIfNotFound: true);
        // UI
        _mUI = Asset.FindActionMap("UI", throwIfNotFound: true);
        _mUIOpenSpellBook = _mUI.FindAction("OpenSpellBook", throwIfNotFound: true);
        _mUIMousePosition = _mUI.FindAction("MousePosition", throwIfNotFound: true);
        _mUILbm = _mUI.FindAction("LBM", throwIfNotFound: true);
        _mUIRbm = _mUI.FindAction("RBM", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(Asset);
    }

    public InputBinding? bindingMask
    {
        get => Asset.bindingMask;
        set => Asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => Asset.devices;
        set => Asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => Asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return Asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return Asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        Asset.Enable();
    }

    public void Disable()
    {
        Asset.Disable();
    }

    // Player
    private readonly InputActionMap _mPlayer;
    private IPlayerActions _mPlayerActionsCallbackInterface;
    private readonly InputAction _mPlayerMovement;
    private readonly InputAction _mPlayerCastbar1;
    private readonly InputAction _mPlayerCastbar2;
    private readonly InputAction _mPlayerCastbar3;
    private readonly InputAction _mPlayerCastbar4;
    private readonly InputAction _mPlayerCastbar5;
    private readonly InputAction _mPlayerCastbar6;
    private readonly InputAction _mPlayerCastbar7;
    private readonly InputAction _mPlayerCastInterrupt;
    private readonly InputAction _mPlayerBlink;
    private readonly InputAction _mPlayerFastTarget;
    public struct PlayerActions
    {
        private @PlayerInputActions _mWrapper;
        public PlayerActions(@PlayerInputActions wrapper) { _mWrapper = wrapper; }
        public InputAction @Movement => _mWrapper._mPlayerMovement;
        public InputAction @Castbar1 => _mWrapper._mPlayerCastbar1;
        public InputAction @Castbar2 => _mWrapper._mPlayerCastbar2;
        public InputAction @Castbar3 => _mWrapper._mPlayerCastbar3;
        public InputAction @Castbar4 => _mWrapper._mPlayerCastbar4;
        public InputAction @Castbar5 => _mWrapper._mPlayerCastbar5;
        public InputAction @Castbar6 => _mWrapper._mPlayerCastbar6;
        public InputAction @Castbar7 => _mWrapper._mPlayerCastbar7;
        public InputAction @CastInterrupt => _mWrapper._mPlayerCastInterrupt;
        public InputAction @Blink => _mWrapper._mPlayerBlink;
        public InputAction @FastTarget => _mWrapper._mPlayerFastTarget;
        public InputActionMap Get() { return _mWrapper._mPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool Enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (_mWrapper._mPlayerActionsCallbackInterface != null)
            {
                @Movement.started -= _mWrapper._mPlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnMovement;
                @Castbar1.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar1;
                @Castbar1.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar1;
                @Castbar1.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar1;
                @Castbar2.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar2;
                @Castbar2.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar2;
                @Castbar2.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar2;
                @Castbar3.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar3;
                @Castbar3.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar3;
                @Castbar3.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar3;
                @Castbar4.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar4;
                @Castbar4.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar4;
                @Castbar4.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar4;
                @Castbar5.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar5;
                @Castbar5.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar5;
                @Castbar5.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar5;
                @Castbar6.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar6;
                @Castbar6.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar6;
                @Castbar6.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar6;
                @Castbar7.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar7;
                @Castbar7.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar7;
                @Castbar7.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastbar7;
                @CastInterrupt.started -= _mWrapper._mPlayerActionsCallbackInterface.OnCastInterrupt;
                @CastInterrupt.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnCastInterrupt;
                @CastInterrupt.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnCastInterrupt;
                @Blink.started -= _mWrapper._mPlayerActionsCallbackInterface.OnBlink;
                @Blink.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnBlink;
                @Blink.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnBlink;
                @FastTarget.started -= _mWrapper._mPlayerActionsCallbackInterface.OnFastTarget;
                @FastTarget.performed -= _mWrapper._mPlayerActionsCallbackInterface.OnFastTarget;
                @FastTarget.canceled -= _mWrapper._mPlayerActionsCallbackInterface.OnFastTarget;
            }
            _mWrapper._mPlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Castbar1.started += instance.OnCastbar1;
                @Castbar1.performed += instance.OnCastbar1;
                @Castbar1.canceled += instance.OnCastbar1;
                @Castbar2.started += instance.OnCastbar2;
                @Castbar2.performed += instance.OnCastbar2;
                @Castbar2.canceled += instance.OnCastbar2;
                @Castbar3.started += instance.OnCastbar3;
                @Castbar3.performed += instance.OnCastbar3;
                @Castbar3.canceled += instance.OnCastbar3;
                @Castbar4.started += instance.OnCastbar4;
                @Castbar4.performed += instance.OnCastbar4;
                @Castbar4.canceled += instance.OnCastbar4;
                @Castbar5.started += instance.OnCastbar5;
                @Castbar5.performed += instance.OnCastbar5;
                @Castbar5.canceled += instance.OnCastbar5;
                @Castbar6.started += instance.OnCastbar6;
                @Castbar6.performed += instance.OnCastbar6;
                @Castbar6.canceled += instance.OnCastbar6;
                @Castbar7.started += instance.OnCastbar7;
                @Castbar7.performed += instance.OnCastbar7;
                @Castbar7.canceled += instance.OnCastbar7;
                @CastInterrupt.started += instance.OnCastInterrupt;
                @CastInterrupt.performed += instance.OnCastInterrupt;
                @CastInterrupt.canceled += instance.OnCastInterrupt;
                @Blink.started += instance.OnBlink;
                @Blink.performed += instance.OnBlink;
                @Blink.canceled += instance.OnBlink;
                @FastTarget.started += instance.OnFastTarget;
                @FastTarget.performed += instance.OnFastTarget;
                @FastTarget.canceled += instance.OnFastTarget;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap _mUI;
    private IUIActions _mUIActionsCallbackInterface;
    private readonly InputAction _mUIOpenSpellBook;
    private readonly InputAction _mUIMousePosition;
    private readonly InputAction _mUILbm;
    private readonly InputAction _mUIRbm;
    public struct UIActions
    {
        private @PlayerInputActions _mWrapper;
        public UIActions(@PlayerInputActions wrapper) { _mWrapper = wrapper; }
        public InputAction @OpenSpellBook => _mWrapper._mUIOpenSpellBook;
        public InputAction @MousePosition => _mWrapper._mUIMousePosition;
        public InputAction Lbm => _mWrapper._mUILbm;
        public InputAction Rbm => _mWrapper._mUIRbm;
        public InputActionMap Get() { return _mWrapper._mUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool Enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (_mWrapper._mUIActionsCallbackInterface != null)
            {
                @OpenSpellBook.started -= _mWrapper._mUIActionsCallbackInterface.OnOpenSpellBook;
                @OpenSpellBook.performed -= _mWrapper._mUIActionsCallbackInterface.OnOpenSpellBook;
                @OpenSpellBook.canceled -= _mWrapper._mUIActionsCallbackInterface.OnOpenSpellBook;
                @MousePosition.started -= _mWrapper._mUIActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= _mWrapper._mUIActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= _mWrapper._mUIActionsCallbackInterface.OnMousePosition;
                Lbm.started -= _mWrapper._mUIActionsCallbackInterface.OnLBM;
                Lbm.performed -= _mWrapper._mUIActionsCallbackInterface.OnLBM;
                Lbm.canceled -= _mWrapper._mUIActionsCallbackInterface.OnLBM;
                Rbm.started -= _mWrapper._mUIActionsCallbackInterface.OnRBM;
                Rbm.performed -= _mWrapper._mUIActionsCallbackInterface.OnRBM;
                Rbm.canceled -= _mWrapper._mUIActionsCallbackInterface.OnRBM;
            }
            _mWrapper._mUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OpenSpellBook.started += instance.OnOpenSpellBook;
                @OpenSpellBook.performed += instance.OnOpenSpellBook;
                @OpenSpellBook.canceled += instance.OnOpenSpellBook;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                Lbm.started += instance.OnLBM;
                Lbm.performed += instance.OnLBM;
                Lbm.canceled += instance.OnLBM;
                Rbm.started += instance.OnRBM;
                Rbm.performed += instance.OnRBM;
                Rbm.canceled += instance.OnRBM;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int _mKeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (_mKeyboardSchemeIndex == -1) _mKeyboardSchemeIndex = Asset.FindControlSchemeIndex("Keyboard");
            return Asset.controlSchemes[_mKeyboardSchemeIndex];
        }
    }
    private int _mKeyboardAltSchemeIndex = -1;
    public InputControlScheme KeyboardAltScheme
    {
        get
        {
            if (_mKeyboardAltSchemeIndex == -1) _mKeyboardAltSchemeIndex = Asset.FindControlSchemeIndex("KeyboardALT");
            return Asset.controlSchemes[_mKeyboardAltSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCastbar1(InputAction.CallbackContext context);
        void OnCastbar2(InputAction.CallbackContext context);
        void OnCastbar3(InputAction.CallbackContext context);
        void OnCastbar4(InputAction.CallbackContext context);
        void OnCastbar5(InputAction.CallbackContext context);
        void OnCastbar6(InputAction.CallbackContext context);
        void OnCastbar7(InputAction.CallbackContext context);
        void OnCastInterrupt(InputAction.CallbackContext context);
        void OnBlink(InputAction.CallbackContext context);
        void OnFastTarget(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnOpenSpellBook(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnLBM(InputAction.CallbackContext context);
        void OnRBM(InputAction.CallbackContext context);
    }
}
