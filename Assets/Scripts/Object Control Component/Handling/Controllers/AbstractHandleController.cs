using System;
using Object_Control_Component.Selection;
using UnityEngine;

namespace Object_Control_Component.Handling.Controllers
{
    public abstract class AbstractHandleController : MonoBehaviour, IHandleController
    {
        private static readonly int AlphaColor = Shader.PropertyToID("_Color");
        
        [Header("Colors")]
        public Color xColor = new Color(1f, 0f, 0f, .5f);
        public Color yColor = new Color(0f, 1f, 0f, .5f);
        public Color zColor = new Color(0f, 0f, 1f, .5f);
        public Color centerColor = new Color(1f, 0.5f, 0f, 0.5f);
        public Color highlightColor = new Color(1f, 1f, 0f, .5f);
        public Color activeColor = new Color(1f, 1f, 0f, .1f);

        [Header("Size")]
        [Range(1f, 5f)]
        public float maxScale = 3f;
        [Range(1f, 10f)] 
        public float scale = 8f;
        
        public bool Handling { get; private set; }
        public abstract HandlesManager.Controls Control { get; }
        
        protected Camera Camera;
        protected bool FirstHandling;
        protected Selectable SelectedObject;
        
        private HandlesManager _handlesManager;
        private  SelectionManager _selectionManager;
        private HandlesManager.Controls _activeControl;
        private HandlesManager.CoordinateSystems _activeCoordinateSystem;
        
        protected virtual void Awake()
        {
            _handlesManager = transform.parent.GetComponent<HandlesManager>();
            _selectionManager = transform.Find("../../Selection").GetComponent<SelectionManager>();
            
            Camera = _handlesManager.trackedCamera.GetComponent<Camera>();

            _handlesManager.ControlChanged += OnControlChanged;
            _handlesManager.CoordinateSystemChanged += OnCoordinateSystemChanged;
            
            _selectionManager.SelectionChanged += OnSelectionChanged;
        }

        protected virtual void Update()
        {
            RefreshScale();
        }
        
        
        private void OnControlChanged(HandlesManager.Controls control)
        {
            _activeControl = control;
            Refresh();
        }
        
        private void OnCoordinateSystemChanged(HandlesManager.CoordinateSystems coordinateSystem)
        {
            _activeCoordinateSystem = coordinateSystem;
            Refresh();
        }
        
        private void OnSelectionChanged(Selectable selectable)
        {
            SelectedObject = selectable;
            Refresh();
        }


        protected abstract void AdjustHandleColor(Handle handle, Color color);

        protected abstract void AdjustHandleColors(Handle handle, Color[] colors);
        
        private void Refresh()
        {
            var active = SelectedObject != null && _activeControl == Control;
            gameObject.SetActive(active);
            
            if (!active)
                return;
            
            transform.position = SelectedObject.transform.position;
            
            switch (_activeCoordinateSystem)
            {
                case HandlesManager.CoordinateSystems.Global:
                    transform.rotation = Quaternion.identity;
                    break;
                case HandlesManager.CoordinateSystems.Local:
                    transform.rotation = SelectedObject.transform.rotation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract void RefreshScale();
        

        public void StartHandling(Handle handle)
        {
            Handling = true;
            FirstHandling = true;
            AdjustHandleColor(handle, activeColor);
        }

        public abstract void ExecuteHandling(Handle handle);

        public void EndHandling(Handle handle)
        {
            Handling = false;
            FirstHandling = false;
            Dehighlight(handle);
            Refresh();
        }
        
        public void Highlight(Handle handle)
        {
            AdjustHandleColor(handle, highlightColor);
        }

        public void Dehighlight(Handle handle)
        {
            AdjustHandleColors(handle, new [] {xColor, yColor, zColor, centerColor});
        }

        
        protected static void AdjustMaterial(Color color, Material material)
        {
            material.SetColor(AlphaColor, color);
        }
        
        protected static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, 
            out Vector3 closestPointLine2,
            Vector3 linePoint1, 
            Vector3 lineVec1, 
            Vector3 linePoint2, 
            Vector3 lineVec2)
        {
            closestPointLine1 = Vector3.zero;
            closestPointLine2 = Vector3.zero;

            var a = Vector3.Dot(lineVec1, lineVec1);
            var b = Vector3.Dot(lineVec1, lineVec2);
            var e = Vector3.Dot(lineVec2, lineVec2);

            var d = a * e - b * b;

            if (d == 0.0f) return false;
            
            var r = linePoint1 - linePoint2;
            var c = Vector3.Dot(lineVec1, r);
            var f = Vector3.Dot(lineVec2, r);

            var s = (b * f - c * e) / d;
            var t = (a * f - c * b) / d;

            closestPointLine1 = linePoint1 + lineVec1 * s;
            closestPointLine2 = linePoint2 + lineVec2 * t;

            return true;
        }
    }
}