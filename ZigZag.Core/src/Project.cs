﻿using System;
using System.Collections.Generic;
using System.Text;
using ZigZag.Core.Commands;


namespace ZigZag.Core
{
    public class Project : GroupNode
    {
        // can do serialization

        // can do save / load

        public void Execute()
        {
            // Copy command queue to local variable, and create new command
            // queue to accept incoming commands, in case more commands are
            // generated by executing the current commands. 
            var commandQueue = m_commandQueue;
            m_commandQueue = new Queue<AbstractCommand>();

            while (commandQueue.Count > 0)
            {
                var command = commandQueue.Dequeue();

                if (command is UndoCommand)
                {
                    if (m_doneCommands.Count > 0)
                    {
                        m_undoneCommands.Push(m_doneCommands.Pop());
                        m_undoneCommands.Peek().Undo();
                    }
                }
                else if (command is RedoCommand)
                {
                    if (m_undoneCommands.Count > 0)
                    {
                        m_doneCommands.Push(m_undoneCommands.Pop());
                        m_doneCommands.Peek().Do();
                    }
                }
                else
                {
                    try
                    {
                        command.Do();
                    }
                    catch(CommandException)
                    {
                        // Ignore command do nothing, continue with next command.
                        continue;
                    }
                    m_doneCommands.Push(command);
                    m_undoneCommands.Clear();
                }
            }

            Process();
        }

        public void Redo()
        {
            m_commandQueue.Enqueue(new RedoCommand());
        }

        public void Undo()
        {
            m_commandQueue.Enqueue(new UndoCommand());
        }

        public void SubmitCommand(AbstractCommand command)
        {
            if (!(command is null))
            {
                m_commandQueue.Enqueue(command);
            }
        }

        private readonly Stack<AbstractCommand> m_doneCommands = new Stack<AbstractCommand>();
        private readonly Stack<AbstractCommand> m_undoneCommands = new Stack<AbstractCommand>();
        private Queue<AbstractCommand> m_commandQueue = new Queue<AbstractCommand>();
    }
}
