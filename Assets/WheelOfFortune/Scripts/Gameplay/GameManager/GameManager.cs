using UnityEngine;
using WheelOfFortune.Config;
using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Events;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelConfig _levelConfig;
    private IEventBus _eventBus;

    [Inject]
    public void Construct(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    void Start()
    {
        _eventBus.Publish(new OnGameStartEvent(_levelConfig.StartLevel));
    }
}
