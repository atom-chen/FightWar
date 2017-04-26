using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holagames;
public class TextTranslator : MonoBehaviour
{
    public enum NewGuildIdEnum
    {
        /// <summary>
        /// 战术
        /// </summary>
        zhanshu = 1,
        /// <summary>
        /// 精英关卡
        /// </summary>
        jingyingguangka = 2,
        /// <summary>
        /// 竞技场
        /// </summary>
        jingjichang = 3,
        /// <summary>
        /// 激励士气
        /// </summary>
        jilishiqi = 4,
        /// <summary>
        /// 商店
        /// </summary>
        shop = 5,
        /// <summary>
        /// 日常副本
        /// </summary>
        richangfuben = 6,
        /// <summary>
        /// 第二个指挥官技能
        /// </summary>
        zhihuiSkill_2 = 7,
        /// <summary>
        /// 夺宝奇兵
        /// </summary>
        duobao = 8,
        /// <summary>
        /// 情报
        /// </summary>
        qingbao = 9,
        /// <summary>
        /// 紧急治疗
        /// </summary>
        jinjizhiliao = 10,
        /// <summary>
        /// 英雄榜
        /// </summary>
        heroRank = 11,
        /// <summary>
        /// 英雄培养
        /// </summary>
        heroStrength = 12,
        /// <summary>
        /// 世界事件
        /// </summary>
        workdEvent = 13,
        /// <summary>
        /// 空降兵
        /// </summary>
        kongjiangbing = 14,
        /// <summary>
        /// 资源占领
        /// </summary>
        ziyuanzhanling = 15,
        /// <summary>
        /// 无敌护盾
        /// </summary>
        wudihudun = 16,
        /// <summary>
        /// 组队副本
        /// </summary>
        zuduifuben = 17,
        /// <summary>
        /// 军团
        /// </summary>
        juntuan = 18,
        /// <summary>
        /// 饰品精炼
        /// </summary>
        shipingjinglian = 19,
        /// <summary>
        /// 震爆弹
        /// </summary>
        zhengbaodan = 21,
        /// <summary>
        /// 丛林冒险
        /// </summary>
        conglingmaoxian = 20,
        /// <summary>
        /// 座驾
        /// </summary>
        zuojia = 22,
        /// <summary>
        /// 征服
        /// </summary>
        zhengfu = 23,
        /// <summary>
        /// 千锤百炼
        /// </summary>
        qiancuibailian = 24,
        /// <summary>
        /// 边境走私
        /// </summary>
        bianjingzousi = 25,
        /// <summary>
        /// 第三个指挥官技能
        /// </summary>
        zhihuiSkill_3 = 26,
        /// <summary>
        /// 技能突破
        /// </summary>
        jinengtupo = 27,
        /// <summary>
        /// 急救物资
        /// </summary>
        jijiuwuzi = 28,
        /// <summary>
        /// 装备精炼
        /// </summary>
        zhuangbeijinglian = 29,
        /// <summary>
        /// 战术导弹
        /// </summary>
        zhanshudaodan = 30,
        /// <summary>
        /// 王者之路
        /// </summary>
        wangzhezhilu = 31,
        /// <summary>
        /// 花式军演
        /// </summary>
        huashijunyan = 32,
        /// <summary>
        /// 净化
        /// </summary>
        jinghua = 33,
        /// <summary>
        /// 实验室
        /// </summary>
        shiyanshi = 34,
        /// <summary>
        /// 极限挑战
        /// </summary>
        jixiantiaozhan = 35,
        /// <summary>
        /// 驱散
        /// </summary>
        qushan = 36,
        /// <summary>
        /// 路障
        /// </summary>
        luzhang = 37,
        /// <summary>
        /// 核武器
        /// </summary>
        hewuqi = 38,
        /// <summary>
        /// 重生
        /// </summary>
        chongsheng = 39,
        /// <summary>
        /// 国战
        /// </summary>
        guozhan = 40,
        /// <summary>
        /// 核电战
        /// </summary>
        hedianzhan = 41,
    }

    public static TextTranslator instance;
    public Hero heroBefore = new Hero();
    public Hero heroNow;
    public int ReloadCount = 0;
    public int GUIReloadCount = 0;
    public string RemainTimesText = "剩馀次数";

    public string NoneText = "无";
    public string TimesText = "次";
    public string PieceText = "个";

    public string RankMoneyText = "悬赏金额";
    public string Rank3Text = "三连胜\n";
    public string Rank5Text = "五胜\n";
    public string Rank10Text = "每日打满十场\n";
    public string Rank200Text = "进入前200名\n";
    public string Rank1000Text = "进入前1000名\n";
    public string RankDayText = "每日排名礼包\n";



    public string ReturnText = "返回";
    public string RollPointText = "阅历";
    public string CharmText = "威望";

    public string RecvTaskText = "接任务";
    public string FinishTaskText = "执行任务";
    public string ReturnTaskText = "回任务";

    public string RandomPokeText = "开始洗牌";
    public string ChoosePokeText = "随机选牌";
    public string LeaveGateText = "离开关卡";

    public string MoneyCountText = "今日可摇";
    public string MoneyMoneyText = "摇一次:";
    public string MoneyItemText = "摇钱令";

    public string StartTrainText = "开始训练";
    public string StopTrainText = "停止训练";
    public string TrainTimeText = "训练时间";
    public string TrainExpText = "获得经验";

    public string StartFishText = "开始钓鱼";
    public string StopFishText = "停止钓鱼";
    public string FishTimeText = "钓鱼时间";
    public string FishMoneyText = "获得金钱";

    public string classText = "品阶";
    public string equipText = "装备";
    public string dischargeText = "卸下";
    public string HPText = "生命";
    public string strengthText = "力量";
    public string intelligenceText = "智力";
    public string agileText = "敏捷";
    public string physicalDefenseText = "物防";
    public string armorText = "护甲";
    public string magicDefenseText = "魔防";
    public string magicResistanceText = "魔抗";
    public string physicalCritText = "物暴";
    public string magicCritText = "魔暴";
    public string attackSpeedText = "攻速";
    public string moveText = "移动";
    public string skillText = "技能";
    public string ConfirmDischargeText = "卸下此宝珠需要花费%星币，确认吗?";
    public string DataNoneText = "(暂无)";

    public string IntensifySuccessText = "强化成功";
    public string ClassUpSuceessText = "进阶成功";
    public string OrbEquipSuceessText = "装备成功";
    public string OrbRemoveSuceessText = "卸下成功";
    public string ClassUpUnableText = "无法进阶";
    public string EquipLevelNotEnoughText = "装备等级不达标";
    public string CoinNotEnoughText = "星币不足";
    public string MaterialNotEnoughText = "材料不足";
    public string OrbEquipFailText = "装备失败";
    public string DataExceptionText = "数据异常";
    public string EquipLevelFullText = "装备等级已达装备强化等级上限";
    public string EquipQuantityErrorText = "装备数目错误";
    public string OrbIntensifyUnableText = "不可合成的宝珠";
    public string OrbLevelFullText = "宝珠等级达上限";

    static public string staminaText = "体力";
    static public string levelText = "等级";

    static public string forceText = "战斗力";
    static public string rankText = "排名";

    static public string todayFinishedAchievement = "今日已完成成就";
    static public string getText = "领取";
    static public string gotoText = "前往";
    static public string gotText = "已领取";

    static public string ownCountString = "拥有 {0} 件";
    static public string sellString = "出售单价： {0}";
    static public string gainsString = "获得[{0}金币]";
    static public string emptyAllString = "空荡荡的背包，求你装点什么进去吧，哪怕是英文第2个字母也行啊。";
    static public string emptyJewelString = "宝珠可以镶嵌在装备上，是提升装备属性的好方法。宝珠也可以用于合成更高等级的宝珠哟。";
    static public string emptyFragmentString = "集满七颗龙珠可以召唤神龙，而收集满足够的碎片则可以召唤……";
    static public string emptyUsedString = "这里将放置的是日常居家旅行，打怪旅行必备消耗品，淘宝都买不到哦！";
    static public string skillNameText = "技能名";

    static public string costGoldText = "消耗星币";
    static public string conditionNotEnoughText = "条件不足";

    static public string countLableString = "未读：{0}/{1}";
    static public string enmptyMailboxString = "邮箱空空如也";
    static public string senderText = "寄信人";

    static public string buyGoldString = "本次购买星币需要花费{0}月石，是否继续？（今日已购买{1}次）";
    static public string buyStaminaString = "购买120体力需要花费{0}月石，是否继续？（今日已购买{1}次）";
    static public string buyExceedString = "当前购买次数已超过上限？（今日已购买{0}次）";
    static public string getGoldString = "恭喜得到{0}金币";
    static public string getGold2String = "[fe00ff]暴击x{0}[-] 恭喜得到{1}金币";
    static public string soundOnText = "声音 开";
    static public string soundOffText = "声音 关";

    static public string vipNotEnoughText = "VIP 等级不足";
    static public string buyCountNotEnoughText = "购买次数不足";
    static public string gemNotEnoughText = "月石不足";
    static public string questNotconformText = "关卡不符合扫荡";
    static public string staminaNotEnoughText = "体力不足";
    static public string wipeCountNotEnoughText = "无扫荡次数";

    static public string PVPCountDownText = "秒内进行挑战";
    static public string PVPCountDown2Text = "分钟内进行挑战";

    static public string refightText = "是否再次战斗？";
    static public string levelToCapText = "等级到达上限";

    //public string[] LoadingTips = new string[22];
    //public string[] GateFightInfo = new string[30];
    //public string[] RewardTaskText = new string[19];
    //public string[] GroupInfoText = new string[15];

    public string ServerInfo;//上次登陆的服务器信息
    public string Announcement;//游戏公告

    public List<string> PopupList = new List<string>();//主界面跑马灯信息;

    public readonly List<string> docQueue = new List<string>();////主界面跑马灯信息;

    void Start()
    {
        instance = this;

    }

    #region 所有界面item描述
    public void ItemDescription(GameObject OneItem, int ItemCode, int ItemCount)
    {
        if (OneItem.GetComponent<BoxCollider>() == null)
        {
            OneItem.AddComponent<BoxCollider>();
        }
        if (OneItem.GetComponent<ItemExplanation>() == null)
        {
            OneItem.AddComponent<ItemExplanation>();
        }
        OneItem.GetComponent<ItemExplanation>().SetAwardItem(ItemCode, ItemCount);
    }
    #endregion

    #region 公告跑马灯

    //添加文档 
    public void AddQueuedoc(string doc)
    {
        lock (this)
        {
            //从队列一端插入内容 
            //docQueue.Enqueue(doc);
            docQueue.Add(doc);
            Console.WriteLine("成功插入文档:");
        }
    }

    //读取文档 
    public string ReadQueuedoc()
    {
        string doc = null;
        lock (this)
        {
            //doc = docQueue.Dequeue();
            doc = docQueue[0];
            docQueue.RemoveAt(0);
            return doc;
        }
    }

    //只读属性,确定队列中是不是还有元素 
    public bool IsQueueAvailable
    {
        get
        {
            return docQueue.Count > 0;
        }
    }


    //public void AddPopupList(string popup) //添加文档 
    //{
    //    PopupList.Add(popup);
    //}

    //public string ReadPopupList() //读取文档 
    //{
    //    string doc = null;
    //    doc = docQueue.Dequeue();
    //    return doc;
    //}

    //public bool IsPopupList  //只读属性,确定队列中是不是还有元素 
    //{
    //    get
    //    {
    //        return docQueue.Count > 0;
    //    }
    //}

    //public void DeletPopupList(int num) //删除信息 
    //{
    //    PopupList.RemoveAt(num);
    //}
    #endregion

    #region Item
    /////////////////////道具相关(以下)/////////////////////
    public List<ItemInfo> itemInfoList = new List<ItemInfo>();
    public class ItemInfo
    {
        public int itemCode;
        public string itemName;
        public string itemDescription;
        public int itemGrade;  //品质框颜色
        public int skillID;
        public int skillLevel;
        public string feedExp;
        public int picID;
        public int sellType;
        public int sellPrice;
        public string Source1;
        public string Source2;
        public string Source3;
        public int ComposeNumber;
        public int itemType;
        public int Stact;
        public int FuncType;
        public string ToValue;
        public int BuyLevel;
        public int RelifeValue;
        public int ExchangeGold;
    }

    public void AddItem(int _itemCode, string _itemName, string _itemDescription, int _itemGrade, int _skillID, int _skillLevel, string _feedExp, int _icon, int _sellType, int _sellPrice
        , string _Source1, string _Source2, string _Source3, string _ComposeNumber, int _itemType, int FuncType, int BuyLevel, int _stack, int _relifeValue, int _exchangeGold)
    {
        ItemInfo newItem = new ItemInfo();
        newItem.itemCode = _itemCode;
        newItem.itemName = _itemName;
        newItem.itemGrade = _itemGrade;
        newItem.itemDescription = _itemDescription;
        newItem.skillID = _skillID;
        newItem.skillLevel = _skillLevel;
        newItem.feedExp = _feedExp;
        newItem.picID = _icon;
        newItem.sellType = _sellType;
        newItem.sellPrice = _sellPrice;
        newItem.Source1 = _Source1;
        newItem.Source2 = _Source2;
        newItem.Source3 = _Source3;
        newItem.itemType = _itemType;
        newItem.Stact = _stack;
        newItem.FuncType = FuncType;
        newItem.ToValue = _ComposeNumber;
        newItem.BuyLevel = BuyLevel;
        string[] dataSplit = _ComposeNumber.Split('$');
        newItem.ComposeNumber = int.Parse(dataSplit[0]);
        newItem.RelifeValue = _relifeValue;
        newItem.ExchangeGold = _exchangeGold;
        itemInfoList.Add(newItem);
    }


    /// <summary>
    /// 得到道具名字
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public string GetItemNameByItemCode(int itemCode)
    {
        string itemName = "";

        for (int i = 0; i < itemInfoList.Count; i++)
        {
            if (itemInfoList[i].itemCode == itemCode)
            {
                itemName = itemInfoList[i].itemName;
                break;
            }
        }
        return itemName;
    }


    /// <summary>
    /// 得到物品描述
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public string GetItemDescriptionByItemCode(int itemCode)
    {
        string itemDescription = "";

        for (int i = 0; i < itemInfoList.Count; i++)
        {
            if (itemInfoList[i].itemCode == itemCode)
            {
                itemDescription = itemInfoList[i].itemDescription;
                break;
            }
        }
        return itemDescription;
    }

    /// <summary>
    /// 得到道具信息
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public ItemInfo GetItemByItemCode(int itemCode)
    {
        ItemInfo item = new ItemInfo();

        for (int i = 0; i < itemInfoList.Count; i++)
        {
            if (itemInfoList[i].itemCode == itemCode)
            {
                item = itemInfoList[i];
                break;
            }
        }
        return item;
    }

    /// <summary>
    /// 得到道具类型  3 进阶材料 5 装备 7 碎片 8 碎片
    /// </summary>  
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public int GetItemTypeByItemCode(int itemCode)
    {
        int itemType = 0;

        if (itemCode.ToString().StartsWith("3"))
        {
            itemType = 3;
        }
        else if (itemCode.ToString().StartsWith("5"))
        {
            itemType = 5;
        }
        else if (itemCode.ToString().StartsWith("7") || itemCode.ToString().StartsWith("8"))
        {
            itemType = 7;
        }
        return itemType;
    }

    public List<ItemInfo> GetItemByFuncType(int FuncType)
    {
        List<ItemInfo> ItemList = new List<ItemInfo>();

        for (int i = 0; i < itemInfoList.Count; i++)
        {
            if (itemInfoList[i].FuncType == FuncType)
            {
                ItemInfo item = new ItemInfo();
                item = itemInfoList[i];
                ItemList.Add(item);
            }
        }
        return ItemList;
    }

    public int GetItemGradeByItemCode(int ItemCode)
    {
        int Grade = 1;

        for (int i = 0; i < itemInfoList.Count; i++)
        {
            if (itemInfoList[i].itemCode == ItemCode)
            {
                Grade = itemInfoList[i].itemGrade;
                break;
            }
        }
        return Grade;
    }

    public string GetNameColorByName(string Name)
    {
        string NameColor = "[ffffff]";

        for (int i = 0; i < itemInfoList.Count; i++)
        {
            if (itemInfoList[i].itemName == Name)
            {
                switch (itemInfoList[i].itemGrade)
                {
                    case 2:
                        NameColor = "[00ff00]";
                        break;
                    case 3:
                        NameColor = "[0000ff]";
                        break;
                    case 4:
                        NameColor = "[6907fd]";
                        break;
                    case 5:
                        NameColor = "[ff6c00]";
                        break;
                    case 6:
                        NameColor = "[ff0000]";
                        break;
                    case 7:
                        NameColor = "[ffff00]";
                        break;
                }
                break;
            }
        }
        return NameColor;
    }

    /////////////////////道具相关(以上)/////////////////////
    #endregion
    #region ItemSort
    /////////////////////道具相关(以下)/////////////////////
    public List<ItemSort> itemSortList = new List<ItemSort>();

    public void AddItemSort(ItemSort _ItemSort)
    {
        itemSortList.Add(_ItemSort);
    }
    public ItemSort GetItemSortByItemCode(int itemCode)
    {
        for (int i = 0; i < itemSortList.Count; i++)
        {
            if (itemSortList[i].ItemID == itemCode)
            {
                return itemSortList[i];
            }
        }
        return null;
    }
    /////////////////////道具相关(以上)/////////////////////
    #endregion

    #region Equip
    /////////////////////装备相关(以下)/////////////////////
    public Dictionary<int, EquipDetailInfo> equipInfoDic = new Dictionary<int, EquipDetailInfo>();

    public void AddEquipInfoList(EquipDetailInfo equipInfo)
    {
        equipInfoDic.Add(equipInfo.equipID, equipInfo);

    }

    public EquipDetailInfo GetEquipInfoByID(int itemID)
    {
        if (equipInfoDic.ContainsKey(itemID))
        {
            return equipInfoDic[itemID];
        }
        return null;
    }


    public Dictionary<int, EquipRefineCost> equipRefineCostDic = new Dictionary<int, EquipRefineCost>();

    public void AddEquipRefineCost(EquipRefineCost equipRefineCost)
    {
        equipRefineCostDic.Add(equipRefineCost.RefineLevel, equipRefineCost);
    }

    public EquipRefineCost GetEquipRefineCostByID(int RefineLevel)
    {
        if (equipRefineCostDic.ContainsKey(RefineLevel))
        {
            return equipRefineCostDic[RefineLevel];
        }
        return null;
    }
    public EquipRefineCost GetEquipRefineCostByNeedID(int heroLevel)
    {
        EquipRefineCost er = null;
        foreach (KeyValuePair<int, EquipRefineCost> item in equipRefineCostDic)
        {
            if (item.Value.Level <= heroLevel)
            {
                er = item.Value;
            }
        }
        return er;
    }
    ///////////////////////装备相关(以上)/////////////////////
    #endregion

    #region VIP
    /////////////////////VIP相关(以下)/////////////////////
    public List<VipInfo> vipInfoArray = new List<VipInfo>();

    public void AddVipUse(VipInfo vipInfo)
    {
        vipInfoArray.Add(vipInfo);
    }
    /////////////////////VIP相关(以上)/////////////////////
    #endregion

    #region Bag
    /////////////////////背包相关(以下)/////////////////////
    /// <summary>
    /// 背包物品list
    /// </summary>
    public BetterList<Item> bagItemList = new BetterList<Item>();
    public BetterList<Item> usefulItemList = new BetterList<Item>();//供饰品强化的材料
    public bool IsNeedUpdateUsefulItemList = true;
    public bool IsNeedUpdateItemInBag = false;//背包异动
    public bool isUpdateBag = false;

    public void SetBagItemList(BetterList<Item> newBagItemList)
    {
        bagItemList = newBagItemList;
    }

    public void AddBagItemList(Item item)
    {
        foreach (Item i in bagItemList)
        {
            if (i.itemCode == item.itemCode)
            {
                i.SetCount(item.itemCount);
                return;
            }
        }

        bagItemList.Add(item);
    }
    /// <summary>
    /// 取得背包里所有装备的list
    /// </summary>
    /// <returns></returns>
    public BetterList<Item> GetAllEquipInBag()
    {
        BetterList<Item> equipList = new BetterList<Item>();
        foreach (Item item in bagItemList)
        {
            if (item.itemCode > 51010 && item.itemCode < 60000 && item.itemCount > 0)
            {
                equipList.Add(item);
            }
        }
        return equipList;
    }
    //背包所有装备自动选择优先度排序 1蓝色功勋 2蓝色饰品 3金色功勋
    void SortHeroListByRules()
    {
        BetterList<Item> equipList = new BetterList<Item>();
        equipList = GetAllEquipInBag();
        int listSize = equipList.size;
        for (int i = 0; i < listSize; i++)
        {
            for (var j = listSize - 1; j > i; j--)
            {
                Item itemA = equipList[i];
                Item itemB = equipList[j];
                if (itemA.itemGrade != 3 && itemB.itemGrade == 3)
                {
                    var temp = equipList[i];
                    equipList[i] = equipList[j];
                    equipList[j] = temp;
                }
            }
        }

    }
    /// <summary>
    /// 取得背包里所有宝珠的list
    /// </summary>
    /// <returns></returns>
    public BetterList<Item> GetAllJewelInBag()
    {
        BetterList<Item> jewelList = new BetterList<Item>();
        foreach (Item item in bagItemList)
        {
            if (item.itemType == Item.ItemType.Orb && item.itemCount > 0)
            {
                jewelList.Add(item);
            }
        }
        return jewelList;
    }

