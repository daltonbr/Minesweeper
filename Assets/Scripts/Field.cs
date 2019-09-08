using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private uint totalBombs;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private Transform cellHolder;

    private Camera _mainCamera;
    
    // List/Array to hold cells
    private Cell[,] _cells; // = new int[9,9];
    private List<Cell> _bombCells;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        size = new Vector2Int(9, 9);
        totalBombs = 10;
        Create(size, totalBombs);
    }

    private void HandleCellLeftMouseUp(Cell cell)
    {
        if (cell.HasMine)
        {
            cell.Activate();
            GameOver();
            return;
        }

        if (cell.MineCount == 0)
        {
            cell.Activate();
            foreach (var neighbor in GetAllInactiveNeighbors(cell))
            {
                if (neighbor.HasMine) continue;
                if (neighbor.MineCount == 0)
                {
                    HandleCellLeftMouseUp(neighbor);
                }
                else
                {
                    if (neighbor.MineCount > 0)
                    {
                        neighbor.Activate();
                    }
                }
            }
            return;
        }

        //cell.BombCount > 0
        cell.Activate();
    }

    private void HandleCellRightMouseUp(Cell cell)
    {
        cell.ToggleFlag();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Left Click");
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider)
            {
                //Debug.Log($"[LeftClick] This hit at {hit.point} @{hit.collider.name}");
                var cell = hit.collider.GetComponent<Cell>();
                if (cell) HandleCellLeftMouseUp(cell);
            }
            return;
        }

        if (Input.GetMouseButtonUp(1))
        {
            //Debug.Log("Right Click");
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider)
            {
                //Debug.Log($"[RightClick] This hit at {hit.point} @{hit.collider.name}");
                var cell = hit.collider.GetComponent<Cell>();
                if (cell) HandleCellRightMouseUp(cell);
            }
            return;
        }
    }

    private void GameOver()
    {
        Debug.LogWarning("[Field] GameOver");
    }

    private void Create(Vector2Int fieldSize, uint bombs)
    {
        Vector2 offset = new Vector2(fieldSize.x / 2f, fieldSize.y / 2f);
        _cells = new Cell[fieldSize.x, fieldSize.y];
        //TODO: Maybe use some presets to define the size/bombs (easy, medium, hard)
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject newGameObject = Instantiate(cellPrefab, cellHolder);
                Cell newCell = newGameObject.GetComponent<Cell>();
                newGameObject.name = $"Cell {i},{j}";
                newGameObject.transform.position = new Vector3(i - offset.x, j - offset.y, 0);
                newCell.Init(new Vector2Int(i, j), false);
//                newCell.OnCellLeftMouseUp += HandleCellLeftMouseUp;
//                newCell.OnCellRightMouseUp += HandleCellRightMouseUp;
                _cells[i, j] = newCell;
            }
        }

        // Define random bombs
        // TODO: we can improve this method by removing the already set places from the next batch
        _bombCells = new List<Cell>();
        for (uint i = 0; i < bombs; i++)
        {
            var randomIndex = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));

            if (_cells[randomIndex.x, randomIndex.y].HasMine == false)
            {
                _cells[randomIndex.x, randomIndex.y].HasMine = true;
                _bombCells.Add(_cells[randomIndex.x, randomIndex.y]);
            }
            else
            {
                i--;
            }
        }

        // Determine all bomb counts
        foreach (Cell bombCell in _bombCells)
        {
            AddBombCountToNeighbors(bombCell.Coordinate);
        }
    }

    private void AddBombCountToNeighbors(Vector2Int coordinate)
    {
        var neighbors = new List<Cell>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // skipping out-of-bounds
                if (coordinate.x + i < 0 || coordinate.x + i >= size.x) continue;
                if (coordinate.y + j < 0 || coordinate.y + j >= size.y) continue;
                // self bomb
                if (i == 0 && j == 0) continue;
                _cells[coordinate.x + i, coordinate.y + j].AddBombCount();
            }
        }
    }

    private List<Cell> GetAllNeighbors(Cell cell)
    {
        var coordinate = cell.Coordinate;
        var neighbors = new List<Cell>();
        for (int i = -1; i <= 1; i++)
        {
            if (coordinate.x + i < 0 || coordinate.x + i >= size.x) continue;
            for (int j = -1; j <= 1; j++)
            {
                if (coordinate.y + j < 0 || coordinate.y + j >= size.y) continue;
                if (i == 0 && j == 0) continue;
                neighbors.Add(_cells[coordinate.x + i, coordinate.y + j]);
            }
        }

        return neighbors;
    }
    
    private List<Cell> GetAllInactiveNeighbors(Cell cell)
    {
        var coordinate = cell.Coordinate;
        var neighbors = new List<Cell>();
        for (int i = -1; i <= 1; i++)
        {
            if (coordinate.x + i < 0 || coordinate.x + i >= size.x) continue;
            for (int j = -1; j <= 1; j++)
            {
                if (coordinate.y + j < 0 || coordinate.y + j >= size.y) continue;
                if (i == 0 && j == 0) continue;
                var currentCell = _cells[coordinate.x + i, coordinate.y + j];
                if (!currentCell.IsActive)
                {
                    neighbors.Add(currentCell);
                }
            }
        }
        return neighbors;
    }
}
