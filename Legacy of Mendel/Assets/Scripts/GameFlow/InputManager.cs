using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private bool inputEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeKeyBindings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeKeyBindings()
    {
        // Default keybindings; these could be loaded from PlayerPrefs or a file for customization
        //Character movement
        keyBindings["MoveUp"] = KeyCode.W;
        keyBindings["MoveDown"] = KeyCode.S;
        keyBindings["MoveLeft"] = KeyCode.A;
        keyBindings["MoveRight"] = KeyCode.D;
        //Control units
        keyBindings["ControlUnits"] = KeyCode.Space;
        //Summoner skills
        keyBindings["SummonerSkill"] = KeyCode.F;
        //Camera Control
        keyBindings["CameraTurnLeft"] = KeyCode.Q;
        keyBindings["CameraTurnRight"] = KeyCode.E;
        //Pause menu
        keyBindings["PauseMenu"] = KeyCode.Escape;
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }

    public bool GetKey(string actionName)
    {
        if(!inputEnabled) { return false; }
        if (keyBindings.TryGetValue(actionName, out KeyCode key))
        {
            return Input.GetKey(key);
        }
        return false;
    }

    public bool GetKeyDown(string actionName)
    {
        if (actionName != "PauseMenu" && !inputEnabled) { return false; }
        if (keyBindings.TryGetValue(actionName, out KeyCode key))
        {
            return Input.GetKeyDown(key);
        }
        return false;
    }

    public bool GetKeyUp(string actionName)
    {
        if (!inputEnabled) { return false; }
        if (keyBindings.TryGetValue(actionName, out KeyCode key))
        {
            return Input.GetKeyUp(key);
        }
        return false;
    }

    public float GetHorizontal()
    {
        float horizontal = 0f;
        if (Input.GetKey(keyBindings["MoveRight"])) horizontal += 1f;
        if (Input.GetKey(keyBindings["MoveLeft"])) horizontal -= 1f;
        return horizontal;
    }

    public float GetVertical()
    {
        float vertical = 0f;
        if (Input.GetKey(keyBindings["MoveUp"])) vertical += 1f;
        if (Input.GetKey(keyBindings["MoveDown"])) vertical -= 1f;
        return vertical;
    }

    public KeyCode GetCurrentKeyForAction(string actionName)
    {
        if (keyBindings.TryGetValue(actionName, out KeyCode currentKey))
        {
            return currentKey;
        }
        else
        {
            Debug.LogWarning($"Action name {actionName} not found in keyBindings.");
            return KeyCode.None; // Return a default or placeholder KeyCode if the action name is not found.
        }
    }

    // Method to reassign keys
    public void SetKeyBinding(string actionName, KeyCode newKey)
    {
        if (keyBindings.ContainsKey(actionName))
        {
            keyBindings[actionName] = newKey;
            // Save the new keybinding, e.g., to PlayerPrefs
        }
    }

    public void StartRebindProcess(string actionName, System.Action onRebindComplete = null)
    {
        StartCoroutine(WaitForKeyPress(actionName, onRebindComplete));
    }

    IEnumerator WaitForKeyPress(string actionName, System.Action onRebindComplete)
    {
        bool keyFound = false;
        while (!keyFound)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Optionally handle ESC-specific logic here
                onRebindComplete?.Invoke();
                yield break; // Immediately exit the coroutine
            }

            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Ensure ESC doesn't set a new binding, unless that's intended
                    if (keyCode == KeyCode.Escape) continue;

                    SetKeyBinding(actionName, keyCode);
                    Debug.Log($"Rebound {actionName} to {keyCode}");
                    keyFound = true;
                    onRebindComplete?.Invoke();
                    yield break; // Exit the coroutine after setting a new key binding
                }
            }
            yield return null; // Wait for the next frame
        }
    }

}