    /// <summary>
    /// 取得背包里所有素材的list
    /// </summary>
    /// <returns></returns>
    public BetterList<Item> GetAllMaterialInBag()
    {
        BetterList<Item> MaterialList = new BetterList<Item>();
        foreach (Item item in bagItemList)
        {
            if (item.itemCode / 10000 == 3 && item.itemCount > 0)
            {
                MaterialList.Add(item);
            }
        }
        return MaterialList;
    }


    /// <summary>
    /// 取得背包里所有技能书的list
    /// </summary>
    /// <returns></returns>
    public BetterList<Item> GetAllSkillBooksInBag()
    {
        BetterList<Item> skillBooksList = new BetterList<Item>();
        foreach (Item item in bagItemList)
        {
            if (item.skillID > 1000 && item.itemCount > 0)
            {
                skillBooksList.Add(item);
            }
        }
        return skillBooksList;
    }
    /// <summary>
    /// 取得背包里所有宝物碎片的list
    /// </summary>
    /// <returns></returns>
    public List<Item> GetAllGrabInBag()
    {
        List<Item> GrabList = new List<Item>();
        foreach (Item item in bagItemList)
        {
            if ((item.itemCode > 80000 && item.itemCode < 85000) || (item.itemCode > 86000 && item.itemCode < 90000))
            {
                GrabList.Add(item);
            }
        }
        return GrabList;
    }

    /// <summary>
    /// 取得背包里所有宝物碎片可以合成的list
    /// </summary>
    /// <returns></returns>
    //public List<Item> GetFinishItem(List<Item> GrabList)
    //{
    //    List<Item> FinishList = new List<Item>();
    //    int num = 0;
    //    foreach (Item item in GrabList)
    //    {

    //        for (int i = 0; i < item.itemGrade; i++)
    //        {
    //            for (int j = 0; j < GrabList.Count; j++)
    //            {
    //                if (item.itemCount != 0)
    //                {
    //                    if (item.itemCode / 10 * 10 + i == GrabList[j].itemCode)
    //                    {
    //                        num++;
    //                    }
    //                }
    //            }
    //            if (num == item.itemGrade)
    //            {
    //                if (item.itemCode % 10 == 0)
    //                {
    //                    FinishList.Add(item);
    //                }
    //            }
    //        }
    //        num = 0;
    //    }
    //    return FinishList;
    //}
    public List<Item> GetFinishItem(List<Item> GrabList)
    {
        bool isTwokey = false;
        bool isThreekey = false;
        bool isFourkey = false;
        bool isFivekey = false;
        bool isSixkey = false;
        List<Item> FinishList = new List<Item>();
        int num = 0;
        foreach (Item item in GrabList)
        {

            for (int i = 0; i < item.itemGrade; i++)
            {
                for (int j = 0; j < GrabList.Count; j++)
                {
                    if (item.itemCount != 0)
                    {
                        if (item.itemCode % 10 == 0)
                        {
                            if (item.itemCode + i == GrabList[j].itemCode)
                            {
                                if (num == GrabList[j].itemCode % 10)
                                {
                                    num++;
                                }
                            }
                        }
                    }
                }
            }
            if (num == item.itemGrade)
            {
                for (int j = 0; j < GrabList.Count; j++)
                {
                    if (item.itemCount != 0 && GrabList[j].itemCount != 0)
                    {
                        if (item.itemCode + 1 == GrabList[j].itemCode)
                        {
                            isTwokey = true;
                        }
                        else if (item.itemCode + 2 == GrabList[j].itemCode)
                        {
                            isThreekey = true;
                        }
                        else if (item.itemCode + 3 == GrabList[j].itemCode)
                        {
                            isFourkey = true;
                        }
                        else if (item.itemCode + 4 == GrabList[j].itemCode)
                        {
                            isFivekey = true;
                        }
                        else if (item.itemCode + 5 == GrabList[j].itemCode)
                        {
                            isSixkey = true;
                        }
                    }
                }
                switch (item.itemGrade)
                {
                    case 3:
                        if (isTwokey != false && isThreekey != false)
                        {
                            FinishList.Add(item);
                        }
                        break;
                    case 4:
                        if (isTwokey != false && isThreekey != false && isFourkey != false)
                        {
                            FinishList.Add(item);
                        }
                        break;
                    case 5:
                        if (isTwokey != false && isThreekey != false && isFourkey != false && isFivekey != false)
                        {
                            FinishList.Add(item);
                        }
                        break;
                    case 6:
                        if (isTwokey != false && isThreekey != false && isFourkey != false && isFivekey != false && isSixkey != false)
                        {
                            FinishList.Add(item);
                        }
                        break;

                }
                isTwokey = false;
                isThreekey = false;
                isFourkey = false;
                isFivekey = false;
                isSixkey = false;
                num = 0;
            }
            //num = 0;
        }
        return FinishList;
    }


    /// <summary>
    /// 取得背包里所有宝物的list
    /// </summary>
    /// <returns></returns>
    public List<Item> GetAllBaoWuInBag()
    {
        List<Item> GrabList = new List<Item>();
        foreach (Item item in bagItemList)
        {
            if (item.itemCode > 50000 && item.itemCode < 60000 && item.itemCount > 0)
            {
                GrabList.Add(item);
            }
        }
        return GrabList;
    }

