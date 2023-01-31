using System.Collections;
using UnityEngine;
using System.Linq;

namespace _Scripts.Managers
{
    public class SwapManager : MonoBehaviour
    {
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private CollapseManager collapseManager;
        [SerializeField] private TileInputManager tileInputManager;

        private void Awake()
        {
            tileInputManager.onTileSwapped += SwapTilesCo;
        }

        private void SwapTilesCo(Tile firstTile, Tile secondTile) =>
            StartCoroutine(SwapTilesOnArray(firstTile, secondTile));

        private IEnumerator SwapTilesOnArray(Tile firstTile, Tile secondTile)
        {
            var clickedGemTile = GridManager.gemArray[firstTile.x, firstTile.y];
            var targetGemTile = GridManager.gemArray[secondTile.x, secondTile.y];

            MoveTilesToPosition(clickedGemTile, targetGemTile);

            yield return new WaitForSeconds(0.5f);

            var firstTileMatches = matchManager.FindMatchesInList(firstTile.x, firstTile.y);
            var secondTileMatches = matchManager.FindMatchesInList(secondTile.x, secondTile.y);

            if (firstTileMatches.Count == 0 && secondTileMatches.Count == 0)
            {
                MoveTilesToPosition(clickedGemTile, targetGemTile);
                tileInputManager.onInputEnabled.Invoke();
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
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