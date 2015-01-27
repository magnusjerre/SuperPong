using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork.Interfaces
{
    public interface MJTransitionListener
    {
        void NotifyTransitionEnded(MJTransition transition);
    }
}
