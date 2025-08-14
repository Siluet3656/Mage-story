using UnityEngine;
using EnemyStaff.ConcreteState;
using EnemyStaff.StateSO;
using Debugging;

namespace EnemyStaff
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyTargeting))]
    [RequireComponent(typeof(EnemyAttack))]
    public class Enemy : MonoBehaviour
    {
        [Header("Player Search Settings")]
        [SerializeField] private EnemyTargetingCircle _attackCircle;
        [SerializeField] private EnemyTargetingCircle _engageCircle;
        
        [Header("States")] 
        [SerializeField] private EnemyIdleSoBase _enemyIdleInstance;
        [SerializeField] private EnemyEngageSoBase _enemyEngageInstance;
        [SerializeField] private EnemyAttackSoBase _enemyAttackInstance;
        [SerializeField] private EnemyWanderingSoBase _enemyWanderingInstance;
        [SerializeField] private EnemyRetreatSoBase _enemyRetreatInstance;
        
        [Header("Debug")] 
        [SerializeField] private EnemyStatePreview _enemyStatePreview;
         
        private void Awake()
        {
            EnemyIdleInstance = Instantiate(_enemyIdleInstance);
            EnemyEngageInstance = Instantiate(_enemyEngageInstance);
            EnemyAttackInstance = Instantiate(_enemyAttackInstance);
            EnemyWanderingInstance = Instantiate(_enemyWanderingInstance);
            EnemyRetreatInstance = Instantiate(_enemyRetreatInstance);
            
            StateMachine = new EnemyStateMachine();

            IdleState = new IdleState(this, StateMachine);
            EngageState = new EngageState(this, StateMachine);
            AttackState = new AttackState(this, StateMachine);
            RetreatState = new RetreatState(this, StateMachine);
            WanderingState = new WanderingState(this, StateMachine);
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