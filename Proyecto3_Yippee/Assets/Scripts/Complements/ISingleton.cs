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
            if (_singleton == null || _singleton == default)
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
#if UNITY_2022_3_OR_NEWER //It should be working even in older versions, it's just a testing(?
            if (Value is UnityEngine.Component comp)
                UnityEngine.Component.Destroy(comp.gameObject);
#endif
        }
        #endregion
    }

    public static class Singleton
    {
        public static T GetSingleton<T>() where T : class, ISingleton<T>
        {
            return ISingleton<T>.GetInstance();
        }

        public static bool TryGetInstance<T>(out T instance) where T : class, ISingleton<T>
        {
            return ISingleton<T>.TryGetInstance(out instance);
        }
    }
}