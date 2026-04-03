# Level Story Event Framework — Phase 1  
# Step-by-Step Testing Workflow

Use this doc to **build and test the framework one feature at a time**. Each step adds a small piece and tells you **how to test it** before moving on. For full script code, use **LevelStoryEventFramework_Phase1_FullGuideAndScripts.md**.

---

## Before You Start

- **Folder:** Create `Assets/Scripts/StoryEvent/`.
- **Scene:** Create or use a sandbox scene in `Assets/Scenes/Sandboxes/` (e.g. **StoryEventSandbox** or **CleanScene**). Do all testing here.
- **Reference:** Keep the Full Guide doc open for copying script code when a step says "implement as in Full Guide."

---

## Step 1 — GameplaySignal (raise and subscribe)

**Goal:** Confirm the signal hub works in isolation.

**What to do:**
1. Create `Assets/Scripts/StoryEvent/GameplaySignal.cs` with the code from the Full Guide (static class with `Raise`, `Subscribe`, `Unsubscribe`, and constant `ThrustUsedSignalId`).
2. Create a **tiny test script** `Assets/Scripts/StoryEvent/Tests/StoryEventTestRunner.cs` and attach it to an empty GameObject in sandbox scene (e.g. object name: `StoryEventTestRunner`).
3. Use this exact test flow in that script:
   - Add a private bool field: `private bool _subscribed;`
   - In `OnEnable()`:
     - `GameplaySignal.Subscribe(GameplaySignal.ThrustUsedSignalId, OnThrustSignalReceived);`
     - set `_subscribed = true;`
   - In `OnDisable()`:
     - if `_subscribed`, call `GameplaySignal.Unsubscribe(GameplaySignal.ThrustUsedSignalId, OnThrustSignalReceived);`
     - set `_subscribed = false;`
   - Add callback method:
     - `private void OnThrustSignalReceived() { Debug.Log("StoryEventTestRunner: ThrustUsed received"); }`
   - In `Update()`:
     - if `Input.GetKeyDown(KeyCode.T)`, call `GameplaySignal.Raise(GameplaySignal.ThrustUsedSignalId);`
     - add `Debug.Log("StoryEventTestRunner: Raised ThrustUsed");` before or after `Raise(...)` so you can verify both sides.
4. In sandbox scene:
   - Create an empty GameObject under root: `StoryEventTesting`.
   - Add component `StoryEventTestRunner`.
   - Save scene.
5. Keep this test script temporary for Step 1 only. Remove component (or disable object) after Step 1 passes to avoid noisy logs in later steps.

**How to test:**
- Enter Play mode.
- Press **T** once:
  - Console should show **"StoryEventTestRunner: Raised ThrustUsed"**.
  - Console should also show **"StoryEventTestRunner: ThrustUsed received"**.
- Press **T** multiple times: each key press should produce one pair of logs.
- Exit Play mode:
  - No errors in Console about null handlers or invalid unsubscribe.
- **Pass:** Signal is raised and received reliably, no exceptions on enter/exit Play mode.

**Suggested commit name (Step 1):**
- `feat(story-event): add GameplaySignal and isolated signal test runner`

---

## Step 2 — Story types and context (data only)

**Goal:** Add the enums and context type so later steps compile. No runtime test yet.

**What to do:**
1. Create `StoryTypes.cs` (enums `StoryMode`, `CompletionType`) — copy from Full Guide.
2. Create `StoryStepContext.cs` (class with Player, UIManager, Controller) — copy from Full Guide.

**How to test:**
- Project compiles. No errors. You can skip Play mode for this step.

---

## Step 3 — One trigger: EnterZoneTrigger + controller stub

**Goal:** Entering a zone calls the controller; controller only accepts the "current" step.

**What to do:**
1. Create `StoryTriggerBase.cs` (abstract, with `controller`, `stepIndex`, `NotifyController()`) — from Full Guide.
2. Create `EnterZoneTrigger.cs` (OnTriggerEnter, tag "Player", NotifyController) — from Full Guide.
3. Create a **minimal** `LevelStorySequenceController.cs` that only has:
   - `[SerializeField] int currentStepIndex = 0;`
   - `[SerializeField] Player player;` and `[SerializeField] UIManager uiManager;` (for later)
   - `public void NotifyTriggerFired(int stepIndex)` that does:  
     `if (stepIndex == currentStepIndex) Debug.Log("Trigger accepted for step " + stepIndex);`  
     `else Debug.Log("Trigger ignored (current step " + currentStepIndex + ", got " + stepIndex + ")");`

