using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private bool _hasBomb;
    private uint _bombCount = 0;
    private Vector2Int _coordinate;
    private Image _image;
    private TMP_Text _text;
    private bool _activated;


    public delegate void CellMouseUp(Cell cell);

    public event CellMouseUp OnCellMouseUp;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    public bool HasBomb
    {
        get => _hasBomb;
        set => _hasBomb = value;
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

    public uint BombCount
    {
        get => _bombCount;
        private set => _bombCount = value;
    }

    private void OnMouseUp()
    {
        OnCellMouseUp?.Invoke(this);
        //Activate();
    }

    public void Create(Vector2Int coordinate, bool hasBomb)
    {
        _hasBomb = hasBomb;
        Coordinate = coordinate;
    }

    public void AddBombCount()
    {
        _bombCount++;
    }

    private void ShowBombCount()
    {
        _text.text = BombCount.ToString();
        _text.enabled = true;
        _image.enabled = false;
    }

    private void ShowImage()
    {
        _image.enabled = true;
    }

    public bool Activate()
    {
        //TODO: check if we have a 'red flag' (preventing from activating)
        //TODO: the 'question mark' don't do anything, is just a visual reminder for the user
        if (_activated) return false;
        _activated = true;
        
        if (HasBomb)
        {
            ShowImage();
            //TODO: game over
            return true;
        }
        ShowBombCount();
        return false;
    }

}
