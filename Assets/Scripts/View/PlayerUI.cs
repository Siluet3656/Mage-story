using UnityEngine;
using UnityEngine.UI;
using Data.Enums;

namespace View
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _blinkRefreshBar;
        [SerializeField] private Image _castBar;
        [SerializeField] private Image _remainderBar;
        [SerializeField] private Image[] _fireShards;
        [SerializeField] private Image[] _frostShards;
        [SerializeField] private Image[] _earthShards;
        [SerializeField] private Image[] _gcdBars;
        
        private void Awake()
        {
            _blinkRefreshBar.fillAmount = 1f;
            _castBar.fillAmount = 0f;
            _remainderBar.fillAmount = 0f;

            SetShardsToFull(_fireShards);
            SetShardsToFull(_frostShards);
            SetShardsToFull(_earthShards);
        }

        private void SetShardsToFull(Image[] shards)
        {
            foreach (var shard in shards)
            {
                shard.fillAmount = 1f;
            }
        }
        
        private Image[] GetShardsByType(ShardType type)
        {
            switch (type)
            {
                case ShardType.FireShard:  return _fireShards;
                case ShardType.FrostShard: return _frostShards;
                case ShardType.EarthShard: return _earthShards;
                default: return null;
            }
        }
        
        public void UpdateCastBar(float amount)
        {
            if (amount > 1f || amount < 0f) return;
            
            _castBar.fillAmount = amount;
        }
        
        public void SetCastBarColor(Color color)
        {
            _castBar.color = color;
        }
        
        public void UpdateBlinkRefreshBar(float value)
        {
            if (value < 0f)
            {
                _blinkRefreshBar.fillAmount = 0f;
                return;
            }
            if (value > 1f)
            {
                _blinkRefreshBar.fillAmount = 1f;
                return;
            }
            _blinkRefreshBar.fillAmount = value;
        }
        
        public void UpdateShardsUI(ShardType type, float[] timers, float shardRefreshTime)
        {
            if (shardRefreshTime < 0f) return;
    
            Image[] shards = GetShardsByType(type);
            if (shards == null || shards.Length != timers.Length) return;
    
            for (int i = 0; i < shards.Length; i++)
            {
                shards[i].fillAmount = timers[i] > 0 
                    ? 1 - (timers[i] / shardRefreshTime) 
                    : 1;
            }
        }

        public void UpdateReminderBar(float amount)
        {
            if (amount > 1f || amount < 0f) return;
            
            _remainderBar.fillAmount = amount;
        }

        public void UpdateGcdBars(float value)
        {
            if (value > 1f || value < 0f) return;
            
            foreach (var bar in _gcdBars)
            {
                bar.fillAmount = value;
            }
        }
    }
}
