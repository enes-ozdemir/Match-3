using System;
using System.Collections;
using System.Linq;
using _Scripts.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class SwapManager : MonoBehaviour
    {
        private MatchManager _matchManager;
        [SerializeField] private CollapseManager collapseManager;
        [SerializeField] private TileInputManager tileInputManager;

        private void Awake()
        {
            tileInputManager.onTileSwapped += SwapTiles;
        }

        private void Start()
        {
            _matchManager = new MatchManager(LevelManager.Instance.GetGridSize());
        }

        private async UniTask SwapTiles(Tile firstTile, Tile secondTile)
        {
            var clickedGemTile = GridManager.gemArray[firstTile.x, firstTile.y];
            var targetGemTile = GridManager.gemArray[secondTile.x, secondTile.y];

            MoveTilesToPosition(clickedGemTile, targetGemTile);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            var firstTileMatches = _matchManager.FindMatchesInList(firstTile.x, firstTile.y);
            var secondTileMatches = _matchManager.FindMatchesInList(secondTile.x, secondTile.y);

            if (firstTileMatches.Count == 0 && secondTileMatches.Count == 0)
            {
                MoveTilesToPosition(clickedGemTile, targetGemTile);
                tileInputManager.onInputEnabled.Invoke();
            }
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

              collapseManager.ClearAndRefillBoard(firstTileMatches.Union(secondTileMatches).ToList());
            }
        }

        private void MoveTilesToPosition(GemTile clickedGemTile, GemTile targetGemTile)
        {
            clickedGemTile.MoveTo(targetGemTile.x, targetGemTile.y);
            targetGemTile.MoveTo(clickedGemTile.x, clickedGemTile.y);
        }
    }
}