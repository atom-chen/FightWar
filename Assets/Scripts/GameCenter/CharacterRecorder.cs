using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

public class CharacterRecorder : MonoBehaviour
{
    public static CharacterRecorder instance;
    public int AccountID = 0;
    public static int CharacterID = 1;
    public static string CTime = "";
    public string GroupName = "";
    public string Title = "";
    public int GameVersion = 1688;
    public int Career = 0;
    public int Sex = 0;
    public int Vitality = 0;
    public int Fight = 0;
    public int Charm = 0;
    public int MaxCharm = 500;
    public int RollPoint = 0;
    public int FightRank = 0;
    public int FightWin = 0;
    public int SenceID = 0;
    public int GateID = 0;
    public int BagCount = 0;
    public int Vip = 0;
    public int Diamond = 0; //现有钻石
    public int PayDiamond = 0; //总充值钻石
    public int VipExp = 0; //vip经验
    public int TodayPayDiamond = 0; //今日充值钻石
    public int ConsumeDiamond = 0; //总消费钻石
    public int TodayConsumeDiamond = 0; //今日消费钻石

    public string[] BuyGiftBag = { "-1", "-1" };//vip购买礼包记录

    public int RankNumber = 0;//排行名次
    public int HonerValue = 0;//荣誉值
    public int headIcon = 0;//头像
    public int PVPRankNumber = 0;//敌方的排名
    public int PVPRoleID = 0;//敌方的ID;

    public int ArmyGroup = 0;//军团币
    public int TrialCurrency = 0;//试炼币
    public int GoldBar = 0;//金条

    public string characterName = "";   //用户名
    public int userId = 0;
    public int level = 1;   //等级
    public int gold = 0;   //金钱
    public int lunaGem = 0; //月石
    public int stamina = 0; //体力
    public int staminaOld = 0; //上一个体力数据
    public int staminaCap = 0; //体力最大值 
    public int sprite = 0;      //精力
    public int spriteCap = 0;   //精力最大值
    public int ghost = 0; //资源

    public int DowerNum=0;//拥有的
    public int techPoint = 0; //情报点    
    public int techNeedPoint = 0; //升级所需情报点  
    public bool ChangeAttribute = false;//是否有属性变化
    public string worldEventList = ""; //世界事件

    public int exp = 0; //经验
    public int expMax = 0; //经验

    public int blood = 0; //血量
    public int bloodMax = 0; //血量
    public bool TaskAddExp = false;//成就经验加成

    public string SelfSign;
    //地图相关
    public int CurCreamGateCount = 1;//当前精英关可以打的次数
    public int lastGateID = 0;//最新的副本ID号
    public int lastGiftGateID = 0;//收复的副本ID号
    public int lastCreamGateID = 0;//最新的精英关ID号
    public int standGateID = -1;//最新的副本ID号
    public int gotoGateID = -1;//最新的副本ID号
    public int cameraGateID = 0;//摄像机锁定到地图点
    public bool IsOpenMapGate = false;
    public int backGateID = -1;
    public int Direction_Back = -1;
    public bool IsJumpMove = false;//为true时，下一次大地图移动直接闪现
    //战斗英雄信息
    public bool IsShowHeroInfo = false;
    //图鉴相关
    public int HeroMapIndex = 0;
    /// <summary>
    /// 记录当前重生的角色
    /// </summary>
    public int RebirthRoleId = 0;

    public int BagIndex = 1;//背包index
    public int mapID = 1; //最近开的地图
    public int RoleTabIndex = 1;
    public int EverydayTab = 1;
    public int FriendListTab = 0;//好友列表的显示选项（0~3）
    /// <summary>
    /// -1代表没有任何现象；0代表冲突；
    /// </summary>
    public int RandomMysteryNumber = -1;

    public bool IsOpenWeapon = false;
    /// <summary>
    /// 
    /// </summary>
    public int ChallengeToZhengFu = -1;
    /// <summary>
    /// -1表示没有任何操作；0表示从图鉴合成进入角色判断
    /// </summary>
    public int CompositeSuipian = -1;
    /// <summary>
    /// 记录背包物品的获取，进入哪一个UI
    /// </summary>
    public Dictionary<string, int> bagItemOpenWindows = new Dictionary<string, int>();

    //角色装备相关
    public bool IsNeedResetHedIndexAfterSortHero = false;//从角色进入装备 需要ResetHedIndex
    public bool isNeedRecordStrengTabType = true;//默认记录装备当前Tab
    public int setEquipTableIndex = 0;
    public bool enterRoleFromMain = false;
    public bool enterMainWindowFirst = false;
    public bool enterMapFromMain = false;
    public bool isOneKeyState = false;//一键强化 包含升品
    public int isReStartTimes = 0;
    public List<int> ListManualSkillId = new List<int>();//手动技能idList，0代表没有手动技能
    public List<int> ListRoleFateId = new List<int>();//英雄开启缘分Id，0代表没有开启该缘分
    /// <summary>
    /// 是否为第一次打开游戏
    /// </summary>
    public bool isFirstMainWindow = true;

    public int Landingdays = 0;//登陆天数，1003，开始时判定图片
    //国家相关
    public int legionCountryID = 0;//所属国家id，0 未加入任何国家。1 联盟 2 部落
    public int NationID = 0;//0是没有军衔
    public int TotalMedals = 0;//累计军功
    public string MilitaryExploitInfo;//军功宝箱领取状态
    public int EnemyMilitaryRankID = 0;//被挑战玩家的军衔Id;


    //军团战相关
    public string LegionPositonStr = "";//布阵信息
    public int LegionHarasPoint = 0;//骚扰点
    public int ReinforcementNum = 0;//援军数量
    public string LegionFestPosition = "0";//骚扰战对手信息
    public int LegionLeftOrRight = 0;//攻方，守方;  0守方，1攻方;
    public int LegionRankListButtonType = 1;//英雄榜按钮位置
    public string LegionMarktarStr = "";//集火公告信息
    public List<LegionwarGetnode> LegionwarGetnodeList = new List<LegionwarGetnode>();//8602 各城市点具体信息
    public bool AutomaticbrushCity = false;//自动刷城按钮是否开启
    //public bool IsClickButtonbrushCity = false;//移动是自动移动，还是点击按钮移动
    public int AutomaticbrushCityID = 0;//自动移动选定的城市id;

    //战队相关
    public MarinesInfomation MarinesInfomation1 = null;      //战队1信息;
    public MarinesInfomation MarinesInfomation2 = null;     //战队2信息;
    public MarinesInfomation MarinesInfomation3 = null;     //战队2信息;
    public int MarinesTabe = 0;                             //选择编队按钮
    public int LegionActionPoint = 0;                       //行动点数
    public List<Harassformation> HarassformationList = new List<Harassformation>();  //当前骚扰编队信息

    //军团相关
    public int legionID = 0;// 0 - 没有加入军团
    public int legionFlag = 1;// 0 - 没有加入军团
    public int legionFightLeftTimes = 0;
    public int needChairmanDealCount = 0;//需要审核的数量
    public int LegionHertRankTab = 0;//军团副本伤害排行tab
    public bool IsfirstEntLegion = false;//是否第一次进入军团战
    public bool IsNoFightEntLegion = false;//是否未开战进入军团战，用来布阵查看场景等
    public string LegionAnouncement = "";//军团公告;


    public string legionName = "";// 
    public LegionItemData myLegionData;// 我的军团数据
    public bool isLegionChairman = false;//是否是军团长
    public int myLegionPosition = 0;//我的军团职位
    public bool isOnlyApplayToJoinIn = true;//是否申请即可加入军团
    //实验室相关
    public bool IsOnputOrRemove = false;//上下实验室
    public bool IsNeedAddEmpetyCount = true;//刚登入时 需要记录空位个数，其他 不需要
    public int empetyCountToOnputHero = 0;//实验室空位 个数

    //聊天记录条数
    public int ButtonChannel = 2;//当前聊天的按钮位置
    public int Tab_Channel1 = 0;
    public int Tab_Channel2 = 0;
    public int Tab_Channel3 = 0;
    public int Tab_Channel4 = 0;
    public bool HaveNewPrivateChatInfo = false;//是否有新的私聊信息
    public bool HaveNuclear=false;//核电战是否有礼包
    //挂机剩余时间
    public int HoldOnLeftTime = 0;
    public int HoldKillNum = 0;//挂机击杀人数

    //军火库倒计时
    public int HoldJunhuoTime = 0;

    //需要扫荡的装备材料
    public int SweptIconID = 0;
    public int SweptIconNum = 0;//扫荡出来材料的数量

    /// <summary>
    /// 主界面奖杯显示
    /// </summary>
    public string FirstPowerName = "0";//战力第一名
    public string FirstPvpName = "0";//pvp第一名
    public string FirstWoodsName = "0";//战力第一名
    public string FirstLegionName = "0";//战力第一名

    /// <summary>
    /// 王者之路
    /// </summary>
    public string KingServer = "0";//挑战的服务器
    public int KingEnemyID = 0;//挑战战队的ID;
    public int KingRank = 0;//战队段位
    public int KingEnemyNum = 0;//战队名次
    public int KingMyNum = 0;//我的名次
    public int KingBuyCount = 0;//副本购买次数
    public int KingChallengeNum = 0;//副本挑战次数
    //public int KingCoin = 0;//王者币;


