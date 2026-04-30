using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeMap
{
    public class CombatStartButton : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private CombatMapGenerator _mapGenerator;
        [SerializeField] private GameObject _mapContainer;

        private bool _isInCombat = false;

        private void Awake()
        {
            if (_startButton != null)
            {
                _startButton.onClick.AddListener(OnStartButtonClick);
            }
        }

        private void OnStartButtonClick()
        {
            if (_isInCombat)
                return;

            StartRoguelikeRun();
        }

        public void StartRoguelikeRun()
        {
            _isInCombat = true;
            
            if (_mapContainer != null)
                _mapContainer.SetActive(true);

            if (_mapGenerator != null)
            {
                _mapGenerator.GenerateMap();
                Debug.Log("Roguelike map generated! Select a node and click Start Combat.");
            }
        }

        public void OnCombatComplete()
        {
            if (_mapGenerator != null)
            {
                _mapGenerator.CompleteCurrentNode();
            }
        }

        public void OnRunComplete()
        {
            _isInCombat = false;
            if (_mapContainer != null)
                _mapContainer.SetActive(false);
            
            Debug.Log("Run complete! Returning to lobby...");
            // Here you would transition back to the lobby scene
        }
    }
}
