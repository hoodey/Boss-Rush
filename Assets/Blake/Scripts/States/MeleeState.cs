using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class MeleeState : State
    {
        public BossLogic BL;


        public MeleeState(StateMachine m, BossLogic BL) : base(m)
        {
            machine = m;
            this.BL = BL;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            elapsedTime = 0f;
            BL.agent.destination = BL.transform.position;
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
