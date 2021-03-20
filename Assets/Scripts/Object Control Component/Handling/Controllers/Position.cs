using UnityEngine;

namespace Object_Control_Component.Handling.Controllers
{
    public class Position : AbstractHandleController
    {
        public override HandlesManager.Controls Control => HandlesManager.Controls.Position;
        
        private Transform _x;
        private Material _xMaterial;
        
        private Transform _y;
        private Material _yMaterial;
        
        private Transform _z;
        private Material _zMaterial;
        
        private Transform _center;
        private Material _centerMaterial;
        
        private Vector3 _lastPoint;

        protected override void Awake()
        {
            _x = transform.Find("X");
            _xMaterial = _x.GetComponent<Renderer>().material;
            _y = transform.Find("Y");
            _yMaterial = _y.GetComponent<Renderer>().material;
            _z = transform.Find("Z");
            _zMaterial = _z.GetComponent<Renderer>().material;
            _center = transform.Find("Center");
            _centerMaterial = _center.GetComponent<Renderer>().material;
            
            base.Awake();
        }

        private void Start()
        {
            AdjustMaterial(xColor, _xMaterial);
            AdjustMaterial(yColor, _yMaterial);
            AdjustMaterial(zColor, _zMaterial);
            AdjustMaterial(centerColor, _centerMaterial);
        }
        

        protected override void AdjustHandleColor(Handle handle, Color color)
        {
            switch (handle.name)
            {
                case "X":
                    AdjustMaterial(color, _xMaterial);
                    break;
                case "Y":
                    AdjustMaterial(color, _yMaterial);
                    break;
                case "Z":
                    AdjustMaterial(color, _zMaterial);
                    break;
            }
        }
        
        protected override void AdjustHandleColors(Handle handle, Color[] colors)
        {
            switch (handle.name)
            {
                case "X":
                    AdjustMaterial(colors[0], _xMaterial);
                    break;
                case "Y":
                    AdjustMaterial(colors[1], _yMaterial);
                    break;
                case "Z":
                    AdjustMaterial(colors[2], _zMaterial);
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

            transform.localScale = Vector3.one * realScale;
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
                SelectedObject.transform.position += difference;
                transform.position += difference;
            }
            else
                FirstHandling = false;
                
            _lastPoint = currentPoint;
        }
    }
}