**Scene setup:**
- 1) Open your sandbox scene (`StoryEventSandbox` or `CleanScene`).
- 2) Create an empty root object: `StoryEventSystem`.
- 3) Add component `LevelStorySequenceController` to `StoryEventSystem`.
- 4) In Inspector for `LevelStorySequenceController`:
  - Assign the scene's **Player** object to `player`.
  - `uiManager` can be left unassigned for **Step 3** (trigger-gate test only). Assign it in **Step 5** when testing ShowPromptAction.
  - Keep `currentStepIndex = 0` in the script for this test.
- 5) Create a trigger object:
  - Hierarchy -> Create 3D Object -> Cube
  - Rename to `Step0_EnterZone`
  - Set Transform so player can pass through it (example: scale x=6, y=3, z=6; place on player path).
  - In Cube Collider, check **Is Trigger**.
- 6) Add `EnterZoneTrigger` to `Step0_EnterZone`.
- 7) In `EnterZoneTrigger` Inspector:
  - `controller` -> drag `StoryEventSystem` object (the one with `LevelStorySequenceController`).
  - `stepIndex` -> set to `0`.
  - `requiredTag` -> set to `Player` (must match your player GameObject tag).
- 8) Verify player setup:
  - Player object has a Collider and Rigidbody (already true in most gameplay scenes).
  - Player tag is exactly `Player`.
- 9) Save scene before testing.

**How to test:**
- **Test A (positive case):**
  1. Enter Play mode.
  2. Move player into `Step0_EnterZone`.
  3. Expected Console log: **"Trigger accepted for step 0"**.
- **Test B (negative gate case):**
  1. Stop Play mode.
  2. Change `EnterZoneTrigger.stepIndex` from `0` to `1`.
  3. Enter Play mode and move player into same trigger.
  4. Expected Console log: **"Trigger ignored (current step 0, got 1)"**.
- **Test C (tag filter check):**
  1. Stop Play mode.
  2. Keep `stepIndex = 0`.
  3. Temporarily change `requiredTag` to something invalid (example: `NotPlayer`).
  4. Enter Play mode and move through trigger.
  5. Expected: **no accepted log** (trigger should not fire for player).
  6. Revert `requiredTag` back to `Player`.
- **Pass criteria:**
  - With `stepIndex=0` and `requiredTag=Player`, trigger is accepted.
  - With wrong `stepIndex`, trigger is ignored.
  - With wrong tag, trigger does not fire.
  - No null reference errors in Console.
- **After pass (cleanup for next steps):**
  - Reset `EnterZoneTrigger.stepIndex` to `0`.
  - Keep `Step0_EnterZone` in scene; you will reuse it for Step 4+.

---

## Step 4 — StoryStep data and step list on controller

**Goal:** Controller has a list of steps; you can see and edit them in the Inspector.

**What to do:**
1. Create `StoryStep.cs` from Full Guide (triggerStepIndex, completionType, completionTimerDuration, completionSignalId, requestedMode, actions list, runImmediately).
2. Add a **minimal** `StoryActionBase.cs` (abstract `Execute(StoryStepContext)`) so `StoryStep.actions` compiles. You will not run actions until Step 5; the list can stay **empty** in the Inspector for every step.
3. In `LevelStorySequenceController`, add `[SerializeField] List<StoryStep> steps = new List<StoryStep>();` and `using System.Collections.Generic;` at the top.
4. In `Start()`, set `currentStepIndex = 0` (use **one** field for the active step; do not keep a second `_currentStepIndex` that `NotifyTriggerFired` does not use).
5. Keep your existing `NotifyTriggerFired` logic (still compares `stepIndex` to `currentStepIndex`). Step 4 does **not** yet read `steps[_currentStepIndex]` — that starts in Step 5.

**Inspector setup (Step 4):**
1. Select the GameObject with `LevelStorySequenceController` (e.g. `StoryEventSystem`).
2. In the Inspector, find the **Steps** field (list).
3. Set **Size** to `3` (or use **+** three times) so you have elements `0`, `1`, `2`.
4. For each element, **leave `Actions` Size = 0** (no action components yet). That is valid for Step 4.
5. Use this as a **recommended smoke-test layout** (proves different enums/fields serialize correctly):

| Element | `triggerStepIndex` | `completionType` | `completionTimerDuration` | `completionSignalId` | `requestedMode` | `runImmediately` |
|---------|-------------------|------------------|-------------------------|------------------------|-----------------|------------------|
| 0 | 0 | `Instant` | (any) | default | `Gameplay` | **false** |
| 1 | 1 | `AfterTimer` | **2** | default | `GuidedGameplay` | **true** |
| 2 | 2 | `AfterGameplaySignal` | (any) | `ThrustUsed` (or `GameplaySignal.ThrustUsedSignalId` default if you match code) | `Cutscene` | **false** |

6. **Save the scene** (`Ctrl+S`). Step 4 is about **data on disk**, not new runtime behavior.

**How to test:**

