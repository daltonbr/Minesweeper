using Core;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : MonoBehaviour
    {
        [Range(1f, 2f)]
        [SerializeField] private float marginOffsetPercentage = 1.2f;

        private Camera _mainCamera;
    
        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();
            Field.OnUpdateFieldSetup += AdjustCameraHeight;
        }

        private void OnDestroy()
        {
            Field.OnUpdateFieldSetup -= AdjustCameraHeight;
        }

        private void AdjustCameraHeight(FieldSetup setup)
        {
            Vector2Int fieldSize = setup.size;
            
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
            
            // Fit from inside
            float fieldAspectRatio = fieldSize.x / (float)fieldSize.y;

            if (fieldAspectRatio > _mainCamera.aspect)
            {
                var ratioOverRatio = fieldAspectRatio / _mainCamera.aspect;
                _mainCamera.orthographicSize = (((fieldSize.x * marginOffsetPercentage)/ fieldAspectRatio) / 2f) * ratioOverRatio; 
                                               
            }
            else
            {
                _mainCamera.orthographicSize = fieldSize.y / 2f * marginOffsetPercentage;
            }
        }
    }
}
