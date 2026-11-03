# Ligue 1 Manager 2027 — Unity Android Starter

An original Unity starter project for a landscape Android football-management game prototype. It is not a finished commercial-scale football manager.

## Requirements
- Unity 6 LTS / Unity 6.0 (the project was prepared against Unity 6 APIs)
- Unity Hub modules for the same Editor version:
  - Android Build Support
  - Android SDK & NDK Tools
  - OpenJDK
- Android device running Android 8.0 (API 26) or newer for this starter build

## Open and create the first scene
1. Extract the ZIP.
2. In Unity Hub, choose **Add > Add project from disk** and select this folder.
3. Open with Unity 6.
4. Wait for scripts to compile.
5. In the Unity menu, choose **Ligue 1 Manager 2027 > 1. Configure Android Player Settings**.
6. Choose **Ligue 1 Manager 2027 > 2. Create Main Scene**.
7. Connect an Android phone with USB debugging enabled, or use Build Settings to build an APK.

## Build an installable APK
1. Confirm Android Build Support, Android SDK & NDK Tools, and OpenJDK are installed in Unity Hub.
2. In Unity, select **Ligue 1 Manager 2027 > 3. Build Android APK**.
3. The build script switches to Android, configures landscape orientation, fullscreen, ARM64 and IL2CPP, creates the scene if missing, and builds:
   `Builds/Android/Ligue1Manager2027.apk`
4. Copy the APK to your Android phone and install it. If Android blocks installation, allow **Install unknown apps** for the file manager/browser used to open it.

## Manual build alternative
- File > Build Profiles (or Build Settings in older Unity versions)
- Select Android and Switch Platform
- Add `Assets/Scenes/Main.unity` to the scene list
- Set Player Settings:
  - Product Name: Ligue 1 Manager 2027
  - Package Name: `com.independentstudio.ligue1manager2027`
  - Default Orientation: Landscape Left
  - Scripting Backend: IL2CPP
  - Target Architectures: ARM64
- Build to an `.apk` file (disable Build App Bundle if you specifically need an APK).

## Release signing
The starter is configured for a local development/test build. Before distributing publicly, create and securely back up your own keystore, configure it under **Project Settings > Player > Android > Publishing Settings**, and never share the keystore password or private signing key. Google Play generally expects an Android App Bundle (AAB) for new store releases.

## Current prototype contents
- Runtime-generated 3D pitch, goals, basic stands and placeholder crowd
- Generic capsule player placeholders and basic movement/ball-chasing
- Rigidbody ball and simple match-event simulation
- Match clock and scoreboard overlay
- Ligue 1 club-name seed data

## Known limitations
- The 3D match is a rough prototype: it does not yet have professional player models, motion-captured animations, tactical AI, realistic refereeing, robust ball control, or complete match rules.
- The current club list is a starter seed, not a guaranteed accurate roster of the 2026–27 Ligue 1 season. Player names, attributes, fixtures and transfers need a verified data source and appropriate rights.
- No official Football Manager assets, branding, code, UI, or player database are included. The intended UI direction is an original management interface inspired by the genre.
- This ZIP is a Unity project, not a compiled APK. An APK must be compiled by Unity with the Android modules installed. This environment does not have the Unity Editor or Android SDK/NDK configured, so it cannot produce or test the APK here.


## Cloud APK build from an Android phone (GitHub Actions + GameCI)

This project includes `.github/workflows/android-apk.yml`. It builds an APK on GitHub-hosted runners; no PC is needed to trigger or download the build once the repository is set up.

1. Create a GitHub account at https://github.com/join (if you do not already have one).
2. Create a **private** repository named `Ligue1Manager2027`.
3. Upload the extracted project files into the repository, keeping folder paths intact (especially `Assets`, `Packages`, `ProjectSettings`, and `.github`).
4. In the repository, open **Settings > Secrets and variables > Actions > New repository secret**.
5. Add a secret named `UNITY_LICENSE` containing your Unity Personal license file contents. GameCI's Unity license setup instructions explain how to obtain/activate a Personal license: https://game.ci/docs/github/activation/
6. Open the repository's **Actions** tab, select **Build Ligue 1 Manager 2027 Android APK**, and tap **Run workflow** (or push to `main`).
7. When the run succeeds, open the run, scroll to **Artifacts**, and download `Ligue1Manager2027-Android-APK`. Extract the downloaded artifact ZIP to get `Ligue1Manager2027.apk`.
8. On Android, open the APK and allow installation from that source if prompted.

Do not put Unity passwords, license files, signing keys, or tokens in source files or chat. Use GitHub Actions Secrets only. A first-time Unity Personal license activation can require a one-time manual activation step. This cloud workflow is a build route, not a guarantee that the current prototype will compile successfully on the first attempt; inspect the Actions logs if it fails.
