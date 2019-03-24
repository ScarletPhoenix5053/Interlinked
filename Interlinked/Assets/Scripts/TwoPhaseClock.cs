using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TwoPhaseClock : MonoBehaviour
{
    // Calls events for use by pops so they know when to move
    // Pops will assign themselves based on thier routine

    public static event ClockEventHandler OnHourTick;

    // Timing options
    [SerializeField] private float _hourDuration = 3f;
    // Timing vars
    private float _timerCurrent = 0;
    private int _hourCurrent = 0;
    private const int _maxHour = 1;

    // Gameobject references
    #pragma warning disable 0649
    [SerializeField] private Image _clockRenderer;
    [SerializeField] private TimerBar _timerBar;
    #pragma warning restore 0649

    // Runtime
    private void Start()
    {
        _clockRenderer.enabled = false;
    }
    private void FixedUpdate()
    {
        if (_timerCurrent <= _hourDuration)
        {
            // increase timer
            _timerCurrent += Time.fixedDeltaTime;
        }
        else
        {
            // switch hour
            if (_hourCurrent >= _maxHour)
            {
                _hourCurrent = 0;
                OnHourTick?.Invoke(0);
                _clockRenderer.enabled = false;
            }
            else
            {
                _hourCurrent++;
                OnHourTick?.Invoke(1);
                _clockRenderer.enabled = true;
            }

            // reset timer
            _timerCurrent = 0;
        }

        // update bar
        _timerBar.UpdateValue(_timerCurrent / _hourDuration);
    }
}
public class UnexpectedHourException : System.Exception
{
    public UnexpectedHourException()
    {
    }

    public UnexpectedHourException(string message)
        : base(message)
    {
    }

    public UnexpectedHourException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}
public delegate void ClockEventHandler(int hourNum);