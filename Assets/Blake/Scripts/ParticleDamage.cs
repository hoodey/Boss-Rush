using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class ParticleDamage : MonoBehaviour
    {
        public ParticleSystem part;
        public List<ParticleCollisionEvent> collisionEvents;



        // Start is called before the first frame update
        void Start()
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
            StartCoroutine(UltimateAttackProcess());
        }

        IEnumerator UltimateAttackProcess()
        {
            part.Play();
            yield return new WaitForSeconds(5.0f);
            part.Stop();
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            Damageable dmg = other.GetComponent<Damageable>();
            int i = 0;

            while (i < numCollisionEvents)
            {
                if (dmg)
                {
                    Damage fire = new Damage();
                    fire.amount = 1;
                    fire.direction = Vector3.zero;
                    fire.knockbackForce = 0;
                    dmg.Hit(fire);
                }
                i++;
            }
        }
    }
}

