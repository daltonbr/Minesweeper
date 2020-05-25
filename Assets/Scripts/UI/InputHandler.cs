using Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace UI
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCamera;

        public delegate void ClickCell(Cell cell);

        public static event ClickCell OnLeftClickCell;
        public static event ClickCell OnRightClickCell;

        private bool _active;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
#if UNITY_EDITOR
            Assert.IsNotNull(_mainCamera);
#endif
            Field.OnGameStart += HandleGameStart;
            Field.OnGameLost += HandleGameLost;
            Field.OnGameWon += HandleGameWon;
        }

        private void OnDestroy()
        {
            Field.OnGameStart -= HandleGameStart;
            Field.OnGameLost -= HandleGameLost;
            Field.OnGameWon -= HandleGameWon;
        }

        private void HandleGameStart()
        {
            _active = true;
        }
        
        private void HandleGameLost()
        {
            _active = false;
        }
        
        private void HandleGameWon()
        {
            _active = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && _active)
            {
                //TODO: consider a regular button
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (!hit.collider) return;
                var cell = hit.collider.GetComponent<Cell>();
                if (cell)
                {
                    OnLeftClickCell?.Invoke(cell);
                }
                return;
            }

            if (Input.GetMouseButtonUp(1) && _active)
            {
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (!hit.collider) return;
                var cell = hit.collider.GetComponent<Cell>();
                if (cell)
                {
                    OnRightClickCell?.Invoke(cell);
                }
            }
        }
    
    }
}
