using UnityEngine;
using System.Collections;

public class AudioEditer : MonoBehaviour
{

    public AudioClip SceneBGM = new AudioClip();
    public AudioClip BossBGM = new AudioClip();
    public AudioClip LastBGM = new AudioClip();
    public AudioClip InitBGM = new AudioClip();
    public AudioClip GachaBGM = new AudioClip();
    public AudioClip GateBGM = new AudioClip();
    public AudioClip WinBGM = new AudioClip();
    public AudioClip LoseBGM = new AudioClip();
    
    public AudioClip Fight1BGM = new AudioClip();
    public AudioClip Fight2BGM = new AudioClip();
    public AudioClip Fight3BGM = new AudioClip();
    public AudioClip Fight4BGM = new AudioClip();

    public AudioClip PVPBGM = new AudioClip();

    AudioClip closeSoundClip = new AudioClip();

    public static AudioEditer instance;

    string LastScene = "";

    public bool playBGM;
    public bool playSound;

    public bool playContSound;

    public AudioSource audioSource;

    //备份信息
    string BackupFight = "";

    void Start()
    {
        instance = this;
        //InitBGM = Resources.Load("Audio/bgm_login") as AudioClip;
        //SceneBGM = Resources.Load("Audio/bgm_lobby") as AudioClip;
        //WinBGM = Resources.Load("Audio/ui_victory") as AudioClip;
        //LoseBGM = Resources.Load("Audio/ui_defeat") as AudioClip;
        //GachaBGM = Resources.Load("Audio/bgm_lottery") as AudioClip;
        //GateBGM = Resources.Load("Audio/bgm_selectmap") as AudioClip;
        //BossBGM = Resources.Load("Audio/bgm_BOSS") as AudioClip;
        //Fight1BGM = Resources.Load("Audio/bgm_battlePVE1") as AudioClip;
        //Fight2BGM = Resources.Load("Audio/bgm_battlePVE2") as AudioClip;
        //Fight3BGM = Resources.Load("Audio/bgm_battlePVE3") as AudioClip;
        //Fight4BGM = Resources.Load("Audio/bgm_battlePVE4") as AudioClip;
        //PVPBGM = Resources.Load("Audio/bgm_battlePVP") as AudioClip;

        audioSource = GameObject.Find("UIRoot").GetComponent<AudioSource>();

        playBGM = (PlayerPrefs.GetInt("musicIsOpen", 1) == 1);
        playSound = (PlayerPrefs.GetFloat("EffectMusicSlider", 1) == 1);
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            audioSource.volume = 1;
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    public void SetFightBGM(string Fight1, string Fight2, string Fight3)
    {
        //Debug.Log(Fight1);
        //Fight1BGM = Resources.Load("Audio/" + Fight1) as AudioClip;
        //Fight2BGM = Resources.Load("Audio/" + Fight2) as AudioClip;
        //Fight3BGM = Resources.Load("Audio/" + Fight3) as AudioClip;
        //PlayLoop("Fight1");
    }

    public void PlayLoop(string Type)
    {
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            AudioClip ac = new AudioClip();
            audioSource.loop = true;

            switch (Type)
            {
                case "Init":
                    ac = Resources.Load("Audio/bgm_login") as AudioClip;
                    break;
                case "Weijia":
                    ac = Resources.Load("Audio/bgm_openning") as AudioClip;
                    break;
                case "Scene":
                    ac = Resources.Load("Audio/bgm_lobby") as AudioClip;
                    break;
                case "Gacha":
                    ac = Resources.Load("Audio/bgm_lottery") as AudioClip;
                    break;
                case "Boss":
                    ac = Resources.Load("Audio/bgm_BOSS") as AudioClip;
                    break;
                case "Gate":
                    ac = Resources.Load("Audio/bgm_selectmap") as AudioClip;
                    break;
                case "Fight1":
                    ac = Resources.Load("Audio/bgm_battlePVE1") as AudioClip;
                    break;
                case "Fight2":
                    ac = Resources.Load("Audio/bgm_battlePVE2") as AudioClip;
                    break;
                case "Fight3":
                    ac = Resources.Load("Audio/bgm_battlePVE3") as AudioClip;
                    break;
                case "Fight4":
                    ac = Resources.Load("Audio/bgm_battlePVE1") as AudioClip;
                    break;
                case "Car":
                    ac = Resources.Load("Audio/bgm_richangfuben") as AudioClip;
                    break;
                case "Wood":
                    ac = Resources.Load("Audio/bgm_jungle") as AudioClip;
                    break;
                case "PVP":
                    ac = Resources.Load("Audio/bgm_battlePVP") as AudioClip;
                    break;
                case "Challenge":
                    ac = Resources.Load("Audio/bgm_chongtu") as AudioClip;
                    break;
                case "Win":
                    audioSource.loop = false;
                    ac = Resources.Load("Audio/ui_victory") as AudioClip;
                    break;
                case "Lose":
                    audioSource.loop = false;
                    ac = Resources.Load("Audio/ui_defeat") as AudioClip;
                    break;
                default:
                    ac = LastBGM;
                    break;
            }

            if (ac != LastBGM || Type == "")
            {
                LastBGM = ac;
                if (playBGM)
                {
                    audioSource.volume = 0;
                    audioSource.clip = ac;
                    audioSource.playOnAwake = true;
                    audioSource.Play();
                    StartCoroutine(SoundFadeIn());
                }
            }
        }
    }

    IEnumerator SoundFadeIn()
    {
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            for (int i = 0; i < 25; i++)
            {
                yield return new WaitForSeconds(0.1f);
                audioSource.volume += 0.04f;
            }
        }
        audioSource.volume = 1;
    }

    public IEnumerator SoundFadeOut()
    {
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            for (int i = 25; i >= 0; i++)
            {
                yield return new WaitForSeconds(0.1f);
                audioSource.volume -= 0.04f;
            }
        }
        audioSource.volume = 0;
    }

    public void PlayLoop(AudioClip ac)
    {
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            audioSource.Stop();
            audioSource.volume = 1;
            audioSource.loop = true;
            audioSource.clip = ac;
            audioSource.playOnAwake = true;
            audioSource.Play();
        }
    }

    public IEnumerator DelaySound(float DelayTimer, string SoundName)
    {
        yield return new WaitForSeconds(DelayTimer / 100f);
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            if (playSound)
            {
                audioSource.PlayOneShot(Resources.Load("Audio/" + SoundName) as AudioClip);
            }
        }
        yield return 0;
    }

    public void StopPlay()
    {
        audioSource.Stop();
    }

    public void PlayOneShot(string SoundName)
    {
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            if (playSound)
            {
                audioSource.PlayOneShot(Resources.Load("Audio/" + SoundName) as AudioClip);
            }
        }
    }

    public void PlayOneShot(AudioClip ac)
    {
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            if (playSound)
            {
                audioSource.PlayOneShot(ac);
            }
        }
    }

    public IEnumerator PlayCountinueSound(string SoundName, float Sec)
    {
        playContSound = true;

        while (playContSound)
        {
            if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
            {
                if (playSound)
                {
                    audioSource.PlayOneShot(Resources.Load("Audio/" + SoundName) as AudioClip);
                }
            }
            yield return new WaitForSeconds(Sec);
        }
        yield return 0;
    }
}
