using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElToro
{
    public class KickLogic : MonoBehaviour
    {
        [SerializeField] int damageAmount;
        [SerializeField] float knockbackForce = 1;
        [SerializeField] GameObject hitEffectPrefab;
        [SerializeField] AudioClipCollection hitSounds;

        public UnityEvent OnContact;
        public UnityEvent OnSuccessfulHit;

        private void OnTriggerEnter(Collider other)
        {
            OnContact?.Invoke();

            if (other.GetComponent<Damageable>())
            {
                //Shoot the player up and away
                var trajectory = -other.transform.forward;
                Quaternion rotation = Quaternion.AngleAxis(45, other.transform.right); // Rotate 45 degrees around the right axis
                trajectory = rotation * trajectory; // Apply rotation to the forward vector
                Damage kick = new Damage();
                kick.amount = damageAmount;
                kick.direction = trajectory;
                kick.knockbackForce = knockbackForce;

                if (other.GetComponent<Damageable>().Hit(kick))
                {
                    OnSuccessfulHit?.Invoke();

                    if (hitEffectPrefab != null)
                    {
                        Instantiate(hitEffectPrefab, other.transform.position, Quaternion.identity);
                    }

                    if (hitSounds != null)
                        SoundEffectsManager.instance.PlayRandomClip(hitSounds.clips, true);
                }
            }
        }
    }
}
