# Level Story Event Framework — Phase 1  
# Full Guide & Scripts

This document is your **single reference**: where to work, what to do, and **all script code** for the Level Story Event Framework. Use it in order: first the work checklist, then the scripts.

---

# PART 1 — WHERE / WHAT (Your Work Checklist)

Do these in order. Each row = one place and one job.

| # | Where (file or scene) | What to do |
|---|------------------------|------------|
| 1 | `Assets/Scripts/StoryEvent/` | Create this folder. All new scripts go here. |
| 2 | `Assets/Scenes/Sandboxes/` | Use for testing. Create **StoryEventSandbox** (or use CleanScene). |
| 3 | `Assets/Scripts/StoryEvent/GameplaySignal.cs` | **Create** — static signal hub: Raise(id), Subscribe(id, callback). See script below. |
| 4 | `Assets/Scripts/StoryEvent/StoryTypes.cs` | **Create** — enums StoryMode, CompletionType. See script below. |
| 5 | `Assets/Scripts/StoryEvent/StoryStepContext.cs` | **Create** — context passed to actions (Player, UIManager). See script below. |
| 6 | `Assets/Scripts/StoryEvent/StoryStep.cs` | **Create** — step data: trigger step index, actions list, completion type/params, requested mode. See script below. |
| 7 | `Assets/Scripts/StoryEvent/StoryTriggerBase.cs` | **Create** — abstract trigger: reference to controller + step index; NotifyController(). See script below. |
| 8 | `Assets/Scripts/StoryEvent/EnterZoneTrigger.cs` | **Create** — OnTriggerEnter (tag "Player"), notify controller for step. See script below. |
| 9 | `Assets/Scripts/StoryEvent/TimerTrigger.cs` | **Create** — timer; when done, notify controller. See script below. |
| 10 | `Assets/Scripts/StoryEvent/GameplaySignalTrigger.cs` | **Create** — subscribe to GameplaySignal; when signal raised, notify controller. See script below. |
| 11 | `Assets/Scripts/StoryEvent/ManualTrigger.cs` | **Create** — public Trigger() or UnityEvent to notify controller. See script below. |
| 12 | `Assets/Scripts/StoryEvent/StoryActionBase.cs` | **Create** — abstract Execute(StoryStepContext). See script below. |
| 13 | `Assets/Scripts/StoryEvent/ShowPromptAction.cs` | **Create** — UIManager.ShowInfoText(title, text, Identity). See script below. |
| 14 | `Assets/Scripts/StoryEvent/LockControlsAction.cs` | **Create** — player.input.DisableInput(). See script below. |
| 15 | `Assets/Scripts/StoryEvent/UnlockControlsAction.cs` | **Create** — player.input.EnableInput(). See script below. |
| 16 | `Assets/Scripts/StoryEvent/EnableCorgiSenseAction.cs` | **Create** — SaveManager.Instance.collectibleData.CorgiSenseEnabled = true. See script below. |
| 17 | `Assets/Scripts/StoryEvent/DisableCorgiSenseAction.cs` | **Create** — set CorgiSenseEnabled = false. See script below. |
| 18 | `Assets/Scripts/StoryEvent/RequestGameplayStateAction.cs` | **Create** — call bridge.ApplyMode(mode). See script below. |
| 19 | `Assets/Scripts/StoryEvent/StoryGameplayBridge.cs` | **Create** — ApplyMode(mode): input lock, etc. Uses Player, InputDriver. See script below. |
| 20 | `Assets/Scripts/StoryEvent/LevelStorySequenceController.cs` | **Create** — list of steps, current index, NotifyTriggerFired, run actions, completion, AdvanceStep(). See script below. |
| 21 | `Assets/Scripts/Gameplay/Player/PlayerStateMachine/SubStates/PlayerThrustState.cs` | **Edit** — when thrust is applied (e.g. in FixedUpdate when you call thrust()), add: `GameplaySignal.Raise(ThrustUsedSignalId);` (use constant string). |
| 22 | `Assets/Scenes/Sandboxes/StoryEventSandbox.unity` | **Create** — duplicate CleanScene or new scene: Player, UIManager, ground, LevelStorySequenceController, one EnterZoneTrigger volume. Configure 5-step flow per sandbox section below. |
| 23 | Play & validate | **Test** — enter zone → prompt → thrust → success → CorgiSense. No sequence breaking. |

