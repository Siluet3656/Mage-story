using UnityEngine;
using UnityEngine.EventSystems;

namespace RoguelikeMap
{
    public class MapNodeVisual : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _nodeSprite;
        [SerializeField] private NodeType _nodeType;
        [SerializeField] private int _nodeIndex;
        
        private CombatMapGenerator _mapGenerator;
        private Color _normalColor;
        private Color _hoverColor = new Color(0.8f, 1f, 0.8f);
        private Color _selectedColor = Color.yellow;
        private Color _lockedColor = new Color(0.5f, 0.5f, 0.5f);
        private bool _isUnlocked;
        private bool _isCompleted;

        public void Initialize(CombatMapGenerator mapGenerator, int nodeIndex, NodeType nodeType, bool isUnlocked, bool isCompleted)
        {
            _mapGenerator = mapGenerator;
            _nodeIndex = nodeIndex;
            _nodeType = nodeType;
            _isUnlocked = isUnlocked;
            _isCompleted = isCompleted;

            if (_nodeSprite != null)
            {
                _normalColor = GetNodeTypeColor(nodeType);
                UpdateVisualState();
            }
        }

        private Color GetNodeTypeColor(NodeType type)
        {
            switch (type)
            {
                case NodeType.Battle: return new Color(1f, 0.3f, 0.3f);
                case NodeType.Elite: return new Color(0.8f, 0.2f, 0.8f);
                case NodeType.Rest: return new Color(0.3f, 1f, 0.3f);
                case NodeType.Event: return new Color(1f, 1f, 0.3f);
                case NodeType.Boss: return new Color(0.2f, 0.2f, 0.2f);
                default: return Color.white;
            }
        }

        private void UpdateVisualState()
        {
            if (!_isUnlocked)
                _nodeSprite.color = _lockedColor;
            else if (_isCompleted)
                _nodeSprite.color = new Color(_normalColor.r * 0.5f, _normalColor.g * 0.5f, _normalColor.b * 0.5f);
            else
                _nodeSprite.color = _normalColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isUnlocked && !_isCompleted)
            {
                _mapGenerator.SelectNode(_nodeIndex);
            }
        }

        private void OnMouseEnter()
        {
            if (_isUnlocked && !_isCompleted && _nodeSprite != null)
            {
                _nodeSprite.color = _hoverColor;
            }
        }

        private void OnMouseExit()
        {
            if (_isUnlocked && _nodeSprite != null)
            {
                UpdateVisualState();
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (_nodeSprite != null && _isUnlocked)
            {
                _nodeSprite.color = isSelected ? _selectedColor : (_isCompleted ? 
                    new Color(_normalColor.r * 0.5f, _normalColor.g * 0.5f, _normalColor.b * 0.5f) : 
                    _normalColor);
            }
        }

        public void MarkAsCompleted()
        {
            _isCompleted = true;
            UpdateVisualState();
        }

        public void Unlock()
        {
            _isUnlocked = true;
            UpdateVisualState();
        }
    }
}
