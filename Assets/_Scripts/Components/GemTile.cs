using _Scripts.Enums;
using _Scripts.Gameplay;
using _Scripts.SO;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Components
{
    public class GemTile : MonoBehaviour
    {
        private Gem _gem;
        private SpriteRenderer _spriteRenderer;
        private GridManager _gridManager;

        public int x;
        public int y;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public GemType GetGemType() => _gem.gemType;

        public void SetGemPosition(int row, int col)
        {
            transform.position = new Vector2(row, col);
            x = row;
            y = col;
        }

        public void InitializeGem(Gem gem, int row, int col, GridManager gridManager)
        {
            _gem = gem;
            _gridManager = gridManager;
            _spriteRenderer.sprite = _gem.sprite;
            ResetScale();
            SetGemPosition(row, col);
        }

        private void ResetScale() => transform.localScale = new Vector3(1, 1, 1);

        public void MoveTo(int row, int col, float duration = 0.3f)
        {
            transform.DOMove(new Vector3(row, col, 0), duration).OnComplete((() =>
            {
                _gridManager.InitGemAtPosition(_gem, this, row, col);
            }));
        }
    }
}