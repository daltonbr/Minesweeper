using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private bool _hasBomb;
    private uint _bombCount = 0;
    private Vector2Int _coordinate;
    private Image _image;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
    }

    public bool HasBomb
    {
        get => _hasBomb;
        set => _hasBomb = value;
    }

    public uint BombCount
    {
        get => _bombCount;
        private set => _bombCount = value;
    }

    private void Start()
    {

    }

    private void OnMouseUp()
    {
        Debug.Log($"HasBomb {HasBomb}");
        if (HasBomb)
        {
            _image.enabled = true;
        }
    }

    public void Create(Vector2Int coordinate, bool hasBomb)
    {
        _hasBomb = hasBomb;
        _coordinate = coordinate;
    }

    public void AddBombCount()
    {
        _bombCount++;
    }
    
}

