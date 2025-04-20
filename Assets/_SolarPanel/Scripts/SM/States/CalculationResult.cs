namespace _SolarPanel.Scripts.SM.States
{
    public class CalculationResult : AppState
    {
        private readonly UIManager uiManager;
        public CalculationResult(AppManager appManager) : base(appManager)
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
            UIManager.Show(uiManager.calculationResult, show);
            uiManager.navigation.Show(show);
            
            uiManager.navigation.ShowButton(ButtonType.next, show);
            uiManager.navigation.SetButtonText(ButtonType.next, Constants.VISUALIZE_BUTTON_TEXT);
            
            uiManager.navigation.ShowButton(ButtonType.previous, show);
            uiManager.navigation.SetButtonText(ButtonType.previous, Constants.BACK_BUTTON_TEXT);
            
            uiManager.navigation.SetHeader(Constants.CALCULATION_RESULT_HEADER);
        }
    }
}