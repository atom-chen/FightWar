using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
using CodeStage.AntiCheat.ObscuredTypes;

public class VersionChecker : MonoBehaviour
{
    public static VersionChecker instance;
    public Dictionary<string, int> DictionaryVersion = new Dictionary<string, int>();
    public static Dictionary<string, AssetBundle> DictionaryAssetBundle = new Dictionary<string, AssetBundle>();
    public int Count = 0;
    public int TotalCount = 0;
    public bool IsLogin = true;
    public bool IsSDKLogin = false;
    List<string> ListUrl = new List<string>();

    void Start()
    {
        instance = this;
        Debug.Log("Version Check Start");
        StartCoroutine(GetVersion());
    }

    IEnumerator GetVersion()
    {
        int CodeVersion = ObscuredPrefs.GetInt("CodeVersion", 1);
        string DownloadUrl = ObscuredPrefs.GetString("DownloadUrl", "");
        string XMLText = "";
        Debug.Log(DownloadUrl);

        Debug.Log(CodeVersion);
        if (Resources.Load("CN/Start/Start") != null && CodeVersion == 1)
        {
            TextAsset TextScript = Resources.Load("CN/Start/Start") as TextAsset;
            XMLText = TextScript.ToString();
        }
        else
        {
            string ScriptUrl = DownloadUrl + "Start.unity3d";

            CodeVersion = UnityEngine.Random.Range(10, 9999);
            Debug.Log(ScriptUrl + " " + CodeVersion);

            //WWW www = WWW.LoadFromCacheOrDownload(ScriptUrl, CodeVersion);
            WWW www = new WWW(ScriptUrl);
            while (!www.isDone)
            {
                yield return new WaitForSeconds(0.001f);
            }

            if (www.assetBundle != null)
            {
                TextAsset TextScript = www.assetBundle.Load("Start") as TextAsset;
                XMLText = TextScript.ToString();
                www.assetBundle.Unload(false);
            }
        }
        CheckDownload(XMLText);
    }

    void CheckDownload(string XMLText)
    {
        SecurityParser SP = new SecurityParser();
        SP.LoadXml(XMLText);
        Debug.Log(XMLText);
        System.Security.SecurityElement SE = SP.ToXml();
        if (SE.SearchForChildByTag("Vers").Children != null)
        {
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Vers").Children)
            {
                Count++;
                TotalCount++;
            }
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Vers").Children)
            {
                Hashtable myVerTable = child.Attributes;
                DictionaryVersion.Add(myVerTable["N"].ToString(), int.Parse(myVerTable["V"].ToString()));
                ListUrl.Add(myVerTable["N"].ToString());
            }


            StartCoroutine(DownloadVersion(ListUrl[0]));
        }
        Debug.Log("CheckStartFinish");
    }

