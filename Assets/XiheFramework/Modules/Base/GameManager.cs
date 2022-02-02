﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using XiheFramework.Base;

namespace XiheFramework {
    public class GameManager : Singleton<GameManager> {
        public int frameRate = 60;

        private const int FrameAtSceneId = 0;

        private readonly Dictionary<Type, GameModule> m_GameModules = new Dictionary<Type, GameModule>();

        private Queue<GameModule> m_RegisterGameModulesQueue = new Queue<GameModule>();

        private void Awake() {
            Application.targetFrameRate = frameRate;

            RegisterAllComponent();

            foreach (var component in m_GameModules.Values) {
                component.Setup();
            }

            Debug.LogFormat("XiheFramework Initialized");
        }

        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        private void RegisterAllComponent() {
            while (m_RegisterGameModulesQueue.Count > 0) {
                var module = m_RegisterGameModulesQueue.Dequeue();

                Instance.m_GameModules.Add(module.GetType(), module);
            }
        }

        public static void RegisterComponent(GameModule component) {
            if (component == null) {
                Debug.LogErrorFormat("Registering a null component");
                return;
            }

            if (Instance.m_GameModules.ContainsKey(component.GetType())) {
                Debug.LogErrorFormat("Component: {0} has already existed", component.GetType().Name);
                return;
            }

            //Instance.m_GameComponents.Add(component.GetType(), component);
            Instance.m_RegisterGameModulesQueue.Enqueue(component);
        }

        public static T GetModule<T>() where T : GameModule {
            var t = typeof(T);
            if (Instance == null) {
                return null;
            }

            if (Instance.m_GameModules.TryGetValue(t, out var value)) {
                return (T) value;
            }

            Debug.LogErrorFormat("Component: {0} does not exist", t.Name);
            return null;
        }

        public static void ShutDown(ShutDownType restartType) {
            for (int i = 0; i < Instance.m_GameModules.Count; i++) {
                Instance.m_GameModules.ElementAt(i).Value.ShutDown();
            }

            //Instance.m_GameComponents.Clear();

            switch (restartType) {
                case ShutDownType.None:
                    break;
                case ShutDownType.Restart:
                    // Game.Scene.LoadScene("Opening");
                    SceneManager.LoadScene("Entrance");
                    //back to menu
                    break;
                case ShutDownType.Quit:
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(restartType), restartType, null);
            }
        }
    }
}