using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Holagames;

public class MouseClick : MonoBehaviour
{
    public Vector3 MapCameraPosition = Vector3.zero;
    public bool IsWorldEvev = false;
    public bool IsResource = false;
    public bool IsEnemy = false;
    public bool IsAction = false;
    Vector3 MouseClickPostion = new Vector3();
    public float MoveSpeed;
    public static MouseClick instance;
    public Animator RoleAnimator;

    public GameObject TextureTouch;
    public GameObject TextureMove;
    public GameObject DragObject;

    float ScreenHeight;
    float ScreenWidth;

    public float TouchPosition;
    public float TouchPositionY;
    public float MovePosition;
    public float MovePositionY;

    public int AutoMode = 0;
    int PositionID = 0;
    public float MoveOffect = 0;
    public float MoveOffectY = 0;
    float MoveDistance = 0;
    float MoveDistanceY = 0;

    int RoleIndex = -1;

    bool IsOut = false;
    public bool IsLock = false;

    public GameObject LabelEffectPrefab;
    public GameObject FightTrail;

    float TotalDistance = 0;
    float SingleDistance = 0;

    int ScopeCount = 0;

    Vector3 StartPosition = Vector3.zero;
    Vector3 NowPosition = Vector3.zero;
    Vector3 EndPosition = Vector3.zero;
    Vector3 LastPosition = Vector3.zero;
    bool IsLastSingle = true;

    void Start()
    {
        MoveSpeed = 0.15f;
        instance = this;

        ScreenHeight = Screen.height / 2;
        ScreenWidth = Screen.width / 2;

        AutoMode = 0;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
#if UC || OPPO || BAIDU || WDJ
        HolagamesSDK.getInstance().exitSDK();    
        return;

#elif HOLA || QIHOO360 || QIANHUAN
        if (CharacterRecorder.instance.userId > 0)
        {
            Dictionary<string, string> mDic = new Dictionary<string, string>();
            mDic.Add("roleId", CharacterRecorder.instance.userId.ToString());
            mDic.Add("roleName", CharacterRecorder.instance.characterName);
            mDic.Add("roleLevel", CharacterRecorder.instance.level.ToString());
            mDic.Add("zoneId", PlayerPrefs.GetString("ServerID"));
            mDic.Add("zoneName", PlayerPrefs.GetString("ServerName"));
            mDic.Add("roleCTime", CharacterRecorder.CTime);
            mDic.Add("roleLevelMTime", Utils.GetNowTimeUTC().ToString());
            mDic.Add("vip", CharacterRecorder.instance.Vip.ToString());
            HolagamesSDK.getInstance().loginGameRole("exit", HolagamesSDK.dictionaryToString(mDic));
        }

        HolagamesSDK.getInstance().exitSDK();    
        return;
#endif
        }

