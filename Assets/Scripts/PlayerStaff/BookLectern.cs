using UnityEngine;
using GameControl;

namespace PlayerStaff
{
    /// <summary>
    /// A book lectern in the lobby that allows the player to open the spellbook and change spells.
    /// Player needs to be in trigger range and press the interact key to open the spellbook.
    /// </summary>
    public class BookLectern : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float _interactionRange = 2f;
        [SerializeField] private KeyCode _interactKey = KeyCode.E;
        
        [Header("Visual Feedback")]
        [SerializeField] private GameObject _interactionPrompt;
        [SerializeField] private SpriteRenderer _highlightRenderer;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _highlightColor = Color.yellow;
        
        private bool _playerInRange;
        private Menu _menu;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _menu = FindObjectOfType<Menu>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (_menu == null)
            {
                Debug.LogError("Menu not found in scene! BookLectern requires a Menu component somewhere in the scene.");
            }
            
            if (_spriteRenderer == null && _highlightRenderer == null)
            {
                _highlightRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
        }
        
        private void Start()
        {
            if (_interactionPrompt != null)
            {
                _interactionPrompt.SetActive(false);
            }
            
            SetHighlight(false);
        }
        
        private void Update()
        {
            if (_playerInRange && Input.GetKeyDown(_interactKey))
            {
                OpenSpellbook();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
                ShowInteractionPrompt(true);
                SetHighlight(true);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
                ShowInteractionPrompt(false);
                SetHighlight(false);
                
                // Close spellbook if player leaves while it's open
                if (_menu != null && _menu.IsBookOpened)
                {
                    _menu.CloseSpellBook();
                }
            }
        }
        
        private void OpenSpellbook()
        {
            if (_menu != null)
            {
                _menu.SwitchSpellBookState();
            }
        }
        
        private void ShowInteractionPrompt(bool show)
        {
            if (_interactionPrompt != null)
            {
                _interactionPrompt.SetActive(show);
            }
        }
        
        private void SetHighlight(bool highlight)
        {
            SpriteRenderer rendererToUse = _highlightRenderer ?? _spriteRenderer;
            
            if (rendererToUse != null)
            {
                rendererToUse.color = highlight ? _highlightColor : _defaultColor;
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            // Draw interaction range circle in editor
            Gizmos.color = new Color(1, 1, 0, 0.3f);
            Gizmos.DrawWireSphere(transform.position, _interactionRange);
        }
    }
}
