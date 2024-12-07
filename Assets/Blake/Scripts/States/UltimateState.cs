using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class UltimateState : State
    {
        public BossLogic BL;
        public float deAggroTimer = 0.0f;
        public float attackCD;
        public float attackTimer = 0.0f;

        public UltimateState(StateMachine m, BossLogic BL) : base(m)
        {
            machine = m;
            this.BL = BL;
            attackCD = BL.ultimateAttackCD;
        }

        public override void OnEnter()
        {
            PerformRangedAttack();
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void PerformRangedAttack()
        {
            //Stop patrol movement
            BL.agent.ResetPath();
            //Turn to player
            var dirToPlayer = (BL.player.transform.position - BL.transform.position).normalized;
            dirToPlayer.y = 0;
            BL.transform.forward = dirToPlayer;
            //Cue audio file of a grunt

            //Attack function called on BossLogic
            BL.UltimateAttack();

            //Old ranged code
            //ranged attack animation (with function trigger)
            //BL.anim.SetTrigger("rangedAttack");
            //BL.transform.forward = dirToPlayer;
        }
    }
}
