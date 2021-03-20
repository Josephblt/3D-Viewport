using UnityEngine;

namespace Object_Control_Component.Selection
{
    public class SelectionManager : MonoBehaviour
    {
        public delegate void SelectionChangedHandler(Selectable selectable);
        
        public event SelectionChangedHandler SelectionChanged;
        
        private Selectable _selectedObject;

        private void Start()
        {
            Select(null);
        }

        
        public void Select(Selectable selectable)
        {
            _selectedObject = _selectedObject == selectable ? null : selectable;
            SelectionChanged?.Invoke(_selectedObject);
        }
    }
}