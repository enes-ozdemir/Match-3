using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class CollapseManager : MonoBehaviour
    {
        [SerializeField] private TileInputManager tileInputManager;
        [SerializeField] private GridManager gridManager;

        private MatchManager _matchManager;
        private int _gridColumnCount;

        private void Start()
        {
            var gridSize = LevelManager.Instance.GetGridSize();
            _gridColumnCount = gridSize.y;
            _matchManager = new MatchManager(gridSize);
        }

        private List<GemTile> CollapseColumn(int col)
        {
            var movingPieces = new List<GemTile>();
            for (int i = 0; i < _gridColumnCount - 1; i++)
            {
                if (GridManager.gemArray[col, i] == null)
                {
                    for (int j = i + 1; j < _gridColumnCount; j++)
                    {
                        if (GridManager.gemArray[col, j] != null)
                        {
                            CollapseGems(col, j, i);

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

        private void CollapseGems(int col, int j, int i)
        {
            GridManager.gemArray[col, j].MoveTo(col, i);
            GridManager.gemArray[col, i] = GridManager.gemArray[col, j];
            GridManager.gemArray[col, i].SetGemPosition(col, i);
        }

        private List<GemTile> CollapseColumn(List<GemTile> gemTiles)
        {
            var movingPieces = new List<GemTile>();
            var columnsToCollapse = GetColumns(gemTiles);

            foreach (var col in columnsToCollapse)
            {
                movingPieces = movingPieces.Union(CollapseColumn(col)).ToList();
            }

            return movingPieces;
        }

        private List<int> GetColumns(List<GemTile> gemTiles)
        {
            var columns = new List<int>();

            foreach (var tile in gemTiles.Where(tile => tile != null).Where(tile => !columns.Contains(tile.x)))
            {
                columns.Add(tile.x);
            }

            return columns;
        }

        public async UniTask ClearAndRefillBoard(List<GemTile> tileList)
        {
            tileInputManager.onInputDisabled.Invoke();
            var matches = tileList;

            do
            {
                await ClearAndCollapseCo(matches);
                gridManager.FillGrid(true);
                matches = _matchManager.FindAllMatches();

                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            } while (matches.Count != 0);

            tileInputManager.onInputEnabled.Invoke();
        }

        private async UniTask ClearAndCollapseCo(List<GemTile> tileList)
        {
            var matches = tileList;

            while (matches.Count > 0)
            {
                gridManager.RemoveGems(matches);
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

                var collapsingGems = CollapseColumn(matches);

                while (!IsCollapsed(collapsingGems))
                {
                    await UniTask.Yield();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

                matches = _matchManager.FindMatchesInList(collapsingGems);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        }

        private bool IsCollapsed(List<GemTile> gemTiles)
        {
            return gemTiles.Where(tile => tile != null).All(tile => !(tile.transform.position.y - tile.y > 0.001f));
        }
    }
}