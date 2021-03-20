namespace Object_Control_Component.Handling.Controllers
{
    public interface IHandleController
    {
        bool Handling { get; }
        
        void StartHandling(Handle handle);

        void ExecuteHandling(Handle handle);
        
        void EndHandling(Handle handle);
        
        void Highlight(Handle handle);
        
        void Dehighlight(Handle handle);
    }
}