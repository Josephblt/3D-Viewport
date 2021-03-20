using UnityEngine;

namespace Object_Control_Component.Handling.Controllers
{
    public class Scale : AbstractHandleController
    {
        public override HandlesManager.Controls Control => HandlesManager.Controls.Scale;

        private Transform _xAxis;
        private Transform _xCube;
        private Material _xAxisMaterial;
        private Material _xCubeMaterial;
        
        private Transform _yAxis;
        private Transform _yCube;
        private Material _yAxisMaterial;
        private Material _yCubeMaterial;
        
        private Transform _zAxis;
        private Transform _zCube;
        private Material _zAxisMaterial;
        private Material _zCubeMaterial;
        
        private Transform _center;
        private Material _centerMaterial;
        
        private Vector3 _lastPoint;
        
        protected override void Awake()
        {
            _xAxis = transform.Find("X/Axis");
            _xAxisMaterial = _xAxis.GetComponent<Renderer>().material;
            _xCube = transform.Find("X/Cube");
            _xCubeMaterial = _xCube.GetComponent<Renderer>().material;
            
            _yAxis = transform.Find("Y/Axis");
            _yAxisMaterial = _yAxis.GetComponent<Renderer>().material;
            _yCube = transform.Find("Y/Cube");
            _yCubeMaterial = _yCube.GetComponent<Renderer>().material;
            
            _zAxis = transform.Find("Z/Axis");
            _zAxisMaterial = _zAxis.GetComponent<Renderer>().material;
            _zCube = transform.Find("Z/Cube");
            _zCubeMaterial = _zCube.GetComponent<Renderer>().material;
            
            _center = transform.Find("Center");
            _centerMaterial = _center.GetComponent<Renderer>().material;
            base.Awake();
        }

        private void Start()
        {
            AdjustMaterial(xColor, _xAxisMaterial);
            AdjustMaterial(xColor, _xCubeMaterial);
            AdjustMaterial(yColor, _yAxisMaterial);
            AdjustMaterial(yColor, _yCubeMaterial);
            AdjustMaterial(zColor, _zAxisMaterial);
            AdjustMaterial(zColor, _zCubeMaterial);
            AdjustMaterial(centerColor, _centerMaterial);
        }

        
        protected override void AdjustHandleColor(Handle handle, Color color)
        {
            switch (handle.name)
            {
                case "X":
                    AdjustMaterial(color, _xAxisMaterial);
                    AdjustMaterial(color, _xCubeMaterial);
                    break;
                case "Y":
                    AdjustMaterial(color, _yAxisMaterial);
                    AdjustMaterial(color, _yCubeMaterial);
                    break;
                case "Z":
                    AdjustMaterial(color, _zAxisMaterial);
                    AdjustMaterial(color, _zCubeMaterial);
                    break;
            }
        }
        
        protected override void AdjustHandleColors(Handle handle, Color[] colors)
        {
            switch (handle.name)
            {
                case "X":
                    AdjustMaterial(colors[0], _xAxisMaterial);
                    AdjustMaterial(colors[0], _xCubeMaterial);
                    break;
                case "Y":
                    AdjustMaterial(colors[1], _yAxisMaterial);
                    AdjustMaterial(colors[1], _yCubeMaterial);
                    break;
                case "Z":
                    AdjustMaterial(colors[2], _zAxisMaterial);
                    AdjustMaterial(colors[2], _zCubeMaterial);
                    break;
            }
        }
        
        protected override void RefreshScale()
        {
            var difference = (Camera.transform.position - SelectedObject.transform.position).magnitude;
            
            var realScale = scale - 1f;
            realScale *= 100f;
            realScale = 999f - realScale;
            realScale = difference / realScale;
            
            if (realScale > maxScale)
                realScale = maxScale;
            
            var selectedTransform = SelectedObject.transform;
            
            var mesh = SelectedObject.GetComponent<MeshFilter>().mesh;
            var meshSize = Vector3.one;
            if (mesh != null)
                meshSize = mesh.bounds.size;
            var objSize = selectedTransform.localScale;
            
            var size = Vector3.Scale(meshSize, objSize);
            
            _xCube.localPosition = Vector3.up * size.x / 2f;
            _xCube.localScale = Vector3.one * realScale * 3f;
            _xAxis.localPosition = Vector3.up * size.x / 4f;
            _xAxis.localScale = new Vector3(2f * realScale, size.x / 4f, 2f * realScale);
            
            _yCube.localPosition = Vector3.up * size.y / 2f;
            _yCube.localScale = Vector3.one * realScale * 3f;
            _yAxis.localPosition = Vector3.up * size.y / 4f;
            _yAxis.localScale = new Vector3(2f * realScale, size.y / 4f, 2f * realScale);
            
            _zCube.localPosition = Vector3.up * size.z / 2f;
            _zCube.localScale = Vector3.one * realScale * 3f;
            _zAxis.localPosition = Vector3.up * size.z / 4f;
            _zAxis.localScale = new Vector3(2f * realScale, size.z / 4f, 2f * realScale);

            _center.localScale = Vector3.one * 5f * realScale;
        }
        

        public override void ExecuteHandling(Handle handle)
        {
            var handleTransform = handle.transform;
            var axisRay = new Ray(handleTransform.position, handleTransform.up);
            var mouseRay = Camera.ScreenPointToRay(Input.mousePosition);
            
            if (!ClosestPointsOnTwoLines(
                out _,
                out var currentPoint,
                mouseRay.origin,
                mouseRay.direction,
                axisRay.origin,
                axisRay.direction)) return;

            if (!FirstHandling)
            {
                var difference = currentPoint - _lastPoint;
                difference = Quaternion.Inverse(SelectedObject.transform.rotation) * difference;

                var mesh = SelectedObject.GetComponent<MeshFilter>().mesh;
                var meshSize = Vector3.one;
                if (mesh != null)
                    meshSize = mesh.bounds.size;

                difference = new Vector3(
                    difference.x / meshSize.x,
                    difference.y / meshSize.y,
                    difference.z / meshSize.z);

                SelectedObject.transform.localScale += difference * 2f;
            }
            else
                FirstHandling = false;
                
            _lastPoint = currentPoint;
        }
    }
}