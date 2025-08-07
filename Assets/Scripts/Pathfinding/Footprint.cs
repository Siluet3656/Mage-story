using UnityEngine;
using Data;

namespace Pathfinding
{
    public class Footprint : MonoBehaviour {
        [SerializeField] private float _lifetime = 5f;
        private float _currentTime;

        private void Awake() {
            _currentTime = 0f;
        }

        private void Update() {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _lifetime) {
                G.FootprintPool.ReturnFootprint(gameObject);
                _currentTime = 0f;
            }
        }
    }
}