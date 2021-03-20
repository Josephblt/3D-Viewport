using UnityEngine;

namespace Axis_Control
{
    public class Axis : MonoBehaviour
    {
        private static readonly int AlphaColor = Shader.PropertyToID("_Color");
        
        public Transform trackedCamera;
    
        [Header("Axis Colors")]
        public Color xColor = new Color(1f, 0f, 0f, .75f);
        public Color yColor = new Color(0, 1f, 0f, .75f);
        public Color zColor = new Color(0f, 0f, 1f, .75f);
        public Color axisCenterColor = new Color(.5f, .5f, .5f, .75f);

        private Transform _xAxis;
        private Transform _xText;
        private Transform _yAxis;
        private Transform _yText;
        private Transform _zAxis;
        private Transform _zText;
        private Transform _axisCenter;

        private void Start()
        {
            _xAxis = transform.Find("X");
            _xText = transform.Find("X/Text");
            _yAxis = transform.Find("Y");
            _yText = transform.Find("Y/Text");
            _zAxis = transform.Find("Z");
            _zText = transform.Find("Z/Text");
            _axisCenter = transform.Find("Center");

            var xMaterial = _xAxis.GetComponent<Renderer>().material;
            var yMaterial = _yAxis.GetComponent<Renderer>().material;
            var zMaterial = _zAxis.GetComponent<Renderer>().material;
            var centerMaterial = _axisCenter.GetComponent<Renderer>().material;
            
            AdjustMaterial(xColor, xMaterial);
            AdjustMaterial(yColor, yMaterial);
            AdjustMaterial(zColor, zMaterial);
            AdjustMaterial(axisCenterColor, centerMaterial);
        }

        private void Update()
        {
            transform.rotation = Quaternion.Inverse(trackedCamera.rotation);
        
            _xText.forward = Vector3.forward;
            _yText.forward = Vector3.forward;
            _zText.forward = Vector3.forward;
        }

        private static void AdjustMaterial(Color color, Material material)
        {
            material.SetColor(AlphaColor, color);
        }
    }
}
