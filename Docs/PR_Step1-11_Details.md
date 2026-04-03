# PR Details — Level Story Event Framework (Phase 1)

## PR Title
`feat(story-event): implement Phase 1 sandbox framework`

## Summary
This PR introduces a reusable Level Story Event Framework and validates it in `StoryEventSandbox`.

The work focuses on modular step sequencing, reusable triggers/actions, and clean integration with existing gameplay systems (without rebuilding the player state machine).

---

## Scope Delivered

### End-to-end workflow (how a level story runs)

This section describes the **runtime** flow, not a build checklist.

1. **Scene setup**: A level (or sandbox) contains one `LevelStorySequenceController`, a `StoryGameplayBridge` wired to the player, and optional trigger objects (`EnterZoneTrigger`, `TimerTrigger`, `ManualTrigger`) each pointing at the controller and a **step index** they are allowed to advance.
2. **Start**: On play, the controller resets to step `0` and optionally runs `runImmediately` steps without waiting for a trigger.
3. **Trigger**: When the current step matches, a trigger calls `NotifyTriggerFired(stepIndex)`. The controller accepts only if `stepIndex` equals the **current** step; otherwise the event is ignored (sequence cannot skip ahead or jump back).
4. **Actions**: For an accepted step, the controller applies the step’s **requested mode** via the bridge, then runs each action in the step’s `actions` list (prompts, locks, CorgiSense, etc.).
5. **Completion**: After actions, the controller resolves **completion** (`Instant`, timer, or gameplay signal). When completion finishes, the sequence advances to the next step and repeats from step 2 (including `runImmediately` for the new step if configured).

**Configuration workflow**: Ordered steps are set up in the Unity Inspector (list size, completion type, timers/signal ids, action components assigned per step). Trigger components live in the scene as separate GameObjects; they are **not** dropped into the actions list. Actions are MonoBehaviours referenced **per step** so the same prompt/control behaviour can be reused with different copy or timing.

---

### 1) Core sequence controller

**Implementation**
- Added `LevelStorySequenceController` with ordered step progression.
- Added current-step gating so only expected step triggers are accepted.
- Added progression handling and step entry flow (`AdvanceStep`, `runImmediately` support).
- Added guard flags to reduce duplicate trigger/completion processing.

**Workflow**
- **Single source of truth**: `currentStepIndex` is the only progression cursor; triggers and actions never “pick” the next step except by advancing in order.
- **Gating**: `NotifyTriggerFired` compares the caller’s `stepIndex` to `currentStepIndex`; mismatches log as ignored so stale or wrong-step triggers are visible in logs and do not advance the sequence.
- **Duplicate protection**: Multiple colliders on the player can fire `OnTriggerEnter` more than once; the controller flags a step as “already handled” so actions and completion do not run twice for the same step.
- **Auto-start steps**: Steps with `runImmediately` run on entry without an external trigger (useful for tutorials that begin as soon as the level loads or right after the previous step completes).

---

### 2) Story step data

**Implementation**
- Added `StoryStep` data model with:
  - trigger step index
  - completion type and config fields (timer/signal)
  - requested mode
  - action list
  - run-immediately flag

**Workflow**
- **Data vs behaviour**: `StoryStep` is **data** (serialized on the controller). `StoryActionBase` components are **behaviour** (referenced from each step). This split keeps the inspector readable: one row per beat, with linked actions and completion rules.
- **Trigger index alignment**: Scene triggers reference a **step index**; that index must match the step order in the controller list. If steps are reordered in the list, trigger indices must be updated to stay aligned.
- **Completion fields**: Timer duration and `completionSignalId` are only meaningful when the step’s `completionType` is set to timer or signal; `requestedMode` is applied when the step runs (before actions).

---

### 3) Trigger system

**Implementation**
- Added reusable triggers:
  - `EnterZoneTrigger`
  - `TimerTrigger`
  - `ManualTrigger`
- Verified trigger gating against `currentStepIndex`.

**Workflow**
- **Placement**: Each trigger is a **MonoBehaviour on a GameObject** in the scene (zone collider, empty object with timer, UI button hooking to `ManualTrigger`, etc.). It calls `NotifyTriggerFired` when its condition is met.
- **Not an action**: Triggers are **not** added to the step `actions` list; they are separate scene components that **start** a step. Putting a trigger in the actions list would not run it as a trigger.
- **Zone flow**: `EnterZoneTrigger` waits for the tagged object (e.g. Player) to enter a trigger volume; then notifies the controller for the configured step.
- **Timer flow**: `TimerTrigger` starts after `Start` or can be enabled per your design; after `delaySeconds` it notifies the controller—useful for scripted beats without player position.
- **Manual flow**: `ManualTrigger` exposes a method to call from buttons, debug keys, or other scripts to complete or advance a gated step when automation is not desired.

---

### 4) Action system

**Implementation**
- Added base action contract `StoryActionBase`.
- Added prompt actions used in sandbox steps.
- Added gameplay-control actions:
  - `LockControlsAction`
  - `UnlockControlsAction`
- Added CorgiSense actions:
  - `EnableCorgiSenseAction`
  - `DisableCorgiSenseAction`
- Added `RequestGameplayStateAction` integration path.

**Workflow**
- **Execution order**: When a step is accepted, actions run **in list order** in the same frame (after mode is applied). Order matters if you lock controls before showing a prompt, or show a prompt before enabling CorgiSense.
- **Context**: Each action receives `StoryStepContext` (player, `UIManager`, controller) so actions stay small and do not hardcode scene references.
- **Prompts**: UI prompts go through existing `UIManager` / info flow so localization and layout stay consistent with the rest of the game.
- **Controls / CorgiSense**: Lock/unlock and CorgiSense toggles change the **gameplay state** the player feels immediately; they do not replace the player FSM—they operate through existing input and data (`InputDriver`, `CollectibleData`).

