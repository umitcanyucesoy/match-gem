using System;
using BoardGame;
using UnityEngine;

namespace Gems
{
    public class Gem : MonoBehaviour
    {
        [Header("-----Gem Settings-----")]
        public Vector2Int _boardGemPosition;
        private Vector2 _firstTouchPosition;
        private Vector2 _finalTouchPosition;
        private bool _dragging;
        private float _swipeAngle = 0;
        
        [Header("-----Gem Elements-----")]
        private Board _board;
        private Gem _otherGem;

        private void Update()
        {
            if(_dragging && Input.GetMouseButtonUp(0))
            {
                _dragging = false;
                _finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            }
        }

        public void SetupGem(Vector2Int position,Board theBoard)
        {
            _boardGemPosition = position;
            _board = theBoard;
        }

        private void OnMouseDown()
        {
            if (Camera.main != null)
            {
                _firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _dragging = true;
            }
        }

        private void CalculateAngle()
        {
            _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Mathf.PI;
            if (Vector3.Distance(_firstTouchPosition, _finalTouchPosition) > 0.5f)
                MoveGem();
        }

        private void MoveGem()
        {
            if (_swipeAngle is < 45 and > -45 && _boardGemPosition.x < _board.width - 1)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x + 1, _boardGemPosition.y];
                _otherGem._boardGemPosition.x--;
                _boardGemPosition.x++;
            }
            else if (_swipeAngle is > 45 and <= 135 && _boardGemPosition.y < _board.height - 1)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x, _boardGemPosition.y + 1];
                _otherGem._boardGemPosition.y--;
                _boardGemPosition.y++;
            }
            else if (_swipeAngle is > -45 and >= -135 && _boardGemPosition.y > 0)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x, _boardGemPosition.y - 1];
                _otherGem._boardGemPosition.y++;
                _boardGemPosition.y--;
            }
            else if (_swipeAngle > 135 ||_swipeAngle < -135 && _boardGemPosition.x > 0)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x + 1, _boardGemPosition.y];
                _otherGem._boardGemPosition.x--;
                _boardGemPosition.x++;
            }
            
            _board.AllGems[_boardGemPosition.x, _boardGemPosition.y] = this;
            _board.AllGems[_otherGem._boardGemPosition.x, _otherGem._boardGemPosition.y] = _otherGem;
        }
    }
}