using System.Collections;
using UnityEngine;

public abstract class SlideableViewModel<T> : ViewModel<T> where T : Model
{
    [SerializeField] private EditorLocalTransform disabledTransformDifference = EditorLocalTransform.Zero;

    [SerializeField] protected float duration = 0.5f;
    [SerializeField] protected bool disableWhenDeactivated = true;

    [Tooltip("Changes whether this item starts enabled or disabled")]
    [SerializeField] protected bool isActive;

    private EditorLocalTransform disabledTransform = EditorLocalTransform.Identity;
    private EditorLocalTransform enabledTransform = EditorLocalTransform.Identity;

    private EditorLocalTransform startTransform;
    private EditorLocalTransform endTransform;
    private float timeSinceMoveStart = float.MaxValue;

    public void SetActive(bool active)
    {
        isActive = active;
        if (active)
        {
            startTransform = disabledTransform;
            endTransform = enabledTransform;
        }
        else
        {
            startTransform = enabledTransform;
            endTransform = disabledTransform;
        }
        timeSinceMoveStart = 0;

        RefreshGameObjectActive();
    }

    protected override void OnModelChanged()
    {
        var isNowActive = IsActive();
        if (isActive != isNowActive)
        {
            SetActive(isNowActive);
        }
    }

    protected abstract bool IsActive();

    private void Start()
    {
        enabledTransform = new EditorLocalTransform(this.transform);
        disabledTransform = enabledTransform + disabledTransformDifference;

        if (isActive)
        {
            transform.UpdateFromEditorLocalTransform(enabledTransform);
        }
        else
        {
            transform.UpdateFromEditorLocalTransform(disabledTransform);
        }

        RefreshGameObjectActive();
    }

    private void Update()
    {
        if (timeSinceMoveStart < duration)
        {
            timeSinceMoveStart += Time.unscaledDeltaTime;
            if (timeSinceMoveStart > duration)
            {
                timeSinceMoveStart = duration;
            }

            var percentComplete = timeSinceMoveStart / duration;
            var nextTransform = GetNextTransform(startTransform, endTransform, percentComplete);
            transform.UpdateFromEditorLocalTransform(nextTransform);
        }
    }

    private void RefreshGameObjectActive()
    {
        if (disableWhenDeactivated)
        {
            if (isActive)
            {
                // we cannot run a coroutine on an inactive gameobject, so set it active with no delay
                gameObject.SetActive(isActive);
            }
            else
            {
                StartCoroutine(SetGameObjectActiveAfterDelay(isActive, duration));
            }
        }
    }

    private IEnumerator SetGameObjectActiveAfterDelay(bool active, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        gameObject.SetActive(active);
    }

    protected abstract EditorLocalTransform GetNextTransform(EditorLocalTransform startTransform, EditorLocalTransform endTransform, float percentComplete);
}
