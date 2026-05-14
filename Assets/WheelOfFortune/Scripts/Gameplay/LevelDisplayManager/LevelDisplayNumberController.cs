using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Gameplay.LevelDisplayManager;
using WheelOfFortune.Utils;
using Zenject;

public class LevelDisplayNumberController : MonoBehaviour
{
    [Header("References")]
    private IUIMover _uiMover;
    private LevelDisplayNumberBehaviour[] _levelNumbers;

    [Header("Config")]
    [SerializeField] private LevelDisplayNumberConfig _numberConfig;

    [Inject]
    public void Construct(IUIMover uiMover)
    {
        _uiMover = uiMover;
    }

    public void Initialize(LevelDisplayNumberBehaviour[] levelNumbers, float stepSize = 80f, float moveDuration = 1f)
    {
        _levelNumbers = levelNumbers;
    }

    [ContextMenu("Test Scroll")]
    public void ScrollNumbers()
    {
        RectTransform[] rectTransforms = new RectTransform[_levelNumbers.Length];
        for (int i = 0; i < _levelNumbers.Length; i++)
        {
            rectTransforms[i] = _levelNumbers[i].transform as RectTransform;
        }

        _uiMover.MoveAll(rectTransforms, new Vector2(-_numberConfig.StepSize, 0), _numberConfig.ScrollDuration);
    }
}
