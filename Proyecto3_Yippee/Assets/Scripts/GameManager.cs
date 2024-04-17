using AvatarController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsComplements;

public class GameManager : MonoBehaviour, ISingleton<GameManager>
{
    public ISingleton<GameManager> Instance => this;
    private PlayerController _playerInstance;

    public PlayerController PlayerInstance => _playerInstance;
    public static PlayerController Player
    {
        get
        {
            if(!ISingleton<GameManager>.TryGetInstance(out var manager))
                return null;
            return manager.PlayerInstance;
        }
    }

    private void Awake()
    {
        Instance.Instantiate();
    }

    private void OnDestroy()
    {
        Instance.RemoveInstance();
    }

    public void SetPlayerInstance(PlayerController player)
    {
        _playerInstance = player;
    }
}
