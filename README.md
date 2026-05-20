# Wheel of Fortune — Unity Case Study

A small Unity case project: spin a configurable prize wheel, collect rewards, dodge bombs, fail and revive, and progress through Bronze → Silver → Gold rank tiers.

The codebase was written with five concrete improvements over a previous case submission in mind. They are described — with file references — in [Addressing the previous feedback](#addressing-the-previous-feedback) below.

---

## Tech Stack

| Area | Choice |
|------|--------|
| Engine | Unity **2021.3.45f2** |
| Dependency Injection | **Zenject** (`SceneInstaller` MonoInstaller) |
| Async | **UniTask** 2.1.0 |
| Tweening | **DOTween** (wheel spin, UI movement, reward effects) |
| UI | **uGUI** + **TextMeshPro** 3.0.6 |
| Persistence | `PlayerPrefs` + `JsonUtility` (generic `ISaveManager`) |
| Timeline | `com.unity.timeline` 1.6.5 |

Full package list: [Packages/manifest.json](Packages/manifest.json).

---

## Architecture at a Glance

Feature-first layout under [Assets/WheelOfFortune/Scripts/](Assets/WheelOfFortune/Scripts/):

```
Scripts/
├── Core/          EventBus, ObjectPool, SaveSystem, Zenject installer
├── Config/        Global ScriptableObject configs
├── Enums/         Rank enum and friends
├── Events/        Global game events (struct-based)
├── Gameplay/      One folder per feature module
└── Utils/         BaseButton, ObjectSpinner, RankDeterminer, UIMover
```

Cross-cutting concerns (DI, events, pooling, save) live in `Core/`. Each gameplay feature is a self-contained folder under `Gameplay/` with its own local `Events/`, `States/`, and `Config/` subfolders where applicable.

---

## Addressing the Previous Feedback

The five items below are direct responses to the feedback received on the earlier case submission. Each one points at the actual code that implements the fix.

### 1. No magic numbers — everything is a ScriptableObject

Every tunable value (spin duration, ease curve, wheel slice count, bomb probability, indicator timings, revive cost, level rank thresholds, reward animation parameters) is exposed on a `[CreateAssetMenu]` ScriptableObject. Nothing is hardcoded inside MonoBehaviours.

Representative config types:

- [SpinnerConfig.cs](Assets/WheelOfFortune/Scripts/Utils/ObjectSpinner/SpinnerConfig.cs) — spin duration, ease, direction
- [WheelConfig.cs](Assets/WheelOfFortune/Scripts/Gameplay/Wheel/WheelConfig.cs) — slice count, radius, bomb distribution
- [IndicatorControllerConfig.cs](Assets/WheelOfFortune/Scripts/Gameplay/IndicatorController/IndicatorControllerConfig.cs)
- [RewardMoveEffectConfig.cs](Assets/WheelOfFortune/Scripts/Gameplay/RewardMoveEffectManager/RewardMoveEffectConfig.cs)
- [LevelDisplayNumberConfig.cs](Assets/WheelOfFortune/Scripts/Gameplay/LevelDisplayManager/Config/LevelDisplayNumberConfig.cs)
- [ReviveCurrencyConfig.cs](Assets/WheelOfFortune/Scripts/Gameplay/Revive/ReviveCurrencyConfig.cs)
- [LevelConfig.cs](Assets/WheelOfFortune/Scripts/Config/LevelConfig.cs) / [LevelRankConfig.cs](Assets/WheelOfFortune/Scripts/Config/LevelRankConfig.cs)
- [WheelRankVisualsConfig.cs](Assets/WheelOfFortune/Scripts/Gameplay/Wheel/RankVisuals/WheelRankVisualsConfig.cs)
- [RankColorPalette.cs](Assets/WheelOfFortune/Scripts/Config/RankColorPalette.cs)

Authored assets live in [Assets/WheelOfFortune/ScriptableObjects/](Assets/WheelOfFortune/ScriptableObjects/) — designers / reviewers can re-balance the game without touching code.

### 2. State system — explicit FSMs

The State pattern is used in three places, each with an interface + concrete state classes that implement `Enter` / `Exit` lifecycle hooks.

- **Spin button states** — [ISpinButtonState.cs](Assets/WheelOfFortune/Scripts/Gameplay/SpinController/States/ISpinButtonState.cs), [InteractableState.cs](Assets/WheelOfFortune/Scripts/Gameplay/SpinController/States/InteractableState.cs), [NonInteractableState.cs](Assets/WheelOfFortune/Scripts/Gameplay/SpinController/States/NonInteractableState.cs)
- **Exit button states** — [IExitButtonState.cs](Assets/WheelOfFortune/Scripts/Gameplay/Exit/States/IExitButtonState.cs), [InteractableState.cs](Assets/WheelOfFortune/Scripts/Gameplay/Exit/States/InteractableState.cs), [NonInteractableState.cs](Assets/WheelOfFortune/Scripts/Gameplay/Exit/States/NonInteractableState.cs)
- **Wheel rank visuals** — [IWheelRankState.cs](Assets/WheelOfFortune/Scripts/Gameplay/Wheel/RankVisuals/IWheelRankState.cs), [BronzeWheelRankState.cs](Assets/WheelOfFortune/Scripts/Gameplay/Wheel/RankVisuals/BronzeWheelRankState.cs), [SilverWheelRankState.cs](Assets/WheelOfFortune/Scripts/Gameplay/Wheel/RankVisuals/SilverWheelRankState.cs), [GoldWheelRankState.cs](Assets/WheelOfFortune/Scripts/Gameplay/Wheel/RankVisuals/GoldWheelRankState.cs)

State transitions are triggered by the event bus, never by checking strings or ints.

### 3. Event manager / event bus

A custom generic event bus replaces direct MonoBehaviour-to-MonoBehaviour references. Events are `struct` types (allocation-free) and the bus is `Subscribe<T> / Publish<T> / UnSubscribe<T>`.

- [IEventBus.cs](Assets/WheelOfFortune/Scripts/Core/EventBus/IEventBus.cs)
- [EventBus.cs](Assets/WheelOfFortune/Scripts/Core/EventBus/EventBus.cs)

Events are grouped by ownership: global events live in [Assets/WheelOfFortune/Scripts/Events/](Assets/WheelOfFortune/Scripts/Events/), and feature-local events live next to the feature that owns them:

- `Gameplay/SpinController/Events/` — `SpinButtonClickedEvent`, `OnSpinCompleteEvent`
- `Gameplay/IndicatorController/Events/` — `OnCollectBombEvent`, `OnCollectedRewardEvent`, `OnStartCollectingRewardEvent`
- `Gameplay/Inventory/Events/` — `OnInventoryChangedEvent`
- `Gameplay/Exit/Events/` — `OnExitButtonClickedEvent`, `OnExitGameEvent`
- `Gameplay/Revive/Events/` — `OnReviveRequestedEvent`
- `Gameplay/LevelFlow/Events/` — [OnLevelFailedEvent.cs](Assets/WheelOfFortune/Scripts/Gameplay/LevelFlow/Events/OnLevelFailedEvent.cs), `OnGiveUpRequestedEvent`

The bus is bound as `AsSingle` in the installer, so every system gets the same instance via constructor / field injection.

### 4. Object pooling

A generic pool manager backed by `Queue<GameObject>`. Pre-warm entries are configured on the manager component; unknown prefabs auto-create their pool on first `Get` call.

- [PoolManager.cs](Assets/WheelOfFortune/Scripts/Core/ObjectPool/PoolManager.cs)
- [ObjectPool.cs](Assets/WheelOfFortune/Scripts/Core/ObjectPool/ObjectPool.cs)
- [PoolEntry.cs](Assets/WheelOfFortune/Scripts/Core/ObjectPool/PoolEntry.cs)

API: `Get(prefab, position, rotation)` / `Get(prefab, parent, …)` / `Release(prefab, obj)`. The reward fly-to-inventory effect is the main consumer — see [RewardMoveEffectManager.cs](Assets/WheelOfFortune/Scripts/Gameplay/RewardMoveEffectManager/RewardMoveEffectManager.cs), which spawns one effect per collected reward and returns it to the pool when the tween completes.

### 5. All systems are configurable

Every system reads its behaviour from a ScriptableObject injected via Zenject or assigned on its component, so there is no recompile loop when balancing. Combined with item 1, this means:

- Wheel layout (slice count, bomb count, slice angle) → `WheelConfig`
- Spin feel (duration, ease, full rotations) → `SpinnerConfig`
- Reward animation (curve, duration, scale) → `RewardMoveEffectConfig`
- Indicator timing / thresholds → `IndicatorControllerConfig`
- Level progression and rank thresholds → `LevelConfig`, `LevelRankConfig`
- Rank visuals & palette → `WheelRankVisualsConfig`, `RankColorPalette`
- Revive cost / currency → `ReviveCurrencyConfig`
- Level number display feel → `LevelDisplayNumberConfig`

A designer can re-tune any of these from the Inspector at [Assets/WheelOfFortune/ScriptableObjects/](Assets/WheelOfFortune/ScriptableObjects/) without opening a `.cs` file.

---

## Dependency Injection Map

All bindings are declared in one place: [SceneInstaller.cs](Assets/WheelOfFortune/Scripts/Core/Zenject/SceneInstaller.cs).

| Interface | Implementation | Lifetime |
|-----------|----------------|----------|
| `IEventBus` | `EventBus` | Single |
| `IUIMover` | `UIDoTweenMover` | Single |
| `IObjectSpinner` | `DoTweenObjectSpinner` | Transient |
| `IRewardDataProvider` | `ResourcesRewardDataProvider` | Single |
| `ISaveManager` | `SaveManager` | Single |
| `RankDeterminer` | — | FromComponentInHierarchy, Single |
| `RankColorPalette` | — | FromInstance, Single |
| `PoolManager` | — | FromComponentInHierarchy, Single |
| `RewardMoveEffectManager`, `CollectedRewardManager`, `Inventory`, `ReviveService` | concrete | Single |

---

## Project Structure

```
Assets/WheelOfFortune/
├── Scenes/GameScene.unity
├── ScriptableObjects/        Authored config assets
└── Scripts/
    ├── Config/               Global configs (LevelConfig, RankColorPalette, …)
    ├── Core/
    │   ├── EventBus/         IEventBus, EventBus
    │   ├── ObjectPool/       PoolManager, ObjectPool, PoolEntry
    │   ├── SaveSystem/       ISaveManager, SaveManager
    │   └── Zenject/          SceneInstaller
    ├── Enums/
    ├── Events/               Global struct events
    ├── Gameplay/
    │   ├── CollectedRewardManager/
    │   ├── CurrencyDisplayController/
    │   ├── Exit/             Button + states + events + panel
    │   ├── GameManager/
    │   ├── IndicatorController/
    │   ├── Inventory/
    │   ├── LevelDisplayManager/
    │   ├── LevelFailedPanelController/
    │   ├── LevelFlow/        Level state events
    │   ├── Revive/           Service + config + events
    │   ├── Reward/           Data, definitions, catalog, provider
    │   ├── RewardMoveEffectManager/   UniTask + pooling
    │   ├── SpinController/   Button + states + events
    │   ├── Wheel/            Mechanics + RankVisuals/ (FSM)
    │   └── ZoneDisplayController/
    └── Utils/
        ├── BaseButton/
        ├── ObjectSpinner/    DOTween wrapper
        ├── RankDeterminer/
        └── UIMover/          DOTween wrapper
```

---

## How to Run

1. Clone the repository.
2. Open the project in **Unity 2021.3.45f2** (other 2021.3.x versions should also work).
3. Open [Assets/WheelOfFortune/Scenes/GameScene.unity](Assets/WheelOfFortune/Scenes/GameScene.unity).
4. Press Play.

---

## Media

> **Note:** For the Drive photos to render inline, each file's sharing setting must be **"Anyone with the link — Viewer"**. Otherwise the images will appear broken.

### 16:9

**Gameplay video**

[<img src="https://img.youtube.com/vi/vBQ41tHJFRw/hqdefault.jpg" width="600" alt="16:9 gameplay video" />](https://www.youtube.com/watch?v=vBQ41tHJFRw)

**Screenshot 1**

<img src="https://drive.google.com/uc?export=view&id=1FZHJ6pfbRnvWSwAy8qHDgYbynOmS8QWy" width="600" alt="16:9 screenshot 1" />

**Screenshot 2**

<img src="https://drive.google.com/uc?export=view&id=1R06zqjQtBc-n_Xe-NcMOqL-IZgt1F7PW" width="600" alt="16:9 screenshot 2" />

**Screenshot 3**

<img src="https://drive.google.com/uc?export=view&id=16uCYqDHRAP3Pc3lAxg4sHDg75ngFgHLx" width="600" alt="16:9 screenshot 3" />

### 20:9

**Gameplay video**

[<img src="https://img.youtube.com/vi/yTubEGO-jLQ/hqdefault.jpg" width="600" alt="20:9 gameplay video" />](https://www.youtube.com/watch?v=yTubEGO-jLQ)

**Screenshot 1**

<img src="https://drive.google.com/uc?export=view&id=1Vc3qjp8PnWPtsAr8PFpe2yU7XJIm7oVH" width="600" alt="20:9 screenshot 1" />

**Screenshot 2**

<img src="https://drive.google.com/uc?export=view&id=1_DEcyrnlQS8kBpwAuvrQzvKuvUsGwQ9-" width="600" alt="20:9 screenshot 2" />

**Screenshot 3**

<img src="https://drive.google.com/uc?export=view&id=1H-Byh8Qtql777ZI-IDFhaSI2LIBLUvLY" width="600" alt="20:9 screenshot 3" />

### 4:3

**Gameplay video**

[<img src="https://img.youtube.com/vi/prO2630oSsQ/hqdefault.jpg" width="600" alt="4:3 gameplay video" />](https://www.youtube.com/watch?v=prO2630oSsQ)

**Screenshot 1**

<img src="https://drive.google.com/uc?export=view&id=1zQqwAvyaFVE0jj2ElWGd7aOkQEIy7Y5-" width="600" alt="4:3 screenshot 1" />

**Screenshot 2**

<img src="https://drive.google.com/uc?export=view&id=1Ins1FEFMyUdgin74lNoQv9WuTUE9SeBe" width="600" alt="4:3 screenshot 2" />

**Screenshot 3**

<img src="https://drive.google.com/uc?export=view&id=1SaMkl5QP4ZVTcfyvYaOFCwCfR11qBGti" width="600" alt="4:3 screenshot 3" />

### Direct links (in case the previews above don't render)

**16:9** — [Video](https://www.youtube.com/watch?v=vBQ41tHJFRw) · [Photo 1](https://drive.google.com/file/d/1FZHJ6pfbRnvWSwAy8qHDgYbynOmS8QWy/view?usp=drive_link) · [Photo 2](https://drive.google.com/file/d/1R06zqjQtBc-n_Xe-NcMOqL-IZgt1F7PW/view?usp=drive_link) · [Photo 3](https://drive.google.com/file/d/16uCYqDHRAP3Pc3lAxg4sHDg75ngFgHLx/view?usp=drive_link)

**20:9** — [Video](https://www.youtube.com/watch?v=yTubEGO-jLQ) · [Photo 1](https://drive.google.com/file/d/1Vc3qjp8PnWPtsAr8PFpe2yU7XJIm7oVH/view?usp=drive_link) · [Photo 2](https://drive.google.com/file/d/1_DEcyrnlQS8kBpwAuvrQzvKuvUsGwQ9-/view?usp=drive_link) · [Photo 3](https://drive.google.com/file/d/1H-Byh8Qtql777ZI-IDFhaSI2LIBLUvLY/view?usp=drive_link)

**4:3** — [Video](https://www.youtube.com/watch?v=prO2630oSsQ) · [Photo 1](https://drive.google.com/file/d/1zQqwAvyaFVE0jj2ElWGd7aOkQEIy7Y5-/view?usp=drive_link) · [Photo 2](https://drive.google.com/file/d/1Ins1FEFMyUdgin74lNoQv9WuTUE9SeBe/view?usp=drive_link) · [Photo 3](https://drive.google.com/file/d/1SaMkl5QP4ZVTcfyvYaOFCwCfR11qBGti/view?usp=drive_link)
