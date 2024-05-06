using UnityEngine;
using AvatarController;
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
            if (!ISingleton<GameManager>.TryGetInstance(out var manager))
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

    public static GameManager GetGameManager()
    {
        if (ISingleton<GameManager>.TryGetInstance(out var manager))
            return manager;

        //It should set itself as the singleton, so this part the code will only triggered once
        var go = new GameObject("GameManager");
        return go.AddComponent<GameManager>();
    }

    public void SetPlayerInstance(PlayerController player)
    {
        if (_playerInstance == null)
        {
            _playerInstance = player;
            _playerInstance.DataContainer.DisablePowers();
        }
        else
        {
            Destroy(player.gameObject);
            Debug.Log("More than one player detected, deleted the copy but not implemented if meant to be a dummy in anothe scene");
        }
    }
}
