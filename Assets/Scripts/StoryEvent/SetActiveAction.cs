using UnityEngine;

public class SetActiveAction : StoryActionBase
{
    [SerializeField] private GameObject[] activate;
    [SerializeField] private GameObject[] deactivate;

    public override void Execute(StoryStepContext context)
    {
        if (activate != null)
        {
            for (int i = 0; i < activate.Length; i++)
                if (activate[i] != null) activate[i].SetActive(true);
        }

        if (deactivate != null)
        {
            for (int i = 0; i < deactivate.Length; i++)
                if (deactivate[i] != null) deactivate[i].SetActive(false);
        }
    }
}