**Test A — Inspector serialization**
1. Select `StoryEventSystem` (controller object).
2. Confirm you see **Steps** with **3** entries and the fields above are readable/editable.
3. Change **Step 1 → completionTimerDuration** to `5`, **Save** scene, **close Unity** (optional), reopen scene, reload project — confirm value is still **5** (or at minimum: save / leave scene / re-enter scene and confirm value persisted).

**Test B — Project compiles**
1. From Unity menu: ensure **no Console errors** after Unity recompiles scripts.
2. If you see errors about `StoryActionBase`, confirm `StoryActionBase.cs` exists and `StoryStep` uses the correct `GameplaySignal` constant name (`ThrustUsedSignalId`).

**Test C — Step 3 trigger behavior unchanged**
1. Confirm `EnterZoneTrigger` on `Step0_EnterZone` still has **controller** = your `LevelStorySequenceController` object and **stepIndex** = `0`.
2. Confirm `LevelStorySequenceController` has **currentStepIndex** = `0` in the Inspector (serialized default).
3. Enter **Play** mode. Move player into **Step0_EnterZone** once.
4. **Expected Console:** `Trigger accepted for step 0` (same as Step 3).
5. Stop Play mode. Set `EnterZoneTrigger.stepIndex` to **1**, enter Play, walk into zone again.
6. **Expected Console:** `Trigger ignored (current step 0, got 1)`.
7. Restore `EnterZoneTrigger.stepIndex` to **0**.

**Test D — Steps list does not drive behavior yet (expectation check)**
1. In the Inspector, give **Step 0** a weird `requestedMode` (e.g. `Cutscene`) and `completionType` `AfterTimer` with duration `99`.
2. Enter Play mode and trigger the zone once (`Trigger accepted for step 0`).
3. **Expected:** **No timer**, **no mode change**, **no reading of `steps[0]`** — behavior is still only the Debug.Log from `NotifyTriggerFired`. That confirms Step 4 scope: **data only** until Step 5–6 wire actions and completion.

**Pass criteria:**
- Three (or more) `StoryStep` entries visible and editable; **Actions** lists can be empty.
- Scene saves without losing step data.
- No compile errors.
- **Test C** passes: accepted vs ignored logs match Step 3.
- **Test D** passes: changing step fields does **not** yet change gameplay (by design).

**Common issues:**
- **`steps` list missing in Inspector:** ensure the field is `[SerializeField]` and `public` or private with `[SerializeField]` on `List<StoryStep>`.
- **Duplicate step index fields on controller:** only **one** `currentStepIndex` should gate `NotifyTriggerFired`; `Start()` should reset that same field.
- **`List<StoryActionBase>` compile error:** add minimal `StoryActionBase` (abstract `Execute`) even if you assign zero actions.
- **Enum names must match `StoryStep` defaults** (e.g. `CompletionType.Instant`, not `instant`) or scripts won’t compile and the Inspector may hide new fields until fixed.

**Suggested commit (Step 4 — detailed):**

```
feat(story-event): add StoryStep model and serialized steps list on sequence controller

- Add StoryStep serializable data (completion type, timer/signal fields, requested mode, runImmediately, actions list).
- Add minimal StoryActionBase so StoryStep.actions compiles; action lists stay empty until Step 5.
- Extend LevelStorySequenceController with List<StoryStep> and single currentStepIndex reset in Start.
- Keep NotifyTriggerFired as Step-3 gate only (does not read steps[] yet).
- Update StoryEventSandbox: 3 sample steps in Inspector for serialization smoke test.

Files: StoryStep.cs, StoryActionBase.cs, StoryTypes.cs (enum align), LevelStorySequenceController.cs, StoryEventSandbox.unity
```

---

## Step 5 — Run one action when trigger fires (ShowPrompt)

**Goal:** When step 0’s trigger fires, one action runs: show a prompt.

**What to do:**
1. Ensure `StoryActionBase.cs` exists (added in Step 4). If missing, create it from Full Guide.
2. Create `ShowPromptAction.cs` (title, text, call `UIManager.ShowInfoText`) — from Full Guide.
3. Create `StoryStepContext.cs` if you didn’t in Step 2 (Player, UIManager, Controller).
4. In `LevelStorySequenceController`:
   - Build a `StoryStepContext` in `Awake`/`Start` (Player, UIManager, Controller = this).
   - In `NotifyTriggerFired`, when `stepIndex == currentStepIndex`: get `steps[currentStepIndex]`, run each action in `step.actions` with that context (call `action.Execute(context)`).

