namespace State_Machine
{
    public abstract class State
    {

        protected readonly StateController StateController;

        public abstract void CheckTransitions();
        public abstract void Act();
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }

        protected State(StateController stateController)
        {
            this.StateController = stateController;
        }
 


    }
}
