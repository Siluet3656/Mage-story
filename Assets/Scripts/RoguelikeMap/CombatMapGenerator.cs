using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeMap
{
    public class CombatMapGenerator : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] private int _columnCount = 7;
        [SerializeField] private int _minRowsPerColumn = 3;
        [SerializeField] private int _maxRowsPerColumn = 5;
        [SerializeField] private float _nodeSpacingX = 3f;
        [SerializeField] private float _nodeSpacingY = 2.5f;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _battleNodePrefab;
        [SerializeField] private GameObject _eliteNodePrefab;
        [SerializeField] private GameObject _restNodePrefab;
        [SerializeField] private GameObject _eventNodePrefab;
        [SerializeField] private GameObject _bossNodePrefab;
        [SerializeField] private GameObject _connectionLinePrefab;
        
        [Header("Parent Transform")]
        [SerializeField] private Transform _nodesParent;
        [SerializeField] private Transform _connectionsParent;

        private List<MapNodeData> _allNodes = new List<MapNodeData>();
        private List<MapNodeVisual> _nodeVisuals = new List<MapNodeVisual>();
        private int _currentNodeIndex = -1;
        private int _selectedNodeIndex = -1;

        public bool IsMapGenerated { get; private set; }
        public event System.Action<int> OnNodeSelected;
        public event System.Action OnMapCompleted;

        public void GenerateMap()
        {
            ClearMap();
            _allNodes.Clear();
            _nodeVisuals.Clear();
            
            int nodeIndex = 0;
            int[] columnRowCounts = new int[_columnCount];
            
            // Generate row counts for each column
            for (int col = 0; col < _columnCount; col++)
            {
                if (col == 0)
                    columnRowCounts[col] = 1; // Start with single node
                else if (col == _columnCount - 1)
                    columnRowCounts[col] = 1; // Boss is single node
                else
                    columnRowCounts[col] = Random.Range(_minRowsPerColumn, _maxRowsPerColumn + 1);
            }

            // Create nodes for each column
            for (int col = 0; col < _columnCount; col++)
            {
                int rowCount = columnRowCounts[col];
                
                for (int row = 0; row < rowCount; row++)
                {
                    MapNodeData nodeData = new MapNodeData
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        Type = DetermineNodeType(col, rowCount, row),
                        IsUnlocked = col == 0 && row == 0,
                        IsCompleted = false
                    };

                    // Create visual
                    GameObject nodePrefab = GetNodePrefab(nodeData.Type);
                    if (nodePrefab != null)
                    {
                        Vector3 position = new Vector3(
                            col * _nodeSpacingX,
                            (row - (rowCount - 1) / 2f) * _nodeSpacingY,
                            0
                        );

                        GameObject nodeObj = Instantiate(nodePrefab, position, Quaternion.identity, _nodesParent);
                        MapNodeVisual nodeVisual = nodeObj.GetComponent<MapNodeVisual>();
                        
                        if (nodeVisual == null)
                            nodeVisual = nodeObj.AddComponent<MapNodeVisual>();

                        nodeVisual.Initialize(this, nodeIndex, nodeData.Type, nodeData.IsUnlocked, nodeData.IsCompleted);
                        _nodeVisuals.Add(nodeVisual);
                    }

                    _allNodes.Add(nodeData);
                    nodeIndex++;
                }
            }

            // Create connections
            CreateConnections(columnRowCounts);
            
            _currentNodeIndex = 0;
            _selectedNodeIndex = 0;
            IsMapGenerated = true;
            
            UpdateNodeVisuals();
        }

        private NodeType DetermineNodeType(int col, int rowCount, int row)
        {
            if (col == _columnCount - 1)
                return NodeType.Boss;
            
            if (col == 0)
                return NodeType.Battle;

            float chance = Random.value;
            if (chance < 0.5f)
                return NodeType.Battle;
            else if (chance < 0.7f)
                return NodeType.Elite;
            else if (chance < 0.9f)
                return NodeType.Rest;
            else
                return NodeType.Event;
        }

        private GameObject GetNodePrefab(NodeType type)
        {
            switch (type)
            {
                case NodeType.Battle: return _battleNodePrefab;
                case NodeType.Elite: return _eliteNodePrefab;
                case NodeType.Rest: return _restNodePrefab;
                case NodeType.Event: return _eventNodePrefab;
                case NodeType.Boss: return _bossNodePrefab;
                default: return _battleNodePrefab;
            }
        }

        private void CreateConnections(int[] columnRowCounts)
        {
            for (int col = 0; col < _columnCount - 1; col++)
            {
                int currentColNodes = columnRowCounts[col];
                int nextColNodes = columnRowCounts[col + 1];

                for (int currentRow = 0; currentRow < currentColNodes; currentRow++)
                {
                    int currentNodeIndex = GetNodeIndex(col, currentRow, columnRowCounts);
                    
                    // Connect to 1-2 nodes in next column
                    int connections = Mathf.Min(2, nextColNodes);
                    int startRow = Mathf.Max(0, currentRow - 1);
                    
                    for (int i = 0; i < connections; i++)
                    {
                        int targetRow = Mathf.Min(startRow + i, nextColNodes - 1);
                        int targetNodeIndex = GetNodeIndex(col + 1, targetRow, columnRowCounts);
                        
                        if (!_allNodes[currentNodeIndex].ConnectedNodeIndices.Contains(targetNodeIndex))
                            _allNodes[currentNodeIndex].ConnectedNodeIndices.Add(targetNodeIndex);
                    }
                }
            }
        }

        private int GetNodeIndex(int col, int row, int[] columnRowCounts)
        {
            int index = 0;
            for (int c = 0; c < col; c++)
                index += columnRowCounts[c];
            return index + row;
        }

        public void SelectNode(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex >= _allNodes.Count)
                return;

            MapNodeData selectedNode = _allNodes[nodeIndex];
            
            // Check if node is reachable from current node
            if (_currentNodeIndex >= 0)
            {
                MapNodeData currentNode = _allNodes[_currentNodeIndex];
                if (!currentNode.ConnectedNodeIndices.Contains(nodeIndex))
                    return;
            }

            _selectedNodeIndex = nodeIndex;
            UpdateNodeVisuals();
            OnNodeSelected?.Invoke(nodeIndex);
        }

        public void StartCombat()
        {
            if (_selectedNodeIndex < 0 || _selectedNodeIndex >= _allNodes.Count)
                return;

            MapNodeData node = _allNodes[_selectedNodeIndex];
            if (!node.IsUnlocked || node.IsCompleted)
                return;

            // Here you would trigger the combat scene
            Debug.Log($"Starting combat at node {_selectedNodeIndex}, Type: {node.Type}");
        }

        public void CompleteCurrentNode()
        {
            if (_selectedNodeIndex < 0)
                return;

            _allNodes[_selectedNodeIndex].IsCompleted = true;
            _currentNodeIndex = _selectedNodeIndex;

            // Unlock connected nodes
            foreach (int connectedIndex in _allNodes[_currentNodeIndex].ConnectedNodeIndices)
            {
                _allNodes[connectedIndex].IsUnlocked = true;
            }

            // Check if map is completed (reached boss)
            if (_allNodes[_currentNodeIndex].ColumnIndex == _columnCount - 1)
            {
                OnMapCompleted?.Invoke();
            }

            UpdateNodeVisuals();
        }

        private void UpdateNodeVisuals()
        {
            for (int i = 0; i < _nodeVisuals.Count; i++)
            {
                if (_nodeVisuals[i] != null)
                {
                    _nodeVisuals[i].SetSelected(i == _selectedNodeIndex);
                    
                    if (_allNodes[i].IsCompleted)
                        _nodeVisuals[i].MarkAsCompleted();
                    else if (_allNodes[i].IsUnlocked)
                        _nodeVisuals[i].Unlock();
                }
            }
        }

        private void ClearMap()
        {
            if (_nodesParent != null)
            {
                foreach (Transform child in _nodesParent)
                    Destroy(child.gameObject);
            }

            if (_connectionsParent != null)
            {
                foreach (Transform child in _connectionsParent)
                    Destroy(child.gameObject);
            }
        }

        public MapNodeData GetCurrentNode()
        {
            if (_currentNodeIndex >= 0 && _currentNodeIndex < _allNodes.Count)
                return _allNodes[_currentNodeIndex];
            return null;
        }

        public MapNodeData GetSelectedNode()
        {
            if (_selectedNodeIndex >= 0 && _selectedNodeIndex < _allNodes.Count)
                return _allNodes[_selectedNodeIndex];
            return null;
        }
    }
}
