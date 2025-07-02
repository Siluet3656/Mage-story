using UnityEngine;
using UnityEngine.Serialization;

namespace View
{
    public class Spellbook : MonoBehaviour
    {
        [FormerlySerializedAs("fireSpellsPage")] [SerializeField] private CanvasGroup _fireSpellsPage;
        [FormerlySerializedAs("frostSpellsPage")] [SerializeField] private CanvasGroup _frostSpellsPage;
        [FormerlySerializedAs("earthSpellsPage")] [SerializeField] private CanvasGroup _earthSpellsPage;
        [FormerlySerializedAs("noElementSpellsPage")] [SerializeField] private CanvasGroup _noElementSpellsPage;
        [FormerlySerializedAs("arcanumsSpellsPage")] [SerializeField] private CanvasGroup _arcanumsSpellsPage;
    
        public void OpenFireSpellsPage()
        {
            _fireSpellsPage.alpha = 1;
            _fireSpellsPage.blocksRaycasts = true;
        
            _frostSpellsPage.alpha = 0;
            _frostSpellsPage.blocksRaycasts = false;
            
            _earthSpellsPage.alpha = 0;
            _earthSpellsPage.blocksRaycasts = false;
        
            _noElementSpellsPage.alpha = 0;
            _noElementSpellsPage.blocksRaycasts = false;
        
            _arcanumsSpellsPage.alpha = 0;
            _arcanumsSpellsPage.blocksRaycasts = false;
        }
    
        public void OpenFrostSpellsPage()
        {
            _fireSpellsPage.alpha = 0;
            _fireSpellsPage.blocksRaycasts = false;
        
            _frostSpellsPage.alpha = 1;
            _frostSpellsPage.blocksRaycasts = true;
            
            _earthSpellsPage.alpha = 0;
            _earthSpellsPage.blocksRaycasts = false;
        
            _noElementSpellsPage.alpha = 0;
            _noElementSpellsPage.blocksRaycasts = false;
        
            _arcanumsSpellsPage.alpha = 0;
            _arcanumsSpellsPage.blocksRaycasts = false;
        }
    
        public void OpenEarthSpellsPage()
        {
            _fireSpellsPage.alpha = 0;
            _fireSpellsPage.blocksRaycasts = false;
        
            _frostSpellsPage.alpha = 0;
            _frostSpellsPage.blocksRaycasts = false;
            
            _earthSpellsPage.alpha = 1;
            _earthSpellsPage.blocksRaycasts = true;
        
            _noElementSpellsPage.alpha = 0;
            _noElementSpellsPage.blocksRaycasts = false;
        
            _arcanumsSpellsPage.alpha = 0;
            _arcanumsSpellsPage.blocksRaycasts = false;
        }

        public void OpenNoElementalSpellsPage()
        {
            _fireSpellsPage.alpha = 0;
            _fireSpellsPage.blocksRaycasts = false;
        
            _frostSpellsPage.alpha = 0;
            _frostSpellsPage.blocksRaycasts = false;
            
            _earthSpellsPage.alpha = 0;
            _earthSpellsPage.blocksRaycasts = false;
        
            _noElementSpellsPage.alpha = 1;
            _noElementSpellsPage.blocksRaycasts = true;
        
            _arcanumsSpellsPage.alpha = 0;
            _arcanumsSpellsPage.blocksRaycasts = false;
        }
    
        public void OpenArcanumSpellsPage()
        {
            _fireSpellsPage.alpha = 0;
            _fireSpellsPage.blocksRaycasts = false;
        
            _frostSpellsPage.alpha = 0;
            _frostSpellsPage.blocksRaycasts = false;
            
            _earthSpellsPage.alpha = 0;
            _earthSpellsPage.blocksRaycasts = false;
        
            _noElementSpellsPage.alpha = 0;
            _noElementSpellsPage.blocksRaycasts = false;
        
            _arcanumsSpellsPage.alpha = 1;
            _arcanumsSpellsPage.blocksRaycasts = true;
        }
    }
}
