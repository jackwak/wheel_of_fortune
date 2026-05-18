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
            _eventBus.Subscribe<LevelChangedEvent>(OnLevelChanged);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<LevelChangedEvent>(OnLevelChanged);
        }

        void Start()
        {
            _initializer.Initialize(_numberController);
        }

        private void OnLevelChanged(LevelChangedEvent eventData)
        {
            _numberController.ScrollNumbers();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                NextLevel();
            }
        }

        int level = 2;
        public void NextLevel()
        {
            _eventBus.Publish(new LevelChangedEvent(level));
            level++;
            Debug.Log(level);
        }
    }
}