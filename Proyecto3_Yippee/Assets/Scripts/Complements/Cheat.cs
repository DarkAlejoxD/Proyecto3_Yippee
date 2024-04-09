using UnityEngine;

namespace UtilsComplements
{
    public abstract class Cheat : MonoBehaviour
    {
        //TODO: Test this class
        //TODO: Make a template to easy create cheats
        protected enum CheatTypeEnum
        {
            ONCE, REINVOKE, TOOGLEABLE
        }

        [Header("DEBUG")]
        [SerializeField] private bool DEBUG_TestCheat = false;

        private int _cheatIndexControl;
        private bool _alreadyInvoked;
        protected virtual CheatTypeEnum CheatType => CheatTypeEnum.ONCE;
        protected abstract string KeyboardCheatReference { get; }
        protected abstract string GamepadCheatReference { get; }

        protected virtual void Start()
        {
            _cheatIndexControl = 0;
        }

        private void KeyboardCheatUpdate()
        {            
            if (!Input.anyKeyDown)
                return;

            if (CheatType != CheatTypeEnum.TOOGLEABLE)
            {
                if (_alreadyInvoked)
                    return;
            }

            char expectedChar = KeyboardCheatReference[_cheatIndexControl];

            if (DEBUG_TestCheat)
                Debug.Log("Expected Char: " + expectedChar);

            if (!Input.GetKeyDown(expectedChar.ToString().ToLower()))
            {
                _cheatIndexControl = 0;
                return;
            }

            int cheatLenght = KeyboardCheatReference.Length;

            if (_cheatIndexControl >= cheatLenght - 1)
            {
                CorrectCombination();
            }
            else
            {
                _cheatIndexControl++;
            }
        }

        private void CorrectCombination()
        {
            switch (CheatType)
            {
                case CheatTypeEnum.ONCE:
                    ActivateCheat();
                    _alreadyInvoked = true;
                    break;

                case CheatTypeEnum.REINVOKE:
                    _alreadyInvoked = false;
                    ActivateCheat();
                    break;

                case CheatTypeEnum.TOOGLEABLE:
                    if (!_alreadyInvoked)
                    {
                        ActivateCheat();
                        _alreadyInvoked = true;
                    }
                    else
                    {
                        DeactivateCheat();
                        _alreadyInvoked = false;
                    }
                    break;
                default:
                    Debug.LogError("Not expected CheatType");
                    break;
            }

            _cheatIndexControl = 0;
        }

        protected abstract void ActivateCheat();
        protected virtual void DeactivateCheat()
        {
            Debug.Log("Not implemented, try override DeactivateCheat() Method");
        }
    }
}