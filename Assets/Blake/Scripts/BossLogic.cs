using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ElToro
{
    public class BossLogic : MonoBehaviour
    {
        [SerializeField] float patrolRange;
        
        public Transform player;
        public NavMeshAgent agent;
        public StateMachine myStateMachine;


        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerLogic>().transform;

            agent = GetComponent<NavMeshAgent>();

            myStateMachine = new StateMachine();

            myStateMachine.ChangeState(new IdleState(myStateMachine, this));
        }

        // Update is called once per frame
        void Update()
        {
            myStateMachine.Update();
        }

        public Vector3 GetDirectionToPlayer()
        {
            var dirToPlayer = (player.transform.position - transform.position).normalized;
            dirToPlayer.y = 0;

            return dirToPlayer;
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
    }
}