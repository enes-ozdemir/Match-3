using _Scripts.SO;
using _Scripts.Systems;
using _Scripts.Util;
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
            tileInputManager.onTileSwapped += SwapTilesOnArray;
        }

        private void SwapTilesOnArray(Tile firstTile, Tile secondTile)
        {
            var clickedGemTile = _gemArray[firstTile.x, firstTile.y];
            var targetGemTile = _gemArray[secondTile.x, secondTile.y];

            clickedGemTile.MoveTo(targetGemTile.x, targetGemTile.y);
            targetGemTile.MoveTo(clickedGemTile.x, clickedGemTile.y);
        }

        private void Start()
        {
            _tileArray = new Tile[gridRowCount, gridColumnCount];
            _gemArray = new GemTile[gridRowCount, gridColumnCount];

            SetupTiles();
            FillGrid();
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
                    var gemObject =
                        ObjectPooler.Instance.SpawnFromPool("Gem", transform.position, quaternion.identity, transform);
                    var randomGem = gemSystem.GetRandomGem();
                    var gemTile = gemObject.GetComponent<GemTile>();

                    InitGemAtPosition(randomGem, gemTile, row, col);
                }
            }
        }
    }
}