

namespace ZigZag.Core.Commands
{
    public abstract class AbstractCommand
    {
        internal abstract void Do();
        internal abstract void Undo();
    }
}
