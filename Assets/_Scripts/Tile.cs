using _Scripts.Managers;
using UnityEngine;

namespace _Scripts
{
    public class Tile : MonoBehaviour
    {
        private int _row;
        private int _col;
        private Camera _camera;
        private Vector3 _firstTouchPos;
        private Vector3 _endTouchPos;

        private GridManager _gridManager;

        public void SetupTile(int row, int col, GridManager gridManager)
        {
            _row = row;
            _col = col;
            _gridManager = gridManager;
        }
    }
}