**Scene setup:**
1. Add **UIManager** to the sandbox if missing — drag prefab `Assets/Objects/PuzzlePieces/SkinDB/SupportPrefabs/UIObject.prefab` into the scene (or duplicate a level’s UI root that contains `UIManager`).
2. On `LevelStorySequenceController`, assign **Player** and **UI Manager** (the object with `UIManager` component).
3. Add **ShowPromptAction** on `StoryEventSystem` (or a child). Set **Title** / **Text** (e.g. `Step 0` / `You entered the zone!`).
4. In **Steps → Element 0 → Actions**, set **Size** to **1** and drag that `ShowPromptAction` component into **Element 0**.
5. Ensure **Element 0** still has **runImmediately** unchecked if you want the prompt only after entering the zone.
6. Save scene.

**How to test:**

**Test A — Prompt on accepted trigger**
1. Enter Play mode.
2. Walk into `Step0_EnterZone` (EnterZoneTrigger **stepIndex = 0**).
3. **Expected:** Info/tutorial UI shows your title and text (via `UIManager.ShowInfoText` → `InfoModel`).
4. **Expected Console:** no NullReferenceException on `ShowPromptAction.Execute`.

**Test B — Prompt does not run when trigger is ignored**
1. Stop Play mode.
2. Set `EnterZoneTrigger.stepIndex` to **1** while controller **currentStepIndex** is **0**.
3. Enter Play, walk into zone.
4. **Expected:** **No** prompt (trigger ignored); optional: only `"Trigger ignored..."` log from Step 3-style logging if you still log there.
5. Restore `EnterZoneTrigger.stepIndex` to **0**.

**Test C — Missing UIManager reference**
1. Temporarily clear **UI Manager** on the controller.
2. Enter Play, trigger zone.
3. **Expected:** either no prompt or a clear error in Console — confirms wiring depends on assignation. Re-assign UIManager before continuing.

**Pass criteria:**
- Prompt appears **only** when the zone trigger fires **and** `stepIndex == currentStepIndex`.
- `Player` + `UIManager` assigned on controller.
- Console free of unhandled exceptions during prompt.

**Suggested commit (Step 5 — detailed):**

```
feat(story-event): execute ShowPromptAction when current step trigger fires

- Add ShowPromptAction calling UIManager.ShowInfoText with EditorLocalTransform.Identity (or arrow).
- Build StoryStepContext in controller (Player, UIManager, Controller).
- On NotifyTriggerFired for current step: run steps[currentStepIndex].actions in order.
- Sandbox: wire UIObject prefab as UIManager; assign controller refs; Step 0 actions list includes ShowPromptAction.

Files: ShowPromptAction.cs, StoryStepContext.cs, LevelStorySequenceController.cs, StoryEventSandbox.unity
```

---

## Step 6 — Completion: Instant and AdvanceStep

**Goal:** When step 0 completes (Instant), the controller advances to step 1; step 1’s trigger can then be accepted.

**What to do:**
1. In `LevelStorySequenceController`, after running actions for the current step, check that step’s `completionType`.
2. If `CompletionType.Instant`, call a new method `AdvanceStep()` which: sets `currentStepIndex++`, then (for now) just `Debug.Log("Advanced to step " + currentStepIndex)`.
3. In `NotifyTriggerFired`, after running actions, call the completion logic (Instant → AdvanceStep).

**Scene setup (minimal):**
- **Step 0:** `completionType = Instant`, actions include `ShowPromptAction` (optional), `runImmediately = false`.
- **Step 1:** add a **second** enter zone (optional) or reuse the same volume: for reuse, after advancing, set `EnterZoneTrigger.stepIndex` to **1** manually in Editor **between** play sessions, or add `Step1_EnterZone` with `stepIndex = 1`.
- Easiest repeatable test: two trigger volumes — `Step0_EnterZone` (**stepIndex 0**), `Step1_EnterZone` (**stepIndex 1**) placed along the path.

**How to test:**

**Test A — Advance after Instant**
1. Enter Play with **currentStepIndex** starting at **0**.
2. Enter **Step0_EnterZone**. Prompt may show (Step 5). **Expected Console:** log like `Advanced to step 1` (or `currentStepIndex is now 1` — use your exact message).
3. **Expected:** entering **Step0_EnterZone** again while on step 1 should **not** run step 0 actions again (ignore or no prompt); verify with logs.

**Test B — Step 1 trigger accepted**
1. From step 1, walk into **Step1_EnterZone** (stepIndex **1**).
2. **Expected:** `"Trigger accepted for step 1"` (or your equivalent) and step 1 actions run if configured.

**Test C — Old step trigger ignored**
1. On **currentStepIndex == 1**, walk into **Step0_EnterZone** only.
2. **Expected:** controller ignores **0** or does not treat it as current step — `"Trigger ignored (current step 1, got 0)"` if you still log that path.

**Pass criteria:**
- `currentStepIndex` increments exactly **once** per Instant completion for that step (no double `AdvanceStep` from one trigger without guarding — add a `completionStarted` flag if you see double advance).
- Trigger gating matches the **new** current step after advance.

**Suggested commit (Step 6 — detailed):**

