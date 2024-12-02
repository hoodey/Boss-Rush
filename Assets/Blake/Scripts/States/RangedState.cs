using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ElToro
{
    public class RangedState : State
    {
        public BossLogic BL;

        public RangedState(StateMachine m, BossLogic BL) : base(m)
        {
            machine = m;
            this.BL = BL;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered Ranged State");
            //Stop patrol movement
            BL.agent.ResetPath();
            //Turn to player
            var dirToPlayer = (BL.player.transform.position - BL.transform.position).normalized;
            dirToPlayer.y = 0;
            BL.transform.forward = dirToPlayer;
            //Cue audio file of a grunt

            //ranged attack animation (with function trigger)
            BL.anim.SetTrigger("rangedAttack");
            BL.transform.forward = dirToPlayer;
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //Evaluate if player is close enough to pursue, in view long enough to do another ranged attack, or out of view
        }

        public override void OnExit()
        {
            Debug.Log("Exited Ranged State");
            base.OnExit();
        }
    }
}
