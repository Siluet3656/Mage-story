using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Menu : MonoBehaviour
{
    [FormerlySerializedAs("spellbook")] [SerializeField] private CanvasGroup _spellbook;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.UI.OpenSpellBook.performed += SwitchSpellBookState;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    private void SwitchSpellBookState(InputAction.CallbackContext context)
    {
        _spellbook.alpha = _spellbook.alpha > 0 ? 0 : 1;
        _spellbook.blocksRaycasts = _spellbook.blocksRaycasts? false : true;
    }
}
