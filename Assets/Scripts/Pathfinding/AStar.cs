using System.Collections.Generic;
using UnityEngine;
using Data;

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

        [SerializeField, Min(0.1f)] private float _minNodeDistance = 0.15f;
        
        private void InitializeNodes(Node start, Node end)
        {
            foreach (Node node in G.NodesOnScene)
            {
                node._gScore = float.MaxValue;
            }
            start._gScore = 0;
            start._hScore = Vector2.Distance(start.transform.position, end.transform.position);
        }

        private Node PopLowestFScoreNode(PriorityQueue<Node> openSet)
        {
            return openSet.Dequeue();
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

        private void ProcessNeighbors(Node currentNode, Node end, PriorityQueue<Node> openSet)
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

        private void AddToOpenSet(Node node, PriorityQueue<Node> openSet)
        {
            if (!openSet.Contains(node))
                openSet.Enqueue(node);
        }
        
        public float MinNodeDistance => _minNodeDistance;

        public List<Node> GeneratePath(Node start, Node end)
        {
            var openSet = new PriorityQueue<Node>((a, b) => a.FScore().CompareTo(b.FScore()));
            
            InitializeNodes(start, end);

            openSet.Enqueue(start);

            while (openSet.Count > 0)
            {
                Node currentNode = PopLowestFScoreNode(openSet);
                if (currentNode == end)
                {
                    return ReconstructPath(end, start);
                }
                ProcessNeighbors(currentNode, end, openSet);
            }
            return null;
        }

        public Node FindNearestNode(Vector2 position)
        {
            Node nearestNode = null;
            float minDistanceSquared = float.MaxValue;

            foreach (Node node in G.NodesOnScene)
            {
                Vector2 nodePosition = node.gameObject.transform.position;
                float distanceSquared = (nodePosition - position).sqrMagnitude;
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    nearestNode = node;
                }
            }
            
            return nearestNode;
        }

        public Node FindFurthestNode(Vector2 position)
        {
            Node furthestNode = null;
            float maxDistanceSquared = float.MinValue;

            foreach (Node node in G.NodesOnScene)
            {
                Vector2 nodePosition = node.gameObject.transform.position;
                float distanceSquared = (nodePosition - position).sqrMagnitude;
                if (distanceSquared > maxDistanceSquared)
                {
                    maxDistanceSquared = distanceSquared;
                    furthestNode = node;
                }
            }

            return furthestNode;
        }
    }
}