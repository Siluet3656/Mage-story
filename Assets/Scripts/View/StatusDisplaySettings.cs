using UnityEngine;

namespace View
{
    [System.Serializable]
    public class StatusDisplaySettings
    {
        [SerializeField] private float _defaultX;
        [SerializeField] private float _defaultY;
        [SerializeField] private float _offset;

        public float DefaultX => _defaultX;
        public float DefaultY => _defaultY;
        public float Offset => _offset;
    }
}