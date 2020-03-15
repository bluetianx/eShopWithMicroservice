using EventBus.Events;

namespace EventBus.Test.Event
{
    public class TestEvent:IntegrationEvent
    {
        public  string Msg { get; set; }
    }
}