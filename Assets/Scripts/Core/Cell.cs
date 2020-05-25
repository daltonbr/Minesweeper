using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class Cell : MonoBehaviour
    {
        private bool _hasMine;
        private Vector2Int _coordinate;
        private Image _image;
        private CellStatus _status;
        private CellTheme _theme;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
        }
        
        internal CellStatus Status
        {
            get => _status;
            set
            {
                _status = value;

                switch (value)
                {
                    case CellStatus.Unknown:
                        _image.sprite = _theme.unknown;
                        break;
                    case CellStatus.Revealed:
                        Reveal();
                        break;
                    case CellStatus.Flag:
                        _image.sprite = _theme.flag;
                        break;
                    case CellStatus.QuestionMark:
                        _image.sprite = _theme.questionMark;
                        break;
                    case CellStatus.FlagWrong:
                        _image.sprite = _theme.flagWrong;
                        break;
                    case CellStatus.Exploded:
                        _image.sprite = _theme.mineExploded;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        internal bool HasMine
        {
            get => _hasMine;
            set => _hasMine = value;
        }
    
        internal Vector2Int Coordinate
        {
            get => _coordinate;
            set => _coordinate = value;
        }
    
        internal uint MineCount { get; private set; }

        internal void Init(Vector2Int coordinate, bool hasMine, in CellTheme theme)
        {
            Coordinate = coordinate;
            _hasMine = hasMine;
            _theme = theme;

            Status = CellStatus.Unknown;
            _image.enabled = true;
        }

        internal void AddMineCount()
        {
            MineCount++;
        }

        internal void Reveal()
        {
            if (HasMine)
            {
                Status = CellStatus.Exploded;
                return;
            }
        
            switch (MineCount)
            {
                case 1:
                    _image.sprite = _theme.mine1;
                    break;
                case 2:
                    _image.sprite = _theme.mine2;
                    break;
                case 3:
                    _image.sprite = _theme.mine3;
                    break;
                case 4:
                    _image.sprite = _theme.mine4;
                    break;
                case 5:
                    _image.sprite = _theme.mine5;
                    break;
                case 6:
                    _image.sprite = _theme.mine6;
                    break;
                case 7:
                    _image.sprite = _theme.mine7;
                    break;
                case 8:
                    _image.sprite = _theme.mine8;
                    break;
                case 0:
                    _image.sprite = _theme.disarmed;
                    break;
                default:
                    Debug.LogWarning("[Cell] Invalid BombCount");
                    break;
            }

            _status = CellStatus.Revealed;
        }

    }
}
