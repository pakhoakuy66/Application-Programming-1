using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerInRange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInChildren<EnemyAI>().IsPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInChildren<EnemyAI>().IsPlayerInRange = false;
        }
    }
}
