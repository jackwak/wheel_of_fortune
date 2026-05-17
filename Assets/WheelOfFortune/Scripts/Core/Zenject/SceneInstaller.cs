using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Utils.UIMover;
using WheelOfFortune.Utils.RankDeterminer;
using Zenject;
using UnityEngine;
using WheelOfFortune.Config;
using WheelOfFortune.Utils.ObjectSpinner;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private RankColorPalette _numberColorPalette;

    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBus>().AsSingle();
        Container.Bind<IUIMover>().To<UIDoTweenMover>().AsSingle();
        Container.Bind<RankDeterminer>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RankColorPalette>().FromInstance(_numberColorPalette).AsSingle();
        Container.Bind<IObjectSpinner>().To<DoTweenObjectSpinner>().AsTransient();
    }
}
