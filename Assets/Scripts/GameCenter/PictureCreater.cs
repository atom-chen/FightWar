using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mono.Xml;
using System.IO;

public class PictureCreater : MonoBehaviour
{
    #region Announce
    public static PictureCreater instance;

    public List<RolePicture> ListRolePicture = new List<RolePicture>();
    public List<RolePicture> ListEnemyPicture = new List<RolePicture>();

    public List<Vector3> ListPosition = new List<Vector3>();

    public List<RoleSequence> ListSequence = new List<RoleSequence>();
    public int NowSequence = 1;

    public Vector3 CameraStartPosition;
    public Vector3 StartEnemyPosition;
    public Vector3 CameraScenePosition;

    public bool CreateType = false;
    /** 摄像机移动范围 **/
    public float SceneSideWidth;
    public float SceneSideHeight;
    /** 摄像机移动范围 **/
    public float GateSideWidth;
    /** 是否进入战斗 **/
    public bool IsFight = false;
    public bool IsResult = false;
    public bool IsFightFinish = false;
    /** 关卡是否开放 **/
    public bool IsRoleInGate = false;
    /** 是否自动寻路 **/
    public bool IsTraceRoad = false;
    /** 是否进入对话 **/
    public bool IsTalk = false;
    /** 点中的角色名字 **/
    public string ClickName = "";
    /** 点中的角色ID **/
    public string ClickID = "";
    /** 显示其它角色 **/
    public bool IsFollow = false;
    public bool IsAuto = true;
    public bool IsSkill = false;
    public bool IsAll = false;
    public bool IsManualSkill = false;
    public bool IsOut = false;
    public bool IsLock = false;
    public bool IsEnemyLock = false;
    public bool IsHand = true;
    public bool IsFirstBlood = false;
    public bool IsLegionWar = false;
    public bool IsCombineSkill = false;
    public bool IsSkip = false;
    public bool IsFireSkill = false;
    public bool IsMySpeedUp = false;
    public bool IsEnemySpeedUp = false;

    /// <summary>
    /// 0是关卡，1是pvp，2是敢死队，3是夺宝，4是世界事件，5是占领资源，6是赏金猎人，7是千锤百炼 ，8是极限挑战 ，9是固若金汤 ，10是攻守兼备 ，11是弃放强攻, 12军团副本, 13巡航, 14搔扰, 15俘虏, 16军衔, 17核电战
    /// </summary>
    public int FightStyle = 0;
    public int PVPRank = 0;
    public int GrabID = 0;
    public string LegionPosition = "";
    public string InstancePosition = "";
    public string PVPPosition = "";
    public string PVEPosition = "";
    public string WoodPosition = "";
    /** 角色前进方向的坐标距离 **/
    Vector3 newMainRoleForwardPosition = new Vector3();
    /** 移动距离 **/
    float MoveDistance = 0.0f;
    float FightTimer = 0;
    public float MyUISliderValue = 0;

    public int SkillFire1 = 0;
    public int SkillFire2 = 0;
    public int SkillFire3 = 0;

    UISlider[] BloodSlider = new UISlider[6];
    UISlider[] SkillSlider = new UISlider[6];
    UISprite[] SkillSprite = new UISprite[6];
    UISprite[] SequenceTexture = new UISprite[8];
    UISprite[] SequenceSprite = new UISprite[8];

    public GameObject Weather;
    public GameObject MyPositions;
    public GameObject MyBases;
    public GameObject MyMoves;
    public GameObject MyPath;
    GameObject FightScene;

    int OldPosition = 0;
    public float FieldView = 60;
    public GameObject MyCamera;
    //public GameObject FightCamera;
    public GameObject EffectCamera;

    public float HardLevel = 1;
    public float CameraY = 35;
    float OffsetX = 2.4f;
    float OffsetY = 0;
    float OffsetZ = 2.078f;
    public int PositionRow = 3;
    public int PositionColumn = 7;
    public int SelectPosition = 0;
    public int EnemyCaptainIndex = 0;
    public int MyCaptainIndex = 0;
    public int ContinueWin = 0;
    public float WeakNum;
    public string SelectName = "";

    int AttackDirection = 1;
    public int AttackCount = 0;

    List<int> ListStopPosition = new List<int>();
    List<int> ListTargetPosition = new List<int>();
    public List<GameObject> ListMove = new List<GameObject>();
    public List<GameObject> ListBase = new List<GameObject>();

    public Dictionary<string, AssetBundle> DictAssetBundle = new Dictionary<string, AssetBundle>();

    FightWindow fw;

    //public GameObject MySkill;
    //public GameObject SkillCamera;

    public int HealIndex = 0;
    public int WeatherID = 0;
    public float WeatherTimer = 0;
    public float WeatherTime = 5;


    public float LimitTimer = 0;
    public float LimitTime = 999;
    public int KillEnemyID = 0;
    public int BossEnemyID = 0;
    public int LimitLose = 200;
    public int LimitWin = 999;
    public int NPCID = 0;
    public int NPCPosition = 0;
    public int LimitBlood = 0;
    public int LimitType = 0;
    public int LimitStar2 = 0;
    public int LimitStar3 = 0;
    public int LimitStarCount2 = 0;
    public int LimitStarCount3 = 0;
    public int PermissionType = 0;
    public int LimitHeroID = 0;
    public int LimitHeroCount = 0;
    public bool RandomDebuff = false;
    public bool RandomBuff = false;
    public int RandomDebuffID = 0;
    public int RandomBuffID = 0;

    public string PVPString = "";
    public string PVPName = "";

    public int FightSkill1 = 0;
    public int FightSkill2 = 0;
    public int FightSkill3 = 0;

    public int FightCount = 0;
    public float FightHintColor = 1;
    public float FightHintTimer = 0;
    public bool FightHint = true;
    public float FightCalculate = 1;
    public float EnemyCalculate = 1;
    public int TotalAttack;

    public string EasyWood;
    public string NormalWood;
    public string HardWood;
    public string WoodBuff;

    public int WoodFloor;
    public int WoodStar;
    public int WoodFight;

    List<NPCTactics> ListNPCTactics = new List<NPCTactics>();
    public List<int> ListTerrainPosition = new List<int>();
    List<int> ListTerrainID = new List<int>();

    public List<float> ListRandom = new List<float>();
    public bool IsRemember = true;
    public int RememberCount = 0;
    public bool IsChip = false;

    public Dictionary<int, string> ListWinRecord = new Dictionary<int, string>();

    int GateNeedLevel = 1;
    string GateTerrain = "";
    int LegionGateID = 0;
    int LegionGroupID = 0;
    string LegionGateString = "";
    int MyWeakPoint = 0;
    int EnemyWeakPoint = 0;

    public List<Buff> SetListBuff = new List<Buff>();

    public Material HurtMaterial;
    public Material BlankMaterial;
    public Material Boss1Material;
    public Material Boss2Material;

    public Material EPositionMaterial;
    public Material BPositionMaterial;
    public Material PositionMaterial;

    public GameObject ActObject;

    public class RoleSequence
    {
        public bool IsMonster;
        public int RoleIndex;
        public int RoleID;
        public float NowSpeed;
    }

    public class RolePicture
    {
        public GameObject RoleObject;
        public GameObject RoleSparkleObject;
        public GameObject RoleSuitObject;
        public GameObject RolePictureObject;
        public GameObject RoleNameObject;
        public GameObject RoleNameRBlodObject;
        public GameObject RoleNameLBlodObject;

        public GameObject RoleGroupObject;
        public GameObject RoleGroupRBlodObject;
        public GameObject RoleGroupLBlodObject;

        public GameObject RoleRedBloodObject;
        public GameObject RoleBlackBloodObject;
        public GameObject RoleTaskObject;
        public GameObject RoleTargetObject;
        public Vector3 RolePictureNewPosition;
        public Vector3 RolePictureLastPosition;
        public Vector3 RolePictureCheckPosition;
        public string RolePictureName;
        public string RolePictureChange;
        public string RolePicturePointID;
        public int RoleID;
        public int RolePictureNowFrame;
        public float RolePictureWidthFrame;
        public float RolePictureTransparent;
        public float RolePictureScaleType;
        public int RolePictureFaceRight;
        public int RolePictureNowTrace;
        public int RolePosition;
        public int RoleMoveStep;
        public int RoleMoveNowStep;
        public int RoleSkillPoint;
        public int RoleMovePosition;
        public int RoleTargetIndex;
        public int RoleContinueKill;
        public bool RoleTargetMonster;
        public bool RoleCaptain;
        public bool RoleCloseEye;

        public float RoleFightSpeed; //攻击CD
        public float RoleFightNowSpeed; //攻击CD

        public bool RolePictureStartAttack;

        public bool ChangePic = false;
        public bool RolePictureAnimation;
        public bool RolePictureMoving;
        public bool RolePictureMonster;
        public bool RolePictureAttackable;
        public bool RolePictureNPC;
        public bool RolePictureFight;
        public bool RolePictureTraceLoop;
        public bool RolePictureTrace;
        public bool RolePictureFollow;
        public bool RolePictureHide;
        public bool RoleShowSkill = false;
        public bool RoleShowBlood = true;
        public bool IsPicture;

        public float RolePictureTimer;
        public float RolePictureTraceTimer;     //計算軌跡時間
        public float RolePictureIdleTimer;
        public float RolePiectureMoveSpeed;
        public float RolePiectureTraceSpeed;
        public float RolePictureAnimationSpeed;
        public float RolePiectureWidthScale;
        public float RolePiectureHeightScale;

        public float RoleMaxBlood;
        public float RoleNowBlood;
        public float RoleNowBloodPosition;
        public float RolePAttack;
        public float RolePDefend;

        public float RoleHit;
        public float RoleNoHit;
        public float RoleCrit;
        public float RoleNoCrit;
        public float RoleDamigeAdd;
        public float RoleDamigeReduce;

        public int RoleAttackType;
        public int RoleCareer;

        public int RoleSkill1;
        public int RoleSkill2;

        public int RoleSkillLevel1 = 1;
        public int RoleSkillLevel2 = 1;

        public int RoleArea;
        public int RoleAttackMode;
        public int RoleDefendMode;
        public int RoleAi;
        public int RoleBio;
        public int RoleAttackFly;
        public int RoleAttackGround;
        public int RoleRace;
        public int RoleForce;
        public int RoleStage;
        public int RoleSex;
        public int RoleRelife;
        public int RoleHurtCount;
        public int[] RoleInnate = new int[18];
        //public TextTranslator.BossAi RoleBossAiInfo;

        public int RoleCharacterRoleID;

        public List<int> RoleFightIndex = new List<int>();
        public List<int> RoleFightType = new List<int>();
        public List<int> RoleFightDamige = new List<int>();

        public bool RoleIsMove;
        public List<Vector3> RolePictureTracePosition = new List<Vector3>();

        //////////////////Buff相关//////////////////
        public List<Buff> ListBuff = new List<Buff>();
        public float BuffAttack;
        public float BuffDefend;
        public float BuffHp;
        public float BuffNoHp;

        public float BuffFightSpeed;

        public float BuffHit;
        public float BuffNoHit;
        public float BuffCrit;
        public float BuffNoCrit;
        public float BuffDamigeAdd;
        public float BuffDamigeReduce;
        public float BuffPoison; //中毒
        public float BuffBurn; //燃烧

        public float BuffHurtAdd;
        public float BuffBackDamige;

        public int BuffAvoidDebuff; //免疫伤害
        public int BuffTargetIndex; //嘲讽
        public int BuffSkillPoint; //怒气
        public int BuffAvoidCount; //免疫伤害

        public bool BuffAvoidDead; //不死
        public bool BuffTotalAttack; //集火
        public bool BuffInvisible; //隐身
        public bool BuffStop; //眩晕
        public bool BuffNoMove; //定身
        public bool BuffSilence; //沉默
        //////////////////Buff相关//////////////////
    }
    #endregion

    #region Picture
    public List<PictureRoleData> PicRoleList = new List<PictureRoleData>();
    public List<PictureRoleData> PicEnemyList = new List<PictureRoleData>();
    ////////////////////////////////////////////////////////////////动画相关(以下)//////////////////////////////////////////////////////////////
    //动画每一帧
    public class PerPictureData
    {
        public string Name;
        public int x;
        public int y;
        public int Width;
        public int Height;
    }
    public class PictureData
    {
        public string Name;
        public string NowAnimation;
        public int Width;
        public int Height;
        public float Left;
        public float Top;
        public float Scale;
        public bool FaceRight = true;
        public List<PerPictureData> PicChild = new List<PerPictureData>();
        public int Pointer = 0;
        public int Count = 1;
        public int Index = 1;

        public Vector3 getPosition(Vector3 Position, float PositionZ, float ScaleType)
        {
            if (FaceRight)
            {
                return new Vector3(Position.x + Left, Position.y + Top + getLocalScale().y * 0.5f, Position.z - getLocalScale().y * 0.2f);
            }
            else
            {
                return new Vector3(Position.x - Left, Position.y + Top + getLocalScale().y * 0.5f, Position.z - getLocalScale().y * 0.2f);
            }
        }

        public Vector3 getLocalScale()
        {
            return new Vector3(((float)PicChild[0].Width / 50f) * Scale, ((float)PicChild[0].Height / 50f) * Scale, 0.00001f);
        }

        public Vector2 getTextureScale()
        {
            if (FaceRight)
            {
                return new Vector2((float)PicChild[0].Width / Width, (float)PicChild[0].Height / Height);
            }
            else
            {
                return new Vector2(-(float)PicChild[0].Width / Width, (float)PicChild[0].Height / Height);
            }
        }

        public void setNextIndex(int SetIndex)
        {
            Index = SetIndex;
            Pointer = (Index - 1) * PicChild.Count / Count;
        }

        public int getLastIndex()
        {
            return Index;
        }

        public Vector2 getNextTextureOffset()
        {
            int LPointer = Pointer;
            Pointer++;
            if (Pointer >= Index * PicChild.Count / Count)
            {
                Pointer = (Index - 1) * PicChild.Count / Count;
            }
            if (FaceRight)
            {
                return new Vector2((float)PicChild[LPointer].x / Width, 1 - ((float)(PicChild[LPointer].y + PicChild[LPointer].Height) / Height));
            }
            else
            {
                return new Vector2((float)PicChild[LPointer].Width / Width + (float)PicChild[LPointer].x / Width, 1 - ((float)(PicChild[LPointer].y + PicChild[LPointer].Height) / Height));
            }

        }
    }
    public class PictureRoleData
    {
        public string Name;
        public PictureData Stand;
        public PictureData Run;
        public PictureData Fight;
        public PictureData Attack;
        public PictureData Stunt;
        public PictureData Power;
        public PictureData Win;
        public PictureData Funny;
        public PictureData Sit;
        public PictureData Hurt;

        public PictureData CurrentAction;
    }
    public void PlayAnimation(List<RolePicture> SetPicture, List<PictureRoleData> SetData, int SetRoleIndex, float SetRoleAnimationSpeed, string SetPlayLevel)
    {
        if (SetPicture[SetRoleIndex].IsPicture)
        {
            SetPicture[SetRoleIndex].RolePictureAnimationSpeed = SetRoleAnimationSpeed;
            SetPicture[SetRoleIndex].RolePictureAnimation = true;
            int LastIndex = SetData[SetRoleIndex].CurrentAction.getLastIndex();
            int LastCount = SetData[SetRoleIndex].CurrentAction.Count;
            if (SetData[SetRoleIndex].Stand.PicChild.Count != 1)
            {
                switch (SetPlayLevel)
                {
                    case "Stand":
                        SetData[SetRoleIndex].Stand.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                        SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Stand;
                        SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                        if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") != null)
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") as Texture;
                        }
                        else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p0") as Texture;
                        }
                        break;
                    case "Attack":
                        SetData[SetRoleIndex].Attack.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                        SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Attack;
                        SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                        if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p3") != null)
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p3") as Texture;
                        }
                        else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p3") as Texture;
                        }
                        break;
                    case "Fight":
                        SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Fight;

                        if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p2") != null)
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p2") as Texture;
                        }
                        else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p2") as Texture;
                        }
                        break;
                    case "Stunt":
                        if (SetData[SetRoleIndex].Stunt == null)
                        {
                            SetData[SetRoleIndex].Attack.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Attack;

                            if (LastCount > 4)
                            {
                                if (LastIndex >= 4)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                                }
                                else if (LastIndex == 3)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                                }
                                else
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                                }
                            }
                            else
                            {
                                SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                            }

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p3") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p3") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p3") as Texture;
                            }
                        }
                        else
                        {
                            SetData[SetRoleIndex].Stunt.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Stunt;
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(1);

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p4") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p4") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p4") as Texture;
                            }
                        }
                        break;
                    case "Power":
                        if (SetData[SetRoleIndex].Power == null)
                        {
                            SetData[SetRoleIndex].Attack.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Attack;

                            if (LastCount > 4)
                            {
                                if (LastIndex >= 4)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                                }
                                else if (LastIndex == 3)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                                }
                                else
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                                }
                            }
                            else
                            {
                                SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                            }

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p3") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p3") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p3") as Texture;
                            }
                        }
                        else
                        {
                            SetData[SetRoleIndex].Stunt.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Power;
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(1);

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p5") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p5") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p5") as Texture;
                            }
                        }
                        break;
                    case "Win":
                        if (SetData[SetRoleIndex].Win == null)
                        {
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Stand;

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") as Texture;

                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p0") as Texture;

                            }
                        }
                        else
                        {
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Win;

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p6") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p6") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p6") as Texture;
                            }
                        }
                        break;
                    case "Funny":
                        SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Funny;

                        if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p7") != null)
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p7") as Texture;
                        }
                        else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                        {
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p7") as Texture;
                        }
                        break;
                    case "Sit":
                        if (SetData[SetRoleIndex].Sit == null)
                        {
                            SetData[SetRoleIndex].Stand.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Stand;
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p0") as Texture;
                            }
                        }
                        else
                        {
                            if (SetData[SetRoleIndex].Hurt == null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.transform.Rotate(0, 0, 90);
                            }
                            SetData[SetRoleIndex].Sit.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Sit;

                            if (LastCount > 4)
                            {
                                if (LastIndex >= 4)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                                }
                                else if (LastIndex == 3)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                                }
                                else
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                                }
                            }
                            else
                            {
                                SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                            }

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p8") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p8") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p8") as Texture;
                            }
                        }
                        break;
                    case "Hurt":
                        if (SetData[SetRoleIndex].Hurt == null)
                        {
                            SetData[SetRoleIndex].Stand.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Stand;
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p0") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p0") as Texture;
                            }
                        }
                        else
                        {
                            SetData[SetRoleIndex].Hurt.FaceRight = SetData[SetRoleIndex].CurrentAction.FaceRight;
                            SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Hurt;
                            SetPicture[SetRoleIndex].RolePictureTimer = 0;

                            if (LastCount > 4)
                            {
                                if (LastIndex >= 4)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                                }
                                else if (LastIndex == 3)
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                                }
                                else
                                {
                                    SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                                }
                            }
                            else
                            {
                                SetData[SetRoleIndex].CurrentAction.setNextIndex(LastIndex);
                            }

                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p9") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p9") as Texture;
                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p9") as Texture;
                            }
                        }
                        break;
                }
            }
            SetPicture[SetRoleIndex].RolePictureNowFrame = 0;
            SetPicture[SetRoleIndex].RolePictureObject.transform.position = SetData[SetRoleIndex].CurrentAction.getPosition(SetPicture[SetRoleIndex].RoleObject.transform.position, SetPicture[SetRoleIndex].RoleObject.transform.position.z, SetPicture[SetRoleIndex].RolePictureScaleType);
            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureOffset = SetData[SetRoleIndex].CurrentAction.getNextTextureOffset();
            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureScale = SetData[SetRoleIndex].CurrentAction.getTextureScale();
            SetPicture[SetRoleIndex].RolePictureObject.transform.localScale = SetData[SetRoleIndex].CurrentAction.getLocalScale();
        }
    }
    public bool AddPicRole(string RoleID, bool IsEnemy)
    {
        PictureRoleData NewPicRole = new PictureRoleData();

        NewPicRole.Name = "Role" + RoleID;
        if (Resources.Load("Role/Role" + RoleID + "/config") != null)
        {
            NewPicRole = ParserXML(Resources.Load("Role/Role" + RoleID + "/config").ToString());
        }
        //else if (DictAssetBundle.ContainsKey("Role" + RoleID.ToString()))
        //{
        //    if (DictAssetBundle["Role" + RoleID.ToString()] != null)
        //    {
        //        NewPicRole = ParserXML(DictAssetBundle["Role" + RoleID.ToString()].Load("config").ToString());
        //    }
        //    else
        //    {
        //        NewPicRole.Stand = null;
        //        NewPicRole.Run = null;
        //        NewPicRole.Fight = null;
        //        NewPicRole.Attack = null;
        //        NewPicRole.Stunt = null;
        //        NewPicRole.Power = null;
        //        NewPicRole.Win = null;
        //        NewPicRole.Funny = null;
        //        NewPicRole.Sit = null;
        //        NewPicRole.Hurt = null;
        //        NewPicRole = ParserXML(Resources.Load("Role/Role0/config").ToString());
        //        StartCoroutine(transform.parent.GetComponent<GameCenter>().gameResourceLoader.GetComponent<ResourceLoader>().DownloadGameResource("Role" + RoleID.ToString()));
        //    }
        //}
        //else
        //{
        //    NewPicRole.Stand = null;
        //    NewPicRole.Run = null;
        //    NewPicRole.Fight = null;
        //    NewPicRole.Attack = null;
        //    NewPicRole.Stunt = null;
        //    NewPicRole.Power = null;
        //    NewPicRole.Win = null;
        //    NewPicRole.Funny = null;
        //    NewPicRole.Sit = null;
        //    NewPicRole.Hurt = null;
        //    NewPicRole = ParserXML(Resources.Load("Role/Role0/config").ToString());
        //    StartCoroutine(transform.parent.GetComponent<GameCenter>().gameResourceLoader.GetComponent<ResourceLoader>().DownloadGameResource("Role" + RoleID.ToString()));
        //}

        if (IsEnemy)
        {
            PicEnemyList.Add(NewPicRole);
        }
        else
        {
            PicRoleList.Add(NewPicRole);
        }
        return true;
    }
    public bool UpdatePicRole(int RoleIndex, string RoleID, int PlayLevel, int IsFaceRight)
    {
        bool FaceRight = true;
        if (IsFaceRight == -1)
        {
            FaceRight = false;
        }
        if (PlayLevel == 0)
        {
            if (Resources.Load("Role/Role" + RoleID + "/config") != null)
            {
                PicRoleList[RoleIndex] = ParserXML(Resources.Load("Role/Role" + RoleID + "/config").ToString());
            }
            else if (DictAssetBundle.ContainsKey("Role" + RoleID.ToString()))
            {
                PicRoleList[RoleIndex] = ParserXML(DictAssetBundle["Role" + RoleID.ToString()].Load("config").ToString());
                if (PicRoleList[RoleIndex].Stand != null)
                {
                    PicRoleList[RoleIndex].Stand.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Run != null)
                {
                    PicRoleList[RoleIndex].Run.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Fight != null)
                {
                    PicRoleList[RoleIndex].Fight.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Attack != null)
                {
                    PicRoleList[RoleIndex].Attack.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Stunt != null)
                {
                    PicRoleList[RoleIndex].Stunt.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Power != null)
                {
                    PicRoleList[RoleIndex].Power.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Win != null)
                {
                    PicRoleList[RoleIndex].Win.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Funny != null)
                {
                    PicRoleList[RoleIndex].Funny.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Sit != null)
                {
                    PicRoleList[RoleIndex].Sit.FaceRight = FaceRight;
                }
                if (PicRoleList[RoleIndex].Hurt != null)
                {
                    PicRoleList[RoleIndex].Hurt.FaceRight = FaceRight;
                }
            }
            else
            {
                PicRoleList[RoleIndex].Stand = null;
                PicRoleList[RoleIndex].Run = null;
                PicRoleList[RoleIndex].Fight = null;
                PicRoleList[RoleIndex].Attack = null;
                PicRoleList[RoleIndex].Stunt = null;
                PicRoleList[RoleIndex].Power = null;
                PicRoleList[RoleIndex].Win = null;
                PicRoleList[RoleIndex].Funny = null;
                PicRoleList[RoleIndex].Sit = null;
                PicRoleList[RoleIndex].Hurt = null;
                return false;
            }
        }
        else
        {
            if (Resources.Load("Monster/Monster" + RoleID + "/config") != null)
            {
                PicRoleList[RoleIndex] = ParserXML(Resources.Load("Monster/Monster" + RoleID + "/config").ToString());
            }
            else if (DictAssetBundle.ContainsKey("Monster" + RoleID.ToString()))
            {
                PicRoleList[RoleIndex] = ParserXML(DictAssetBundle["Monster" + RoleID.ToString()].Load("config").ToString());
            }
            else
            {
                PicRoleList[RoleIndex].Stand = null;
                PicRoleList[RoleIndex].Run = null;
                PicRoleList[RoleIndex].Fight = null;
                PicRoleList[RoleIndex].Attack = null;
                PicRoleList[RoleIndex].Stunt = null;
                PicRoleList[RoleIndex].Power = null;
                PicRoleList[RoleIndex].Win = null;
                PicRoleList[RoleIndex].Funny = null;
                PicRoleList[RoleIndex].Sit = null;
                PicRoleList[RoleIndex].Hurt = null;
                return false;
            }
        }


        PicRoleList[RoleIndex].CurrentAction = PicRoleList[RoleIndex].Stand;
        return true;
    }
    public PictureRoleData ParserXML(string ParseXMLText)
    {
        PictureRoleData NewPicRole = new PictureRoleData();
        NewPicRole.Stand = null;
        NewPicRole.Run = null;
        NewPicRole.Fight = null;
        NewPicRole.Attack = null;
        NewPicRole.Stunt = null;
        NewPicRole.Power = null;
        NewPicRole.Win = null;
        NewPicRole.Funny = null;
        NewPicRole.Sit = null;
        NewPicRole.Hurt = null;
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            foreach (System.Security.SecurityElement childPD in SE.Children)
            {
                PictureData PD = new PictureData();

                PD.Name = childPD.Attribute("imagePath");
                PD.Width = int.Parse(childPD.Attribute("width"));
                PD.Height = int.Parse(childPD.Attribute("height"));
                PD.Left = float.Parse(childPD.Attribute("left"));
                PD.Top = float.Parse(childPD.Attribute("top"));
                PD.Scale = float.Parse(childPD.Attribute("scale"));

                foreach (System.Security.SecurityElement child in childPD.Children)
                {
                    PerPictureData NewPPD = new PerPictureData();
                    NewPPD.Name = child.Attribute("n");
                    NewPPD.x = int.Parse(child.Attribute("x"));
                    NewPPD.y = int.Parse(child.Attribute("y"));
                    NewPPD.Width = int.Parse(child.Attribute("w"));
                    NewPPD.Height = int.Parse(child.Attribute("h"));
                    PD.PicChild.Add(NewPPD);
                }

                switch (childPD.Attribute("type"))
                {
                    case "0":
                        PD.Count = 5;
                        PD.NowAnimation = "idle";
                        PD.setNextIndex(5);
                        NewPicRole.Stand = PD;
                        break;
                    case "1":
                        PD.Count = 5;
                        PD.NowAnimation = "run";
                        PD.setNextIndex(5);
                        NewPicRole.Run = PD;
                        break;
                    case "2":
                        NewPicRole.Fight = PD;
                        break;
                    case "3":
                        PD.Count = 5;
                        PD.NowAnimation = "attack";
                        PD.setNextIndex(5);
                        NewPicRole.Attack = PD;
                        break;
                    case "4":
                        PD.Count = 1;
                        PD.setNextIndex(1);
                        NewPicRole.Stunt = PD;
                        break;
                    case "5":
                        PD.Count = 1;
                        PD.setNextIndex(1);
                        NewPicRole.Power = PD;
                        break;
                    case "6":
                        NewPicRole.Win = PD;
                        break;
                    case "7":
                        NewPicRole.Funny = PD;
                        break;
                    case "8":
                        PD.Count = 3;
                        PD.setNextIndex(3);
                        NewPicRole.Sit = PD;
                        break;
                    case "9":
                        PD.Count = 3;
                        PD.setNextIndex(3);
                        NewPicRole.Hurt = PD;
                        break;
                }
            }

            if (NewPicRole.Sit == null)
            {
                PictureData SitPD = new PictureData();

                SitPD.Name = NewPicRole.Stand.Name;
                SitPD.Width = NewPicRole.Stand.Width;
                SitPD.Height = NewPicRole.Stand.Height;
                SitPD.Left = NewPicRole.Stand.Left;
                SitPD.Top = NewPicRole.Stand.Top;
                SitPD.Scale = NewPicRole.Stand.Scale;

                PerPictureData NewSitPD = new PerPictureData();
                NewSitPD = NewPicRole.Stand.PicChild[0];
                SitPD.PicChild.Add(NewSitPD);
                SitPD.PicChild.Add(NewSitPD);
                SitPD.PicChild.Add(NewSitPD);

                SitPD.Count = 3;
                SitPD.setNextIndex(3);
                NewPicRole.Sit = SitPD;
            }
        }
        catch (Exception ex)  //如果XML出错时的处理
        {
            Debug.Log("PictureCreater XML:" + ex.ToString());
        }
        return NewPicRole;
    }
    public GameObject CreatePicRole(List<PictureRoleData> SetData, string RoleName, int RoleID, Vector3 StartPosition, float ScaleType, int IsFaceRight)
    {
        int PicRoleIndex = SetData.Count - 1;
        bool FaceRight = true;
        if (IsFaceRight == -1)
        {
            FaceRight = false;
        }
        if (SetData[PicRoleIndex].Stand != null)
        {
            SetData[PicRoleIndex].Stand.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Run != null)
        {
            SetData[PicRoleIndex].Run.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Fight != null)
        {
            SetData[PicRoleIndex].Fight.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Attack != null)
        {
            SetData[PicRoleIndex].Attack.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Stunt != null)
        {
            SetData[PicRoleIndex].Stunt.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Power != null)
        {
            SetData[PicRoleIndex].Power.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Win != null)
        {
            SetData[PicRoleIndex].Win.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Funny != null)
        {
            SetData[PicRoleIndex].Funny.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Sit != null)
        {
            SetData[PicRoleIndex].Sit.FaceRight = FaceRight;
        }
        if (SetData[PicRoleIndex].Hurt != null)
        {
            SetData[PicRoleIndex].Hurt.FaceRight = FaceRight;
        }
        SetData[PicRoleIndex].CurrentAction = SetData[PicRoleIndex].Stand;
        GameObject RoleCube = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(RoleCube.GetComponent("MeshCollider"));
        RoleCube.AddComponent("BoxCollider");
        RoleCube.renderer.castShadows = false;
        RoleCube.renderer.receiveShadows = false;
        RoleCube.name = RoleName;

        RoleCube.renderer.material.color = Color.white;
        RoleCube.renderer.material.shader = Shader.Find("Unlit/Transparent");
        RoleCube.renderer.material.mainTexture = Resources.Load("Game/ren") as Texture;

        SetData[PicRoleIndex].CurrentAction.setNextIndex(3);
        SetData[PicRoleIndex].CurrentAction.FaceRight = true;

        if (SetData[PicRoleIndex].CurrentAction != null)
        {
            RoleCube.transform.position = SetData[PicRoleIndex].CurrentAction.getPosition(StartPosition, StartPosition.z, 0);
            RoleCube.renderer.material.mainTextureOffset = SetData[PicRoleIndex].CurrentAction.getNextTextureOffset();
            RoleCube.renderer.material.mainTextureScale = SetData[PicRoleIndex].CurrentAction.getTextureScale();
            RoleCube.transform.localScale = SetData[PicRoleIndex].CurrentAction.getLocalScale();
        }
        else
        {
            RoleCube.transform.position = StartPosition;
        }

        RoleCube.transform.Rotate(new Vector3(45, 0, 0));

        if (Resources.Load("Role/Role" + RoleID + "/p0") != null)
        {
            RoleCube.renderer.material.mainTexture = Resources.Load("Role/Role" + RoleID + "/p3") as Texture;
            RoleCube.renderer.material.mainTexture = Resources.Load("Role/Role" + RoleID + "/p1") as Texture;
            RoleCube.renderer.material.mainTexture = Resources.Load("Role/Role" + RoleID + "/p0") as Texture;
        }
        else if (DictAssetBundle.ContainsKey("Role" + RoleID))
        {
            RoleCube.renderer.material.mainTexture = DictAssetBundle["Role" + RoleID].Load("p0") as Texture;
            DictAssetBundle["Role" + RoleID].Load("p1");
        }
        else
        {
            RoleCube.renderer.material.mainTexture = Resources.Load("Game/ren") as Texture;
        }
        return RoleCube;
    }
    ////////////////////////////////////////////////////////////////动画相关(以上)//////////////////////////////////////////////////////////////
    #endregion

    #region ListPosition
    public void SetListPosition(int SetRow, int CreateType)
    {
        if (CreateType == 0)
        {
            ListPosition.Clear();
            ListPosition.Add(new Vector3(100, 100, 100));

            PositionRow = SetRow;
            PositionColumn = PositionRow + 4;
            int CenterX = PositionColumn / 2;
            float CenterZ = PositionRow / 2f;
            Vector3 StartPosition = new Vector3(-CenterX * OffsetX, OffsetY, CenterZ * OffsetZ);

            Destroy(MyPositions);
            Destroy(MyBases);

            MyPositions = new GameObject("MyPositions");
            MyBases = new GameObject("MyBases");
            MyMoves = new GameObject("MyMoves");
            MyPath = new GameObject("MyPath");


            for (int i = 0; i < PositionRow * PositionColumn + 30; i++)
            {
                GameObject MyMove = null;
                ListBase.Add(MyMove);
            }

            for (int i = 0; i < PositionRow * PositionColumn + 30; i++)
            {
                GameObject MyMove = null;
                ListMove.Add(MyMove);
            }

            for (int j = 0; j < PositionColumn; j++)
            {
                for (int i = 0; i < PositionRow; i++)
                {
                    //Destroy(GameObject.Find("SkillObject"));

                    switch (PositionRow)
                    {
                        case 3:
                            if (i % 3 == 0)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j - OffsetX / 2f, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else if (i % 3 == 1)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j + OffsetX / 2f, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            break;
                        case 4:
                            if (i % 4 == 0)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else if (i % 4 == 1)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j + OffsetX / 2f, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else if (i % 4 == 2)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j + OffsetX, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j + OffsetX + OffsetX / 2f, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            break;
                        case 5:
                            if (i % 5 == 0)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j - OffsetX, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else if (i % 5 == 1)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j - OffsetX / 2f, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else if (i % 5 == 2)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else if (i % 5 == 3)
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j + OffsetX / 2f, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            else
                            {
                                ListPosition.Add(new Vector3(StartPosition.x + OffsetX * j + OffsetX, OffsetY, StartPosition.z - OffsetZ * i));
                            }
                            break;
                    }
                }
            }
        }

        if (CreateType == 0)
        {
            ListStopPosition.Clear();
        }
        else
        {
            if (!IsFight)
            {
                MyBases.SetActive(true);
                MyMoves.SetActive(true);
            }
        }



        for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
        {
            if (PositionRow == 3)
            {
                if (i == 1 || i == 21)
                {
                    ListStopPosition.Add(i);
                    continue;
                }
            }
            else if (PositionRow == 4)
            {
                if (i == 1 || i == 2 || i == 28 || i == 30 || i == 31 || i == 32)
                {
                    ListStopPosition.Add(i);
                    continue;
                }
            }
            else if (PositionRow == 5)
            {
                if (i == 1 || i == 2 || i == 3 || i == 4 || i == 6 || i == 7 || i == 39 || i == 40 || i == 42 || i == 43 || i == 44 || i == 45)
                {
                    ListStopPosition.Add(i);
                    continue;
                }
            }

            if (CreateType == 0)
            {



                ListBase[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //DestroyImmediate(ListBase[i].GetComponent("MeshCollider"));
                ListBase[i].renderer.material = BlankMaterial;

                ListBase[i].renderer.receiveShadows = false;
                ListBase[i].renderer.castShadows = false;

                ListBase[i].name = "Base" + i.ToString();
                ListBase[i].transform.localScale = new Vector3(OffsetX * 1.25f, OffsetX * 1.25f, OffsetX * 1.25f);
                ListBase[i].transform.Rotate(90, 90, 0);
                ListBase[i].transform.position = ListPosition[i] + new Vector3(0, 0.01f, 0);

                //ListBase[i] = new GameObject("Base" + i.ToString());
                //GameObject BatchBase = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //DestroyImmediate(BatchBase.GetComponent("MeshCollider"));
                //BatchBase.renderer.material = BlankMaterial;

                //BatchBase.renderer.receiveShadows = false;
                //BatchBase.renderer.castShadows = false;

                //BatchBase.name = "Base";
                //BatchBase.transform.localScale = new Vector3(OffsetX * 1.25f, OffsetX * 1.25f, OffsetX * 1.25f);
                //BatchBase.transform.localPosition = Vector3.zero;
                //BatchBase.transform.Rotate(90, 90, 0);                
                //BatchBase.transform.parent = ListBase[i].transform;

                //ListBase[i].isStatic = true;
                //ListBase[i].transform.position = ListPosition[i] + new Vector3(0, 0.01f, 0); 
                ListBase[i].transform.parent = MyBases.transform;


                DestroyImmediate(GameObject.Find("Move" + i.ToString()));
                ListMove[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                DestroyImmediate(ListMove[i].GetComponent("MeshCollider"));
                ListMove[i].renderer.material = BlankMaterial;

                ListMove[i].renderer.receiveShadows = false;
                ListMove[i].renderer.castShadows = false;

                ListMove[i].name = "Move" + i.ToString();
                ListMove[i].transform.localScale = new Vector3(OffsetX * 1.25f, OffsetX * 1.25f, OffsetX * 1.25f);
                ListMove[i].transform.Rotate(90, 90, 0);
                ListMove[i].transform.position = ListPosition[i] + new Vector3(0, 0.03f, 0);
                ListMove[i].transform.parent = MyMoves.transform;

                //GameObject SkillObject = GameObject.Instantiate(Resources.Load("Prefab/Effect/Skill_JiaoXia_UI", typeof(GameObject)), ListPosition[i], Quaternion.identity) as GameObject;
                //SkillObject.name = "SkillObject";
                //SkillObject.transform.parent = ListBase[i].transform;
                //SkillObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                //SkillObject.SetActive(false);





                //GameObject BloodObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //BloodObject.renderer.material.mainTexture = Resources.Load("Game/xuetiao5", typeof(Texture)) as Texture;
                //BloodObject.renderer.material.shader = Shader.Find("Unlit/Transparent");
                //BloodObject.name = "BloodObject";
                //BloodObject.transform.localScale = new Vector3(2.15f, 2.15f, 2.15f);
                //BloodObject.transform.Rotate(90, 0, 0);
                //BloodObject.transform.position = ListPosition[i] + new Vector3(0, 0.01f, 0);
                //BloodObject.transform.parent = go.transform;

                //GameObject BloodObject1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //BloodObject1.renderer.material.mainTexture = Resources.Load("Game/xuetiao1", typeof(Texture)) as Texture;
                //BloodObject1.renderer.material.shader = Shader.Find("Unlit/Transparent");
                //BloodObject1.name = "RedBloodObject" + i.ToString();
                //BloodObject1.transform.localScale = new Vector3(2.15f, 2.15f, 2.15f);
                //BloodObject1.transform.Rotate(90, 0, 0);
                //BloodObject1.transform.position = ListPosition[i] + new Vector3(0, 0.01f, 0);
                //BloodObject1.transform.parent = BloodObject.transform;

                //GameObject BloodObject2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //BloodObject2.renderer.material.mainTexture = Resources.Load("Game/xuetiao3", typeof(Texture)) as Texture;
                //BloodObject2.renderer.material.shader = Shader.Find("Unlit/Transparent");
                //BloodObject2.name = "RedSkillObject" + i.ToString();
                //BloodObject2.transform.localScale = new Vector3(2.15f, 2.15f, 2.15f);
                //BloodObject2.transform.Rotate(90, 0, 0);
                //BloodObject2.transform.position = ListPosition[i] + new Vector3(0, 0.01f, 0);
                //BloodObject2.transform.parent = BloodObject.transform;

                //BloodObject.SetActive(false);

                GameObject go1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                DestroyImmediate(go1.GetComponent("MeshCollider"));
                go1.renderer.material = BPositionMaterial;

                go1.name = "Position" + i.ToString();
                go1.transform.localScale = new Vector3(OffsetX * 1.25f, OffsetX * 1.25f, OffsetX * 1.25f);
                go1.transform.Rotate(90, 90, 0);
                go1.transform.position = ListPosition[i] + new Vector3(0, 0.02f, 0);
                go1.transform.parent = MyPositions.transform;
            }
            else if (CreateType == 1)
            {
                //ListBase[i].renderer.material.mainTexture = Resources.Load("Game/bposition", typeof(Texture)) as Texture;
                ListBase[i].renderer.material = BPositionMaterial;

                if (!IsFight)
                {
                    //ListMove[i].renderer.material.mainTexture = Resources.Load("Game/blank", typeof(Texture)) as Texture;
                    ListBase[i].renderer.material = BlankMaterial;
                }
            }
            else
            {
                //ListBase[i].renderer.material.mainTexture = Resources.Load("Game/bposition", typeof(Texture)) as Texture;
                ListBase[i].renderer.material = BPositionMaterial;
                if (!IsFight)
                {
                    //ListMove[i].renderer.material.mainTexture = Resources.Load("Game/blank", typeof(Texture)) as Texture;
                    ListBase[i].renderer.material = BlankMaterial;
                }
            }
        }
        for (int i = 1; i < PositionRow * (PositionColumn / 2) + 1; i++)
        {
            if (PositionRow == 3)
            {
                if (i == 1 || i == 9)
                {
                    continue;
                }
            }
            else if (PositionRow == 4)
            {
                if (i == 1 || i == 2 || i == 12 || i == 14 || i == 15 || i == 16)
                {
                    continue;
                }
            }
            else if (PositionRow == 5)
            {
                if (i == 1 || i == 2 || i == 3 || i == 4 || i == 6 || i == 7 || i == 20)
                {
                    continue;
                }
                if (i == 19)
                {
                    i = 21;
                }
            }

            if (CreateType == 0)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //DestroyImmediate(go.GetComponent("MeshCollider"));
                go.renderer.material = BPositionMaterial;

                go.name = "Position" + i.ToString();
                go.transform.localScale = new Vector3(OffsetX * 1.25f, OffsetX * 1.25f, OffsetX * 1.25f);
                go.transform.Rotate(90, 90, 0);
                go.transform.position = ListPosition[i] + new Vector3(0, 0.02f, 0);
                go.transform.parent = MyPositions.transform;

                StartCoroutine(ShowBase(i, "position"));
            }
            else
            {
                if (!IsFight)
                {
                    //ListBase[i].renderer.material.mainTexture = Resources.Load("Game/position", typeof(Texture)) as Texture;
                    ListBase[i].renderer.material = PositionMaterial;
                }
                else
                {
                    //ListBase[i].renderer.material.mainTexture = Resources.Load("Game/bposition", typeof(Texture)) as Texture;
                    ListBase[i].renderer.material = BPositionMaterial;
                }
            }
        }

        for (int i = PositionRow * (PositionColumn / 2) + 1; i < PositionRow * PositionColumn + 1; i++)
        {
            if (PositionRow == 3)
            {
                if (i == 10 || i == 11 || i == 12 || i == 13 || i == 21)
                {
                    continue;
                }
            }
            else if (PositionRow == 4)
            {
                if (i == 14 || i == 15 || i == 16 || i == 17 || i == 18 || i == 28 || i == 30 || i == 31 || i == 32)
                {
                    continue;
                }
            }
            else if (PositionRow == 5)
            {
                if (i == 21 || i == 22 || i == 23 || i == 24 || i == 26 || i == 27 || i == 39 || i == 40 || i == 42 || i == 43 || i == 44 || i == 45)
                {
                    continue;
                }
                if (i == 19)
                {
                    i = 21;
                }
            }

            if (CreateType == 0)
            {
                //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //DestroyImmediate(go.GetComponent("MeshCollider"));
                //go.renderer.material.mainTexture = Resources.Load("Game/eposition", typeof(Texture)) as Texture;
                //go.renderer.material.shader = Shader.Find("Unlit/Transparent Colored");

                //go.name = "Enemy" + i.ToString();
                //go.transform.localScale = new Vector3(OffsetX * 1.25f, OffsetX * 1.25f, OffsetX * 1.25f);
                //go.transform.Rotate(90, 90, 0);
                //go.transform.position = ListPosition[i] + new Vector3(0, 0.02f, 0);
                //go.transform.parent = MyPositions.transform;

                if (GameObject.Find("Position" + i.ToString()) != null)
                {
                    GameObject.Find("Position" + i.ToString()).renderer.material = BPositionMaterial;
                }

                StartCoroutine(ShowBase(i, "eposition"));
            }
            else
            {
                if (!IsFight)
                {
                    //ListBase[i].renderer.material.mainTexture = Resources.Load("Game/eposition", typeof(Texture)) as Texture;
                    ListBase[i].renderer.material = EPositionMaterial;
                }
                else
                {
                    //ListBase[i].renderer.material.mainTexture = Resources.Load("Game/bposition", typeof(Texture)) as Texture;
                    ListBase[i].renderer.material = BPositionMaterial;
                }
            }
        }
        if (!IsFight && !IsFightFinish)
        {
            MyPositions.SetActive(false);
        }
    }

    IEnumerator ShowBase(int PositionID, string PictureName)
    {
        //yield return new WaitForSeconds(0.01f);
        yield return new WaitForEndOfFrame();
        //ListBase[PositionID].renderer.material.mainTexture = Resources.Load("Game/" + PictureName, typeof(Texture)) as Texture;
        if (PictureName == "position")
        {
            ListBase[PositionID].renderer.material = PositionMaterial;
        }
        else if (PictureName == "eposition")
        {
            ListBase[PositionID].renderer.material = EPositionMaterial;
        }
        else if (PictureName == "bposition")
        {
            ListBase[PositionID].renderer.material = BPositionMaterial;
        }
        else
        {
            ListBase[PositionID].renderer.material = BlankMaterial;
        }
    }

    #endregion

    #region StartFight
    public void StartFight()
    {
        IsLock = false;
        IsEnemyLock = false;
        IsResult = false;
        IsFirstBlood = false;
        FightStyle = 0;
        AttackCount = 0;
        KillEnemyID = 0;
        BossEnemyID = 0;
        LimitLose = 200;
        LimitWin = 999;
        LimitTime = 999;
        LimitTimer = 0;
        NPCID = 0;
        NPCPosition = 0;
        LimitBlood = 0;
        PermissionType = 0;
        LimitType = 0;
        LimitStar2 = 0;
        LimitStar3 = 0;
        LimitStarCount2 = 0;
        LimitStarCount3 = 0;
        LimitHeroID = 0;
        LimitHeroCount = 0;
        RandomDebuff = false;
        RandomBuff = false;
        RandomDebuffID = 0;
        RandomBuffID = 0;

        StopAllCoroutines();

        ListNPCTactics.Clear();
        TextTranslator.Gate g = TextTranslator.instance.GetGateByID(SceneTransformer.instance.NowGateID);
        SceneTransformer.instance.NowSceneID = g.mapID;

        WeatherID = g.battleMapID;
        GateNeedLevel = g.level;
        GateTerrain = g.terrain;
        BossEnemyID = g.bossID;
        ListTerrainID.Clear();
        ListTerrainPosition.Clear();
        if (g.scriptID1 > 0)
        {
            GateLimit gl = TextTranslator.instance.GetGateLimitDicByID(g.scriptID1);
            switch (gl.LimitTerm)
            {
                case 1:
                    LimitBlood = gl.LimitParam1;
                    break;
                case 2:
                    PermissionType = 1;
                    break;
                case 3:
                    PermissionType = 2;
                    break;
                case 4:
                    PermissionType = 3;
                    break;
                case 5:
                    LimitType = 1;
                    break;
                case 6:
                    LimitType = 2;
                    break;
                case 7:
                    LimitType = 3;
                    break;
                case 8:
                    LimitHeroCount = gl.LimitParam1;
                    break;
                case 9:
                    break;
                case 10:
                    RandomBuff = true;
                    RandomBuffID = gl.LimitParam1;
                    break;
                case 11:
                    RandomDebuff = true;
                    RandomDebuffID = gl.LimitParam1;
                    break;
                case 12:
                    break;
                case 13:
                    LimitHeroID = gl.LimitParam1;
                    break;
                case 14:
                    LimitTime = gl.LimitParam1;
                    break;
            }

            switch (gl.WinTerm)
            {
                case 1:
                    break;
                case 2:
                    KillEnemyID = gl.WinParam1;
                    break;
                case 3:
                    NPCID = gl.WinParam1;
                    NPCPosition = gl.WinParam2;
                    break;
                case 4:
                    LimitWin = gl.WinParam1;
                    break;
                case 5:
                    LimitLose = gl.WinParam1;
                    break;
                case 6:
                    NPCID = gl.WinParam1;
                    NPCPosition = gl.WinParam2;
                    break;
            }

            switch (gl.Star2Term)
            {
                case 1:
                    LimitStar2 = 1;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
                case 2:
                    LimitStar2 = 2;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
                case 3:
                    LimitStar2 = 3;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
            }

            switch (gl.Star3Term)
            {
                case 1:
                    LimitStar3 = 1;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
                case 2:
                    LimitStar3 = 2;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
                case 3:
                    LimitStar3 = 3;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
            }
        }
        if (g.scriptID2 > 0)
        {
            ListNPCTactics = TextTranslator.instance.GetNPCTacticsByGroupID(g.scriptID2);
        }

        PositionRow = 5;
        SetListPosition(PositionRow, 1);
        StartCoroutine(NewTest(""));
    }
    public void StartPVP(string PVP)
    {
        PVPString = PVP;
        IsResult = false;
        IsLock = false;
        IsEnemyLock = false;
        IsFirstBlood = false;
        AttackCount = 0;
        KillEnemyID = 0;
        BossEnemyID = 0;
        LimitLose = 200;
        LimitWin = 999;
        LimitTime = 999;
        LimitTimer = 0;
        NPCID = 0;
        NPCPosition = 0;
        LimitBlood = 0;
        PermissionType = 0;
        LimitType = 0;
        LimitStar2 = 0;
        LimitStar3 = 0;
        LimitStarCount2 = 0;
        LimitStarCount3 = 0;
        LimitHeroID = 0;
        LimitHeroCount = 0;
        RandomDebuff = false;
        RandomBuff = false;
        RandomDebuffID = 0;
        RandomBuffID = 0;

        StopAllCoroutines();
        ListNPCTactics.Clear();

        int GateID = 10000 + UnityEngine.Random.Range(1, 7);

        TextTranslator.Gate g = TextTranslator.instance.GetGateByID(GateID);
        SceneTransformer.instance.NowSceneID = g.mapID;
        WeatherID = g.battleMapID;

        PositionRow = 5;
        SetListPosition(PositionRow, 1);
        StartCoroutine(NewTest(PVP));
    }

    public void StartLegionGate(string LegionGate)
    {
        PVPString = LegionGate;
        IsResult = false;
        IsLock = false;
        IsEnemyLock = false;
        IsFirstBlood = false;
        AttackCount = 0;
        KillEnemyID = 0;
        BossEnemyID = 0;
        LimitLose = 200;
        LimitWin = 999;
        LimitTime = 999;
        LimitTimer = 0;
        NPCID = 0;
        NPCPosition = 0;
        LimitBlood = 0;
        PermissionType = 0;
        LimitType = 0;
        LimitStar2 = 0;
        LimitStar3 = 0;
        LimitStarCount2 = 0;
        LimitStarCount3 = 0;
        LimitHeroID = 0;
        LimitHeroCount = 0;
        RandomDebuff = false;
        RandomBuff = false;
        RandomDebuffID = 0;
        RandomBuffID = 0;

        StopAllCoroutines();
        ListNPCTactics.Clear();
        TextTranslator.Gate g = TextTranslator.instance.GetGateByID(SceneTransformer.instance.NowGateID);
        SceneTransformer.instance.NowSceneID = g.mapID;
        WeatherID = g.battleMapID;
        GateNeedLevel = g.level;
        GateTerrain = g.terrain;
        BossEnemyID = g.bossID;
        ListTerrainID.Clear();
        ListTerrainPosition.Clear();
        if (g.scriptID1 > 0)
        {
            GateLimit gl = TextTranslator.instance.GetGateLimitDicByID(g.scriptID1);
            switch (gl.LimitTerm)
            {
                case 1:
                    LimitBlood = gl.LimitParam1;
                    break;
                case 2:
                    PermissionType = 1;
                    break;
                case 3:
                    PermissionType = 2;
                    break;
                case 4:
                    PermissionType = 3;
                    break;
                case 5:
                    LimitType = 1;
                    break;
                case 6:
                    LimitType = 2;
                    break;
                case 7:
                    LimitType = 3;
                    break;
                case 8:
                    LimitHeroCount = gl.LimitParam1;
                    break;
                case 9:
                    break;
                case 10:
                    RandomBuff = true;
                    RandomBuffID = gl.LimitParam1;
                    break;
                case 11:
                    RandomDebuff = true;
                    RandomDebuffID = gl.LimitParam1;
                    break;
                case 12:
                    break;
                case 13:
                    LimitHeroID = gl.LimitParam1;
                    break;
                case 14:
                    LimitTime = gl.LimitParam1;
                    break;
            }

            switch (gl.WinTerm)
            {
                case 1:
                    break;
                case 2:
                    KillEnemyID = gl.WinParam1;
                    break;
                case 3:
                    NPCID = gl.WinParam1;
                    NPCPosition = gl.WinParam2;
                    break;
                case 4:
                    LimitWin = gl.WinParam1;
                    break;
                case 5:
                    LimitLose = gl.WinParam1;
                    break;
            }

            switch (gl.Star2Term)
            {
                case 1:
                    LimitStar2 = 1;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
                case 2:
                    LimitStar2 = 2;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
                case 3:
                    LimitStar2 = 3;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
            }

            switch (gl.Star3Term)
            {
                case 1:
                    LimitStar3 = 1;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
                case 2:
                    LimitStar3 = 2;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
                case 3:
                    LimitStar3 = 3;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
            }
        }

        PositionRow = 5;
        SetListPosition(PositionRow, 1);
        StartCoroutine(NewTest(LegionGate));
    }


    public void StartLegionWar()
    {
        IsResult = false;
        IsLock = false;
        IsEnemyLock = false;
        IsFirstBlood = false;
        IsLegionWar = false;
        ContinueWin = 0;
        AttackCount = 0;
        KillEnemyID = 0;
        BossEnemyID = 0;
        LimitLose = 200;
        LimitWin = 999;
        LimitTime = 999;
        LimitTimer = 0;
        NPCID = 0;
        NPCPosition = 0;
        LimitBlood = 0;
        PermissionType = 0;
        LimitType = 0;
        LimitStar2 = 0;
        LimitStar3 = 0;
        LimitStarCount2 = 0;
        LimitStarCount3 = 0;
        LimitHeroID = 0;
        LimitHeroCount = 0;
        RandomDebuff = false;
        RandomBuff = false;
        RandomDebuffID = 0;
        RandomBuffID = 0;

        StopAllCoroutines();
        ListNPCTactics.Clear();
        TextTranslator.Gate g = TextTranslator.instance.GetGateByID(SceneTransformer.instance.NowGateID);
        SceneTransformer.instance.NowSceneID = g.mapID;
        WeatherID = g.battleMapID;
        GateNeedLevel = g.level;
        GateTerrain = g.terrain;
        BossEnemyID = g.bossID;
        ListTerrainID.Clear();
        ListTerrainPosition.Clear();
        if (g.scriptID1 > 0)
        {
            GateLimit gl = TextTranslator.instance.GetGateLimitDicByID(g.scriptID1);
            switch (gl.LimitTerm)
            {
                case 1:
                    LimitBlood = gl.LimitParam1;
                    break;
                case 2:
                    PermissionType = 1;
                    break;
                case 3:
                    PermissionType = 2;
                    break;
                case 4:
                    PermissionType = 3;
                    break;
                case 5:
                    LimitType = 1;
                    break;
                case 6:
                    LimitType = 2;
                    break;
                case 7:
                    LimitType = 3;
                    break;
                case 8:
                    LimitHeroCount = gl.LimitParam1;
                    break;
                case 9:
                    break;
                case 10:
                    RandomBuff = true;
                    RandomBuffID = gl.LimitParam1;
                    break;
                case 11:
                    RandomDebuff = true;
                    RandomDebuffID = gl.LimitParam1;
                    break;
                case 12:
                    break;
                case 13:
                    LimitHeroID = gl.LimitParam1;
                    break;
                case 14:
                    LimitTime = gl.LimitParam1;
                    break;
            }

            switch (gl.WinTerm)
            {
                case 1:
                    break;
                case 2:
                    KillEnemyID = gl.WinParam1;
                    break;
                case 3:
                    NPCID = gl.WinParam1;
                    NPCPosition = gl.WinParam2;
                    break;
                case 4:
                    LimitWin = gl.WinParam1;
                    break;
                case 5:
                    LimitLose = gl.WinParam1;
                    break;
            }

            switch (gl.Star2Term)
            {
                case 1:
                    LimitStar2 = 1;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
                case 2:
                    LimitStar2 = 2;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
                case 3:
                    LimitStar2 = 3;
                    LimitStarCount2 = gl.Star2Param1;
                    break;
            }

            switch (gl.Star3Term)
            {
                case 1:
                    LimitStar3 = 1;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
                case 2:
                    LimitStar3 = 2;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
                case 3:
                    LimitStar3 = 3;
                    LimitStarCount3 = gl.Star3Param1;
                    break;
            }
        }

        PositionRow = 5;
        SetListPosition(PositionRow, 1);
        StartCoroutine(NewTest(""));
    }
    public void StartWood(string Wood, int NowFloor, int NowStar, int NowFight)
    {
        PVPString = Wood;
        WoodFloor = NowFloor;
        WoodStar = NowStar;
        WoodFight = NowFight;
        IsLock = false;
        IsEnemyLock = false;
        IsResult = false;
        IsFirstBlood = false;
        FightStyle = 2;
        AttackCount = 0;
        KillEnemyID = 0;
        BossEnemyID = 0;
        LimitLose = 200;
        LimitWin = 999;
        LimitTime = 999;
        LimitTimer = 0;
        NPCID = 0;
        NPCPosition = 0;
        LimitBlood = 0;
        PermissionType = 0;
        LimitType = 0;
        LimitStar2 = 0;
        LimitStar3 = 0;
        LimitStarCount2 = 0;
        LimitStarCount3 = 0;
        LimitHeroID = 0;
        LimitHeroCount = 0;
        RandomDebuff = false;
        RandomBuff = false;
        RandomDebuffID = 0;
        RandomBuffID = 0;

        StopAllCoroutines();
        ListNPCTactics.Clear();

        LimitStar2 = 1;
        LimitStarCount2 = 1;
        LimitStar3 = 1;
        LimitStarCount3 = 0;

        int GateID = 10000 + UnityEngine.Random.Range(1, 4);
        TextTranslator.Gate g = TextTranslator.instance.GetGateByID(GateID);
        SceneTransformer.instance.NowSceneID = g.mapID;
        WeatherID = g.battleMapID;

        PositionRow = 5;
        SetListPosition(PositionRow, 1);
        StartCoroutine(NewTest(Wood));
    }
    #endregion

    #region StopFight
    public void StopFight(bool IsDestroy)
    {
        IsRoleInGate = false;
        WeatherTimer = 0;
        WeatherID = 0;
        IsFight = false;
        MyCamera.SetActive(true);
        //MySkill.SetActive(false);
        MyBases.SetActive(false);
        MyMoves.SetActive(false);
        MyPositions.SetActive(false);
        ActObject.SetActive(false);
        EffectMaker.instance.Destroy2Deffect();
        StopAllCoroutines();

        for (int j = 0; j < ListPosition.Count; j++)
        {
            Destroy(GameObject.Find("TerrainObject" + j.ToString()));
        }

        for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
        {
            if (ListMove[i] != null)
            {
                ListMove[i].renderer.material = BlankMaterial;
            }
        }

        if (IsDestroy)
        {
            StartCoroutine(CloseFight());
        }
    }
    IEnumerator CloseFight()
    {
        IsSkip = false;
        DestroyAllComponent();
        yield return new WaitForSeconds(0.04f);
        DestroyImmediate(GameObject.Find("FightScene"));
        yield return new WaitForSeconds(0.01f);
        if (FightStyle != 2)
        {
            RenderSettings.fog = false;
            RenderSettings.ambientLight = Color.white;            
            LightmapSettings.lightmaps = null;
        }
    }
    #endregion

    #region InitFight
    public IEnumerator NewTest(string PVP)
    {
        DestroyAllComponent();
        ListOpenNode.Clear();
        ListCloseNode.Clear();
        ListFindNode.Clear();
        SceneTransformer.instance.ShowMainScene(false);
        MyBases.SetActive(true);
        IsRoleInGate = true;
        FightCount = 0;
        FightCalculate = 1;
        yield return new WaitForSeconds(0.01f);
        UIManager.instance.OpenPanel("FightWindow", false);
        fw = GameObject.Find("FightWindow").GetComponent<FightWindow>();
        yield return new WaitForSeconds(0.01f);

        if (FightStyle == 2)
        {
            NetworkHandler.instance.SendProcess("1510#;");
        }

        for (int i = 0; i < 8; i++)
        {
            SequenceTexture[i] = GameObject.Find("Texture" + (i + 1).ToString()).GetComponent<UISprite>();
            SequenceSprite[i] = GameObject.Find("Texture" + (i + 1).ToString()).transform.Find("SpriteEdge").gameObject.GetComponent<UISprite>();
        }

        FightScene = GameObject.Find("FightScene");
        if (FightScene == null)
        {
            RenderSettings.fog = false;
            RenderSettings.skybox = (Material)Resources.Load("Skybox/Skybox1");
            FightScene = GameObject.Instantiate(Resources.Load("Prefab/Scene/Map" + SceneTransformer.instance.NowSceneID.ToString("00"), typeof(GameObject))) as GameObject;
            BattleMap bm = TextTranslator.instance.battleMapDic[SceneTransformer.instance.NowSceneID * 10 + PictureCreater.instance.WeatherID];

            LightMapAsset lightAsset = Resources.Load("LightMapAsset/lightMapAsset_" + bm.lightMapNum) as LightMapAsset;
            int count = lightAsset.lightmapFar.Length;
            LightmapData[] lightmapDatas = new LightmapData[count];
            for (int i = 0; i < count; i++)
            {
                LightmapData lightmapData = new LightmapData();
                lightmapData.lightmapFar = lightAsset.lightmapFar[i];
                lightmapData.lightmapNear = lightAsset.lightmapNear[i];
                lightmapDatas[i] = lightmapData;
            }
            LightmapSettings.lightmaps = lightmapDatas;

            if (PlayerPrefs.GetFloat("ElectractySlider") == 0) //耗电模式
            {
                DestroyImmediate(GameObject.Find("sun"));
                DestroyImmediate(GameObject.Find("huangsha"));
                DestroyImmediate(GameObject.Find("shandian"));
                DestroyImmediate(GameObject.Find("xiayu"));
                DestroyImmediate(GameObject.Find("Fire_and_Light"));
                DestroyImmediate(GameObject.Find("eff"));
            }

            //GameObject.Find("DirectLight").GetComponent<Light>().intensity = bm.DirectLight;
            //GameObject.Find("DirectLight").GetComponent<Light>().color = new Color32((byte)bm.DirectLightR, (byte)bm.DirectLightG, (byte)bm.DirectLightB, 255);

            //GameObject.Find("DirectLight").SetActive(false);

            //if (PlayerPrefs.GetFloat("ElectractySlider") > 0)
            //{
            //    GameObject.Find("DarkLight").GetComponent<Light>().intensity = bm.DarkLight;
            //    GameObject.Find("DarkLight").GetComponent<Light>().color = new Color32((byte)bm.DarkLightR, (byte)bm.DarkLightG, (byte)bm.DarkLightB, 255);

            //    GameObject.Find("BackLight").GetComponent<Light>().intensity = bm.BackLight;
            //    GameObject.Find("BackLight").GetComponent<Light>().color = new Color32((byte)bm.BackLightR, (byte)bm.BackLightG, (byte)bm.BackLightB, 255);


            //    GameObject.Find("DarkLight").SetActive(false);
            //    GameObject.Find("BackLight").SetActive(false);
            //}
            //else
            //{
            //    GameObject.Find("DarkLight").SetActive(false);
            //    GameObject.Find("BackLight").SetActive(false);
            //}
        }


        if (PlayerPrefs.GetFloat("EffectSlider") > 0)
        {
            PictureCreater.instance.SetWeather(WeatherID);
        }
        FightScene.transform.rotation = new Quaternion(0, 0, 0, 0);
        FightScene.transform.localScale = Vector3.one;
        FightScene.name = "FightScene";

        IsFight = false;
        IsFightFinish = false;
        FightTimer = 0;
        NowSequence = 1;

        yield return new WaitForEndOfFrame();
        FightSkill1 = CharacterRecorder.instance.ListManualSkillId[0];
        FightSkill2 = CharacterRecorder.instance.ListManualSkillId[1];
        FightSkill3 = CharacterRecorder.instance.ListManualSkillId[2];

        IsAll = false;
        MyUISliderValue = 1f;

        SkillFire1 = 0;
        SkillFire2 = 0;
        SkillFire3 = 0;

        CameraY = 35;
        FieldView = 60;
        MyCamera.GetComponent<Camera>().fieldOfView = FieldView;
        //FightCamera.GetComponent<Camera>().fieldOfView = MyCamera.GetComponent<Camera>().fieldOfView;
        MyCamera.GetComponent<MouseClick>().MoveOffect = 0;
        MyCamera.GetComponent<MouseClick>().MoveOffectY = 0;
        MyCamera.GetComponent<MouseClick>().TouchPosition = 0;
        MyCamera.GetComponent<MouseClick>().TouchPositionY = 0;
        MyCamera.GetComponent<MouseClick>().MovePosition = 0;
        MyCamera.GetComponent<MouseClick>().MovePositionY = 0;
        FightScene.transform.position = new Vector3(0, -0.1f, 0);

        MyCamera.transform.position = new Vector3(0, 8.8f, -6.6f);
        MyCamera.transform.rotation = new Quaternion(0, 0, 0, 0);

        foreach (Component c in MyPositions.GetComponentsInChildren(typeof(MeshCollider), true))
        {
            c.gameObject.GetComponent<MeshCollider>().enabled = true;
        }

        yield return new WaitForEndOfFrame();
        fw.MyUISlider.value = MyUISliderValue;

        for (int j = 0; j < ListPosition.Count; j++)
        {
            Destroy(GameObject.Find("TerrainObject" + j.ToString()));
        }
        ListTerrainPosition.Clear();
        ListTerrainID.Clear();
        ListStopPosition.Clear();

        if (FightStyle == 2)
        {
            fw.Tactics.SetActive(false);
            fw.TacticsInfo.SetActive(false);

            string[] dataSplit = PVP.Split('!');

            int StartNum = 2;
            if (PlayerPrefs.GetInt("ThreatBuff" + "_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID")) == 1)
            {
                StartNum = 4;
            }
            for (int i = StartNum; i < dataSplit.Length - 1; i++)
            {
                string[] secSplit = dataSplit[i].Split('$');
                HeroInfo h = TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(secSplit[0]));
                switch (h.heroCarrerType)
                {
                    case 1://攻
                        CreateRole(int.Parse(secSplit[0]), "Enemy" + secSplit[0] + "_" + ListEnemyPicture.Count.ToString() + "_" + secSplit[1], int.Parse(secSplit[1]), Color.red, WoodFight * 0.311111f, WoodFight / 15 * 5.6f, h.heroHit * 1000f + (WoodFloor + 1) * 5f, h.heroSpeed / 10f, true, false, 1, 1.5f, h.heroNoHit * 1000f + (WoodFloor + 1) * 5f, secSplit[0], 0, WoodFight * 0.07777778f, h.heroPcritical * 1000f + (WoodFloor + 1) * 5f, WoodFight * 0.00777778f, h.heroNoCri * 1000f + (WoodFloor + 1) * 5f, h.heroDamigeAdd * 1000f, h.heroDamigeReduce * 1000f, h.heroSkillList[0], h.heroSkillList[1], 0, 1, h.heroAi, h.heroArea, h.heroMove, 0, "");
                        break;
                    case 2://防
                        CreateRole(int.Parse(secSplit[0]), "Enemy" + secSplit[0] + "_" + ListEnemyPicture.Count.ToString() + "_" + secSplit[1], int.Parse(secSplit[1]), Color.red, WoodFight * 0.4666667f, WoodFight * 0.56f, h.heroHit * 1000f + (WoodFloor + 1) * 5f, h.heroSpeed / 10f, true, false, 1, 1.5f, h.heroNoHit * 1000f + (WoodFloor + 1) * 5f, secSplit[0], 0, WoodFight * 0.0466667f, h.heroPcritical * 1000f + (WoodFloor + 1) * 5f, WoodFight * 0.0233333f, h.heroNoCri * 1000f + (WoodFloor + 1) * 5f, h.heroDamigeAdd * 1000f, h.heroDamigeReduce * 1000f, h.heroSkillList[0], h.heroSkillList[1], 0, 1, h.heroAi, h.heroArea, h.heroMove, 0, "");
                        break;
                    case 3://特
                        CreateRole(int.Parse(secSplit[0]), "Enemy" + secSplit[0] + "_" + ListEnemyPicture.Count.ToString() + "_" + secSplit[1], int.Parse(secSplit[1]), Color.red, WoodFight * 0.388889f, WoodFight / 15 * 7f, h.heroHit * 1000f + (WoodFloor + 1) * 5f, h.heroSpeed / 10f, true, false, 1, 1.5f, h.heroNoHit * 1000f + (WoodFloor + 1) * 5f, secSplit[0], 0, WoodFight * 0.062222f, h.heroPcritical * 1000f + (WoodFloor + 1) * 5f, WoodFight * 0.0155556f, h.heroNoCri * 1000f + (WoodFloor + 1) * 5f, h.heroDamigeAdd * 1000f, h.heroDamigeReduce * 1000f, h.heroSkillList[0], h.heroSkillList[1], 0, 1, h.heroAi, h.heroArea, h.heroMove, 0, "");
                        break;
                }

            }
        }

        if (FightStyle == 6 || FightStyle == 7)
        {
            fw.Tactics.SetActive(false);
            fw.TacticsInfo.SetActive(false);

            List<GateMap> ListGateMap = new List<GateMap>();
            ListGateMap = TextTranslator.instance.GetGateMapByID(SceneTransformer.instance.NowGateID);

            for (int i = 0; i < ListGateMap.Count; i++)
            {
                for (int j = 0; j < TextTranslator.instance.enemyInfoList.Count; j++)
                {
                    if (ListGateMap[i].MonsterID == TextTranslator.instance.enemyInfoList[j].enemyID)
                    {
                        if (ListGateMap[i].PosID > 0)
                        {
                            Debug.LogError(TextTranslator.instance.enemyInfoList[j].hp + " " + ListGateMap[i].HpRate);
                            CreateRole(TextTranslator.instance.enemyInfoList[j].pic, "Enemy" + TextTranslator.instance.enemyInfoList[j].enemyID + "_" + ListEnemyPicture.Count.ToString() + "_" + ListGateMap[i].PosID.ToString(), ListGateMap[i].PosID, Color.cyan, (LimitBlood > 0) ? LimitBlood : TextTranslator.instance.enemyInfoList[j].hp * ListGateMap[i].HpRate / 100f, (LimitBlood > 0) ? LimitBlood : TextTranslator.instance.enemyInfoList[j].hp * ListGateMap[i].HpRate / 100f, TextTranslator.instance.enemyInfoList[j].hit * 1000f, TextTranslator.instance.enemyInfoList[j].spd / 10f, true, false, 1, 1.5f, TextTranslator.instance.enemyInfoList[j].noHit * 1000f, TextTranslator.instance.enemyInfoList[j].enemyID.ToString(), 0, TextTranslator.instance.enemyInfoList[j].atk * ListGateMap[i].AtkRate / 100f, TextTranslator.instance.enemyInfoList[j].crit * 1000f, TextTranslator.instance.enemyInfoList[j].def * ListGateMap[i].DefRate / 400f, TextTranslator.instance.enemyInfoList[j].noCrit * 1000f, TextTranslator.instance.enemyInfoList[j].dmgBonus * 1000f, TextTranslator.instance.enemyInfoList[j].dmgReduce * 1000f, TextTranslator.instance.enemyInfoList[j].skill[0], TextTranslator.instance.enemyInfoList[j].skill[1], TextTranslator.instance.enemyInfoList[j].bossAi, 1, TextTranslator.instance.enemyInfoList[j].ai, 999, 0, ListGateMap[i].InitialRage, "");
                        }
                    }
                }
            }
        }
        else if (FightStyle == 0 || FightStyle == 4 || FightStyle == 5 || FightStyle == 8 || FightStyle == 9 || FightStyle == 10 || FightStyle == 11 || FightStyle == 17)
        {

            CreateTerrain();

            List<GateMap> ListGateMap = new List<GateMap>();
            ListGateMap = TextTranslator.instance.GetGateMapByID(SceneTransformer.instance.NowGateID);

            int NeedForce = TextTranslator.instance.GetGateByID(SceneTransformer.instance.NowGateID).needForce;
            if (NeedForce > 0)
            {
                if (CharacterRecorder.instance.Fight < NeedForce)
                {
                    FightCalculate = 1 / (1.4f + (NeedForce - CharacterRecorder.instance.Fight) * 0.001f / CharacterRecorder.instance.level);
                    if (FightCalculate < 0.4f)
                    {
                        FightCalculate = 0.4f;
                    }
                    Debug.LogError("FightCalculate:" + FightCalculate + " " + NeedForce + " " + CharacterRecorder.instance.Fight);
                }
                else
                {
                    EnemyCalculate = 1 / (1 + (CharacterRecorder.instance.Fight - NeedForce) * 0.001f / CharacterRecorder.instance.level);
                    if (EnemyCalculate < 0.4f)
                    {
                        EnemyCalculate = 0.4f;
                    }
                }
            }
            //FightCalculate



            if (SceneTransformer.instance.NowGateID == 89998 || SceneTransformer.instance.NowGateID == 89999)
            {

                string[] dataSplit = CharacterRecorder.instance.EnemyInfoStr.Split(';');
                for (int i = 4; i < dataSplit.Length - 1; i++)
                {
                    string[] secSplit = dataSplit[i].Split('$');
                    HeroInfo h = TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(secSplit[0]));
                    int SkillPoint = int.Parse(secSplit[12]);
                    if (i == dataSplit.Length - 2)
                    {
                        SkillPoint += 500;
                        EnemyCaptainIndex = i;
                    }
                    CreateRole(int.Parse(secSplit[0]), h.heroName, int.Parse(secSplit[1]), Color.red, int.Parse(secSplit[2]), int.Parse(secSplit[2]), float.Parse(secSplit[8]), float.Parse(secSplit[15]) / 10f, true, false, 1, 1.5f, float.Parse(secSplit[7]), secSplit[0], 0, int.Parse(secSplit[3]), float.Parse(secSplit[5]), int.Parse(secSplit[4]) / 4f, float.Parse(secSplit[6]), float.Parse(secSplit[9]), float.Parse(secSplit[10]), h.heroSkillList[0], h.heroSkillList[1], 0, int.Parse(secSplit[13]), int.Parse(secSplit[16]), h.heroArea, h.heroMove, SkillPoint, "");
                }
            }
            else
            {
                for (int i = 0; i < ListGateMap.Count; i++)
                {
                    for (int j = 0; j < TextTranslator.instance.enemyInfoList.Count; j++)
                    {
                        if (ListGateMap[i].MonsterID == TextTranslator.instance.enemyInfoList[j].enemyID)
                        {
                            if (ListGateMap[i].PosID > 0)
                            {
                                int MonsterLevel = ListGateMap[i].MonsterLevel;
                                int HpRate = ListGateMap[i].HpRate;
                                int AtkRate = ListGateMap[i].AtkRate;
                                int DefRate = ListGateMap[i].DefRate;
                                if (FightStyle == 17)
                                {
                                    MonsterLevel = ListGateMap[i].MonsterLevel * CharacterRecorder.instance.NuclearLevel / 5;
                                    HpRate *= CharacterRecorder.instance.NuclearLevel;
                                    AtkRate *= CharacterRecorder.instance.NuclearLevel;
                                    DefRate *= CharacterRecorder.instance.NuclearLevel;
                                }
                                CreateRole(TextTranslator.instance.enemyInfoList[j].pic, "Enemy" + TextTranslator.instance.enemyInfoList[j].enemyID + "_" + ListEnemyPicture.Count.ToString() + "_" + ListGateMap[i].PosID.ToString(), ListGateMap[i].PosID, Color.red, (LimitBlood > 0) ? LimitBlood : TextTranslator.instance.enemyInfoList[j].hp * HpRate / 100f, (LimitBlood > 0) ? LimitBlood : TextTranslator.instance.enemyInfoList[j].hp * HpRate / 100f, (TextTranslator.instance.enemyInfoList[j].hit + Math.Max((MonsterLevel * 0.01f) - 0.2f, 0)) * 1000f, TextTranslator.instance.enemyInfoList[j].spd / 10f, true, false, 1, 1.5f, (TextTranslator.instance.enemyInfoList[j].noHit + Math.Max((MonsterLevel * 0.02f) - 0.4f, 0)) * 1000f, TextTranslator.instance.enemyInfoList[j].enemyID.ToString(), 0, TextTranslator.instance.enemyInfoList[j].atk * AtkRate / 100f, (TextTranslator.instance.enemyInfoList[j].crit + Math.Max((MonsterLevel * 0.005f) - 0.1f, 0)) * 1000f, TextTranslator.instance.enemyInfoList[j].def * DefRate / 400f, (TextTranslator.instance.enemyInfoList[j].noCrit + Math.Max((MonsterLevel * 0.03f) - 0.6f, 0)) * 1000f, TextTranslator.instance.enemyInfoList[j].dmgBonus * 1000f, TextTranslator.instance.enemyInfoList[j].dmgReduce * 1000f, TextTranslator.instance.enemyInfoList[j].skill[0], TextTranslator.instance.enemyInfoList[j].skill[1], TextTranslator.instance.enemyInfoList[j].bossAi, 1, TextTranslator.instance.enemyInfoList[j].ai, TextTranslator.instance.enemyInfoList[j].area, ListGateMap[i].MonsterRarity > 0 ? 0 : TextTranslator.instance.enemyInfoList[j].mv, ListGateMap[i].InitialRage, "");

                                if (KillEnemyID == TextTranslator.instance.enemyInfoList[j].enemyID)
                                {
                                    //ListEnemyPicture[ListEnemyPicture.Count - 1].RoleObject.GetComponent<ColliderDisplayText>().SetBoss();
                                    ListEnemyPicture[ListEnemyPicture.Count - 1].RoleObject.transform.localScale *= 1.2f;
                                    //GameObject Boss = GameObject.Instantiate(Resources.Load("Prefab/Effect/WF_BOSS_ui", typeof(GameObject))) as GameObject;
                                    //Boss.transform.parent = ListEnemyPicture[ListEnemyPicture.Count - 1].RoleObject.transform;
                                    //Boss.transform.localPosition = Vector3.zero;


                                    ////////////////////////BOSS发光////////////////////////
                                    if (ListEnemyPicture[ListEnemyPicture.Count - 1].RolePictureObject.transform.Find("HeroBody2") != null)
                                    {
                                        if (ListEnemyPicture[ListEnemyPicture.Count - 1].RolePictureObject.transform.Find("HeroBody") != null)
                                        {

                                            SkinnedMeshRenderer s = ListEnemyPicture[ListEnemyPicture.Count - 1].RolePictureObject.transform.Find("HeroBody").gameObject.GetComponent<SkinnedMeshRenderer>();
                                            Material[] ListMaterial = new Material[s.materials.Length];
                                            for (int n = 0; n < s.materials.Length - 1; n++)
                                            {
                                                ListMaterial[n] = s.materials[n];
                                            }
                                            ListMaterial[s.materials.Length - 1] = Boss1Material;
                                            ListMaterial[s.materials.Length - 2] = Boss2Material;
                                            s.materials = ListMaterial;

                                            SkinnedMeshRenderer s1 = ListEnemyPicture[ListEnemyPicture.Count - 1].RolePictureObject.transform.Find("HeroBody2").gameObject.GetComponent<SkinnedMeshRenderer>();
                                            Material[] ListMaterial1 = new Material[s1.materials.Length];
                                            for (int n = 0; n < s1.materials.Length - 1; n++)
                                            {
                                                ListMaterial1[n] = s1.materials[n];
                                            }
                                            ListMaterial1[s1.materials.Length - 1] = Boss1Material;
                                            ListMaterial1[s1.materials.Length - 2] = Boss2Material;
                                            s1.materials = ListMaterial1;
                                        }
                                    }
                                    else if (ListEnemyPicture[ListEnemyPicture.Count - 1].RolePictureObject.transform.Find("HeroBody") != null)
                                    {
                                        SkinnedMeshRenderer s = ListEnemyPicture[ListEnemyPicture.Count - 1].RolePictureObject.transform.Find("HeroBody").gameObject.GetComponent<SkinnedMeshRenderer>();
                                        Material[] ListMaterial = new Material[s.materials.Length];
                                        for (int n = 0; n < s.materials.Length - 1; n++)
                                        {
                                            ListMaterial[n] = s.materials[n];
                                        }
                                        ListMaterial[s.materials.Length - 1] = Boss1Material;
                                        ListMaterial[s.materials.Length - 2] = Boss2Material;
                                        s.materials = ListMaterial;
                                    }
                                    ////////////////////////BOSS发光////////////////////////
                                }
                                break;
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < TextTranslator.instance.enemyInfoList.Count; j++)
            {
                if (NPCID == TextTranslator.instance.enemyInfoList[j].enemyID)
                {
                    CreateRole(TextTranslator.instance.enemyInfoList[j].pic, "NPC" + TextTranslator.instance.enemyInfoList[j].enemyID, NPCPosition, Color.cyan, TextTranslator.instance.enemyInfoList[j].hp, TextTranslator.instance.enemyInfoList[j].hp, TextTranslator.instance.enemyInfoList[j].hit * 1000f, 99999, false, false, 1, 1.5f, TextTranslator.instance.enemyInfoList[j].noHit * 1000f, "Enemy" + TextTranslator.instance.enemyInfoList[j].enemyID.ToString(), 0, TextTranslator.instance.enemyInfoList[j].atk, TextTranslator.instance.enemyInfoList[j].crit * 1000f, TextTranslator.instance.enemyInfoList[j].def / 4, TextTranslator.instance.enemyInfoList[j].noCrit * 1000f, TextTranslator.instance.enemyInfoList[j].dmgBonus * 1000f, TextTranslator.instance.enemyInfoList[j].dmgReduce * 1000f, 0, 0, TextTranslator.instance.enemyInfoList[j].bossAi, 1, TextTranslator.instance.enemyInfoList[j].ai, 0, 0, 0, "");
                    break;
                }
            }

            if (RandomBuff)
            {
                int Index = 0;
                foreach (var h in ListEnemyPicture)
                {
                    if (PictureCreater.instance.RandomBuffID == 0)
                    {
                        Buff NewBuff = TextTranslator.instance.GetBuffByID(UnityEngine.Random.Range(709, 717));
                        RoleAddBuff(ListEnemyPicture, Index, NewBuff);
                    }
                    else
                    {
                        Buff NewBuff = TextTranslator.instance.GetBuffByID(PictureCreater.instance.RandomBuffID);
                        RoleAddBuff(ListEnemyPicture, Index, NewBuff);
                    }
                    Index++;
                }
            }
        }
        else if (FightStyle == 12)
        {
            string[] dataSplit = PVP.Split(';');
            string[] dataSplits = dataSplit[2].Split('$');
            LegionGateID = int.Parse(dataSplit[1]);
            LegionGroupID = int.Parse(dataSplit[0]);
            LegionGateString = dataSplit[2];
            LegionGate _LegionGate = TextTranslator.instance.GetLegionGateByGroupIDAndBoxID(LegionGroupID, LegionGateID);
            List<GateMap> ListGateMap = new List<GateMap>();
            SceneTransformer.instance.NowGateID = _LegionGate.NextGateID;
            ListGateMap = TextTranslator.instance.GetGateMapByID(SceneTransformer.instance.NowGateID);

            int k = 0;
            for (int i = 0; i < ListGateMap.Count; i++)
            {
                for (int j = 0; j < TextTranslator.instance.enemyInfoList.Count; j++)
                {
                    if (ListGateMap[i].MonsterID == TextTranslator.instance.enemyInfoList[j].enemyID)
                    {
                        if (ListGateMap[i].PosID > 0)
                        {
                            int MaxBlood = _LegionGate.BossBloodList[k];
                            Debug.Log(k);
                            Debug.Log(dataSplits[k]);
                            int NowBlood = int.Parse(dataSplits[k]);

                            if (NowBlood > 0)
                            {
                                CreateRole(TextTranslator.instance.enemyInfoList[j].pic, "Enemy" + TextTranslator.instance.enemyInfoList[j].enemyID + "_" + ListEnemyPicture.Count.ToString() + "_" + ListGateMap[i].PosID.ToString(), ListGateMap[i].PosID, Color.red, MaxBlood, NowBlood, TextTranslator.instance.enemyInfoList[j].hit * 1000f, TextTranslator.instance.enemyInfoList[j].spd / 10f, true, false, 1, 1.5f, TextTranslator.instance.enemyInfoList[j].noHit * 1000f, TextTranslator.instance.enemyInfoList[j].enemyID.ToString(), 0, TextTranslator.instance.enemyInfoList[j].atk * ListGateMap[i].AtkRate / 100f, TextTranslator.instance.enemyInfoList[j].crit * 1000f, TextTranslator.instance.enemyInfoList[j].def * ListGateMap[i].DefRate / 400f, TextTranslator.instance.enemyInfoList[j].noCrit * 1000f, TextTranslator.instance.enemyInfoList[j].dmgBonus * 1000f, TextTranslator.instance.enemyInfoList[j].dmgReduce * 1000f, TextTranslator.instance.enemyInfoList[j].skill[0], TextTranslator.instance.enemyInfoList[j].skill[1], TextTranslator.instance.enemyInfoList[j].bossAi, 1, TextTranslator.instance.enemyInfoList[j].ai, TextTranslator.instance.enemyInfoList[j].area, ListGateMap[i].MonsterRarity > 0 ? 0 : TextTranslator.instance.enemyInfoList[j].mv, ListGateMap[i].InitialRage, "");
                            }
                            else
                            {
                                CreateRole(TextTranslator.instance.enemyInfoList[j].pic, "Enemy" + TextTranslator.instance.enemyInfoList[j].enemyID + "_" + ListEnemyPicture.Count.ToString() + "_" + ListGateMap[i].PosID.ToString(), 0, Color.red, MaxBlood, NowBlood, TextTranslator.instance.enemyInfoList[j].hit * 1000f, TextTranslator.instance.enemyInfoList[j].spd / 10f, true, false, 1, 1.5f, TextTranslator.instance.enemyInfoList[j].noHit * 1000f, TextTranslator.instance.enemyInfoList[j].enemyID.ToString(), 0, TextTranslator.instance.enemyInfoList[j].atk * ListGateMap[i].AtkRate / 100f, TextTranslator.instance.enemyInfoList[j].crit * 1000f, TextTranslator.instance.enemyInfoList[j].def * ListGateMap[i].DefRate / 400f, TextTranslator.instance.enemyInfoList[j].noCrit * 1000f, TextTranslator.instance.enemyInfoList[j].dmgBonus * 1000f, TextTranslator.instance.enemyInfoList[j].dmgReduce * 1000f, TextTranslator.instance.enemyInfoList[j].skill[0], TextTranslator.instance.enemyInfoList[j].skill[1], TextTranslator.instance.enemyInfoList[j].bossAi, 1, TextTranslator.instance.enemyInfoList[j].ai, TextTranslator.instance.enemyInfoList[j].area, ListGateMap[i].MonsterRarity > 0 ? 0 : TextTranslator.instance.enemyInfoList[j].mv, ListGateMap[i].InitialRage, "");
                                ListEnemyPicture[k].RoleObject.SetActive(false);
                            }

                            if (KillEnemyID == TextTranslator.instance.enemyInfoList[j].enemyID)
                            {
                                //ListEnemyPicture[ListEnemyPicture.Count - 1].RoleObject.GetComponent<ColliderDisplayText>().SetBoss();
                                ListEnemyPicture[ListEnemyPicture.Count - 1].RoleObject.transform.localScale *= 1.2f;
                                GameObject Boss = GameObject.Instantiate(Resources.Load("Prefab/Effect/WF_BOSS_ui", typeof(GameObject))) as GameObject;
                                Boss.transform.parent = ListEnemyPicture[ListEnemyPicture.Count - 1].RoleObject.transform;
                                Boss.transform.localPosition = Vector3.zero;
                            }
                            k++;
                        }
                    }
                }
            }
        }
        else if (FightStyle == 1 || FightStyle == 3 || FightStyle == 13 || FightStyle == 15 || FightStyle == 16)
        {
            GateTerrain = "5$1!8$1!11$1!35$1!38$1!41$1!";
            CreateTerrain();

            fw.Tactics.SetActive(false);
            fw.TacticsInfo.SetActive(false);

            string[] dataSplit = PVP.Split(';');
            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                string[] secSplit = dataSplit[i].Split('$');
                HeroInfo h = TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(secSplit[0]));
                int SkillPoint = int.Parse(secSplit[12]);
                if (i == dataSplit.Length - 2)
                {
                    SkillPoint += 500;
                    EnemyCaptainIndex = i;
                }
                CreateRole(int.Parse(secSplit[0]), h.heroName, int.Parse(secSplit[1]), Color.red, int.Parse(secSplit[2]), int.Parse(secSplit[2]), float.Parse(secSplit[8]), float.Parse(secSplit[15]) / 10f, true, false, 1, 1.5f, float.Parse(secSplit[7]), secSplit[0], 0, int.Parse(secSplit[3]), float.Parse(secSplit[5]), int.Parse(secSplit[4]) / 4f, float.Parse(secSplit[6]), float.Parse(secSplit[9]), float.Parse(secSplit[10]), h.heroSkillList[0], h.heroSkillList[1], int.Parse(secSplit[14]), int.Parse(secSplit[13]), int.Parse(secSplit[16]), h.heroArea, h.heroMove, SkillPoint, "");
            }
        }
        else if (FightStyle == 14)
        {
            NetworkHandler.instance.SendProcess("8614#" + CharacterRecorder.instance.LegionHarasPoint + ";");
        }

        SetSequence();
        if (GameObject.Find("LoadingWindow") != null)
        {
            GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().IsClose = true;
        }
        yield return new WaitForSeconds(1f);

        if (IsRemember)
        {
#if !ClashRoyale
            for (int i = 0; i < ListRolePicture.Count; i++)
            {
                if (ListRolePicture[i].RolePosition > 0 && ListRolePicture[i].RoleObject.name.IndexOf("NPC") == -1)
                {
                    //Debug.LogError(ListRolePicture[i].RolePosition);
                    SetPictureForwardPosition(ListRolePicture, ListRolePicture[i].RolePosition, i);
                }
            }

            for (int i = 0; i < ListEnemyPicture.Count; i++)
            {
                if (ListEnemyPicture[i].RolePosition > 0)
                {
                    SetPictureForwardPosition(ListEnemyPicture, ListEnemyPicture[i].RolePosition, i);
                }
            }
#endif
        }
        else
        {
            StartCoroutine(fw.StartFight());
            if (PictureCreater.instance.FightStyle == 0 && !NetworkHandler.instance.IsCreate)
            {
                //ShrinkMessage.SetActive(true);
                //GateDescription.GetComponent<TweenPosition>().PlayForward();
                fw.ShrinkMessage.SetActive(false);
            }
            if (FightStyle == 0)
            {
                fw.BeginFight();
            }
        }
        yield return 0;
    }


    public void CreateLegionWar(string LegionWar, string Name)
    {
        IsRoleInGate = true;
        string[] dataSplit = LegionWar.Split(';');
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            HeroInfo h = TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(secSplit[0]));
            int SkillPoint = int.Parse(secSplit[12]);

            WeakNum = (100 - TextTranslator.instance.GetLegionWeakByWinNum(int.Parse(secSplit[18]))) / 100f;
            Debug.Log(WeakNum);

            if (i == dataSplit.Length - 2)
            {
                SkillPoint += 500;
                EnemyCaptainIndex = i;
            }
            if (int.Parse(secSplit[15]) > 0)
            {
                CreateRole(int.Parse(secSplit[0]), h.heroName, int.Parse(secSplit[1]), Color.red, int.Parse(secSplit[2]), int.Parse(secSplit[15]) * WeakNum, float.Parse(secSplit[8]), float.Parse(secSplit[17]) / 10f, true, false, 1, 1.5f, float.Parse(secSplit[7]), secSplit[0], int.Parse(secSplit[16]), int.Parse(secSplit[3]) * WeakNum, float.Parse(secSplit[5]), int.Parse(secSplit[4]) / 4f * WeakNum, float.Parse(secSplit[6]), float.Parse(secSplit[9]), float.Parse(secSplit[10]), h.heroSkillList[0], h.heroSkillList[1], int.Parse(secSplit[14]), int.Parse(secSplit[13]), int.Parse(secSplit[19]), h.heroArea, h.heroMove, SkillPoint, "");
            }
            else
            {
                CreateRole(int.Parse(secSplit[0]), h.heroName, 0, Color.red, int.Parse(secSplit[2]), int.Parse(secSplit[15]), float.Parse(secSplit[8]), h.heroSpeed / 10f, true, false, 1, 1.5f, float.Parse(secSplit[7]), secSplit[0], int.Parse(secSplit[16]), int.Parse(secSplit[3]), float.Parse(secSplit[5]), int.Parse(secSplit[4]) / 4f, float.Parse(secSplit[6]), float.Parse(secSplit[9]), float.Parse(secSplit[10]), h.heroSkillList[0], h.heroSkillList[1], int.Parse(secSplit[14]), int.Parse(secSplit[13]), h.heroAi, h.heroArea, h.heroMove, SkillPoint, "");
            }
        }

        for (int i = 0; i < ListEnemyPicture.Count; i++)
        {
            if (ListEnemyPicture[i].RolePosition > 0)
            {
                SetPictureForwardPosition(ListEnemyPicture, ListEnemyPicture[i].RolePosition, i);
            }
        }
        WeakNum = 0;

        if (IsLegionWar)
        {
            ContinueWin++;
            WeakNum = (100 - TextTranslator.instance.GetLegionWeakByWinNum(MyWeakPoint)) / 100f;
            Debug.Log(WeakNum);
            for (int i = 0; i < ListRolePicture.Count; i++)
            {
                string[] dataSplit1 = LegionPosition.Split('!');
                for (int j = 0; j < dataSplit1.Length - 1; j++)
                {
                    string[] secSplit = dataSplit1[j].Split('$');
                    if (int.Parse(secSplit[0]) == ListRolePicture[i].RoleCharacterRoleID)
                    {
                        int Position = int.Parse(secSplit[1]);
                        ListRolePicture[i].RolePosition = Position;
                        ListRolePicture[i].RoleObject.transform.position = ListPosition[Position];
                        ListRolePicture[i].RoleNowBlood *= WeakNum;
                        ListRolePicture[i].RolePAttack *= WeakNum;
                        ListRolePicture[i].BuffDefend *= WeakNum;
                        break;
                    }
                }
            }
            if (ContinueWin == 3)
            {
                NetworkHandler.instance.SendProcess(string.Format("7002#{0};{1};{2};{3}", 32, CharacterRecorder.instance.characterName, 3, CharacterRecorder.instance.LegionHarasPoint));
            }
            else if (ContinueWin == 5)
            {
                NetworkHandler.instance.SendProcess(string.Format("7002#{0};{1};{2};{3}", 33, CharacterRecorder.instance.characterName, 5, CharacterRecorder.instance.LegionHarasPoint));
            }
            else if (ContinueWin == 8)
            {
                NetworkHandler.instance.SendProcess(string.Format("7002#{0};{1};{2};{3}", 34, CharacterRecorder.instance.characterName, 8, CharacterRecorder.instance.LegionHarasPoint));
            }
            else if (ContinueWin == 10)
            {
                NetworkHandler.instance.SendProcess(string.Format("7002#{0};{1};{2};{3}", 35, CharacterRecorder.instance.characterName, 10, CharacterRecorder.instance.LegionHarasPoint));
            }
            ReSequence();
            StartCoroutine(DelayFight());
        }
        else
        {
            LegionPosition = "";
            fw.FightPosition = "";
            for (int i = 0; i < CharacterRecorder.instance.HarassformationList.Count; i++)
            {

                foreach (var h in CharacterRecorder.instance.ownedHeroList)
                {
                    if (h.characterRoleID == CharacterRecorder.instance.HarassformationList[i].HeroId)
                    {
                        int SkillID = TextTranslator.instance.GetHeroInfoByHeroID(h.cardID).heroSkillList[0];
                        int SkillID2 = TextTranslator.instance.GetHeroInfoByHeroID(h.cardID).heroSkillList[1];
                        if (CharacterRecorder.instance.HarassformationList[i].BloodNum > 0)
                        {
                            int SkillPoint = h.skillPoint;
                            if (i == CharacterRecorder.instance.HarassformationList.Count - 1)
                            {
                                SkillPoint += 500;
                                EnemyCaptainIndex = i;
                            }
                            MyWeakPoint = (int)CharacterRecorder.instance.HarassformationList[i].WeakPoint;
                            WeakNum = (100 - TextTranslator.instance.GetLegionWeakByWinNum(MyWeakPoint)) / 100f;
                            PictureCreater.instance.CreateRole(h.cardID, h.name, CharacterRecorder.instance.HarassformationList[i].Position, Color.cyan, CharacterRecorder.instance.HarassformationList[i].MaxBloodNum, CharacterRecorder.instance.HarassformationList[i].BloodNum * WeakNum, h.hit + h.hitAdd, h.aspd / 10f, false, false, 1, 1.5f, h.dodge + h.dodgeAdd, "R" + h.characterRoleID.ToString(), h.characterRoleID, (int)((h.strength + h.strengthAdd) * WeakNum), h.physicalCrit + h.physicalCritAdd, (h.physicalDefense + h.physicalDefenseAdd) / 4f * WeakNum, h.UNphysicalCrit + h.UNphysicalCritAdd, h.moreDamige + h.moreDamigeAdd, h.avoidDamige + h.avoidDamigeAdd, SkillID, SkillID2, h.force, h.skillLevel, h.WeaponList[0].WeaponClass, h.area, h.move, SkillPoint, "");
                        }
                        fw.FightPosition += h.cardID + "$" + CharacterRecorder.instance.HarassformationList[i].Position + "!";
                        LegionPosition += h.characterRoleID + "$" + CharacterRecorder.instance.HarassformationList[i].Position + "!";
                        break;
                    }
                }

            }

            //for (int i = 0; i < ListRolePicture.Count; i++)
            //{
            //    if (ListRolePicture[i].RolePosition > 0 && ListRolePicture[i].RoleObject.name.IndexOf("NPC") == -1)
            //    {
            //        //Debug.LogError(ListRolePicture[i].RolePosition);
            //        SetPictureForwardPosition(ListRolePicture, ListRolePicture[i].RolePosition, i);
            //    }
            //}

            ReSequence();
            Time.timeScale = PictureCreater.instance.HardLevel;
            PictureCreater.instance.SetListPosition(PictureCreater.instance.PositionRow, 2);
            StartCoroutine(PictureCreater.instance.ShowFight());
            GameObject.Find("FightWindow").GetComponent<FightWindow>().BackButton.SetActive(false);
            GameObject.Find("AutoButton").SetActive(false);
            GameObject.Find("Bottom").SetActive(false);
            GameObject.Find("FightWindow").GetComponent<FightWindow>().GateDescription.GetComponent<TweenPosition>().PlayForward();
            StartCoroutine(DelayFight());
        }
        fw.SetWintreakAndWeakAndGarrisonLabelValue(Name);
        IsLegionWar = true;
        IsResult = false;
    }

    IEnumerator DelayFight()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = PictureCreater.instance.HardLevel;
        IsFight = true;
    }

    public void ChangePvpPosition(bool IsEnemy)
    {
        string[] dataSplit = PictureCreater.instance.PVPPosition.Split('!');
        string NewPosition = "";
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            //rw.SetTeamInfo(i, int.Parse(secSplit[0]), int.Parse(secSplit[1]));
            string RoleID = secSplit[0];
            string Position = secSplit[1];

            if (IsEnemy)
            {
                if (Position == "21")
                {
                    Position = "31";
                }
                else if (Position == "16")
                {
                    Position = "36";
                }
                else if (Position == "11")
                {
                    Position = "41";
                }

                else if (Position == "17")
                {
                    Position = "32";
                }
                else if (Position == "12")
                {
                    Position = "37";
                }

                else if (Position == "18")
                {
                    Position = "28";
                }
                else if (Position == "13")
                {
                    Position = "33";
                }
                else if (Position == "8")
                {
                    Position = "38";
                }
                else if (Position == "14")
                {
                    Position = "29";
                }
                else if (Position == "9")
                {
                    Position = "34";
                }

                else if (Position == "15")
                {
                    Position = "25";
                }
                else if (Position == "10")
                {
                    Position = "30";
                }
                else if (Position == "5")
                {
                    Position = "35";
                }
            }
            else
            {
                if (Position == "31")
                {
                    Position = "21";
                }
                else if (Position == "36")
                {
                    Position = "16";
                }
                else if (Position == "41")
                {
                    Position = "11";
                }

                else if (Position == "32")
                {
                    Position = "17";
                }
                else if (Position == "37")
                {
                    Position = "12";
                }

                else if (Position == "28")
                {
                    Position = "18";
                }
                else if (Position == "33")
                {
                    Position = "13";
                }
                else if (Position == "38")
                {
                    Position = "8";
                }

                else if (Position == "29")
                {
                    Position = "14";
                }
                else if (Position == "34")
                {
                    Position = "9";
                }

                else if (Position == "25")
                {
                    Position = "15";
                }
                else if (Position == "30")
                {
                    Position = "10";
                }
                else if (Position == "35")
                {
                    Position = "5";
                }
            }

            NewPosition += RoleID + "$" + Position + "!";
        }

        PictureCreater.instance.PVPPosition = NewPosition;
    }

    void CreateTerrain()
    {
        ////////////////////////////////创建地形（以下）////////////////////////////////
        if (GateTerrain.IndexOf("!") > -1)
        {
            string[] Terrains = GateTerrain.Split('!');

            foreach (var t in Terrains)
            {
                if (t.IndexOf("$") > -1)
                {
                    string[] Terrain = t.Split('$');
                    int TerrainID = int.Parse(Terrain[1]);
                    int TerrainPosition = int.Parse(Terrain[0]);
                    ListTerrainPosition.Add(TerrainPosition);
                    ListTerrainID.Add(TerrainID);
                    /*
                    0	普通
                    1	毒
                    2	剧毒
                    3	麻痹
                    4	火
                    11	治愈
                    21	熔岩
                    22	崖
                    */
                    if (TerrainID != 0)
                    {
                        GameObject TempObject = null;
                        if (TerrainID == 1)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/shitou", typeof(GameObject))) as GameObject;
                            ListStopPosition.Add(TerrainPosition);
                        }
                        else if (TerrainID == 2)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_JiaGong", typeof(GameObject))) as GameObject;
                        }
                        else if (TerrainID == 3)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_JiaFang", typeof(GameObject))) as GameObject;
                        }
                        else if (TerrainID == 4)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_JiaXue", typeof(GameObject))) as GameObject;
                        }
                        else if (TerrainID == 5)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_DiLei", typeof(GameObject))) as GameObject;
                        }
                        else if (TerrainID == 6)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_JianGong", typeof(GameObject))) as GameObject;
                        }
                        else if (TerrainID == 7)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_JianFang", typeof(GameObject))) as GameObject;
                        }
                        else if (TerrainID == 8)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_JiaNuQi", typeof(GameObject))) as GameObject;
                        }
                        else if (TerrainID == 9)
                        {
                            TempObject = GameObject.Instantiate(Resources.Load("Prefab/Terrain/DaoJu_JianNuQi", typeof(GameObject))) as GameObject;
                        }
                        TempObject.transform.position = ListPosition[TerrainPosition] + new Vector3(0, 0.11f, 0);
                        TempObject.name = "TerrainObject" + TerrainPosition.ToString();
                        TempObject.transform.parent = FightScene.transform;
                    }
                }
            }
        }
        ////////////////////////////////创建地形（以上）////////////////////////////////
    }

    public IEnumerator Newbie()
    {
        FightStyle = 99;
        PositionRow = 5;
        SetListPosition(PositionRow, 1);
        yield return new WaitForSeconds(1f);

        AudioEditer.instance.PlayLoop("Boss");

        DestroyAllComponent();
        MyBases.SetActive(true);
        IsRoleInGate = true;
        FightCount = 0;
        yield return new WaitForSeconds(0.01f);
        UIManager.instance.OpenPanel("FightWindow", false);
        fw = GameObject.Find("FightWindow").GetComponent<FightWindow>();
        fw.GateDescription.SetActive(false);
        fw.ShrinkMessage.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        for (int i = 0; i < 8; i++)
        {
            SequenceTexture[i] = GameObject.Find("Texture" + (i + 1).ToString()).GetComponent<UISprite>();
            SequenceSprite[i] = GameObject.Find("Texture" + (i + 1).ToString()).transform.Find("SpriteEdge").gameObject.GetComponent<UISprite>();
        }
        SceneTransformer.instance.NowGateID = 99999;
        CharacterRecorder.instance.lastGateID = 99999;
        GameObject FightScene = GameObject.Find("FightScene");
        if (FightScene == null)
        {
            FightScene = GameObject.Instantiate(Resources.Load("Prefab/Scene/Map02", typeof(GameObject))) as GameObject;  //新手引导

            BattleMap bm = TextTranslator.instance.battleMapDic[10];

            LightMapAsset lightAsset = Resources.Load("LightMapAsset/lightMapAsset_20") as LightMapAsset;
            int count = lightAsset.lightmapFar.Length;
            LightmapData[] lightmapDatas = new LightmapData[count];
            for (int i = 0; i < count; i++)
            {
                LightmapData lightmapData = new LightmapData();
                lightmapData.lightmapFar = lightAsset.lightmapFar[i];
                lightmapData.lightmapNear = lightAsset.lightmapNear[i];
                lightmapDatas[i] = lightmapData;
            }
            LightmapSettings.lightmaps = lightmapDatas;

            //GameObject.Find("DirectLight").GetComponent<Light>().intensity = bm.DirectLight;
            //GameObject.Find("DirectLight").GetComponent<Light>().color = new Color32((byte)bm.DirectLightR, (byte)bm.DirectLightG, (byte)bm.DirectLightB, 255);

            //if (PlayerPrefs.GetFloat("ElectractySlider") > 0)
            //{
            //    GameObject.Find("DarkLight").GetComponent<Light>().intensity = bm.DarkLight;
            //    GameObject.Find("DarkLight").GetComponent<Light>().color = new Color32((byte)bm.DarkLightR, (byte)bm.DarkLightG, (byte)bm.DarkLightB, 255);

            //    GameObject.Find("BackLight").GetComponent<Light>().intensity = bm.BackLight;
            //    GameObject.Find("BackLight").GetComponent<Light>().color = new Color32((byte)bm.BackLightR, (byte)bm.BackLightG, (byte)bm.BackLightB, 255);
            //}
            //else
            //{
            //    GameObject.Find("DarkLight").SetActive(false);
            //    GameObject.Find("BackLight").SetActive(false);
            //}
        }

        if (PlayerPrefs.GetFloat("EffectSlider") > 0)
        {
            PictureCreater.instance.SetWeather(WeatherID);
        }
        FightScene.transform.rotation = new Quaternion(0, 0, 0, 0);
        FightScene.transform.localScale = Vector3.one;
        FightScene.name = "FightScene";

        IsFight = false;
        IsFightFinish = false;
        FightTimer = 0;
        NowSequence = 1;

        yield return new WaitForEndOfFrame();

        fw.Tactics.SetActive(false);
        fw.TacticsInfo.SetActive(false);

        CreateRole(60023, "Enemy", 17, Color.cyan, 50000, 50000, 1000, 108.0f, false, false, 1, 1.5f, 0, "", 0, 6933, 0, 2000, 0, 0, 0, 1025, 2028, 0, 1, 4, 1, 2, 1000, "");
        CreateRole(60029, "Enemy", 18, Color.cyan, 50000, 50000, 1000, 109.8f, false, false, 1, 1.5f, 0, "", 0, 6987, 0, 2000, 0, 0, 0, 1036, 2028, 0, 1, 5, 1, 2, 1000, "");
        CreateRole(60027, "Enemy", 14, Color.cyan, 50000, 50000, 1000, 105.8f, false, false, 1, 1.5f, 0, "", 0, 6254, 0, 2000, 0, 0, 0, 1035, 2028, 0, 1, 2, 1, 2, 1000, "");
        CreateRole(60032, "Enemy", 21, Color.cyan, 45000, 45000, 1000, 105.5f, false, false, 1, 1.5f, 0, "", 0, 6741, 0, 2000, 0, 0, 0, 1029, 2028, 0, 1, 1, 12, 2, 1000, "");
        CreateRole(60026, "Enemy", 15, Color.cyan, 50000, 50000, 1000, 106.3f, false, false, 1, 1.5f, 0, "", 0, 6344, 0, 2000, 0, 0, 0, 1034, 2028, 0, 1, 5, 1, 2, 1000, "");


        CreateRole(60018, "Enemy", 28, Color.red, 50000, 50000, 1000, 107.1f, true, false, 1, 1.5f, 0, "", 0, 6854, 0, 0, 0, 0, 0, 1030, 2028, 0, 1, 4, 2, 2, 1000, "");
        CreateRole(60030, "Enemy", 33, Color.red, 98447, 98447, 1000, 100.0f, true, false, 1, 1.5f, 0, "", 0, 99999, 0, 0, 0, 0, 0, 1039, 2028, 0, 1, 5, 2, 2, 1000, "");
        CreateRole(60012, "Enemy", 27, Color.red, 120000, 120000, 1000, 108.9f, true, false, 1, 1.5f, 0, "", 0, 6454, 0, 0, 0, 0, 0, 1010, 2028, 0, 1, 3, 2, 2, 1000, "");
        CreateRole(60020, "Enemy", 24, Color.red, 60000, 60000, 1000, 106.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 2, 2, 2, 1000, "");

        CreateRole(65400, "Enemy", 29, Color.red, 60000, 60000, 1000, 6.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000, "");
        CreateRole(65400, "Enemy", 32, Color.red, 60000, 60000, 1000, 6.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000, "");
        CreateRole(65501, "Enemy", 38, Color.red, 60000, 60000, 1000, 6.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000, "");


        //CreateRole(60102, "Enemy", 12, Color.cyan, 45000, 45000, 1000, 5.5f, false, false, 1, 1.5f, 0, "", 0, 6741, 0, 2000, 0, 0, 0, 1029, 2028, 0, 1, 0, 1022, 2, 1000);
        //CreateRole(60102, "Enemy", 9, Color.cyan, 45000, 45000, 1000, 5.5f, false, false, 1, 1.5f, 0, "", 0, 6741, 0, 2000, 0, 0, 0, 1029, 2028, 0, 1, 0, 1022, 2, 1000);
        //CreateRole(60103, "Enemy", 11, Color.cyan, 45000, 45000, 1000, 5.5f, false, false, 1, 1.5f, 0, "", 0, 6741, 0, 2000, 0, 0, 0, 1029, 2028, 0, 1, 0, 1022, 2, 1000);
        //CreateRole(60103, "Enemy", 8, Color.cyan, 45000, 45000, 1000, 5.5f, false, false, 1, 1.5f, 0, "", 0, 6741, 0, 2000, 0, 0, 0, 1029, 2028, 0, 1, 0, 1022, 2, 1000);
        //CreateRole(60103, "Enemy", 5, Color.cyan, 45000, 45000, 1000, 5.5f, false, false, 1, 1.5f, 0, "", 0, 6741, 0, 2000, 0, 0, 0, 1029, 2028, 0, 1, 0, 1022, 2, 1000);

        IsAll = false;
        MyUISliderValue = 1f;

        SkillFire1 = 0;
        SkillFire2 = 0;
        SkillFire3 = 0;

        CameraY = 35;
        FieldView = 60;
        MyCamera.GetComponent<Camera>().fieldOfView = FieldView;
        //FightCamera.GetComponent<Camera>().fieldOfView = MyCamera.GetComponent<Camera>().fieldOfView;
        MyCamera.GetComponent<MouseClick>().MoveOffect = 0;
        MyCamera.GetComponent<MouseClick>().MoveOffectY = 0;
        MyCamera.GetComponent<MouseClick>().TouchPosition = 0;
        MyCamera.GetComponent<MouseClick>().TouchPositionY = 0;
        MyCamera.GetComponent<MouseClick>().MovePosition = 0;
        MyCamera.GetComponent<MouseClick>().MovePositionY = 0;
        FightScene.transform.position = new Vector3(0, -0.1f, 0);

        MyCamera.transform.position = new Vector3(0, 8.8f, -6.6f);
        MyCamera.transform.rotation = new Quaternion(0, 0, 0, 0);


        yield return new WaitForEndOfFrame();
        fw.MyUISlider.value = MyUISliderValue;

        for (int j = 0; j < ListPosition.Count; j++)
        {
            Destroy(GameObject.Find("TerrainObject" + j.ToString()));
        }

        ListStopPosition.Clear();
        List<GateMap> ListGateMap = new List<GateMap>();
        ListGateMap = TextTranslator.instance.GetGateMapByID(SceneTransformer.instance.NowGateID);

        //Debug.Log(ListGateMap.Count + " " + SceneTransformer.instance.NowGateID);
        //for (int i = 0; i < ListGateMap.Count; i++)
        //{
        //    for (int j = 0; j < TextTranslator.instance.enemyInfoList.Count; j++)
        //    {
        //        if (ListGateMap[i].MonsterID == TextTranslator.instance.enemyInfoList[j].enemyID)
        //        {
        //            if (ListGateMap[i].PosID > 0)
        //            {
        //                //CreateRole(TextTranslator.instance.enemyInfoList[j].pic, "Enemy" + TextTranslator.instance.enemyInfoList[j].enemyID + "_" + i.ToString(), ListGateMap[i].PosID, Color.red, TextTranslator.instance.enemyInfoList[j].hp + ListGateMap[i].HpRate, TextTranslator.instance.enemyInfoList[j].hp + ListGateMap[i].HpRate, 1, TextTranslator.instance.enemyInfoList[j].spd / 10f, true, false, 1, 1.5f, 0.2f, TextTranslator.instance.enemyInfoList[j].enemyID.ToString(), 0, TextTranslator.instance.enemyInfoList[j].atk + ListGateMap[i].AtkRate, 0, TextTranslator.instance.enemyInfoList[j].def + ListGateMap[i].DefRate, 0, TextTranslator.instance.enemyInfoList[j].skill[0], TextTranslator.instance.enemyInfoList[j].skill[2], TextTranslator.instance.enemyInfoList[j].bossAi, 0, TextTranslator.instance.enemyInfoList[j].ai, TextTranslator.instance.enemyInfoList[j].area, TextTranslator.instance.enemyInfoList[j].mv, 0);
        //            }
        //        }
        //    }
        //}


        SetSequence();
        if (GameObject.Find("LoadingWindow") != null)
        {
            GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().IsClose = true;
        }
    }

    public void NewbieMove()
    {
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            if (ListRolePicture[i].RolePosition > 0)
            {
                SetPictureForwardPosition(ListRolePicture, ListRolePicture[i].RolePosition, i);
            }
        }
    }
    public void NewbieBlood()
    {
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            ListRolePicture[i].RoleRedBloodObject.SetActive(true);
        }

        for (int i = 0; i < ListEnemyPicture.Count; i++)
        {
            ListEnemyPicture[i].RoleRedBloodObject.SetActive(true);
        }
    }
    #endregion
    void Start()
    {
        instance = this;
        MyCamera = GameObject.Find("MainCamera");
        EffectCamera = GameObject.Find("EffectCamera");
        HurtMaterial = (Material)Resources.Load("Materials/HeroHurt");
        BlankMaterial = (Material)Resources.Load("Materials/HeroBlank");
        Boss1Material = (Material)Resources.Load("Materials/BOSS01");
        Boss2Material = (Material)Resources.Load("Materials/BOSS02");

        EPositionMaterial = (Material)Resources.Load("Materials/eposition");
        BPositionMaterial = (Material)Resources.Load("Materials/bposition");
        PositionMaterial = (Material)Resources.Load("Materials/position");

        ActObject = (GameObject)Instantiate(Resources.Load("Prefab/Effect/XingDong05"));
        ActObject.name = "ActObject";
        ActObject.SetActive(false);
        SetListPosition(5, 0);
        //StartCoroutine(OpenMain());
    }
    public IEnumerator OpenMain()
    {
        yield return new WaitForSeconds(1f);
        //if (!NetworkHandler.instance.IsCreate)
        //{
        //    UIManager.instance.OpenPanel("MainWindow", false);
        //}
        //if (GameObject.Find("CameraPosition") != null)
        //{
        //    PictureCreater.instance.MySkill = GameObject.Find("CameraPosition");
        //    PictureCreater.instance.SkillCamera = GameObject.Find("SkillCamera");
        //    PictureCreater.instance.MySkill.SetActive(false);
        //}

    }

    public void DestroyLeastComponent()  //战斗结束后把物件消除
    {
        ListRolePicture[ListRolePicture.Count - 1].RoleFightIndex.Clear();
        ListRolePicture[ListRolePicture.Count - 1].RolePictureTraceLoop = false;
        DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RolePictureObject);
        DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RoleTargetObject);
        DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RoleNameObject);
        DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RoleTaskObject);
        DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RoleBlackBloodObject);
        DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RoleRedBloodObject);
        if (ListRolePicture[ListRolePicture.Count - 1].RoleObject.GetComponent<ColliderDisplayText>() != null)
        {
            DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RoleObject.GetComponent<ColliderDisplayText>().mText);
        }
        DestroyImmediate(ListRolePicture[ListRolePicture.Count - 1].RoleObject);
        ListRolePicture.RemoveAt(ListRolePicture.Count - 1);
    }
    public void DestroyAllComponent()  //战斗结束后把物件消除
    {
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            ListRolePicture[i].RoleFightIndex.Clear();
            ListRolePicture[i].RolePictureTraceLoop = false;
            DestroyImmediate(ListRolePicture[i].RolePictureObject);
            DestroyImmediate(ListRolePicture[i].RoleTargetObject);
            DestroyImmediate(ListRolePicture[i].RoleNameObject);
            DestroyImmediate(ListRolePicture[i].RoleTaskObject);
            DestroyImmediate(ListRolePicture[i].RoleBlackBloodObject);
            DestroyImmediate(ListRolePicture[i].RoleRedBloodObject);
            if (ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>() != null)
            {
                DestroyImmediate(ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().mText);
            }
            DestroyImmediate(ListRolePicture[i].RoleObject);

        }

        ListRolePicture.Clear();

        for (int i = 0; i < ListEnemyPicture.Count; i++)
        {
            ListEnemyPicture[i].RoleFightIndex.Clear();
            ListEnemyPicture[i].RolePictureTraceLoop = false;
            DestroyImmediate(ListEnemyPicture[i].RolePictureObject);
            DestroyImmediate(ListEnemyPicture[i].RoleTargetObject);
            DestroyImmediate(ListEnemyPicture[i].RoleNameObject);
            DestroyImmediate(ListEnemyPicture[i].RoleTaskObject);
            DestroyImmediate(ListEnemyPicture[i].RoleBlackBloodObject);
            DestroyImmediate(ListEnemyPicture[i].RoleRedBloodObject);
            if (ListEnemyPicture[i].RoleObject.GetComponent<ColliderDisplayText>() != null)
            {
                DestroyImmediate(ListEnemyPicture[i].RoleObject.GetComponent<ColliderDisplayText>().mText);
            }
            DestroyImmediate(ListEnemyPicture[i].RoleObject);
        }

        ListEnemyPicture.Clear();

        PicRoleList.Clear();
        PicEnemyList.Clear();

        DestroyImmediate(Weather);
    }


    public void DestroyEnemyComponent()  //战斗结束后把物件消除
    {
        for (int i = 0; i < ListEnemyPicture.Count; i++)
        {
            ListEnemyPicture[i].RoleFightIndex.Clear();
            ListEnemyPicture[i].RolePictureTraceLoop = false;
            DestroyImmediate(ListEnemyPicture[i].RolePictureObject);
            DestroyImmediate(ListEnemyPicture[i].RoleTargetObject);
            DestroyImmediate(ListEnemyPicture[i].RoleNameObject);
            DestroyImmediate(ListEnemyPicture[i].RoleTaskObject);
            DestroyImmediate(ListEnemyPicture[i].RoleBlackBloodObject);
            DestroyImmediate(ListEnemyPicture[i].RoleRedBloodObject);
            if (ListEnemyPicture[i].RoleObject.GetComponent<ColliderDisplayText>() != null)
            {
                DestroyImmediate(ListEnemyPicture[i].RoleObject.GetComponent<ColliderDisplayText>().mText);
            }
            DestroyImmediate(ListEnemyPicture[i].RoleObject);
        }
        ListEnemyPicture.Clear();
        PicEnemyList.Clear();
    }


    public void DestroyComponentByName(string ObjectName)  //战斗结束后把物件消除
    {
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            if (ListRolePicture[i].RoleObject.name == ObjectName)
            {
                ListRolePicture[i].RoleFightIndex.Clear();
                ListRolePicture[i].RolePictureTraceLoop = false;
                DestroyImmediate(ListRolePicture[i].RolePictureObject);
                DestroyImmediate(ListRolePicture[i].RoleTargetObject);
                DestroyImmediate(ListRolePicture[i].RoleNameObject);
                DestroyImmediate(ListRolePicture[i].RoleTaskObject);
                DestroyImmediate(ListRolePicture[i].RoleBlackBloodObject);
                DestroyImmediate(ListRolePicture[i].RoleRedBloodObject);
                if (ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>() != null)
                {
                    DestroyImmediate(ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().mText);
                }
                DestroyImmediate(ListRolePicture[i].RoleObject);
                ListRolePicture.RemoveAt(i);
                break;
            }
        }
    }

    #region CreateRole
    IEnumerator AirSupply(GameObject SupplyObject, List<RolePicture> SetPicture)
    {
        SupplyObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        SupplyObject.SetActive(true);
        if (!SetPicture[SetPicture.Count - 1].IsPicture)
        {
            SupplyObject.GetComponent<Animator>().Play("down1");
        }
        GameObject Shadow = SupplyObject.transform.Find("Shadow").gameObject;
        SupplyObject.transform.localPosition += new Vector3(0, 3f, 0);
        Shadow.transform.localPosition -= new Vector3(0, 3f, 0);
        if (!IsSkip || true)
        {
            for (int i = 0; i < 15; i++)
            {
                SupplyObject.transform.localPosition -= new Vector3(0, 0.2f, 0);
                Shadow.transform.localPosition += new Vector3(0, 0.2f, 0);
                yield return new WaitForSeconds(0.01f);
            }
            if (!SetPicture[SetPicture.Count - 1].IsPicture)
            {
                SupplyObject.GetComponent<Animator>().SetFloat("dn", 0);
            }
            Shadow.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            SupplyObject.transform.localPosition -= new Vector3(0, 3f, 0);
            SupplyObject.GetComponent<Animator>().SetFloat("dn", 0);
            Shadow.transform.localPosition = Vector3.zero;
        }
        SupplyObject.transform.Find("polySurface1").gameObject.SetActive(false);
        InsertSequence(SetPicture, SetPicture.Count - 1, SetPicture[0].RolePictureMonster);
        yield return 0;
    }



    public int CreateRole(int RoleID, string Name, int PositionID, Color NameColor, float MaxBlood, float NowBlood, float Hit, float FightSpeed, bool IsEnemy, bool IsNPC, int IsFaceRight, float ScaleType, float NoHit, string PointID, int SetCharacterRoleID, float SetPAttack, float Crit, float SetPDefend, float NoCrit, float DamigeAdd, float DamigeReduce, int SkillID1, int SkillID2, int BossAi, int SkillLevel, int Ai, int Area, int MoveStep, int SkillPoint, string Innate)
    {
        try
        {
            //1命中7抗爆13加速
            //加伤   2普攻伤害提升  4达到血量加伤
            //加buff 3防提高攻击力    8攻提高血量    14直接加怒
            //吸血   5攻击吸血
            //怒气   6杀人得怒     15概率最少怒得怒      18夺人怒气
            //速度   9队长提速
            //扺抗   10不受debuff
            //反伤   11反弹伤害
            //减伤   12减技能伤害
            //免伤   16致命免伤
            //行动   17概率行动一次
            //Debug.LogError(RoleID + "AAAAAAAAAAA" + Innate);
            //角色圖片初始化
            int newRoleID = RoleID;
            RolePicture NewRolePicture = new RolePicture();
            Vector3 StartPosition = ListPosition[PositionID];


            if (RoleID == 65901 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12)
            {
                NewRolePicture.RolePictureObject = GameObject.Instantiate(Resources.Load("Prefab/Role/60019", typeof(GameObject))) as GameObject;
            }
            else
            {
#if ClashRoyale
                if (RoleID < 60011)
#else
                if (false)
#endif
                {
                    if (AddPicRole(RoleID.ToString(), IsEnemy))
                    {
                        if (IsEnemy)
                        {
                            NewRolePicture.RolePictureObject = CreatePicRole(PicEnemyList, Name, RoleID, StartPosition, ScaleType, 1);
                        }
                        else
                        {
                            NewRolePicture.RolePictureObject = CreatePicRole(PicRoleList, Name, RoleID, StartPosition, ScaleType, 0);
                        }
                    }
                    NewRolePicture.IsPicture = true;
                    Destroy(NewRolePicture.RolePictureObject.GetComponent("BoxCollider"));
                }
                else
                {
                    NewRolePicture.RolePictureObject = GameObject.Instantiate(Resources.Load("Prefab/Role/" + RoleID.ToString(), typeof(GameObject))) as GameObject;
                    NewRolePicture.IsPicture = false;
                    if (AddPicRole(RoleID.ToString(), IsEnemy))
                    {

                    }

                    foreach (Component c in NewRolePicture.RolePictureObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true))
                    {
                        if (c.name == "whiteflag")
                        {
                            c.gameObject.SetActive(false);
                        }
                    }
                }
            }


            if (RoleID == 69999 || RoleID == 69998)
            {
                RoleID = 60001;
                if (IsEnemy)
                {
                    StartCoroutine(AirSupply(NewRolePicture.RolePictureObject, ListEnemyPicture));
                }
                else
                {
                    StartCoroutine(AirSupply(NewRolePicture.RolePictureObject, ListRolePicture));
                }
            }

            HeroInfo hi = TextTranslator.instance.GetHeroInfoByHeroID(RoleID);
            ScaleType = hi.heroScale;
            NewRolePicture.RoleAttackType = hi.heroCarrerType;
            NewRolePicture.RolePosition = PositionID;

            if (!IsEnemy)
            {
                if (PointID.IndexOf("Enemy") > -1)
                {
                    NewRolePicture.RoleObject = new GameObject("NPC" + RoleID.ToString());
                }
                else
                {
                    NewRolePicture.RoleObject = new GameObject("Role" + RoleID.ToString());
                }
            }
            else
            {
                NewRolePicture.RoleObject = new GameObject(Name);
            }
            NewRolePicture.RoleObject.transform.position = StartPosition;
#if !ClashRoyale
            NewRolePicture.RoleObject.AddComponent<BoxCollider>();
            NewRolePicture.RoleObject.GetComponent<BoxCollider>().center = new Vector3(0, 0.01f, 0);
            NewRolePicture.RoleObject.GetComponent<BoxCollider>().size = new Vector3(2.25f, 0.01f, 2.25f);
#endif
            if (Innate != "")
            {
                string[] Innates = Innate.Split('!');
                for (int i = 0; i < 18; i++)
                {
                    string[] SingleInnate = Innates[i].Split('$');
                    NewRolePicture.RoleInnate[i] = int.Parse(SingleInnate[1]);
                }
            }

            NewRolePicture.RoleObject.transform.parent = gameObject.transform;
            NewRolePicture.RolePictureFaceRight = IsFaceRight;
            NewRolePicture.RolePictureScaleType = ScaleType;
            NewRolePicture.RoleID = RoleID;
            NewRolePicture.RoleCharacterRoleID = SetCharacterRoleID;
            //加buff 3防提高攻击力    8攻提高血量    14直接加怒
            Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(3, NewRolePicture.RoleInnate[2]);
            if (SetInnate != null)
            {
                SetPAttack += SetPDefend * (SetInnate.Value1 / 100f);
            }
            NewRolePicture.RolePAttack = SetPAttack;
            NewRolePicture.RolePDefend = SetPDefend;

            NewRolePicture.RoleCrit = Crit;
            NewRolePicture.RoleNoCrit = NoCrit;

            NewRolePicture.RoleHit = Hit;
            NewRolePicture.RoleNoHit = NoHit;

            NewRolePicture.RoleDamigeAdd = DamigeAdd;
            NewRolePicture.RoleDamigeReduce = DamigeReduce;

            NewRolePicture.RoleArea = Area;
            NewRolePicture.RoleAttackMode = hi.heroAtkType;
            NewRolePicture.RoleDefendMode = hi.heroDefType;
            NewRolePicture.RoleAi = Ai;  //这值用来当神器



            //NewRolePicture.RoleAi = 1;
            if (NewRolePicture.RoleAi > 0 && hi.heroRarity > 3)
            {
                foreach (Component c in NewRolePicture.RolePictureObject.GetComponentsInChildren(typeof(Transform), true))
                {
                    if (c.name == "Object001" || c.name == "Object002" || c.name == "Object003" || c.name == "Object004")
                    {
                        if (c.gameObject.renderer != null)
                        {
                            Material[] ListMaterial = new Material[c.gameObject.renderer.materials.Length];
                            for (int i = 0; i < c.gameObject.renderer.materials.Length - 1; i++)
                            {
                                ListMaterial[i] = c.gameObject.renderer.materials[i];
                            }
                            ListMaterial[c.gameObject.renderer.materials.Length - 1] = (Material)Resources.Load("Materials/color_0" + NewRolePicture.RoleAi.ToString());

                            c.gameObject.renderer.materials = ListMaterial;
                        }
                        if (c.gameObject.transform.Find("W" + NewRolePicture.RoleAi.ToString()) != null)
                        {
                            c.gameObject.transform.Find("W" + NewRolePicture.RoleAi.ToString()).gameObject.SetActive(true);
                        }
                    }
                }

                //foreach (Component c in NewRolePicture.RolePictureObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                //{
                //    if (c.name == "Object001" || c.name == "Object002" || c.name == "Object003" || c.name == "Object004")
                //    {
                //        Material[] ListMaterial = new Material[c.gameObject.renderer.materials.Length];
                //        for (int i = 0; i < c.gameObject.renderer.materials.Length - 1; i++)
                //        {
                //            ListMaterial[i] = c.gameObject.renderer.materials[i];
                //        }
                //        ListMaterial[c.gameObject.renderer.materials.Length - 1] = (Material)Resources.Load("Materials/color_0" + NewRolePicture.RoleAi.ToString());

                //        c.gameObject.renderer.materials = ListMaterial;

                //        if (c.gameObject.transform.Find("W" + NewRolePicture.RoleAi.ToString()) != null)
                //        {
                //            c.gameObject.transform.Find("W" + NewRolePicture.RoleAi.ToString()).gameObject.SetActive(true);
                //        }
                //    }
                //}
            }


            NewRolePicture.RoleBio = hi.heroBio;
            NewRolePicture.RoleRace = hi.heroRace;
            NewRolePicture.RoleAttackFly = hi.heroAtkFly;
            NewRolePicture.RoleAttackGround = 1;
            NewRolePicture.RoleForce = BossAi;
            NewRolePicture.RoleCareer = hi.heroCarrerType;
            NewRolePicture.RoleSex = hi.sex;

            if (SkillID2 == 2040) //复活概率
            {
                NewRolePicture.RoleRelife = 100;
            }
            else if (SkillID2 == 2046)
            {
                NewRolePicture.RoleRelife = 35;
            }

            //if (BossAi != 0)
            //{
            //    NewRolePicture.RoleBossAiInfo = TextTranslator.instance.GetBossAi(BossAi);
            //}

            NewRolePicture.RoleMoveStep = MoveStep;
            NewRolePicture.RoleMoveNowStep = MoveStep;
            //加buff 3防提高攻击力    8攻提高血量    14直接加怒
            SetInnate = TextTranslator.instance.GetInnatesByTwo(14, NewRolePicture.RoleInnate[13]);
            if (SetInnate != null)
            {
                SkillPoint += SetInnate.Value1;
            }

            NewRolePicture.RoleSkillPoint = SkillPoint > 1000 ? 1000 : SkillPoint;

            if (!NewRolePicture.IsPicture)
            {
                NewRolePicture.RolePictureObject.transform.localScale = new Vector3(ScaleType, ScaleType, ScaleType);
            }
            NewRolePicture.RolePictureObject.transform.position = StartPosition;
            NewRolePicture.RolePictureObject.name = "Role";

            if (!NewRolePicture.IsPicture)
            {
                if (IsEnemy)
                {
                    NewRolePicture.RolePictureObject.transform.Rotate(0, -90, 0);
                }
                else
                {
                    NewRolePicture.RolePictureObject.transform.Rotate(0, 90, 0);
                }
            }

            NewRolePicture.RolePictureObject.transform.parent = NewRolePicture.RoleObject.transform;

            //加buff 3防提高攻击力    8攻提高血量    14直接加怒
            SetInnate = TextTranslator.instance.GetInnatesByTwo(8, NewRolePicture.RoleInnate[7]);
            if (SetInnate != null)
            {
                MaxBlood += SetPAttack * (SetInnate.Value1 / 100f);
                NowBlood += SetPAttack * (SetInnate.Value1 / 100f);
            }

            NewRolePicture.RoleMaxBlood = MaxBlood;
            NewRolePicture.RoleNowBlood = NowBlood;


            NewRolePicture.RoleCaptain = false;
            NewRolePicture.RolePictureMoving = false;
            NewRolePicture.RolePictureTraceLoop = false;
            NewRolePicture.RolePictureNewPosition = StartPosition;
            NewRolePicture.RolePictureTimer = 0;
            NewRolePicture.RolePictureTraceTimer = 0;
            NewRolePicture.RolePictureIdleTimer = 0;
            NewRolePicture.RolePictureNowFrame = 0;
            NewRolePicture.RolePictureAnimationSpeed = 0;
            NewRolePicture.RolePictureMonster = IsEnemy;
            NewRolePicture.RolePictureNPC = IsNPC;
            NewRolePicture.RolePictureFight = false;
            NewRolePicture.RolePictureNowTrace = 0;
            NewRolePicture.RolePicturePointID = PointID;
            NewRolePicture.RolePiectureTraceSpeed = 0;
            NewRolePicture.RolePictureTransparent = 1;
            NewRolePicture.RolePictureAttackable = true;
            NewRolePicture.RolePiectureMoveSpeed = 0.08f;
            NewRolePicture.RoleTargetIndex = -1;

            NewRolePicture.RoleFightSpeed = 300 - FightSpeed;
            NewRolePicture.RoleFightNowSpeed = 300 - FightSpeed;
            NewRolePicture.RolePictureStartAttack = false;
            NewRolePicture.RolePictureTrace = false;
            NewRolePicture.RolePictureFollow = false;
            NewRolePicture.RolePictureHide = false;
            NewRolePicture.RoleCloseEye = true;

            NewRolePicture.RoleSkill1 = SkillID1;
            NewRolePicture.RoleSkillLevel1 = SkillLevel;
            NewRolePicture.RoleSkill2 = SkillID2;
            NewRolePicture.RoleStage = 1;

            if (MaxBlood > 0)
            {
                NewRolePicture.RoleObject.AddComponent<ColliderDisplayText>();
                if (fw != null)
                {
                    NewRolePicture.RoleObject.GetComponent<ColliderDisplayText>().Init(NewRolePicture.RoleObject.transform);
                    NewRolePicture.RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SkillPoint);

                    ////////////////////////血量條(以下)////////////////////////
                    NewRolePicture.RoleObject.GetComponent<ColliderDisplayText>().mSlider.value = 1;
                    NewRolePicture.RoleRedBloodObject = NewRolePicture.RoleObject.GetComponent<ColliderDisplayText>().mSlider.gameObject;
                    if (IsEnemy)
                    {
                        NewRolePicture.RoleRedBloodObject.transform.Find("Foreground").gameObject.GetComponent<UISprite>().spriteName = "jingyantiao1_11_2";
                    }
                    else if (PositionID > 0)
                    {
                        FightCount++;
                    }
                    if (FightStyle != 2)
                    {
                        NewRolePicture.RoleRedBloodObject.SetActive(false);
                    }
                    else
                    {
                        NewRolePicture.RoleObject.GetComponent<ColliderDisplayText>().mSlider.value = NowBlood / MaxBlood;
                    }
                    ////////////////////////血量條(以上)////////////////////////
                }
            }

            if (IsRoleInGate)
            {
                if (IsRemember)
                {
                    if (IsEnemy)
                    {
                        if (NameColor != Color.cyan)
                        {
                            if (FightStyle == 14)
                            {
                                NewRolePicture.RoleObject.transform.position += new Vector3(6, 0, 0);
                            }
                            else
                            {
                                NewRolePicture.RoleObject.transform.position += new Vector3(0.0001f, 0, 0);
                            }
                            if (!NewRolePicture.IsPicture)
                            {
                                NewRolePicture.RolePictureObject.GetComponent<Animator>().SetFloat("id", 0);
                            }
                        }
                    }
                    else
                    {
                        if (newRoleID != 69999 && NameColor == Color.cyan && Name.IndexOf("NPC") == -1)
                        {
                            NewRolePicture.RoleObject.transform.position -= new Vector3(6, 0, 0);
                        }
                    }
                }
            }


            ////////////////////选择攻击目标///////////////////

            NewRolePicture.RoleTargetObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            DestroyImmediate(NewRolePicture.RoleTargetObject.GetComponent("MeshCollider"));
            NewRolePicture.RoleTargetObject.renderer.material.mainTexture = Resources.Load("Game/jiantou_green", typeof(Texture)) as Texture;
            NewRolePicture.RoleTargetObject.renderer.material.shader = Shader.Find("Unlit/Transparent Colored");

            NewRolePicture.RoleTargetObject.renderer.receiveShadows = false;
            NewRolePicture.RoleTargetObject.renderer.castShadows = false;

            NewRolePicture.RoleTargetObject.name = "TargetObject";
            NewRolePicture.RoleTargetObject.transform.localScale = new Vector3(OffsetX * 0.75f, OffsetX * 0.75f, OffsetX * 0.75f);
            NewRolePicture.RoleTargetObject.transform.Rotate(90, 90, 0);
            NewRolePicture.RoleTargetObject.transform.parent = NewRolePicture.RoleObject.transform;
            NewRolePicture.RoleTargetObject.transform.localPosition = Vector3.zero + new Vector3(1, 0, 0);
            NewRolePicture.RoleTargetObject.SetActive(false);
            ////////////////////选择攻击目标///////////////////

            if (Name.IndexOf("NPC") > -1)
            {
                if (!NewRolePicture.IsPicture)
                {
                    NewRolePicture.RolePictureObject.GetComponent<Animator>().SetFloat("box", 0);
                    NewRolePicture.RolePictureObject.transform.Rotate(0, 180, 0);
                    NewRolePicture.RolePictureNPC = true;
                }
            }

            if (RoleID == 65390 || RoleID == 65391)
            {
                NewRolePicture.RoleObject.transform.Rotate(0, -180, 0);
                NewRolePicture.RoleFightSpeed = 999999;
                NewRolePicture.RoleFightNowSpeed = 999999;
            }

            if (IsEnemy)
            {
                ListEnemyPicture.Add(NewRolePicture);
            }
            else
            {
                ListRolePicture.Add(NewRolePicture);
            }

            if (PositionID == 0)
            {
                NewRolePicture.RoleObject.SetActive(false);
                if (fw != null)
                {
                    NewRolePicture.RoleRedBloodObject.SetActive(false);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("CreateRole" + ex.ToString());
        }
        if (IsEnemy)
        {
            return ListEnemyPicture.Count - 1;
        }
        else
        {
            return ListRolePicture.Count - 1;
        }
    }


    #endregion

    #region Move
#if ClashRoyale
    public void ChangeDirection(List<RolePicture> SetPicture, List<PictureRoleData> SetData, Vector3 NowPosition, Vector3 SetPosition, int SetRoleIndex)
#else
    public void ChangeDirection(List<RolePicture> SetPicture, Vector3 NowPosition, Vector3 SetPosition, int SetRoleIndex)
#endif
    {
#if ClashRoyale
        if (SetPicture[SetRoleIndex].IsPicture)
        {
            ////////////////////////决定顺序////////////////////////
            if (SetData[SetRoleIndex].CurrentAction.Count == 5)
            {
                if (SetPicture[SetRoleIndex].RoleObject.transform.position.x > SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z > SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = false;

                    if (((SetPicture[SetRoleIndex].RoleObject.transform.position.z - SetPosition.z) / (SetPicture[SetRoleIndex].RoleObject.transform.position.x - SetPosition.x)) > 2f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 5)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(5);
                        }
                    }
                    else if (((SetPicture[SetRoleIndex].RoleObject.transform.position.z - SetPosition.z) / (SetPicture[SetRoleIndex].RoleObject.transform.position.x - SetPosition.x)) < 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 3)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 4)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(4);
                        }
                    }
                }
                else if (SetPicture[SetRoleIndex].RoleObject.transform.position.x > SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z <= SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = false;

                    if (((SetPosition.z - SetPicture[SetRoleIndex].RoleObject.transform.position.z) / (SetPicture[SetRoleIndex].RoleObject.transform.position.x - SetPosition.x)) > 2f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 1)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                        }
                    }
                    else if (((SetPosition.z - SetPicture[SetRoleIndex].RoleObject.transform.position.z) / (SetPicture[SetRoleIndex].RoleObject.transform.position.x - SetPosition.x)) < 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 3)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 2)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                        }
                    }
                }
                else if (SetPicture[SetRoleIndex].RoleObject.transform.position.x < SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z > SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = true;

                    if (((SetPicture[SetRoleIndex].RoleObject.transform.position.z - SetPosition.z) / (SetPosition.x - SetPicture[SetRoleIndex].RoleObject.transform.position.x)) > 2f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 5)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(5);
                        }
                    }
                    else if (((SetPicture[SetRoleIndex].RoleObject.transform.position.z - SetPosition.z) / (SetPosition.x - SetPicture[SetRoleIndex].RoleObject.transform.position.x)) < 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 3)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 4)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(4);
                        }
                    }
                }
                else if (SetPicture[SetRoleIndex].RoleObject.transform.position.x < SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z <= SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = true;

                    if (((SetPosition.z - SetPicture[SetRoleIndex].RoleObject.transform.position.z) / (SetPosition.x - SetPicture[SetRoleIndex].RoleObject.transform.position.x)) > 2f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 1)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                        }
                    }
                    else if (((SetPosition.z - SetPicture[SetRoleIndex].RoleObject.transform.position.z) / (SetPosition.x - SetPicture[SetRoleIndex].RoleObject.transform.position.x)) < 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 3)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 2)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                        }
                    }
                }
            }
            else if (SetData[SetRoleIndex].CurrentAction.Count == 3)
            {
                if (SetPicture[SetRoleIndex].RoleObject.transform.position.x > SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z > SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = false;

                    if (((SetPicture[SetRoleIndex].RoleObject.transform.position.z - SetPosition.z) / (SetPicture[SetRoleIndex].RoleObject.transform.position.x - SetPosition.x)) > 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 3)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 2)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                        }
                    }
                }
                else if (SetPicture[SetRoleIndex].RoleObject.transform.position.x > SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z <= SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = false;

                    if (((SetPosition.z - SetPicture[SetRoleIndex].RoleObject.transform.position.z) / (SetPicture[SetRoleIndex].RoleObject.transform.position.x - SetPosition.x)) > 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 1)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 2)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                        }
                    }
                }
                else if (SetPicture[SetRoleIndex].RoleObject.transform.position.x < SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z > SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = true;

                    if (((SetPicture[SetRoleIndex].RoleObject.transform.position.z - SetPosition.z) / (SetPosition.x - SetPicture[SetRoleIndex].RoleObject.transform.position.x)) > 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 3)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(3);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 2)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                        }
                    }
                }
                else if (SetPicture[SetRoleIndex].RoleObject.transform.position.x < SetPosition.x && SetPicture[SetRoleIndex].RoleObject.transform.position.z <= SetPosition.z)
                {
                    SetData[SetRoleIndex].CurrentAction.FaceRight = true;

                    if (((SetPosition.z - SetPicture[SetRoleIndex].RoleObject.transform.position.z) / (SetPosition.x - SetPicture[SetRoleIndex].RoleObject.transform.position.x)) > 0.5f)
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 1)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(1);
                        }
                    }
                    else
                    {
                        if (SetData[SetRoleIndex].CurrentAction.getLastIndex() != 2)
                        {
                            SetData[SetRoleIndex].CurrentAction.setNextIndex(2);
                        }
                    }
                }
            }
            ////////////////////////决定顺序////////////////////////
        }
#else
        if (false)
        {

        }
#endif
        else
        {
            SetPicture[SetRoleIndex].RolePictureObject.transform.transform.rotation = new Quaternion(0, 0, 0, 0);
            if (SetPicture[SetRoleIndex].RolePictureObject.transform.position.x > SetPosition.x && SetPicture[SetRoleIndex].RolePictureObject.transform.position.z > SetPosition.z)
            {
                float Offset = Mathf.Atan((SetPicture[SetRoleIndex].RolePictureObject.transform.position.x - SetPosition.x) / (SetPicture[SetRoleIndex].RolePictureObject.transform.position.z - SetPosition.z)) * 180 / Mathf.PI + 180;
                SetPicture[SetRoleIndex].RolePictureObject.transform.Rotate(0f, Offset, 0f);
            }
            else if (SetPicture[SetRoleIndex].RolePictureObject.transform.position.x < SetPosition.x && SetPicture[SetRoleIndex].RolePictureObject.transform.position.z < SetPosition.z)
            {
                float Offset = Mathf.Atan((SetPicture[SetRoleIndex].RolePictureObject.transform.position.x - SetPosition.x) / (SetPicture[SetRoleIndex].RolePictureObject.transform.position.z - SetPosition.z)) * 180 / Mathf.PI;
                SetPicture[SetRoleIndex].RolePictureObject.transform.Rotate(0f, Offset, 0f);
            }
            else if (SetPicture[SetRoleIndex].RolePictureObject.transform.position.x > SetPosition.x && SetPicture[SetRoleIndex].RolePictureObject.transform.position.z < SetPosition.z)
            {
                float Offset = Mathf.Atan((SetPicture[SetRoleIndex].RolePictureObject.transform.position.x - SetPosition.x) / (SetPicture[SetRoleIndex].RolePictureObject.transform.position.z - SetPosition.z)) * 180 / Mathf.PI;
                SetPicture[SetRoleIndex].RolePictureObject.transform.Rotate(0f, Offset, 0f);
            }
            else
            {
                float Offset = Mathf.Atan((SetPicture[SetRoleIndex].RolePictureObject.transform.position.x - SetPosition.x) / (SetPicture[SetRoleIndex].RolePictureObject.transform.position.z - SetPosition.z)) * 180 / Mathf.PI + 180;
                if (Offset == 180)
                {
                    if (SetPosition.z < SetPicture[SetRoleIndex].RolePictureObject.transform.position.z)
                    {
                        SetPicture[SetRoleIndex].RolePictureObject.transform.Rotate(0f, Offset, 0f);
                    }
                }
                else if (Offset <= 360 && Offset >= -360)
                {
                    SetPicture[SetRoleIndex].RolePictureObject.transform.Rotate(0f, Offset, 0f);
                }
            }
        }
    }
    public bool CheckPosition(int PositionID)
    {
        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            if (ListRolePicture[RoleIndex].RolePosition == PositionID && ListRolePicture[RoleIndex].RoleNowBlood > 0)
            {
                return true;
            }
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            if (ListEnemyPicture[RoleIndex].RolePosition == PositionID && ListEnemyPicture[RoleIndex].RoleNowBlood > 0)
            {
                return true;
            }
        }

        for (int i = 0; i < ListStopPosition.Count; i++)
        {
            if (PositionID == ListStopPosition[i])
            {
                return true;
            }
        }

        for (int i = 0; i < ListStopPosition.Count; i++)
        {
            if (PositionID == ListStopPosition[i])
            {
                return true;
            }
        }

        if (ListPosition.Count <= PositionID)
        {
            return true;
        }
        if (1 > PositionID)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    int CheckSlantPosition(List<RolePicture> SetPicture, int PositionID, int SetRoleIndex, bool IsEnemy, bool IsRecursive)
    {
        int ReturnPositionID = -1;
        int NewPositionID = -1;
        Vector3 NowPosition = ListPosition[SetPicture[SetRoleIndex].RolePosition];
        Vector3 TargetPosition = ListPosition[PositionID];

        if (NowPosition.z > TargetPosition.z)
        {
            if (IsEnemy)
            {
                if (NowPosition.x > TargetPosition.x)
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                    }
                }
                else
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                    }
                }
            }
            else
            {
                if (NowPosition.x < TargetPosition.x)
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                    }
                }
                else
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                    }
                }
            }
        }
        else
        {
            if (IsEnemy)
            {
                if (NowPosition.x > TargetPosition.x)
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                    }
                }
                else
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                    }
                }
            }
            else
            {
                if (NowPosition.x < TargetPosition.x)
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                    }
                }
                else
                {
                    if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                    }
                }
            }
        }

        if (CheckPosition(NewPositionID))
        {
            if (NowPosition.x > TargetPosition.x)
            {
                NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow;
            }
            else
            {
                NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow;
            }

            if (CheckPosition(NewPositionID))
            {
                if (NowPosition.z < TargetPosition.z)
                {
                    if (IsEnemy)
                    {
                        if (NowPosition.x > TargetPosition.x)
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                            }
                        }
                        else
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                            }
                        }
                    }
                    else
                    {
                        if (NowPosition.x < TargetPosition.x)
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                            }
                        }
                        else
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 0)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                            }
                        }
                    }
                }
                else
                {
                    if (IsEnemy)
                    {
                        if (NowPosition.x > TargetPosition.x)
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                            }
                        }
                        else
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                            }
                        }
                    }
                    else
                    {
                        if (NowPosition.x < TargetPosition.x)
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow + 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition - 1;
                            }
                        }
                        else
                        {
                            if ((SetPicture[SetRoleIndex].RolePosition % PositionRow) == 1)
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + 1;
                            }
                            else
                            {
                                NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow - 1;
                            }
                        }
                    }
                }


                if (CheckPosition(NewPositionID))
                {
                    if (IsRecursive)
                    {
                        if ((NewPositionID = CheckSlantPosition(SetPicture, PositionID, SetRoleIndex, false, false)) != -1)
                        {
                            if (CheckPosition(NewPositionID))
                            {
                            }
                            else
                            {
                                ReturnPositionID = NewPositionID;
                            }
                        }
                    }
                }
                else
                {
                    ReturnPositionID = NewPositionID;
                }
            }
            else
            {
                ReturnPositionID = NewPositionID;
            }
        }
        else
        {
            ReturnPositionID = NewPositionID;
        }

        return ReturnPositionID;
    }
    ///////////////////////////设定图片前进目标(以下)///////////////////////////
    public void SetPictureForwardPosition(List<RolePicture> SetPicture, int PositionID, int SetRoleIndex)
    {
        SetPicture[SetRoleIndex].RolePiectureMoveSpeed = SetPicture[SetRoleIndex].RolePiectureMoveSpeed;
        //if (SetPicture[SetRoleIndex].RoleBio == 1)
        //{
        //    AudioEditer.instance.PlayOneShot("Move_hero");
        //}

        int NewPositionID = -1;
        Vector3 NowPosition = ListPosition[SetPicture[SetRoleIndex].RolePosition];
        Vector3 TargetPosition = ListPosition[PositionID];

        if (SetPicture[SetRoleIndex].RoleID == 65390 || SetPicture[SetRoleIndex].RoleID == 65391)
        {
            return;
        }

        if (SetPicture[SetRoleIndex].RolePosition != PositionID)
        {
            if (NowPosition.z == TargetPosition.z)
            {
                if (NowPosition.x > TargetPosition.x)
                {
                    NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow;
                }
                else
                {
                    NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow;
                }

                if (CheckPosition(NewPositionID))
                {
                    //////////////////////////走斜线(以下)////////////////////////
                    if ((NewPositionID = CheckSlantPosition(SetPicture, PositionID, SetRoleIndex, true, true)) == -1)
                    {
                        return;
                    }
                    //////////////////////////走斜线(以上)//////////////////////// 
                }
            }
            else if (NowPosition.x == TargetPosition.x)
            {
                //////////////////////////走斜线(以下)////////////////////////
                if ((NewPositionID = CheckSlantPosition(SetPicture, PositionID, SetRoleIndex, true, true)) == -1)
                {
                    return;
                }
                //////////////////////////走斜线(以上)//////////////////////// 
            }
            else
            {
                if (Math.Abs(NowPosition.x - TargetPosition.x) > OffsetX)
                {
                    if (NowPosition.x > TargetPosition.x)
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition - PositionRow;
                    }
                    else
                    {
                        NewPositionID = SetPicture[SetRoleIndex].RolePosition + PositionRow;
                    }

                    if (CheckPosition(NewPositionID))
                    {
                        //////////////////////////走斜线(以下)////////////////////////
                        if ((NewPositionID = CheckSlantPosition(SetPicture, PositionID, SetRoleIndex, true, true)) == -1)
                        {
                            return;
                        }
                        //////////////////////////走斜线(以上)////////////////////////   
                    }
                }
                else if (Math.Abs(NowPosition.z - TargetPosition.z) > OffsetZ)
                {
                    //////////////////////////走斜线(以下)////////////////////////
                    if ((NewPositionID = CheckSlantPosition(SetPicture, PositionID, SetRoleIndex, true, true)) == -1)
                    {
                        return;
                    }
                    //////////////////////////走斜线(以上)////////////////////////      
                }
                else
                {
                    if (CheckPosition(PositionID))
                    {
                        //////////////////////////走斜线(以下)////////////////////////
                        if ((NewPositionID = CheckSlantPosition(SetPicture, PositionID, SetRoleIndex, true, true)) == -1)
                        {
                            return;
                        }
                        //////////////////////////走斜线(以上)////////////////////////   
                    }
                    else
                    {
                        NewPositionID = PositionID;
                    }
                }
            }
        }
        else
        {
            NewPositionID = PositionID;
        }

        SetPicture[SetRoleIndex].RoleShowSkill = false;
        SetPicture[SetRoleIndex].RoleShowBlood = false;
        //GameObject.Find("Base" + SetPicture[SetRoleIndex].RolePosition).transform.Find("SkillObject").gameObject.SetActive(false);
        //GameObject.Find("Base" + SetPicture[SetRoleIndex].RolePosition).transform.Find("BloodObject").gameObject.SetActive(false);

        if (!SetPicture[SetRoleIndex].IsPicture)
        {
            SetPicture[SetRoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 0);
        }
        Vector3 SetPosition = ListPosition[NewPositionID];


        ////////////////////////决定顺序////////////////////////
#if ClashRoyale
        if (SetPicture[SetRoleIndex].IsPicture)
        {
            if (SetData[SetRoleIndex] != null)
            {
                if (SetData[SetRoleIndex].Stand != null)
                {
                    //if (SetData[SetRoleIndex].CurrentAction == SetData[SetRoleIndex].Hurt)
                    //{
                    //    if (SetPicture[SetRoleIndex].RolePictureTimer < 0.2f)
                    //    {
                    //        return;
                    //    }
                    //}

                    SetData[SetRoleIndex].CurrentAction = SetData[SetRoleIndex].Run;

                    if (!SetPicture[SetRoleIndex].RolePictureMoving)
                    {
                        if (SetData[SetRoleIndex].Run.PicChild.Count != 1)
                        {
                            if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p1") != null)
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p1") as Texture;

                            }
                            else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                            {
                                SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p1") as Texture;
                            }
                        }

                        SetPicture[SetRoleIndex].RolePictureObject.transform.position = SetData[SetRoleIndex].CurrentAction.getPosition(SetPicture[SetRoleIndex].RoleObject.transform.position, SetPicture[SetRoleIndex].RoleObject.transform.position.z, SetPicture[SetRoleIndex].RolePictureScaleType);
                        SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureOffset = SetData[SetRoleIndex].CurrentAction.getNextTextureOffset();
                        SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureScale = SetData[SetRoleIndex].CurrentAction.getTextureScale();
                        SetPicture[SetRoleIndex].RolePictureObject.transform.localScale = SetData[SetRoleIndex].CurrentAction.getLocalScale();
                    }
                    else
                    {

                        if (SetPicture[SetRoleIndex].RoleObject.transform.position.x < SetPosition.x && !SetData[SetRoleIndex].Stand.FaceRight)
                        {
                            if (SetData[SetRoleIndex].Run.PicChild.Count != 1)
                            {
                                if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p1") != null)
                                {
                                    SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p1") as Texture;

                                }
                                else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                                {
                                    SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p1") as Texture;

                                }
                            }

                            SetPicture[SetRoleIndex].RolePictureObject.transform.position = SetData[SetRoleIndex].CurrentAction.getPosition(SetPicture[SetRoleIndex].RoleObject.transform.position, SetPicture[SetRoleIndex].RoleObject.transform.position.z, SetPicture[SetRoleIndex].RolePictureScaleType);
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureOffset = SetData[SetRoleIndex].CurrentAction.getNextTextureOffset();
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureScale = SetData[SetRoleIndex].CurrentAction.getTextureScale();
                            SetPicture[SetRoleIndex].RolePictureObject.transform.localScale = SetData[SetRoleIndex].CurrentAction.getLocalScale();

                        }
                        else if (SetPicture[SetRoleIndex].RoleObject.transform.position.x > SetPosition.x && SetData[SetRoleIndex].Stand.FaceRight)
                        {
                            if (SetData[SetRoleIndex].Run.PicChild.Count != 1)
                            {
                                if (Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p1") != null)
                                {
                                    SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[SetRoleIndex].RoleID.ToString() + "/p1") as Texture;

                                }
                                else if (DictAssetBundle.ContainsKey("Role" + SetPicture[SetRoleIndex].RoleID.ToString()))
                                {
                                    SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[SetRoleIndex].RoleID.ToString()].Load("p1") as Texture;

                                }
                            }

                            SetPicture[SetRoleIndex].RolePictureObject.transform.position = SetData[SetRoleIndex].CurrentAction.getPosition(SetPicture[SetRoleIndex].RoleObject.transform.position, SetPicture[SetRoleIndex].RoleObject.transform.position.z, SetPicture[SetRoleIndex].RolePictureScaleType);
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureOffset = SetData[SetRoleIndex].CurrentAction.getNextTextureOffset();
                            SetPicture[SetRoleIndex].RolePictureObject.renderer.material.mainTextureScale = SetData[SetRoleIndex].CurrentAction.getTextureScale();
                            SetPicture[SetRoleIndex].RolePictureObject.transform.localScale = SetData[SetRoleIndex].CurrentAction.getLocalScale();
                        }
                    }
                }
            }
        }

        ChangeDirection(SetPicture, SetData, SetPicture[SetRoleIndex].RoleObject.transform.position, SetPosition, SetRoleIndex);
#else
        ChangeDirection(SetPicture, SetPicture[SetRoleIndex].RoleObject.transform.position, SetPosition, SetRoleIndex);
#endif
        if (IsSkip)
        {
            SetPicture[SetRoleIndex].RolePictureMoving = true;
            SetPicture[SetRoleIndex].RolePictureNewPosition = SetPosition;
            SetPicture[SetRoleIndex].RolePosition = NewPositionID;
            SetPicture[SetRoleIndex].RoleObject.transform.position = SetPosition;
        }
        else
        {
            SetPicture[SetRoleIndex].RolePictureMoving = true;
            ////////////////////////决定顺序////////////////////////
            SetPicture[SetRoleIndex].RolePictureNewPosition = SetPosition;
            newMainRoleForwardPosition = SetPicture[SetRoleIndex].RolePictureNewPosition - SetPicture[SetRoleIndex].RoleObject.transform.position;
            MoveDistance = Vector3.Distance(SetPicture[SetRoleIndex].RolePictureNewPosition, SetPicture[SetRoleIndex].RoleObject.transform.position);
            SetPicture[SetRoleIndex].RolePictureCheckPosition = newMainRoleForwardPosition / MoveDistance * SetPicture[SetRoleIndex].RolePiectureMoveSpeed * 80;
            SetPicture[SetRoleIndex].RolePosition = NewPositionID;
            SetListPosition(PositionRow, 2);
        }

        if (SetPicture[SetRoleIndex].RoleMovePosition == NewPositionID)
        {
            SetPicture[SetRoleIndex].RoleMovePosition = 0;
            if (SetPicture[SetRoleIndex].RoleTargetObject.activeSelf)
            {
                SetPicture[SetRoleIndex].RoleTargetIndex = -1;
                SetPicture[SetRoleIndex].RoleTargetObject.SetActive(false);
            }
            //AddSequence();
        }

    }
    ///////////////////////////设定图片前进目标(以上)///////////////////////////
    #endregion

    #region Find
    public int GetIndexByPointID(List<RolePicture> SetPicture, string PointID)
    {

        for (int GetRoleIndex = 0; GetRoleIndex < SetPicture.Count; GetRoleIndex++)
        {
            if (SetPicture[GetRoleIndex].RolePicturePointID == PointID)
            {
                return GetRoleIndex;
            }
        }
        return -1;
    }
    public int GetIndexByName(List<RolePicture> SetPicture, string Name)
    {

        for (int GetRoleIndex = 0; GetRoleIndex < SetPicture.Count; GetRoleIndex++)
        {
            if (SetPicture[GetRoleIndex].RoleObject.name == Name)
            {
                return GetRoleIndex;
            }
        }
        return -1;
    }
    public int GetIndexByCharacterRoleID(List<RolePicture> SetPicture, int CharacterRoleID)
    {

        for (int GetRoleIndex = 0; GetRoleIndex < SetPicture.Count; GetRoleIndex++)
        {
            if (SetPicture[GetRoleIndex].RoleCharacterRoleID == CharacterRoleID)
            {
                return GetRoleIndex;
            }
        }
        return -1;
    }
    GameObject FindChildObject(GameObject RootObject, string ObjectName)
    {
        foreach (Component c in RootObject.GetComponentsInChildren(typeof(Transform), true))
        {
            if (c.name == ObjectName)
            {
                return c.gameObject;
            }
        }
        return null;
    }
    #endregion

    #region Update
    void FixedUpdate()
    {
#if ClashRoyale
        UpdatePicture(ListRolePicture, PicRoleList, ListEnemyPicture, PicEnemyList);
        UpdatePicture(ListEnemyPicture, PicEnemyList, ListRolePicture, PicRoleList);
#else
        UpdatePicture(ListRolePicture, ListEnemyPicture);
        UpdatePicture(ListEnemyPicture, ListRolePicture);
#endif

        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            if (WeatherID == 3)
            {
                WeatherTimer += Time.deltaTime;
                if (WeatherTimer > WeatherTime)
                {
                    Weather = GameObject.Instantiate(Resources.Load("Prefab/Weather/TianQi_ShanDian", typeof(GameObject))) as GameObject;
                    Weather.name = "ShanDian";

                    WeatherTimer = 0;
                    WeatherTime = UnityEngine.Random.Range(5, 10);
                    AudioEditer.instance.PlayOneShot("bgs_flashlight");
                }
            }
        }

        if (FightTimer > 0 && !IsLock)
        {
            FightTimer += Time.deltaTime;
            if (FightTimer > 8)
            {
                PictureCreater.instance.AttackCount = 0;
                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                {
                    ListRolePicture[RoleIndex].RolePictureStartAttack = false;
                }

                for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
                {
                    ListEnemyPicture[RoleIndex].RolePictureStartAttack = false;
                }
            }
        }

        if (IsFight && !IsLock)
        {
            LimitTimer += Time.deltaTime;
            if (LimitTimer > LimitTime)
            {
                IsFight = false;
                StartCoroutine(ShowLose());
            }
        }
        //else if (IsRoleInGate)  //这是没人上阵时会闪先拿掉了
        //{
        //    if (FightCount < 5 || SkillFire1 == 1 || SkillFire2 == 1 || SkillFire3 == 1)
        //    {
        //        FightHintTimer += Time.deltaTime;
        //        if (FightHintTimer > 0.01f)
        //        {
        //            FightHintTimer -= 0.01f;
        //            if (FightHint)
        //            {
        //                FightHintColor -= 0.05f;
        //                if (FightHintColor < 0.5f)
        //                {
        //                    FightHint = false;
        //                }
        //            }
        //            else
        //            {
        //                FightHintColor += 0.05f;
        //                if (FightHintColor > 1f)
        //                {
        //                    FightHintColor = 1;
        //                    FightHint = true;
        //                }
        //            }
        //            for (int i = 1; i < PositionRow * PositionColumn / 2; i++)
        //            {
        //                if (GameObject.Find("Position" + i.ToString()) != null)
        //                {
        //                    //Debug.Log(GameObject.Find("Base" + i.ToString()).renderer.material.mainTexture);


        //                    GameObject.Find("Position" + i.ToString()).renderer.material.color = new Color(1, 1, 1, FightHintColor);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 1; i < PositionRow * PositionColumn / 2; i++)
        //        {
        //            if (GameObject.Find("Base" + i.ToString()) != null)
        //            {
        //                //Debug.Log(GameObject.Find("Base" + i.ToString()).renderer.material.mainTexture);


        //                GameObject.Find("Base" + i.ToString()).renderer.material.color = new Color(1, 1, 1, 1);
        //            }
        //        }
        //    }
        //}
    }



    void UpdatePicture(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture)
    {
        for (int RoleIndex = 0; RoleIndex < SetPicture.Count; RoleIndex++)
        {
            if (SetPicture[RoleIndex].RoleObject.activeSelf)  //只有显示的图片要运算
            {
                /////////////////////////眨眼////////////////////////
                if (SetPicture[RoleIndex].RoleID > 60010 && SetPicture[RoleIndex].RoleID < 60100 && !IsFight)
                {
                    if (SetPicture[RoleIndex].RoleCloseEye)
                    {
                        foreach (Component c in SetPicture[RoleIndex].RoleObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                        {
                            if (c.name.IndexOf("biyan") > -1)
                            {
                                SetPicture[RoleIndex].RoleCloseEye = false;
                                c.gameObject.SetActive(false);
                                break;
                            }
                        }
                    }
                    if (UnityEngine.Random.Range(0, 100) == 0)
                    {
                        foreach (Component c in SetPicture[RoleIndex].RoleObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                        {

                            if (c.name.IndexOf("biyan") > -1)
                            {
                                SetPicture[RoleIndex].RoleCloseEye = true;
                                c.gameObject.SetActive(true);
                                break;
                            }
                        }
                    }


                }
                /////////////////////////眨眼////////////////////////
                if (SetPicture[RoleIndex].RoleSkillPoint >= 1000)
                {
                    if (!SetPicture[RoleIndex].RoleShowSkill && SetPicture[RoleIndex].RoleNowBlood > 0)
                    {
                        //GameObject.Find("Base" + SetPicture[RoleIndex].RolePosition).transform.Find("SkillObject").gameObject.SetActive(true);
                        SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteSkill.SetActive(true);
                        SetPicture[RoleIndex].RoleShowSkill = true;
                    }
                }
                //if (SetPicture[RoleIndex].RoleNowBlood > 0)
                //{
                //    if (!SetPicture[RoleIndex].RoleShowBlood && SetPicture[RoleIndex].RolePosition > 0)
                //    {
                //        Debug.LogError(SetPicture[RoleIndex].RolePosition);
                //        GameObject.Find("Base" + SetPicture[RoleIndex].RolePosition).transform.Find("BloodObject").gameObject.SetActive(true);
                //        SetPicture[RoleIndex].RoleShowBlood = true;
                //    }
                //}


                #region RolePictureMoving
                //////////////////////判断人物移动(以下)//////////////////////      
                if (SetPicture[RoleIndex].RolePictureMoving)
                {
                    MoveDistance = Vector3.Distance(SetPicture[RoleIndex].RolePictureNewPosition, SetPicture[RoleIndex].RoleObject.transform.position);

                    if (MoveDistance >= Vector3.Distance(Vector3.zero, SetPicture[RoleIndex].RolePictureCheckPosition * Time.deltaTime) && !IsSkip)
                    {
                        SetPicture[RoleIndex].RoleObject.transform.position += SetPicture[RoleIndex].RolePictureCheckPosition * Time.deltaTime;

                        /////////////////auto操作/////////////////
                        if (!IsLock)
                        {
                            for (int i = 0; i < ListRolePicture.Count; i++)
                            {
                                UpdateTargetObject(i);
                            }
                        }
                        /////////////////auto操作/////////////////
                    }
                    else
                    {
                        //SetPicture[RoleIndex].RoleObject.transform.position = SetPicture[RoleIndex].RolePictureNewPosition;
                        SetPicture[RoleIndex].RolePictureMoving = false;

                        if (!SetPicture[RoleIndex].IsPicture)
                        {
                            if (SetPicture[RoleIndex].RoleMoveNowStep == 0 || !IsFight || SetPicture[RoleIndex].RoleObject.name.IndexOf("NPC") > -1)
                            {
                                SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 2);
                            }
                        }
#if ClahsRoyale
                        else
                        {
                            ////////////////////////////////////////////////////////////////停下來要回归原始动作(以下)////////////////////////////////////////////////////////////////////////////////
                            if (SetData[RoleIndex].CurrentAction.Count > 3)
                            {
                                SetData[RoleIndex].Stand.setNextIndex(SetData[RoleIndex].CurrentAction.getLastIndex());
                                SetData[RoleIndex].CurrentAction = SetData[RoleIndex].Stand;
                                SetData[RoleIndex].Stand.FaceRight = SetData[RoleIndex].Run.FaceRight;
                                if (SetData[RoleIndex].Stand.FaceRight)
                                {
                                    SetPicture[RoleIndex].RolePictureObject.transform.localPosition = new Vector3(SetData[RoleIndex].Stand.Left * SetPicture[RoleIndex].RolePictureScaleType, SetData[RoleIndex].Stand.Top + (SetPicture[RoleIndex].RolePictureScaleType - 1) * (0.24f + SetData[RoleIndex].Stand.Top), -SetData[RoleIndex].Stand.Left * SetPicture[RoleIndex].RolePictureScaleType);
                                }
                                else
                                {
                                    SetPicture[RoleIndex].RolePictureObject.transform.localPosition = new Vector3(-SetData[RoleIndex].Stand.Left * SetPicture[RoleIndex].RolePictureScaleType, SetData[RoleIndex].Stand.Top + (SetPicture[RoleIndex].RolePictureScaleType - 1) * (0.24f + SetData[RoleIndex].Stand.Top), SetData[RoleIndex].Stand.Left * SetPicture[RoleIndex].RolePictureScaleType);
                                }

                                if (SetData[RoleIndex].Stand.PicChild.Count != 1)
                                {
                                    if (Resources.Load("Role/Role" + SetPicture[RoleIndex].RoleID.ToString() + "/p0") != null)
                                    {
                                        SetPicture[RoleIndex].RolePictureObject.renderer.material.mainTexture = Resources.Load("Role/Role" + SetPicture[RoleIndex].RoleID.ToString() + "/p0") as Texture;
                                    }
                                    else
                                    {
                                        SetPicture[RoleIndex].RolePictureObject.renderer.material.mainTexture = DictAssetBundle["Role" + SetPicture[RoleIndex].RoleID.ToString()].Load("p0") as Texture;
                                    }
                                }
                                SetPicture[RoleIndex].RolePictureObject.transform.position = SetData[RoleIndex].CurrentAction.getPosition(SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position.z, SetPicture[RoleIndex].RolePictureScaleType);
                                SetPicture[RoleIndex].RolePictureObject.renderer.material.mainTextureOffset = SetData[RoleIndex].CurrentAction.getNextTextureOffset();
                                SetPicture[RoleIndex].RolePictureObject.renderer.material.mainTextureScale = SetData[RoleIndex].CurrentAction.getTextureScale();

                                if (SetPicture[RoleIndex].RolePictureFight && SetPicture[RoleIndex].RoleNowBlood > 0)
                                {
                                    //SetPicture[RoleIndex].RoleFightSkill = 0;
                                    PlayAnimation(SetPicture, SetData, RoleIndex, 0.1f, "Stand");

                                    //if (SetPicture[RoleIndex].RoleFightSkill < 2)
                                    //{
                                    //    float LeastDistance = SetPicture[RoleIndex].RoleFightDistance + 1f;
                                    //    int LeastIndex = -1;
                                    //    float MonsterDistance = 0;
                                    //    for (int MonsterIndex = 0; MonsterIndex < SetEnemyPicture.Count; MonsterIndex++)
                                    //    {
                                    //        if (SetEnemyPicture[MonsterIndex].RoleNowBlood > 0)
                                    //        {
                                    //            MonsterDistance = Vector2.Distance(new Vector2(SetEnemyPicture[MonsterIndex].RoleObject.transform.position.x, SetEnemyPicture[MonsterIndex].RoleObject.transform.position.z), new Vector2(SetPicture[RoleIndex].RoleObject.transform.position.x, SetPicture[RoleIndex].RoleObject.transform.position.z));
                                    //            if (MonsterDistance < LeastDistance)
                                    //            {
                                    //                LeastIndex = MonsterIndex;
                                    //                LeastDistance = MonsterDistance;
                                    //            }
                                    //        }
                                    //    }

                                    //    if (SetPicture[RoleIndex].RoleFightIndex.Count > 0)
                                    //    {
                                    //        if (SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleNowBlood > 0)
                                    //        {
                                    //            if (LeastIndex == SetPicture[RoleIndex].RoleFightIndex[0])
                                    //            {
                                    //                PlayAnimation(SetPicture, SetData, RoleIndex, 0.1f, "Attack");
                                    //                if (!SetPicture[RoleIndex].RoleFightIndex.Contains(LeastIndex))
                                    //                {
                                    //                    SetPicture[RoleIndex].RoleFightIndex.Add(LeastIndex);
                                    //                    SetPicture[RoleIndex].RoleFightType.Add(0);
                                    //                    if (SetPicture[RoleIndex].RoleAttack - SetEnemyPicture[LeastIndex].RoleDefend >= SetPicture[RoleIndex].RoleAttack / 10)
                                    //                    {
                                    //                        SetPicture[RoleIndex].RoleFightDamige.Add((int)(SetPicture[RoleIndex].RoleAttack - SetEnemyPicture[LeastIndex].RoleDefend));
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        SetPicture[RoleIndex].RoleFightDamige.Add((int)(SetPicture[RoleIndex].RoleAttack / 10));
                                    //                    }
                                    //                }
                                    //                /////////////////决定攻击方向(以下)/////////////////
                                    //                ChangeDirection(SetPicture, SetData, SetPicture[RoleIndex].RolePictureObject.transform.position, SetEnemyPicture[LeastIndex].RoleObject.transform.position, RoleIndex);
                                    //                /////////////////决定攻击方向(以上)/////////////////
                                    //            }
                                    //            else
                                    //            {
                                    //                if (!SetPicture[RoleIndex].RolePictureTrace)
                                    //                {
                                    //                    LeastIndex = SetPicture[RoleIndex].RoleFightIndex[0];
                                    //                    float PartnerDistance = Vector2.Distance(new Vector2(SetEnemyPicture[LeastIndex].RoleObject.transform.position.x, SetEnemyPicture[LeastIndex].RoleObject.transform.position.z), new Vector2(SetPicture[RoleIndex].RoleObject.transform.position.x, SetPicture[RoleIndex].RoleObject.transform.position.z));
                                    //                    float MoveOffset = SetPicture[RoleIndex].RoleFightDistance;
                                    //                    float PositionX = UnityEngine.Random.Range(-1.4f, 1.4f);
                                    //                    float PositionZ = UnityEngine.Random.Range(-1.4f, 1.4f);
                                    //                    Vector3 MoveVector = new Vector3(PositionX, 0, PositionZ) + (SetEnemyPicture[LeastIndex].RoleObject.transform.position - SetPicture[RoleIndex].RoleObject.transform.position) * (PartnerDistance - MoveOffset) / PartnerDistance;
                                    //                    //////////怪向伙伴(以下)//////////
                                    //                    SetPictureForwardPosition(SetPicture, SetData, SetPicture[RoleIndex].RoleObject.transform.position + MoveVector, RoleIndex, SetPicture[RoleIndex].RolePiectureMoveSpeed, 0, "m_" + SetPicture[RoleIndex].RoleID + "_1_0", 0, 0, 0, 1, 1);
                                    //                    //////////怪向伙伴(以下)//////////
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                else
                                {
                                    PlayAnimation(SetPicture, SetData, RoleIndex, 0.2f, "Stand");
                                }
                            }
                            ////////////////////////////////////////////////////////////////停下來要回归原始动作(以上)////////////////////////////////////////////////////////////////////////////////
                        }
#endif

                        /////////////////////判断走入地形（以下）/////////////////////
                        if (IsFight)
                        {
                            for (int t = 0; t < ListTerrainPosition.Count; t++)
                            {
                                if (SetPicture[RoleIndex].RolePosition == ListTerrainPosition[t])
                                {
                                    TextTranslator.TerrainInfo ti = TextTranslator.instance.GetTerrainInfoByID(ListTerrainID[t]);
                                    if (ti.terrainID == 5)
                                    {
                                        if (SetPicture[RoleIndex].RoleRace == 3)  //飞机不踩雷
                                        {
                                            break;
                                        }
                                        AudioEditer.instance.PlayOneShot("Hit_boom");
                                    }
                                    else
                                    {
                                        AudioEditer.instance.PlayOneShot("ui_recieve");
                                    }
                                    Destroy(GameObject.Find("TerrainObject" + ListTerrainPosition[t]));
                                    Buff NewBuff = TextTranslator.instance.GetBuffByID(ti.buff);
                                    if (NewBuff != null)
                                    {
                                        RoleAddBuff(SetPicture, RoleIndex, NewBuff);
                                    }
                                    ListTerrainID.RemoveAt(t);
                                    ListTerrainPosition.RemoveAt(t);

                                    break;
                                }
                            }
                        }
                        /////////////////////判断走入地形（以上）/////////////////////


                        /////////////////////判断威斯恪（以上）/////////////////////
                        if (SetPicture[RoleIndex].RoleID == 65901 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && SetPicture[RoleIndex].RolePictureMonster && IsFight)
                        {
                            IsLock = true;
                            StartCoroutine(ShowWesker(SetPicture, RoleIndex));
                        }

                        /////////////////////判断威斯恪（以上）/////////////////////
                    }
                }
                //////////////////////判断人物移动(以上)//////////////////////
                #endregion
                else
                //////////////////////判断人物静止(以下)//////////////////////
                {
#if ClashRoyale
                    if (SetPicture[RoleIndex].IsPicture)
                    {
                        SetPicture[RoleIndex].RolePictureTimer += Time.deltaTime;
                        float PlayTimer = 0;
                        if (SetPicture[RoleIndex].RolePictureAnimation)
                        {
                            PlayTimer = SetPicture[RoleIndex].RolePictureAnimationSpeed;
                        }
                        else
                        {
                            PlayTimer = 0.2f;
                        }
                        if ((SetPicture[RoleIndex].RolePictureTimer - PlayTimer) > 0) //每过0.2秒换一张图
                        {
                            SetPicture[RoleIndex].RolePictureTimer -= PlayTimer;

                            ///////////////////////////////////////怪被击飞(以下)/////////////////////////////////////
                            //if (SetData[RoleIndex].CurrentAction.PicChild.Count < 4)
                            //{
                            //    SetPicture[RoleIndex].RolePictureTransparent -= 0.06f;
                            //    if (SetPicture[RoleIndex].RolePictureTransparent < 0.1f)
                            //    {
                            //        if (IsRoleInGate)
                            //        {
                            //            SetPicture[RoleIndex].RoleObject.transform.position = new Vector3(50, 50, 50);
                            //            SetPicture[RoleIndex].RolePictureMonster = false;
                            //        }
                            //        SetPicture[RoleIndex].RoleObject.SetActive(false);
                            //    }
                            //    SetPicture[RoleIndex].RolePictureObject.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                            //    SetPicture[RoleIndex].RolePictureObject.transform.renderer.material.SetColor("_Color", new Color(1, 1, 1, SetPicture[RoleIndex].RolePictureTransparent));
                            //}
                            ///////////////////////////////////////怪被击飞(以上)/////////////////////////////////////
                            //else if (SetData[RoleIndex].CurrentAction.PicChild.Count == 20 && SetData[RoleIndex].CurrentAction.Count == 5)
                            //{
                            //    /////////////////怪反击(以下)/////////////////
                            //    if (SetPicture[RoleIndex].RolePictureMonster)
                            //    {
                            //        for (int PartnerIndex = 0; PartnerIndex < SetEnemyPicture.Count - 6; PartnerIndex++)
                            //        {
                            //            if (SetEnemyPicture[PartnerIndex].RoleNowBlood > 0)
                            //            {
                            //                if (SetEnemyPicture[PartnerIndex].RoleFightIndex.Contains(RoleIndex))
                            //                {
                            //                    SetPicture[RoleIndex].RolePictureFight = true;
                            //                    PlayAnimation(SetPicture, SetData, RoleIndex, 0.1f, "Attack");

                            //                    if (!SetPicture[RoleIndex].RoleFightIndex.Contains(PartnerIndex))
                            //                    {
                            //                        SetPicture[RoleIndex].RoleFightIndex.Add(PartnerIndex);
                            //                        SetPicture[RoleIndex].RoleFightType.Add(0);
                            //                        if (SetPicture[RoleIndex].RoleAttack - SetEnemyPicture[PartnerIndex].RoleDefend >= SetPicture[RoleIndex].RoleAttack / 10)
                            //                        {
                            //                            SetPicture[RoleIndex].RoleFightDamige.Add((int)(SetPicture[RoleIndex].RoleAttack - SetEnemyPicture[PartnerIndex].RoleDefend));
                            //                        }
                            //                        else
                            //                        {
                            //                            SetPicture[RoleIndex].RoleFightDamige.Add((int)(SetPicture[RoleIndex].RoleAttack / 10));
                            //                        }
                            //                    }

                            //                    float PartnerDistance = Vector2.Distance(new Vector2(SetEnemyPicture[PartnerIndex].RoleObject.transform.position.x, SetEnemyPicture[PartnerIndex].RoleObject.transform.position.z), new Vector2(SetPicture[RoleIndex].RoleObject.transform.position.x, SetPicture[RoleIndex].RoleObject.transform.position.z));
                            //                    if (PartnerDistance > SetPicture[RoleIndex].RoleFightDistance)
                            //                    {
                            //                        float MoveOffset = SetPicture[RoleIndex].RoleFightDistance;
                            //                        float PositionX = UnityEngine.Random.Range(-1.4f, 1.4f);
                            //                        float PositionZ = UnityEngine.Random.Range(-1.4f, 1.4f);
                            //                        Vector3 MoveVector = new Vector3(PositionX, 0, PositionZ) + (SetEnemyPicture[PartnerIndex].RoleObject.transform.position - SetPicture[RoleIndex].RoleObject.transform.position) * (PartnerDistance - MoveOffset) / PartnerDistance;
                            //                        //////////怪向伙伴(以下)//////////
                            //                        SetPictureForwardPosition(SetPicture, SetData, SetPicture[RoleIndex].RoleObject.transform.position + MoveVector, RoleIndex, SetPicture[RoleIndex].RolePiectureMoveSpeed, 0, "m_" + SetPicture[RoleIndex].RoleID + "_1_0", 0, 0, 0, 1, 1);
                            //                        //////////怪向伙伴(以下)//////////
                            //                    }
                            //                    break;
                            //                }
                            //            }
                            //        }
                            //    }
                            //    /////////////////怪反击(以上)/////////////////
                            //}

                            if (SetData[RoleIndex].CurrentAction != null && SetPicture[RoleIndex].RolePictureAttackable)
                            {
                                /////////////动作做完才换图(以下)/////////////
                                if (SetPicture[RoleIndex].RolePictureNowFrame == 1)
                                {
                                    Debug.Log(SetPicture[RoleIndex].RolePictureNowFrame);
                                    if (SetPicture[RoleIndex].RolePictureFight)
                                    {
                                        Debug.Log("SSSSSSSSSSSSSSSSS" + SetData[RoleIndex].CurrentAction.Index + " " + SetData[RoleIndex].CurrentAction.Count);
                                        PlayAnimation(SetPicture, SetData, RoleIndex, 0.1f, "Stand");
                                        if (SetPicture[RoleIndex].RoleFightIndex.Count > 0)
                                        {
                                            /////////////////决定攻击方向(以下)/////////////////
                                            ChangeDirection(SetPicture, SetData, SetPicture[RoleIndex].RolePictureObject.transform.position, SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, RoleIndex);
                                            /////////////////决定攻击方向(以上)/////////////////
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("DDDDDDDDDDDDDDDDD" + SetData[RoleIndex].CurrentAction.Index + " " + SetData[RoleIndex].CurrentAction.Count);
                                        int LastIndex = SetData[RoleIndex].CurrentAction.Index;
                                        //if (SetData[RoleIndex].CurrentAction.Count == 3)
                                        //{
                                        //    if (LastIndex == 1)
                                        //    {
                                        //        SetData[RoleIndex].CurrentAction.setNextIndex(2);
                                        //    }
                                        //    else if (LastIndex == 2)
                                        //    {
                                        //        SetData[RoleIndex].CurrentAction.setNextIndex(3);
                                        //    }
                                        //    else
                                        //    {
                                        //        SetData[RoleIndex].CurrentAction.setNextIndex(4);
                                        //    }

                                        //}
                                        //else if (SetData[RoleIndex].CurrentAction.Count == 1)
                                        //{
                                        //    SetData[RoleIndex].CurrentAction.setNextIndex(3);
                                        //}
                                        PlayAnimation(SetPicture, SetData, RoleIndex, 0.2f, "Stand");
                                    }
                                    SetPicture[RoleIndex].RolePictureNowFrame = 0;
                                }
                                else
                                {
                                    int Point = SetData[RoleIndex].CurrentAction.Pointer;
                                    SetPicture[RoleIndex].RolePictureObject.transform.position = SetData[RoleIndex].CurrentAction.getPosition(SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position.z, SetPicture[RoleIndex].RolePictureScaleType);
                                    SetPicture[RoleIndex].RolePictureObject.renderer.material.mainTextureOffset = SetData[RoleIndex].CurrentAction.getNextTextureOffset();
                                    SetPicture[RoleIndex].RolePictureObject.renderer.material.mainTextureScale = SetData[RoleIndex].CurrentAction.getTextureScale();

                                    int LPoint = SetData[RoleIndex].CurrentAction.PicChild.Count / SetData[RoleIndex].CurrentAction.Count;

                                    ////////////////////////////////////////////////////////////////攻击特效（以下）//////////////////////////////////////////////////////////////////////////////
                                    //if (SetPicture[RoleIndex].RolePictureFight && SetPicture[RoleIndex].RoleFightIndex.Count > 0 && Point == ((SetData[RoleIndex].CurrentAction.Index - 1) * LPoint) + 6)
                                    //{
                                    //    if (SetPicture[RoleIndex].RoleFightDistance < 5)
                                    //    {
                                    //        if (SetPicture[RoleIndex].RoleFightSkill == 0)
                                    //        {
                                    //            EffectMaker.instance.Create2DEffect("s2", "s2", SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, new Vector3(2f, 0, 2f), 0.2f, 0, 0.2f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, false, false, false, true, true, false, true, SetPicture[RoleIndex].RolePictureMonster);
                                    //        }
                                    //    }
                                    //    else if (SetPicture[RoleIndex].RoleFightDistance < 7)
                                    //    {
                                    //        if (SetPicture[RoleIndex].RoleFightSkill == 0)
                                    //        {
                                    //            EffectMaker.instance.Create2DEffect("s3", "s3", SetPicture[RoleIndex].RoleObject.transform.position, SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, new Vector3(4f, 0, 0.4f), 0.4f, 0.1f, 0, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, true, false, false, false, true, false, true, SetPicture[RoleIndex].RolePictureMonster);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (SetPicture[RoleIndex].RoleFightSkill == 0)
                                    //        {
                                    //            //EffectMaker.instance.Create2DEffect("s2", "s1", SetPicture[RoleIndex].RoleObject.transform.position, SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, new Vector3(2f, 0, 2f), 0.2f, 0.1f, 0, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, false, false, false, false, true, false, true, SetPicture[RoleIndex].RolePictureMonster);
                                    //            EffectMaker.instance.Create2DEffect("s3", "s3", SetPicture[RoleIndex].RoleObject.transform.position, SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, new Vector3(4f, 0, 0.4f), 0.4f, 0.1f, 0, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, true, false, false, false, true, false, true, SetPicture[RoleIndex].RolePictureMonster);
                                    //        }
                                    //    }
                                    //}
                                    ////////////////////////////////////////////////////////////////////攻击特效（以上）//////////////////////////////////////////////////////////////////////////

                                    if (Point > SetData[RoleIndex].CurrentAction.Pointer && SetData[RoleIndex].CurrentAction.NowAnimation == "attack") //除待机跑步外只播一次
                                    {
                                        SetPicture[RoleIndex].RolePictureNowFrame = 1;
                                    }
                                    Debug.Log(SetPicture[RoleIndex].RolePictureNowFrame + " " + SetData[RoleIndex].CurrentAction.PicChild.Count + " " + Point + " " + SetData[RoleIndex].CurrentAction.Pointer + " " + SetData[RoleIndex].CurrentAction.Count + " " + SetData[RoleIndex].CurrentAction.Index);
                                }
                                /////////////动作做完才换图(以上)/////////////
                            }
                        }
                    }
#endif





                    #region Role Dead
                    //////////////////////////人物死亡(以下)//////////////////////
                    if (SetPicture[RoleIndex].RoleNowBlood < 0)
                    {
                        SetPicture[RoleIndex].RolePictureTimer += Time.deltaTime;
                        float PlayTimer = 2f;
                        if (SetPicture[RoleIndex].RolePictureTimer > PlayTimer) //每过0.2秒换一张图
                        {
                            SetPicture[RoleIndex].RolePictureTimer -= PlayTimer;
                            SetPicture[RoleIndex].RolePictureTimer += 1.9f;


                            SetPicture[RoleIndex].RolePictureTransparent -= 0.1f;
                            if (SetPicture[RoleIndex].RolePictureTransparent < 0.1f)
                            {
                                ////////////////////死里逃生//////////////////
                                //if (SetPicture[RoleIndex].RoleRelife > 0 && SetPicture[RoleIndex].RolePicturePointID != KillEnemyID.ToString())
                                //{
                                //    //foreach (var r in SetPicture)
                                //    {
                                //        //if (r.RoleSkillPoint == -1)
                                //        {
                                //            SetPicture[RoleIndex].RolePosition = SetPicture[RoleIndex].RolePosition;
                                //            SetPicture[RoleIndex].RoleObject.transform.position = ListPosition[SetPicture[RoleIndex].RolePosition];
                                //            SetPicture[RoleIndex].RolePictureTimer = 0;
                                //            SetPicture[RoleIndex].RoleObject.SetActive(true);
                                //            SetPicture[RoleIndex].RolePictureTransparent = 1;
                                //            SetPicture[RoleIndex].RolePictureAttackable = true;

                                //            if (!SetPicture[RoleIndex].IsPicture)
                                //            {
                                //                SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().Play("idle");
                                //                if (SetPicture[RoleIndex].RoleID == 65901)
                                //                {
                                //                    SetPicture[RoleIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("idle");
                                //                }
                                //            }

                                //            AudioEditer.instance.PlayOneShot("ui_qianghua");
                                //            GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WF_revive", typeof(GameObject)), ListPosition[SetPicture[RoleIndex].RolePosition], Quaternion.identity) as GameObject;
                                //            go.AddComponent<DestroySelf>();

                                //            foreach (Component c in SetPicture[RoleIndex].RoleObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true))
                                //            {
                                //                foreach (var m in c.renderer.materials)
                                //                {
                                //                    m.shader = Shader.Find("LB/HeroRim");
                                //                    m.SetColor("_Color", new Color(1, 1, 1, SetPicture[RoleIndex].RolePictureTransparent));
                                //                }

                                //            }
                                //            foreach (Component c in SetPicture[RoleIndex].RoleObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                                //            {
                                //                foreach (var m in c.renderer.materials)
                                //                {
                                //                    Debug.Log(c.name);
                                //                    if (c.name != "Shadow" && c.name != "Dao_glow")
                                //                    {
                                //                        m.shader = Shader.Find("LB/HeroRim");
                                //                        m.SetColor("_Color", new Color(1, 1, 1, SetPicture[RoleIndex].RolePictureTransparent));
                                //                    }
                                //                }
                                //            }

                                //            if (SetPicture[RoleIndex].RoleSkill2 == 2040)
                                //            {
                                //                SetPicture[RoleIndex].RoleSkillPoint = 0;
                                //                SetPicture[RoleIndex].RoleRelife = 0;
                                //                SetPicture[RoleIndex].RoleNowBlood = SetPicture[RoleIndex].RoleMaxBlood / 4;
                                //            }
                                //            else
                                //            {
                                //                SetPicture[RoleIndex].RoleSkillPoint = 1000;
                                //                SetPicture[RoleIndex].RoleRelife /= 2;
                                //                SetPicture[RoleIndex].RoleNowBlood = SetPicture[RoleIndex].RoleMaxBlood * 35 / 100;
                                //            }

                                //            SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().mSlider.value = SetPicture[RoleIndex].RoleNowBlood / SetPicture[RoleIndex].RoleMaxBlood;
                                //            break;
                                //        }
                                //    }
                                //}
                                ////////////////////死里逃生//////////////////

                                if (IsRoleInGate)
                                {
                                    SetPicture[RoleIndex].RoleObject.transform.position = new Vector3(50, 50, 50);
                                }
                                SetPicture[RoleIndex].RoleObject.SetActive(false);
                            }
                            foreach (Component c in SetPicture[RoleIndex].RoleObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true))
                            {
                                foreach (var m in c.renderer.materials)
                                {
                                    m.shader = Shader.Find("Transparent/Diffuse");
                                    m.SetColor("_Color", new Color(1, 1, 1, SetPicture[RoleIndex].RolePictureTransparent));
                                }

                            }
                            foreach (Component c in SetPicture[RoleIndex].RoleObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                            {
                                foreach (var m in c.renderer.materials)
                                {
                                    if (c.name != "Shadow" && c.name != "Dao_glow")
                                    {
                                        m.shader = Shader.Find("Transparent/Diffuse");
                                        m.SetColor("_Color", new Color(1, 1, 1, SetPicture[RoleIndex].RolePictureTransparent));
                                    }
                                }
                            }
                        }
                    }
                    //////////////////////////人物死亡(以上)//////////////////////
                    #endregion
                    //////////////////////////////////////判定攻击(以下)//////////////////////////////////////
                    else if (SetPicture[RoleIndex].RoleNowBlood > 0 && IsFight)
                    {
                        if (ListSequence[0].RoleIndex == RoleIndex && ListSequence[0].IsMonster == SetPicture[RoleIndex].RolePictureMonster)
                        {
                            if (!IsLock && !IsEnemyLock)
                            {
                                if (!SetPicture[RoleIndex].BuffStop)
                                {
                                    if (!SetPicture[RoleIndex].RolePictureStartAttack)
                                    {
                                        Destroy(GameObject.Find("WalkObject"));
                                        int GoPosition = 0;
                                        if (SetPicture[RoleIndex].RoleSkillPoint < 1000 || SetPicture[RoleIndex].BuffSilence || (FightStyle == 2 && IsSkill == false && !SetPicture[RoleIndex].RolePictureMonster)) //普攻
                                        {
                                            List<int> TargetIndex = new List<int>();
                                            //if (IsAuto || SetPicture[RoleIndex].RolePictureMonster)
                                            {
                                                TargetIndex = FindTarget(SetPicture[RoleIndex].RoleTargetIndex, SetPicture, SetEnemyPicture, 0, RoleIndex, false);
                                            }
                                            //else if (SetPicture[RoleIndex].RoleTargetIndex > -1)
                                            //{
                                            //    TargetIndex = FindTarget(SetPicture[RoleIndex].RoleTargetIndex, SetPicture, SetEnemyPicture, RoleIndex, 0);
                                            //}
                                            if (TargetIndex.Count > 0 && SetPicture[RoleIndex].RoleMovePosition == 0)
                                            {
                                                FightTimer = 0.01f;
                                                if (SetPicture[RoleIndex].RoleTargetObject.activeSelf)
                                                {
                                                    SetPicture[RoleIndex].RoleTargetIndex = -1;
                                                    SetPicture[RoleIndex].RoleTargetObject.SetActive(false);
                                                }
                                                SetPicture[RoleIndex].RolePictureStartAttack = true;
                                                SetPicture[RoleIndex].RoleFightIndex.Clear();
                                                SetPicture[RoleIndex].RoleFightType.Clear();
                                                SetPicture[RoleIndex].RoleFightDamige.Clear();

                                                int Damige = 0;
                                                if (SetPicture[RoleIndex].RoleArea == 1022)
                                                {
                                                    Damige = -(int)(SetPicture[RoleIndex].RolePAttack * (1 + SetPicture[RoleIndex].BuffAttack));

                                                    SetPicture[RoleIndex].RoleFightIndex.Add(TargetIndex[0]);
                                                    SetPicture[RoleIndex].RoleFightType.Add(0);
                                                    SetPicture[RoleIndex].RoleFightDamige.Add((int)(Damige));

                                                    if (TargetIndex[0] == -1)
                                                    {
                                                        AddSequence();
                                                        continue;
                                                    }
                                                    ////////////////////////决定顺序////////////////////////
#if ClashRoyale
                                                    ChangeDirection(SetPicture, SetData, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, RoleIndex);
#else
                                                    ChangeDirection(SetPicture, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, RoleIndex);
#endif
                                                    ////////////////////////决定顺序////////////////////////

                                                    if (SetPicture[RoleIndex].RoleSkill2 == 2005) //背动加怒
                                                    {
                                                        SetPicture[RoleIndex].RoleSkillPoint += 30;
                                                        SetPicture[TargetIndex[0]].RoleSkillPoint += 30;
                                                    }
                                                    else if (SetPicture[RoleIndex].RoleSkill2 == 2038) //背动加怒
                                                    {
                                                        SetPicture[RoleIndex].RoleSkillPoint += 70;
                                                        SetPicture[TargetIndex[0]].RoleSkillPoint += 70;
                                                    }
                                                    else if (SetPicture[RoleIndex].RoleSkill2 == 2050) //背动加怒
                                                    {
                                                        SetPicture[RoleIndex].RoleSkillPoint += 50;
                                                        SetPicture[TargetIndex[0]].RoleSkillPoint += 50;
                                                    }
                                                }
                                                else
                                                {
                                                    Damige = GetDamige(SetPicture, SetEnemyPicture, RoleIndex, TargetIndex);

                                                    SetPicture[RoleIndex].RoleFightIndex.Add(TargetIndex[0]);
                                                    SetPicture[RoleIndex].RoleFightType.Add(0);
                                                    SetPicture[RoleIndex].RoleFightDamige.Add((int)(Damige));

                                                    ////////////////////////决定顺序////////////////////////
#if ClashRoyale
                                                    ChangeDirection(SetPicture, SetData, SetPicture[RoleIndex].RoleObject.transform.position, SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, RoleIndex);
#else
                                                    ChangeDirection(SetPicture, SetPicture[RoleIndex].RoleObject.transform.position, SetEnemyPicture[SetPicture[RoleIndex].RoleFightIndex[0]].RoleObject.transform.position, RoleIndex);
#endif
                                                    ////////////////////////决定顺序////////////////////////
                                                }


                                                if (!IsSkip)
                                                {
                                                    if (!SetPicture[RoleIndex].IsPicture)
                                                    {
                                                        ActObject.transform.position = SetPicture[RoleIndex].RoleObject.transform.position;
                                                        ActObject.SetActive(true);
                                                        SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 2);
                                                        SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().Play("attack");
                                                        if (SetPicture[RoleIndex].RoleID == 65901)
                                                        {
                                                            SetPicture[RoleIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("attack");
                                                        }
                                                    }
                                                }

                                                if (!SetPicture[RoleIndex].RoleTargetObject.activeSelf)
                                                {
                                                    SetPicture[RoleIndex].RoleTargetIndex = -1;
                                                }

                                                //Debug.LogError(SetPicture[RoleIndex].RoleID);
                                                FightMotion fm = TextTranslator.instance.fightMotionDic[((SetPicture[RoleIndex].RoleID > 65000 && SetPicture[RoleIndex].RoleID < 65300) ? SetPicture[RoleIndex].RoleID - 5000 : SetPicture[RoleIndex].RoleID) * 10 + 1];
                                                SetEffect(SetPicture, SetEnemyPicture, RoleIndex, fm, TargetIndex, false);
                                                //SetPicture[RoleIndex].RoleSkillPoint += 2;
                                                SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[RoleIndex].RoleSkillPoint);
                                            }
                                            else if (SetPicture[RoleIndex].RoleMovePosition != 0 && SetPicture[RoleIndex].RoleMoveNowStep > 0)
                                            {
                                                SetPicture[RoleIndex].RoleMoveNowStep--;
                                                int RoleMovePosition = SetPicture[RoleIndex].RoleMovePosition;
                                                GoPosition = FindGoPosition(SetPicture, SetEnemyPicture, RoleIndex, SetPicture[RoleIndex].RoleMovePosition);
                                                SetPicture[RoleIndex].RoleMovePosition = RoleMovePosition;

#if ClashRoyale
                                                SetPictureForwardPosition(SetPicture, SetData, SetPicture[RoleIndex].RoleMovePosition, RoleIndex);
#else
                                                SetPictureForwardPosition(SetPicture, GoPosition, RoleIndex);
#endif
                                            }
                                            else
                                            {
                                                FightTimer = 0.01f;
                                                float LeastDistance = 999;
                                                float LeastBlood = 1;
                                                int LeastIndex = -1;
                                                HealIndex = -1;

                                                if ((SetPicture[RoleIndex].RoleArea == 1022) && (FightStyle != 2 || (FightStyle == 2 && !IsHand) || SetPicture[RoleIndex].RolePictureMonster))
                                                {
                                                    for (int MonsterIndex = 0; MonsterIndex < SetPicture.Count; MonsterIndex++)
                                                    {
                                                        if (SetPicture[MonsterIndex].RolePictureAttackable && SetPicture[MonsterIndex].RoleNowBlood > 0)
                                                        {
                                                            if (LeastBlood > SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood)
                                                            {
                                                                LeastBlood = SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood;
                                                                LeastIndex = MonsterIndex;
                                                                HealIndex = MonsterIndex;
                                                                GoPosition = SetPicture[MonsterIndex].RolePosition;
                                                            }
                                                        }
                                                    }
                                                    GoPosition = FindGoPosition(SetPicture, SetEnemyPicture, RoleIndex, GoPosition);
                                                }
                                                else
                                                {
                                                    GoPosition = FindGoPosition(SetPicture, SetEnemyPicture, RoleIndex, SetPicture[RoleIndex].RoleMovePosition);
                                                    //Debug.LogError(SetPicture[RoleIndex].RoleObject.name + " " + GoPosition + " " + ListCloseNode.Count);
                                                    if (GoPosition == 0)
                                                    {
                                                        for (int MonsterIndex = 0; MonsterIndex < SetEnemyPicture.Count; MonsterIndex++)
                                                        {
                                                            if (SetEnemyPicture[MonsterIndex].RolePictureAttackable)
                                                            {
                                                                float MonsterDistance = Vector2.Distance(new Vector2(ListPosition[SetEnemyPicture[MonsterIndex].RolePosition].x, ListPosition[SetEnemyPicture[MonsterIndex].RolePosition].z), new Vector2(ListPosition[SetPicture[RoleIndex].RolePosition].x, ListPosition[SetPicture[RoleIndex].RolePosition].z));
                                                                if (MonsterDistance < LeastDistance && SetEnemyPicture[MonsterIndex].RoleObject.activeSelf)
                                                                {
                                                                    LeastDistance = MonsterDistance;
                                                                    LeastIndex = MonsterIndex;
                                                                    GoPosition = SetEnemyPicture[MonsterIndex].RolePosition;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (SetPicture[RoleIndex].RoleMoveNowStep > 0 && (FightStyle != 2 || SetPicture[RoleIndex].RolePictureMonster || !IsHand))
                                                {
                                                    if (IsAuto || SetPicture[RoleIndex].RolePictureMonster)
                                                    {
                                                        SetPicture[RoleIndex].RoleMoveNowStep--;
#if ClashRoyale
                                                        SetPictureForwardPosition(SetPicture, SetData, GoPosition, RoleIndex);
#else
                                                        SetPictureForwardPosition(SetPicture, GoPosition, RoleIndex);
#endif
                                                    }
                                                }
                                                else
                                                {
                                                    ////////////////////////决定顺序////////////////////////
#if ClashRoyale
                                                    ChangeDirection(SetPicture, SetData, SetPicture[RoleIndex].RoleObject.transform.position, (SetPicture[RoleIndex].RolePictureMonster ? SetPicture[RoleIndex].RoleObject.transform.position - new Vector3(1, 0, 0) : SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(1, 0, 0)), RoleIndex);
#else
                                                    ChangeDirection(SetPicture, SetPicture[RoleIndex].RoleObject.transform.position, (SetPicture[RoleIndex].RolePictureMonster ? SetPicture[RoleIndex].RoleObject.transform.position - new Vector3(1, 0, 0) : SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(1, 0, 0)), RoleIndex);
#endif

                                                    ////////////////////////决定顺序////////////////////////
                                                    SetPicture[RoleIndex].RoleMovePosition = 0;
                                                    if (SetPicture[RoleIndex].RoleTargetObject.activeSelf)
                                                    {
                                                        SetPicture[RoleIndex].RoleTargetIndex = -1;
                                                        SetPicture[RoleIndex].RoleTargetObject.SetActive(false);
                                                    }

                                                    if (IsAuto || SetPicture[RoleIndex].RolePictureMonster)
                                                    {
                                                        SetPicture[RoleIndex].RoleSkillPoint += 300; //什么事都没做
                                                        SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[RoleIndex].RoleSkillPoint);
                                                        AddSequence();
                                                    }
                                                }
                                            }
                                        }
                                        else //Skill
                                        {
                                            Skill skill = TextTranslator.instance.GetSkillByID(SetPicture[RoleIndex].RoleSkill1, 1);

                                            List<int> TargetIndex = new List<int>();
                                            if (IsAuto || SetPicture[RoleIndex].RolePictureMonster)
                                            {
                                                TargetIndex = FindTarget(SetPicture[RoleIndex].RoleTargetIndex, SetPicture, SetEnemyPicture, skill.area, RoleIndex, false);
                                            }
                                            else if (SetPicture[RoleIndex].RoleTargetIndex > -1)
                                            {
                                                TargetIndex = FindTarget(SetPicture[RoleIndex].RoleTargetIndex, SetPicture, SetEnemyPicture, skill.area, RoleIndex, false);
                                            }

                                            if (TargetIndex.Count > 0 && SetPicture[RoleIndex].RoleMovePosition == 0)
                                            {
                                                SetPicture[RoleIndex].RoleShowSkill = false;
                                                //GameObject.Find("Base" + SetPicture[RoleIndex].RolePosition).transform.Find("SkillObject").gameObject.SetActive(false);
                                                SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteSkill.SetActive(false);
#if ClashRoyale
                                                if (ExecuteSkill(SetPicture, SetData, SetEnemyPicture, RoleIndex))
#else
                                                if (ExecuteSkill(SetPicture, SetEnemyPicture, RoleIndex))
#endif
                                                {
                                                    FightTimer = 0.01f;
                                                    if (SetPicture[RoleIndex].RoleTargetObject.activeSelf)
                                                    {
                                                        SetPicture[RoleIndex].RoleTargetIndex = -1;
                                                        SetPicture[RoleIndex].RoleTargetObject.SetActive(false);
                                                    }
                                                    SetPicture[RoleIndex].RolePictureStartAttack = true;
                                                    if (!SetPicture[RoleIndex].IsPicture)
                                                    {
                                                        SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 2);
                                                        SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().Play("idle");
                                                        if (SetPicture[RoleIndex].RoleID == 65901)
                                                        {
                                                            SetPicture[RoleIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("idle");
                                                        }
                                                    }
#if ClashRoyale
                                                    else
                                                    {
                                                        PlayAnimation(SetPicture, SetData, RoleIndex, 0.1f, "Attack");
                                                    }
#endif
                                                    if (!SetPicture[RoleIndex].RoleTargetObject.activeSelf)
                                                    {
                                                        SetPicture[RoleIndex].RoleTargetIndex = -1;
                                                    }
                                                    SetPicture[RoleIndex].RoleSkillPoint = 0;
                                                    SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[RoleIndex].RoleSkillPoint);
                                                }
                                            }
                                            else if (SetPicture[RoleIndex].RoleMovePosition != 0 && SetPicture[RoleIndex].RoleMoveNowStep > 0)
                                            {
                                                SetPicture[RoleIndex].RoleMoveNowStep--;
                                                int RoleMovePosition = SetPicture[RoleIndex].RoleMovePosition;
                                                GoPosition = FindGoPosition(SetPicture, SetEnemyPicture, RoleIndex, SetPicture[RoleIndex].RoleMovePosition);
                                                SetPicture[RoleIndex].RoleMovePosition = RoleMovePosition;
#if ClashRoyale
                                                SetPictureForwardPosition(SetPicture, SetData, SetPicture[RoleIndex].RoleMovePosition, RoleIndex);
#else
                                                SetPictureForwardPosition(SetPicture, GoPosition, RoleIndex);
#endif
                                            }
                                            else
                                            {
                                                FightTimer = 0.01f;
                                                float LeastDistance = 999;
                                                int LeastIndex = -1;

                                                GoPosition = FindGoPosition(SetPicture, SetEnemyPicture, RoleIndex, SetPicture[RoleIndex].RoleMovePosition);

                                                //Debug.LogError(SetPicture[RoleIndex].RoleObject.name + " " + GoPosition + " " + ListCloseNode.Count);
                                                if (GoPosition == 0)
                                                {
                                                    for (int MonsterIndex = 0; MonsterIndex < SetEnemyPicture.Count; MonsterIndex++)
                                                    {
                                                        if (SetEnemyPicture[MonsterIndex].RolePictureAttackable)
                                                        {
                                                            float MonsterDistance = Vector2.Distance(new Vector2(ListPosition[SetEnemyPicture[MonsterIndex].RolePosition].x, ListPosition[SetEnemyPicture[MonsterIndex].RolePosition].z), new Vector2(ListPosition[SetPicture[RoleIndex].RolePosition].x, ListPosition[SetPicture[RoleIndex].RolePosition].z));
                                                            if (MonsterDistance < LeastDistance && SetEnemyPicture[MonsterIndex].RoleObject.activeSelf)
                                                            {
                                                                LeastDistance = MonsterDistance;
                                                                LeastIndex = MonsterIndex;
                                                                GoPosition = SetEnemyPicture[LeastIndex].RolePosition;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (SetPicture[RoleIndex].RoleMoveNowStep > 0 && (FightStyle != 2 || SetPicture[RoleIndex].RolePictureMonster || !IsHand))
                                                {
                                                    if (IsAuto || SetPicture[RoleIndex].RolePictureMonster)
                                                    {
                                                        SetPicture[RoleIndex].RoleMoveNowStep--;
#if ClashRoyale
                                                        SetPictureForwardPosition(SetPicture, SetData, SetEnemyPicture[LeastIndex].RolePosition, RoleIndex);
#else
                                                        SetPictureForwardPosition(SetPicture, GoPosition, RoleIndex);
#endif
                                                    }
                                                }
                                                else
                                                {
                                                    ////////////////////////决定顺序////////////////////////
#if ClashRoyale
                                                    ChangeDirection(SetPicture, SetData, SetPicture[RoleIndex].RoleObject.transform.position, (SetPicture[RoleIndex].RolePictureMonster ? SetPicture[RoleIndex].RoleObject.transform.position - new Vector3(1, 0, 0) : SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(1, 0, 0)), RoleIndex);
#else
                                                    ChangeDirection(SetPicture, SetPicture[RoleIndex].RoleObject.transform.position, (SetPicture[RoleIndex].RolePictureMonster ? SetPicture[RoleIndex].RoleObject.transform.position - new Vector3(1, 0, 0) : SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(1, 0, 0)), RoleIndex);
#endif
                                                    ////////////////////////决定顺序////////////////////////
                                                    SetPicture[RoleIndex].RoleMovePosition = 0;
                                                    if (SetPicture[RoleIndex].RoleTargetObject.activeSelf)
                                                    {
                                                        SetPicture[RoleIndex].RoleTargetIndex = -1;
                                                        SetPicture[RoleIndex].RoleTargetObject.SetActive(false);
                                                    }
                                                    if (IsAuto || SetPicture[RoleIndex].RolePictureMonster)
                                                    {
                                                        AddSequence();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    AddSequence();
                                }
                            }
                        }
                    }
                    //////////////////////////////////////判定攻击(以下)//////////////////////////////////////
                }
                //////////////////////判断人物静止(以上)//////////////////////
            }
            else
            {
                if (ListSequence.Count > 0 && IsFight)
                {
                    if (ListSequence[0].RoleIndex == RoleIndex && ListSequence[0].IsMonster == SetPicture[RoleIndex].RolePictureMonster)
                    {
                        AddSequence();
                    }
                }
            }
        }
    }
    #endregion

    #region Skill


    void SetSkillBuff(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SetRoleIndex, Skill skill, bool IsBegin)
    {
        if (IsBegin)
        {
            if (skill.BuffID > 800 && skill.BuffID < 900) //光环作用我方
            {
                if (skill.BuffID > 850) //光环作用全体
                {
                    Buff NewBuff = TextTranslator.instance.GetBuffByID(skill.BuffID);
                    //if (NewBuff != null)
                    {
                        for (int i = 0; i < SetPicture.Count; i++)
                        {
                            RoleAddBuff(SetPicture, i, NewBuff);
                        }
                    }
                }
                else
                {
                    Buff NewBuff = TextTranslator.instance.GetBuffByID(skill.BuffID);
                    //if (NewBuff != null)
                    {
                        RoleAddBuff(SetPicture, SetRoleIndex, NewBuff);
                    }
                }
            }

            switch (skill.parameter1)
            {
                case 508: //姓别克星
                    for (int i = 0; i < SetEnemyPicture.Count; i++)
                    {
                        if (SetEnemyPicture[i].RoleSex == skill.skillVal1 && SetEnemyPicture[i].RolePosition > 0)
                        {
                            Buff NewBuff = TextTranslator.instance.GetBuffByID(skill.skillDuration1);
                            //if (NewBuff != null)
                            {
                                RoleAddBuff(SetEnemyPicture, i, NewBuff);
                            }
                        }
                    }
                    break;
                case 517: //开场满怒
                    for (int i = 0; i < SetEnemyPicture.Count; i++)
                    {
                        if (SetEnemyPicture[i].RoleID == skill.skillDuration1 && SetEnemyPicture[i].RolePosition > 0)
                        {
                            Buff NewBuff = TextTranslator.instance.GetBuffByID(skill.skillVal1);
                            //if (NewBuff != null)
                            {
                                RoleAddBuff(SetPicture, SetRoleIndex, NewBuff);
                            }
                        }
                    }
                    break;
                case 518: //开场满怒
                    for (int i = 0; i < SetEnemyPicture.Count; i++)
                    {
                        if ((SetEnemyPicture[i].RoleID == skill.skillDuration1
                            || SetEnemyPicture[i].RoleID == skill.parameter2
                            || SetEnemyPicture[i].RoleID == skill.skillVal2
                            || SetEnemyPicture[i].RoleID == skill.skillDuration2
                            || SetEnemyPicture[i].RoleID == skill.parameter3) && SetEnemyPicture[i].RolePosition > 0)
                        {
                            Buff NewBuff = TextTranslator.instance.GetBuffByID(skill.skillVal1);
                            //if (NewBuff != null)
                            {
                                RoleAddBuff(SetPicture, SetRoleIndex, NewBuff);
                            }
                            break;
                        }
                    }
                    break;
                case 519: //嘲风
                    Buff NewBuff1 = TextTranslator.instance.GetBuffByID(skill.skillDuration1);
                    NewBuff1.parameter1 = SetRoleIndex;
                    //if (NewBuff != null)
                    {
                        RoleAddBuff(SetPicture, SetRoleIndex, NewBuff1);
                    }
                    break;
            }


            Skill PostiveSkill = TextTranslator.instance.GetSkillByID(skill.skillID, 1);
            int PostiveBuffID = PostiveSkill.BuffID;
            Buff NewPostiveBuff = null;
            switch (skill.skillID) //技能特别效果
            {
                case 2039:  //阴阳加治了
                    NewPostiveBuff = TextTranslator.instance.GetBuffByID(PostiveSkill.skillDuration1);
                    if (NewPostiveBuff != null)
                    {
                        RoleAddBuff(SetPicture, SetRoleIndex, NewPostiveBuff);
                    }
                    break;
            }
        }
        else
        {
            if (skill.BuffID > 900) //作用我方
            {
                if (skill.BuffID > 950) //作用全体
                {
                    Buff NewBuff = TextTranslator.instance.GetBuffByID(skill.BuffID);
                    //if (NewBuff != null)
                    {
                        for (int i = 0; i < SetPicture.Count; i++)
                        {
                            RoleAddBuff(SetPicture, i, NewBuff);
                        }
                    }
                }
                else
                {
                    Buff NewBuff = TextTranslator.instance.GetBuffByID(skill.BuffID);
                    if (NewBuff != null)
                    {
                        RoleAddBuff(SetPicture, SetRoleIndex, NewBuff);
                    }
                }
            }



            Skill PostiveSkill = TextTranslator.instance.GetSkillByID(skill.skillID, 1);
            int PostiveBuffID = PostiveSkill.BuffID;
            Buff NewPostiveBuff = null;
            switch (skill.skillID) //技能特别效果
            {
                case 2042:  //雷电攻防一体
                    NewPostiveBuff = TextTranslator.instance.GetBuffByID(PostiveSkill.skillDuration1);
                    if (NewPostiveBuff != null)
                    {
                        RoleAddBuff(SetPicture, SetRoleIndex, NewPostiveBuff);
                    }
                    NewPostiveBuff = TextTranslator.instance.GetBuffByID(PostiveSkill.parameter1);
                    if (NewPostiveBuff != null)
                    {
                        RoleAddBuff(SetPicture, SetRoleIndex, NewPostiveBuff);
                    }
                    break;
            }
        }




    }

#if ClashRoyale
    public bool ExecuteSkill(List<RolePicture> SetPicture, List<PictureRoleData> SetData, List<RolePicture> SetEnemyPicture, int SetRoleIndex)
#else
    public bool ExecuteSkill(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SetRoleIndex)
#endif
    {
        SetListBuff.Clear();
        //Debug.LogError(SetPicture[SetRoleIndex].RoleSkill1 + " " + SetPicture[SetRoleIndex].RoleSkillLevel1);
        Skill skill = TextTranslator.instance.GetSkillByID(SetPicture[SetRoleIndex].RoleSkill1, SetPicture[SetRoleIndex].RoleSkillLevel1);
        Skill postiveSkill = TextTranslator.instance.GetSkillByID(SetPicture[SetRoleIndex].RoleSkill2, SetPicture[SetRoleIndex].RoleSkillLevel2);
        //Debug.LogError(skill.skillID + " " + skill.area);
        List<int> TargetIndex = FindTarget(SetPicture[SetRoleIndex].RoleTargetIndex, SetPicture, SetEnemyPicture, skill.area, SetRoleIndex, false);
        //Debug.LogError("Skill Area" + skill.area.ToString() + " Target Index" + TargetIndex.Count + " " + TargetIndex[0]);
        if (TargetIndex.Count > 0)
        {
            //Debug.LogError("Skill Area" + skill.area.ToString() + " Target Index" + TargetIndex.Count + " " + TargetIndex[0]);
            StartCoroutine(fw.ShowSkillEffect(SetPicture[SetRoleIndex].RoleID, skill.skillID, SetRoleIndex, SetPicture[SetRoleIndex].RolePictureMonster));
            SkillDamige(SetPicture, SetEnemyPicture, SetRoleIndex, skill, TargetIndex);
            SetPicture[SetRoleIndex].RoleRedBloodObject.SetActive(true);
            SetPicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(SetPicture[SetRoleIndex].RoleSkill1);

            ////////////////////设定我方buff///////////////////////
            if (postiveSkill != null)
            {
                SetSkillBuff(SetPicture, SetEnemyPicture, SetRoleIndex, postiveSkill, false);
            }
            SetSkillBuff(SetPicture, SetEnemyPicture, SetRoleIndex, skill, false);
            ////////////////////设定我方buff///////////////////////

            if (SetPicture[SetRoleIndex].RoleFightDamige[0] > 0)
            {
                ////////////////////////决定顺序////////////////////////
#if ClashRoyale
                ChangeDirection(SetPicture, SetData, SetPicture[SetRoleIndex].RoleObject.transform.position, SetEnemyPicture[SetPicture[SetRoleIndex].RoleFightIndex[0]].RoleObject.transform.position, SetRoleIndex);
#else
                ChangeDirection(SetPicture, SetPicture[SetRoleIndex].RoleObject.transform.position, SetEnemyPicture[SetPicture[SetRoleIndex].RoleFightIndex[0]].RoleObject.transform.position, SetRoleIndex);
#endif
                ////////////////////////决定顺序////////////////////////
            }

            StartCoroutine(DelayExecuteSkill(SetPicture, SetEnemyPicture, SetRoleIndex, TargetIndex, IsCombineSkill));



            ////////////////////嘲讽//////////////////
            //if (SetPicture[SetRoleIndex].RoleSkill2 == 2007)
            //{
            //    for (int i = 0; i < SetPicture.Count; i++)
            //    {
            //        SetPicture[i].RolePictureAttackable = false;
            //    }
            //    SetPicture[SetRoleIndex].RolePictureAttackable = true;

            //    TextTranslator.TerrainInfo ti = new TextTranslator.TerrainInfo();
            //    ti.buff = 9;
            //    ti.count = 1;
            //    SetRoleBuff(SetPicture, SetRoleIndex, ti);
            //    SetPicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuff(9012);
            //}
            ////////////////////嘲讽//////////////////

            ////////////////////集火//////////////////
            //if (SetPicture[SetRoleIndex].RoleSkill2 == 2004)
            //{
            //    for (int i = 0; i < SetEnemyPicture.Count; i++)
            //    {
            //        SetEnemyPicture[i].RolePictureAttackable = false;
            //    }
            //    SetEnemyPicture[SetPicture[SetRoleIndex].RoleFightIndex[0]].RolePictureAttackable = true;

            //    TextTranslator.TerrainInfo ti = new TextTranslator.TerrainInfo();
            //    ti.buff = 9;
            //    ti.count = 2;
            //    SetRoleBuff(SetEnemyPicture, SetPicture[SetRoleIndex].RoleFightIndex[0], ti);
            //    SetEnemyPicture[SetPicture[SetRoleIndex].RoleFightIndex[0]].RoleObject.GetComponent<ColliderDisplayText>().SetBuff(9012);
            //}
            ////////////////////集火//////////////////
            return true;
        }
        return false;
    }

    IEnumerator DelayExecuteSkill(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SetRoleIndex, List<int> TargetIndex, bool isDelay)
    {
        if (!IsSkip)
        {
            ActObject.transform.position = SetPicture[SetRoleIndex].RoleObject.transform.position;
            ActObject.SetActive(true);
            if (isDelay)
            {
                yield return new WaitForSeconds(2f);
            }

            yield return new WaitForSeconds(0.1f);

            SetPicture[SetRoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 2);
            SetPicture[SetRoleIndex].RolePictureObject.GetComponent<Animator>().Play("skill");
            if (SetPicture[SetRoleIndex].RoleID == 65901)
            {
                SetPicture[SetRoleIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("skill");
            }
        }

        FightMotion fm = TextTranslator.instance.fightMotionDic[((SetPicture[SetRoleIndex].RoleID > 65000 && SetPicture[SetRoleIndex].RoleID < 65300) ? SetPicture[SetRoleIndex].RoleID - 5000 : SetPicture[SetRoleIndex].RoleID) * 10 + 2];
        SetEffect(SetPicture, SetEnemyPicture, SetRoleIndex, fm, TargetIndex, true);

        if (NetworkHandler.instance.IsCreate)
        {
            if (SetPicture[SetRoleIndex].RoleID == 65304)
            {
                StartCoroutine(ShowFire(SetPicture, SetRoleIndex, fm));
            }
        }

        if (!SetPicture[SetRoleIndex].IsPicture)
        {
            if (SetPicture[SetRoleIndex].RoleSkill1 == 1022)
            {
                SetPicture[SetRoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("box", 0);
            }
            else if (SetPicture[SetRoleIndex].RoleSkill1 == 1027)
            {
                SetPicture[SetRoleIndex].RolePictureObject.transform.Find("HeroBody").gameObject.renderer.material = (Material)Resources.Load("Materials/Ada1");
                SetPicture[SetRoleIndex].RolePictureObject.transform.Find("Object001").gameObject.renderer.material = (Material)Resources.Load("Materials/Ada1");
            }
        }
        yield return 0;
    }

    IEnumerator ShowFire(List<RolePicture> SetPicture, int RoleIndex, FightMotion fm)
    {
        yield return new WaitForSeconds(1.2f);
        AudioEditer.instance.PlayOneShot("B_captain");
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefab/Effect/ZhiHuiGuan_BOSS_skill_02"));
        yield return new WaitForSeconds(1.2f);
        AudioEditer.instance.PlayOneShot("B_captain");
        GameObject go1 = (GameObject)Instantiate(Resources.Load("Prefab/Effect/ZhiHuiGuan_BOSS_skill_02"));
        go.transform.Rotate(new Vector3(0, 45, 0));
    }
    IEnumerator ShowWesker(List<RolePicture> SetPicture, int RoleIndex)
    {
        //while (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) != 12 || PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) != 11)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //}
        SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().Play("skill");
        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/Wesker_skill", typeof(GameObject)), SetPicture[RoleIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(1f);
        Destroy(SetPicture[RoleIndex].RolePictureObject);
        AudioEditer.instance.PlayOneShot("S_wsg");
        SetPicture[RoleIndex].RolePictureObject = GameObject.Instantiate(Resources.Load("Prefab/Role/65901", typeof(GameObject))) as GameObject;
        SetPicture[RoleIndex].RolePictureObject.transform.parent = SetPicture[RoleIndex].RoleObject.transform;
        SetPicture[RoleIndex].RolePictureObject.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        SetPicture[RoleIndex].RolePictureObject.transform.localPosition = Vector3.zero;
        SetPicture[RoleIndex].RolePictureObject.transform.Rotate(0, -90, 0);
        SetPicture[RoleIndex].RolePictureObject.name = "Role";
        IsLock = true;
        fw.SkillMask1.SetActive(false);
        fw.SkillCD1 = 0;
        //yield return new WaitForSeconds(0.1f);
        //PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 11);
        //LuaDeliver.instance.UseGuideStation();
    }
    IEnumerator SharkCamera(int DelayTimer, int SharkTime, int SharkType, int SharkScope)
    {
        yield return new WaitForSeconds(DelayTimer / 100f);
        while (SharkTime > 0)
        {
            SharkTime--;
            if (SharkType == 2)
            {
                if (SharkTime % 2 == 1)
                {
                    MyCamera.transform.position += new Vector3(0, SharkScope / 100f, 0);
                }
                else
                {
                    MyCamera.transform.position -= new Vector3(0, SharkScope / 100f, 0);
                }
            }
            else if (SharkType == 1)
            {
                if (SharkTime % 2 == 1)
                {
                    MyCamera.transform.position += new Vector3(SharkScope / 100f, 0, 0);
                }
                else
                {
                    MyCamera.transform.position -= new Vector3(SharkScope / 100f, 0, 0);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return 0;
    }

    IEnumerator SharkCamera()
    {
        MyCamera.transform.position -= new Vector3(0, 0.3f, 0);
        yield return new WaitForSeconds(0.01f);
        MyCamera.transform.position += new Vector3(0, 0.3f, 0);
        yield return new WaitForSeconds(0.01f);


        //MyCamera.transform.position += new Vector3(0.3f, 0, 0);
        //yield return new WaitForSeconds(0.01f);
        //MyCamera.transform.position -= new Vector3(0.3f, 0, 0);
        //yield return new WaitForSeconds(0.01f);
        //MyCamera.transform.position += new Vector3(0.2f, 0, 0);
        //yield return new WaitForSeconds(0.01f);
        //MyCamera.transform.position -= new Vector3(0.2f, 0, 0);
        //yield return new WaitForSeconds(0.01f);
    }
    int GetDamige(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, List<int> TargetIndex)
    {
        ////////////////////设定我方buff///////////////////////
        Skill PostiveSkill = TextTranslator.instance.GetSkillByID(SetPicture[RoleIndex].RoleSkill2, SetPicture[RoleIndex].RoleSkillLevel2);
        if (PostiveSkill != null)
        {
            SetSkillBuff(SetPicture, SetEnemyPicture, RoleIndex, PostiveSkill, false);
        }
        ////////////////////设定我方buff///////////////////////

        int Damige = 1;
        Damige = (int)(SetPicture[RoleIndex].RolePAttack * (1 + SetPicture[RoleIndex].BuffAttack) - SetEnemyPicture[TargetIndex[0]].RolePDefend - SetEnemyPicture[TargetIndex[0]].BuffDefend);

        if (!SetPicture[RoleIndex].RolePictureMonster)
        {
            //Debug.LogError("FightCalculate" + FightCalculate);
            Damige = (int)(Damige * FightCalculate);
        }
        else
        {
            //Debug.LogError("EnemyCalculate" + EnemyCalculate);
            Damige = (int)(Damige * EnemyCalculate);
        }

        if (Damige < (int)(SetEnemyPicture[TargetIndex[0]].RolePDefend * 2))
        {
            Damige = (int)(SetPicture[RoleIndex].RolePAttack);
        }

        //加伤   2普攻伤害提升  4达到血量加伤
        Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(2, SetPicture[RoleIndex].RoleInnate[1]);
        if (SetInnate != null)
        {
            Damige *= (int)(SetInnate.Value1 / 100f);
        }


        if (SetPicture[RoleIndex].RoleID == 60033) //天使普攻回血
        {
            List<int> RoleFightIndex = new List<int>();
            List<int> RoleFightCate = new List<int>();
            List<int> RoleFightDamige = new List<int>();
            float LeastBlood = 1;
            int LeastIndex = RoleIndex;

            for (int MonsterIndex = 0; MonsterIndex < SetPicture.Count; MonsterIndex++)
            {
                if (SetPicture[MonsterIndex].RolePictureAttackable && SetPicture[MonsterIndex].RoleNowBlood > 0 && SetPicture[MonsterIndex].RolePosition > 0)
                {
                    if (LeastBlood > SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood)
                    {
                        LeastBlood = SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood;
                        LeastIndex = MonsterIndex;
                    }
                }
            }

            RoleFightIndex.Add(LeastIndex);
            RoleFightCate.Add(0);
            RoleFightDamige.Add((int)(-Damige));
            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 1.5f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[RoleIndex].RolePictureMonster, "", null);
        }
        return Damige;
    }
    public bool SkillDamige(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, Skill SkillID, List<int> TargetIndex)
    {
        bool IsSuccess = true;
        SetPicture[RoleIndex].RoleFightIndex.Clear();
        SetPicture[RoleIndex].RoleFightType.Clear();
        SetPicture[RoleIndex].RoleFightDamige.Clear();
        switch (SkillID.parameter1)
        {
            case 17:
                SkillAttribute(SetPicture, SetEnemyPicture, RoleIndex, SkillID, TargetIndex, SkillID.parameter1, -1);
                break;
            case 10:
                SkillAttribute(SetPicture, SetEnemyPicture, RoleIndex, SkillID, TargetIndex, 0, 0);
                break;
            default:
                SkillAttribute(SetPicture, SetEnemyPicture, RoleIndex, SkillID, TargetIndex, 0, 1);
                break;
        }



        return IsSuccess;
    }
    public bool SkillAttribute(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, Skill SkillID, List<int> TargetIndex, int FightDamige, int IsDamige)
    {
        bool IsSuccess = true;

        if (SkillID.parameter2 == 23) //对目标及其周围敌人造成伤害
        {
            for (int j = 0; j < TargetIndex.Count; j++)
            {
                int i = TargetIndex[j];
                if (IsDamige > 0)
                {
                    Debug.Log(i);
                    if (j == 0)
                    {
                        FightDamige = (int)(((SetPicture[RoleIndex].RolePAttack / SkillID.weather * (1 + SetPicture[RoleIndex].BuffAttack)) - SetEnemyPicture[i].RolePDefend / SkillID.weather - SetEnemyPicture[i].BuffDefend / SkillID.weather) * SkillID.skillVal1 / 100f);
                    }
                    else
                    {
                        FightDamige = (int)(((SetPicture[RoleIndex].RolePAttack / SkillID.weather * (1 + SetPicture[RoleIndex].BuffAttack)) - SetEnemyPicture[i].RolePDefend / SkillID.weather - SetEnemyPicture[i].BuffDefend / SkillID.weather) * SkillID.skillVal2 / 100f);
                    }
                    //Debug.LogError(FightDamige + "BBBBBBBBBBBBBBBBBBBBBB" + SetPicture[RoleIndex].RolePAttack + " " + SetPicture[RoleIndex].BuffAttack + " " + SetEnemyPicture[i].RolePDefend);
                    ///////////////////技能伤害加成///////////////////
                    switch (SkillID.parameter2)
                    {
                        case 24:
                            if (SetEnemyPicture[i].RoleRace == 3)
                            {
                                FightDamige += (int)(SetPicture[RoleIndex].RolePAttack * SkillID.skillVal2 / 100f);
                            }
                            break;
                        case 25:
                            if (SetEnemyPicture[i].RoleRace == 2)
                            {
                                FightDamige += (int)(SetPicture[RoleIndex].RolePAttack * SkillID.skillVal2 / 100f);
                            }
                            break;
                    }
                    ///////////////////技能伤害加成///////////////////

                    if (!SetPicture[RoleIndex].RolePictureMonster)
                    {
                        //Debug.LogError("FightCalculate" + FightCalculate);
                        FightDamige = (int)(FightDamige * FightCalculate);
                    }
                    else
                    {
                        //Debug.LogError("EnemyCalculate" + EnemyCalculate);
                        FightDamige = (int)(FightDamige * EnemyCalculate);
                    }

                    if (FightDamige < (int)(SetPicture[RoleIndex].RolePAttack / 3f))
                    {
                        FightDamige = (int)(SetPicture[RoleIndex].RolePAttack / 3f);
                    }

                    FightDamige *= IsDamige;
                }
                else if (IsDamige < 0)
                {
                    FightDamige = (int)(SetPicture[RoleIndex].RolePAttack * SkillID.skillVal1 / 100);

                    if (FightDamige > 0)
                    {
                        FightDamige *= IsDamige;
                    }
                }
                else
                {
                    FightDamige = 0;
                }

                SetPicture[RoleIndex].RoleFightIndex.Add(i);
                SetPicture[RoleIndex].RoleFightType.Add(0);
                SetPicture[RoleIndex].RoleFightDamige.Add(FightDamige);
            }
        }
        else//同等，即效果对所有目标角色同等，每个人独立计算其数值，次数
        {
            foreach (int i in TargetIndex)
            {
                if (IsDamige > 0)
                {
                    Debug.Log(i);
                    FightDamige = (int)(((SetPicture[RoleIndex].RolePAttack / SkillID.weather * (1 + SetPicture[RoleIndex].BuffAttack)) - SetEnemyPicture[i].RolePDefend / SkillID.weather - SetEnemyPicture[i].BuffDefend) * SkillID.skillVal1 / 100f);

                    ///////////////////技能伤害加成///////////////////
                    switch (SkillID.parameter2)
                    {
                        case 24:
                            if (SetEnemyPicture[i].RoleBio == 1)
                            {
                                FightDamige += (int)(SetPicture[RoleIndex].RolePAttack * SkillID.skillVal2 / 100f);
                            }
                            break;
                        case 25:
                            if (SetEnemyPicture[i].RoleBio == 0 && SetEnemyPicture[i].RoleRace == 0)
                            {
                                FightDamige += (int)(SetPicture[RoleIndex].RolePAttack * SkillID.skillVal2 / 100f);
                            }
                            break;
                    }
                    ///////////////////技能伤害加成///////////////////

                    if (!SetPicture[RoleIndex].RolePictureMonster)
                    {
                        //Debug.LogError("FightCalculate" + FightCalculate);
                        FightDamige = (int)(FightDamige * FightCalculate);
                    }
                    else
                    {
                        //Debug.LogError("EnemyCalculate" + EnemyCalculate);
                        FightDamige = (int)(FightDamige * EnemyCalculate);
                    }

                    if (FightDamige < (int)(SetPicture[RoleIndex].RolePAttack / 3f))
                    {
                        FightDamige = (int)(SetPicture[RoleIndex].RolePAttack / 3f);
                    }

                    FightDamige *= IsDamige;
                }
                else if (IsDamige < 0)
                {
                    FightDamige = (int)(SetPicture[RoleIndex].RolePAttack * SkillID.skillVal1 / 100);

                    if (FightDamige > 0)
                    {
                        FightDamige *= IsDamige;
                    }
                }
                else
                {
                    FightDamige = 0;
                }

                SetPicture[RoleIndex].RoleFightIndex.Add(i);
                SetPicture[RoleIndex].RoleFightType.Add(0);
                SetPicture[RoleIndex].RoleFightDamige.Add(FightDamige);
            }
        }
        return IsSuccess;
    }
    public void PlayEffect(List<RolePicture> SetPicture, int RoleIndex, FightMotion fm)
    {
        for (int i = 0; i < fm.effectList.size; i++)
        {
            FightEffect fe = TextTranslator.instance.fightEffectDic[fm.effectList[i]];

            if (fe.effect == "wesker_plus_skill")
            {
                continue;
            }

            if (fe.effect != "0")
            {
                if (fe.projectile == 9999)
                {
                    if (fe.effectParent != "0")
                    {
                        EffectMaker.instance.Create2DEffect("~" + fe.effect, "", FindChildObject(SetPicture[RoleIndex].RoleObject, fe.effectParent), new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), MyCamera.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, SetPicture[RoleIndex].RolePictureMonster, "", null);
                    }
                    else
                    {
                        EffectMaker.instance.Create2DEffect("~" + fe.effect, "", null, new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), MyCamera.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, SetPicture[RoleIndex].RolePictureMonster, "", null);
                    }
                }
                else
                {
                    if (fe.effectParent != "0")
                    {
                        EffectMaker.instance.Create2DEffect("~" + fe.effect, "", FindChildObject(SetPicture[RoleIndex].RoleObject, fe.effectParent), SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(SetPicture[RoleIndex].RolePictureFaceRight * fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, false, SetPicture[RoleIndex].RolePictureMonster, "", null);
                    }
                    else
                    {
                        EffectMaker.instance.Create2DEffect("~" + fe.effect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(SetPicture[RoleIndex].RolePictureFaceRight * fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, false, SetPicture[RoleIndex].RolePictureMonster, "", null);
                    }
                }
            }
            StartCoroutine(AudioEditer.instance.DelaySound(fe.soundDelay, fe.sound));
        }
    }
    #endregion

    #region WinLose
    public IEnumerator FightBoss()
    {
        IsFightFinish = true;
        IsRoleInGate = false;
        fw.ExitButton.SetActive(false);
        fw.LabelCombo.SetActive(false);


        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            if (ListRolePicture[RoleIndex].RolePictureAttackable)
            {
                ListRolePicture[RoleIndex].RoleTargetObject.SetActive(false);
            }
        }

        //for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
        //{
        //    if (ListBase[i] != null)
        //    {
        //        ListBase[i].transform.Find("SkillObject").gameObject.SetActive(false);
        //    }
        //}

        if (FightStyle != 2)
        {
            while (MyUISliderValue > 0.5)
            {
                MyUISliderValue -= 0.005f;
                fw.SetCamera();

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    public IEnumerator ShowWin(List<RolePicture> SetPicture)
    {
        bool IsEnemyAlive = false;
        fw.MySequence.gameObject.SetActive(false);
        if (FightStyle == 0)
        {
            if (TextTranslator.instance.GetGateByID(SceneTransformer.instance.NowGateID).bossID != 0)
            {
                Time.timeScale = 0.3f;
                fw.FightMask.SetActive(true);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 0, 0, 0.6f);
                yield return new WaitForSeconds(0.05f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 255, 255, 0.6f);
                yield return new WaitForSeconds(0.05f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 0, 0, 0.6f);
                yield return new WaitForSeconds(0.05f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 255, 255, 0.6f);
                yield return new WaitForSeconds(0.05f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 0, 0, 0.6f); ;
                yield return new WaitForSeconds(0.05f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 255, 255, 0.6f);
                yield return new WaitForSeconds(0.05f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 0, 0, 0.6f); ;
                yield return new WaitForSeconds(0.1f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 255, 255, 0.6f);
                yield return new WaitForSeconds(0.1f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 0, 0, 0.6f); ;
                yield return new WaitForSeconds(0.1f);
                fw.FightMask.GetComponent<UISprite>().color = new Color(255, 255, 255, 0.6f);
                fw.FightMask.SetActive(false);
                fw.FightMask.GetComponent<UISprite>().color = new Color(0, 0, 0, 0.6f);
                yield return new WaitForSeconds(0.05f);
                Time.timeScale = 1;
            }
        }
        /////////////////////////////敌军逃跑/////////////////////////////


        /////////////////////////血量条//////////////////////////
        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(false);
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            ListEnemyPicture[RoleIndex].RoleRedBloodObject.SetActive(false);
        }
        /////////////////////////血量条//////////////////////////

        yield return new WaitForSeconds(0.1f);
        if (FightStyle != 2)
        {
            for (int j = 0; j < PictureCreater.instance.ListEnemyPicture.Count; j++)
            {
                if (ListEnemyPicture[j].RoleNowBlood > 0)
                {
                    GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/GanTanHao", typeof(GameObject)), PictureCreater.instance.ListPosition[ListEnemyPicture[j].RolePosition] + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
                    go.name = "GanTanHao" + ListEnemyPicture[j].RolePosition.ToString();
                    go.transform.parent = ListEnemyPicture[j].RoleObject.transform;
                    IsEnemyAlive = true;
                }
            }
            if (IsEnemyAlive)
            {
                yield return new WaitForSeconds(1f);
                for (int j = 0; j < PictureCreater.instance.ListEnemyPicture.Count; j++)
                {
                    if (ListEnemyPicture[j].RoleNowBlood > 0 && !ListEnemyPicture[j].IsPicture)
                    {
                        ListEnemyPicture[j].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 0);
                        ListEnemyPicture[j].RolePictureMoving = true;
                        ListEnemyPicture[j].RolePictureNewPosition = new Vector3(100, 0, 0);
                        newMainRoleForwardPosition = ListEnemyPicture[j].RolePictureNewPosition - ListEnemyPicture[j].RoleObject.transform.position;
                        MoveDistance = Vector3.Distance(ListEnemyPicture[j].RolePictureNewPosition, ListEnemyPicture[j].RoleObject.transform.position);
                        ListEnemyPicture[j].RolePictureCheckPosition = newMainRoleForwardPosition / MoveDistance * ListEnemyPicture[j].RolePiectureMoveSpeed * 80;
                        ListEnemyPicture[j].RolePictureObject.transform.rotation = Quaternion.identity;
                        ListEnemyPicture[j].RolePictureObject.transform.Rotate(0f, 90, 0f);
                    }
                }
                yield return new WaitForSeconds(0.6f);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < PictureCreater.instance.ListEnemyPicture.Count; j++)
                    {
                        if (ListEnemyPicture[j].RoleNowBlood > 0)
                        {
                            ListEnemyPicture[j].RolePictureTransparent -= 0.25f;
                            foreach (Component c in ListEnemyPicture[j].RoleObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true))
                            {
                                foreach (var m in c.renderer.materials)
                                {
                                    m.shader = Shader.Find("Transparent/Diffuse");
                                    m.SetColor("_Color", new Color(1, 1, 1, ListEnemyPicture[j].RolePictureTransparent));
                                }

                            }
                            foreach (Component c in ListEnemyPicture[j].RoleObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                            {
                                foreach (var m in c.renderer.materials)
                                {
                                    m.shader = Shader.Find("Transparent/Diffuse");
                                    m.SetColor("_Color", new Color(1, 1, 1, ListEnemyPicture[j].RolePictureTransparent));
                                }
                            }
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }
            /////////////////////////////敌军逃跑/////////////////////////////
        }
        if (!IsResult)
        {
            IsResult = true;
            fw.Tactics.SetActive(false);
            foreach (RolePicture r in SetPicture)
            {
                if (r.RolePictureAttackable && !r.IsPicture)
                {
                    r.RolePictureObject.GetComponent<Animator>().SetFloat("ft", 2);
                    r.RolePictureObject.GetComponent<Animator>().SetFloat("box", 2);
                }
            }
            //yield return new WaitForSeconds(1.5f);

            Time.timeScale = 1;
            yield return new WaitForSeconds(1.5f);

            int FightStar = 1;
            string HeroPosition = "";
            string WoodString = "";
            foreach (var h in ListRolePicture)
            {
                if (h.RolePosition > 0 && h.RoleCharacterRoleID > 0 && h.RolePicturePointID.IndexOf("Enemy") == -1)
                {
                    HeroPosition += h.RoleCharacterRoleID.ToString() + "$";
                }
                int NewBlood = (int)(h.RoleMaxBlood * 0.9f - h.RoleNowBlood);
                WoodString += h.RoleCharacterRoleID.ToString() + "$" + (NewBlood > 0 ? NewBlood : 0).ToString() + "$" + (h.RoleSkillPoint > 1000 ? "1000" : (h.RoleSkillPoint + 100).ToString()) + "$" + (h.RoleNowBlood <= 0 ? "1" : "0") + "!";
            }

            for (int i = 0; i < CharacterRecorder.instance.ownedHeroList.size; i++)
            {
                if (CharacterRecorder.instance.ownedHeroList[i].characterRoleID == -1)
                {
                    CharacterRecorder.instance.ownedHeroList.RemoveAt(i);
                }
            }

            FightStar += GetStar(LimitStar2, LimitStarCount2);
            FightStar += GetStar(LimitStar3, LimitStarCount3);

            if (IsRemember)
            {
                if (FightStyle == 2)
                {
                    NetworkHandler.instance.SendProcess("1511#" + WoodString + ";");
                    NetworkHandler.instance.SendProcess("1503#" + WoodFloor.ToString() + ";" + WoodStar.ToString() + ";" + FightStar + ";");
                }
                else if (FightStyle == 0)
                {
                    //结果出现前对话
                    if (((CharacterRecorder.instance.GuideID[1] == 0 && CharacterRecorder.instance.GuideID[0] >= 3 && SceneTransformer.instance.NowGateID == 10005) || (CharacterRecorder.instance.GuideID[3] == 0 && CharacterRecorder.instance.GuideID[2] >= 3 &&
                        SceneTransformer.instance.NowGateID == 10007) || (CharacterRecorder.instance.GuideID[6] == 0 && CharacterRecorder.instance.GuideID[5] > 4 && SceneTransformer.instance.NowGateID == 10018)
                        || (CharacterRecorder.instance.GuideID[8] == 0 && CharacterRecorder.instance.GuideID[7] >= 2 &&
                        SceneTransformer.instance.NowGateID == 10021) || (CharacterRecorder.instance.GuideID[25] < 4 && SceneTransformer.instance.NowGateID == 10010))
                         && SceneTransformer.instance.CheckGuideIsFinish())
                    {

                        SceneTransformer.instance.TempSendString = "2002#" + SceneTransformer.instance.NowGateID.ToString() + ";" + FightStar + ";" + HeroPosition + ";";
                        StartCoroutine(SceneTransformer.instance.NewbieGuide());
                    }
                    else if (TextTranslator.instance.GetTalkById(SceneTransformer.instance.NowGateID % 10000 + 200, 1) != null && SceneTransformer.instance.NowGateID == CharacterRecorder.instance.lastGateID)
                    {
                        IsChip = true;
                        SceneTransformer.instance.SetEventNumber(SceneTransformer.instance.NowGateID % 10000 + 200, 1);
                        SceneTransformer.instance.TempSendString = "2002#" + SceneTransformer.instance.NowGateID.ToString() + ";" + FightStar + ";" + HeroPosition + ";";
                    }
                    else
                    {
                        NetworkHandler.instance.SendProcess("2002#" + SceneTransformer.instance.NowGateID.ToString() + ";" + FightStar + ";" + HeroPosition + ";");
                    }
                }
                else if (FightStyle == 5)
                {
                    NetworkHandler.instance.SendProcess("1131#" + SceneTransformer.instance.NowGateID.ToString() + ";" + HeroPosition + ";");
                }
                else if (FightStyle == 4)
                {
                    NetworkHandler.instance.SendProcess("2203#" + SceneTransformer.instance.NowWorldEventID.ToString() + ";" + HeroPosition + ";");
                }
                else if (FightStyle == 3)
                {
                    NetworkHandler.instance.SendProcess("1405#1;" + GrabID.ToString() + ";" + PVPRank.ToString() + ";");

                    if (!ListWinRecord.ContainsKey(PVPRank))
                    {
                        ListWinRecord.Add(PVPRank, PictureCreater.instance.PVPPosition);
                    }
                    else
                    {
                        ListWinRecord[PVPRank] = PictureCreater.instance.PVPPosition;
                    }
                }
                else if (FightStyle == 6)
                {
                    Debug.LogError("赏金赢");
                    fw.OneSlider.SetActive(false);
                    fw.EverydayBossobj.transform.Find("HPNum").GetComponent<UILabel>().text = "X0";
                    NetworkHandler.instance.SendProcess("1702#" + CharacterRecorder.instance.EveryDiffID + ";" + "100" + ";");
                    CharacterRecorder.instance.EveryDiffID = 0;
                }
                else if (FightStyle == 7)
                {
                    fw.OneSlider.SetActive(false);
                    fw.EverydayBossobj.transform.Find("HPNum").GetComponent<UILabel>().text = "X0";
                    NetworkHandler.instance.SendProcess("1703#" + CharacterRecorder.instance.EveryDiffID + ";" + "100" + ";");
                    Debug.LogError("千锤赢");
                    CharacterRecorder.instance.EveryDiffID = 0;
                }
                else if (FightStyle == 8)
                {
                    Debug.LogError("极限挑战赢");
                    NetworkHandler.instance.SendProcess("1704#" + CharacterRecorder.instance.EveryDiffID + ";");
                    //NetworkHandler.instance.SendProcess("2002#" + SceneTransformer.instance.NowGateID.ToString() + ";" + FightStar + ";" + HeroPosition + ";");
                    CharacterRecorder.instance.EveryDiffID = 0;
                }
                else if (FightStyle == 9)
                {
                    Debug.LogError("固若金汤赢");
                    NetworkHandler.instance.SendProcess("1705#" + CharacterRecorder.instance.EveryDiffID + ";3;");
                    CharacterRecorder.instance.EveryDiffID = 0;
                }
                else if (FightStyle == 10)
                {
                    Debug.LogError("攻守兼备赢");
                    NetworkHandler.instance.SendProcess("1705#" + CharacterRecorder.instance.EveryDiffID + ";4;");
                    CharacterRecorder.instance.EveryDiffID = 0;
                }
                else if (FightStyle == 11)
                {
                    Debug.LogError("弃防强攻赢");
                    NetworkHandler.instance.SendProcess("1705#" + CharacterRecorder.instance.EveryDiffID + ";5;");
                    CharacterRecorder.instance.EveryDiffID = 0;
                }
                else if (FightStyle == 12)
                {
                    string[] dataSplits = LegionGateString.Split('$');
                    string GateString = "";
                    int k = 0;
                    int TotalDamige = 0;
                    foreach (var h in ListEnemyPicture)
                    {
                        if (h.RoleNowBlood < 0)
                        {
                            h.RoleNowBlood = 0;
                        }
                        int NewBlood = int.Parse(dataSplits[k]) - (int)(h.RoleNowBlood);
                        if (NewBlood < 0)
                        {
                            NewBlood = 0;
                        }
                        TotalDamige += NewBlood;
                        GateString += NewBlood + "$";
                        k++;
                    }

                    Debug.LogError("军团副本赢");
                    AudioEditer.instance.PlayLoop("Lose");
                    NetworkHandler.instance.SendProcess("8306#" + LegionGroupID + ";" + LegionGateID + ";" + GateString + ";" + TotalDamige + ";");


                    if (LegionGroupID == PlayerPrefs.GetInt("LegionGroupID") && (PlayerPrefs.GetInt("LegionGateIDNum") + 1) == 5)//yy  11.22
                    {
                        NetworkHandler.instance.SendProcess("7002#17;" + CharacterRecorder.instance.characterName + ";" + LegionGroupID + ";0;");
                    }
                }
                else if (FightStyle == 13)
                {
                    NetworkHandler.instance.SendProcess("6308#1;" + PVPRank.ToString() + ";");

                    if (!ListWinRecord.ContainsKey(PVPRank))
                    {
                        ListWinRecord.Add(PVPRank, PictureCreater.instance.PVPPosition);
                    }
                    else
                    {
                        ListWinRecord[PVPRank] = PictureCreater.instance.PVPPosition;
                    }
                }
                else if (FightStyle == 14)
                {
                    if (CharacterRecorder.instance.LegionFestPosition != "")
                    {
                        string GateString = "";
                        string[] dataSplits = CharacterRecorder.instance.LegionFestPosition.Split(';');
                        for (int i = 0; i < dataSplits.Length - 1; i++)
                        {
                            string[] secSplit = dataSplits[i].Split('$');
                            EnemyWeakPoint = int.Parse(secSplit[18]);
                            float EnemyWeakNum = (100 - TextTranslator.instance.GetLegionWeakByWinNum(EnemyWeakPoint)) / 100f;
                            if (int.Parse(secSplit[15]) > 0)
                            {
                                if (ListEnemyPicture[i].RoleNowBlood < 0)
                                {
                                    ListEnemyPicture[i].RoleNowBlood = 0;
                                }
                                int NewBlood = 1000 + int.Parse(secSplit[15]) - (int)(ListEnemyPicture[i].RoleNowBlood);
                                NewBlood = (int)(NewBlood / EnemyWeakNum);
                                if (NewBlood < 0)
                                {
                                    NewBlood = 0;
                                }
                                GateString += ListEnemyPicture[i].RoleCharacterRoleID + "$" + NewBlood + "!";
                            }
                        }
                        EnemyWeakPoint++;
                        ResetLegionWar(); //设定国战血量
                        NetworkHandler.instance.SendProcess("8615#" + CharacterRecorder.instance.LegionHarasPoint + ";" + GateString + ";1;" + EnemyWeakPoint + ";" + CharacterRecorder.instance.MarinesTabe + ";");



                    }
                    else
                    {
                        ResetLegionWar(); //设定国战血量
                        NetworkHandler.instance.SendProcess("8615#" + CharacterRecorder.instance.LegionHarasPoint + ";0;1;100;" + CharacterRecorder.instance.MarinesTabe + ";");
                    }
                }
                else if (FightStyle == 15)
                {
                    GameObject.Find("FightWindow").transform.Find("GrabWin").gameObject.SetActive(true);
                    StartCoroutine(ConquerResultEffect(true));
                    //NetworkHandler.instance.SendProcess("6504#" + CharacterRecorder.instance.TabeID + ";" + CharacterRecorder.instance.KengID + ";" + CharacterRecorder.instance.PlayerUid + ";");
                }
                else if (FightStyle == 16)
                {
                    NetworkHandler.instance.SendProcess("8103#" + CharacterRecorder.instance.legionCountryID + ";" + CharacterRecorder.instance.EnemyMilitaryRankID + ";1;");
                }
                else if (FightStyle == 17)
                {
                    NetworkHandler.instance.SendProcess("6602#" + CharacterRecorder.instance.NuclearLevel + ";");
                }
                else
                {
                    NetworkHandler.instance.SendProcess("6005#" + PVPRank + ";1;");

                    if (!ListWinRecord.ContainsKey(PVPRank))
                    {
                        ListWinRecord.Add(PVPRank, PictureCreater.instance.PVPPosition);
                    }
                    else
                    {
                        ListWinRecord[PVPRank] = PictureCreater.instance.PVPPosition;
                    }
                }
            }
            else
            {
                fw.Pause.SetActive(true);
                fw.Pause.transform.FindChild("GoButton/Sprite").gameObject.GetComponent<UISprite>().spriteName = "zhandou_word3";
            }
#if !ClashRoyale
            if (FightStyle != 14)
            {
                yield return new WaitForSeconds(1f);

                for (int RoleIndex = 0; RoleIndex < SetPicture.Count; RoleIndex++)
                {
                    if (SetPicture[RoleIndex].RolePictureAttackable)
                    {
                        ChangeDirection(SetPicture, SetPicture[RoleIndex].RoleObject.transform.position, MyCamera.transform.position, RoleIndex);
                        SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().Play("attack");
                    }
                }

                yield return new WaitForSeconds(2f);
                for (int RoleIndex = 0; RoleIndex < SetPicture.Count; RoleIndex++)
                {
                    if (SetPicture[RoleIndex].RolePictureAttackable)
                    {
                        ChangeDirection(SetPicture, SetPicture[RoleIndex].RoleObject.transform.position, MyCamera.transform.position, RoleIndex);

                        SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().Play("attack");
                    }
                }
            }
#endif
        }
        PictureCreater.instance.PVPRank = 0;
    }


    void ResetLegionWar()
    {
        string TeamString = "";
        for (int i = 0; i < CharacterRecorder.instance.HarassformationList.Count; i++)
        {
            foreach (var h in ListRolePicture)
            {
                if (h.RoleCharacterRoleID == CharacterRecorder.instance.HarassformationList[i].HeroId)
                {
                    MyWeakPoint = (int)CharacterRecorder.instance.HarassformationList[i].WeakPoint;
                    if (h.RoleNowBlood < 0)
                    {
                        h.RoleNowBlood = 0;
                    }
                    int NewBlood = 1000 + (int)(CharacterRecorder.instance.HarassformationList[i].BloodNum * (100 - CharacterRecorder.instance.HarassformationList[i].WeakPoint) / 100f) - (int)(h.RoleNowBlood);
                    NewBlood = (int)(NewBlood / WeakNum);
                    if (NewBlood < 1)
                    {
                        NewBlood = 0;
                    }

                    TeamString += h.RoleCharacterRoleID + "$" + NewBlood + "!";
                    break;
                }
            }
        }
        MyWeakPoint++;
        NetworkHandler.instance.SendProcess("8638#" + CharacterRecorder.instance.MarinesTabe + ";" + TeamString + ";" + MyWeakPoint + ";");
    }

    int GetStar(int StarType, int Count)
    {
        int Star = 0;
        int DeadCount = 0;
        switch (StarType)
        {
            case 1:
                foreach (var h in ListRolePicture)
                {
                    if (h.RolePosition > 0 && h.RolePicturePointID.IndexOf("Enemy") == -1)
                    {
                        if (h.RoleNowBlood <= 0)
                        {
                            DeadCount++;
                        }
                    }
                }
                if (DeadCount <= Count)
                {
                    Star = 1;
                }
                break;
            case 2:
                string[] dataSplit = PictureCreater.instance.PVEPosition.Split('!');
                if ((dataSplit.Length - 1) <= Count)
                {
                    Star = 1;
                }
                break;
            case 3:
                if (NowSequence <= Count)
                {
                    Star = 1;
                }
                break;
        }
        return Star;
    }
    IEnumerator ShowLose()
    {
        fw.MySequence.gameObject.SetActive(false);
        /////////////////////////血量条//////////////////////////
        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(false);
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            ListEnemyPicture[RoleIndex].RoleRedBloodObject.SetActive(false);
        }
        /////////////////////////血量条//////////////////////////

        if (!IsResult)
        {
            IsResult = true;

            Time.timeScale = 1;
            yield return new WaitForSeconds(0.5f);
            if (!NetworkHandler.instance.IsCreate)
            {
                if (IsRemember)
                {
                    if (FightStyle == 3)
                    {
                        NetworkHandler.instance.SendProcess("1405#0;" + GrabID.ToString() + ";" + PVPRank.ToString() + ";");
                    }
                    else if (FightStyle == 6)
                    {
                        Debug.Log("sss" + CharacterRecorder.instance.EveryDiffID);
                        NetworkHandler.instance.SendProcess("1702#" + CharacterRecorder.instance.EveryDiffID + ";" + ((1 - PictureCreater.instance.ListEnemyPicture[0].RoleNowBlood / PictureCreater.instance.ListEnemyPicture[0].RoleMaxBlood) * 100).ToString("00") + ";");
                        CharacterRecorder.instance.EveryDiffID = 0;
                    }
                    else if (FightStyle == 7)
                    {
                        Debug.Log("sss" + CharacterRecorder.instance.EveryDiffID);
                        NetworkHandler.instance.SendProcess("1703#" + CharacterRecorder.instance.EveryDiffID + ";" + ((1 - PictureCreater.instance.ListEnemyPicture[0].RoleNowBlood / PictureCreater.instance.ListEnemyPicture[0].RoleMaxBlood) * 100).ToString("00") + ";");
                        CharacterRecorder.instance.EveryDiffID = 0;
                    }
                    else if (FightStyle == 8)
                    {
                        Debug.LogError("极限挑战输");
                        AudioEditer.instance.PlayLoop("Lose");
                        UIManager.instance.OpenPanel("ResultWindow", false);
                        ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                        rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
                        CharacterRecorder.instance.EveryDiffID = 0;
                    }
                    else if (FightStyle == 9)
                    {
                        Debug.LogError("固若金汤输");
                        AudioEditer.instance.PlayLoop("Lose");
                        UIManager.instance.OpenPanel("ResultWindow", false);
                        ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                        rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
                        CharacterRecorder.instance.EveryDiffID = 0;
                    }
                    else if (FightStyle == 10)
                    {
                        Debug.LogError("攻守兼备输");
                        AudioEditer.instance.PlayLoop("Lose");
                        UIManager.instance.OpenPanel("ResultWindow", false);
                        ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                        rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
                        CharacterRecorder.instance.EveryDiffID = 0;
                    }
                    else if (FightStyle == 11)
                    {
                        Debug.LogError("弃防强攻输");
                        AudioEditer.instance.PlayLoop("Lose");
                        UIManager.instance.OpenPanel("ResultWindow", false);
                        ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                        rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
                        CharacterRecorder.instance.EveryDiffID = 0;
                    }
                    else if (FightStyle == 12)
                    {
                        string[] dataSplits = LegionGateString.Split('$');
                        string GateString = "";
                        int k = 0;
                        int TotalDamige = 0;
                        foreach (var h in ListEnemyPicture)
                        {
                            //Debug.LogError(dataSplits[k] + " " + h.RoleNowBlood);
                            //Debug.LogError(int.Parse(dataSplits[k]) + " " + (int)(h.RoleNowBlood));
                            if (h.RoleNowBlood < 0)
                            {
                                h.RoleNowBlood = 0;
                            }
                            int NewBlood = int.Parse(dataSplits[k]) - (int)(h.RoleNowBlood);
                            if (NewBlood < 0)
                            {
                                NewBlood = 0;
                            }
                            TotalDamige += NewBlood;
                            GateString += NewBlood + "$";
                            k++;
                        }

                        Debug.LogError("军团副本输");
                        AudioEditer.instance.PlayLoop("Lose");
                        NetworkHandler.instance.SendProcess("8306#" + LegionGroupID + ";" + LegionGateID + ";" + GateString + ";" + TotalDamige + ";");
                    }
                    else if (FightStyle == 13)
                    {
                        NetworkHandler.instance.SendProcess("6308#0;" + PVPRank.ToString() + ";");
                    }
                    else if (FightStyle == 14)
                    {
                        if (CharacterRecorder.instance.LegionFestPosition != "")
                        {
                            string GateString = "";
                            string[] dataSplits = CharacterRecorder.instance.LegionFestPosition.Split(';');
                            for (int i = 0; i < dataSplits.Length - 1; i++)
                            {
                                string[] secSplit = dataSplits[i].Split('$');
                                EnemyWeakPoint = int.Parse(secSplit[18]);
                                float EnemyWeakNum = (100 - TextTranslator.instance.GetLegionWeakByWinNum(EnemyWeakPoint)) / 100f;
                                if (int.Parse(secSplit[15]) > 0)
                                {
                                    if (ListEnemyPicture[i].RoleNowBlood < 0)
                                    {
                                        ListEnemyPicture[i].RoleNowBlood = 0;
                                    }
                                    int NewBlood = int.Parse(secSplit[15]) - (int)(ListEnemyPicture[i].RoleNowBlood);
                                    NewBlood = (int)(NewBlood / EnemyWeakNum);
                                    if (NewBlood < 0)
                                    {
                                        NewBlood = 0;
                                    }
                                    GateString += ListEnemyPicture[i].RoleCharacterRoleID + "$" + NewBlood + "!";
                                }
                            }
                            EnemyWeakPoint++;
                            ResetLegionWar(); //设定国战血量
                            NetworkHandler.instance.SendProcess("8615#" + CharacterRecorder.instance.LegionHarasPoint + ";" + GateString + ";0;" + EnemyWeakPoint + ";" + CharacterRecorder.instance.MarinesTabe + ";");

                        }
                        else
                        {
                            Debug.LogError("搔扰失败");
                            UIManager.instance.OpenSinglePanel("HarassResultWindow", false);
                            GameObject HR = GameObject.Find("HarassResultWindow");
                            if (HR != null)
                            {
                                HR.GetComponent<HarassResultWindow>().LegionWarStartWar(false);
                            }
                            ResetLegionWar(); //设定国战血量
                        }
                    }
                    else if (FightStyle == 15)
                    {
                        Debug.LogError("俘虏输");
                        GameObject.Find("FightWindow").transform.Find("GrabLose").gameObject.SetActive(true);
                        StartCoroutine(ConquerResultEffect(false));
                        //UIManager.instance.BackTwoUI("ConquerWindow");                    
                        //if (GameObject.Find("ConquerWindow") != null && CharacterRecorder.instance.TabeID != 0)
                        //{
                        //    NetworkHandler.instance.SendProcess("6502#" + CharacterRecorder.instance.TabeID + ";");
                        //    GameObject.Find("ConquerWindow").GetComponent<ConquerWindow>().CheckGateWindow.SetActive(false);
                        //    GameObject.Find("ConquerWindow").GetComponent<ConquerWindow>().HarvestWindow.SetActive(true);
                        //    UIManager.instance.OpenPromptWindow("俘虏失败", PromptWindow.PromptType.Hint, null, null);
                        //}
                        //else
                        //{
                        //    NetworkHandler.instance.SendProcess("6501#;");
                        //}
                    }
                    else if (FightStyle == 16)
                    {
                        NetworkHandler.instance.SendProcess("8103#" + CharacterRecorder.instance.legionCountryID + ";" + CharacterRecorder.instance.EnemyMilitaryRankID + ";2;");
                    }
                    else if (FightStyle == 17)
                    {
                        Debug.LogError("弃防强攻输");
                        AudioEditer.instance.PlayLoop("Lose");
                        UIManager.instance.OpenPanel("ResultWindow", false);
                        ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                        rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
                    }
                    else if (FightStyle != 1)
                    {
                        if (FightStyle == 2)
                        {
                            string WoodString = "";
                            foreach (var h in ListRolePicture)
                            {
                                WoodString += h.RoleCharacterRoleID.ToString() + "$" + ((int)(h.RoleMaxBlood - h.RoleNowBlood)).ToString() + "$" + (h.RoleSkillPoint > 1000 ? "1000" : h.RoleSkillPoint.ToString()) + "$" + (h.RoleNowBlood <= 0 ? "1" : "0") + "!";
                            }

                            if (FightStyle == 2)
                            {
                                NetworkHandler.instance.SendProcess("1511#" + WoodString + ";");
                            }
                        }

                        AudioEditer.instance.PlayLoop("Lose");
                        UIManager.instance.OpenPanel("ResultWindow", false);
                        ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                        if (FightStyle == 2)
                        {
                            rw.SetWoodFailed();
                        }
                        else
                        {
                            if (CharacterRecorder.instance.GuideID[59] == 0)//失败引导
                            {
                                CharacterRecorder.instance.GuideID[59] = 1;
                                StartCoroutine(SceneTransformer.instance.NewbieGuide());
                            }
                            rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
                        }

                    }
                    else
                    {
                        NetworkHandler.instance.SendProcess("6005#" + PVPRank + ";0;");
                    }
                }
                else
                {
                    fw.Pause.SetActive(true);
                    fw.Pause.transform.FindChild("GoButton/Sprite").gameObject.GetComponent<UISprite>().spriteName = "zhandou_word3";
                }
            }
            else
            {
                Debug.LogError("新手引导战斗失败！");
                UIManager.instance.OpenPanel("WhiteWindow");
                //PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 1);
                StartCoroutine(AudioEditer.instance.SoundFadeOut());
            }
        }
        PictureCreater.instance.PVPRank = 0;
    }

    /// <summary>
    /// 俘虏战斗结算
    /// </summary>
    public IEnumerator ConquerResultEffect(bool isWin)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("FightWindow").transform.Find("GrabWin").gameObject.SetActive(false);
        GameObject.Find("FightWindow").transform.Find("GrabLose").gameObject.SetActive(false);
        if (isWin)
        {
            NetworkHandler.instance.SendProcess("6504#" + CharacterRecorder.instance.TabeID + ";" + CharacterRecorder.instance.KengID + ";" + CharacterRecorder.instance.PlayerUid + ";");
        }
        else
        {
            PictureCreater.instance.StopFight(true);
            UIManager.instance.BackTwoUI("ConquerWindow");
            if (GameObject.Find("ConquerWindow") != null && CharacterRecorder.instance.TabeID != 0)
            {
                NetworkHandler.instance.SendProcess("6502#" + CharacterRecorder.instance.TabeID + ";");
                GameObject.Find("ConquerWindow").GetComponent<ConquerWindow>().CheckGateWindow.SetActive(false);
                GameObject.Find("ConquerWindow").GetComponent<ConquerWindow>().HarvestWindow.SetActive(true);
                UIManager.instance.OpenPromptWindow("俘虏失败", PromptWindow.PromptType.Hint, null, null);
            }
            else
            {
                NetworkHandler.instance.SendProcess("6501#;");
            }
        }
    }
    public IEnumerator ShowFight()
    {
        fw.Repress.SetActive(false);
        MyMoves.SetActive(false);

        for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
        {
            if (ListMove[i] != null)
            {
                ListMove[i].renderer.material = BlankMaterial;
            }
        }

        if (!NetworkHandler.instance.IsCreate)
        {
            //fw.MySequence.alpha = 1;
        }

        ///////////////////////关卡剧情对话///////////////////////
        if (SceneTransformer.instance.NowGateID == CharacterRecorder.instance.lastGateID)
        {
            List<FightTalk> _ListFightTalk = TextTranslator.instance.GetFightTalkByGateID(SceneTransformer.instance.NowGateID, 0);
            if (_ListFightTalk.Count == 0)
            {

            }
            else
            {
                IsLock = true;
                foreach (var t in _ListFightTalk)
                {
                    if (t.TalkKind == 1)
                    {
                        bool IsRepeat = false;
                        foreach (var r in ListRolePicture)
                        {
                            if (r.RoleID == t.RoleID)
                            {
                                r.RoleRedBloodObject.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().LabelTalk.text = t.Talk;
                                IsRepeat = true;
                                AudioEditer.instance.PlayOneShot("Talk" + t.FightTalkID);
                                break;
                            }
                        }

                        if (!IsRepeat)
                        {
                            HeroInfo hi = TextTranslator.instance.GetHeroInfoByHeroID(t.RoleID);
                            CreateRole(t.RoleID, "援军", t.TalkType, Color.cyan, 20000, 20000, 1000, 10f, false, false, 1, 1.5f, 0, "Enemy", 0, 500, 0, 20, 0, 0, 0, hi.heroSkillList[0], 0, 0, 1, 0, hi.heroArea, hi.heroMove, 1000, "");
                            SetPictureForwardPosition(ListRolePicture, t.TalkType, ListRolePicture.Count - 1);
                            AudioEditer.instance.PlayOneShot("Talk" + t.FightTalkID);
                            InsertSequence(ListRolePicture, ListRolePicture.Count - 1, false);
                            ListRolePicture[ListRolePicture.Count - 1].RoleRedBloodObject.SetActive(true);
                            ListRolePicture[ListRolePicture.Count - 1].RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(true);
                            ListRolePicture[ListRolePicture.Count - 1].RoleObject.GetComponent<ColliderDisplayText>().LabelTalk.text = t.Talk;
                        }


                        yield return new WaitForSeconds(2f);
                        foreach (var r in ListRolePicture)
                        {
                            r.RoleRedBloodObject.SetActive(false);
                            r.RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(false);
                        }
                    }
                    else if (t.TalkKind == 2)
                    {
                        foreach (var r in ListEnemyPicture)
                        {
                            if (r.RoleID == t.RoleID)
                            {
                                r.RoleRedBloodObject.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().LabelTalk.text = t.Talk;
                                AudioEditer.instance.PlayOneShot("Talk" + t.FightTalkID);
                                break;
                            }
                        }
                        yield return new WaitForSeconds(1.5f);
                        foreach (var r in ListEnemyPicture)
                        {
                            r.RoleRedBloodObject.SetActive(false);
                            r.RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(false);
                        }
                    }
                }
                //用于解锁剧情对话
                if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) != 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) != 8)
                {
                    IsLock = false;
                }
            }
        }
        ///////////////////////关卡剧情对话///////////////////////



        /////////////////被动光环////////////////////
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            if (ListRolePicture[i].RolePosition > 0)
            {
                ////////////////////设定我方buff///////////////////////
                Skill postiveSkill = TextTranslator.instance.GetSkillByID(ListRolePicture[i].RoleSkill2, ListRolePicture[i].RoleSkillLevel2);
                if (postiveSkill != null)
                {
                    SetSkillBuff(ListRolePicture, ListEnemyPicture, i, postiveSkill, true);
                }
                ////////////////////设定我方buff///////////////////////
            }
        }

        for (int i = 0; i < ListEnemyPicture.Count; i++)
        {
            if (ListEnemyPicture[i].RolePosition > 0)
            {
                ////////////////////设定我方buff///////////////////////
                Skill postiveSkill = TextTranslator.instance.GetSkillByID(ListEnemyPicture[i].RoleSkill2, ListEnemyPicture[i].RoleSkillLevel2);
                if (postiveSkill != null)
                {
                    SetSkillBuff(ListEnemyPicture, ListRolePicture, i, postiveSkill, true);
                }
                ////////////////////设定我方buff///////////////////////
            }
        }
        /////////////////被动光环////////////////////

        //for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
        //{
        //    if (GameObject.Find("Move" + i.ToString()) != null)
        //    {
        //        GameObject.Find("Move" + i.ToString()).renderer.material.mainTexture = Resources.Load("Game/blank", typeof(Texture)) as Texture;
        //    }
        //}

        for (int j = 1; j < PictureCreater.instance.PositionRow * PictureCreater.instance.PositionColumn + 1; j++)
        {
            DestroyImmediate(GameObject.Find("GanTanHao" + j.ToString()));
        }

        if (CharacterRecorder.instance.level > 3)
        {
            if (FightStyle == 0 || FightStyle == 4 || FightStyle == 5)
            {
                if (IsRemember)
                {
                    fw.Tactics.SetActive(true);
                }
            }
            if (FightStyle == 2)
            {
                fw.HandSkill.SetActive(true);
            }
            if (FightStyle == 6 || FightStyle == 7)
            {
                Time.timeScale = 1;
            }
        }
#if !ClashRoyale
        MyPositions.SetActive(true);
#endif

        foreach (Component c in MyPositions.GetComponentsInChildren(typeof(MeshCollider), true))
        {
            c.gameObject.GetComponent<MeshCollider>().enabled = false;
        }

        MyBases.SetActive(false);
        TotalAttack = 0;
        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
            if (ListRolePicture[RoleIndex].RolePosition > 0)
            {
                //ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                ListRolePicture[RoleIndex].RoleShowBlood = false;
                TotalAttack += (int)(ListRolePicture[RoleIndex].BuffAttack + ListRolePicture[RoleIndex].RolePAttack);
            }
            if (ListRolePicture[RoleIndex].RolePosition == SelectPosition && FightStyle != 2)
            {
                ListRolePicture[RoleIndex].RoleSkillPoint += 500; //队长加怒
                MyCaptainIndex = RoleIndex;
            }

            ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(false);
        }


        if (RandomDebuff)
        {
            int Index = 0;
            foreach (var h in ListRolePicture)
            {
                if (RandomDebuffID == 0)
                {
                    Buff NewBuff = TextTranslator.instance.GetBuffByID(UnityEngine.Random.Range(717, 728));
                    RoleAddBuff(ListRolePicture, Index, NewBuff);
                }
                else
                {
                    Buff NewBuff = TextTranslator.instance.GetBuffByID(RandomDebuffID);
                    RoleAddBuff(ListRolePicture, Index, NewBuff);
                }
                Index++;
            }
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            ListEnemyPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
            //ListEnemyPicture[RoleIndex].RoleRedBloodObject.SetActive(true);
            ListEnemyPicture[RoleIndex].RoleShowBlood = false;
            if (!ListEnemyPicture[RoleIndex].IsPicture)
            {
                ListEnemyPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("id", 2);
            }
        }

        yield return new WaitForSeconds(0.01f);

        if (FightStyle != 2)
        {
            while (MyUISliderValue > 0.5f)
            {
                MyUISliderValue -= 0.02f;
                fw.SetCamera();
                MyCamera.GetComponent<Camera>().fieldOfView -= 0.05f;
                //FightCamera.GetComponent<Camera>().fieldOfView = MyCamera.GetComponent<Camera>().fieldOfView;
                yield return new WaitForEndOfFrame();
            }
        }

        //if (ListSequence[0].IsMonster)
        //{
        //    if (GameObject.Find("Position" + ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition.ToString()) != null)
        //    {
        //        GameObject.Find("Position" + ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition.ToString()).renderer.material.mainTexture = Resources.Load("Game/green", typeof(Texture)) as Texture;
        //    }
        //    //ShowPosition(ListEnemyPicture[ListSequence[0].RoleIndex].RoleObject.name, ListPosition[ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition], ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition, true);
        //}
        //else
        //{
        //    if (GameObject.Find("Position" + ListRolePicture[ListSequence[0].RoleIndex].RolePosition.ToString()) != null)
        //    {
        //        GameObject.Find("Position" + ListRolePicture[ListSequence[0].RoleIndex].RolePosition.ToString()).renderer.material.mainTexture = Resources.Load("Game/green", typeof(Texture)) as Texture;
        //    }
        //    //ShowPosition(ListRolePicture[ListSequence[0].RoleIndex].RoleObject.name, ListPosition[ListRolePicture[ListSequence[0].RoleIndex].RolePosition], ListRolePicture[ListSequence[0].RoleIndex].RolePosition, true);
        //    //ListRolePicture[ListSequence[0].RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 0);
        //}



    }
    #endregion

    #region Sequence
    public void SetSequence()
    {
        IsMySpeedUp = false;
        IsEnemySpeedUp = false;

        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            if (ListRolePicture[RoleIndex].RoleID == 60038)
            {
                IsMySpeedUp = true;
                break;
            }
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            if (ListEnemyPicture[RoleIndex].RoleID == 60038)
            {
                IsEnemySpeedUp = true;
                break;
            }
        }

        ListSequence.Clear();

        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            if (RoleIndex == MyCaptainIndex && FightStyle != 2)
            {
                float FightSpeed = 300 - ListRolePicture[RoleIndex].RoleFightSpeed;
                //速度   9队长提速
                Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(9, ListRolePicture[RoleIndex].RoleInnate[8]);
                if (SetInnate != null)
                {
                    FightSpeed *= (SetInnate.Value1 / 100f);
                }
                FightSpeed = 300 - FightSpeed;
                ListRolePicture[RoleIndex].RoleFightNowSpeed = FightSpeed;
            }
            else
            {
                ListRolePicture[RoleIndex].RoleFightNowSpeed = ListRolePicture[RoleIndex].RoleFightSpeed;
            }
            if (IsMySpeedUp)
            {
                ListRolePicture[RoleIndex].RoleFightNowSpeed -= 5;
            }
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            if (RoleIndex == EnemyCaptainIndex && FightStyle != 2)
            {
                float FightSpeed = 300 - ListEnemyPicture[RoleIndex].RoleFightSpeed;
                //速度   9队长提速
                Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(9, ListEnemyPicture[RoleIndex].RoleInnate[8]);
                if (SetInnate != null)
                {
                    FightSpeed *= (SetInnate.Value1 / 100f);
                }
                FightSpeed = 300 - FightSpeed;
                ListEnemyPicture[RoleIndex].RoleFightNowSpeed = FightSpeed;
            }
            else
            {
                ListEnemyPicture[RoleIndex].RoleFightNowSpeed = ListEnemyPicture[RoleIndex].RoleFightSpeed;
            }
            if (IsEnemySpeedUp)
            {
                ListEnemyPicture[RoleIndex].RoleFightNowSpeed -= 5;
            }
        }

        for (int i = 0; i < 3000; i++)
        {
            ListSequence.Add(GetLeastSequence());
        }
        StartCoroutine(ShowSequence(0));

        int SequenceCount = GetPositionCount();

        if (Application.loadedLevelName != "Downloader")
        {
            if (FightStyle != 17)
            {
                for (int i = 0; i < SequenceCount; i++)
                {
                    if (ListSequence[i].IsMonster)
                    {
                        ListEnemyPicture[ListSequence[i].RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSequence(i + 1, ListEnemyPicture[ListSequence[i].RoleIndex].RoleAttackType);
                    }
                    else
                    {
                        ListRolePicture[ListSequence[i].RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSequence(i + 1, ListRolePicture[ListSequence[i].RoleIndex].RoleAttackType);
                    }
                }
            }

            SetRepress();
        }
    }

    public void SetRepress()
    {
        if (FightStyle != 6 && FightStyle != 7 && CharacterRecorder.instance.level > 2)
        {
            int[] Front = new int[5];
            int[] Enemy = new int[5] { 31, 32, 28, 29, 25 };
            int[] FrontType = new int[5];
            float[] Center = new float[5] { 26, 24.5f, 23, 21.5f, 20 };
            for (int i = 0; i < 5; i++)
            {
                FrontType[i] = 3;
            }
            ////////////////兵种相克(以下)//////////////////
            for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
            {
                if (ListRolePicture[RoleIndex].RoleNowBlood > 0 && ListRolePicture[RoleIndex].RolePosition > 0 && ListRolePicture[RoleIndex].RolePosition < 22)
                {
                    int RolePosition = ListRolePicture[RoleIndex].RolePosition % 5;
                    bool IsFind = false;
                    switch (RolePosition)
                    {
                        case 1:
                            if (Front[0] < ListRolePicture[RoleIndex].RolePosition)
                            {
                                Front[0] = ListRolePicture[RoleIndex].RolePosition;
                                FrontType[0] = 0;
                                for (int i = 1; i <= 6; i++)
                                {
                                    for (int SetRoleIndex = 0; SetRoleIndex < ListEnemyPicture.Count; SetRoleIndex++)
                                    {
                                        if ((Front[0] + i * 5) == ListEnemyPicture[SetRoleIndex].RolePosition)
                                        {
                                            FrontType[0] = GetRepress(ListRolePicture[RoleIndex].RoleAttackType, ListEnemyPicture[SetRoleIndex].RoleAttackType);
                                            Enemy[0] = ListEnemyPicture[SetRoleIndex].RolePosition;
                                            IsFind = true;
                                            break;
                                        }
                                    }
                                    if (IsFind)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        case 2:
                            if (Front[1] < ListRolePicture[RoleIndex].RolePosition)
                            {
                                Front[1] = ListRolePicture[RoleIndex].RolePosition;
                                FrontType[1] = 0;
                                for (int i = 1; i <= 6; i++)
                                {
                                    for (int SetRoleIndex = 0; SetRoleIndex < ListEnemyPicture.Count; SetRoleIndex++)
                                    {
                                        if ((Front[1] + i * 5) == ListEnemyPicture[SetRoleIndex].RolePosition)
                                        {
                                            FrontType[1] = GetRepress(ListRolePicture[RoleIndex].RoleAttackType, ListEnemyPicture[SetRoleIndex].RoleAttackType);
                                            Enemy[1] = ListEnemyPicture[SetRoleIndex].RolePosition;
                                            IsFind = true;
                                            break;
                                        }
                                    }
                                    if (IsFind)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        case 3:
                            if (Front[2] < ListRolePicture[RoleIndex].RolePosition)
                            {
                                Front[2] = ListRolePicture[RoleIndex].RolePosition;
                                FrontType[2] = 0;
                                for (int i = 1; i <= 6; i++)
                                {
                                    for (int SetRoleIndex = 0; SetRoleIndex < ListEnemyPicture.Count; SetRoleIndex++)
                                    {
                                        if ((Front[2] + i * 5) == ListEnemyPicture[SetRoleIndex].RolePosition)
                                        {
                                            FrontType[2] = GetRepress(ListRolePicture[RoleIndex].RoleAttackType, ListEnemyPicture[SetRoleIndex].RoleAttackType);
                                            Enemy[2] = ListEnemyPicture[SetRoleIndex].RolePosition;
                                            IsFind = true;
                                            break;
                                        }
                                    }
                                    if (IsFind)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        case 4:
                            if (Front[3] < ListRolePicture[RoleIndex].RolePosition)
                            {
                                Front[3] = ListRolePicture[RoleIndex].RolePosition;
                                FrontType[3] = 0;
                                for (int i = 1; i <= 6; i++)
                                {
                                    for (int SetRoleIndex = 0; SetRoleIndex < ListEnemyPicture.Count; SetRoleIndex++)
                                    {
                                        if ((Front[3] + i * 5) == ListEnemyPicture[SetRoleIndex].RolePosition)
                                        {
                                            FrontType[3] = GetRepress(ListRolePicture[RoleIndex].RoleAttackType, ListEnemyPicture[SetRoleIndex].RoleAttackType);
                                            Enemy[3] = ListEnemyPicture[SetRoleIndex].RolePosition;
                                            IsFind = true;
                                            break;
                                        }
                                    }
                                    if (IsFind)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        case 0:
                            if (Front[4] < ListRolePicture[RoleIndex].RolePosition)
                            {
                                Front[4] = ListRolePicture[RoleIndex].RolePosition;
                                FrontType[4] = 0;
                                for (int i = 1; i <= 6; i++)
                                {
                                    for (int SetRoleIndex = 0; SetRoleIndex < ListEnemyPicture.Count; SetRoleIndex++)
                                    {
                                        if ((Front[4] + i * 5) == ListEnemyPicture[SetRoleIndex].RolePosition)
                                        {
                                            FrontType[4] = GetRepress(ListRolePicture[RoleIndex].RoleAttackType, ListEnemyPicture[SetRoleIndex].RoleAttackType);
                                            Enemy[4] = ListEnemyPicture[SetRoleIndex].RolePosition;
                                            IsFind = true;
                                            break;
                                        }
                                    }
                                    if (IsFind)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            fw.Repress.SetActive(true);
            for (int i = 0; i < 5; i++)
            {
                switch (FrontType[i])
                {
                    case 0:
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.SetActive(false);
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.SetActive(true);
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.GetComponent<UISprite>().spriteName = "arrow61";
                        ////fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().width = 50 + (Enemy[i] - Front[i]) * (20 + i * 3);
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().fillAmount = (0 + (Enemy[i] - Front[i]) * (20 + i * 3)) / 1000f;
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().invert = true;
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().spriteName = "arrow6";
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").localPosition = new Vector3(((Enemy[i] + Front[i]) / 2f - Center[i]) * (24 + i * 3) - (500 - (0 + (Enemy[i] - Front[i]) * (20 + i * 3)) / 2f), 0, 0);
                        break;
                    case 1:
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.SetActive(true);
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.GetComponent<UISprite>().spriteName = "arrow51";
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().width = 50 + (Enemy[i] - Front[i]) * (20 + i * 3);
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().fillAmount = (0 + (Enemy[i] - Front[i]) * (20 + i * 3)) / 1000f;
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().invert = true;
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().spriteName = "arrow5";
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").localPosition = new Vector3(((Enemy[i] + Front[i]) / 2f - Center[i]) * (24 + i * 3) - (500 - (0 + (Enemy[i] - Front[i]) * (20 + i * 3)) / 2f), 0, 0);
                        break;
                    case 2:
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.SetActive(true);
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.GetComponent<UISprite>().spriteName = "arrow71";
                        //fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().width = 50 + (Enemy[i] - Front[i]) * (20 + i * 3);
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().fillAmount = (0 + (Enemy[i] - Front[i]) * (20 + i * 3)) / 1000f;
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().invert = false;
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").gameObject.GetComponent<UISprite>().spriteName = "arrow7";
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString() + "/Arrow").localPosition = new Vector3(((Enemy[i] + Front[i]) / 2f - Center[i]) * (24 + i * 3) + (500 - (0 + (Enemy[i] - Front[i]) * (20 + i * 3)) / 2f), 0, 0);
                        break;
                    case 3:
                        fw.Repress.transform.Find("Repress" + (i + 1).ToString()).gameObject.SetActive(false);
                        break;
                }
            }
            ////////////////兵种相克(以上)//////////////////
        }
    }

    public int GetRepress(int RoleType, int EnemyType)
    {
        if (RoleType == EnemyType)
        {
            return 3;
        }
        else if (RoleType == 1 && EnemyType == 3)
        {
            return 1;
        }
        else if (RoleType == 3 && EnemyType == 2)
        {
            return 1;
        }
        else if (RoleType == 2 && EnemyType == 1)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public void ReSequence()
    {
        ListSequence.Clear();
        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            ListRolePicture[RoleIndex].RoleFightNowSpeed = ListRolePicture[RoleIndex].RoleFightSpeed;
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            ListEnemyPicture[RoleIndex].RoleFightNowSpeed = ListEnemyPicture[RoleIndex].RoleFightSpeed;
        }

        for (int i = 0; i < 3000; i++)
        {
            ListSequence.Add(GetLeastSequence());
        }
        StartCoroutine(ShowSequence(0));
    }
    int GetPositionCount()
    {
        int Count = ListEnemyPicture.Count;
        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            if (ListRolePicture[RoleIndex].RolePicturePointID == "Enemy" + NPCID.ToString())
            {
                continue;
            }

            if (ListRolePicture[RoleIndex].RolePosition > 0)
            {
                Count++;
            }
        }
        return Count;
    }
    void ResetSequence(int RoleIndex, bool IsMonster)
    {
        List<int> ListRemoveIndex = new List<int>();
        for (int i = ListSequence.Count - 1; i >= 0; i--)
        {
            if (ListSequence[i].RoleIndex == RoleIndex && ListSequence[i].IsMonster == IsMonster)
            {
                ListSequence.RemoveAt(i);
            }
        }
    }
    void InsertSequence(List<RolePicture> SetPicture, int RoleIndex, bool IsMonster)
    {
        int InsertLimit = 30;
        for (int j = 0; j < 100; j++)
        {
            SetPicture[RoleIndex].RoleFightNowSpeed += SetPicture[RoleIndex].RoleFightSpeed;
            for (int i = 0; i < ListSequence.Count - 1; i++)
            {
                if (ListSequence[i].NowSpeed <= SetPicture[RoleIndex].RoleFightNowSpeed && SetPicture[RoleIndex].RoleFightNowSpeed <= ListSequence[i + 1].NowSpeed)
                {
                    if (ListSequence[i].RoleIndex != RoleIndex || ListSequence[i].IsMonster != IsMonster)
                    {
                        InsertLimit--;
                        RoleSequence LeastSequence = new RoleSequence();
                        LeastSequence.RoleID = SetPicture[RoleIndex].RoleID;
                        LeastSequence.RoleIndex = RoleIndex;
                        LeastSequence.IsMonster = IsMonster;
                        ListSequence.Insert(i + 1, LeastSequence);
                        break;
                    }
                }
            }
            InsertLimit--;
            if (ListSequence.Count > 3000 || InsertLimit <= 0)
            {
                break;
            }
        }
    }
    IEnumerator ShowSequence(int AddNum)
    {
        SequenceTexture[7].gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        SequenceTexture[0].gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        for (int i = 0; i < 8; i++)
        {
            SequenceTexture[i].spriteName = ListSequence[i + AddNum].RoleID.ToString();
            if (ListSequence[i + AddNum].IsMonster)
            {
                SequenceSprite[i].spriteName = "yxkuang2";
            }
            else
            {
                SequenceSprite[i].spriteName = "yxkuang1";
            }
            if (IsFight)
            {
                SequenceTexture[i].gameObject.transform.position += new Vector3(0, 0.16f, 0);
            }
        }
        yield return new WaitForEndOfFrame();
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                //
                if (IsFight)
                {
                    SequenceTexture[i].gameObject.transform.position -= new Vector3(0, 0.032f, 0);
                }
            }
            SequenceTexture[0].gameObject.transform.localScale += new Vector3(0.04f, 0.04f, 0.04f);
            yield return new WaitForSeconds(0.01f);
        }
        SequenceTexture[7].gameObject.SetActive(true);
    }
    public void AddSequence()
    {
        ListCloseNode.Clear();
        ListOpenNode.Clear();
        ListFindNode.Clear();
        FightTimer = 0;
        IsCombineSkill = false;

        //Debug.LogError(ListSequence.Count + " " + ListSequence[0].RoleID);

        if (ListSequence.Count > 0)
        {
            if (ListSequence[0].IsMonster)
            {
                if (ListEnemyPicture.Count > ListSequence[0].RoleIndex)
                {
                    ListEnemyPicture[ListSequence[0].RoleIndex].RoleMoveNowStep = ListEnemyPicture[ListSequence[0].RoleIndex].RoleMoveStep;
                }
            }
            else
            {
                if (ListRolePicture.Count > ListSequence[0].RoleIndex)
                {
                    ListRolePicture[ListSequence[0].RoleIndex].RoleMoveNowStep = ListRolePicture[ListSequence[0].RoleIndex].RoleMoveStep;
                    ShowContinueKill(ListSequence[0].RoleIndex);
                    ListRolePicture[ListSequence[0].RoleIndex].RoleContinueKill = 0;
                }
            }

            for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
            {
                if (GameObject.Find("Position" + i.ToString()) != null)
                {
                    GameObject.Find("Position" + i.ToString()).renderer.material = BPositionMaterial;
                }
            }

            PictureCreater.instance.AttackCount = 0;
            ChangeSequence(0);


            ///////////////////////关卡剧情对话///////////////////////

            List<FightTalk> _ListFightTalk = TextTranslator.instance.GetFightTalkByGateID(SceneTransformer.instance.NowGateID, NowSequence);
            if (_ListFightTalk.Count == 0)
            {

            }
            else
            {
                foreach (var t in _ListFightTalk)
                {
                    if (t.TalkKind == 1)
                    {
                        foreach (var r in ListRolePicture)
                        {
                            if (r.RoleID == t.RoleID)
                            {
                                r.RoleRedBloodObject.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().LabelTalk.text = t.Talk;
                                if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
                                {

                                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 6);
                                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 8);
                                    LuaDeliver.instance.UseGuideStation();
                                    IsLock = true;
                                    //LuaDeliver.GuideIsLock();        
                                    StartCoroutine(CloseFightTalk(r, 5f));
                                }
                                else
                                {
                                    StartCoroutine(CloseFightTalk(r, 4f));
                                }
                                break;
                            }
                        }
                    }
                    else if (t.TalkKind == 2)
                    {
                        foreach (var r in ListEnemyPicture)
                        {
                            if (r.RoleID == t.RoleID)
                            {
                                r.RoleRedBloodObject.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(true);
                                r.RoleObject.GetComponent<ColliderDisplayText>().LabelTalk.text = t.Talk;
                                StartCoroutine(CloseFightTalk(r, 4f));
                                break;
                            }
                        }
                    }
                }
            }
            ///////////////////////关卡剧情对话///////////////////////
        }
    }

    IEnumerator CloseFightTalk(RolePicture SetPicture, float DelaySec)
    {
        yield return new WaitForSeconds(DelaySec);
        SetPicture.RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.SetActive(false);
    }

    void ShowContinueKill(int SetRoleIndex)
    {
        switch (ListRolePicture[SetRoleIndex].RoleContinueKill)
        {
            case 1:
                if (!IsFirstBlood)
                {
                    foreach (NcCurveAnimation c in fw.Kill1.GetComponentsInChildren(typeof(NcCurveAnimation), true))
                    {
                        c.ResetAnimation();
                    }
                    fw.Kill1.SetActive(true);

                    StartCoroutine(fw.ShowHeroSkill(ListRolePicture[SetRoleIndex].RoleID.ToString()));
                    AudioEditer.instance.PlayOneShot("FirstBlood");
                }
                IsFirstBlood = true;
                break;
            case 2:
                IsFirstBlood = true;

                foreach (NcCurveAnimation c in fw.Kill2.GetComponentsInChildren(typeof(NcCurveAnimation), true))
                {
                    c.ResetAnimation();
                }
                fw.Kill2.SetActive(true);
                StartCoroutine(fw.ShowHeroSkill(ListRolePicture[SetRoleIndex].RoleID.ToString()));
                AudioEditer.instance.PlayOneShot("DoubleKill");
                break;
            case 3:
                IsFirstBlood = true;

                foreach (NcCurveAnimation c in fw.Kill3.GetComponentsInChildren(typeof(NcCurveAnimation), true))
                {
                    c.ResetAnimation();
                }
                fw.Kill3.SetActive(true);
                StartCoroutine(fw.ShowHeroSkill(ListRolePicture[SetRoleIndex].RoleID.ToString()));
                AudioEditer.instance.PlayOneShot("TripleKill");
                break;
            case 4:

                foreach (NcCurveAnimation c in fw.Kill4.GetComponentsInChildren(typeof(NcCurveAnimation), true))
                {
                    c.ResetAnimation();
                }
                IsFirstBlood = true;
                fw.Kill4.SetActive(true);
                StartCoroutine(fw.ShowHeroSkill(ListRolePicture[SetRoleIndex].RoleID.ToString()));
                AudioEditer.instance.PlayOneShot("QuatreKill");
                break;
            case 5:

                foreach (NcCurveAnimation c in fw.Kill5.GetComponentsInChildren(typeof(NcCurveAnimation), true))
                {
                    c.ResetAnimation();
                }
                IsFirstBlood = true;
                fw.Kill5.SetActive(true);
                StartCoroutine(fw.ShowHeroSkill(ListRolePicture[SetRoleIndex].RoleID.ToString()));
                AudioEditer.instance.PlayOneShot("PentaKill");
                break;
            case 6:

                foreach (NcCurveAnimation c in fw.Kill6.GetComponentsInChildren(typeof(NcCurveAnimation), true))
                {
                    c.ResetAnimation();
                }
                IsFirstBlood = true;
                fw.Kill6.SetActive(true);
                StartCoroutine(fw.ShowHeroSkill(ListRolePicture[SetRoleIndex].RoleID.ToString()));
                AudioEditer.instance.PlayOneShot("PentaKill");
                break;

        }
        ListRolePicture[SetRoleIndex].RoleContinueKill = 0;
    }

    public void ChangeSequence(int AddNum)
    {
        NowSequence++;

        ///////////////敌方指挥官技能////////////////
        if (ListNPCTactics.Count > 0)
        {
            if (NowSequence == ListNPCTactics[0].goNum && NowSequence != 0)
            {
                IsEnemyLock = true;
                StartCoroutine(FireAutoSkill(ListNPCTactics[0].manualSkillID, ListEnemyPicture, ListRolePicture));
                ListNPCTactics.RemoveAt(0);
            }
        }
        ///////////////敌方指挥官技能////////////////

        if (NowSequence > LimitLose)
        {
            IsFight = false;
            StartCoroutine(ShowLose());
        }
        else if (NowSequence > LimitWin)
        {
            IsFight = false;
            StartCoroutine(ShowWin(ListRolePicture));
        }
        else
        {
            if (LimitLose < 200)
            {
                fw.LabelCombo.SetActive(true);
                fw.LabelCombo.transform.Find("LabelStep").gameObject.GetComponent<UILabel>().text = NowSequence.ToString();
                if (LimitLose - NowSequence < 6)
                {
                    fw.LabelCombo.transform.Find("LabelStep").gameObject.GetComponent<UILabel>().color = Color.red;
                    fw.LabelCombo.transform.Find("LabelStep").localPosition = new Vector3(-80, 0, 0);
                    fw.LabelCombo.transform.Find("LabelStep").GetComponent<TweenScale>().ResetToBeginning();
                    fw.LabelCombo.transform.Find("LabelStep").GetComponent<TweenScale>().enabled = true;
                }
                fw.LabelCombo.transform.Find("LabelLimit").gameObject.GetComponent<UILabel>().text = "/" + LimitLose.ToString();
            }
            else if (LimitWin < 999)
            {
                fw.LabelCombo.SetActive(true);
                fw.LabelCombo.transform.Find("LabelStep").gameObject.GetComponent<UILabel>().text = NowSequence.ToString();
                if (LimitWin - NowSequence < 6)
                {
                    fw.LabelCombo.transform.Find("LabelStep").gameObject.GetComponent<UILabel>().color = Color.red;
                    fw.LabelCombo.transform.Find("LabelStep").localPosition = new Vector3(-80, 0, 0);
                    fw.LabelCombo.transform.Find("LabelStep").GetComponent<TweenScale>().enabled = true;
                }
                fw.LabelCombo.transform.Find("LabelLimit").gameObject.GetComponent<UILabel>().text = "/" + LimitWin.ToString();
            }
            else if (LimitStar3 == 3)
            {
                fw.LabelCombo.SetActive(true);
                fw.LabelCombo.transform.Find("LabelStep").gameObject.GetComponent<UILabel>().text = NowSequence.ToString();
                if (LimitStarCount3 - NowSequence < 6)
                {
                    fw.LabelCombo.transform.Find("LabelStep").gameObject.GetComponent<UILabel>().color = Color.red;
                    fw.LabelCombo.transform.Find("LabelStep").localPosition = new Vector3(-80, 0, 0);
                    fw.LabelCombo.transform.Find("LabelStep").GetComponent<TweenScale>().enabled = true;
                }
                fw.LabelCombo.transform.Find("LabelLimit").gameObject.GetComponent<UILabel>().text = "/" + LimitStarCount3.ToString();
            }
#if !ClashRoyale
            if (ListSequence[0].IsMonster)
            {
                ListEnemyPicture[ListSequence[0].RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 2);
            }
            else
            {
                ListRolePicture[ListSequence[0].RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 2);
            }
#endif
            ListSequence.RemoveAt(0);


            if (NetworkHandler.instance.IsCreate)
            {
                if (ListSequence[0].RoleID == 60030)
                {
                    if (ListEnemyPicture[ListSequence[0].RoleIndex].RoleSkillPoint < 2000)
                    {
                        PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 4);
                        LuaDeliver.instance.UseGuideStation();
                        LuaDeliver.AudioNewGuide("Story03");
                        IsLock = true;

                    }
                }
            }

            for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
            {
                ListRolePicture[RoleIndex].RolePictureStartAttack = false;
            }

            for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
            {
                ListEnemyPicture[RoleIndex].RolePictureStartAttack = false;
            }

            SetListPosition(PictureCreater.instance.PositionRow, 2);

            //if (ListSequence[0].IsMonster)
            //{
            //    if (GameObject.Find("Position" + ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition.ToString()) != null)
            //    {
            //        GameObject.Find("Position" + ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition.ToString()).renderer.material.mainTexture = Resources.Load("Game/green", typeof(Texture)) as Texture;
            //        //ShowPosition(ListEnemyPicture[ListSequence[0].RoleIndex].RoleObject.name, ListPosition[ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition], ListEnemyPicture[ListSequence[0].RoleIndex].RolePosition, true);
            //    }
            //}
            //else
            //{
            //    if (GameObject.Find("Position" + ListRolePicture[ListSequence[0].RoleIndex].RolePosition.ToString()) != null)
            //    {
            //        GameObject.Find("Position" + ListRolePicture[ListSequence[0].RoleIndex].RolePosition.ToString()).renderer.material.mainTexture = Resources.Load("Game/green", typeof(Texture)) as Texture;
            //        //ShowPosition(ListRolePicture[ListSequence[0].RoleIndex].RoleObject.name, ListPosition[ListRolePicture[ListSequence[0].RoleIndex].RolePosition], ListRolePicture[ListSequence[0].RoleIndex].RolePosition, true);
            //        //ListRolePicture[ListSequence[0].RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 0);
            //    }
            //}

            if (ListSequence[0].IsMonster)
            {
                if (ListEnemyPicture.Count > ListSequence[0].RoleIndex)
                {
                    ResetRoleBuff(ListEnemyPicture, ListSequence[0].RoleIndex);
                    //CalculateRoleBuff(ListEnemyPicture, ListSequence[0].RoleIndex);
                    if (ListEnemyPicture[ListSequence[0].RoleIndex].BuffStop)
                    {
                        AddSequence();
                    }
                }

            }
            else
            {
                if (ListRolePicture.Count > ListSequence[0].RoleIndex)
                {
                    ResetRoleBuff(ListRolePicture, ListSequence[0].RoleIndex);
                    //CalculateRoleBuff(ListRolePicture, ListSequence[0].RoleIndex);
                    if (ListRolePicture[ListSequence[0].RoleIndex].BuffStop)
                    {
                        AddSequence();
                    }
                }
            }
            StartCoroutine(ShowSequence(AddNum));
        }

        if (FightStyle == 2)
        {
            WoodSequence();
        }
    }

    public void ShowXingDong()
    {
        if (!ListRolePicture[ListSequence[0].RoleIndex].IsPicture)
        {
            if (GameObject.Find("WalkObject") == null)
            {
                ListRolePicture[ListSequence[0].RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("ft", 0);

                GameObject Walk = GameObject.Instantiate(Resources.Load("Prefab/Effect/XingDong", typeof(GameObject))) as GameObject;
                Walk.transform.parent = ListRolePicture[ListSequence[0].RoleIndex].RoleObject.transform;
                Walk.transform.localPosition = Vector3.zero;
                Walk.name = "WalkObject";
            }
        }
    }

    public void WoodSequence()
    {
        if (!ListSequence[0].IsMonster)
        {
            if (IsHand)
            {
                IsLock = true;
                IsSkill = false;
            }
            else
            {
                IsLock = false;
                IsSkill = true;
            }
            if (!ListRolePicture[ListSequence[0].RoleIndex].IsPicture)
            {
                ShowXingDong();

                if (ListRolePicture[ListSequence[0].RoleIndex].RoleArea == 1022) //丛林显示血量
                {
                    for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                    {
                        if (ListRolePicture[RoleIndex].RolePosition > 0 && ListRolePicture[RoleIndex].RoleNowBlood > 0)
                        {
                            ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                        }
                    }

                    for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
                    {
                        ListEnemyPicture[RoleIndex].RoleRedBloodObject.SetActive(false);
                    }
                }
                else
                {
                    for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
                    {
                        if (ListEnemyPicture[RoleIndex].RoleNowBlood > 0)
                        {
                            ListEnemyPicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                        }
                    }

                    for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                    {
                        if (ListSequence[0].RoleIndex != RoleIndex)
                        {
                            ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(false);
                        }
                    }
                }
            }
            ListRolePicture[ListSequence[0].RoleIndex].RoleRedBloodObject.SetActive(true);
            ListRolePicture[ListSequence[0].RoleIndex].RoleTargetIndex = -1;

            if (ListRolePicture[ListSequence[0].RoleIndex].RoleSkillPoint >= 1000)
            {
                fw.HandSkillButton2.GetComponent<UISprite>().spriteName = "buttonskill";
                fw.HandSkillEffect.SetActive(true);
            }
            else
            {
                fw.HandSkillButton2.GetComponent<UISprite>().spriteName = "buttonskillan";
                fw.HandSkillEffect.SetActive(false);
            }

            MyBases.SetActive(true);

            for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
            {
                if (ListMove[i] != null)
                {
                    ListMove[i].renderer.material = BlankMaterial;
                }
            }

            StartCoroutine(ShowTarget(ListRolePicture, ListSequence[0].RoleIndex, ListRolePicture[ListSequence[0].RoleIndex].RolePosition, true, false));
        }
        else
        {
            IsLock = false;
            IsSkill = true;
        }
    }

    RoleSequence GetLeastSequence()
    {
        RoleSequence LeastSequence = new RoleSequence();
        LeastSequence.IsMonster = true;
        float MaxTimer = 99999;
        float MaxForce = 0;

        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            if (ListRolePicture[RoleIndex].RolePicturePointID == "Enemy" + NPCID.ToString())
            {
                continue;
            }

            if (ListRolePicture[RoleIndex].RoleFightNowSpeed <= MaxTimer && ListRolePicture[RoleIndex].RolePictureAttackable && ListRolePicture[RoleIndex].RolePosition > 0)
            {
                if (ListRolePicture[RoleIndex].RoleFightNowSpeed == MaxTimer)
                {
                    if (ListRolePicture[RoleIndex].RoleForce >= MaxForce)
                    {
                        MaxTimer = ListRolePicture[RoleIndex].RoleFightNowSpeed;
                        MaxForce = ListRolePicture[RoleIndex].RoleForce;
                        LeastSequence.RoleIndex = RoleIndex;
                        LeastSequence.RoleID = ListRolePicture[RoleIndex].RoleID;
                        LeastSequence.IsMonster = false;
                    }
                }
                else
                {
                    MaxTimer = ListRolePicture[RoleIndex].RoleFightNowSpeed;
                    MaxForce = ListRolePicture[RoleIndex].RoleForce;
                    LeastSequence.RoleIndex = RoleIndex;
                    LeastSequence.RoleID = ListRolePicture[RoleIndex].RoleID;
                    LeastSequence.IsMonster = false;
                }
            }
        }

        for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
        {
            if (ListEnemyPicture[RoleIndex].RoleFightNowSpeed <= MaxTimer && ListEnemyPicture[RoleIndex].RolePictureAttackable && ListEnemyPicture[RoleIndex].RolePosition > 0)
            {
                if (ListEnemyPicture[RoleIndex].RoleFightNowSpeed == MaxTimer)
                {
                    if (ListEnemyPicture[RoleIndex].RoleForce >= MaxForce)
                    {
                        MaxTimer = ListEnemyPicture[RoleIndex].RoleFightNowSpeed;
                        MaxForce = ListEnemyPicture[RoleIndex].RoleForce;
                        LeastSequence.RoleIndex = RoleIndex;
                        LeastSequence.RoleID = ListEnemyPicture[RoleIndex].RoleID;
                        LeastSequence.IsMonster = true;
                    }
                }
                else
                {
                    MaxTimer = ListEnemyPicture[RoleIndex].RoleFightNowSpeed;
                    MaxForce = ListEnemyPicture[RoleIndex].RoleForce;
                    LeastSequence.RoleIndex = RoleIndex;
                    LeastSequence.RoleID = ListEnemyPicture[RoleIndex].RoleID;
                    LeastSequence.IsMonster = true;
                }
            }
        }

        if (LeastSequence.IsMonster)
        {
            if (ListEnemyPicture.Count > LeastSequence.RoleIndex)
            {
                float FightSpeed = ListEnemyPicture[LeastSequence.RoleIndex].RoleFightSpeed;
                if (LeastSequence.RoleIndex == MyCaptainIndex && FightStyle != 2)
                {
                    FightSpeed = 300 - ListEnemyPicture[LeastSequence.RoleIndex].RoleFightSpeed;
                    //速度   9队长提速
                    Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(9, ListEnemyPicture[LeastSequence.RoleIndex].RoleInnate[8]);
                    if (SetInnate != null)
                    {
                        FightSpeed *= (SetInnate.Value1 / 100f);
                    }
                    FightSpeed = 300 - FightSpeed;
                }
                if (IsMySpeedUp)
                {
                    FightSpeed -= 5;
                }

                ListEnemyPicture[LeastSequence.RoleIndex].RoleFightNowSpeed += FightSpeed;
                LeastSequence.NowSpeed = ListEnemyPicture[LeastSequence.RoleIndex].RoleFightNowSpeed;
            }
        }
        else
        {
            if (ListRolePicture.Count > LeastSequence.RoleIndex)
            {
                float FightSpeed = ListRolePicture[LeastSequence.RoleIndex].RoleFightSpeed;
                if (LeastSequence.RoleIndex == MyCaptainIndex && FightStyle != 2)
                {
                    FightSpeed = 300 - ListRolePicture[LeastSequence.RoleIndex].RoleFightSpeed;
                    //速度   9队长提速
                    Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(9, ListRolePicture[LeastSequence.RoleIndex].RoleInnate[8]);
                    if (SetInnate != null)
                    {
                        FightSpeed *= (SetInnate.Value1 / 100f);
                    }
                    FightSpeed = 300 - FightSpeed;
                }
                if (IsEnemySpeedUp)
                {
                    FightSpeed -= 5;
                }

                ListRolePicture[LeastSequence.RoleIndex].RoleFightNowSpeed += FightSpeed;
                LeastSequence.NowSpeed = ListRolePicture[LeastSequence.RoleIndex].RoleFightNowSpeed;
            }
        }
        return LeastSequence;
    }
    #endregion

    #region Handle

    public bool CheckAutoHandle()
    {
        if (FightStyle == 0 && !fw.TacticsInfo.activeSelf)
        {
            return true;
        }
        return false;
    }



    public void UpdateTargetObject(int SetRoleIndex)
    {
        if (ListRolePicture[SetRoleIndex].RoleTargetObject.activeSelf)
        {
            if (ListRolePicture[SetRoleIndex].RoleTargetIndex > -1)
            {
                if (ListRolePicture[SetRoleIndex].RoleTargetMonster)
                {
                    if (ListEnemyPicture[ListRolePicture[SetRoleIndex].RoleTargetIndex].RoleNowBlood > 0)
                    {
                        ListRolePicture[SetRoleIndex].RoleMovePosition = ListEnemyPicture[ListRolePicture[SetRoleIndex].RoleTargetIndex].RolePosition;
                    }
                }
                else
                {
                    if (ListRolePicture[ListRolePicture[SetRoleIndex].RoleTargetIndex].RoleNowBlood > 0)
                    {
                        ListRolePicture[SetRoleIndex].RoleMovePosition = ListRolePicture[ListRolePicture[SetRoleIndex].RoleTargetIndex].RolePosition;
                    }
                }
            }
            if (ListRolePicture[SetRoleIndex].RoleMovePosition == 0)
            {
                if (ListRolePicture[SetRoleIndex].RoleTargetObject.activeSelf)
                {
                    ListRolePicture[SetRoleIndex].RoleTargetIndex = -1;
                    ListRolePicture[SetRoleIndex].RoleTargetObject.SetActive(false);
                }
            }
            else
            {
                float MoveDistance = Vector3.Distance(ListRolePicture[SetRoleIndex].RoleObject.transform.position, ListPosition[ListRolePicture[SetRoleIndex].RoleMovePosition]);
                ListRolePicture[SetRoleIndex].RoleTargetObject.transform.position = ListRolePicture[SetRoleIndex].RoleObject.transform.position + (ListPosition[ListRolePicture[SetRoleIndex].RoleMovePosition] - ListRolePicture[SetRoleIndex].RoleObject.transform.position) / 2f + new Vector3(0, 0.1f, 0);

                ListRolePicture[SetRoleIndex].RoleTargetObject.transform.LookAt(ListPosition[ListRolePicture[SetRoleIndex].RoleMovePosition] + new Vector3(0, 0.1f, 0));
                ListRolePicture[SetRoleIndex].RoleTargetObject.transform.Rotate(90, 0, 0);
                ListRolePicture[SetRoleIndex].RoleTargetObject.transform.localScale = new Vector3(ListRolePicture[SetRoleIndex].RoleTargetObject.transform.localScale.x, MoveDistance, ListRolePicture[SetRoleIndex].RoleTargetObject.transform.localScale.z);
            }
        }
    }

    public void SetMoveTarget(int SetRoleIndex, int MoveTarget, bool IsTarget, bool IsEnemy, bool IsUp)
    {
        if (SetRoleIndex < ListRolePicture.Count)
        {
            ListRolePicture[SetRoleIndex].RoleTargetObject.SetActive(true);
            if (IsTarget)
            {
                if (ListRolePicture[SetRoleIndex].RolePosition == MoveTarget)
                {
                    ListRolePicture[SetRoleIndex].RoleMovePosition = 0;
                    if (ListRolePicture[SetRoleIndex].RoleTargetObject.activeSelf)
                    {
                        ListRolePicture[SetRoleIndex].RoleTargetIndex = -1;
                        ListRolePicture[SetRoleIndex].RoleTargetObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (MoveTarget == 0)
                {
                    IsLock = true;
                    if (!MyBases.activeSelf)
                    {
                        GameObject Walk = GameObject.Instantiate(Resources.Load("Prefab/Effect/XingDongDian", typeof(GameObject))) as GameObject;
                        Walk.transform.parent = ListRolePicture[SetRoleIndex].RoleObject.transform;
                        Walk.transform.localPosition = Vector3.zero;
                        Walk.name = "HandleObject";
                    }
                    MyBases.SetActive(true);
                    StartCoroutine(ShowTarget(ListRolePicture, SetRoleIndex, ListRolePicture[SetRoleIndex].RolePosition, true, false));
                }
                else
                {
                    if (ListRolePicture[SetRoleIndex].RolePosition != MoveTarget)
                    {
                        if (ListRolePicture[SetRoleIndex].RoleMovePosition != MoveTarget)
                        {
                            ListRolePicture[SetRoleIndex].RoleMovePosition = MoveTarget;
                            if (GameObject.Find("HandleObject") != null)
                            {
                                GameObject.Find("HandleObject").transform.position = ListPosition[MoveTarget];
                                GameObject.Find("HandleObject").transform.Find("XingDong03").gameObject.SetActive(true);
                                GameObject.Find("HandleObject").transform.Find("XingDong04").gameObject.SetActive(false);

                                for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
                                {
                                    if (ListEnemyPicture[RoleIndex].RolePosition == MoveTarget && ListEnemyPicture[RoleIndex].RoleNowBlood > 0)
                                    {
                                        GameObject.Find("HandleObject").transform.Find("XingDong03").gameObject.SetActive(false);
                                        GameObject.Find("HandleObject").transform.Find("XingDong04").gameObject.SetActive(true);
                                    }
                                }
                            }
                            PictureCreater.instance.UpdateTargetObject(SetRoleIndex);
                        }
                    }
                    else
                    {
                        if (ListRolePicture[SetRoleIndex].RoleTargetObject.activeSelf)
                        {
                            ListRolePicture[SetRoleIndex].RoleTargetIndex = -1;
                            ListRolePicture[SetRoleIndex].RoleTargetObject.SetActive(false);
                        }
                    }
                }
            }

            if (IsUp)
            {
                IsLock = false;
                MyBases.SetActive(false);
                DestroyImmediate(GameObject.Find("HandleObject"));
                for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
                {
                    if (ListMove[i] != null)
                    {
                        ListMove[i].renderer.material = BlankMaterial;
                    }
                }
            }
        }
    }
    public void SetMovePosition(int MovePosition)
    {
        if (!ListSequence[0].IsMonster)
        {
            //Debug.LogError(ListSequence[0].RoleIndex + " " + ListSequence[0].IsMonster + " " + MovePosition);
            IsLock = false;
            MyBases.SetActive(false);
            fw.HandCancelButton.SetActive(false);
            ListRolePicture[ListSequence[0].RoleIndex].RoleMovePosition = MovePosition;

            for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
            {
                if (ListMove[i] != null)
                {
                    ListMove[i].renderer.material = BlankMaterial;
                }
            }

            if (MovePosition == 0 && ListRolePicture[ListSequence[0].RoleIndex].RoleMoveNowStep == 0)
            {
                AddSequence();
            }
        }
    }
    public void SetTargetIndex(bool IsTargetMonster, int TargetIndex)
    {
        if (!ListSequence[0].IsMonster)
        {
            Debug.LogError(ListSequence[0].RoleIndex + " " + ListSequence[0].IsMonster + " " + TargetIndex);
            //IsLock = false;
            ListRolePicture[ListSequence[0].RoleIndex].RoleTargetIndex = TargetIndex;
            ListRolePicture[ListSequence[0].RoleIndex].RoleTargetMonster = IsTargetMonster;

            //if (ListSequence[0].RoleIndex == TargetIndex && !IsTargetMonster)
            //{
            //    ListRolePicture[ListSequence[0].RoleIndex].RoleSkillPoint += 2;
            //    ListRolePicture[ListSequence[0].RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(ListRolePicture[ListSequence[0].RoleIndex].RoleSkillPoint);
            //    AddSequence();
            //}
        }
    }

    public void SetRoleTargetIndex(int SetRoleIndex, bool IsTargetMonster, int TargetIndex)
    {
        //IsLock = false;
        if (SetRoleIndex == TargetIndex && !IsTargetMonster && ListRolePicture[SetRoleIndex].RoleArea != 1022)
        {

        }
        else
        {
            ListRolePicture[SetRoleIndex].RoleTargetIndex = TargetIndex;
            ListRolePicture[SetRoleIndex].RoleTargetMonster = IsTargetMonster;
        }
    }
    public void SetPosition(string ObjectName, int PositionID)
    {
        if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 13)
        {
            if (ObjectName == "Role60016" && PositionID != 0)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_707);

                PositionID = 18;
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 14);
                LuaDeliver.instance.UseGuideStation();
            }
        }
        else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6)
        {
            if (ObjectName == "Role60032" && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7 && PositionID != 17)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1005);
                PositionID = 8;
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 9);
                LuaDeliver.instance.UseGuideStation();
            }
            if (ObjectName == "Role60032" && PositionID != 17)
            {
                PositionID = 21;
            }
        }

        else if (CharacterRecorder.instance.GuideID[0] == 3 && SceneTransformer.instance.NowGateID == 10005)
        {
            if (ObjectName == "Role60028")
            {
                PositionID = 0;
                CharacterRecorder.instance.GuideID[0] += 1;
                StartCoroutine(SceneTransformer.instance.NewbieGuide());
            }
        }
        if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) < 13 && PositionID == 0)
        {
            for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
            {
                if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName)
                {
                    ListRolePicture[RoleIndex].RoleObject.transform.position = ListPosition[ListRolePicture[RoleIndex].RolePosition];
                    return;
                }
            }
        }

        bool IsSetCaptain = false;
        for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        {
            if (ListRolePicture[RoleIndex].RolePosition > 0)
            {
                IsSetCaptain = true;
                break;
            }
        }
        if (!IsSetCaptain)
        {
            PictureCreater.instance.SetCaptain(ObjectName);
            SelectPosition = PositionID;
        }
        IsSetCaptain = true;


        if (IsFight)
        {
            SetListPosition(PositionRow, 2);
        }
        else
        {
            SetListPosition(PositionRow, 1);
        }
        if (PositionID == 0)
        {
            for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
            {
                if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName && ListRolePicture[RoleIndex].RolePosition == SelectPosition)
                {
                    IsSetCaptain = false;
                    break;
                }
            }



            for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
            {
                if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName)
                {
                    ListRolePicture[RoleIndex].RolePosition = PositionID;
                    ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
                    foreach (var h in CharacterRecorder.instance.ownedHeroList)
                    {
                        if (h.characterRoleID == ListRolePicture[RoleIndex].RoleCharacterRoleID)
                        {
                            if (h.position > 0)
                            {
                                SelectName = "";
                                FightCount--;
                            }
                            else
                            {
                                ////ShowPosition(SelectName, Vector3.zero, SelectPosition, false);
                                for (int i = 0; i < ListRolePicture.Count; i++)
                                {
                                    //ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetButton(false);
                                    ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(false);
                                }
                                //for (int i = 0; i < ListRolePicture.Count; i++)
                                //{
                                //    if (ListRolePicture[i].RoleObject.name == SelectName)
                                //    {
                                //        ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetButton(true);
                                //        //ListRolePicture[i].RolePictureObject.GetComponent<Animator>().Play("skill");
                                //        break;
                                //    }
                                //}
                                UIManager.instance.OpenPromptWindow("请将英雄布置在我方蓝色区域", PromptWindow.PromptType.Hint, null, null);
                            }
                            h.position = PositionID;
                            fw.SetRoleVisible(h.cardID, true, (int)ListRolePicture[RoleIndex].RoleNowBlood, (int)ListRolePicture[RoleIndex].RoleMaxBlood, ListRolePicture[RoleIndex].RoleSkillPoint);
                            ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(false);
                            break;
                        }
                    }
                    ListRolePicture[RoleIndex].RoleObject.SetActive(false);
                    ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
                    break;
                }
            }


            if (!IsSetCaptain)
            {
                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                {
                    if (ListRolePicture[RoleIndex].RolePosition > 0)
                    {
                        PictureCreater.instance.SetCaptain(ListRolePicture[RoleIndex].RoleObject.name);
                        SelectPosition = ListRolePicture[RoleIndex].RolePosition;
                        break;
                    }
                }
            }
        }
        else
        {
            if (!CheckPosition(PositionID))
            {
                int NowIndex = -1;
                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                {
                    if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName)
                    {
                        NowIndex = RoleIndex;
                    }
                }
                bool IsLimit = false;
                int Limit = (LimitHeroCount > 0) ? LimitHeroCount : (CharacterRecorder.instance.level < 25 ? 5 : 6);
                int Count = 0;
                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                {
                    if (ListRolePicture[RoleIndex].RolePosition > 0 && NowIndex != RoleIndex && ListRolePicture[RoleIndex].RolePicturePointID.IndexOf("R") > -1)
                    {
                        Limit--;
                        Count++;
                    }
                    if (Limit <= 0)
                    {
                        IsLimit = true;

                        if (CharacterRecorder.instance.level < 25 && LimitHeroCount == 0)
                        {
                            UIManager.instance.OpenPromptWindow("25级开放上阵第6名英雄", PromptWindow.PromptType.Hint, null, null);
                        }
                        else
                        {
                            UIManager.instance.OpenPromptWindow("最多上阵" + ((LimitHeroCount > 0) ? LimitHeroCount : 6).ToString() + "名战士", PromptWindow.PromptType.Hint, null, null);
                        }
                    }
                }

                if (!IsLimit)
                {
                    for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                    {
                        if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName)
                        {
                            ListRolePicture[RoleIndex].RolePosition = PositionID;
                            ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(true);
                            foreach (var h in CharacterRecorder.instance.ownedHeroList)
                            {
                                if (h.characterRoleID == ListRolePicture[RoleIndex].RoleCharacterRoleID)
                                {
                                    PlayRoleSound(ListRolePicture[RoleIndex].RoleID);

                                    FightCount++;

                                    h.position = PositionID;
                                    SelectName = ObjectName;
                                    fw.SetRoleVisible(h.cardID, false, (int)ListRolePicture[RoleIndex].RoleNowBlood, (int)ListRolePicture[RoleIndex].RoleMaxBlood, ListRolePicture[RoleIndex].RoleSkillPoint);
                                    //if (Count < 4)
                                    //{
                                    //    SelectPosition = 11;
                                    //    while (CheckPosition(SelectPosition))
                                    //    {
                                    //        SelectPosition--;
                                    //    }
                                    //    ShowPosition("", Vector3.zero, SelectPosition, false);
                                    //}
                                    //else
                                    //{
                                    ShowPosition(ObjectName, Vector3.zero, PositionID, false);
                                    //}
                                    for (int i = 0; i < ListRolePicture.Count; i++)
                                    {
                                        //ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetButton(false);
                                        ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(false);
                                    }
                                    //ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetButton(true);

                                    if (!ListRolePicture[RoleIndex].RoleCaptain && FightStyle != 2)
                                    {
                                        ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(true);
                                    }
                                    else if (FightStyle == 2)
                                    {
                                        ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                                    }
                                    else
                                    {
                                        SelectPosition = ListRolePicture[RoleIndex].RolePosition;
                                    }
                                    StopAllCoroutines();
                                    //StartCoroutine(ShowAttack(RoleIndex));
                                    //StartCoroutine(ShowCaptain(RoleIndex));
                                    //StartCoroutine(ShowPath(RoleIndex));


                                    for (int NewRoleIndex = 0; NewRoleIndex < ListRolePicture.Count; NewRoleIndex++)
                                    {
                                        if (ListRolePicture[NewRoleIndex].RoleObject.name == ObjectName)
                                        {
                                            ListRolePicture[NewRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
                                            if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
                                            {
                                                StartCoroutine(ShowTarget(ListRolePicture, NewRoleIndex, PositionID, true, false));
                                            }
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                            ListRolePicture[RoleIndex].RoleObject.transform.position = ListPosition[ListRolePicture[RoleIndex].RolePosition];
                            break;
                        }
                    }
                }
                else
                {
                    for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                    {
                        if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName)
                        {
                            ListRolePicture[RoleIndex].RoleObject.transform.position = ListPosition[0];
                            SetListPosition(PositionRow, 1);
                            ShowPosition(SelectName, Vector3.zero, PositionID, false);
                            for (int i = 0; i < ListRolePicture.Count; i++)
                            {
                                //ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetButton(false);
                                ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(false);
                            }
                            for (int i = 0; i < ListRolePicture.Count; i++)
                            {
                                if (ListRolePicture[i].RoleObject.name == SelectName)
                                {
                                    if (!ListRolePicture[i].RoleCaptain && FightStyle != 2)
                                    {
                                        ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(true);
                                    }
                                    else if (FightStyle == 2)
                                    {
                                        ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                                    }
                                    //ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetButton(true);
                                    StopAllCoroutines();
                                    //StartCoroutine(ShowAttack(RoleIndex));
                                    //StartCoroutine(ShowCaptain(RoleIndex));
                                    //StartCoroutine(ShowPath(RoleIndex));
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                int SetRoleIndex = -1;
                bool IsChange = false;
                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                {
                    if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName)
                    {
                        SetRoleIndex = RoleIndex;
                        break;
                    }
                }

                for (int i = 0; i < ListRolePicture.Count; i++)
                {
                    ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(false);
                }

                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                {
                    if (ListRolePicture[RoleIndex].RolePosition == PositionID)
                    {
                        PlayRoleSound(ListRolePicture[SetRoleIndex].RoleID);


                        if (ListRolePicture[RoleIndex].RolePosition == SelectPosition && ListRolePicture[SetRoleIndex].RolePosition == 0)
                        {
                            PictureCreater.instance.SetCaptain(ListRolePicture[SetRoleIndex].RoleObject.name);
                            SelectPosition = ListRolePicture[RoleIndex].RolePosition;
                        }

                        ListRolePicture[RoleIndex].RolePosition = ListRolePicture[SetRoleIndex].RolePosition;
                        ListRolePicture[RoleIndex].RoleObject.transform.position = ListPosition[ListRolePicture[SetRoleIndex].RolePosition];
                        ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(true);
                        //ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetButton(false);
                        ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(false);
                        fw.SetRoleVisible(ListRolePicture[RoleIndex].RoleID, false, (int)ListRolePicture[RoleIndex].RoleNowBlood, (int)ListRolePicture[RoleIndex].RoleMaxBlood, ListRolePicture[RoleIndex].RoleSkillPoint);

                        foreach (var h in CharacterRecorder.instance.ownedHeroList)
                        {
                            if (h.cardID == ListRolePicture[RoleIndex].RoleID)
                            {
                                h.position = ListRolePicture[SetRoleIndex].RolePosition;
                                break;
                            }
                        }

                        if (ListRolePicture[SetRoleIndex].RolePosition == 0)
                        {
                            ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
                            fw.SetRoleVisible(ListRolePicture[RoleIndex].RoleID, true, (int)ListRolePicture[RoleIndex].RoleNowBlood, (int)ListRolePicture[RoleIndex].RoleMaxBlood, ListRolePicture[RoleIndex].RoleSkillPoint);
                        }

                        ListRolePicture[SetRoleIndex].RolePosition = PositionID;
                        ListRolePicture[SetRoleIndex].RoleObject.transform.position = ListPosition[PositionID];
                        ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(true);
                        //ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetButton(true);
                        if (!ListRolePicture[SetRoleIndex].RoleCaptain && FightStyle != 2)
                        {
                            ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(true);
                        }
                        else if (FightStyle == 2)
                        {
                            ListRolePicture[SetRoleIndex].RoleRedBloodObject.SetActive(true);
                        }
                        StopAllCoroutines();
                        //StartCoroutine(ShowAttack(RoleIndex));
                        //StartCoroutine(ShowCaptain(SetRoleIndex));
                        //StartCoroutine(ShowPath(RoleIndex));
                        fw.SetRoleVisible(ListRolePicture[SetRoleIndex].RoleID, false, (int)ListRolePicture[SetRoleIndex].RoleNowBlood, (int)ListRolePicture[SetRoleIndex].RoleMaxBlood, ListRolePicture[SetRoleIndex].RoleSkillPoint);

                        SelectName = ObjectName;
                        ShowPosition(ObjectName, Vector3.zero, PositionID, false);
                        IsChange = true;

                        foreach (var h in CharacterRecorder.instance.ownedHeroList)
                        {
                            if (h.cardID == ListRolePicture[SetRoleIndex].RoleID)
                            {
                                h.position = PositionID;
                                break;
                            }
                        }

                        for (int NewRoleIndex = 0; NewRoleIndex < ListRolePicture.Count; NewRoleIndex++)
                        {
                            if (ListRolePicture[NewRoleIndex].RoleObject.name == ObjectName)
                            {
                                ListRolePicture[NewRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
                                StartCoroutine(ShowTarget(ListRolePicture, NewRoleIndex, PositionID, true, false));
                                break;
                            }
                        }

                        break;
                    }
                }

                if (!IsChange)
                {
                    if (ListRolePicture[SetRoleIndex].RolePosition != 0)
                    {
                        ListRolePicture[SetRoleIndex].RoleObject.transform.position = ListPosition[ListRolePicture[SetRoleIndex].RolePosition];
                        ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(true);
                        fw.SetRoleVisible(ListRolePicture[SetRoleIndex].RoleID, false, (int)ListRolePicture[SetRoleIndex].RoleNowBlood, (int)ListRolePicture[SetRoleIndex].RoleMaxBlood, ListRolePicture[SetRoleIndex].RoleSkillPoint);

                        for (int NewRoleIndex = 0; NewRoleIndex < ListRolePicture.Count; NewRoleIndex++)
                        {
                            if (ListRolePicture[NewRoleIndex].RoleObject.name == ObjectName)
                            {
                                ListRolePicture[NewRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
                                StartCoroutine(ShowTarget(ListRolePicture, NewRoleIndex, PositionID, true, false));
                                break;
                            }
                        }
                    }
                    else
                    {
                        ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
                        ListRolePicture[SetRoleIndex].RoleObject.transform.position = ListPosition[ListRolePicture[SetRoleIndex].RolePosition];
                    }
                }
            }
        }
    }
    public void SetCaptain(string CaptainName)
    {
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetButton(false);
            ListRolePicture[i].RoleCaptain = false;
        }
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            if (ListRolePicture[i].RoleObject.name == CaptainName && ListRolePicture[i].RolePosition > 0)
            {
                ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetButton(true);
                ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().CaptainButton.SetActive(false);
                SelectPosition = ListRolePicture[i].RolePosition;
                ListRolePicture[i].RoleCaptain = true;
                StartCoroutine(ShowCaptain(i, ListRolePicture[i].RoleSkillPoint));
                break;
            }
        }
    }
    IEnumerator ShowPath(int SetRoleIndex)
    {

        yield return new WaitForSeconds(0.01f);
        if (MyPath == null)
        {
            MyPath = new GameObject("MyPath");
        }

        for (int i = 0; i < ListEnemyPicture.Count; i++)
        {
            List<int> TargetIndex = FindTarget(SetRoleIndex, ListEnemyPicture, ListRolePicture, 0, i, true);
            if (TargetIndex.Count > 0)
            {
                ListTargetPosition.Add(i);
            }
        }

        List<GameObject> ListPath = new List<GameObject>();
        int PathID = 0;
        int PositionID = ListRolePicture[SetRoleIndex].RolePosition;
        GameObject go = ListBase[PositionID];
        foreach (int j in ListTargetPosition)
        {
            float MoveDistance = Vector3.Distance(ListPosition[PositionID], ListEnemyPicture[j].RoleObject.transform.position);
            int MoveCount = (int)(MoveDistance * 2f);
            Vector3 UnitDistance = (ListPosition[PositionID] - ListEnemyPicture[j].RoleObject.transform.position) / MoveCount;
            for (int k = 1; k <= MoveCount - 2; k++)
            {
                GameObject TempObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                DestroyImmediate(TempObject.GetComponent("MeshCollider"));
                if (PathID % 2 == 0)
                {
                    TempObject.renderer.material.mainTexture = Resources.Load("Game/arrow", typeof(Texture)) as Texture;
                }
                else
                {
                    TempObject.renderer.material.mainTexture = Resources.Load("Game/arrow4", typeof(Texture)) as Texture;
                }
                TempObject.renderer.material.shader = Shader.Find("Unlit/Transparent Colored");
                TempObject.transform.localScale = new Vector3(0.3f, 0.5f, 0.5f);
                TempObject.transform.position = ListEnemyPicture[j].RoleObject.transform.position + UnitDistance * k + new Vector3(0, 0.5f, 0);
                TempObject.transform.LookAt(go.transform.position + new Vector3(0, 0.5f, 0));
                TempObject.transform.Rotate(90, 0, 0);
                TempObject.name = "Path";
                TempObject.transform.parent = MyPath.transform;
                ListPath.Add(TempObject);
                PathID++;
                //yield return new WaitForSeconds(0.02f);
            }
        }

        ListTargetPosition.Clear();
        int s = 0;
        while (true)
        {
            for (int i = 0; i < ListPath.Count; i++)
            {
                if (ListPath[i] != null)
                {
                    ListPath[i].SetActive(true);

                    if (i % 2 == s)
                    {
                        ListPath[i].renderer.material.mainTexture = Resources.Load("Game/arrow", typeof(Texture)) as Texture;
                    }
                    else
                    {
                        ListPath[i].renderer.material.mainTexture = Resources.Load("Game/arrow4", typeof(Texture)) as Texture;
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
            if (s == 0)
            {
                s = 1;
            }
            else
            {
                s = 0;
            }
        }
    }
    IEnumerator ShowCaptain(int SetRoleIndex, int NowSkillPoint)
    {
        for (int i = 0; i < ListRolePicture.Count; i++)
        {
            ListRolePicture[i].RoleRedBloodObject.SetActive(false);
            ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(0);
            ListRolePicture[i].RoleObject.GetComponent<ColliderDisplayText>().BuffCaptain.alpha = 0;
        }

        //for (int i = 0; i < 100; i++)
        //{
        //    ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().BuffCaptain.alpha += 0.01f;
        //    yield return new WaitForSeconds(0.01f);
        //}
        //ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().BuffCaptain.alpha = 0;
        yield return new WaitForSeconds(1.3f);
        ListRolePicture[SetRoleIndex].RoleRedBloodObject.SetActive(true);
        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.01f);
            ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(NowSkillPoint + 20 * i);
        }
        //yield return new WaitForSeconds(0.3f);
        //ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(NowSkillPoint + 1);
        //yield return new WaitForSeconds(0.3f);
        //ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(NowSkillPoint + 2);
        //yield return new WaitForSeconds(0.3f);
        //ListRolePicture[SetRoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(NowSkillPoint + 3);
        yield return new WaitForSeconds(3);
        ListRolePicture[SetRoleIndex].RoleRedBloodObject.SetActive(false);
        yield return 0;
    }
    public int ShowPosition(string ObjectName, Vector3 SetPosition, int PositionID, bool IsShow)
    {
        if (PositionID == 0)
        {
            RaycastHit[] RaycastHitObject;
            RaycastHitObject = Physics.RaycastAll(SetPosition + new Vector3(0, 50, 0), new Vector3(0, -100, 0), 100);

            for (int i = 0; i < RaycastHitObject.Length; i++)
            {
                if (RaycastHitObject[i].transform.name.IndexOf("Position") > -1)
                {
                    PositionID = int.Parse(RaycastHitObject[i].transform.name.Replace("Position", ""));
                    break;
                }
            }
        }

        //MyPosition.transform.position = ListPosition[PositionID] + new Vector3(0, 0.03f, 0);

        SetListPosition(PositionRow, 2);


        //////////////////为了优化先拿掉（以下）//////////////////
        //if (IsShow)
        //{
        //    for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
        //    {
        //        if (ListRolePicture[RoleIndex].RoleObject.name == ObjectName)
        //        {
        //            ListRolePicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteNum.SetActive(false);
        //            StartCoroutine(ShowTarget(ListRolePicture, RoleIndex, PositionID, false, false));
        //        }
        //    }
        //}
        //////////////////为了优化先拿掉（以上）//////////////////

        if (PositionID != 0)
        {
            if (OldPosition != PositionID)
            {
                if (OldPosition != 0)
                {
                    //GameObject.Find("Base" + OldPosition.ToString()).renderer.material.mainTexture = Resources.Load("Game/position", typeof(Texture)) as Texture;
                }
                OldPosition = PositionID;
            }
            ListBase[PositionID].renderer.material.mainTexture = Resources.Load("Game/green", typeof(Texture)) as Texture;
        }

        //if (ObjectName == "")
        //{
        //    GameObject.Find("Base" + PositionID.ToString()).renderer.material.mainTexture = Resources.Load("Game/green1", typeof(Texture)) as Texture;
        //}
        return PositionID;
    }
    public IEnumerator ShowTerrain(int PositionID)
    {
        for (int i = 0; i < ListTerrainPosition.Count; i++)
        {
            if (ListTerrainPosition[i] == PositionID)
            {
                fw.InfoBoard.SetActive(true);
                TextTranslator.TerrainInfo ti = TextTranslator.instance.GetTerrainInfoByID(ListTerrainID[i]);
                fw.SetTerrainInfo(ti.name, ti.terrainEffect, ti.icon.ToString());
                yield return new WaitForSeconds(3f);
                fw.InfoBoard.SetActive(false);
                break;
            }
        }
        yield return 0;
    }
    public IEnumerator ShowTarget(List<RolePicture> SetPicture, int RoleIndex, int PositionID, bool IsShow, bool IsMove)
    {
        MyMoves.SetActive(true);
        List<int> ListNode = new List<int>();
        if (PositionID != 0)
        {
            //float DelaySec = 0.01f;
            int Area = SetPicture[RoleIndex].RoleArea;
            int Move = SetPicture[RoleIndex].RoleMoveStep;
            //string PicPosition = "walk";


            for (int i = 1; i < PositionRow * PositionColumn + 30; i++)
            {
                if (ListMove[i] != null)
                {
                    ListMove[i].renderer.material = BlankMaterial;
                }
            }


            if (!IsMove)
            {
                if (Area == 1022)
                {
                    Area = 1;
                }
                else if (Area == 12)
                {
                    Area = 2;
                }
                else if (Area == 123)
                {
                    Area = 3;
                }
                //Area = Move + Area;
                Area = Move;
                SetListPosition(PositionRow, 2);
            }


            if (!IsShow)
            {
                int OldPosition = ListRolePicture[RoleIndex].RolePosition;
                ListRolePicture[RoleIndex].RolePosition = PositionID;
                for (int j = 0; j < ListEnemyPicture.Count; j++)
                {
                    List<int> TargetIndex = FindTarget(RoleIndex, ListEnemyPicture, ListRolePicture, 0, j, true);
                    if (TargetIndex.Count > 0)
                    {
                        DestroyImmediate(GameObject.Find("DeGanTanHao" + ListEnemyPicture[j].RolePosition.ToString()));
                        if (GameObject.Find("GanTanHao" + ListEnemyPicture[j].RolePosition.ToString()) == null)
                        {
                            GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/GanTanHao", typeof(GameObject)), PictureCreater.instance.ListPosition[ListEnemyPicture[j].RolePosition] + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
                            go.name = "GanTanHao" + ListEnemyPicture[j].RolePosition.ToString();
                            if (!ListEnemyPicture[j].IsPicture)
                            {
                                ListEnemyPicture[j].RolePictureObject.GetComponent<Animator>().SetFloat("id", 2);
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (GameObject.Find("GanTanHao" + ListEnemyPicture[j].RolePosition.ToString()) != null)
                        {
                            DestroyImmediate(GameObject.Find("GanTanHao" + ListEnemyPicture[j].RolePosition.ToString()));

                            if (GameObject.Find("DeGanTanHao" + ListEnemyPicture[j].RolePosition.ToString()) == null)
                            {
                                GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/GanTanHao02", typeof(GameObject)), ListPosition[ListEnemyPicture[j].RolePosition] + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
                                go.name = "DeGanTanHao" + ListEnemyPicture[j].RolePosition.ToString();
                                if (!ListEnemyPicture[j].IsPicture)
                                {
                                    ListEnemyPicture[j].RolePictureObject.GetComponent<Animator>().SetFloat("id", 0);
                                }
                            }
                        }
                    }
                }
                ListRolePicture[RoleIndex].RolePosition = OldPosition;
                //for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
                //{
                //    bool IsHave = false;
                //    foreach (int l in ListNode)
                //    {
                //        if (l == i)
                //        {
                //            IsHave = true;
                //            break;
                //        }
                //    }

                //    if (IsHave)
                //    {
                //        for (int j = 0; j < ListEnemyPicture.Count; j++)
                //        {
                //            if (ListEnemyPicture[j].RolePosition == i)
                //            {
                //                DestroyImmediate(GameObject.Find("DeGanTanHao" + i.ToString()));
                //                if (GameObject.Find("GanTanHao" + i.ToString()) == null)
                //                {
                //                    GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/GanTanHao", typeof(GameObject)), PictureCreater.instance.ListPosition[i] + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
                //                    go.name = "GanTanHao" + i.ToString();

                //                    ListEnemyPicture[j].RolePictureObject.GetComponent<Animator>().SetFloat("id", 2);
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (GameObject.Find("GanTanHao" + i.ToString()) != null)
                //        {
                //            DestroyImmediate(GameObject.Find("GanTanHao" + i.ToString()));

                //            if (GameObject.Find("DeGanTanHao" + i.ToString()) == null)
                //            {
                //                GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/GanTanHao02", typeof(GameObject)), PictureCreater.instance.ListPosition[i] + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
                //                go.name = "DeGanTanHao" + i.ToString();

                //                for (int j = 0; j < ListEnemyPicture.Count; j++)
                //                {
                //                    if (ListEnemyPicture[j].RolePosition == i)
                //                    {
                //                        ListEnemyPicture[j].RolePictureObject.GetComponent<Animator>().SetFloat("id", 0);
                //                        break;
                //                    }
                //                }
                //                while (GameObject.Find("Path" + i.ToString()) != null)
                //                {
                //                    DestroyImmediate(GameObject.Find("Path" + i.ToString()));
                //                }
                //            }
                //        }
                //    }
                //}
            }
            else
            {
                StartCoroutine(CheckArea(ListNode, PositionID, Area));
                if (SetPicture[RoleIndex].RolePictureMonster)
                {
                    if (ListEnemyPicture[RoleIndex].RoleArea == 1022)
                    {
                        yield return new WaitForSeconds(0.8f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.6f);
                    }
                }
                else
                {
                    if (ListRolePicture[RoleIndex].RoleArea == 1022)
                    {
                        yield return new WaitForSeconds(0.8f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.6f);
                    }
                }

                foreach (int a in ListNode)
                {
                    List<int> ListTarget = new List<int>();
                    if (SetPicture[RoleIndex].RolePictureMonster)
                    {
                        int RolePosition = ListEnemyPicture[RoleIndex].RolePosition;
                        ListEnemyPicture[RoleIndex].RolePosition = a;

                        if (ListEnemyPicture[RoleIndex].RoleArea == 1022)
                        {
                            for (int i = 0; i < ListEnemyPicture.Count; i++)
                            {
                                if (ListEnemyPicture[i].RolePosition > 0 && ListEnemyPicture[i].RoleNowBlood > 0)
                                {
                                    ListTarget = FindTarget(i, ListEnemyPicture, ListRolePicture, 0, RoleIndex, true);
                                    foreach (int t in ListTarget)
                                    {
                                        ListMove[ListEnemyPicture[t].RolePosition].renderer.material.mainTexture = Resources.Load("Game/healrange", typeof(Texture)) as Texture;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ListRolePicture.Count; i++)
                            {
                                if (ListRolePicture[i].RolePosition > 0 && ListRolePicture[i].RoleNowBlood > 0)
                                {
                                    ListTarget = FindTarget(i, ListEnemyPicture, ListRolePicture, 0, RoleIndex, true);
                                    foreach (int t in ListTarget)
                                    {
                                        ListMove[ListRolePicture[t].RolePosition].renderer.material.mainTexture = Resources.Load("Game/attack", typeof(Texture)) as Texture;
                                    }
                                }
                            }
                        }
                        ListEnemyPicture[RoleIndex].RolePosition = RolePosition;
                    }
                    else
                    {
                        int RolePosition = ListRolePicture[RoleIndex].RolePosition;
                        ListRolePicture[RoleIndex].RolePosition = a;

                        if (ListRolePicture[RoleIndex].RoleArea == 1022)
                        {
                            for (int i = 0; i < ListRolePicture.Count; i++)
                            {
                                if (ListRolePicture[i].RolePosition > 0 && ListRolePicture[i].RoleNowBlood > 0 && RoleIndex != i)
                                {
                                    ListTarget = FindTarget(i, ListRolePicture, ListEnemyPicture, 0, RoleIndex, true);
                                    foreach (int t in ListTarget)
                                    {
                                        ListMove[ListRolePicture[t].RolePosition].renderer.material.mainTexture = Resources.Load("Game/healrange", typeof(Texture)) as Texture;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ListEnemyPicture.Count; i++)
                            {
                                if (ListEnemyPicture[i].RolePosition > 0 && ListEnemyPicture[i].RoleNowBlood > 0)
                                {
                                    ListTarget = FindTarget(i, ListRolePicture, ListEnemyPicture, 0, RoleIndex, true);
                                    foreach (int t in ListTarget)
                                    {
                                        ListMove[ListEnemyPicture[t].RolePosition].renderer.material.mainTexture = Resources.Load("Game/attack", typeof(Texture)) as Texture;
                                    }
                                }
                            }
                        }
                        ListRolePicture[RoleIndex].RolePosition = RolePosition;
                    }

                }
                yield return new WaitForSeconds(3);
            }
        }


        if (!IsFight && FightStyle != 2)
        {
            for (int i = 1; i < PositionRow * PositionColumn + 1; i++)
            {
                if (ListMove[i] != null)
                {
                    ListMove[i].renderer.material = BlankMaterial;
                }
            }
        }
        //MyMoves.SetActive(false);
    }

    IEnumerator CheckArea(List<int> ListNode, int PositionID, int Area)
    {
        if (IsLock || !IsFight)
        {
            if (!ListNode.Contains(PositionID))
            {
                ListNode.Add(PositionID);
                ListMove[PositionID].renderer.material.mainTexture = Resources.Load("Game/walk", typeof(Texture)) as Texture;
            }
            yield return new WaitForSeconds(0.02f);
            if (Area > 0)
            {
                if (!CheckPosition(PositionID + PositionRow))
                {
                    StartCoroutine(CheckArea(ListNode, PositionID + PositionRow, Area - 1));
                }
                yield return new WaitForSeconds(0.01f);
                if (ListPosition[PositionID].z < ListPosition[PositionID + PositionRow - 1].z)
                {
                    if (!CheckPosition(PositionID + PositionRow - 1))
                    {
                        StartCoroutine(CheckArea(ListNode, PositionID + PositionRow - 1, Area - 1));
                    }
                }
                yield return new WaitForSeconds(0.01f);
                if (ListPosition[PositionID].z < ListPosition[PositionID - 1].z)
                {
                    if (!CheckPosition(PositionID - 1))
                    {
                        StartCoroutine(CheckArea(ListNode, PositionID - 1, Area - 1));
                    }
                }
                yield return new WaitForSeconds(0.01f);
                if (!CheckPosition(PositionID - PositionRow))
                {
                    StartCoroutine(CheckArea(ListNode, PositionID - PositionRow, Area - 1));
                }
                yield return new WaitForSeconds(0.01f);
                if (ListPosition[PositionID].z > ListPosition[PositionID - PositionRow + 1].z)
                {
                    if (!CheckPosition(PositionID - PositionRow + 1))
                    {
                        StartCoroutine(CheckArea(ListNode, PositionID - PositionRow + 1, Area - 1));
                    }
                }
                yield return new WaitForSeconds(0.01f);
                if (ListPosition[PositionID].z > ListPosition[PositionID + 1].z)
                {
                    if (!CheckPosition(PositionID + 1))
                    {
                        StartCoroutine(CheckArea(ListNode, PositionID + 1, Area - 1));
                    }
                }
            }
        }
    }


    class PathNode
    {
        public int NowPositionID;
        public int PrePositionID;
        public int NowMoveStep;
    }

    List<PathNode> ListOpenNode = new List<PathNode>();
    List<PathNode> ListCloseNode = new List<PathNode>();
    List<PathNode> ListFindNode = new List<PathNode>();

    bool CheckTargetArea(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SetRoleIndex, int PositionID, int GoPosition)
    {
        bool IsFind = false;
        PathNode NewNode = new PathNode();
        NewNode.PrePositionID = 0;
        NewNode.NowPositionID = PositionID;
        NewNode.NowMoveStep = 0;
        ListOpenNode.Add(NewNode);
        while (ListOpenNode.Count > 0)
        {
            if (AddCloseList(SetPicture, SetEnemyPicture, SetRoleIndex, ListOpenNode[0], GoPosition))
            {
                IsFind = true;
                break;
            }
        }
        return IsFind;
    }

    bool AddCloseList(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SetRoleIndex, PathNode NowNode, int GoPosition)
    {
        bool IsArrive = false;
        int PositionID = NowNode.NowPositionID;
        int MoveStep = NowNode.NowMoveStep;

        foreach (var n in ListCloseNode)
        {
            if (PositionID == n.NowPositionID)
            {
                ListOpenNode.RemoveAt(0);
                return IsArrive;
            }
        }

        if (!CheckEnemyPosition(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID + PositionRow, GoPosition))
        {
            if (AddOpenList(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID, PositionID + PositionRow, MoveStep + 1, GoPosition))
            {
                IsArrive = true;
            }
        }
        if (!CheckEnemyPosition(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID - PositionRow, GoPosition))
        {
            if (AddOpenList(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID, PositionID - PositionRow, MoveStep + 1, GoPosition))
            {
                IsArrive = true;
            }
        }
        if (ListPosition[PositionID].z < ListPosition[PositionID + PositionRow - 1].z)
        {
            if (!CheckEnemyPosition(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID + PositionRow - 1, GoPosition))
            {
                if (AddOpenList(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID, PositionID + PositionRow - 1, MoveStep + 1, GoPosition))
                {
                    IsArrive = true;
                }
            }
        }
        if (ListPosition[PositionID].z < ListPosition[PositionID - 1].z)
        {
            if (!CheckEnemyPosition(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID - 1, GoPosition))
            {
                if (AddOpenList(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID, PositionID - 1, MoveStep + 1, GoPosition))
                {
                    IsArrive = true;
                }
            }
        }
        if (ListPosition[PositionID].z > ListPosition[PositionID + 1].z)
        {
            if (!CheckEnemyPosition(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID + 1, GoPosition))
            {
                if (AddOpenList(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID, PositionID + 1, MoveStep + 1, GoPosition))
                {
                    IsArrive = true;
                }
            }
        }
        if (ListPosition[PositionID].z > ListPosition[PositionID - PositionRow + 1].z)
        {
            if (!CheckEnemyPosition(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID - PositionRow + 1, GoPosition))
            {
                if (AddOpenList(SetPicture, SetEnemyPicture, SetRoleIndex, PositionID, PositionID - PositionRow + 1, MoveStep + 1, GoPosition))
                {
                    IsArrive = true;
                }
            }
        }

        PathNode NewNode = new PathNode();
        NewNode.PrePositionID = NowNode.PrePositionID;
        NewNode.NowPositionID = NowNode.NowPositionID;
        NewNode.NowMoveStep = NowNode.NowMoveStep;

        ListCloseNode.Add(NewNode);
        ListOpenNode.RemoveAt(0);

        return IsArrive;
    }

    bool AddOpenList(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SetRoleIndex, int PrePositionID, int PositionID, int MoveStep, int GoPosition)
    {
        bool IsArrive = false;

        //foreach (var n in ListCloseNode)
        //{
        //    if (PositionID == n.NowPositionID)
        //    {
        //        return IsArrive;
        //    }
        //}

        PathNode NewNode = new PathNode();
        NewNode.PrePositionID = PrePositionID;
        NewNode.NowPositionID = PositionID;
        NewNode.NowMoveStep = MoveStep;

        ListOpenNode.Add(NewNode);
        if (GoPosition > 0)
        {
            if (SetPicture[SetRoleIndex].RoleArea > 1 && SetPicture[SetRoleIndex].RoleArea < 1000)
            {
                int EnemyIndex = -1;
                for (int i = 0; i < SetEnemyPicture.Count; i++)
                {
                    if (SetEnemyPicture[i].RolePosition == GoPosition)
                    {
                        EnemyIndex = i;
                    }
                }

                if (EnemyIndex > -1)
                {
                    List<int> TargetIndex = new List<int>();
                    int OldPositionID = SetPicture[SetRoleIndex].RolePosition;
                    SetPicture[SetRoleIndex].RolePosition = PositionID;

                    TargetIndex = FindTarget(EnemyIndex, SetPicture, SetEnemyPicture, 0, SetRoleIndex, false);
                    SetPicture[SetRoleIndex].RolePosition = OldPositionID;

                    if (TargetIndex.Count > 0)
                    {
                        if (ListFindNode.Count == 0)
                        {
                            ListFindNode.Add(NewNode);
                        }
                        return true;
                    }
                }
                else
                {
                    if (PositionID == GoPosition)
                    {
                        if (ListFindNode.Count == 0)
                        {
                            ListFindNode.Add(NewNode);
                        }
                        return true;
                    }
                }
            }
            else
            {
                if (PositionID == GoPosition)
                {
                    if (ListFindNode.Count == 0)
                    {
                        ListFindNode.Add(NewNode);
                    }
                    return true;
                }
            }
        }
        else
        {
            if (SetPicture[SetRoleIndex].RoleArea == 1022)
            {
                if (PositionID == GoPosition)
                {
                    if (ListFindNode.Count == 0)
                    {
                        ListFindNode.Add(NewNode);
                    }
                    return true;
                }
            }
            else if (SetPicture[SetRoleIndex].RoleArea > 1)
            {
                List<int> TargetIndex = new List<int>();
                int OldPositionID = SetPicture[SetRoleIndex].RolePosition;
                SetPicture[SetRoleIndex].RolePosition = PositionID;

                TargetIndex = FindTarget(-1, SetPicture, SetEnemyPicture, 0, SetRoleIndex, false);
                SetPicture[SetRoleIndex].RolePosition = OldPositionID;

                if (TargetIndex.Count > 0)
                {
                    if (ListFindNode.Count == 0)
                    {
                        ListFindNode.Add(NewNode);
                    }
                    return true;
                }
            }
            else
            {
                foreach (var r in SetEnemyPicture)
                {
                    if (r.RolePosition == PositionID && r.RoleNowBlood > 0 && !r.BuffInvisible)
                    {
                        if (ListFindNode.Count == 0)
                        {
                            ListFindNode.Add(NewNode);
                        }
                        return true;
                    }
                }
            }
        }

        if (NewNode.NowMoveStep > 8)
        {
            return true;
        }

        return IsArrive;
    }

    public bool CheckEnemyPosition(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SetRoleIndex, int PositionID, int GoPosition)
    {
        for (int RoleIndex = 0; RoleIndex < SetPicture.Count; RoleIndex++)
        {
            if (SetPicture[RoleIndex].RolePosition == PositionID && SetPicture[RoleIndex].RoleNowBlood > 0 && SetPicture[SetRoleIndex].RoleArea != 1022)
            {
                return true;
            }
        }

        for (int RoleIndex = 0; RoleIndex < SetPicture.Count; RoleIndex++)
        {
            if (SetPicture[RoleIndex].RolePosition == PositionID && SetPicture[RoleIndex].RolePosition != GoPosition && SetPicture[RoleIndex].RoleNowBlood > 0 && SetPicture[SetRoleIndex].RoleArea == 1022)
            {
                return true;
            }
        }

        for (int RoleIndex = 0; RoleIndex < SetEnemyPicture.Count; RoleIndex++)
        {
            if (SetEnemyPicture[RoleIndex].RolePosition == PositionID && SetEnemyPicture[RoleIndex].RoleNowBlood > 0 && (SetPicture[SetRoleIndex].RoleArea == 1022 || SetEnemyPicture[RoleIndex].BuffInvisible || (GoPosition != 0 && SetEnemyPicture[RoleIndex].RolePosition != GoPosition)))
            {
                return true;
            }
        }


        for (int i = 0; i < ListStopPosition.Count; i++)
        {
            if (PositionID == ListStopPosition[i])
            {
                return true;
            }
        }

        for (int i = 0; i < ListStopPosition.Count; i++)
        {
            if (PositionID == ListStopPosition[i])
            {
                return true;
            }
        }

        if (ListPosition.Count <= PositionID)
        {
            return true;
        }
        if (1 > PositionID)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    int FindGoPosition(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, int GoPosition)
    {
        if (ListCloseNode.Count == 0)
        {
            List<int> ListNode = new List<int>();
            if (CheckTargetArea(SetPicture, SetEnemyPicture, RoleIndex, SetPicture[RoleIndex].RolePosition, GoPosition))
            {
                if (ListFindNode.Count > 0)
                {
                    PathNode NowNode = ListFindNode[0];
                    while (NowNode.PrePositionID != SetPicture[RoleIndex].RolePosition && NowNode.NowPositionID != SetPicture[RoleIndex].RolePosition)
                    {
                        foreach (var n in ListCloseNode)
                        {
                            if (n.NowPositionID == NowNode.PrePositionID)
                            {
                                NowNode = n;
                                break;
                            }
                        }
                    }
                    GoPosition = NowNode.NowPositionID;
                }
            }
        }
        else if (ListFindNode.Count > 0)
        {
            PathNode NowNode = ListFindNode[0];
            while (NowNode.PrePositionID != SetPicture[RoleIndex].RolePosition && NowNode.NowPositionID != SetPicture[RoleIndex].RolePosition)
            {
                foreach (var n in ListCloseNode)
                {
                    if (n.NowPositionID == NowNode.PrePositionID)
                    {
                        NowNode = n;
                        break;
                    }
                }
                if (NowNode.NowMoveStep == 0)
                {
                    break;
                }
            }

            GoPosition = NowNode.NowPositionID;
        }
        return GoPosition;
    }
    #endregion

    #region ManualSkill
    public IEnumerator FireAutoSkill(int ManualSkillID, List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture)
    {
        ManualSkill ms = TextTranslator.instance.GetManualSkillByType(ManualSkillID);
        if (ms == null)
        {
            ms = TextTranslator.instance.GetManualSkillByID(ManualSkillID);
        }
        else
        {
            if (!IsSkip)
            {
                StartCoroutine(fw.ShowEnemySkill(ms.skillName));
                yield return new WaitForSeconds(1);
            }
        }

        List<int> RandomList = new List<int>();
        int SkillRoleIndex = 0;
        bool IsRepeat = true;
        switch (ms.Type)
        {
            case 1:
            case 2:
                for (int k = 0; k < SetPicture.Count; k++)
                {
                    if (SetPicture[k].RoleNowBlood > 0 && SetPicture[k].RolePosition > 0)
                    {
                        RandomList.Add(k);
                    }
                }
                if (RandomList.Count > 0)
                {
                    SkillRoleIndex = RandomList[UnityEngine.Random.Range(0, RandomList.Count)];
                    if (SetPicture[0].RolePictureMonster)
                    {
                        FireSkill(SetPicture, SetEnemyPicture, ms.skillType, 0, SkillRoleIndex, 1, GateNeedLevel);
                    }
                    else
                    {
                        FireSkill(SetPicture, SetEnemyPicture, ms.skillType, 0, SkillRoleIndex, 1, CharacterRecorder.instance.level);
                    }
                }
                break;
            case 3:
            case 4:
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        RandomList.Add(k);
                    }
                }
                if (RandomList.Count > 0)
                {
                    SkillRoleIndex = RandomList[UnityEngine.Random.Range(0, RandomList.Count)];
                    if (SetPicture[0].RolePictureMonster)
                    {
                        FireSkill(SetPicture, SetEnemyPicture, ms.skillType, 1, SkillRoleIndex, 1, GateNeedLevel);
                    }
                    else
                    {
                        FireSkill(SetPicture, SetEnemyPicture, ms.skillType, 1, SkillRoleIndex, 1, CharacterRecorder.instance.level);
                    }
                }
                break;
            case 5:
                while (IsRepeat)
                {
                    SkillRoleIndex = UnityEngine.Random.Range(8, 39);
                    IsRepeat = false;
                    for (int k = 0; k < SetPicture.Count; k++)
                    {
                        if (SetPicture[k].RolePosition > 0)
                        {
                            if (SkillRoleIndex == SetPicture[k].RolePosition)
                            {
                                IsRepeat = true;
                            }
                        }
                    }
                    for (int k = 0; k < SetEnemyPicture.Count; k++)
                    {
                        if (SetEnemyPicture[k].RolePosition > 0)
                        {
                            if (SkillRoleIndex == SetEnemyPicture[k].RolePosition)
                            {
                                IsRepeat = true;
                            }
                        }
                    }
                    for (int k = 0; k < ListTerrainPosition.Count; k++)
                    {
                        if (SkillRoleIndex == ListTerrainPosition[k])
                        {
                            IsRepeat = true;
                        }
                    }
                }

                if (SetPicture[0].RolePictureMonster)
                {
                    FireSkill(SetPicture, SetEnemyPicture, ms.skillType, 2, SkillRoleIndex, 1, GateNeedLevel);
                }
                else
                {
                    FireSkill(SetPicture, SetEnemyPicture, ms.skillType, 2, SkillRoleIndex, 1, CharacterRecorder.instance.level);
                }

                break;
        }
    }
    public void CloseSkill(List<RolePicture> SetPicture, int SkillPosition)
    {
        if (!SetPicture[0].RolePictureMonster)
        {
            switch (SkillPosition)
            {
                case 1:
                    SkillFire1 = 2;
                    fw.SkillMask1.SetActive(true);
                    fw.SkillEffectOK1.SetActive(false);
                    fw.SkillCD1 = TextTranslator.instance.GetManualSkillByID(FightSkill1).coolDown;
                    fw.SkillUpCD1 = fw.SkillCD1;

                    break;
                case 2:
                    SkillFire2 = 2;
                    fw.SkillMask2.SetActive(true);
                    fw.SkillEffectOK2.SetActive(false);
                    fw.SkillCD2 = TextTranslator.instance.GetManualSkillByID(FightSkill2).coolDown;
                    fw.SkillUpCD2 = fw.SkillCD2;
                    break;
                case 3:
                    SkillFire3 = 2;
                    fw.SkillMask3.SetActive(true);
                    fw.SkillEffectOK3.SetActive(false);
                    fw.SkillCD3 = TextTranslator.instance.GetManualSkillByID(FightSkill3).coolDown;
                    fw.SkillUpCD3 = fw.SkillCD3;
                    break;
            }
        }
    }
    public void FireSkill(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int SkillID, int IsEnemy, int SkillRoleIndex, int SkillPosition, int SkillLevel)
    {
        IsFireSkill = true;
        bool IsClose = false;
        //Debug.Log("SkillID" + SkillID.ToString() + "IsEnemy" + IsEnemy.ToString());
        ManualSkill ms = null;
        if (SkillID < 1000)
        {
            ms = TextTranslator.instance.GetManualSkillByType(SkillID);
            SkillID = ms.skillID;
            StartCoroutine(UnlockEnemy());
        }
        else
        {
            ms = TextTranslator.instance.GetManualSkillByID(SkillID);
            StartCoroutine(Unlock());
            IsClose = true;
        }

        if (IsEnemy == 1) //作用在敌人
        {
            int MovePosition = SetEnemyPicture[SkillRoleIndex].RolePosition;
            if (SkillID == 3003) //战术导弹
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("J_daodan");
                EffectMaker.instance.Create2DEffect("~DingDianDaoDan", "", null, ListPosition[MovePosition], ListPosition[MovePosition], Vector3.one, 0f, 0f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);

                List<int> FightIndex = new List<int>();
                List<int> FightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();

                int TargetID = CheckTarget(false, -1, false, MovePosition, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition + 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition - 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition + 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition - 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition + 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition - 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                for (int i = 0; i < RoleFightDamige.Count; i++)
                {
                    if (RoleFightDamige[i] < 1)
                    {
                        RoleFightDamige[i] = 100;
                    }
                }
                EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 1, 1, 1, 1, -2, FightIndex, FightCate, RoleFightDamige, null, null, false, false, false, false, false, false, false, false, false, "", null);

                FightIndex.Clear();
                FightCate.Clear();
                RoleFightDamige.Clear();

                TargetID = CheckTarget(false, -1, true, MovePosition, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition + 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition - 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition + 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition - 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition + 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition - 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                for (int i = 0; i < RoleFightDamige.Count; i++)
                {
                    if (RoleFightDamige[i] < 1)
                    {
                        RoleFightDamige[i] = 100;
                    }
                }
                EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 1, 1, 1, 1, -2, FightIndex, FightCate, RoleFightDamige, null, null, false, false, false, false, false, false, false, false, true, "", null);


            }
            else if (SkillID == 3005) //手榴弹
            {
                //SceneTransformer.instance.SendBoxGuide();
                if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 12)
                {
                    UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_1307);
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 13);
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
                    LuaDeliver.instance.UseGuideStation();
                }

                CloseSkill(SetPicture, SkillPosition);
                StartCoroutine(AudioEditer.instance.DelaySound(80f, "S_carbine"));


                List<int> FightIndex = new List<int>();
                List<int> FightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();


                int TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition + 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition - 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition + 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition - 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition + 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition - 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }

                StartCoroutine(ClearTerrain(MovePosition));
                StartCoroutine(ClearTerrain(MovePosition + 1));
                StartCoroutine(ClearTerrain(MovePosition - 1));
                StartCoroutine(ClearTerrain(MovePosition + 5));
                StartCoroutine(ClearTerrain(MovePosition - 5));
                StartCoroutine(ClearTerrain(MovePosition + 4));
                StartCoroutine(ClearTerrain(MovePosition - 4));


                for (int i = 0; i < RoleFightDamige.Count; i++)
                {
                    if (RoleFightDamige[i] < 1)
                    {
                        RoleFightDamige[i] = 100;
                    }
                }
                //FightIndex.Add(SkillRoleIndex);
                //FightCate.Add(0);
                //RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[SkillRoleIndex].RolePDefend - SetEnemyPicture[SkillRoleIndex].BuffDefend));

                //EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 0, 1, 1, 1, 0, FightIndex, FightCate, RoleFightDamige, null, false, false, false, false, false, false, false, false, false, "", null);
                EffectMaker.instance.Create2DEffect("~BuBing_Skill_02", "", null, ListPosition[MovePosition - 5], ListPosition[MovePosition], new Vector3(0.3f, 0.01f, 0.03f), 0.1f, 0, 1, 1, 1, -2, FightIndex, FightCate, RoleFightDamige, null, null, true, false, false, false, false, false, true, false, SetPicture[0].RolePictureMonster, "", null);
                EffectMaker.instance.Create2DEffect("~WF_ShouLiuDan", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0.8f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
            }
            else if (SkillID == 3006) //集火
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("S_captain");

                Buff NewBuff = TextTranslator.instance.GetBuffByID(ms.buffID);
                if (NewBuff != null)
                {
                    Buff SkillBuff = new Buff(NewBuff);
                    SkillBuff.parameter1 = SkillRoleIndex;
                    RoleAddBuff(SetEnemyPicture, SkillRoleIndex, SkillBuff);
                }
            }
            else if (SkillID == 3007) //震爆弹
            {
                CloseSkill(SetPicture, SkillPosition);
                StartCoroutine(AudioEditer.instance.DelaySound(80f, "S_carbine"));

                List<int> FightIndex = new List<int>();
                List<int> FightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();


                int TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend));

                    if (RoleFightDamige[0] < 1)
                    {
                        RoleFightDamige[0] = 100;
                    }
                }


                Buff NewBuff = TextTranslator.instance.GetBuffByID(ms.buffID);
                List<Buff> NewListBuff = new List<Buff>();
                NewListBuff.Add(NewBuff);
                EffectMaker.instance.Create2DEffect("~BuBing_Skill_02", "", null, ListPosition[MovePosition - 5], ListPosition[MovePosition], new Vector3(0.3f, 0.01f, 0.03f), 0.1f, 0, 1, 1, 1, 0, FightIndex, FightCate, RoleFightDamige, NewListBuff, null, true, false, false, false, false, false, true, false, SetPicture[0].RolePictureMonster, "", null);
                EffectMaker.instance.Create2DEffect("~WF_ShanGuangDan01", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0.8f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
                //EffectMaker.instance.Create2DEffect("~WF_ShanGuangDan02", "", null, PictureCreater.instance.ListPosition[MovePosition] + new Vector3(0, 1.5f, 0), PictureCreater.instance.ListPosition[MovePosition] + new Vector3(0, 1.5f, 0), Vector3.one, 0f, 1.3f, 10f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
            }
            else if (SkillID == 3011) //驱散敌Buff
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("W_medici");
                EffectMaker.instance.Create2DEffect("~WF_QuSan02", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 1, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);


                for (int b = 0; b < SetEnemyPicture[SkillRoleIndex].ListBuff.Count; b++)
                {
                    if (SetEnemyPicture[SkillRoleIndex].ListBuff[b].action == 1)
                    {
                        SetEnemyPicture[SkillRoleIndex].ListBuff[b].round = -1;
                    }
                }
                CalculateRoleBuff(SetEnemyPicture, SkillRoleIndex);
            }
        }
        else if (IsEnemy == 0) //作用在自已
        {
            int MovePosition = SetPicture[SkillRoleIndex].RolePosition;
            if (SkillID == 3001)
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("W_medici");
                EffectMaker.instance.Create2DEffect("~ZhiLiao", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);

                List<int> FightIndex = new List<int>();
                List<int> FightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();

                for (int t = 0; t < SetPicture.Count; t++)
                {
                    if (SetPicture[t].RolePosition == MovePosition)
                    {
                        FightIndex.Add(t);
                        FightCate.Add(0);
                        RoleFightDamige.Add(-(int)(SetPicture[t].RoleMaxBlood * ms.skillVal1 / 100f));
                        break;
                    }
                }
                EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 0.1f, 1, 1, 1, -1, FightIndex, FightCate, RoleFightDamige, null, null, false, false, false, false, false, false, false, false, SetPicture[0].RolePictureMonster, "", null);
            }
            else if (SkillID == 3004)
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("ui_powerbig");
                //EffectMaker.instance.Create2DEffect("~JiaBuff_Gong", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);

                for (int t = 0; t < SetPicture.Count; t++)
                {
                    if (SetPicture[t].RolePosition == MovePosition)
                    {
                        EffectMaker.instance.Create2DEffect("~JiaBuff_Gong", "", null, SetPicture[t].RoleObject.transform.position - new Vector3(0, 0, 0.2f), SetPicture[t].RoleObject.transform.position - new Vector3(0, 0, 0.2f), new Vector3(0.5f, 0.5f, 0.5f), 0.04f, 0, 2, 1, 1, t, null, null, null, null, null, false, false, true, false, false, false, false, false, SetPicture[t].RolePictureMonster, "", null);
                        break;
                    }
                }

                Buff NewBuff = TextTranslator.instance.GetBuffByID(ms.buffID);
                if (NewBuff != null)
                {
                    Buff SkillBuff = new Buff(NewBuff);
                    RoleAddBuff(SetPicture, SkillRoleIndex, SkillBuff);
                }
            }
            else if (SkillID == 3009)
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("W_medici");
                EffectMaker.instance.Create2DEffect("~WF_JiJiuWuZi", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);

                List<int> FightIndex = new List<int>();
                List<int> FightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();

                for (int t = 0; t < SetPicture.Count; t++)
                {
                    if (SetPicture[t].RolePosition > 0 && SetPicture[t].RoleNowBlood > 0)
                    {
                        FightIndex.Add(t);
                        FightCate.Add(0);
                        RoleFightDamige.Add(-(int)(SetPicture[t].RoleMaxBlood * ms.skillVal1 / 100f));
                    }
                }
                EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 0.1f, 1, 1, 1, -1, FightIndex, FightCate, RoleFightDamige, null, null, false, false, false, false, false, false, false, false, SetPicture[0].RolePictureMonster, "", null);
            }
            else if (SkillID == 3010)
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("S_shield");
                //EffectMaker.instance.Create2DEffect("~WF_WuDiHuDun", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);

                Buff NewBuff = TextTranslator.instance.GetBuffByID(ms.buffID);
                if (NewBuff != null)
                {
                    Buff SkillBuff = new Buff(NewBuff);
                    RoleAddBuff(SetPicture, SkillRoleIndex, SkillBuff);
                }
            }
            else if (SkillID == 3012) //驱散敌Debuff
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("W_medici");
                EffectMaker.instance.Create2DEffect("~WF_QuSan01", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 1, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);


                for (int b = 0; b < SetPicture[SkillRoleIndex].ListBuff.Count; b++)
                {
                    if (SetPicture[SkillRoleIndex].ListBuff[b].action == 2)
                    {
                        SetPicture[SkillRoleIndex].ListBuff[b].round = -1;
                    }
                }
                CalculateRoleBuff(SetPicture, SkillRoleIndex);
            }
        }
        else if (IsEnemy == 2) //作用在空地
        {
            int MovePosition = SkillRoleIndex;

            //if (SkillID == 1)
            //{
            //    XMLParser.WaveTerrain t = new XMLParser.WaveTerrain();
            //    t.Buff = 11;
            //    t.ID = 4;
            //    t.Destoyed = 3;
            //    t.EffectTime = 50;
            //    t.HP = 1000;
            //    t.Parameter1 = 2;
            //    t.PositionID = MovePosition;
            //    t.Status = true;
            //    XMLParser.instance.ListWave[0].ListWaveTerrain.Add(t);

            //    GameObject TempObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            //    DestroyImmediate(TempObject.GetComponent("MeshCollider"));
            //    TempObject.renderer.material.mainTexture = Resources.Load("Game/blood", typeof(Texture)) as Texture;
            //    TempObject.renderer.material.shader = Shader.Find("Unlit/Transparent Colored");
            //    TempObject.transform.Rotate(90, 0, 0);
            //    TempObject.transform.localScale = new Vector3(2, 2, 2);

            //    TempObject.transform.position = ListPosition[t.PositionID] + new Vector3(0, 0.11f, 0);
            //    TempObject.name = "TerrainObject" + t.PositionID.ToString();
            //    TempObject.transform.parent = gameObject.transform;

            //    GameObject.Find("FightWindow").GetComponent<FightWindow>().SkillMask1.SetActive(true);
            //}

            if (SkillID == 3002)
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("J_sanbing");
                if (SetPicture[0].RolePictureMonster)
                {
                    CreateRole(69998, "伞兵", MovePosition, Color.cyan, ms.skillVal1 * SkillLevel + ms.skillVal2, ms.skillVal1 * SkillLevel + ms.skillVal2, 1000, 11.6f, SetPicture[0].RolePictureMonster, false, 1, 1.5f, 0, "Enemy", 0, ms.skillVal3 * SkillLevel + ms.skillVal4, 0, (ms.skillVal3 * SkillLevel + ms.skillVal4) * ms.skillVal5, 0, 0, 0, 1001, 0, 0, 1, 0, 1, 2, 0, "");
                }
                else
                {
                    CreateRole(69999, "伞兵", MovePosition, Color.cyan, ms.skillVal1 * SkillLevel + ms.skillVal2, ms.skillVal1 * SkillLevel + ms.skillVal2, 1000, 11.6f, SetPicture[0].RolePictureMonster, false, 1, 1.5f, 0, "Enemy", 0, ms.skillVal3 * SkillLevel + ms.skillVal4, 0, (ms.skillVal3 * SkillLevel + ms.skillVal4) * ms.skillVal5, 0, 0, 0, 1001, 0, 0, 1, 0, 1, 2, 0, "");
                }
            }
            else if (SkillID == 3003)
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("J_daodan");
                EffectMaker.instance.Create2DEffect("~DingDianDaoDan", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);

                List<int> FightIndex = new List<int>();
                List<int> FightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();

                int TargetID = CheckTarget(false, -1, true, MovePosition, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition + 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition - 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition + 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition - 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition + 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, true, MovePosition - 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListRolePicture[TargetID].RolePDefend - ListRolePicture[TargetID].BuffDefend));
                }
                EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 1, 1, 1, 1, -2, FightIndex, FightCate, RoleFightDamige, null, null, false, false, false, false, false, false, false, false, true, "", null);

                FightIndex.Clear();
                FightCate.Clear();
                RoleFightDamige.Clear();

                TargetID = CheckTarget(false, -1, false, MovePosition, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition + 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition - 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition + 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition - 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition + 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, false, MovePosition - 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - ListEnemyPicture[TargetID].RolePDefend - ListEnemyPicture[TargetID].BuffDefend));
                }
                EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 1, 1, 1, 1, -2, FightIndex, FightCate, RoleFightDamige, null, null, false, false, false, false, false, false, false, false, false, "", null);
            }
            else if (SkillID == 3005) //手榴弹
            {
                if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 12)
                {
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 13);
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
                    LuaDeliver.instance.UseGuideStation();
                }

                CloseSkill(SetPicture, SkillPosition);
                StartCoroutine(AudioEditer.instance.DelaySound(80f, "S_carbine"));


                List<int> FightIndex = new List<int>();
                List<int> FightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();


                int TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition + 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition - 1, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition + 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition - 5, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition + 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }
                TargetID = CheckTarget(false, -1, SetPicture[0].RolePictureMonster, MovePosition - 4, true);
                if (TargetID > -1)
                {
                    FightIndex.Add(TargetID);
                    FightCate.Add(0);
                    RoleFightDamige.Add((int)((ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[TargetID].RolePDefend - SetEnemyPicture[TargetID].BuffDefend) * ms.skillVal3));
                }


                StartCoroutine(ClearTerrain(MovePosition));
                StartCoroutine(ClearTerrain(MovePosition + 1));
                StartCoroutine(ClearTerrain(MovePosition - 1));
                StartCoroutine(ClearTerrain(MovePosition + 5));
                StartCoroutine(ClearTerrain(MovePosition - 5));
                StartCoroutine(ClearTerrain(MovePosition + 4));
                StartCoroutine(ClearTerrain(MovePosition - 4));

                for (int i = 0; i < RoleFightDamige.Count; i++)
                {
                    if (RoleFightDamige[i] < 1)
                    {
                        RoleFightDamige[i] = 100;
                    }
                }
                //FightIndex.Add(SkillRoleIndex);
                //FightCate.Add(0);
                //RoleFightDamige.Add((int)(ms.skillVal1 * SkillLevel + ms.skillVal2 - SetEnemyPicture[SkillRoleIndex].RolePDefend - SetEnemyPicture[SkillRoleIndex].BuffDefend));

                //EffectMaker.instance.Create2DEffect("Test", "blank", null, ListPosition[MovePosition], ListPosition[MovePosition], new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 0, 1, 1, 1, 0, FightIndex, FightCate, RoleFightDamige, null, false, false, false, false, false, false, false, false, false, "", null);
                EffectMaker.instance.Create2DEffect("~BuBing_Skill_02", "", null, ListPosition[MovePosition - 5], ListPosition[MovePosition], new Vector3(0.3f, 0.01f, 0.03f), 0.1f, 0, 1, 1, 1, -2, FightIndex, FightCate, RoleFightDamige, null, null, true, false, false, false, false, false, true, false, SetPicture[0].RolePictureMonster, "", null);
                EffectMaker.instance.Create2DEffect("~WF_ShouLiuDan", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0.8f, 5f, 1, 1, SkillRoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
            }
            else if (SkillID == 3008)
            {
                CloseSkill(SetPicture, SkillPosition);
                AudioEditer.instance.PlayOneShot("ui_qianghua");
                GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WF_LuZhang", typeof(GameObject)), ListPosition[MovePosition], Quaternion.identity) as GameObject;
                go.name = "TerrainObject" + MovePosition.ToString();
                go.transform.parent = FightScene.transform;
                ListStopPosition.Add(MovePosition);
            }
        }

        if (IsClose)
        {
            if (PictureCreater.instance.SkillFire1 == 1)
            {
                PictureCreater.instance.SkillFire1 = 0;
            }
            if (PictureCreater.instance.SkillFire2 == 1)
            {
                PictureCreater.instance.SkillFire2 = 0;
            }
            if (PictureCreater.instance.SkillFire3 == 1)
            {
                PictureCreater.instance.SkillFire3 = 0;
            }

            fw.Tactics.GetComponent<TweenPosition>().enabled = false;
            fw.Tactics.transform.localPosition = new Vector3(fw.Tactics.transform.localPosition.x, -10, fw.Tactics.transform.localPosition.z);


            for (int j = 0; j < PictureCreater.instance.ListRolePicture.Count; j++)
            {
                //PictureCreater.instance.ListRolePicture[j].RolePictureObject.layer = 8;
                PictureCreater.instance.ListRolePicture[j].RoleObject.GetComponent<ColliderDisplayText>().HideTarget();
                PictureCreater.instance.ListRolePicture[j].RoleRedBloodObject.SetActive(false);
                //foreach (var c in PictureCreater.instance.ListRolePicture[j].RoleObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true))
                //{
                //    c.gameObject.layer = 8;
                //}

                //foreach (var c in PictureCreater.instance.ListRolePicture[j].RoleObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                //{
                //    if (c.name != "Shadow")
                //    {
                //        c.gameObject.layer = 8;
                //    }
                //}
            }

            for (int j = 0; j < PictureCreater.instance.ListEnemyPicture.Count; j++)
            {
                //PictureCreater.instance.ListEnemyPicture[j].RolePictureObject.layer = 8;
                PictureCreater.instance.ListEnemyPicture[j].RoleObject.GetComponent<ColliderDisplayText>().HideTarget();
                PictureCreater.instance.ListEnemyPicture[j].RoleRedBloodObject.SetActive(false);
                //foreach (var c in PictureCreater.instance.ListEnemyPicture[j].RoleObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true))
                //{
                //    c.gameObject.layer = 8;
                //}

                //foreach (var c in PictureCreater.instance.ListEnemyPicture[j].RoleObject.GetComponentsInChildren(typeof(MeshRenderer), true))
                //{
                //    if (c.name != "Shadow")
                //    {
                //        c.gameObject.layer = 8;
                //    }
                //}
            }

            foreach (Component c in PictureCreater.instance.MyMoves.GetComponentsInChildren(typeof(MeshRenderer), true))
            {
                c.gameObject.renderer.material = BlankMaterial;
            }

            MyPositions.SetActive(true);
            MyBases.SetActive(false);
        }
    }

    IEnumerator ClearTerrain(int Position)
    {
        yield return new WaitForSeconds(1);
        for (int t = 0; t < ListTerrainPosition.Count; t++)
        {
            if (Position == ListTerrainPosition[t])
            {
                TextTranslator.TerrainInfo ti = TextTranslator.instance.GetTerrainInfoByID(ListTerrainID[t]);
                if (ti.terrainID == 5)
                {
                    Destroy(GameObject.Find("TerrainObject" + ListTerrainPosition[t]));
                    AudioEditer.instance.PlayOneShot("Hit_boom");
                    GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WF_ShouLiuDan", typeof(GameObject)), ListPosition[Position], Quaternion.identity) as GameObject;
                    ListTerrainID.RemoveAt(t);
                    ListTerrainPosition.RemoveAt(t);
                }
                break;
            }
        }
    }
    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(1.5f);
        IsLock = false;
    }
    IEnumerator UnlockEnemy()
    {
        if (!IsSkip)
        {
            yield return new WaitForSeconds(1.5f);
        }
        IsEnemyLock = false;
    }
    #endregion

    #region Buff
    void ResetRoleBuff(List<RolePicture> SetPicture, int RoleIndex)
    {
        int i = 0;
        List<int> RemoveList = new List<int>();
        foreach (var b in SetPicture[RoleIndex].ListBuff)
        {
            List<int> RoleFightIndex = new List<int>();
            List<int> RoleFightCate = new List<int>();
            List<int> RoleFightDamige = new List<int>();

            b.round--;
            if (b.round < 1)
            {
                RemoveList.Add(i);
            }
            switch (b.buffType)
            {
                case 1: //免伤提升
                    break;
                case 2: //闪避减少
                    break;
                case 3: //防御力减少
                    break;
                case 4: //中毒
                    ////////////////////////////Buff参数计算（以下）////////////////////////////
                    RoleFightIndex.Add(RoleIndex);
                    RoleFightCate.Add(0);
                    RoleFightDamige.Add((int)SetPicture[RoleIndex].BuffPoison);

                    if (SetPicture[RoleIndex].RoleNowBlood > 0)
                    {
                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, !SetPicture[RoleIndex].RolePictureMonster, "", null);
                    }
                    ////////////////////////////Buff参数计算（以上）////////////////////////////
                    break;
                case 5: //燃烧
                    ////////////////////////////Buff参数计算（以下）////////////////////////////
                    RoleFightIndex.Add(RoleIndex);
                    RoleFightCate.Add(0);
                    RoleFightDamige.Add((int)SetPicture[RoleIndex].BuffBurn);

                    if (SetPicture[RoleIndex].RoleNowBlood > 0)
                    {
                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, !SetPicture[RoleIndex].RolePictureMonster, "", null);
                    }
                    ////////////////////////////Buff参数计算（以上）////////////////////////////
                    break;
                case 6: //减少怒气
                    break;
                case 7: //攻击力减少
                    break;
                case 8: //眩晕
                    break;
                case 9: //定身
                    SetPicture[RoleIndex].RoleMoveNowStep = 0;
                    break;
                case 10: //命中减少
                    break;
                case 11: //攻击力提升
                    break;
                case 12: //被嘲讽
                    SetPicture[RoleIndex].RoleTargetIndex = SetPicture[RoleIndex].BuffTargetIndex;
                    break;
                case 13: //减速
                    break;
                case 14: //反伤
                    break;
                case 15: //爆击提升
                    break;
                case 16: //免疫一切减益效果
                    break;
                case 17: //隐身
                    break;
                case 18: //防御力提升
                    break;
                case 19: //命中提升
                    break;
                case 20: //闪避提升
                    break;
                case 21: //抗爆提升
                    break;
                case 22: //加伤提升
                    break;
                case 23: //恢复
                    ////////////////////////////Buff参数计算（以下）////////////////////////////
                    RoleFightIndex.Add(RoleIndex);
                    RoleFightCate.Add(0);
                    RoleFightDamige.Add((int)-b.parameter1);

                    if (SetPicture[RoleIndex].RoleNowBlood > 0)
                    {
                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[RoleIndex].RolePictureMonster, "", null);
                    }
                    ////////////////////////////Buff参数计算（以上）////////////////////////////
                    break;
                case 24: //爆击减少
                    break;
                case 25: //抗爆减少
                    break;
                case 26: //加伤减少
                    break;
                case 27: //免伤减少
                    break;
                case 28: //减少治疗效果
                    break;
                case 29: //治疗效果提升
                    break;
                case 30: //抵消伤害护盾
                    break;
                case 31: //隐身+易伤+攻击UP
                    break;
                case 32: //集火
                    break;
                case 33: //移除负面
                    break;
                case 34: //沉默
                    break;
                case 35: //回复怒气
                    break;
                case 36: //伤害减少
                    break;
                case 37: //加血
                    break;
                case 38: //减血
                    break;
            }
            i++;
        }

        for (i = RemoveList.Count - 1; i >= 0; i--)
        {
            if (SetPicture[RoleIndex].ListBuff[RemoveList[i]].buffType == 31 && !SetPicture[RoleIndex].IsPicture)
            {
                SetPicture[RoleIndex].RolePictureObject.GetComponent<Animator>().SetFloat("box", 2);
            }
            if (SetPicture[RoleIndex].ListBuff[RemoveList[i]].buffType == 17)
            {
                SetPicture[RoleIndex].RolePictureObject.transform.Find("HeroBody").gameObject.renderer.material = (Material)Resources.Load("Materials/Ada");
                SetPicture[RoleIndex].RolePictureObject.transform.Find("Object001").gameObject.renderer.material = (Material)Resources.Load("Materials/Ada");
            }
            SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().RemoveSetBuff(SetPicture[RoleIndex].ListBuff[RemoveList[i]].buffIcon);
            DestroyImmediate(SetPicture[RoleIndex].ListBuff[RemoveList[i]].effectObject);
            SetPicture[RoleIndex].ListBuff.RemoveAt(RemoveList[i]);


            if (SetPicture[RoleIndex].RolePictureMonster)
            {
                foreach (var r in ListRolePicture)
                {
                    if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                    {
                        Destroy(GameObject.Find("WenHao" + r.RoleObject.name));
                    }
                }
            }
            else
            {
                foreach (var r in ListEnemyPicture)
                {
                    if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                    {
                        Destroy(GameObject.Find("WenHao" + r.RoleObject.name));
                    }
                }
            }
        }

        CalculateRoleBuff(SetPicture, RoleIndex);
    }
    void RemoveRoleBuff(List<RolePicture> SetPicture, int RoleIndex)
    {
        foreach (var b in SetPicture[RoleIndex].ListBuff)
        {
            SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().RemoveSetBuff(b.buffIcon);
            DestroyImmediate(b.effectObject);
        }

        SetPicture[RoleIndex].ListBuff.Clear();
        CalculateRoleBuff(SetPicture, RoleIndex);
    }
    void CalculateRoleBuff(List<RolePicture> SetPicture, int RoleIndex)
    {
        SetPicture[RoleIndex].BuffAttack = 0;
        SetPicture[RoleIndex].BuffDefend = 0;
        SetPicture[RoleIndex].BuffHp = 0;
        SetPicture[RoleIndex].BuffNoHp = 0;

        SetPicture[RoleIndex].BuffFightSpeed = 0;

        SetPicture[RoleIndex].BuffHit = 0;
        SetPicture[RoleIndex].BuffNoHit = 0;
        SetPicture[RoleIndex].BuffCrit = 0;
        SetPicture[RoleIndex].BuffNoCrit = 0;
        SetPicture[RoleIndex].BuffDamigeAdd = 0;
        SetPicture[RoleIndex].BuffDamigeReduce = 0;
        SetPicture[RoleIndex].BuffPoison = 0; //中毒
        SetPicture[RoleIndex].BuffBurn = 0; //燃烧

        SetPicture[RoleIndex].BuffHurtAdd = 0;
        SetPicture[RoleIndex].BuffBackDamige = 0;
        SetPicture[RoleIndex].BuffAvoidDebuff = 0; //免疫伤害        
        SetPicture[RoleIndex].BuffTargetIndex = -1; //嘲讽        
        SetPicture[RoleIndex].BuffSkillPoint = 0; //怒气

        SetPicture[RoleIndex].BuffAvoidDead = false; //免疫伤害
        SetPicture[RoleIndex].BuffTotalAttack = false; //集火
        SetPicture[RoleIndex].BuffInvisible = false; //隐身
        SetPicture[RoleIndex].BuffAvoidDead = false; //不死
        SetPicture[RoleIndex].BuffStop = false; //眩晕
        SetPicture[RoleIndex].BuffNoMove = false; //定身
        SetPicture[RoleIndex].BuffSilence = false; //沉默

        int i = 0;
        int j = 0;
        List<int> RemoveList = new List<int>();

        foreach (var b in SetPicture[RoleIndex].ListBuff)
        {
            if (b.round > -1)
            {
                List<int> RoleFightIndex = new List<int>();
                List<int> RoleFightCate = new List<int>();
                List<int> RoleFightDamige = new List<int>();

                switch (b.buffType)
                {
                    case 1: //免伤提升
                        SetPicture[RoleIndex].BuffDamigeReduce += b.parameter1 * 1000;
                        break;
                    case 2: //闪避减少
                        SetPicture[RoleIndex].BuffNoHit -= b.parameter1;
                        break;
                    case 3: //防御力减少
                        SetPicture[RoleIndex].BuffDefend -= b.parameter1;
                        break;
                    case 4: //中毒
                        SetPicture[RoleIndex].BuffPoison = b.parameter1;
                        break;
                    case 5: //燃烧
                        SetPicture[RoleIndex].BuffBurn = b.parameter1;
                        break;
                    case 6: //减少怒气
                        b.round--;
                        SetPicture[RoleIndex].RoleSkillPoint -= (int)b.parameter1;
                        SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[RoleIndex].RoleSkillPoint < 0 ? 0 : SetPicture[RoleIndex].RoleSkillPoint);
                        break;
                    case 7: //攻击力减少
                        SetPicture[RoleIndex].BuffAttack -= b.parameter1;
                        break;
                    case 8: //眩晕                        
                        SetPicture[RoleIndex].BuffStop = true;
                        if (SetPicture[RoleIndex].RoleSkill2 == 2044) //魔免
                        {
                            if (GetRandom(SetPicture[RoleIndex].RoleID) < 90)
                            {
                                SetPicture[RoleIndex].BuffStop = false;
                                if (!RemoveList.Contains(i))
                                {
                                    RemoveList.Add(i);
                                }
                            }
                        }
                        break;
                    case 9: //定身
                        SetPicture[RoleIndex].BuffNoMove = true;
                        if (SetPicture[RoleIndex].RoleSkill2 == 2044) //魔免
                        {
                            if (GetRandom(SetPicture[RoleIndex].RoleID) < 90)
                            {
                                SetPicture[RoleIndex].BuffNoMove = false;
                                if (!RemoveList.Contains(i))
                                {
                                    RemoveList.Add(i);
                                }
                            }
                        }
                        break;
                    case 10: //命中减少
                        SetPicture[RoleIndex].BuffHit -= b.parameter1;
                        break;
                    case 11: //攻击力提升
                        SetPicture[RoleIndex].BuffAttack += b.parameter1;
                        break;
                    case 12: //被嘲讽
                        SetPicture[RoleIndex].BuffTargetIndex = (int)b.parameter1;
                        break;
                    case 13: //减速
                        SetPicture[RoleIndex].BuffFightSpeed += b.parameter1;
                        break;
                    case 14: //反伤
                        SetPicture[RoleIndex].BuffBackDamige += b.parameter1;
                        break;
                    case 15: //爆击提升
                        SetPicture[RoleIndex].BuffCrit += b.parameter1;
                        break;
                    case 16: //免疫一切减益效果
                        SetPicture[RoleIndex].BuffAvoidDebuff = 1;
                        break;
                    case 17: //隐身
                        SetPicture[RoleIndex].BuffInvisible = true;

                        if (SetPicture[RoleIndex].RolePictureMonster)
                        {
                            foreach (var r in ListRolePicture)
                            {
                                if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                                {
                                    if (GameObject.Find("WenHao" + r.RoleObject.name) == null)
                                    {
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WenHao", typeof(GameObject)), PictureCreater.instance.ListPosition[r.RolePosition] + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
                                        go.name = "WenHao" + r.RoleObject.name;
                                        go.transform.parent = r.RoleObject.transform;
                                        go.transform.localScale = Vector3.one;
                                        go.transform.localPosition = new Vector3(0, 0.33f, 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var r in ListEnemyPicture)
                            {
                                if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                                {
                                    if (GameObject.Find("WenHao" + r.RoleObject.name) == null)
                                    {
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WenHao", typeof(GameObject)), PictureCreater.instance.ListPosition[r.RolePosition] + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
                                        go.name = "WenHao" + r.RoleObject.name;
                                        go.transform.parent = r.RoleObject.transform;
                                        go.transform.localScale = Vector3.one;
                                        go.transform.localPosition = new Vector3(0, 0.33f, 0);
                                    }
                                }
                            }
                        }

                        break;
                    case 18: //防御力提升
                        SetPicture[RoleIndex].BuffDefend += b.parameter1;
                        break;
                    case 19: //命中提升
                        SetPicture[RoleIndex].BuffHit += b.parameter1;
                        break;
                    case 20: //闪避提升
                        SetPicture[RoleIndex].BuffNoHit += b.parameter1;
                        break;
                    case 21: //抗爆提升
                        SetPicture[RoleIndex].BuffNoCrit += b.parameter1;
                        break;
                    case 22: //加伤提升
                        SetPicture[RoleIndex].BuffDamigeAdd += b.parameter1 * 1000;
                        break;
                    case 23: //恢复
                        SetPicture[RoleIndex].BuffHp += b.parameter1;
                        break;
                    case 24: //爆击减少
                        SetPicture[RoleIndex].BuffCrit -= b.parameter1;
                        break;
                    case 25: //抗爆减少
                        SetPicture[RoleIndex].BuffNoCrit -= b.parameter1;
                        break;
                    case 26: //加伤减少
                        SetPicture[RoleIndex].BuffDamigeAdd -= b.parameter1 * 1000;
                        break;
                    case 27: //免伤减少
                        SetPicture[RoleIndex].BuffDamigeReduce -= b.parameter1 * 1000;
                        break;
                    case 28: //减少治疗效果
                        SetPicture[RoleIndex].BuffNoHp += b.parameter1;
                        break;
                    case 29: //治疗效果提升
                        SetPicture[RoleIndex].BuffNoHp -= b.parameter1;
                        break;
                    case 30: //抵消伤害护盾
                        SetPicture[RoleIndex].BuffAvoidCount = (int)b.parameter1;
                        break;
                    case 31: //隐身+易伤+攻击UP
                        SetPicture[RoleIndex].BuffInvisible = true;
                        SetPicture[RoleIndex].BuffHurtAdd += b.parameter1;

                        if (SetPicture[RoleIndex].RolePictureMonster)
                        {
                            foreach (var r in ListRolePicture)
                            {
                                if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                                {
                                    if (GameObject.Find("WenHao" + r.RoleObject.name) == null)
                                    {
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WenHao", typeof(GameObject)), PictureCreater.instance.ListPosition[r.RolePosition] + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
                                        go.name = "WenHao" + r.RoleObject.name;
                                        go.transform.parent = r.RoleObject.transform;
                                        go.transform.localScale = Vector3.one;
                                        go.transform.localPosition = new Vector3(0, 0.33f, 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var r in ListEnemyPicture)
                            {
                                if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                                {
                                    if (GameObject.Find("WenHao" + r.RoleObject.name) == null)
                                    {
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WenHao", typeof(GameObject)), PictureCreater.instance.ListPosition[r.RolePosition] + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
                                        go.name = "WenHao" + r.RoleObject.name;
                                        go.transform.parent = r.RoleObject.transform;
                                        go.transform.localScale = Vector3.one;
                                        go.transform.localPosition = new Vector3(0, 0.33f, 0);
                                    }
                                }
                            }
                        }
                        break;
                    case 32: //集火
                        if (SetPicture[0].RolePictureMonster)
                        {
                            foreach (var p in ListRolePicture)
                            {
                                if (p.RoleArea != 1022 && !p.RoleTargetObject.activeSelf)
                                {
                                    p.BuffTargetIndex = (int)b.parameter1;
                                    p.RoleTargetIndex = p.BuffTargetIndex;
                                }
                            }
                        }
                        else
                        {
                            foreach (var p in ListEnemyPicture)
                            {
                                if (p.RoleArea != 1022 && !p.RoleTargetObject.activeSelf)
                                {
                                    p.BuffTargetIndex = (int)b.parameter1;
                                    p.RoleTargetIndex = p.BuffTargetIndex;
                                }
                            }
                        }
                        break;
                    case 33: //移除负面
                        foreach (var rb in SetPicture[RoleIndex].ListBuff)
                        {
                            j = 0;
                            if (rb.action == 2)
                            {
                                if (!RemoveList.Contains(j))
                                {
                                    RemoveList.Add(j);
                                }
                            }
                            j++;
                        }
                        break;
                    case 34: //沉默
                        SetPicture[RoleIndex].BuffSilence = true;
                        if (SetPicture[RoleIndex].RoleSkill2 == 2044) //魔免
                        {
                            if (GetRandom(SetPicture[RoleIndex].RoleID) < 90)
                            {
                                SetPicture[RoleIndex].BuffSilence = false;
                                if (!RemoveList.Contains(i))
                                {
                                    RemoveList.Add(i);
                                }
                            }
                        }
                        break;
                    case 35: //回复怒气
                        b.round--;
                        SetPicture[RoleIndex].RoleSkillPoint += (int)b.parameter1;
                        SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[RoleIndex].RoleSkillPoint);
                        break;
                    case 36: //伤害减少
                        SetPicture[RoleIndex].BuffDamigeAdd -= b.parameter1 * 1000;
                        break;
                    case 37: //加血
                        ////////////////////////////Buff参数计算（以下）////////////////////////////
                        RoleFightIndex.Add(RoleIndex);
                        RoleFightCate.Add(0);
                        RoleFightDamige.Add(-(int)(SetPicture[RoleIndex].RoleMaxBlood * b.parameter1));

                        if (SetPicture[RoleIndex].RoleNowBlood > 0)
                        {
                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[RoleIndex].RolePictureMonster, "", null);
                        }
                        b.parameter1 = 0;
                        ////////////////////////////Buff参数计算（以上）////////////////////////////
                        break;
                    case 38: //减血
                        ////////////////////////////Buff参数计算（以下）////////////////////////////
                        RoleFightIndex.Add(RoleIndex);
                        RoleFightCate.Add(0);
                        RoleFightDamige.Add((int)(SetPicture[RoleIndex].RoleMaxBlood * b.parameter1));

                        if (SetPicture[RoleIndex].RoleNowBlood > 0)
                        {
                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, !SetPicture[RoleIndex].RolePictureMonster, "", null);
                        }
                        b.parameter1 = 0;
                        ////////////////////////////Buff参数计算（以上）////////////////////////////
                        break;
                    case 40: //不死
                        SetPicture[RoleIndex].BuffAvoidDead = true;
                        break;
                }
            }
            else
            {
                if (!RemoveList.Contains(i))
                {
                    RemoveList.Add(i);
                }
            }
            i++;
        }

        for (i = RemoveList.Count - 1; i >= 0; i--)
        {
            SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().RemoveSetBuff(SetPicture[RoleIndex].ListBuff[RemoveList[i]].buffIcon);
            DestroyImmediate(SetPicture[RoleIndex].ListBuff[RemoveList[i]].effectObject);
            SetPicture[RoleIndex].ListBuff.RemoveAt(RemoveList[i]);
        }
    }
    public void RoleAddBuff(List<RolePicture> SetPicture, int RoleIndex, Buff AddBuff)
    {
        Buff NewBuff = new Buff(AddBuff);


        //扺抗   10不受debuff
        Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(10, SetPicture[RoleIndex].RoleInnate[9]);
        if (SetInnate != null && NewBuff.buffType == 2)
        {
            if (GetRandom(SetPicture[RoleIndex].RoleID) < SetInnate.Value1)
            {
                return;
            }
        }


        if (NewBuff.stack == 1)
        {
            foreach (var b in SetPicture[RoleIndex].ListBuff)
            {
                if (b.buffID == NewBuff.buffID)
                {

                    if (SetPicture[RoleIndex].RolePosition > 0 && SetPicture[RoleIndex].RoleNowBlood > 0)
                    {
                        b.AddBuff(NewBuff);
                        SetPicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                        SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuff(NewBuff.buffIcon);

                        CalculateRoleBuff(SetPicture, RoleIndex);
                        if (NewBuff.startEffect != "0")
                        {
                            GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/" + NewBuff.startEffect, typeof(GameObject)), SetPicture[RoleIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                            go.transform.parent = SetPicture[RoleIndex].RolePictureObject.transform;
                        }
                    }

                    return;
                }
            }
        }
        else
        {
            foreach (var b in SetPicture[RoleIndex].ListBuff)
            {
                if (b.buffID == NewBuff.buffID)
                {
                    if (SetPicture[RoleIndex].RolePosition > 0 && SetPicture[RoleIndex].RoleNowBlood > 0)
                    {
                        b.ResetBuff(NewBuff);
                        SetPicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                        SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuff(NewBuff.buffIcon);

                        CalculateRoleBuff(SetPicture, RoleIndex);
                        if (NewBuff.startEffect != "0")
                        {
                            GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/" + NewBuff.startEffect, typeof(GameObject)), SetPicture[RoleIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                            go.transform.parent = SetPicture[RoleIndex].RolePictureObject.transform;
                        }
                    }

                    return;
                }
            }
        }

        switch (NewBuff.buffType)
        {
            case 4:
                if (SetPicture[RoleIndex].RoleBio != 1)
                {
                    return;
                }
                break;
        }

        if (NewBuff.effectName != "0")
        {
            //Debug.LogError(NewBuff.effectName);
            NewBuff.effectObject = GameObject.Instantiate(Resources.Load("Prefab/Effect/" + NewBuff.effectName, typeof(GameObject)), SetPicture[RoleIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
            NewBuff.effectObject.transform.parent = SetPicture[RoleIndex].RolePictureObject.transform;
            NewBuff.effectObject.name = "BuffEffect";
        }
        if (NewBuff.startEffect != "0")
        {
            GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/" + NewBuff.startEffect, typeof(GameObject)), SetPicture[RoleIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
            go.AddComponent<DestroySelf>();
            go.name = "AddBuffEffect";
        }
        if (SetPicture[RoleIndex].ListBuff.Count > 0)
        {
            if (SetPicture[RoleIndex].ListBuff[0].des == "1")
            {
                if (NewBuff.des == "1")
                {
                    if (SetPicture[RoleIndex].ListBuff.Count > 0)
                    {
                        SetPicture[RoleIndex].ListBuff[0].effectObject.AddComponent<ShowMe>();
                        NewBuff.effectObject.AddComponent<ShowMe>().SetShowTimer(1f);
                    }
                }
            }
        }


        if (SetPicture[RoleIndex].RolePosition > 0 && SetPicture[RoleIndex].RoleNowBlood > 0)
        {
            SetPicture[RoleIndex].ListBuff.Add(NewBuff);
            SetPicture[RoleIndex].RoleRedBloodObject.SetActive(true);
            SetPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuff(NewBuff.buffIcon);
            CalculateRoleBuff(SetPicture, RoleIndex);
        }
    }

    List<Buff> GetActiveBuff(List<RolePicture> SetPicture, int RoleIndex, int SkillID, int SkillLevel)
    {
        List<Buff> ActiveBuff = new List<Buff>();
        Skill ActiveSkill = TextTranslator.instance.GetSkillByID(SkillID, SkillLevel);
        int ActiveBuffID = ActiveSkill.BuffID;
        Buff NewBuff = null;
        if (ActiveBuffID != 0)
        {
            NewBuff = TextTranslator.instance.GetBuffByID(ActiveBuffID);
            Buff AddBuff = new Buff(NewBuff);
            if (AddBuff != null)
            {

                switch (AddBuff.buffType)
                {
                    case 4: //中毒
                        AddBuff.parameter1 = SetPicture[RoleIndex].RolePAttack * AddBuff.parameter1;
                        break;
                    case 5: //燃烧
                        AddBuff.parameter1 = SetPicture[RoleIndex].RolePAttack * AddBuff.parameter1;
                        break;
                }

                if (AddBuff.target == 0)
                {
                    RoleAddBuff(SetPicture, RoleIndex, AddBuff);
                }
                else
                {
                    ActiveBuff.Add(AddBuff);
                }
            }
        }


        List<int> RoleFightIndex = new List<int>();
        List<int> RoleFightCate = new List<int>();
        List<int> RoleFightDamige = new List<int>();

        switch (SkillID) //技能特别效果
        {
            case 1003:
                //RoleFightIndex.Add(RoleIndex);
                //RoleFightCate.Add(0);
                //RoleFightDamige.Add((int)(-ActiveSkill.skillVal2 / 100f * SetPicture[RoleIndex].RolePAttack));
                //EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[RoleIndex].RolePictureMonster, "", null);

                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillDuration1);
                if (NewBuff != null)
                {
                    RoleAddBuff(SetPicture, RoleIndex, NewBuff);
                }
                break;

            case 1025: //巴尼吸攻吸防
                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillDuration1);
                if (NewBuff != null)
                {
                    ActiveBuff.Add(NewBuff);
                }
                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                if (NewBuff != null)
                {
                    ActiveBuff.Add(NewBuff);
                }
                break;
            case 1026: //古烈减攻减防
                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillDuration1);
                if (NewBuff != null)
                {
                    ActiveBuff.Add(NewBuff);
                }
                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                if (NewBuff != null)
                {
                    ActiveBuff.Add(NewBuff);
                }
                break;
            case 1028: //威斯克回血

                RoleFightIndex.Add(RoleIndex);
                RoleFightCate.Add(0);
                if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 13)
                {
                    RoleFightDamige.Add((int)(-0.68f * SetPicture[RoleIndex].RoleMaxBlood));
                }
                else
                {
                    RoleFightDamige.Add((int)(-0.08f * SetPicture[RoleIndex].RoleMaxBlood));
                }
                EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 1.5f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[RoleIndex].RolePictureMonster, "", null);
                break;

            case 1030: //欧巴削弱治疗
                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                if (NewBuff != null)
                {
                    ActiveBuff.Add(NewBuff);
                }
                break;

            //case 1033:
            //    RoleFightIndex.Add(RoleIndex);
            //    RoleFightCate.Add(0);
            //    RoleFightDamige.Add((int)(-ActiveSkill.skillVal2 / 100f * SetPicture[RoleIndex].RoleMaxBlood));
            //    EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position, SetPicture[RoleIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 1.5f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[RoleIndex].RolePictureMonster, "", null);
            //    break;
            case 1035: //阴阳给全体加盾
                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillVal2);
                if (NewBuff != null)
                {
                    for (int i = 0; i < SetPicture.Count; i++)
                    {
                        if (SetPicture[i].RolePosition > 0 && SetPicture[i].RoleNowBlood > 0)
                        {
                            RoleAddBuff(SetPicture, i, new Buff(NewBuff));
                        }
                    }
                }
                break;
            case 1046: //蓝博不死
                NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillVal3);
                if (NewBuff != null)
                {
                    RoleAddBuff(SetPicture, RoleIndex, new Buff(NewBuff));
                }
                break;
        }

        return ActiveBuff;
    }
    void SetEffect(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, FightMotion fm, List<int> TargetIndex, bool isShowSkill)
    {
        Buff PostiveBuff = null;
        ////////////////设定敌方buff////////////////////
        List<Buff> ActiveBuff = new List<Buff>();
        if (isShowSkill)
        {
            if (SetPicture[RoleIndex].RoleSkill1 != 0)
            {
                List<Buff> NewBuff = GetActiveBuff(SetPicture, RoleIndex, SetPicture[RoleIndex].RoleSkill1, SetPicture[RoleIndex].RoleSkillLevel1);
                if (NewBuff != null)
                {
                    foreach (var b in NewBuff)
                    {
                        ActiveBuff.Add(b);
                    }
                }

                foreach (var b in SetListBuff)
                {
                    ActiveBuff.Add(b);
                }
            }
        }
        /////////////////设定敌方buff///////////////////

        for (int i = 0; i < fm.effectList.size; i++)
        {
            FightEffect fe = TextTranslator.instance.fightEffectDic[fm.effectList[i]];

            if (!IsSkip)
            {
                if (fe.effect != "0")
                {
                    if (fe.projectile == 9999)
                    {
                        if (fe.effectParent != "0")
                        {
                            EffectMaker.instance.Create2DEffect("~" + fe.effect, "", FindChildObject(SetPicture[RoleIndex].RoleObject, fe.effectParent), new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), MyCamera.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, "", null);
                        }
                        else
                        {
                            EffectMaker.instance.Create2DEffect("~" + fe.effect, "", null, new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), new Vector3(MyCamera.transform.position.x, MyCamera.transform.position.y, 0), MyCamera.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, "", null);
                        }
                    }
                    else
                    {
                        if (fe.effectParent != "0")
                        {
                            EffectMaker.instance.Create2DEffect("~" + fe.effect, "", FindChildObject(SetPicture[RoleIndex].RoleObject, fe.effectParent), SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(SetPicture[RoleIndex].RolePictureFaceRight * fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, "", null);
                        }
                        else
                        {
                            EffectMaker.instance.Create2DEffect("~" + fe.effect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(SetPicture[RoleIndex].RolePictureFaceRight * fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RoleObject.transform.position + new Vector3(fe.effectPosX / 100f, 0, fe.effectPosY / 100f), SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, 0.04f, fm.effectTimeList[i] / 100f, 8, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, "", null);
                        }
                    }
                }
                StartCoroutine(AudioEditer.instance.DelaySound(fe.soundDelay, fe.sound));
                StartCoroutine(SharkCamera(fe.shakeDelay, fe.shakeTime, fe.shake, fe.shakeRange));
            }

            if (fe.projectile > 0 && fe.projectile < 9999)
            {
                FightProjectile fp = TextTranslator.instance.fightProjectileDic[fe.projectile];

                Vector3 RoleOffst = new Vector3(fp.projectilePosX / 100f, fp.projectilePosZ / 100f, fp.projectilePosY / 100f);
                Vector3 EnemyOffst = new Vector3(fp.touchEffectPosX / 100f, fp.projectilePosZ / 100f, fp.projectilePosY / 100f);
                Quaternion rotation = Quaternion.Euler(new Vector3(0, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles.y, 0) + new Vector3(0, -90, 0));
                RoleOffst = rotation * RoleOffst;
                EnemyOffst = rotation * EnemyOffst;

                if (fp.projectileEffect == "0")
                {
                    if (fp.touchMode == 1)
                    {
                        for (int t = 0; t < TargetIndex.Count; t++)
                        {
                            List<int> FightIndex = new List<int>();
                            List<int> FightCate = new List<int>();
                            List<int> RoleFightDamige = new List<int>();

                            FightIndex.Add(SetPicture[RoleIndex].RoleFightIndex[t]);
                            FightCate.Add(SetPicture[RoleIndex].RoleFightType[t]);
                            RoleFightDamige.Add(SetPicture[RoleIndex].RoleFightDamige[t]);

                            if (fp.allyFlag == 1)
                            {
                                EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), new Vector3(0.5f, 0.5f, 0.5f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                            }
                            else
                            {
                                if (fp.summonFlag == 1)
                                {
                                    if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                    {
                                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                    }
                                    else
                                    {
                                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                    }
                                }
                                else if (fp.summonFlag == 2)
                                {
                                    if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                    {
                                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                    }
                                    else
                                    {
                                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                    }
                                }
                                else if (fp.summonFlag == 3)
                                {
                                    if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                    {
                                        if (t == 0)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                        }
                                        else if (t == 1 && TargetIndex.Count > 2)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, 0.6f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                        }
                                        else if (t == 2 && TargetIndex.Count > 2)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.4f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                        }
                                        else if (t == 1)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, 0.6f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                        }
                                    }
                                    else
                                    {
                                        if (t == 0)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                        }
                                        else if (t == 1 && TargetIndex.Count > 2)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, 0.6f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                        }
                                        else if (t == 2 && TargetIndex.Count > 2)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.4f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                        }
                                        else if (t == 1)
                                        {
                                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, 0.6f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                        }
                                    }
                                }
                                else
                                {
                                    if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                    {
                                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                    }
                                    else
                                    {
                                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                    }
                                }
                            }
                        }
                    }
                    else if (fp.touchMode == 2)
                    {
                        if (fp.allyFlag == 1)
                        {
                            EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), new Vector3(0.5f, 0.5f, 0.5f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                        }
                        else
                        {
                            if (fp.summonFlag == 1)
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("Test", "blank", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("Test", "blank", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                            else if (fp.summonFlag == 2)
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                            else
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("Test", "blank", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (fp.touchMode == 1)
                    {
                        for (int t = 0; t < TargetIndex.Count; t++)
                        {
                            List<int> FightIndex = new List<int>();
                            List<int> FightCate = new List<int>();
                            List<int> RoleFightDamige = new List<int>();

                            FightIndex.Add(SetPicture[RoleIndex].RoleFightIndex[t]);
                            FightCate.Add(SetPicture[RoleIndex].RoleFightType[t]);
                            RoleFightDamige.Add(SetPicture[RoleIndex].RoleFightDamige[t]);

                            if (fp.allyFlag == 1)
                            {
                                EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), new Vector3(0.5f, 0.5f, 0.5f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                            }
                            else
                            {
                                if (fp.summonFlag == 1)
                                {
                                    if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                    {
                                        EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                    }
                                    else
                                    {
                                        EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                    }
                                }
                                else if (fp.summonFlag == 2)
                                {
                                    if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                    {
                                        EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                    }
                                    else
                                    {
                                        EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                    }
                                }
                                else
                                {
                                    if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                    {
                                        EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                    }
                                    else
                                    {
                                        EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f + 0.01f * t, fp.distroy / 10f, 1, 1, RoleIndex, FightIndex, FightCate, RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                    }
                                }
                            }
                        }
                    }
                    else if (fp.touchMode == 2)
                    {
                        if (fp.allyFlag == 1)
                        {
                            EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), new Vector3(0.5f, 0.5f, 0.5f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                        }
                        else
                        {
                            if (fp.summonFlag == 1)
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                            else if (fp.summonFlag == 2)
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                            else
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (fp.allyFlag == 1)
                        {
                            EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), SetPicture[TargetIndex[0]].RoleObject.transform.position + new Vector3(SetPicture[TargetIndex[0]].RolePictureFaceRight * fp.projectilePosX / 100f, fp.projectilePosY / 100f, 0), new Vector3(0.5f, 0.5f, 0.5f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, SetPicture[RoleIndex].RoleFightIndex, SetPicture[RoleIndex].RoleFightType, SetPicture[RoleIndex].RoleFightDamige, ActiveBuff, PostiveBuff, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                        }
                        else
                        {
                            if (fp.summonFlag == 1)
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                            else if (fp.summonFlag == 2)
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, null, null, null, null, null, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, new Vector3(0.3f, 0.01f, 0.03f), fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, null, null, null, null, null, true, false, false, false, false, false, true, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                            else
                            {
                                if (SetEnemyPicture[TargetIndex[0]].RoleBio == 1)
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect, fp);
                                }
                                else
                                {
                                    EffectMaker.instance.Create2DEffect("~" + fp.projectileEffect, "", null, SetPicture[RoleIndex].RoleObject.transform.position + RoleOffst, SetEnemyPicture[TargetIndex[0]].RoleObject.transform.position + EnemyOffst, SetPicture[RoleIndex].RolePictureObject.transform.localEulerAngles, fp.projectileSpeed / 100f, fp.effectDelay / 100f, fp.distroy / 10f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, false, false, false, false, isShowSkill, SetPicture[RoleIndex].RolePictureMonster, fp.touchEffect2, fp);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region Target
    int CheckTarget(bool IsVisible, int NewTargetIndex, bool IsMonster, int PositionID, bool IsManualSkill)
    {
        if (IsMonster)
        {
            if (NewTargetIndex > -1)
            {
                if (ListRolePicture[NewTargetIndex].RolePosition == PositionID && (ListRolePicture[NewTargetIndex].RolePictureAttackable || IsManualSkill))
                {
                    if (IsVisible && ListRolePicture[NewTargetIndex].BuffInvisible)
                    {

                    }
                    else
                    {
                        return NewTargetIndex;
                    }
                }
            }
            else
            {
                for (int k = 0; k < ListRolePicture.Count; k++)
                {
                    if (ListRolePicture[k].RolePosition == PositionID && (ListRolePicture[k].RolePictureAttackable || IsManualSkill))
                    {
                        //Debug.Log(k + " " + ListRolePicture[k].RolePictureAttackable);
                        if (PositionID != 0 && ListRolePicture[k].RoleNowBlood > 0)
                        {
                            if (IsVisible && ListRolePicture[k].BuffInvisible)
                            {

                            }
                            else
                            {
                                return k;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //Debug.Log(NewTargetIndex + " " + PositionID);
            if (NewTargetIndex > -1)
            {
                if (ListEnemyPicture.Count > NewTargetIndex)
                {
                    if (ListEnemyPicture[NewTargetIndex].RolePosition == PositionID && (ListEnemyPicture[NewTargetIndex].RolePictureAttackable || IsManualSkill))
                    {
                        if (IsVisible && ListEnemyPicture[NewTargetIndex].BuffInvisible)
                        {

                        }
                        else
                        {
                            return NewTargetIndex;
                        }
                    }
                }
                else
                {
                    return NewTargetIndex;
                }
            }
            else
            {
                for (int k = 0; k < ListEnemyPicture.Count; k++)
                {
                    //Debug.Log(k + " " + ListEnemyPicture[k].RolePosition + " " + PositionID + " " + ListEnemyPicture[k].RolePictureAttackable);
                    if (ListEnemyPicture[k].RolePosition == PositionID && (ListEnemyPicture[k].RolePictureAttackable || IsManualSkill))
                    {
                        //Debug.Log(k + " " + ListRolePicture[k].RolePictureAttackable + " " + ListEnemyPicture[k].RoleNowBlood + " " + IsVisible + " " + ListEnemyPicture[k].BuffInvisible);
                        if (PositionID != 0 && ListEnemyPicture[k].RoleNowBlood > 0)
                        {
                            if (IsVisible && ListEnemyPicture[k].BuffInvisible)
                            {

                            }
                            else
                            {
                                return k;
                            }
                        }
                    }
                }
            }
        }
        return -1;
    }
    List<int> CheckDirectCenter(int NewTargetIndex, List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, int Position, int Distance, int Scope)
    {
        List<int> TargetIndex = new List<int>();
        int SetTargetIndex = -1;
        int SubPosition = (Position - SetPicture[RoleIndex].RolePosition) / Distance;
        switch (PositionRow)
        {
            case 3:
                for (int i = 1; i <= Scope; i++)
                {
                    if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + SubPosition * i, false)) > -1)
                    {
                        switch (SubPosition)
                        {
                            case 3:
                                AttackDirection = 1;
                                break;
                            case -3:
                                AttackDirection = 2;
                                break;
                            case 1:
                                AttackDirection = 3;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case -2:
                                AttackDirection = 4;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case 2:
                                AttackDirection = 5;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case -1:
                                AttackDirection = 6;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                        }
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, RoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;
            case 4:
                for (int i = 1; i <= Scope; i++)
                {
                    if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + SubPosition * i, false)) > -1)
                    {
                        switch (SubPosition)
                        {
                            case 4:
                                AttackDirection = 1;
                                break;
                            case -4:
                                AttackDirection = 2;
                                break;
                            case 1:
                                AttackDirection = 3;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case -3:
                                AttackDirection = 4;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case 3:
                                AttackDirection = 5;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case -1:
                                AttackDirection = 6;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                        }
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, RoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;
            case 5:
                for (int i = 1; i <= Scope; i++)
                {
                    if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + SubPosition * i, false)) > -1)
                    {
                        switch (SubPosition)
                        {
                            case 5:
                                AttackDirection = 1;
                                break;
                            case -5:
                                AttackDirection = 2;
                                break;
                            case 1:
                                AttackDirection = 3;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case -4:
                                AttackDirection = 4;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case 4:
                                AttackDirection = 5;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                            case -1:
                                AttackDirection = 6;
                                if (SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                                {
                                    continue;
                                }
                                break;
                        }
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, RoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;
        }
        return TargetIndex;
    }
    int CheckTargetCenter(int NewTargetIndex, List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, int Distance)
    {
        int PositionID = SetPicture[RoleIndex].RolePosition;
        int SetTargetIndex = -1;
        int PositionNum = PositionID % PositionRow;
        switch (Distance)
        {
            case 0:
                break;
            case 1:
                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + PositionRow, false)) > -1) //右
                {
                    AttackDirection = 1;
                    return SetTargetIndex;
                }
                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - PositionRow, false)) > -1) //左
                {
                    AttackDirection = 2;
                    return SetTargetIndex;
                }

                if (ListPosition.Count > SetPicture[RoleIndex].RolePosition + 1)
                {
                    if (ListPosition[SetPicture[RoleIndex].RolePosition + 1].z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 1, false)) > -1 && PositionNum != 0) //右下
                        {
                            AttackDirection = 3;
                            return SetTargetIndex;
                        }
                    }
                }
                if (SetPicture[RoleIndex].RolePosition - PositionRow + 1 > 0)
                {
                    if (ListPosition[SetPicture[RoleIndex].RolePosition - PositionRow + 1].z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - PositionRow + 1, false)) > -1 && PositionNum != 0) //左下
                        {
                            AttackDirection = 4;
                            return SetTargetIndex;
                        }
                    }
                }
                if (ListPosition.Count > SetPicture[RoleIndex].RolePosition + PositionRow + 1)
                {
                    if (ListPosition[SetPicture[RoleIndex].RolePosition + PositionRow - 1].z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + PositionRow - 1, false)) > -1 && PositionNum != 1) //右上
                        {
                            AttackDirection = 5;
                            return SetTargetIndex;
                        }
                    }
                }
                if (ListPosition[SetPicture[RoleIndex].RolePosition - 1].z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                {
                    if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 1, false)) > -1 && PositionNum != 1) //左上
                    {
                        AttackDirection = 6;
                        return SetTargetIndex;
                    }
                }
                break;
            case 2:
                switch (PositionRow)
                {
                    case 5:
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 10, false)) > -1)
                        {
                            AttackDirection = 1;
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 10, false)) > -1)
                        {
                            AttackDirection = 2;
                            return SetTargetIndex;
                        }

                        if (ListPosition.Count > SetPicture[RoleIndex].RolePosition + 2)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition + 2].z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 2, false)) > -1 && PositionNum != 0 && PositionNum != 4)
                                {
                                    AttackDirection = 3;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (SetPicture[RoleIndex].RolePosition - 3 > 0)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition - 3].z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 3, false)) > -1 && PositionNum != 0 && PositionNum != 4)
                                {
                                    AttackDirection = 3;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (SetPicture[RoleIndex].RolePosition - 8 > 0)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition - 8].z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 8, false)) > -1 && PositionNum != 0 && PositionNum != 4)
                                {
                                    AttackDirection = 4;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (ListPosition.Count > SetPicture[RoleIndex].RolePosition + 6)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition + 6].z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 6, false)) > -1 && PositionNum != 0)
                                {
                                    AttackDirection = 3;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (SetPicture[RoleIndex].RolePosition - 9 > 0)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition - 9].z < SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 9, false)) > -1 && PositionNum != 0)
                                {
                                    AttackDirection = 4;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (ListPosition.Count > SetPicture[RoleIndex].RolePosition + 9)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition + 9].z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 9, false)) > -1 && PositionNum != 1)
                                {
                                    AttackDirection = 5;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (SetPicture[RoleIndex].RolePosition - 6 > 0)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition - 6].z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 6, false)) > -1 && PositionNum != 1)
                                {
                                    AttackDirection = 6;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (ListPosition.Count > SetPicture[RoleIndex].RolePosition + 8)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition + 8].z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 8, false)) > -1 && PositionNum != 1 && PositionNum != 2)
                                {
                                    AttackDirection = 5;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (ListPosition.Count > SetPicture[RoleIndex].RolePosition + 3)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition + 3].z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 3, false)) > -1 && PositionNum != 1 && PositionNum != 2)
                                {
                                    AttackDirection = 5;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        if (SetPicture[RoleIndex].RolePosition - 2 > 0)
                        {
                            if (ListPosition[SetPicture[RoleIndex].RolePosition - 2].z > SetPicture[RoleIndex].RoleObject.transform.position.z)
                            {
                                if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 2, false)) > -1 && PositionNum != 1 && PositionNum != 2)
                                {
                                    AttackDirection = 6;
                                    return SetTargetIndex;
                                }
                            }
                        }
                        break;
                }
                break;
            case 3:
                switch (PositionRow)
                {
                    case 5:
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 15, false)) > -1)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 15, false)) > -1)
                        {
                            return SetTargetIndex;
                        }

                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 3, false)) > -1 && PositionNum != 0 && PositionNum != 3 && PositionNum != 4)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 2, false)) > -1 && PositionNum != 0 && PositionNum != 3 && PositionNum != 4)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 7, false)) > -1 && PositionNum != 0 && PositionNum != 3 && PositionNum != 4)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 12, false)) > -1 && PositionNum != 0 && PositionNum != 3 && PositionNum != 4)
                        {
                            return SetTargetIndex;
                        }


                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 7, false)) > -1 && PositionNum != 0 && PositionNum != 4)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 13, false)) > -1 && PositionNum != 0 && PositionNum != 4)
                        {
                            return SetTargetIndex;
                        }

                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 12, false)) > -1 && PositionNum != 0)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 14, false)) > -1 && PositionNum != 0)
                        {
                            return SetTargetIndex;
                        }

                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 14, false)) > -1 && PositionNum != 1)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 12, false)) > -1 && PositionNum != 1)
                        {
                            return SetTargetIndex;
                        }

                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 13, false)) > -1 && PositionNum != 1 && PositionNum != 2)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 7, false)) > -1 && PositionNum != 1 && PositionNum != 2)
                        {
                            return SetTargetIndex;
                        }

                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 12, false)) > -1 && PositionNum != 1 && PositionNum != 2 && PositionNum != 3)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 7, false)) > -1 && PositionNum != 1 && PositionNum != 2 && PositionNum != 3)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition + 2, false)) > -1 && PositionNum != 1 && PositionNum != 2 && PositionNum != 3)
                        {
                            return SetTargetIndex;
                        }
                        if ((SetTargetIndex = CheckTarget(true, NewTargetIndex, SetPicture[RoleIndex].RolePictureMonster, SetPicture[RoleIndex].RolePosition - 3, false)) > -1 && PositionNum != 1 && PositionNum != 2 && PositionNum != 3)
                        {
                            return SetTargetIndex;
                        }
                        break;
                }
                break;
            default:
                break;
        }
        return -1;
    }
    int SelectNewTarget(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, int TargetID)
    {
        int NewTargetID = TargetID;
        if (TargetID % 3 != 0)
        {
            TargetID += 3 - (TargetID % 3);
        }
        for (int k = 0; k < SetEnemyPicture.Count; k++)
        {
            if (TargetID / 3 == SetEnemyPicture[k].RolePosition / 3)
            {
                if ((SetEnemyPicture[k].RolePosition - SetPicture[RoleIndex].RolePosition) % 3 == 0)
                {
                    NewTargetID = SetEnemyPicture[k].RolePosition;
                }
            }
        }


        return NewTargetID;
    }

    public List<int> FindTarget(int NewTargetIndex, List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int Area, int FindRoleIndex, bool IsShow)
    {
        List<int> TargetIndex = new List<int>();
        int SetTargetIndex = -1;
#if ClashRoyale
        float LeastDistance = 3;
        for (int MonsterIndex = 0; MonsterIndex < SetEnemyPicture.Count; MonsterIndex++)
        {
            if (SetEnemyPicture[MonsterIndex].RolePictureAttackable)
            {
                float MonsterDistance = Vector2.Distance(new Vector2(SetEnemyPicture[MonsterIndex].RoleObject.transform.position.x, SetEnemyPicture[MonsterIndex].RoleObject.transform.position.z), new Vector2(SetPicture[FindRoleIndex].RoleObject.transform.position.x, SetPicture[FindRoleIndex].RoleObject.transform.position.z));
                if (MonsterDistance < LeastDistance && SetEnemyPicture[MonsterIndex].RoleObject.activeSelf)
                {
                    LeastDistance = MonsterDistance;
                    SetTargetIndex = MonsterIndex;

                }
            }
        }
        if (SetTargetIndex > -1)
        {
            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
        }
#else
        Debug.Log(FindRoleIndex.ToString()); //这注记不能删
        List<int> RandomList = new List<int>();


        int TargetID = 0;
        float LeastBlood = 99999999;
        float MaxValue = -100;
        float MaxBlood = -1;
        int LeastIndex = -1;
        int SecondIndex = -1;
        int RandomCount = 1;
        int AliveCount = 0;

        if (Area == 0)
        {
            Area = SetPicture[FindRoleIndex].RoleArea;
        }

        if (FightStyle == 6 || FightStyle == 7)
        {
            if (IsFight)
            {
                if (Area != 0)
                {
                    Area = 99;
                }
            }
        }

        if (FightStyle == 2 && !IsFight && Area == 1022)
        {
            Area = 1;
        }

        if (FightStyle != 2 && IsFight && Area != 1020 && Area != 1022 && NewTargetIndex > -1 && !IsLock && !SetPicture[FindRoleIndex].RoleTargetObject.activeSelf) //集火
        {
            Area = 999;
        }


        if (SetPicture[FindRoleIndex].RolePictureFaceRight != 1)
        {
            TargetID = 0;
        }
        switch (Area)
        {
            case 1: //前方1格  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;

            case 2: //前方2格  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;

            case 3: //前方3格  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 3)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;

            case 12: //前方1,2格  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;
            case 21: //前方1,2格范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 1, 2);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;
            case 123: //前方1,2,3格  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 3)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;
            case 321: //前方1,2,3格范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 1, 3);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 2, 3);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 3)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;
            case 3321: //前方1,2,3格T范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 1, 3);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 2, 3);
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 3)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }

                switch (AttackDirection)
                {
                    case 1:
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 3 - (PositionRow - 1), true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 3 - 1, true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        break;
                    case 2:
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * -3 + (PositionRow - 1), true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * -3 + 1, true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        break;
                    case 3:
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 2 + PositionRow, true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z > SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 2 - (PositionRow - 1), true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z > SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        break;
                    case 4:
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) * 2 + PositionRow, true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z > SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) * 2 - 1, true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z > SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        break;
                    case 5:
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) * 2 - PositionRow, true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z < SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) * 2 + 1, true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z < SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        break;
                    case 6:
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 2 - PositionRow, true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z > SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 2 + (PositionRow - 1), true)) > -1)
                        {
                            if (SetPicture[FindRoleIndex].RoleObject.transform.position.z > SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                        break;
                }

                break;

            case 11: //前方1格横条范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                break;
            case 112: //前方1,2格横条
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                break;
            case 1122: //前方1,2格横条范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                break;
            case 13: //前方1格扇形范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                break;
            case 131: //前方1格
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 2 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 2 - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) * 2 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) * 2 + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) * 2 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) * 2 - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 2 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 2 + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                break;
            case 133: //前方1格
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    int Limit3Count = 0;
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    LeastIndex = SetTargetIndex;
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        Limit3Count++;
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        Limit3Count++;
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if (ListPosition[TargetID + 1].z < SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                        {
                            Limit3Count++;
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                    if (ListPosition[TargetID - 1].z > SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                        {
                            if (Limit3Count < 3)
                            {
                                Limit3Count++;
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                    }
                    if (ListPosition[TargetID + (PositionRow - 1)].z > SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                        {
                            if (Limit3Count < 3)
                            {
                                Limit3Count++;
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                    }
                    if (ListPosition[TargetID - (PositionRow - 1)].z < SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                        {
                            if (Limit3Count < 3)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                        }
                    }
                }
                break;
            case 136: //前方1格圆范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    LeastIndex = SetTargetIndex;
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if (ListPosition[TargetID + 1].z < SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                    if (ListPosition[TargetID - 1].z > SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                    if (ListPosition[TargetID + (PositionRow - 1)].z > SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                    if (ListPosition[TargetID - (PositionRow - 1)].z < SetEnemyPicture[LeastIndex].RoleObject.transform.position.z)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                }

                //if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                //{
                //    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 1, 2);

                //    switch (AttackDirection)
                //    {
                //        case 1:
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + (PositionRow - 1), true)) > -1)
                //            {
                //                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //            }
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + 1, true)) > -1)
                //            {
                //                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //            }
                //            break;
                //        case 2:
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - (PositionRow - 1), true)) > -1)
                //            {
                //                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //            }
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - 1, true)) > -1)
                //            {
                //                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //            }
                //            break;
                //        case 3:
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 + PositionRow, true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 - (PositionRow - 1), true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            break;
                //        case 4:
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) - PositionRow, true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) + 1, true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            break;
                //        case 5:
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) + PositionRow, true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) - 1, true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            break;
                //        case 6:
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 - PositionRow, true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 + (PositionRow - 1), true)) > -1)
                //            {
                //                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                //                {
                //                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //                }
                //            }
                //            break;
                //    }
                //}
                break;
            case 1234: //前方1,2格圆范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 1, 2);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1 - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1 + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = CheckDirectCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, SetEnemyPicture[SetTargetIndex].RolePosition, 2, 3);

                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 + (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2 + 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 - (PositionRow - 1), true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2 - 1, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 2 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 2 - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) * 2 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) * 2 + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) * 2 + PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) * 2 - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 2 - PositionRow, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 2 + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                break;
            case 12345: //前方1,2格圆范围
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 3)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;
            case 36: //前方3格范围  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 3)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;
            case 26: //前方1,2格范围  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;
            case 55: //我自已
                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, FindRoleIndex);
                break;
            case 121: //前方1,2格  
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow * 2, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow * 2, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 2, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - (PositionRow - 1) * 2, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + (PositionRow - 1) * 2, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 2, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                else if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 2)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    switch (AttackDirection)
                    {
                        case 1:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 2:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                            }
                            break;
                        case 3:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 4:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 5:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z <= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                        case 6:
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                            {
                                if (SetPicture[FindRoleIndex].RoleObject.transform.position.z >= SetEnemyPicture[SetTargetIndex].RoleObject.transform.position.z)
                                {
                                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                                }
                            }
                            break;
                    }
                }
                break;
            case 1011: //我方全体血最少的人类+自已
                //if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, true, SetPicture[FindRoleIndex].RolePosition + PositionRow, false)) > -1)
                //{
                //    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //}
                //else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, true, SetPicture[FindRoleIndex].RolePosition - PositionRow, false)) > -1)
                //{
                //    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //}
                //else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, true, SetPicture[FindRoleIndex].RolePosition + 1, false)) > -1)
                //{
                //    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //}
                //else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, true, SetPicture[FindRoleIndex].RolePosition - 1, false)) > -1)
                //{
                //    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //}
                //else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, true, SetPicture[FindRoleIndex].RolePosition + PositionRow - 1, false)) > -1)
                //{
                //    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //}
                //else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, true, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, false)) > -1)
                //{
                //    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                //}

                for (int MonsterIndex = 0; MonsterIndex < SetPicture.Count; MonsterIndex++)
                {
                    if (SetPicture[MonsterIndex].RolePictureAttackable && SetPicture[MonsterIndex].RoleNowBlood > 0 && SetPicture[MonsterIndex].RolePosition > 0)
                    {
                        if (LeastBlood > SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood)
                        {
                            LeastBlood = SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood;
                            LeastIndex = MonsterIndex;
                            HealIndex = MonsterIndex;
                        }
                    }
                }

                if (FightStyle == 2 && NewTargetIndex != -1)
                {
                    HealIndex = NewTargetIndex;
                }

                if (HealIndex != -1)
                {
                    if (SetPicture[FindRoleIndex].RolePosition == SetPicture[HealIndex].RolePosition)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition + PositionRow == SetPicture[HealIndex].RolePosition)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition - PositionRow == SetPicture[HealIndex].RolePosition)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition + 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z < SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition - 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z > SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition + PositionRow - 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z > SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition - PositionRow + 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z < SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RoleMoveNowStep == 0)
                    {
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                }

                if (TargetIndex.Count > 0)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, FindRoleIndex);

                    if (SetPicture[FindRoleIndex].RoleSkill2 == 2050) //背动加怒
                    {
                        SetPicture[FindRoleIndex].RoleSkillPoint += 50;
                        SetPicture[TargetIndex[0]].RoleSkillPoint += 50;
                    }
                    else if (SetPicture[FindRoleIndex].RoleSkill2 == 2005) //背动加怒
                    {
                        SetPicture[FindRoleIndex].RoleSkillPoint += 30;
                        SetPicture[TargetIndex[0]].RoleSkillPoint += 30;
                    }

                }
                break;
            case 1020: //我方全体
                for (int k = 0; k < SetPicture.Count; k++)
                {
                    if (SetPicture[k].RoleNowBlood > 0 && SetPicture[k].RolePosition > 0)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[k].RolePosition, true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                            if (SetPicture[FindRoleIndex].RoleSkill2 == 2038) //背动加怒
                            {
                                SetPicture[SetTargetIndex].RoleSkillPoint += 70;
                            }
                        }
                    }
                }


                break;
            case 1021: //我周围
                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, FindRoleIndex);
                if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, false)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, false)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                if (ListPosition[SetPicture[FindRoleIndex].RolePosition + 1].z < SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                {
                    if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                if (ListPosition[SetPicture[FindRoleIndex].RolePosition - 1].z > SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                {
                    if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                if (ListPosition[SetPicture[FindRoleIndex].RolePosition + PositionRow - 1].z > SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                {
                    if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                if (ListPosition[SetPicture[FindRoleIndex].RolePosition - PositionRow + 1].z < SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                {
                    if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;

            case 1022: //我方全体血最少的人类
                for (int MonsterIndex = 0; MonsterIndex < SetPicture.Count; MonsterIndex++)
                {
                    if (SetPicture[MonsterIndex].RolePictureAttackable && SetPicture[MonsterIndex].RoleNowBlood > 0 && SetPicture[MonsterIndex].RolePosition > 0)
                    {
                        if (LeastBlood > SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood)
                        {
                            LeastBlood = SetPicture[MonsterIndex].RoleNowBlood / SetPicture[MonsterIndex].RoleMaxBlood;
                            LeastIndex = MonsterIndex;
                            HealIndex = MonsterIndex;
                        }
                    }
                }

                if ((FightStyle == 2 || IsLock || !IsFight || SetPicture[FindRoleIndex].RoleTargetObject.activeSelf) && NewTargetIndex != -1)
                {
                    HealIndex = NewTargetIndex;
                }
                //for (int k = 0; k < SetPicture.Count; k++)
                //{
                //    if ((SetPicture[k].RoleMaxBlood - SetPicture[k].RoleNowBlood) > MaxBlood && SetPicture[k].RoleBio == 1 && SetPicture[k].RoleNowBlood > 0)
                //    {
                //        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[k].RolePosition, true)) > -1)
                //        {
                //            LeastIndex = k;
                //            MaxBlood = (SetPicture[k].RoleMaxBlood - SetPicture[k].RoleNowBlood);
                //        }
                //    }
                //}
                //if (LeastIndex != -1)
                //{
                //    TargetIndex.Add(LeastIndex);
                //}
                if (HealIndex != -1)
                {
                    if (SetPicture[FindRoleIndex].RolePosition == SetPicture[HealIndex].RolePosition)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition + PositionRow == SetPicture[HealIndex].RolePosition)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition - PositionRow == SetPicture[HealIndex].RolePosition)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition + 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z < SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition - 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z > SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition + PositionRow - 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z > SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RolePosition - PositionRow + 1 == SetPicture[HealIndex].RolePosition && SetPicture[HealIndex].RoleObject.transform.position.z < SetPicture[FindRoleIndex].RoleObject.transform.position.z)
                    {
                        TargetIndex.Add(HealIndex);
                    }
                    else if (SetPicture[FindRoleIndex].RoleMoveNowStep == 0 || SetPicture[FindRoleIndex].BuffNoMove)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition + PositionRow - 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                        else if ((SetTargetIndex = CheckTarget(false, -1, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[FindRoleIndex].RolePosition - PositionRow + 1, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                }
                break;
            case 1023: //我方全体血最少的机械
                for (int k = 0; k < SetPicture.Count; k++)
                {
                    if ((SetPicture[k].RoleMaxBlood - SetPicture[k].RoleNowBlood) > MaxBlood && SetPicture[k].RoleBio == 0 && SetPicture[k].RoleNowBlood > 0)
                    {
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, !SetPicture[FindRoleIndex].RolePictureMonster, SetPicture[k].RolePosition, true)) > -1)
                        {
                            LeastIndex = k;
                            MaxBlood = (SetPicture[k].RoleMaxBlood - SetPicture[k].RoleNowBlood);
                        }
                    }
                }
                if (LeastIndex != -1)
                {
                    TargetIndex.Add(LeastIndex);
                }
                break;
            case 93: //敌方随机三目标
                RandomCount = 3;

                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        AliveCount++;
                    }
                }
                if (AliveCount <= RandomCount)
                {
                    for (int k = 0; k < SetEnemyPicture.Count; k++)
                    {
                        if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, k);
                            Debug.Log("SetTargetIndex" + SetTargetIndex);
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < SetEnemyPicture.Count; k++)
                    {
                        float RandomNum = GetRandom(SetPicture[FindRoleIndex].RoleID);
                        if ((RandomNum < 60 && AliveCount > RandomCount))
                        {
                            AliveCount--;
                        }
                        else if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, k);
                            Debug.Log("SetTargetIndex" + SetTargetIndex);
                        }
                    }
                }
                break;
            case 66: //敌方血最少
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        if (SetEnemyPicture[k].RoleNowBlood < LeastBlood)
                        {
                            LeastBlood = SetEnemyPicture[k].RoleNowBlood;
                            LeastIndex = k;
                        }
                    }
                }

                if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[LeastIndex].RolePosition, true)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                else
                {
                    for (int k = 0; k < SetEnemyPicture.Count; k++)
                    {
                        if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                        {
                            if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[k].RolePosition, false)) > -1)
                            {
                                TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, k);
                            }
                        }
                    }
                }
                break;

            case 99: //敌方全体
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[k].RolePosition, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, k);
                        }
                    }
                }
                break;
            case 999: //敌方全体集火
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[k].RolePosition, false)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, k);
                        }
                    }
                }
                break;
            case 901: //敌方最后一个
                if (SetEnemyPicture[0].RoleID < 65000)
                {
                    MaxValue = 100;
                }
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleID < 65000)
                    {
                        if (SetEnemyPicture[k].RoleNowBlood > 0)
                        {
                            if (SetEnemyPicture[k].RoleObject.transform.position.x < MaxValue)
                            {
                                MaxValue = SetEnemyPicture[k].RoleObject.transform.position.x;
                                LeastIndex = k;
                            }
                        }
                    }
                    else
                    {
                        if (SetEnemyPicture[k].RoleNowBlood > 0)
                        {
                            if (SetEnemyPicture[k].RoleObject.transform.position.x > MaxValue)
                            {
                                MaxValue = SetEnemyPicture[k].RoleObject.transform.position.x;
                                LeastIndex = k;
                            }
                        }
                    }
                }

                if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[LeastIndex].RolePosition, true)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    Debug.Log("SetTargetIndex" + SetTargetIndex);
                }
                break;


            case 990: //敌方最近
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        if (LeastBlood > Vector3.Distance(SetEnemyPicture[k].RoleObject.transform.position, SetPicture[FindRoleIndex].RoleObject.transform.position))
                        {
                            LeastBlood = Vector3.Distance(SetEnemyPicture[k].RoleObject.transform.position, SetPicture[FindRoleIndex].RoleObject.transform.position);
                            LeastIndex = k;
                        }
                    }
                }

                if (FightStyle == 2 && NewTargetIndex != -1)
                {
                    LeastIndex = NewTargetIndex;
                }

                if (LeastIndex != -1)
                {
                    if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[LeastIndex].RolePosition, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;


            case 991: //敌方随机1
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        RandomList.Add(k);
                    }
                }

                LeastIndex = RandomList[GetRandomInt(0, RandomList.Count, SetPicture[FindRoleIndex].RoleID)]; // RandomList[UnityEngine.Random.Range(0, RandomList.Count)];
                if (FightStyle == 2 && NewTargetIndex != -1)
                {
                    LeastIndex = NewTargetIndex;
                }

                if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[LeastIndex].RolePosition, true)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                }
                break;

            case 9916: //敌方随机1+周围  优先机械
                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        RandomList.Add(k);
                    }
                }

                LeastIndex = RandomList[GetRandomInt(0, RandomList.Count, SetPicture[FindRoleIndex].RoleID)];  //RandomList[UnityEngine.Random.Range(0, RandomList.Count)];

                for (int k = 0; k < SetEnemyPicture.Count; k++)
                {
                    if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                    {
                        if (SetEnemyPicture[k].RoleBio == 0)
                        {
                            LeastIndex = k;
                        }
                    }
                }


                if (FightStyle == 2 && NewTargetIndex != -1)
                {
                    LeastIndex = NewTargetIndex;
                }



                if ((SetTargetIndex = CheckTarget(false, NewTargetIndex, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[LeastIndex].RolePosition, true)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - (PositionRow - 1), true)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;

            case 1341: //周围1人
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    LeastIndex = SetTargetIndex;
                    TargetID = SetEnemyPicture[SetTargetIndex].RolePosition;

                    if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    else if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    else if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    else if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    else if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID + PositionRow - 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                    else if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, TargetID - PositionRow + 1, false)) > -1)
                    {
                        TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                    }
                }
                break;

            case 1992: //前方1格+随机2人
                if ((SetTargetIndex = CheckTargetCenter(NewTargetIndex, SetPicture, SetEnemyPicture, FindRoleIndex, 1)) > -1)
                {
                    TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);

                    for (int k = 0; k < SetEnemyPicture.Count; k++)
                    {
                        if (SetEnemyPicture[k].RoleNowBlood > 0 && SetEnemyPicture[k].RolePosition > 0)
                        {
                            RandomList.Add(k);
                        }
                    }

                    if (RandomList.Count > 1)
                    {
                        SecondIndex = SetTargetIndex;
                        while (LeastIndex == -1 || SetTargetIndex == LeastIndex)
                        {
                            LeastIndex = RandomList[GetRandomInt(0, RandomList.Count, SetPicture[FindRoleIndex].RoleID)]; //RandomList[UnityEngine.Random.Range(0, RandomList.Count)];
                        }
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[LeastIndex].RolePosition, true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                    if (RandomList.Count > 2)
                    {
                        while (SecondIndex == LeastIndex || SetTargetIndex == LeastIndex)
                        {
                            LeastIndex = RandomList[GetRandomInt(0, RandomList.Count, SetPicture[FindRoleIndex].RoleID)];  //RandomList[UnityEngine.Random.Range(0, RandomList.Count)];
                        }
                        if ((SetTargetIndex = CheckTarget(false, -1, SetPicture[FindRoleIndex].RolePictureMonster, SetEnemyPicture[LeastIndex].RolePosition, true)) > -1)
                        {
                            TargetIndex = AddTargetIndex(SetPicture, SetEnemyPicture, FindRoleIndex, TargetIndex, SetTargetIndex);
                        }
                    }
                }
                break;
        }
        if (TargetIndex.Count > 0 && NewTargetIndex > -1 && !IsShow)
        {
            SetPicture[FindRoleIndex].RoleMovePosition = 0; //why
        }
#endif
        return TargetIndex;
    }
    List<int> AddTargetIndex(List<RolePicture> SetPicture, List<RolePicture> SetEnemyPicture, int RoleIndex, List<int> TargetIndex, int Index)
    {
        for (int i = 0; i < TargetIndex.Count; i++)
        {
            if (TargetIndex[i] == Index)
            {
                return TargetIndex;
            }
        }
        TargetIndex.Add(Index);
        return TargetIndex;
    }
    #endregion

    #region Sound
    public void PlayRoleSound(int RoleID)
    {
        string Sound = "";
        string Add = "";
        if (UnityEngine.Random.Range(0, 99) % 2 == 0)
        {
            Add = "2";
        }

        switch (RoleID)
        {
            case 60001:
                Sound = "V_carbine" + Add;
                break;
            case 60002:
                Sound = "V_sniper" + Add;
                break;
            case 60003:
                Sound = "V_shield" + Add;
                break;
            case 60004:
                Sound = "V_captain" + Add;
                break;
            case 60005:
                Sound = "V_medici" + Add;
                break;
            case 60006:
                Sound = "V_fireman" + Add;
                break;
            case 60007:
                Sound = "V_machinegun" + Add;
                break;
            case 60008:
                Sound = "V_bio" + Add;
                break;
            case 60009:
                Sound = "V_engineer" + Add;
                break;
            case 60010:
                Sound = "V_SmallCarnoon" + Add;
                break;
            case 60011:
                Sound = "V_伯纳德" + Add;
                break;
            case 60012: //凯特丽娜
                Sound = "V_katarina" + Add;
                break;
            case 60013: //古烈
                Sound = "V_gulie" + Add;
                break;
            case 60014:
                Sound = "V_snake" + Add;
                break;
            case 60015: //阿卡琳
                Sound = "V_akali" + Add;
                break;
            case 60016:
                Sound = "V_graves" + Add;
                break;
            case 60017:
                Sound = "V_raiden" + Add;
                break;
            case 60018: //欧巴
                Sound = "V_Obama" + Add;
                break;
            case 60019: //威斯克
                Sound = "V_vsg" + Add;
                break;
            case 60020:
                Sound = "V_jinx" + Add;
                break;
            case 60021:
                Sound = "V_ada" + Add;
                break;
            case 60022: //里昂
                Sound = "V_leo" + Add;
                break;
            case 60023: //巴尼
                Sound = "V_bani" + Add;
                break;
            case 60024: //劳拉
                Sound = "V_Lara" + Add;
                break;
            case 60025: //春丽
                Sound = "V_Chunli" + Add;
                break;
            case 60026: //圣诞
                Sound = "V_Chris" + Add;
                break;
            case 60027: //阴阳
                Sound = "V_Lilianjie" + Add;
                break;
            case 60028: //茱莉亚
                Sound = "V_Julia" + Add;
                break;
            case 60029: //战壕
                Sound = "V_trench" + Add;
                break;
            //case 60030: //维嘉
            //    Sound = "V_trench" + Add;
            //    break;
            case 60031: //暴走圣诞
                Sound = "V_Chris" + Add;
                break;
            case 60032: //卖琪
                Sound = "V_maggie" + Add;
                break;
            case 60100:
                Sound = "V_moto" + Add;
                break;
            case 60101:
                Sound = "V_hamvee" + Add;
                break;
            case 60102:
                Sound = "V_tank" + Add;
                break;
            case 60103:
                Sound = "V_carnoon" + Add;
                break;
            case 60104:
                Sound = "V_artillery" + Add;
                break;
            case 60200:
                Sound = "V_airfight" + Add;
                break;
            case 60201:
                Sound = "V_helicopter" + Add;
                break;
            default:
                break;
        }
        AudioEditer.instance.PlayOneShot(Sound);
    }
    #endregion

    #region Weather
    public void SetWeather(int WeatherNum)
    {
        /*
        0正常晴天
        1黄沙
        2灰尘
        3闪电
        4下雪
        5下雨
        */
        if (Weather != null)
        {
            Destroy(Weather);
        }
        if (PlayerPrefs.GetFloat("ElectractySlider") != 0) //耗电模式
        {
            if (WeatherNum == 1)
            {
                Weather = GameObject.Instantiate(Resources.Load("Prefab/Weather/TianQi_HuangSha", typeof(GameObject))) as GameObject;

                if (GameObject.Find("sun") != null)
                {
                    GameObject.Find("sun").SetActive(false);
                }

                if (GameObject.Find("shandian") != null)
                {
                    GameObject.Find("shandian").SetActive(false);
                }

                if (GameObject.Find("xiayu") != null)
                {
                    GameObject.Find("xiayu").SetActive(false);
                }

                if (GameObject.Find("xiaxue") != null)
                {
                    GameObject.Find("xiaxue").SetActive(false);
                }
                StartCoroutine(AudioEditer.instance.PlayCountinueSound("bgs_wind", 3));
            }
            else if (WeatherNum == 2)
            {
                Weather = GameObject.Instantiate(Resources.Load("Prefab/Weather/TianQi_HuiCheng", typeof(GameObject))) as GameObject;

                if (GameObject.Find("sun") != null)
                {
                    GameObject.Find("sun").SetActive(false);
                }
                if (GameObject.Find("huangsha") != null)
                {
                    GameObject.Find("huangsha").SetActive(false);
                }
                if (GameObject.Find("shandian") != null)
                {
                    GameObject.Find("shandian").SetActive(false);
                }
                if (GameObject.Find("xiayu") != null)
                {
                    GameObject.Find("xiayu").SetActive(false);
                }
                if (GameObject.Find("xiaxue") != null)
                {
                    GameObject.Find("xiaxue").SetActive(false);
                }
                //StartCoroutine(AudioEditer.instance.PlayCountinueSound("bgs_citywar", 74));
            }
            else if (WeatherNum == 3)
            {
                Weather = GameObject.Instantiate(Resources.Load("Prefab/Weather/TianQi_ShanDian", typeof(GameObject))) as GameObject;

                if (GameObject.Find("sun") != null)
                {
                    GameObject.Find("sun").SetActive(false);
                }
                if (GameObject.Find("huangsha") != null)
                {
                    GameObject.Find("huangsha").SetActive(false);
                }
                if (GameObject.Find("xiayu") != null)
                {
                    GameObject.Find("xiayu").SetActive(false);
                }
                if (GameObject.Find("xiaxue") != null)
                {
                    GameObject.Find("xiaxue").SetActive(false);
                }

                AudioEditer.instance.PlayOneShot("bgs_flashlight");
            }
            else if (WeatherNum == 4)
            {
                Weather = GameObject.Instantiate(Resources.Load("Prefab/Weather/TianQi_XiaXue", typeof(GameObject))) as GameObject;

                if (GameObject.Find("sun") != null)
                {
                    GameObject.Find("sun").SetActive(false);
                }
                if (GameObject.Find("huangsha") != null)
                {
                    GameObject.Find("huangsha").SetActive(false);
                }
                if (GameObject.Find("shandian") != null)
                {
                    GameObject.Find("shandian").SetActive(false);
                }
                if (GameObject.Find("xiayu") != null)
                {
                    GameObject.Find("xiayu").SetActive(false);
                }
            }
            else if (WeatherNum == 5)
            {
                Weather = GameObject.Instantiate(Resources.Load("Prefab/Weather/TianQi_XiaYu", typeof(GameObject))) as GameObject;

                if (GameObject.Find("sun") != null)
                {
                    GameObject.Find("sun").SetActive(false);
                }
                if (GameObject.Find("huangsha") != null)
                {
                    GameObject.Find("huangsha").SetActive(false);
                }
                if (GameObject.Find("shandian") != null)
                {
                    GameObject.Find("shandian").SetActive(false);
                }
                if (GameObject.Find("xiaxue") != null)
                {
                    GameObject.Find("xiaxue").SetActive(false);
                }
                StartCoroutine(AudioEditer.instance.PlayCountinueSound("bgs_rain", 6));
            }
            else if (WeatherNum == 6)
            {
                Weather = GameObject.Instantiate(Resources.Load("Prefab/Weather/TianQi_DaXue", typeof(GameObject))) as GameObject;

                if (GameObject.Find("sun") != null)
                {
                    GameObject.Find("sun").SetActive(false);
                }
                if (GameObject.Find("huangsha") != null)
                {
                    GameObject.Find("huangsha").SetActive(false);
                }
                if (GameObject.Find("shandian") != null)
                {
                    GameObject.Find("shandian").SetActive(false);
                }
                if (GameObject.Find("xiayu") != null)
                {
                    GameObject.Find("xiayu").SetActive(false);
                }
                StartCoroutine(AudioEditer.instance.PlayCountinueSound("bgs_storm", 24));
            }
            else
            {
                if (GameObject.Find("huangsha") != null)
                {
                    GameObject.Find("huangsha").SetActive(false);
                }
                if (GameObject.Find("shandian") != null)
                {
                    GameObject.Find("shandian").SetActive(false);
                }
                if (GameObject.Find("xiayu") != null)
                {
                    GameObject.Find("xiayu").SetActive(false);
                }
                if (GameObject.Find("xiaxue") != null)
                {
                    GameObject.Find("xiaxue").SetActive(false);
                }
            }
        }
    }
    #endregion

    #region Result
    public IEnumerator SetPictureEffect(bool IsEnemy, int StartIndex, int TargetIndex, float Damige, List<Buff> TargetBuff1, Buff TargetBuff2, string HurtType, FightProjectile fp, bool IsSkill)
    {
        List<RolePicture> SetPicture;
        List<RolePicture> SetEnemyPicture;
        bool IsContinueHurt = false;
        //Debug.LogError(Damige + " " + IsEnemy + " " + StartIndex + " " + TargetIndex + " " + Rannn);
        if (IsEnemy)
        {
            if (Damige > 0)
            {
                SetPicture = ListRolePicture;
                SetEnemyPicture = ListEnemyPicture;
            }
            else
            {
                SetPicture = ListEnemyPicture;
                SetEnemyPicture = ListRolePicture;
            }
        }
        else
        {
            if (Damige > 0)
            {
                SetPicture = ListEnemyPicture;
                SetEnemyPicture = ListRolePicture;
            }
            else
            {
                SetPicture = ListRolePicture;
                SetEnemyPicture = ListEnemyPicture;
            }
        }

        //if (StartIndex > -1)
        //{
        //    PictureCreater.instance.AttackCount--; //why要移上来啊
        //}

        if (fp != null)
        {
            AudioEditer.instance.PlayOneShot(fp.sound);
        }

        bool IsAvoid = false;
        if (TargetIndex < SetPicture.Count)
        {

            SetPicture[TargetIndex].RoleRedBloodObject.SetActive(true);
            if (TargetBuff1 != null)
            {
                foreach (var b in TargetBuff1)
                {
                    RoleAddBuff(SetPicture, TargetIndex, b);
                }
            }
            if (TargetBuff2 != null)
            {
                RoleAddBuff(SetPicture, TargetIndex, TargetBuff2);
            }

            if (HurtType != "")
            {
                if (FightStyle != 6 && FightStyle != 7)
                {
                    if (fp.effectRatio == 1) //特效一直在身上
                    {
                        EffectMaker.instance.Create2DEffect("~" + HurtType, "", SetPicture[TargetIndex].RolePictureObject, SetPicture[TargetIndex].RoleObject.transform.position + new Vector3(fp.touchEffectPosX / 100f, 0, fp.touchEffectPosY / 100f), SetPicture[TargetIndex].RoleObject.transform.position + new Vector3(fp.touchEffectPosX / 100f, 0, fp.touchEffectPosY / 100f), SetPicture[TargetIndex].RolePictureObject.transform.localEulerAngles, 0.1f, 0, 5f, 1, 1, TargetIndex, null, null, null, null, null, false, false, false, true, false, false, false, !IsEnemy, SetPicture[TargetIndex].RolePictureMonster, "", null);
                    }
                    else
                    {
                        EffectMaker.instance.Create2DEffect("~" + HurtType, "", null, SetPicture[TargetIndex].RoleObject.transform.position + new Vector3(fp.touchEffectPosX / 100f, 0, fp.touchEffectPosY / 100f), SetPicture[TargetIndex].RoleObject.transform.position + new Vector3(fp.touchEffectPosX / 100f, 0, fp.touchEffectPosY / 100f), SetPicture[TargetIndex].RolePictureObject.transform.localEulerAngles, 0.1f, 0, 5f, 1, 1, TargetIndex, null, null, null, null, null, false, false, false, true, false, false, false, !IsEnemy, SetPicture[TargetIndex].RolePictureMonster, "", null);
                    }
                }
            }

            if (Damige > 0)
            {
                //////////////////扺消攻击//////////////////
                if (SetPicture[TargetIndex].BuffAvoidCount > 0)
                {
                    SetPicture[TargetIndex].BuffAvoidCount--;
                    if (SetPicture[TargetIndex].BuffAvoidCount <= 0)
                    {
                        for (int b = 0; b < SetPicture[TargetIndex].ListBuff.Count; b++)
                        {
                            if (SetPicture[TargetIndex].ListBuff[b].buffType == 30)
                            {
                                SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().RemoveSetBuff(SetPicture[TargetIndex].ListBuff[b].buffIcon);
                                DestroyImmediate(SetPicture[TargetIndex].ListBuff[b].effectObject);
                                SetPicture[TargetIndex].ListBuff.RemoveAt(b);
                            }
                        }
                    }


                    IsAvoid = true;
                }
                else
                {
                    if (StartIndex > -1)
                    {
                        float RandomNum = GetRandom(SetEnemyPicture[StartIndex].RoleID);
                        /////////////////先算闪避/////////////////
                        float Dodge = (SetEnemyPicture[StartIndex].RoleHit + SetEnemyPicture[StartIndex].BuffHit - SetPicture[TargetIndex].RoleNoHit - SetPicture[TargetIndex].BuffNoHit) / 10;
                        //Debug.Log("Dodge" + Dodge + "RandomNum" + RandomNum + "Hit" + SetEnemyPicture[StartIndex].RoleHit + "RoleNoHit" + SetPicture[TargetIndex].RoleNoHit + " " + Rannn);

                        /////////////////先算闪避/////////////////
                        if (RandomNum < Dodge || CharacterRecorder.instance.level < 6)
                        {

                            Damige = Damige * (1 + (SetEnemyPicture[StartIndex].RoleDamigeAdd + SetEnemyPicture[StartIndex].BuffDamigeAdd - SetPicture[TargetIndex].RoleDamigeReduce - SetPicture[TargetIndex].BuffDamigeReduce) / 1000f);

                            /////////////////////////////////////二级克制(以下)/////////////////////////////////////
                            switch (SetEnemyPicture[StartIndex].RoleAttackType)
                            {
                                case 1:
                                    if (SetPicture[TargetIndex].RoleAttackType == 1)
                                    {

                                    }
                                    else if (SetPicture[TargetIndex].RoleAttackType == 2)
                                    {
                                        Damige *= 0.8f;
                                    }
                                    else if (SetPicture[TargetIndex].RoleAttackType == 3)
                                    {
                                        Damige *= 1.2f;
                                    }
                                    break;
                                case 2:
                                    if (SetPicture[TargetIndex].RoleAttackType == 1)
                                    {
                                        Damige *= 1.2f;
                                    }
                                    else if (SetPicture[TargetIndex].RoleAttackType == 2)
                                    {

                                    }
                                    else if (SetPicture[TargetIndex].RoleAttackType == 3)
                                    {
                                        Damige *= 0.8f;
                                    }
                                    break;
                                case 3:
                                    if (SetPicture[TargetIndex].RoleAttackType == 1)
                                    {
                                        Damige *= 0.8f;
                                    }
                                    else if (SetPicture[TargetIndex].RoleAttackType == 2)
                                    {
                                        Damige *= 1.2f;
                                    }
                                    else if (SetPicture[TargetIndex].RoleAttackType == 3)
                                    {

                                    }
                                    break;
                            }
                            /////////////////////////////////////二级克制(以上)/////////////////////////////////////


                            ///////////////////////////国战伤害加成///////////////////////////
                            if (FightStyle == 14)
                            {
                                if (!SetEnemyPicture[StartIndex].RolePictureMonster && CharacterRecorder.instance.NationID > 0)
                                {
                                    Damige *= (1 + TextTranslator.instance.GetNationByID(CharacterRecorder.instance.NationID).BattlefieldDamageBonus);
                                }
                            }
                            ///////////////////////////国战伤害加成///////////////////////////

                            //////////////////机械克星////////////////////
                            if (SetEnemyPicture[StartIndex].RoleSkill2 == 2009)
                            {
                                if (SetPicture[TargetIndex].RoleBio == 0)
                                {
                                    Damige *= 1.3f;
                                }
                            }
                            else if (SetEnemyPicture[StartIndex].RoleSkill2 == 2048)
                            {
                                if (SetPicture[TargetIndex].RoleBio == 0)
                                {
                                    Damige *= 1.5f;
                                }
                            }
                            //////////////////机械克星////////////////////

                            //////////////////机械化部队////////////////////
                            if (SetPicture[TargetIndex].RoleSkill2 == 2041)
                            {
                                if (SetEnemyPicture[StartIndex].RoleBio == 0)
                                {
                                    Damige *= 0.85f;
                                }
                            }
                            //////////////////机械化部队////////////////////

                            //////////////////非机械化部队////////////////////
                            if (SetEnemyPicture[StartIndex].RoleSkill2 == 2012)
                            {
                                if (SetPicture[TargetIndex].RoleBio == 1)
                                {
                                    Damige *= 1.1f;
                                }
                            }
                            //////////////////非机械化部队////////////////////

                            //////////////////被动晕////////////////////
                            if (SetEnemyPicture[StartIndex].RoleSkill2 == 2019)
                            {
                                if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < 10)
                                {

                                    Skill ActiveSkill = TextTranslator.instance.GetSkillByID(2019, 1);
                                    Buff NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.BuffID);
                                    if (NewBuff != null)
                                    {
                                        RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                    }
                                }
                            }
                            //////////////////被动晕////////////////////

                            //////////////////满血有加成////////////////////
                            if (SetEnemyPicture[StartIndex].RoleSkill1 == 1024 && IsSkill)
                            {
                                if (SetPicture[TargetIndex].RoleNowBlood / SetPicture[TargetIndex].RoleMaxBlood > 0.5f)
                                {
                                    Damige *= 1.25f;
                                }
                            }
                            //////////////////满血有加成////////////////////

                            //////////////////吸星大法////////////////////
                            if (SetEnemyPicture[StartIndex].RoleSkill1 == 1025 && IsSkill)
                            {
                                SetEnemyPicture[StartIndex].RolePAttack += SetPicture[TargetIndex].RolePAttack / 3.3f;
                                SetEnemyPicture[StartIndex].RolePDefend += SetPicture[TargetIndex].RolePDefend / 3.3f;
                            }
                            //////////////////吸星大法////////////////////

                            //////////////////必杀抵抗////////////////////
                            if (SetPicture[TargetIndex].RoleSkill2 == 2046 && IsSkill)
                            {
                                Damige *= 0.8f;
                            }
                            //////////////////必杀抵抗////////////////////

                            //////////////////残血有加成////////////////////
                            if (SetEnemyPicture[StartIndex].RoleSkill1 == 1027 && IsSkill)
                            {
                                if (SetPicture[TargetIndex].RoleNowBlood / SetPicture[TargetIndex].RoleMaxBlood < 0.5f)
                                {
                                    Damige *= 1.25f;
                                }
                            }
                            //////////////////残血有加成////////////////////

                            //////////////////重生战士////////////////////
                            if (SetEnemyPicture[StartIndex].RoleSkill2 == 2040)
                            {
                                Damige *= 1 + ((1 - SetEnemyPicture[StartIndex].RoleNowBlood / SetEnemyPicture[StartIndex].RoleMaxBlood) * 0.3f);
                            }
                            //////////////////重生战士////////////////////

                            //////////////////暴击//////////////////
                            float Crit = (SetEnemyPicture[StartIndex].RoleCrit + SetEnemyPicture[StartIndex].BuffCrit - SetPicture[TargetIndex].RoleNoCrit - SetPicture[TargetIndex].BuffNoCrit) / 10;
                            RandomNum = GetRandom(SetEnemyPicture[StartIndex].RoleID);
                            //Debug.Log("Crit" + Crit.ToString() + " RandomNum" + RandomNum.ToString());
                            if (RandomNum < Crit)
                            {
                                Damige *= 2;
                                if (!IsSkip)
                                {
                                    StartCoroutine(SharkCamera());
                                    SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().mText.Add(-Damige, Color.red, 0f);
                                }
                            }
                            else
                            {
                                if (!IsSkip)
                                {
                                    SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().mText.Add(-Damige, Color.yellow, 0f);
                                }
                            }
                            //////////////////暴击//////////////////


                            //////////////////盾兵反击//////////////////
                            if (SetPicture[TargetIndex].RoleSkill2 == 2003 || SetPicture[TargetIndex].RoleSkill2 == 2049)
                            {
                                List<int> RoleFightIndex = new List<int>();
                                List<int> RoleFightCate = new List<int>();
                                List<int> RoleFightDamige = new List<int>();
                                RoleFightIndex.Add(StartIndex);
                                RoleFightCate.Add(0);
                                int BackDamige = 1;
                                if (SetPicture[TargetIndex].RoleSkill2 == 2003)
                                {
                                    BackDamige = (int)(Damige / 10f);
                                }
                                else
                                {
                                    BackDamige = (int)(Damige * 15f / 100f);
                                }
                                if (BackDamige < 1)
                                {
                                    BackDamige = 1;
                                }
                                RoleFightDamige.Add(BackDamige);

                                EffectMaker.instance.Create2DEffect("Test", "blank", null, SetEnemyPicture[StartIndex].RoleObject.transform.position, SetEnemyPicture[StartIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0.5f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[TargetIndex].RolePictureMonster, "", null);
                                //SetEnemyPicture[StartIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuff(9014);   
                            }
                            //////////////////盾兵反击//////////////////

                            //反伤   11反弹伤害
                            Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(11, SetEnemyPicture[StartIndex].RoleInnate[10]);
                            if (SetInnate != null)
                            {
                                List<int> RoleFightIndex = new List<int>();
                                List<int> RoleFightCate = new List<int>();
                                List<int> RoleFightDamige = new List<int>();
                                RoleFightIndex.Add(StartIndex);
                                RoleFightCate.Add(0);
                                int BackDamige = 1;
                                BackDamige = (int)(Damige * SetInnate.Value1 / 100f);

                                if (BackDamige < 1)
                                {
                                    BackDamige = 1;
                                }
                                RoleFightDamige.Add(BackDamige);

                                EffectMaker.instance.Create2DEffect("Test", "blank", null, SetEnemyPicture[StartIndex].RoleObject.transform.position, SetEnemyPicture[StartIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0.5f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[TargetIndex].RolePictureMonster, "", null);
                            }

                            //////////////////机枪//////////////////
                            if (SetEnemyPicture[StartIndex].RoleID == 60007 || SetEnemyPicture[StartIndex].RoleID == 65007 || SetEnemyPicture[StartIndex].RoleID == 60101 || SetEnemyPicture[StartIndex].RoleID == 65101)
                            {
                                IsContinueHurt = true;
                            }
                            //////////////////机枪//////////////////

                            ////////////////////击退//////////////////
                            //if (SetEnemyPicture[StartIndex].RoleSkill2 == 2012)
                            //{
                            //    if (SetEnemyPicture[StartIndex].RolePosition + PositionRow == SetPicture[TargetIndex].RolePosition)
                            //    {
                            //        if (!CheckPosition(SetPicture[TargetIndex].RolePosition + PositionRow))
                            //        {
                            //            SetPicture[TargetIndex].RolePosition += PositionRow;
                            //            SetPicture[TargetIndex].RoleObject.transform.position = ListPosition[SetPicture[TargetIndex].RolePosition];
                            //            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(9012);
                            //        }
                            //    }
                            //    else if (SetEnemyPicture[StartIndex].RolePosition - PositionRow == SetPicture[TargetIndex].RolePosition)
                            //    {
                            //        if (!CheckPosition(SetPicture[TargetIndex].RolePosition - PositionRow))
                            //        {
                            //            SetPicture[TargetIndex].RolePosition -= PositionRow;
                            //            SetPicture[TargetIndex].RoleObject.transform.position = ListPosition[SetPicture[TargetIndex].RolePosition];
                            //            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(9012);
                            //        }
                            //    }
                            //    else if (SetPicture[TargetIndex].RolePosition % PositionRow == 1 || SetPicture[TargetIndex].RolePosition % PositionRow == 0)
                            //    {
                            //    }
                            //    else if (SetEnemyPicture[StartIndex].RolePosition + 1 == SetPicture[TargetIndex].RolePosition)
                            //    {
                            //        if (!CheckPosition(SetPicture[TargetIndex].RolePosition + 1))
                            //        {
                            //            SetPicture[TargetIndex].RolePosition += 1;
                            //            SetPicture[TargetIndex].RoleObject.transform.position = ListPosition[SetPicture[TargetIndex].RolePosition];
                            //            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(9012);
                            //        }
                            //    }
                            //    else if (SetEnemyPicture[StartIndex].RolePosition - (PositionRow - 1) == SetPicture[TargetIndex].RolePosition)
                            //    {
                            //        if (!CheckPosition(SetPicture[TargetIndex].RolePosition - (PositionRow - 1)))
                            //        {
                            //            SetPicture[TargetIndex].RolePosition -= (PositionRow - 1);
                            //            SetPicture[TargetIndex].RoleObject.transform.position = ListPosition[SetPicture[TargetIndex].RolePosition];
                            //            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(9012);
                            //        }
                            //    }
                            //    else if (SetEnemyPicture[StartIndex].RolePosition + (PositionRow - 1) == SetPicture[TargetIndex].RolePosition)
                            //    {
                            //        if (!CheckPosition(SetPicture[TargetIndex].RolePosition + (PositionRow - 1)))
                            //        {
                            //            SetPicture[TargetIndex].RolePosition += (PositionRow - 1);
                            //            SetPicture[TargetIndex].RoleObject.transform.position = ListPosition[SetPicture[TargetIndex].RolePosition];
                            //            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(9012);
                            //        }
                            //    }
                            //    else if (SetEnemyPicture[StartIndex].RolePosition - 1 == SetPicture[TargetIndex].RolePosition)
                            //    {
                            //        if (!CheckPosition(SetPicture[TargetIndex].RolePosition - 1))
                            //        {
                            //            SetPicture[TargetIndex].RolePosition -= 1;
                            //            SetPicture[TargetIndex].RoleObject.transform.position = ListPosition[SetPicture[TargetIndex].RolePosition];
                            //            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(9012);
                            //        }
                            //    }
                            //}
                            ////////////////////击退//////////////////

                            if (SetPicture[TargetIndex].RoleID == 65390)
                            {
                                if (SetPicture[TargetIndex].RoleNowBlood / SetPicture[TargetIndex].RoleMaxBlood < 0.001f)
                                {
                                    if (GameObject.Find("65390_1") != null)
                                    {
                                        DestroyImmediate(SetPicture[TargetIndex].RolePictureObject);
                                        SetPicture[TargetIndex].RolePictureObject = GameObject.Instantiate(Resources.Load("Prefab/Role/65390_2", typeof(GameObject))) as GameObject;
                                        SetPicture[TargetIndex].RolePictureObject.transform.localScale = new Vector3(2, 2, 2);
                                        SetPicture[TargetIndex].RolePictureObject.transform.position = SetPicture[TargetIndex].RoleObject.transform.position;
                                        SetPicture[TargetIndex].RolePictureObject.transform.Rotate(0, -180, 0);
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/YunChaoChe_03", typeof(GameObject)), SetPicture[TargetIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                                        go.transform.parent = SetPicture[TargetIndex].RolePictureObject.transform;
                                        SetPicture[TargetIndex].RolePictureObject.transform.parent = SetPicture[TargetIndex].RoleObject.transform;
                                    }
                                }
                                else if (SetPicture[TargetIndex].RoleNowBlood / SetPicture[TargetIndex].RoleMaxBlood < 0.5f)
                                {
                                    if (GameObject.Find("65390") != null)
                                    {
                                        DestroyImmediate(SetPicture[TargetIndex].RolePictureObject);
                                        SetPicture[TargetIndex].RolePictureObject = GameObject.Instantiate(Resources.Load("Prefab/Role/65390_1", typeof(GameObject))) as GameObject;
                                        SetPicture[TargetIndex].RolePictureObject.transform.localScale = new Vector3(2, 2, 2);
                                        SetPicture[TargetIndex].RolePictureObject.transform.position = SetPicture[TargetIndex].RoleObject.transform.position;
                                        SetPicture[TargetIndex].RolePictureObject.transform.Rotate(0, -180, 0);
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/YunChaoChe_02", typeof(GameObject)), SetPicture[TargetIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                                        go.transform.parent = SetPicture[TargetIndex].RolePictureObject.transform;
                                        SetPicture[TargetIndex].RolePictureObject.transform.parent = SetPicture[TargetIndex].RoleObject.transform;
                                    }
                                }
                            }
                            else if (SetPicture[TargetIndex].RoleID == 65391)
                            {
                                if (SetPicture[TargetIndex].RoleNowBlood / SetPicture[TargetIndex].RoleMaxBlood < 0.001f)
                                {
                                    if (GameObject.Find("65391_1") != null)
                                    {
                                        DestroyImmediate(SetPicture[TargetIndex].RolePictureObject);
                                        SetPicture[TargetIndex].RolePictureObject = GameObject.Instantiate(Resources.Load("Prefab/Role/65391_2", typeof(GameObject))) as GameObject;
                                        SetPicture[TargetIndex].RolePictureObject.transform.localScale = new Vector3(2, 2, 2);
                                        SetPicture[TargetIndex].RolePictureObject.transform.position = SetPicture[TargetIndex].RoleObject.transform.position;
                                        SetPicture[TargetIndex].RolePictureObject.transform.Rotate(0, -180, 0);
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/YunChaoChe_03", typeof(GameObject)), SetPicture[TargetIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                                        go.transform.parent = SetPicture[TargetIndex].RolePictureObject.transform;
                                        SetPicture[TargetIndex].RolePictureObject.transform.parent = SetPicture[TargetIndex].RoleObject.transform;
                                    }
                                }
                                else if (SetPicture[TargetIndex].RoleNowBlood / SetPicture[TargetIndex].RoleMaxBlood < 0.5f)
                                {
                                    if (GameObject.Find("65391") != null)
                                    {
                                        DestroyImmediate(SetPicture[TargetIndex].RolePictureObject);
                                        SetPicture[TargetIndex].RolePictureObject = GameObject.Instantiate(Resources.Load("Prefab/Role/65391_1", typeof(GameObject))) as GameObject;
                                        SetPicture[TargetIndex].RolePictureObject.transform.localScale = new Vector3(2, 2, 2);
                                        SetPicture[TargetIndex].RolePictureObject.transform.position = SetPicture[TargetIndex].RoleObject.transform.position;
                                        SetPicture[TargetIndex].RolePictureObject.transform.Rotate(0, -180, 0);
                                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/YunChaoChe_02", typeof(GameObject)), SetPicture[TargetIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                                        go.transform.parent = SetPicture[TargetIndex].RolePictureObject.transform;
                                        SetPicture[TargetIndex].RolePictureObject.transform.parent = SetPicture[TargetIndex].RoleObject.transform;
                                    }
                                }
                            }
                        }
                        else
                        {
                            IsAvoid = true;
                            //Debug.LogError("MissMissMissMissMissMissMissMissMissMissMiss");
                            EffectMaker.instance.Create2DEffect("~Miss", "", null, SetPicture[TargetIndex].RoleObject.transform.position, SetPicture[TargetIndex].RoleObject.transform.position, SetPicture[TargetIndex].RolePictureObject.transform.localEulerAngles, 0.1f, 0, 3f, 1, 1, TargetIndex, null, null, null, null, null, false, false, false, true, false, false, false, !IsEnemy, SetPicture[TargetIndex].RolePictureMonster, "", null);
                        }
                    }
                    else
                    {
                        if (!IsSkip)
                        {
                            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().mText.Add(-Damige, Color.yellow, 0f);
                        }
                    }

                    if (SetPicture[TargetIndex].RoleID == 65390 || SetPicture[TargetIndex].RoleID == 65391)
                    {
                        if (UnityEngine.Random.Range(0, 100) > 70)
                        {
                            GameObject hurt = GameObject.Instantiate(Resources.Load("Prefab/Effect/Truck_hit01", typeof(GameObject)), SetPicture[TargetIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                            hurt.transform.parent = SetPicture[TargetIndex].RolePictureObject.transform;
                        }
                        else
                        {
                            GameObject hurt = GameObject.Instantiate(Resources.Load("Prefab/Effect/Truck_hit02", typeof(GameObject)), SetPicture[TargetIndex].RoleObject.transform.position, Quaternion.identity) as GameObject;
                            hurt.transform.parent = SetPicture[TargetIndex].RolePictureObject.transform;
                        }
                    }
                }
                //////////////////扺消攻击//////////////////
            }
            else
            {
                //////////////////暴击//////////////////
                if (StartIndex > -1)
                {
                    float Crit = (SetPicture[StartIndex].RoleCrit + SetPicture[StartIndex].BuffCrit) / 10;
                    float RandomNum = GetRandom(SetPicture[StartIndex].RoleID);
                    //Debug.Log("Crit" + Crit.ToString() + " RandomNum" + RandomNum.ToString());
                    if (RandomNum < Crit)
                    {
                        Damige *= 2;
                        StartCoroutine(SharkCamera());
                    }
                }
                //////////////////暴击//////////////////



                Damige = (int)(Damige * (1 - SetPicture[TargetIndex].BuffNoHp)); //改变加血效果
                if (!IsSkip)
                {
                    SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().mText.Add(-Damige, Color.green, 0f);
                }
            }


            bool IsFinish = true;

            if (StartIndex > -1)
            {
                GameObject go1 = (GameObject)Instantiate(Resources.Load("Prefab/Effect/XingDong06"));
                go1.transform.position = SetPicture[TargetIndex].RoleObject.transform.position;
                Innates SetInnate;
                if (Damige > 0)
                {
                    //加伤   2普攻伤害提升  4达到血量加伤
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(4, SetEnemyPicture[StartIndex].RoleInnate[3]);
                    if (SetInnate != null)
                    {
                        Damige *= (int)(SetInnate.Value1 / 100f);
                    }

                    //吸血   5攻击吸血
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(5, SetEnemyPicture[StartIndex].RoleInnate[4]);
                    if (SetInnate != null)
                    {
                        List<int> RoleFightIndex = new List<int>();
                        List<int> RoleFightCate = new List<int>();
                        List<int> RoleFightDamige = new List<int>();
                        RoleFightIndex.Add(StartIndex);
                        RoleFightCate.Add(0);
                        RoleFightDamige.Add((int)(-Damige * SetInnate.Value1 / 100f));
                        EffectMaker.instance.Create2DEffect("Test", "blank", null, SetEnemyPicture[StartIndex].RoleObject.transform.position, SetEnemyPicture[StartIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 1.5f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetEnemyPicture[StartIndex].RolePictureMonster, "", null);
                    }

                    //怒气   6杀人得怒     15概率最少怒得怒      18夺人怒气
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(15, SetEnemyPicture[StartIndex].RoleInnate[14]);
                    if (SetInnate != null)
                    {
                        int RoleSkillPoint = 1000;
                        int LeastIndex = 0;
                        if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < SetInnate.Value1)
                        {
                            for (int n = 0; n < SetEnemyPicture.Count; n++)
                            {
                                if (SetEnemyPicture[n].RoleNowBlood > 0 && SetEnemyPicture[n].RoleSkillPoint < RoleSkillPoint)
                                {
                                    RoleSkillPoint = SetEnemyPicture[n].RoleSkillPoint;
                                    LeastIndex = n;
                                }
                            }
                        }

                        SetEnemyPicture[LeastIndex].RoleSkillPoint += SetInnate.Value2;
                        if (SetEnemyPicture[LeastIndex].RoleSkillPoint > 1000)
                        {
                            SetEnemyPicture[LeastIndex].RoleSkillPoint = 1000;
                        }
                    }

                    //怒气   6杀人得怒     15概率最少怒得怒      18夺人怒气
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(18, SetEnemyPicture[StartIndex].RoleInnate[17]);
                    if (SetInnate != null)
                    {
                        if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < SetInnate.Value1 && !IsSkill)
                        {
                            SetPicture[TargetIndex].RoleSkillPoint -= SetInnate.Value2;
                            if (SetPicture[TargetIndex].RoleSkillPoint < 0)
                            {
                                SetPicture[TargetIndex].RoleSkillPoint = 0;
                            }
                        }
                    }

                    //减伤   12减技能伤害
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(12, SetPicture[TargetIndex].RoleInnate[11]);
                    if (SetInnate != null)
                    {
                        if (IsSkill)
                        {
                            Damige *= (int)(SetInnate.Value1 / 100f);
                        }
                    }
                    //免伤   16致命免伤
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(16, SetPicture[TargetIndex].RoleInnate[15]);
                    if (SetInnate != null)
                    {
                        if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < SetInnate.Value1)
                        {
                            if (!IsAvoid)
                            {
                                IsAvoid = true;
                                SetPicture[TargetIndex].RoleInnate[15] = 0;
                            }
                        }
                    }

                    if (IsSkill && SetEnemyPicture[StartIndex].RoleSkill2 == 2057) //嘉米夺怒
                    {
                        int RoleSkillPoint = 1000;
                        int LeastIndex = 0;
                        for (int n = 0; n < SetEnemyPicture.Count; n++)
                        {
                            if (SetEnemyPicture[n].RoleNowBlood > 0 && SetEnemyPicture[n].RoleSkillPoint < RoleSkillPoint)
                            {
                                RoleSkillPoint = SetEnemyPicture[n].RoleSkillPoint;
                                LeastIndex = n;
                            }
                        }

                        SetEnemyPicture[LeastIndex].RoleSkillPoint += SetPicture[TargetIndex].RoleSkillPoint;
                        if (SetEnemyPicture[LeastIndex].RoleSkillPoint > 1000)
                        {
                            SetEnemyPicture[LeastIndex].RoleSkillPoint = 1000;
                        }
                        SetPicture[TargetIndex].RoleSkillPoint = 0;
                    }
                }

                if (Damige > 0)
                {
                    //行动   17概率行动一次
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(17, SetEnemyPicture[StartIndex].RoleInnate[16]);
                    if (SetInnate != null)
                    {
                        if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < SetInnate.Value1)
                        {
                            if (!IsSkip)
                            {
                                yield return new WaitForSeconds(0.9f);
                            }
                            IsFinish = false;
                            ListCloseNode.Clear();
                            ListOpenNode.Clear();
                            ListFindNode.Clear();
                            SetEnemyPicture[StartIndex].RolePictureStartAttack = false;
                            SetEnemyPicture[StartIndex].RoleMoveNowStep = 0;
                            SetEnemyPicture[StartIndex].RoleInnate[16] = 0;
                        }
                    }
                }
                else
                {
                    //行动   17概率行动一次
                    SetInnate = TextTranslator.instance.GetInnatesByTwo(17, SetPicture[StartIndex].RoleInnate[16]);
                    if (SetInnate != null)
                    {
                        if (GetRandom(SetPicture[StartIndex].RoleID) < SetInnate.Value1)
                        {
                            if (!IsSkip)
                            {
                                yield return new WaitForSeconds(0.9f);
                            }
                            IsFinish = false;
                            ListCloseNode.Clear();
                            ListOpenNode.Clear();
                            ListFindNode.Clear();
                            SetPicture[StartIndex].RolePictureStartAttack = false;
                            SetPicture[StartIndex].RoleMoveNowStep = 0;
                            SetPicture[StartIndex].RoleInnate[16] = 0;
                        }
                    }
                }
            }
            //Debug.LogError(SetPicture[TargetIndex].RoleHurtCount + "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB" + TargetIndex);
            if (!IsAvoid)
            {
                float ChangeBlood = SetPicture[TargetIndex].RoleNowBlood;
                SetPicture[TargetIndex].RoleNowBlood -= Damige;
                float OffsetBlood = (SetPicture[TargetIndex].RoleNowBlood - ChangeBlood) / 10f;
                //Debug.LogError(SetPicture[TargetIndex].RoleID + " " + IsEnemy + " " + TargetIndex + " " + SetPicture[TargetIndex].RoleNowBlood + " " + Damige + " " + ListSequence.Count);

                if (SetPicture[TargetIndex].RoleNowBlood > SetPicture[TargetIndex].RoleMaxBlood)
                {
                    SetPicture[TargetIndex].RoleNowBlood = SetPicture[TargetIndex].RoleMaxBlood;
                }

                StartCoroutine(DelayChangeBlood(SetPicture, TargetIndex, ChangeBlood, OffsetBlood));
                //for (int i = 0; i < 10; i++)
                //{
                //    ChangeBlood += OffsetBlood;
                //    /////////////////血量条改变数值(以下)/////////////////
                //    SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().mSlider.value = ChangeBlood / SetPicture[TargetIndex].RoleMaxBlood;
                //    /////////////////血量条改变数值(以上)/////////////////
                //    yield return new WaitForEndOfFrame();
                //}

                /////////////////////////////////////////////////////////
                if (SetPicture[TargetIndex].BuffAvoidDead)
                {
                    if (SetPicture[TargetIndex].RoleNowBlood < 1)
                    {
                        SetPicture[TargetIndex].RoleNowBlood = 1;
                    }
                }
                /////////////////////////////////////////////////////////

                if (SetPicture[TargetIndex].RoleNowBlood < 1)
                {
                    SetPicture[TargetIndex].RoleRedBloodObject.SetActive(false);
                    SetPicture[TargetIndex].RoleSkillPoint = 0;
                    //  Debug.LogError(SetPicture[TargetIndex].RolePosition);
                    //GameObject.Find("Base" + SetPicture[TargetIndex].RolePosition).transform.Find("SkillObject").gameObject.SetActive(false);
                    //GameObject.Find("Base" + SetPicture[TargetIndex].RolePosition).transform.Find("BloodObject").gameObject.SetActive(false);


                    if (SetPicture[TargetIndex].RoleNowBlood > -99999999)
                    {
                        //怒气   6杀人得怒     15概率最少怒得怒      18夺人怒气
                        if (StartIndex > -1)
                        {
                            Innates SetInnate = TextTranslator.instance.GetInnatesByTwo(6, SetEnemyPicture[StartIndex].RoleInnate[5]);
                            if (SetInnate != null)
                            {
                                for (int n = 0; n < SetEnemyPicture.Count; n++)
                                {
                                    SetEnemyPicture[n].RoleSkillPoint += SetInnate.Value1;
                                    if (SetEnemyPicture[n].RoleSkillPoint > 1000)
                                    {
                                        SetEnemyPicture[n].RoleSkillPoint = 1000;
                                    }
                                }
                            }
                        }
                        //////////////////隐身//////////////////
                        if (SetPicture[TargetIndex].BuffInvisible)
                        {
                            if (SetPicture[TargetIndex].RolePictureMonster)
                            {
                                foreach (var r in ListRolePicture)
                                {
                                    if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                                    {
                                        Destroy(GameObject.Find("WenHao" + r.RoleObject.name));
                                    }
                                }
                            }
                            else
                            {
                                foreach (var r in ListEnemyPicture)
                                {
                                    if (r.RolePosition > 0 && r.RoleNowBlood > 0)
                                    {
                                        Destroy(GameObject.Find("WenHao" + r.RoleObject.name));
                                    }
                                }
                            }
                        }
                        //////////////////隐身//////////////////

                        RemoveRoleBuff(SetPicture, TargetIndex);
                        if (!SetPicture[TargetIndex].IsPicture)
                        {
                            SetPicture[TargetIndex].RolePictureObject.GetComponent<Animator>().Play("dead");
                            if (SetPicture[TargetIndex].RoleID == 65901)
                            {
                                SetPicture[TargetIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("dead");
                            }
                        }
                        SetPicture[TargetIndex].RolePictureAttackable = false;
                        SetPicture[TargetIndex].RoleNowBlood = -99999999;




                        if (SetPicture[TargetIndex].RoleRace == 3)
                        {
                            SetPicture[TargetIndex].RolePictureTimer = 2;
                            SetPicture[TargetIndex].RolePictureTransparent = 0.1f;
                            GameObject BlowObject = GameObject.Instantiate(Resources.Load("Prefab/Effect/TongYong_dead02", typeof(GameObject)), ListPosition[SetPicture[TargetIndex].RolePosition], Quaternion.identity) as GameObject;
                            BlowObject.name = "BlowObject";
                        }
                        else if (SetPicture[TargetIndex].RoleRace == 2)
                        {
                            SetPicture[TargetIndex].RolePictureTimer = 2;
                            SetPicture[TargetIndex].RolePictureTransparent = 0.1f;
                            GameObject BlowObject = GameObject.Instantiate(Resources.Load("Prefab/Effect/TongYong_dead01", typeof(GameObject)), ListPosition[SetPicture[TargetIndex].RolePosition], Quaternion.identity) as GameObject;
                            BlowObject.name = "BlowObject";
                        }

                        //////////////////再战//////////////////
                        if (SetPicture[TargetIndex].RoleSkill2 == 2011)
                        {
                            if (SetPicture[TargetIndex].RolePictureMonster)
                            {
                                CreateRole(65001, "伞兵", SetPicture[TargetIndex].RolePosition, Color.cyan, SetPicture[TargetIndex].RoleMaxBlood / 10f, SetPicture[TargetIndex].RoleMaxBlood / 10f, SetPicture[TargetIndex].RoleHit, 11.6f, true, false, 1, 1.5f, SetPicture[TargetIndex].RoleNoHit, "Enemy60001", 0, SetPicture[TargetIndex].RolePAttack, SetPicture[TargetIndex].RoleCrit, SetPicture[TargetIndex].RolePDefend, SetPicture[TargetIndex].RoleNoCrit, SetPicture[TargetIndex].RoleDamigeAdd, SetPicture[TargetIndex].RoleDamigeReduce, 1001, 2001, 0, 1, 0, 1, 2, 0, "");
                            }
                            else
                            {
                                CreateRole(60001, "伞兵", SetPicture[TargetIndex].RolePosition, Color.red, SetPicture[TargetIndex].RoleMaxBlood / 10f, SetPicture[TargetIndex].RoleMaxBlood / 10f, SetPicture[TargetIndex].RoleHit, 11.6f, false, false, 1, 1.5f, SetPicture[TargetIndex].RoleNoHit, "Enemy60001", 0, SetPicture[TargetIndex].RolePAttack, SetPicture[TargetIndex].RoleCrit, SetPicture[TargetIndex].RolePDefend, SetPicture[TargetIndex].RoleNoCrit, SetPicture[TargetIndex].RoleDamigeAdd, SetPicture[TargetIndex].RoleDamigeReduce, 1001, 2001, 0, 1, 0, 1, 2, 0, "");
                            }
                            InsertSequence(SetPicture, SetPicture.Count - 1, SetPicture[TargetIndex].RolePictureMonster);
                            if (!IsSkip)
                            {
                                yield return new WaitForSeconds(2f);
                            }
                        }
                        //////////////////再战//////////////////

                        //////////////////死里逃生//////////////////
                        if (SetPicture[TargetIndex].RoleRelife > 0)
                        {
                            PictureCreater.instance.AttackCount++;
                            float RandomNum = GetRandom(SetPicture[TargetIndex].RoleID);
                            if (RandomNum < SetPicture[TargetIndex].RoleRelife)
                            {
                                //int i = 0;
                                //foreach (var r in SetPicture)
                                //{
                                //    if (r.RolePosition > 0 && r.RoleNowBlood < 1 && r.RoleSkill2 != 8888)
                                //    {
                                //        if (r.RolePicturePointID == KillEnemyID.ToString())
                                //        {
                                //            break;
                                //        }
                                //        //r.RolePosition = 99;
                                //        r.RoleNowBlood = r.RoleMaxBlood / 10;
                                //        r.RoleFightNowSpeed = 0;
                                //        r.RoleSkillPoint = -1;
                                //        InsertSequence(SetPicture, i, r.RolePictureMonster);
                                //        //r.RoleObject.SetActive(true);
                                //        yield return new WaitForSeconds(3.5f);
                                //        break;
                                //    }
                                //    i++;
                                //}

                                if (SetPicture[TargetIndex].RolePicturePointID == KillEnemyID.ToString())
                                {

                                }
                                else
                                {
                                    SetPicture[TargetIndex].RoleNowBlood = 1;
                                    if (!IsSkip)
                                    {
                                        yield return new WaitForSeconds(3.5f);
                                    }

                                    SetPicture[TargetIndex].RolePictureAttackable = true;
                                    SetPicture[TargetIndex].RoleObject.transform.position = ListPosition[SetPicture[TargetIndex].RolePosition];

                                    AudioEditer.instance.PlayOneShot("ui_qianghua");
                                    GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/WF_revive", typeof(GameObject)), ListPosition[SetPicture[TargetIndex].RolePosition], Quaternion.identity) as GameObject;
                                    go.AddComponent<DestroySelf>();

                                    SetPicture[TargetIndex].RolePictureObject.GetComponent<Animator>().Play("idle");
                                    if (SetPicture[TargetIndex].RoleID == 65901)
                                    {
                                        SetPicture[TargetIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("idle");
                                    }

                                    if (SetPicture[TargetIndex].RoleSkill2 == 2040)
                                    {
                                        SetPicture[TargetIndex].RoleSkillPoint = 0;
                                        SetPicture[TargetIndex].RoleRelife = 0;
                                        SetPicture[TargetIndex].RoleNowBlood = SetPicture[TargetIndex].RoleMaxBlood / 4;
                                    }
                                    else
                                    {
                                        SetPicture[TargetIndex].RoleSkillPoint = 1000;
                                        SetPicture[TargetIndex].RoleRelife /= 2;
                                        SetPicture[TargetIndex].RoleNowBlood = SetPicture[TargetIndex].RoleMaxBlood * 0.35f;
                                    }
                                }
                            }
                            else
                            {
                                ResetSequence(TargetIndex, SetPicture[TargetIndex].RolePictureMonster);
                            }
                            PictureCreater.instance.AttackCount--;
                        }
                        else
                        {
                            ResetSequence(TargetIndex, SetPicture[TargetIndex].RolePictureMonster);
                        }
                        //////////////////死里逃生//////////////////

                        /////////////////////多杀/////////////////////
                        if (StartIndex > -1)
                        {
                            SetEnemyPicture[StartIndex].RoleContinueKill++;

                            SetEnemyPicture[StartIndex].RoleSkillPoint += 400; //杀死人加怒
                            SetEnemyPicture[StartIndex].RoleRedBloodObject.SetActive(true);
                            SetEnemyPicture[StartIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetEnemyPicture[StartIndex].RoleSkillPoint);
                            SetEnemyPicture[StartIndex].RoleObject.GetComponent<ColliderDisplayText>().AddRage();
                        }
                        /////////////////////多杀/////////////////////
                    }
                    for (int PartnerIndex = 0; PartnerIndex < SetEnemyPicture.Count; PartnerIndex++)
                    {
                        if (SetEnemyPicture[PartnerIndex].RoleFightIndex.Contains(TargetIndex))
                        {
                            SetEnemyPicture[PartnerIndex].RoleFightIndex.Remove(TargetIndex);
                            SetEnemyPicture[PartnerIndex].RolePictureFight = false;
                        }

                        if (SetEnemyPicture[PartnerIndex].RoleTargetIndex == TargetIndex)
                        {
                            SetEnemyPicture[PartnerIndex].RoleTargetIndex = -1;
                            SetEnemyPicture[PartnerIndex].RoleMovePosition = 0;
                            SetEnemyPicture[PartnerIndex].RoleTargetObject.SetActive(false);
                        }
                    }

                    foreach (var r in SetEnemyPicture)
                    {
                        if (r.RoleTargetIndex == TargetIndex)
                        {
                            r.RoleTargetIndex = -1;
                        }
                    }

                    if (StartIndex > -1)
                    {
                        //////////////////连击//////////////////
                        if (SetEnemyPicture[StartIndex].RoleSkill2 == 2002)
                        {
                            if (!IsSkip)
                            {
                                yield return new WaitForSeconds(0.9f);
                            }
                            IsFinish = false;
                            ListCloseNode.Clear();
                            ListOpenNode.Clear();
                            ListFindNode.Clear();
                            SetEnemyPicture[StartIndex].RolePictureStartAttack = false;
                            SetEnemyPicture[StartIndex].RoleMoveNowStep = 0;
                            SetEnemyPicture[StartIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(2002);
                        }
                        //////////////////连击//////////////////

                        //////////////////连击//////////////////
                        if (SetEnemyPicture[StartIndex].RoleSkill2 == 2053)
                        {
                            if (!IsSkip)
                            {
                                yield return new WaitForSeconds(0.9f);
                            }
                            IsFinish = false;
                            ListCloseNode.Clear();
                            ListOpenNode.Clear();
                            ListFindNode.Clear();
                            SetEnemyPicture[StartIndex].RolePictureStartAttack = false;
                            SetEnemyPicture[StartIndex].RoleMoveNowStep = 2;
                            SetEnemyPicture[StartIndex].RoleSkillPoint = 1000;
                            SetEnemyPicture[StartIndex].RoleObject.GetComponent<ColliderDisplayText>().SetBuffText(2002);
                        }
                        //////////////////连击//////////////////

                        //////////////////死亡诅咒////////////////////
                        if (SetPicture[TargetIndex].RoleSkill2 == 2011)
                        {
                            Buff NewBuff = TextTranslator.instance.GetBuffByID(286); //死亡诅咒
                            RoleAddBuff(SetEnemyPicture, StartIndex, NewBuff);
                            NewBuff = TextTranslator.instance.GetBuffByID(287); //死亡诅咒
                            RoleAddBuff(SetEnemyPicture, StartIndex, NewBuff);
                        }
                        //////////////////死亡诅咒////////////////////

                        //////////////////自爆//////////////////
                        //if (UnityEngine.Random.Range(0, 100) < 20)
                        {
                            if (SetPicture[TargetIndex].RoleSkill2 == 2006)
                            {
                                List<int> RoleFightIndex = new List<int>();
                                List<int> RoleFightCate = new List<int>();
                                List<int> RoleFightDamige = new List<int>();

                                for (int i = 0; i < SetEnemyPicture.Count; i++)
                                {
                                    if (Vector3.Distance(SetEnemyPicture[i].RoleObject.transform.position, SetPicture[TargetIndex].RoleObject.transform.position) < 2 * OffsetZ)
                                    {
                                        if (SetEnemyPicture[i].RoleNowBlood > 0)
                                        {
                                            RoleFightIndex.Add(i);
                                            RoleFightCate.Add(0);
                                            RoleFightDamige.Add((int)(SetPicture[TargetIndex].RolePAttack * 0.25f));
                                        }
                                    }
                                }

                                EffectMaker.instance.Create2DEffect("~huoyanbing_Dead", "", null, SetPicture[TargetIndex].RoleObject.transform.position, SetPicture[TargetIndex].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0.5f, 2, 6, 2, -1, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, SetPicture[TargetIndex].RolePictureMonster, "", null);
                                AudioEditer.instance.PlayOneShot("Hit_boom");
                            }
                        }
                        //////////////////自爆//////////////////
                    }


                    ////////////////有一方没人了////////////////
                    int ActiveCount = 0;
                    foreach (RolePicture r in SetPicture)
                    {
                        if ((r.RoleNowBlood > 0 && r.RolePosition > 0) || r.RoleSkillPoint == -1)
                        {
                            ActiveCount++;
                        }
                    }

                    if (ActiveCount == 0 && IsFight && Damige != 0)
                    {
                        IsFight = false;

                        StartCoroutine(FightBoss());
                        if (!IsEnemy)
                        {
                            if (PictureCreater.instance.FightStyle == 6 || PictureCreater.instance.FightStyle == 7)
                            {
                                fw.OneSlider.SetActive(false);
                                fw.EverydayBossobj.transform.Find("HPNum").GetComponent<UILabel>().text = "X0";
                            }
                            StartCoroutine(ShowWin(SetEnemyPicture));//fightstyle  6 7 关闭

                        }
                        else
                        {
                            StartCoroutine(ShowLose());
                        }
                    }
                    else
                    {
                        if (SetPicture[TargetIndex].RolePicturePointID == KillEnemyID.ToString())
                        {
                            IsFight = false;
                            StartCoroutine(FightBoss());
                            StartCoroutine(ShowWin(SetEnemyPicture));
                        }
                        else if (SetPicture[TargetIndex].RolePicturePointID == "Enemy" + NPCID.ToString())
                        {
                            IsFight = false;
                            StartCoroutine(FightBoss());
                            StartCoroutine(ShowLose());
                        }
                    }
                    ////////////////有一方没人了////////////////
                }
                else
                {
                    if (Damige > 0 && !IsSkip)
                    {
                        if (StartIndex != -1)
                        {
                            if (!SetPicture[TargetIndex].IsPicture)
                            {
                                AnimatorStateInfo animatorState = SetPicture[TargetIndex].RolePictureObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                                if (animatorState.IsName("Base Layer.idle"))
                                {
                                    SetPicture[TargetIndex].RolePictureObject.GetComponent<Animator>().CrossFade("hurt", 0);
                                    if (SetPicture[TargetIndex].RoleID == 65901)
                                    {
                                        SetPicture[TargetIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("hurt");
                                    }
                                }

                                if (SetPicture[TargetIndex].RolePictureObject.transform.Find("HeroBody2") != null)
                                {
                                    if (SetPicture[TargetIndex].RolePictureObject.transform.Find("HeroBody") != null)
                                    {
                                        SkinnedMeshRenderer s = SetPicture[TargetIndex].RolePictureObject.transform.Find("HeroBody").gameObject.GetComponent<SkinnedMeshRenderer>();
                                        if (s.materials.Length > 1)
                                        {
                                            Material[] ListMaterial = new Material[s.materials.Length];
                                            for (int i = 0; i < s.materials.Length - 1; i++)
                                            {
                                                ListMaterial[i] = s.materials[i];
                                            }
                                            ListMaterial[s.materials.Length - 1] = HurtMaterial;

                                            s.materials = ListMaterial;
                                            yield return new WaitForSeconds(0.1f);

                                            if (KillEnemyID.ToString() == SetPicture[TargetIndex].RolePicturePointID || BossEnemyID.ToString() == SetPicture[TargetIndex].RolePicturePointID)
                                            {
                                                ListMaterial[s.materials.Length - 1] = Boss1Material;
                                                ListMaterial[s.materials.Length - 2] = Boss2Material;
                                            }
                                            else
                                            {
                                                ListMaterial[s.materials.Length - 1] = BlankMaterial;
                                            }
                                            s.materials = ListMaterial;


                                            SkinnedMeshRenderer s1 = SetPicture[TargetIndex].RolePictureObject.transform.Find("HeroBody2").gameObject.GetComponent<SkinnedMeshRenderer>();
                                            Material[] ListMaterial1 = new Material[s1.materials.Length];
                                            for (int i = 0; i < s1.materials.Length - 1; i++)
                                            {
                                                ListMaterial1[i] = s1.materials[i];
                                            }
                                            ListMaterial1[s1.materials.Length - 1] = HurtMaterial;

                                            s1.materials = ListMaterial1;
                                            yield return new WaitForSeconds(0.1f);
                                            if (KillEnemyID.ToString() == SetPicture[TargetIndex].RolePicturePointID || BossEnemyID.ToString() == SetPicture[TargetIndex].RolePicturePointID)
                                            {
                                                ListMaterial1[s1.materials.Length - 1] = Boss1Material;
                                                ListMaterial1[s1.materials.Length - 2] = Boss2Material;
                                            }
                                            else
                                            {
                                                ListMaterial1[s1.materials.Length - 1] = BlankMaterial;
                                            }
                                            s1.materials = ListMaterial1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (SetPicture[TargetIndex].RolePictureObject.transform.Find("HeroBody") != null)
                                    {
                                        SkinnedMeshRenderer s = SetPicture[TargetIndex].RolePictureObject.transform.Find("HeroBody").gameObject.GetComponent<SkinnedMeshRenderer>();
                                        if (s.materials.Length > 1)
                                        {
                                            Material[] ListMaterial = new Material[s.materials.Length];
                                            for (int i = 0; i < s.materials.Length - 1; i++)
                                            {
                                                ListMaterial[i] = s.materials[i];
                                            }
                                            ListMaterial[s.materials.Length - 1] = HurtMaterial;
                                            s.materials = ListMaterial;
                                            yield return new WaitForSeconds(0.1f);
                                            if (KillEnemyID.ToString() == SetPicture[TargetIndex].RolePicturePointID || BossEnemyID.ToString() == SetPicture[TargetIndex].RolePicturePointID)
                                            {
                                                ListMaterial[s.materials.Length - 1] = Boss1Material;
                                                ListMaterial[s.materials.Length - 2] = Boss2Material;
                                            }
                                            else
                                            {
                                                ListMaterial[s.materials.Length - 1] = BlankMaterial;
                                            }
                                            s.materials = ListMaterial;
                                        }
                                    }
                                }


                                if (IsContinueHurt)
                                {
                                    yield return new WaitForSeconds(0.3f);
                                    SetPicture[TargetIndex].RolePictureObject.GetComponent<Animator>().CrossFade("hurt", 0);
                                    if (SetPicture[TargetIndex].RoleID == 65901)
                                    {
                                        SetPicture[TargetIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("hurt");
                                    }
                                    yield return new WaitForSeconds(0.3f);
                                    SetPicture[TargetIndex].RolePictureObject.GetComponent<Animator>().CrossFade("hurt", 0);
                                    if (SetPicture[TargetIndex].RoleID == 65901)
                                    {
                                        SetPicture[TargetIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("hurt");
                                    }
                                    yield return new WaitForSeconds(0.3f);
                                    SetPicture[TargetIndex].RolePictureObject.GetComponent<Animator>().CrossFade("hurt", 0);
                                    if (SetPicture[TargetIndex].RoleID == 65901)
                                    {
                                        SetPicture[TargetIndex].RolePictureObject.transform.Find("Wesker").gameObject.GetComponent<Animator>().Play("hurt");
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }

                int HurtCount = 1;
                ///////////////新版怒气计算///////////////
                if (IsAvoid)
                {
                    Damige = 0;
                }
                if (!IsSkill)
                {
                    if (StartIndex > -1) //普攻的攻方加怒
                    {
                        if (Damige >= 0)
                        {
                            //if (Damige > SetPicture[TargetIndex].RoleMaxBlood)
                            //{
                            //    SetEnemyPicture[StartIndex].RoleSkillPoint += 390;
                            //}
                            //else
                            //{
                            //    SetEnemyPicture[StartIndex].RoleSkillPoint += (int)(90 + 300 * Damige / SetPicture[TargetIndex].RoleMaxBlood);
                            //}

                            SetEnemyPicture[StartIndex].RoleSkillPoint += 300;
                            SetEnemyPicture[StartIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetEnemyPicture[StartIndex].RoleSkillPoint);
                        }
                        else if (Damige < 0)
                        {
                            //Debug.LogError(SetPicture.Count + " " + TargetIndex);
                            //if (-Damige > SetPicture[TargetIndex].RoleMaxBlood)
                            //{
                            //    SetPicture[StartIndex].RoleSkillPoint += 390;
                            //}
                            //else
                            //{
                            //    SetPicture[StartIndex].RoleSkillPoint += (int)(90 + 300 * -Damige / SetPicture[TargetIndex].RoleMaxBlood);
                            //}

                            SetPicture[StartIndex].RoleSkillPoint += 300;
                            SetPicture[StartIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[StartIndex].RoleSkillPoint);
                        }
                    }
                }
                else
                {
                    if (StartIndex > -1) //普攻的攻方加怒
                    {
                        if (Damige > 0)
                        {
                            Skill ActiveSkill = TextTranslator.instance.GetSkillByID(SetEnemyPicture[StartIndex].RoleSkill1, SetEnemyPicture[StartIndex].RoleSkillLevel1);
                            HurtCount = ActiveSkill.weather;
                        }
                        else
                        {
                            Skill ActiveSkill = TextTranslator.instance.GetSkillByID(SetPicture[StartIndex].RoleSkill1, SetPicture[StartIndex].RoleSkillLevel1);
                            HurtCount = ActiveSkill.weather;
                        }
                    }
                }


                if (Damige > 0 && StartIndex > -1)
                {
                    SetPicture[TargetIndex].RoleHurtCount++;
                    //Debug.LogError(SetPicture[TargetIndex].RoleHurtCount + "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB" + HurtCount);

                    if (SetPicture[TargetIndex].RoleHurtCount >= HurtCount)
                    {
                        //Debug.LogError("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB" + SetPicture[TargetIndex].RoleSkillPoint);
                        SetPicture[TargetIndex].RoleHurtCount = 0;
                        //SetPicture[TargetIndex].RoleSkillPoint += (int)(150 + 1500 * Damige / SetPicture[TargetIndex].RoleMaxBlood); //我方被打加怒                    
                        if (IsSkill)
                        {
                            SetPicture[TargetIndex].RoleSkillPoint += 150; //被技能打
                        }
                        else
                        {
                            SetPicture[TargetIndex].RoleSkillPoint += 200; //被打
                        }
                        //Debug.LogError("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB" + SetPicture[TargetIndex].RoleSkillPoint);
                        SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[TargetIndex].RoleSkillPoint);
                        //Debug.LogError(SetPicture[TargetIndex].RoleSkillPoint + " " + TargetIndex);
                        int SkillID = SetEnemyPicture[StartIndex].RoleSkill1;
                        Skill ActiveSkill = TextTranslator.instance.GetSkillByID(SkillID, SetEnemyPicture[StartIndex].RoleSkillLevel1);
                        Buff NewBuff = null;
                        if (IsSkill)
                        {
                            switch (SkillID) //技能特别效果
                            {
                                case 1009: //爆弹概率晕                                    
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;
                                case 1010: //布雷斯塔概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;
                                case 1022: //斯内克有概率晕
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1024: //李昂概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1028: //威斯克有概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;


                                case 1032: //劳拉有概率禁足
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1039: //维嘉有概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillVal2)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillDuration2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;
                                case 1011: //霹雳游侠有概率减怒
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1023: //雷电有概率减怒
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1030: //欧巴回怒气
                                    for (int i = 0; i < SetEnemyPicture.Count; i++) //回怒
                                    {
                                        if (SetEnemyPicture[i].RolePosition > 0 && SetEnemyPicture[i].RoleNowBlood > 0)
                                        {
                                            Buff AddBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillDuration1);
                                            if (AddBuff != null)
                                            {
                                                RoleAddBuff(SetEnemyPicture, i, new Buff(AddBuff));
                                            }
                                        }
                                    }
                                    break;
                                case 1044: //巴洛克沉默后排

                                    if (SetPicture[TargetIndex].RoleArea > 1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(297);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }

                                    break;
                            }
                        }

                        SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[TargetIndex].RoleSkillPoint);
                    }
                }
                ///////////////新版怒气计算///////////////

                //Debug.Log("BBBBBBBBBBBBCCCCCCCCCCc" + PictureCreater.instance.AttackCount + " " + StartIndex + " " + IsFinish + " " + IsAvoid + " " + Damige + " " + TargetIndex + " " + Rannn);



                if (StartIndex > -1)
                {
                    PictureCreater.instance.AttackCount--;
                    if (Damige > 0)
                    {
                        if (!IsFirstBlood && !SetEnemyPicture[StartIndex].RolePictureMonster && PictureCreater.instance.AttackCount == 0 && SetEnemyPicture[StartIndex].RoleContinueKill > 0)
                        {
                            ShowContinueKill(StartIndex);
                        }
                    }
                    if (PictureCreater.instance.AttackCount == 0 && IsFinish)
                    {
                        if (NetworkHandler.instance.IsCreate)
                        {
                            yield return new WaitForSeconds(0.7f);
                        }

                        if (!IsSkip)
                        {
                            if (FightStyle != 15)
                            {
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        //if (MySkill.activeSelf)
                        //{
                        //    MySkill.SetActive(false);
                        //    MySkill.transform.Find("SkillCamera").position = Vector3.zero;
                        //    MySkill.transform.Find("SkillCamera").rotation = Quaternion.identity;
                        //    MySkill.transform.position = Vector3.zero;
                        //    MySkill.transform.rotation = Quaternion.identity;
                        //    MyCamera.SetActive(true);
                        //    yield return new WaitForSeconds(1);
                        //}

                        AddSequence();
                    }
                }


                /////////////////血量条改变数值(以下)/////////////////
                SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().mSlider.value = SetPicture[TargetIndex].RoleNowBlood / SetPicture[TargetIndex].RoleMaxBlood;
                /////////////////血量条改变数值(以上)/////////////////

                if (!IsSkip)
                {
                    yield return new WaitForSeconds(3f);

                    if (SetPicture.Count > TargetIndex)
                    {
                        if (FightStyle == 2)
                        {
                            if (ListRolePicture[ListSequence[0].RoleIndex].RoleArea == 1022)  //丛林显示血量条
                            {
                                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                                {
                                    if (ListRolePicture[RoleIndex].RolePosition > 0 && ListRolePicture[RoleIndex].RoleNowBlood > 0)
                                    {
                                        ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                                    }
                                }

                                for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
                                {
                                    ListEnemyPicture[RoleIndex].RoleRedBloodObject.SetActive(false);
                                }
                            }
                            else
                            {
                                for (int RoleIndex = 0; RoleIndex < ListEnemyPicture.Count; RoleIndex++)
                                {
                                    if (ListEnemyPicture[RoleIndex].RoleNowBlood > 0)
                                    {
                                        ListEnemyPicture[RoleIndex].RoleRedBloodObject.SetActive(true);
                                    }
                                }

                                for (int RoleIndex = 0; RoleIndex < ListRolePicture.Count; RoleIndex++)
                                {
                                    if (ListSequence[0].RoleIndex != RoleIndex)
                                    {
                                        ListRolePicture[RoleIndex].RoleRedBloodObject.SetActive(false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SetPicture[TargetIndex].RoleRedBloodObject != null)
                            {
                                if (!SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SpriteTalk.activeSelf)
                                {
                                    SetPicture[TargetIndex].RoleRedBloodObject.SetActive(false);
                                }
                                SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().RemoveBuffText();
                            }
                        }
                    }
                }
            }
            else
            {
                if (StartIndex > -1)
                {
                    SetPicture[TargetIndex].RoleHurtCount++;
                    //Debug.LogError("AAAAAAAAAAAAAAAAA" + StartIndex + " " + PictureCreater.instance.AttackCount + " " + PictureCreater.instance.AttackCount + " " + IsFinish + " " +  + SetPicture[TargetIndex].RoleHurtCount);
                    int HurtCount = 1;
                    if (IsSkill)
                    {
                        Skill ActiveSkill = TextTranslator.instance.GetSkillByID(SetEnemyPicture[StartIndex].RoleSkill1, SetEnemyPicture[StartIndex].RoleSkillLevel1);
                        HurtCount = ActiveSkill.weather;
                    }
                    if (SetPicture[TargetIndex].RoleHurtCount >= HurtCount)
                    {
                        //Debug.LogError("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB" + SetPicture[TargetIndex].RoleSkillPoint);
                        SetPicture[TargetIndex].RoleHurtCount = 0;
                        //SetPicture[TargetIndex].RoleSkillPoint += (int)(150 + 1500 * Damige / SetPicture[TargetIndex].RoleMaxBlood); //我方被打加怒                    
                        if (IsSkill)
                        {
                            SetPicture[TargetIndex].RoleSkillPoint += 150; //被技能打
                        }
                        else
                        {
                            SetPicture[TargetIndex].RoleSkillPoint += 200; //被打
                        }
                        //Debug.LogError("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB" + SetPicture[TargetIndex].RoleSkillPoint);
                        SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[TargetIndex].RoleSkillPoint);
                        //Debug.LogError(SetPicture[TargetIndex].RoleSkillPoint + " " + TargetIndex);
                        int SkillID = SetEnemyPicture[StartIndex].RoleSkill1;
                        Skill ActiveSkill = TextTranslator.instance.GetSkillByID(SkillID, SetEnemyPicture[StartIndex].RoleSkillLevel1);
                        Buff NewBuff = null;
                        if (IsSkill)
                        {
                            switch (SkillID) //技能特别效果
                            {
                                case 1009: //爆弹概率晕                                    
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;
                                case 1010: //布雷斯塔概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;
                                case 1022: //斯内克有概率晕
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1024: //李昂概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1028: //威斯克有概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;


                                case 1032: //劳拉有概率禁足
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1039: //维嘉有概率沉默
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillVal2)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillDuration2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;
                                case 1011: //霹雳游侠有概率减怒
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1023: //雷电有概率减怒
                                    if (GetRandom(SetEnemyPicture[StartIndex].RoleID) < ActiveSkill.skillDuration1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.parameter2);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }
                                    break;

                                case 1030: //欧巴回怒气
                                    for (int i = 0; i < SetEnemyPicture.Count; i++) //回怒
                                    {
                                        if (SetEnemyPicture[i].RolePosition > 0 && SetEnemyPicture[i].RoleNowBlood > 0)
                                        {
                                            Buff AddBuff = TextTranslator.instance.GetBuffByID(ActiveSkill.skillDuration1);
                                            if (AddBuff != null)
                                            {
                                                RoleAddBuff(SetEnemyPicture, i, new Buff(AddBuff));
                                            }
                                        }
                                    }
                                    break;
                                case 1044: //巴洛克沉默后排

                                    if (SetPicture[TargetIndex].RoleArea > 1)
                                    {
                                        NewBuff = TextTranslator.instance.GetBuffByID(297);
                                        if (NewBuff != null)
                                        {
                                            RoleAddBuff(SetPicture, TargetIndex, NewBuff);
                                        }
                                    }

                                    break;
                            }
                        }

                        SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().SetSkillPoint(SetPicture[TargetIndex].RoleSkillPoint);
                    }



                    PictureCreater.instance.AttackCount--;
                    if (Damige > 0)
                    {
                        if (!IsFirstBlood && !SetEnemyPicture[StartIndex].RolePictureMonster && PictureCreater.instance.AttackCount == 0 && SetEnemyPicture[StartIndex].RoleContinueKill > 0)
                        {
                            ShowContinueKill(StartIndex);
                        }
                    }
                    if (PictureCreater.instance.AttackCount == 0 && IsFinish)
                    {
                        if (!IsSkip)
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                        //if (MySkill.activeSelf)
                        //{
                        //    MySkill.SetActive(false);
                        //    MySkill.transform.Find("SkillCamera").position = Vector3.zero;
                        //    MySkill.transform.Find("SkillCamera").rotation = Quaternion.identity;
                        //    MySkill.transform.position = Vector3.zero;
                        //    MySkill.transform.rotation = Quaternion.identity;
                        //    MyCamera.SetActive(true);
                        //    yield return new WaitForSeconds(1);
                        //}
                        AddSequence();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("AAAA");
        }
    }

    public void ResetRandom()
    {
        IsFireSkill = false;
        IsRemember = true;
        IsSkip = false;
        RememberCount = 0;
        ListRandom.Clear();
    }

    public float GetRandom(int RoleID)
    {
        float RandomNum = UnityEngine.Random.Range(0f, 100f);

        if (IsRemember)
        {
            //Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa" + RoleID + " " + RandomNum + " " + ListRandom.Count);
            ListRandom.Add(RandomNum);
        }
        else
        {
            if (RememberCount < ListRandom.Count)
            {
                RandomNum = ListRandom[RememberCount];
                //Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa" + RoleID + " " + RandomNum + " " + RememberCount);
                RememberCount++;
            }
        }
        return RandomNum;
    }

    public int GetRandomInt(int Min, int Max, int RoleID)
    {
        int RandomNum = UnityEngine.Random.Range(Min, Max);

        if (IsRemember)
        {
            ListRandom.Add(RandomNum);
            //Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa" + RoleID + " " + RandomNum + " " + ListRandom.Count);
        }
        else
        {
            if (RememberCount < ListRandom.Count)
            {
                RandomNum = (int)(ListRandom[RememberCount]);
                //Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa" + RoleID + " " + RandomNum + " " + RememberCount);
                RememberCount++;
            }
        }
        return RandomNum;
    }

    public void SkillAddRoleBuff(bool IsEnemy, int RoleIndex, int BuffID)
    {
        if (IsEnemy)
        {
            Buff NewBuff = TextTranslator.instance.GetBuffByID(BuffID);
            RoleAddBuff(ListEnemyPicture, RoleIndex, NewBuff);
        }
        else
        {
            Buff NewBuff = TextTranslator.instance.GetBuffByID(BuffID);
            RoleAddBuff(ListRolePicture, RoleIndex, NewBuff);
        }
    }

    IEnumerator DelayChangeBlood(List<RolePicture> SetPicture, int TargetIndex, float ChangeBlood, float OffsetBlood)
    {
        for (int i = 0; i < 10; i++)
        {
            ChangeBlood += OffsetBlood;
            /////////////////血量条改变数值(以下)/////////////////
            SetPicture[TargetIndex].RoleObject.GetComponent<ColliderDisplayText>().mSlider.value = ChangeBlood / SetPicture[TargetIndex].RoleMaxBlood;
            /////////////////血量条改变数值(以上)/////////////////
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}


