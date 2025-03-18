using UnityEngine;
public class Spellbook : MonoBehaviour
{
    [SerializeField] private CanvasGroup fireSpellsPage;
    [SerializeField] private CanvasGroup frostSpellsPage;
    [SerializeField] private CanvasGroup earthSpellsPage;
    [SerializeField] private CanvasGroup noElementSpellsPage;
    
    public void OpenFireSpellsPage()
    {
        fireSpellsPage.alpha = 1;
        fireSpellsPage.blocksRaycasts = true;
        
        frostSpellsPage.alpha = 0;
        frostSpellsPage.blocksRaycasts = false;
            
        earthSpellsPage.alpha = 0;
        earthSpellsPage.blocksRaycasts = false;
        
        noElementSpellsPage.alpha = 0;
        noElementSpellsPage.blocksRaycasts = false;
    }
    
    public void OpenFrostSpellsPage()
    {
        fireSpellsPage.alpha = 0;
        fireSpellsPage.blocksRaycasts = false;
        
        frostSpellsPage.alpha = 1;
        frostSpellsPage.blocksRaycasts = true;
            
        earthSpellsPage.alpha = 0;
        earthSpellsPage.blocksRaycasts = false;
        
        noElementSpellsPage.alpha = 0;
        noElementSpellsPage.blocksRaycasts = false;
    }
    
    public void OpenEarthSpellsPage()
    {
        fireSpellsPage.alpha = 0;
        fireSpellsPage.blocksRaycasts = false;
        
        frostSpellsPage.alpha = 0;
        frostSpellsPage.blocksRaycasts = false;
            
        earthSpellsPage.alpha = 1;
        earthSpellsPage.blocksRaycasts = true;
        
        noElementSpellsPage.alpha = 0;
        noElementSpellsPage.blocksRaycasts = false;
    }

    public void OpenNoElementalSpellsPage()
    {
        fireSpellsPage.alpha = 0;
        fireSpellsPage.blocksRaycasts = false;
        
        frostSpellsPage.alpha = 0;
        frostSpellsPage.blocksRaycasts = false;
            
        earthSpellsPage.alpha = 0;
        earthSpellsPage.blocksRaycasts = false;
        
        noElementSpellsPage.alpha = 1;
        noElementSpellsPage.blocksRaycasts = true;
    }
}
