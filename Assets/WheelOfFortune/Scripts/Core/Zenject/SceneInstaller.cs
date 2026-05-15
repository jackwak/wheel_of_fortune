using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Utils.UIMover;
using WheelOfFortune.Utils.RankDeterminer;
using Zenject;
using UnityEngine;
using WheelOfFortune.Config;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private RankColorPalette _numberColorPalette;

    public override void InstallBindings()
    {
        Container.Bind<IEventBus>().To<EventBus>().AsSingle();
        Container.Bind<IUIMover>().To<UIDoTweenMover>().AsSingle();
        Container.Bind<RankDeterminer>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RankColorPalette>().FromInstance(_numberColorPalette).AsSingle();
    }
}
