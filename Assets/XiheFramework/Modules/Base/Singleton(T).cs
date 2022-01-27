﻿using System.Runtime.CompilerServices;
using FlowCanvas.Nodes;
using UnityEngine;

namespace XiheFramework.Base {
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        // Check to see if we're about to be destroyed.
        // private static object m_SyncRoot = new object();

        private static T m_Instance;

        public static T Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = FindObjectOfType<T>();
                }

                return m_Instance;
            }
            private set => m_Instance = value;
        }

        // private void OnApplicationQuit() {
        //     shuttingDown = true;
        // }
        //
        //
        // private void OnDestroy() {
        //     shuttingDown = true;
        // }
    }
}