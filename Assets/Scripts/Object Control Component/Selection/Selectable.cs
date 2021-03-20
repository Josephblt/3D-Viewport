using UnityEngine;
using UnityEngine.EventSystems;

namespace Object_Control_Component.Selection
{
    public class Selectable : MonoBehaviour
    {
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        private static readonly int ObjectColor = Shader.PropertyToID("_Color");
        private static readonly int WireframeColor = Shader.PropertyToID("_WireframeColor");
        private static readonly int WireframeSmooth = Shader.PropertyToID("_WireframeSmooth");
        private static readonly int WireframeWidth = Shader.PropertyToID("_WireframeWidth");

        public SelectionManager selectionManager;
        
        [Header("Wireframe Properties")]
        public Color highlightColor = new Color(1f, .75f, .5f, .1f);
        public Color selectedColor = new Color(1f, .5f, .5f, .1f);
        [Range(0, 10)] public float smoothness = 2f;
        [Range(0, 10)] public float thickness = 2f;

        private Material _material;
        private bool _selected;

        private void Awake()
        {
            _material = GetComponent<Renderer>().materials[1];
            _material.SetFloat(Alpha, 1f);
            _material.SetColor(ObjectColor, Color.clear);
            _material.SetColor(WireframeColor, Color.clear);
            _material.SetFloat(WireframeSmooth, smoothness);
            _material.SetFloat(WireframeWidth, thickness);
            
            selectionManager.SelectionChanged += OnSelectionChanged;
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            selectionManager.Select(this);
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Dehighlight();
                return;
            }

            Highlight();
        }

        private void OnMouseExit()
        {
            Dehighlight();
        }

        
        private void OnSelectionChanged(Selectable selectable)
        {
            if (selectable == this)
            {
                _selected = true;
                _material.SetColor(WireframeColor, selectedColor);
            }
            else
            {
                _selected = false;
                _material.SetColor(WireframeColor, Color.clear);
            }

        }


        private void Highlight()
        {
            if (!_selected)
                _material.SetColor(WireframeColor, highlightColor);
        }
        
        private void Dehighlight()
        {
            if (!_selected)
                _material.SetColor(WireframeColor, Color.clear);
        } 
    }
}