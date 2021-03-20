using UnityEngine;
using UnityEngine.EventSystems;

namespace View_Cube_Component
{
    public class ViewCube : MonoBehaviour
    {
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        private static readonly int ObjectColor = Shader.PropertyToID("_Color");
        private static readonly int WireframeColor = Shader.PropertyToID("_WireframeColor");
        private static readonly int WireframeSmooth = Shader.PropertyToID("_WireframeSmooth");
        private static readonly int WireframeWidth = Shader.PropertyToID("_WireframeWidth");

        public Transform trackedCamera;

        [Header("Cube Properties")]
        public Color cubeColor = new Color(.5f, .5f, .5f);
        public Color cubeWireframeColor = new Color(1f, 1f, 1f);
        
        [Header("Target Properties")]
        public Color targetHighlightColor = new Color(.1f, .1f, .1f);
        public Color targetWireframeHighlightColor = new Color(1f, 1f, 0f);
        public Color targetSelectedColor = new Color(.25f, .25f, .25f);
        public Color targetWireframeSelectedColor = new Color(1f, 1f, 1f);
        

        [Header("Common Properties")]
        [Range(0, 1)]
        public float enhanceAlpha = .75f;
        [Range(0, 1)]
        public float fadeAlpha = .25f;
        [Range(0, 10)]
        public float wireframeSmoothness = 1f;
        [Range(0, 10)]
        public float wireframeWidth = 1f;

        [Header("View Properties")] 
        public ViewTarget defaultTarget;
        [Range(0, 1000)]
        public float distance = 700f;
        [Range(0.01f, 5f)]
        public float speed = .25f;
        
        public bool Enhanced { get; private set; }
        
        private Vector3 _fromPosition;
        private Quaternion _fromRotation;
        private Material _material;
        private bool _rotating;
        private float _rotationLerp;
        private Vector3 _targetRotation;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _material.SetFloat(Alpha, fadeAlpha);
            _material.SetColor(ObjectColor, cubeColor);
            _material.SetColor(WireframeColor, cubeWireframeColor);
            _material.SetFloat(WireframeSmooth, wireframeSmoothness);
            _material.SetFloat(WireframeWidth, wireframeWidth);

            StartRotation(defaultTarget.targetRotation);
        }

        private void Update()
        {
            transform.rotation = Quaternion.Inverse(trackedCamera.rotation);
            
            if(_rotating)
                RotateToTarget();
        }

        private void OnMouseExit()
        {
            Fade();
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Fade();
                return;
            }
            
            Enhance();
        }

        
        private void RotateToTarget()
        {
            _rotationLerp += Time.deltaTime / speed;
        
            var toRotation = Quaternion.Euler(_targetRotation);
            var toPosition = toRotation * (Vector3.back * distance);
        
            trackedCamera.rotation = Quaternion.Lerp(_fromRotation, toRotation, _rotationLerp);
            trackedCamera.position = Vector3.Lerp(_fromPosition, toPosition, _rotationLerp);

            _rotating = _rotationLerp < 1f;
        }
        
    
        public void Enhance()
        {
            Enhanced = true;
            _material.SetFloat(Alpha, enhanceAlpha);
        }

        public void Fade()
        {
            Enhanced = false;
            _material.SetFloat(Alpha, fadeAlpha);
        }
        
        public void StartRotation(Vector3 targetRotation)
        {
            _targetRotation = targetRotation;
            _fromPosition = trackedCamera.position;
            _fromRotation = trackedCamera.rotation;
            _rotating = true;
            _rotationLerp = 0;
        }
    }
}
