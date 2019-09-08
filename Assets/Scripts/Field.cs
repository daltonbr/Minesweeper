using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [SerializeField] private Vector2Int fieldSize;
    [SerializeField] private uint totalBombs;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellHolder;

    private Cell[,] _cells;
    private List<Cell> _mineCells;
    private Camera _mainCamera;

    public delegate void GameWon();
    public static event GameWon OnGameWon;
    
    public delegate void GameLost();
    public static event GameLost OnGameLost;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        fieldSize = new Vector2Int(9, 9);
        totalBombs = 10;
        Create(fieldSize, totalBombs);
    }

    private void HandleCellLeftMouseUp(Cell cell)
    {
        if (cell.HasMine)
        {
            cell.Activate();
            GameIsLost();
            return;
        }
        
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
        if (IsGameWon())
        {
            Debug.Log("[Field] You won!");
        }
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

    private void GameIsLost()
    {
        Debug.Log("[Field] GameOver");
        OnGameLost?.Invoke();
    }

    private void Create(Vector2Int fieldSize, uint bombs)
    {
        Vector2 offset = new Vector2(fieldSize.x / 2f, fieldSize.y / 2f);
        _cells = new Cell[fieldSize.x, fieldSize.y];
        //TODO: Maybe use some presets to define the size/bombs (easy, medium, hard)
        for (int i = 0; i < this.fieldSize.x; i++)
        {
            for (int j = 0; j < this.fieldSize.y; j++)
            {
                GameObject newGameObject = Instantiate(cellPrefab, cellHolder);
                Cell newCell = newGameObject.GetComponent<Cell>();
                newGameObject.name = $"Cell {i},{j}";
                newGameObject.transform.position = new Vector3(i - offset.x, j - offset.y, 0);
                newCell.Init(new Vector2Int(i, j), false);
                _cells[i, j] = newCell;
            }
        }

        // Define random mines
        // TODO: we can improve this method by removing the already set places from the next batch
        _mineCells = new List<Cell>();
        for (uint i = 0; i < bombs; i++)
        {
            var randomIndex = new Vector2Int(Random.Range(0, this.fieldSize.x), Random.Range(0, this.fieldSize.y));

            if (_cells[randomIndex.x, randomIndex.y].HasMine == false)
            {
                _cells[randomIndex.x, randomIndex.y].HasMine = true;
                _mineCells.Add(_cells[randomIndex.x, randomIndex.y]);
            }
            else
            {
                i--;
            }
        }

        // Determine all bomb counts
        foreach (Cell bombCell in _mineCells)
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
                if (coordinate.x + i < 0 || coordinate.x + i >= fieldSize.x) continue;
                if (coordinate.y + j < 0 || coordinate.y + j >= fieldSize.y) continue;
                // self mine
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
            if (coordinate.x + i < 0 || coordinate.x + i >= fieldSize.x) continue;
            for (int j = -1; j <= 1; j++)
            {
                if (coordinate.y + j < 0 || coordinate.y + j >= fieldSize.y) continue;
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
            if (coordinate.x + i < 0 || coordinate.x + i >= fieldSize.x) continue;
            for (int j = -1; j <= 1; j++)
            {
                if (coordinate.y + j < 0 || coordinate.y + j >= fieldSize.y) continue;
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

    private bool IsGameWon()
    {
        for (int i = 0; i < fieldSize.x; i++)
        {
            for (int j = 0; j < fieldSize.y; j++)
            {
                if (!_cells[i, j].HasMine && !_cells[i, j].IsActive)
                {
                    return false;
                }
            }
        }
        OnGameWon?.Invoke();
        return true;
    }
}
