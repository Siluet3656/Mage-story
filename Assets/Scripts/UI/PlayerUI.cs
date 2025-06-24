using UnityEngine;
using UnityEngine.UI;
using Data.Enums;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _blinkRefreshBar;
        [SerializeField] private Image _castBar;
        [SerializeField] private Image _remainderBar;
        [SerializeField] private Image[] _fireShards;
        [SerializeField] private Image[] _frostShards;
        [SerializeField] private Image[] _earthShards;
        
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
        
        public void ChangeCastBarFillAmount(float amount)
        {
            _castBar.fillAmount = amount;
        }
        
        public void SetCastBarColor(Color color)
        {
            _castBar.color = color;
        }
        
        public void SetBlinkRefreshBarFillAmount(float value)
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
            _remainderBar.fillAmount = amount;
        }
    }
}
