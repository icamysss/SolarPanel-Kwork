using _SolarPanel.Scripts.UI.CalculationResults;
using Unity.VisualScripting;
using UnityEngine;

namespace _SolarPanel.Scripts.UI
{
   public class UIManager : MonoBehaviour
   {
      public static UIManager Instance;
   
      public CanvasGroup houseParameters;
      public Results results;
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
         Show(powerConsumption, false);
         Show(visualization, false);
        
         navigation.Initialize();
         results.Initialize();
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
