using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimeLeftUpdater : MonoBehaviour
{
    private const float UpdateInterval = 1;
    
    [SerializeField] private Text timeLeftText;
    private GameplayController _gameplayController;

    private float _waveSpawnTime;
    private float _lastUpdateTime;
    private Wave? _currentWave;
    private int _currentWaveNumber;

    private void Awake()
    {
        _gameplayController = FindObjectOfType<GameplayController>();
        _lastUpdateTime = 0;
    }

    void Start()
    {
        UpdateNewSpawnTime();
        UpdateText();
    }
    
    void Update()
    {
        if (_currentWaveNumber < _gameplayController.TotalWavesCount)
        {
            if (_lastUpdateTime >= UpdateInterval)
            {
                _lastUpdateTime = 0;
    
                if (_currentWaveNumber != _gameplayController.CurrentWaveNumber)
                {
                    UpdateNewSpawnTime();
                }
    
                UpdateText();
            }
            _waveSpawnTime -= Time.deltaTime;
            _lastUpdateTime += Time.deltaTime;
        }
    }
    
    private void UpdateNewSpawnTime()
    {
        _currentWaveNumber = _gameplayController.CurrentWaveNumber;
        _currentWave = _gameplayController.GetCurrentWave();

        float? currentWaveTimeToSpawn = _currentWave?.TimeToSpawn;

        if (currentWaveTimeToSpawn != null)
        {
            _waveSpawnTime = (float) currentWaveTimeToSpawn;
        }
    }

    private void UpdateText()
    {
        var roundedTime = Mathf.Round(_waveSpawnTime);
        var cappedTime = Math.Max(0, roundedTime);
        timeLeftText.text = cappedTime.ToString();
    }
}
