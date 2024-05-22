using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame //add it to a concrete namespace
{
    public class DEBUG_MapPositions : MonoBehaviour
    {
        #region Fields
        [Header("Section1")]
        public Transform _player;
        public List<DEBUG_PositionMapped> _positionMap = new();

        public void AddItem()
        {
            _positionMap.Add(new DEBUG_PositionMapped());
        }
        #endregion    
    }

    [Serializable]
    public class DEBUG_PositionMapped
    {
        public Vector3 WorldPos = Vector3.zero;
        public string Identifier = "";

        public void SetPlayerPosition(Transform player)
        {
            WorldPos = player.position;
        }

        public void TeleportPlayerTo(Transform player)
        {
            player.position = WorldPos;
        }
    }
}

#region DEBUG
#if UNITY_EDITOR
namespace BaseGame
{
    using System.Linq;
    using UnityEditor;

    [CustomEditor(typeof(DEBUG_MapPositions))]
    public class DEBUG_MapPositionsEditor : Editor
    {
        DEBUG_MapPositions _object;

        private void OnEnable()
        {
            _object = (DEBUG_MapPositions)target;
            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            var list = _object._positionMap;

            _object._player = (Transform)EditorGUILayout.ObjectField("Player", _object._player, typeof(Transform), true);

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.Space(10);
                DrawMapControl(list.ElementAt(i));
            }

            if (DrawButton("AddItem"))
            {
                _object.AddItem();
            }
        }

        public void DrawMapControl(DEBUG_PositionMapped map)
        {
            EditorGUILayout.BeginHorizontal();

            map.Identifier = EditorGUILayout.TextField(map.Identifier, GUILayout.Width(100));

            if (DrawButton("SavePosition"))
            {
                map.SetPlayerPosition(_object._player);
            }

            if (DrawButton("GoToPosition"))
            {
                map.TeleportPlayerTo(_object._player);
            }

            if (DrawButton("-", 20))
            {
                RemoveItem(map);
            }

            if (DrawButton("^", 20))
            {
                MoveItem(map, -1);
            }

            if (DrawButton("v", 20))
            {
                MoveItem(map, 1);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox($"Position: {map.WorldPos}", MessageType.Info);
        }

        public void MoveItem(DEBUG_PositionMapped item, int variation)
        {
            int index = _object._positionMap.IndexOf(item);
            index += variation;
            index = Math.Clamp(index, 0, _object._positionMap.Count - 1);
            _object._positionMap.Remove(item);
            _object._positionMap.Insert(index, item);
        }

        public void RemoveItem(DEBUG_PositionMapped item)
        {
            _object._positionMap.Remove(item);
        }

        public bool DrawButton(string name, int width = 100)
        {
            return GUILayout.Button(name, GUILayout.Width(width));
        }
    }
}
#endif
#endregion
