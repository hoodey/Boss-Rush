using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class KickState : State
    {
        public BossLogic BL;


        public KickState(StateMachine m, BossLogic BL) : base(m)
        {
            machine = m;
            this.BL = BL;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            BL.agent.ResetPath();
            var dirToPlayer = (BL.player.transform.position - BL.transform.position).normalized;
            dirToPlayer.y = 0;
            BL.transform.forward = dirToPlayer;
            BL.anim.SetTrigger("swing");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

    }
}