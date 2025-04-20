namespace _SolarPanel.Scripts.SM.States
{
    public class EnteringHouseParameters : AppState
    {
        private readonly UIManager uiManager;


        public EnteringHouseParameters(AppManager appManager) : base(appManager)
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
            UIManager.Show(uiManager.houseParameters, show);
            uiManager.navigation.Show(show);
            uiManager.navigation.ShowButton(ButtonType.next, show);
            uiManager.navigation.SetButtonText(ButtonType.next, Constants.NEXT_BUTTON_TEXT);
            uiManager.navigation.SetHeader(Constants.HOUSE_PARAMETERS_HEADER);
        }
    }
}