using ZigZag.Core;


namespace ZigZag.Runtime
{
    public interface IEditor
    {
        public void OpenEditor();
        public void CloseEditor();

        public void ProjectChanged(Project project);

        public void Update();

        public bool WantsToClose();
    }
}
