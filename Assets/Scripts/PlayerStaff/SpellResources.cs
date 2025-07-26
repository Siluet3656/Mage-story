using UnityEngine;
using Data.Enums;
using View;

namespace PlayerStaff
{
    [RequireComponent(typeof(PlayerUI))]
    public class SpellResources : MonoBehaviour
    {
        [Header("Shards")]
        [SerializeField] private float _shardRefreshTime = 5f;
        [Header("Remainder")]
        [SerializeField] private int _maxRemainder = 100;

        private PlayerUI _ui;
        
        private int _currentRemainder;
        private int _fireShardCount;
        private int _frostShardCount;
        private int _earthShardCount;

        private float[] _fireShardRefreshTimers;
        private float[] _frostShardRefreshTimers;
        private float[] _earthShardRefreshTimers;

        private ShardType _lastSpentShardType;
        private bool _hasSpentShard = false;
        
        private const int MaxShards = 3;

        private void Awake()
        {
            _ui = GetComponent<PlayerUI>();
            
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

            _lastSpentShardType = type;
            _hasSpentShard = true;
            
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
                        timers[j] = _shardRefreshTime;
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
            _ui.UpdateReminderBar((float)_currentRemainder / _maxRemainder);
            
            _ui.UpdateShardsUI(ShardType.FireShard, _fireShardRefreshTimers, _shardRefreshTime);
            _ui.UpdateShardsUI(ShardType.FrostShard, _frostShardRefreshTimers, _shardRefreshTime);
            _ui.UpdateShardsUI(ShardType.EarthShard, _earthShardRefreshTimers, _shardRefreshTime);
        }
        
        private void RestoreShard(ref int shardCount, float[] timers)
        {
            if (shardCount >= MaxShards) return;
            
            for (int i = 0; i < timers.Length; i++)
            {
                if (timers[i] > 0)
                {
                    timers[i] = 0;
                    shardCount = Mathf.Min(MaxShards, shardCount + 1);
                    return;
                }
            }
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
        
        public void RestoreLastSpentShard()
        {
            if (!_hasSpentShard) return;

            switch (_lastSpentShardType)
            {
                case ShardType.FireShard:
                    RestoreShard(ref _fireShardCount, _fireShardRefreshTimers);
                    break;
                case ShardType.FrostShard:
                    RestoreShard(ref _frostShardCount, _frostShardRefreshTimers);
                    break;
                case ShardType.EarthShard:
                    RestoreShard(ref _earthShardCount, _earthShardRefreshTimers);
                    break;
            }
        }
    }
}