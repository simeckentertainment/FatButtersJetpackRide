using UnityEngine;

public class LockThrustAction : StoryActionBase
{
    private const string TokenName = "[StoryThrustLock]";

    public override void Execute(StoryStepContext context)
    {
        if (context?.Player == null) return;

        var token = FindToken(context.Player);
        if (token == null)
        {
            var go = new GameObject(TokenName);
            go.transform.SetParent(context.Player.transform, false);
            var box = go.AddComponent<BoxCollider>();
            box.enabled = false;
            token = box;
        }

        if (!context.Player.CollidersInJetpackKillZone.Contains(token))
            context.Player.CollidersInJetpackKillZone.Add(token);
    }

    private static Collider FindToken(Player player)
    {
        for (int i = 0; i < player.CollidersInJetpackKillZone.Count; i++)
        {
            var c = player.CollidersInJetpackKillZone[i];
            if (c != null && c.gameObject.name == TokenName)
                return c;
        }
        var existing = player.transform.Find(TokenName);
        return existing != null ? existing.GetComponent<Collider>() : null;
    }
}
