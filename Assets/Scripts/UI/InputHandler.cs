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
    
        private void Awake()
        {
            _mainCamera = Camera.main;
#if UNITY_EDITOR
            Assert.IsNotNull(_mainCamera);
#endif
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
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

            if (Input.GetMouseButtonUp(1))
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
