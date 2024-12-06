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
            //Choose an initial patrol
            RandomPatrol();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //If we reach our destination, pick a new spot and patrol
            if (BL.transform.position == PatrolPosition)
            {
                RandomPatrol();
            }
            //If we choose a location that doesn't exist, choose a new one
            if (PatrolPosition == null)
            {
                RandomPatrol();
            }
            //This code handles transitioning to a melee pursue state
            if (BL.PlayerInSight && Vector3.Distance(BL.player.position, BL.transform.position) <= BL.meleePursueRange)
            {
                machine.ChangeState(new PursueState(machine, BL));
            }
            //This code will start ranged attacks because player is not close enough to start pursuing
            else if (BL.PlayerInSight)
            {
                machine.ChangeState(new RangedState(machine, BL));
            }
        }
        public override void OnExit()
        {
            base.OnExit();
        }

        //Function for patrolling
        public void RandomPatrol()
        {

            PatrolPosition = BL.GetRandomPointInRange();
            
            if (PatrolPosition != null)
            {
                BL.agent.SetDestination(PatrolPosition.Value);
            }
        }
    }
}
