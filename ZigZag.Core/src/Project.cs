using System;
using System.Collections.Generic;
using System.Text;
using ZigZag.Core.Commands;


namespace ZigZag.Core
{
    public class Project : ProcessNode
    {
        // can do serialization

        // can do save / load

        // has command queue and stack

        public void Execute()
        {
            // do command queue
            Process();
        }

        public void Redo()
        {

        }

        public void Undo()
        {

        }

        public bool CanRedo()
        {
            return true;
        }

        public bool CanUndo()
        {
            return true;
        }

        private readonly Stack<AbstractCommand> m_doneCommands = new Stack<AbstractCommand>();
        private readonly Stack<AbstractCommand> m_undoneCommands = new Stack<AbstractCommand>();
        private readonly Queue<AbstractCommand> m_commandQueue = new Queue<AbstractCommand>();
    }
}
