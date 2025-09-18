using UnityEngine;

namespace ForShaders
{
    [RequireComponent(typeof(Renderer))]
    public class FireAnimator : MonoBehaviour
    {
        private static readonly int TimeValue = Shader.PropertyToID("_TimeValue");

        [SerializeField] private Material _fireMaterial;
        [SerializeField, Min(1f)] private float _scrollSpeed = 1f;

        private void Update()
        {
            if (_fireMaterial != null)
            {
                _fireMaterial.SetFloat(TimeValue, Time.time * _scrollSpeed);
            }
        }
    }
}