    /// <summary>
    /// 世界boss相关
    /// </summary>
    public float BossBlood = 0;//boss血量
    public int ClearBossNum = 0;//复活次数
    public int BossLevel = 0;//boss等级
    public int MyLocationInWorldBoss = 0;//我在服务器位置
    public bool IsTimeToOpen = false;
    /// <summary>
    /// 组队副本相关
    /// </summary>
    public int CopyNumber;//副本编号
    public int TeamID;//队伍编号
    public int TeamPosition;//队伍位置
    public List<int> CopyHeroIconList = new List<int>();//组队副本上阵头像保存
    public List<string> CopyHeroNameList = new List<string>();//组队副本上阵头像保存
    public bool IsCaptain = false;//是否队长
    public string TeamAwardList;//副本关卡总奖励
    public int TeamFightNum;//可副本次数
    public int TeamHelpTime;//组队帮打时间

    /// <summary>
    /// 军团红包相关信息
    /// </summary>
    public bool isLegionRedPoint = false;       //军团红包红点
    public bool isRichRedneckPoint = false;     //土豪红包红点
    public bool isRechargeRedPoint = false;     //充值红包红点
    public bool isGrabRedPoint = false;         //抢红包红点
    public bool isOpenRechargeRed = false;      //土豪红包是否开启

    /// <summary>
    /// 丛林冒险
    /// </summary>
    public bool Automaticbrushfield = false;//自动刷野是否开
    public List<int> AutomaticBuffList = new List<int>();//自动勾选的buff列表
    public int AutomaticOpenBoxNum = 0;//自动开箱的数量
    public int OpenTreasureNum = 0;//宝箱连续开启数量
    public int CanMoveLayer = 0;//可以跳过的楼层
    /// <summary>
    /// 竞技场相关信息
    /// </summary>

    public int PvpChallengeNum = 0;//挑战次数
    public int PvpPoint = 0;//Pvp积分
    public int GetPointLayer = 0;//领取的积分层级
    public int GetRankLayer = 0;//一次排行领取层级
    public int HaveRankLayer = 0;//一次排行可领取的层级
    public int MaxRankNumber = 0;//一次最大排名
    public int PvpRefreshTime = 0;//竞技场刷新时间;

    public int GachaOnce = 0;
    public int GachaMore = 0;
    public int GachaMoreTime = 0;
    public int GachaLotteryNum = 0;//钻石单抽次数
    public string HappyBoxInfo;//活跃度信息

    public bool MailButtonOnClick = false;
    public int MailCount = 0;//未读邮件数量
    public int applayFriendListCount = 0;//申请加好友个数
    public List<FriendItemData> MyFriendList = new List<FriendItemData>();//我的好友数据
    public List<int> MyFriendUIDList = new List<int>();
    public int loginSignCount = 0;//七日登录可领取数量
    public bool IsOpenloginSign = true;//七日活动是否开启
    public bool signRedPointState;
    public int RankShopType = 10001;
    public int ActivityTime;//主界面活动时间

    public int PVPComeNum = 0;//进入竞技场界面的次数
    public List<PVPItemData> PVPItemList = new List<PVPItemData>();
    public bool IsOpenCreamCN;//是否打开对应精英关
    public bool IsOpenEventList;//是否显示世界事件列表
    public int InitSelectGate;//选择某个地图点上的某一普通关，从1开始
    public int InitSelectCreamGate;//选择某个地图点上的某一精英关，从1开始
    public string EnemyInfoStr;//敌军入侵信息
    public int WorldEventRefreshCost = 0;//世界事件下次刷新需要钻石数
    public int WorldEventFightCount = 0;//世界事件剩余战斗次数


    public int OpenServiceFinalAward;//开服最终奖励状态 0不可领，1，可领，2领过

    public int BagItemCode;//背包物品使用的物品id;5002用

    public int BuyGold = 0;//点金前金币
    public int FightOld = 0;//战力变更前的战力
    public bool IsOpenFight = true;//战力变更界面开关
    public bool IsNeedOpenFight = true;//是否需要战力变更界面

    public bool IsOpen = true;//主界面任务红点开关
    public bool IsOpeGacha = true;//抽卡红点开关
    public int TenCaChaNumber = 0;
    public bool ActivityRewardIsGet = false;//十连抽活动是否获得
    /// <summary>
    /// 当前剧情进度
    /// </summary>
    public int storyID;

    public List<Role> ListRole = new List<Role>();
    public List<Hero> ListHero = new List<Hero>();
    public List<Hero> ListPVPHero = new List<Hero>();

    public BetterList<Hero> ownedHeroList = new BetterList<Hero>();   //拥有的英雄列表
    public List<int> heroIdList = new List<int>(); //招募前拥有的英雄列表Id
    /// <summary>
    /// 刷新已拥有的角色前
    /// </summary>
    public List<Hero> upDateOwnenHeroList = new List<Hero>();

    public string PositionString = "";

    public int EveryDiffID = 0;//日常副本难度
    public int NuclearLevel = 0;//核电战进度
    public int HistoryFloor;//敢死队历史最高层
    public int NowFloor;//敢死队历史最高层
    public int CanGetRewardID = 0;
    public int HadRewardID = 0;
    public bool isSkip = false;//敢死队是否跳过;
    public bool isWoods = false;
    public bool isOpenAdvance = false;//宝箱是否打开
    public int WoodsRankID = 0;
    /// <summary>
    /// 新手引导
    /// </summary>

    public List<int> GuideID = new List<int>();
    public int NowGuideID = 0;
    /// <summary>
    /// 夺宝是否失败
    /// </summary>

    public int OnceSuceessID = 0;
    public bool isFailed = false;
    public int FailedID = 0;
    public int ClickItemID = 0;
    public TextTranslator.ItemInfo ItemCentreDetails;
    public bool isToQualify = false;
    public bool isFiveButtonOnce = false;
    /// <summary>
    /// 夺宝积分
    /// </summary>

    public int GrabIntegrationPoint = 0;
    public int GrabIntegrationGetPointLayer = 0;
    public int GrabIntegrationHavePointLayer = 0;
    public bool GrabIntegrationOpen = false;
    public bool GrabIntegrationRedPoint = false;
    /// <summary>
    /// 日常副本红点条件
    /// </summary>

    public List<int> EveryDayNumberRedPoint = new List<int>();
    public List<int> EveryDayTimerRedPoint = new List<int>();
    /// <summary>
    /// 走私次数
    /// </summary>

    public int SmuggleNum = 0;
    public int SmuggleTime = 0;
    /// <summary>
    /// 科技
    /// </summary>

    public bool TechRedPoint = false;
    /// <summary>
    /// 征服
    /// </summary>

    public int TabeID = 0;
    public int PlayerUid = 0;
    public int KengID = 0;
    public bool isConquerRedPoint = false;

    public string HostageName = "";
    public int HostageRoleID = 0;
    //领取体力红点
    public bool isPowerRedPoint = false;
    //全民基金
    public bool isFoundationRedPoint = false;
    public bool isFoundationPoint = false;
    public bool isBenifPoint = false;
    public bool isBenifRedPoint = false;
    public bool isBuyTheFoundation = false;
    //全域红点list
    public List<int> RedPointList = new List<int>();
    //秘宝新手引导
    public bool isHadFriteStone = false;
    //红将第一次打开
    public bool isRedHeroWindowFirst = false;
    public bool isGaChaFromMainWindow = false;
    //神器碎片
    public int HeroWeaponID = 0;
    public bool isWeaponGachaFree = false;
    public int heroPresentWeapon = 85001;
    //是否为任务前往
    public bool isTaskGoto = false;
    //英雄显示是否是点击打开
    public bool isGaChaFromItemClick = false;
    //聊天开启后第一次打开
    public bool IsXitongTalk = false;
    public bool IsWorldTalk = false;
    public bool IsJunTuanTalk = false;
    public bool IsTeamTalk = false;
    //兑换活动list
    public static List<int> ItemExchangButtonList = new List<int>();
    public static List<int> ItemFixedOpenList = new List<int>();
    public int OpenWindowButtonID = 0;
    public int AboutHeroInfoId = -1;

    /// <summary>
    /// 金蛋活动是否开启
    /// </summary>
    public bool IsGoldOpen = false;
    public bool IsGoldEggRedPoint = false;
    /// <summary>
    /// 记录是否电量低的时候提示
    /// </summary>
    public bool IsOpenPowerMode = true;
    /// <summary>
    /// 许愿活动是否开启
    /// </summary>
    public bool IsWishOpen = false;
    public bool IsWishRedpoint = false;

    public int MonthCardDay = -1;//月卡刷新天数
    //是否播放芯片特效
    public bool SBlaoxu = false;


    //去掉万位后的数
    public string ChangeNum(int num)
    {
        string text;
        return text = num <= 1000000 ? num.ToString() : (num / 10000).ToString() + "W";
    }

