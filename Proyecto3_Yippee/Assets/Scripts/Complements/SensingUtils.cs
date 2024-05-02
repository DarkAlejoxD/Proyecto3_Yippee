using UnityEngine;

namespace UtilsComplements
{
    public static class SensingUtils
    {
        /// <summary> Get the nearets index from the player </summary>
        /// <returns></returns>
        public static int GetNearestIndex<T>(T[] list, Transform target) where T : Component
        {
            int index = 0;

            for (int i = 1; i < list.Length; i++)
            {
                float distanceToLast = Vector3.Distance(list[index].transform.position, target.position);
                float distanceToCurrent = Vector3.Distance(list[i].transform.position, target.position);

                if (distanceToLast > distanceToCurrent)
                    index = i;
            }

            return index;
        }

        public static T GetNearestObject<T>(T[] list, Transform target) where T : Component
        {
            int index = GetNearestIndex(list, target);
            return list[index];
        }

        ///// <summary> Get nearest index only having in consideration 1 axis </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="direction"> the evaluated 1D axis </param>
        ///// /// <param name="absolute"> Get only the nearest positive value </param>
        ///// <returns></returns>
        //private int GetNearestIndex<T>(T[] list, Transform target, Vector3 direction, bool onlyPositive = false) where T : Component
        //{
        //    //int index = 0;

        //    //for (int i = 1; i < list.Length; i++)
        //    //{
        //    //    float dotToLast = Vector3.Distance(list[index].transform.position, target.position);
        //    //    float dotToCurrent = Vector3.Distance(list[i].transform.position, target.position);

        //    //    if (dotToLast > dotToCurrent)
        //    //        index = i;
        //    //}

        //    //return index;
        //    return -1;
        //}
    }
}