```
feat(story-event): add Instant completion and AdvanceStep after step actions

- After running actions for current step, evaluate completionType == Instant → AdvanceStep().
- AdvanceStep increments currentStepIndex and logs or enters next step (per your implementation).
- Optional: guard so completion only runs once per step entry (flag/clear on advance).
- Sandbox: two EnterZone triggers for step 0 and step 1 gates.

Files: LevelStorySequenceController.cs, StoryEventSandbox.unity (extra trigger / wiring)
```

---

## Step 7 — Completion: AfterTimer

**Goal:** A step can complete after a delay (e.g. 3 seconds) instead of instantly.

**What to do:**
1. In the controller, when `completionType == AfterTimer`, start a coroutine: wait `completionTimerDuration` seconds, then call `AdvanceStep()`.
2. Use the full controller completion logic from the Full Guide (so you don’t double-advance): e.g. a `_currentStepCompletionStarted` flag and only start completion once per step.

**Scene setup:**
- Add a step (e.g. step 1) with completion type **AfterTimer**, duration 2. No trigger needed if you set `runImmediately` true for that step (so when you advance to step 1, it runs immediately and then waits 2s). Or keep step 1 trigger-based and completion AfterTimer 2.

**How to test:**

**Test A — runImmediately + AfterTimer**
1. Configure **Step 1:** `runImmediately = true`, `completionType = AfterTimer`, `completionTimerDuration = 2`, actions optional (e.g. second ShowPrompt).
2. Enter Play. After Step 0 completes (Instant), Step 1 should **auto-start** (no new trigger needed if your controller calls the same “enter step” path on advance).
3. Start a timer when step 1 begins (or when actions finish). **Expected:** ~**2 seconds** later, `AdvanceStep()` runs and **currentStepIndex** becomes **2** (or logs show advance).

**Test B — Timer does not double-fire**
1. Watch Console: you should **not** see two advance logs for the same step from one entry. If you do, add `_currentStepCompletionStarted` / reset on advance (see Full Guide).

**Test C — AfterDialogueDuration (optional same frame as AfterTimer)**
1. If your enum has `AfterDialogueDuration`, implement it as the same coroutine wait using `completionTimerDuration` set to match `InfoModel` message length (~3s).
2. Show prompt, wait duration, then advance.

**Pass criteria:**
- Delay matches `completionTimerDuration` within reasonable frame tolerance.
- No duplicate advances for a single step activation.

**Suggested commit (Step 7 — detailed):**

```
feat(story-event): add AfterTimer completion with coroutine and single-fire guard

- On AfterTimer / AfterDialogueDuration: start WaitForSeconds(completionTimerDuration) then AdvanceStep.
- Use _currentStepCompletionStarted (or equivalent) to prevent double completion per step.
- Sandbox: Step 1 runImmediately + AfterTimer to validate auto-advance without second trigger.

Files: LevelStorySequenceController.cs, StoryEventSandbox.unity
```

---

## Step 8 — ThrustUsed signal and AfterGameplaySignal completion

**Goal:** A step completes when the player uses thrust (gameplay signal).

**What to do:**
1. In **PlayerThrustState** (where thrust is applied), add `GameplaySignal.Raise(GameplaySignal.ThrustUsedSignalId);` as in the Full Guide (e.g. when you call `thrust()` and use fuel).
2. In the controller, when `completionType == AfterGameplaySignal`, subscribe to `step.completionSignalId` with a callback that calls `AdvanceStep()` and unsubscribes (use the Full Guide’s pattern with `_subscribedCompletionSignalId` so you unsubscribe the right handler).

**Scene setup:**
- Add a step (e.g. step 2) with completion **AfterGameplaySignal**, signal ID **"ThrustUsed"**. No trigger or runImmediately so the step is already “entered” when you advance to it (e.g. step 1 runs ShowPrompt "Press thrust", completion AfterTimer; step 2 has no trigger, runImmediately = true, completion AfterGameplaySignal "ThrustUsed").

**How to test:**

**Test A — Signal subscription**
1. Configure a step (e.g. Step 2) with `runImmediately = true`, `completionType = AfterGameplaySignal`, `completionSignalId = ThrustUsed` (must match `GameplaySignal.ThrustUsedSignalId` value `"ThrustUsed"`).
2. Enter Play, advance until that step becomes active.
3. **Expected:** controller subscribes once (no spam in Console). Press / hold thrust until `PlayerThrustState` raises the signal.

**Test B — Advance on thrust**
1. After thrust, **Expected:** `AdvanceStep()` runs, `currentStepIndex` increments, subscription is **removed** (no further thrust advances until next signal-completion step).

**Test C — Wrong signal ID**
1. Set `completionSignalId` to `"WrongId"`. Thrust should **not** complete the step. Raise correct id only via gameplay or revert field.

