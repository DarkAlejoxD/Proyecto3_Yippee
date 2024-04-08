public interface ISingleton<T> where T : class, ISingleton<T>
{
    //TODO: Complete this class. Ask a teacher if recommends this style of Singleton o Generic interfaces/classes.
    private static T _singleton;

    public ISingleton<T> Instance => this;
    public T Value => (T)this;

    #region Static Fields & Methods

    #endregion

    #region Instance Fields & Methods
    public sealed void Instantiate()
    {
        if (_singleton == null || _singleton == default)        
            _singleton = Value;
        
        else
            Invalidate();        
    }

    public sealed void RemoveInstance()
    {
        if (_singleton == Value)        
            _singleton = null;        
    }

    public void Invalidate()
    {
#if UNITY_2022_3_OR_NEWER
        if(Value is UnityEngine.Component comp)        
            UnityEngine.Component.Destroy(comp);        
#endif
    }
    #endregion
}