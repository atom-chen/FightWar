using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskDesigner : MonoBehaviour
{
    public static TaskDesigner instance;
    /************************************************翻译完成********************************************/
    public List<Task> ListTask = new List<Task>();
    public List<MissionVice> ListMissionVice = new List<MissionVice>();
    public int RecvTaskID = 1000;//现在的任务
    public int FinishTaskID = 1000;//已经完成的任务
    public int ReturnTaskID = 1000;//已经完成的任务

    public int TaskID = 1000;

    public int MissionIndex = -1;
    public int GateID;
    public int ViceGateID;
    public int TaskType = 0;
    public int TaskNumber = 0;

    public string TaskName = "";
    public string TaskGetDescription1;
    public string TaskGetDescription2;
    public string TaskGetDescription3;
    public string TaskGetAnswer1;
    public string TaskGetAnswer2;
    public string TaskGetAnswer3;
    public string TaskReturnDescription1;
    public string TaskReturnDescription2;
    public string TaskReturnDescription3;
    public string TaskReturnAnswer1;
    public string TaskReturnAnswer2;
    public string TaskReturnAnswer3;
    public string ReturnPointID;
    public string GetPointID;
    public string TaskContent;
    public string TaskAnswer;
    public int TaskLevel;
    public int TaskMoney;
    public float TaskExp;
    public bool IsReward = true;
    public bool IsPartner = false;
    public bool IsGo = false;
    public bool IsGuideTask = true;

    public class MissionVice
    {
        public int MissionViceID;
        public string MissionViceName;
        public string MissionViceGetDescription1;
        public string MissionViceGetDescription2;
        public string MissionViceGetDescription3;
        public string MissionViceGetAnswer1;
        public string MissionViceGetAnswer2;
        public string MissionViceGetAnswer3;
        public string MissionViceReturnDescription1;
        public string MissionViceReturnDescription2;
        public string MissionViceReturnDescription3;
        public string MissionViceReturnAnswer1;
        public string MissionViceReturnAnswer2;
        public string MissionViceReturnAnswer3;

        public int GateID;
        public int Level;
        public int PreMission;
        public float Exp;
        public int Money;
        public string Description;
        public string GetPointID;
        public string ReturnPointID;
        public string Bonus;
        public string MonsterName1;
        public int MonsterID1;
        public int MonsterNum1;
        public int MonsterMaxNum1;
        public string MonsterName2;
        public int MonsterID2;
        public int MonsterNum2;
        public int MonsterMaxNum2;
        public string MonsterName3;
        public int MonsterID3;
        public int MonsterNum3;
        public int MonsterMaxNum3;
        public int MissionState;
    }

    public class Task
    {
        public int TaskID;
        public int GateID;
        public int Level;
        public int PreMission;
        public float Exp;
        public int Money;
        public string TaskName;
        public string TaskGetDescription1;
        public string TaskGetDescription2;
        public string TaskGetDescription3;
        public string TaskGetAnswer1;
        public string TaskGetAnswer2;
        public string TaskGetAnswer3;
        public string TaskReturnDescription1;
        public string TaskReturnDescription2;
        public string TaskReturnDescription3;
        public string TaskReturnAnswer1;
        public string TaskReturnAnswer2;
        public string TaskReturnAnswer3;

        public string Description;
        public string GetPointID;
        public string ReturnPointID;
        public string Bonus;
    }

    void Start()
    {
        instance = this;
        ////////////////////////////////////////////////////////XML///////////////////////////////////////////////////
        //TextAsset LuaText = (TextAsset)Resources.Load("CN/Item/Item");
        //XMLParser.instance.ParseXMLItemScript(LuaText.text);
        ////////////////////////////////////////////////////////XML///////////////////////////////////////////////////
    }

    public void Init(int _RecvTaskID, int _FinishTaskID, int _ReturnTaskID)
    {
        RecvTaskID = _RecvTaskID;
        FinishTaskID = _FinishTaskID;
        ReturnTaskID = _ReturnTaskID;
    }

    public void CreateMissionVice(int MissionViceID, string MissionViceName, string MissionViceGetDescription1, string MissionViceGetAnswer1, string MissionViceReturnDescription1, string MissionViceReturnAnswer1, string GetPointID, string ReturnPointID, string Description, int PreMission, int GateID, int Level, float Exp, int Money, string Bonus, string MonsterName1, int MonsterID1, int MonsterMaxNum1, string MonsterName2, int MonsterID2, int MonsterMaxNum2, string MonsterName3, int MonsterID3, int MonsterMaxNum3)
    {
        MissionVice NewMissionVice = new MissionVice();
        NewMissionVice.MissionViceID = MissionViceID;
        NewMissionVice.MonsterName1 = MonsterName1;
        NewMissionVice.MonsterID1 = MonsterID1;
        NewMissionVice.MonsterNum1 = 0;
        NewMissionVice.MonsterMaxNum1 = MonsterMaxNum1;
        NewMissionVice.MonsterName2 = MonsterName2;
        NewMissionVice.MonsterID2 = MonsterID2;
        NewMissionVice.MonsterNum2 = 0;
        NewMissionVice.MonsterMaxNum2 = MonsterMaxNum2;
        NewMissionVice.MonsterName3 = MonsterName3;
        NewMissionVice.MonsterID3 = MonsterID3;
        NewMissionVice.MonsterNum3 = 0;
        NewMissionVice.MonsterMaxNum3 = MonsterMaxNum3;
        NewMissionVice.MissionState = 5;

        NewMissionVice.MissionViceName = MissionViceName;
        NewMissionVice.MissionViceGetDescription1 = MissionViceGetDescription1;
        NewMissionVice.MissionViceGetDescription2 = "";
        NewMissionVice.MissionViceGetDescription3 = "";
        NewMissionVice.MissionViceGetAnswer1 = MissionViceGetAnswer1;
        NewMissionVice.MissionViceGetAnswer2 = "";
        NewMissionVice.MissionViceGetAnswer3 = "";
        NewMissionVice.MissionViceReturnDescription1 = MissionViceReturnDescription1;
        NewMissionVice.MissionViceReturnDescription2 = "";
        NewMissionVice.MissionViceReturnDescription3 = "";
        NewMissionVice.MissionViceReturnAnswer1 = MissionViceReturnAnswer1;
        NewMissionVice.MissionViceReturnAnswer2 = "";
        NewMissionVice.MissionViceReturnAnswer3 = "";
        NewMissionVice.GetPointID = GetPointID;
        NewMissionVice.ReturnPointID = ReturnPointID;
        NewMissionVice.Description = Description;
        NewMissionVice.PreMission = PreMission;
        NewMissionVice.GateID = GateID;
        NewMissionVice.Level = Level;
        NewMissionVice.Exp = Exp;
        NewMissionVice.Money = Money;
        NewMissionVice.Bonus = Bonus;

        ListMissionVice.Add(NewMissionVice);
    }

    public void CreateTask(int TaskID, string TaskName, string TaskGetDescription1, string TaskGetDescription2, string TaskGetDescription3, string TaskGetAnswer1, string TaskGetAnswer2, string TaskGetAnswer3, string TaskReturnDescription1, string TaskReturnDescription2, string TaskReturnDescription3, string TaskReturnAnswer1, string TaskReturnAnswer2, string TaskReturnAnswer3, string GetPointID, string ReturnPointID, string Description, int PreMission, int GateID, int Level, float Exp, int Money, string Bonus)
    {
        Task NewTask = new Task();
        NewTask.TaskID = TaskID;
        NewTask.TaskName = TaskName;
        NewTask.TaskGetDescription1 = TaskGetDescription1;
        NewTask.TaskGetDescription2 = TaskGetDescription2;
        NewTask.TaskGetDescription3 = TaskGetDescription3;
        NewTask.TaskGetAnswer1 = TaskGetAnswer1;
        NewTask.TaskGetAnswer2 = TaskGetAnswer2;
        NewTask.TaskGetAnswer3 = TaskGetAnswer3;
        NewTask.TaskReturnDescription1 = TaskReturnDescription1;
        NewTask.TaskReturnDescription2 = TaskReturnDescription2;
        NewTask.TaskReturnDescription3 = TaskReturnDescription3;
        NewTask.TaskReturnAnswer1 = TaskReturnAnswer1;
        NewTask.TaskReturnAnswer2 = TaskReturnAnswer2;
        NewTask.TaskReturnAnswer3 = TaskReturnAnswer3;
        NewTask.GetPointID = GetPointID;
        NewTask.ReturnPointID = ReturnPointID;
        NewTask.Description = Description;
        NewTask.PreMission = PreMission;
        NewTask.GateID = GateID;
        NewTask.Level = Level;
        NewTask.Exp = Exp;
        NewTask.Money = Money;
        NewTask.Bonus = Bonus;
        ListTask.Add(NewTask);
    }

    public void CheckTask()
    {
        foreach (Task t in ListTask)
        {
            if (FinishTaskID == RecvTaskID && ReturnTaskID == RecvTaskID) //新任务还没接
            {
                if (t.TaskID == RecvTaskID + 1)
                {
                    TaskType = 1;
                    TaskLevel = t.Level;
                    //if (TaskLevel <= int.Parse(transform.parent.GetComponent<GameCenter>().gameCharacterRecorder.GetComponent<CharacterRecorder>().CharacterLevel))
                    {
                        TaskName = t.TaskName;
                        TaskContent = t.TaskGetDescription1;
                        TaskAnswer = t.TaskGetAnswer1;
                        TaskMoney = t.Money;
                        TaskExp = t.Exp;
                        TaskGetDescription1 = t.TaskGetDescription1;
                        TaskGetDescription2 = t.TaskGetDescription2;
                        TaskGetDescription3 = t.TaskGetDescription3;
                        TaskGetAnswer1 = t.TaskGetAnswer1;
                        TaskGetAnswer2 = t.TaskGetAnswer2;
                        TaskGetAnswer3 = t.TaskGetAnswer3;
                        TaskID = t.TaskID;
                        GateID = t.GateID;
                        GetPointID = t.GetPointID;
                        ReturnPointID = t.ReturnPointID;
                    }
                    break;
                }
            }
            else if (FinishTaskID == RecvTaskID)  //现在完成
            {
                if (t.TaskID == RecvTaskID)
                {
                    TaskName = t.TaskName;
                    TaskLevel = t.Level;
                    TaskContent = t.TaskReturnDescription1;
                    TaskAnswer = t.TaskReturnAnswer1;
                    TaskMoney = t.Money;
                    TaskExp = t.Exp;
                    TaskReturnDescription1 = t.TaskReturnDescription1;
                    TaskReturnDescription2 = t.TaskReturnDescription2;
                    TaskReturnDescription3 = t.TaskReturnDescription3;
                    TaskReturnAnswer1 = t.TaskReturnAnswer1;
                    TaskReturnAnswer2 = t.TaskReturnAnswer2;
                    TaskReturnAnswer3 = t.TaskReturnAnswer3;
                    TaskID = t.TaskID;
                    GateID = t.GateID;
                    GetPointID = "";
                    ReturnPointID = t.ReturnPointID;
                    TaskType = 2;
                    break;
                }
            }
            else if (FinishTaskID == (RecvTaskID - 1)) //接了还沒完成
            {
                if (t.TaskID == RecvTaskID)
                {
                    TaskType = 3;
                    TaskName = t.TaskName;
                    TaskLevel = t.Level;
                    TaskContent = t.TaskGetDescription1;
                    TaskAnswer = t.TaskGetAnswer1;
                    TaskMoney = t.Money;
                    TaskExp = t.Exp;
                    TaskGetDescription1 = t.TaskGetDescription1;
                    TaskGetDescription2 = t.TaskGetDescription2;
                    TaskGetDescription3 = t.TaskGetDescription3;
                    TaskGetAnswer1 = t.TaskGetAnswer1;
                    TaskGetAnswer2 = t.TaskGetAnswer2;
                    TaskGetAnswer3 = t.TaskGetAnswer3;

                    TaskReturnDescription1 = t.TaskGetDescription1;
                    TaskReturnDescription2 = t.TaskGetDescription2;
                    TaskReturnDescription3 = t.TaskGetDescription3;
                    TaskReturnAnswer1 = t.TaskGetAnswer1;
                    TaskReturnAnswer2 = t.TaskGetAnswer2;
                    TaskReturnAnswer3 = t.TaskGetAnswer3;
                    TaskID = t.TaskID;
                    GateID = t.GateID;
                    GetPointID = "";
                    ReturnPointID = t.ReturnPointID;
                    break;
                }
            }
        }
        TaskContent = TaskContent.Replace("[XXXX]", transform.parent.GetComponent<GameCenter>().gameCharacterRecorder.GetComponent<CharacterRecorder>().characterName);
    }

    public void AddMonsterNum(int MonsterID, int MonsterNum)
    {
        foreach (MissionVice m in ListMissionVice)
        {
            if (m.MissionState == 1)
            {
                if (m.MonsterID1 == MonsterID)
                {
                    m.MonsterNum1 += MonsterNum;
                }
                if (m.MonsterID2 == MonsterID)
                {
                    m.MonsterNum2 += MonsterNum;
                }
                if (m.MonsterID3 == MonsterID)
                {
                    m.MonsterNum3 += MonsterNum;
                }
                if (m.MonsterMaxNum1 <= m.MonsterNum1 && m.MonsterMaxNum2 <= m.MonsterNum2 && m.MonsterMaxNum3 <= m.MonsterNum3)
                {
                    if (m.MissionState == 1)
                    {
                        m.MissionState = 2;
                    }
                }
            }
        }
    }


    public void CheckMissionVice()
    {
        int i = 0;
        foreach (MissionVice m in ListMissionVice)
        {
            if (transform.GetComponent<TaskDesigner>().MissionIndex == -1)
            {
                if (m.MissionState == 1)
                {
                    transform.GetComponent<TaskDesigner>().MissionIndex = i;
                    transform.GetComponent<TaskDesigner>().ViceGateID = m.GateID;
                }
            }
            if (m.MonsterMaxNum1 <= m.MonsterNum1 && m.MonsterMaxNum2 <= m.MonsterNum2 && m.MonsterMaxNum3 <= m.MonsterNum3)
            {
                if (m.MissionState == 1)
                {
                    m.MissionState = 2;
                }
            }
            if (m.MissionState >= 0 && m.MissionState <= 2)
            {
            }
            i++;
        }
    }
}
