using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace //add it to a concrete namespace
{    
    public class NavBarController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<NavItem> _navItems;
        
        #endregion    

        #region Unity Logic
        private void Start()
        {           
            foreach(NavItem navItem in _navItems)
            {
                navItem.Deactivate();
                navItem.gameObject.SetActive(false);
            }

            _navItems[0].gameObject.SetActive(true);
            _navItems[0].Activate();
        }

        private void Update()
        {        
        }
        #endregion

        #region Static Methods
        public static void StaticMethod()
        {
        }
        #endregion

        #region Public Methods
        public void PublicMethod()
        {
        }
        #endregion

        #region Private Methods
        private void PrivateMethod()
        {
        }
        #endregion
    }
}

public class NavItem : MonoBehaviour
{
    private bool _active;


    //Pa cosas futuras
    public void Activate()
    {
        _active = true;
    }

    public void Deactivate()
    {
        _active = false;
        
    }
}
