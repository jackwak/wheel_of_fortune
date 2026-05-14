using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Utils;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBus>().AsSingle();
        Container.Bind<IUIMover>().To<UIDoTweenMover>().AsSingle();
    }
}
