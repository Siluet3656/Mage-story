using UnityEngine;
using PlayerStaff;

namespace Shard
{
    public class PlayersShard : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothTime;

        private Transform _targetTransform;
        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _targetTransform = FindObjectOfType<PlayerMovement>().GetComponent<Transform>();
            
            if (_targetTransform == null)
            {
                Debug.LogError("No player!");
                enabled = false;
            }
        }

        private void LateUpdate()
        {
            Vector3 targetPosition = _targetTransform.TransformPoint(_offset);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        }
    }
}