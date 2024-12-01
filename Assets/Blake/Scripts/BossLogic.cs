using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ElToro
{
    public class BossLogic : MonoBehaviour
    {
        [SerializeField] float patrolRange;
        [SerializeField] GameObject rangedObject;
        
        public Transform player;
        public NavMeshAgent agent;
        public StateMachine myStateMachine;
        public Animator anim;
        public Rigidbody rb;
        public bool PlayerInSight = false;
        public Collider meleeWeapon;
        public float meleeSwingCooldown = 2.0f;
        public float rangedAttackCD = 3f;
        public float LastSwing = 0f;
        public float meleePursueRange = 30.0f;

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
        }

        // Update is called once per frame
        void Update()
        {
            NavSpeed = Mathf.Sqrt(Mathf.Pow(agent.velocity.x, 2f) + Mathf.Pow(agent.velocity.z, 2f));
            anim.SetFloat("speed", NavSpeed);
            myStateMachine.Update();
            LastSwing += Time.deltaTime;
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

        public void OnEnterMelee()
        {
            if (LastSwing >= meleeSwingCooldown)
            {
                Debug.Log("First Attack");
                myStateMachine.ChangeState(new MeleeState(myStateMachine, this));
                LastSwing = 0f;
            }

        }

        public void OnStayMelee()
        {
            if (LastSwing >= meleeSwingCooldown)
            {
                Debug.Log("Attacking again");
                myStateMachine.ChangeState(new MeleeState(myStateMachine, this));
                LastSwing = 0f;
            }
        }

        public void OnExitMelee()
        {
            StartCoroutine(meleeCooldown());
        }

        public void HitBoxOn()
        {
            meleeWeapon.enabled = true;
        }

        public void HitBoxOff()
        {
            meleeWeapon.enabled = false;
        }
        public void RangedAttack()
        {
            Vector3 above = player.transform.position;
            GameObject p = Instantiate(rangedObject, above, Quaternion.identity);
        }

        IEnumerator meleeCooldown()
        {
            while (LastSwing < meleeSwingCooldown)
            {
                yield return new WaitForSeconds(0.2f);
            }
            myStateMachine.ChangeState(new PursueState(myStateMachine, this));
        }
    }
}