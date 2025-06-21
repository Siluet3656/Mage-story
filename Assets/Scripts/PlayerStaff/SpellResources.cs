using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PlayerStaff
{
    public class SpellResources : MonoBehaviour
    {
        [SerializeField] private int _maxRemainder = 100;
        [SerializeField] private Image _remainderBar;
        [SerializeField] private Image[] _fireShards;
        [SerializeField] private Image[] _frostShards;
        [SerializeField] private Image[] _earthShards;

        private int _currentRemainder;
        private int _fireShardCount;
        private int _frostShardCount;
        private int _earthShardCount;

        private float[] _fireShardRefreshTimers;
        private float[] _frostShardRefreshTimers;
        private float[] _earthShardRefreshTimers;

        private const int MaxShards = 3;
        private const float ShardRefreshTime = 5f;

        private void Start()
        {
            _fireShardCount = MaxShards;
            _frostShardCount = MaxShards;
            _earthShardCount = MaxShards;

            _fireShardRefreshTimers = new float[MaxShards];
            _frostShardRefreshTimers = new float[MaxShards];
            _earthShardRefreshTimers = new float[MaxShards];
        }

        private void Update()
        {
            UpdateShardRefreshTimers();
            UpdateUI();
        }

        public bool HasEnoughResources(Vector3Int shardCost, float remainderCost)
        {
            return _fireShardCount >= shardCost.x &&
                   _frostShardCount >= shardCost.y &&
                   _earthShardCount >= shardCost.z &&
                   _currentRemainder >= remainderCost;
        }

        public void ConsumeResources(Vector3Int shardCost, float remainderCost)
        {
            UseShards(ShardType.FireShard, shardCost.x);
            UseShards(ShardType.FrostShard, shardCost.y);
            UseShards(ShardType.EarthShard, shardCost.z);
            _currentRemainder -= (int)remainderCost;
        }

        private void UseShards(ShardType type, int amount)
        {
            if (amount <= 0) return;

            switch (type)
            {
                case ShardType.FireShard:
                    _fireShardCount = Mathf.Max(0, _fireShardCount - amount);
                    StartRefreshTimers(_fireShardRefreshTimers, amount);
                    break;
                case ShardType.FrostShard:
                    _frostShardCount = Mathf.Max(0, _frostShardCount - amount);
                    StartRefreshTimers(_frostShardRefreshTimers, amount);
                    break;
                case ShardType.EarthShard:
                    _earthShardCount = Mathf.Max(0, _earthShardCount - amount);
                    StartRefreshTimers(_earthShardRefreshTimers, amount);
                    break;
            }

            GainRemainder(20 * amount);
        }

        private void StartRefreshTimers(float[] timers, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < timers.Length; j++)
                {
                    if (timers[j] <= 0)
                    {
                        timers[j] = ShardRefreshTime;
                        break;
                    }
                }
            }
        }

        private void UpdateShardRefreshTimers()
        {
            UpdateSingleShardType(_fireShardRefreshTimers, ref _fireShardCount);
            UpdateSingleShardType(_frostShardRefreshTimers, ref _frostShardCount);
            UpdateSingleShardType(_earthShardRefreshTimers, ref _earthShardCount);
        }

        private void UpdateSingleShardType(float[] timers, ref int shardCount)
        {
            for (int i = 0; i < timers.Length; i++)
            {
                if (timers[i] > 0)
                {
                    timers[i] -= Time.deltaTime;
                    if (timers[i] <= 0 && shardCount < MaxShards)
                    {
                        shardCount++;
                    }
                }
            }
        }

        private void GainRemainder(int amount)
        {
            _currentRemainder = Mathf.Min(_maxRemainder, _currentRemainder + amount);
        }

        private void UpdateUI()
        {
            _remainderBar.fillAmount = (float)_currentRemainder / _maxRemainder;

            // Update shard UI visuals
            UpdateShardUI(_fireShards, _fireShardRefreshTimers);
            UpdateShardUI(_frostShards, _frostShardRefreshTimers);
            UpdateShardUI(_earthShards, _earthShardRefreshTimers);
        }

        private void UpdateShardUI(Image[] shardImages, float[] timers)
        {
            for (int i = 0; i < shardImages.Length; i++)
            {
                shardImages[i].fillAmount = timers[i] > 0
                    ? 1 - (timers[i] / ShardRefreshTime)
                    : 1;
            }
        }
    }
}