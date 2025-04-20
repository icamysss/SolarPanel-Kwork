using System;
using _SolarPanel.Scripts.SM;
using _SolarPanel.Scripts.SM.States;
using Unity.VisualScripting;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    
    public static Action<AppState> OnChangeState;
    
    private AppState CurrentAppState;
    private AppState PreviousAppState;
    private AppState NextAppState;
    
    private EnteringHouseParameters enteringHouseParameters;
    private EnteringPowerConsumption enteringPowerConsumption;
    private CalculationResult calcResult;
    private Visualization visualization;
    
    private void Awake()
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
        enteringHouseParameters = new EnteringHouseParameters(this, enteringPowerConsumption);
        enteringPowerConsumption = new EnteringPowerConsumption(this, calcResult, enteringHouseParameters);
        calcResult = new CalculationResult(this, visualization, enteringPowerConsumption);
        visualization = new Visualization(this, null, calcResult);
        
        CurrentAppState = enteringHouseParameters;
        CurrentAppState.Enter();
        NextAppState = enteringPowerConsumption;
        PreviousAppState = null;
    }


    public void NextState()
    {
        SetState(CurrentAppState.NextState);
    }
    public void PreviousState()
    {
        SetState(CurrentAppState.PreviousState);
    }
    public void SetState(AppState state)
    {
        if (state == null)
        {
           Debug.LogWarning("Cannot set state to null!");
        }
        
        CurrentAppState?.Exit();
        CurrentAppState = state;
        CurrentAppState.Enter();
        OnChangeState?.Invoke(CurrentAppState);
    }
}