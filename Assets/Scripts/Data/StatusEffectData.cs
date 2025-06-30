using Data.Enums;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "StatusEffectData", menuName = "StatusEffects/StatusEffectData", order = 51)]
    public class StatusEffectData : ScriptableObject
    {
        [SerializeField] private StatusType _type;
        [SerializeField]  private StatusCategory _category;
        [SerializeField]  private float _baseDuration;
        [SerializeField]  private GameObject _visualEffectPrefab;

        public StatusType Type => _type;
        public StatusCategory Category => _category;
        public float BaseDuration => _baseDuration;
        public GameObject VisualEffectPrefab => _visualEffectPrefab;
    }
}