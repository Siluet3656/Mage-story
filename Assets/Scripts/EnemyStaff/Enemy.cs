using Data;
using Data.Enums;
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
        
        [Header("Debug")] 
        [SerializeField] private EnemyStatePreview _enemyStatePreview;
         
        private void Awake()
        {
            EnemyIdleInstance = Instantiate(EnemyData.Instance.GetEnemyConfig(EnemyName.Prisoner).EnemyIdle);
            EnemyEngageInstance = Instantiate(EnemyData.Instance.GetEnemyConfig(EnemyName.Prisoner).EnemyEngage);
            EnemyAttackInstance = Instantiate(EnemyData.Instance.GetEnemyConfig(EnemyName.Prisoner).EnemyAttack);
            EnemyWanderingInstance = Instantiate(EnemyData.Instance.GetEnemyConfig(EnemyName.Prisoner).EnemyWandering);
            EnemyRetreatInstance = Instantiate(EnemyData.Instance.GetEnemyConfig(EnemyName.Prisoner).EnemyRetreat);
            
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