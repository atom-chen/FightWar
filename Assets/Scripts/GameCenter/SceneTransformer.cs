using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Gate
{
    public string GateName;
    public string GateID;
    public int TaskID;
}

public class SceneTransformer : MonoBehaviour
{
    public static SceneTransformer instance;

    public int NowSceneID = 0;
    public int LastSceneID = 0;
    public int NowTeamID = 0;
    public int NowGateID = 0;
    public int NowChapterID = 1;
    public int NowWorldEventID = 0;
    public int NowGateLevel = 0;
    public int NowGateStar = 0;
    public int NowFightRank = 0;
    public bool IsVitality = false;
    public bool IsGate = false;
    public float SceneCamera = 0f;
    public List<NPC> ListNPC = new List<NPC>();
    public List<Gate> ListGate = new List<Gate>();

    public Vector3 WeatherPosition;
    int MonsterTotalCount = 0;

    public GameObject MainScene;
    public string TempSendString;

    public static bool IsDoubleClick = false;
    public float GuideTimer = 0;
    public GameObject NewGuideObj;
    int NowID = 0;
    //五秒后自动播放下一段
    public bool isNextTalk = false;
    public int Talkid = 0;
    public int Countid = 0;

    public int ReloadCount = 0;
    public class NPC
    {
        public string NPCName;
        public string PointID;
        public Vector3 NPCPosition;
        public string NPCPictureName;
        public float Scale;
        public float Offset;
        public int Stand;
        public int Funny;
        public int Width;
        public int Height;
        public int WidthCount;
        public int HeightCount;
        public string NPCConversation;
    }
    //世界事件对话
    public string heroName = "";
    public string heroIcon = "";
    //主界面提醒点击
    public bool isNewGuide = false;
    //用于判断是否在按钮的时候卡死 跳过新手引导
    public bool isClickSkip = false;
    void Start()
    {
        instance = this;
    }

    public void CreateGate(string GateName, string GateID, int TaskID)
    {
        Gate NewGate = new Gate();
        NewGate.GateName = GateName;
        NewGate.GateID = GateID;
        NewGate.TaskID = TaskID;
        ListGate.Add(NewGate);
    }

    public void CreateNPC(string NPCName, string PointID, Vector3 NPCPosition, string NPCPictureName, string NPCConversation, int Stand, int Funny, int Width, int Height, float Scale, float Offset)
    {
        NPC NewNPC = new NPC();
        NewNPC.NPCName = NPCName;
        NewNPC.PointID = PointID;
        NewNPC.NPCPosition = NPCPosition;
        NewNPC.NPCPictureName = NPCPictureName;
        NewNPC.NPCConversation = NPCConversation;
        NewNPC.Funny = Funny;
        NewNPC.Stand = Stand;
        NewNPC.Width = Width;
        NewNPC.Height = Height;
        NewNPC.Scale = Scale;
        NewNPC.Offset = Offset;
        ListNPC.Add(NewNPC);
    }

    public string GetNPCConversation(string NPCName)
    {
        string NPCConversation = "";
        foreach (NPC n in ListNPC)
        {
            if (n.NPCName == NPCName)
            {
                NPCConversation = n.NPCConversation;
            }
        }
        return NPCConversation;
    }

    public string GetNPCName(string PointID)
    {
        string NPCName = "";
        foreach (NPC n in ListNPC)
        {
            if (n.PointID == PointID)
            {
                NPCName = n.NPCName;
            }
        }
        return NPCName;
    }

    //public void ChangeGate(int GateID, string Monster, string Boss, string Transport)
    //{
    //    MonsterTotalCount = 0;
    //    NowGateID = GateID;
    //    NowGateStar = 10;
    //    if (GateID > 7)
    //    {
    //        GateID = 7;
    //    }
    //    PictureCreater.instance.RoleScenePosition = PictureCreater.instance.ListRolePicture[0].RoleObject.transform.position;
    //    PictureCreater.instance.CameraScenePosition = GameObject.Find("MainCamera").transform.position;
    //    TextAsset LuaText = (TextAsset)Resources.Load("Gate/Gate" + GateID.ToString() + "/config");

    //    XMLParser.instance.ParseXMLNodeScript(LuaText.ToString());

    //    PictureCreater.instance.DestroyAllComponent();

    //    ///////////计算摄像机边界(以下)/////////
    //    PictureCreater.instance.SceneSideWidth = 14f * (1f - (250f / Screen.height)) + ((750f - Screen.width) / Screen.height * 4f);// -(Screen.width / Screen.height * 0.005f);  //计算不同荧幕大小边界
    //    PictureCreater.instance.SceneSideHeight = 4f * (1f - (250f / Screen.height)) + ((750f - Screen.width) / Screen.height * 4f);// -(Screen.width / Screen.height * 0.005f);  //计算不同荧幕大小边界
    //    ///////////计算摄像机边界(以上)/////////

    //    PictureCreater.instance.IsRoleInGate = true;
    //    if (GateID < 3)
    //    {
    //        CreateMap(8, 4, 4, 4, "Gate", GateID);
    //    }
    //    else if (GateID == 3)
    //    {
    //        CreateMap(15, 5, 4, 4, "Gate", GateID);
    //    }
    //    else if (GateID == 4)
    //    {
    //        CreateMap(13, 5, 4, 4, "Gate", GateID);
    //    }
    //    else if (GateID == 5)
    //    {
    //        CreateMap(13, 6, 4, 4, "Gate", GateID);
    //    }
    //    else if (GateID == 6)
    //    {
    //        CreateMap(13, 6, 4, 4, "Gate", GateID);
    //    }
    //    else if (GateID == 7)
    //    {
    //        CreateMap(11, 4, 4, 4, "Gate", GateID);
    //    }

    //    Vector3 StartPosition = PictureCreater.instance.RoleStartPosition;
    //    /////////////建立魔法阵(以下)/////////////
    //    GameObject Magic = new GameObject("Magic");
    //    Magic.transform.position = new Vector3(StartPosition.x, StartPosition.y, StartPosition.y + 5);
    //    Magic.transform.localScale = new Vector3(2.5f, 2.5f, 1);
    //    Magic.AddComponent<SpriteRenderer>();
    //    Magic.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magic", typeof(Sprite)) as Sprite;
    //    Magic.AddComponent<Animator>();
    //    Magic.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magic_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube0 = new GameObject("MagicCube0");
    //    MagicCube0.transform.position = new Vector3(StartPosition.x, StartPosition.y - 1f, StartPosition.y - 1.5f);
    //    MagicCube0.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube0.AddComponent<SpriteRenderer>();
    //    MagicCube0.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube0.AddComponent<Animator>();
    //    MagicCube0.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube1 = new GameObject("MagicCube1");
    //    MagicCube1.transform.position = new Vector3(StartPosition.x, StartPosition.y + 2f, StartPosition.y + 1.5f);
    //    MagicCube1.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube1.AddComponent<SpriteRenderer>();
    //    MagicCube1.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube1.AddComponent<Animator>();
    //    MagicCube1.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube2 = new GameObject("MagicCube2");
    //    MagicCube2.transform.position = new Vector3(StartPosition.x - 2f, StartPosition.y, StartPosition.y - 0.5f);
    //    MagicCube2.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube2.AddComponent<SpriteRenderer>();
    //    MagicCube2.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube2.AddComponent<Animator>();
    //    MagicCube2.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube3 = new GameObject("MagicCube3");
    //    MagicCube3.transform.position = new Vector3(StartPosition.x + 2f, StartPosition.y, StartPosition.y - 0.5f);
    //    MagicCube3.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube3.AddComponent<SpriteRenderer>();
    //    MagicCube3.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube3.AddComponent<Animator>();
    //    MagicCube3.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube4 = new GameObject("MagicCube4");
    //    MagicCube4.transform.position = new Vector3(StartPosition.x - 2f, StartPosition.y + 1f, StartPosition.y + 0.5f);
    //    MagicCube4.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube4.AddComponent<SpriteRenderer>();
    //    MagicCube4.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube4.AddComponent<Animator>();
    //    MagicCube4.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube5 = new GameObject("MagicCube5");
    //    MagicCube5.transform.position = new Vector3(StartPosition.x + 2f, StartPosition.y + 1f, StartPosition.y + 0.5f);
    //    MagicCube5.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube5.AddComponent<SpriteRenderer>();
    //    MagicCube5.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube5.AddComponent<Animator>();
    //    MagicCube5.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube6 = new GameObject("MagicCube6");
    //    MagicCube6.transform.position = new Vector3(StartPosition.x + 1f, StartPosition.y - 0.7f, StartPosition.y - 1.2f);
    //    MagicCube6.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube6.AddComponent<SpriteRenderer>();
    //    MagicCube6.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube6.AddComponent<Animator>();
    //    MagicCube6.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube7 = new GameObject("MagicCube7");
    //    MagicCube7.transform.position = new Vector3(StartPosition.x - 1f, StartPosition.y - 0.7f, StartPosition.y - 1.2f);
    //    MagicCube7.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube7.AddComponent<SpriteRenderer>();
    //    MagicCube7.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube7.AddComponent<Animator>();
    //    MagicCube7.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube8 = new GameObject("MagicCube8");
    //    MagicCube8.transform.position = new Vector3(StartPosition.x + 1f, StartPosition.y + 1.7f, StartPosition.y + 1.2f);
    //    MagicCube8.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube8.AddComponent<SpriteRenderer>();
    //    MagicCube8.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube8.AddComponent<Animator>();
    //    MagicCube8.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

    //    GameObject MagicCube9 = new GameObject("MagicCube9");
    //    MagicCube9.transform.position = new Vector3(StartPosition.x - 1f, StartPosition.y + 1.7f, StartPosition.y + 1.2f);
    //    MagicCube9.transform.localScale = new Vector3(1, 1, 1);
    //    MagicCube9.AddComponent<SpriteRenderer>();
    //    MagicCube9.GetComponent<SpriteRenderer>().sprite = Resources.Load("Effect/magiccube", typeof(Sprite)) as Sprite;
    //    MagicCube9.AddComponent<Animator>();
    //    MagicCube9.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Effect/magiccube_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
    //    /////////////建立魔法阵(以上)/////////////

    //    string[] Monsters = Monster.Split('!');
    //    foreach (string m in Monsters)
    //    {
    //        if (m != "")
    //        {
    //            string[] MonsterData = m.Split('$');
    //            int MonsterCount = int.Parse(MonsterData[0]);
    //            int MonsterRow = int.Parse(MonsterData[1]);
    //            int MonsterDelay = int.Parse(MonsterData[2]);
    //            int MonsterSpeed = int.Parse(MonsterData[3]);
    //            int MonsterRoleID = int.Parse(MonsterData[4]);
    //            int MonsterDistance = int.Parse(MonsterData[5]);
    //            int MonsterBlood = int.Parse(MonsterData[6]);
    //            int MonsterAttack = int.Parse(MonsterData[7]);
    //            int MonsterDefend = int.Parse(MonsterData[8]);
    //            StartCoroutine(CreateMonster(MonsterCount, MonsterRow, MonsterDelay, MonsterSpeed, MonsterRoleID, MonsterDistance, MonsterBlood, MonsterAttack, MonsterDefend, PictureCreater.instance.StartEnemyPosition));
    //        }
    //    }

    //    string[] Bosses = Boss.Split('!');
    //    foreach (string b in Bosses)
    //    {
    //        if (b != "")
    //        {
    //            string[] BossData = b.Split('$');
    //            int MonsterDelay = int.Parse(BossData[0]);
    //            int MonsterSpeed = int.Parse(BossData[1]);
    //            int MonsterRoleID = int.Parse(BossData[2]);
    //            int MonsterDistance = int.Parse(BossData[3]);
    //            int MonsterBlood = int.Parse(BossData[4]);
    //            int MonsterAttack = int.Parse(BossData[5]);
    //            int MonsterDefend = int.Parse(BossData[6]);
    //            StartCoroutine(CreateBoss(MonsterDelay, MonsterSpeed, MonsterRoleID, MonsterDistance, MonsterBlood, MonsterAttack, MonsterDefend, PictureCreater.instance.StartEnemyPosition));
    //        }
    //    }
    //}

    //IEnumerator CreateMonster(int MonsterCount, int MonsterRow, int MonsterDelay, int MonsterSpeed, int MonsterRoleID, int MonsterDistance, int MonsterBlood, int MonsterAttack, int MonsterDefend, Vector3 StartPosition)
    //{
    //    List<Vector3> ListMonsterTrace = new List<Vector3>();
    //    ListMonsterTrace.Add(PictureCreater.instance.RoleStartPosition - StartPosition);
    //    ListMonsterTrace.Add(new Vector3(-2, 0, 0));
    //    int i = 0;
    //    float PositionY = 0;

    //    yield return new WaitForSeconds(MonsterDelay);

    //    while (i < MonsterCount)
    //    {
    //        for (int j = 0; j < MonsterRow; j++)
    //        {
    //            if (PictureCreater.instance.IsRoleInGate)
    //            {
    //                PositionY = StartPosition.y + (0.7f * (j % 5));
    //                int RoleIndex = PictureCreater.instance.CreateRole(MonsterRoleID, "Monster" + i.ToString(), new Vector3(StartPosition.x + (j / 5) * 2.5f, PositionY, PositionY), Color.white, MonsterBlood, MonsterBlood, MonsterDistance / 10f, true, false, 1, 2f, MonsterTotalCount.ToString(), 0, MonsterAttack, MonsterDefend, 0, 0, 0, 0, 0, 0);
    //                PictureCreater.instance.SetPictureTracePosition(PictureCreater.instance.ListEnemyPicture, ListMonsterTrace, RoleIndex, MonsterSpeed * 0.0015f, 0, false, false);
    //            }
    //            i++;
    //            MonsterTotalCount++;
    //        }
    //        yield return new WaitForSeconds(2);
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //}

    //IEnumerator CreateBoss(int MonsterDelay, int MonsterSpeed, int MonsterRoleID, int MonsterDistance, int MonsterBlood, int MonsterAttack, int MonsterDefend, Vector3 StartPosition)
    //{
    //    List<Vector3> ListMonsterTrace = new List<Vector3>();
    //    ListMonsterTrace.Add(PictureCreater.instance.RoleStartPosition - StartPosition);
    //    ListMonsterTrace.Add(new Vector3(-2, 0, 0));

    //    yield return new WaitForSeconds(MonsterDelay);

