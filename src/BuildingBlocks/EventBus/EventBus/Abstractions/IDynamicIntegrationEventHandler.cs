using System.Threading.Tasks;

namespace DefaultNamespace
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}