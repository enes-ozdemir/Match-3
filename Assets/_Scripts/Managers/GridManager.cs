using _Scripts.SO;
using _Scripts.Systems;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int gridRowCount = 8;
        [SerializeField] private int gridColumnCount = 8;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GemController gemSystem;

        private float _cellSize;

        private Tile[,] _tileArray;
        private GemTile[,] _gemArray;

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
                    var tileObject = Instantiate(tilePrefab, new Vector3(row, col, 0), Quaternion.identity, transform);
                    tileObject.name = "Tile " + row + "-" + col;

                    var tile = tileObject.GetComponent<Tile>();
                    _tileArray[row, col] = tile;
                    _tileArray[row, col].SetupTile(row, col, this);
                }
            }
        }

        private void InitGemAtPosition(Gem randomGem, GemTile gemTile, int x, int y)
        {
            if (gemTile == null) return;
            gemTile.InitializeGem(randomGem, x, y);
        }

        private void FillGrid()
        {
            for (int row = 0; row < gridRowCount; row++)
            {
                for (int col = 0; col < gridColumnCount; col++)
                {
                    var gemObject =
                        ObjectPooler.Instance.SpawnGemFromPool("Gem", transform.position, quaternion.identity);
                    var randomGem = gemSystem.GetRandomGem();
                    var gemTile = gemObject.GetComponent<GemTile>();

                    InitGemAtPosition(randomGem,gemTile, row, col);
                }
            }
        }
    }
}