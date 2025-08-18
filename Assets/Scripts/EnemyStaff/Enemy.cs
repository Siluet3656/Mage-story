using UnityEngine;
using Data;
using Data.EnemyConfigs;
using Data.Enums;
using EnemyStaff.ConcreteState;
using EnemyStaff.StateSO;
using EntityStaff;
using View;
using Debugging;

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
        
        [Header("Debug")] 
        [SerializeField] private EnemyStatePreview _enemyStatePreview;

        private EnemyConfig _enemyConfig;
        
        private Hp _hp;
        private EnemyView _enemyView;
        private EnemyAttack _enemyAttack;

        private EnemyName _name;

        private void Awake()
        {
            _enemyConfig = EnemyData.Instance.GetEnemyConfig(EnemyName.Prisoner);
            
            EnemyIdleInstance = Instantiate(_enemyConfig.EnemyIdle);
            EnemyEngageInstance = Instantiate(_enemyConfig.EnemyEngage);
            EnemyAttackInstance = Instantiate(_enemyConfig.EnemyAttack);
            EnemyWanderingInstance = Instantiate(_enemyConfig.EnemyWandering);
            EnemyRetreatInstance = Instantiate(_enemyConfig.EnemyRetreat);
            
            StateMachine = new EnemyStateMachine();

            IdleState = new IdleState(this, StateMachine);
            EngageState = new EngageState(this, StateMachine);
            AttackState = new AttackState(this, StateMachine);
            RetreatState = new RetreatState(this, StateMachine);
            WanderingState = new WanderingState(this, StateMachine);

            _hp = GetComponent<Hp>();
            _enemyView = GetComponent<EnemyView>();
            _enemyAttack = GetComponent<EnemyAttack>();
        }

        private void OnEnable()
        {
            _hp.OnDeath += ReturnToPool;
            _hp.SetMaxHealth(_enemyConfig.MaxHp);
            _hp.InitializeHealth();
            
            _name = _enemyConfig.Name;
            
            _enemyView.SetSprite(_enemyConfig.Sprite);
            _enemyView.SetTitle(_enemyConfig.Title);

            if (_enemyConfig is IMelee meleeConfig) _enemyAttack.SetMeleeAttackStats(meleeConfig.AttackDamage, meleeConfig.AttackRate);
        }

        private void OnDisable()
        {
            _hp.OnDeath -= ReturnToPool;
        }

        private void Start()
        {
            EnemyIdleInstance.Initialize(gameObject,this);
            EnemyEngageInstance.Initialize(gameObject,this);
            EnemyAttackInstance.Initialize(gameObject,this);
            EnemyWanderingInstance.Initialize(gameObject,this);
            EnemyRetreatInstance.Initialize(gameObject,this);
            
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentEnemyState.FrameUpdate(Time.deltaTime);
            
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
        
        public EnemyTargetingCircle EngageCircle => _engageCircle;
        public EnemyTargetingCircle AttackCircle => _attackCircle;
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