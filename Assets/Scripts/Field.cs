using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private uint totalBombs;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private Transform cellHolder;
    
    // List/Array to hold cells
    private Cell[,] _cells;// = new int[9,9];  
    
    private void Start()
    {
        size = new Vector2Int(9, 9);
        totalBombs = 10;
        Create(size, totalBombs);
    }

    private void Create(Vector2Int fieldSize, uint bombs)
    {
        Vector2 offset = new Vector2(fieldSize.x/2f, fieldSize.y/2f);
        _cells = new Cell[fieldSize.x, fieldSize.y];
        //TODO: Maybe use some presets to define the size/bombs (easy, medium, hard)
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject newGameObject = Instantiate(cellPrefab, cellHolder);
                Cell newCell = newGameObject.GetComponent<Cell>();
                newGameObject.name = $"Cell {i},{j}";
                newGameObject.transform.position = new Vector3(i - offset.x, j -offset.y, 0);
                newCell.Create(size, false);
                _cells[i, j] = newCell;
            }
        }

        // Define random bombs
        // TODO: we can improve this method by removing the already set places from the next batch
        for (uint i = 0; i < bombs; i++)
        {
            var randomIndex = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));

            if (_cells[randomIndex.x, randomIndex.y].HasBomb == false)
            {
                _cells[randomIndex.x, randomIndex.y].HasBomb = true;  
            }
            else
            {
                i--;
            }
        }
        
    }
    
}