        //////////////////////判断鼠标点击移动(以下)//////////////////////
        if (PictureCreater.instance.IsRemember)
        {
            if (!PictureCreater.instance.IsManualSkill)
            {
                if (PictureCreater.instance.IsFight)
                {
                    /////////////////////////////开战后(以下)/////////////////////////////
                    if (PictureCreater.instance.FightStyle == 6 || PictureCreater.instance.FightStyle == 7)
                    {
                        if (Input.touchCount > 0)
                        {
                            for (int i = 0; i < Input.touchCount; i++)
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                                {
                                    GameObject.Find("FightWindow").GetComponent<FightWindow>().ClickNumber++;
                                    Vector3 ClickPosition = new Vector3(-100, 0, 0) + new Vector3(Input.GetTouch(i).position.x - Screen.width / 2, Input.GetTouch(i).position.y - Screen.height / 2, 0) / 360;
                                    GameObject TouchObject = GameObject.Instantiate(Resources.Load("Prefab/Effect/PingMuDianJi", typeof(GameObject)), new Vector3(ClickPosition.x, ClickPosition.y, 0), Quaternion.identity) as GameObject;
                                    TouchObject.name = "Touch";

                                    AudioEditer.instance.PlayOneShot("touchtofight");
                                    //StartCoroutine(PictureCreater.instance.AddSequence());
                                    List<int> RoleFightIndex = new List<int>();
                                    List<int> RoleFightCate = new List<int>();
                                    List<int> RoleFightDamige = new List<int>();

                                    RoleFightIndex.Add(0);
                                    RoleFightCate.Add(0);
                                    RoleFightDamige.Add(PictureCreater.instance.TotalAttack);

                                    EffectMaker.instance.Create2DEffect("Test", "blank", null, PictureCreater.instance.ListEnemyPicture[0].RoleObject.transform.position, PictureCreater.instance.ListEnemyPicture[0].RoleObject.transform.position, new Vector3(1f, 1.2f, 1.2f), 0.1f, 0, 2, 6, 2, -2, RoleFightIndex, RoleFightCate, RoleFightDamige, null, null, false, false, false, true, false, false, false, true, !PictureCreater.instance.ListEnemyPicture[0].RolePictureMonster, "", null);
                                }
                            }
                        }
                    }


                    if (Input.GetMouseButtonDown(0)) //鼠标左键
                    {
                        if (UICamera.hoveredObject != null)
                        {
                            if (UICamera.hoveredObject.name == "UIRoot")
                            {
                                RaycastHit RaycastHitObject;
                                Ray HitRay;
                                HitRay = camera.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(HitRay, out RaycastHitObject))
                                {
                                    if (RaycastHitObject.transform.name != "dimian" && RaycastHitObject.transform.name != "UIRoot" && RaycastHitObject.transform.name != "airwall")
                                    {
                                        //if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 8)
                                        //{
                                        //    SceneTransformer.instance.NewGuideButtonClick();
                                        //}
                                        IsLock = true;
                                        if (RaycastHitObject.transform.name.IndexOf("Role") > -1 || RaycastHitObject.transform.name.IndexOf("NPC") > -1)
                                        {
                                            for (int SetRoleIndex = 0; SetRoleIndex < PictureCreater.instance.ListRolePicture.Count; SetRoleIndex++)
                                            {
                                                if (PictureCreater.instance.ListRolePicture[SetRoleIndex].RoleObject.name == RaycastHitObject.transform.name)
                                                {
                                                    RoleIndex = SetRoleIndex;

                                                    /////////////////////auto操作（以下）//////////////////////
                                                    if (PictureCreater.instance.CheckAutoHandle())
                                                    {
                                                        PictureCreater.instance.SetMoveTarget(SetRoleIndex, 0, false, false, false);
                                                    }
                                                    /////////////////////auto操作（以上）//////////////////////
                                                    break;
                                                }
                                            }
                                        }
                                        else if (RaycastHitObject.transform.name.IndexOf("Enemy") > -1)
                                        {
                                            for (int SetRoleIndex = 0; SetRoleIndex < PictureCreater.instance.ListEnemyPicture.Count; SetRoleIndex++)
                                            {
                                                if (PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RoleObject.name == RaycastHitObject.transform.name)
                                                {
                                                    RoleIndex = SetRoleIndex;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                TouchPosition = Input.mousePosition.x;
                                TouchPositionY = Input.mousePosition.y;
                            }
                        }
                    }
                    if (Input.GetMouseButton(0)) //鼠标左键
                    {
                        if (!IsLock)
                        {
                            if (UICamera.hoveredObject != null)
                            {
                                if (UICamera.hoveredObject.name == "UIRoot" && ((PictureCreater.instance.FightStyle != 6 && PictureCreater.instance.FightStyle != 7 && PictureCreater.instance.FightStyle != 2 && CharacterRecorder.instance.level > 10) || NetworkHandler.instance.IsCreate))
                                {
                                    RaycastHit RaycastHitObject;
                                    Ray HitRay;
                                    HitRay = camera.ScreenPointToRay(Input.mousePosition);
                                    if (Physics.Raycast(HitRay, out RaycastHitObject))
                                    {

                                    }
                                    if (!PictureCreater.instance.IsLock)
                                    {
                                        MovePosition = Input.mousePosition.x;
                                        MovePositionY = Input.mousePosition.y;

                                        MoveDistance = MovePosition - TouchPosition;
                                        MoveDistanceY = TouchPositionY - MovePositionY;
                                        MoveDistance += MoveOffect;
                                        //MoveDistanceY += MoveOffectY;

                                        if ((PictureCreater.instance.MyCamera.transform.position.y + MoveDistanceY) > 9f)
                                        {
                                            MoveDistanceY = 9f - PictureCreater.instance.MyCamera.transform.position.y;
                                        }
                                        else if ((PictureCreater.instance.MyCamera.transform.position.y + MoveDistanceY) < 2f)
                                        {
                                            MoveDistanceY = 2f - PictureCreater.instance.MyCamera.transform.position.y;
                                        }

                                        Quaternion rotation = Quaternion.Euler(0, MoveDistance / 2, 0);
                                        PictureCreater.instance.MyCamera.transform.position = rotation * new Vector3(0, PictureCreater.instance.MyCamera.transform.position.y + MoveDistanceY / 5f, -6.6f);
                                        PictureCreater.instance.MyCamera.transform.LookAt(new Vector3(0, 0, 0));
                                        TouchPositionY = MovePositionY;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (UICamera.hoveredObject != null)
                            {
                                /////////////////////auto操作（以下）//////////////////////
                                if (PictureCreater.instance.CheckAutoHandle() && RoleIndex > -1)
                                {
                                    RaycastHit RaycastHitObject;
                                    Ray HitRay;
                                    HitRay = camera.ScreenPointToRay(Input.mousePosition);
                                    if (Physics.Raycast(HitRay, out RaycastHitObject))
                                    {

                                        if (RaycastHitObject.transform.name.IndexOf("Role") > -1 || RaycastHitObject.transform.name.IndexOf("NPC") > -1)
                                        {
                                            for (int SetRoleIndex = 0; SetRoleIndex < PictureCreater.instance.ListRolePicture.Count; SetRoleIndex++)
                                            {
                                                if (PictureCreater.instance.ListRolePicture[SetRoleIndex].RoleObject.name == RaycastHitObject.transform.name)
                                                {
                                                    if (PictureCreater.instance.ListMove[PictureCreater.instance.ListRolePicture[SetRoleIndex].RolePosition].renderer.material.mainTexture.name != "blank")
                                                    {
                                                        PictureCreater.instance.SetRoleTargetIndex(RoleIndex, false, -1);
                                                        PictureCreater.instance.SetMoveTarget(RoleIndex, PictureCreater.instance.ListRolePicture[SetRoleIndex].RolePosition, false, false, false);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (RaycastHitObject.transform.name.IndexOf("Enemy") > -1)
                                        {
                                            for (int SetRoleIndex = 0; SetRoleIndex < PictureCreater.instance.ListEnemyPicture.Count; SetRoleIndex++)
                                            {
                                                if (PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RoleObject.name == RaycastHitObject.transform.name)
                                                {
                                                    if (PictureCreater.instance.ListMove[PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition].renderer.material.mainTexture.name != "blank")
                                                    {
                                                        PictureCreater.instance.SetRoleTargetIndex(RoleIndex, false, -1);
                                                        PictureCreater.instance.SetMoveTarget(RoleIndex, PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition, false, false, false);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (RaycastHitObject.transform.name.IndexOf("Base") > -1)
                                        {
                                            int MovePosition = int.Parse(RaycastHitObject.transform.name.Replace("Base", ""));
                                            if (PictureCreater.instance.ListMove[MovePosition].renderer.material.mainTexture.name != "blank")
                                            {
                                                PictureCreater.instance.SetRoleTargetIndex(RoleIndex, false, -1);
                                                PictureCreater.instance.SetMoveTarget(RoleIndex, MovePosition, false, false, false);
                                            }
                                        }

                                    }
                                }
                                /////////////////////auto操作（以上）//////////////////////
                            }
                        }
                    }
                    /////////////////////////////开战后(以上)/////////////////////////////
                }
                else
                {
                    /////////////////////////////未开战前(以下)/////////////////////////////
                    if (Input.GetMouseButtonDown(0)) //鼠标左键
                    {
                        if (UICamera.hoveredObject != null && UICamera.hoveredObject.name == "UIRoot")
                        {
                            if (PictureCreater.instance.IsRoleInGate && !PictureCreater.instance.IsFightFinish)
                            {
                                RaycastHit RaycastHitObject;
                                Ray HitRay;
                                HitRay = camera.ScreenPointToRay(Input.mousePosition);

                                if (Physics.Raycast(HitRay, out RaycastHitObject))
                                {
                                    if (RaycastHitObject.transform.name != "dimian" && RaycastHitObject.transform.name != "UIRoot" && RaycastHitObject.transform.name != "airwall" && RaycastHitObject.transform.name.IndexOf("Role") > -1)
                                    {
                                        //for (int i = 1; i < PictureCreater.instance.PositionRow * PictureCreater.instance.PositionColumn + 1; i++)
                                        //{
                                        //    DestroyImmediate(GameObject.Find("GanTanHao" + i.ToString()));
                                        //    if (GameObject.Find("DeGanTanHao" + i.ToString()) == null)
                                        //    {
                                        //        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/GanTanHao02", typeof(GameObject)), PictureCreater.instance.ListPosition[i] + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
                                        //        go.name = "DeGanTanHao" + i.ToString();
                                        //    }
                                        //}

                                        //////////////////////新手引导//////////////////////
                                        if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 3 && (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 13 || PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 14))
                                        {
                                            return;
                                        }
                                        else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 7)
                                        {
                                            if (RaycastHitObject.transform.gameObject.name == "Role60032")
                                            {

                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                        //////////////////////新手引导//////////////////////

                                        DestroyImmediate(GameObject.Find("MyPath"));

                                        DragObject = RaycastHitObject.transform.gameObject;
                                        PictureCreater.instance.MyPositions.SetActive(true);
                                        //if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4 && DragObject.name == "Role60032")
                                        //{
                                        //    ResourceLoader.instance.SetGuideButtonLocalPosition(new Vector3(-303, -2, 0));
                                        //    ResourceLoader.instance.SetGuideArrow(new Vector3(-303, -2, 0), 0);
                                        //    GameObject go = PictureCreater.instance.MyPositions;
                                        //    for (int g = 0; g < go.transform.childCount; g++)
                                        //    {
                                        //        GameObject obj = go.transform.GetChild(g).gameObject;
                                        //        go.transform.Find(obj.name).gameObject.SetActive(false);
                                        //    }
                                        //    go.transform.Find("Position11").gameObject.SetActive(true);
                                        //    go.transform.Find("Position7").gameObject.SetActive(true);
                                        //    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 5);
                                        //}
                                        //if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 4 && DragObject.name == "Role60032")
                                        //{
                                        //    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 5);
                                        //}

                                    }
                                    else if (RaycastHitObject.transform.name.IndexOf("Enemy") > -1)
                                    {
                                        RoleIndex = int.Parse(RaycastHitObject.transform.name.Split('_')[1]);
                                        PositionID = int.Parse(RaycastHitObject.transform.name.Split('_')[2]);
                                        string Enemy = RaycastHitObject.transform.name.Split('_')[0];
                                        int EnemyID = int.Parse(Enemy.Replace("Enemy", ""));
                                        PictureCreater.instance.ListEnemyPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().SetDetailInfo(EnemyID);
                                        StopAllCoroutines();
                                        StartCoroutine(PictureCreater.instance.ShowTarget(PictureCreater.instance.ListEnemyPicture, RoleIndex, PositionID, true, false));
                                    }
                                    /////////////////////手动操作（以下）//////////////////////
                                    else if (RaycastHitObject.transform.name.IndexOf("Base") > -1)
                                    {
                                        int MovePosition = int.Parse(RaycastHitObject.transform.name.Replace("Base", ""));
                                        StartCoroutine(PictureCreater.instance.ShowTerrain(MovePosition));
                                    }
                                    /////////////////////手动操作（以上）//////////////////////
                                }
                            }
                        }
                    }
                    if (Input.GetMouseButton(0)) //鼠标左键
                    {
                        if (PictureCreater.instance.IsRoleInGate)
                        {
                            if (DragObject != null)
                            {
                                PictureCreater.instance.MyPositions.SetActive(true);
                                Ray HitRay;
                                RaycastHit[] RaycastHitObject;
                                HitRay = camera.ScreenPointToRay(Input.mousePosition);
                                RaycastHitObject = Physics.RaycastAll(HitRay);

                                for (int i = 0; i < RaycastHitObject.Length; i++)
                                {
                                    //Debug.LogError(RaycastHitObject[i].point + "  ; ; ; ; ;  " + RaycastHitObject[i].transform.name + "::: ;;;  ;   " + RaycastHitObject[i].transform.name.IndexOf("dimian"));
                                    if (RaycastHitObject[i].transform.name.IndexOf("dimian") > -1)
                                    {
                                        MouseClickPostion = RaycastHitObject[i].point;
                                        DragObject.transform.position = MouseClickPostion + new Vector3(0, 0.5f, 0);

                                        ////////////为了优化  暂时先拿掉（以下）///////////////
                                        PositionID = PictureCreater.instance.ShowPosition(DragObject.name, MouseClickPostion, 0, true);

                                        //if (PositionID == 0)
                                        //{
                                        //    for (int j = 1; j < PictureCreater.instance.PositionRow * PictureCreater.instance.PositionColumn + 1; j++)
                                        //    {
                                        //        if (GameObject.Find("Move" + j.ToString()) != null)
                                        //        {
                                        //            if (GameObject.Find("GanTanHao" + j.ToString()) != null)
                                        //            {
                                        //                DestroyImmediate(GameObject.Find("GanTanHao" + j.ToString()));

                                        //                if (GameObject.Find("DeGanTanHao" + j.ToString()) == null)
                                        //                {
                                        //                    GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/GanTanHao02", typeof(GameObject)), PictureCreater.instance.ListPosition[j] + new Vector3(0, 1.5f, 0), Quaternion.identity) as GameObject;
                                        //                    go.name = "DeGanTanHao" + j.ToString();

                                        //                    foreach (var p in PictureCreater.instance.ListEnemyPicture)
                                        //                    {
                                        //                        if (p.RolePosition == j)
                                        //                        {
                                        //                            p.RolePictureObject.GetComponent<Animator>().SetFloat("id", 0);
                                        //                            break;
                                        //                        }
                                        //                    }

                                        //                    while (GameObject.Find("Path" + j.ToString()) != null)
                                        //                    {
                                        //                        DestroyImmediate(GameObject.Find("Path" + j.ToString()));
                                        //                    }
                                        //                }
                                        //            }
                                        //        }
                                        //    }

                                        //    PictureCreater.instance.SetListPosition(PictureCreater.instance.PositionRow, 1);
                                        //}
                                        ////////////为了优化  暂时先拿掉（以上）///////////////
                                        PictureCreater.instance.IsOut = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    /////////////////////////////未开战前(以上)/////////////////////////////
                }
            }
            if (Input.GetMouseButtonUp(0)) //鼠标左键
            {
                if (PictureCreater.instance.IsFight && UICamera.hoveredObject.name == "UIRoot")
                {
                    MoveOffect = MoveDistance;
                    MoveOffectY = MoveDistanceY;
                    IsLock = false;

                    RaycastHit RaycastHitObject;
                    Ray HitRay;
                    HitRay = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(HitRay, out RaycastHitObject))
                    {
                        Debug.LogError(RoleIndex);
                        if (RoleIndex > -1)
                        {
                            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 6 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 9)
                            {
                                Debug.LogError("ssssssssss111111111");
                                /////////////////////auto操作（以下）//////////////////////
                                if (PictureCreater.instance.CheckAutoHandle())
                                {
                                    PictureCreater.instance.SetMoveTarget(RoleIndex, 9, false, false, true);
                                }
                                /////////////////////auto操作（以上）//////////////////////
                                SceneTransformer.instance.NewGuideButtonClick();
                            }
                            else
                            {
                                if (RaycastHitObject.transform.name.IndexOf("Role") > -1 || RaycastHitObject.transform.name.IndexOf("NPC") > -1)
                                {
                                    for (int SetRoleIndex = 0; SetRoleIndex < PictureCreater.instance.ListRolePicture.Count; SetRoleIndex++)
                                    {
                                        if (PictureCreater.instance.ListRolePicture[SetRoleIndex].RoleObject.name == RaycastHitObject.transform.name)
                                        {
                                            if (RoleIndex == SetRoleIndex)
                                            {
                                                Debug.LogError(RoleIndex + " " + PictureCreater.instance.IsHand);
                                                ///////////////////手动操作（以下）//////////////////////
                                                Debug.LogError(PictureCreater.instance.IsHand);
                                                if (PictureCreater.instance.IsHand && PictureCreater.instance.FightStyle == 2)
                                                {
                                                    PictureCreater.instance.SetMovePosition(PictureCreater.instance.ListRolePicture[SetRoleIndex].RolePosition);
                                                    PictureCreater.instance.SetTargetIndex(false, RoleIndex);
                                                }
                                                ///////////////////手动操作（以上）//////////////////////
                                                if (PictureCreater.instance.SkillFire1 == 1 || PictureCreater.instance.SkillFire2 == 1 || PictureCreater.instance.SkillFire3 == 1)
                                                {
                                                    FireSkill(0, RoleIndex);
                                                    //EffectMaker.instance.Create2DEffect("~DiMianDianJi_ui", "", null, PictureCreater.instance.ListPosition[PictureCreater.instance.ListRolePicture[SetRoleIndex].RolePosition], PictureCreater.instance.ListPosition[PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition], Vector3.one, 0f, 0f, 1.5f, 1, 1, RoleIndex, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
                                                }
                                            }

                                            ///////////////////auto操作（以下）//////////////////////
                                            if (PictureCreater.instance.CheckAutoHandle())
                                            {
                                                Debug.LogError("Up");
                                                PictureCreater.instance.SetRoleTargetIndex(RoleIndex, false, SetRoleIndex);
                                                PictureCreater.instance.SetMoveTarget(RoleIndex, PictureCreater.instance.ListRolePicture[SetRoleIndex].RolePosition, true, false, true);
                                            }
                                            ///////////////////auto操作（以上）//////////////////////
                                            break;
                                        }
                                    }
                                }
                                else if (RaycastHitObject.transform.name.IndexOf("Enemy") > -1)
                                {
                                    for (int SetRoleIndex = 0; SetRoleIndex < PictureCreater.instance.ListEnemyPicture.Count; SetRoleIndex++)
                                    {
                                        if (PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RoleObject.name == RaycastHitObject.transform.name)
                                        {
                                            if (RoleIndex == SetRoleIndex)
                                            {
                                                Debug.LogError(RoleIndex + " " + PictureCreater.instance.IsHand);
                                                /////////////////////手动操作（以下）//////////////////////
                                                if (PictureCreater.instance.IsHand && PictureCreater.instance.FightStyle == 2)
                                                {
                                                    PictureCreater.instance.SetMovePosition(PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition);
                                                    PictureCreater.instance.SetTargetIndex(true, RoleIndex);
                                                    EffectMaker.instance.Create2DEffect("~DiMianDianJi_ui", "", null, PictureCreater.instance.ListPosition[PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition], PictureCreater.instance.ListPosition[PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition], Vector3.one, 0f, 0f, 1.5f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
                                                }
                                                /////////////////////手动操作（以上）//////////////////////
                                                if (PictureCreater.instance.SkillFire1 == 1 || PictureCreater.instance.SkillFire2 == 1 || PictureCreater.instance.SkillFire3 == 1)
                                                {
                                                    FireSkill(1, RoleIndex);
                                                    EffectMaker.instance.Create2DEffect("~DiMianDianJi_ui", "", null, PictureCreater.instance.ListPosition[PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition], PictureCreater.instance.ListPosition[PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition], Vector3.one, 0f, 0f, 1.5f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
                                                }
                                            }

                                            /////////////////////auto操作（以下）//////////////////////
                                            if (PictureCreater.instance.CheckAutoHandle())
                                            {
                                                Debug.LogError("UpEnemy");
                                                PictureCreater.instance.SetRoleTargetIndex(RoleIndex, true, SetRoleIndex);
                                                PictureCreater.instance.SetMoveTarget(RoleIndex, PictureCreater.instance.ListEnemyPicture[SetRoleIndex].RolePosition, true, true, true);
                                            }
                                            /////////////////////auto操作（以上）//////////////////////
                                            break;
                                        }
                                    }
                                }
                                else if (RaycastHitObject.transform.name.IndexOf("Base") > -1)
                                {
                                    /////////////////////auto操作（以下）//////////////////////
                                    if (PictureCreater.instance.CheckAutoHandle())
                                    {
                                        int MovePosition = int.Parse(RaycastHitObject.transform.name.Replace("Base", ""));
                                        if (PictureCreater.instance.ListMove[MovePosition].renderer.material.mainTexture.name != "blank")
                                        {
                                            Debug.LogError(MovePosition);
                                            PictureCreater.instance.SetMoveTarget(RoleIndex, MovePosition, false, false, true);
                                        }
                                        else
                                        {
                                            Debug.LogError(MovePosition + "AAAAAAAAAAAAA" + RoleIndex);
                                            PictureCreater.instance.SetMoveTarget(RoleIndex, MovePosition, true, false, true);
                                        }
                                    }
                                    /////////////////////auto操作（以上）//////////////////////
                                }
                                else
                                {
                                    /////////////////////auto操作（以下）//////////////////////
                                    if (PictureCreater.instance.CheckAutoHandle())
                                    {
                                        PictureCreater.instance.SetMoveTarget(RoleIndex, 0, true, false, true);
                                    }
                                    /////////////////////auto操作（以上）//////////////////////
                                }
                            }
                        }
                        else
                        {
                            if (RaycastHitObject.transform.name.IndexOf("Base") > -1)
                            {
                                Debug.LogError(RoleIndex + " " + PictureCreater.instance.IsHand);
                                /////////////////////手动操作（以下）//////////////////////
                                if (PictureCreater.instance.IsHand && PictureCreater.instance.FightStyle == 2)
                                {
                                    int MovePosition = int.Parse(RaycastHitObject.transform.name.Replace("Base", ""));
                                    if (PictureCreater.instance.ListMove[MovePosition].renderer.material.mainTexture.name != "blank")
                                    {
                                        PictureCreater.instance.SetMovePosition(MovePosition);
                                        EffectMaker.instance.Create2DEffect("~DiMianDianJi_ui", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 1.5f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
                                    }
                                }
                                /////////////////////手动操作（以上）//////////////////////
                                if (PictureCreater.instance.SkillFire1 == 1 || PictureCreater.instance.SkillFire2 == 1 || PictureCreater.instance.SkillFire3 == 1)
                                {
                                    int MovePosition = int.Parse(RaycastHitObject.transform.name.Replace("Base", ""));
                                    FireSkill(2, MovePosition);
                                    EffectMaker.instance.Create2DEffect("~Skill_JiaoXia_UI", "", null, PictureCreater.instance.ListPosition[MovePosition], PictureCreater.instance.ListPosition[MovePosition], Vector3.one, 0f, 0f, 1.5f, 1, 1, RoleIndex, null, null, null, null, null, false, false, false, true, false, false, false, false, false, "", null);
                                }
                            }
                            //else
                            //{
                            //    PictureCreater.instance.SetMovePosition(0);
                            //}
                        }
                    }
                    else
                    {
                        /////////////////////auto操作（以下）//////////////////////
                        if (PictureCreater.instance.CheckAutoHandle())
                        {
                            PictureCreater.instance.SetMoveTarget(RoleIndex, 0, true, false, true);
                        }
                        /////////////////////auto操作（以上）//////////////////////
                    }
                }
                else
                {
                    MoveOffect = 0;
                    MovePosition = 0;
                    TouchPosition = 0;
                    MoveDistance = 0;

                    MoveOffectY = 0;
                    MovePositionY = 0;
                    TouchPositionY = 0;
                    MoveDistanceY = 0;
                }
                PictureCreater.instance.IsManualSkill = false;

                if (!PictureCreater.instance.IsFight && !PictureCreater.instance.IsFightFinish)
                {
                    if (DragObject != null)
                    {
                        if (!PictureCreater.instance.IsOut)
                        {
                            //if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 2 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
                            //{
                            //    //PositionID = 7;
                            //    if (PositionID == 7)
                            //    {
                            //        PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) + 1);
                            //        LuaDeliver.instance.UseGuideStation();
                            //    }
                            //    else
                            //    {
                            //        PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 4);
                            //        LuaDeliver.instance.UseGuideStation();
                            //        PositionID = 11;
                            //    }
                            //}
                            //else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) == 5)
                            //{
                            //    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideSubStateName()) + 1);
                            //    LuaDeliver.instance.UseGuideStation();
                            //}
                            PictureCreater.instance.SetPosition(DragObject.name, PositionID);
                            PictureCreater.instance.MyPositions.SetActive(false);

                            PictureCreater.instance.SetSequence();
                        }
                        else
                        {
                            Debug.LogError("AAAAAA");
                        }
                        DragObject = null;
                    }
                    else
                    {
                        if (RoleIndex != -1)
                        {
                            PictureCreater.instance.ListEnemyPicture[RoleIndex].RoleObject.GetComponent<ColliderDisplayText>().DetailInfo.SetActive(false);
                            RoleIndex = -1;
                        }
                    }
                }
                else
                {
                    if (RoleIndex != -1)
                    {
                        RoleIndex = -1;
                    }
                    if (!PictureCreater.instance.IsFight && PictureCreater.instance.IsRoleInGate)
                    {
                        PictureCreater.instance.SetListPosition(PictureCreater.instance.PositionRow, 1);
                    }
                }
                IsOut = false;
            }
            //////////////////////判断鼠标点击移动(以上)//////////////////////
        }
    }

    void FireSkill(int IsEnemy, int SkillRoleIndex)
    {
        int SkillID = PictureCreater.instance.FightSkill1;
        if (PictureCreater.instance.SkillFire1 == 1)
        {
            SkillID = PictureCreater.instance.FightSkill1;
            PictureCreater.instance.FireSkill(PictureCreater.instance.ListRolePicture, PictureCreater.instance.ListEnemyPicture, SkillID, IsEnemy, SkillRoleIndex, 1, CharacterRecorder.instance.level);
        }
        if (PictureCreater.instance.SkillFire2 == 1)
        {
            SkillID = PictureCreater.instance.FightSkill2;
            PictureCreater.instance.FireSkill(PictureCreater.instance.ListRolePicture, PictureCreater.instance.ListEnemyPicture, SkillID, IsEnemy, SkillRoleIndex, 2, CharacterRecorder.instance.level);
        }
        else if (PictureCreater.instance.SkillFire3 == 1)
        {
            SkillID = PictureCreater.instance.FightSkill3;
            PictureCreater.instance.FireSkill(PictureCreater.instance.ListRolePicture, PictureCreater.instance.ListEnemyPicture, SkillID, IsEnemy, SkillRoleIndex, 3, CharacterRecorder.instance.level);
        }

        if (GameObject.Find("SkillUI1") != null)
        {
            GameObject.Find("SkillUI1").SetActive(false);
        }
        if (GameObject.Find("SkillUI2") != null)
        {
            GameObject.Find("SkillUI2").SetActive(false);
        }
        if (GameObject.Find("SkillUI3") != null)
        {
            GameObject.Find("SkillUI3").SetActive(false);
        }

        GameObject.Find("FightWindow").GetComponent<FightWindow>().Tactics.SetActive(true);
        GameObject.Find("FightWindow").GetComponent<FightWindow>().TacticsInfo.SetActive(false);
    }
}
