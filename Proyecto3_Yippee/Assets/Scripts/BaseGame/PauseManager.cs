using UnityEngine;
using InputController;
using UtilsComplements;

namespace BaseGame
{
    public class PauseManager : MonoBehaviour, ISingleton<PauseManager>
    {
        [Header("References")]
        [SerializeField] private GameObject _firstSelected;
        [SerializeField] private GameObject _canvas;

        private Menus _pauseMap;

        public ISingleton<PauseManager> Instance => this;

        private bool _paused;

        public static bool IsPaused
        {
            get
            {
                if (Singleton.TryGetInstance(out PauseManager manager))
                    return manager._paused;

                return false;
            }
        }

        private void Awake()
        {
            Instance.Instantiate();
            _pauseMap = new();
            _pauseMap.Pause.Enable();
        }

        private void Update()
        {
            if (_pauseMap.Pause.PauseUnpause.WasPerformedThisFrame())
            {
                SetPaused(!_paused);
            }
        }

        private void OnDestroy() => Instance.RemoveInstance();

        public static void SetPauseActive(bool active)
        {
            if (Singleton.TryGetInstance(out PauseManager manager))
            {
                manager.SetPaused(active);
            }
        }

        public void SetPaused(bool active)
        {
            if (active)
            {
                if (_paused)
                    return;
                _paused = true;
                Time.timeScale = 0f;
                _canvas.SetActive(true);
            }
            else
            {
                if (!_paused)
                    return;
                _paused = false;
                _canvas.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}