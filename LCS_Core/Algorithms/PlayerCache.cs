using UnityEngine;

public static class PlayerCache
{
    public static Transform PlayerTransform { get; private set; }

    public static void CachePlayer(Transform player)
    {
        if (player == null) return;
        PlayerTransform = player;
    }
}