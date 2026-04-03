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

    private bool _useArrowTransform;

    public bool UseArrowTransform
    {
        get => _useArrowTransform;
        set
        {
            _useArrowTransform = value;
            Refresh();
        }
    }

    public void ShowMessage(string text, EditorLocalTransform arrowTransform, bool useArrowTransform)
    {
        _infoTitle = string.Empty;
        _infoText = text;
        _arrowTransform = arrowTransform;
        _useArrowTransform = useArrowTransform;

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
