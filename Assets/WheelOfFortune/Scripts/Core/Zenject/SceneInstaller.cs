using WheelOfFortune.Core.EventBus;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBus>().AsSingle();
    }
}
