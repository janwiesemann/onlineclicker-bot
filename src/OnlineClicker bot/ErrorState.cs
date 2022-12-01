using System;

namespace OnlineClicker_bot
{
    internal class ErrorState : StateBase
    {
        public ErrorState(Exception ex)
        {
            Exception = ex;
        }

        public ErrorState() : this(new Exception())
        { }

        public Exception Exception { get; }
    }
}