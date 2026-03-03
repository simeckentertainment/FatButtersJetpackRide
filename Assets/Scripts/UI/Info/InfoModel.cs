using UnityEngine;

public class InfoModel : Model
{
    [SerializeField] public UIManager uiManager;

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

    public void SetText(string title, string text)
    {
        _infoTitle = title;
        _infoText = text;
        Refresh();
    }

    public void DismissInfoText()
    {
        uiManager.DismissInfoText();
    }
}
