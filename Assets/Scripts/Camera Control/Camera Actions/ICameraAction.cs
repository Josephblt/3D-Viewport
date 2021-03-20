namespace Camera_Control.Camera_Actions
{
    public interface ICameraAction
    {
        CameraController.CameraAction CameraAction { get; }

        void StartAction();

        void UpdateAction();
    }
}