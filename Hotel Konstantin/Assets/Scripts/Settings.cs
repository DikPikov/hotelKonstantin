using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private PostProcessVolume PostProcess;
    [SerializeField] private PlayerRotation Rotation;
    private Config Config;

    public event SimpleVoid OnChanges = null;

    public Config _Config
    {
        get
        {
            return Config;
        }
        set
        {
            Config = value;

            File.WriteAllText(Path.Combine(Application.persistentDataPath, "config.txt"), JsonUtility.ToJson(Config));

            UpdateSettings();

            if (OnChanges != null)
            {
                OnChanges.Invoke();
            }
        }
    }

    private void Awake()
    {
        Time.timeScale = 1;
        string path = Path.Combine(Application.persistentDataPath, "config.txt");

        Config = null;

        if (File.Exists(path))
        {
            Config = JsonUtility.FromJson<Config>(File.ReadAllText(path));
        }

        if(Config == null)
        {
            Config = new Config();
        }

        UpdateSettings();
    }

    public void UpdateSettings()
    {
        Mixer.SetFloat("Volume", Config.Audio == 0 ? -80 : (-30 * (1 - Config.Audio)));

        Screen.SetResolution(Config.XResolution, Config.YResolution, Config.FullScreen);

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Rotation._Sensitivity = Config.Sensitivity;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[Config.LanguageID];

        PostProcess.weight = Config.PostProcessing ? 1 : 0;

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
    public bool FullScreen;

    public float Sensitivity;

    public float Audio;

    public int Quality;
    public bool PostProcessing;

    public bool AlwaysShowInterface;

    public int LanguageID;

    /*
    Коды раскладок: 
    ru Русский [0]
    en Английский [1]
    sv Шведский [2]
    pl Польский [3]
    de Немецкий [4]
    fr Французский [5]
    it Итальянский [6]
    es Испанский [7]
    pt Португальский [8]
    tr Турецкий [9]
    zh Китайский (упрощенный) [10]
    hi Хинди [11]
    vi Вьетнамский [12]
    id Индонезийский [13]
    */



    public Config()
    {
        int high = 0;
        for (int i = 1; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[high].width < Screen.resolutions[i].width)
            {
                high = i;
            }
        }

        XResolution = Screen.resolutions[high].width;
        YResolution = Screen.resolutions[high].height;
        FullScreen = true;

        Sensitivity = 1;
        Audio = 0.5f;
        Quality = 1;
        PostProcessing = true;
        LanguageID = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        AlwaysShowInterface = false;
    }

    public Config Clone()
    {
        Config config = new Config();

        config.XResolution = XResolution;
        config.YResolution = YResolution; 
        config.FullScreen = FullScreen;

        config.Sensitivity = Sensitivity;

        config.Audio = Audio;

        config.Quality = Quality;
        config.PostProcessing = PostProcessing;

        config.AlwaysShowInterface = AlwaysShowInterface;
        
        config.LanguageID = LanguageID;

        return config;

    }
}