---

# PART 2 — Architecture Summary

- **Player state machine:** Do not change. Story only locks input and shows UI via a thin integration layer.
- **LevelStorySequenceController:** Holds ordered steps, current step index. Only the current step can advance the sequence.
- **StoryStep:** Trigger (by step index), list of actions, completion type (Instant / AfterDialogue / AfterTimer / AfterSignal / Manual), requested mode.
- **Triggers:** Notify the controller when condition is met; controller accepts only if step index matches current.
- **Actions:** Execute(StoryStepContext) — call UIManager, InputDriver, CollectibleData via context/bridge.
- **StoryGameplayBridge:** Single place that applies requested mode (input lock). Uses `InputDriver.EnableInput()` / `DisableInput()`.
- **GameplaySignal:** Static Raise(id) / Subscribe(id, Action). Used for "ThrustUsed" and manual completion.
- **Sandbox:** All work and testing in `Assets/Scenes/Sandboxes/`, not in production levels.

---

# PART 3 — Integration Points

| Existing asset | How story framework uses it |
|----------------|------------------------------|
| **Player** | Reference in controller; pass to context for actions. |
| **Player.input** (InputDriver) | `DisableInput()` / `EnableInput()` for Lock/Unlock controls. |
| **UIManager.ShowInfoText(title, text, arrowTransform)** | ShowPrompt action calls this; use `EditorLocalTransform.Identity` if no arrow. |
| **InfoModel** | Has messageDuration (~3s). For "after dialogue" completion use a timer with same duration or add OnMessageHidden later. |
| **SaveManager.Instance.collectibleData.CorgiSenseEnabled** | Enable/Disable CorgiSense actions set this. No Save() needed for sandbox. |
| **EditorLocalTransform** | Use Identity for prompt arrow when not needed. |

---

# PART 4 — Inspector & Sandbox Test Flow

**LevelStorySequenceController (Inspector):**
- List of StoryStep. Each step: trigger step index (or "this step"), completion type + optional timer duration / signal ID, requested mode, list of actions (inline or references).
- Assign **Player**, **UIManager**, **StoryGameplayBridge** (optional; can be on same GameObject).
- **Actions:** Put action components (ShowPromptAction, LockControlsAction, etc.) on the same GameObject as the controller or on child objects. In each step's **actions** list, drag those components in. You can reuse the same component in multiple steps if needed.

**Triggers:** Each trigger: assign **LevelStorySequenceController** and **step index** (0, 1, 2...). EnterZoneTrigger needs a trigger Collider on same GameObject.

**Sandbox setup note:** If you use `PlayerGameplayPrefab`, avoid duplicate runtime roots. Keep only one active player/camera stack and ensure exactly **one AudioListener** is enabled in the scene.

**Sandbox 5-step flow:**
1. Step 0: EnterZoneTrigger → completion Instant.
2. Step 1: No trigger (immediate); ShowPrompt "Tilt left or right to aim, then hold thrust to fly."; completion AfterTimer 3f.
3. Step 2: UnlockControls; completion AfterSignal "ThrustUsed".
4. Step 3: ShowPrompt "Success!"; completion AfterTimer 3f.
5. Step 4: EnableCorgiSense; completion Instant.

---

# PART 5 — All Scripts

Copy each block into the file path shown. Adjust namespace if your project uses one (these use no namespace to match existing scripts).

---

## File: `Assets/Scripts/StoryEvent/GameplaySignal.cs`

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static hub for story/tutorial signals. Raise(id) and Subscribe(id, callback).
/// </summary>
public static class GameplaySignal
{
    public const string ThrustUsedSignalId = "ThrustUsed";
    public const string ObjectiveCompleteSignalId = "ObjectiveComplete";
    public const string StepCompleteSignalId = "StepComplete";

