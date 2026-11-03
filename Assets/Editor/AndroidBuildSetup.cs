using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AndroidBuildSetup
{
    private const string SceneFolder = "Assets/Scenes";
    private const string ScenePath = SceneFolder + "/Main.unity";
    private const string OutputFolder = "Builds/Android";
    private const string ApkPath = OutputFolder + "/Ligue1Manager2027.apk";

    [MenuItem("Ligue 1 Manager 2027/1. Configure Android Player Settings")]
    public static void ConfigureAndroid()
    {
        PlayerSettings.companyName = "Independent Studio";
        PlayerSettings.productName = "Ligue 1 Manager 2027";
        PlayerSettings.applicationIdentifier = "com.independentstudio.ligue1manager2027";
        PlayerSettings.bundleVersion = "0.1.0";
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;
        PlayerSettings.runInBackground = false;

        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.useCustomKeystore = false;

        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        Debug.Log("Ligue 1 Manager 2027 Android settings configured. Add a production signing keystore before publishing.");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Ligue 1 Manager 2027/2. Create Main Scene")]
    public static void CreateMainScene()
    {
        Directory.CreateDirectory(SceneFolder);
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var bootstrap = new GameObject("GameBootstrap");
        bootstrap.AddComponent<GameBootstrap>();
        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.Refresh();
        Debug.Log("Created " + ScenePath);
    }

    [MenuItem("Ligue 1 Manager 2027/3. Build Android APK")]
    public static void BuildApk()
    {
        ConfigureAndroid();
        if (!Directory.Exists(SceneFolder) || !File.Exists(ScenePath))
            CreateMainScene();

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            bool switched = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            if (!switched)
            {
                Debug.LogError("Could not switch to Android. Install Android Build Support, Android SDK & NDK Tools, and OpenJDK in Unity Hub.");
                return;
            }
        }

        Directory.CreateDirectory(OutputFolder);
        var options = new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = ApkPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded: " + Path.GetFullPath(ApkPath) + " (" + report.summary.totalSize + " bytes)");
        else
            Debug.LogError("APK build failed: " + report.summary.result + ". Check the Unity Console for details.");
    }
}
