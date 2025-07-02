using EntityResources;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class HpView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _additionalHealthBar;
        [SerializeField] private Image _mainShieldStackBar;
        [SerializeField] private Image[] _shieldStackBars;

        private Hp _hp;

        private void Awake()
        {
            _hp = GetComponent<Hp>();
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            _hp.OnHealthChanged += UpdateHealthBar;
            _hp.OnAdditionalHealthChanged += UpdateOverHealthBar;
            _hp.OnShieldStacksChanged += UpdateShieldUI;
        }

        private void UnsubscribeFromEvents()
        {
            if (_hp == null) return;
            
            _hp.OnHealthChanged -= UpdateHealthBar;
            _hp.OnAdditionalHealthChanged -= UpdateOverHealthBar;
            _hp.OnShieldStacksChanged -= UpdateShieldUI;
        }

        private void UpdateHealthBar(float currentHealth)
        {
            if (_hp == null || _healthBar == null) return;
            
            _healthBar.fillAmount = currentHealth / _hp.MaxHealth;
        }

        private void UpdateOverHealthBar(float overHealth)
        {
            if (_hp == null || _additionalHealthBar == null) return;
            
            _additionalHealthBar.fillAmount = overHealth / _hp.FrostAegisAdditionalHealthAmount;
        }

        private void UpdateShieldUI(int shieldStacks)
        {
            if (_mainShieldStackBar == null || _shieldStackBars == null) return;
            
            _mainShieldStackBar.gameObject.SetActive(shieldStacks > 0);
            
            for (int i = 0; i < _shieldStackBars.Length; i++)
            {
                if (i < _shieldStackBars.Length)
                {
                    _shieldStackBars[i].gameObject.SetActive(i < shieldStacks);
                }
            }
        }
    }
}