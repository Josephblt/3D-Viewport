using UnityEngine;

namespace Object_Control_Component.Handling
{
    public class HandlesManager : MonoBehaviour
    {
        public enum Controls
        {
            Position,
            Rotation,
            Scale
        }

        public enum CoordinateSystems
        {
            Global,
            Local
        }
        
        public Transform trackedCamera;

        public delegate void ControlChangedHandler(Controls control);
        public delegate void CoordinateSystemChangedHandler(CoordinateSystems coordinateSystem);
        
        public event ControlChangedHandler ControlChanged;
        public event CoordinateSystemChangedHandler CoordinateSystemChanged;
        
        private Controls _activeControl;
        private CoordinateSystems _activeCoordinateSystem;
        private Camera _handlesCamera;

        private void Start()
        {
            _handlesCamera = transform.Find("Handles Camera").GetComponent<Camera>();
            _handlesCamera.farClipPlane = trackedCamera.GetComponent<Camera>().farClipPlane;
            
            ActivateControl(Controls.Position);
            ActivateCoordinateSystem(CoordinateSystems.Global);
        }

        private void Update()
        {
            RefreshHandlesCamera();
        }

        
        private void ActivateControl(Controls control)
        {
            if (control == _activeControl)
                return;
            
            _activeControl = control;
            ControlChanged?.Invoke(_activeControl);
        }
        
        private void ActivateCoordinateSystem(CoordinateSystems coordinateSystem)
        {
            if (coordinateSystem == _activeCoordinateSystem)
                return;

            _activeCoordinateSystem = coordinateSystem;
            CoordinateSystemChanged?.Invoke(_activeCoordinateSystem);
        }
        
        private void RefreshHandlesCamera()
        {
            var handleCameraTransform = _handlesCamera.transform;
            handleCameraTransform.rotation = trackedCamera.rotation;
            handleCameraTransform.position = trackedCamera.position;
        }

        
        public void ActivatePosition()
        {
            ActivateControl(Controls.Position);
        }

        public void ActivateRotation()
        {
            ActivateControl(Controls.Rotation);
        }

        public void ActivateScale()
        {
            ActivateControl(Controls.Scale);
            ActivateCoordinateSystem(CoordinateSystems.Local);
        }
        
        public void ActivateGlobal()
        {
            ActivateCoordinateSystem(CoordinateSystems.Global);
        }
        
        public void ActivateLocal()
        {
            ActivateCoordinateSystem(CoordinateSystems.Local);
        }
    }
}