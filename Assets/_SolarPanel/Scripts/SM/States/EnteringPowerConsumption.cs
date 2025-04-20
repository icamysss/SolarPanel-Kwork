namespace _SolarPanel.Scripts.SM.States
{
    public class EnteringPowerConsumption : AppState
    {
        private readonly UIManager uiManager;
        public EnteringPowerConsumption(AppManager appManager) : base(appManager)
        {
            uiManager = UIManager.Instance;
        }

        public override void Enter()
        {
            base.Enter();
            ShowUI(true);
        }

        public override void Exit()
        {
            base.Exit();
            ShowUI(false);
        }

        private void ShowUI(bool show)
        {
            UIManager.Show(uiManager.powerConsumption, show);
            uiManager.navigation.Show(show);
            
            uiManager.navigation.ShowButton(ButtonType.next, show);
            uiManager.navigation.SetButtonText(ButtonType.next, Constants.CALCULATE_BUTTON_TEXT);
            
            uiManager.navigation.ShowButton(ButtonType.previous, show);
            uiManager.navigation.SetButtonText(ButtonType.previous, Constants.BACK_BUTTON_TEXT);
            
            uiManager.navigation.SetHeader(Constants.POWER_CONSUMPTION_HEADER);
        }
    }
}