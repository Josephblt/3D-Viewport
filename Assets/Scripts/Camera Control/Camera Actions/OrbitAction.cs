using UnityEngine;

namespace Camera_Control.Camera_Actions
{
    public class OrbitAction : ICameraAction
    {
        public OrbitAction(CameraController cameraController)
        {
            Controller = cameraController;
            _targetCamera =  Controller.GetComponent<Camera>();
        }

        private Vector3 _orbitStartPoint;
        private readonly Camera _targetCamera;

        private CameraController Controller { get; }

        public CameraController.CameraAction CameraAction => CameraController.CameraAction.Orbit;

        public void StartAction()
        {
            var mouseRay = _targetCamera.ScreenPointToRay(Input.mousePosition);
            _orbitStartPoint = Vector3.zero;
            Physics.Raycast(mouseRay, out var hit);
            _orbitStartPoint = mouseRay.GetPoint(hit.distance);
        }

        public void UpdateAction()
        {
            var axisX = Controller.MouseDelta.x;
            var axisY = Controller.MouseDelta.y;

            var cameraTransform = Controller.transform;
            var xRotation = Quaternion.AngleAxis(-axisY, cameraTransform.right);
            var yRotation = Quaternion.AngleAxis(axisX, Vector3.up);
            var orbitPivot = cameraTransform.position - _orbitStartPoint;        
        
            cameraTransform.position = _orbitStartPoint + yRotation * xRotation * orbitPivot;
            cameraTransform.rotation = yRotation * xRotation * cameraTransform.rotation;
        }
    }
}