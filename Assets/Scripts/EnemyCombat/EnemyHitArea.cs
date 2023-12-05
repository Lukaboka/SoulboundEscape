using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitArea : MonoBehaviour
{
    [SerializeField] private bool isPlayerInHitArea = false;

    private PlayerCombat _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCombat>(out PlayerCombat player))
        {
            _player = player;
            isPlayerInHitArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerCombat>(out PlayerCombat player))
        {
            _player = null;
            isPlayerInHitArea = false;
        }
    }

    public PlayerCombat GetIfPlayerIsInHitbox()
    {
        return _player;
    }
}
