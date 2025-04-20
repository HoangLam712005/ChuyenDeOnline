using UnityEngine;
using Fusion;

public class FireballBehaviour : NetworkBehaviour
{
    public int damage = 20;
    public float speed = 10f;
    public float lifetime = 5f;

    private PlayerRef owner;

    public void SetOwner(PlayerRef player)
    {
        owner = player;
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            // Hủy sau lifetime giây nếu không va chạm
            Invoke(nameof(DestroyFireball), lifetime);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            transform.position += transform.forward * speed * Runner.DeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;

        // Nếu trúng Player
        PlayerProperties target = other.GetComponent<PlayerProperties>();
        if (target != null && target.Object.InputAuthority != owner)
        {
            Debug.Log("Fireball trúng người chơi: " + target.name);
            target.RpcTakeDamage(damage);
            DestroyFireball();
        }
        else if (!other.isTrigger) // Va trúng tường, vật thể cứng
        {
            DestroyFireball();
        }
    }

    private void DestroyFireball()
    {
        if (Object != null && Object.IsValid)
        {
            Runner.Despawn(Object);
        }
    }
}
