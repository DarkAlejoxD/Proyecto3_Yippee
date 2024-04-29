using UnityEditor;
using UnityEngine;

namespace UtilsComplements
{
    public static class UtilMethods
    {
#if UNITY_EDITOR
        [MenuItem(itemName: "Utils/Put Mesh Collider To Every MeshRenderer", isValidateFunction: false, priority = 1)]
        private static void MeshRendererToCollider()
        {
            var list = GameObject.FindObjectsOfType<MeshRenderer>();

            foreach (var item in list)
            {
                if (!item.TryGetComponent<MeshCollider>(out var a))
                {
                    item.gameObject.AddComponent<MeshCollider>();
                }
            }
        }

        [MenuItem(itemName: "Utils/Check Parity MeshCollider to Renderer", isValidateFunction: false, priority = 1)]
        private static void CheckParityMeshColliderRenderer()
        {
            var list = GameObject.FindObjectsOfType<MeshCollider>();

            for (int i = 0; i < list.Length; i++)
            {
                var item = list[i];
                if (!item.TryGetComponent<MeshRenderer>(out var a))
                {
                    GameObject.Destroy(item);
                }
            }
        }
#endif
    }
}