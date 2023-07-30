using UnityEngine;

public class SpikeKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            player.Die();
    }
}
