using Object_Control_Component.Selection;
using UnityEngine;
using Selectable = Object_Control_Component.Selection.Selectable;
using Toggle = UnityEngine.UI.Toggle;

namespace Object_Control_Component.Handling
{
    public class UIManager : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Toggle _position;
        private Toggle _rotation;
        private Toggle _scale;
        private Toggle _global;
        private Toggle _local;
        
        private HandlesManager _handlesManager;
        private SelectionManager _selectionManager;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _position = transform.Find("Controls/Position").GetComponent<Toggle>();
            _rotation = transform.Find("Controls/Rotation").GetComponent<Toggle>();
            _scale = transform.Find("Controls/Scale").GetComponent<Toggle>();
            _global = transform.Find("Coordinate Systems/Global").GetComponent<Toggle>();
            _local = transform.Find("Coordinate Systems/Local").GetComponent<Toggle>();
            
            _handlesManager = transform.Find("../Handling").GetComponent<HandlesManager>();
            _selectionManager = transform.Find("../Selection").GetComponent<SelectionManager>();
            
            _handlesManager.ControlChanged += OnControlChanged;
            _handlesManager.CoordinateSystemChanged += OnCoordinateSystemChanged;
            _selectionManager.SelectionChanged += OnSelectionChanged;
        }

        
        private void OnSelectionChanged(Selectable selectable)
        {
            if (selectable != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = true;
            }
            else
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = false;
            }
        }
        
        private void OnCoordinateSystemChanged(HandlesManager.CoordinateSystems coordinateSystem)
        {
            _global.isOn = coordinateSystem == HandlesManager.CoordinateSystems.Global;
            _local.isOn = coordinateSystem == HandlesManager.CoordinateSystems.Local;
        }

        private void OnControlChanged(HandlesManager.Controls control)
        {
            _position.isOn = control == HandlesManager.Controls.Position;
            _rotation.isOn = control == HandlesManager.Controls.Rotation;
            _scale.isOn = control == HandlesManager.Controls.Scale;
            _global.interactable = control != HandlesManager.Controls.Scale;
        }
    }
}
