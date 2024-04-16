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
