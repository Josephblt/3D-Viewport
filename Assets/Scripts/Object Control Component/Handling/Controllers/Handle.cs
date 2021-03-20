using UnityEngine;
using UnityEngine.EventSystems;

namespace Object_Control_Component.Handling.Controllers
{
    public class Handle : MonoBehaviour
    {
        private IHandleController _controller;

        private void Start()
        {
            _controller = transform.parent.GetComponent<IHandleController>();
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (!_controller.Handling)
                _controller.StartHandling(this);
        }

        private void OnMouseDrag()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _controller.EndHandling(this);
                _controller.Dehighlight(this);
                return;
            }

            if (_controller.Handling)
                _controller.ExecuteHandling(this);
        }
        
        private void OnMouseUp()
        {
            _controller.EndHandling(this);
            _controller.Dehighlight(this);
        }
        
        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _controller.EndHandling(this);
                _controller.Dehighlight(this);
                return;
            }

            if (!_controller.Handling)
                _controller.Highlight(this);
        }

        private void OnMouseExit()
        {
            if (!_controller.Handling)
                _controller.Dehighlight(this);
        }
    }
}
