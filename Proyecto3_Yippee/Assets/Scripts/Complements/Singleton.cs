using System;
using System.Reflection;

namespace UtilsComplements
{
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

        //Fails
//#if UNITY_EDITOR
//        [UnityEditor.MenuItem(itemName: "Utils/ResetSingletons", isValidateFunction: false, priority = 1)]
//        public static void ResetSingletons()
//        {
//            var interfaceType = typeof(ISingleton<>);
//            var list = ExtraFunctions.GetGenericInterfaceImplementations(interfaceType);

//            foreach (var item in list)
//            {
//                var instance = Activator.CreateInstance(item);

//                var method = item.GetMethod("ForceRemoveInstance");
//                method.Invoke(instance, new object[0]);
//            }   
//        }
//#endif
    }
}