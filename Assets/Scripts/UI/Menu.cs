using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _spellBook;

        public void SwitchSpellBookState()
        {
            _spellBook.alpha = _spellBook.alpha > 0 ? 0 : 1;
            _spellBook.blocksRaycasts = !_spellBook.blocksRaycasts;
        }
    }
}
