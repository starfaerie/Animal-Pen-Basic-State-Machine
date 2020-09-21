using UnityEngine;

namespace State_Machine
{
    public class ChaseState : State
    {
        private Vector3 _destination;
        private float _range = 10f;
        public ChaseState(StateController stateController): base(stateController)
        {

        }
        public override void CheckTransitions()
        {
            if (!StateController.CheckIfInRange(_range))
            {
                StateController.SetState(new PatrolState(StateController));
            }
        }
        public override void Act()
        {
            if (StateController.enemyToChase == null) return;
        
            _destination = StateController.enemyToChase.transform.position;
            StateController.ai.SetTarget(_destination);
        }
        public override void OnStateEnter()
        {
            StateController.ChangeColor(Color.green);
            StateController.ai.agent.speed = .8f;
        }
    }
}
