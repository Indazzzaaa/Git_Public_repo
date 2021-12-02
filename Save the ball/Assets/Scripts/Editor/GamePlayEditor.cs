using System;
using System.IO;
using Codice.CM.Common.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Editor
{
    public class GamePlayEditor : EditorWindow
    {
        #region variables

        private bool _fouldOut;
        private string _sceneName;

        #endregion


        // Add menu named "Sumant" to the Window menu
        [MenuItem("Sumant/Scene Manager")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            GamePlayEditor window =
                (GamePlayEditor) EditorWindow.GetWindow(typeof(GamePlayEditor));
            window.Show();
        }

        private void GuiItems()
        {
            titleContent = new GUIContent("Scene Manager");

            EditorGUILayout.Space(10);

            var buttonStyles = GUI.skin.button;
            buttonStyles.padding = new RectOffset(10, 10, 10, 10);

            buttonStyles.stretchWidth = true;
            // GUI.color = Color.yellow;
            // this shows the level info
            _fouldOut = EditorGUILayout.Foldout(_fouldOut, "Go to scene");
            if (_fouldOut)
            {
                CreateButtonForEachSceneFile();
            }

            GUILayout.Space(15);
            GUI.color = EditorApplication.isPlaying ? Color.red :
                EditorApplication.isPaused ? Color.yellow : Color.green;
            buttonStyles.stretchWidth = false;
            // this will help us to enter and exit the play mode 
            if (GUILayout.Button(EditorApplication.isPlaying
                ? "Exit play"
                : (EditorApplication.isPaused ? "Resume Play" : "Enter Play")))
            {
                if (EditorApplication.isPlaying)
                {
                    EditorApplication.ExitPlaymode();
                }
                else
                {
                    EditorApplication.EnterPlaymode();
                }
            }
        }

        private void OnGUI()
        {
            GuiItems();
            EditorGUILayout.Space(20);
            CreateNewScene();
        }

        private void CreateNewScene()
        {
            GUI.color = Color.white;
            _sceneName = EditorGUILayout.TextField("Scene Name", _sceneName);
            GUI.color = Color.grey;

            if (GUILayout.Button("Create Scene"))
            {
                EditorSceneManager.SaveOpenScenes();
                var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                EditorSceneManager.SaveScene(newScene, $"Assets/Scenes/{_sceneName}.unity");
            }
        }

        private void CreateButtonForEachSceneFile()
        {
            var info = new DirectoryInfo("Assets/Scenes");
            var fileInfo = info.GetFiles();
            foreach (var file in fileInfo)
            {
                if (!file.Name.EndsWith(".unity")) continue;
                var name = file.Name;
                if (GUILayout.Button(name.Substring(0, name.IndexOf(".", StringComparison.Ordinal))))
                {
                    EditorSceneManager.SaveOpenScenes();
                    EditorSceneManager.OpenScene(file.FullName);
                    // Debug.Log("Hello there");
                }
            }
        }


        #region Test

        private void GetTheAllFileFromDirectory()
        {
            var info = new DirectoryInfo("Assets/Scenes");
            var fileInfo = info.GetFiles();
            foreach (var file in fileInfo)
            {
                if (file.Name.EndsWith(".unity"))
                {
                    Debug.Log(file.FullName);
                    // Debug.Log(file.Name);
                }
            }
        }

        #endregion
    }
}