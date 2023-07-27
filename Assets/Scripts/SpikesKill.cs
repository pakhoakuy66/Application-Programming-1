using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class SpikeKill : MonoBehaviour
{
    [SerializeField] private float stopCamDelay = .5f;

    private IEnumerator WaitAndExecute(float waitTime, Action callBack)
    {
        yield return new WaitForSeconds(waitTime);
        callBack.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.Die();
            CinemachineVirtualCamera vcam = player.GetComponentInChildren<CinemachineVirtualCamera>();
            StartCoroutine(WaitAndExecute(stopCamDelay, () => vcam.Follow = vcam.transform.parent = null));
        }
    }
}
