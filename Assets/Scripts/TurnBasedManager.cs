using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnBasedManager : MonoBehaviour
{
    private static TurnBasedManager _instance;
    [SerializeField] private PlayerTurn playerOne;
    [SerializeField] private PlayerTurn playerTwo;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera enemyCamera;
    [SerializeField] private Camera thirdCamera;
    [SerializeField] private float timeBetweenTurns;
    private int _currentPlayerIndex;
    private bool _isWaitingForNextTurn; 
    private float _turnDelay;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _currentPlayerIndex = 1;
            SetActiveCamera(playerCamera);
        }
    }
    private void Start()
    {
            playerOne.SetPlayerTurn(1);
            playerTwo.SetPlayerTurn(2);        
    }
    private void Update()
    {
        if (_isWaitingForNextTurn)
        {
            _turnDelay += Time.deltaTime;
            if (_turnDelay >= timeBetweenTurns)
            {
                _turnDelay = 0;
                _isWaitingForNextTurn = false;
                ChangeTurn();
            }
        }
    }

    public bool IsItPlayerTurn(int index)
    {
        if (_isWaitingForNextTurn)
        { 
            return false;
        }
        return index == _currentPlayerIndex;
    }

    public static TurnBasedManager GetInstance()
    {
        return _instance;
    }

    public void TriggerChangeTurn()
    {
        _isWaitingForNextTurn = true;
    }

    private void ChangeTurn()
    {
        if (_currentPlayerIndex == 1)
        {
            SetActiveCamera(enemyCamera);
            _currentPlayerIndex = 2;
            playerTwo.SetActivePlayer();
        }
        else if (_currentPlayerIndex == 2)
        {
            SetActiveCamera(playerCamera);
            _currentPlayerIndex = 1;
            playerOne.SetActivePlayer();
        }
    }

    public void ChangeToThirdCamera()
    {
        SetActiveCamera(thirdCamera);
    }

    private void SetActiveCamera(Camera camera)
    {
        thirdCamera.depth = thirdCamera == camera ? 1 : -1;
        playerCamera.depth = playerCamera == camera ? 1 : -1;
        enemyCamera.depth = enemyCamera == camera ? 1 : -1;
    }
}