    private static readonly Dictionary<string, Action> Handlers = new Dictionary<string, Action>();

    public static void Subscribe(string signalId, Action callback)
    {
        if (string.IsNullOrEmpty(signalId) || callback == null) return;
        if (!Handlers.ContainsKey(signalId))
            Handlers[signalId] = null;
        Handlers[signalId] += callback;
    }

    public static void Unsubscribe(string signalId, Action callback)
    {
        if (string.IsNullOrEmpty(signalId) || !Handlers.ContainsKey(signalId)) return;
        Handlers[signalId] -= callback;
    }

    public static void Raise(string signalId)
    {
        if (string.IsNullOrEmpty(signalId) || !Handlers.ContainsKey(signalId)) return;
        Handlers[signalId]?.Invoke();
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/StoryTypes.cs`

```csharp
using UnityEngine;

public enum StoryMode
{
    Gameplay,
    GuidedGameplay,
    Cutscene,
    PromptMode,
    CorgiSense
}

public enum CompletionType
{
    Instant,
    AfterDialogueDuration,
    AfterTimer,
    AfterGameplaySignal,
    Manual
}
```

---

## File: `Assets/Scripts/StoryEvent/StoryStepContext.cs`

```csharp
using UnityEngine;

/// <summary>
/// Passed to story actions so they can access Player, UIManager, etc.
/// </summary>
public class StoryStepContext
{
    public Player Player { get; set; }
    public UIManager UIManager { get; set; }
    public LevelStorySequenceController Controller { get; set; }
}
```

---

## File: `Assets/Scripts/StoryEvent/StoryStep.cs`

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoryStep
{
    [Tooltip("Step index this trigger belongs to (e.g. 0 = first step). Trigger fires when controller is on this step.")]
    public int triggerStepIndex;

    [Tooltip("Completion type for this step.")]
    public CompletionType completionType = CompletionType.Instant;

    [Tooltip("For AfterTimer: duration in seconds.")]
    public float completionTimerDuration = 3f;

    [Tooltip("For AfterGameplaySignal: signal ID to wait for.")]
    public string completionSignalId = GameplaySignal.ThrustUsedSignalId;

    [Tooltip("Requested gameplay/story mode when this step runs.")]
    public StoryMode requestedMode = StoryMode.Gameplay;

    [Tooltip("Actions to run when step is entered (in order).")]
    public List<StoryActionBase> actions = new List<StoryActionBase>();

    [Tooltip("If true, this step does not require a trigger; run actions as soon as step is entered.")]
    public bool runImmediately;
}
```

---

## File: `Assets/Scripts/StoryEvent/StoryTriggerBase.cs`

```csharp
using UnityEngine;

/// <summary>
/// Base for triggers that notify the sequence controller when a condition is met.
/// Assign controller and stepIndex in Inspector. Only the controller's current step can advance.
/// </summary>
public abstract class StoryTriggerBase : MonoBehaviour
{
    [SerializeField] protected LevelStorySequenceController controller;
    [SerializeField] protected int stepIndex;

    protected void NotifyController()
    {
        if (controller != null)
            controller.NotifyTriggerFired(stepIndex);
    }

    protected virtual void OnValidate()
    {
        if (controller == null)
            controller = FindFirstObjectByType<LevelStorySequenceController>();
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/EnterZoneTrigger.cs`

```csharp
using UnityEngine;

public class EnterZoneTrigger : StoryTriggerBase
{
    [SerializeField] private string requiredTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || string.IsNullOrEmpty(requiredTag)) return;
        if (other.CompareTag(requiredTag))
            NotifyController();
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/TimerTrigger.cs`

```csharp
using System.Collections;
using UnityEngine;

public class TimerTrigger : StoryTriggerBase
{
    [SerializeField] private float delaySeconds = 1f;
    [SerializeField] private bool startOnEnable = true;

    private void OnEnable()
    {
        if (startOnEnable)
            StartCoroutine(RunTimer());
    }

    public void StartTimer()
    {
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(delaySeconds);
        NotifyController();
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/GameplaySignalTrigger.cs`

```csharp
using UnityEngine;

public class GameplaySignalTrigger : StoryTriggerBase
{
    [SerializeField] private string signalId = GameplaySignal.ThrustUsedSignalId;

    private void OnEnable()
    {
        GameplaySignal.Subscribe(signalId, OnSignalRaised);
    }

    private void OnDisable()
    {
        GameplaySignal.Unsubscribe(signalId, OnSignalRaised);
    }

    private void OnSignalRaised()
    {
        NotifyController();
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/ManualTrigger.cs`

```csharp
using UnityEngine;
using UnityEngine.Events;

public class ManualTrigger : StoryTriggerBase
{
    [SerializeField] private UnityEvent onTriggerButton;

    public void Trigger()
    {
        NotifyController();
    }

    private void OnEnable()
    {
        if (onTriggerButton != null)
            onTriggerButton.AddListener(Trigger);
    }

    private void OnDisable()
    {
        if (onTriggerButton != null)
            onTriggerButton.RemoveListener(Trigger);
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/StoryActionBase.cs`

```csharp
using UnityEngine;

public abstract class StoryActionBase : MonoBehaviour
{
    public abstract void Execute(StoryStepContext context);
}
```

---

## File: `Assets/Scripts/StoryEvent/ShowPromptAction.cs`

```csharp
using UnityEngine;

public class ShowPromptAction : StoryActionBase
{
    [SerializeField] private string title = "Tutorial";
    [SerializeField] private string text = "Tilt left or right to aim, then hold thrust to fly.";
    [SerializeField] private bool useArrowTransform;
    [SerializeField] private EditorLocalTransform arrowTransform;

    public override void Execute(StoryStepContext context)
    {
        if (context?.UIManager == null) return;
        var transformToUse = useArrowTransform ? arrowTransform : EditorLocalTransform.Identity;
        context.UIManager.ShowInfoText(title, text, transformToUse);
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/LockControlsAction.cs`

```csharp
using UnityEngine;

public class LockControlsAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (context?.Player?.input == null) return;
        context.Player.input.DisableInput();
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/UnlockControlsAction.cs`

```csharp
using UnityEngine;

public class UnlockControlsAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (context?.Player?.input == null) return;
        context.Player.input.EnableInput();
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/EnableCorgiSenseAction.cs`

```csharp
using UnityEngine;

public class EnableCorgiSenseAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (SaveManager.Instance?.collectibleData == null) return;
        SaveManager.Instance.collectibleData.CorgiSenseEnabled = true;
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/DisableCorgiSenseAction.cs`

```csharp
using UnityEngine;

public class DisableCorgiSenseAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (SaveManager.Instance?.collectibleData == null) return;
        SaveManager.Instance.collectibleData.CorgiSenseEnabled = false;
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/StoryGameplayBridge.cs`

```csharp
using UnityEngine;

/// <summary>
/// Single place that applies story-requested mode to Player/Input. Do not add new player states.
/// </summary>
public class StoryGameplayBridge : MonoBehaviour
{
    [SerializeField] private Player player;

    public void ApplyMode(StoryMode mode)
    {
        if (player?.input == null) return;
        switch (mode)
        {
            case StoryMode.Gameplay:
            case StoryMode.GuidedGameplay:
            case StoryMode.CorgiSense:
                player.input.EnableInput();
                break;
            case StoryMode.Cutscene:
            case StoryMode.PromptMode:
                player.input.DisableInput();
                break;
            default:
                player.input.EnableInput();
                break;
        }
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/RequestGameplayStateAction.cs`

```csharp
using UnityEngine;

public class RequestGameplayStateAction : StoryActionBase
{
    [SerializeField] private StoryMode mode = StoryMode.Gameplay;
    [SerializeField] private StoryGameplayBridge bridge;

    public override void Execute(StoryStepContext context)
    {
        var b = bridge != null ? bridge : context?.Controller?.GetBridge();
        if (b != null)
            b.ApplyMode(mode);
    }
}
```

---

## File: `Assets/Scripts/StoryEvent/LevelStorySequenceController.cs`

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages ordered story steps. Only the current step can advance the sequence.
/// Assign Player, UIManager, and optional StoryGameplayBridge in Inspector.
/// </summary>
public class LevelStorySequenceController : MonoBehaviour
{
    [SerializeField] private List<StoryStep> steps = new List<StoryStep>();
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private StoryGameplayBridge bridge;

    private int _currentStepIndex;
    private bool _currentStepTriggerFired;
    private bool _currentStepCompletionStarted;
    private string _subscribedCompletionSignalId; // so we unsubscribe the correct one
    private StoryStepContext _context;

    private void Awake()
    {
        _context = new StoryStepContext
        {
            Player = player,
            UIManager = uiManager,
            Controller = this
        };
    }

    private void Start()
    {
        _currentStepIndex = 0;
        _currentStepTriggerFired = false;
        _currentStepCompletionStarted = false;
        TryEnterCurrentStep();
    }

    /// <summary>
    /// Called by triggers. Only advances if stepIndex matches current step.
    /// </summary>
    public void NotifyTriggerFired(int stepIndex)
    {
        if (stepIndex != _currentStepIndex) return;
        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;

        _currentStepTriggerFired = true;
        RunCurrentStepActions();
    }

    private void TryEnterCurrentStep()
    {
        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;

        var step = steps[_currentStepIndex];
        if (step.runImmediately)
        {
            _currentStepTriggerFired = true;
            RunCurrentStepActions();
        }
    }

    private void RunCurrentStepActions()
    {
        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;
        var step = steps[_currentStepIndex];

        if (bridge != null)
            bridge.ApplyMode(step.requestedMode);

        if (step.actions != null)
        {
            foreach (var action in step.actions)
            {
                if (action != null)
                    action.Execute(_context);
            }
        }

        StartCompletionForCurrentStep();
    }

    private void StartCompletionForCurrentStep()
    {
        if (_currentStepCompletionStarted) return;
        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;

        var step = steps[_currentStepIndex];
        _currentStepCompletionStarted = true;

        switch (step.completionType)
        {
            case CompletionType.Instant:
                AdvanceStep();
                break;
            case CompletionType.AfterTimer:
            case CompletionType.AfterDialogueDuration:
                StartCoroutine(CompleteAfterTimer(step.completionTimerDuration));
                break;
            case CompletionType.AfterGameplaySignal:
                _subscribedCompletionSignalId = step.completionSignalId;
                GameplaySignal.Subscribe(_subscribedCompletionSignalId, OnCompletionSignalRaised);
                break;
            case CompletionType.Manual:
                _subscribedCompletionSignalId = GameplaySignal.StepCompleteSignalId;
                GameplaySignal.Subscribe(_subscribedCompletionSignalId, OnCompletionSignalRaised);
                break;
        }
    }

    private void OnCompletionSignalRaised()
    {
        if (!string.IsNullOrEmpty(_subscribedCompletionSignalId))
        {
            GameplaySignal.Unsubscribe(_subscribedCompletionSignalId, OnCompletionSignalRaised);
            _subscribedCompletionSignalId = null;
        }
        AdvanceStep();
    }

    private IEnumerator CompleteAfterTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        AdvanceStep();
    }

    private void AdvanceStep()
    {
        _currentStepCompletionStarted = false;
        _currentStepTriggerFired = false;
        _currentStepIndex++;

        if (_currentStepIndex >= steps.Count)
        {
            // Sequence complete
            return;
        }

        TryEnterCurrentStep();
    }

    /// <summary>
    /// For RequestGameplayStateAction when bridge not set on action.
    /// </summary>
    public StoryGameplayBridge GetBridge() => bridge;
}
```

---

## ObjectiveCompleteTrigger (optional)

If you want a dedicated "objective complete" trigger that fires for a step, use **GameplaySignalTrigger** with `signalId = GameplaySignal.ObjectiveCompleteSignalId`, and from level logic call `GameplaySignal.Raise(GameplaySignal.ObjectiveCompleteSignalId)`. No extra script required. If you prefer a separate component:

## File: `Assets/Scripts/StoryEvent/ObjectiveCompleteTrigger.cs` (optional)

```csharp
using UnityEngine;

public class ObjectiveCompleteTrigger : GameplaySignalTrigger
{
    private void OnValidate()
    {
        // Set default signal to ObjectiveComplete in Inspector or here
    }
}
```

You can set in Inspector: signalId = `GameplaySignal.ObjectiveCompleteSignalId`. So this file is optional; **GameplaySignalTrigger** is enough.

---

## Wiring ThrustUsed in PlayerThrustState

**Where:** `Assets/Scripts/Gameplay/Player/PlayerStateMachine/SubStates/PlayerThrustState.cs`

**What:** When thrust is actually applied, raise the signal so "wait for thrust" steps can complete.

Add at the top of the file (with other usings) nothing extra. Add one line where you call `thrust()` and use fuel — e.g. after `thrust();` and `UseFuel(isBoosting);` in FixedUpdate:

```csharp
// After thrust(); UseFuel(isBoosting); in the block where stateAge >= 3 and GoThrust
GameplaySignal.Raise(GameplaySignal.ThrustUsedSignalId);
```

Example placement (inside FixedUpdate, in the block that runs thrust):

```csharp
if (stateAge == 3)
{
    if (player.input.GoThrust)
    {
        player.vfx.StartPrimaryThrusters();
        thrust();
        UseFuel(isBoosting);
        GameplaySignal.Raise(GameplaySignal.ThrustUsedSignalId);
    }
}
if (stateAge > 3)
{
    if (player.input.GoThrust)
    {
        thrust();
        UseFuel(isBoosting);
        GameplaySignal.Raise(GameplaySignal.ThrustUsedSignalId);
    }
}
```

(You may want to raise only once per "press" to avoid completing the step in one frame; if so, add a small cooldown or a flag so you raise only the first time thrust is used in a given step. For minimal sandbox, raising every frame while thrusting is acceptable.)

---

# PART 6 — Fixing OnCompletionSignalRaised (Unsubscribe by step)

The controller subscribes to a **step-specific** signal in `StartCompletionForCurrentStep`. In the script above, `OnCompletionSignalRaised` unsubscribes using `_subscribedCompletionSignalId` (the signal id captured when subscribing), then clears that field and advances. For **Manual**, it subscribes to `StepCompleteSignalId` and unsubscribes the same way. If you use a shared callback for multiple signals, always keep the subscribed signal id so you can unsubscribe exactly what you added.

---

# PART 7 — Summary Checklist

- [ ] Create folder `Assets/Scripts/StoryEvent/`.
- [ ] Add all scripts from Part 5 to the correct paths.
- [ ] In **PlayerThrustState**, add `GameplaySignal.Raise(GameplaySignal.ThrustUsedSignalId);` where thrust is applied.
- [ ] Create **StoryEventSandbox** scene: Player, UIManager, ground, **LevelStorySequenceController** (assign Player, UIManager, Bridge), add 5 steps (see Part 4). Step 0: EnterZoneTrigger on a trigger volume. Steps 1–4: runImmediately = true, completion and actions as in Part 4. For step 2 (wait for thrust) use CompletionType.AfterGameplaySignal and completionSignalId = "ThrustUsed". No trigger component needed for steps 1–4 if runImmediately is true; for step 2 the controller subscribes to "ThrustUsed" and advances when it fires.
- [ ] Play: enter zone → prompt → thrust → success → CorgiSense. Confirm re-entering the zone on step 1 does not break the sequence.

This document contains the full guide and all scripts. Use it as the single source for Phase 1 implementation.
