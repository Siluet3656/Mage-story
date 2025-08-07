using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Pathfinding
{
    public class FootprintPool : MonoBehaviour {
        [SerializeField] private GameObject _footprintPrefab;
        [SerializeField] private int _initialPoolSize = 5;
    
        private readonly Queue<GameObject> _pool = new Queue<GameObject>();

        private void Awake() {
            for (int i = 0; i < _initialPoolSize; i++) {
                CreateNewFootprint();
            }

            G.FootprintPool = this;
        }

        private GameObject CreateNewFootprint() {
            var footprint = Instantiate(_footprintPrefab, transform, true);
            footprint.SetActive(false);
            _pool.Enqueue(footprint);
            return footprint;
        }

        public GameObject GetFootprint() { 
            if (_pool.Count <= 0) {
                CreateNewFootprint(); 
            }
            var footprint = _pool.Dequeue();
            footprint.SetActive(true);
            G.ActiveFootprints.Add(footprint);
            footprint.transform.SetParent(null);
            return footprint;
        }

        public void ReturnFootprint(GameObject footprint) {
            footprint.SetActive(false);
            G.ActiveFootprints.Remove(footprint);
            _pool.Enqueue(footprint);
            footprint.transform.SetParent(transform);
        }
    }
}