using _SolarPanel.Scripts.UI.CalculationResults;
using _SolarPanel.Scripts.UI.HouseParameters;
using UnityEngine;

namespace _SolarPanel.Scripts.UI
{
   public class UIManager : MonoBehaviour
   {
      public static UIManager Instance;
   
      public HouseParamInput houseParameters;
      public Results results;
      public PowerConsumption.PowerConsumption powerConsumption;
      public Visualization.Visualization visualization;
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
         navigation.Initialize();
         results.Initialize();
         powerConsumption.Initialize();
         houseParameters.Initialize();
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
