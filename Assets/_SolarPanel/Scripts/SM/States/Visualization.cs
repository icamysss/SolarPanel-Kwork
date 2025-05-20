using _SolarPanel.Scripts.UI;
using UnityEngine;

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
            VisualizationManager.Instance.Generate();
            var cam = Object.FindFirstObjectByType<CameraController>();
            if (cam != null)
            {
                var x = DataManager.Instance.HouseParam.HouseWidth + 2;
                cam.MinRunTimeDistance = DataManager.Instance.HouseParam.HouseWidth / 2;
                cam.SetXPosition(x);
                cam.LookPoint = VisualizationManager.Instance.StartPosition;
               //cam.gameObject.transform.position = VisualizationManager.Instance.StartPosition;
            }

            var exel = Object.FindFirstObjectByType<ExcelGenerator>();
            exel.GenerateExcelFile();
        }

        public override void Exit()
        {
            base.Exit();
            ShowUI(false);
        }
        
        private void ShowUI(bool show = true)
        {
            uiManager.visualizationUI.UpdateText();
            uiManager.visualizationUI.Show(show);
            uiManager.navigation.Show(show);
            
            uiManager.navigation.ShowButton(ButtonType.restart, show);
            uiManager.navigation.SetButtonText(ButtonType.restart, Constants.RESTART_BUTTON_TEXT );
            
            uiManager.navigation.ShowButton(ButtonType.previous, show);
            uiManager.navigation.SetButtonText(ButtonType.previous, Constants.BACK_BUTTON_TEXT);
            
            uiManager.navigation.SetHeader(DataManager.Instance.SelectedPanel.PanelName);
        }
    }
}