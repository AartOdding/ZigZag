

namespace ZigZag.Runtime
{
    public interface IEditor
    {
        public void OpenEditor();
        public void CloseEditor();

        public void Update();

        public bool WantsToClose();
    }
}
