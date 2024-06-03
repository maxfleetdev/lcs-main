using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerCache.CachePlayer(this.transform);
    }
}