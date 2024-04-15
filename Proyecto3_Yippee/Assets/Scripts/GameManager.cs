using AvatarController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsComplements;

public class GameManager : MonoBehaviour, ISingleton<GameManager>
{
    public ISingleton<GameManager> Instance => this;
    private PlayerController _playerInstance;

    public PlayerController PlayerInstance;

    private void Awake()
    {
        Instance.Instantiate();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
