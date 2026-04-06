using System.Collections;
using UnityEngine;

public class InfoModel : Model
{
    [SerializeField] private float messageDuration = 3;

    private string _infoText;
    public string InfoText
    {
        get
        {
            return _infoText;
        }
        set
        {
            _infoText = value;
            Refresh();
        }
    }

    private bool _showingInfo;
    public bool ShowingInfo
    {
        get
        {
            return _showingInfo;
        }
        set
        {
            _showingInfo = value;
            Refresh();
        }
    }

    private bool _showArrow;
    public bool ShowArrow
    {
        get
        {
            return _showArrow;
        }
        set
        {
            _showArrow = value;
            Refresh();
        }
    }

    private EditorLocalTransform _arrowTransform;
    public EditorLocalTransform ArrowTransform
    {
        get
        {
            return _arrowTransform;
        }
        set
        {
            _arrowTransform = value;
            Refresh();
        }
    }

    public void ShowMessage(string text, EditorLocalTransform arrowTransform = default, bool showArrow = false)
    {
        _infoText = text;
        _arrowTransform = arrowTransform;
        _showArrow = showArrow;

        Refresh();
        StartCoroutine(ToggleInfoMessageVisibility(messageDuration));
    }

    private IEnumerator ToggleInfoMessageVisibility(float duration)
    {
        ShowingInfo = true;
        yield return new WaitForSeconds(duration);
        ShowingInfo = false;
    }
}
