using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SolarPanel.Scripts.UI
{
   [RequireComponent(typeof(CanvasGroup))]
   public class Navigation : MonoBehaviour
   {
      [SerializeField] private Button nextButton;
      [SerializeField] private Button previousButton;
      [SerializeField] private Button restartButton;
      [SerializeField] private TextMeshProUGUI header;
   
      private CanvasGroup canvasGroup;
      private AppManager appManager;

      private void Awake()
      {
         canvasGroup = GetComponent<CanvasGroup>();
      }
     
      
      public void Initialize()
      {
         appManager = AppManager.Instance;
         Show(false);
         
         nextButton.onClick.RemoveAllListeners();
         nextButton.onClick.AddListener(appManager.NextState);
      
         previousButton.onClick.RemoveAllListeners();
         previousButton.onClick.AddListener(appManager.PreviousState);
         
         restartButton.onClick.RemoveAllListeners();
         restartButton.onClick.AddListener(appManager.Restart);
         
         ShowButton(ButtonType.next, false);
         ShowButton(ButtonType.previous, false);
         ShowButton(ButtonType.restart, false);
         
         SetButtonText(ButtonType.next, Constants.NEXT_BUTTON_TEXT);
         SetButtonText(ButtonType.previous, Constants.BACK_BUTTON_TEXT);
         SetButtonText(ButtonType.restart, Constants.RESTART_BUTTON_TEXT);

         header.text = "";
      }
      public void Show(bool show = true)
      {
         UIManager.Show(canvasGroup, show);
      }
      public void ShowButton(ButtonType buttonType, bool show = true)
      {
         switch (buttonType)
         {
            case ButtonType.next:
               nextButton.gameObject.SetActive(show);
               break;
            case ButtonType.previous:
               previousButton.gameObject.SetActive(show);
               break;
            case ButtonType.restart:
               restartButton.gameObject.SetActive(show);
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
         }
      }
      public void SetHeader(string header)
      {
         this.header.text = header;
      }
      public void SetButtonText(ButtonType btnType, string text)
      {
         switch (btnType)
         {
            case ButtonType.next:
               nextButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
               break;
            case ButtonType.previous:
               previousButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
               break;
            case ButtonType.restart:
               restartButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(btnType), btnType, null);
         }
      }
   }

   public enum ButtonType
   {
      next, previous, restart
   }
}
