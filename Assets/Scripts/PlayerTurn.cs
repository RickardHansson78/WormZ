using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    [SerializeField] Player player;

    private int _playerIndex;

    public void SetPlayerTurn(int index)
    {
        player.UpdateAmmoCanvas();
        _playerIndex = index;
    }
    
    public void SetActivePlayer()
    {
        player.UpdateAmmoCanvas();
    }

    public bool IsPlayerTurn()
    {
        return TurnBasedManager.GetInstance().IsItPlayerTurn(_playerIndex);
    }
}