using System.Collections;
using UnityEngine;

public class InfoModel : Model
{
    [SerializeField] private float messageDuration = 3;

    private string _infoTitle;
    public string InfoTitle 
    {
        get
        {
            return _infoTitle;
        }
        set
        {
            _infoTitle = value;
            Refresh();
        }
    }

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

    public void ShowMessage(string title, string text, EditorLocalTransform arrowTransform)
    {
        _infoTitle = title;
        _infoText = text;
        _arrowTransform = arrowTransform;

        StartCoroutine(ToggleInfoMessageVisibility(messageDuration));
    }

    private IEnumerator ToggleInfoMessageVisibility(float duration)
    {
        ShowingInfo = true;
        yield return new WaitForSecondsRealtime(duration);
        ShowingInfo = false;
    }
}
