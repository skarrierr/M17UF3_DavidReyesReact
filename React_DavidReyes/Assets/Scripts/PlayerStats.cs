using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    public const int MaxHealth = 100;
    public NetworkVariable<int> health = new NetworkVariable<int>(MaxHealth);
    public NetworkVariable<int> score = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            health.Value = MaxHealth;
            score.Value = 0;
        }
    }

    // Permitimos que otros (no solo el owner) invoquen este RPC:
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage, ulong attackerId)
    {
        if (!IsServer) return;

        health.Value -= damage;

        if (health.Value <= 0)
        {
            health.Value = MaxHealth;

            // Respawn
            Vector3 respawnPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            transform.position = respawnPos;

            // Sumar puntos al atacante
            PlayerStats attacker = FindPlayerById(attackerId);
            if (attacker != null)
                attacker.score.Value += 100;
        }
    }

    private PlayerStats FindPlayerById(ulong clientId)
    {
        foreach (var netObj in NetworkManager.Singleton.SpawnManager.SpawnedObjects.Values)
        {
            if (netObj.TryGetComponent(out PlayerStats stats) &&
                netObj.OwnerClientId == clientId)
                return stats;
        }
        return null;
    }
}
