using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class FleeState : State
    {
        private Vector3 _destination;
        private float _range = 8f;
        private float _tagRadius = 3f;
        public FleeState(StateController stateController) : base(stateController)
        {

        }
        public override void CheckTransitions()
        {
            if (!StateController.CheckIfInRange(_range))
            {
                StateController.SetState(new PatrolState(StateController));
            }

            if (!(Vector3.Distance(StateController.transform.position,
                StateController.enemyToChase.gameObject.transform.position) > _tagRadius))
            {
                StateController.SetState(new ChaseState(StateController));
            }
        }
        public override void Act()
        {
            _destination = StateController.getFlee();
            StateController.ai.SetTarget(_destination);
        }

        public override void OnStateEnter()
        {
            _destination = StateController.getChased();
            if(StateController.ai.agent != null)
            {
                StateController.ai.agent.speed = 2f;
            }
            StateController.ai.SetTarget(_destination);
            StateController.ChangeColor(Color.red);

        }
    }
}
