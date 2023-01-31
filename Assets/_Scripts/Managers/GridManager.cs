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
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private ScoreManager scoreManager;

        private Tile[,] _tileArray;
        public static GemTile[,] gemArray;

        private void Awake()
        {
            matchManager.SetGridSize(gridRowCount, gridColumnCount);
            _tileArray = new Tile[gridRowCount, gridColumnCount];
            gemArray = new GemTile[gridRowCount, gridColumnCount];
        }

        private void Start()
        {
            SetupTiles();
            FillGrid(true);
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
                //Todo add camera shake maybe
            }
        }

        public void RemoveGems(List<GemTile> gemList)
        {
            foreach (var gem in gemList.Where(gem => gem != null))
            {
                RemoveGem(gem.x, gem.y);
            }

            Camera.main.transform.DOShakePosition(0.1f, 0.1f, 1);
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
            gemArray[x, y] = gemTile;
            gemTile.InitializeGem(randomGem, x, y, this);
        }

        public void FillGrid(bool isNewGem = false)
        {
            for (int row = 0; row < gridRowCount; row++)
            {
                for (int col = 0; col < gridColumnCount; col++)
                {
                    if (gemArray[row, col] != null) continue;

                    FillTileRandomly(row, col, isNewGem);

                    while (IsGemNotValid(row, col))
                    {
                        RemoveGem(row, col, 0f, false);
                        FillTileRandomly(row, col, isNewGem);
                    }
                }
            }

            tileInputManager.onInputEnabled.Invoke();
        }

        public void ClearGrid()
        {
            for (int row = 0; row < gridRowCount; row++)
            {
                for (int col = 0; col < gridColumnCount; col++)
                {
                    RemoveGem(row, col, 1f, false);
                }
            }
        }

        private bool IsGemNotValid(int row, int col, int minLenght = 3)
        {
            var leftMatch = matchManager.FindMatch(row, col, Vector2.left, minLenght);
            var downMatch = matchManager.FindMatch(row, col, Vector2.down, minLenght);

            if (leftMatch == null) leftMatch = new List<GemTile>();
            if (downMatch == null) downMatch = new List<GemTile>();

            return leftMatch.Count > 0 || downMatch.Count > 0;
        }

        private void FillTileRandomly(int row, int col, bool isNewGem = false)
        {
            var gemObject =
                ObjectPooler.Instance.SpawnFromPool("Gem", transform.position, quaternion.identity, transform);
            var randomGem = gemSystem.GetRandomGem();
            var gemTile = gemObject.GetComponent<GemTile>();
            InitGemAtPosition(randomGem, gemTile, row, col);

            if (isNewGem)
            {
                gemObject.transform.position = new Vector3(row, col + 7, 0);
                gemTile.MoveTo(row, col, 0.2f);
            }
        }

        public IEnumerator RefillRoutine()
        {
            FillGrid(true);
            yield return null;
        }
    }
}