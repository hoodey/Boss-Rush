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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            OnContact?.Invoke();

            if (other.GetComponent<Damageable>())
            {
                Vector3 dir = other.transform.position - transform.position;
                dir.Normalize();

                Debug.Log("test kick effect");
                //Shoot the player up and away
                var trajectory = other.transform.forward;
                Quaternion rotation = Quaternion.AngleAxis(45, Vector3.right); // Rotate 45 degrees around the right axis
                trajectory = rotation * -trajectory; // Apply rotation to the forward vector
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
