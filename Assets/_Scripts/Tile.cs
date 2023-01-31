using _Scripts.Managers;
using UnityEngine;

namespace _Scripts
{
    public class Tile : MonoBehaviour
    {
        public int x;
        public int y;

        private TileInputManager _tileInputManager;

        public void SetupTile(int row, int col,TileInputManager tileInputManager)
        {
            x = row;
            y = col;
            _tileInputManager = tileInputManager;
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