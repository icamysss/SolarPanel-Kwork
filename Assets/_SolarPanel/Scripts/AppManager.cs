using System;
using _SolarPanel.Scripts.SM;
using _SolarPanel.Scripts.SM.States;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    public static Action<AppState> OnChangeState;

    private AppState CurrentAppState;

    public EnteringHouseParameters enteringHouseParameters;
    public EnteringPowerConsumption enteringPowerConsumption;
    public CalculationResult calcResult;
    public Visualization visualization;

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
        #region  SM

        enteringHouseParameters = new EnteringHouseParameters(this);
        enteringPowerConsumption = new EnteringPowerConsumption(this);
        calcResult = new CalculationResult(this);
        visualization = new Visualization(this);

        enteringHouseParameters.PreviousState = null;
        enteringHouseParameters.NextState = enteringPowerConsumption;

        enteringPowerConsumption.PreviousState = enteringHouseParameters;
        enteringPowerConsumption.NextState = calcResult;

        calcResult.PreviousState = enteringPowerConsumption;
        calcResult.NextState = visualization;

        visualization.PreviousState = calcResult;
        visualization.NextState = null;

        CurrentAppState = enteringHouseParameters;
        CurrentAppState.Enter();

        #endregion
    }


    public void NextState()
    {
        SetState(CurrentAppState.NextState);
    }

    public void PreviousState()
    {
        SetState(CurrentAppState.PreviousState);
    }

    public void Restart()
    {
        SetState(enteringHouseParameters);
    }

    private void SetState(AppState state)
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