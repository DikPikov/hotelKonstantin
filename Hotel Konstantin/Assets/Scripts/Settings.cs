using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private PostProcessVolume PostProcess;
    [SerializeField] private PlayerRotation Rotation;
    private Config Config;

    public event SimpleVoid OnChanges = null;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("Initialized") == 0)
        {
            PlayerPrefs.SetInt("Initialized", 1);

            PlayerPrefs.SetInt("Xres", Screen.width);
            PlayerPrefs.SetInt("Yres", Screen.height);

            PlayerPrefs.SetFloat("Sens", 1);

            PlayerPrefs.SetFloat("Audio", 1);

            PlayerPrefs.SetInt("Qual", 1);
            PlayerPrefs.SetInt("PostP", 1);

            PlayerPrefs.SetInt("FullScreen", 0);

            PlayerPrefs.Save();
        }

        Config = new Config();
    }

    public void SetConfig(Config config)
    {
        config.Save();

        Config = new Config();

        UpdateSettings();

        if (OnChanges != null)
        {
            OnChanges.Invoke();
        }
    }

    public void UpdateSettings()
    {
        Mixer.SetFloat("Volume", Config.Audio == 0 ? -80 : (-30 * (1 - Config.Audio)));

        Screen.SetResolution(Config.XResolution, Config.YResolution, Config.FullScreen == 1);

        Rotation._Sensitivity = Config.Sensitivity;

        PostProcess.weight = Config.PostProcessing == 1 ? 1 : 0;

        QualitySettings.SetQualityLevel(Config.Quality, true);
        switch (Config.Quality)
        {
            case 0:
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier1;
                break;
            case 1:
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier2;
                break;
            case 2:
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier3;
                break;
        }
    }
}

public class Config
{
    public int XResolution;
    public int YResolution;
    public int FullScreen;

    public float Sensitivity;

    public float Audio;

    public int Quality;
    public int PostProcessing;

    public Config()
    {
        XResolution = PlayerPrefs.GetInt("Xres");
        YResolution = PlayerPrefs.GetInt("Yres");

        Sensitivity = PlayerPrefs.GetFloat("Sens");

        Audio = PlayerPrefs.GetFloat("Audio");

        Quality = PlayerPrefs.GetInt("Qual");
        PostProcessing = PlayerPrefs.GetInt("PostP");

        FullScreen = PlayerPrefs.GetInt("FullScreen");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Xres", XResolution);
        PlayerPrefs.SetInt("Yres", YResolution);

        PlayerPrefs.SetFloat("Sens", Sensitivity);

        PlayerPrefs.SetFloat("Audio", Audio);

        PlayerPrefs.SetInt("Qual", Quality);
        PlayerPrefs.SetInt("PostP", PostProcessing);

        PlayerPrefs.SetInt("FullScreen", FullScreen);
        
        PlayerPrefs.Save();
    }
}
