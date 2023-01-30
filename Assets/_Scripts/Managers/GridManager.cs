using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.SO;
using _Scripts.Systems;
using _Scripts.Util;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int gridRowCount = 8;
        [SerializeField] private int gridColumnCount = 8;
        [SerializeField] private GemController gemSystem;
        [SerializeField] private TileInputManager tileInputManager;

        private Tile[,] _tileArray;
        private GemTile[,] _gemArray;

        private void Awake()
        {
            tileInputManager.onTileSwapped += SwapTilesCo;
        }

        private void Start()
        {
            _tileArray = new Tile[gridRowCount, gridColumnCount];
            _gemArray = new GemTile[gridRowCount, gridColumnCount];

            SetupTiles();
            FillGrid();
        }

        private void SwapTilesCo(Tile firstTile, Tile secondTile) =>
            StartCoroutine(SwapTilesOnArray(firstTile, secondTile));

        private IEnumerator SwapTilesOnArray(Tile firstTile, Tile secondTile)
        {
            var clickedGemTile = _gemArray[firstTile.x, firstTile.y];
            var targetGemTile = _gemArray[secondTile.x, secondTile.y];

            clickedGemTile.MoveTo(targetGemTile.x, targetGemTile.y);
            targetGemTile.MoveTo(clickedGemTile.x, clickedGemTile.y);

            yield return new WaitForSeconds(0.5f);

            var firstTileMatches = FindAllMatches(firstTile.x, firstTile.y);
            var secondTileMatches = FindAllMatches(secondTile.x, secondTile.y);

            if (firstTileMatches.Count == 0 && secondTileMatches.Count == 0)
            {
                clickedGemTile.MoveTo(targetGemTile.x, targetGemTile.y);
                targetGemTile.MoveTo(clickedGemTile.x, clickedGemTile.y);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                RemoveGems(firstTileMatches);
                RemoveGems(secondTileMatches);
            }
        }

        private void RemoveGem(int x, int y)
        {
            var gemToRemove = _gemArray[x, y];

            if (gemToRemove != null)
            {
                _gemArray[x, y] = null;
                gemToRemove.transform.DOScale(0, 0.5f).OnComplete((() => { gemToRemove.gameObject.SetActive(false); }));
            }
        }

        private void RemoveGems(List<GemTile> gemList)
        {
            foreach (var gem in gemList)
            {
                var gemToRemove = _gemArray[gem.x, gem.y];

                if (gemToRemove != null)
                {
                    _gemArray[gem.x, gem.y] = null;
                    gemToRemove.transform.DOScale(0, 0.5f).OnComplete((() =>
                    {
                        gemToRemove.gameObject.SetActive(false);
                    }));
                }
            }
        }

        public void ClearBoard()
        {
            for (int row = 0; row < gridRowCount; row++)
            {
                for (int column = 0; column < gridColumnCount; column++)
                {
                    RemoveGem(row, column);
                }
            }
        }

        private void SetupTiles()
        {
            for (int row = 0; row < gridRowCount; row++)
            {
                for (int col = 0; col < gridColumnCount; col++)
                {
                    var tileObject = ObjectPooler.Instance.SpawnFromPool("Tile", new Vector3(row, col, 0),
                        Quaternion.identity, transform);
                    tileObject.name = "Tile " + row + "-" + col;

                    var tile = tileObject.GetComponent<Tile>();
                    _tileArray[row, col] = tile;
                    _tileArray[row, col].SetupTile(row, col, this, tileInputManager);
                }
            }
        }

        public void InitGemAtPosition(Gem randomGem, GemTile gemTile, int x, int y)
        {
            if (gemTile == null) return;
            _gemArray[x, y] = gemTile;
            gemTile.InitializeGem(randomGem, x, y, this);
        }

        private void FillGrid()
        {
            for (int row = 0; row < gridRowCount; row++)
            {
                for (int col = 0; col < gridColumnCount; col++)
                {
                    FillPositionRandomly(row, col);

                    while (IsGemNotValid(row, col))
                    {
                        RemoveGem(row, col);
                        FillPositionRandomly(row, col);
                    }
                }
            }
        }

        private bool IsGemNotValid(int row, int col, int minLenght = 3)
        {
            var leftMatch = FindMatch(row, col, Vector2.left, minLenght);
            var downMatch = FindMatch(row, col, Vector2.down, minLenght);

            if (leftMatch == null) leftMatch = new List<GemTile>();
            if (downMatch == null) downMatch = new List<GemTile>();

            return leftMatch.Count > 0 || downMatch.Count > 0;
        }

        private void FillPositionRandomly(int row, int col)
        {
            var gemObject =
                ObjectPooler.Instance.SpawnFromPool("Gem", transform.position, quaternion.identity, transform);
            var randomGem = gemSystem.GetRandomGem();
            var gemTile = gemObject.GetComponent<GemTile>();

            InitGemAtPosition(randomGem, gemTile, row, col);
        }

        private List<GemTile> FindMatch(int x, int y, Vector2 dir, int minMatchLenght = 3)
        {
            var matches = new List<GemTile>();
            var startTile = _gemArray[x, y];

            if (startTile == null) return null;

            matches.Add(startTile);

            int nextRow;
            int nextCol;

            int maxDistance = (gridRowCount > gridColumnCount) ? gridRowCount : gridColumnCount;

            for (int i = 1; i < maxDistance - 1; i++)
            {
                nextRow = x + (int) Mathf.Clamp(dir.x, -1, 1) * i;
                nextCol = y + (int) Mathf.Clamp(dir.y, -1, 1) * i;

                if (!IsPositionValid(nextRow, nextCol)) break;

                var nextTile = _gemArray[nextRow, nextCol];
                if (nextTile == null) break;

                if (nextTile.GetGemType() == startTile.GetGemType() && !matches.Contains(nextTile))
                {
                    matches.Add(nextTile);
                }
                else break;
            }

            if (matches.Count >= minMatchLenght)
            {
                print("Its a match");
                return matches;
            }

            return null;
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

        private List<GemTile> FindAllMatches(int x, int y, int minLenght = 3)
        {
            var horizontalMatch = FindHorizontalMatch(x, y, minLenght);
            var verticalMatch = FindVerticalMatch(x, y, minLenght);

            if (horizontalMatch == null) horizontalMatch = new List<GemTile>();
            if (verticalMatch == null) verticalMatch = new List<GemTile>();

            var combinedMatches = horizontalMatch.Union(verticalMatch).ToList();

            return combinedMatches;
        }

        private bool IsPositionValid(int nextRow, int nextCol)
        {
            return !(nextRow < 0 || nextRow >= gridRowCount
                                 || nextCol < 0 || nextCol >= gridColumnCount);
        }
    }
}