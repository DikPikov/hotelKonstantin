using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Text ResolutionInfo;
    [SerializeField] private Text QualityInfo;
    [SerializeField] private Text SensitivityInfo;
    [SerializeField] private Toggle FullScreen;
    [SerializeField] private Slider Audio;
    [SerializeField] private Slider Sensitivity;

    [SerializeField] private Settings Settings;

    private Config Config = null;

    private int Resolution = 0;

    private void Start()
    {
        Settings.OnChanges += UpdateInfo;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        Config = new Config();

        ResolutionInfo.text = $"{Config.XResolution}x{Config.YResolution}";
        switch (Config.Quality)
        {
            case 0:
                QualityInfo.text = $"Низкое";
                break;
            case 1:
                QualityInfo.text = $"Среднее";
                break;
            case 2:
                QualityInfo.text = $"Высокое";
                break;
        }

        SensitivityInfo.text = $"Чувствительность: {Config.Sensitivity}";
        FullScreen.isOn = Config.FullScreen == 1;
        Audio.value = Config.Audio;
        Sensitivity.value = Config.Sensitivity / 4f;

        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Config.XResolution == Screen.resolutions[i].width && Config.YResolution == Screen.resolutions[i].height)
            {
                Resolution = i;
                break;
            }
        }
    }

    public void Apply()
    {
        Settings.SetConfig(Config);
    }

    public void SetResolution()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Resolution = (Resolution + 1) % Screen.resolutions.Length;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Resolution--;
            if(Resolution < 0)
            {
                Resolution = Screen.resolutions.Length - 1;
            }
        }

        Config.XResolution = Screen.resolutions[Resolution].width;
        Config.YResolution = Screen.resolutions[Resolution].height;

        ResolutionInfo.text = $"{Config.XResolution}x{Config.YResolution}";
    }

    public void SetQuality()
    {
        Config.Quality = (Config.Quality + 1) % 3;
        switch (Config.Quality)
        {
            case 0:
                QualityInfo.text = $"Низкое";
                break;
            case 1:
                QualityInfo.text = $"Среднее";
                break;
            case 2:
                QualityInfo.text = $"Высокое";
                break;
        }
    }

    public void SetSensitivity(float value)
    {
        Config.Sensitivity = value * 4;
        SensitivityInfo.text = $"Чувствительность: {Config.Sensitivity}";
    }

    public void SetAudio(float value)
    {
        Config.Audio = value;
    }

    public void SetFullScreen(bool state)
    {
        Config.FullScreen = state ? 1 : 0;
    }
}
