using System;
using _Scripts.Managers;
using _Scripts.SO;
using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class GemTile : MonoBehaviour
    {
        private Gem _gem;
        public int x;
        public int y;
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;
        private Vector3 _firstTouchPos;
        private Vector3 _endTouchPos;

        private GridManager _gridManager;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void SetGemPosition(int row, int col)
        {
            transform.position = new Vector2(row, col);
            x = row;
            y = col;
        }

        public void InitializeGem(Gem gem, int row, int col, GridManager gridManager)
        {
            _gem = gem;
            _gridManager = gridManager;
            _spriteRenderer.sprite = gem.sprite;
            SetGemPosition(row, col);
        }

        public void MoveTo(int row, int col)
        {
            transform.DOMove(new Vector3(row, col, 0), 0.5f).OnComplete((() =>
            {
                //todo fix this
                _gridManager.InitGemAtPosition(_gem, this, row, col);
            }));
        }
    }
}