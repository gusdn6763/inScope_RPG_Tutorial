using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class keybindManager : MonoBehaviour
{
    private static keybindManager instance;

    public Dictionary<string, KeyCode> keybinds { get; set; }

    public Dictionary<string, KeyCode> ActionBinds { get; set; }

    private string bindName;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        keybinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("Action1", KeyCode.Alpha1);
        BindKey("Action2", KeyCode.Alpha2);
        BindKey("Action3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = keybinds;

        if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }
        if (!currentDictionary.ContainsValue(keyBind))
        {
            currentDictionary.Add(key, keyBind);
            UIManager.instance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[myKey] = KeyCode.None;
            UIManager.instance.UpdateKeyText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        bindName = string.Empty;
    }
}
