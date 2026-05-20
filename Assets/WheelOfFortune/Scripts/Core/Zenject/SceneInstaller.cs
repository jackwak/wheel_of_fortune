using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Utils.UIMover;
using WheelOfFortune.Utils.RankDeterminer;
using Zenject;
using UnityEngine;
using WheelOfFortune.Config;
using WheelOfFortune.Utils.ObjectSpinner;
using WheelOfFortune.Core.ObjectPool;
using WheelOfFortune.Gameplay.Inventory;
using WheelOfFortune.Gameplay.Revive;
using WheelOfFortune.Gameplay.Reward;
using WheelOfFortune.Gameplay.RewardMoveEffectManager;
using WheelOfFortune.Gameplay.CollectedReward;
using WheelOfFortune.Core.SaveSystem;

namespace WheelOfFortune.Core.Zenject
{
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
            Container.Bind<IRewardDataProvider>().To<ResourcesRewardDataProvider>().AsSingle();
            Container.Bind<PoolManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RewardMoveEffectManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CollectedRewardManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Inventory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ReviveService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ISaveManager>().To<SaveManager>().AsSingle();
        }
    }
}