namespace UtilsComplements
{
    /// <summary>
    /// Put Instance.Instantiate() inside the Awake() Method.
    /// Put Instance.RemoveInstance() inside the OnDestroy Method.
    /// Override Invalidate() method if necessary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleton<T> where T : class, ISingleton<T>
    {
        
        private static T _singleton;    //Static Singleton Instance

        public ISingleton<T> Instance { get; }  //Reference to the interface methods inside a class
        public T Value => (T)this;              //Reference to the class inside the interface

        //Call these functions with "ISingleton<T>.xxx();" or "Singleton.xxx<T>()"
        #region Static Fields & Methods
        public static T GetInstance() => _singleton;
        public static bool TryGetInstance(out T instance)
        {
            instance = _singleton;
            return !(_singleton == null || _singleton == default);
        }
        #endregion

        #region Instance Fields & Methods
        public void Instantiate()
        {
            if (_singleton == null)            
                _singleton = Value;
            else
                Invalidate();
        }

        public void RemoveInstance()
        {
            if (_singleton == Value)
                _singleton = null;
        }

        public void Invalidate()
        {
#if UNITY_2017_1_OR_NEWER
            if (Value is UnityEngine.Component comp)
            {
                //UnityEngine.Debug.Log("It already Exists", comp);//_singleton as UnityEngine.Component);
                UnityEngine.Component.Destroy(comp.gameObject);
            }
#endif
        }
        #endregion
    }
}