using UnityEngine;

namespace Object_Control_Component.Handling.Controllers
{
    public class Rotation : AbstractHandleController
    {
        public override HandlesManager.Controls Control => HandlesManager.Controls.Rotation;
        
        private Transform _tilt;
        private Material _tiltMaterial;
        
        private Transform _xy;
        private Transform _xyCulling;
        private Material _xyMaterial;
        
        private Transform _yz;
        private Transform _yzCulling;
        private Material _yzMaterial;
        
        private Transform _zx;
        private Transform _zxCulling;
        private Material _zxMaterial;
        
        private Vector3 _lastPoint;

        protected override void Awake()
        {
            _tilt = transform.Find("Tilt");
            _tiltMaterial = _tilt.GetComponent<Renderer>().material;
            _xy = transform.Find("XY");
            _xyCulling = transform.Find("XY/Culling");
            _xyMaterial = _xy.GetComponent<Renderer>().material;
            _yz = transform.Find("YZ");
            _yzCulling = transform.Find("YZ/Culling");
            _yzMaterial = _yz.GetComponent<Renderer>().material;
            _zx = transform.Find("ZX");
            _zxCulling = transform.Find("ZX/Culling");
            _zxMaterial = _zx.GetComponent<Renderer>().material;

            base.Awake();
        }

        private void Start()
        {
            AdjustMaterial(xColor, _xyMaterial);
            AdjustMaterial(Color.clear, _xyCulling.GetComponent<Renderer>().material);
            AdjustMaterial(yColor, _yzMaterial);
            AdjustMaterial(Color.clear, _yzCulling.GetComponent<Renderer>().material);
            AdjustMaterial(zColor, _zxMaterial);
            AdjustMaterial(Color.clear, _zxCulling.GetComponent<Renderer>().material);
            AdjustMaterial(centerColor, _tiltMaterial);
        }

        protected override void Update()
        {
            RefreshTilt();
            base.Update();
        }
        
        
        protected override void AdjustHandleColor(Handle handle, Color color)
        {
            switch (handle.name)
            {
                case "Tilt":
                    AdjustMaterial(color, _tiltMaterial);
                    break;
                case "XY":
                    AdjustMaterial(color, _xyMaterial);
                    break;
                case "YZ":
                    AdjustMaterial(color, _yzMaterial);
                    break;
                case "ZX":
                    AdjustMaterial(color, _zxMaterial);
                    break;
            }
        }
        
        protected override void AdjustHandleColors(Handle handle, Color[] colors)
        {
            switch (handle.name)
            {
                case "XY":
                    AdjustMaterial(colors[0], _xyMaterial);
                    break;
                case "YZ":
                    AdjustMaterial(colors[1], _yzMaterial);
                    break;
                case "ZX":
                    AdjustMaterial(colors[2], _zxMaterial);
                    break;
                case "Tilt":
                    AdjustMaterial(colors[3], _tiltMaterial);
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
        
        
        private void RefreshTilt()
        {
            _tilt.transform.up = Camera.transform.forward;
        }
        
        
        public override void ExecuteHandling(Handle handle)
        {
            var plane = new Plane(handle.transform.up, transform.position);
            var mouseRay = Camera.ScreenPointToRay(Input.mousePosition);

            if (!plane.Raycast(mouseRay, out var distance)) return;
            
            if (!FirstHandling)
            {
                var sTransform = SelectedObject.transform;
                    
                var tTransform = transform;
                var tPosition = tTransform.position;
                var tRotation = tTransform.rotation;
                    
                var point = mouseRay.GetPoint(distance);
                var direction1 = _lastPoint - tPosition;
                var direction2 = point - tPosition;
                
                var angle = Vector3.Angle(direction1, direction2);
                var axis = Vector3.Cross(direction1, direction2);

                var difference = Quaternion.AngleAxis(angle, axis);
                    
                sTransform.rotation = difference * sTransform.rotation;
                tTransform.rotation = Quaternion.AngleAxis(angle, axis) * tRotation;
            }
            else
                FirstHandling = false;

            _lastPoint = mouseRay.GetPoint(distance);
        }
    }
}