**Test D — Fuel / input**
1. Ensure player has fuel and input enabled so `PlayerThrustState` can run; if thrust never triggers, fix `InputDriver` / `JetpackActivationPossible` first.

**Pass criteria:**
- Thrust raises `GameplaySignal.Raise(ThrustUsedSignalId)` from player code.
- Step completes only when the configured signal id is raised.
- No subscription leak (OnDisable / unsubscribe on advance).

**Suggested commit (Step 8 — detailed):**

```
feat(story-event): complete steps via AfterGameplaySignal; raise ThrustUsed from thrust

- PlayerThrustState: GameplaySignal.Raise(ThrustUsedSignalId) when thrust applies (per Full Guide).
- LevelStorySequenceController: subscribe to step.completionSignalId; unsubscribe via tracked id; AdvanceStep on signal.
- Sandbox: step uses completionSignalId ThrustUsed; validate advance after thrust.

Files: PlayerThrustState.cs, LevelStorySequenceController.cs, StoryEventSandbox.unity
```

---

## Step 9 — Lock / Unlock controls (actions + bridge)

**Goal:** Story can disable and re-enable player input.

**What to do:**
1. Create `StoryGameplayBridge.cs` (ApplyMode: Gameplay/GuidedGameplay/CorgiSense → EnableInput; Cutscene/PromptMode → DisableInput) — from Full Guide.
2. Create `LockControlsAction.cs` and `UnlockControlsAction.cs` (call `player.input.DisableInput()` / `EnableInput()`) — from Full Guide.
3. Add the bridge component to the scene (e.g. on the controller GameObject). Assign Player. In the controller, add a `[SerializeField] StoryGameplayBridge bridge` and assign it.
4. When running step actions, after running the action list, call `bridge.ApplyMode(step.requestedMode)` so the requested mode is applied per step.
5. Create `RequestGameplayStateAction.cs` if you want steps to request mode via an action (optional for this test).

**Scene setup:**
- One step: add **LockControlsAction**. Requested mode for that step = Cutscene. Enter trigger → prompt (if you have one) and **input should be disabled** (player doesn’t move/thrust).
- Next step: add **UnlockControlsAction** and requested mode Gameplay. After that step runs, **input should work again**.

**How to test:**

**Test A — LockControlsAction**
1. Add a step with **LockControlsAction** in **actions**, and/or set `requestedMode = Cutscene` so **StoryGameplayBridge** disables input.
2. Enter Play, advance until that step runs.
3. **Expected:** `InputDriver` stops updating amalgam input (no thrust / no rotation from input). Player should feel “frozen” for gameplay controls.

**Test B — UnlockControlsAction**
1. Next step: **UnlockControlsAction** and `requestedMode = Gameplay` (or bridge enables input).
2. **Expected:** thrust and rotation work again.

**Test C — Order of operations**
1. If you both **LockControlsAction** and **ApplyMode(Cutscene)** run, confirm intended order (usually run actions first, then ApplyMode, or document your order). Behavior should match design — no flicker one frame with input on.

**Pass criteria:**
- Lock and unlock are repeatable across multiple sequences.
- No NullReference on `player.input` (assign Player on controller **and** on bridge if required).

**Suggested commit (Step 9 — detailed):**

```
feat(story-event): add StoryGameplayBridge and lock/unlock input actions

- StoryGameplayBridge.ApplyMode maps StoryMode to InputDriver.EnableInput/DisableInput.
- LockControlsAction / UnlockControlsAction invoke player input disable/enable.
- Controller applies requestedMode after actions (or documented order).
- Sandbox: dedicate steps to lock then unlock and verify thrust/rotation.

Files: StoryGameplayBridge.cs, LockControlsAction.cs, UnlockControlsAction.cs,
       LevelStorySequenceController.cs, StoryEventSandbox.unity
```

---

## Step 10 — Enable / Disable CorgiSense (actions only)

**Goal:** Story can turn CorgiSense on/off.

**What to do:**
1. Create `EnableCorgiSenseAction.cs` and `DisableCorgiSenseAction.cs` (set `SaveManager.Instance.collectibleData.CorgiSenseEnabled`) — from Full Guide.
2. Add these components in the scene and add them to a step’s actions list.

**How to test:**

**Test A — Enable**
1. Note initial value: `SaveManager.Instance.collectibleData.CorgiSenseEnabled` (Inspector on asset if exposed, or add temporary `Debug.Log` in action).
2. Run step with **EnableCorgiSenseAction**.
3. **Expected:** flag becomes **true**; if sandbox includes CorgiSense UI + **Finish** tag object, UI may appear — not all sandboxes have Finish; flag toggle is enough for **Pass**.

**Test B — Disable**
1. Run step with **DisableCorgiSenseAction**. **Expected:** flag **false**.

