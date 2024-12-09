using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace ElToro
{
    public class BossLogic : MonoBehaviour
    {
        public enum Phase
        {
            ONE,
            TWO,
            THREE
        }

        [SerializeField] float patrolRange;
        [SerializeField] GameObject rangedObject;
        [SerializeField] public Transform kickSpot;
        [SerializeField] GameObject fireBreath;
        [SerializeField] Transform fireBreathSpot;
        [SerializeField] public GameManager gm;
        [SerializeField] AudioClip kickRoar;
        [SerializeField] AudioClip phase3Roar;
        [SerializeField] AudioClip rangedRoar;

        public float maxHealth = 100f;
        public Transform player;
        public NavMeshAgent agent;
        public StateMachine myStateMachine;
        public Animator anim;
        public Rigidbody rb;
        public Damageable myDamage;
        public bool PlayerInSight = false;
        public Collider meleeWeapon;
        public Collider foot;
        public float meleeSwingCooldown = 2.0f;
        public float rangedAttackCD = 3f;
        public float LastSwing = 0f;
        public float meleePursueRange = 30.0f;
        public bool kicking = false;
        public int kickCounter = 0;
        public int hitsToKick = 0;
        public float ultimateAttackCD = 5.0f;
        public float lastFireBreath = 0f;
        public bool isDead = false;
        Phase currentPhase;

        public float NavSpeed;


        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerLogic>().transform;

            rb = GetComponent<Rigidbody>();

            anim = GetComponent<Animator>();

            agent = GetComponent<NavMeshAgent>();

            myStateMachine = new StateMachine();

            myStateMachine.ChangeState(new IdleState(myStateMachine, this));

            myDamage = GetComponent<Damageable>();
        }

        // Update is called once per frame
        void Update()
        {
            NavSpeed = Mathf.Sqrt(Mathf.Pow(agent.velocity.x, 2f) + Mathf.Pow(agent.velocity.z, 2f));
            anim.SetFloat("speed", NavSpeed);
            myStateMachine.Update();
            LastSwing += Time.deltaTime;

            if (myDamage.GetCurrentHealth()/maxHealth >= 0.75f && currentPhase != Phase.ONE)
            {
                currentPhase = Phase.ONE;
                agent.speed = 4f;
            }
            else if (myDamage.GetCurrentHealth()/maxHealth >= 0.50f && myDamage.GetCurrentHealth()/maxHealth < 0.75f && currentPhase != Phase.TWO)
            {
                currentPhase = Phase.TWO;
                meleeSwingCooldown = 1.5f;
                rangedAttackCD = 2f;
                agent.speed = 6f;
                hitsToKick = 3;
            }
            else if (myDamage.GetCurrentHealth() / maxHealth <= 0.50f && currentPhase != Phase.THREE)
            {
                currentPhase = Phase.THREE;
                agent.speed = 9f;
                meleePursueRange = 15f;
                meleeSwingCooldown = 0.5f;
                hitsToKick = 10;
                SoundEffectsManager.instance.PlayAudioClip(phase3Roar, true);
            }

            if (currentPhase == Phase.THREE)
            {
                lastFireBreath += Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            AttemptToSeePlayer();
        }

        void AttemptToSeePlayer()
        {
            Vector3 higherPos = new Vector3(transform.position.x, transform.position.y+2f, transform.position.z);
            Vector3 directionToPlayer = (player.position - higherPos).normalized;
            Vector3 forwardDirection = transform.forward;

            float dot = Vector3.Dot(forwardDirection, directionToPlayer);
            if (dot > 0.5f)
            {
                RaycastHit hit;
                
                if(Physics.Raycast(higherPos, directionToPlayer, out hit, 100f))
                {
                    if (hit.transform.gameObject == player.gameObject)
                    {
                        //Debug.Log(gameObject.name + " sees the player!");
                        PlayerInSight = true;
                    }
                    else
                    {
                        PlayerInSight = false;
                    }
                }
                else
                {
                    PlayerInSight = false;
                }
            }
            else
            {
                PlayerInSight = false;
            }
        }

        public Vector3? GetRandomPointInRange()
        {
            Vector3 offset = new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));

            NavMeshHit hit;

            bool gotPoint = NavMesh.SamplePosition(transform.position + offset, out hit, 5, NavMesh.AllAreas);

            if (gotPoint)
            {
                return hit.position;
            }

            return null;
        }

        public void OnStayKick()
        {
            if (kicking)
            {
                return;
            }
            if (kickCounter >= hitsToKick && !isDead)
            {
                kickCounter = 0;
                kicking = true;
                StaticInputManager.input.Disable();
                myStateMachine.ChangeState(new KickState(myStateMachine, this));
            }
        }

        public void OnExitKick()
        {
            kickCounter = 0;
        }

        public void OnEnterMelee()
        {
            if (kicking)
            { 
                return; 
            }
            if (LastSwing >= meleeSwingCooldown && !isDead)
            {
                myStateMachine.ChangeState(new MeleeState(myStateMachine, this));
                LastSwing = 0f;
            }

        }

        public void OnStayMelee()
        {
            if (kicking)
            {
                return;
            }
            if (LastSwing >= meleeSwingCooldown && !isDead)
            {
                myStateMachine.ChangeState(new MeleeState(myStateMachine, this));
                LastSwing = 0f;
            }
        }

        public void OnExitMelee()
        {
            if (!isDead)
                StartCoroutine(MeleeCooldown());
        }

        public void HitBoxOn()
        {
            if (kicking)
            {
                foot.enabled = true;
                return;
            }
            meleeWeapon.enabled = true;
        }

        public void HitBoxOff()
        {
            if (kicking)
            {
                foot.enabled = false;
                return;
            }
            meleeWeapon.enabled = false;
        }
        public void RangedAttack()
        {
            SoundEffectsManager.instance.PlayAudioClip(rangedRoar, true);
            Vector3 above = player.transform.position;
            GameObject p = Instantiate(rangedObject, above, Quaternion.identity);
        }

        public void UltimateAttack()
        {
            if (lastFireBreath >= ultimateAttackCD)
            {
                GameObject f = Instantiate(fireBreath, fireBreathSpot.position, fireBreathSpot.rotation);
                StartCoroutine(UltimateAttackProcess());
            }
        }

        IEnumerator UltimateAttackProcess()
        {
            yield return new WaitForSeconds(5.0f);
            lastFireBreath = 0.0f;
            myStateMachine.ChangeState(new PatrolState(myStateMachine, this));
        }

        IEnumerator MeleeCooldown()
        {
            while (LastSwing < meleeSwingCooldown)
            {
                yield return new WaitForSeconds(0.2f);
            }
            if (!kicking)
            {
                myStateMachine.ChangeState(new PursueState(myStateMachine, this));
            }
        }

        public void OnFinishKick()
        {
            //code for handling after the kick finishes animation
            StaticInputManager.input.Enable();
            kicking = false;
            myStateMachine.ChangeState(new IdleState(myStateMachine, this));
        }

        public void WhenHit()
        {
            if (myStateMachine.currentState.ToString() == "ElToro.PatrolState" || myStateMachine.currentState.ToString() == "ElToro.IdleState")
            {
                myStateMachine.ChangeState(new PursueState(myStateMachine, this));
            }
            //Animation for being hit (was causing bugs)
            //anim.SetTrigger("gotHit");            

            //Handle warning noise for upcoming kick
            if (kickCounter == hitsToKick -1)
            {
                SoundEffectsManager.instance.PlayAudioClip(kickRoar, true);
            }

            //Handle queueing up a kick
            kickCounter++;
        }

        public void die()
        {
            isDead = true;
            myStateMachine.ChangeState(new DeathState(myStateMachine, this));
        }

        public Phase GetCurrentPhase()
        {
            return currentPhase;
        }
    }
}