using UnityEngine;
using Data;
using Data.EnemyConfigs;
using Data.Enums;
using EnemyStaff.ConcreteState;
using EnemyStaff.StateSO;
using EntityStaff;
using View;
using Debugging;
using PlayerStaff;
using Statuses;

namespace EnemyStaff
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyTargeting))]
    [RequireComponent(typeof(EnemyAttack))]
    [RequireComponent(typeof(EnemyView))]
    [RequireComponent(typeof(Hp))]
    public class Enemy : MonoBehaviour
    {
        [Header("Player Search Settings")]
        [SerializeField] private EnemyTargetingCircle _attackCircle;
        [SerializeField] private EnemyTargetingCircle _engageCircle;
        [SerializeField] private TargetingCircle _healCastingCircle;
        
        [Header("Debug")] 
        [SerializeField] private EnemyStatePreview _enemyStatePreview;

        private EnemyConfig _enemyConfig;
        
        private Hp _hp;
        private EnemyView _enemyView;
        private EnemyAttack _enemyAttack;
        private StatusController _enemyStatusController;

        private EnemyName _name;

        private bool _isPooled;

        private void Awake()
        { 
            _hp = GetComponent<Hp>();
            _enemyView = GetComponent<EnemyView>();
            _enemyAttack = GetComponent<EnemyAttack>();
            _enemyStatusController = GetComponent<StatusController>();

            StateMachine = new EnemyStateMachine();

            IdleState = new IdleState(this, StateMachine);
            EngageState = new EngageState(this, StateMachine);
            AttackState = new AttackState(this, StateMachine);
            RetreatState = new RetreatState(this, StateMachine);
            WanderingState = new WanderingState(this, StateMachine);
        }
        
        private void Initialize(EnemyName enemyName)
        {
            _name = enemyName;
            _enemyConfig = EnemyData.Instance.GetEnemyConfig(_name);
            
            _enemyView.SetSprite(_enemyConfig.Sprite);
            _enemyView.SetTitle(_enemyConfig.Title);

            if (_enemyConfig is IMelee meleeConfig) _enemyAttack.SetAttackStats(meleeConfig.AttackDamage, meleeConfig.AttackRate);
            if (_enemyConfig is IRange rangeConfig) _enemyAttack.SetAttackStats(rangeConfig.AttackDamage, rangeConfig.AttackRate);
            if (_enemyConfig is ICaster casterConfig) _enemyAttack.SetAttackStats(casterConfig.CastValue, casterConfig.CastRate);
            
            _hp.SetMaxHealth(_enemyConfig.MaxHp);
            _hp.InitializeHealth();
            
            EnemyIdleInstance = Instantiate(_enemyConfig.EnemyIdle);
            EnemyEngageInstance = Instantiate(_enemyConfig.EnemyEngage);
            EnemyAttackInstance = Instantiate(_enemyConfig.EnemyAttack);
            EnemyWanderingInstance = Instantiate(_enemyConfig.EnemyWandering);
            EnemyRetreatInstance = Instantiate(_enemyConfig.EnemyRetreat);
            
            EnemyIdleInstance.Initialize(gameObject,this);
            EnemyEngageInstance.Initialize(gameObject,this);
            EnemyAttackInstance.Initialize(gameObject,this);
            EnemyWanderingInstance.Initialize(gameObject,this);
            EnemyRetreatInstance.Initialize(gameObject,this);
            
            StateMachine.Initialize(IdleState);
        }

        private void OnEnable()
        {
            _hp.OnDeath += ReturnToPool;

            if (_isPooled) Initialize(_name);
        }

        private void OnDisable()
        {
            _hp.OnDeath -= ReturnToPool;
        }

        private void Update()
        {
            StateMachine.CurrentEnemyState.FrameUpdate(Time.deltaTime);
            
            if (_enemyStatePreview != null)
                _enemyStatePreview.UpdateState(StateMachine.CurrentEnemyState);
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentEnemyState.PhysicsUpdate();
        }

        private void ReturnToPool()
        {
            EnemyFactory.Instance.ReturnEnemy(_name, this);
        }

        public void AddToPool(EnemyName enemyName)
        {
            Initialize(enemyName);
            _isPooled = true;
        }
        
        public EnemyTargetingCircle EngageCircle => _engageCircle;
        public EnemyTargetingCircle AttackCircle => _attackCircle;
        public TargetingCircle HealCastingCircle => _healCastingCircle;
        public StatusController EnemyStatusController => _enemyStatusController;
        public EnemyStateMachine StateMachine { get; private set; }
        public IdleState IdleState { get; private set; }
        public EngageState EngageState { get; private set; }
        public AttackState AttackState { get; private set; }
        public RetreatState RetreatState { get; private set; }
        public WanderingState WanderingState { get; private set; }
        public EnemyIdleSoBase EnemyIdleInstance { get; private set; }
        public EnemyEngageSoBase EnemyEngageInstance { get; private set; }
        public EnemyAttackSoBase EnemyAttackInstance { get; private set; }
        public EnemyWanderingSoBase EnemyWanderingInstance { get; private set; }
        public EnemyRetreatSoBase EnemyRetreatInstance { get; private set; }
    }
}