using System;
using BoardGame;
using Cysharp.Threading.Tasks;
using Data;
using UnityEngine;

namespace Gems
{
    public enum GemType
    {
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
    }
    
    public class Gem : MonoBehaviour
    {
        [Header("-----Gem Settings-----")]
        private Vector2Int _boardGemPosition;
        private Vector2 _firstTouchPosition;
        private Vector2 _finalTouchPosition;
        private Vector2Int _previousPosition;
        private bool _dragging;
        private float _swipeAngle = 0;
        
        [Header("-----Gem Elements-----")]
        [SerializeField] private GemData gemData;
        public bool isMatched = false;
        public GemType gemType;
        private Board _board;
        private Gem _otherGem;

        private void Update()
        {
            MoveGem();
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

        private void OnMouseUp()
        {
            if (_dragging)
            {
                _dragging = false;
                _finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            }
        }

        private void CalculateAngle()
        {
            _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Mathf.PI;
            if (Vector3.Distance(_firstTouchPosition, _finalTouchPosition) > 0.5f)
                PositionSwapGem();
        }

        private void PositionSwapGem()
        {
            _previousPosition = _boardGemPosition;
            
            if (_swipeAngle > -45 && _swipeAngle <= 45 && _boardGemPosition.x < _board.width - 1)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x + 1, _boardGemPosition.y];
                _otherGem._boardGemPosition.x -= 1;
                _boardGemPosition.x += 1;
            }
            else if (_swipeAngle > 45 && _swipeAngle <= 135 && _boardGemPosition.y < _board.height - 1)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x, _boardGemPosition.y + 1];
                _otherGem._boardGemPosition.y -= 1;
                _boardGemPosition.y += 1;
            }
            else if ((_swipeAngle > 135 || _swipeAngle <= -135) && _boardGemPosition.x > 0)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x - 1, _boardGemPosition.y];
                _otherGem._boardGemPosition.x += 1;
                _boardGemPosition.x -= 1;
            }
            else if (_swipeAngle < -45 && _swipeAngle >= -135 && _boardGemPosition.y > 0)
            {
                _otherGem = _board.AllGems[_boardGemPosition.x, _boardGemPosition.y - 1];
                _otherGem._boardGemPosition.y += 1;
                _boardGemPosition.y -= 1;
            }
            
            _board.AllGems[_boardGemPosition.x, _boardGemPosition.y] = this;
            _board.AllGems[_otherGem._boardGemPosition.x, _otherGem._boardGemPosition.y] = _otherGem;
            
            CheckMoveTask().Forget();
        }

        private void MoveGem()
        {
            if (Vector2.Distance(transform.position, new Vector3(_boardGemPosition.x, _boardGemPosition.y, 0)) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(_boardGemPosition.x, _boardGemPosition.y, 0)
                    , gemData.gemMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(_boardGemPosition.x, _boardGemPosition.y, 0);
                _board.AllGems[(int)transform.position.x, (int)transform.position.y] = this;
            }
        }

        private async UniTask CheckMoveTask()
        {
            await UniTask.WaitUntil(() => !_dragging);
            await UniTask.WaitUntil(() => Vector2.Distance(transform.position,
                new Vector3(_boardGemPosition.x, _boardGemPosition.y, 0)) < 0.1f);
            
            MatchFinder.Instance.FindAllMatches();
            if (_otherGem != null)
            {
                if (!_otherGem.isMatched && !isMatched)
                {
                    _otherGem._boardGemPosition = _boardGemPosition;
                    _boardGemPosition = _previousPosition;
                    
                    _board.AllGems[_boardGemPosition.x, _boardGemPosition.y] = this;
                    _board.AllGems[_otherGem._boardGemPosition.x, _otherGem._boardGemPosition.y] = _otherGem;
                }
            }
        }
    }
}