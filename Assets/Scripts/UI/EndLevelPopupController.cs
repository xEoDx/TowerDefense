﻿using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelPopupController : MonoBehaviour
{
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;
    
    
    private GameplayController _gameplayController;    
    void Start()
    {
        _gameplayController = FindObjectOfType<GameplayController>();
        _gameplayController.OnGameEnd += OnGameEndListener;
    }

    private void OnGameEndListener(GameplayController.GameEndReason gameEndReason)
    {
        switch (gameEndReason)
        {
            case GameplayController.GameEndReason.Win:
                winPopup.SetActive(true);
                break;
            case GameplayController.GameEndReason.Lose:
                losePopup.SetActive(false);
                break;
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}