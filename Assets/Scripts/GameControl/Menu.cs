using UnityEngine;

namespace GameControl
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _spellBook;

        public bool IsBookOpened => _spellBook.alpha > 0;
        
        public void SwitchSpellBookState()
        {
            _spellBook.alpha = _spellBook.alpha > 0 ? 0 : 1;
            _spellBook.blocksRaycasts = !_spellBook.blocksRaycasts;
        }

        public void CloseSpellBook()
        {
            _spellBook.alpha = 0;
            _spellBook.blocksRaycasts = false;
        }
    }
}
