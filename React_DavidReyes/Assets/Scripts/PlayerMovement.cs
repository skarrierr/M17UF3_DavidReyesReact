using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(NetworkTransform))]
public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Solo dueño envía petición de movimiento
        if (!IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
            return;

        // Llamamos al servidor para que mueva el objeto
        MoveServerRpc(h, v);
    }

    [ServerRpc]
    void MoveServerRpc(float horizontal, float vertical)
    {
        // Esto se ejecuta en el servidor (host)
        Vector3 delta = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        transform.position += delta;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"[PlayerMovement] Spawned. IsOwner = {IsOwner}, Owner = {OwnerClientId}, Local = {NetworkManager.Singleton.LocalClientId}");
    }
}
