using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.AStar
{
    using UnityEngine;
    public class AStar
    {
        private List<Vector2> walkableTiles;

        public AStar(List<Vector2> walkableTiles)
        {
            this.walkableTiles = walkableTiles;
        }

        public List<Vector2> FindPath(Vector2 start, Vector2 end)
        {
            var openList = new List<Node>();
            var closedList = new HashSet<Node>();
            var startNode = new Node(start);
            var endNode = new Node(end);

            startNode.Cost = 0;
            startNode.Heuristic = GetDistance(startNode, endNode);
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                var currentNode = openList.OrderBy(n => n.TotalCost).First();

                if (currentNode.Position == endNode.Position)
                {
                    return GetPath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (closedList.Any(n => n.Position == neighbor.Position))
                        continue;

                    var tentativeCost = currentNode.Cost + GetDistance(currentNode, neighbor);

                    if (tentativeCost < neighbor.Cost || !openList.Contains(neighbor))
                    {
                        neighbor.Cost = tentativeCost;
                        neighbor.Heuristic = GetDistance(neighbor, endNode);
                        neighbor.Parent = currentNode;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            return new List<Vector2>(); // Yol bulunamazsa boş liste dön

        }

        private List<Node> GetNeighbors(Node node)
        {
            var neighbors = new List<Node>();

            var directions = new Vector2[]
            {
                new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1),
                new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1)
            };

            foreach (var direction in directions)
            {
                var neighborPos = node.Position + direction;
                if (walkableTiles.Contains(neighborPos))
                {
                    neighbors.Add(new Node(neighborPos));
                }
            }
            return neighbors;
        }

        private float GetDistance(Node a, Node b)
        {
            return Vector2.Distance(a.Position, b.Position);
        }

        private List<Vector2> GetPath(Node endNode)
        {
            List<Vector2> path = new List<Vector2>();
            Node current = endNode;
            while (current != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
    }

}
