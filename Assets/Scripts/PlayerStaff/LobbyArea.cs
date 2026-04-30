using UnityEngine;

namespace PlayerStaff
{
    /// <summary>
    /// A simple lobby area controller for the roguelike game.
    /// This marks an area as a safe zone/lobby where players can prepare before entering dungeons.
    /// </summary>
    public class LobbyArea : MonoBehaviour
    {
        [Header("Lobby Settings")]
        [SerializeField] private string _lobbyName = "Starting Lobby";
        [SerializeField] private bool _allowCombat = false;
        
        [Header("Spawn Points")]
        [SerializeField] private Transform[] _spawnPoints;
        
        [Header("References")]
        [SerializeField] private BookLectern[] _bookLecterns;
        
        private void Awake()
        {
            Debug.Log($"Lobby '{_lobbyName}' initialized. Combat allowed: {_allowCombat}");
            
            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                Debug.LogWarning($"No spawn points defined in lobby '{_lobbyName}'. Creating default spawn point at lobby center.");
                _spawnPoints = new Transform[1];
                _spawnPoints[0] = transform;
            }
        }
        
        public Transform GetRandomSpawnPoint()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                return transform;
            }
            
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex];
        }
        
        public Transform GetSpawnPoint(int index)
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                return transform;
            }
            
            return _spawnPoints[Mathf.Clamp(index, 0, _spawnPoints.Length - 1)];
        }
        
        public bool IsCombatAllowed()
        {
            return _allowCombat;
        }
        
        private void OnDrawGizmos()
        {
            // Draw lobby bounds (assuming box collider or just the area)
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                Bounds bounds = col.bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
            else
            {
                // Default visualization if no collider
                Gizmos.DrawWireSphere(transform.position, 5f);
            }
            
            // Draw spawn points
            if (_spawnPoints != null)
            {
                foreach (Transform spawnPoint in _spawnPoints)
                {
                    if (spawnPoint != null)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(spawnPoint.position, 0.3f);
                    }
                }
            }
        }
    }
}
