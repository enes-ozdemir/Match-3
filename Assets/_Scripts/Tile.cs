using _Scripts.Managers;
using UnityEngine;

namespace _Scripts
{
    public class Tile : MonoBehaviour
    {
        public int x;
        public int y;

        private GridManager _gridManager;
        private TileInputManager _tileInputManager;

        public void SetupTile(int row, int col, GridManager gridManager, TileInputManager tileInputManager)
        {
            x = row;
            y = col;
            _gridManager = gridManager;
            _tileInputManager = tileInputManager;
        }

        public Vector2Int GetTilePos()
        {
            return new Vector2Int(x, y);
        }

        private void OnMouseDown()
        {
            if (_tileInputManager == null) return;

            _tileInputManager.SelectTile(this);
        }

        private void OnMouseEnter()
        {
            if (_tileInputManager == null) return;

            _tileInputManager.DragTileToTarget(this);
        }

        private void OnMouseUp()
        {
            if (_tileInputManager == null) return;

            _tileInputManager.ReleaseTile();
        }
    }
}