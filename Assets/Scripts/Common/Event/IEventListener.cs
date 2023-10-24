namespace Common.Event
{
    public interface IEvent
    {
    }



    public interface IEventListener
    {
    }
    
    public interface IEventListener<in T> : IEventListener where T : IEvent
    {
        public void OnEventTriggered(T evn);
    }

}