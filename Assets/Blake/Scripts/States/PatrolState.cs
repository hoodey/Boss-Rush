using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class PatrolState : State
    {
        public BossLogic BL;
        public Vector3? PatrolPosition;
        public PatrolState(StateMachine m, BossLogic BL) : base(m)
        {
            this.BL = BL;
            machine = m;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered Patrol State");
            base.OnEnter();
            RandomPatrol();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (BL.transform.position == PatrolPosition)
            {
                Debug.Log("Pathing to a new point");
                RandomPatrol();
            }
            if (PatrolPosition == null)
            {
                RandomPatrol();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void RandomPatrol()
        {

            PatrolPosition = BL.GetRandomPointInRange();
            
            Debug.Log(PatrolPosition);
            if (PatrolPosition != null)
            {
                BL.agent.SetDestination(PatrolPosition.Value);
            }
        }
    }
}
