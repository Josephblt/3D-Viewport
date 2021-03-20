using System.Collections.Generic;
using System.Linq;
using Camera_Control.Camera_Actions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Camera_Control
{
    public class CameraController : MonoBehaviour
    {
        public Vector2 MouseDelta { get; private set; }

        public enum CameraAction
        {
            None,
            Orbit,
            Pan,
            Rotate,
            Zoom
        }

        private IEnumerable<ICameraAction> _cameraActions;
        private ICameraAction _currentAction;
        private Vector2 _lastMousePosition;
    
        private void Start()
        {
            _lastMousePosition = Input.mousePosition;
            LoadCameraControllers();
        }

        private void Update()
        {
            MouseDelta = new Vector2(Input.mousePosition.x - _lastMousePosition.x,
                Input.mousePosition.y - _lastMousePosition.y);

            if (EventSystem.current.IsPointerOverGameObject() || Input.GetMouseButton(0))
                ChangeActiveMode(CameraAction.None);
            else if (Input.GetMouseButton(1) && Input.GetMouseButton(2))
                ChangeActiveMode(CameraAction.Orbit);
            else if (Input.GetMouseButton(2))
                ChangeActiveMode(CameraAction.Pan);
            else if (Input.GetMouseButton(1))
                ChangeActiveMode(CameraAction.Rotate);
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
                ChangeActiveMode(CameraAction.Zoom);
            else
                ChangeActiveMode(CameraAction.None);

            _currentAction?.UpdateAction();

            _lastMousePosition = Input.mousePosition;
        }


        private void ChangeActiveMode(CameraAction cameraAction)
        {
            if (_currentAction?.CameraAction == cameraAction) return;
        
            _currentAction = _cameraActions.FirstOrDefault(cc => cc.CameraAction == cameraAction);
            _currentAction?.StartAction();
        }

        private void LoadCameraControllers()
        {
            _cameraActions = new List<ICameraAction>
            {
                new OrbitAction(this),
                new PanAction(this),
                new RotateAction(this),
                new ZoomAction(this)
            };
        }    
    }
}