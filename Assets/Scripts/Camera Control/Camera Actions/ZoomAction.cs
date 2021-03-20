using UnityEngine;

namespace Camera_Control.Camera_Actions
{
    public class ZoomAction : ICameraAction
    {
        public ZoomAction(CameraController cameraController)
        {
            _controller = cameraController;
            _targetCamera = _controller.GetComponent<Camera>();
        }
    
        private readonly CameraController _controller;
        private readonly Camera _targetCamera;

        public CameraController.CameraAction CameraAction => CameraController.CameraAction.Zoom;

        public void StartAction()
        {
        }

        public void UpdateAction()
        {
            var wheelDelta = Input.GetAxis("Mouse ScrollWheel") * .5f;
            
            var zoomDirection = _controller.transform.forward;
            var zoomAmount = wheelDelta * 500f;
            
            var mouseRay = _targetCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out var hit))
            {
                zoomDirection = mouseRay.direction;
                zoomAmount = hit.distance * wheelDelta;
            }
            
            _controller.transform.position += zoomDirection * zoomAmount;
        }
    }
}