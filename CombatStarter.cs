using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CombatStarter : MonoBehaviour
{
    [Header("References")]
    public GameObject lobbyArea;
    public GameObject combatArea;
    
    [Header("Map Generation")]
    public int mapWidth = 7; // Number of columns (stages)
    public int mapHeight = 5; // Number of rows per column
    public float nodeSpacing = 2f;
    public float startYOffset = 0f;
    
    [Header("Node Prefabs")]
    public GameObject battleNodePrefab;
    public GameObject eliteNodePrefab;
    public GameObject restNodePrefab;
    public GameObject eventNodePrefab;
    public GameObject bossNodePrefab;
    public GameObject connectionLinePrefab;
    
    [Header("Map Data")]
    public MapData currentMapData;
    
    private List<MapNode> allNodes = new List<MapNode>();
    private List<LineRenderer> connectionLines = new List<LineRenderer>();
    private MapNode selectedNode;
    
    public static CombatStarter Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        if (combatArea != null)
            combatArea.SetActive(false);
    }
    
    public void StartCombatSequence()
    {
        if (lobbyArea != null)
            lobbyArea.SetActive(false);
        
        if (combatArea != null)
            combatArea.SetActive(true);
        
        GenerateMap();
    }
    
    public void GenerateMap()
    {
        ClearMap();
        
        currentMapData = new MapData
        {
            width = mapWidth,
            height = mapHeight,
            nodes = new List<MapNode>()
        };
        
        allNodes.Clear();
        connectionLines.Clear();
        
        // Generate nodes column by column (left to right)
        for (int col = 0; col < mapWidth; col++)
        {
            int nodesInColumn = Random.Range(3, mapHeight + 1);
            List<int> availableRows = Enumerable.Range(0, mapHeight).ToList();
            
            List<MapNode> columnNodes = new List<MapNode>();
            
            for (int i = 0; i < nodesInColumn; i++)
            {
                if (availableRows.Count == 0) break;
                
                int rowIndex = Random.Range(0, availableRows.Count);
                int row = availableRows[rowIndex];
                availableRows.RemoveAt(rowIndex);
                
                Vector3 position = new Vector3(
                    col * nodeSpacing,
                    row * nodeSpacing + startYOffset,
                    0
                );
                
                MapNode node = CreateNode(col, row, position);
                columnNodes.Add(node);
                allNodes.Add(node);
                currentMapData.nodes.Add(node);
            }
            
            // Connect to previous column
            if (col > 0)
            {
                List<MapNode> previousColumn = allNodes.Where(n => n.column == col - 1).ToList();
                
                foreach (MapNode currentNode in columnNodes)
                {
                    List<MapNode> reachableNodes = previousColumn.Where(n => 
                        Mathf.Abs(n.row - currentNode.row) <= 1
                    ).ToList();
                    
                    if (reachableNodes.Count > 0)
                    {
                        MapNode targetNode = reachableNodes[Random.Range(0, reachableNodes.Count)];
                        CreateConnection(targetNode, currentNode);
                        currentNode.connectedFrom.Add(targetNode);
                        targetNode.connectedTo.Add(currentNode);
                    }
                }
            }
        }
        
        // Add start node
        MapNode startNode = allNodes.Where(n => n.column == 0).OrderBy(n => n.row).FirstOrDefault();
        if (startNode != null)
        {
            startNode.nodeType = NodeType.Start;
            startNode.isCompleted = true;
            SelectNode(startNode);
        }
        
        // Add boss node at the end
        MapNode bossNode = allNodes.Where(n => n.column == mapWidth - 1).OrderByDescending(n => n.row).FirstOrDefault();
        if (bossNode != null)
        {
            bossNode.nodeType = NodeType.Boss;
        }
        
        // Assign random types to other nodes
        foreach (var node in allNodes)
        {
            if (node.nodeType == NodeType.None && !node.isCompleted)
            {
                float roll = Random.value;
                if (col == mapWidth - 1)
                    node.nodeType = NodeType.Boss;
                else if (roll < 0.6f)
                    node.nodeType = NodeType.Battle;
                else if (roll < 0.8f)
                    node.nodeType = NodeType.Rest;
                else if (roll < 0.95f)
                    node.nodeType = NodeType.Event;
                else
                    node.nodeType = NodeType.Elite;
            }
        }
        
        UpdateNodeVisuals();
    }
    
    private MapNode CreateNode(int col, int row, Vector3 position)
    {
        GameObject nodePrefab = battleNodePrefab;
        
        // Choose appropriate prefab based on column (for visual variety)
        if (col == mapWidth - 1)
            nodePrefab = bossNodePrefab != null ? bossNodePrefab : battleNodePrefab;
        
        GameObject nodeObj = Instantiate(nodePrefab, position, Quaternion.identity);
        nodeObj.transform.SetParent(combatArea?.transform);
        
        MapNode node = nodeObj.GetComponent<MapNode>();
        if (node == null)
            node = nodeObj.AddComponent<MapNode>();
        
        // Add visual component
        MapNodeVisual visual = nodeObj.GetComponent<MapNodeVisual>();
        if (visual == null)
            visual = nodeObj.AddComponent<MapNodeVisual>();
        
        node.column = col;
        node.row = row;
        node.position = position;
        node.SetGameObject(nodeObj);
        
        return node;
    }
    
    private void CreateConnection(MapNode from, MapNode to)
    {
        if (connectionLinePrefab != null)
        {
            GameObject lineObj = Instantiate(connectionLinePrefab);
            lineObj.transform.SetParent(combatArea?.transform);
            
            LineRenderer line = lineObj.GetComponent<LineRenderer>();
            if (line != null)
            {
                line.SetPosition(0, from.position);
                line.SetPosition(1, to.position);
                connectionLines.Add(line);
            }
        }
    }
    
    public void SelectNode(MapNode node)
    {
        if (selectedNode != null)
            selectedNode.isSelected = false;
        
        selectedNode = node;
        
        if (node != null)
        {
            node.isSelected = true;
            
            if (node.nodeType != NodeType.Start && !node.isCompleted)
            {
                EnterCombat(node);
            }
        }
        
        UpdateNodeVisuals();
    }
    
    public void EnterCombat(MapNode node)
    {
        Debug.Log($"Entering combat at node ({node.column}, {node.row}) - Type: {node.nodeType}");
        
        // Here you would initialize your combat system
        // For now, we'll just mark the node as completed after combat
        
        node.isCompleted = true;
        
        // Auto-advance to next available node for demonstration
        Invoke(nameof(AutoAdvance), 2f);
    }
    
    private void AutoAdvance()
    {
        if (selectedNode == null) return;
        
        List<MapNode> nextNodes = selectedNode.connectedTo;
        
        if (nextNodes.Count > 0)
        {
            MapNode nextNode = nextNodes.FirstOrDefault(n => !n.isCompleted);
            if (nextNode != null)
            {
                SelectNode(nextNode);
            }
            else
            {
                Debug.Log("Map completed!");
                ReturnToLobby();
            }
        }
        else
        {
            Debug.Log("No more nodes available!");
            ReturnToLobby();
        }
    }
    
    public void ReturnToLobby()
    {
        if (combatArea != null)
            combatArea.SetActive(false);
        
        if (lobbyArea != null)
            lobbyArea.SetActive(true);
        
        ClearMap();
    }
    
    private void UpdateNodeVisuals()
    {
        foreach (var node in allNodes)
        {
            node.UpdateVisual();
        }
        
        foreach (var line in connectionLines)
        {
            if (line != null)
            {
                MapNode fromNode = allNodes.FirstOrDefault(n => n.position == line.GetPosition(0));
                MapNode toNode = allNodes.FirstOrDefault(n => n.position == line.GetPosition(1));
                
                if (fromNode != null && toNode != null)
                {
                    line.enabled = fromNode.isCompleted;
                }
            }
        }
    }
    
    private void ClearMap()
    {
        foreach (var node in allNodes)
        {
            if (node != null && node.gameObject != null)
                Destroy(node.gameObject);
        }
        
        foreach (var line in connectionLines)
        {
            if (line != null && line.gameObject != null)
                Destroy(line.gameObject);
        }
        
        allNodes.Clear();
        connectionLines.Clear();
    }
}