---

### 5) Completion modes

**Implementation**
- Implemented and tested:
  - `Instant`
  - `AfterTimer`
  - `AfterDialogueDuration` (timer-based handling path)
  - `AfterGameplaySignal`

**Workflow**
- **Instant**: Advance as soon as the step’s actions and completion start are processed—shortest path for “tutorial stamp” transitions.
- **After timer / dialogue duration**: Wait `completionTimerDuration` then advance; use for “read this prompt for N seconds” before the next beat.
- **After gameplay signal**: Subscribe once to `GameplaySignal` with `completionSignalId`; when the game raises that signal (e.g. thrust used), unsubscribe and advance. Use for “wait until the player actually does the thing.”
- **Single completion**: Completion is started once per step (guarded) so timers and subscriptions do not stack if something fires twice.

---

### 6) Gameplay signal integration

**Implementation**
- Added `GameplaySignal` event hub.
- Wired thrust signal from player thrust logic:
  - `PlayerThrustState` raises `ThrustUsed`.
- Controller listens for matching signal id and advances step.

**Workflow**
- **Publish / subscribe**: Gameplay code can `Raise` a named signal; story code can `Subscribe` by string id. No direct reference from gameplay prefabs to story prefabs is required beyond the shared id.
- **Thrust path**: When the player uses thrust in the relevant state, `ThrustUsed` fires; any step waiting on that signal with matching `completionSignalId` completes and advances.
- **Extensibility**: New signals can be added for future objectives (collect item, reach altitude, etc.) without changing the controller API—only new ids and raisers.

---

### 7) Mode/state integration

**Implementation**
- Added `StoryGameplayBridge` to map story mode requests to input enable/disable behavior.
- Added controller getter for bridge access by actions.

**Workflow**
- **Requested mode**: Each `StoryStep` can set `requestedMode` (e.g. `Gameplay`, `Cutscene`, `PromptMode`). When the step runs, the controller calls `StoryGameplayBridge.ApplyMode` **before** actions so the player is in the intended mode for prompts or cutscene-like locks.
- **Bridge vs FSM**: The bridge does **not** add new player states; it applies policy (e.g. enable/disable input) consistent with the existing `InputDriver` setup. Complex cutscenes can still be layered later without rewriting this contract.
- **Actions**: `RequestGameplayStateAction` can request the same mode path through the bridge when a step needs an explicit mode change mid-action list.

---

### 8) Sandbox validation

**Implementation**
- Implemented and iterated scenario in `StoryEventSandbox`.
- Validated prompt flow, timer flow, signal flow, lock/unlock behavior, and CorgiSense toggles.
- Added testing notes in docs for repeatable validation.

**Workflow**
- **Isolation**: Sandbox uses the same prefabs and patterns as production (`PlayerGameplayPrefab`, `UIObject`, `SaveManager`) so behaviour matches real levels without touching shipping content.
- **Iteration**: Configure steps and triggers in the sandbox scene → enter Play → verify console logs (accepted vs ignored triggers) and UI prompts → adjust step indices, completion types, or trigger delays as needed.
- **Documentation**: Internal guides describe how to reproduce scenarios (ground plane, single `AudioListener`, CorgiSense wiring) so the same checks can be repeated across machines.

---

## Why this matters
- Provides reusable **story/tutorial sequencing** infrastructure for levels.
- Prevents sequence breaking by enforcing current-step gating.
- Keeps integration low-risk by using existing systems (`Player`, `InputDriver`, `UIManager`, `SaveManager`) instead of reworking player state machine.
- Establishes a validated sandbox pattern before production-level rollout.

---

## Validation focus

### A. Sequence gating
- Old triggers are ignored once step advances.
- Only current step trigger/progression path is accepted.

### B. Prompt/action execution
- Step actions execute in configured order.
- Prompt actions show expected text when UI references are assigned.

### C. Completion behavior
- Instant steps advance once.
- Timer steps advance after configured delay.
- Signal steps advance only on matching signal id (`ThrustUsed`).

### D. Gameplay control
- Lock action disables input.
- Unlock action restores input.

### E. CorgiSense toggle
- Enable/disable actions set `CollectibleData.CorgiSenseEnabled` true/false.
- Optional visual confirmation can be done if CorgiSense UI is wired in sandbox.

### F. Trigger components
- TimerTrigger and ManualTrigger can drive step activation without zone trigger.
- Both respect step-index gating.

---

## Test Plan (Checklist)

- [ ] Current-step trigger gating works (matching step accepted, stale step ignored).
- [ ] Prompt actions render expected tutorial text in sandbox.
- [ ] Completion modes work: Instant, timer-based, and gameplay-signal-based.
- [ ] Gameplay signal integration works: thrust raises `ThrustUsed`, matching step advances.
- [ ] Control lock/unlock actions correctly disable and restore player input.
- [ ] CorgiSense enable/disable actions toggle `CollectibleData.CorgiSenseEnabled` as expected.
- [ ] Timer and manual triggers can drive step execution without zone dependency.
- [ ] Re-entering old triggers does not break sequence order.

---

## Notes / Known Testing Constraints
- Sandbox UI prefab may show extra overlays not directly related to story tests; focus validation on target prompts/actions and controller logs.
- Ensure exactly one active `AudioListener` in scene when validating.
- If validating CorgiSense visually, required UI references and a `Finish`-tagged target should be configured.

---

## Evidence to Attach in PR
- Inspector screenshot of `LevelStorySequenceController` step configuration.
- In-game screenshot(s) showing prompt actions.
- Console screenshot showing accepted/ignored logs and step progression.
- Optional screenshot/log showing CorgiSense enable/disable transitions.

