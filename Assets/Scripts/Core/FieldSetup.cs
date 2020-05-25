using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Data/FieldSetup", menuName = "Minesweeper/FieldSetup", order = 1)]
    public class FieldSetup : ScriptableObject
    {
        public string configName;
        public Vector2Int size;
        public uint mines;
    }
}