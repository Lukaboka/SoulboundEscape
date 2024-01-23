using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
	[SerializeField] private PowerupType powerup;
	[SerializeField] private PowerupStrength strength;

	[SerializeField] private ParticleSystem particle2;

	[SerializeField] private ParticleSystem claimedParticle;

	private bool isClaimed = false;

    private void Awake()
    {
        switch(strength)
        {
			case PowerupStrength.low:
				break;
			case PowerupStrength.mid:
				break;
			case PowerupStrength.high:
				break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		if (isClaimed) { return; }
        if (other.TryGetComponent<Health>(out Health health))
        {
			isClaimed = true;
			particle2.gameObject.SetActive(false);
			var player = health.GetComponentInParent<PlayerCombat>();
			player.Powerup(powerup, strength);
			claimedParticle.gameObject.SetActive(true);
			Destroy(gameObject, claimedParticle.main.duration);
        }
    }
}


public enum PowerupType
{
	Speed,
	Damage,
	Health
}

public enum PowerupStrength
{
	low,
	mid,
	high
}