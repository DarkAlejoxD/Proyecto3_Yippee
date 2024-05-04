using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UtilsComplements.Cheats
{
    [DisallowMultipleComponent]
    public class Cheat_Reset : Cheat, ISingleton<Cheat_Reset>
    {
        private const string KEYBOARD_CHEAT_CODE = "RESTART";
        private const string GAMEPAD_CHEAT_CODE = "[Insert code without spaces]";

        public ISingleton<Cheat_Reset> Instance => this;

        protected override string KeyboardCheatReference => KEYBOARD_CHEAT_CODE;
        protected override string GamepadCheatReference => GAMEPAD_CHEAT_CODE;
        protected override CheatTypeEnum CheatType => CheatTypeEnum.REINVOKE;

        #region UnityLogic

        private void Awake()
        {
            Instance.Instantiate();
        }

        //protected override void Start()
        //{
        //    base.Start();
        //}

        //protected override void Update()
        //{
        //    base.Update();
        //}

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }
        #endregion

        //#region Public Methods //Uncomment this if you dont want to delete the gameObject. Destroys only the component.
        //public void Invalidate()
        //{
        //    Destroy(this);
        //}
        //#endregion

        #region Private Methods
        protected override void ActivateCheat()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /*
        protected override void DeactivateCheat()
        {
            //Write here if want a toogleCheat(?, delete it otherwise
        }
        */
        #endregion

        #region DEBUG
#if UNITY_EDITOR
        [MenuItem(itemName: "Cheats/Cheat_Reset", isValidateFunction: false, priority = 1)]
        private static void ActivateCheatFromMenu()
        {
            if (!ISingleton<Cheat_Reset>.TryGetInstance(out var cheat))
                return;

            cheat.CorrectCombination();
        }
#endif
        #endregion
    }
}
