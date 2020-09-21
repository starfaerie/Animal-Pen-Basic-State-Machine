using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class FleeState : State
    {
        private Vector3 _destination;
        public FleeState(StateController stateController) : base(stateController)
        {

        }
        public override void CheckTransitions()
        {
            if (!StateController.CheckIfInRange())
            {
                StateController.SetState(new PatrolState(StateController));
            }
        }
        public override void Act()
        {
            _destination = StateController.getChased();
            StateController.ai.SetTarget(_destination);
        }

        public override void OnStateEnter()
        {
            _destination = StateController.getChased();
            if(StateController.ai.agent != null)
            {
                StateController.ai.agent.speed = 1f;
            }
            StateController.ai.SetTarget(_destination);
            StateController.ChangeColor(Color.red);

        }
    }
}
