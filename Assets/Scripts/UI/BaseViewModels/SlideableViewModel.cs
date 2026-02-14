using UnityEngine;

public abstract class SlideableViewModel<T> : ViewModel<T> where T : Model
{
    [SerializeField] private EditorLocalTransform disabledTransformDifference = EditorLocalTransform.Zero;

    [SerializeField] protected float duration = 0.5f;

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

    protected abstract EditorLocalTransform GetNextTransform(EditorLocalTransform startTransform, EditorLocalTransform endTransform, float percentComplete);
}
