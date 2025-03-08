using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private CanvasGroup spellbook;
    private KeyCode spellbookKey = KeyCode.T;

    private void Update()
    {
        if (Input.GetKeyDown(spellbookKey))
        {
            spellbook.alpha = spellbook.alpha > 0 ? 0 : 1;
            spellbook.blocksRaycasts = spellbook.blocksRaycasts == true? false : true;
        }
    }
}