[System.Serializable]
public class MapData
{
    public int width;
    public int height;
    public List<MapNode> nodes;
}

[System.Serializable]
public class MapNode
{
    public int column;
    public int row;
    public Vector3 position;
    public NodeType nodeType = NodeType.None;
    public bool isCompleted;
    public bool isSelected;
    
    public List<MapNode> connectedFrom = new List<MapNode>();
    public List<MapNode> connectedTo = new List<MapNode>();
    
    [HideInInspector]
    public GameObject gameObject;
    
    public void UpdateVisual()
    {
        if (gameObject == null) return;
        
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            switch (nodeType)
            {
                case NodeType.Start:
                    renderer.color = Color.green;
                    break;
                case NodeType.Battle:
                    renderer.color = isCompleted ? Color.gray : Color.red;
                    break;
                case NodeType.Elite:
                    renderer.color = isCompleted ? Color.gray : new Color(1f, 0.5f, 0f);
                    break;
                case NodeType.Rest:
                    renderer.color = isCompleted ? Color.gray : Color.blue;
                    break;
                case NodeType.Event:
                    renderer.color = isCompleted ? Color.gray : Color.yellow;
                    break;
                case NodeType.Boss:
                    renderer.color = isCompleted ? Color.gray : Color.magenta;
                    break;
                default:
                    renderer.color = Color.white;
                    break;
            }
            
            if (isSelected)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
                renderer.transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                renderer.transform.localScale = Vector3.one;
            }
        }
    }
    
    public void SetGameObject(GameObject go)
    {
        gameObject = go;
        
        MapNodeVisual visual = go.GetComponent<MapNodeVisual>();
        if (visual != null)
        {
            visual.nodeType = nodeType;
        }
    }
}

public enum NodeType
{
    None,
    Start,
    Battle,
    Elite,
    Rest,
    Event,
    Boss
}
