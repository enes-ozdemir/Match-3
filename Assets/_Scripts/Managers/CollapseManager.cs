using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _Scripts.Managers
{
    public class CollapseManager : MonoBehaviour
    {
        [SerializeField] private int gridRowCount = 8;
        [SerializeField] private int gridColumnCount = 8;
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private TileInputManager tileInputManager;
        [SerializeField] private GridManager gridManager;

        private List<GemTile> CollapseColumn(int col, float collapseTime = 0.3f)
        {
            var movingPieces = new List<GemTile>();
            for (int i = 0; i < gridColumnCount - 1; i++)
            {
                if (GridManager.gemArray[col, i] == null)
                {
                    for (int j = i + 1; j < gridColumnCount; j++)
                    {
                        if (GridManager.gemArray[col, j] != null)
                        {
                            GridManager.gemArray[col, j].MoveTo(col, i);
                            GridManager.gemArray[col, i] = GridManager.gemArray[col, j];
                            GridManager.gemArray[col, i].SetGemPosition(col, i);
                            if (!movingPieces.Contains(GridManager.gemArray[col, i]))
                            {
                                movingPieces.Add(GridManager.gemArray[col, i]);
                            }

                            GridManager.gemArray[col, j] = null;
                            break;
                        }
                    }
                }
            }

            return movingPieces;
        }

        private List<GemTile> CollapseColumn(List<GemTile> gemTiles)
        {
            var movingPieces = new List<GemTile>();
            var columsToCollapse = GetColumns(gemTiles);

            foreach (var col in columsToCollapse)
            {
                movingPieces = movingPieces.Union(CollapseColumn(col)).ToList();
            }

            return movingPieces;
        }

        private List<int> GetColumns(List<GemTile> gemTiles)
        {
            var columns = new List<int>();

            foreach (var tile in gemTiles)
            {
                if (!columns.Contains(tile.x))
                {
                    columns.Add(tile.x);
                }
            }

            return columns;
        }

        public void ClearAndRefillBoard(List<GemTile> tileList)
        {
            StartCoroutine(ClearAndRefillBoardCo(tileList));
        }

        private IEnumerator ClearAndRefillBoardCo(List<GemTile> tileList)
        {
            tileInputManager.onInputDisabled.Invoke();
            var matches = tileList;

            do
            {
                yield return StartCoroutine(ClearAndCollapseCo(matches));
                yield return null;
                yield return StartCoroutine(gridManager.RefillRoutine());
                matches = matchManager.FindAllMatches();

                yield return new WaitForSeconds(0.2f);
            } while (matches.Count != 0);

            tileInputManager.onInputEnabled.Invoke();
        }

        private IEnumerator ClearAndCollapseCo(List<GemTile> tileList)
        {
            foreach (var tile in tileList)
            {
                print(tile.GetGemType() + $"Count list {tileList.Count}");
            }

            yield return new WaitForSeconds(0.5f);
            var matches = tileList;

            while (matches.Count > 0)
            {
                gridManager.RemoveGems(matches);
                yield return new WaitForSeconds(0.5f);
                var collapsingGems = CollapseColumn(matches);

                while (!IsCollapsed(collapsingGems))
                {
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);
                matches = matchManager.FindMatchesInList(collapsingGems);
            }

            yield return null;
        }

        private bool IsCollapsed(List<GemTile> gemTiles)
        {
            foreach (var tile in gemTiles)
            {
                if (tile != null)
                {
                    if (tile.transform.position.y - (float) tile.y > 0.001f)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}