using UnityEngine;
using UnityEngine.EventSystems;

namespace View_Cube_Component
{
    public class ViewTarget : MonoBehaviour
    {
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        private static readonly int ObjectColor = Shader.PropertyToID("_Color");
        private static readonly int WireframeColor = Shader.PropertyToID("_WireframeColor");
        private static readonly int WireframeSmooth = Shader.PropertyToID("_WireframeSmooth");
        private static readonly int WireframeWidth = Shader.PropertyToID("_WireframeWidth");
        
        public Vector3 targetRotation;
        
        private bool _highlighted;
        private Material _material;
        private Transform _trackedCamera;
        private ViewCube _viewCube;

        private void Start()
        {
            var viewCube = transform.Find("../../../View Cube");
            _viewCube = viewCube.GetComponent<ViewCube>();
        
            _trackedCamera = _viewCube.trackedCamera;
            
            _material = GetComponent<Renderer>().material;
            _material.renderQueue = 3100;
            _material.SetFloat(Alpha, _viewCube.fadeAlpha);
            _material.SetColor(ObjectColor, Color.clear);
            _material.SetColor(WireframeColor, Color.clear);
            _material.SetFloat(WireframeSmooth, _viewCube.wireframeSmoothness);
            _material.SetFloat(WireframeWidth, _viewCube.wireframeWidth);
        }

        private void Update()
        {
            _material.SetFloat(Alpha, _viewCube.Enhanced ? _viewCube.enhanceAlpha : _viewCube.fadeAlpha);

            if (_highlighted)
                Highlight();
            else if (IsSelected())
                Select();
            else
                Hide();
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _viewCube.Fade();
                _highlighted = false;
                return;
            }
            
            _viewCube.StartRotation(targetRotation);
        }

        private void OnMouseExit()
        {
            _viewCube.Fade();
            _highlighted = false;
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _viewCube.Fade();
                _highlighted = false;
                return;
            }
            
            _viewCube.Enhance();
            _highlighted = true;
        }
        
        
        private void Hide()
        {
            _material.SetColor(ObjectColor, Color.clear);
            _material.SetColor(WireframeColor, Color.clear);
        }
        
        private void Highlight()
        {
            _material.SetColor(ObjectColor, _viewCube.targetHighlightColor);
            _material.SetColor(WireframeColor, _viewCube.targetWireframeHighlightColor);
        }

        private void Select()
        {
            _material.SetColor(ObjectColor, _viewCube.targetSelectedColor);
            _material.SetColor(WireframeColor, _viewCube.targetWireframeSelectedColor);
        }
        
        private bool IsSelected()
        {
            var eulerAngles = _trackedCamera.eulerAngles;
            var position = _trackedCamera.position;
            var targetPosition = Quaternion.Euler(targetRotation) * (Vector3.back * _viewCube.distance);
            
            return Approximately(ClampAngle(targetRotation.x), ClampAngle(eulerAngles.x)) &&
                   Approximately(ClampAngle(targetRotation.y), ClampAngle(eulerAngles.y)) &&
                   Approximately(ClampAngle(targetRotation.z), ClampAngle(eulerAngles.z)) &&
                   Approximately(targetPosition.x, position.x) &&
                   Approximately(targetPosition.y, position.y) &&
                   Approximately(targetPosition.z, position.z);
        }
        
        
        private static float ClampAngle(float angle)
        {
            var clampedAngle = angle % 360f;
            if (clampedAngle < 0f)
                clampedAngle += 360f;
            if (clampedAngle >= 360f)
                 clampedAngle = 0f;
            return clampedAngle;
        }

        private static bool Approximately(float a, float b, float threshold = 0.001f)
        {
            return Mathf.Abs(a- b) <= threshold;
        }
    }
}