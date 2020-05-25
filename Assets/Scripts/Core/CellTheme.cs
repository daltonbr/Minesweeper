using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "CellTheme", menuName = "Minesweeper/CellTheme")]
    public class CellTheme : ScriptableObject
    {
        public Sprite unknown;
        public Sprite disarmed;
        public Sprite flag;
        public Sprite questionMark;
        public Sprite mineExploded;
        public Sprite flagWrong;
    
        public Sprite mine1;
        public Sprite mine2;
        public Sprite mine3;
        public Sprite mine4;
        public Sprite mine5;
        public Sprite mine6;
        public Sprite mine7;
        public Sprite mine8;
    }
}
