using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitArea : MonoBehaviour
{
    [SerializeField] private bool isPlayerInHitArea = false;

    private Health _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out Health player))
        {
            _player = player;
            isPlayerInHitArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Health>(out Health player))
        {
            _player = null;
            isPlayerInHitArea = false;
        }
    }

    public Health GetIfPlayerIsInHitbox()
    {
        return _player;
    }
}
