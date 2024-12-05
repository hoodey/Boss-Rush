using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class KickState : State
    {
        public BossLogic BL;
        public bool kickAnim = false;

        public KickState(StateMachine m, BossLogic BL) : base(m)
        {
            machine = m;
            this.BL = BL;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var dirToBoss = (BL.transform.position - BL.player.transform.position).normalized;
            dirToBoss.y = 0;
            BL.player.forward = dirToBoss;
            BL.agent.SetDestination(BL.kickSpot.position);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (BL.agent.remainingDistance <= 0.2f && !kickAnim)
            {
                kickAnim = true;
                BL.agent.ResetPath();
                //Turn to player and turn player
                
                var dirToPlayer = (BL.player.transform.position - BL.transform.position).normalized;
                dirToPlayer.y = 0;
                BL.transform.forward = dirToPlayer;
                BL.anim.SetTrigger("kickAttack");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            kickAnim = false;
        }

    }
}