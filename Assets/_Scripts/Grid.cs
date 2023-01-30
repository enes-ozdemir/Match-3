using System;
using UnityEngine;

namespace _Scripts
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private int gridRowCount = 8;
        [SerializeField] private int gridColumnCount = 8;
        [SerializeField] private GameObject tilePrefab;

        private float _cellSize;

        private Tile[,] _tileArray;

        private void Start()
        {
            _tileArray = new Tile[gridRowCount, gridColumnCount];
            SetupTiles();
        }

        private void SetupTiles()
        {
            for (int x = 0; x < gridRowCount; x++)
            {
                for (int y = 0; y < gridColumnCount; y++)
                {
                    Debug.Log($"{x},{y}");

                    var tileObject = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    tileObject.name = "Tile " + x + "-" + y;
                    var tile = tileObject.GetComponent<Tile>();
                    _tileArray[x, y] = tile;
                }
            }
        }
    }
}