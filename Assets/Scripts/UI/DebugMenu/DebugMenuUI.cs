using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Helper behavior which manages the debug UI. </summary>
public class DebugMenuUI : MonoBehaviour
{
    #region Editor

    [Header("Global")] [Tooltip("Display the debug UI?")]
    public bool displayUI;

    #endregion // Editor

    #region Internal

    /// <summary> Dimensions of the main window. </summary>
    private static Vector2 WINDOW_DIMENSION = new Vector2(256.0f, 192.0f);

    /// <summary> Base padding used within the UI. </summary>
    private static float BASE_PADDING = 8.0f;

    /// <summary> Rectangle representing the screen drawing area. </summary>
    private Rect mScreenRect;

    /// <summary> Rectangle representing the main window. </summary>
    private Rect mMainWindowRect;

    /// <summary> Dummy value used for demonstration. </summary>
    private float mDummyValue = 0.0f;

    #endregion // Internal

    #region Interface

    #endregion // Interface

    /// <summary> Called when the script instance is first loaded. </summary>
    private void Awake()
    {
    }

    /// <summary> Called before the first frame update. </summary>
    void Start()
    {
        // Deduce the drawing screen area from the main camera.
        var mainCamera = GameSettings.Instance.mainCamera;
        mScreenRect = new Rect(
            mainCamera.rect.x * Screen.width,
            mainCamera.rect.y * Screen.height,
            mainCamera.rect.width * Screen.width,
            mainCamera.rect.height * Screen.height
        );
        // Initially place the debug window into the top right corner.
        mMainWindowRect = new Rect(
            mScreenRect.x + mScreenRect.width - WINDOW_DIMENSION.x, mScreenRect.y,
            WINDOW_DIMENSION.x, WINDOW_DIMENSION.y
        );
    }

    /// <summary> Update called once per frame. </summary>
    void Update()
    {
    }

    /// <summary> Called when GUI drawing should be happening. </summary>
    private void OnGUI()
    {
        if (displayUI)
        {
            // Only draw the window if we are currently displaying it.
            mMainWindowRect = GUI.Window(0, mMainWindowRect, MainWindowUI, "Cheat Console");
            // Limit the window position to the screen area.
            mMainWindowRect.x = Mathf.Clamp(
                mMainWindowRect.x, mScreenRect.x,
                mScreenRect.x + mScreenRect.width - WINDOW_DIMENSION.x
            );
            mMainWindowRect.y = Mathf.Clamp(
                mMainWindowRect.y, mScreenRect.y,
                mScreenRect.y + mScreenRect.height - WINDOW_DIMENSION.y
            );
        }
    }

    /// <summary> Function used for drawing the main window. </summary>
private void MainWindowUI(int windowId)
{
    GUILayout.BeginArea(new Rect(
        BASE_PADDING, 2.0f * BASE_PADDING,
        WINDOW_DIMENSION.x - 2.0f * BASE_PADDING,
        WINDOW_DIMENSION.y - 3.0f * BASE_PADDING
    ));
    {
        GUILayout.BeginVertical();
        {
            // Currency Slider
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Currency: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));
                var currency = InventoryManager.Instance.availableCurrency;
                currency = (int)GUILayout.HorizontalSlider(currency, 0.0f, 1000.0f, GUILayout.ExpandWidth(true));
                if (GUI.changed)
                {
                    InventoryManager.Instance.availableCurrency = currency;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10); // Add space between sections

            // Enable Dummy Character Button
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Dummy",
                    GUILayout.Width(WINDOW_DIMENSION.x / 4.0f),
                    GUILayout.Height(WINDOW_DIMENSION.y / 6.0f)))
                {
                    GameManager.Instance.TogglePlayerCharacter();
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10); // Add space between sections

            // Debug Tools Section
            GUILayout.BeginVertical("", GUI.skin.box);
            {
                // Interactive Mode Toggle
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Interactive Mode: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));
                    var interactiveMode = GameManager.Instance.interactiveMode;
                    if (GUILayout.Button(interactiveMode ? "Enabled" : "Disabled", GUILayout.ExpandWidth(true)))
                    {
                        GameManager.Instance.interactiveMode = !interactiveMode;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                // Master Volume Slider
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Master Volume: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));
                    var masterVolume = SoundManager.Instance.masterVolume;
                    masterVolume = GUILayout.HorizontalSlider(masterVolume, -80.0f, 20.0f, GUILayout.ExpandWidth(true));
                    if (GUI.changed)
                    {
                        SoundManager.Instance.masterVolume = masterVolume;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                // Master Mute Toggle
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Sound Muted: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));
                    var masterMuted = SoundManager.Instance.masterMuted;
                    if (GUILayout.Button(masterMuted ? "Muted" : "Unmuted", GUILayout.ExpandWidth(true)))
                    {
                        SoundManager.Instance.masterMuted = !masterMuted;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }
    GUILayout.EndArea();

    GUI.DragWindow(new Rect(
        2.0f * BASE_PADDING, 0.0f,
        WINDOW_DIMENSION.x - 4.0f * BASE_PADDING,
        WINDOW_DIMENSION.y
    ));
}



}

