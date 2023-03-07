using System;
using System.Threading;
using _Scripts.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class TileInputManager : MonoBehaviour
    {
        private Tile _selectedTile;
        private Tile _targetTile;

        public Func<Tile, Tile,UniTask> onTileSwapped;
        public Action onInputDisabled;
        public Action onInputEnabled;

        [HideInInspector] public bool canInput = true;

        private void OnEnable()
        {
            onInputDisabled += DisableInput;
            onInputEnabled += EnableInput;
        }

        private void OnDisable()
        {
            onInputDisabled -= DisableInput;
            onInputEnabled -= EnableInput;
        }

        private void DisableInput() => canInput = false;
        private void EnableInput() => canInput = true;

        public void SelectTile(Tile tile)
        {
            if (!canInput) return;
            if (_selectedTile != null) return;

            _selectedTile = tile;
        }

        public void DragTileToTarget(Tile tile)
        {
            if (!canInput) return;
            if (_selectedTile == null || !CanSwap(tile, _selectedTile)) return;

            _targetTile = tile;
        }

        public void ReleaseTile()
        {
            if (!canInput) return;
            if (_selectedTile == null || _targetTile == null) return;

            SwapTiles(_selectedTile, _targetTile);
        }

        private void SwapTiles(Tile selectedTile, Tile targetTile)
        {
            onTileSwapped.Invoke(selectedTile, targetTile);
            DisableInput();
            _selectedTile = null;
            _targetTile = null;
        }

        private bool CanSwap(Tile selectedTile, Tile targetTile)
        {
            return (Mathf.Abs(selectedTile.x - targetTile.x) == 1 && selectedTile.y == targetTile.y) ||
                   (Mathf.Abs(selectedTile.y - targetTile.y) == 1 && selectedTile.x == targetTile.x);
        }
    }
}