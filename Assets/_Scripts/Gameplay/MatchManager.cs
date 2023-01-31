using System.Collections.Generic;
using System.Linq;
using _Scripts.Components;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class MatchManager
    {
        private int _gridRowCount;
        private int _gridColumnCount;

        public MatchManager(Vector2Int grid)
        {
            _gridRowCount = grid.x;
            _gridColumnCount = grid.y;
        }
        
        public MatchManager(int x,int y)
        {
            _gridRowCount = x;
            _gridColumnCount = y;
        }

        private bool IsPositionValid(int nextRow, int nextCol)
        {
            return !(nextRow < 0 || nextRow >= _gridRowCount
                                 || nextCol < 0 || nextCol >= _gridColumnCount);
        }

        public List<GemTile> FindMatch(int x, int y, Vector2 dir, int minMatchLenght = 3)
        {
            var matches = new List<GemTile>();
            var startTile = GridManager.gemArray[x, y];

            if (startTile == null) return null;

            matches.Add(startTile);

            int maxDistance = (_gridRowCount > _gridColumnCount) ? _gridRowCount : _gridColumnCount;

            for (int i = 1; i < maxDistance - 1; i++)
            {
                var nextRow = x + (int) Mathf.Clamp(dir.x, -1, 1) * i;
                var nextCol = y + (int) Mathf.Clamp(dir.y, -1, 1) * i;

                if (!IsPositionValid(nextRow, nextCol)) break;

                var nextTile = GridManager.gemArray[nextRow, nextCol];
                if (nextTile == null) break;

                if (nextTile.GetGemType() == startTile.GetGemType() && !matches.Contains(nextTile))
                {
                    matches.Add(nextTile);
                }
                else break;
            }

            return matches.Count >= minMatchLenght ? matches : null;
        }

        public List<GemTile> FindMatchesInList(List<GemTile> gemTiles, int minMatchLenght = 3)
        {
            var matches = new List<GemTile>();

            foreach (var tile in gemTiles)
            {
                matches = matches.Union(FindMatchesInList(tile.x, tile.y, minMatchLenght)).ToList();
            }

            return matches;
        }

        private List<GemTile> FindVerticalMatch(int x, int y, int minLength = 3)
        {
            var upMatches = FindMatch(x, y, Vector2.up, 2);
            var downMatches = FindMatch(x, y, Vector2.down, 2);

            if (upMatches == null) upMatches = new List<GemTile>();
            if (downMatches == null) downMatches = new List<GemTile>();

            var combinedMatches = upMatches.Union(downMatches).ToList();

            return combinedMatches.Count >= minLength ? combinedMatches : null;
        }

        private List<GemTile> FindHorizontalMatch(int x, int y, int minLength = 3)
        {
            var leftMatches = FindMatch(x, y, Vector2.left, 2);
            var rightMatches = FindMatch(x, y, Vector2.right, 2);

            if (leftMatches == null) leftMatches = new List<GemTile>();
            if (rightMatches == null) rightMatches = new List<GemTile>();

            var combinedMatches = leftMatches.Union(rightMatches).ToList();

            return combinedMatches.Count >= minLength ? combinedMatches : null;
        }

        public List<GemTile> FindMatchesInList(int x, int y, int minLenght = 3)
        {
            var horizontalMatch = FindHorizontalMatch(x, y, minLenght);
            var verticalMatch = FindVerticalMatch(x, y, minLenght);

            if (horizontalMatch == null) horizontalMatch = new List<GemTile>();
            if (verticalMatch == null) verticalMatch = new List<GemTile>();

            var combinedMatches = horizontalMatch.Union(verticalMatch).ToList();

            return combinedMatches;
        }

        public List<GemTile> FindAllMatches()
        {
            var allMatches = new List<GemTile>();

            for (int row = 0; row < _gridRowCount; row++)
            {
                for (int col = 0; col < _gridColumnCount; col++)
                {
                    var matches = FindMatchesInList(row, col);
                    allMatches = allMatches.Union(matches).ToList();
                }
            }

            return allMatches;
        }
    }
}