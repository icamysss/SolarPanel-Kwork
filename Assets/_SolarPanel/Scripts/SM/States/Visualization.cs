using _SolarPanel.Scripts.UI;

namespace _SolarPanel.Scripts.SM.States
{
    public class Visualization: AppState
    {
        private readonly UIManager uiManager;
        
        public Visualization(AppManager appManager) : base(appManager)
        {
            uiManager = UIManager.Instance;
        }

        public override void Enter()
        {
            base.Enter();
            ShowUI();
        }

        public override void Exit()
        {
            base.Exit();
            ShowUI(false);
        }
        
        private void ShowUI(bool show = true)
        {
            uiManager.visualization.Show(show);
            uiManager.navigation.Show(show);
            
            uiManager.navigation.ShowButton(ButtonType.restart, show);
            uiManager.navigation.SetButtonText(ButtonType.restart, Constants.RESTART_BUTTON_TEXT );
            
            uiManager.navigation.ShowButton(ButtonType.previous, show);
            uiManager.navigation.SetButtonText(ButtonType.previous, Constants.BACK_BUTTON_TEXT);
            
            uiManager.navigation.SetHeader(Constants.VISUALIZATION_HEADER);
        }
    }
}