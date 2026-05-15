using UnityEngine;
using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Events;
using Zenject;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    public class LevelDisplayManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelDisplayInitializer _initializer;
        [SerializeField] private LevelDisplayNumberController _numberController;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        void OnEnable()
        {
            _eventBus.Subscribe<LevelChangedEventData>(OnLevelChanged);
        }

        void OnDisable()
        {
            _eventBus.Unsubscribe<LevelChangedEventData>(OnLevelChanged);
        }

        void Start()
        {
            _initializer.Initialize(_numberController);
        }

        private void OnLevelChanged(LevelChangedEventData eventData)
        {
            _numberController.ScrollNumbers();
        }

        int level = 2;

        [ContextMenu(" Next Level ")]
        public void NextLevel()
        {
            _eventBus.Publish(new LevelChangedEventData(level));
            level++;
        }
    }
}