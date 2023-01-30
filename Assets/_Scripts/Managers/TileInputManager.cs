﻿using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class TileInputManager : MonoBehaviour
    {
        private Tile _selectedTile;
        private Tile _targetTile;

        public Action<Tile, Tile> onTileSwapped;

        public void SelectTile(Tile tile)
        {
            if (_selectedTile != null) return;

            print($"Clicked to {tile.GetTilePos()}");
            _selectedTile = tile;
        }

        public void DragTileToTarget(Tile tile)
        {
            if (_selectedTile == null || !CanSwap(tile, _selectedTile)) return;

            _targetTile = tile;
        }

        public void ReleaseTile()
        {
            if (_selectedTile == null || _targetTile == null) return;

            SwapTiles(_selectedTile, _targetTile);
        }

        private void SwapTiles(Tile selectedTile, Tile targetTile)
        {
            onTileSwapped.Invoke(selectedTile, targetTile);

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