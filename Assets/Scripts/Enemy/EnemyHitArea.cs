using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitArea : MonoBehaviour
{
    private Health _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out Health player))
        {
            _player = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Health>(out Health player))
        {
            _player = null;
        }
    }

    public Health GetIfPlayerIsInHitbox()
    {
        return _player;
    }
}
