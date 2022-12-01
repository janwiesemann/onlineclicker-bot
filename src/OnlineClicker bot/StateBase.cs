namespace OnlineClicker_bot
{
    internal abstract class StateBase : NotifyBase
    {
        public virtual bool IsLoading { get; }

        public virtual void Start()
        { }

        public virtual void Stop()
        { }
    }
}