**Test C — Persistence (optional)**
1. If you call `Save()` elsewhere, document whether story should persist setting — for sandbox, in-memory toggle only is OK.

**Pass criteria:**
- Actions set `CollectibleData.CorgiSenseEnabled` without null SaveManager reference.
- No crash when SaveManager exists in scene (see Step 3 sandbox checklist).

**Suggested commit (Step 10 — detailed):**

```
feat(story-event): add EnableCorgiSense and DisableCorgiSense story actions

- EnableCorgiSenseAction / DisableCorgiSenseAction set SaveManager.Instance.collectibleData.CorgiSenseEnabled.
- Sandbox: steps toggle flag; verify in Inspector or debug log (optional UIObject with CorgiSense HUD if present).

Files: EnableCorgiSenseAction.cs, DisableCorgiSenseAction.cs, StoryEventSandbox.unity
```

---

## Step 11 — Optional: TimerTrigger and ManualTrigger

**Goal:** Triggers that don’t need a zone (timer or manual call).

**What to do:**
1. Create `TimerTrigger.cs` and `ManualTrigger.cs` from the Full Guide.
2. In the scene, add a TimerTrigger (assign controller, step index, delay). Or add ManualTrigger and call `Trigger()` from a UI button or key.

**Scene setup — TimerTrigger:**
1. Empty GameObject `TimerStepTrigger`, add `TimerTrigger`, assign `LevelStorySequenceController`, set `stepIndex` to match the step you want to **start** (usually current step awaiting external trigger — align with your controller design: either trigger fires **entry** to step or **completion**; document your choice).
2. Set `delaySeconds` to a small value (e.g. `1`).

**Scene setup — ManualTrigger:**
1. Add `ManualTrigger`, assign controller + `stepIndex`.
2. Wire **Trigger()** to a **UI Button** OnClick, or use a tiny test script `Input.GetKeyDown` that calls `manualTrigger.Trigger()` on the component.

**How to test:**

**Test A — TimerTrigger**
1. Enter Play when current step expects this trigger. **Expected:** after delay, `NotifyTriggerFired` runs **only if** `stepIndex == currentStepIndex`.

**Test B — ManualTrigger**
1. Press button / key. **Expected:** same acceptance rule as EnterZone; wrong step → ignored.

**Test C — Spam / repeat**
1. Spam ManualTrigger. **Expected:** controller should not break sequence (no double advance without guards).

**Pass criteria:**
- Timer and manual paths behave like EnterZoneTrigger for gating.
- Document whether trigger activates **step entry** vs **optional side effect** in your controller.

**Suggested commit (Step 11 — detailed):**

```
feat(story-event): add TimerTrigger and ManualTrigger story trigger primitives

- TimerTrigger: delay then NotifyController (configurable startOnEnable).
- ManualTrigger: public Trigger() + optional UnityEvent hookup.
- Sandbox: verify both respect currentStepIndex gating on LevelStorySequenceController.

Files: TimerTrigger.cs, ManualTrigger.cs, StoryEventSandbox.unity
```

---

## Step 12 — Full 5-step sandbox flow (end-to-end)

**Goal:** Run the full example flow once: zone → prompt → wait for thrust → success prompt → CorgiSense.

**What to do:**
1. Ensure all scripts from the Full Guide are in place (including controller completion logic, subscriptions, AdvanceStep, optional GameplaySignalTrigger if you use it).
2. In the sandbox scene, configure exactly 5 steps as in the Full Guide:
   - Step 0: EnterZoneTrigger, completion Instant, optional ShowPrompt.
   - Step 1: runImmediately true, ShowPrompt "Tilt left or right to aim, then hold thrust to fly.", completion AfterTimer 3.
   - Step 2: runImmediately true, UnlockControls (and optional prompt), completion AfterGameplaySignal "ThrustUsed".
   - Step 3: runImmediately true, ShowPrompt "Success!", completion AfterTimer 3.
   - Step 4: runImmediately true, EnableCorgiSense, completion Instant.
3. One EnterZoneTrigger in the scene for step 0. Assign controller and step index 0.

**Pre-flight checklist:**
- [ ] `SaveManager` + ground + **Player** in scene; **UIManager** (`UIObject.prefab`) assigned on controller.
- [ ] `EnterZoneTrigger` only on **Step 0** with **stepIndex 0**.
- [ ] Steps list has **5 elements** (indices **0–4** in `List` terms — the “5-step flow” in prose maps to list elements 0..4).
- [ ] `PlayerThrustState` raises **ThrustUsed** (Step 8).
- [ ] Remove or disable **StoryEventTestRunner** / extra debug keys that spam signals.
- [ ] Exactly **one AudioListener** active in scene (disable duplicate listeners from extra cameras/prefabs).

**How to test:**

