using UnityEngine;

namespace _SolarPanel.Scripts.SM
{
    public abstract class AppState
    {
        private protected readonly AppManager AppManager;
        public readonly AppState NextState;
        public readonly AppState PreviousState;
        
        protected AppState(AppManager appManager, AppState nextState = null, AppState previousState = null)
        {
            AppManager = appManager;
            PreviousState = previousState;
            NextState = nextState;
        }
        
        public virtual void Enter()
        {
            Debug.Log($"Enter {GetType().Name} state");
        }

        public virtual void Exit()
        {
            Debug.Log($"Exit {GetType().Name} state");
        }
    }
}