    //    if (PictureCreater.instance.IsRoleInGate)
    //    {
    //        int RoleIndex = PictureCreater.instance.CreateRole(MonsterRoleID, XMLParser.instance.GetRoleName(MonsterRoleID), new Vector3(StartPosition.x, StartPosition.y, StartPosition.y), Color.red, MonsterBlood, MonsterBlood, MonsterDistance / 10f, true, false,1, 3f, "Boss" + MonsterRoleID.ToString(), 0, MonsterAttack, MonsterDefend, 0, 0, 0, 0, 0, 0);
    //        PictureCreater.instance.SetPictureTracePosition(PictureCreater.instance.ListEnemyPicture, ListMonsterTrace, RoleIndex, 0.015f, 0, false, false);
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //}


    public void ShowMainScene(bool IsShow)
    {
        if (MainScene == null)
        {
            MainScene = GameObject.Instantiate(Resources.Load("GUI/MapWindow", typeof(GameObject))) as GameObject;
            MainScene.name = "MapObject";
        }

        PictureCreater.instance.MyBases.SetActive(false);
        PictureCreater.instance.MyMoves.SetActive(false);
        PictureCreater.instance.MyPositions.SetActive(false);
        MainScene.SetActive(IsShow);
    }

    public void CreateMap(int SceneWidthCount, int SceneHeightCount, float SceneWidth, float SceneHeight, string SceneType, int SceneID)
    {
        /////////计算摄像机边界(以下)/////////
        PictureCreater.instance.SceneSideWidth = (SceneWidth - 2) * (SceneWidthCount - 2) * (1f - (250f / Screen.height)) + ((750f - Screen.width) / Screen.height * 4f);// -(Screen.width / Screen.height * 0.005f);  //计算不同荧幕大小边界
        PictureCreater.instance.SceneSideHeight = (SceneHeight - 1.3f) * ((SceneHeightCount) - 2f) * (1f - (250f / Screen.height)) + ((750f - Screen.width) / Screen.height * 4f);// -(Screen.width / Screen.height * 0.005f);  //计算不同荧幕大小边界

        if (SceneID == 999 || SceneID == 998)
        {
            PictureCreater.instance.SceneSideWidth = 0;
            PictureCreater.instance.SceneSideHeight = 0;
        }
        /////////计算摄像机边界(以上)/////////

        //GameObject.Find("MainCamera").transform.position = new Vector3(PictureCreater.instance.CameraStartPosition.x, PictureCreater.instance.CameraStartPosition.y, GameObject.Find("MainCamera").transform.position.z);

        for (int i = 0; i <= GameObject.Find("BG").transform.childCount; i++)
        {
            Destroy(GameObject.Find("BG" + i.ToString()));
        }

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        for (int j = 0; j < SceneHeightCount; j++)
        {
            for (int i = 0; i < SceneWidthCount; i++)
            {
                ////////////////////////地图(以下)////////////////////////
                GameObject BackgroundObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                DestroyImmediate(BackgroundObject.GetComponent("MeshCollider"));
                BackgroundObject.renderer.castShadows = false;
                BackgroundObject.renderer.receiveShadows = false;
                BackgroundObject.name = "BG" + (SceneWidthCount * j + i).ToString();

                BackgroundObject.transform.position = new Vector3(-SceneWidth * (SceneWidthCount - 1) / 2 + SceneWidth * i, 0, SceneHeight * (SceneHeightCount - 1) / 2 - SceneHeight * j);
                BackgroundObject.renderer.material.mainTexture = Resources.Load(SceneType + "/" + SceneType + SceneID.ToString() + "/" + (100 + SceneWidthCount * j + i).ToString("00"), typeof(Texture)) as Texture;

                BackgroundObject.transform.Rotate(90f, 0f, 0f);
                BackgroundObject.transform.localScale = new Vector3(SceneWidth, SceneHeight, 1);
                BackgroundObject.renderer.material.mainTextureOffset = new Vector2(0, 0);
                BackgroundObject.renderer.material.mainTextureScale = new Vector2(1, 1); //8X8 可調
                BackgroundObject.renderer.material.shader = Shader.Find("Unlit/Texture");
                BackgroundObject.renderer.material.color = Color.white;
                BackgroundObject.transform.parent = GameObject.Find("BG").transform;
                ////////////////////////地图(以上)////////////////////////
            }
        }
    }
    float timer = 0;
    bool canSendDeal = false;
    void Update()
    {
        //Debug.Log(IsDoubleClick);
        if (CharacterRecorder.instance.GuideID.Count > 0)
        {
            if (CharacterRecorder.instance.GuideID[CharacterRecorder.instance.NowGuideID] > 0 && CharacterRecorder.instance.GuideID[CharacterRecorder.instance.NowGuideID] < 99)
            {
                GuideTimer += Time.deltaTime;
                //Debug.Log(GuideTimer);
                if (GuideTimer > 4f &&
                    GameObject.Find("BigGuide") == null && (GameObject.Find("MinGuide") != null || GameObject.Find("MaxWindow") != null ||
                    GameObject.Find("GuideArrow") != null || GameObject.Find("SmallGuide") != null || GameObject.Find("CommonGuide") != null ||
                    GameObject.Find("TalkGuide") != null || GameObject.Find("MoveGuide") != null) && isNewGuide == false && CharacterRecorder.instance.GuideID[10] != 11)//关闭夺宝战斗结束后的跳过
                {
                    Debug.LogError("NowID " + CharacterRecorder.instance.NowGuideID);
                    GuideTimer = 0;
                    //StartCoroutine(NewbieGuide());
                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SkipGuideButton.SetActive(true);
                    if (CharacterRecorder.instance.GuideID[17] == 1 || CharacterRecorder.instance.GuideID[18] == 3 || CharacterRecorder.instance.GuideID[19] == 2)
                    {
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SkipGuideButton.transform.localPosition = new Vector3(523, -343, 0);
                    }
                    else
                    {
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SkipGuideButton.transform.localPosition = new Vector3(523, 343, 0);
                    }
                }
            }
        }

        if (CharacterRecorder.instance.HoldJunhuoTime > 0) //yy
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f)
            {
                CharacterRecorder.instance.HoldJunhuoTime -= 1;
                timer -= 1;
            }
            if (CharacterRecorder.instance.HoldJunhuoTime == 0)
            {
                if (GameObject.Find("MainWindow") != null)
                {
                    GameObject.Find("MainWindow").GetComponent<MainWindow>().SureJunhuokuIsOpen();
                }
            }
        }
        //if (CharacterRecorder.instance.HoldOnLeftTime > 0) //kino拿掉挂机
        //{
        //    timer += Time.deltaTime;
        //    if (timer >= 1.0f)
        //    {
        //        CharacterRecorder.instance.HoldOnLeftTime -= 1;
        //        timer = 0;
        //    }
        //    canSendDeal = true;
        //}
        //else
        //{
        //    if (canSendDeal)
        //    {
        //        CharacterRecorder.instance.HoldOnLeftTime = 10;
        //        if (CharacterRecorder.instance.characterName != "") //创建姓名后才能发送1902
        //        {
        //            NetworkHandler.instance.SendProcess("1902#;");
        //            ReloadCount++;

        //            if (ReloadCount > 10 && GameObject.Find("MainWindow") != null)
        //            {
        //                Debug.LogError("Collect");
        //                ReloadCount = 0;
        //                Resources.UnloadUnusedAssets();
        //                System.GC.Collect();
        //            }
        //        }

        //        //CharacterRecorder.instance.HoldOnLeftTime = 60;
        //        canSendDeal = false;
        //    }
        //}
    }
    #region //新手引导

    public string GetGuideStateName()
    {
        string name = "GuideState_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID");
        return name;
    }

    public string GetGuideSubStateName()
    {
        string name = "GuideSubState_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID");
        return name;
    }
    public bool CheckGuideIsFinish()
    {
        if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) > 17)
        {
            return true;
        }
        return false;
    }

    public void AddGuideID(GameObject Button, int id)
    {
        Debug.Log("Button:" + Button.name + "--------:" + id);
        UIEventListener.Get(Button).onClick = null;
        UIEventListener.Get(Button).onClick += delegate(GameObject go)
        {
            CharacterRecorder.instance.GuideID[id] += 1;
            StartCoroutine(NewbieGuide());
        };
    }
    //button
    public void AddClick(GameObject go, int id)
    {
        Debug.LogError(go + "BBBBBBBBDDDD" + id);
        NowID = id;
        if (go != null)
        {
            UIEventListener.Get(go).onClick += NewClick;
        }
        else
        {
            StartCoroutine(GuideDelayCsharp(0.1f));
        }
        Debug.Log(UIEventListener.Get(go).onClick.GetInvocationList().Length + "  **************");
    }

    public void AddChangeClick(GameObject go, int id)
    {
        NowID = id;
        if (go != null)
        {
            UIEventListener.Get(go).onClick += NewClick;

            if (UIEventListener.Get(go).onClick.GetInvocationList().Length == 2)
            {
                UIEventListener.VoidDelegate Temp1 = (UIEventListener.VoidDelegate)UIEventListener.Get(go).onClick.GetInvocationList()[0];
                UIEventListener.VoidDelegate Temp2 = (UIEventListener.VoidDelegate)UIEventListener.Get(go).onClick.GetInvocationList()[1];
                UIEventListener.Get(go).onClick = null;
                UIEventListener.Get(go).onClick += Temp2;
                UIEventListener.Get(go).onClick += Temp1;
            }
            else if (UIEventListener.Get(go).onClick.GetInvocationList().Length > 2)
            {
                UIEventListener.VoidDelegate Temp1 = (UIEventListener.VoidDelegate)UIEventListener.Get(go).onClick.GetInvocationList()[0];
                UIEventListener.VoidDelegate Temp2 = (UIEventListener.VoidDelegate)UIEventListener.Get(go).onClick.GetInvocationList()[1];
                UIEventListener.Get(go).onClick = null;
                UIEventListener.Get(go).onClick += Temp1;
                UIEventListener.Get(go).onClick += Temp2;
            }
        }
        else
        {
            StartCoroutine(GuideDelayCsharp(0.1f));
        }
    }

    void NewClick(GameObject go)
    {
        Debug.LogError(go + " " + NowID);
        if (go != null)
        {
            Debug.LogError(CharacterRecorder.instance.GuideID[NowID] + "-----------------------------------" + NowID);
            CharacterRecorder.instance.GuideID[NowID] += 1;
            StartCoroutine(NewbieGuide());

            UIEventListener.Get(go).onClick -= NewClick;
            Debug.LogError(CharacterRecorder.instance.GuideID[NowID] + "-----------------------------------" + NowID);
        }
    }

    //list
    public void AddGuideList(string RecvString)
    {
        CharacterRecorder.instance.GuideID.Clear();
        string[] guideSplit = RecvString.Split('$');
        for (int i = 0; i < guideSplit.Length - 1; i++)
        {
            if (guideSplit[i] != "")
            {
                CharacterRecorder.instance.GuideID.Add(int.Parse(guideSplit[i]));
            }
            else
            {
                CharacterRecorder.instance.GuideID.Add(0);
            }
        }
        if (guideSplit.Length < 70)
        {
            for (int i = guideSplit.Length - 1; i < 70; i++)
            {
                CharacterRecorder.instance.GuideID.Add(0);
            }
        }
    }

    //延迟
    public void SetGuideDelayCsharp(float delayTime)
    {
        StartCoroutine(GuideDelayCsharp(delayTime));
    }
    IEnumerator GuideDelayCsharp(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(NewbieGuide());
    }
    #endregion

    public void SendGuide()
    {
        string SendString = "";
        if (CharacterRecorder.instance.GuideID[CharacterRecorder.instance.NowGuideID] != 99)
        {
            CharacterRecorder.instance.GuideID[CharacterRecorder.instance.NowGuideID] = 99;
        }
        for (int i = 0; i < CharacterRecorder.instance.GuideID.Count; i++)
        {
            SendString += CharacterRecorder.instance.GuideID[i] + "$";
        }
        NetworkHandler.instance.SendProcess("9402#" + SendString);
    }

    //强制新手引导领取宝箱卡死
    public void SendBoxGuide()
    {
        Debug.LogError("SSSSS   " + PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + "   " + PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()));
        if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 5)
        {
            if (CharacterRecorder.instance.lastGateID == 10002)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 6);
            }
        }
    }
    //新手引导按钮点击事件
    public void NewGuideButtonClick()
    {
        if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) < 18)
        {
            #region
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 0)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_300);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_301);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_302);
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_303);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_304);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 1 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_305);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_504);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_600);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_601);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_604);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_700);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_701);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 8)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_702);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 10)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_704);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 12)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_706);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 15)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_709);
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_710);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_800);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_801);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_802);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_803);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_806);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 8)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_807);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 9)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_900);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 5 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 0)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_900);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 5 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_901);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1000);
                if (GameObject.Find("GateInfoWindow") != null && GameObject.Find("MapObject") != null)
                {
                    GameObject.Find("GateInfoWindow").SetActive(false);
                    GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().NewGuideHandle1();
                }
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1002);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 6)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1004);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 9)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1006);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 7 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1008);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 8 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1100);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 8 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1101);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1102);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1103);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1104);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1105);
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1106);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1107);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 6)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1108);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1109);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 10 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 0)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1200);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 10 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1200);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 10 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1202);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 10 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1204);
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1205);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 11 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1206);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1300);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1301);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1303);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1304);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 10)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1305);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 11)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1306);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 13 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1308);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 14 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1310);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 15 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 0)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1400);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 15 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1400);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 15 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1401);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 15 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1403);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 15 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1406);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 15 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 8)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1407);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 16 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 0)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1500);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 16 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1500);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 16 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1501);
                if (GameObject.Find("GateInfoWindow") != null && GameObject.Find("MapObject") != null)
                {
                    GameObject.Find("GateInfoWindow").SetActive(false);
                }
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 16 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1502);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 17 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {

            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 17 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1600);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 17 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {

            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 17 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1602);
            }
            #endregion

            //Debug.LogError("现在引导ID：" + PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + "   " + PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()));
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 0)
            {
                StartCoroutine(PictureCreater.instance.Newbie());
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 1 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_306);
                //LuaDeliver.OpenPanel("StartNameWindow", true);
                //VersionChecker.instance.StartLogin();
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) + 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 1 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_501);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 2);
                UIManager.instance.OpenSinglePanel("CardWindow", false);
                GameObject _CardWindow = GameObject.Find("CardWindow");
                if (_CardWindow != null)
                {
                    _CardWindow.GetComponent<CardWindow>().SetCardInfo(60026);
                }
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 8)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_605);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 15)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 9)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 2);
            }
            //else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 5 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2) //kino
            //{
            //    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
            //    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            //}
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 9)
            {
                PictureCreater.instance.IsLock = false;
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1007);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 7 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 8 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 2)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);

            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 9 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 10 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 11 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 13 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 14 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 15 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 8)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 16 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 17 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            }
            else if (GameObject.Find("NewGuideWindow") != null) //kino拿掉了 不然连点引导会跳
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) + 1);
            }
            LuaDeliver.instance.UseGuideStation();
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 1)
            {
                GameObject.Find("GuideLegionWar").SetActive(false);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4)
            {
                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 6)
            {
                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                LuaDeliver.GuideIsLock();
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
            {
                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 1 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 3)
            {
                LuaDeliver.ClosePanel("NewGuideWindow", false);
            }

        }
        if (GameObject.Find("NewGuideWindow") != null)
        {
            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().timer = 0;
        }
    }
    IEnumerator GuideSpeakInfo()
    {
        yield return new WaitForSeconds(0.4f);
        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "这次任务有些棘手，兄弟们别掉以轻心!", 1, "ST", "圣诞");
    }
    //新手引导强制写死
    public void AddButtonClick(int id)
    {
        if (CharacterRecorder.instance.lastGateID >= 10005)
        {
            //Debug.LogError("现在引导ID    " + id + "       " + CharacterRecorder.instance.GuideID[id]);
            // if (CheckClick(id))
            {
                GuideTimer = 0;
                CharacterRecorder.instance.GuideID[id] += 1;
                StartCoroutine(NewbieGuide());
                if (GameObject.Find("NewGuideWindow") != null)
                {
                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().timer = 0;
                }
            }
            //Debug.LogError("点击后引导ID    " + id + "       " + CharacterRecorder.instance.GuideID[id]);
        }
    }
    public void NewGuideTalkButtonClick()
    {
        PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) + 1);
    }
    public void TalkButtonClick(int id)
    {
        if (CharacterRecorder.instance.lastGateID >= 1005)
        {
            CharacterRecorder.instance.GuideID[id] += 1;
            StartCoroutine(NewbieGuide());
        }
    }
    #region 新手引导



    public IEnumerator NewbieGuide()
    {
        while (GameObject.Find("NewGuideWindow") == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        IsDoubleClick = false;

        #region --关卡引导

        if (CheckGuideIsFinish())
        {
            if (GameObject.Find("FightWindow") && PictureCreater.instance.FightStyle == 0)
            {
                #region 第5-1  索引0
                if (CharacterRecorder.instance.GuideID[0] < 5 && CharacterRecorder.instance.lastGateID == 10005)
                {
                    CharacterRecorder.instance.NowGuideID = 0;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[0] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    bool isHadRole = false;
                    for (int j = 0; j < CharacterRecorder.instance.ownedHeroList.size; j++)
                    {
                        if (60028 == CharacterRecorder.instance.ownedHeroList[j].cardID)
                        {
                            isHadRole = false;
                            break;
                        }
                        else
                        {
                            isHadRole = true;
                        }
                    }
                    if (isHadRole)
                    {
                        CharacterRecorder.instance.GuideID[0] = 99;
                        SendGuide();
                    }
                    switch (CharacterRecorder.instance.GuideID[0])
                    {
                        case 0:
                            if (GameObject.Find("LoadingWindow") == null)
                            {
                                LuaDeliver.AudioNewGuide("Story40");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "阴阳老弟，修炼不如实战，直接去打倒[ff0000]维嘉军团[-]就是最好的修炼。", 2, "ST", "圣诞");
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);

                            }
                            break;
                        case 1:
                            LuaDeliver.AudioNewGuide("Story41");
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "想法倒是可以，那我们就先实战一把。", 1, "YY", "阴阳");
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1604);
                            break;
                        case 2:
                            LuaDeliver.AudioNewGuide("Story42");
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官！我们必须在[d35818]50[-]步数内打倒阴阳，以免误伤。\n记得多用[d35818]手榴弹[-]，这样会打得快一点。", 1, "Lili", "杰西卡");
                            break;
                        //case 3:
                        //    LuaDeliver.AudioNewGuide("Guide29");
                        //    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                        //    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(7, "请先[d35818]拖动[-]茱莉亚[d35818]下阵[-]", 1, "Lili", "杰西卡");
                        //    LuaDeliver.AddLabelClick(new Vector3(0, -300, 0));
                        //    LuaDeliver.AddArrowMove(new Vector3(-280, 50, 0),
                        //                new Vector3(-340, -340, 0));
                        //    break;
                        case 3:
                            if (GameObject.Find("AutoButton"))
                            {
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1605);
                                LuaDeliver.AudioNewGuide("Guide30");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                LuaDeliver.AddGuideClick(GameObject.Find("AutoButton"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "请在[d35818]限制步数[-]内打赢阴阳", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(200, -200, 0));
                            }
                            else
                            {
                                CharacterRecorder.instance.GuideID[0] += 1;
                                StartCoroutine(NewbieGuide());
                            }
                            break;
                        case 4:
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1606);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1607);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[0] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
                #region 第5-2 索引1
                else if (CharacterRecorder.instance.GuideID[1] < 4 && CharacterRecorder.instance.GuideID[0] >= 4 && PictureCreater.instance.IsResult && CharacterRecorder.instance.lastGateID == 10005)
                {
                    CharacterRecorder.instance.NowGuideID = 1;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[1] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[1])
                    {
                        case 0:
                            LuaDeliver.AudioNewGuide("Story43");
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "看来你身手依旧，今天就加入敢死队，我们一起去干掉维嘉吧！", 1, "ST", "圣诞");
                            ////AddGuideID(GameObject.Find("BigGuide"), 1);
                            break;
                        case 1:
                            LuaDeliver.AudioNewGuide("Story44");
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "好！我回去准备一下。[d35818]明天[-]登录游戏，我准时回来报到！", 1, "YY", "阴阳");
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1608);
                            ////AddGuideID(GameObject.Find("BigGuide"), 1);
                            break;
                        case 2:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            NetworkHandler.instance.SendProcess(TempSendString);
                            CharacterRecorder.instance.GuideID[1] += 1;
                            StartCoroutine(NewbieGuide());
                            break;
                        default:
                            if (GameObject.Find("ResultWindow") != null)
                            {
                                if (GameObject.Find("BackButton") != null)
                                {
                                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                    //////AddChangeClick(GameObject.Find("BackButton"),, 1);
                                    LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);

                                    CharacterRecorder.instance.GuideID[1] += 1;
                                    SendGuide();
                                }
                                else
                                {
                                    SetGuideDelayCsharp(0.1f);
                                }
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                    }
                }
                #endregion
                #region 第7-1 索引2
                else if (CharacterRecorder.instance.GuideID[2] < 4 && CharacterRecorder.instance.lastGateID == 10007 && NowGateID == 10007)
                {
                    CharacterRecorder.instance.NowGuideID = 2;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[2] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[2])
                    {
                        case 0:

                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，请留意[ff0000]地上的物品[-]，喝掉这罐能量饮料会增加[ff0000]1000点[-]怒气，敌我双方都会触发哦~", 1, "Lili", "杰西卡");
                            ////AddGuideID(GameObject.Find("BigGuide"), 2);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "嘿！老伙计们！快过来喝[ff8c04]能量饮料[-]!", 2, "Bani", "巴尼");
                            ////AddGuideID(GameObject.Find("BigGuide"), 2);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1902);
                            break;
                        case 2:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "你个混蛋！终于出现了！不扁你真是心里不爽啊！", 1, "MQ", "麦琪");
                            ////AddGuideID(GameObject.Find("BigGuide"), 2);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1902);
                            break;
                        default:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[2] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
                #region 第7-2 索引3
                else if (CharacterRecorder.instance.GuideID[3] < 4 && CharacterRecorder.instance.GuideID[2] >= 3 && PictureCreater.instance.IsResult && CharacterRecorder.instance.lastGateID == 10007)
                {
                    CharacterRecorder.instance.NowGuideID = 3;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[3] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[3])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "对不起了！我还有重要事情要做，需要暂时分开一段时间了。这些[ff8c04]能量饮料[-]记得合理使用，希望下次见面你们可以变的更强！", 1, "Bani", "巴尼");
                            ////AddGuideID(GameObject.Find("BigGuide"), 3);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "喂！回来！(该死，跑的好快！)", 2, "MQ", "麦琪");
                            ////AddGuideID(GameObject.Find("BigGuide"), 3);
                            break;
                        case 2:
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1904);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            NetworkHandler.instance.SendProcess(TempSendString);
                            CharacterRecorder.instance.GuideID[3] += 1;
                            StartCoroutine(NewbieGuide());
                            break;
                        default:
                            if (GameObject.Find("ResultWindow") != null)
                            {
                                if (GameObject.Find("BackButton") != null)
                                {
                                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                    //////AddChangeClick(GameObject.Find("BackButton"),, 3);
                                    LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);

                                    CharacterRecorder.instance.GuideID[3] += 1;
                                    SendGuide();
                                }
                                else
                                {
                                    SetGuideDelayCsharp(0.1f);
                                }
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            //CharacterRecorder.instance.GuideID[3] += 1;  
                            break;
                    }
                }
                #endregion
                #region 第11 索引4
                else if (CharacterRecorder.instance.GuideID[4] < 3 && CharacterRecorder.instance.lastGateID == 10011 && NowGateID == 10011)
                {
                    CharacterRecorder.instance.NowGuideID = 4;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[4] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[4])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，联盟军的精锐部队出现了！只要[ff8c04]击败敌方的队长[-]就可以赢得这场战斗的胜利。", 1, "Lili", "杰西卡");
                            ////AddGuideID(GameObject.Find("BigGuide"), 4);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "我们只要[ff8c04]合理的布阵[-]和运用战术技能打败敌方队长，就能取得胜利。", 1, "ST", "圣诞");
                            ////AddGuideID(GameObject.Find("BigGuide"), 4);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2603);
                            break;
                        default:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[4] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
                #region 第18-1 索引5
                else if (CharacterRecorder.instance.GuideID[5] < 3 && CharacterRecorder.instance.lastGateID == 10018 && NowGateID == 10018)
                {
                    CharacterRecorder.instance.NowGuideID = 5;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[5] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[5])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官！敌方的指挥官也开始投入战斗了！请小心[ff8c04]敌方的战术技能[-]！", 1, "Lili", "杰西卡");
                            ////AddGuideID(GameObject.Find("BigGuide"), 5);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "小心，有情报显示他会使用让己方[ff8c04]增加攻击力[-]的战术技能！", 2, "Lili", "杰西卡");
                            ////AddGuideID(GameObject.Find("BigGuide"), 5);
                            break;
                        default:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[5] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
                #region 第18-2 索引6
                else if (CharacterRecorder.instance.GuideID[6] < 2 && CharacterRecorder.instance.GuideID[5] > 2 && CharacterRecorder.instance.lastGateID == 10018 && PictureCreater.instance.IsResult)
                {
                    CharacterRecorder.instance.NowGuideID = 6;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[6] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[6])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "年轻人，有一套。我们后会有期。", 1, "Victor", "敌方指挥官");//敌方指挥官
                            ////AddGuideID(GameObject.Find("BigGuide"), 6);
                            break;
                        default:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            SendGuide();
                            if (CharacterRecorder.instance.GuideID[59] != 1)
                            {
                                NetworkHandler.instance.SendProcess(TempSendString);
                                CharacterRecorder.instance.GuideID[6] += 1;
                            }
                            else
                            {
                                CharacterRecorder.instance.GuideID[6] = 99;
                                StartCoroutine(NewbieGuide());
                            }
                            break;
                    }
                }
                #endregion
                #region 第21-1 索引7
                else if (CharacterRecorder.instance.GuideID[7] < 3 && CharacterRecorder.instance.lastGateID == 10021 && NowGateID == 10021)
                {
                    CharacterRecorder.instance.NowGuideID = 7;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[7] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[7])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "劳拉！你怎么会和贼匪一窝了？！", 1, "ST", "圣诞");
                            //AddGuideID(GameObject.Find("BigGuide"), 7);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "拿人钱财替人消灾，今天恐怕要得罪了！", 2, "LARA", "劳拉");
                            //AddGuideID(GameObject.Find("BigGuide"), 7);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3800);
                            break;
                        default:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[7] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
                #region 第21-2 索引8
                else if (CharacterRecorder.instance.GuideID[8] < 4 && CharacterRecorder.instance.GuideID[7] >= 2 && CharacterRecorder.instance.lastGateID == 10021 && PictureCreater.instance.IsResult)
                {
                    CharacterRecorder.instance.NowGuideID = 8;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[8] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[8])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "我很想帮你们，但是我现在急需一笔资金，联盟军出的起，我。。", 1, "LARA", "劳拉");
                            //AddGuideID(GameObject.Find("BigGuide"), 8);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "没关系，人各有志，等我们筹到了一定的资金，一定回来找你。", 2, "ST", "圣诞");
                            //AddGuideID(GameObject.Find("BigGuide"), 8);
                            break;
                        case 2:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "等你们筹到资金一定要来找我，我在[d35818]首充[-]等著你们。\n这个[d35818]芯片[-]也给你们吧。", 1, "LARA", "劳拉");
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3802);
                            SceneTransformer.instance.ShowXinPian();
                            break;
                        default:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            SendGuide();
                            if (CharacterRecorder.instance.GuideID[59] != 1)
                            {
                                NetworkHandler.instance.SendProcess(TempSendString);
                                CharacterRecorder.instance.GuideID[8] += 1;
                            }
                            else
                            {
                                CharacterRecorder.instance.GuideID[8] = 99;
                                StartCoroutine(NewbieGuide());
                            }
                            break;
                    }
                }
                #endregion
                #region  第8 索引15
                else if (CharacterRecorder.instance.GuideID[15] < 4 && CharacterRecorder.instance.lastGateID == 10008 && NowGateID == 10008)
                {
                    CharacterRecorder.instance.NowGuideID = 15;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[15] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[15])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "快上！前面就一个人，看我一枪秒了他！", 1, "glfs", "男枪");
                            //AddGuideID(GameObject.Find("BigGuide"), 15);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "等等！你看天上！", 2, "ST", "圣诞");
                            //AddGuideID(GameObject.Find("BigGuide"), 15);
                            break;
                        case 2:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "我了个去！空降团啊！", 1, "glfs", "男枪");
                            //AddGuideID(GameObject.Find("BigGuide"), 15);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2103);
                            break;
                        default:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[15] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
                #region  第20 索引22
                else if (CharacterRecorder.instance.GuideID[22] < 3 && CharacterRecorder.instance.lastGateID == 10019 && NowGateID == 10019)
                {
                    CharacterRecorder.instance.NowGuideID = 22;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[22] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[22])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "前面好像有人被围堵了！", 1, "ST", "圣诞");
                            //AddGuideID(GameObject.Find("BigGuide"), 22);
                            break;
                        case 1:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "快帮我解决这帮烦人的家伙！", 2, "LARA", "劳拉");
                            //AddGuideID(GameObject.Find("BigGuide"), 22);
                            break;
                        //case 2:
                        //GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "男枪已经冲上去了！", 1, "ST", "圣诞");
                        //AddGuideID(GameObject.Find("BigGuide"), 22);
                        //break;
                        //case 3:
                        //GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "HELP～！", 2, "LARA", "劳拉");
                        //AddGuideID(GameObject.Find("BigGuide"), 22);
                        // break;
                        case 2:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[22] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
                #region 第9关 自动战斗 索引35
                else if (CharacterRecorder.instance.GuideID[35] < 4 && CharacterRecorder.instance.lastGateID == 10006 && NowGateID == 10006)
                {
                    CharacterRecorder.instance.NowGuideID = 35;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[35] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[35])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，可以点战斗加速。", 2, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 35);

                            break;
                        case 1:
                            if (GameObject.Find("SpeedButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "激活战斗加速", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(-300, -300, 0));
                                //GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddChangeClick(GameObject.Find("HandButton"), 35);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2303);
                                LuaDeliver.AddGuideClick(GameObject.Find("SpeedButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 2:
                            if (GameObject.Find("AutoButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddChangeClick(GameObject.Find("AutoButton"), 35);
                                LuaDeliver.AddGuideClick(GameObject.Find("AutoButton"), 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2304);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        default:
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2305);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[35] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion

                #region   失败前往升品
                else if (CharacterRecorder.instance.GuideID[59] >= 1 && CharacterRecorder.instance.GuideID[59] < 3)
                {
                    CharacterRecorder.instance.NowGuideID = 59;
                    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[59] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                    switch (CharacterRecorder.instance.GuideID[59])
                    {
                        case 1:
                            if (GameObject.Find("ButtonShenping") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "[-][249BD2]快点去升品[-]\n[ff8304]提高[-][8ccef2][-]战斗力吧[-]。", 1, "Lili", "杰西卡");
                                LuaDeliver.AddGuideClick(GameObject.Find("ButtonShenping"), 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3100);
                                LuaDeliver.AddLabelClick(new Vector3(300, 130, 0));
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 2:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[59] += 1;
                            SendGuide();
                            break;
                    }
                }
                #endregion
            }
        #endregion
            #region 角色升级,军粮 5-2 索引26
            if (CharacterRecorder.instance.GuideID[26] < 9 && CharacterRecorder.instance.GuideID[1] > 3 && CharacterRecorder.instance.lastGateID == 10006 && GameObject.Find("FightWindow") == null && GameObject.Find("ResultWindow") == null)
            {
                CharacterRecorder.instance.NowGuideID = 26;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[26] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                bool isNewGuide = false;
                Hero hero = CharacterRecorder.instance.GetHeroByRoleID(60016);
                if (hero.level <= 6)
                {
                    isNewGuide = true;
                }
                if (isNewGuide)
                {
                    switch (CharacterRecorder.instance.GuideID[26])
                    {
                        case 0:
                            LuaDeliver.AudioNewGuide("Story45");
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我方英雄可以通过消耗军粮来[D35818]升级[-]提升战斗力，请让我来为您介绍使用方法。", 1, "Lili", "杰西卡");
                            ////AddGuideID(GameObject.Find("BigGuide"), 26);
                            break;
                        case 1:
                            if (GameObject.Find("MapUiWindow") != null)
                            {
                                if (GameObject.Find("BackButton") != null)
                                {
                                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                    //////AddChangeClick(GameObject.Find("BackButton"),, 26);
                                    LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.4f);
                                    UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1700);
                                }
                                else
                                {
                                    SetGuideDelayCsharp(0.1f);
                                }
                            }
                            else
                            {
                                CharacterRecorder.instance.GuideID[26] += 1;
                                StartCoroutine(NewbieGuide());
                            }
                            break;

                        case 2:
                            if (GameObject.Find("MainWindow") != null)
                            {
                                LuaDeliver.AudioNewGuide("Guide31");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddChangeClick(GameObject.Find("RoleButton"), 26);
                                LuaDeliver.AddGuideClick(GameObject.Find("RoleButton"), 0.4f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "首先打开英雄界面", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(300, 100, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1701);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 3:
                            if (GameObject.Find("SpriteTab2") != null)
                            {
                                LuaDeliver.AudioNewGuide("Guide32");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddClick(GameObject.Find("SpriteTab2"), 26);
                                LuaDeliver.AddGuideClick(GameObject.Find("SpriteTab2"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "进入升级界面", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(200, -50, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1702);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 4:
                            if (GameObject.Find("10001") != null)
                            {
                                LuaDeliver.AudioNewGuide("Guide33");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddClick(GameObject.Find("10001"), 26);
                                LuaDeliver.AddGuideClick(GameObject.Find("10001"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "点击使用军粮可增加英雄的等级", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(400, -200, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1703);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 5:
                            if (GameObject.Find("10001") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddClick(GameObject.Find("10001"), 26);
                                LuaDeliver.AddGuideClick(GameObject.Find("10001"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "点击使用军粮可增加英雄的等级", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(400, -200, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1703);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 6:
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                // ////AddChangeClick(GameObject.Find("BackButton"),, 26);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1704);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 7:
                            if (GameObject.Find("WorldButton") != null)
                            {
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1705);
                                LuaDeliver.AudioNewGuide("Guide34");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                // ////AddChangeClick(GameObject.Find("BackButton"),, 26);
                                LuaDeliver.AddGuideClick(GameObject.Find("WorldButton"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "接下来就交给长官您自由行动了", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(200, -200, 0));
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        default:
                            if (GameObject.Find("MapUiWindow") != null)
                            {
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1707);
                                UIManager.instance.OpenSinglePanel("AnnouncementWindow", false);
                                GameObject.Find("AnnouncementWindow").GetComponent<AnnouncementWindow>().SetTexture("mingridengru_di1");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 1, "", "");
                                CharacterRecorder.instance.GuideID[26] += 1;
                                SendGuide();
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                    }
                }
            }
            #endregion
            #region 装备升级 7-2后 索引28
            if (CharacterRecorder.instance.GuideID[3] >= 2 && CharacterRecorder.instance.GuideID[28] < 7 && CharacterRecorder.instance.lastGateID == 10008 && GameObject.Find("FightWindow") == null && GameObject.Find("ResultWindow") == null)//CharacterRecorder.instance.GuideID[3] > 2 
            {
                CharacterRecorder.instance.NowGuideID = 28;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[28] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                bool isNewGuide = false;
                Hero hero = CharacterRecorder.instance.GetHeroByRoleID(60016);
                foreach (var _OneEquipInfo in hero.equipList)
                {
                    TextTranslator.ItemInfo mItemInfo = TextTranslator.instance.GetItemByItemCode(_OneEquipInfo.equipCode);
                    if (mItemInfo.itemGrade < 2)
                    {
                        isNewGuide = true;
                    }
                }
                if (isNewGuide)
                {
                    switch (CharacterRecorder.instance.GuideID[28])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我方的军用资金可以用来[D35818]强化[-]英雄的装备，请让我来为您介绍使用方法。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 28);
                            break;
                        case 1:
                            if (GameObject.Find("MapUiWindow") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
                                //AddChangeClick(GameObject.Find("BackButton"),, 28);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2000);
                            }
                            else
                            {
                                CharacterRecorder.instance.GuideID[28] += 1;
                                StartCoroutine(NewbieGuide());
                            }
                            break;
                        case 2:
                            if (GameObject.Find("MainWindow") != null)
                            {
                                for (int j = 0; j < CharacterRecorder.instance.ownedHeroList.size; j++)
                                {
                                    if (60016 == CharacterRecorder.instance.ownedHeroList[j].cardID)
                                    {
                                        TextTranslator.instance.HeadIndex = j;
                                        break;
                                    }
                                }
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddChangeClick(GameObject.Find("RoleButton"), 28);
                                LuaDeliver.AddGuideClick(GameObject.Find("EquipButton"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "首先打开装备界面", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(400, 0, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2001);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 3:
                            //if (GameObject.Find("Equip1") != null)
                            //{
                            //    UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1900);
                            //    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                            //    //AddChangeClick(GameObject.Find("1"), 28);
                            //    LuaDeliver.AddGuideClick(GameObject.Find("Equip1"), 0.4f);
                            //}
                            //else
                            //{
                            //    SetGuideDelayCsharp(0.1f);
                            //}
                            StrengEquipWindow.ClickIndex = 1;
                            CharacterRecorder.instance.GuideID[28] += 1;
                            StartCoroutine(NewbieGuide());
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2002);
                            break;
                        case 4:
                            if (GameObject.Find("AllUpButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddClick(GameObject.Find("OneKeyButton"), 28);
                                LuaDeliver.AddGuideClick(GameObject.Find("AllUpButton"), 0.2f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "让我们全部升级男枪的装备吧", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(-200, -300, 0));
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 5:
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
                                //AddChangeClick(GameObject.Find("BackButton"),, 28);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            break;
                        default:
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2003);
                            //if (GameObject.Find("MainWindow") != null) //kino
                            //{
                            //    UIManager.instance.OpenSinglePanel("AnnouncementWindow", false);
                            //    GameObject.Find("AnnouncementWindow").GetComponent<AnnouncementWindow>().SetTexture("Announcement3");
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 1, "", "");
                            CharacterRecorder.instance.GuideID[28] += 1;
                            SendGuide();
                            //}
                            //else
                            //{
                            //    SetGuideDelayCsharp(0.1f);
                            //}
                            break;
                    }
                }
            }
            #endregion
            #region 第八关 升星晋升 37
            if (CharacterRecorder.instance.GuideID[37] < 8 && GameObject.Find("FightWindow") == null && GameObject.Find("ResultWindow") == null && CharacterRecorder.instance.GuideID[15] > 3 && CharacterRecorder.instance.lastGateID == 10009)
            {
                CharacterRecorder.instance.NowGuideID = 37;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[37] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                bool isNewGuide = false;
                Hero hero = CharacterRecorder.instance.GetHeroByRoleID(60016);
                if (hero.rank < 2)
                {
                    isNewGuide = true;
                }
                if (isNewGuide)
                {
                    switch (CharacterRecorder.instance.GuideID[37])
                    {
                        case 0:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，恭喜您得到了宝贵的战功！英雄积累战功就可以[d35818]升星[-]，极大增强战斗力！请让我来为您介绍使用方法。", 1, "Lili", "卡洁西");
                            ////AddGuideID(GameObject.Find("BigGuide"), 37);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2200);
                            break;
                        case 1:
                            if (GameObject.Find("MapUiWindow") != null)
                            {
                                if (GameObject.Find("BackButton") != null)
                                {
                                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                    //////AddChangeClick(GameObject.Find("BackButton"),, 37);
                                    LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.4f);
                                }
                                else
                                {
                                    SetGuideDelayCsharp(0.1f);
                                }
                            }
                            else
                            {
                                CharacterRecorder.instance.GuideID[37] += 1;
                                StartCoroutine(NewbieGuide());
                            }
                            break;
                        case 2:
                            if (GameObject.Find("MainWindow") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddChangeClick(GameObject.Find("RoleButton"), 37);
                                LuaDeliver.AddGuideClick(GameObject.Find("RoleButton"), 0.4f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "首先打开英雄界面", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(400, 0, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2201);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 3:
                            if (GameObject.Find("SpriteTab5") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddClick(GameObject.Find("SpriteTab5"), 37);
                                LuaDeliver.AddGuideClick(GameObject.Find("SpriteTab5"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "进入升星晋升界面", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(50, 0, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2202);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 4:
                            if (GameObject.Find("UpButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                ////AddClick(GameObject.Find("UpButton"), 37);
                                LuaDeliver.AddGuideClick(GameObject.Find("UpButton"), 0.1f);
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "让男枪晋升到二星吧，这样会极大增强他的战斗力", 1, "Lili", "杰西卡");
                                LuaDeliver.AddLabelClick(new Vector3(520, -300, 0));
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2203);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        default:
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2204);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2205);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 1, "", "");
                            CharacterRecorder.instance.GuideID[37] += 1;
                            SendGuide();
                            break;
                    }
                }
            }
            #endregion
            #region 精英本新手引导 索引25,27,33
            if (CharacterRecorder.instance.GuideID[25] < 4 && CharacterRecorder.instance.lastGateID == 10010 && NowGateID == 10010 && PictureCreater.instance.IsResult && PictureCreater.instance.FightStyle == 0)
            {
                CharacterRecorder.instance.NowGuideID = 25;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[25] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[25])
                {
                    case 0:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，在之前的据点发现了黑锋军团的精锐部队，挑战他们可以获得丰厚的报酬，请让我来为您引路！", 1, "Lili", "杰西卡");
                        //AddGuideID(GameObject.Find("BigGuide"), 25);

                        break;
                    case 1:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2403);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        NetworkHandler.instance.SendProcess(TempSendString);
                        CharacterRecorder.instance.GuideID[25] += 1;
                        StartCoroutine(NewbieGuide());
                        break;
                    case 2:
                        if (GameObject.Find("BackButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //////AddChangeClick(GameObject.Find("BackButton"),, 25);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton").gameObject, 0.1f);

                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2404);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[25] += 1;
                        SendGuide();
                        break;
                }
            }
            //else if (CharacterRecorder.instance.GuideID[27] < 5 && CharacterRecorder.instance.lastGateID == 10011 && CharacterRecorder.instance.GuideID[25] > 2)
            //{
            //    CharacterRecorder.instance.NowGuideID = 27;
            //    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[27] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
            //    switch (CharacterRecorder.instance.GuideID[27])
            //    {
            //        case 0:
            //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //            GameObject.Find("GuideButtonPoint").transform.localPosition = new Vector3(-403, 4, 0);
            //            LuaDeliver.AddArrowClick(new Vector3(-403, 4, 0), 27);

            //            break;
            //        case 1:

            //            if (GameObject.Find("Tab2Button") != null)
            //            {
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                ////AddClick(GameObject.Find("Tab2Button"), 27);
            //                LuaDeliver.AddGuideClick(GameObject.Find("Tab2Button"), 0.4f);
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2500);
            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            break;
            //        case 2:
            //            if (GameObject.Find("StartButton") == null)
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            else
            //            {
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                ////AddChangeClick(GameObject.Find("StartButton"), 27);
            //                LuaDeliver.AddGuideClick(GameObject.Find("StartButton"), 0.4f);
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2501);
            //            }
            //            break;
            //        case 3:
            //            if (GameObject.Find("LoadingWindow") == null)
            //            {
            //                if (GameObject.Find("AutoButton") == null)
            //                {
            //                    SetGuideDelayCsharp(0.1f);
            //                }
            //                else
            //                {
            //                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                    // //AddChangeClick(GameObject.Find("AutoButton"), 27);
            //                    LuaDeliver.AddGuideClick(GameObject.Find("AutoButton"), 0.4f);
            //                }
            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            break;
            //        default:
            //            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2503);
            //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
            //            CharacterRecorder.instance.GuideID[27] += 1;
            //            SendGuide();
            //            break;
            //    }
            //}
            //else if (CharacterRecorder.instance.GuideID[33] < 2 && CharacterRecorder.instance.GuideID[27] > 3 && CharacterRecorder.instance.lastGateID == 10011 && GameObject.Find("FightWindow") == null)
            //{
            //    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[33] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
            //    switch (CharacterRecorder.instance.GuideID[33])
            //    {
            //        case 0:
            //            if (GameObject.Find("SpriteBox") == null)
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            else
            //            {
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                //AddChangeClick(GameObject.Find("SpriteBox"), 33);
            //                LuaDeliver.AddGuideClick(GameObject.Find("SpriteBox"), 0.4f);
            //            }
            //            break;
            //        case 1:
            //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
            //            CharacterRecorder.instance.GuideID[33] += 1;
            //            SendGuide();
            //            break;
            //    }
            //}
            #endregion
            #region 竞技场引导索引21   11关后
            else if (CharacterRecorder.instance.GuideID[21] < 10 && CharacterRecorder.instance.lastGateID == 10012)
            {
                CharacterRecorder.instance.NowGuideID = 21;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[21] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[21])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，通往[D35818]竞技场[-]的道路已经修复，请让我来为您介绍参加规则吧。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 21);

                        }
                        break;
                    case 1:
                        if (GameObject.Find("TaskWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //////AddChangeClick(GameObject.Find("BackButton"),, 21);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[21] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 21);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[21] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 21);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[21] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 21);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2700);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[21] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 21);
                            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2701);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 6:
                        GameObject challenge = GameObject.Find("ChallengeWindow");
                        if (challenge != null)
                        {
                            challenge.GetComponent<ChallengeWindow>().ItemC[0].GetComponent<UIToggle>().value = true;
                        }
                        if (GameObject.Find("C1") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("C1"), 21);
                            LuaDeliver.AddGuideClick(GameObject.Find("C1"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2702);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("PVPWindow") != null)
                        {
                            GameObject.Find("PVPWindow/ALL/Scroll View").GetComponent<UIScrollView>().enabled = false;//第一次新手pvp防止滑动
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "您可以在这里自由挑战其他的战队，每天会根据排名发放奖励，每天挑战多次还可以[ff8c04]获得更多奖励[-]。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 21);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2703);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 8:
                        if (GameObject.Find("PVPWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            GameObject.Find("GuideButtonPoint").transform.localPosition = new Vector3(-380, -230, 0);
                            LuaDeliver.AddArrowClick(new Vector3(-380, -230, 0), 0);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2704);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[21] += 1;
                        SendGuide();
                        break;
                }
            }
            #endregion

            #region  战术  21关回到大地图
            if (CharacterRecorder.instance.lastGateID == 10021 && NowGateID == 10020 && CharacterRecorder.instance.GuideID[29] < 8)
            {
                CharacterRecorder.instance.NowGuideID = 29;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[29] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[29])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，您已经解锁[D35818]第二个战术技能栏位[-]，请及时进行配置。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 29);

                        }
                        break;
                    case 1:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 29);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[29] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 29);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[29] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 29);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2900);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[29] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("TaskWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 29);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[29] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("TacticsButton"), 29);
                            LuaDeliver.AddGuideClick(GameObject.Find("TacticsButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2901);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 6:
                        if (GameObject.Find("TacticsWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(7, "拖动[D35818]技能[-]上阵。", 1, "Lili", "杰西卡");
                            LuaDeliver.AddLabelClick(new Vector3(200, 125, 0));
                            LuaDeliver.AddArrowMove(new Vector3(-100, 120, 0), new Vector3(10, -330, 0));
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2902);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2903);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[29] += 1;
                        SendGuide();
                        break;

                }
            }
            #endregion
            #region  索引20 任务 13关过后
            //else if (CharacterRecorder.instance.GuideID[20] < 7 && CharacterRecorder.instance.lastGateID == 10014 && NowGateID == 10013)  //kino
            //{
            //    CharacterRecorder.instance.NowGuideID = 20;
            //    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[20] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
            //    switch (CharacterRecorder.instance.GuideID[20])
            //    {
            //        case 0:
            //            if (GameObject.Find("FightWindow") != null)
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            else
            //            {
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，最新的[D35818]日常委托[-]已经打印出来了，请跟随我来确认一下内容吧。", 1, "Lili", "杰西卡");
            //                //AddGuideID(GameObject.Find("BigGuide"), 20);
            //            }
            //            break;
            //        case 1:
            //            if (GameObject.Find("ResultWindow") != null)
            //            {
            //                if (GameObject.Find("BackButton") != null)
            //                {
            //                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                    ////AddChangeClick(GameObject.Find("BackButton"),, 20);
            //                    LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
            //                }
            //                else
            //                {
            //                    SetGuideDelayCsharp(0.1f);
            //                }
            //            }
            //            else
            //            {
            //                CharacterRecorder.instance.GuideID[20] += 1;
            //                StartCoroutine(NewbieGuide());
            //            }
            //            break;
            //        case 2:
            //            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2000);
            //            if (GameObject.Find("SweptWindow") != null)
            //            {
            //                if (GameObject.Find("SweptComfineButton") != null)
            //                {
            //                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                    // //AddChangeClick(GameObject.Find("SweptComfineButton"), 20);
            //                    LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
            //                }
            //                else
            //                {
            //                    SetGuideDelayCsharp(0.1f);
            //                }
            //            }
            //            else
            //            {
            //                CharacterRecorder.instance.GuideID[20] += 1;
            //                StartCoroutine(NewbieGuide());
            //            }
            //            break;
            //        case 3:
            //            if (GameObject.Find("MapUiWindow") != null)
            //            {
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2001);
            //                if (GameObject.Find("BackButton") != null)
            //                {
            //                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                    ////AddChangeClick(GameObject.Find("BackButton"),, 20);
            //                    LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
            //                }
            //                else
            //                {
            //                    SetGuideDelayCsharp(0.1f);
            //                }
            //            }
            //            else
            //            {
            //                CharacterRecorder.instance.GuideID[20] += 1;
            //                StartCoroutine(NewbieGuide());
            //            }
            //            break;
            //        case 4:
            //            if (GameObject.Find("MainWindow") != null)
            //            {
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2002);
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                //AddChangeClick(GameObject.Find("TaskButton"), 20);
            //                LuaDeliver.AddGuideClick(GameObject.Find("TaskButton"), 0.4f);
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "首先打开任务界面", 1, "Lili", "杰西卡");
            //                LuaDeliver.AddLabelClick(new Vector3(0, -300, 0));

            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            break;
            //        case 5:
            //            if (GameObject.Find("1") != null)
            //            {
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2003);
            //                GameObject TaskObj = GameObject.Find("1");
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
            //                //AddClick(TaskObj, 20);
            //                LuaDeliver.AddGuideClick(TaskObj, 0.1f);
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(5, "登录任务已经可以完成了呢", 1, "Lili", "杰西卡");
            //                LuaDeliver.AddLabelClick(new Vector3(0, -300, 0));
            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            break;
            //        default:
            //            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2004);
            //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
            //            CharacterRecorder.instance.GuideID[20] += 1;
            //            SendGuide();
            //            break;
            //    }
            //}
            #endregion
            #region 赏金猎人 通关16
            else if (CharacterRecorder.instance.lastGateID == 10017 && CharacterRecorder.instance.GuideID[9] < 12)
            {
                CharacterRecorder.instance.NowGuideID = 9;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[9] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[9])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") == null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官！我军[D35818]金库[-]日渐消耗，需要补充金币，正好我发现一个途径可以快速获取金币。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 9);

                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 1:
                        if (GameObject.Find("TaskWindow"))
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[9] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 9);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[9] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[9] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2800);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[9] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2801);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 6:
                        GameObject challenge1 = GameObject.Find("ChallengeWindow");
                        if (challenge1 != null)
                        {
                            challenge1.GetComponent<ChallengeWindow>().ItemC[1].GetComponent<UIToggle>().value = true;
                        }
                        if (GameObject.Find("C2") != null)
                        {
                            //challenge1.GetComponent<ChallengeWindow>().GetCenterNum(2);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("C2"), 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("C2"), 0.4f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2802);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("BountyHunter") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddClick(GameObject.Find("BountyHunter"), 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("BountyHunter"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2803);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 8:
                        if (GameObject.Find("JoinButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddClick(GameObject.Find("JoinButton"), 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("JoinButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2804);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 9:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "根据您的战队等级，可以挑战的难度也不一样，当然[D35818]难度越高奖励越好[-]。", 1, "Lili", "杰西卡");//斯内格
                        //AddGuideID(GameObject.Find("BigGuide"), 9);
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2805);
                        break;
                    case 10:
                        if (GameObject.Find("Easy") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("Easy"), 9);
                            LuaDeliver.AddGuideClick(GameObject.Find("Easy"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2806);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2807);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[9] += 1;
                        SendGuide();
                        break;

                }
            }
            #endregion
            #region 夺宝 通关22关 索引10
            else if (CharacterRecorder.instance.GuideID[10] < 16 && CharacterRecorder.instance.lastGateID == 10023)
            {
                CharacterRecorder.instance.NowGuideID = 10;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[10] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                if (TextTranslator.instance.GetItemByID(51030) != null && CharacterRecorder.instance.GuideID[10] == 0)
                {
                    CharacterRecorder.instance.GuideID[10] = 15;
                }
                switch (CharacterRecorder.instance.GuideID[10])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我军可以通过[D35818]夺取[-]其他战队的勋章碎片组合成勋章[D35818]提升战斗力[-]，请让我来为您介绍获取方法。", 1, "Lili", "杰西卡");
                        }

                        break;
                    case 1:
                        if (GameObject.Find("TaskWindow"))
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[10] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 10);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[10] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[10] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3000);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[10] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3001);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 6:
                        GameObject challenge2 = GameObject.Find("ChallengeWindow");
                        if (challenge2 != null)
                        {
                            challenge2.GetComponent<ChallengeWindow>().ItemC[2].GetComponent<UIToggle>().value = true;
                        }
                        if (GameObject.Find("C3") != null)
                        {
                            //challenge2.GetComponent<ChallengeWindow>().GetCenterNum(3);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("C3"), 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("C3"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3002);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("GrabItemWindow") != null)
                        {
                            if (TextTranslator.instance.GetItemByID(81030) != null)
                            {
                                CharacterRecorder.instance.GuideID[10] = 12;
                                StartCoroutine(NewbieGuide());
                            }
                            else
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "正好我这里已经有两个碎片了，所以只需要再夺取一个碎片就可以进行合成，请点击空格开始抢夺。", 1, "Lili", "杰西卡");//斯内格
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3003);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 8:
                        if (GameObject.Find("ItemTwo") != null)
                        {
                            GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().SetInfo(GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().NewBieObJ.GetComponent<GrabItem>().ItemInfo);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddClick(GameObject.Find("ItemTwo"), 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("ItemTwo"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3004);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 9:
                        if (GameObject.Find("RobberyHeroList") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            GameObject.Find("GuideButtonPoint").transform.localPosition = new Vector3(300, -110, 0);
                            LuaDeliver.AddArrowClick(new Vector3(300, -110, 0), 0);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3005);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 10:
                        if (GameObject.Find("LoadingWindow") == null)
                        {
                            if (GameObject.Find("AutoButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("AutoButton"), 10);
                                LuaDeliver.AddGuideClick(GameObject.Find("AutoButton").gameObject, 0.4f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3006);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 11:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        if (GameObject.Find("GrabFinishWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 10);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton").gameObject, 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3008);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }

                        break;
                    case 12:
                        if (GameObject.Find("GrabItemWindow") != null)
                        {
                            //GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().SetInfo(GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().NewBieObJ.GetComponent<GrabItem>().ItemInfo);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            LuaDeliver.AddGuideClick(GameObject.Find("SynthesisButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3009);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 13:

                        if (GameObject.Find("ItemMessage") != null && GameObject.Find("GotoButton") != null)
                        {
                            GameObject.Find("ItemMessage").GetComponent<UIPanel>().depth = 18;
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            LuaDeliver.AddGuideClick(GameObject.Find("GotoButton").gameObject, 0.4f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3010);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 14:
                        if (GameObject.Find("AwakeButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
                            //AddChangeClick(GameObject.Find("AwakeButton"), 32);
                            LuaDeliver.AddGuideClick(GameObject.Find("AwakeButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3011);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3012);
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3013);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[10] += 1;
                        SendGuide();
                        break;


                }
            }
            #endregion
            #region 情报 通关26 索引11
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.qingbao).Level && CharacterRecorder.instance.GuideID[11] < 9)
            {
                CharacterRecorder.instance.NowGuideID = 11;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[11] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[11])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我军的[D35818]情报中心[-]已经升级完毕，现在可以通过投入情报点来强化我军英雄。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 11);

                        }
                        break;
                    case 1:
                        if (GameObject.Find("TaskWindow"))
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 11);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[11] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 11);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[11] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 11);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[11] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 11);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3100);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[11] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("TechButton"), 11);
                            LuaDeliver.AddGuideClick(GameObject.Find("TechButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3101);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 6:
                        if (GameObject.Find("100") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddClick(GameObject.Find("100"), 11);
                            LuaDeliver.AddGuideClick(GameObject.Find("100"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3102);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("UpButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("UpButton"), 11);
                            LuaDeliver.AddGuideClick(GameObject.Find("UpButton"), 0.4f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3103);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3104);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[11] += 1;
                        SendGuide();
                        break;

                }
            }
            #endregion
            #region 角色培养 通关30关 索引30
            if (CharacterRecorder.instance.lastGateID == 10031 && CharacterRecorder.instance.GuideID[30] < 10)
            {
                CharacterRecorder.instance.NowGuideID = 30;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[30] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[30])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我军现缴获一批敌军专用的[D35818]强化药剂[-]，似乎也可以对我方英雄使用，赶快去试试吧。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 30);

                        }
                        break;
                    case 1:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 30);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[30] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 30);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[30] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 30);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3200);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[30] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("TaskWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 30);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[30] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("RoleButton"), 30);
                            LuaDeliver.AddGuideClick(GameObject.Find("RoleButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3201);
                        }
                        else
                        {
                            //CharacterRecorder.instance.GuideID[30] += 1;
                            //StartCoroutine(NewbieGuide());
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 6:
                        if (GameObject.Find("SpriteTab3") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                            //AddClick(GameObject.Find("SpriteTab3"), 30);
                            LuaDeliver.AddGuideClick(GameObject.Find("SpriteTab3"), 0.5f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3202);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("TrainButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                            //AddChangeClick(GameObject.Find("TrainButton"), 30);
                            LuaDeliver.AddGuideClick(GameObject.Find("TrainButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3203);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 8:
                        if (GameObject.Find("ReplaceButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                            //AddChangeClick(GameObject.Find("TrainButton"), 30);
                            LuaDeliver.AddGuideClick(GameObject.Find("ReplaceButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3204);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 1, "", "");
                        CharacterRecorder.instance.GuideID[30] += 1;
                        SendGuide();
                        break;
                }
            }
            #endregion
            #region 敢死队 通关65关 索引12
            else if (CharacterRecorder.instance.lastGateID == 10058 && CharacterRecorder.instance.GuideID[12] < 13)
            {
                CharacterRecorder.instance.NowGuideID = 12;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[12] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[12])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {                            
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，根据地图显示，前方[D35818]丛林地带[-]曾经是混战的战场，传说还留存着很多宝藏，以及一位传奇英雄在看守！\n我已经按捺不住要去[D35818]探险[-]了！", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 12);

                        }
                        break;
                    case 1:
                        if (GameObject.Find("TaskWindow"))
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 12);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[12] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 12);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[12] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 12);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[12] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 12);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3600);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[12] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:

                        if (GameObject.Find("MainWindow") != null)
                        {
                            //UIManager.instance.NewGuideAnchor("280");
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 12);
                            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3601);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 6:

                        GameObject cw = GameObject.Find("ChallengeWindow");
                        if (cw != null)
                        {
                            cw.GetComponent<ChallengeWindow>().ItemC[4].GetComponent<UIToggle>().value = true;
                        }

                        if (GameObject.Find("C5") != null)
                        {
                            //cw.GetComponent<ChallengeWindow>().GetCenterNum(5);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("C2"), 12);
                            LuaDeliver.AddGuideClick(GameObject.Find("C5"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3602);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("WoodsTheExpendables") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "看来参观者不少！我们要击败他们继续前进！", 1, "glfs", "男枪");
                            //AddGuideID(GameObject.Find("BigGuide"), 12);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3603);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 8:

                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "副本会[D35818]每天自动重置[-]哦，不过会[D35818]保留前一天的一部分进度[-]。！", 2, "glfs", "男枪");
                        //AddGuideID(GameObject.Find("BigGuide"), 12);
                        break;
                    case 9:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3604);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                        GameObject.Find("GuideButtonPoint").transform.localPosition = new Vector3(0, 130, 0);
                        LuaDeliver.AddArrowClick(new Vector3(0, 130, 0), 0);

                        break;
                    case 10:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        if (GameObject.Find("FightWindow") != null)
                        {
                            if (GameObject.Find("Right") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddChangeClick(GameObject.Find("Right").transform.Find("Button").gameObject, 12);
                                LuaDeliver.AddGuideClick(GameObject.Find("Right").transform.Find("Button").gameObject, 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3605);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 11:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3606);
                        if (GameObject.Find("LoadingWindow") == null)
                        {
                            if (GameObject.Find("FightWindow") != null && GameObject.Find("WoodsTheExpendablesMapList") == null)
                            {
                                UIManager.instance.OpenSinglePanel("AnnouncementWindow", false);
                                GameObject.Find("AnnouncementWindow").GetComponent<AnnouncementWindow>().SetTexture("Announcement2");
                                if (GameObject.Find("AnnouncementWindow") != null)
                                {
                                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                    //AddChangeClick(GameObject.Find("KnowButton"), 12);
                                    LuaDeliver.AddGuideClick(GameObject.Find("KnowButton"), 0.1f);
                                    GameObject.Find("NewGuideWindow/MinGuide/GuideButtonPoint").GetComponent<UISprite>().alpha = 0.1f;

                                }
                                else
                                {
                                    SetGuideDelayCsharp(0.1f);
                                }
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3608);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[12] += 1;
                        SendGuide();
                        break;


                }
            }
            #endregion

            #region 组队副本 通关48关 索引38
            else if (CharacterRecorder.instance.lastGateID == 10049 && CharacterRecorder.instance.GuideID[38] < 17)
            {
                CharacterRecorder.instance.NowGuideID = 38;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[38] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                if (CharacterRecorder.instance.isHadFriteStone && CharacterRecorder.instance.GuideID[38] < 9)
                {
                    CharacterRecorder.instance.GuideID[38] = 99;
                    SendGuide();
                }
                switch (CharacterRecorder.instance.GuideID[38])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，跟踪劳拉的密探回来了，我们发现一种可以用来强化士兵的[d35818]秘宝[-]，我现在就告诉你位置。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 38);

                        }
                        break;
                    case 1:
                        if (GameObject.Find("TaskWindow"))
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 38);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[38] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 38);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[38] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 38);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[38] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3700);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 38);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[38] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3701);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 38);
                            LuaDeliver.AddGuideClick(GameObject.Find("EquipButton"), 0.1f);
                            CharacterRecorder.instance.setEquipTableIndex = 3;
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 6:
                        if (GameObject.Find("StrengEquipWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3702);
                            if (GameObject.Find("40101") != null)
                            {
                                GameObject go = GameObject.Find("40101");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                // //AddChangeClick(GameObject.Find("SweptComfineButton") 38);
                                LuaDeliver.AddGuideClick(go, 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("MoveOnButton") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3703);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 38);
                            LuaDeliver.AddGuideClick(GameObject.Find("MoveOnButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 8:
                        if (GameObject.Find("StrengEquipWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3704);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "英雄装备[ff0000]秘宝[-]可以大幅提升战斗力，我这就告诉你劳拉常去的“摸金”点。", 1, "Lili", "杰西卡");
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 9:
                        if (GameObject.Find("BackButton") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3705);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 10:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3706);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 11:
                        GameObject challenge3 = GameObject.Find("ChallengeWindow");
                        if (challenge3 != null)
                        {
                            challenge3.GetComponent<ChallengeWindow>().ItemC[3].GetComponent<UIToggle>().value = true;
                        }
                        if (GameObject.Find("C4") != null)
                        {
                            //GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>().GetCenterNum(4);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("C5"), 38);
                            LuaDeliver.AddGuideClick(GameObject.Find("C4"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3502);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 12:
                        if (GameObject.Find("TeamCopyWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "高稀有度的[ff0000]秘宝[-]非常难以获得，建议与其他指挥官组队探索", 1, "Lili", "杰西卡");
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 13:
                        if (GameObject.Find("CreateButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            LuaDeliver.AddGuideClick(GameObject.Find("CreateButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 14:
                        if (GameObject.Find("CreateTeamWindow/All/Message/CreateButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            LuaDeliver.AddGuideClick(GameObject.Find("CreateTeamWindow/All/Message/CreateButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 15:
                        if (GameObject.Find("TeamInvitationWindow") != null)
                        {
                            if (GameObject.Find("AutoStartButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                LuaDeliver.AddGuideClick(GameObject.Find("AutoStartButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3503);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[38] += 1;
                        SendGuide();
                        break;


                }
            }
            #endregion
            #region  军团 57关
            #endregion
            #region 27级 通关72关 索引13
            else if (CharacterRecorder.instance.lastGateID == 10073 && CharacterRecorder.instance.GuideID[13] < 12)
            {
                CharacterRecorder.instance.NowGuideID = 13;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[13] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[13])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我军的固定补给已经到位，可以在那里获取[D35818]更多的演练[-]，用以英雄升级。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 13);
                        }
                        break;
                    case 1:
                        if (GameObject.Find("TaskWindow"))
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[13] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("ResultWindow") != null)
                        {
                            if (GameObject.Find("BackButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                ////AddChangeClick(GameObject.Find("BackButton"),, 13);
                                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[13] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 3:
                        if (GameObject.Find("SweptWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[13] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 4:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[13] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 5:

                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 6:
                        GameObject cw1 = GameObject.Find("ChallengeWindow");
                        if (cw1 != null)
                        {
                            cw1.GetComponent<ChallengeWindow>().ItemC[1].GetComponent<UIToggle>().value = true;
                        }
                        if (GameObject.Find("C2") != null)
                        {
                            //GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>().GetCenterNum(2);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("C2"), 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("C2"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("Thousand") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddClick(GameObject.Find("Thousand"), 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("Thousand"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 8:
                        if (GameObject.Find("JoinButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddClick(GameObject.Find("JoinButton"), 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("JoinButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 9:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "根据您的战队等级，可以挑战的难度也不一样，当然难度越高奖励越好。", 1, "Lili", "杰西卡");//斯内格
                        //AddGuideID(GameObject.Find("BigGuide"), 13);
                        break;
                    case 10:
                        if (GameObject.Find("Easy") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("Easy"), 13);
                            LuaDeliver.AddGuideClick(GameObject.Find("Easy"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    default:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[13] += 1;
                        SendGuide();
                        break;

                }
            }
            #endregion
            #region 角色Skill 通关78关  索引31号
            if (CharacterRecorder.instance.lastGateID == 10079 && CharacterRecorder.instance.GuideID[31] < 9)
            {
                CharacterRecorder.instance.NowGuideID = 31;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[31] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                GuideSkipWindow();
                //switch (CharacterRecorder.instance.GuideID[31])
                //{
                //    case 0:
                //        if (GameObject.Find("FightWindow") != null)
                //        {
                //            SetGuideDelayCsharp(0.1f);
                //        }
                //        else
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "报告" + "长官！我军在敌方仓库发现了这种[D35818]不明用途的糖水[-]，我喝了一些感觉整个人都聪明了许多！", 1, "Lili", "杰西卡");
                //            //AddGuideID(GameObject.Find("BigGuide"), 31);
                //        }
                //        break;
                //    case 1:
                //        if (GameObject.Find("ResultWindow") != null)
                //        {
                //            if (GameObject.Find("BackButton") != null)
                //            {
                //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //                ////AddChangeClick(GameObject.Find("BackButton"),, 31);
                //                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //            }
                //            else
                //            {
                //                SetGuideDelayCsharp(0.1f);
                //            }
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[31] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 2:
                //        if (GameObject.Find("SweptWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 31);
                //            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[31] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 3:
                //        if (GameObject.Find("MapUiWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            ////AddChangeClick(GameObject.Find("BackButton"),, 31);
                //            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[31] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 4:
                //        if (GameObject.Find("TaskWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            ////AddChangeClick(GameObject.Find("BackButton"),, 31);
                //            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[31] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 5:
                //        if (GameObject.Find("MainWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            ////AddChangeClick(GameObject.Find("RoleButton"), 31);
                //            LuaDeliver.AddGuideClick(GameObject.Find("RoleButton"), 0.1f);
                //        }
                //        else
                //        {

                //            SetGuideDelayCsharp(0.3f);
                //        }
                //        break;
                //    case 6:
                //        if (GameObject.Find("SpriteTab6") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                //            //AddClick(GameObject.Find("SpriteTab6"), 31);
                //            LuaDeliver.AddGuideClick(GameObject.Find("SpriteTab6"), 0.1f);
                //        }
                //        else
                //        {
                //            SetGuideDelayCsharp(0.1f);
                //        }
                //        break;
                //    case 7:
                //        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1,
                //             "长官，还有一些没喝完的就给他用吧，长按“开始注射”可以快速使用。不过一定要注意！每天[D35818]24点[-]会清空能量槽，请谨慎使用。", 1, "Lili", "杰西卡");
                //        //AddGuideID(GameObject.Find("BigGuide"), 31);
                //        break;
                //    default:
                //        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                //        CharacterRecorder.instance.GuideID[31] += 1;
                //        SendGuide();
                //        break;
                //}
            }
            #endregion
            #region 花式军事 通关90关 索引14
            else if (CharacterRecorder.instance.lastGateID == 10091 && CharacterRecorder.instance.GuideID[14] < 9)
            {
                CharacterRecorder.instance.NowGuideID = 14;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[14] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                GuideSkipWindow();
                //switch (CharacterRecorder.instance.GuideID[14])
                //{
                //    case 0:
                //        if (GameObject.Find("FightWindow") != null)
                //        {
                //            SetGuideDelayCsharp(0.1f);
                //        }
                //        else
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，现在我军可以每天进行更多[d35818]演练[-]，奖励丰富，请记得每天查看。", 1, "Lili", "杰西卡");
                //            //AddGuideID(GameObject.Find("BigGuide"), 14);
                //        }
                //        break;
                //    case 1:
                //        if (GameObject.Find("TaskWindow"))
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            ////AddChangeClick(GameObject.Find("BackButton"),, 14);
                //            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[14] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 2:
                //        if (GameObject.Find("ResultWindow") != null)
                //        {
                //            if (GameObject.Find("BackButton") != null)
                //            {
                //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //                ////AddChangeClick(GameObject.Find("BackButton"),, 14);
                //                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //            }
                //            else
                //            {
                //                SetGuideDelayCsharp(0.1f);
                //            }
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[14] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 3:
                //        if (GameObject.Find("SweptWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 14);
                //            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[14] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 4:
                //        if (GameObject.Find("MapUiWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            ////AddChangeClick(GameObject.Find("BackButton"),, 14);
                //            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[14] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 5:

                //        if (GameObject.Find("MainWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            // //AddChangeClick(GameObject.Find("SweptComfineButton") 14);
                //            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                //        }
                //        else
                //        {
                //            SetGuideDelayCsharp(0.3f);
                //        }
                //        break;
                //    case 6:
                //        if (GameObject.Find("ChallengeWindow") != null)
                //        {
                //            GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>().GetCenterNum(2);
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            //AddChangeClick(GameObject.Find("C2"), 14);
                //            LuaDeliver.AddGuideClick(GameObject.Find("C2"), 0.1f);
                //        }
                //        else
                //        {
                //            SetGuideDelayCsharp(0.3f);
                //        }
                //        break;
                //    case 7:
                //        if (GameObject.Find("MilitaryExercise") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            //AddClick(GameObject.Find("MilitaryExercise"), 14);
                //            LuaDeliver.AddGuideClick(GameObject.Find("MilitaryExercise"), 0.1f);
                //        }
                //        else
                //        {
                //            SetGuideDelayCsharp(0.3f);
                //        }
                //        break;
                //    default:
                //        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                //        CharacterRecorder.instance.GuideID[14] += 1;
                //        SendGuide();
                //        break;
                //}
            }
            #endregion
            #region 装备精炼  通关53关 索引34
            else if (CharacterRecorder.instance.lastGateID == 10054 && CharacterRecorder.instance.GuideID[34] < 7 && TextTranslator.instance.GetItemCountByID(10103) > 10)
            {
                CharacterRecorder.instance.NowGuideID = 34;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[34] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[34])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我们收集到的这种能源可以用来[d35818]精炼装备[-]，请让我为您介绍使用方法。", 1, "Lili", "杰西卡");
                        }
                        break;
                    case 1:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3900);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("BackButton"),, 34);
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[34] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3901);
                            for (int j = 0; j < CharacterRecorder.instance.ownedHeroList.size; j++)
                            {
                                if (60016 == CharacterRecorder.instance.ownedHeroList[j].cardID)
                                {
                                    TextTranslator.instance.HeadIndex = j;
                                    break;
                                }
                            }
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("RoleButton"), 34);
                            LuaDeliver.AddGuideClick(GameObject.Find("EquipButton"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 3:
                        if (GameObject.Find("StrengEquipWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3902);
                            if (GameObject.Find("SpriteTab2") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                                //AddClick(GameObject.Find("SpriteTab2"), 34);
                                LuaDeliver.AddGuideClick(GameObject.Find("SpriteTab2"), 0.2f);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 4:
                        if (GameObject.Find("Equip5") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3903);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                            //AddClick(GameObject.Find("10012"), 34);
                            LuaDeliver.AddGuideClick(GameObject.Find("Equip5"), 0.2f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 5:
                        if (GameObject.Find("TakeOnesButton") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3904);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 1, "", "");
                            //AddClick(GameObject.Find("10012"), 34);
                            LuaDeliver.AddGuideClick(GameObject.Find("TakeOnesButton"), 0.2f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3905);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[34] += 1;
                        SendGuide();
                        break;

                }

            }
            #endregion
            #region 极限挑战 通关96关 索引36
            else if (CharacterRecorder.instance.lastGateID == 10097 && CharacterRecorder.instance.GuideID[36] < 9 && false)
            {
                CharacterRecorder.instance.NowGuideID = 36;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[36] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                GuideSkipWindow();
                //switch (CharacterRecorder.instance.GuideID[36])
                //{
                //    case 0:
                //        if (GameObject.Find("FightWindow") != null)
                //        {
                //            SetGuideDelayCsharp(0.1f);
                //        }
                //        else
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，现在我军可以每天进行更多[d35818]挑战[-]，奖励丰富，请记得每天查看。", 1, "Lili", "杰西卡");
                //            //AddGuideID(GameObject.Find("BigGuide"), 36);
                //        }
                //        break;
                //    case 1:
                //        if (GameObject.Find("TaskWindow"))
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            ////AddChangeClick(GameObject.Find("BackButton"),, 36);
                //            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[36] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 2:
                //        if (GameObject.Find("ResultWindow") != null)
                //        {
                //            if (GameObject.Find("BackButton") != null)
                //            {
                //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //                ////AddChangeClick(GameObject.Find("BackButton"),, 36);
                //                LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //            }
                //            else
                //            {
                //                SetGuideDelayCsharp(0.1f);
                //            }
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[36] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 3:
                //        if (GameObject.Find("SweptWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            // //AddChangeClick(GameObject.Find("SweptComfineButton"), 36);
                //            LuaDeliver.AddGuideClick(GameObject.Find("SweptComfineButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[36] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 4:
                //        if (GameObject.Find("MapUiWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            ////AddChangeClick(GameObject.Find("BackButton"),, 36);
                //            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                //        }
                //        else
                //        {
                //            CharacterRecorder.instance.GuideID[36] += 1;
                //            StartCoroutine(NewbieGuide());
                //        }
                //        break;
                //    case 5:

                //        if (GameObject.Find("MainWindow") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            // //AddChangeClick(GameObject.Find("SweptComfineButton") 36);
                //            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                //        }
                //        else
                //        {
                //            SetGuideDelayCsharp(0.3f);
                //        }
                //        break;
                //    case 6:
                //        if (GameObject.Find("ChallengeWindow") != null)
                //        {
                //            GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>().GetCenterNum(2);
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            //AddChangeClick(GameObject.Find("C2"), 36);
                //            LuaDeliver.AddGuideClick(GameObject.Find("C2"), 0.1f);
                //        }
                //        else
                //        {
                //            SetGuideDelayCsharp(0.3f);
                //        }
                //        break;
                //    case 7:
                //        if (GameObject.Find("Challenge") != null)
                //        {
                //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                //            //AddClick(GameObject.Find("Challenge"), 36);
                //            LuaDeliver.AddGuideClick(GameObject.Find("Challenge"), 0.1f);
                //        }
                //        else
                //        {
                //            SetGuideDelayCsharp(0.3f);
                //        }
                //        break;
                //    default:
                //        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                //        CharacterRecorder.instance.GuideID[36] += 1;
                //        SendGuide();
                //        break;
                //}
            }
            #endregion

            #region 征服
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zhengfu).Level && CharacterRecorder.instance.GuideID[58] < 10)
            {
                CharacterRecorder.instance.NowGuideID = 58;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[58] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[58])
                {
                    case 0:
                        if (GameObject.Find("FightWindow") != null)
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，我们在废弃的矿洞中搭建了采矿点，但需要[ff0000]征服[-]大量俘虏让我们的采集区正常工作，让我们来试试看吧。", 1, "Lili", "杰西卡");

                        }
                        break;
                    case 1:
                        if (GameObject.Find("MapUiWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_4000);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            LuaDeliver.AddGuideClick(GameObject.Find("BackButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3100);
                        }
                        else
                        {
                            CharacterRecorder.instance.GuideID[58] += 1;
                            StartCoroutine(NewbieGuide());
                        }
                        break;
                    case 2:
                        if (GameObject.Find("MainWindow") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            // //AddChangeClick(GameObject.Find("SweptComfineButton") 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("ChallengeButton"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3001);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 3:
                        GameObject challenge5 = GameObject.Find("ChallengeWindow");
                        if (challenge5 != null)
                        {
                            challenge5.GetComponent<ChallengeWindow>().ItemC[5].GetComponent<UIToggle>().value = true;
                        }
                        if (GameObject.Find("C6") != null)
                        {
                            //GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>().GetCenterNum(6);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            ////AddChangeClick(GameObject.Find("C3"), 10);
                            LuaDeliver.AddGuideClick(GameObject.Find("C6"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3002);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.3f);
                        }
                        break;
                    case 4:
                        if (GameObject.Find("Gate1") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_4002);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            LuaDeliver.AddGuideClick(GameObject.Find("Gate1"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3102);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 5:
                        if (GameObject.Find("HarvestWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_4003);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，选择一个[ff0000]英雄[-]为建筑带来[ff0000]收益加成[-]", 1, "Lili", "杰西卡");
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 6:
                        if (GameObject.Find("Pit1") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_4004);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddClick(GameObject.Find("100"), 11);
                            LuaDeliver.AddGuideClick(GameObject.Find("Pit1"), 0.1f);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 7:
                        if (GameObject.Find("CheckSelfHeroWindow") != null)
                        {
                            foreach (Component c in GameObject.Find("NewGuideWindow").GetComponentsInChildren(typeof(Transform), true))
                            {
                                c.gameObject.layer = 11;
                            }
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_4005);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddClick(GameObject.Find("100"), 11);
                            GameObject.Find("GuideButtonPoint").transform.localPosition = new Vector3(-80, 100, 0);
                            LuaDeliver.AddArrowClick(new Vector3(-80, 100, 0), 0);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 8:
                        if (GameObject.Find("HarvestWindow") != null)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_4006);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "所添加的英雄/好友/俘虏[ff0000]战力越高[-]所获得的[ff0000]收益越多[-]，当然升级建筑也是提升收益的方式。接下来您自己研究一下吧！", 1, "Lili", "杰西卡");
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    default:
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_4007);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[58] += 1;
                        SendGuide();
                        break;

                }
            }
            #endregion
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zhihuiSkill_3).Level && CharacterRecorder.instance.GuideID[40] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 40;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.jijiuwuzi).Level && CharacterRecorder.instance.GuideID[41] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 41;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zhanshudaodan).Level && CharacterRecorder.instance.GuideID[42] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 42;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.wangzhezhilu).Level && CharacterRecorder.instance.GuideID[43] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 43;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.jinghua).Level && CharacterRecorder.instance.GuideID[44] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 44;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.shiyanshi).Level && CharacterRecorder.instance.GuideID[45] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 45;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.qushan).Level && CharacterRecorder.instance.GuideID[46] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 46;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.bianjingzousi).Level && CharacterRecorder.instance.GuideID[47] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 47;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.luzhang).Level && CharacterRecorder.instance.GuideID[48] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 48;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.jinjizhiliao).Level && CharacterRecorder.instance.GuideID[49] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 49;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.kongjiangbing).Level && CharacterRecorder.instance.GuideID[50] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 50;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.wudihudun).Level && CharacterRecorder.instance.GuideID[51] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 51;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.juntuan).Level && CharacterRecorder.instance.GuideID[52] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 52;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zhengbaodan).Level && CharacterRecorder.instance.GuideID[53] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 53;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zhuangbeijinglian).Level && CharacterRecorder.instance.GuideID[54] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 54;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zuojia).Level && CharacterRecorder.instance.GuideID[56] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 56;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.jingyingguangka).Level && CharacterRecorder.instance.GuideID[27] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 27;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.chongsheng).Level && CharacterRecorder.instance.GuideID[60] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 60;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.guozhan).Level && CharacterRecorder.instance.GuideID[61] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 61;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.hewuqi).Level && CharacterRecorder.instance.GuideID[62] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 62;
                GuideSkipWindow();
            }
            else if (CharacterRecorder.instance.lastGateID - 1 == TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.hedianzhan).Level && CharacterRecorder.instance.GuideID[63] == 0)
            {
                CharacterRecorder.instance.NowGuideID = 63;
                GuideSkipWindow();
            }

            //首冲送劳拉
            else if (CharacterRecorder.instance.lastGateID == 10022 && CharacterRecorder.instance.GuideID[57] == 0 && CharacterRecorder.instance.GetHeroByRoleID(60024) == null)
            {
                if (GameObject.Find("MapUiWindow") != null)
                {
                    Debug.LogError("首冲送劳拉");
                    UIManager.instance.OpenSinglePanel("FirstRechargeWindow", false);
                    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 1, "", "");
                    CharacterRecorder.instance.GuideID[57] = 99;
                    UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3803);
                    SendGuide();
                }
                else
                {
                    SetGuideDelayCsharp(0.1f);
                }
            }

            #region --夺宝后神器晋升 索引32
            //if (CharacterRecorder.instance.GuideID[32] < 7 && CharacterRecorder.instance.GuideID[10] > 16 && TextTranslator.instance.GetItemByID(51030) != null)
            //{
            //    CharacterRecorder.instance.NowGuideID = 32;
            //    Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[32] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
            //    switch (CharacterRecorder.instance.GuideID[32])
            //    {
            //        case 0:
            //            if (GameObject.Find("MainWindow") != null)
            //            {
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1,  "接下来让我们把这个饰品给男枪装备上吧。", 1, "Lili", "杰西卡");
            //                //AddGuideID(GameObject.Find("BigGuide"), 32);
            //            }
            //            break;
            //        case 1:
            //            if (GameObject.Find("RoleButton") != null)
            //            {
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2500);
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
            //                //AddChangeClick(GameObject.Find("RoleButton"), 32);
            //                LuaDeliver.AddGuideClick(GameObject.Find("RoleButton"), 0.1f);
            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            break;
            //        case 2:
            //            //if (GameObject.Find("HeroItem60016") != null)
            //            //{
            //            //    GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
            //            //    //AddChangeClick(GameObject.Find("HeroItem60016"), 32);
            //            //    LuaDeliver.AddGuideClick(GameObject.Find("HeroItem60016"), 0.4f);
            //            //}
            //            //else
            //            //{
            //            //    SetGuideDelayCsharp(0.1f);
            //            //}
            //            CharacterRecorder.instance.GuideID[32] += 1;
            //            StartCoroutine(NewbieGuide());
            //            break;
            //        case 3:
            //            if (GameObject.Find("5") != null)
            //            {
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2501);
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
            //                //AddChangeClick(GameObject.Find("5"), 32);
            //                LuaDeliver.AddGuideClick(GameObject.Find("5"), 0.1f);
            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            break;
            //        case 4:
            //            if (GameObject.Find("StrengEquipWindow") != null)
            //            {
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2502);
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
            //                //AddClick(GameObject.Find("SpriteTab3"), 32);
            //                LuaDeliver.AddGuideClick(GameObject.Find("SpriteTab3"), 0.1f);
            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }
            //            break;
            //        case 5:
            //            if (GameObject.Find("AwakeButton") != null)
            //            {
            //                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2504);
            //                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "", 0, "", "");
            //                //AddChangeClick(GameObject.Find("AwakeButton"), 32);
            //                LuaDeliver.AddGuideClick(GameObject.Find("AwakeButton"), 0.1f);

            //            }
            //            else
            //            {
            //                SetGuideDelayCsharp(0.1f);
            //            }

            //            break;
            //        default:
            //            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2505);
            //            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 0, "", "");
            //            CharacterRecorder.instance.GuideID[32] += 1;
            //            SendGuide();
            //            break;
            //    }

            //}
            #endregion

            #region 世界事件和资源  索引 17-19  34关  39关
            if (CharacterRecorder.instance.GuideID[18] < 6 && GameObject.Find("MapUiWindow") != null && CharacterRecorder.instance.lastGateID == 10034)
            {
                CharacterRecorder.instance.NowGuideID = 18;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[18] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[18])
                {
                    case 0:
                        GameObject.Find("WordButton").transform.Find("SpriteSuo").gameObject.SetActive(false);
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "哥们！您的BP机响了！", 1, "ST", "圣诞");// "报告" + 
                        Resources.UnloadUnusedAssets();
                        System.GC.Collect();
                        //AddGuideID(GameObject.Find("BigGuide"), 18);

                        break;
                    case 1:
                        if (GameObject.Find("WordButton") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddClick(GameObject.Find("WordButton"), 18);
                            LuaDeliver.AddGuideClick(GameObject.Find("WordButton").gameObject, 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3300);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 2:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "这里是委托中心发来的实时[D35818]委托任务[-]，有[D35818]完成时间的限制[-]。每隔3小时会自动刷新一次，请记得即时完成。", 1, "Lili", "杰西卡");//巴洛克
                        //AddGuideID(GameObject.Find("BigGuide"), 18);
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3301);
                        break;
                    case 3:
                        if (GameObject.Find("WorldEvent_Item0") != null)
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddClick(GameObject.Find("WorldEvent_Item0"), 18);
                            LuaDeliver.AddGuideClick(GameObject.Find("WorldEvent_Item0"), 0.1f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3302);
                        }
                        else
                        {
                            SetGuideDelayCsharp(0.1f);
                        }
                        break;
                    case 4:
                        if (GameObject.Find("StartBtn") == null)
                        {

                            SetGuideDelayCsharp(0.1f);
                        }
                        else
                        {
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                            //AddChangeClick(GameObject.Find("StartBtn"), 18);
                            LuaDeliver.AddGuideClick(GameObject.Find("StartBtn"), 0.4f);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3303);
                        }
                        break;
                    default:
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                        CharacterRecorder.instance.GuideID[18] += 1;
                        SendGuide();
                        break;
                }
            }
            if (CharacterRecorder.instance.GuideID[19] < 4 && CharacterRecorder.instance.lastGateID == 10039 && NowGateID == 10038 && GameObject.Find("MapObject"))
            {
                CharacterRecorder.instance.NowGuideID = 19;
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[19] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                //GameObject Map = GameObject.Find("MapObject");
                //if (Map.transform.Find("MapCon").GetComponent<MapWindow>().getreslist.Count == 0)
                {
                    switch (CharacterRecorder.instance.GuideID[19])
                    {
                        case 0:
                            GameObject.Find("ResourceButton").transform.Find("SpriteSuo").gameObject.SetActive(false);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "长官，侦测发现前方有一处资源点被贼匪占领，消灭他们就可以由我军接管资源点！", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 19);

                            break;
                        case 1:
                            if (GameObject.Find("ResourceButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddClick(GameObject.Find("ResourceButton"), 19);
                                LuaDeliver.AddGuideClick(GameObject.Find("ResourceButton").gameObject, 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3400);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 2:
                            if (GameObject.Find("Resource_Item0") == null)
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            else
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddClick(GameObject.Find("Resource_Item0"), 19);
                                LuaDeliver.AddGuideClick(GameObject.Find("Resource_Item0"), 0.4f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3401);
                            }
                            break;
                        default:
                            if (GameObject.Find("FightButton") == null)
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            else
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddChangeClick(GameObject.Find("FightButton"), 19);
                                LuaDeliver.AddGuideClick(GameObject.Find("FightButton"), 0.4f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3402);
                                CharacterRecorder.instance.GuideID[19] += 1;
                                SendGuide();
                            }
                            break;
                    }
                }
            }
            if (CharacterRecorder.instance.GuideID[17] < 7 && CharacterRecorder.instance.GuideID[19] > 2 && GameObject.Find("MapUiWindow") != null && GameObject.Find("NewGuideSkipWindow") == null)
            {
                CharacterRecorder.instance.NowGuideID = 17;
                GameObject Map = GameObject.Find("MapObject");
                if (Map.transform.Find("MapCon").GetComponent<MapWindow>().getreslist.Count > 0)
                {
                    switch (CharacterRecorder.instance.GuideID[17])
                    {
                        case 0:
                            if (GameObject.Find("ResourceButton") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddClick(GameObject.Find("ResourceButton"), 17);
                                LuaDeliver.AddGuideClick(GameObject.Find("ResourceButton").gameObject, 0.1f);

                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 1:
                            if (GameObject.Find("Resource_Item0") == null)
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            else
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddClick(GameObject.Find("Resource_Item0"), 17);
                                LuaDeliver.AddGuideClick(GameObject.Find("Resource_Item0"), 0.4f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3407);
                            }
                            break;
                        case 2:
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(1, "选择一个需要培养的英雄进行巡逻，可以在一定时间后收获战利品。", 1, "Lili", "杰西卡");
                            //AddGuideID(GameObject.Find("BigGuide"), 17);
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3408);
                            break;
                        case 3:
                            if (GameObject.Find("SpriteList") != null)
                            {
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddClick(GameObject.Find("SpriteList"), 17);
                                LuaDeliver.AddGuideClick(GameObject.Find("SpriteList").gameObject, 0.1f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3409);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 4:
                            if (GameObject.Find("Hero60016") != null)
                            {
                                GameObject go = GameObject.Find("Hero60016");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddClick(go, 17);
                                LuaDeliver.AddGuideClick(go, 0.4f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3410);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        case 5:
                            if (GameObject.Find("FightButton") != null)
                            {
                                GameObject _Start = GameObject.Find("FightButton");
                                GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(2, "战斗", 0, "", "");
                                //AddClick(_Start, 17);
                                LuaDeliver.AddGuideClick(_Start, 0.4f);
                                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3411);
                            }
                            else
                            {
                                SetGuideDelayCsharp(0.1f);
                            }
                            break;
                        default:
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_3412);
                            GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
                            CharacterRecorder.instance.GuideID[17] += 1;
                            SendGuide();
                            break;
                    }
                }
            }
            #endregion
            #region 赏金 索引23 千锤 索引24
            if (PictureCreater.instance.FightStyle == 6 && CharacterRecorder.instance.GuideID[23] < 2 && GameObject.Find("FightWindow").GetComponent<FightWindow>().isStartFight != false)
            {
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[23] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[23])
                {
                    case 0:
                        PictureCreater.instance.IsLock = true;
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().FightMask.SetActive(true);
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().EverydayBossobj.transform.Find("MouseMessage").gameObject.SetActive(true);
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().EverydayBossobj.transform.Find("BigMessage").gameObject.SetActive(true);
                        StartCoroutine(DelayTime(2, 23));
                        break;
                    case 1:
                        PictureCreater.instance.IsLock = false;
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 1, "", "");
                        CharacterRecorder.instance.GuideID[23] += 1;
                        SendGuide();
                        break;
                }
            }
            else if (PictureCreater.instance.FightStyle == 7 && CharacterRecorder.instance.GuideID[24] < 2 && GameObject.Find("FightWindow").GetComponent<FightWindow>().isStartFight != false)
            {
                Debug.Log("现在的ID是" + "   " + CharacterRecorder.instance.GuideID[24] + "等级" + CharacterRecorder.instance.level + "类型：" + PictureCreater.instance.FightStyle);
                switch (CharacterRecorder.instance.GuideID[24])
                {
                    case 0:
                        PictureCreater.instance.IsLock = true;
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().FightMask.SetActive(true);
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().EverydayBossobj.transform.Find("MouseMessage").gameObject.SetActive(true);
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().EverydayBossobj.transform.Find("BigMessage").gameObject.SetActive(true);
                        StartCoroutine(DelayTime(2, 24));
                        break;
                    case 1:
                        SetGuideDelayCsharp(3f);
                        PictureCreater.instance.IsLock = false;
                        GameObject.Find("NewGuideWindow").GetComponent<NewGuidWindow>().SetInfo(0, "", 1, "", "");
                        CharacterRecorder.instance.GuideID[24] += 1;
                        SendGuide();
                        break;

                }
            }
            #endregion
        }
    }
    #endregion
    void GuideSkipWindow()
    {
        if (GameObject.Find("MapUiWindow") != null)
        {
            UIManager.instance.OpenSinglePanel("NewGuideSkipWindow", false);
        }
        else
        {
            SetGuideDelayCsharp(0.1f);
        }
    }
    #region 世界事件对话和点击灯塔

    public void ShowXinPian()
    {
        GameObject go1 = (GameObject)Instantiate(Resources.Load("Prefab/Effect/XinPianHuoDe"));
        go1.name = "XinPianHuoDe";
        Transform t1 = go1.transform;
        t1.parent = GameObject.Find("UIRoot").transform;
        t1.localPosition = Vector3.zero;
        t1.localRotation = Quaternion.identity;
        t1.localScale = Vector3.one;
        AudioEditer.instance.PlayOneShot("ui_CPU4");
    }
    public void SetEventNumber(int talkid, int countid)
    {
        //Debug.Log("世界对话" + talkid + "序列号" + countid);
        Talkid = talkid;
        Countid = countid;
        NewGuideObj = GameObject.Find("NewGuideWindow");
        NewGuideObj.GetComponent<NewGuidWindow>().EventTalk = true;
        Talk talkdic = TextTranslator.instance.GetTalkById(talkid, countid);
        if (talkdic != null)
        {
            if (countid == 1)
            {
                if (PictureCreater.instance.IsChip && SceneTransformer.instance.TempSendString != "")
                {
                    ShowXinPian();
                }
            }
            int LeftRight = 0;
            string GuildLabel = "";
            if (talkdic.LeftType == 99)
            {
                LeftRight = 7;
                GuildLabel = talkdic.LeftDialog;
            }
            else
            {
                if (talkdic.LeftDialog == "")
                {
                    LeftRight = 2;
                    GuildLabel = talkdic.RightDialog;
                    GetTalkHeroInfo(talkdic.RightType);
                }
                else
                {
                    LeftRight = 1;
                    GuildLabel = talkdic.LeftDialog;
                    GetTalkHeroInfo(talkdic.LeftType);
                }
            }
            if (GameObject.Find("NewGuideWindow") != null)
            {
                if (talkdic.LeftType == 99)
                {
                    NewGuideObj.GetComponent<NewGuidWindow>().SetInfo(3, GuildLabel, 0, "", "");
                    UIEventListener.Get(NewGuideObj.transform.Find("MaxWindow/MaxWindowBg").gameObject).onClick = delegate(GameObject go)
                    {
                        DestroyImmediate(GameObject.Find("blackbg"));
                        countid++;
                        SetEventNumber(talkid, countid);
                    };
                }
                else
                {
                    NewGuideObj.GetComponent<NewGuidWindow>().SetInfo(1, GuildLabel, LeftRight, heroIcon, heroName);
                    UIEventListener.Get((NewGuideObj.transform.Find("BigGuide").gameObject)).onClick = delegate(GameObject go)
                    {
                        DestroyImmediate(GameObject.Find("blackbg"));
                        countid++;
                        SetEventNumber(talkid, countid);
                    };
                }
            }
            else
            {
                StartCoroutine(DelayEvent(talkid, countid));
            }
        }
        else
        {
            NewGuideObj.GetComponent<NewGuidWindow>().EventTalk = false;
            DestroyImmediate(GameObject.Find("blackbg"));
            NewGuideObj.GetComponent<NewGuidWindow>().SetInfo(0, "战斗", 0, "", "");
            if (PictureCreater.instance.IsChip && SceneTransformer.instance.TempSendString != "")
            {
                NetworkHandler.instance.SendProcess(TempSendString);
                PictureCreater.instance.IsChip = false;
                SceneTransformer.instance.TempSendString = "";
            }
        }
    }
    IEnumerator DelayEvent(int _talkid, int _countid)
    {
        yield return new WaitForSeconds(0.1f);
        SetEventNumber(_talkid, _countid);
    }

    public void GetTalkHeroInfo(int enumid)
    {
        switch (enumid)
        {
            case 1:
                heroName = "瑞恩";
                heroIcon = "JQB";
                break;
            case 2:
                heroName = "瓦西里";
                heroIcon = "JJB";
                break;
            case 3:
                heroName = "辛吉德";
                heroIcon = "DB";
                break;
            case 4:
                heroName = "乔治·W·雷德";
                heroIcon = "Victor";
                break;
            case 5:
                heroName = "茱莉亚";
                heroIcon = "juliya";
                break;
            case 6:
                heroName = "布兰德";
                heroIcon = "PHB";
                break;
            case 7:
                heroName = "斯内格";
                heroIcon = "SNAKE";
                break;
            case 8:
                heroName = "男枪";
                heroIcon = "glfs";
                break;
            case 9:
                heroName = "威斯克";
                heroIcon = "WSG";
                break;
            case 10:
                heroName = "博士";
                heroIcon = "Doctor";
                break;
            case 11:
                heroName = "杰西卡";
                heroIcon = "Lili";
                break;
            case 12:
                heroName = "巴尼";
                heroIcon = "Bani";
                break;
            case 13:
                heroName = "凯特丽娜";
                heroIcon = "KTL";
                break;
            case 15:
                heroName = "艾达·王";
                heroIcon = "ADA";
                break;
            case 17:
                heroName = "奥巴马";
                heroIcon = "aobama";
                break;
            case 18:
                heroName = "战壕";
                heroIcon = "ZH";
                break;
            case 19:
                heroName = "EZ";
                heroIcon = "EZ";
                break;
            case 20:
                heroName = "春丽";
                heroIcon = "chunli";
                break;
            case 21:
                heroName = "麦琪";
                heroIcon = "MQ";
                break;
            case 22:
                heroName = "圣诞";
                heroIcon = "ST";
                break;
            case 23:
                heroName = "劳拉";
                heroIcon = "LARA";
                break;
            case 24:
                heroName = "暴走斯坦森";
                heroIcon = "BZSTS";
                break;
            case 25:
                heroName = "阿卡琳";
                heroIcon = "AKL";
                break;
            case 26:
                heroName = "维嘉";
                heroIcon = "WJ";
                break;
        }
    }
    #endregion
    //日常副本延迟三秒
    public IEnumerator DelayTime(float time, int id)
    {
        float num = 0;
        StartCoroutine(GameObject.Find("FightWindow").GetComponent<FightWindow>().ShowMessage());
        while (num < time)
        {
            num += 0.02f;
            yield return new WaitForSeconds(0.02f);
            if (num >= time)
            {
                if (UIEventListener.Get(GameObject.Find("FightMask").gameObject).onClick == null)
                {
                    UIEventListener.Get(GameObject.Find("FightMask").gameObject).onClick += delegate(GameObject go)
                    {
                        if (PictureCreater.instance.FightStyle == 6)
                        {
                        }
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().EverydayBossobj.transform.Find("MouseMessage").gameObject.SetActive(false);
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().EverydayBossobj.transform.Find("BigMessage").gameObject.SetActive(false);
                        GameObject.Find("FightWindow").GetComponent<FightWindow>().FightMask.SetActive(false);
                        UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_2810);
                        CharacterRecorder.instance.GuideID[id] += 1;
                        StartCoroutine(NewbieGuide());
                    };
                }

            }
        }
    }


}