**Test 1 — Happy path (in order)**
1. Enter Play with `currentStepIndex` at **0**.
2. Enter **Step0_EnterZone** → Step **0** actions run, **Instant** completion → advance to **1**.
3. Step **1** (`runImmediately` if configured): prompt **"Tilt left or right to aim, then hold thrust to fly."** (or your text) → **AfterTimer** ~**3s** → advance to **2**.
4. Step **2**: input **unlocked** if you added **UnlockControlsAction** → use thrust once → **AfterGameplaySignal `ThrustUsed`** → advance to **3**.
5. Step **3**: **"Success!"** prompt → **AfterTimer** ~**3s** → advance to **4**.
6. Step **4**: **EnableCorgiSenseAction** → **Instant** → sequence **done** (log “complete” if you add one).

**Test 2 — Sequence integrity (regression)**
1. During Step **1** or **2**, walk back into **Step0_EnterZone**.
2. **Expected:** **no** reset to step 0, **no** skipped indices — old trigger either **ignored** or produces no state change.
3. Optional: spam **EnterZoneTrigger** during step 0 — should not double-**AdvanceStep** if guarded.

**Test 3 — Console hygiene**
1. No **NullReferenceException** during full playthrough.
2. Unsubscribe **GameplaySignal** handlers when steps complete (no duplicate thrust advances).

**Pass criteria:**
- Full tutorial path completes in correct order without manual Inspector edits mid-play.
- Re-entering early triggers does **not** break order.
- Ready to copy sandbox wiring patterns into a real level (later phase).

**Suggested commit (Step 12 — detailed):**

```
test(story-event): end-to-end StoryEventSandbox five-step flow and regression checks

- Configure StoryEventSandbox steps 0–4: zone → timed prompt → thrust signal → success prompt → corgisense.
- Validate AdvanceStep order, AfterTimer pacing, ThrustUsed completion, and ignore stale EnterZone triggers.
- Document or remove temporary debug objects; ensure SaveManager + UIObject present for reliable runs.

Files: StoryEventSandbox.unity, LevelStorySequenceController.cs, PlayerThrustState.cs (if thrust wiring),
       optional story actions: ShowPromptAction, UnlockControlsAction, EnableCorgiSenseAction, etc.
```

---

## Summary: Testing workflow order

| Step | What you test |
|------|----------------|
| 1 | GameplaySignal raise/subscribe |
| 2 | Story types compile |
| 3 | EnterZoneTrigger + controller accept only current step |
| 4 | Step list in Inspector |
| 5 | One action (ShowPrompt) runs when trigger fires |
| 6 | Instant completion and AdvanceStep |
| 7 | AfterTimer completion |
| 8 | ThrustUsed signal and AfterGameplaySignal completion |
| 9 | Lock/Unlock controls via bridge and actions |
| 10 | Enable/Disable CorgiSense |
| 11 | TimerTrigger and ManualTrigger (optional) |
| 12 | Full 5-step flow and no sequence breaking |

## Suggested commit messages (Steps 1–12)

Use **one commit after each step passes**. Each step’s **full** subject + body is also written under that step above (Steps **4–12** expanded inline). Below is a **quick reference**; copy the block from the step section when you commit for richer history.

| Step | Subject (first line only) |
|------|----------------------------|
| 1 | `feat(story-event): add GameplaySignal and isolated signal test runner` |
| 2 | `feat(story-event): add StoryMode and CompletionType enums with step context` |
| 3 | `feat(story-event): add EnterZoneTrigger and minimal sequence controller trigger gate` |
| 4 | `feat(story-event): add StoryStep model and serialized steps list on sequence controller` |
| 5 | `feat(story-event): execute ShowPromptAction when current step trigger fires` |
| 6 | `feat(story-event): add Instant completion and AdvanceStep after step actions` |
| 7 | `feat(story-event): add AfterTimer completion with coroutine and single-fire guard` |
| 8 | `feat(story-event): complete steps via AfterGameplaySignal; raise ThrustUsed from thrust` |
| 9 | `feat(story-event): add StoryGameplayBridge and lock/unlock input actions` |
| 10 | `feat(story-event): add EnableCorgiSense and DisableCorgiSense story actions` |
| 11 | `feat(story-event): add TimerTrigger and ManualTrigger story trigger primitives` |
| 12 | `test(story-event): end-to-end StoryEventSandbox five-step flow and regression checks` |

**Commit format (recommended):**

```
<type>(<scope>): <short imperative summary>

- Bullet: what changed (feature / behavior).
- Bullet: scene or wiring (e.g. StoryEventSandbox.unity).
- Bullet: risk / follow-up if any.

Files: <paths>
```

**Types:** `feat` (new behavior), `test` (coverage / sandbox validation), `fix`, `docs`, `refactor`.

Use **LevelStoryEventFramework_Phase1_FullGuideAndScripts.md** for full script code. Use this doc to implement and test **one feature at a time**.
