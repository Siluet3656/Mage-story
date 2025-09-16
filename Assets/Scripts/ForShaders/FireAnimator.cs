using UnityEngine;

namespace ForShaders
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FireAnimator : MonoBehaviour
    {
        private static readonly int TimeValue = Shader.PropertyToID("_TimeValue");
        
        [SerializeField] private Material _fireMaterial;
        
        private void Update()
        {
            if (_fireMaterial != null)
            {
                _fireMaterial.SetFloat(TimeValue, Time.time);
            }
        }
    }
}