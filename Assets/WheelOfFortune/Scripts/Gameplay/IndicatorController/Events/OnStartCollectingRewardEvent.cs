using System;
using UnityEngine;
using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.IndicatorController.Events
{
    public struct OnStartCollectingRewardEvent
    {
        public RewardData RewardData { get; }
        public Vector3 StartPosition { get; }
        public Vector3 TargetPosition { get; }

        public OnStartCollectingRewardEvent(RewardData rewardData, Vector3 startPosition, Vector3 targetPosition)
        {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            RewardData = rewardData;
        }
    }
}