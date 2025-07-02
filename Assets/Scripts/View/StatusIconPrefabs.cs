using UnityEngine;

namespace View
{
    [System.Serializable]
    public class StatusIconPrefabs
    {
        [SerializeField] private GameObject _slow;
        [SerializeField] private GameObject _poison;
        [SerializeField] private GameObject _fireMark;
        [SerializeField] private GameObject _fireAura;
        [SerializeField] private GameObject _iceTomb;

        public GameObject Slow => _slow;
        public GameObject Poison => _poison;
        public GameObject FireMark => _fireMark;
        public GameObject FireAura => _fireAura;
        public GameObject IceTomb => _iceTomb;
    }
}