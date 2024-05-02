using System.Collections.Generic;
using UnityEngine;
using UtilsComplements;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Poltergeist
{
    public class PoltergeistManager : MonoBehaviour, ISingleton<PoltergeistManager>
    {
        [Header("Lists")]
        private List<Poltergeist_Item> _poltergeistList = new();
        private Poltergeist_Item[] _evaluatedPoltergeists;

        private int _indexControl;

        public ISingleton<PoltergeistManager> Instance => this;

        #region Unity Logic
        private void Awake()
        {
            _poltergeistList = new();
            Instance.Instantiate();
        }

        private void OnDestroy() => Instance.RemoveInstance();
        #endregion

        #region Public Methods
        #region Private Methods
        /// <summary>
        /// Should be triggered at the beginning of the poltergeistMode
        /// </summary>
        public void StartPoltergeist(Transform target, float radius)
        {
            _evaluatedPoltergeists = GetNearPoltergeist(target, radius);
        }

        internal void AddPoltergeist(Poltergeist_Item item)
        {
            _poltergeistList ??= new();

            if (!_poltergeistList.Contains(item))
                _poltergeistList.Add(item);
            else
                Debug.LogWarning("You're trying to ad an existing polter Item in the Polter manager", item);
        }

        internal void RemovePoltergeist(Poltergeist_Item item)
        {
            _poltergeistList ??= new();

            if (_poltergeistList.Contains(item))
                _poltergeistList.Remove(item);
        }
        #endregion
        
        private Poltergeist_Item[] GetNearPoltergeist(Transform target, float radius)
        {
            //Check nullity
            if (_poltergeistList.Count <= 0)
                return null;

            //GetNearest
            List<Poltergeist_Item> nearList = new();

            for (int i = 0; i < _poltergeistList.Count; i++)
            {
                float distance = Vector3.Distance(_poltergeistList[i].transform.position,
                                                  target.position);
                if (distance < radius)
                    nearList.Add(_poltergeistList[i]);
            }

            //Sort them by a direction (The camera axis temp)
            //[0]-1 [n/2]0 [n]1

            Vector3 direction = Camera.main.transform.right;
            Poltergeist_Item[] sortedList = new Poltergeist_Item[nearList.Count];

            for (int i = 0; i < nearList.Count; i++)
            {
                if (i == 0)
                {
                    sortedList[i] = nearList[i];
                    continue;
                }

                Poltergeist_Item lastItem = nearList[i];

                for (int j = 0; j < i; j++)
                {
                    Vector3 targetToCurrentPolter = target.position - lastItem.transform.position;
                    targetToCurrentPolter.Normalize();
                    float dotProduct = Vector3.Dot(targetToCurrentPolter, direction);

                    Vector3 targetToLocatedPolter = target.position - sortedList[j].transform.position;
                    targetToLocatedPolter.Normalize();
                    float dotLocated = Vector3.Dot(targetToLocatedPolter, direction);

                    if (dotProduct < dotLocated)
                    {
                        var handler = sortedList[j]; //keep this position's item
                        sortedList[j] = lastItem; //asign the item to the list
                        lastItem = handler; //asign the object we grab from the sortedlist to the lastItem
                    }
                }

                sortedList[i] = lastItem;
            }

            return sortedList;
        }
        #endregion

        #region DEBUG
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //TODO: Draw beziers? jsjs
        }
#endif
        #endregion
    }
}