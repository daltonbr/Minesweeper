﻿using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private Vector2Int size;
        [SerializeField] private uint totalMines;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform cellHolder;
        [SerializeField] private CellTheme cellTheme;
    
        private Cell[,] _cells;
        private List<Cell> _mineCells;

        public delegate void GameEnd();
        public static event GameEnd OnGameLost;
        public static event GameEnd OnGameWon;

        private void Start()
        {
            //TODO: create fields dynamically
            size = new Vector2Int(9, 9);
            totalMines = 10;
            Create(size, totalMines);

            InputHandler.OnLeftClickCell += HandleLeftClickCell;
            InputHandler.OnRightClickCell += CycleFlagStatus;
        }

        private void OnDestroy()
        {
            InputHandler.OnLeftClickCell -= HandleLeftClickCell;
            InputHandler.OnRightClickCell -= CycleFlagStatus;
        }

        private void HandleLeftClickCell(Cell cell)
        {
            TryReveal(cell);
        }

        private void TryReveal(Cell cell)
        {
            if (cell.Status != CellStatus.Unknown) return;
            
            if (cell.HasMine)
            {
                cell.Reveal();
                GameIsLost();
                return;
            }
            
            cell.Reveal();
            foreach (Cell neighbor in GetAllUnknownNeighbors(cell))
            {
                if (neighbor.HasMine) continue;
                if (neighbor.MineCount == 0)
                {
                    TryReveal(neighbor);
                }
                else
                {
                    neighbor.Reveal();
                }
            }
            
            if (IsGameWon())
            {
                Debug.Log("[Field] You won!");
            }
        }

        /// <summary>
        /// Cycle between `Unknown`, `Flag` and `QuestionMark`
        /// </summary>
        private void CycleFlagStatus(Cell cell)
        {
            var status = cell.Status;
            switch (status)
            {
                case CellStatus.Unknown:
                {
                    cell.Status = CellStatus.Flag;
                    if (IsGameWon()) return;
                    break;
                }
                case CellStatus.Flag:
                    cell.Status = CellStatus.QuestionMark;
                    break;
                case CellStatus.QuestionMark:
                    cell.Status = CellStatus.Unknown;
                    break;
            }
        }

        private void GameIsLost()
        {
            //TODO: End Game State
            Debug.Log("[Field] GameOver");
            CheckWrongPlacedFlags();
            CheckMinesNotFlagged();
            OnGameLost?.Invoke();
        }

        private void CheckWrongPlacedFlags()
        {
            foreach (var cell in _cells)
            {
                if (cell.Status == CellStatus.Flag && cell.HasMine == false)
                {
                    cell.Status = CellStatus.FlagWrong;
                }
            }
        }

        private void CheckMinesNotFlagged()
        {
            foreach (var mineCell in _mineCells.Where(
                        mineCell => mineCell.Status == CellStatus.Unknown ||
                                    mineCell.Status == CellStatus.QuestionMark))
            {
                mineCell.Status = CellStatus.Exploded;
            }
        }
    
        private void Create(Vector2Int fieldSize, uint mines)
        {
            Vector2 offset = new Vector2(fieldSize.x / 2f, fieldSize.y / 2f);
            _cells = new Cell[fieldSize.x, fieldSize.y];
            //TODO: Use some presets to define size/mines (easy, medium, expert)
            for (int i = 0; i < this.size.x; i++)
            {
                for (int j = 0; j < this.size.y; j++)
                {
                    GameObject newGameObject = Instantiate(cellPrefab, cellHolder);
                    Cell newCell = newGameObject.GetComponent<Cell>();
                    newGameObject.name = $"Cell {i},{j}";
                    newGameObject.transform.position = new Vector3(i - offset.x, j - offset.y, 0);
                    newCell.Init(new Vector2Int(i, j), false, in cellTheme);
                    _cells[i, j] = newCell;
                }
            }

            // Define random mines
            // TODO: we can improve this method by removing the already set places from the next batch
            _mineCells = new List<Cell>();
            for (uint i = 0; i < mines; i++)
            {
                var randomPosition = new Vector2Int(Random.Range(0, this.size.x), Random.Range(0, this.size.y));

                if (_cells[randomPosition.x, randomPosition.y].HasMine == false)
                {
                    _cells[randomPosition.x, randomPosition.y].HasMine = true;
                    _mineCells.Add(_cells[randomPosition.x, randomPosition.y]);
                }
                else
                {
                    i--;
                }
            }

            // Determine all bomb counts
            foreach (Cell mineCell in _mineCells)
            {
                AddMineCountToNeighbors(mineCell.Coordinate);
            }
        }

        private void AddMineCountToNeighbors(Vector2Int coordinate)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // skipping out-of-bounds
                    if (coordinate.x + i < 0 || coordinate.x + i >= size.x) continue;
                    if (coordinate.y + j < 0 || coordinate.y + j >= size.y) continue;
                    // exclude self
                    if (i == 0 && j == 0) continue;
                    _cells[coordinate.x + i, coordinate.y + j].AddMineCount();
                }
            }
        }
        
        private IEnumerable<Cell> GetAllUnknownNeighbors(Cell cell)
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
                    if (currentCell.Status == CellStatus.Unknown)
                    {
                        neighbors.Add(currentCell);
                    }
                }
            }
            return neighbors;
        }

        private bool IsGameWon()
        {
            foreach (var cell in _cells)
            {
                if (cell.Status == CellStatus.Unknown ||
                    cell.Status == CellStatus.QuestionMark ||
                    cell.Status == CellStatus.Flag && cell.HasMine == false)
                {
                    return false;
                }
            }
            OnGameWon?.Invoke();
            return true;
        }
    }
}
