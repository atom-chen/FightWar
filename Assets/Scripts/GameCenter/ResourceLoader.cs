using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class ResourceLoader : MonoBehaviour
{
    public static ResourceLoader instance;
    public AssetBundle GameAssetBundle;
    public string NowForwardNPC = "";
    public Texture2D MainScene;
    public Texture2D TowerScene;

    GUISkin GameSkin;
    Texture2D BlackTexture;

    public Font GameFont;

    float DownloadProgress;
    string DownloadMessage = "";
    int RetryCount = 0;
    public bool IsBlackScreen = false;
    public bool IsShow = false;
    public float ShowTimer = 0;
    int LodingTipsIndex = 0;
    int LodingImageIndex = 0;
    bool TipsChange = true;

    public bool NewMail = false;//漂流瓶


    public bool TeamInstance = false;//进入多人副本
    public int TeamInstanceID;//队伍ID
    public int InstanceID;//副本ID

    public bool IsReDownload = false;
    public bool IsDownload = false;
    public bool IsCheckDownload = false;
    public string ReDownloadScene = "";
    public string DownloadEnv = "";

    GUISkin LoadingSkin;

    public List<string> ListDownload = new List<string>();

    public Vector3 StartPosition = Vector3.zero;
    Vector3 EndPosition = Vector3.zero;

    bool IsGuide = false;

    void Awake()
    {
        //GameFont = Resources.Load("Fonts/CN/DroidSansFallback", typeof(Font)) as Font;
    }

    void Start()
    {
        instance = this;
    }


    public IEnumerator GetGameResource(bool IsDownload, string NewScene)
    {
        //GameObject.Find("gameScriptManager").GetComponent<ScriptManager>().guiLoadWindow.SetActive(true);
        //DownloadMessage = GameObject.Find("gameTextTranslator").GetComponent<TextTranslator>().DownloadText;

        ////////////////清空人物(以下)//////////////
        //for (int RoleIndex = transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture.Count - 1; RoleIndex > 0; RoleIndex--)
        //{
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RolePictureObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleShadowObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleNameObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleNameRBlodObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleNameLBlodObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleGroupObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleGroupRBlodObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleGroupLBlodObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleBlackBloodObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleRedBloodObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleTaskObject);
        //    DestroyImmediate(transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[RoleIndex].RoleObject);
        //    transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture.RemoveAt(RoleIndex);
        //    transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().PicRoleList.RemoveAt(RoleIndex);
        //    transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().NowRoleCount--;
        //}
        ////////////////清空人物(以上)//////////////

        //Resources.UnloadUnusedAssets();
        //System.GC.Collect();
        //Debug.LogError(NewScene);
        ////////////////////////预先Load图(以下)//////////////////////
        //if (NewScene.IndexOf("Scene") > -1)
        //{
        //    Resources.Load("Effect/taskfinish");
        //    Resources.Load("Effect/levelup");
        //    Resources.Load("Effect/arrow");
        //    Resources.Load("Effect/road");
        //}
        ////////////////////////预先Load图(以上)//////////////////////
        yield return new WaitForSeconds(0.01f);
        IsReDownload = false;
        ////////////////////如果本机有就用本机(以下)////////////////////    
        if (Resources.Load("CN/" + NewScene + "/" + NewScene) != null) //CheckDownload
        {
            TextAsset LuaText = (TextAsset)Resources.Load("CN/" + NewScene + "/" + NewScene);
            SetLUAText(NewScene, LuaText.ToString());
            //yield return StartCoroutine(FinishDownloadGameResource(NewScene));
        }
        else
        {
            yield return StartCoroutine(DownloadGameResource(IsDownload, NewScene));
        }
        ////////////////////如果本机有就用本机(以上)////////////////////      
    }

    public IEnumerator DownloadGameResource(bool IsNowDownload, string NewScene)
    {
        //        ////////////////////////////////判断下载环境(以下)////////////////////////////////////////
        //        if (NewScene.IndexOf("BGScene") > -1 || NewScene.IndexOf("BGGate") > -1 || NewScene.IndexOf("BGInstance") > -1)
        //        {
        //            if (!IsCheckDownload)
        //            {
        //                if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//运营商网络
        //                {
        //                    //DownloadEnv = GameObject.Find("gameTextTranslator").GetComponent<TextTranslator>().DownloadEnv1Text;
        //#if UNITY_ANDROID
        //                    ReDownloadScene = NewScene;
        //                    if (!IsReDownload && NewScene != "Start")
        //                    {
        //                        IsReDownload = true;
        //                        IsDownload = false;
        //                    }
        //#else
        //                    IsDownload = true;
        //#endif
        //                }
        //                else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)//wifi网络
        //                {
        //                    //DownloadEnv = GameObject.Find("gameTextTranslator").GetComponent<TextTranslator>().DownloadEnv2Text;
        //                    IsDownload = true;

        //                    //ReDownloadScene = NewScene;
        //                    //if (!IsReDownload && NewScene != "Start")
        //                    //{
        //                    //    IsReDownload = true;
        //                    //    IsDownload = false;
        //                    //}
        //                }
        //                else if (Application.internetReachability == NetworkReachability.NotReachable)//无网络
        //                {
        //                    //DownloadEnv = GameObject.Find("gameTextTranslator").GetComponent<TextTranslator>().DownloadEnv3Text;
        //                    ReDownloadScene = NewScene;
        //#if UNITY_ANDROID
        //                    if (!IsReDownload && NewScene != "Start")
        //                    {
        //                        IsReDownload = true;
        //                        IsDownload = false;
        //                    }
        //#else
        //                    IsDownload = true;
        //#endif
        //                }
        //            }
        //        }
        //        else
        //        {
        //            IsDownload = true;
        //        }
        //        ////////////////////////////////判断下载环境(以上)////////////////////////////////////////
        IsDownload = true;
        if (IsDownload)
        {
            string Url = "";
            if (Application.platform == RuntimePlatform.Android)
            {
                Url = ObscuredPrefs.GetString("DownloadUrl", "") + NewScene + ".unity3d";
                //Url = "file://D:\\AssetBundle\\" + transform.parent.GetComponent<GameCenter>().GameLanguage + "\\Web\\" + NewScene + ".unity3d";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Url = ObscuredPrefs.GetString("DownloadUrl", "") + NewScene + ".unity3d";
                //Url = "file://D:\\AssetBundle\\" + transform.parent.GetComponent<GameCenter>().GameLanguage + "\\Web\\" + NewScene + ".unity3d";
            }
            else
            {
                Url = ObscuredPrefs.GetString("DownloadUrl", "") + NewScene + ".unity3d";
                //Url = "file://D:\\AssetBundle\\" + transform.parent.GetComponent<GameCenter>().GameLanguage + "\\Web\\" + NewScene + ".unity3d";
            }
            WWW www;
            ///////////////////本机开发不要用Cache(以下)///////////////////
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                //www = new WWW(Url);
                //Debug.Log(NewScene);
                www = WWW.LoadFromCacheOrDownload(Url, VersionChecker.instance.DictionaryVersion[NewScene]);
            }
            else if (NewScene == "Start")
            {
                www = new WWW(Url);
            }
            else
            {
                //www = new WWW(Url);
                if (IsNowDownload)
                {
                    www = new WWW(Url);
                }
                else
                {
                    www = WWW.LoadFromCacheOrDownload(Url, VersionChecker.instance.DictionaryVersion[NewScene]);
                }
            }
            ///////////////////本机开发不要用Cache(以上)///////////////////
            //Debug.Log(Url);
            while (!www.isDone)
            {
                yield return new WaitForSeconds(0.001f);
                DownloadProgress = www.progress;
            }
            if (www.assetBundle != null)
            {
                RetryCount = 0;
                //if (NewScene.IndexOf("BG") > -1)
                //{
                //    GameAssetBundle = www.assetBundle;
                //}
                //if (NewScene.IndexOf("Monster") > -1 || NewScene.IndexOf("Role") > -1 || NewScene.IndexOf("BG") > -1 || NewScene.IndexOf("Head") > -1)
                //{
                //    if (!transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().DictAssetBundle.ContainsKey(NewScene))
                //    {
                //        Debug.Log("Add DictAssetBundle");
                //        transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().DictAssetBundle.Add(NewScene, www.assetBundle);
                //    }
                //    else
                //    {
                //        Debug.Log("Update DictAssetBundle");
                //        transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().DictAssetBundle[NewScene] = www.assetBundle;
                //    }
                //}
                AssetBundle ab = www.assetBundle;
                //Debug.Log(NewScene);
                TextAsset LuaText = (TextAsset)www.assetBundle.Load(NewScene);
                if (LuaText != null)
                {
                    //Debug.Log(NewScene + " " + LuaText.ToString());
                    SetLUAText(NewScene, LuaText.ToString());
                    www.assetBundle.Unload(true);
                }
                else
                {
                    SetLUAText(NewScene, "");
                }
                yield return StartCoroutine(FinishDownloadGameResource(NewScene));
            }
            else
            {
                RetryCount++;
                if (RetryCount > 3)
                {
                    if (www.error != null)
                    {
                        if (Application.internetReachability == NetworkReachability.NotReachable)//无网络
                        {
                            IsReDownload = true;
                        }
                        if (!ListDownload.Contains(NewScene))
                        {
                            Debug.Log("ListDownload Add:" + NewScene);
                            ListDownload.Add(NewScene);
                        }
                    }
                    yield return www;
                }
                else
                {
                    IsReDownload = false;
                    yield return StartCoroutine(DownloadGameResource(false, NewScene));
                }
            }
            www.Dispose();
            www = null;
        }
        else
        {
            if (!ListDownload.Contains(NewScene))
            {
                Debug.Log("ListDownload Add:" + NewScene);
                ListDownload.Add(NewScene);
            }
        }
    }

    IEnumerator FinishDownloadGameResource(string NewScene)
    {
        yield return new WaitForSeconds(0.1f);
        //if (NewScene.IndexOf("Role") > -1 && NewScene.Length > 4)
        //{
        //    transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ShowRole(NewScene);
        //}
        //else if (NewScene.IndexOf("Monster") > -1)
        //{
        //    transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ShowMonster(NewScene);
        //}
        DownloadProgress = 0;
    }

    void SetLUAText(string NewScene, string LuaText)
    {

        if (NewScene == "Start")
        {
            //GameObject.Find("gameXMLParser").GetComponent<XMLParser>().ParseXMLStartScript(LuaText);
        }
        else if (NewScene == "Initial")
        {

        }
        else if (NewScene == "Server")
        {
            //GameObject.Find("gameXMLParser").GetComponent<XMLParser>().ParseXMLServerScript(LuaText);
        }
        else if (NewScene == "EquipRefine")
        {
            XMLParser.instance.ParseXMLEquipRefineScript(LuaText);
        }
        else if (NewScene == "Task")
        {
            //GameObject.Find("gameXMLParser").GetComponent<XMLParser>().ParseXMLTaskScript(LuaText);
        }
        else if (NewScene == "MissionVice")
        {
            //GameObject.Find("gameXMLParser").GetComponent<XMLParser>().ParseXMLMissionViceScript(LuaText);
        }
        else if (NewScene == "Role")
        {
            XMLParser.instance.ParseXMLRoleScript(LuaText);
        }
        else if (NewScene == "Enemy")
        {
            XMLParser.instance.ParseXMLEnemyScript(LuaText);
        }
        else if (NewScene == "BossAi")
        {
            XMLParser.instance.ParseXMLBossAiScript(LuaText);
        }
        else if (NewScene == "Terrain")
        {
            XMLParser.instance.ParseXMLTerrainScript(LuaText);
        }
        else if (NewScene == "Skill")
        {
            XMLParser.instance.ParseXMLSkillScript(LuaText);
        }
        else if (NewScene == "ServerList") //yy
        {
            XMLParser.instance.ParseXMLServerListsScript(LuaText);
        }
        else if (NewScene == "DownloadList") //yy
        {
            XMLParser.instance.ParseXMLDownloadListsScript(LuaText);
        }
        else if (NewScene == "Vip")
        {
            XMLParser.instance.ParseXMLVipScript(LuaText);
        }
        else if (NewScene == "Gate")
        {
            XMLParser.instance.ParseXMLGateScript(LuaText);
        }
        else if (NewScene == "Achievement")
        {
            XMLParser.instance.ParseXMLAchievementScript(LuaText);
        }
        else if (NewScene == "HappyBox") //yy
        {
            XMLParser.instance.ParseXMLHappyBoxScript(LuaText);
        }
        else if (NewScene == "WorldBoss") //yy
        {
            XMLParser.instance.ParseXMLWorldBossScript(LuaText);
        }
        else if (NewScene == "WorldBossReward") //yy
        {
            XMLParser.instance.ParseXMLWorldBossRewardScript(LuaText);
        }
        else if (NewScene == "KingRoad") //yy
        {
            XMLParser.instance.ParseXMLKingRoadRewardScript(LuaText);
        }
        else if (NewScene == "LegionCraps") //yy
        {
            XMLParser.instance.ParseXMLLegionCrapScript(LuaText);
        }
        else if (NewScene == "LegionCity") //yy
        {
            XMLParser.instance.ParseXMLLegionCityScript(LuaText);
        }
        else if (NewScene == "ControlGateOpen") //yy
        {
            XMLParser.instance.ParseXMLControlGateOpenScript(LuaText);
        }
        else if (NewScene == "LegionRedBags") //yy
        {
            XMLParser.instance.ParseXMLLegionRedBagScript(LuaText);
        }
        else if (NewScene == "TeamGate") //yy
        {
            XMLParser.instance.ParseXMLTeamGateScript(LuaText);
        }
        else if (NewScene == "GachaPreview") //yy
        {
            XMLParser.instance.ParseXMLGachaPreviewScript(LuaText);
        }
        else if (NewScene == "Nation") //yy
        {
            XMLParser.instance.ParseXMLNationScript(LuaText);
        }
        else if (NewScene == "BattlefieldPoints") //yy
        {
            XMLParser.instance.ParseXMLBattlefieldPointsScript(LuaText);
        }
        else if (NewScene == "BattlefieldKill") //yy
        {
            XMLParser.instance.ParseXMLBattlefieldKillScript(LuaText);
        }
        else if (NewScene == "ArmsDealers") //yy
        {
            XMLParser.instance.ParseXMLArmsDealersScript(LuaText);
        }
        else if (NewScene == "SmallGoal") //yy
        {
            XMLParser.instance.ParseXMLSmallGoalScript(LuaText);
        }
        else if (NewScene == "ResourceTycoon") //yy
        {
            XMLParser.instance.ParseXMLResourceTycoonScript(LuaText);
        }
        else if (NewScene == "Sign")
        {
            XMLParser.instance.ParseXMLSignScript(LuaText);
        }
        else if (NewScene == "InnateExchange")
        {
            XMLParser.instance.ParseXMLInnateExchangesScript(LuaText);
        }
        else if (NewScene == "SignExtra")
        {
            XMLParser.instance.ParseXMLSignExtraScript(LuaText);
        }
        else if (NewScene == "ActivitySevenLogin")
        {
            XMLParser.instance.ParseXMLActivitySevenLoginScript(LuaText);
        }
        else if (NewScene == "LegionTrain")
        {
            XMLParser.instance.ParseXMLLegionTrainScript(LuaText);
        }
        else if (NewScene == "Legion")
        {
            XMLParser.instance.ParseXMLLegionScript(LuaText);
        }
        else if (NewScene == "LegionTask")
        {
            XMLParser.instance.ParseXMLLegionTaskScript(LuaText);
        }
        else if (NewScene == "LegionRank")
        {
            XMLParser.instance.ParseXMLLegionRankScript(LuaText);
        }
        else if (NewScene == "LegionFirstPass")
        {
            XMLParser.instance.ParseXMLLegionFirstPassScript(LuaText);
        }
        else if (NewScene == "LegionGate")
        {
            XMLParser.instance.ParseXMLLegionGateScript(LuaText);
        }
        else if (NewScene == "LegionGateBox")
        {
            XMLParser.instance.ParseXMLLegionGateBoxScript(LuaText);
        }
        else if (NewScene == "LabsLimit")
        {
            XMLParser.instance.ParseXMLLabsLimitScript(LuaText);
        }
        else if (NewScene == "LabsPoint")
        {
            XMLParser.instance.ParseXMLLabsPointScript(LuaText);
        }
        else if (NewScene == "Question")
        {
            XMLParser.instance.ParseXMLQuestionScript(LuaText);
        }
        else if (NewScene == "ActivitySevenDay")
        {
            XMLParser.instance.ParseXMLActivitySevenDayScript(LuaText);
        }
        else if (NewScene == "ActivitySevenRank")
        {
            XMLParser.instance.ParseXMLActivitySevenRankScript(LuaText);
        }
        else if (NewScene == "ActivitySevenHero")
        {
            XMLParser.instance.ParseXMLActivitySevenHeroScript(LuaText);
        }
        else if (NewScene == "ActivityHalfLimitBuy")
        {
            XMLParser.instance.ParseXMLActivityHalfLimitBuyScript(LuaText);
        }
        else if (NewScene == "Item")
        {
            XMLParser.instance.ParseXMLItemScript(LuaText);
        }
        else if (NewScene == "ItemSort")
        {
            XMLParser.instance.ParseXMLItemSortScript(LuaText);
        }
        else if (NewScene == "NewGuide")
        {
            XMLParser.instance.ParseXMLNewGuideScript(LuaText);
        }
        else if (NewScene == "ManualSkill")
        {
            XMLParser.instance.ParseXMLManualSkillScript(LuaText);
        }
        else if (NewScene == "Buff")
        {
            XMLParser.instance.ParseXMLBuffScript(LuaText);
        }
        else if (NewScene == "NpcTactics")
        {
            XMLParser.instance.ParseXMLNPCTacticsScript(LuaText);
        }
        else if (NewScene == "FightMotion")
        {
            XMLParser.instance.ParseXMLFightMotionScript(LuaText);
        }
        else if (NewScene == "FightEffect")
        {
            XMLParser.instance.ParseXMLFightEffectScript(LuaText);
        }
        else if (NewScene == "FightProjectile")
        {
            XMLParser.instance.ParseXMLFightProjectileScript(LuaText);
        }
        else if (NewScene == "FightCamera")
        {
            XMLParser.instance.ParseXMLFightCameraScript(LuaText);
        }
        else if (NewScene == "BattleMap")
        {
            XMLParser.instance.ParseXMLBattleMapScript(LuaText);
        }
        else if (NewScene == "Chapter")
        {
            XMLParser.instance.ParseXMLChapterScript(LuaText);
        }
        else if (NewScene == "GateCompleteBox")
        {
            XMLParser.instance.ParseXMLGateCompleteBoxScript(LuaText);
        }
        else if (NewScene == "GateRankBox")
        {
            XMLParser.instance.ParseXMLGateRankBoxScript(LuaText);
        }
        else if (NewScene == "GateLimit")
        {
            XMLParser.instance.ParseXMLGateLimitScript(LuaText);
        }
        else if (NewScene == "RoleRankNeed")
        {
            XMLParser.instance.ParseXMLRoleRankNeedScript(LuaText);
        }
        else if (NewScene == "Career")
        {
            XMLParser.instance.ParseXMLCareerScript(LuaText);
        }
        else if (NewScene == "RoleClassUp")
        {
            XMLParser.instance.ParseXMLRoleClassUpScript(LuaText);
        }
        else if (NewScene == "EquipClassUp")
        {
            XMLParser.instance.ParseXMLEquipClassUpScript(LuaText);
        }
        else if (NewScene == "EquipStrong")
        {
            XMLParser.instance.ParseXMLEquipStrongScript(LuaText);
        }
        else if (NewScene == "StrengthenMaster")
        {
            XMLParser.instance.ParseXMLStrengthenMasterScript(LuaText);
        }
        else if (NewScene == "RoleTalent")
        {
            XMLParser.instance.ParseXMLRoleTalentScript(LuaText);
        }
        else if (NewScene == "EquipExp")
        {
            XMLParser.instance.ParseXMLEquipExpScript(LuaText);
        }
        else if (NewScene == "EquipStrongQuality")
        {
            XMLParser.instance.ParseXMLEquipStrongQualityScript(LuaText);
        }
        else if (NewScene == "EquipRefineCost")
        {
            XMLParser.instance.ParseXMLEquipRefineCostScript(LuaText);
        }
        else if (NewScene == "Resource")
        {
            XMLParser.instance.ParseXMLResourceScript(LuaText);
        }
        else if (NewScene == "RoleWash")
        {
            XMLParser.instance.ParseXMLRoleWashScript(LuaText);
        }
        else if (NewScene == "RoleFate")
        {
            XMLParser.instance.ParseXMLRoleFateScript(LuaText);
        }
        else if (NewScene == "RoleBreach")
        {
            XMLParser.instance.ParseXMLRoleBreachScript(LuaText);
        }
        else if (NewScene == "WorldEvent")
        {
            XMLParser.instance.ParseXMLWorldEventScript(LuaText);
        }
        else if (NewScene == "Map")
        {
            XMLParser.instance.ParseXMLMapScript(LuaText);
        }
        else if (NewScene == "RoleDestiny")
        {
            XMLParser.instance.ParseXMLRoleDestinyScript(LuaText);
        }
        else if (NewScene == "RoleDestinyCost")
        {
            XMLParser.instance.ParseXMLRoleDestinyCostScript(LuaText);
        }
        else if (NewScene == "EquipStrongCost")
        {
            XMLParser.instance.ParseXMLEquipStrongCostScript(LuaText);
        }
        else if (NewScene == "RareTreasureOpen")
        {
            XMLParser.instance.ParseXMLRareTreasureOpenScript(LuaText);
        }
        else if (NewScene == "RareTreasureAttr")
        {
            XMLParser.instance.ParseXMLRareTreasureAttrScript(LuaText);
        }
        else if (NewScene == "RareTreasureExp")
        {
            XMLParser.instance.ParseXMLRareTreasureExpScript(LuaText);
        }
        else if (NewScene == "Innate")
        {
            XMLParser.instance.ParseXMLInnatesScript(LuaText);
        }
        else if (NewScene == "EverydayActivity")
        {
            XMLParser.instance.ParseXMLEverydayActivityScript(LuaText);
        }
        else if (NewScene == "EverydayActivityAward")
        {
            XMLParser.instance.ParseXMLEveryActivityRewardScript(LuaText);
        }
        else if (NewScene == "Talk")
        {
            XMLParser.instance.ParseXMLTalkScript(LuaText);
        }
        else if (NewScene == "TechTree")
        {
            XMLParser.instance.ParseXMLTechTreeScript(LuaText);
        }
        else if (NewScene == "Tower")
        {
            XMLParser.instance.ParseXMLTowerScript(LuaText);
        }
        else if (NewScene == "TowerBoxCost")
        {
            XMLParser.instance.ParseXMLTowerBoxCostScript(LuaText);
        }
        else if (NewScene == "Market")
        {
            XMLParser.instance.ParseXMLMarketScript(LuaText);
        }
        else if (NewScene == "PvpReward")
        {
            XMLParser.instance.ParseXMLPVPRewardScript(LuaText);
        }
        else if (NewScene == "PvpPointShop")
        {
            XMLParser.instance.ParseXMLPvpPointShopScript(LuaText);
        }
        else if (NewScene == "PvpOnceReward")
        {
            XMLParser.instance.ParseXMLPvpOnceRewardScript(LuaText);
        }
        else if (NewScene == "SmuggleCar")
        {
            XMLParser.instance.ParseXMLSmuggleCarScript(LuaText);
        }
        else if (NewScene == "SuperCar")
        {
            XMLParser.instance.ParseXMLSuperCarScript(LuaText);
        }
        else if (NewScene == "WeaponUpStar")
        {
            XMLParser.instance.ParseXMLWeaponUpStarScript(LuaText);
        }
        else if (NewScene == "WeaponUpClass")
        {
            XMLParser.instance.ParseXMLWeaponUpClassScript(LuaText);
        }
        else if (NewScene == "WeaponMaterial")
        {
            XMLParser.instance.ParseXMLWeaponMaterialScript(LuaText);
        }
        else if (NewScene == "WeaponWheel")
        {
            XMLParser.instance.ParseXMLWeaponWheelScript(LuaText);
        }
        else if (NewScene == "FightTalk")
        {
            XMLParser.instance.ParseXMLFightTalkScript(LuaText);
        }
        else if (NewScene == "TowerRankReward")
        {
            XMLParser.instance.ParseXMLTowerRankRewardScript(LuaText);
        }
        else if (NewScene == "TowerShop")
        {
            XMLParser.instance.ParseXMLTowerShopScript(LuaText);
        }
        else if (NewScene == "Barrage")
        {
            XMLParser.instance.ParseXMLBarrageEventScript(LuaText);
        }
        else if (NewScene == "Fortress")
        {
            XMLParser.instance.ParseXMLFortressEventScript(LuaText);
        }
        else if (NewScene == "CombinSkill")
        {
            XMLParser.instance.ParseXMLCombinSkillScript(LuaText);
        }
        else if (NewScene == "ActivitiesCenter")
        {
            XMLParser.instance.ParseXMLActivitiesCenterScript(LuaText);
        }
        else if (NewScene == "ActivityDayExchange")
        {
            XMLParser.instance.ParseXMLActivityDayExchangeScript(LuaText);
        }
        else if (NewScene == "ActivityExchange")
        {
            XMLParser.instance.ParseXMLActivityExchangeScript(LuaText);
        }
        else if (NewScene == "ActivityGrowthFund")
        {
            XMLParser.instance.ParseXMLActivityGrowthFundScript(LuaText);
        }
        else if (NewScene == "ActivityBonusVip")
        {
            XMLParser.instance.ParseXMLActivityBonusVipScript(LuaText);
        }
        else if (NewScene == "ActivityItemFixed")
        {
            XMLParser.instance.ParseXMLActivityItemFixedScript(LuaText);
        }
        else if (NewScene == "ActivityGachaHeroRank")
        {
            XMLParser.instance.ParseXMLActivityGachaHeroRankScript(LuaText);
        }
        else if (NewScene == "ActivityGachaHeroPoint")
        {
            XMLParser.instance.ParseXMLActivityGachaHeroPointScript(LuaText);
        }
        else if (NewScene == "ActionEvent")
        {
            XMLParser.instance.ParseXMLActionEventScript(LuaText);
        }
        else if (NewScene == "ShopCenter")
        {
            XMLParser.instance.ParseXMLShopCenterScript(LuaText);
        }
        else if (NewScene == "ShopCenterPeculiar")
        {
            XMLParser.instance.ParseXMLShopCenterPeculiarScript(LuaText);
        }
        else if (NewScene == "Exp")
        {
            XMLParser.instance.ParseXMLExpScript(LuaText);
        }
        else if (NewScene == "Wish")
        {
            XMLParser.instance.ParseXMLWishScript(LuaText);
        }
        else if (NewScene == "LegionWeak")
        {
            XMLParser.instance.ParseXMLLegionWeakScript(LuaText);
        }
        else if (NewScene=="RoleGrow")
        {
            XMLParser.instance.ParseXMLRoleGrowScript(LuaText);
        }
        else if (NewScene == "IndianaPoint")
        {
            XMLParser.instance.ParseXMLIndianaPointScript(LuaText);
        }
        else if (NewScene == "Exchange")
        {
            XMLParser.instance.ParseXMLExchargeScript(LuaText);
        }
        else if (NewScene == "Chip")
        {
            XMLParser.instance.ParseXMLChipScript(LuaText);
        }
        else if (NewScene== "NuclearPowerPlan")
        {
            XMLParser.instance.ParseXMLNuclearPowerPlanScript(LuaText);
        }
        
    }



    /// <summary>
    /// 统一的载入资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static UnityEngine.Object Load(string path)
    {
        return Resources.Load(path);
    }

    public void AddGuideClick(GameObject go, float delayTime)
    {
        //StopCoroutine(SetGuideClick(go, delayTime));
        StopAllCoroutines();
        StartCoroutine(SetGuideClick(go, delayTime));
    }

    IEnumerator SetGuideClick(GameObject go, float delayTime)
    {
        SceneTransformer.instance.isClickSkip = true;
        //Debug.LogError("ssssssssssssssss1");
        if (go!=null&&go.name != "40101")
        {
            if (GameObject.Find("MaskWindow") == null)
            {
                UIManager.instance.OpenSinglePanel("MaskWindow", false);
            }
            yield return new WaitForSeconds(0.2f);
            DestroyImmediate(GameObject.Find("MaskWindow"));
        }
        //Debug.LogError(go + "  " + GameObject.Find("GuideButtonPoint"));
        if (GameObject.Find("GuideButtonPoint") != null && go != null)
        {
            SetGuideArrow(go, 0);
        }

    }
    //新手引导finddelay
    public void SetGuideDelay(float delayTime)
    {

        StartCoroutine(GuideDelay(delayTime));
    }

    IEnumerator GuideDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        LuaDeliver.instance.UseGuideStation();
    }
    //新手引导C#delay
    public void SetGuideDelayCsharp(float delayTime)
    {

        StartCoroutine(GuideDelayCsharp(delayTime));
    }

    IEnumerator GuideDelayCsharp(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }
    //

    public void SetGuideArrow(GameObject go, float EndR)
    {
        //StopCoroutine(SetArrow(EndPV3, EndR));        
        StopAllCoroutines();
        StartCoroutine(SetArrow(go, EndR));
    }

    IEnumerator SetArrow(GameObject go, float EndR)
    {
        SceneTransformer.instance.isClickSkip = true;
        Debug.LogError("ssssssssssssssss2");
        while (GameObject.Find("GuideArrow") == null)
        {
            yield return new WaitForSeconds(0.01f);
        }

        GameObject Arrow = GameObject.Find("GuideArrow");
        GameObject GuideButtonPoint = GameObject.Find("GuideButtonPoint");

        while (true)
        {
            if (Arrow != null && GuideButtonPoint != null)
            {
                 if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 8)
                 {
                     break;
                 }
                 if (go != null)
                 {
                     GuideButtonPoint.transform.position = go.transform.position;
                     Arrow.transform.localPosition = new Vector3(GuideButtonPoint.transform.localPosition.x + 20, GuideButtonPoint.transform.localPosition.y - 20, 0);
                     Arrow.transform.eulerAngles = new Vector3(0, 0, EndR);
                 }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetGuideArrow(Vector3 EndPV3, float EndR)
    {
        //StopCoroutine(SetArrow(EndPV3, EndR));        
        StopAllCoroutines();
        StartCoroutine(SetArrow(EndPV3, EndR));
    }

    IEnumerator SetArrow(Vector3 EndPV3, float EndR)
    {
        //yield return new WaitForSeconds(0.5f);
        SceneTransformer.instance.isClickSkip = true;
        Debug.LogError("ssssssssssssssss3");
        while (GameObject.Find("GuideArrow") == null)
        {
            yield return new WaitForSeconds(0.01f);
        }

        GameObject Arrow = GameObject.Find("GuideArrow");
        GameObject GuideButtonPoint = GameObject.Find("GuideButtonPoint");

        while (true)
        {
            if (Arrow != null)
            {
                Arrow.transform.localPosition = new Vector3(EndPV3.x+ 20, EndPV3.y - 20, 0);
                Arrow.transform.eulerAngles = new Vector3(0, 0, EndR);
            }
            yield return new WaitForSeconds(0.1f);
        }

        //GameObject go = GameObject.Find("GuideArrow");
        //UISpriteAnimation SA = go.transform.Find("Arrow").gameObject.GetComponent<UISpriteAnimation>();
        //if (SA.isActiveAndEnabled)
        //{
        //    SA.ResetToBeginning();
        //    SA.enabled = false;
        //}

        //for (float i = 0; i < 0.4f; i += 0.01f)
        //{
        //    if (go != null)
        //    {
        //        go.transform.localPosition = Vector3.Lerp(go.transform.localPosition, EndPV3, i);
        //        go.transform.eulerAngles = Vector3.Lerp(go.transform.eulerAngles, new Vector3(0, 0, EndR), i);
        //    }
        //    //GameObject.Find("GuideArrow").transform.Rotate(new Vector3(0, 0, Mathf.Lerp(GameObject.Find("GuideArrow").transform.eulerAngles.z, EndR, i)));

        //    yield return new WaitForSeconds(0.01f);
        //}
        //if (go != null)
        //{
        //    SA.enabled = true;
        //}
    }

    public void MoveGuideArrow(Vector3 StartPV3, Vector3 EndPV3)
    {
        StopCoroutine(MoveArrow(StartPV3, EndPV3));
        StartCoroutine(MoveArrow(StartPV3, EndPV3));
    }

    IEnumerator MoveArrow(Vector3 StartPV3, Vector3 EndPV3)
    {
        SceneTransformer.instance.isClickSkip = true;
        Debug.LogError("ssssssssssssssss4");
        EndPV3 = new Vector3(EndPV3.x, EndPV3.y - 20, 0);
        StartPV3 = new Vector3(StartPV3.x, StartPV3.y - 20, 0);
        //yield return new WaitForSeconds(0.5f);
        while (GameObject.Find("GuideArrow") == null)
        {
            yield return new WaitForSeconds(0.01f);
        }

        if (GameObject.Find("GuideButtonPoint") != null)
        {
            GameObject.Find("GuideButtonPoint").transform.localPosition = StartPV3 - new Vector3(0, -20, 0);
            //GameObject.Find("GuideButtonPoint").transform.localPosition = new Vector3(10000, 0, 0); ;
        }

        GameObject go = GameObject.Find("GuideArrow");
        UISpriteAnimation SA = go.transform.Find("Arrow").gameObject.GetComponent<UISpriteAnimation>();
        if (SA.isActiveAndEnabled)
        {
            SA.ResetToBeginning();
            SA.enabled = false;
        }

        while (true)
        {
            for (float i = 0; i < 1f; i += 0.02f)
            {
                if (go != null)
                {
                    go.transform.localPosition = Vector3.Lerp(StartPV3, EndPV3, i);
                }
                //GameObject.Find("GuideArrow").transform.Rotate(new Vector3(0, 0, Mathf.Lerp(GameObject.Find("GuideArrow").transform.eulerAngles.z, EndR, i)));

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public void SetGuideButtonLocalPosition(Vector3 v3)
    {
        GameObject.Find("GuideButtonPoint").transform.localPosition = v3;
    }
    //lua时间刷新
    public void UpdateShowTime()
    {
        CancelInvoke("UpdateTime");
        Invoke("UpdateTime", 1);
    }
    void UpdateTime()
    {
        LuaDeliver.instance.func1.Call();
    }
    //打开跳转活动界面
    public void OpenActivity(int ID)
    {
        CharacterRecorder.instance.OpenWindowButtonID = ID;
        NetworkHandler.LuaSendProcess("9601#;");
        LuaDeliver.OpenSinglePanel("EventWindow", false);
        LuaDeliver.SetLocalPosition(LuaDeliver.GameFindObj("EventWindow"), new Vector3(0, 60, 0));
        StartCoroutine(DelayOpenWindow());
    }
    IEnumerator DelayOpenWindow()
    {
        yield return new WaitForSeconds(0.1f);
        LuaDeliver.instance.OpenWindow.Call();
    }
}

