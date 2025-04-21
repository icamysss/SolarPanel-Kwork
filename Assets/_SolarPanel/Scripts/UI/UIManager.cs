using UnityEngine;

namespace _SolarPanel.Scripts.UI
{
   public class UIManager : MonoBehaviour
   {
      public static UIManager Instance;
   
      public CanvasGroup houseParameters;
      public CanvasGroup calculationResult;
      public CanvasGroup powerConsumption;
      public CanvasGroup visualization;
      public Navigation navigation;

      public void Awake()
      {
         if (Instance == null) Instance = this;
         else if (Instance != this) Destroy(gameObject);
         DontDestroyOnLoad(gameObject);
      }

      private void Start()
      {
         Initialize();
      }

      private void Initialize()
      {
         Show(houseParameters, false);
         Show(calculationResult, false);
         Show(powerConsumption, false);
         Show(visualization, false);
         navigation.Initialize();
      }

      public static void Show(CanvasGroup canvasGroup, bool show = true)
      {
         canvasGroup.alpha = show ? 1 : 0;
         canvasGroup.interactable = show;
         canvasGroup.blocksRaycasts = show;
         canvasGroup.ignoreParentGroups = true;
      }
   
   }
}
