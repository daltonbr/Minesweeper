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
        //Debug.Log($"HasBomb {HasBomb}");
        if (HasBomb)
        {
            _image.enabled = true;
            return;
        }

        if (BombCount >= 0)
        {
            ShowBombCount();
            return;
        }
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

    public void ShowBombCount()
    {
        _text.text = BombCount.ToString();
        _text.enabled = true;
        _image.enabled = false;
    }
}