    /// <summary>
    /// 取得背包里的某个物品
    /// </summary>
    /// <returns></returns>
    public Item GetItemByID(int itemCode)
    {
        foreach (Item item in bagItemList)
        {
            if (item.itemCode == itemCode && item.itemCount > 0)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 背包物品数量减
    /// </summary>
    /// <returns></returns>
    public void SetItemCountReduceByID(int itemCode, int count)
    {
        int reduceCount = count;
        int i = 0;
        foreach (Item item in bagItemList)
        {
            if (item.itemCode == itemCode && item.itemCount > 0)
            {
                //Debug.LogError(12121212121);
                item.SetCount(reduceCount);
                if (item.itemCount <= 0)
                {
                    //  Debug.LogError(00000000000);
                    reduceCount = Mathf.Abs(item.itemCount);
                    item.SetCount(0);
                    //bagItemList.RemoveAt(i);
                    return;
                }
                else
                {
                    //  Debug.LogError(">>>>>>>>>>>00000");
                    return;
                }
            }
            i++;
        }
    }

    /// <summary>
    /// 背包物品数量加
    /// </summary>
    /// <returns></returns>
    public void SetItemCountAddByID(int itemCode, int count)
    {
        int addCount = count;
        int _stact = GetItemByItemCode(itemCode).Stact;
        //Debug.LogError(itemCode);
        //if (GetItemByID(itemCode) == null || itemCode.ToString()[0] == '5')
        if (GetItemByID(itemCode) == null || _stact == 1)
        {
            //Debug.LogError(itemCode + "************" + count);
            bagItemList.Add(new Item(itemCode, count));
        }
        else
        {
            Item addItem = GetItemByID(itemCode);
            //Debug.LogError(itemCode + "&&&&&&&&" + count);
            addItem.AddCount(count);
        }
    }

    /// <summary>
    /// 取得背包中某物品的数量
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public int GetItemCountByID(int itemCode)
    {
        int itemCount = 0;

        foreach (Item item in bagItemList)
        {
            if (item.itemCode == itemCode)
            {
                itemCount += item.itemCount;
            }
        }
        return itemCount;
    }

    /////////////////////背包相关(以上)/////////////////////
    #endregion


    #region 英雄天赋库
    public Dictionary<int, int> NowHeroUse = new Dictionary<int, int>();
    public Dictionary<int, int> HeroIannateNumDic = new Dictionary<int, int>();
    public Dictionary<int, string[]> HeroInnateDic = new Dictionary<int, string[]>();//人物天赋信息表
    public Dictionary<int, string> HeroInnateList = new Dictionary<int, string>();//人物天赋信息表

    public void AddHeroNum(int id, int num)
    {
        if (NowHeroUse.ContainsKey(id))
        {
            NowHeroUse.Remove(id);
        }
        NowHeroUse.Add(id, num);
    }

    public int GetNowNum(int HeroId)
    {
        if (NowHeroUse.ContainsKey(HeroId))
        {
            return NowHeroUse[HeroId];
        }
        return 0;
    }
    public void AddIannateNum(int HeroID, int inaateNum)
    {
        if (HeroIannateNumDic.ContainsKey(HeroID))
        {
            HeroIannateNumDic.Remove(HeroID);
        }
        HeroIannateNumDic.Add(HeroID, inaateNum);
    }

    public int GetNumByID(int HeroID)
    {
        if (HeroIannateNumDic.ContainsKey(HeroID))
        {
            return HeroIannateNumDic[HeroID];
        }
        return 0;
    }

    public void AddHeroDicv(int HeroID, string[] innates)
    {
        if (HeroInnateDic.ContainsKey(HeroID))
        {
            HeroInnateDic.Remove(HeroID);
            HeroInnateList.Remove(HeroID);
        }
        HeroInnateDic.Add(HeroID, innates);
        string Innate = "";
        for (int i = 0; i < innates.Length; i++)
        {
            Innate += (i + 1).ToString() + "$" + innates[i] + "!";
        }
        HeroInnateList.Add(HeroID, Innate);
    }


    /// <summary>
    /// 获得特定英雄的天赋列表
    /// </summary>
    /// <param name="HeroID"></param>
    /// <returns></returns>
    public string[] GetDowerByheroID(int HeroID)
    {
        if (HeroInnateDic.ContainsKey(HeroID))
        {
            return HeroInnateDic[HeroID];
        }
        return null;
    }
    #endregion
    #region 天赋表
    public List<Innates> InnatesList = new List<Innates>();
    public void AddInnate(Innates innates)
    {
        InnatesList.Add(innates);
    }
    /// <summary>
    /// 获得Innates place">位置
    /// </summary>
    /// <param name="place">位置</param>
    /// <param name="num">点数</param>
    /// <returns></returns>
    public Innates GetInnatesByTwo(int EffectType, int Seat)
    {
        for (int i = 0; i < InnatesList.Count; i++)
        {
            if (InnatesList[i].EffectType == EffectType && InnatesList[i].Seat == Seat)
            {
                return InnatesList[i];

            }
        }

        return null;
    }
    #endregion

    #region 天赋消耗
    public List<InnateExchange> innateExchange = new List<InnateExchange>();
    public void AddInnateExchangeList(InnateExchange inn)
    {
        innateExchange.Add(inn);
    }

    public InnateExchange GetInnaByTalent(int num)
    {
        for (int i = 0; i < innateExchange.Count; i++)
        {
            if (innateExchange[i].TalentPoint == num)
            {
                return innateExchange[i];
            }
        }
        return null;
    }
    #endregion

    #region Hero
    /// <summary>
    /// 队伍编队数据字符串 
    /// </summary>
    public string teamPositionDataString;

    ///////////////////////////英雄信息相关///////////////////////////
    public BetterList<HeroInfo> heroInfoList = new BetterList<HeroInfo>();

    public int HeadIndex = 0;      //头像滑动条位置

    public void AddHeroInfo(int heroID, int carrerType, string careerShow, string heroName, string heroCareer, string heroDes, string heroInfo, int heroScale, int heroBio, int heroAtkType, int heroDefType, int heroRace, int heroRarity, int heroState, int heroAtkFly, int heroToSort, int heroArea, int heroAi,
        int heroClass, int heroHP, int heroStrong, int heroIntell, int heroAgile, int heroPhyDef, int heroPhyRed, float heroDamigeAdd, float heroDamigeReduce, float heroHit, float heroNoHit, float heroCri, float heroNoCri, int heroMcritical, int heroSpeed, int heroMove,
        int Skill1, int Skill2, int Skill3, int Skill4, int newHeroPiece, string heroPetPhrase, string InForces, int powerSort, int Sex, int WeaponID, int _AtkScore, int _DefScore, int _HpScore, int _SkillScore)
    {
        heroInfoList.Add(new HeroInfo(heroID, carrerType, careerShow, heroName, heroCareer, heroDes, heroInfo, heroScale, heroBio, heroAtkType, heroDefType, heroRace, heroRarity, heroState, heroAtkFly, heroToSort, heroArea, heroAi, heroClass,
            heroHP, heroStrong, heroIntell, heroAgile, heroPhyDef, heroPhyRed, heroDamigeAdd, heroDamigeReduce, heroHit, heroNoHit, heroCri, heroNoCri, heroMcritical, heroSpeed, heroMove, Skill1, Skill2, Skill3, Skill4, newHeroPiece, heroPetPhrase, InForces, powerSort, Sex, WeaponID, _AtkScore, _DefScore, _HpScore, _SkillScore));
    }

    public HeroInfo GetHeroInfoByHeroID(int heroID)
    {
        foreach (HeroInfo hi in heroInfoList)
        {
            if (hi.heroID == heroID)
            {
                return hi;
            }
        }
        return null;
    }

    public BetterList<CareerInfo> heroCareerInfoList = new BetterList<CareerInfo>();

    public void AddCareerInfoList(CareerInfo _careerInfo)
    {
        heroCareerInfoList.Add(_careerInfo);
    }

    public CareerInfo GetHeroCareerByHeroToSort(int heroToSort)
    {
        foreach (var item in heroCareerInfoList)
        {
            if (item.CareerID == heroToSort)
            {
                return item;
            }
        }
        return null;
    }

    public string GetArmor(int Armor)
    {
        string ReturnArmor = "";
        switch (Armor)
        {
            case 1:
                ReturnArmor = "无甲";
                break;
            case 2:
                ReturnArmor = "轻甲";
                break;
            case 3:
                ReturnArmor = "重甲";
                break;
            case 4:
                ReturnArmor = "建筑";
                break;
            case 5:
                ReturnArmor = "特殊";
                break;
        }
        return ReturnArmor;
    }

    public string GetArmorInfo(int Armor)
    {
        string ReturnArmor = "";
        switch (Armor)
        {
            case 1:
                ReturnArmor = "克制[00ff00]特殊[-]\n被[ff0000]轻甲[-]克制";
                break;
            case 2:
                ReturnArmor = "克制[00ff00]无甲[-]\n被[ff0000]重甲[-]克制";
                break;
            case 3:
                ReturnArmor = "克制[00ff00]轻甲[-]\n被[ff0000]建筑[-]克制";
                break;
            case 4:
                ReturnArmor = "克制[00ff00]重甲[-]\n被[ff0000]特殊[-]克制";
                break;
            case 5:
                ReturnArmor = "克制[00ff00]建筑[-]\n被[ff0000]无甲[-]克制";
                break;
        }
        return ReturnArmor;
    }
    ////////////////////////////////////////////////////////////////
    #endregion

    #region Friend
    ///////////////////////////好友列表以下////////////////////////////
    public List<Friend> ListFriend = new List<Friend>();

    public class Friend
    {
        public int FriendCharacterID;
        public string FriendCharacterName;
        public int FriendRelation;
        public int FriendStatus;
        public int FriendRoyalty;
        public int FriendKind;
        public int FriendCamp;
        public int FriendLevel;
    }

    public void CreateFriend(int tFriendCharacterID, string tFriendCharacterName, int tFriendLevel, int tFriendRelation, int tFriendStatus, int tFriendRoyalty, int tFriendKind, int tFriendCamp)
    {
        Friend NewFriend = new Friend();
        NewFriend.FriendCharacterID = tFriendCharacterID;
        NewFriend.FriendCharacterName = tFriendCharacterName;
        NewFriend.FriendRelation = tFriendRelation;
        NewFriend.FriendStatus = tFriendStatus;
        NewFriend.FriendRoyalty = tFriendRoyalty;
        NewFriend.FriendKind = tFriendKind;
        NewFriend.FriendCamp = tFriendCamp;
        NewFriend.FriendLevel = tFriendLevel;
        ListFriend.Add(NewFriend);
        ListFriend.Sort(new CRoyalty());
        ListFriend.Sort(new CStatus());
    }

    public class CStatus : IComparer<Friend>
    {
        public int Compare(Friend x, Friend y)
        {
            return y.FriendStatus.CompareTo(x.FriendStatus);
        }

    }
    public class CRoyalty : IComparer<Friend>
    {
        public int Compare(Friend x, Friend y)
        {
            return x.FriendRoyalty.CompareTo(y.FriendRoyalty);
        }

    }
    ////////////////////////好友列表以上///////////////////////////////
    #endregion

    #region Instance
    /////////////////////副本相关(以下)/////////////////////
    public List<InstanceInfo> ListInstanceInfo = new List<InstanceInfo>();
    public class InstanceInfo
    {
        public int InstanceID;
        public string InstanceName;
        public string InstanceNote;
        public string InstanceGet1;
        public string InstanceGet2;
        public string InstanceGet3;
        public string InstanceGet4;
        public string InstanceGet5;
        public string InstanceGet6;
        public string InstanceGet7;
        public string InstanceGet8;
        public string InstanceGet9;
    }
    public void CreateInstanceInfo(int ID, string Name, string Note, string Get1, string Get2, string Get3, string Get4, string Get5, string Get6, string Get7, string Get8, string Get9)
    {
        InstanceInfo NewInfo = new InstanceInfo();
        NewInfo.InstanceID = ID;
        NewInfo.InstanceName = Name;
        NewInfo.InstanceNote = Note;
        NewInfo.InstanceGet1 = Get1;
        NewInfo.InstanceGet2 = Get2;
        NewInfo.InstanceGet3 = Get3;
        NewInfo.InstanceGet4 = Get4;
        NewInfo.InstanceGet5 = Get5;
        NewInfo.InstanceGet6 = Get6;
        NewInfo.InstanceGet7 = Get7;
        NewInfo.InstanceGet8 = Get8;
        NewInfo.InstanceGet9 = Get9;
        ListInstanceInfo.Add(NewInfo);
    }
    /////////////////////副本相关(以上)/////////////////////
    #endregion

    #region Skill
    /////////////////////技能相关(以下)/////////////////////

    public List<Skill> skilllist = new List<Skill>();

    public void AddSkillList(Skill s)
    {
        skilllist.Add(s);
    }

    public Skill GetSkillByID(int skillID, int level)
    {
        for (int i = 0; i < skilllist.Count; i++)
        {
            if (skilllist[i].skillID == skillID && skilllist[i].skillLevel == level)
            {
                return skilllist[i];
            }
        }
        return null;
    }
    public List<Skill> GetHeroSkillListByID(int skillID)
    {
        List<Skill> HeroSkilllist = new List<Skill>();
        for (int i = 0; i < skilllist.Count; i++)
        {
            if (skilllist[i].skillID == skillID)
            {
                HeroSkilllist.Add(skilllist[i]);
            }
        }
        return HeroSkilllist;
    }
    //所有手动技能
    public List<ManualSkill> ManualSkillList = new List<ManualSkill>();

    public void AddManualSkillList(ManualSkill s)
    {
        ManualSkillList.Add(s);
    }

    public ManualSkill GetManualSkillByID(int skillID)
    {
        for (int i = 0; i < ManualSkillList.Count; i++)
        {
            if (ManualSkillList[i].skillID == skillID)
            {
                return ManualSkillList[i];
            }
        }
        return null;
    }

    public ManualSkill GetManualSkillByType(int type)
    {
        for (int i = 0; i < ManualSkillList.Count; i++)
        {
            if (ManualSkillList[i].skillType == type)
            {
                return ManualSkillList[i];
            }
        }
        return null;
    }
    //我的手动技能
    public List<ManualSkill> MyManualSkillList = new List<ManualSkill>();
    public List<ManualSkill> GetMyManualSkillList(int Camp)
    {
        MyManualSkillList.Clear();
        for (int i = 0; i < ManualSkillList.Count; i++)
        {
            if (ManualSkillList[i].Camp == Camp)
            {
                MyManualSkillList.Add(ManualSkillList[i]);
            }
        }
        return MyManualSkillList;
    }

    /////////////////////技能相关(以上)/////////////////////
    #endregion


    #region Buff
    /////////////////////Buff相关(以下)/////////////////////
    public List<Buff> buffList = new List<Buff>();

    public void AddBuffList(Buff b)
    {
        buffList.Add(b);
    }

    public Buff GetBuffByID(int buffID)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].buffID == buffID)
            {
                return buffList[i];
            }
        }
        return null;
    }
    /////////////////////Buff相关(以上)/////////////////////
    #endregion

    #region NPCTactics
    /////////////////////NPCTactics相关(以下)/////////////////////
    public List<NPCTactics> NPCTacticsList = new List<NPCTactics>();

    public void AddNPCTacticsList(NPCTactics b)
    {
        NPCTacticsList.Add(b);
    }

    public NPCTactics GetNPCTacticsByID(int npcTacticsID)
    {
        for (int i = 0; i < NPCTacticsList.Count; i++)
        {
            if (NPCTacticsList[i].npcTacticsID == npcTacticsID)
            {
                return NPCTacticsList[i];
            }
        }
        return null;
    }

    public List<NPCTactics> GetNPCTacticsByGroupID(int groupID)
    {
        List<NPCTactics> ListNPCTactics = new List<NPCTactics>();
        for (int i = 0; i < NPCTacticsList.Count; i++)
        {
            if (NPCTacticsList[i].groupID == groupID)
            {
                ListNPCTactics.Add(NPCTacticsList[i]);
            }
        }
        return ListNPCTactics;
    }
    /////////////////////NPCTactics相关(以上)/////////////////////
    #endregion

    #region Gate
    /////////////////////关卡相关(以下)////////////////////
    public List<Gate> listGate = new List<Gate>();
    public class Gate
    {
        public int id;   //1 世界对话, 2 军团对话, 3 当前
        public string name;
        public int group;
        public int chapter;
        public int level;
        public int mapID;
        public int battleMapID;
        public string picture;
        public string description;
        public int scriptID1;
        public int scriptID2;
        public int scriptID3;
        public int staminaRequire;

        public int itemID1;
        public int itemID2;
        public int itemID3;
        public int itemID4;
        public int itemID5;
        public int itemID6;

        public string itemNum1;
        public string itemNum2;
        public string itemNum3;
        public string itemNum4;
        public string itemNum5;
        public string itemNum6;

        public int heroPiece;
        public int sweepCount;
        public int icon;
        public int bossID;
        public int needForce;
        public int needLevel;
        public string terrain;
        public int roleID;
        public int type;
        public int playerExpBonus;
        public int star;

        public Gate(int _id, string _name, int _group, int _chapter, int _level, string _picture, string _description, int _mapID, int _battleMapID, int _scriptID1, int _scriptID2, int _scriptID3, int _staminaRequire, int _itemID1, int _itemID2, int _itemID3, int _itemID4, int _itemID5, int _itemID6, int _heroPiece, int _sweepCoun, int _icon, int _bossID, int _needForce, string _terrain, int _roleID, int _type, int _needLevel, int _PlayerExpBonus, string _itemNum1, string _itemNum2, string _itemNum3, string _itemNum4, string _itemNum5, string _itemNum6)
        {
            id = _id;
            name = _name;
            group = _group;
            chapter = _chapter;
            level = _level;
            picture = _picture;
            mapID = _mapID;
            battleMapID = _battleMapID;
            star = 0;
            description = _description;
            scriptID1 = _scriptID1;
            scriptID2 = _scriptID2;
            scriptID3 = _scriptID3;

            staminaRequire = _staminaRequire;

            itemID1 = _itemID1;
            itemID2 = _itemID2;
            itemID3 = _itemID3;
            itemID4 = _itemID4;
            itemID5 = _itemID5;
            itemID6 = _itemID6;

            itemNum1 = _itemNum1;
            itemNum2 = _itemNum2;
            itemNum3 = _itemNum3;
            itemNum4 = _itemNum4;
            itemNum5 = _itemNum5;
            itemNum6 = _itemNum6;

            heroPiece = _heroPiece;
            sweepCount = _sweepCoun;
            icon = _icon;
            bossID = _bossID;
            needForce = _needForce;
            terrain = _terrain;
            type = _type;
            needLevel = _needLevel;
            playerExpBonus = _PlayerExpBonus;
            roleID = _roleID;
        }
    }

    public List<Gate> GetChapterGate(int _chapter)
    {
        List<Gate> ListChapterGate = new List<Gate>();
        foreach (Gate g in listGate)
        {
            if (g.chapter == _chapter)
            {
                ListChapterGate.Add(g);
            }
        }
        return ListChapterGate;
    }

    public List<Gate> GetGroupGate(int _group, int type)
    {
        List<Gate> ListGroupGate = new List<Gate>();
        foreach (Gate g in listGate)
        {
            if (g.group == _group && g.type == type)
            {
                ListGroupGate.Add(g);
            }
        }
        return ListGroupGate;
    }

    public int GetChapterMaxGate(int _chapter)
    {
        List<Gate> ListChapterGate = new List<Gate>();
        foreach (Gate g in listGate)
        {
            if (g.chapter == _chapter && g.id < 20000)
            {
                ListChapterGate.Add(g);
            }
        }
        return ListChapterGate[ListChapterGate.Count - 1].id;
    }

    public int GetChapterMaxLevel(int _chapter)
    {
        List<Gate> ListChapterGate = new List<Gate>();
        foreach (Gate g in listGate)
        {
            if (g.chapter == _chapter && g.id < 20000)
            {
                ListChapterGate.Add(g);
            }
        }
        return ListChapterGate[ListChapterGate.Count - 1].level;
    }

    public int GetGateCharterByGroupID(int _groupId)
    {
        foreach (Gate g in listGate)
        {
            if (g.group == _groupId && g.id < 20000)
            {
                return g.chapter;
            }
        }
        return 0;
    }

    public int GetMaxGateIDByGroupID(int _groupId)
    {
        List<Gate> ListChapterGate = new List<Gate>();
        foreach (Gate g in listGate)
        {
            if (g.group == _groupId && g.id < 20000)
            {
                ListChapterGate.Add(g);
            }
        }
        return ListChapterGate[ListChapterGate.Count - 1].id;
    }

    public void AddGate(int _id, string _name, int _group, int _chapter, int _level, string _picture, string _description, int _mapID, int _battleMapID, int _scriptID1, int _scriptID2, int _scriptID3, int _staminaRequire, int _itemID1, int _itemID2, int _itemID3, int _itemID4, int _itemID5, int _itemID6, int _heroPiece, int _sweepCoun, int _icon, int _bossID, int _needForce, string _terrain, int _roleID, int _type, int _needLevel, int _PlayerExpBonus, string _itemNum1, string _itemNum2, string _itemNum3, string _itemNum4, string _itemNum5, string _itemNum6)
    {
        Gate NewGate = new Gate(_id, _name, _group, _chapter, _level, _picture, _description, _mapID, _battleMapID, _scriptID1, _scriptID2, _scriptID3, _staminaRequire, _itemID1, _itemID2, _itemID3, _itemID4, _itemID5, _itemID6, _heroPiece, _sweepCoun, _icon, _bossID, _needForce, _terrain, _roleID, _type, _needLevel, _PlayerExpBonus, _itemNum1, _itemNum2, _itemNum3, _itemNum4, _itemNum5, _itemNum6);
        listGate.Add(NewGate);
    }

    public Gate GetGateByID(int id)
    {
        foreach (Gate g in listGate)
        {
            if (g.id == id)
            {
                return g;
            }
        }
        return null;
    }

    public List<Gate> GetGateByType(int type)
    {
        List<Gate> list = new List<Gate>();
        foreach (Gate g in listGate)
        {
            if (g.type == type)
            {
                list.Add(g);
            }
        }
        return list;
    }

    public Gate GetGateByTypeGroup(int type, int Group)
    {
        foreach (Gate g in listGate)
        {
            if (g.type == type && g.group == Group)
            {
                return g;
            }
        }
        return null;
    }

    public string GetGateDesByID(int id)
    {
        foreach (Gate g in listGate)
        {
            if (g.id == id)
            {
                return g.description;
            }
        }
        return "";
    }


    public List<int> GetGateMapList(int MaxId)
    {
        List<int> MapList = new List<int>();
        foreach (Gate g in listGate)
        {
            if (g.id < MaxId)
            {
                MapList.Add(g.id);
            }
        }
        return MapList;
    }
    /////////////////////关卡相关(以上)////////////////////
    #endregion

    #region Enemy
    /////////////////////怪物相关(以下)////////////////////
    public List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();
    public class EnemyInfo
    {
        public int enemyID;
        public int roleID;
        public string name;
        public int type;
        public int rank;

        public int lv;
        public int atk;
        public int hp;
        public int def;
        public float hit;
        public float noHit;
        public float crit;
        public float noCrit;
        public float dmgBonus;
        public float dmgReduce;
        public int area;
        public int ai;
        public int spd;
        public int mv;
        public int classNum;
        public float size;
        public int pic;
        public int bossAi;
        public int[] skill;
    }
    public void AddEnemy(int _enemyID, int _roleID, string _name, int _type, int _rank, int _lv, int _atk, int _hp, int _def, float _hit, float _noHit, float _crit, float _noCrit, float _dmgBonus, float _dmgReduce, int _bossAi)
    {
        EnemyInfo newEnemy = new EnemyInfo();
        newEnemy.enemyID = _enemyID;
        newEnemy.roleID = _roleID;
        newEnemy.pic = _roleID;
        newEnemy.name = _name;
        newEnemy.type = _type;
        newEnemy.rank = _rank;
        newEnemy.lv = _lv;
        newEnemy.atk = _atk;
        newEnemy.hp = _hp;
        newEnemy.def = _def;
        newEnemy.hit = _hit;
        newEnemy.noHit = _noHit;
        newEnemy.crit = _crit;
        newEnemy.noCrit = _noCrit;
        newEnemy.dmgBonus = _dmgBonus;
        newEnemy.dmgReduce = _dmgReduce;

        HeroInfo hi = GetHeroInfoByHeroID(_roleID);

        if (hi != null)
        {
            newEnemy.area = hi.heroArea;
            newEnemy.ai = hi.heroAi;
            newEnemy.spd = hi.heroSpeed;
            newEnemy.mv = hi.heroMove;
            newEnemy.size = hi.heroScale;
            newEnemy.bossAi = _bossAi;
            if (hi.heroSkillList.Count > 1)
            {
                newEnemy.skill = new int[6] { hi.heroSkillList[0], hi.heroSkillList[1], 0, 0, 0, 0 };
            }
            else
            {
                newEnemy.skill = new int[6] { 0, 0, 0, 0, 0, 0 };
            }
        }
        else
        {
            //Debug.LogError(_enemyID);
        }
        enemyInfoList.Add(newEnemy);
    }

    public EnemyInfo GetEnemyInfoByID(int _enemyID)
    {
        foreach (var e in enemyInfoList)
        {
            if (e.enemyID == _enemyID)
            {
                return e;
            }
        }
        return null;
    }
    /////////////////////怪物相关(以上)////////////////////
    #endregion

    #region BossAi
    /////////////////////怪物相关(以下)////////////////////
    public List<BossAi> bossAiList = new List<BossAi>();
    public class BossAi
    {
        public int bossAiId;

        public int stage1;
        public int trigger1;
        public int parameter01;
        public int skill1;
        public int action1;
        public string actionParameter1;
        public int model1;
        public int enemy1;
        public int size1;

        public int stage2;
        public int trigger2;
        public int parameter02;
        public int skill2;
        public int action2;
        public string actionParameter2;
        public int model2;
        public int enemy2;
        public int size2;

        public int stage3;
        public int trigger3;
        public int parameter03;
        public int skill3;
        public int action3;
        public string actionParameter3;
        public int model3;
        public int enemy3;
        public int size3;
    }
    public void AddBossAi(int _bossAiId, int _stage1, int _trigger1, int _parameter01, int _skill1, int _action1, string _actionParameter1, int _model1, int _enemy1, int _size1, int _stage2, int _trigger2, int _parameter02, int _skill2, int _action2, string _actionParameter2, int _model2, int _enemy2, int _size2, int _stage3, int _trigger3, int _parameter03, int _skill3, int _action3, string _actionParameter3, int _model3, int _enemy3, int _size3)
    {
        BossAi newBossAi = new BossAi();
        newBossAi.bossAiId = _bossAiId;

        newBossAi.stage1 = _stage1;
        newBossAi.trigger1 = _trigger1;
        newBossAi.parameter01 = _parameter01;
        newBossAi.skill1 = _skill1;
        newBossAi.action1 = _action1;
        newBossAi.actionParameter1 = _actionParameter1;
        newBossAi.model1 = _model1;
        newBossAi.enemy1 = _enemy1;
        newBossAi.size1 = _size1;

        newBossAi.stage2 = _stage2;
        newBossAi.trigger2 = _trigger2;
        newBossAi.parameter02 = _parameter02;
        newBossAi.skill2 = _skill2;
        newBossAi.action2 = _action2;
        newBossAi.actionParameter2 = _actionParameter2;
        newBossAi.model2 = _model2;
        newBossAi.enemy2 = _enemy2;
        newBossAi.size2 = _size2;

        newBossAi.stage3 = _stage3;
        newBossAi.trigger3 = _trigger3;
        newBossAi.parameter03 = _parameter03;
        newBossAi.skill3 = _skill3;
        newBossAi.action3 = _action3;
        newBossAi.actionParameter3 = _actionParameter3;
        newBossAi.model3 = _model3;
        newBossAi.enemy3 = _enemy3;
        newBossAi.size3 = _size3;

        bossAiList.Add(newBossAi);
    }

    public BossAi GetBossAi(int GetBossAiId)
    {
        foreach (BossAi ai in bossAiList)
        {
            if (ai.bossAiId == GetBossAiId)
            {
                return ai;
            }
        }
        return null;
    }
    /////////////////////怪物相关(以上)////////////////////
    #endregion

    #region Terrain
    /////////////////////地形相关(以下)////////////////////
    public List<TerrainInfo> terrainInfoList = new List<TerrainInfo>();
    public class TerrainInfo
    {
        public int terrainID;
        public string name;
        public string terrainEffect;
        public int buff;
        public int icon;
    }
    public void AddTerrain(int _terrainID, string _name, string _terrainEffect, int _buff, int _icon)
    {
        TerrainInfo newTerrain = new TerrainInfo();
        newTerrain.terrainID = _terrainID;
        newTerrain.name = _name;
        newTerrain.terrainEffect = _terrainEffect;
        newTerrain.buff = _buff;
        newTerrain.icon = _icon;
        terrainInfoList.Add(newTerrain);
    }
    public TerrainInfo GetTerrainInfoByID(int TerrainID)
    {
        foreach (TerrainInfo t in terrainInfoList)
        {
            if (t.terrainID == TerrainID)
            {
                return t;
            }
        }
        return null;
    }
    /////////////////////地形相关(以上)////////////////////
    #endregion

    #region Mail
    //////////////////// 邮件相关 ////////////////////////
    public BetterList<Mail> mailList = new BetterList<Mail>();

    public bool isMailWindowOpen = false;

    public void AddMailList(Mail mail)
    {
        mailList.Add(mail);
    }

    /////////////////////////////////////////////////////
    #endregion

    #region Chat
    public BetterList<ChatItemData> ChatItemDataList = new BetterList<ChatItemData>();
    public void AddChatItemData(ChatItemData _ChatItemData)
    {
        ChatItemDataList.Add(_ChatItemData);
    }

    public ChatItemData ChatItemOnTeamCopy = null;//组队或者军团邀请最新一条;


    /* public List<ChatItemData> GetChatItemDataListByID(int channel)
     {
         if (ChatItemDataDic.ContainsKey(channel))
         {
             ChatItemDataList.Add(ChatItemDataDic[channel].);
         }
         return ChatItemDataList;
     }*/
    #endregion

    #region Achievement
    ///////////////////// 成就相关 ///////////////////////
    public BetterList<Achievement> achievementList = new BetterList<Achievement>();

    public void AddAchievementList(Achievement achievement)
    {
        achievementList.Add(achievement);
    }
    public Achievement GetAchievementByID(int _AchievementID)
    {
        foreach (var item in TextTranslator.instance.achievementList)
        {
            if (item.achievementID == _AchievementID)
            {
                return item;
            }
        }
        return null;
    }

    public void SetAchievementInfo(int AchieveType, string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (AchieveType == 1)
        {
            for (int i = 1; i < dataSplit.Length - 1; i++)
            {
                string[] secSplit = dataSplit[i].Split('$');
                int _id = int.Parse(secSplit[0]);
                int _completeCount = int.Parse(secSplit[1]);
                int _totleCount = int.Parse(secSplit[2]);
                int _gotState = int.Parse(secSplit[3]);
                Achievement _CurAchievement = GetAchievementByID(_id);//TextTranslator.instance.GetAchievementByID(_id);
                _CurAchievement.SetCompletCount(_completeCount, _totleCount, _gotState, _CurAchievement.Type);
            }
        }
        else if (AchieveType == 2)
        {
            for (int i = 1; i < dataSplit.Length - 1; i++)
            {
                string[] secSplit = dataSplit[i].Split('$');
                int _id = int.Parse(secSplit[0]);
                int _gotState = int.Parse(secSplit[1]);
                Achievement _CurAchievement = GetAchievementByID(_id);//TextTranslator.instance.GetAchievementByID(_id);
                //_CurAchievement.SetCompletState(_gotState);
                _CurAchievement.SetCompletCount(int.Parse(secSplit[2]), _CurAchievement.totalCount, _gotState, _CurAchievement.Type);
            }
        }
    }
    public int AchievementComplete()
    {
        int CompleteNum = 0;
        int GateId = CharacterRecorder.instance.lastGateID - 10000;
        foreach (Achievement Ach in achievementList)
        {
            //Ach.completeState == 1&&Ach.achievementID<417&&CharacterRecorder.instance.level>=Ach.OpenLevel&&GateId>=Ach.OpenGate
            if (Ach.completeState == 1 && Ach.achievementName != "精益求精" && CharacterRecorder.instance.level >= Ach.OpenLevel && GateId >= Ach.OpenGate)
            {
                CompleteNum += 1;
                break;
            }
            else if (Ach.completeState == 1 && Ach.achievementName == "精益求精" && Ach.preAchievement == 0) //Ach.completeState == 1 && Ach.achievementID == 417
            {
                CompleteNum += 1;
                break;
            }//Ach.completeState == 1 && Ach.achievementID > 188 && TextTranslator.instance.GetAchievementByID(Ach.preAchievement).completeState == 2
            else if (Ach.completeState == 1 && Ach.achievementName == "精益求精" && TextTranslator.instance.GetAchievementByID(Ach.preAchievement).completeState == 2)
            {
                CompleteNum += 1;
                break;
            }
        }
        return CompleteNum;
    }
    public int TaskCompleteNum()
    {
        int CompleteNum = 0;
        //int GateId = CharacterRecorder.instance.lastGateID - 10000;
        foreach (Achievement Ach in achievementList)
        {
            if (Ach.resetTime == 1 && Ach.completeState == 1)
            {
                CompleteNum += 1;
                break;
            }
        }
        return CompleteNum;
    }

    public int AchievementCompleteNum()
    {
        int CompleteNum = 0;
        //int GateId = CharacterRecorder.instance.lastGateID - 10000;
        foreach (Achievement Ach in achievementList)
        {
            if (Ach.resetTime == 0)
            {
                if (Ach.completeState == 1 && Ach.achievementName != "精益求精")
                {
                    CompleteNum += 1;
                    break;
                }
                else if (Ach.completeState == 1 && Ach.achievementName == "精益求精" && Ach.preAchievement == 0)
                {
                    CompleteNum += 1;
                    break;
                }
                else if (Ach.completeState == 1 && Ach.achievementName == "精益求精" && TextTranslator.instance.GetAchievementByID(Ach.preAchievement).completeState == 2)
                {
                    CompleteNum += 1;
                    break;
                }
            }
        }
        return CompleteNum;
    }
    ////////////////////////////////////////////////////
    #endregion
    #region Sign
    ///////////////////// 签到相关 ///////////////////////
    public BetterList<Sign> signList = new BetterList<Sign>();
    public BetterList<Sign> signPerMonthList = new BetterList<Sign>();
    public void AddSignList(Sign sign)
    {
        signList.Add(sign);
    }
    public int GetEachSignState(int targetSignId, int _signID, int _signState)
    {
        if (targetSignId < _signID)
        {
            return 2;//已签
        }
        else if (targetSignId == _signID)
        {
            switch (_signState)
            {
                case 0: return 1;//可签
                case 1: return 2;//已签
                //default: Debug.LogError("参数错误"); return 0;//不可签break;
            }
        }
        return 0;//不可签
    }
    public BetterList<Sign> GetSignPerMonthListBySignID(int _signID)
    {
        signPerMonthList.Clear();
        int startIndex = 0 + _signID / 30 * 30;
        int endIndex = 29 + _signID / 30 * 30;
        for (int i = startIndex; i <= endIndex; i++)
        {
            if (i < signList.size)
            {
                signPerMonthList.Add(signList[i]);
            }
        }
        return signPerMonthList;
    }
    public Sign GetSignBySignID(int _signID)
    {
        for (int i = 0; i < signPerMonthList.size; i++)
        {
            if (signPerMonthList[i].signID == _signID)
            {
                return signPerMonthList[i];
            }
        }
        return null;
    }
    ////////////////////////////////////////////////////
    #endregion

    #region SignExtra
    ///////////////////// 签到累计相关 ///////////////////////
    public BetterList<SignExtra> signExtraList = new BetterList<SignExtra>();
    public BetterList<SignExtra> signExtraPerPageList = new BetterList<SignExtra>();
    public bool signExtraCanGetState = false;
    public int SignExtraIDHadGet = 0;
    public int SignExtraNumForRight = 0;//累计签到 分母对应的层SignExtraID
    public int SignExtraRealNumForRight = 0;//累计签到 分母数字
    public void AddSignExtraList(SignExtra signExtra)
    {
        signExtraList.Add(signExtra);
    }
    public BetterList<SignExtra> GetSignExtraPerPageListBySignExtraID(int _signId, int _state, int _SignExtraID1, int _SignExtraID2)
    {
        signExtraPerPageList.Clear();
        int indexToShow = _SignExtraID1 + 1;
        if (indexToShow < signExtraList.size)
        {
            for (int j = 0; j < signExtraList.size; j++)
            {
                if (signExtraList[j].SignExtraID == indexToShow)
                {
                    signExtraPerPageList.Add(signExtraList[j]);
                }
            }
        }
        if (_SignExtraID1 == _SignExtraID2)
        {
            signExtraCanGetState = false;
            SignExtraNumForRight = _SignExtraID1 + 1;
            SignExtraRealNumForRight = GetSignExtraBySignExtraID(SignExtraNumForRight).Day;
        }
        else if (_SignExtraID1 < _SignExtraID2)
        {
            SignExtraNumForRight = _SignExtraID1 + 1;
            SignExtraRealNumForRight = GetSignExtraBySignExtraID(SignExtraNumForRight).Day;
            if (GetEachSignState(_SignExtraID1 + 1, _signId, _state) == 2)
            {
                signExtraCanGetState = true;
            }
        }
        else
        {
            Debug.Log("不可能的情况");
        }
        return signExtraPerPageList;
    }
    /*  public BetterList<SignExtra> GetSignExtraPerPageListBySignExtraID(int _SignExtraID1, int _SignExtraID2)
      {
          signExtraPerPageList.Clear();
          if (_SignExtraID1 == _SignExtraID2)
          {
              signExtraHadGet = true;
              SignExtraNumForRight = _SignExtraID1;
              for (int j = 0; j < signExtraList.size; j++)
              {
                  if (signExtraList[j].SignExtraID == _SignExtraID1)
                  {
                      signExtraPerPageList.Add(signExtraList[j]);
                  }
              }
          }
          else
          {
              int startIndex = _SignExtraID1 + 1;
              int endIndex = _SignExtraID2;
              SignExtraNumForRight = startIndex;
              for (int i = startIndex; i <= endIndex; i++)
              {
                  for (int j = 0; j < signExtraList.size; j++)
                  {
                      if (signExtraList[j].SignExtraID == i)
                      {
                          signExtraPerPageList.Add(signExtraList[j]);
                      }
                  }
              }
          }
          return signExtraPerPageList;
      } */
    public SignExtra GetSignExtraBySignExtraID(int _signID)
    {
        for (int i = 0; i < signExtraList.size; i++)
        {
            if (signExtraList[i].SignExtraID == _signID)
            {
                return signExtraList[i];
            }
        }
        return null;
    }
    ////////////////////////////////////////////////////
    #endregion

    #region ActivitySevenLogin
    ///////////////////// 登入签到相关 ///////////////////////
    public BetterList<ActivitySevenLogin> ActivitySevenLoginList = new BetterList<ActivitySevenLogin>();
    public void AddActivitySevenLoginList(ActivitySevenLogin _ActivitySevenLogin)
    {
        ActivitySevenLoginList.Add(_ActivitySevenLogin);
    }
    public ActivitySevenLogin GetActivitySevenLoginByDay(int _Day)
    {
        for (int i = 0; i < ActivitySevenLoginList.size; i++)
        {
            if (_Day == ActivitySevenLoginList[i].Day)
            {
                return ActivitySevenLoginList[i];
            }
        }
        return null;
    }
    #endregion

    #region LegionTrain
    ///////////////////// 军团训练场相关 ///////////////////////
    public List<LegionTrain> LegionTrainList = new List<LegionTrain>();//军团训练场
    public void AddLegionTrainList(LegionTrain _ActivitySevenLogin)
    {
        LegionTrainList.Add(_ActivitySevenLogin);
    }
    public LegionTrain GetLegionTrainByID(int _TrainID)
    {
        for (int i = 0; i < LegionTrainList.Count; i++)
        {
            if (_TrainID == LegionTrainList[i].TrainID)
            {
                return LegionTrainList[i];
            }
        }
        return null;
    }
    #endregion

    #region Legion
    ///////////////////// 军团捐献相关 ///////////////////////
    public BetterList<Legion> LegionList = new BetterList<Legion>();
    public void AddLegionList(Legion _ActivitySevenLogin)
    {
        LegionList.Add(_ActivitySevenLogin);
    }
    public Legion GetLegionByID(int _TrainID)
    {
        for (int i = 0; i < LegionList.size; i++)
        {
            if (_TrainID == LegionList[i].Level)
            {
                return LegionList[i];
            }
        }
        return null;
    }
    public Legion GetLegionByNeedExp(int _NeedExp)
    {
        for (int i = 0; i < LegionList.size; i++)
        {
            if (_NeedExp == LegionList[i].NeedExp)
            {
                return LegionList[i];
            }
        }
        return null;
    }
    #endregion

    #region LegionTask
    ///////////////////// 军团任务相关 ///////////////////////
    public BetterList<LegionTask> LegionTaskList = new BetterList<LegionTask>();
    public void AddLegionTaskList(LegionTask _ActivitySevenLogin)
    {
        LegionTaskList.Add(_ActivitySevenLogin);
    }
    public LegionTask GetLegionTaskByID(int _TrainID)
    {
        for (int i = 0; i < LegionTaskList.size; i++)
        {
            if (_TrainID == LegionTaskList[i].LegionTaskID)
            {
                return LegionTaskList[i];
            }
        }
        return null;
    }
    #endregion

    #region LegionRank
    ///////////////////// 军团副本伤害奖励 ///////////////////////
    public BetterList<LegionRank> LegionRankList = new BetterList<LegionRank>();
    public void AddLegionRankList(LegionRank _ActivitySevenLogin)
    {
        LegionRankList.Add(_ActivitySevenLogin);
    }
    public LegionRank GetLegionRankByRankNum(int _TrainID)
    {
        for (int i = 0; i < LegionRankList.size; i++)
        {
            if (_TrainID == LegionRankList[i].RankNum)
            {
                return LegionRankList[i];
            }
        }
        return null;
    }
    #endregion

    #region LegionFirstPass
    ///////////////////// 军团副本通关奖励 ///////////////////////
    public BetterList<LegionRank> LegionFirstPassList = new BetterList<LegionRank>();
    public void AddLegionFirstPass(LegionRank _ActivitySevenLogin)
    {
        LegionFirstPassList.Add(_ActivitySevenLogin);
    }
    public LegionRank GetLegionFirstPassByRankNum(int _TrainID)
    {
        for (int i = 0; i < LegionFirstPassList.size; i++)
        {
            if (_TrainID == LegionFirstPassList[i].RankNum)
            {
                return LegionFirstPassList[i];
            }
        }
        return null;
    }
    public LegionRank GetLegionFirstPassByRankID(int _TrainID)
    {
        for (int i = 0; i < LegionFirstPassList.size; i++)
        {
            if (_TrainID == LegionFirstPassList[i].RankID)
            {
                return LegionFirstPassList[i];
            }
        }
        return null;
    }
    #endregion

    #region LegionGate
    ///////////////////// 军团副本相关 ///////////////////////
    public BetterList<LegionGate> LegionGateList = new BetterList<LegionGate>();
    public BetterList<LegionGate> LefionSmallGateList = new BetterList<LegionGate>();
    public BetterList<int> LeginGateGroupList = new BetterList<int>();
    public void AddLegionGateList(LegionGate _ActivitySevenLogin)
    {
        LegionGateList.Add(_ActivitySevenLogin);
        if (!LeginGateGroupList.Contains(_ActivitySevenLogin.GateGroupID))
        {
            LeginGateGroupList.Add(_ActivitySevenLogin.GateGroupID);
        }
    }
    public LegionGate GetLegionGateByID(int _TrainID)
    {
        for (int i = 0; i < LegionGateList.size; i++)
        {
            if (_TrainID == LegionGateList[i].LegionGateID)
            {
                return LegionGateList[i];
            }
        }
        return null;
    }
    public LegionGate GetLegionGateByGroupIDAndBoxID(int _GroupID, int GateBoxID)
    {
        for (int i = 0; i < LegionGateList.size; i++)
        {
            if (_GroupID == LegionGateList[i].GateGroupID && GateBoxID == LegionGateList[i].GateBoxID)
            {
                return LegionGateList[i];
            }
        }
        return null;
    }
    public BetterList<LegionGate> GetLegionSmallGateListByGateGroupID(int _GateGroupID)
    {
        LefionSmallGateList.Clear();
        for (int i = 0; i < LegionGateList.size; i++)
        {
            if (_GateGroupID == LegionGateList[i].GateGroupID)
            {
                LefionSmallGateList.Add(LegionGateList[i]);
            }
        }
        return LefionSmallGateList;
    }
    #endregion

    #region LegionGateBox
    ///////////////////// 军团副本相关 ///////////////////////
    public List<LegionGateBox> LegionGateBoxList = new List<LegionGateBox>();
    public void AddLegionGateBoxList(LegionGateBox _ActivitySevenLogin)
    {
        LegionGateBoxList.Add(_ActivitySevenLogin);
    }
    public LegionGateBox GetLegionGateBoxByID(int _TrainID)
    {
        for (int i = 0; i < LegionGateBoxList.Count; i++)
        {
            if (_TrainID == LegionGateBoxList[i].LegionGateBoxID)
            {
                return LegionGateBoxList[i];
            }
        }
        return null;
    }
    public List<LegionGateBox> GetLegionGateBoxByGateBoxID(int _GateBoxID)
    {
        List<LegionGateBox> _myLegionGateBoxList = new List<LegionGateBox>();
        for (int i = 0; i < LegionGateBoxList.Count; i++)
        {
            if (_GateBoxID == LegionGateBoxList[i].GateBoxID)
            {
                _myLegionGateBoxList.Add(LegionGateBoxList[i]);
            }
        }
        return _myLegionGateBoxList;
    }
    #endregion

    #region LabsLimit
    ///////////////////// 改造实验室 ///////////////////////
    public int roleType = 1;//// 改造实验室类型
    public BetterList<LabsLimit> LabsLimitList = new BetterList<LabsLimit>();
    public BetterList<ReformLabItemData> OneTypeLabsLimitList = new BetterList<ReformLabItemData>();
    public void AddLabsLimitList(LabsLimit _ActivitySevenLogin)
    {
        LabsLimitList.Add(_ActivitySevenLogin);
    }
    public LabsLimit GetLabsLimitByID(int _type)
    {
        for (int i = 0; i < LabsLimitList.size; i++)
        {
            if (_type == LabsLimitList[i].RoleType)
            {
                return LabsLimitList[i];
            }
        }
        return null;
    }
    public BetterList<ReformLabItemData> GetOneTypeLabsLimitByRoleType(int _type)
    {
        OneTypeLabsLimitList.Clear();
        LabsLimit _targetLabsLimit = GetLabsLimitByID(_type);
        for (int i = 0; i < _targetLabsLimit.LabItemDataList.size; i++)
        {
            OneTypeLabsLimitList.Add(_targetLabsLimit.LabItemDataList[i]);
        }
        return OneTypeLabsLimitList;
    }
    public ReformLabItemData GetOneLabsItemByRoleTypeAndPosNum(int _type, int _posNum)
    {
        LabsLimit _targetLabsLimit = GetLabsLimitByID(_type);
        for (int i = 0; i < _targetLabsLimit.LabItemDataList.size; i++)
        {
            if (_targetLabsLimit.LabItemDataList[i].LabItemPosNum == _posNum)
            {
                return _targetLabsLimit.LabItemDataList[i];
            }
        }
        return null;
    }
    #endregion

    #region LabsPoint
    ///////////////////// 改造实验室积分 ///////////////////////
    public BetterList<LabsPoint> LabsPointList = new BetterList<LabsPoint>();
    public void AddLabsPointList(LabsPoint _ActivitySevenLogin)
    {
        LabsPointList.Add(_ActivitySevenLogin);
    }
    public LabsPoint GetLabsPointByID(int LabsPointType)
    {
        for (int i = 0; i < LabsPointList.size; i++)
        {
            if (LabsPointType == LabsPointList[i].LabsPointType)
            {
                return LabsPointList[i];
            }
        }
        return null;
    }

    #endregion

    #region Question
    ///////////////////// 问卷 ///////////////////////
    public BetterList<Question> QuestionList = new BetterList<Question>();
    public void AddQuestionList(Question _ActivitySevenLogin)
    {
        QuestionList.Add(_ActivitySevenLogin);
    }
    public Question GetQuestionByID(int QuestionID)
    {
        for (int i = 0; i < QuestionList.size; i++)
        {
            if (QuestionID == QuestionList[i].QuestionID)
            {
                return QuestionList[i];
            }
        }
        return null;
    }
    #endregion

    #region ActivitySevenDay
    ///////////////////// 七日活动表 ///////////////////////
    public Dictionary<int, ActivitySevenDay> ActivitySevenDayDic = new Dictionary<int, ActivitySevenDay>();
    public void AddActivitySevenDay(int id, ActivitySevenDay _ActivitySevenDay)
    {
        ActivitySevenDayDic.Add(id, _ActivitySevenDay);
    }
    public ActivitySevenDay GetActivitySevenDayByID(int id)
    {
        if (ActivitySevenDayDic.ContainsKey(id))
        {
            return ActivitySevenDayDic[id];
        }
        return null;
    }
    public void SetActivitySevenInfo(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        for (int i = 2; i < dataSplit.Length - 2; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            int _id = int.Parse(secSplit[0]);
            int _gotState = int.Parse(secSplit[1]);
            int _completeCount = int.Parse(secSplit[2]);
            ActivitySevenDay _ActivitySevenDay = GetActivitySevenDayByID(_id);
            _ActivitySevenDay.SetCompletState(_gotState, _completeCount);
        }
    }
    public int SevenDayCompleteNum() //可领取数量，红点用
    {
        int CompleteNum = 0;
        Dictionary<int, ActivitySevenDay>.ValueCollection valueColl = TextTranslator.instance.ActivitySevenDayDic.Values;
        foreach (ActivitySevenDay _Act in valueColl)
        {
            if (_Act.CompleteState == 1)
            {
                CompleteNum += 1;
            }
        }
        return CompleteNum;
    }
    #endregion

    #region ActivitySevenRank
    ///////////////////// 七日排行表 ///////////////////////
    public BetterList<ActivitySevenRank> ActivitySevenRankList = new BetterList<ActivitySevenRank>();
    public void AddActivitySevenRank(ActivitySevenRank _ActivitySevenRank)
    {
        ActivitySevenRankList.Add(_ActivitySevenRank);
    }
    public ActivitySevenRank GetActivitySevenRankByID(int id)
    {
        for (int i = 0; i < ActivitySevenRankList.size; i++)
        {
            if (ActivitySevenRankList[i].ActivitySevenRankID == id)
            {
                return ActivitySevenRankList[i];
            }
        }
        return null;
    }
    #endregion

    #region ActivitySevenDay
    ///////////////////// 七日英雄表 ///////////////////////
    public BetterList<ActivitySevenHero> ActivitySevenHeroList = new BetterList<ActivitySevenHero>();
    public void AddActivitySevenHero(ActivitySevenHero _ActivitySevenHero)
    {
        ActivitySevenHeroList.Add(_ActivitySevenHero);
    }
    public ActivitySevenHero GetActivitySevenHeroByID(int id)
    {
        for (int i = 0; i < ActivitySevenHeroList.size; i++)
        {
            if (ActivitySevenHeroList[i].ActivitySevenHeroID == id)
            {
                return ActivitySevenHeroList[i];
            }
        }
        return null;
    }
    #endregion

    #region ActivityHalfLimitBuy
    ///////////////////// 七日折扣 ///////////////////////
    public Dictionary<int, ActivityHalfLimitBuy> ActivityHalfLimitBuyDic = new Dictionary<int, ActivityHalfLimitBuy>();
    public void AddActivityHalfLimitBuy(int id, ActivityHalfLimitBuy ActivityHalfLimitBuy)
    {
        ActivityHalfLimitBuyDic.Add(id, ActivityHalfLimitBuy);
    }
    public ActivityHalfLimitBuy GetActivityHalfLimitBuyByID(int id)
    {
        if (ActivityHalfLimitBuyDic.ContainsKey(id))
        {
            return ActivityHalfLimitBuyDic[id];
        }
        return null;
    }
    #endregion

    #region NewGuide
    ///////////////////// NewGuide等级开启相关 ///////////////////////
    public BetterList<NewGuide> newGuideList = new BetterList<NewGuide>();
    public BetterList<NewGuide> newGuideWillOpenList = new BetterList<NewGuide>();
    public void AddNewGuideList(NewGuide _NewGuide)
    {
        newGuideList.Add(_NewGuide);
    }

    public NewGuide GetGuildByType(int type)
    {
        for (var i = 0; i < newGuideList.size; i++)
        {
            if (newGuideList[i].NewGuideID == type)
            {
                return newGuideList[i];
            }
        }

        return null;
    }

    public BetterList<NewGuide> FindNewGuideWillOpenListByCurLevel(int myLevel)
    {
        if (newGuideWillOpenList == null)
        {
            return null;
        }
        newGuideWillOpenList.Clear();
        List<int> BigList = new List<int>();
        //int startIndex = GetStartIndexByCurLevel(myLevel);
        foreach (NewGuide gatelist in newGuideList)
        {
            if (gatelist.Level > myLevel)
            {
                BigList.Add(gatelist.Level);
            }
        }
        BigList.Sort();
        if (BigList.Count > 0)
        {
            if (FindNewGuideNowOpenByCurLevel(BigList[0]) != null)
            {
                newGuideWillOpenList.Add(FindNewGuideNowOpenByCurLevel(BigList[0]));
            }
        }
        if (BigList.Count > 1)
        {
            if ((FindNewGuideNowOpenByCurLevel(BigList[1])) != null)
            {
                newGuideWillOpenList.Add(FindNewGuideNowOpenByCurLevel(BigList[1]));
            }
        }
        if (BigList.Count > 2)
        {
            if ((FindNewGuideNowOpenByCurLevel(BigList[2])) != null)
            {
                newGuideWillOpenList.Add(FindNewGuideNowOpenByCurLevel(BigList[2]));
            }
        }
        return newGuideWillOpenList;
    }
    public NewGuide FindNewGuideNextWillOpenByCurLevel(int myLevel)
    {
        List<int> BigList = new List<int>();
        foreach (NewGuide gatelist in newGuideList)
        {
            if (gatelist.Level > myLevel)
            {
                BigList.Add(gatelist.Level);
            }
        }
        if (BigList.Count != 0)
        {
            BigList.Sort();
            if (myLevel < newGuideList[0].Level)
            {
                return newGuideList[0];
            }
            else
            {
                if (BigList.Count >= 0)
                {
                    return FindNewGuideNowOpenByCurLevel(BigList[0]);
                }

            }
        }
        return null;
    }

    public NewGuide FindNewGuideNowOpenByCurLevel(int myLevel)
    {
        for (int i = 0; i < newGuideList.size; i++)
        {
            if (newGuideList[i].Level == myLevel)
            {
                return newGuideList[i];
            }
        }
        return null;
    }
    int GetStartIndexByCurLevel(int myLevel)
    {
        //int startIndex = 0;
        //Debug.LogError("myLevel.." + myLevel + ".....newGuideList[0].Level....." + newGuideList[0].Level);
        if (myLevel <= newGuideList[0].Level)
        {
            return 0;
        }
        if (myLevel == newGuideList[newGuideList.size - 1].Level)
        {
            return newGuideList.size - 1;
        }
        if (myLevel > newGuideList[newGuideList.size - 1].Level)
        {
            return -1;
        }
        for (int i = 0; i < newGuideList.size; i++)
        {
            if (newGuideList[i].Level >= myLevel)
            {
                return i;
            }
        }
        return -1;
    }
    ////////////////////////////////////////////////////
    #endregion

    #region FightMotion
    public Dictionary<int, FightMotion> fightMotionDic = new Dictionary<int, FightMotion>();

    public void AddFightMotion(int id, FightMotion fm)
    {
        fightMotionDic.Add(id, fm);
    }
    #endregion

    #region FightProjectile
    public Dictionary<int, FightProjectile> fightProjectileDic = new Dictionary<int, FightProjectile>();

    public void AddFightProjectile(int id, FightProjectile fp)
    {
        fightProjectileDic.Add(id, fp);
    }
    #endregion




    #region FightEffect
    public Dictionary<int, FightEffect> fightEffectDic = new Dictionary<int, FightEffect>();

    public void AddFightEffect(int id, FightEffect fe)
    {
        fightEffectDic.Add(id, fe);
    }
    #endregion

    #region FightCamera
    public Dictionary<int, FightCamera> fightCameraDic = new Dictionary<int, FightCamera>();

    public void AddFightCamera(int id, FightCamera fc)
    {
        fightCameraDic.Add(id, fc);
    }
    #endregion

    #region BattleMap
    public Dictionary<int, BattleMap> battleMapDic = new Dictionary<int, BattleMap>();

    public void AddBattleMap(int id, BattleMap bm)
    {
        battleMapDic.Add(id, bm);
    }
    #endregion

    #region Chapter
    public List<GateChapter> listChapter = new List<GateChapter>();

    public void AddChapter(GateChapter gc)
    {
        listChapter.Add(gc);
    }
    #endregion

    #region GateCompleteBox
    public List<GateCompleteBox> listGateCompleteBox = new List<GateCompleteBox>();

    public void AddGateCompleteBox(GateCompleteBox gc)
    {
        listGateCompleteBox.Add(gc);
    }

    public GateCompleteBox GetGateCompleteBox(int GroupId)
    {
        foreach (GateCompleteBox gcb in listGateCompleteBox)
        {
            if (gcb.GroupID == GroupId)
            {
                return gcb;
            }
        }
        return null;
    }
    public int GetGateCompleteBoxGroup(int id)
    {
        foreach (GateCompleteBox gcb in listGateCompleteBox)
        {
            if (gcb.GateCompleteBoxID == id)
            {
                return gcb.GroupID;
            }
        }
        return 0;
    }
    #endregion

    #region GateRankBox
    public List<GateRankBox> listGateRankBox = new List<GateRankBox>();

    public void AddGateRankBox(GateRankBox gc)
    {
        listGateRankBox.Add(gc);
    }

    public GateRankBox GetGateRankBox(int GroupId, int GateType)
    {
        foreach (GateRankBox gcb in listGateRankBox)
        {
            if (gcb.GroupID == GroupId && gcb.GateType == GateType)
            {
                return gcb;
            }
        }
        return null;
    }
    #endregion

    #region GateLimit
    public Dictionary<int, GateLimit> GateLimitDic = new Dictionary<int, GateLimit>();
    public void AddGateLimitDic(int id, GateLimit GateLimit)
    {
        GateLimitDic.Add(id, GateLimit);
    }
    public GateLimit GetGateLimitDicByID(int id)
    {
        if (GateLimitDic.ContainsKey(id))
        {
            return GateLimitDic[id];
        }
        return null;
    }
    #endregion

    #region RoleRankNeed
    public List<RoleRankNeedInfo> listRoleRankNeed = new List<RoleRankNeedInfo>();

    public void AddRoleRankNeed(RoleRankNeedInfo rrni)
    {
        listRoleRankNeed.Add(rrni);
    }

    public RoleRankNeedInfo GetRoleRankNeedByRankLevel(int rankLevel)
    {
        for (int i = 0; i < listRoleRankNeed.Count; i++)
        {
            if (listRoleRankNeed[i].heroRank == rankLevel)
            {
                return listRoleRankNeed[i];
            }
        }
        return null;
    }

    #endregion

    #region RoleClassUp
    public BetterList<RoleClassUp> RoleClassUpList = new BetterList<RoleClassUp>();

    public void AddRoleClassUp(RoleClassUp rcu)
    {
        RoleClassUpList.Add(rcu);
    }

    public RoleClassUp GetRoleClassUpInfoByID(int roleID, int classLevel)
    {
        foreach (var item in RoleClassUpList)
        {
            if (item.ID == roleID && item.Color == classLevel)
            {
                return item;
            }
        }
        return null;
    }
    public RoleClassUp GetRoleClassUpInfoByIDAndLevel(int roleID, int roleLevel)
    {
        RoleClassUp roleClass = null;
        if (roleLevel >= 76)
        {
            foreach (var item in RoleClassUpList)
            {
                if (item.ID == roleID && item.Levelcap <= roleLevel)
                {
                    roleClass = item;
                }
            }
        }
        else
        {
            foreach (var item in RoleClassUpList)
            {
                if (item.ID == roleID && item.Levelcap <= roleLevel)
                {
                    if (item.Levelcap != 0)
                    {
                        roleClass = item;
                    }
                }
            }

        }

        return roleClass;
    }

    #endregion

    #region EquipClassUp
    public BetterList<EquipClassUp> EquipClassUpList = new BetterList<EquipClassUp>();

    public void AddEquipClassUp(EquipClassUp ecu)
    {
        EquipClassUpList.Add(ecu);
    }

    public EquipClassUp GetEquipClassUpInfoByID(int roleID, int classLevel)
    {
        foreach (var item in EquipClassUpList)
        {
            if (item.EquipID == roleID && item.ClassLevel == classLevel)
            {
                Debug.Log(item.EquipID);
                return item;
            }
        }
        return null;
    }


    #endregion

    #region EquipStrong
    public BetterList<EquipStrong> EquipStrongList = new BetterList<EquipStrong>();

    public void AddEquipStrong(EquipStrong ecu)
    {
        EquipStrongList.Add(ecu);
    }

    public EquipStrong GetEquipStrongByID(int equipID)
    {
        foreach (var item in EquipStrongList)
        {
            if (item.EquipId == equipID)
            {
                return item;
            }
        }
        return null;
    }
    public EquipStrong GetEquipStrongByID(int equipID, int equipColorNum)
    {
        foreach (var item in EquipStrongList)
        {
            if (item.EquipId == equipID && item.EquipColor == equipColorNum)
            {
                return item;
            }
        }
        return null;
    }
    public EquipStrong GetEquipStrongByPositionRaceColor(int part, int race, int equipColorNum)
    {
        foreach (var item in EquipStrongList)
        {
            if (item.Part == part && item.Race == race && item.EquipColor == equipColorNum)
            {
                return item;
            }
        }
        return null;
    }
    #endregion


    #region Exchange

    public BetterList<Exchange> chargeItemList = new BetterList<Exchange>();

    public void AddExchangeItem(Hashtable myHash)
    {
        Exchange mItem = new Exchange(myHash);
        chargeItemList.Add(mItem);
    }


    public Exchange GetExchangeById(int id)
    {
        foreach (var i in chargeItemList)
        {
            if (i.exchangeID == id)
            {
                return i;
            }
        }

        return null;
    }
    public Exchange GetExchangeByCash(int cash)
    {
        foreach (var i in chargeItemList)
        {
            if (i.cash == cash)
            {
                return i;
            }
        }
        return null;
    }

    #endregion
    #region StrengthenMaster

    public BetterList<StrengthenMaster> strengthenMasterList = new BetterList<StrengthenMaster>();
    private BetterList<StrengthenMaster> _returnstrengthenMasterList = new BetterList<StrengthenMaster>();
    public void AddStrengthenMaster(Hashtable myHash)
    {
        StrengthenMaster mMaster = new StrengthenMaster(myHash);
        strengthenMasterList.Add(mMaster);
    }

    /// <summary>
    /// 根据类型查找强化大师数据
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public BetterList<StrengthenMaster> GetStrengthenMasterByType(int _type)
    {
        _returnstrengthenMasterList.Clear();
        for (var i = 0; i < strengthenMasterList.size; i++)
        {
            if (strengthenMasterList[i].type == _type)
            {
                _returnstrengthenMasterList.Add(strengthenMasterList[i]);
            }
        }
        return _returnstrengthenMasterList;
    }

    public StrengthenMaster GetStrengthenMasterByTypeAndLevel(int _type, int level)
    {
        if (level > 0 && level <= GetStrengthenMasterByType(_type).size)
        {
            return GetStrengthenMasterByType(_type)[level - 1];
        }
        return null;
    }


    public StrengthenMaster GetStrengthenMasterByTypeAndNeedLevel(int _type, int level)
    {
        _returnstrengthenMasterList.Clear();
        for (var i = 0; i < strengthenMasterList.size; i++)
        {
            if (strengthenMasterList[i].type == _type)
            {
                _returnstrengthenMasterList.Add(strengthenMasterList[i]);
            }
        }

        for (var i = 1; i < _returnstrengthenMasterList.size; i++)
        {
            if (_returnstrengthenMasterList[i].needLevel > level && _returnstrengthenMasterList[i - 1].needLevel <= level)
            {
                return _returnstrengthenMasterList[i];
            }
        }
        return null;
    }

    #endregion

    #region Talent
    public Dictionary<int, RoleTalent> talentDic = new Dictionary<int, RoleTalent>();

    public void AddTalent(int id, RoleTalent ta)
    {
        talentDic.Add(id, ta);
    }

    public RoleTalent GetTalentByID(int TalentID)
    {
        if (talentDic.ContainsKey(TalentID))
        {
            return talentDic[TalentID];
        }
        return null;
    }
    public List<RoleTalent> GetHeroTalentListByHeroID(int _heroId)
    {
        List<RoleTalent> heroTalentList = new List<RoleTalent>();
        for (int i = 1; i <= talentDic.Count; i++)
        {
            if (talentDic[i].RoleID == _heroId)
            {
                heroTalentList.Add(talentDic[i]);
            }
        }
        return heroTalentList;
    }
    #endregion

    #region EquipExp
    public Dictionary<int, int> EquipExpDic = new Dictionary<int, int>();

    public void AddEquipExp(int id, int exp)
    {
        EquipExpDic.Add(id, exp);
    }

    public int GetEquipExpByID(int Lv)
    {
        if (EquipExpDic.ContainsKey(Lv))
        {
            return EquipExpDic[Lv];
        }
        return -1;
    }

    #endregion
    #region Resource
    public Dictionary<int, Resource> ResourceDic = new Dictionary<int, Resource>();

    public void AddResource(int id, Resource re)
    {
        ResourceDic.Add(id, re);
    }
    public Resource GetResourceByID(int id)
    {
        if (ResourceDic.ContainsKey(id))
        {
            return ResourceDic[id];
        }
        return null;
    }
    public Resource GetResourceMapId(int id)
    {
        for (int i = 1; i < ResourceDic.Count; i++)
        {
            if (ResourceDic[i].MapId == id)
            {
                return ResourceDic[i];

            }
        }
        return null;
    }
    #endregion

    #region EquipStrongQuality
    public BetterList<EquipStrongQuality> EquipStrongQualityList = new BetterList<EquipStrongQuality>();

    public void AddEquipStrongQuality(EquipStrongQuality esq)
    {
        EquipStrongQualityList.Add(esq);
    }
    public EquipStrongQuality GetEquipStrongQualityByID(int color, int race, int part)
    {
        foreach (var item in EquipStrongQualityList)
        {
            if (item.Color == color && item.Race == race && item.Part == part)
            {
                return item;
            }
        }
        return null;
    }
    public EquipStrongQuality GetEquipStrongQualityByIDAndColor(int equipId, int color, int race)
    {
        foreach (var item in EquipStrongQualityList)
        {
            if (item.Color == color && item.Race == race && item.EquipCode == equipId)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region  WorldEvent
    public Dictionary<int, WorldEvent> WorldEventDic = new Dictionary<int, WorldEvent>();
    public void AddWorldEvent(int id, WorldEvent World)
    {
        WorldEventDic.Add(id, World);
    }
    public string GetWorldEventNameByID(int Id)
    {
        if (WorldEventDic.ContainsKey(Id))
        {
            return WorldEventDic[Id].Name;
        }
        return "";
    }

    public WorldEvent GetWorldEventByID(int Id)
    {
        if (WorldEventDic.ContainsKey(Id))
        {
            return WorldEventDic[Id];
        }
        return null;
    }
    public WorldEvent GetWorldEventByGateID(int GateID)
    {
        foreach (var g in WorldEventDic)
        {
            if (g.Value.GatePoint == GateID)
            {
                return g.Value;
            }
        }
        return null;
    }

    public string GetWorldEventResultDesByID(int Id)
    {
        if (WorldEventDic.ContainsKey(Id))
        {
            return WorldEventDic[Id].ResultDes;
        }
        return "";
    }
    #endregion

    #region  Map
    public Dictionary<int, GateMap> MapDic = new Dictionary<int, GateMap>();
    public void AddGateMap(int id, GateMap Map)
    {
        MapDic.Add(id, Map);
    }
    public string GetMapNameByID(int Id)
    {
        if (MapDic.ContainsKey(Id))
        {
            return MapDic[Id].Name;
        }
        return "";
    }

    public List<GateMap> GetGateMapByID(int GateID)
    {
        List<GateMap> ListGateMap = new List<GateMap>();
        foreach (var g in MapDic)
        {
            if (g.Value.GateID == GateID)
            {
                ListGateMap.Add(g.Value);
            }
        }
        return ListGateMap;
    }
    #endregion

    #region RoleWash
    public BetterList<RoleWash> RoleWashList = new BetterList<RoleWash>();

    public void AddRoleWash(RoleWash rw)
    {
        RoleWashList.Add(rw);
    }

    public RoleWash GetRoleWashByID(int RoleId)
    {
        foreach (var item in RoleWashList)
        {
            if (item.RoleID == RoleId)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region RoleFate
    public BetterList<RoleFate> RoleFateList = new BetterList<RoleFate>();
    public BetterList<RoleFate> MyRoleFateList = new BetterList<RoleFate>();

    public void AddRoleFate(RoleFate rf)
    {
        RoleFateList.Add(rf);
    }
    public BetterList<RoleFate> GetMyRoleFateListByRoleID(int RoleID)
    {
        MyRoleFateList.Clear();
        foreach (var item in RoleFateList)
        {
            if (item.RoleID == RoleID)
            {
                MyRoleFateList.Add(item);
            }
        }
        return MyRoleFateList;
    }
    public RoleFate GetRoleFateByID(int RoleFateID)
    {
        foreach (var item in RoleFateList)
        {
            if (item.RoleFateID == RoleFateID)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region RoleBreach
    public BetterList<RoleBreach> RoleBreachList = new BetterList<RoleBreach>();

    public void AddRoleBreach(RoleBreach rb)
    {
        RoleBreachList.Add(rb);
    }

    public RoleBreach GetRoleBreachByID(int RoleId, int level)
    {
        foreach (var item in RoleBreachList)
        {
            if (item.roleId == RoleId && item.rank == level)
            {
                return item;
            }
        }
        return null;
    }
    public List<RoleBreach> GetRoleBreachByRoleID(int RoleId)
    {
        List<RoleBreach> list = new List<RoleBreach>();
        foreach (var item in RoleBreachList)
        {
            if (item.roleId == RoleId)
            {
                list.Add(item);
            }
        }
        return list;
    }
    #endregion

    #region RoleDestiny
    public BetterList<RoleDestiny> RoleDestinyList = new BetterList<RoleDestiny>();
    public void AddRoleDestiny(int _roleID, int _level, int _needlevel, int _maxExp, float _hp, float __atk, float _def)
    {
        RoleDestiny rd = new RoleDestiny();
        rd.Level = _level;
        rd.RoleId = _roleID;
        rd.NeedLevel = _needlevel;
        rd.MaxExp = _maxExp;
        rd.HP = _hp;
        rd.ATK = __atk;
        rd.DEF = _def;

        RoleDestinyList.Add(rd);
    }

    public RoleDestiny GetRoleDestinyByID(int RoleId, int level)
    {
        //Debug.LogError(RoleId + "&&&&&&()()()" + level);
        foreach (var i in RoleDestinyList)
        {
            if (i.RoleId == RoleId && i.Level == level)
            {
                //Debug.LogError(i.RoleId + "******" + i.Level);
                return i;
            }
        }
        //Debug.LogError("猪猪猪");
        return null;
    }

    public RoleDestiny GetRoleDestingByIdAndLevel(int RoleId, int roleLevel)
    {
        foreach (var item in RoleDestinyList)
        {
            if (item.RoleId == RoleId)
            {
                int level = 0;
                if (roleLevel % 10 == 0)
                {
                    level = (roleLevel / 10 + 1) * 10;
                }
                else
                {
                    level = Mathf.CeilToInt(roleLevel * 1.0f / 10) * 10;
                }

                if (level == item.NeedLevel)
                {
                    return item;
                }
            }
        }
        return null;
    }

    #endregion

    #region RoleDestinyCost
    public Dictionary<int, RoleDestinyCost> RoleDestinyCostDic = new Dictionary<int, RoleDestinyCost>();

    public void AddRoleDestinyCost(int level, RoleDestinyCost cost)
    {
        RoleDestinyCostDic.Add(level, cost);
    }
    public RoleDestinyCost GetRoleDestinyCostByID(int skilllevel)
    {
        if (RoleDestinyCostDic.ContainsKey(skilllevel))
        {
            return RoleDestinyCostDic[skilllevel];
        }
        return null;
    }
    #endregion

    #region EquipStrongCost
    public BetterList<EquipStrongCost> EquipStrongCostList = new BetterList<EquipStrongCost>();

    public void AddEquipStrongCost(EquipStrongCost rb)
    {
        EquipStrongCostList.Add(rb);
    }
    /// <summary>
    /// 装备强化花费
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public EquipStrongCost GetEquipStrongCostDataByID(int level, int RoleRarity)
    {
        foreach (var item in EquipStrongCostList)
        {
            if (item.StrongLevel == level && item.RoleRarity == RoleRarity)
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    /// 装备强化花费
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetEquipStrongCostByID(int level, int RoleRarity, int position)
    {
        //Debug.Log(level + "..RoleRarity.." + RoleRarity + "..position.." + position);
        foreach (var item in EquipStrongCostList)
        {
            if (item.StrongLevel == level && item.RoleRarity == RoleRarity)
            {
                return item.CostList[position - 1];
            }
        }
        return 0;
    }

    #endregion

    #region RareTreasureOpen
    public Dictionary<int, RareTreasureOpen> RareTreasureOpenDic = new Dictionary<int, RareTreasureOpen>();

    public void AddRareTreasureOpen(int RareTreasureOpenID, RareTreasureOpen _RareTreasureOpen)
    {
        /* RareTreasureOpenDic.Clear();
         RareTreasureOpenDic.Add(1, new RareTreasureOpen(1, 20));
         RareTreasureOpenDic.Add(2, new RareTreasureOpen(1, 30));
         RareTreasureOpenDic.Add(3, new RareTreasureOpen(1, 40));
         RareTreasureOpenDic.Add(4, new RareTreasureOpen(1, 50));
         RareTreasureOpenDic.Add(5, new RareTreasureOpen(1, 60));
         RareTreasureOpenDic.Add(6, new RareTreasureOpen(1, 70));
         RareTreasureOpenDic.Add(7, new RareTreasureOpen(1, 80));*/
        RareTreasureOpenDic.Add(RareTreasureOpenID, _RareTreasureOpen);
    }
    #endregion

    #region RareTreasureAttr
    public BetterList<RareTreasureAttr> RareTreasureAttrList = new BetterList<RareTreasureAttr>();

    public void AddRareTreasureAttr(RareTreasureAttr rb)
    {
        RareTreasureAttrList.Add(rb);
    }
    /// <summary>
    /// 秘宝属性
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public RareTreasureAttr GetRareTreasureAttrByID(int RareTreasureID)
    {
        foreach (var item in RareTreasureAttrList)
        {
            if (item.RareTreasureID == RareTreasureID)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region RareTreasureExp
    public BetterList<RareTreasureExp> RareTreasureExpList = new BetterList<RareTreasureExp>();

    public void AddRareTreasureExp(RareTreasureExp rb)
    {
        RareTreasureExpList.Add(rb);
    }
    /// <summary>
    /// 秘宝经验
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public RareTreasureExp GetRareTreasureExpByID(int level, int color)
    {
        foreach (var item in RareTreasureExpList)
        {
            if (item.Level == level && item.Color == color)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region EverydayActivity
    public Dictionary<int, EverydayActivity> EverydayActivityDic = new Dictionary<int, EverydayActivity>();
    public void AddEverydayActivityDic(int id, EverydayActivity EverydayActivity)
    {
        EverydayActivityDic.Add(id, EverydayActivity);
    }
    public EverydayActivity GetEverydayActivityDicByID(int id)
    {
        if (EverydayActivityDic.ContainsKey(id))
        {
            return EverydayActivityDic[id];
        }
        return null;
    }
    #endregion

    #region EverydayActivityAward
    public Dictionary<int, EverydayActivityAward> EverydayActivityAwardDic = new Dictionary<int, EverydayActivityAward>();
    public void AddEverydayActivityAwardDic(int id, EverydayActivityAward EverydayActivityAward)
    {
        EverydayActivityAwardDic.Add(id, EverydayActivityAward);
    }
    public EverydayActivityAward GetEverydayActivityAwardDicByID(int id)
    {
        if (EverydayActivityAwardDic.ContainsKey(id))
        {
            return EverydayActivityAwardDic[id];
        }
        return null;
    }
    #endregion

    #region TechTree
    public BetterList<TechTree> TechTreeList = new BetterList<TechTree>();
    public void AddTechTreeDic(int TechTreeID, int Depth, int EffectType, float EffectVal1, float EffectVal2, int Level, int UnLockPreID,
                      int UnLockNeedLevel, int UnLockPreDepthPoint, int LevelUpNeedPoint, string Name, int Icon, string Des, int GlodCost, string NoOpenDescription)
    {
        TechTreeList.Add(new TechTree(TechTreeID, Depth, EffectType, EffectVal1, EffectVal2, Level, UnLockPreID,
                     UnLockNeedLevel, UnLockPreDepthPoint, LevelUpNeedPoint, Name, Icon, Des, GlodCost, NoOpenDescription));
    }
    public TechTree GetTechTreerByIcon(int icon, int level)
    {
        foreach (TechTree tech in TechTreeList)
        {
            if (tech.Icon == icon && tech.Level == level)
            {
                return tech;
            }

        }
        return null;
    }
    public TechTree GetTechTreeByID(int id)
    {
        foreach (TechTree tech in TechTreeList)
        {
            if (tech.TechTreeID == id)
            {
                return tech;
            }

        }
        return null;
    }

    public List<int> GetTechListByUnlockLevel(int level)
    {
        List<int> list = new List<int>();
        foreach (var item in TechTreeList)
        {
            if (item.UnLockNeedLevel <= level)
            {
                list.Add(item.TechTreeID);
            }
        }
        return list;
    }
    #endregion
    #region Tower
    public Dictionary<int, TowerData> TowerDic = new Dictionary<int, TowerData>();
    public void AddTowerDic(int id, TowerData Tower)
    {
        TowerDic.Add(id, Tower);
    }
    public TowerData GetTowerByID(int id)
    {
        if (TowerDic.ContainsKey(id))
        {
            return TowerDic[id];
        }
        return null;
    }
    #endregion
    #region TowerBoxCost
    public Dictionary<int, TowerBoxCostData> TowerBoxCostDic = new Dictionary<int, TowerBoxCostData>();
    public void AddTowerBoxCostDic(int id, TowerBoxCostData Tower)
    {
        TowerBoxCostDic.Add(id, Tower);
    }
    public TowerBoxCostData GetTowerBoxCostByID(int id)
    {
        if (TowerBoxCostDic.ContainsKey(id))
        {
            return TowerBoxCostDic[id];
        }
        return null;
    }
    #endregion
    #region  Market
    public Dictionary<int, Market> MarketDic = new Dictionary<int, Market>();
    public void AddMarketDic(int id, Market Market)
    {
        MarketDic.Add(id, Market);
    }
    public Market GetMarketByID(int Id)
    {
        if (MarketDic.ContainsKey(Id))
        {
            return MarketDic[Id];
        }
        return null;
    }
    public Market GetMarketByBuyCount(int buyCount)
    {
        int marketId = buyCount;
        if (buyCount <= 20)
        {
            marketId = buyCount;
        }
        else if (buyCount > 20 && buyCount <= 50)
        {
            marketId = 21;
        }
        else if (buyCount > 50 && buyCount <= 100)
        {
            marketId = 22;
        }
        else if (buyCount > 100)
        {
            marketId = 23;
        }
        return TextTranslator.instance.GetMarketByID(marketId); ;
    }
    //public int GetMarketIDByBuyCount(int count)
    //{
    //    for (int i = 0; i < MarketDic.Count; i++)
    //    {
    //        if (i + 1 < MarketDic.Count)
    //        {
    //            if (count >= GetMarketBuyCountByID(i) && count < GetMarketBuyCountByID(i + 1))
    //            {
    //                return i + 1;
    //            }
    //        }
    //    }
    //    return 0;
    //}

    public int GetMarketIDByBuyCount(int count)
    {
        int Num = 0;
        for (int i = 0; i < MarketDic.Count; i++)
        {
            if (count >= MarketDic[i + 1].BuyCount)
            {
                Num = i + 1;
            }
        }
        return Num;
    }
    public int GetMarketBuyCountByID(int Id)
    {
        if (MarketDic.ContainsKey(Id))
        {
            return MarketDic[Id].BuyCount;
        }
        return 0;
    }
    #endregion
    #region PVPReward
    public Dictionary<int, PVPReward> PVPRewardDic = new Dictionary<int, PVPReward>();
    public void AddPVPRewardDic(int id, PVPReward PVPReward)
    {
        PVPRewardDic.Add(id, PVPReward);
    }
    public PVPReward GetPVPRewardByID(int id)
    {
        if (PVPRewardDic.ContainsKey(id))
        {
            return PVPRewardDic[id];
        }
        return null;
    }
    #endregion
    #region PvpOnceReward
    public Dictionary<int, PvpOnceReward> PvpOnceRewardDic = new Dictionary<int, PvpOnceReward>();
    public void AddPvpOnceRewardDic(int id, PvpOnceReward PvpOnceReward)
    {
        PvpOnceRewardDic.Add(id, PvpOnceReward);
    }
    public PvpOnceReward GetPvpOnceRewardByID(int id)
    {
        if (PvpOnceRewardDic.ContainsKey(id))
        {
            return PvpOnceRewardDic[id];
        }
        return null;
    }
    #endregion

    #region PvpPointShop
    public Dictionary<int, PvpPointShop> PvpPointShopDic = new Dictionary<int, PvpPointShop>();
    public void AddPvpPointShopDic(int id, PvpPointShop _PvpPointShop)
    {
        PvpPointShopDic.Add(id, _PvpPointShop);
    }

    public PvpPointShop GetPvpPointShopByID(int id)
    {
        if (PvpPointShopDic.ContainsKey(id))
        {
            return PvpPointShopDic[id];
        }
        return null;
    }
    #endregion
    #region SmuggleCar
    public Dictionary<int, SmuggleCar> SmuggleCarDic = new Dictionary<int, SmuggleCar>();
    public void AddSmuggleCarDic(int id, SmuggleCar _SmuggleCar)
    {
        SmuggleCarDic.Add(id, _SmuggleCar);
    }

    public SmuggleCar GetSmuggleCarByID(int id)
    {
        if (SmuggleCarDic.ContainsKey(id))
        {
            return SmuggleCarDic[id];
        }
        return null;
    }
    #endregion
    #region SuperCar
    public Dictionary<int, SuperCar> SuperCarDic = new Dictionary<int, SuperCar>();
    public void AddSuperCarDic(int id, SuperCar _SuperCar)
    {
        SuperCarDic.Add(id, _SuperCar);
    }

    public SuperCar GetSuperCarByID(int id)
    {
        if (SuperCarDic.ContainsKey(id))
        {
            return SuperCarDic[id];
        }
        return null;
    }
    #endregion

    #region WeaponUpStar
    public BetterList<WeaponUpStar> WeaponUpStarDic = new BetterList<WeaponUpStar>();
    public void AddWeaponUpStarDic(int _ID, int _WeaponID, int _Color, int _Star, float _Hp, float _Atk, float _Def, float _UpStarRand, int _NeedItemID1, int _NeddItemNum1)
    {
        WeaponUpStarDic.Add(new WeaponUpStar(_ID, _WeaponID, _Color, _Star, _Hp, _Atk, _Def, _UpStarRand, _NeedItemID1, _NeddItemNum1));
    }

    public WeaponUpStar GetWeaponUpStarByID(int id, int classid, int star)
    {
        foreach (WeaponUpStar UpStar in WeaponUpStarDic)
        {
            if (id == UpStar.WeaponID && classid == UpStar.Color && star == UpStar.Star)
            {
                return UpStar;
            }

        }
        return null;
    }

    public WeaponUpStar GetWeaponUpStarByWeaponUpStarID(int WeaponUpStarID)
    {
        foreach (WeaponUpStar UpStar in WeaponUpStarDic)
        {
            if (WeaponUpStarID == UpStar.ID)
            {
                return UpStar;
            }

        }
        return null;
    }
    #endregion

    #region WeaponUpClasss
    public BetterList<WeaponUpClass> WeaponUpClassDic = new BetterList<WeaponUpClass>();
    public void AddWeaponUpClassDic(int _ID, int _WeaponID, int _UpClassType, int _Hp, int _Atk, int _Def, int _NeedGold, int _StoneNeedNum, int _RoleDebrisNum)
    {
        WeaponUpClassDic.Add(new WeaponUpClass(_ID, _WeaponID, _UpClassType, _Hp, _Atk, _Def, _NeedGold, _StoneNeedNum, _RoleDebrisNum));
    }

    public WeaponUpClass GetWeaponUpClassByID(int id, int classType)
    {
        foreach (WeaponUpClass UpClass in WeaponUpClassDic)
        {
            if (id == UpClass.WeaponID && classType == UpClass.UpClassType)
            {
                return UpClass;
            }
        }
        return null;
    }
    #endregion

    #region WeaponMaterial
    public Dictionary<int, WeaponMaterial> WeaponMaterialDic = new Dictionary<int, WeaponMaterial>();
    public void AddWeaponMaterialDic(int id, WeaponMaterial _WeaponMaterial)
    {
        WeaponMaterialDic.Add(id, _WeaponMaterial);
    }

    public WeaponMaterial GetWeaponMaterialByID(int id)
    {
        if (WeaponMaterialDic.ContainsKey(id))
        {
            return WeaponMaterialDic[id];
        }
        return null;
    }
    #endregion

    #region WeaponWheel
    public Dictionary<int, WeaponWheel> WeaponWheelDic = new Dictionary<int, WeaponWheel>();
    public void AddWeaponWheelDic(int id, WeaponWheel _WeaponWheel)
    {
        WeaponWheelDic.Add(id, _WeaponWheel);
    }

    public WeaponWheel GetWeaponWheelByID(int id)
    {
        if (WeaponWheelDic.ContainsKey(id))
        {
            return WeaponWheelDic[id];
        }
        return null;
    }
    #endregion
    #region FightTalk
    public List<FightTalk> ListFightTalk = new List<FightTalk>();
    public void AddFightTalk(FightTalk _FightTalk)
    {
        ListFightTalk.Add(_FightTalk);
    }

    public List<FightTalk> GetFightTalkByRoleID(int RoleID, int TalkType, int TalkKind)
    {
        List<FightTalk> _ListFightTalk = new List<FightTalk>();

        foreach (var t in ListFightTalk)
        {
            if (t.RoleID == RoleID && t.GateID == 0 && t.TalkType == TalkType && (t.TalkKind == TalkKind || t.TalkKind == 4))
            {
                _ListFightTalk.Add(t);
            }
        }
        return _ListFightTalk;
    }

    public List<FightTalk> GetFightTalkByGateID(int GateID, int Round)
    {
        List<FightTalk> _ListFightTalk = new List<FightTalk>();

        foreach (var t in ListFightTalk)
        {
            if (t.GateID == GateID && t.TalkRound == Round)
            {
                _ListFightTalk.Add(t);
            }
        }
        return _ListFightTalk;
    }
    #endregion

    #region TowerRankReward
    public Dictionary<int, TowerRankReward> TowerRankRewardDic = new Dictionary<int, TowerRankReward>();
    public void AddTowerRankRewardDic(int id, TowerRankReward Tower)
    {
        TowerRankRewardDic.Add(id, Tower);
    }
    public TowerRankReward GetTowerRankRewardByID(int id)
    {
        if (TowerRankRewardDic.ContainsKey(id))
        {
            return TowerRankRewardDic[id];
        }
        return null;
    }
    #endregion
    #region TowerShop
    public Dictionary<int, TowerShop> TowerShopDic = new Dictionary<int, TowerShop>();
    public void AddTowerShopDic(int id, TowerShop Tower)
    {
        TowerShopDic.Add(id, Tower);
    }
    public TowerShop GetTowerShopByID(int id)
    {
        if (TowerShopDic.ContainsKey(id))
        {
            return TowerShopDic[id];
        }
        return null;
    }
    public List<TowerShop> getAllTowerShop()
    {
        List<TowerShop> TowerShopList = new List<TowerShop>();
        foreach (var g in TowerShopDic)
        {
            TowerShopList.Add(g.Value);
        }
        return TowerShopList;
    }
    #endregion
    #region ActionEvent
    public Dictionary<int, ActionEvent> ActionEventDic = new Dictionary<int, ActionEvent>();
    public void AddActionEventDic(int id, ActionEvent Action)
    {
        ActionEventDic.Add(id, Action);
    }
    public int GetTalkId(int _actionEventID)
    {
        if (ActionEventDic.ContainsKey(_actionEventID))
        {
            return ActionEventDic[_actionEventID].TalkID;
        }
        return 0;
    }

    public ActionEvent GetActionEventById(int id)
    {
        if (ActionEventDic.ContainsKey(id))
        {
            return ActionEventDic[id];
        }
        return null;
    }
    public ActionEvent GetActionEventByGateId(int id)
    {
        foreach (var action in ActionEventDic)
        {
            if (action.Value.GateID == id)
            {
                return action.Value;
            }
        }
        return null;
    }
    public List<ActionEvent> GetActionEventsByGroup(int group)
    {
        List<ActionEvent> ae = new List<ActionEvent>();
        foreach (var action in ActionEventDic)
        {
            if (action.Value.GroupID == group)
            {
                ae.Add(action.Value);
            }
        }
        return ae;
    }
    #endregion
    #region Vip
    public Dictionary<int, Vip> VipDic = new Dictionary<int, Vip>();
    public void AddVipDic(int id, Vip Vip)
    {
        VipDic.Add(id, Vip);
    }
    public Vip GetVipDicByID(int id)
    {
        if (VipDic.ContainsKey(id))
        {
            return VipDic[id];
        }
        return null;
    }
    #endregion
    #region Talk
    public BetterList<Talk> TalkList = new BetterList<Talk>();
    public void AddTalkDic(int _TalkID, int _InTurnID, int _LeftType, string _LeftDialog, int _RightType, string _RightDialog)
    {
        TalkList.Add(new Talk(_TalkID, _InTurnID, _LeftType, _LeftDialog, _RightType, _RightDialog));
    }
    public Talk GetTalkById(int talkid, int inturnid)
    {
        foreach (Talk talk in TalkList)
        {
            if (talk.TalkID == talkid && talk.InTurnID == inturnid)
            {
                return talk;
            }
        }
        return null;
    }
    #endregion
    #region Barrage
    public BetterList<Barrage> BarrageList = new BetterList<Barrage>();
    public void AddBarrageDic(int _ID, int _GateID, string _Des, int _OutTime, int _Position, int _Color)
    {
        BarrageList.Add(new Barrage(_ID, _GateID, _Des, _OutTime, _Position, _Color));
    }
    public int GetBarrageByCount(int _GateID)
    {
        int count = 1;
        foreach (Barrage Barrage in BarrageList)
        {
            if (Barrage.GateID == _GateID && Barrage.ID == count)
            {
                count++;
            }
        }
        return count;
    }
    public Barrage GetBarrageById(int _GateID, int _ID)
    {
        foreach (Barrage Barrage in BarrageList)
        {
            if (Barrage.GateID == _GateID && Barrage.ID == _ID)
            {
                return Barrage;
            }
        }
        return null;
    }
    #endregion

    #region Fortress
    public BetterList<Fortress> FortressList = new BetterList<Fortress>();
    public void AddFortressDic(int _ID, int _Type, int _Level, int _BulidLevel, int _NeedItemID, int _NeddItemNum, int _BonusItemId, float _BonusItemNum, float _FriendBase, float _FriendGrow)
    {
        FortressList.Add(new Fortress(_ID, _Type, _Level, _BulidLevel, _NeedItemID, _NeddItemNum, _BonusItemId, _BonusItemNum, _FriendBase, _FriendGrow));
    }
    public Fortress GetFortressById(int _Level, int _ID)
    {
        foreach (Fortress Fortress in FortressList)
        {
            if (Fortress.Level == _Level && Fortress.Type == _ID)
            {
                return Fortress;
            }
        }
        return null;
    }

    public Fortress GetFortressByListId(int _BulidLevel, int _ID)
    {
        foreach (Fortress Fortress in FortressList)
        {
            if (Fortress.BulidLevel == _BulidLevel && Fortress.Type == _ID)
            {
                return Fortress;
            }
        }
        return null;
    }
    #endregion

    #region
    public Dictionary<int, CombinSkill> CombinSkillDic = new Dictionary<int, CombinSkill>();
    public void AddCombinSkillDic(int id, CombinSkill CombinSkill)
    {
        CombinSkillDic.Add(id, CombinSkill);
    }

    public CombinSkill GetCombinSkillByID(int id)
    {
        if (CombinSkillDic.ContainsKey(id))
        {
            return CombinSkillDic[id];
        }
        return null;
    }
    #endregion
    #region 英雄颜色枚举
    public int SetHeroNameColor(UILabel NameColor, UISprite frame, UISprite pinJieSprite, string Name, int ClassNumber)
    {
        int addNum = 0;
        switch (ClassNumber)
        {
            case 1:
                NameColor.text = "[dbdbdb]" + Name;//白色
                frame.spriteName = "yxdi0";
                pinJieSprite.spriteName = "zbkuang1";
                addNum = 0;
                break;
            case 2:
                NameColor.text = "[00ff3c]" + Name + "[-]";//绿色 [07f476]
                frame.spriteName = "yxdi1";
                pinJieSprite.spriteName = "zbkuang2";
                addNum = 0;
                break;
            case 3:
                NameColor.text = "[00ff3c]" + Name + "+1[-]";
                frame.spriteName = "yxdi1";
                pinJieSprite.spriteName = "zbkuang2";
                addNum = 1;
                break;
            case 4:
                NameColor.text = "[00ff3c]" + Name + "+2[-]";
                frame.spriteName = "yxdi1";
                pinJieSprite.spriteName = "zbkuang2";
                addNum = 2;
                break;
            case 5:
                NameColor.text = "[009cff]" + Name + "[-]";//蓝色  [00f0ff]
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 0;
                break;
            case 6:
                NameColor.text = "[009cff]" + Name + "+1[-]";
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 1;
                break;
            case 7:
                NameColor.text = "[009cff]" + Name + "+2[-]";
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 2;
                break;
            case 8:
                NameColor.text = "[009cff]" + Name + "+3[-]";
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 3;
                break;
            case 9:
                NameColor.text = "[b500ff]" + Name + "[-]";//紫色  [d879ff]
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 0;
                break;
            case 10:
                NameColor.text = "[b500ff]" + Name + "+1[-]";
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 1;
                break;
            case 11:
                NameColor.text = "[b500ff]" + Name + "+2[-]";
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 2;
                break;
            case 12:
                NameColor.text = "[b500ff]" + Name + "+3[-]";
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 3;
                break;
            case 13:
                NameColor.text = "[b500ff]" + Name + "+4[-]";
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 4;
                break;
            case 14:
                NameColor.text = "[ff6c00]" + Name + "[-]";//橙色  [ff9000]
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 0;
                break;
            case 15:
                NameColor.text = "[ff6c00]" + Name + "+1[-]";
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 1;
                break;
            case 16:
                NameColor.text = "[ff6c00]" + Name + "+2[-]";
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 2;
                break;
            case 17:
                NameColor.text = "[ff6c00]" + Name + "+3[-]";
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 3;
                break;
            case 18:
                NameColor.text = "[ff6c00]" + Name + "+4[-]";
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 4;
                break;
            case 19:
                NameColor.text = "[ff0000]" + Name + "[-]";//红色  [f64b7f]
                frame.spriteName = "yxdi5";
                pinJieSprite.spriteName = "zbkuang6";
                addNum = 0;
                break;
            case 20:
                NameColor.text = "[ff0000]" + Name + "+1[-]";
                frame.spriteName = "yxdi5";
                pinJieSprite.spriteName = "zbkuang6";
                addNum = 1;
                break;
            default:
                break;
        }
        return addNum;
    }
    public string SetHeroNameColor(string Name, int ClassNumber)
    {
        string NameColor = null;
        switch (ClassNumber)
        {
            case 1:
                NameColor = "[dbdbdb]" + Name;//白色
                break;
            case 2:
                NameColor = "[00ff3c]" + Name + "[-]";//绿色
                break;
            case 3:
                NameColor = "[00ff3c]" + Name + "+1[-]";
                break;
            case 4:
                NameColor = "[00ff3c]" + Name + "+2[-]";
                break;
            case 5:
                NameColor = "[009cff]" + Name + "[-]";//蓝色
                break;
            case 6:
                NameColor = "[009cff]" + Name + "+1[-]";
                break;
            case 7:
                NameColor = "[009cff]" + Name + "+2[-]";
                break;
            case 8:
                NameColor = "[009cff]" + Name + "+3[-]";
                break;
            case 9:
                NameColor = "[b500ff]" + Name + "[-]";//紫色
                break;
            case 10:
                NameColor = "[b500ff]" + Name + "+1[-]";
                break;
            case 11:
                NameColor = "[b500ff]" + Name + "+2[-]";
                break;
            case 12:
                NameColor = "[b500ff]" + Name + "+3[-]";
                break;
            case 13:
                NameColor = "[b500ff]" + Name + "+4[-]";
                break;
            case 14:
                NameColor = "[ff6c00]" + Name + "[-]";//橙色
                break;
            case 15:
                NameColor = "[ff6c00]" + Name + "+1[-]";
                break;
            case 16:
                NameColor = "[ff6c00]" + Name + "+2[-]";
                break;
            case 17:
                NameColor = "[ff6c00]" + Name + "+3[-]";
                break;
            case 18:
                NameColor = "[ff6c00]" + Name + "+4[-]";
                break;
            case 19:
                NameColor = "[ff0000]" + Name + "[-]";//红色
                break;
            case 20:
                NameColor = "[ff0000]" + Name + "+1[-]";
                break;
            default:
                break;
        }
        return NameColor;
    }
    public int SetHeroNameColor(UILabel NameColor, UISprite frame, string Name, int ClassNumber)
    {
        int addNum = 0;
        switch (ClassNumber)
        {
            case 1:
                NameColor.text = "[dbdbdb]" + Name;//白色
                frame.spriteName = "yxdi0";
                addNum = 0;
                break;
            case 2:
                NameColor.text = "[00ff3c]" + Name + "[-]";//绿色
                frame.spriteName = "yxdi1";
                addNum = 0;
                break;
            case 3:
                NameColor.text = "[00ff3c]" + Name + "+1[-]";
                frame.spriteName = "yxdi1";
                addNum = 1;
                break;
            case 4:
                NameColor.text = "[00ff3c]" + Name + "+2[-]";
                frame.spriteName = "yxdi1";
                addNum = 2;
                break;
            case 5:
                NameColor.text = "[009cff]" + Name + "[-]";//蓝色
                frame.spriteName = "yxdi2";
                addNum = 0;
                break;
            case 6:
                NameColor.text = "[009cff]" + Name + "+1[-]";
                frame.spriteName = "yxdi2";
                addNum = 1;
                break;
            case 7:
                NameColor.text = "[009cff]" + Name + "+2[-]";
                frame.spriteName = "yxdi2";
                addNum = 2;
                break;
            case 8:
                NameColor.text = "[009cff]" + Name + "+3[-]";
                frame.spriteName = "yxdi2";
                addNum = 3;
                break;
            case 9:
                NameColor.text = "[b500ff]" + Name + "[-]";//紫色
                frame.spriteName = "yxdi3";
                addNum = 0;
                break;
            case 10:
                NameColor.text = "[b500ff]" + Name + "+1[-]";
                frame.spriteName = "yxdi3";
                addNum = 1;
                break;
            case 11:
                NameColor.text = "[b500ff]" + Name + "+2[-]";
                frame.spriteName = "yxdi3";
                addNum = 2;
                break;
            case 12:
                NameColor.text = "[b500ff]" + Name + "+3[-]";
                frame.spriteName = "yxdi3";
                addNum = 3;
                break;
            case 13:
                NameColor.text = "[b500ff]" + Name + "+4[-]";
                frame.spriteName = "yxdi3";
                addNum = 4;
                break;
            case 14:
                NameColor.text = "[ff6c00]" + Name + "[-]";//橙色
                frame.spriteName = "yxdi4";
                addNum = 0;
                break;
            case 15:
                NameColor.text = "[ff6c00]" + Name + "+1[-]";
                frame.spriteName = "yxdi4";
                addNum = 1;
                break;
            case 16:
                NameColor.text = "[ff6c00]" + Name + "+2[-]";
                frame.spriteName = "yxdi4";
                addNum = 2;
                break;
            case 17:
                NameColor.text = "[ff6c00]" + Name + "+3[-]";
                frame.spriteName = "yxdi4";
                addNum = 3;
                break;
            case 18:
                NameColor.text = "[ff6c00]" + Name + "+4[-]";
                frame.spriteName = "yxdi4";
                addNum = 4;
                break;
            case 19:
                NameColor.text = "[ff0000]" + Name + "[-]";//红色
                frame.spriteName = "yxdi5";
                addNum = 0;
                break;
            case 20:
                NameColor.text = "[ff0000]" + Name + "+1[-]";
                frame.spriteName = "yxdi5";
                addNum = 1;
                break;
            default:
                break;
        }
        return addNum;
    }

    public int SetHeroNameColor(UILabel NameColor, string Name, int ClassNumber)
    {
        int addNum = 0;
        switch (ClassNumber)
        {
            case 1:
                NameColor.text = "[dbdbdb]" + Name;//白色
                addNum = 0;
                break;
            case 2:
                NameColor.text = "[00ff3c]" + Name + "[-]";//绿色
                addNum = 0;
                break;
            case 3:
                NameColor.text = "[00ff3c]" + Name + "+1[-]";
                addNum = 1;
                break;
            case 4:
                NameColor.text = "[00ff3c]" + Name + "+2[-]";
                addNum = 2;
                break;
            case 5:
                NameColor.text = "[009cff]" + Name + "[-]";//蓝色
                addNum = 0;
                break;
            case 6:
                NameColor.text = "[009cff]" + Name + "+1[-]";
                addNum = 1;
                break;
            case 7:
                NameColor.text = "[009cff]" + Name + "+2[-]";
                addNum = 2;
                break;
            case 8:
                NameColor.text = "[009cff]" + Name + "+3[-]";
                addNum = 3;
                break;
            case 9:
                NameColor.text = "[b500ff]" + Name + "[-]";//紫色
                addNum = 0;
                break;
            case 10:
                NameColor.text = "[b500ff]" + Name + "+1[-]";
                addNum = 1;
                break;
            case 11:
                NameColor.text = "[b500ff]" + Name + "+2[-]";
                addNum = 2;
                break;
            case 12:
                NameColor.text = "[b500ff]" + Name + "+3[-]";
                addNum = 3;
                break;
            case 13:
                NameColor.text = "[b500ff]" + Name + "+4[-]";
                addNum = 4;
                break;
            case 14:
                NameColor.text = "[ff6c00]" + Name + "[-]";//橙色
                addNum = 0;
                break;
            case 15:
                NameColor.text = "[ff6c00]" + Name + "+1[-]";
                addNum = 1;
                break;
            case 16:
                NameColor.text = "[ff6c00]" + Name + "+2[-]";
                addNum = 2;
                break;
            case 17:
                NameColor.text = "[ff6c00]" + Name + "+3[-]";
                addNum = 3;
                break;
            case 18:
                NameColor.text = "[ff6c00]" + Name + "+4[-]";
                addNum = 4;
                break;
            case 19:
                NameColor.text = "[ff0000]" + Name + "[-]";//红色
                addNum = 0;
                break;
            case 20:
                NameColor.text = "[ff0000]" + Name + "+1[-]";
                addNum = 1;
                break;
            default:
                break;
        }
        return addNum;
    }

    public int SetHeroNameColor(UISprite frame, UISprite pinJieSprite, int ClassNumber)
    {
        int addNum = 0;
        switch (ClassNumber)
        {
            case 1:
                //白色
                frame.spriteName = "yxdi0";
                pinJieSprite.spriteName = "zbkuang1";
                addNum = 0;
                break;
            case 2:
                //绿色
                frame.spriteName = "yxdi1";
                pinJieSprite.spriteName = "zbkuang2";
                addNum = 0;
                break;
            case 3:
                frame.spriteName = "yxdi1";
                pinJieSprite.spriteName = "zbkuang2";
                addNum = 1;
                break;
            case 4:
                frame.spriteName = "yxdi1";
                pinJieSprite.spriteName = "zbkuang2";
                addNum = 2;
                break;
            case 5:
                //蓝色
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 0;
                break;
            case 6:
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 1;
                break;
            case 7:
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 2;
                break;
            case 8:
                frame.spriteName = "yxdi2";
                pinJieSprite.spriteName = "zbkuang3";
                addNum = 3;
                break;
            case 9:
                //紫色
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 0;
                break;
            case 10:
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 1;
                break;
            case 11:
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 2;
                break;
            case 12:
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 3;
                break;
            case 13:
                frame.spriteName = "yxdi3";
                pinJieSprite.spriteName = "zbkuang4";
                addNum = 4;
                break;
            case 14:
                //橙色
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 0;
                break;
            case 15:
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 1;
                break;
            case 16:
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 2;
                break;
            case 17:
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 3;
                break;
            case 18:
                frame.spriteName = "yxdi4";
                pinJieSprite.spriteName = "zbkuang5";
                addNum = 4;
                break;
            case 19:
                //红色
                frame.spriteName = "yxdi5";
                pinJieSprite.spriteName = "zbkuang6";
                addNum = 0;
                break;
            case 20:
                frame.spriteName = "yxdi5";
                pinJieSprite.spriteName = "zbkuang6";
                addNum = 1;
                break;
            default:
                break;
        }
        return addNum;
    }

    public int SetHeroNameColor(UISprite frame, int ClassNumber)
    {
        int addNum = 0;
        switch (ClassNumber)
        {
            case 1:
                //白色
                frame.spriteName = "yxdi0";
                addNum = 0;
                break;
            case 2:
                //绿色
                frame.spriteName = "yxdi1";
                addNum = 0;
                break;
            case 3:
                frame.spriteName = "yxdi1";
                addNum = 1;
                break;
            case 4:
                frame.spriteName = "yxdi1";
                addNum = 2;
                break;
            case 5:
                //蓝色
                frame.spriteName = "yxdi2";
                addNum = 0;
                break;
            case 6:
                frame.spriteName = "yxdi2";
                addNum = 1;
                break;
            case 7:
                frame.spriteName = "yxdi2";
                addNum = 2;
                break;
            case 8:
                frame.spriteName = "yxdi2";
                addNum = 3;
                break;
            case 9:
                //紫色
                frame.spriteName = "yxdi3";
                addNum = 0;
                break;
            case 10:
                frame.spriteName = "yxdi3";
                addNum = 1;
                break;
            case 11:
                frame.spriteName = "yxdi3";
                addNum = 2;
                break;
            case 12:
                frame.spriteName = "yxdi3";
                addNum = 3;
                break;
            case 13:
                frame.spriteName = "yxdi3";
                addNum = 4;
                break;
            case 14:
                //橙色
                frame.spriteName = "yxdi4";
                addNum = 0;
                break;
            case 15:
                frame.spriteName = "yxdi4";
                addNum = 1;
                break;
            case 16:
                frame.spriteName = "yxdi4";
                addNum = 2;
                break;
            case 17:
                frame.spriteName = "yxdi4";
                addNum = 3;
                break;
            case 18:
                frame.spriteName = "yxdi4";
                addNum = 4;
                break;
            case 19:
                //红色
                frame.spriteName = "yxdi5";
                addNum = 0;
                break;
            case 20:
                frame.spriteName = "yxdi5";
                addNum = 1;
                break;
            default:
                break;
        }
        return addNum;
    }
    #endregion

    #region 军团职位描述
    public string GetLegionPoisitionByPosId(int officialPosition)
    {
        string _officialPosition = "";
        switch (officialPosition)
        {
            case 0: _officialPosition = "士兵"; break;
            case 1: _officialPosition = "精英"; break;
            case 2: _officialPosition = "副团长"; break;
            case 3: _officialPosition = "团长"; break;
        }
        return _officialPosition;
    }
    public string GetLegionLimitByState(int stateValue1)
    {
        string _Str = "";
        switch (stateValue1)
        {
            case 1: _Str = "申请自动加入"; break;
            case 0: _Str = "需审核才能加入"; break;
            case -1: _Str = "禁止任何人加入"; break;
        }
        return _Str;
    }
    public string GetLegionLevelLimitByState(int stateValue2)
    {
        string _Str = "";
        switch (stateValue2)
        {
            case 0: _Str = "无限制"; break;
            default: _Str = stateValue2 + "级"; break;
        }
        return _Str;
    }
    #endregion

    #region 取得目标战队信息
    public TargetPlayerInfo targetPlayerInfo;
    #endregion

    #region ServerList
    public BetterList<ServerList> ServerLists = new BetterList<ServerList>();
    public void AddServerLists(ServerList serverList)
    {
        ServerLists.Add(serverList);
    }

    public ServerList GetServerListsByID(string _ServerTag)
    {
        foreach (var item in TextTranslator.instance.ServerLists)
        {
            if (item.ServerTag == _ServerTag)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region DownloadList
    public BetterList<DownloadList> DownloadLists = new BetterList<DownloadList>();
    public void AddDownloadLists(DownloadList downloadList)
    {
        DownloadLists.Add(downloadList);
    }

    public string GetServerListsByPlatform(string _Platform)
    {
        foreach (var item in TextTranslator.instance.DownloadLists)
        {
            if (item.Platform == _Platform)
            {
                return item.Url;
            }
        }
        return "";
    }
    #endregion

    #region ShopCenter
    public Dictionary<int, ShopCenter> ShopCenterDic = new Dictionary<int, ShopCenter>();

    public void AddShopCenterDic(int id, ShopCenter _ShopCenter)
    {
        ShopCenterDic.Add(id, _ShopCenter);
    }
    public ShopCenter GetShopCenterByID(int id)
    {
        if (ShopCenterDic.ContainsKey(id))
        {
            return ShopCenterDic[id];
        }
        return null;
    }
    #endregion

    #region ShopCenterPeculiar
    public Dictionary<int, ShopCenterPeculiar> ShopCenterPeculiarDic = new Dictionary<int, ShopCenterPeculiar>();

    public void AddShopCenterPeculiarDic(int id, ShopCenterPeculiar _ShopCenterPeculiar)
    {
        ShopCenterPeculiarDic.Add(id, _ShopCenterPeculiar);
    }
    public ShopCenterPeculiar GetShopCenterPeculiarByID(int id)
    {
        if (ShopCenterPeculiarDic.ContainsKey(id))
        {
            return ShopCenterPeculiarDic[id];
        }
        return null;
    }
    #endregion

    #region HappyBox
    public BetterList<HappyBox> HappyBoxList = new BetterList<HappyBox>();
    public void AddHappyBoxList(HappyBox _HappyBox)
    {
        HappyBoxList.Add(_HappyBox);
    }

    public HappyBox GetHappyBoxListByID(int Id)
    {
        foreach (var item in TextTranslator.instance.HappyBoxList)
        {
            if (item.HappyBoxID == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region TeamGate
    public BetterList<TeamGate> TeamGateList = new BetterList<TeamGate>();
    public void AddTeamGateList(TeamGate _TeamGate)
    {
        TeamGateList.Add(_TeamGate);
    }

    public TeamGate GetTeamGateListByID(int Id)
    {
        foreach (var item in TextTranslator.instance.TeamGateList)
        {
            if (item.TeamGateID == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region WorldBossReward
    public BetterList<WorldBossReward> WorldBossRewardList = new BetterList<WorldBossReward>();
    public void AddWorldBossRewardList(WorldBossReward _WorldBossReward)
    {
        WorldBossRewardList.Add(_WorldBossReward);
    }

    public WorldBossReward GetWorldBossRewardListID(int Id)
    {
        foreach (var item in TextTranslator.instance.WorldBossRewardList)
        {
            if (item.WorldBossID == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region WorldBoss
    public BetterList<WorldBoss> WorldBossList = new BetterList<WorldBoss>();
    public void AddWorldBossList(WorldBoss _WorldBoss)
    {
        WorldBossList.Add(_WorldBoss);
    }

    public WorldBoss GetWorldBossByID(int Id)
    {
        foreach (var item in TextTranslator.instance.WorldBossList)
        {
            if (item.MonsterLevel == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region KingRoad
    public BetterList<KingRoad> KingRoadList = new BetterList<KingRoad>();
    public void AddKingRoadList(KingRoad _KingRoad)
    {
        KingRoadList.Add(_KingRoad);
    }

    public KingRoad GetKingRoadByID(int Id)
    {
        foreach (var item in TextTranslator.instance.KingRoadList)
        {
            if (item.KingRank == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region LegionCrap
    public BetterList<LegionCrap> LegionCrapList = new BetterList<LegionCrap>();
    public void AddLegionCrapList(LegionCrap _LegionCrap)
    {
        LegionCrapList.Add(_LegionCrap);
    }

    public LegionCrap GetLegionCrapByID(int Id)
    {
        foreach (var item in TextTranslator.instance.LegionCrapList)
        {
            if (item.CrapsType == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region LegionCity
    public BetterList<LegionCity> LegionCityList = new BetterList<LegionCity>();
    public void AddLegionCityList(LegionCity _LegionCity)
    {
        LegionCityList.Add(_LegionCity);
    }

    public LegionCity GetLegionCityByID(int Id)
    {
        foreach (var item in TextTranslator.instance.LegionCityList)
        {
            if (item.CityID == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region ControlGateOpen
    public BetterList<ControlGateOpen> ControlGateOpenList = new BetterList<ControlGateOpen>();
    public void AddControlGateOpenList(ControlGateOpen _ControlGateOpen)
    {
        ControlGateOpenList.Add(_ControlGateOpen);
    }

    public ControlGateOpen GetControlGateOpenByID(int Id)
    {
        foreach (var item in TextTranslator.instance.ControlGateOpenList)
        {
            if (item.ID == Id)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region  LegionRedBag
    public BetterList<LegionRedBag> LegionRedBagList = new BetterList<LegionRedBag>();
    public void AddLegionRedBagList(LegionRedBag _LegionRedBag)
    {
        LegionRedBagList.Add(_LegionRedBag);
    }

    public LegionRedBag GetLegionRedBagByID(int Id)
    {
        foreach (var item in TextTranslator.instance.LegionRedBagList)
        {
            if (item.LegionRedID == Id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion


    #region GachaPreview
    public BetterList<GachaPreview> GachaPreviewList = new BetterList<GachaPreview>();
    public void AddGachaPreviewList(GachaPreview _GachaPreview)
    {
        GachaPreviewList.Add(_GachaPreview);
    }

    public GachaPreview GetGachaPreviewByID(int Id)
    {
        foreach (var item in TextTranslator.instance.GachaPreviewList)
        {
            if (item.ID == Id)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region Exp
    public class ExpsHero
    {
        public static ExpsHero instence;
        /// <summary>
        /// 角色的等级
        /// </summary>
        public int Level;
        /// <summary>
        /// 玩家升级所需的经验
        /// </summary>
        public int PlayerExp;

        /// <summary>
        /// 角色升级所需要的经验
        /// </summary>
        public int RoleExp2;

        public int StaminaCap;
        public int StaminaMaxCap;

        public int EnergyCap;
        public int EnergyMaxCap;
        public int RoleExp3;
        public int RoleExp4;
        public int RoleExp5;
        public int RoleExp6;

        public int BuyGoldNum;

        public int StaminaBonus;
    }
    public List<ExpsHero> ExpsheroList = new List<ExpsHero>();
    public void ExpsHeroInfo(int _level, int _playerExp, int _roleExp2, int _staminaCap, int _staminaMaxCap, int _energyCap, int _energyMaxCap, int _roleExp3, int _roleExp4, int _roleExp5, int _roleExp6, int _buyGoldNum, int _staminaBonus)
    {
        ExpsHero expsHero = new ExpsHero();
        expsHero.Level = _level;
        expsHero.PlayerExp = _playerExp;
        expsHero.RoleExp2 = _roleExp2;
        expsHero.StaminaCap = _staminaCap;
        expsHero.StaminaMaxCap = _staminaMaxCap;
        expsHero.EnergyCap = _energyCap;
        expsHero.EnergyMaxCap = _energyMaxCap;
        expsHero.RoleExp3 = _roleExp3;
        expsHero.RoleExp4 = _roleExp4;
        expsHero.RoleExp5 = _roleExp5;
        expsHero.RoleExp6 = _roleExp6;
        expsHero.BuyGoldNum = _buyGoldNum;
        expsHero.StaminaBonus = _staminaBonus;
        TextTranslator.instance.ExpsheroList.Add(expsHero);
    }
    /// <summary>
    /// 通过等级获取当前等级所需要的不同经验
    /// </summary>
    /// <param name="nowLevel">玩家或者角色的等级</param>
    /// <returns>ExpsHero对象</returns>
    public ExpsHero GetExpsHeroInfoByLevel(int nowLevel)
    {
        foreach (var item in ExpsheroList)
        {
            if (item.Level == nowLevel)
            {
                return item;
            }
        }
        return null;
    }
    #endregion
    #region Wish
    public class WishInfo
    {
        public int WishID;
        public int ItemID;
        public int ItemNum;
        public int MaxCrit;
    }
    public List<WishInfo> WishInfoList = new List<WishInfo>();

    public void WishItemInfo(int _WishID, int _ItemID, int _ItemNum, int _MaxCrit)
    {
        WishInfo wishInfo = new WishInfo();
        wishInfo.WishID = _WishID;
        wishInfo.ItemID = _ItemID;
        wishInfo.ItemNum = _ItemNum;
        wishInfo.MaxCrit = _MaxCrit;
        WishInfoList.Add(wishInfo);
    }
    /// <summary>
    /// 获取许愿的对象
    /// </summary>
    /// <param name="wishId"></param>
    /// <returns></returns>
    public WishInfo GetWishInfobyWishID(int wishId)
    {
        foreach (var item in WishInfoList)
        {
            if (item.WishID == wishId)
            {
                return item;
            }
        }
        return null;
    }
    #endregion
    #region LegionWeak
    public class LegionWeak
    {
        /// <summary>
        /// 号码Num
        /// </summary>
        public int WinNum { get; set; }
        /// <summary>
        /// 虚弱度
        /// </summary>
        public float Decrease { get; set; }
    }
    /// <summary>
    /// LegionWeakList 的集合
    /// </summary>
    public List<LegionWeak> LegionWeakList = new List<LegionWeak>();
    /// <summary>
    /// 添加LegionWeakList集合
    /// </summary>
    /// <param name="_WinNum">号码Num</param>
    /// <param name="_Decrease">虚弱度</param>
    public void LegionWeakInfo(int _WinNum, float _Decrease)
    {
        LegionWeak _LegionWeak = new LegionWeak();
        _LegionWeak.WinNum = _WinNum;
        _LegionWeak.Decrease = _Decrease;
        LegionWeakList.Add(_LegionWeak);
    }
    /// <summary>
    /// 通过虚弱度获取LegionWeak
    /// </summary>
    /// <param name="_Decrease">虚弱度</param>
    /// <returns>LegionWeak</returns>
    public LegionWeak GetLegionWeakByDecrease(float _Decrease)
    {
        foreach (var item in LegionWeakList)
        {
            if (item.Decrease == _Decrease)
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    /// 通过号码Num获取LegionWeak
    /// </summary>
    /// <param name="_WinNum">号码Num</param>
    /// <returns>LegionWeak</returns>
    public float GetLegionWeakByWinNum(int _WinNum)
    {
        if (_WinNum > 15)
        {
            _WinNum = 15;
        }
        foreach (var item in LegionWeakList)
        {
            if (item.WinNum == _WinNum)
            {
                return item.Decrease;
            }
        }
        return 0;
    }
    #endregion

    #region RoleGrow
    public BetterList<RoleGrow> RoleGrowList = new BetterList<RoleGrow>();
    public void AddRoleGrowList(RoleGrow roleGrow)
    {
        RoleGrowList.Add(roleGrow);
    }
    public RoleGrow GetRoleGrowInfoByRoleId(int roleId, int rank)
    {
        RoleGrow roleGrow = null;
        for (int i = 0; i < RoleGrowList.size; i++)
        {
            if (RoleGrowList[i].RoleID == roleId && rank == RoleGrowList[i].Star)
            {
                roleGrow = RoleGrowList[i];
            }
        }
        return roleGrow;
    }
    #endregion
    #region  ActivitiesCenter 日常活动开放列表
    public BetterList<ActivitiesCenter> ActivitiesCenterDic = new BetterList<ActivitiesCenter>();
    public void AddActivitiesCenterDic(ActivitiesCenter _ActivitiesCenter)
    {
        ActivitiesCenterDic.Add(_ActivitiesCenter);
    }

    public ActivitiesCenter GetActivitiesCenterDicByID(int id)
    {
        foreach (var item in ActivitiesCenterDic)
        {
            if (item.ActivityID == id)
            {
                return item;
            }
        }

        return null;
    }
    #endregion

    #region  ActivityDayExchange每日充值送好礼
    public Dictionary<int, ActivityDayExchange> ActivityDayExchangeDic = new Dictionary<int, ActivityDayExchange>();
    public void AddActivityDayExchangeDic(int id, ActivityDayExchange _ActivityDayExchange)
    {
        ActivityDayExchangeDic.Add(id, _ActivityDayExchange);
    }

    public ActivityDayExchange GetActivityDayExchangeDicByID(int id)
    {
        if (ActivityDayExchangeDic.ContainsKey(id))
        {
            return ActivityDayExchangeDic[id];
        }
        return null;
    }
    #endregion
    #region  ActivityExchange 充值活动
    public BetterList<ActivityExchange> ActivityExchangeDic = new BetterList<ActivityExchange>();

    public void AddActivityExchangeDic(int _ExchangeID, int _ActivityID, int _GroupID, string _Des, int _Type, int _Condition, int _Point,
        int _ItemID1, int _ItemNum1, int _ItemID2, int _ItemNum2, int _ItemID3, int _ItemNum3, int _ItemID4, int _ItemNum4)
    {
        ActivityExchangeDic.Add(new ActivityExchange(_ExchangeID, _ActivityID, _GroupID, _Des, _Type, _Condition, _Point,
         _ItemID1, _ItemNum1, _ItemID2, _ItemNum2, _ItemID3, _ItemNum3, _ItemID4, _ItemNum4));
    }
    public ActivityExchange GetActivityExchangeDicByID(int _ActivityID, int _GroupID)
    {
        foreach (ActivityExchange exchange in ActivityExchangeDic)
        {
            if (exchange.ActivityID == _ActivityID && exchange.GroupID == _GroupID)
            {
                return exchange;
            }

        }
        return null;
    }
    #endregion
    #region  ActivityGrowthFund  全民基金
    public Dictionary<int, ActivityGrowthFund> ActivityGrowthFundDic = new Dictionary<int, ActivityGrowthFund>();
    public void AddActivityGrowthFundDic(int id, ActivityGrowthFund _ActivityGrowthFund)
    {
        ActivityGrowthFundDic.Add(id, _ActivityGrowthFund);
    }

    public ActivityGrowthFund GetActivityGrowthFundDicByID(int id)
    {
        if (ActivityGrowthFundDic.ContainsKey(id))
        {
            return ActivityGrowthFundDic[id];
        }
        return null;
    }

    public int GetActivityGrowthFundDicLengthByID(int TypeID)
    {
        int count = 1;
        int num = 0;
        while (ActivityGrowthFundDic.ContainsKey(count))
        {
            if (ActivityGrowthFundDic[count].Type == TypeID)
            {
                num++;
            }
            count++;
        }
        return num;
    }
    #endregion
    #region ActivityBonusVip 登陆送VIP
    public Dictionary<int, ActivityBonusVip> ActivityBonusVipDic = new Dictionary<int, ActivityBonusVip>();

    public void AddActivityBonusVipDic(int id, ActivityBonusVip _ActivityBonusVip)
    {
        ActivityBonusVipDic.Add(id, _ActivityBonusVip);
    }
    public ActivityBonusVip GetActivityBonusVipDicByID(int id)
    {
        if (ActivityBonusVipDic.ContainsKey(id))
        {
            return ActivityBonusVipDic[id];
        }
        return null;
    }
    public int GetActivityBonusVipDicLength()
    {
        int count = 1;
        while (ActivityBonusVipDic.ContainsKey(count))
        {
            count++;
        }
        return count;
    }
    public int GetActivityBonusVipDicIndexByDayID(int DayID)
    {
        int index = 1;
        while (ActivityBonusVipDic.ContainsKey(index))
        {
            if (index == GetActivityBonusVipDicLength())
            {
                if (ActivityBonusVipDic[index].Param1 <= DayID)
                {
                    return index;
                }
            }
            else
            {
                if (ActivityBonusVipDic[index].Param1 <= DayID && ActivityBonusVipDic[index + 1].Param1 > DayID)
                {
                    return index;
                }
            }
            index++;
        }
        return 0;

    }
    #endregion
    #region  ActivityItemFixed  活动通用兑换界面
    public BetterList<ActivityItemFixed> ActivityItemFixedDic = new BetterList<ActivityItemFixed>();
    public static List<ActivityItemFixed> ActivityItemFixedList = new List<ActivityItemFixed>();
    public void AddActivityItemFixedDic(int _ItemFixedID, int _ActivityID, int _Round, int _Type, int _ConvertType, int _Param1, int _Param2, string _Des, int _ConvertCount,
        int _ItemID1, int _ItemNum1, int _ItemID2, int _ItemNum2, int _ItemID3, int _ItemNum3, int _ItemID4, int _ItemNum4)
    {
        ActivityItemFixedDic.Add(new ActivityItemFixed(_ItemFixedID, _ActivityID, _Round, _Type, _ConvertType, _Param1, _Param2, _Des, _ConvertCount,
         _ItemID1, _ItemNum1, _ItemID2, _ItemNum2, _ItemID3, _ItemNum3, _ItemID4, _ItemNum4));
    }
    public ActivityItemFixed GetActivityItemFixedDicByID(int _ActivityID, int _Round, int _ConvertType)
    {
        foreach (ActivityItemFixed ItemFixed in ActivityItemFixedDic)
        {
            if (ItemFixed.ActivityID == _ActivityID && ItemFixed.Round == _Round && ItemFixed.ConvertType == _ConvertType)
            {
                return ItemFixed;
            }

        }
        return null;
    }
    public ActivityItemFixed GetActivityItemFixedDicByID(int _ActivityID, int _Round)
    {
        foreach (ActivityItemFixed ItemFixed in ActivityItemFixedDic)
        {
            if (ItemFixed.ActivityID == _ActivityID && ItemFixed.Round == _Round)
            {
                return ItemFixed;
            }
        }
        return null;
    }
    public int GetActivityItemFixedDicLength(int _ActivityID, int _Round)
    {
        int count = 0;
        ActivityItemFixedList.Clear();
        foreach (ActivityItemFixed ItemFixed in ActivityItemFixedDic)
        {
            if (ItemFixed.ActivityID == _ActivityID && ItemFixed.Round == _Round)
            {
                count++;
                ActivityItemFixedList.Add(ItemFixed);
            }
        }
        return count;
    }
    public ActivityItemFixed GetActivityItemFixedDicByIndex(int _index)
    {
        foreach (ActivityItemFixed ItemFixed in ActivityItemFixedDic)
        {
            if (ItemFixed.ItemFixedID == _index)
            {
                return ItemFixed;
            }

        }
        return null;
    }
    #endregion
    #region 活动显示红将排行
    public BetterList<ActivityGachaHeroRank> ActivityGachaHeroRankDic = new BetterList<ActivityGachaHeroRank>();
    public void AddActivityGachaHeroRankDic(ActivityGachaHeroRank _ActivityGachaHeroRank)
    {
        ActivityGachaHeroRankDic.Add(_ActivityGachaHeroRank);
    }

    public ActivityGachaHeroRank GetActivityGachaHeroRankDicByID(int ID, int ActivityID)
    {

        foreach (var item in ActivityGachaHeroRankDic)
        {
            if (item.Rank == ID && item.ActivityID == ActivityID)
            {
                return item;
            }
        }
        return null;
    }
    public int GetActivityGachaHeroRankDicLength(int ActivityID)
    {
        int count = 1;
        foreach (var item in ActivityGachaHeroRankDic)
        {
            if (item.Rank == count && item.ActivityID == ActivityID)
            {
                count++;
            }
        }
        return count;
    }
    #endregion
    #region 活动显示红将积分宝箱
    public BetterList<ActivityGachaHeroPoint> ActivityGachaHeroPointDic = new BetterList<ActivityGachaHeroPoint>();
    public void AddActivityGachaHeroPointDic(ActivityGachaHeroPoint _ActivityGachaHeroPoint)
    {
        ActivityGachaHeroPointDic.Add(_ActivityGachaHeroPoint);
    }

    public ActivityGachaHeroPoint GetActivityGachaHeroPointDicByID(int ID, int ActivityID)
    {
        foreach (var item in ActivityGachaHeroPointDic)
        {
            if (item.Rank == ID && item.ActivityID == ActivityID)
            {
                return item;
            }
        }
        return null;
    }
    #endregion
    #region 夺宝积分IndianaPoint
    public Dictionary<int, IndianaPoint> IndianaPointDic = new Dictionary<int, IndianaPoint>();
    public void AddIndianaPointDic(int id, IndianaPoint _IndianaPoint)
    {
        IndianaPointDic.Add(id, _IndianaPoint);
    }

    public IndianaPoint GetIndianaPointByID(int id)
    {
        if (IndianaPointDic.ContainsKey(id))
        {
            return IndianaPointDic[id];
        }
        return null;
    }
    #endregion

    #region PrivateChat

    public BetterList<PrivateChatItemData> PrivateChatItemDataList = new BetterList<PrivateChatItemData>();
    public void AddChatItemData(PrivateChatItemData _PrivateChatItemData)
    {
        PrivateChatItemDataList.Add(_PrivateChatItemData);
    }
    /* public List<ChatItemData> GetChatItemDataListByID(int channel)
     {
         if (ChatItemDataDic.ContainsKey(channel))
         {
             ChatItemDataList.Add(ChatItemDataDic[channel].);
         }
         return ChatItemDataList;
     }*/
    #endregion

    #region Nation
    public BetterList<Nation> NationList = new BetterList<Nation>();
    public void AddNationList(Nation _Nation)
    {
        NationList.Add(_Nation);
    }

    public Nation GetNationByID(int Id)
    {
        foreach (var item in NationList)
        {
            if (item.ID == Id)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region BattlefieldPoints
    public BetterList<BattlefieldPoints> BattlefieldPointsList = new BetterList<BattlefieldPoints>();
    public void AddBattlefieldPointsList(BattlefieldPoints _BattlefieldPoints)
    {
        BattlefieldPointsList.Add(_BattlefieldPoints);
    }

    public BattlefieldPoints GetBattlefieldPointsByID(int Id)
    {
        foreach (var item in BattlefieldPointsList)
        {
            if (item.ID == Id)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region BattlefieldKill
    public BetterList<BattlefieldKill> BattlefieldKillList = new BetterList<BattlefieldKill>();
    public void AddBattlefieldKillList(BattlefieldKill _BattlefieldKill)
    {
        BattlefieldKillList.Add(_BattlefieldKill);
    }

    public BattlefieldKill GetBattlefieldKillByID(int Id)
    {
        foreach (var item in BattlefieldKillList)
        {
            if (item.ID == Id)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region ArmsDealers
    public BetterList<ArmsDealers> ArmsDealersList = new BetterList<ArmsDealers>();
    public void AddArmsDealersList(ArmsDealers _ArmsDealers)
    {
        ArmsDealersList.Add(_ArmsDealers);
    }

    public ArmsDealers GetArmsDealersByID(int Id)
    {
        foreach (var item in ArmsDealersList)
        {
            if (item.ArmsDealerID == Id)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region SmallGoal
    public BetterList<SmallGoal> SmallGoalList = new BetterList<SmallGoal>();
    public void AddSmallGoalList(SmallGoal _SmallGoal)
    {
        SmallGoalList.Add(_SmallGoal);
    }

    public SmallGoal GetSmallGoalByID(int Id)
    {
        foreach (var item in SmallGoalList)
        {
            if (item.SmallGoalID == Id)
            {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region ResourceTycoon
    public BetterList<ResourceTycoon> ResourceTycoonList = new BetterList<ResourceTycoon>();
    public void AddResourceTycoonList(ResourceTycoon _ResourceTycoon)
    {
        ResourceTycoonList.Add(_ResourceTycoon);
    }

    public ResourceTycoon GetResourceTycoonByID(int Id)
    {
        foreach (var item in ResourceTycoonList)
        {
            if (item.ResourceTycoonID == Id)
            {
                return item;
            }
        }
        return null;
    }

    public void SetResourceTycoonStateByID(int Id, int GetState, int GetNum)
    {
        ResourceTycoon RT = GetResourceTycoonByID(Id);
        if (RT != null)
        {
            RT.AddAwardState(GetState, GetNum);
        }
    }
    #endregion

    #region NuclearPowerPlan
    public BetterList<NuclearPowerPlan> NuclearPowerPlanList = new BetterList<NuclearPowerPlan>();
    public void AddNuclearPowerPlanList(int EncouragingTimestId, int DiamondsValue, int AddForce, int AddOneHeroAtk, int AddOneHeroHp)
    {
        NuclearPowerPlanList.Add(new NuclearPowerPlan(EncouragingTimestId, DiamondsValue, AddForce, AddOneHeroAtk, AddOneHeroHp));
    }
    public NuclearPowerPlan GetNuclearPlanByID(int num)
    {
        foreach (var item in NuclearPowerPlanList)
        {
            if (item.EncouragingTimestId == num)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region Chip
    public BetterList<Chip> ChipList = new BetterList<Chip>();
    //<Chip ChipID="1" Color="1" SlotNum="1" EffectType1="1" EffectVal1="1000" EffectType2="0" EffectVal2="0" EffectType3="0" EffectVal3="0" Des="全体加生命+1000" />
    public void AddChipList(int _ChipID, int _Color, int _SlotNum, int _EffectType1, float _EffectVal1, int _EffectType2, float _EffectVal2, int _EffectType3, float _EffectVal3, string _Des)
    {
        ChipList.Add(new Chip(_ChipID, _Color, _SlotNum, _EffectType1, _EffectVal1, _EffectType2, _EffectVal2, _EffectType3, _EffectVal3, _Des));
    }
    public Chip GetChipListByID(int _Color, int _SlotNum)
    {
        foreach (var item in ChipList)
        {
            if (item.Color == _Color && item.SlotNum == _SlotNum)
            {
                return item;
            }
        }
        return null;
    }
    public Chip GetChipListByChipID(int _ChipID)
    {
        foreach (var item in ChipList)
        {
            if (item.ChipID == _ChipID)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region Recharge
    public void Recharge(int echargeId)
    {
        Exchange exchange = TextTranslator.instance.GetExchangeById(echargeId);
        if (exchange != null)
        {
            int exchangeId = exchange.exchangeID;
            if (exchangeId == 7)
            {
                Debug.Log("MonthCardDay " + CharacterRecorder.instance.MonthCardDay);
                if (CharacterRecorder.instance.MonthCardDay == 0 || CharacterRecorder.instance.MonthCardDay > 30)
                {
                    //NetworkHandler.instance.SendProcess("9501#" + exchangeId.ToString() + ";");
                }
                else
                {
                    UIManager.instance.OpenPromptWindow("亲,月卡时间还没有结束呢", PromptWindow.PromptType.Hint, null, null);
                    return;
                }
            }
            else if (exchangeId == 8)
            {
                if (exchange.isfristDiamond == false)
                {
                    //NetworkHandler.instance.SendProcess("9501#" + exchangeId.ToString() + ";");
                }
                else
                {
                    UIManager.instance.OpenPromptWindow("亲,您已经拥有万年卡了", PromptWindow.PromptType.Hint, null, null);
                    return;
                }
            }
        }

#if MY_WAR_DEBUG || UNITY_EDITOR
#if UNITY_IOSOFFCIAL
				Exchange mItem = TextTranslator.instance.GetExchangeById(echargeId);
				if(mItem!=null){
					Unibiller.initiatePurchase(Unibiller.GetPurchasableItemById(SetIOSBill(mItem.exchangeID)));//充值
				}
#else
        NetworkHandler.instance.SendProcess("9501#" + echargeId.ToString() + ";");

        //HolagamesSDK.getInstance().Pay("1_1_10001_xxx_1_1_60钻石");//BAiDU diamond, zoneid, guid, name, level, paytype, ProductName
#endif
#elif UNITY_ANDROID
                Exchange mItem = TextTranslator.instance.GetExchangeById(echargeId);
                if (mItem != null)
                {   
#if JINLI
                APaymentHelperDemo.instance.Pay(mItem.cash*100, mItem.exchangeName, 1, string.Format("{0}-{1}-{2}-{3}", CharacterRecorder.instance.userId.ToString(), PlayerPrefs.GetString("ServerID"),echargeId.ToString(),mItem.cash.ToString())); ;
#elif UC
                HolagamesSDK.getInstance().Pay(mItem.cash + "_" + PlayerPrefs.GetString("ServerID") + "_" + CharacterRecorder.instance.userId.ToString() + "_" + CharacterRecorder.instance.characterName + "_" + CharacterRecorder.instance.level.ToString() + "_" + echargeId.ToString());  //UC diamond, zoneid, guid, name, level, paytype
#elif WDJ
                HolagamesSDK.getInstance().Pay(mItem.cash + "_" + PlayerPrefs.GetString("ServerID") + "_" + CharacterRecorder.instance.userId.ToString() + "_" + CharacterRecorder.instance.characterName + "_" + CharacterRecorder.instance.level.ToString() + "_" + echargeId.ToString());  //UC diamond, zoneid, guid, name, level, paytype
#elif XIAOMI
                HolagamesSDK.getInstance().Pay(mItem.cash + "_" + PlayerPrefs.GetString("ServerID") + "_" + CharacterRecorder.instance.userId.ToString() + "_" + CharacterRecorder.instance.characterName + "_" + CharacterRecorder.instance.level.ToString() + "_" + mItem.exchangeName + "_" + mItem.exchangeID + "_vip" + CharacterRecorder.instance.Vip.ToString() + "_" + CharacterRecorder.instance.userId.ToString() + UnityEngine.Random.Range(1, 10000) + "_" + CharacterRecorder.instance.userId.ToString() + "-" + PlayerPrefs.GetString("ServerID") + "-" + mItem.exchangeID + "-" + mItem.cash);  //XIAOMI diamond, zoneid, guid, name, level, Product_Name, paytype, vip0, orderid, ext
#elif QQ
                HolagamesSDK.getInstance().Pay("1_" + mItem.cash * 10 + "_" + CharacterRecorder.instance.userId.ToString() + "_" + PlayerPrefs.GetString("ServerID") + "_" + echargeId.ToString()); //QQ zoneid, diamond, guid, serverid, paytype		
#elif QIHOO360
                HolagamesSDK.getInstance().Pay(mItem.cash * 100 + "_" + mItem.exchangeName + "_" + mItem.exchangeID + "_" + CharacterRecorder.instance.characterName + "_" + CharacterRecorder.instance.userId.ToString() + "_" + CharacterRecorder.instance.userId.ToString() + UnityEngine.Random.Range(1, 10000) + "_" + CharacterRecorder.instance.userId.ToString() + "-" + PlayerPrefs.GetString("ServerID") + "-" + mItem.exchangeID + "-" + mItem.cash);  //360  Product_Price Product_Name Product_Id Role_Name Role_Id OrderId guid-server_id-pay_type-cash
#elif OPPO || VIVO || MEIZU || HUAWEI
                HolagamesSDK.getInstance().Pay(mItem.cash + "_" + PlayerPrefs.GetString("ServerID") + "_" + CharacterRecorder.instance.userId.ToString() + "_" + CharacterRecorder.instance.characterName + "_" + CharacterRecorder.instance.level.ToString() + "_" + echargeId.ToString() + "_" + mItem.exchangeName);  //OPPO diamond, zoneid, guid, name, level, paytype, ProductName
#elif HOLA || QIANHUAN
                HolagamesSDK.getInstance().Pay(mItem.cash + "_" + PlayerPrefs.GetString("ServerID") + "_" + CharacterRecorder.instance.userId.ToString() + "_" + CharacterRecorder.instance.characterName + "_" + CharacterRecorder.instance.level.ToString() + "_" + echargeId.ToString() + "_" + mItem.exchangeName+"_"+ PlayerPrefs.GetString("ServerName"));  //OPPO diamond, zoneid, guid, name, level, paytype, ProductName, Servername
#elif BAIDU
                HolagamesSDK.getInstance().Pay(mItem.cash + "_" + PlayerPrefs.GetString("ServerID") + "_" + CharacterRecorder.instance.userId.ToString() + "_" + CharacterRecorder.instance.characterName + "_" + CharacterRecorder.instance.level.ToString() + "_" + echargeId.ToString() + "_" + mItem.exchangeName);  //OPPO diamond, zoneid, guid, name, level, paytype, ProductName
#else
                    HolagamesSDK.getInstance().Pay(PlayerPrefs.GetString("ServerID") + "_" + mItem.cash * 10 + "_" + CharacterRecorder.instance.userId.ToString() + "_" + PlayerPrefs.GetString("ServerID") + "_" + echargeId.ToString());
#endif



                }
#elif UNITY_IOSOFFCIAL
				Exchange mItem = TextTranslator.instance.GetExchangeById(echargeId);
				if(mItem!=null){
				Unibiller.initiatePurchase(Unibiller.GetPurchasableItemById(SetIOSBill(mItem.exchangeID)));//充值
				}
#elif KY
                Exchange mItem = TextTranslator.instance.GetExchangeById(echargeId);

                if (mItem != null)
                {                    
                    APaymentHelperDemo.instance.Pay(mItem.cash*100, mItem.exchangeName, 1, string.Format("{0}-{1}-{2}-{3}", CharacterRecorder.instance.userId.ToString(), PlayerPrefs.GetString("ServerID"),echargeId.ToString(),mItem.cash.ToString())); ;
                }
#endif
    }

    public string SetIOSBill(int itemType)
    {
        string itemId = "";
        switch (itemType)
        {
            case 1:
                itemId = "com.qianhuan.yxgsd.ios.d60"; break;
            case 2:
                itemId = "com.qianhuan.yxgsd.ios.d300"; break;
            case 3:
                itemId = "com.qianhuan.yxgsd.ios.d980"; break;
            case 4:
                itemId = "com.qianhuan.yxgsd.ios.d1980"; break;
            case 5:
                itemId = "com.qianhuan.yxgsd.ios.d3280"; break;
            case 6:
                itemId = "com.qianhuan.yxgsd.ios.d6480"; break;
            case 7:
                itemId = "com.qianhuan.yxgsd.ios.m300"; break;
            case 8:
                itemId = "com.qianhuan.yxgsd.ios.m980"; break;
            case 9:
                itemId = "com.qianhuan.yxgsd.ios.d10"; break;
        }
        return itemId;
    }
    #endregion

    #region GC
    public void DoGC()
    {
        Resources.UnloadUnusedAssets();
        //System.GC.Collect();
    }
    #endregion
}

public class TargetPlayerInfo
{
    //战队id;头像id;战队姓名;等级;vip;战力;竟技场排名;军团名称;军团职位;捐献;角色唯一ID $等级$军衔$颜色!...角色唯一ID $等级$军衔$颜色!;
    public int uId { get; set; }
    public int headIcon { get; set; }
    public string name { get; set; }
    public int level { get; set; }
    public int vip { get; set; }
    public int fight { get; set; }
    public int pvpRank { get; set; }
    public string legionName { get; set; }
    public int legionPosition { get; set; }
    public int contribute { get; set; }

    public BetterList<RoleInfoOfTargetPlayer> roleList = new BetterList<RoleInfoOfTargetPlayer>();
    public TargetPlayerInfo(int uid, int headIcon, string name, int level, int vip, int fight, int pvpRank, string legionName, int legionPosition, int contribute, BetterList<RoleInfoOfTargetPlayer> roleList)
    {
        this.uId = uid;
        this.headIcon = headIcon;
        this.name = name;
        this.level = level;
        this.vip = vip;
        this.fight = fight;
        this.pvpRank = pvpRank;
        this.legionName = legionName;
        this.legionPosition = legionPosition;
        this.contribute = contribute;
        this.roleList = roleList;
    }
}



public class RoleInfoOfTargetPlayer
{
    public int roleId { get; set; }
    public int roleLevel { get; set; }
    public int roleJunXian { get; set; }//军衔
    public int color { get; set; }//classNum 颜色枚举 1 - 20
    public int characterId { get; set; }
    public RoleInfoOfTargetPlayer(int roleId, int roleLevel, int roleJunXian, int color, int _characterId)
    {
        this.roleId = roleId;
        this.roleLevel = roleLevel;
        this.roleJunXian = roleJunXian;
        this.color = color;
        this.characterId = _characterId;

    }
}
