using System.Collections;
using UnityEngine;
using Data.Enums;

namespace Effects
{
    public class SpellImpactHolder : MonoBehaviour
    {
        [SerializeField] private GameObject _spellImpacts;
        [SerializeField] private GameObject _fireSpellImpact;
        [SerializeField] private GameObject _frostSpellImpact;
        [SerializeField] private GameObject _earthSpellImpact;
        [SerializeField] private GameObject _noElementSpellImpact;

        private readonly float _impactDuration = 1f;

        private void OnDisable()
        {
            StopAllCoroutines();
            DisableAllImpacts();
        }

        private IEnumerator ImpactAwait()
        {
            yield return new WaitForSeconds(_impactDuration);

            DisableAllImpacts();
        }

        private void DisableAllImpacts()
        {
            _spellImpacts.SetActive(false);
            _fireSpellImpact.SetActive(false);
            _frostSpellImpact.SetActive(false);
            _earthSpellImpact.SetActive(false);
            _noElementSpellImpact.SetActive(false);
        }
        
        private void FireImpact()
        {
            if (!gameObject.activeInHierarchy) return;
            
            _spellImpacts.SetActive(true);
            _fireSpellImpact.SetActive(true);

            StartCoroutine(ImpactAwait());
        }
        
        private void FrostImpact()
        {
            if (!gameObject.activeInHierarchy) return;
            
            _spellImpacts.SetActive(true);
            _frostSpellImpact.SetActive(true);

            StartCoroutine(ImpactAwait());
        }
        
        private void EarthImpact()
        {
            if (!gameObject.activeInHierarchy) return;
            
            _spellImpacts.SetActive(true);
            _earthSpellImpact.SetActive(true);

            StartCoroutine(ImpactAwait());
        }
        
        private void NoElementImpact()
        {
            if (!gameObject.activeInHierarchy) return;
            
            _spellImpacts.SetActive(true);
            _noElementSpellImpact.SetActive(true);

            StartCoroutine(ImpactAwait());
        }
        
        public void Impact(SpellElementType elementType)
        {
            switch (elementType)
            {
                case SpellElementType.Fire:
                    FireImpact();
                    break;
                case SpellElementType.Frost:
                    FrostImpact();
                    break;
                case SpellElementType.Earth:
                    EarthImpact();
                    break;
                case SpellElementType.NoElemental:
                    NoElementImpact();
                    break;
            }
        }
    }
}
