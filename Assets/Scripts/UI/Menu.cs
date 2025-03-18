using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private CanvasGroup spellbook;
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
        spellbook.alpha = spellbook.alpha > 0 ? 0 : 1;
        spellbook.blocksRaycasts = spellbook.blocksRaycasts? false : true;
    }
}