    public IEnumerator SyncHeroListFormServer()
    {        
        NetworkHandler.instance.SendProcess("3001#0;");
        yield return new WaitForSeconds(0.1f);
        SortHeroListByForce();
        if (!NetworkHandler.instance.IsCreate)
        {
            enterMainWindowFirst = true;
            UIManager.instance.OpenPanel("MainWindow", false);
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) >= 18)
            {
                UIManager.instance.OpenSinglePanel("AnnouncementWindow", false);
                GameObject.Find("AnnouncementWindow").GetComponent<AnnouncementWindow>().SetTextureOnLoadingWindow("Announcement1");
                //NetworkHandler.instance.SendProcess("1901#;"); //kino
            }
        }
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("5001#");
        yield return new WaitForSeconds(0.1f);
        if (lastGateID - 1 > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.conglingmaoxian).Level)
        {
            NetworkHandler.instance.SendProcess("1501#;"); //敢死队	取得层数
            yield return new WaitForSeconds(0.1f);
        }
        if (GameObject.Find("LoadingWindow") != null)
        {
            GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().IsClose = true;
        }
        NetworkHandler.instance.SendProcess("1011#"); //取得编队
        yield return new WaitForSeconds(0.1f);
        if (level >= 19)
        {
            NetworkHandler.instance.SendProcess("3101#0;"); //取得秘宝
            yield return new WaitForSeconds(0.1f);
        }
        if (lastGateID - 1 > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.bianjingzousi).Level)
        {
            NetworkHandler.instance.SendProcess("6301#;"); //取得走私
            yield return new WaitForSeconds(0.1f);
        }
        NetworkHandler.instance.SendProcess("1031#"); //取得战术
        yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("2016#0;"); //取得宝箱状态 kino
        //yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("2201#;"); //取得世界事件列表 kino
        //yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1204#;"); //取得活跃度
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1201#1;"); //任务
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1201#2;"); //成就
        yield return new WaitForSeconds(0.1f);
        if (lastGateID - 1 > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zuojia).Level)
        {
            NetworkHandler.instance.SendProcess("3201#0;"); //取得豪车
            yield return new WaitForSeconds(0.1f);
        }
        //NetworkHandler.instance.SendProcess("6001#;"); //取得pvp状态
        NetworkHandler.instance.SendProcess("6006#1;"); //取得pvp编队
        NetworkHandler.instance.SendProcess("6006#2;"); //丛林
        NetworkHandler.instance.SendProcess("6006#3;"); //军团搔扰
        NetworkHandler.instance.SendProcess("6006#4;"); //军团副本
        NetworkHandler.instance.SendProcess("6006#6;"); //取得pvp编队
        //yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("6009#;"); //取得积分奖励
        //yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("6011#;"); //取得排名奖励
        //yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("5204#;"); //抽奖状态          
        //yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9135#");  //取得十连抽 kino
        yield return new WaitForSeconds(0.1f);
        if (lastGateID - 1 > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.qingbao).Level)
        {
            NetworkHandler.instance.SendProcess("1601#;"); //科技
            yield return new WaitForSeconds(0.1f);
        }
        //NetworkHandler.instance.SendProcess("9001#;"); //取得邮件列表
        //yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("7103#;"); //取得待申请通过好友列表 kino
        //yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("7101#;"); //取得好友列表
        //yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9131#;"); //取得七日登入签到
        yield return new WaitForSeconds(0.1f);
        for (int i = 1; i < 8; i++)
        {
            ReformLabItemData _targetlabitemdata = TextTranslator.instance.GetOneLabsItemByRoleTypeAndPosNum(i, 1);
            if (CharacterRecorder.instance.level >= _targetlabitemdata.NeedLevel)
            {
                NetworkHandler.instance.SendProcess(string.Format("1801#{0};", i));//实验室数据
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9201#;"); //VIP
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("6201#;"); //取得世界boss状态
        yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("8018#;"); //取得训练场
        //yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("2001#;"); //初始化MapStarList
        yield return new WaitForSeconds(0.1f);
        //NetworkHandler.instance.SendProcess("9151#1;"); //问卷调查 kino
        //yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9608#"); //基金
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9610#"); //基金福利
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9612#"); //登陆送VIP
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1091#"); //领取体力
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("8626#"); //集火
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9701#"); //砸金蛋
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9721#"); //取得心愿
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9601#");//活动开启列表
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9711#"); //红将排行
        NetworkHandler.instance.SendProcess("9712#"); //红将箱子
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("3305#0;"); //取得转盘
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1921#"); //小目标
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("5303#"); //军火库
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("6601#;"); //核电战
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1621#0;"); //核电战
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("8701#;");//军团红包红点刷新
        NetworkHandler.instance.SendProcess("8703#;");
        NetworkHandler.instance.SendProcess("8705#;");
        NetworkHandler.instance.SendProcess("8707#;");
        NetworkHandler.instance.SendProcess("8708#;");

        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("8022#;");//军团公告        
        //军团长 获取军团申请列表
        if (CharacterRecorder.instance.isLegionChairman)
        {
            NetworkHandler.instance.SendProcess("8012#;");
        }
        NetworkHandler.instance.SendProcess("9411#;"); //红点
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9101#;"); //取得签到列表
        yield return new WaitForSeconds(0.1f);
        ActivityRedPointSendPress();
        yield return new WaitForSeconds(0.1f);
        SortHeroListByForce();
        if (SystemInfo.systemMemorySize < 1000)
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
    /// <summary>
    /// 活动红点
    /// </summary>
    public void ActivityRedPointSendPress()
    {
        NetworkHandler.instance.SendProcess("9614#130000"); //兑换活动
        if (ItemFixedOpenList.Count == 0)
        {
            NetworkHandler.instance.SendProcess("9602#120001");
            NetworkHandler.instance.SendProcess("9604#140001");
        }
        for (int i = 0; i < ItemFixedOpenList.Count; i++)
        {
            if (ItemFixedOpenList[i] >= 120000 && ItemFixedOpenList[i] < 130000)//每日充值
            {
                NetworkHandler.instance.SendProcess("9602#" + ItemFixedOpenList[i]);
            }
            else if (ItemFixedOpenList[i] >= 140000 && ItemFixedOpenList[i] < 150000)//单笔充值
            {
                NetworkHandler.instance.SendProcess("9604#" + ItemFixedOpenList[i]);
            }
        }
    }
    public BetterList<bool> AllRedPoint = new BetterList<bool>();
    /// <summary>
    /// 0是任务,1是大放送,2是丛林冒险,3是走私,4是战术,5是竞技场,6是招募,7是签到,8是邮件,9是好友,10是七日登入,11是vip,12训练场,13实验室,14征服,15图鉴,16情报,17英雄,18装备,19活动,20军团
    /// 21是冲突,22是日常副本,23是夺宝,24是组队,25是王者,26军团副本,27军团成员,28军团训练,29军团捐献,30军团任务,31军团红包,32军团酒吧 33登陆送VIP 34十连抽 35全名基金  37全名福利  36红将
    /// 38基金V2  39-45兑换活动红点  46兑换活动主界面红点  48敢死队 49走私  50单笔充值  51每日充值 52军团副本刮刮乐  53 chip 54商城 55核电战
    /// </summary>
    public void SetRedPoint(int type, bool isOpenRedPoint)
    {
        if (AllRedPoint.size > type)
        {
            AllRedPoint[type] = isOpenRedPoint;
        }
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().UpdateRedPoint(type);
        }
    }

    /// <summary>
    /// 0是任务,1是大放送,2是丛林冒险,3是走私,4是战术,5是竞技场,6是招募,7是签到,8是邮件,9是好友,10是七日登入,11是vip,12训练场,13实验室,14征服,15图鉴,16情报,17英雄,18装备,19活动,20军团
    /// 21是冲突,22是日常副本,23是夺宝,24是组队,25是王者,26军团副本,27军团成员,28军团训练,29军团捐献,30军团任务,31军团红包,32军团酒吧
    /// </summary>
    public bool GetRedPoint(int type)
    {
        if (AllRedPoint.size > type)
        {
            return AllRedPoint[type];
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 0是任务,1是大放送,2是丛林冒险,3是走私,4是战术,5是竞技场,6是招募,7是签到,8是邮件,9是好友,10是七日登入,11是vip,12训练场,13实验室,14征服,15图鉴,16情报,17英雄,18装备,19活动,20军团
    /// 21是冲突,22是日常副本,23是夺宝,24是组队,25是王者,26军团副本,27军团成员,28军团训练,29军团捐献,30军团任务,31军团红包,32军团酒吧
    /// </summary>
    public void CheckRedPoint()
    {
        //0是任务
        if (CharacterRecorder.instance.IsOpen)
        {
            StartCoroutine(IETaskRedPoint());
        }
        else if (CharacterRecorder.instance.IsOpen == false)
        {
            int num = TextTranslator.instance.TaskCompleteNum() + TextTranslator.instance.AchievementCompleteNum();//TextTranslator.instance.AchievementComplete();
            if (CharacterRecorder.instance.HappyBoxInfo != null)
            {
                string Receive = CharacterRecorder.instance.HappyBoxInfo;
                string[] dataSplit = Receive.Split(';');
                string[] RewardSplit = dataSplit[1].Split('$');
                if (int.Parse(RewardSplit[0]) == 1 || int.Parse(RewardSplit[1]) == 1 || int.Parse(RewardSplit[2]) == 1 || int.Parse(RewardSplit[3]) == 1)
                {
                    num = 1;
                }
            }
            if (num > 0)
            {
                SetRedPoint(0, true);
            }
            else
            {
                SetRedPoint(0, false);
            }
        }
        MainTaskReadPoint();
        //1是大放送
        SetSevenDayRedPoint();
        //2是丛林冒险
        //3是走私
        //4是战术
        //5是竞技场
        //6是招募
        GaChaRedPont();
        //7是签到
        SetRedPoint(7, CharacterRecorder.instance.signRedPointState);//签到小红点

        //8是邮件
        MailRedPont();
        //9是好友
        SetFriendRedPoint();
        //10是七日登入
        SetLoginSignRedPoint();
        //11是vip
        FirstVipRedpoint();
        //12训练场
        //13实验室
        SetLabRedPoint();
        //15图鉴
        HeroMapRedPoint();
        //16情报
        if (CharacterRecorder.instance.techPoint >= 6 && CharacterRecorder.instance.lastGateID > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.qingbao).Level)
        {
            SetRedPoint(16, true);
        }
        else
        {
            SetRedPoint(16, false);
        }

        //17英雄
        SetRedPoint(17, GetRoleButtonRedPointState());//角色小红点 
        //18装备
        SetRedPoint(18, GetEquipButtonRedPointState());//装备小红点 
        //19活动
        //20军团
        if (!GetRedPoint(20))
        {
            bool _boolState0 = CharacterRecorder.instance.needChairmanDealCount > 0 ? true : false;
            bool _boolState1 = CharacterRecorder.instance.legionFightLeftTimes > 0 ? true : false;
            bool redPointOnMain = false;
            if (CharacterRecorder.instance.legionID == 0)
            {
                redPointOnMain = false;
            }
            else if (_boolState0 || _boolState1)
            {
                redPointOnMain = true;
            }
            SetRedPoint(20, redPointOnMain);//军团小红点
        }

        //21是冲突
        Collision();
        //十连抽后弹出四选一活动界面
        StartCoroutine(DelayShowTenGacha());
        //22是日常副本
        //23是夺宝
        //24是组队
        //25是王者
        //26军团副本
        //27军团成员
        //28军团训练
        //29军团捐献
        //30军团任务
        //31军团红包
        //32军团酒吧
        //53chip
        if (TextTranslator.instance.GetItemCountByID(90014) > 0)
        {
            SetRedPoint(53, true);
        }
        else
        {
            SetRedPoint(53, false);
        }
    }
    IEnumerator DelayShowTenGacha()
    {
        if ((CharacterRecorder.instance.TenCaChaNumber == 1 && CharacterRecorder.instance.GuideID[55] == 0))//CharacterRecorder.instance.ActivityRewardIsGet == false
        {
            SetRedPoint(34, true);
            CharacterRecorder.instance.GuideID[55] = 99;
            SceneTransformer.instance.SendGuide();
            yield return new WaitForSeconds(0.5f);
            ResourceLoader.instance.OpenActivity(1);
            //UIManager.instance.OpenSinglePanel("AnnouncementWindow", false);
            //GameObject.Find("AnnouncementWindow").GetComponent<AnnouncementWindow>().SetTexture("TenGacha");
        }
        else if (CharacterRecorder.instance.isPowerRedPoint || CharacterRecorder.instance.isFoundationPoint || CharacterRecorder.instance.isBenifPoint || (CharacterRecorder.instance.TenCaChaNumber == 1 && CharacterRecorder.instance.ActivityRewardIsGet == false))
        {
            SetRedPoint(34, true);
        }
        else
        {
            SetRedPoint(34, false);
        }
    }
    /// <summary>
    /// 0是任务,1是大放送,2是丛林冒险,3是走私,4是战术,5是竞技场,6是招募,7是签到,8是邮件,9是好友,10是七日登入,11是vip,12训练场,13实验室,14征服,15图鉴,16情报,17英雄,18装备,19活动,20军团
    /// 21是冲突,22是日常副本,23是夺宝,24是组队,25是王者,26军团副本,27军团成员,28军团训练,29军团捐献,30军团任务,31军团红包,32军团酒吧
    /// </summary>
    /// 
    public void GetTaskRedPoint()
    {
        int num = TextTranslator.instance.TaskCompleteNum() + TextTranslator.instance.AchievementCompleteNum();
        if (CharacterRecorder.instance.HappyBoxInfo != null)
        {
            string Receive = CharacterRecorder.instance.HappyBoxInfo;
            string[] dataSplit = Receive.Split(';');
            string[] RewardSplit = dataSplit[1].Split('$');
            if (int.Parse(RewardSplit[0]) == 1 || int.Parse(RewardSplit[1]) == 1 || int.Parse(RewardSplit[2]) == 1 || int.Parse(RewardSplit[3]) == 1)
            {
                num = 1;
            }
        }
        if (num > 0)
        {
            SetRedPoint(0, true);
        }
        else
        {
            SetRedPoint(0, false);
        }
        CharacterRecorder.instance.IsOpen = false;
    }
    private bool GetRoleButtonRedPointState()
    {
        //for (int i = 0; i < CharacterRecorder.instance.ownedHeroList.size; i++)
        int usefulCount = CharacterRecorder.instance.ownedHeroList.size >= 6 ? 6 : CharacterRecorder.instance.ownedHeroList.size;
        for (int i = 0; i < usefulCount; i++) //角色小红点 排行前六名的角色
        {
            Hero mHero = CharacterRecorder.instance.ownedHeroList[i];
            if (GetHeroRedPointStateOfHeroTabs(mHero))// || GetHeroRedPointStateOfEquip(mHero))//去掉装备红点
            {
                return true;
            }
        }
        return false;
    }
    private bool GetEquipButtonRedPointState()
    {
        //for (int i = 0; i < CharacterRecorder.instance.ownedHeroList.size; i++)
        int usefulCount = CharacterRecorder.instance.ownedHeroList.size >= 6 ? 6 : CharacterRecorder.instance.ownedHeroList.size;
        for (int i = 0; i < usefulCount; i++) //装备小红点 排行前六名的角色
        {
            Hero mHero = CharacterRecorder.instance.ownedHeroList[i];
            if (GetHeroRedPointStateOfEquip(mHero))
            {
                return true;
            }
        }
        return false;
    }
    private void SetHeroItemRedPoint(Hero mHero)
    {
        GameObject _RedPoint = gameObject.transform.FindChild("RedPoint").gameObject;
        if (GetHeroRedPointStateOfHeroTabs(mHero) || GetHeroRedPointStateOfEquip(mHero))
        {
            _RedPoint.SetActive(true);
        }
        else
        {
            _RedPoint.SetActive(false);
        }
    }
    bool GetHeroRedPointStateOfHeroTabs(Hero _curHero)
    {
        //升品
        RoleClassUp rcu = TextTranslator.instance.GetRoleClassUpInfoByID(_curHero.cardID, _curHero.classNumber);
        //Debug.Log("cardId...." + _curHero.cardID + "...level..." + _curHero.level);
        //Debug.Log(rcu.Levelcap);
        //Debug.Log(GetShengPinOneContionState(rcu.NeedItemList));
        if (_curHero.level > rcu.Levelcap && GetShengPinOneContionState(rcu.NeedItemList) == true)
        {
            return true;
        }
        //军衔
        RoleBreach rb = TextTranslator.instance.GetRoleBreachByID(_curHero.cardID, _curHero.rank);
        int item1Count = TextTranslator.instance.GetItemCountByID(10102);
        //Debug.Log("cardID..." + _curHero.cardID + "...rank..." + _curHero.rank + ".." + rb);
        int item2Count = TextTranslator.instance.GetItemCountByID(rb.roleId + 10000);
        if (CharacterRecorder.instance.lastGateID > 10008 && _curHero.level >= rb.levelCup && item1Count >= rb.stoneNeed && item2Count >= rb.debrisNum)
        {
            return true;
        }
        return false;
    }
    bool GetShengPinOneContionState(List<Item> classUpList)
    {
        for (int i = 0; i < classUpList.Count; i++)
        {
            int bagItemCount1 = TextTranslator.instance.GetItemCountByID(classUpList[i].itemCode);
            if (bagItemCount1 < classUpList[i].itemCount)
            {
                return false;
            }
        }
        return true;
    }
    bool GetHeroRedPointStateOfEquip(Hero mHero)
    {
        foreach (var _OneEquipInfo in mHero.equipList)
        {
            if (_OneEquipInfo.equipPosition < 5)
            {
                //Debug.LogError(_OneEquipInfo.equipColorNum + "..." + IsAdvanceState(_OneEquipInfo.equipLevel, _OneEquipInfo.equipColorNum));
                int _EquipLevel = _OneEquipInfo.equipLevel;
                int _EquipColorNum = _OneEquipInfo.equipColorNum;
                if (IsAdvanceState(_EquipLevel, _EquipColorNum) && IsEnoughToAdvance(_OneEquipInfo, mHero) && IsAdvanceMaterailEnough(_OneEquipInfo, mHero))//升品
                {
                    return true;
                }
                else if (IsAdvanceState(_EquipLevel, _EquipColorNum) == false && IsEnoughToUpGrade(_OneEquipInfo, mHero) && _OneEquipInfo.equipLevel < CharacterRecorder.instance.level)//升级
                {
                    return true;
                }
                else if (WeaponMainWindow(mHero.cardID, mHero.WeaponList[0].WeaponID, mHero.WeaponList[0].WeaponClass, mHero.WeaponList[0].WeaponStar))
                {
                    return true;
                }
            }
        }
        return false;
    }
    bool IsEnoughToUpGrade(Hero.EquipInfo _OneEquipInfo, Hero mHero)
    {
        HeroInfo mHeroInfo = TextTranslator.instance.GetHeroInfoByHeroID(mHero.cardID);
        int needMoney = TextTranslator.instance.GetEquipStrongCostByID(_OneEquipInfo.equipLevel, mHeroInfo.heroRarity, _OneEquipInfo.equipPosition);
        if (CharacterRecorder.instance.gold >= needMoney)
        {
            return true;
        }
        return false;
    }
    bool IsAdvanceMaterailEnough(Hero.EquipInfo _OneEquipInfo, Hero mHero)
    {
        HeroInfo mHeroInfo = TextTranslator.instance.GetHeroInfoByHeroID(mHero.cardID);
        EquipStrongQuality esq = TextTranslator.instance.GetEquipStrongQualityByIDAndColor(_OneEquipInfo.equipCode, _OneEquipInfo.equipColorNum, mHeroInfo.heroRace);
        for (int i = 0; i < esq.materialItemList.size; i++)
        {
            int itemCode = esq.materialItemList[i].itemCode;
            int itemCountInBag = TextTranslator.instance.GetItemCountByID(itemCode);
            if (esq.materialItemList[i].itemCount > itemCountInBag)
            {
                return false;
            }
        }
        return true;
    }
    bool IsEnoughToAdvance(Hero.EquipInfo _OneEquipInfo, Hero mHero)
    {
        int _EquipPosition = _OneEquipInfo.equipPosition;
        HeroInfo mHeroInfo = TextTranslator.instance.GetHeroInfoByHeroID(mHero.cardID);
        EquipStrongQuality esq = TextTranslator.instance.GetEquipStrongQualityByIDAndColor(mHero.equipList[_EquipPosition - 1].equipCode, mHero.equipList[_EquipPosition - 1].equipColorNum, mHeroInfo.heroRace);
        if (CharacterRecorder.instance.gold >= esq.Money)
        {
            return true;
        }
        return false;
    }
    bool IsAdvanceState(int _EquipLevel, int _EquipColorNum)
    {
        if (_EquipColorNum * 5 == _EquipLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #region 核武器
    public bool WeaponMainWindow(int HeroID, int WeaponID, int WeaponClass, int WeaponStar)
    {
        Debug.LogError("" + HeroID + "       " + WeaponID + "       " + WeaponClass + "       " + WeaponStar);
        WeaponMaterial ItemMaterial;
        WeaponUpClass ItemUpClass;
        WeaponUpStar ItemUpStar;
        ItemMaterial = TextTranslator.instance.GetWeaponMaterialByID(WeaponID);
        if (WeaponStar == 0)
        {
            if (WeaponClass == 0)
            {
                ItemUpClass = TextTranslator.instance.GetWeaponUpClassByID(WeaponID, 2);
                ItemUpStar = TextTranslator.instance.GetWeaponUpStarByID(WeaponID, 1, 1);
                if (TextTranslator.instance.GetItemCountByID(ItemMaterial.WeaponID + 30000) >= ItemMaterial.NeedDebris)
                {
                    // Debug.LogError("aaaa1");
                    return true;
                }
            }
            else
            {
                ItemUpClass = TextTranslator.instance.GetWeaponUpClassByID(WeaponID, WeaponClass + 1);
                ItemUpStar = TextTranslator.instance.GetWeaponUpStarByID(WeaponID, WeaponClass, 1);
                if (TextTranslator.instance.GetItemCountByID(ItemUpStar.NeedItemID1) >= ItemUpStar.NeddItemNum1)
                {
                    // Debug.LogError("aaaa2");
                    return true;
                }
            }

        }
        else
        {
            if (WeaponClass != 5)
            {
                ItemUpClass = TextTranslator.instance.GetWeaponUpClassByID(WeaponID, WeaponClass + 1);

            }
            else
            {
                ItemUpClass = TextTranslator.instance.GetWeaponUpClassByID(WeaponID, 5);
            }
            ItemUpStar = TextTranslator.instance.GetWeaponUpStarByID(WeaponID, WeaponClass, WeaponStar == 5 ? 5 : WeaponStar + 1);
            if (WeaponStar == 5 && WeaponClass != 5)
            {
                if (CharacterRecorder.instance.gold > ItemUpClass.NeedGold && TextTranslator.instance.GetItemCountByID(10105) >= ItemUpClass.StoneNeedNum &&
                TextTranslator.instance.GetItemCountByID(HeroID + 10000) >= ItemUpClass.RoleDebrisNum)
                {
                    // Debug.LogError("aaaa3");
                    return true;
                }
            }
            else if (WeaponStar != 5 && WeaponClass != 5)
            {
                if (TextTranslator.instance.GetItemCountByID(ItemUpStar.NeedItemID1) >= ItemUpStar.NeddItemNum1)
                {
                    //Debug.LogError("aaaa4");
                    return true;
                }

            }
        }
        if (isWeaponGachaFree)
        {
            //Debug.LogError("aaaa5");
            return true;
        }
        return false;
    }
    #endregion

    ///<summary>
    /// 10是七日登入.
    ///<summary>       
    public void SetLoginSignRedPoint()
    {
        if (CharacterRecorder.instance.loginSignCount > 0)
        {
            SetRedPoint(10, true);//登录签到Seven小红点
        }
        else
        {
            SetRedPoint(10, false);
        }
    }

    /// <summary>
    /// 大放送红点刷新
    /// <summary>
    public void SetSevenDayRedPoint()
    {
        if (CharacterRecorder.instance.ActivityTime != 0)
        {
            if (CharacterRecorder.instance.IsOpen)
            {
                StartCoroutine(SevenDayRedpoint());
            }
            else
            {
                bool IsRed = false;
                if (CharacterRecorder.instance.OpenServiceFinalAward == 1)
                {
                    IsRed = true;
                }
                if (TextTranslator.instance.SevenDayCompleteNum() > 0 || IsRed)
                {
                    SetRedPoint(1, true);
                }
                else
                {
                    SetRedPoint(1, false);
                }
            }
        }
    }
    IEnumerator SevenDayRedpoint()
    {
        NetworkHandler.instance.SendProcess("9121#;");
        yield return new WaitForSeconds(0.3f);
        bool IsRed = false;
        if (CharacterRecorder.instance.OpenServiceFinalAward == 1)
        {
            IsRed = true;
        }
        if (TextTranslator.instance.SevenDayCompleteNum() > 0 || IsRed)
        {
            SetRedPoint(1, true);
        }
        else
        {
            SetRedPoint(1, false);
        }
    }
    IEnumerator IETaskRedPoint()
    {
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1201#1;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1201#2;");

    }

    #region 图鉴红点
    public void HeroMapRedPoint()
    {
        bool IsShowRedPoint = false;
        foreach (var hero in TextTranslator.instance.heroInfoList)
        {
            //HeroInfo mhero = TextTranslator.instance.GetHeroInfoByHeroID(hero.cardID);
            if (hero.heroID > 60000 && hero.heroID < 65000)
            {
                HeroInfo _hero = TextTranslator.instance.GetHeroInfoByHeroID(hero.heroID);
                if (TextTranslator.instance.GetItemCountByID(hero.heroID + 10000) >= _hero.heroPiece)
                {
                    IsShowRedPoint = true;
                    foreach (var mhero in CharacterRecorder.instance.ownedHeroList)
                    {
                        if (hero.heroID == mhero.cardID)
                        {
                            IsShowRedPoint = false;
                            break;
                        }
                    }
                    if (IsShowRedPoint)
                    {
                        break;
                    }
                }
            }
        }
        SetRedPoint(15, IsShowRedPoint);
    }
    #endregion
    ///好友红点   
    public void SetFriendRedPoint()
    {
        //if (CharacterRecorder.instance.applayFriendListCount > 0)
        //{
        //    SetRedPoint(9, true);//好友小红点
        //}
        //else
        //{
        //    SetRedPoint(9, false);//好友小红点
        //}
    }
    #region 招募红点判断
    public void GaChaRedPont()
    {
        //if (CharacterRecorder.instance.IsOpeGacha)
        //{
        //    StartCoroutine(Gachapoint());
        //}
        //else
        //{
        int num1 = CharacterRecorder.instance.GachaOnce;
        int num2 = CharacterRecorder.instance.GachaMore;
        int time = CharacterRecorder.instance.GachaMoreTime;
        if (num1 > 0 && time == 0 || num2 > 0)
        {
            SetRedPoint(6, true);
        }
        else
        {
            SetRedPoint(6, false);
        }
        //}
    }
    //IEnumerator Gachapoint()
    //{
    //    NetworkHandler.instance.SendProcess("5204#;");
    //    CharacterRecorder.instance.IsOpeGacha = false;
    //    yield return new WaitForSeconds(0.2f);
    //    int num1 = CharacterRecorder.instance.GachaOnce;
    //    int num2 = CharacterRecorder.instance.GachaMore;
    //    int time = CharacterRecorder.instance.GachaMoreTime;
    //    if (num1 > 0 && time == 0 || num2 > 0)
    //    {
    //        SetRedPoint(6, true);
    //    }
    //    else
    //    {
    //        SetRedPoint(6, false);
    //    }

    //}
    #endregion
    #region 邮件红点判断
    public void MailRedPont()
    {
        if (CharacterRecorder.instance.MailCount > 0)
        {
            SetRedPoint(8, true);
        }
        else
        {
            SetRedPoint(8, false);
        }
    }
    #endregion
    #region 冲突红点判断
    /// <summary>
    /// 0是任务,1是大放送,2是丛林冒险,3是走私,4是战术,5是竞技场,6是招募,7是签到,8是邮件,9是好友,10是七日登入,11是vip,12训练场,13实验室,14征服,15图鉴,16情报,17英雄,18装备,19活动,20军团
    /// 21是冲突,22是日常副本,23是夺宝,24是组队,25是王者,26军团副本,27军团成员,28军团训练,29军团捐献,30军团任务,31军团红包,32军团酒吧 33登陆送VIP 34十连抽 35全名基金  37全名福利  36红将
    /// 38基金V2  39-45兑换活动红点  46兑换活动主界面红点  48敢死队 49走私 
    /// </summary>
    public void Collision()
    {
        //int Pvp = 0;//竞技场红点
        if (CharacterRecorder.instance.lastGateID > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.jingjichang).Level)
        {
            if ((CharacterRecorder.instance.GetPointLayer < 10 && CharacterRecorder.instance.PvpPoint > CharacterRecorder.instance.GetPointLayer * 2
                || CharacterRecorder.instance.GetRankLayer < CharacterRecorder.instance.HaveRankLayer || CharacterRecorder.instance.PvpChallengeNum > 0 && CharacterRecorder.instance.PvpRefreshTime == 0))
            {
                SetRedPoint(5, true);
                //Pvp = 1;
            }
            else
            {
                SetRedPoint(5, false);
                //Pvp = 0;
            }
        }
        else
        {
            SetRedPoint(5, false);
            //Pvp = 0;
        }

        // int Challenge = 0;//夺宝红点
        List<Item> ItemList = new List<Item>();
        ItemList = TextTranslator.instance.GetAllGrabInBag();
        List<Item> FinishItemList = TextTranslator.instance.GetFinishItem(ItemList);
        if (FinishItemList.Count > 0 || CharacterRecorder.instance.GrabIntegrationRedPoint)
        {
            SetRedPoint(23, true);
            //Challenge = 1;
        }
        else
        {
            SetRedPoint(23, false);
            //Challenge = 0;
        }

        //int Expendable = 0;
        //敢死队
        if (CharacterRecorder.instance.CanGetRewardID > CharacterRecorder.instance.HadRewardID)
        {
            SetRedPoint(48, true);
            // Expendable = 1;
        }
        else
        {
            SetRedPoint(48, false);
            // Expendable = 0;
        }
        // int Copy = 0;//日常副本
        for (int i = 0; i < CharacterRecorder.instance.EveryDayNumberRedPoint.Count; i++)
        {
            if (CharacterRecorder.instance.EveryDayNumberRedPoint[i] != 0 || CharacterRecorder.instance.EveryDayTimerRedPoint[i] != 0)
            {
                SetRedPoint(22, true);
                // Copy = 1;
                break;
            }
            else
            {
                SetRedPoint(22, false);
                // Copy = 0;

            }
        }
        // int Smuggle = 0;
        if (CharacterRecorder.instance.SmuggleNum > 0 && CharacterRecorder.instance.SmuggleTime == 0 && CharacterRecorder.instance.lastGateID > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.bianjingzousi).Level)
        {
            SetRedPoint(49, true);
            //Smuggle = 1;
        }
        else
        {
            SetRedPoint(49, false);
            // Smuggle = 0;
        }
        //int Conquer = 0;
        if (CharacterRecorder.instance.isConquerRedPoint && TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.zhengfu).Level < CharacterRecorder.instance.lastGateID)
        {
            SetRedPoint(14, true);
            // Conquer = 1;
        }
        else
        {
            SetRedPoint(14, false);
            // Conquer = 0;
        }
        // if ((Pvp == 1 || Challenge == 1 || Expendable == 1 || Copy == 1 || Smuggle == 1 || Conquer == 1))//&&CharacterRecorder.instance.level>=25
        if (AllRedPoint[14] || AllRedPoint[49] || AllRedPoint[22] || AllRedPoint[23] || AllRedPoint[48] || AllRedPoint[5])
        {
            SetRedPoint(21, true);
        }
        else
        {
            SetRedPoint(21, false);
        }
    }
    #endregion
    #region 购买体力，消费金币等在主界面刷新任务红点
    public void MainTaskReadPoint()
    {
        StartCoroutine(Main_TaskReadPoint());
    }
    IEnumerator Main_TaskReadPoint() //购买体力，消费金币等在主界面刷新任务红点
    {
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1201#1;");
        yield return new WaitForSeconds(0.2f);
        int num = TextTranslator.instance.TaskCompleteNum() + TextTranslator.instance.AchievementCompleteNum();//TextTranslator.instance.AchievementComplete();
        if (num > 0 && CharacterRecorder.instance.level >= 10)
        {
            SetRedPoint(0, true);
        }
        else
        {
            SetRedPoint(0, false);
        }
    }
    #endregion
    #region 首冲红点
    public void FirstVipRedpoint()
    {
        Vip vipOne = TextTranslator.instance.GetVipDicByID(1);
        if (CharacterRecorder.instance.BuyGiftBag[0] == "1")
        {
            SetRedPoint(11, false);
        }
        else if (CharacterRecorder.instance.BuyGiftBag[0] == "0")
        {
            SetRedPoint(11, true);
        }
    }
    #endregion
    #region 实验室小红点
    public void SetLabRedPoint()
    {
        if (CharacterRecorder.instance.empetyCountToOnputHero > 0 && CharacterRecorder.instance.lastGateID > TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.shiyanshi).Level)
        {
            SetRedPoint(13, true);
        }
        else
        {
            SetRedPoint(13, false);
        }
    }
    #endregion
    #region 兑换活动红点
    public void ItemFixedListIsOpen(string Contents)
    {
        string[] dataSplit = Contents.Split(';');
        ItemFixedOpenList.Clear();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            //if (int.Parse(dataSplit[i]) > 130000 && int.Parse(dataSplit[i]) < 140000)
            {
                ItemFixedOpenList.Add(int.Parse(dataSplit[i]));
            }
        }
    }
    /// <summary>
    /// 每日充值
    /// </summary>
    /// <param name="Contents"></param>
    public void EveryDayPayReward(string Contents)
    {   
        string[] dataSplit = Contents.Split('!');
        List<int> list = new List<int>();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] infoSplit = dataSplit[i].Split('$');
            if (int.Parse(infoSplit[2]) == 1)
            {
                SetRedPoint(51, true);
                return;
            }
            else
            {
                SetRedPoint(51, false);
            }
        }
    }
    /// <summary>
    /// 单笔充值红点
    /// </summary>
    /// <param name="Contents"></param>
    public void OncePayReward(string Contents)
    {
        string[] dataSplit = Contents.Split('!');
        List<int> list = new List<int>();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] infoSplit = dataSplit[i].Split('$');
            if (int.Parse(infoSplit[1]) == 1)
            {
                SetRedPoint(50, true);
                return;
            }
            else
            {
                SetRedPoint(50, false);
            }
        }
    }
    /// <summary>
    /// 兑换活动红点显示
    /// </summary>
    /// <param name="Contents"></param>
    public void ChangeItemExchangRedPoint(string Contents)
    {
        string[] dataSplit = Contents.Split('!');
        ItemExchangButtonList.Clear();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] infoSplit = dataSplit[i].Split('$');
            if (int.Parse(infoSplit[4]) == 1)
            {
                bool isAdd = false;
                if (ItemExchangButtonList.Count == 0)
                {
                    ItemExchangButtonList.Add(TextTranslator.instance.GetActivityItemFixedDicByIndex(int.Parse(infoSplit[0])).ActivityID);
                }
                else
                {
                    for (int j = 0; j < ItemExchangButtonList.Count; j++)
                    {
                        //Debug.LogError(int.Parse(infoSplit[0]) + "    " + infoSplit[4] + "   ");
                        if (ItemExchangButtonList[j] == TextTranslator.instance.GetActivityItemFixedDicByIndex(int.Parse(infoSplit[0])).ActivityID)
                        {
                            isAdd = true;
                            break;
                        }
                        else
                        {
                            isAdd = false;
                        }
                    }
                    if (isAdd == false)
                    {
                        ItemExchangButtonList.Add(TextTranslator.instance.GetActivityItemFixedDicByIndex(int.Parse(infoSplit[0])).ActivityID);
                    }
                }
            }
        }
        for (int i = 0; i < ItemExchangButtonList.Count; i++)
        {
            bool isSame = false;
            for (int j = 0; j < ItemFixedOpenList.Count; j++)
            {
                if (ItemExchangButtonList[i] == ItemFixedOpenList[j])
                {
                    isSame = true;
                }
            }
            if (isSame == false)
            {
                ItemExchangButtonList.Remove(ItemExchangButtonList[i]);
            }
        }
        if (ItemExchangButtonList.Count > 0 && ItemExchangButtonList[0] != 0)
        {
            SetRedPoint(46, true);
        }
        else
        {
            SetRedPoint(46, false);
        }

    }
    #endregion
    /// ******************************************************************************以上是红点显示*****************************************************************************///
    public void AddOwnedHeroList(Hero hero)
    {
        ownedHeroList.Add(hero);
        //ownedHeroList.Sort();
    }
    //战力降序排序
    public void SortHeroListByForce()
    {
        int listSize = ownedHeroList.size;
        for (int i = 0; i < listSize; i++)
        {
            for (var j = listSize - 1; j > i; j--)
            {
                Hero heroA = ownedHeroList[i];
                Hero heroB = ownedHeroList[j];
                if (heroA.force < heroB.force)
                {
                    var temp = ownedHeroList[i];
                    ownedHeroList[i] = ownedHeroList[j];
                    ownedHeroList[j] = temp;
                }
            }
        }
    }
    static public int SortHero(Hero a, Hero b)
    {
        return a.sort.CompareTo(b.sort);
    }

    IEnumerator AutoUpdateLegionData()
    {
        yield return new WaitForSeconds(60.0f);
        NetworkHandler.instance.SendProcess(string.Format("8018#{0};", ""));
        StartCoroutine(AutoUpdateLegionData());
    }
    public IEnumerator AutoUpdateLegionCheckPartLeftTime()
    {
        yield return new WaitForSeconds(1.0f);
        CheckPart.leftTime -= 1;
        //Debug.Log("CheckPart.leftTime..........." + CheckPart.leftTime);
        if (CheckPart.leftTime > 0)
        {
            StartCoroutine(AutoUpdateLegionCheckPartLeftTime());
        }
        else
        {
            StopCoroutine(AutoUpdateLegionCheckPartLeftTime());
        }
    }
    /* public IEnumerator AutoUpdateHoldOnLeftTime()//1901  1902 //去掉挂机奖励的协成 放到 SceneTransformer的Update里
     {
         yield return new WaitForSeconds(1.0f);
         HoldOnLeftTime -= 1;
         if (HoldOnLeftTime > 0)
         {
             StartCoroutine(AutoUpdateHoldOnLeftTime());
         }
         else
         {
             NetworkHandler.instance.SendProcess("1902#;");
             StopCoroutine(AutoUpdateHoldOnLeftTime());
         }
         if (GachaMore == 0 && GachaOnce > 0 && GachaMoreTime == 0)
         {
             NetworkHandler.instance.SendProcess("5204#;");
             GachaMoreTime--;
         }
         else if (GachaMore == 0 && GachaOnce > 0 && GachaMoreTime > 0)
         {
             GachaMoreTime--;
         }
         if (lastGateID > 10011 && PvpChallengeNum > 0 && PvpRefreshTime == 0)
         {
             StartCoroutine(SendPvPInfo());
             PvpRefreshTime--;
         }
         else if (lastGateID > 10011 && PvpChallengeNum > 0 && PvpRefreshTime >0)
         {
             PvpRefreshTime--;
         }
     }*/

    IEnumerator SendPvPInfo()
    {
        NetworkHandler.instance.SendProcess("6001#;"); //取得pvp状态
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("6009#;"); //取得积分奖励
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("6011#;"); //取得排名奖励
        yield return new WaitForSeconds(0.5f);
        //if (GameObject.Find("MainWindow") != null)
        //{
        //    GameObject.Find("MainWindow").GetComponent<MainWindow>().Collision();
        //}
        Collision();
    }
    /// <summary>
    /// 取得拥有的英雄的一个引用
    /// </summary>
    /// <param name="characterRoleID"></param>
    /// <returns></returns>
    public Hero GetHeroByCharacterRoleID(int characterRoleID)
    {
        foreach (Hero hero in ownedHeroList)
        {
            if (hero.characterRoleID == characterRoleID)
            {
                return hero;
            }
        }
        return null;
    }

    public Hero GetHeroByRoleID(int RoleID)
    {
        foreach (Hero hero in ownedHeroList)
        {
            if (hero.cardID == RoleID)
            {
                return hero;
            }
        }
        return null;
    }


    public class Role
    {
        public int CharacterRoleID;
        public string Name;
        public int RoleID;
        public int Career;
        public int Sex;
        public int Grade;
        public int Star;
        public int NowStar;
        public int Position;
        public int OtherPosition;
        public int Fight;
        public int Level;
        public int Exp;
        public int Blood;
        public int Attack;
        public int Defend;
        public int Skill1;
        public int SkillLevel1;
        public int Skill2;
        public int SkillLevel2;
        public int Skill3;
        public int SkillLevel3;
        public int Skill4;
        public int SkillLevel4;
        public int PartnerSkill1;
        public int PartnerSkill2;
    }


    void Start()
    {
        instance = this;
        //StartCoroutine(AutoUpdateLegionData());
        if (AllRedPoint.size < 100)
        {
            for (int i = 0; i < 100; i++)
            {
                AllRedPoint.Add(false);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            CharacterRecorder.instance.EveryDayNumberRedPoint.Add(0);
            CharacterRecorder.instance.EveryDayTimerRedPoint.Add(0);
        }
    }

    public int SetRoleNewPosition(int CharacterRoleID, int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.CharacterRoleID == CharacterRoleID)
            {
                int OldPosition = r.Position;
                r.Position = Position;
                return OldPosition;
            }
        }
        return 0;
    }

    public int SetTrainNewPosition(int CharacterRoleID, int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.CharacterRoleID == CharacterRoleID)
            {
                int OldPosition = r.OtherPosition;
                r.OtherPosition = Position;
                return OldPosition;
            }
        }
        return 0;
    }

    public void SetRole(int SetCharacterRoleID, int SetStar, int SetNowStar, int SetPosition, int SetOtherPosition, int SetFight, int SetLevel, int SetExp, int SetBlood, int SetAttack, int SetDefend)
    {
        for (int i = 0; i < ListRole.Count; i++)
        {
            if (ListRole[i].CharacterRoleID == SetCharacterRoleID)
            {
                ListRole[i].Star = SetStar;
                ListRole[i].NowStar = SetNowStar;
                ListRole[i].Position = SetPosition;
                ListRole[i].OtherPosition = SetOtherPosition;
                ListRole[i].Fight = SetFight;
                ListRole[i].Level = SetLevel;
                ListRole[i].Exp = SetExp;
                ListRole[i].Blood = SetBlood;
                ListRole[i].Attack = SetAttack;
                ListRole[i].Defend = SetDefend;
                if (i == 0)
                {
                    if (GameObject.Find("LabelCharacterLevel") != null)
                    {
                        GameObject.Find("LabelCharacterLevel").GetComponent<UILabel>().text = ListRole[0].Level.ToString();
                    }
                }
                break;
            }
        }
    }

    public void AddRole(int SetCharacterRoleID, string SetName, int SetRoleID, int SetCareer, int SetSex, int SetGrade, int SetStar, int SetNowStar, int SetPosition, int SetOtherPosition, int SetFight, int SetLevel, int SetExp, int SetBlood, int SetAttack, int SetDefend, int SetSkill1, int SetSkillLevel1, int SetSkill2, int SetSkillLevel2, int SetSkill3, int SetSkillLevel3, int SetSkill4, int SetSkillLevel4, int SetPartnerSkill1, int SetPartnerSkill2)
    {
        if (ListRole.Count > 0)
        {
            Color GradeColor;
            if (SetGrade == 0)
            {
                GradeColor = Color.white;
            }
            else if (SetGrade == 1)
            {
                GradeColor = Color.blue;
            }
            else if (SetGrade == 2)
            {
                GradeColor = new Color(1, 0, 1);
            }
            else if (SetGrade == 3)
            {
                GradeColor = Color.red;
            }
            else if (SetGrade == 4)
            {
                GradeColor = Color.yellow;
            }
            else
            {
                GradeColor = Color.cyan;
            }

            if (SetPosition > 0)
            {
                //int Index = gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(SetRoleID, SetName, new Vector3(0, 0, 0), 1, 0, 400, 400, 4, 1, GradeColor, SetBlood, SetBlood, (SetCareer == 2 ? 2.5f : 5.5f), false, false, false, false, false, 1, 2f, CharacterID + "_" + SetPosition, 0, 0);

                //Vector3 StartPosition = gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[0].RoleObject.transform.position;
                //if (SetPosition == 1)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x, StartPosition.y + 1f, StartPosition.y + 1f);
                //}
                //else if (SetPosition == 2)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x + 0.7f, StartPosition.y, StartPosition.y);
                //}
                //else if (SetPosition == 3)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x - 0.7f, StartPosition.y, StartPosition.y);
                //}
                //else if (SetPosition == 4)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x + 1.4f, StartPosition.y - 0.5f, StartPosition.y - 0.5f);
                //}
                //else if (SetPosition == 5)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x - 1.4f, StartPosition.y - 0.5f, StartPosition.y - 0.5f);
                //}
            }
        }
        else
        {

            ////////////////////////创建主角(以下)////////////////////
            //gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(SetRoleID, Name, new Vector3(0, 0, 0), 1, 0, 400, 400, 4, 1, Color.cyan, SetBlood, SetBlood, (SetCareer == 2 ? 2.5f : 5.5f), false, false, false, false, false, 1, 3f, CharacterID.ToString(), 0, 0);
            //GameObject.Find("MainCamera").transform.position = new Vector3(0, 0, GameObject.Find("MainCamera").transform.position.z);
            ////////////////////////创建主角(以上)////////////////////
        }
        Role NewRole = new Role();
        NewRole.CharacterRoleID = SetCharacterRoleID;
        NewRole.Name = SetName;
        NewRole.RoleID = SetRoleID;
        NewRole.Career = SetCareer;
        NewRole.Sex = SetSex;
        NewRole.Grade = SetGrade;
        NewRole.Star = SetStar;
        NewRole.NowStar = SetNowStar;
        NewRole.Position = SetPosition;
        NewRole.OtherPosition = SetOtherPosition;
        NewRole.Fight = SetFight;
        NewRole.Level = SetLevel;
        NewRole.Exp = SetExp;
        NewRole.Blood = SetBlood;
        NewRole.Attack = SetAttack;
        NewRole.Defend = SetDefend;
        NewRole.Skill1 = SetSkill1;
        NewRole.SkillLevel1 = SetSkillLevel1;
        NewRole.Skill2 = SetSkill2;
        NewRole.SkillLevel2 = SetSkillLevel2;
        NewRole.Skill3 = SetSkill3;
        NewRole.SkillLevel3 = SetSkillLevel3;
        NewRole.Skill4 = SetSkill4;
        NewRole.SkillLevel4 = SetSkillLevel4;
        NewRole.PartnerSkill1 = SetPartnerSkill1;
        NewRole.PartnerSkill2 = SetPartnerSkill2;
        ListRole.Add(NewRole);
    }


    public int GetCharacterRoleIDByPosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.Position == Position)
            {
                return r.CharacterRoleID;
            }
        }
        return 0;
    }

    public int GetCharacterRoleIDByOtherPosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.OtherPosition == Position)
            {
                return r.CharacterRoleID;
            }
        }
        return 0;
    }

    public int GetRoleIDByPosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.Position == Position)
            {
                return r.RoleID;
            }
        }
        return 0;
    }

    public int GetRoleIDByOtherPosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.OtherPosition == Position)
            {
                return r.RoleID;
            }
        }
        return 0;
    }

    public int GetGradeByPosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.Position == Position)
            {
                return r.Grade;
            }
        }
        return 0;
    }

    public int GetGradeByOtherPosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (r.OtherPosition == Position)
            {
                return r.Grade;
            }
        }
        return 0;
    }

    public void ResetRole()
    {
        foreach (Role r in ListRole)
        {
            if (r.Name == characterName)
            {
                continue;
            }
            Color GradeColor;
            if (r.Grade == 0)
            {
                GradeColor = Color.white;
            }
            else if (r.Grade == 1)
            {
                GradeColor = Color.blue;
            }
            else if (r.Grade == 2)
            {
                GradeColor = new Color(1, 0, 1);
            }
            else if (r.Grade == 3)
            {
                GradeColor = Color.red;
            }
            else if (r.Grade == 4)
            {
                GradeColor = Color.yellow;
            }
            else
            {
                GradeColor = Color.cyan;
            }

            if (r.Position > 0)
            {
                //int Index = gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(0, 10000, 10000), 1, 0, 400, 400, 4, 1, GradeColor, r.Blood, r.Blood, (r.Career == 2 ? 2.5f : 5.5f), false, false, false, false, false, 1, 2f, CharacterID + "_" + r.Position.ToString(), 0, 0);

                //Vector3 StartPosition = gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[0].RoleObject.transform.position;
                //if (r.Position == 1)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x, StartPosition.y + 1f, StartPosition.y + 1f);
                //}
                //else if (r.Position == 2)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x + 0.7f, StartPosition.y, StartPosition.y);
                //}
                //else if (r.Position == 3)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x - 0.7f, StartPosition.y, StartPosition.y);
                //}
                //else if (r.Position == 4)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x + 1.4f, StartPosition.y - 0.5f, StartPosition.y - 0.5f);
                //}
                //else if (r.Position == 5)
                //{
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().ListRolePicture[Index].RoleObject.transform.position = new Vector3(StartPosition.x - 1.4f, StartPosition.y - 0.5f, StartPosition.y - 0.5f);
                //}
            }
        }
    }

    public void SetRolePosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (Position != r.Position)
            {
                continue;
            }
            //switch (r.Position)
            {
                //case 1:
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(0f, 138.5f, 138.5f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //1
                //    break;
                //case 2:
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(1.3f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //2
                //    break;
                //case 3:
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(-1.3f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //3
                //    break;
                //case 4:
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(2.3f, 138.2f, 138.2f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //4
                //    break;
                //case 5:
                //    gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(-2.3f, 138.2f, 138.2f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //5
                //    break;
            }
            if (r.Grade == 1)
            {
                GameObject.Find("LabelRole" + r.Position.ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                GameObject.Find("LabelRole" + r.Position.ToString()).GetComponent<UILabel>().color = Color.blue;
            }
            else if (r.Grade == 2)
            {
                GameObject.Find("LabelRole" + r.Position.ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                GameObject.Find("LabelRole" + r.Position.ToString()).GetComponent<UILabel>().color = new Color(1, 0, 1);
            }
            else if (r.Grade == 3)
            {
                GameObject.Find("LabelRole" + r.Position.ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                GameObject.Find("LabelRole" + r.Position.ToString()).GetComponent<UILabel>().color = Color.red;
            }
        }
    }

    public void SetTrainPosition(int Position)
    {
        foreach (Role r in ListRole)
        {
            if (Position != r.OtherPosition && Position != (r.OtherPosition - 4))
            {
                continue;
            }
            //switch (r.OtherPosition)
            {
                //case 1:
                //    if (GameObject.Find("TrainWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(-3.75f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //1
                //    }
                //    break;
                //case 5:
                //    if (GameObject.Find("FishWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(-3.75f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //1
                //    }
                //    break;
                //case 2:
                //    if (GameObject.Find("TrainWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(-1.25f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //3
                //    }
                //    break;
                //case 6:
                //    if (GameObject.Find("FishWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(-1.25f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //3
                //    }
                //    break;
                //case 3:
                //    if (GameObject.Find("TrainWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(1.25f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //2
                //    }
                //    break;
                //case 7:
                //    if (GameObject.Find("FishWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(1.25f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //2
                //    }
                //    break;
                //case 4:
                //    if (GameObject.Find("TrainWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(3.75f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //4
                //    }
                //    break;
                //case 8:
                //    if (GameObject.Find("FishWindow") != null)
                //    {
                //        gameObject.transform.parent.GetComponent<GameCenter>().gamePictureCreater.GetComponent<PictureCreater>().CreateRole(r.RoleID, r.Name, new Vector3(3.75f, 139.3f, 139.3f), 1, 0, 400, 400, 4, 1, Color.white, 10000, 10000, 1f, false, false, false, false, false, 1, 2f, "Role" + r.CharacterRoleID.ToString(), 0, 0); //4
                //    }
                //    break;
            }
            if (GameObject.Find("TrainWindow") != null && r.OtherPosition < 5)
            {
                if (r.Grade == 1)
                {
                    GameObject.Find("LabelRole" + r.OtherPosition.ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                    GameObject.Find("LabelRole" + r.OtherPosition.ToString()).GetComponent<UILabel>().color = Color.blue;
                }
                else if (r.Grade == 2)
                {
                    GameObject.Find("LabelRole" + r.OtherPosition.ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                    GameObject.Find("LabelRole" + r.OtherPosition.ToString()).GetComponent<UILabel>().color = new Color(1, 0, 1);
                }
                else if (r.Grade == 3)
                {
                    GameObject.Find("LabelRole" + r.OtherPosition.ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                    GameObject.Find("LabelRole" + r.OtherPosition.ToString()).GetComponent<UILabel>().color = Color.red;
                }
            }
            else if (GameObject.Find("FishWindow") != null && r.OtherPosition > 4)
            {
                if (r.Grade == 1)
                {
                    GameObject.Find("LabelRole" + (r.OtherPosition - 4).ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                    GameObject.Find("LabelRole" + (r.OtherPosition - 4).ToString()).GetComponent<UILabel>().color = Color.blue;
                }
                else if (r.Grade == 2)
                {
                    GameObject.Find("LabelRole" + (r.OtherPosition - 4).ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                    GameObject.Find("LabelRole" + (r.OtherPosition - 4).ToString()).GetComponent<UILabel>().color = new Color(1, 0, 1);
                }
                else if (r.Grade == 3)
                {
                    GameObject.Find("LabelRole" + (r.OtherPosition - 4).ToString()).GetComponent<UILabel>().text = "LV:" + r.Level + " " + r.Name;
                    GameObject.Find("LabelRole" + (r.OtherPosition - 4).ToString()).GetComponent<UILabel>().color = Color.red;
                }
            }
        }
    }
    public void SetNewPlayerPrefsKey() //新建号时确立所有本地保存状态
    {
        string name = PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID");
        PlayerPrefs.SetInt("PvpRankPodition_" + name, 0);//竞技场布阵红点
        PlayerPrefs.SetInt("ActivityFinalReward_" + name, 0);//大放送最终奖励
        PlayerPrefs.SetInt("QuestionState_" + name, 0);//问卷调查
    }


    // 初始化角色信息
    public void SetCharacter(string setName, int setLevel, int setGold, int setLunaGem, int setStamina, int setStaminaCap, int setSpirit, int setSpiritCap, int setStoryID, int setGhost, int setExp, int setExpMax,
        int setLastGateID, int setFight, int setMapID, int userId)
    {
        characterName = setName;
        level = setLevel;
        gold = setGold;
        lunaGem = setLunaGem;
        stamina = setStamina;
        staminaCap = setStaminaCap;
        sprite = setSpirit;
        spriteCap = setSpiritCap;
        ghost = setGhost;

        exp = setExp;
        expMax = setExpMax;

        blood = 9999;
        bloodMax = 9999;
        lastGateID = setLastGateID;
        lastGiftGateID = setLastGateID;
        this.storyID = setStoryID;
        mapID = setMapID;
        Fight = setFight;
        this.userId = userId;
        PlayerPrefs.SetInt("UserID", userId);
    }





    ///////////////////////////////////////////数值变化 (以下)///////////////////////////////////////////
    public void AddMoney(int added)
    {
        gold = gold + added;
        RefreshMainUI();
    }

    public void SetMoney(int setMoney)
    {
        gold = setMoney;
        RefreshMainUI();
    }


    public void AddLunaGem(int added)
    {
        lunaGem = lunaGem + added;
        RefreshMainUI();
    }

    public void SetLunaGem(int setLunaGem)
    {
        lunaGem = setLunaGem;
        RefreshMainUI();
    }


    public void AddStamina(int added)
    {
        stamina = stamina + added;
        RefreshMainUI();
    }

    public void SetStamina(int setStamina)
    {
        stamina = setStamina;
        RefreshMainUI();
    }

    public void RefreshMainUI()
    {
        //foreach (CommonTopUI ui in CommonTopUI.topUIList)
        //{
        //    ui.RefreshUIInfo();
        //}
    }


    /// <summary>
    /// 设定等级
    /// </summary>
    public void SetLevel(int setLevel)
    {
        CharacterRecorder.instance.level = setLevel;
    }
    ///////////////////////////////////////////数值变化 (以上)///////////////////////////////////////////
}

