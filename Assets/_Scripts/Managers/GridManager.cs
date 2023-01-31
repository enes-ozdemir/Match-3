using System;
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
        [SerializeField] private GemProvider gemProvider;
        [SerializeField] private TileInputManager tileInputManager;
        [SerializeField] private ScoreManager scoreManager;

        private MatchManager _matchManager;
        private Tile[,] _tileArray;
        private int _gridRowCount;
        private int _gridColumnCount;

        public static GemTile[,] gemArray;

        public void SetupGrid()
        {
            Debug.Log("Grid is being set up");
            SetGridSize();
            _matchManager = new MatchManager(_gridRowCount, _gridColumnCount);

            ClearGrid();
            SetupArrays(_gridRowCount, _gridColumnCount);
            SetupTiles();
            FillGrid(true);
        }

        private void SetupArrays(int x, int y)
        {
            _tileArray = new Tile[x, y];
            gemArray = new GemTile[x, y];
        }

        public void RemoveGems(List<GemTile> gemList)
        {
            foreach (var gem in gemList.Where(gem => gem != null))
            {
                RemoveGem(gem.x, gem.y);
            }

            Camera.main.transform.DOShakePosition(0.1f, 0.1f, 1);
        }

        private void RemoveGem(int x, int y, float duration = 0.3f, bool isPlayerMove = true)
        {
            var gemToRemove = gemArray[x, y];

            if (gemToRemove != null)
            {
                gemArray[x, y] = null;
                gemToRemove.transform.DOScale(1.2f, duration).OnComplete((() =>
                {
                    gemToRemove.transform.DOScale(0, duration).OnComplete((() =>
                    {
                        gemToRemove.gameObject.SetActive(false);
                    }));
                }));
                if (isPlayerMove) scoreManager.AddScore(1);
            }
        }

        private void SetupTiles()
        {
            for (int row = 0; row < _gridRowCount; row++)
            {
                for (int col = 0; col < _gridColumnCount; col++)
                {
                    var tileObject = ObjectPooler.Instance.SpawnFromPool("Tile", new Vector2(row, col),
                        Quaternion.identity, transform);
                    tileObject.name = "Tile " + row + "-" + col;

                    var tile = tileObject.GetComponent<Tile>();
                    _tileArray[row, col] = tile;
                    _tileArray[row, col].SetupTile(row, col, tileInputManager);
                }
            }
        }

        public void FillGrid(bool isNewGem = false)
        {
            for (int row = 0; row < _gridRowCount; row++)
            {
                for (int col = 0; col < _gridColumnCount; col++)
                {
                    if (gemArray[row, col] != null) continue;

                    FillTileWithGem(row, col, isNewGem);

                    while (IsGemNotValid(row, col))
                    {
                        RemoveGem(row, col, 0f, false);
                        FillTileWithGem(row, col, isNewGem);
                    }
                }
            }

            Debug.Log("Grid filled with Gems");
            tileInputManager.onInputEnabled.Invoke();
        }

        private void ClearGrid()
        {
            if (gemArray == null) return;

            for (int row = 0; row < _gridRowCount; row++)
            {
                for (int col = 0; col < _gridColumnCount; col++)
                {
                    RemoveGem(row, col, 1f, false);
                }
            }

            Debug.Log("Grid Cleared");
        }

        private bool IsGemNotValid(int row, int col, int minLenght = 3)
        {
            var leftMatch = _matchManager.FindMatch(row, col, Vector2.left, minLenght);
            var downMatch = _matchManager.FindMatch(row, col, Vector2.down, minLenght);

            if (leftMatch == null) leftMatch = new List<GemTile>();
            if (downMatch == null) downMatch = new List<GemTile>();

            return leftMatch.Count > 0 || downMatch.Count > 0;
        }

        private void FillTileWithGem(int row, int col, bool isNewGem = false)
        {
            var gemObject =
                ObjectPooler.Instance.SpawnFromPool("Gem", transform.position, quaternion.identity, transform);
            var randomGem = gemProvider.GetRandomGem();
            var gemTile = gemObject.GetComponent<GemTile>();
            InitGemAtPosition(randomGem, gemTile, row, col);

            if (isNewGem)
            {
                gemObject.transform.position = new Vector3(row, col + 7, 0);
                gemTile.MoveTo(row, col, 0.2f);
            }
        }

        public void InitGemAtPosition(Gem randomGem, GemTile gemTile, int x, int y)
        {
            if (gemTile == null) return;
            gemArray[x, y] = gemTile;
            gemTile.InitializeGem(randomGem, x, y, this);
        }

        private void SetGridSize()
        {
            var gridSize = LevelManager.Instance.GetGridSize();
            _gridRowCount = gridSize.x;
            _gridColumnCount = gridSize.y;
        }
    }
}