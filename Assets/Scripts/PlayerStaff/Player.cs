using UnityEngine;
using Data;

namespace PlayerStaff
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _footprintSpawnRate;
        
        private float _passedTime;
       
        
        private void Awake()
        {
            G.Player = this;
        }

        private void Update()
        {
            _passedTime += Time.deltaTime;

            if (_passedTime >= _footprintSpawnRate)
            {
                _passedTime = 0f;
                
                var footprint= G.FootprintPool.GetFootprint();
                footprint.transform.position = transform.position;
            }
        }
    }
}
