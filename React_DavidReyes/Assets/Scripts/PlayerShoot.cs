using Unity.Netcode;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    [Header("Raycast Settings")]
    public float shootRange = 50f;
    public int damage = 21;
    public Transform firePoint;      // Arrástralo en el inspector

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"[{OwnerClientId}] Intento de disparo desde jugador");
            TryShoot();
        }
    }

    void TryShoot()
    {
        // Origen y dirección desde el FirePoint
        Vector3 origin = firePoint.position;
        Vector3 direction = transform.forward;

        // Debug: dibuja el rayo 5s
        Debug.DrawRay(origin, direction * shootRange, Color.red, 5f);

        // Lanza el raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hit, shootRange))
        {
            Debug.Log($"[{OwnerClientId}] Impactó en: {hit.collider.name}");

            // Detecta PlayerStats en el objeto o su padre
            if (hit.collider.GetComponentInParent<PlayerStats>() is PlayerStats enemyStats)
            {
                if (enemyStats.OwnerClientId != OwnerClientId)
                {
                    Debug.Log($"[{OwnerClientId}] Dañando a jugador {enemyStats.OwnerClientId}");
                    enemyStats.TakeDamageServerRpc(damage, OwnerClientId);
                }
                else
                {
                    Debug.Log($"[{OwnerClientId}] Self-hit ignorado");
                }
            }
            else
            {
                Debug.Log($"[{OwnerClientId}] No era un jugador");
            }
        }
        else
        {
            Debug.Log($"[{OwnerClientId}] No impactó nada");
        }
    }
}
