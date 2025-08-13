using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Pathfinding
{
    public class NodeGridGenerator : MonoBehaviour
    {
        [Header("Grid Settings")] 
        [SerializeField, Min(1)] private int _width = 10;
        [SerializeField, Min(1)] private int _height = 10;
        [SerializeField, Min(0.01f)] private float _nodeSize = 1f;
        [SerializeField] private Node _nodePrefab;
        [SerializeField] private LayerMask _unwalkableMask;

        [Header("Connections")] 
        [SerializeField] private bool _enableDiagonalConnections = true;

        [SerializeField] private bool _validateDiagonalMovement = true;

        [Header("Editor Generation")] 
        [SerializeField] private bool _generateInEditMode = true;
        [SerializeField] private bool _generateInPlayMode = false;

        [SerializeField] private bool _clearBeforeGenerate = true;

        private Node[,] _grid;

        #if UNITY_EDITOR
        public void GenerateGridInEditor()
        {
            if (_clearBeforeGenerate)
            {
                ClearExistingNodes();
            }

            GenerateGrid();
            BuildConnections();

            // Сохраняем изменения в сцене
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        public void ClearExistingNodes()
        {
            // Собираем дочерние объекты безопасным способом
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }

            // Удаляем в обратном порядке
            for (int i = children.Count - 1; i >= 0; i--)
            {
                if (children[i] != null)
                {
                    DestroyImmediate(children[i]);
                }
            }

            _grid = null;
        }

        void OnValidate()
        {
            if (_generateInEditMode && !EditorApplication.isPlaying)
            {
                EditorApplication.delayCall += DelayedGenerate; // Задержка для безопасного вызова
            }
        }

        void DelayedGenerate()
        {
            if (this == null) return; // Проверка если объект уже уничтожен
            GenerateGridInEditor();
        }
        #endif

        void Start()
        {
            if (_generateInPlayMode)
            {
                GenerateGrid();
                BuildConnections();
            }
        }

        void GenerateGrid()
        {
            _grid = new Node[_width, _height];
            Vector2 startPos = (Vector2)transform.position - new Vector2(_width * _nodeSize * 0.5f, _height * _nodeSize * 0.5f);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector2 spawnPos = startPos + new Vector2(x * _nodeSize, y * _nodeSize);
                    Node node = Instantiate(_nodePrefab, spawnPos, Quaternion.identity, transform);
                    node.name = $"Node_{x}_{y}";
                    
                    // Проверка проходимости в 2D
                    Collider2D obstacle = Physics2D.OverlapCircle(spawnPos, _nodeSize * 0.4f, _unwalkableMask);
                    if (obstacle != null)
                    {
                        #if UNITY_EDITOR
                        DestroyImmediate(node.gameObject);
                        #else
                        Destroy(node.gameObject);
                        #endif
                        
                        continue;
                    }

                    _grid[x, y] = node;
                }
            }
        }

        void BuildConnections()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (_grid[x, y] == null) continue;

                    _grid[x, y].Connections.Clear();

                    // Основные направления (4-way)
                    TryAddConnection(x + 1, y, _grid[x, y]); // Right
                    TryAddConnection(x - 1, y, _grid[x, y]); // Left
                    TryAddConnection(x, y + 1, _grid[x, y]); // Up
                    TryAddConnection(x, y - 1, _grid[x, y]); // Down

                    // Диагонали (8-way)
                    if (_enableDiagonalConnections)
                    {
                        TryAddDiagonalConnection(x + 1, y + 1, _grid[x, y]); // Up-Right
                        TryAddDiagonalConnection(x - 1, y + 1, _grid[x, y]); // Up-Left
                        TryAddDiagonalConnection(x + 1, y - 1, _grid[x, y]); // Down-Right
                        TryAddDiagonalConnection(x - 1, y - 1, _grid[x, y]); // Down-Left
                    }
                }
            }
        }

        void TryAddConnection(int x, int y, Node originNode)
        {
            if (IsWithinGrid(x, y) && _grid[x, y] != null)
            {
                originNode.Connections.Add(_grid[x, y]);
            }
        }
        
        Vector2 WorldToGridPosition(Vector2 worldPosition)
        {
            Vector2 startPos = (Vector2)transform.position - new Vector2(_width * _nodeSize * 0.5f, _height * _nodeSize * 0.5f);
            Vector2 gridPos = (worldPosition - startPos) / _nodeSize;
            return new Vector2(Mathf.FloorToInt(gridPos.x), Mathf.FloorToInt(gridPos.y));
        }

        void TryAddDiagonalConnection(int x, int y, Node originNode)
        {
            if (!_validateDiagonalMovement)
            {
                TryAddConnection(x, y, originNode);
                return;
            }

            // Проверяем, что соседние клетки тоже проходимы
            if (IsWithinGrid(x, y) && _grid[x, y] != null)
            {
                // Получаем координаты originNode в сетке
                Vector2 originGridPos = WorldToGridPosition(originNode.transform.position);
                int originX = (int)originGridPos.x;
                int originY = (int)originGridPos.y;

                bool horizontalClear = IsWithinGrid(x, originY) && _grid[x, originY] != null;
                bool verticalClear = IsWithinGrid(originX, y) && _grid[originX, y] != null;

                if (horizontalClear && verticalClear)
                {
                    originNode.Connections.Add(_grid[x, y]);
                }
            }
        }

        bool IsWithinGrid(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        #region Debug
        void OnDrawGizmosSelected()
        {
            if (_grid == null) return;

            Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.3f);
            foreach (Node node in _grid)
            {
                if (node != null)
                {
                    Gizmos.DrawWireCube(node.transform.position, Vector3.one * _nodeSize * 0.9f);
                }
            }
        }
        #endregion
    }
}