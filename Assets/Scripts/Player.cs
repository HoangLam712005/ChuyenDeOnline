using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private Renderer objectRenderer;

    public override void Spawned()
    {
        objectRenderer = GetComponent<Renderer>();

        if (Object.HasStateAuthority)
        {
            // Tạo màu ngẫu nhiên trên server và đồng bộ cho tất cả client
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            RPCSetColor(randomColor);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPCSetColor(Color color, RpcInfo info = default)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = color;
        }
    }
}