    IEnumerator DownloadVersion(string Name)
    {
        //Debug.Log(Name + " " + Count);
        int NewVersion = DictionaryVersion[Name];
        string DownloadUrl = ObscuredPrefs.GetString("DownloadUrl", "");
        string ScriptUrl = DownloadUrl + Name + ".unity3d";
        bool IsDownload = true;
        if (Name == "Start")
        {
            if (PlayerPrefs.GetInt("StartVer", 0) == 0)
            {
                PlayerPrefs.SetInt("StartVer", NewVersion);
            }
            else if (PlayerPrefs.GetInt("StartVer", 0) != NewVersion)
            {
                PlayerPrefs.SetInt("StartVer", NewVersion);
            }
            else
            {
                IsDownload = false;
                TotalCount = 1;
                Count = 1;
            }
        }
        else if (Name == "Version")
        {
            if (CharacterRecorder.instance.GameVersion < NewVersion)
            {
                IsDownload = false;
                TotalCount = 1;
                Count = 0;
                StartCoroutine(ResourceLoader.instance.GetGameResource(false, "DownloadList"));
            }
        }
        Debug.Log(IsDownload + " " + Count);
        if (IsDownload && Count > 0)
        {
            //Debug.Log(ScriptUrl + " " + DictionaryVersion[Name]);
            WWW www = WWW.LoadFromCacheOrDownload(ScriptUrl, NewVersion);
            GameObject.Find("LabelDownloadContent").GetComponent<UILabel>().text = string.Format("Downloading...({0}/{1})", TotalCount - Count + 1, TotalCount);
            yield return www;
            //while (!www.isDone)
            //{
            //    GameObject.Find("ProgressBar").GetComponent<UISlider>().value = www.progress;
            //    yield return new WaitForEndOfFrame();
            //}
            GameObject.Find("ProgressBar").GetComponent<UISlider>().value = ((TotalCount - Count + 1f) / TotalCount);
            www.Dispose();
        }
        Debug.Log("DownloadFinish" + Count);
        Count--;
        if (Count == 0)
        {
            Debug.Log("CheckAllFinish");
            LuaDeliver.instance.StartLua();
            if (PlayerPrefs.GetInt("FirstInstall", 0) == 0 && false)
            {
                PlayerPrefs.SetInt("FirstInstall", 1);
                PlayerPrefs.SetString("ServerID", "0");
                PlayerPrefs.SetInt("UserID", 0);

                PlayerPrefs.SetInt("GuideState_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 0);
                PlayerPrefs.SetInt("GuideSubState_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 0);
                NetworkHandler.instance.IsCreate = true;
                UIManager.instance.OpenPanel("LoadingWindow", false);
                GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().IsAuto = true;
                GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().ShowHero(0, true);
                if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0)
                {
                    UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_201);
                }
                //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
                StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Item"));
                //TextAsset LuaText = (TextAsset)Resources.Load("CN/Item/Item");
                //XMLParser.instance.ParseXMLItemScript(LuaText.text);
                //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
            }
            else
            {
                StartLogin();
            }
            yield return 0;
        }
        else if (Count < 0)
        {
#if UNITY_ANDROID
            UIManager.instance.OpenPromptWindow("亲，当前版本不是最新\n\n点击确定开始下载\n(请在wifi环境下载)", PromptWindow.PromptType.Confirm, DownloadGame, QuitGame);
#else
            UIManager.instance.OpenPromptWindow("亲，当前版本不是最新\n\n请先下载最新版本进行游戏", QuitGame, QuitGame);
#endif
        }
        else
        {
            //Debug.Log(ScriptUrl + " " + DictionaryVersion[Name] + " " + TotalCount + " " + Count + " " + ListUrl.Count);
            StartCoroutine(DownloadVersion(ListUrl[TotalCount - Count]));
        }
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void DownloadGame()
    {
        try
        {
            string Platform = "";
#if XIAOMI
            Platform = "XIAOMI";
#elif OPPO
            Platform = "OPPO";
#elif UC
            Platform = "UC";
#elif QIHOO360
            Platform = "QIHOO360";
#elif QQ
            Platform = "QQ";
#elif BAIDU
            Platform = "BAIDU";
#elif HUAWEI
            Platform = "HUAWEI";
#elif HOLA
            Platform = "HOLA";
#elif MEIZU
            Platform = "MEIZU";
#elif JINLI
            Platform = ObscuredPrefs.GetString("Account").Split('_')[0];
#endif
            if (Platform != "")
            {
                string Url = TextTranslator.instance.GetServerListsByPlatform(Platform);
                GameObject.Find("MainCamera").GetComponent<HuaWeiGameCenter>().DownloadApk(Url);
            }
        }
        catch (Exception e)
        {

        }
    }

    public void StartLogin()
    {
        Debug.Log("StartLogin");
        StartCoroutine(DelayStartLogin());
    }

    IEnumerator DelayStartLogin()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ServerList"));
        yield return new WaitForSeconds(1f);
        if ((GameObject.Find("LoginWindow") == null && GameObject.Find("AccountWindow") == null) && !GameObject.Find("MainCamera").GetComponent<HuaWeiGameCenter>().IsInit && PlayerPrefs.GetInt("Relogin") == 0)
        {
            Debug.LogError("AccountWindow");
#if UNITY_EDITOR || UNITY_IOSOFFCIAL
            GameObject.Find("MainCamera").GetComponent<HuaWeiGameCenter>().IsInit = true;
            UIManager.instance.OpenPanel("AccountWindow", true);
#elif UNITY_ANDROID || KY

#if MY_WAR_DEBUG            
            UIManager.instance.OpenPanel("AccountWindow", true);
#else
            GameObject.Find("MainCamera").GetComponent<HuaWeiGameCenter>().StartSDK();
#endif
            //UIManager.instance.OpenPanel("AccountWindow");
            //UIManager.instance.ClosePanel("LoadingWindow");
#endif

        }
        yield return new WaitForSeconds(3f);
#if KY
        GameObject.Find("MainCamera").GetComponent<HuaWeiGameCenter>().IsInit = true;
#endif
        if ((GameObject.Find("LoginWindow") == null && GameObject.Find("AccountWindow") == null && GameObject.Find("LoadingWindow") != null))
        {
            //if(GameObject.Find("MainCamera").GetComponent<HuaWeiGameCenter>().IsInit)
            {
                UIManager.instance.OpenPanel("LoginWindow", true);
            }
        }

    }

    public void UpdateGUIAtlas(string GUIName, string AtlasName)
    {
        if (GameObject.Find(GUIName) != null)
        {
            UpdateAtlas(GameObject.Find(GUIName), AtlasName);
        }
    }

    public static void UpdateAtlas(GameObject go, string AtlasName)
    {
        UIAtlas atlas = (UIAtlas)Resources.Load("Atlas/" + AtlasName, typeof(UIAtlas));
        go.GetComponent<UISprite>().atlas = atlas;
        //UIAtlas atlas = null;
        //if (!DictionaryAssetBundle.ContainsKey(AtlasName))
        //{
        //    string url = ObscuredPrefs.GetString("DownloadUrl", "") + "/" + AtlasName + ".unity3d";
        //    WWW www = WWW.LoadFromCacheOrDownload(url, VersionChecker.instance.DictionaryVersion[AtlasName]);
        //    atlas = (UIAtlas)www.assetBundle.Load(AtlasName, typeof(UIAtlas));
        //    DictionaryAssetBundle.Add(AtlasName, www.assetBundle);
        //    www.Dispose();
        //}
        //else
        //{
        //    atlas = (UIAtlas)DictionaryAssetBundle[AtlasName].Load(AtlasName, typeof(UIAtlas));
        //}


        //UISprite sprite = NGUITools.AddSprite(go, atlas, "x_ui_close_1");
        //sprite.transform.localScale = new Vector3(1, 1, 1);
        //sprite.transform.localPosition = new Vector3(0, 0, 0);
        //sprite.MakePixelPerfect();

        //foreach (var c in go.transform.GetComponentsInChildren(typeof(UISprite), true))
        //{
        //    Debug.Log(((UISprite)c).atlas.name);
        //    if (((UISprite)c).atlas.name == AtlasName)
        //    {
        //        ((UISprite)c).atlas = atlas;
        //    }       
        //}
    }
}
