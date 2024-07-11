using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings_AudioUnit : MonoBehaviour
{
    //this will be responsible for contrlling and updating because of a certain audio.
    //now i need to define

    public Setting_AudioType audioType;

    [SerializeField] Slider _slider; //
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] TextMeshProUGUI nameText;

    private void Update()
    {
        
    }

    bool isInit;
    public void UpdateAudioValue(SettingsData data)
    {

       float audioValue = data.Get_Audio(audioType);


        nameText.text = audioType.ToString();

        _slider.value = audioValue;
        _inputField.text = audioValue.ToString();

        isInit = true;
    }

    public void UpdateSlider()
    {
        if (!isInit) return;
        float value = _slider.value;
        _inputField.text = ((int)value).ToString();
        UpdateValue((int)value);

    }
    public void UpdateInputText()
    {
        //you need to press enter to confirm the value_Level in the thing. so everytime you press enter we check
        //we need to check 

        int value = 0;
        bool IsSuccess = int.TryParse(_inputField.text, out value);

        if (!IsSuccess)
        {
            Debug.Log("failed to parse");
        }

        value = Mathf.Clamp(value, 0, 100); 

        _inputField.text = value.ToString();    

        _slider.value = value;

        UpdateValue(value);
    }

    public void UpdateValue(int value)
    {
        GameHandler.instance._settingsData.Set_Audio(audioType, value);
        Debug.Log("update value " + audioType + " " + value);
    }

    //and we instantly update it.

    //


    private void OnDisable()
    {
        //we check if the text input is wrong.
    }

}
public enum Setting_AudioType
{
    Master,
    Sfx,
    BackgroundMusic
}