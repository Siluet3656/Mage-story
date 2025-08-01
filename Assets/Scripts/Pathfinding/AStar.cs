using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    public class AStar : MonoBehaviour
    {
        #region Singleton

        public static AStar Instance;
        AStar()
        {
            if (Instance != null) return;
            Instance = this;
        }

        #endregion

        private readonly List<Node> _nodesOnScene = new List<Node>();
        
        private void Awake()
        {
            RefreshNodesOnScene();
        }

        private void InitializeNodes(Node start, Node end)
        {
            foreach (Node node in _nodesOnScene)
            {
                node._gScore = float.MaxValue;
            }
            start._gScore = 0;
            start._hScore = Vector2.Distance(start.transform.position, end.transform.position);
        }

        private Node PopLowestFScoreNode(List<Node> openSet)
        {
            int lowestIndex = 0;
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestIndex].FScore())
                    lowestIndex = i;
            }
            
            Node lowestNode = openSet[lowestIndex];
            openSet.RemoveAt(lowestIndex);
            return lowestNode;
        }

        private List<Node> ReconstructPath(Node end, Node start)
        {
            List<Node> path = new List<Node>();
            Node current = end;
            
            while (current != start)
            {
                path.Add(current);
                current = current._cameFrom;
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        private void ProcessNeighbors(Node currentNode, Node end, List<Node> openSet)
        {
            foreach (Node neighbor in currentNode.Connections)
            {
                float tentativeGScore = currentNode._gScore + 
                    Vector2.Distance(currentNode.transform.position, neighbor.transform.position);
                
                if (tentativeGScore >= neighbor._gScore)
                    continue;

                UpdateNeighbor(neighbor, currentNode, end, tentativeGScore);
                AddToOpenSet(neighbor, openSet);
            }
        }

        private void UpdateNeighbor(Node neighbor, Node currentNode, Node end, float gScore)
        {
            neighbor._cameFrom = currentNode;
            neighbor._gScore = gScore;
            neighbor._hScore = Vector2.Distance(neighbor.transform.position, end.transform.position);
        }

        private void AddToOpenSet(Node node, List<Node> openSet)
        {
            if (!openSet.Contains(node))
                openSet.Add(node);
        }
        
        public readonly float MinNodeDistance = 0.25f;
        
        public List<Node> NodesOnScene => _nodesOnScene;
        
        public void RefreshNodesOnScene()
        {
            _nodesOnScene.Clear();
            _nodesOnScene.AddRange(FindObjectsOfType<Node>());
        }

        public List<Node> GeneratePath(Node start, Node end)
        {
            List<Node> openSet = new List<Node>();
            InitializeNodes(start, end);
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                Node currentNode = PopLowestFScoreNode(openSet);
                if (currentNode == end)
                    return ReconstructPath(end, start);

                ProcessNeighbors(currentNode, end, openSet);
            }
            
            return null;
        }

        public Node FindNearestNode(Vector2 position)
        {
            Node foundNode = _nodesOnScene
                .OrderBy(node => Vector2.Distance(position, node.gameObject.transform.position))
                .FirstOrDefault();
            
            return foundNode;
        }

        public Node FindFurthestNode(Vector2 position)
        {
            Node foundNode = _nodesOnScene
                .OrderBy(node => Vector2.Distance(position, node.gameObject.transform.position))
                .LastOrDefault();
            
            return foundNode;
        }
    }
}