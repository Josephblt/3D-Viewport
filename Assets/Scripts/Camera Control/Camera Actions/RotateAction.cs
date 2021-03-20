using UnityEngine;

namespace Camera_Control.Camera_Actions
{
    public class RotateAction : ICameraAction
    {
        public RotateAction(CameraController cameraController)
        {
            _controller = cameraController;
            _targetCamera = _controller.GetComponent<Camera>();
        }

        private readonly CameraController _controller;
        private readonly Camera _targetCamera;

        public CameraController.CameraAction CameraAction => CameraController.CameraAction.Rotate;

        public void StartAction()
        {
        }

        public void UpdateAction()
        {
            var axisX = _controller.MouseDelta.x;
            var axisY = _controller.MouseDelta.y;

            var cameraTransform = _targetCamera.transform;
            var xRotation = Quaternion.AngleAxis(-axisY, cameraTransform.right);
            var yRotation = Quaternion.AngleAxis(axisX, Vector3.up);
            cameraTransform.rotation = yRotation * xRotation * cameraTransform.rotation;
        }
    }
}