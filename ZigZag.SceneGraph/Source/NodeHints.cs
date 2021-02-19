using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigZag.SceneGraph
{
    [Flags]
    public enum NodeHints
    {
        None = 0,
        HasProtrudingChildren = 1 << 0
    }
}
