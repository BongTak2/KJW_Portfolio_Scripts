using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FloatAmount
{
    [SerializeField] private float _max;
    public float Max
    {
        get => _max;
        set
        {
            if (value < 0)
                value = 0;
            if (value > _max)
            {
                Current = Current * (value / _max);
                _max = value;
            }
            else if (value < _max)
            {
                _max = value;
                Current = Current;
            }
        }

    }
    [SerializeField] private float _current;
    public float Current
    {
        get => _current;
        set => _current = Mathf.Clamp(value, 0, Max);
    }
    public float Rate => (Max == 0) ? 0 : (Current / Max);

}

[SerializeField]
public struct IntAmount
{
    [SerializeField] private int _max;
    public int Max
    {
        get => _max;
        set
        {
            if (value < 0)
                value = 0;
            if (value > _max)
            {
                Current = Mathf.RoundToInt(Current * (value / (float)_max));
                _max = value;
            }
            else if (value < _max)
            {
                _max = value;
                Current = Current;
            }
        }

    }
    [SerializeField] private int _current;
    public int Current
    {
        get => _current;
        set => _current = Mathf.Clamp(value, 0, Max);
    }
    public float Rate => (Max == 0) ? 0 : (Current / (float)Max);
}

[SerializeField]
public struct AtkStatus
{
    [SerializeField] private float atkPower;
    public float AtkPower
    {
        get => atkPower;
        set => atkPower = value;
    }

    [SerializeField] private float atkSpeed;
    public float AtkSpeed
    {
        get => atkSpeed;
        set => atkSpeed = Mathf.Clamp(value, 0.1f, 3f);
    }

    [SerializeField] private float atkRange;
    public float AtkRange
    {
        get => atkRange;
        set => atkRange = Mathf.Clamp(value, 0.5f, 20f);
    }
}
