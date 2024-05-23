using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilsComplements;
using static UtilsComplements.AsyncTimer;
using BaseGame;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Poltergeist
{
    public class PoltergeistManager : MonoBehaviour, ISingleton<PoltergeistManager>
    {
        [Header("Lists")]
        private List<Poltergeist_Item> _poltergeistList = new();
        private List<Poltergeist_Item> _nearPoltergeists;

        private int _indexControl;
        private bool _evaluating;

        [Header("Attributes")]
        [SerializeField, Min(0.01f)] private float _timerToDie = 2;

        public ISingleton<PoltergeistManager> Instance => this;

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();
            _evaluating = false;
        }

        private void OnDestroy() => Instance.RemoveInstance();
        #endregion

        #region Public Methods
        /// <summary>
        /// Should be triggered at the beginning of the poltergeistMode
        /// </summary>
        public void StartPoltergeist(Transform target, float radius)
        {
            _evaluating = false;
            //_nearPoltergeists = GetNearPoltergeist(target, radius);
            UpdateNearestObjects(target, radius);
            StopAllCoroutines();

            if (_poltergeistList == null)
            {
                GameManager.GetGameManager().PlayerInstance.PolterNotFound();
                return;            
            }

            if(_nearPoltergeists.Count <= 0)
                GameManager.GetGameManager().PlayerInstance.PolterNotFound();

            foreach (var item in _nearPoltergeists)
            {
                item.StartPoltergeist();
            }
            _indexControl = SensingUtils.GetNearestIndex(_nearPoltergeists.ToArray(), target,
                                                         Camera.main.transform.right);
        }

        /// <summary> Get next node </summary>
        /// <param name="direction"> should be a number between -1 & 1</param>
        public Poltergeist_Item GetNext(int direction)
        {
            if (!_evaluating) // if it's not initialized
            {
                _evaluating = true;
                if (direction < 0)
                    _indexControl--;
            }
            else
            {
                if (direction > 0)
                    _indexControl++;
                else
                    _indexControl--;
            }

            _indexControl = Math.Clamp(_indexControl, 0, _nearPoltergeists.Count - 1);
            Debug.Log("IndexControl: " + _indexControl +
                      "\nNear: " + _nearPoltergeists.Count +
                      "\nTotal: " + _poltergeistList.Count);
            return _nearPoltergeists[_indexControl];
        }

        public void UpdateOrder(Poltergeist_Item item)
        {
            _nearPoltergeists = _nearPoltergeists.OrderBy(item =>
                Camera.main.WorldToScreenPoint(item.transform.position).x).ToList();
            _indexControl = _nearPoltergeists.IndexOf(item);
        }

        public void EndPoltergeist()
        {
            StartCoroutine(TimerCoroutine(_timerToDie, () =>
            {
                foreach (var item in _nearPoltergeists)
                {
                    item.EndPoltergeist();
                }
            }));
        }

        public void ActivatePoltergeist(Poltergeist_Item item) => item.Manipulate();

        public void DeactivateManipulation(Poltergeist_Item item) => item.NoManipulating();

        internal void AddPoltergeist(Poltergeist_Item item)
        {
            if (_poltergeistList == null)
                _poltergeistList = new List<Poltergeist_Item>();

            if (!_poltergeistList.Contains(item))            
                _poltergeistList.Add(item);
            
            else
                Debug.LogWarning("You're trying to ad an existing polter Item in the Polter " +
                    "manager", item);
        }

        internal void RemovePoltergeist(Poltergeist_Item item)
        {
            if (_poltergeistList == null || _poltergeistList.Count <= 0)
                return;

            if (_poltergeistList.Contains(item))
                _poltergeistList.Remove(item);
        }
        #endregion

        private void UpdateNearestObjects(Transform target, float radius)
        {
            //Check nullity
            if (_poltergeistList.Count <= 0)
                return;

            //GetNearest
            List<Poltergeist_Item> nearList = new(_poltergeistList.Count);

            for (int i = 0; i < _poltergeistList.Count; i++)
            {
                float distance = Vector3.Distance(_poltergeistList[i].transform.position,
                                                  target.position);
                if (distance < radius)
                    nearList.Add(_poltergeistList[i]);
            }


            _nearPoltergeists = nearList.OrderBy(item =>
                Camera.main.WorldToScreenPoint(item.transform.position).x).ToList();

            ////Sort them by a direction (The camera axis temp)
            ////[0]-1 [n/2]0 [n]1
            //List<Poltergeist_Item> sortedList = new();

            //sortedList.Add(nearList[0]);



            //for (int i = 1; i < nearList.Count; i++)
            //{
            //    var lastItem = nearList[i];

            //    for (int j = 0; j < i; j++)
            //    {
            //        Vector2 screenPosLast = Camera.main.WorldToScreenPoint(lastItem.transform.position);

            //        var currentItem = sortedList[j];
            //        Vector2 screenPosCurrent = Camera.main.WorldToScreenPoint(currentItem.transform.position);

            //        if (screenPosLast.x < screenPosCurrent.x)
            //        {

            //        }
            //    }

            //    sortedList[i] = lastItem;
            //}

        }

        #region Private Methods
        //private Poltergeist_Item[] GetNearPoltergeist(Transform target, float radius)
        //{
        //    //Check nullity
        //    if (_poltergeistList.Count <= 0)
        //        return null;

        //    //GetNearest
        //    List<Poltergeist_Item> nearList = new();

        //    for (int i = 0; i < _poltergeistList.Count; i++)
        //    {
        //        float distance = Vector3.Distance(_poltergeistList[i].transform.position,
        //                                          target.position);
        //        if (distance < radius)
        //            nearList.Add(_poltergeistList[i]);
        //    }

        //    //Sort them by a direction (The camera axis temp)
        //    //[0]-1 [n/2]0 [n]1

        //    Vector3 evaluatedDir = Camera.main.transform.right;
        //    Poltergeist_Item[] sortedList = new Poltergeist_Item[nearList.Count];

        //    sortedList[0] = nearList[0];
        //    for (int i = 1; i < nearList.Count; i++)
        //    {
        //        Poltergeist_Item lastItem = nearList[i];

        //        for (int j = 0; j < i; j++)
        //        {
        //            //Get values from current evaluation (lastItem)
        //            Vector3 targetToCurrentPolter = target.position - lastItem.transform.position;
        //            float distanceToCurrent = targetToCurrentPolter.magnitude;
        //            targetToCurrentPolter.Normalize();
        //            float dotProduct = Vector3.Dot(targetToCurrentPolter, evaluatedDir);

        //            //Get the values from the located item in the sortedList
        //            Vector3 targetToLocatedPolter = target.position - sortedList[j].transform.position;
        //            float distanceToLocated = targetToLocatedPolter.magnitude;
        //            targetToLocatedPolter.Normalize();
        //            float dotLocated = Vector3.Dot(targetToLocatedPolter, evaluatedDir);

        //            //Calculate the distance base in the axis
        //            float reflexDistanceCurrent = (evaluatedDir * distanceToCurrent).magnitude * dotProduct;
        //            float reflexDistanceLocated = (evaluatedDir * distanceToLocated).magnitude * dotLocated;

        //            if (reflexDistanceCurrent < reflexDistanceLocated)
        //            {
        //                Poltergeist_Item handler = sortedList[j]; //keep this position's item
        //                sortedList[j] = lastItem; //asign the item to the list
        //                lastItem = handler; //asign the object we grab from the sortedlist to the lastItem
        //            }
        //        }

        //        sortedList[i] = lastItem;
        //    }

        //    return sortedList;
        //}
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