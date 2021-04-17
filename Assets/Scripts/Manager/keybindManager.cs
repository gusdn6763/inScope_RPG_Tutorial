using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager instance;

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

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = keybinds;

        //key값에 ACTION 문자가 포함되어 있으면 액션키로 변경  
        if (key.Contains("ACTION"))
        {
            currentDictionary = ActionBinds;
        }
        //현재 딕셔너리에 키값이 포함되어 있지 않으면  
        if (!currentDictionary.ContainsKey(key))
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
        UIManager.instance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }
    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;
            Debug.Log(Event.current);
            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
