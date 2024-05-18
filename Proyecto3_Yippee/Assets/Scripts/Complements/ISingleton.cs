namespace UtilsComplements
{
    /// <summary>
    /// Script rewrited.
    /// Short Version of my ISingleton interface.
    /// 
    /// Put Instance.Instantiate() inside the Awake() Method.
    /// Put Instance.RemoveInstance() inside the OnDestroy Method.
    /// Override Invalidate() method if necessary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleton<T> where T : class, ISingleton<T>
    {
        //TODO: Ask a teacher if recommends this style of Singleton o Generic interfaces/classes.
        private static T _singleton;
        private static bool _exists = false;

        public ISingleton<T> Instance { get; }
        public T Value => (T)this;

        #region Static Fields & Methods
        //Call these functions with ISingleton<T>.xxx();
        public static bool Exists() => _exists; //(?
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
            //Uncomment this to reset Singleton
            //_singleton = null;
            //return;

            if (_singleton == null)
            {
                _singleton = Value;
                _exists = true;
            }
            else
                Invalidate();
        }

        public void RemoveInstance()
        {
            if (_singleton == Value)
            {
                _singleton = null;
                _exists = false;
            }
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