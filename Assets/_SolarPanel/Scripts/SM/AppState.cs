using UnityEngine;

namespace _SolarPanel.Scripts.SM
{
    public abstract class AppState
    {
        private protected readonly AppManager AppManager;
        public AppState NextState;
        public AppState PreviousState;
        
        protected AppState(AppManager appManager)
        {
            AppManager = appManager;
        }
        
        public virtual void Enter()
        {
           // Debug.Log($"Enter {GetType().Name} state");
        }

        public virtual void Exit()
        {
          //  Debug.Log($"Exit {GetType().Name} state");
        }
    }
}