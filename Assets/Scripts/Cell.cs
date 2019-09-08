using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private bool _hasMine;
    private uint _mineCount = 0;
    private Vector2Int _coordinate;
    private Image _image;
    //private TMP_Text _text;
    private bool _activated;
    private bool _flagged;

    [SerializeField] private Sprite cellEmpty;
    [SerializeField] private Sprite cellActivated;
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite flag;
    [SerializeField] private Sprite question;
    [SerializeField] private Sprite mine;

    [SerializeField] private Sprite mine1;
    [SerializeField] private Sprite mine2;
    [SerializeField] private Sprite mine3;
    [SerializeField] private Sprite mine4;
    [SerializeField] private Sprite mine5;
    [SerializeField] private Sprite mine6;
    [SerializeField] private Sprite mine7;
    [SerializeField] private Sprite mine8;
    
    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        //_text = GetComponentInChildren<TMP_Text>();
    }

    public bool HasMine
    {
        get => _hasMine;
        set => _hasMine = value;
    }

    public bool IsActive
    {
        get => _activated;
    }

    public Vector2Int Coordinate
    {
        get => _coordinate;
        set => _coordinate = value;
    }

    public uint MineCount
    {
        get => _mineCount;
        private set => _mineCount = value;
    }
    
    public void Init(Vector2Int coordinate, bool hasBomb)
    {
        _hasMine = hasBomb;
        Coordinate = coordinate;
        _image.sprite = empty;
        _image.enabled = true;
    }

    public void AddBombCount()
    {
        _mineCount++;
    }

    private void ShowMineCount()
    {
        if (HasMine)
        {
            _image.sprite = mine;
            Debug.LogWarning("[Cell] try to showMineCount in a mined cell");
            return;
        }
        
        switch (MineCount)
        {
            case 1:
                _image.sprite = mine1;
                break;
            case 2:
                _image.sprite = mine2;
                break;
            case 3:
                _image.sprite = mine3;
                break;
            case 4:
                _image.sprite = mine4;
                break;
            case 5:
                _image.sprite = mine5;
                break;
            case 6:
                _image.sprite = mine6;
                break;
            case 7:
                _image.sprite = mine7;
                break;
            case 8:
                _image.sprite = mine8;
                break;
            case 0:
                _image.sprite = cellActivated;
                break;
            default:
                Debug.LogWarning("[Cell] Invalid BombCount");
                break;
        }
        _image.enabled = true;
    }

    private void ShowMine()
    {
        _image.sprite = mine;
        _image.enabled = true;
    }

    public bool Activate()
    {
        //TODO: check if we have a 'red flag' (preventing from activating)
        //TODO: the 'question mark' don't do anything, is just a visual reminder for the user
        if (_activated) return false;
        if (_flagged) return false;
        
        _activated = true;
        _image.sprite = cellActivated;
        
        if (HasMine)
        {
            ShowMine();
            //TODO: game over
            return true;
        }
        ShowMineCount();
        return false;
    }

    public void ToggleFlag()
    {
        if (_activated) return;
        
        _flagged = !_flagged;
        if (_flagged)
        {
            _image.sprite = flag;
        }
        else
        {
            _image.sprite = empty;
        }
    }
}
