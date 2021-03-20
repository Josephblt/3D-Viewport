using UnityEngine;

namespace Camera_Control.Camera_Actions
{
    public class PanAction : ICameraAction
    {
        public PanAction(CameraController cameraController)
        {
            _controller = cameraController;
            _targetCamera = _controller.GetComponent<Camera>();
        }

        private bool _anchoredPan;
        private Ray _cameraRay;
        private Plane _panPlane;
        private Vector3 _panStartPoint;
        
        private readonly CameraController _controller;
        private readonly Camera _targetCamera;
    
        public CameraController.CameraAction CameraAction => CameraController.CameraAction.Pan;

        public void StartAction()
        {
            _cameraRay = _targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        
            var mouseRay = _targetCamera.ScreenPointToRay(Input.mousePosition);
            _anchoredPan = Physics.Raycast(mouseRay, out var hitInfo);

            if (!(hitInfo.distance > _targetCamera.nearClipPlane)) return;
            _panStartPoint = mouseRay.GetPoint(hitInfo.distance);
            _panPlane = new Plane(_cameraRay.direction, _panStartPoint);
        }

        public void UpdateAction()
        {
            if (Mathf.Approximately(_controller.MouseDelta.magnitude, 0f)) return;
        
            if (_anchoredPan)
            {
                var mouseRay = _targetCamera.ScreenPointToRay(Input.mousePosition);
                _panPlane.Raycast(mouseRay, out var distance);
                var panVector = _panStartPoint - mouseRay.GetPoint(distance);
                _targetCamera.transform.Translate(panVector, Space.World);
            }
            else
            {
                var panVector = new Vector3(_controller.MouseDelta.x, _controller.MouseDelta.y, 0f);
                _targetCamera.transform.Translate(-panVector);
            }
        }
    }
}