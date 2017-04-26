using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using System.Linq;
//using LuaInterface;
using CodeStage.AntiCheat.ObscuredTypes;
using Holagames;

public class NetworkHandler : MonoBehaviour
{
    public static NetworkHandler instance;

    public bool ConnectFlag = false;
    public bool IsAuto = false;
    public bool IsCreate = false;
    public bool IsFirst = false;
    public bool IsLogin = false;
    public bool IsSDKLogin = false;
    public bool IsPrefetch = false;
    public bool IsReconnect = false;
    bool IsIdle = false;
    bool IsBag = true;
    bool IsKickOff = false;
    int ReconnectCount = 0;
    public int IdleCount = 0;
    public float ReconnectTimer = 0.1f;
    static bool IsException = false;
    static string ErrorLog = "";
    Queue SocketArray = new Queue();
    string PacketQueue = "";  //解决封包连续传送
    List<string> ResendString = new List<string>();
    public string CheckSendString = "";
    public string Account = "";
    public string Password = "";
    ///////////////////////////////////////////////////Socket 变数(以下)////////////////////////////////////////////////////
    byte[] Data = new byte[16384];            // 封包大小

    public string GameHost;
    public string GamePort;
    public static Socket GameClient;
    ///////////////////////////////////////////////////Socket 变数(以上)/////////////////////////////////////////////////////
    int PacketLength = 0;
    /// <summary>
    /// 第一次进入游戏，刷新角色英雄列表
    /// </summary>
    bool isFirstUpHero = true;
    void Start()
    {
        instance = this;
        GameHost = ObscuredPrefs.GetString("GameHost");
        GamePort = ObscuredPrefs.GetString("GamePort");
        GameSocketConnect();
        IsLogin = false;

        //if (PlayerPrefs.GetString("FirstLogin") == "")
        //{
        //    Debug.LogError("AAAAAAAAAAA");
        //    if (Application.loadedLevelName == "Downloader")
        //    {
        //        LuaDeliver.instance.StartLua();
        //        StartCoroutine(NetworkHandler.instance.LoadScene());
        //    }
        //    else
        //    {
        //        NetworkHandler.instance.IsCreate = true;
        //        //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
        //        StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Item"));
        //        //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
        //    }
        //}
    }

    void Update()
    {
        if (IsPrefetch)
        {
            ////////////////////////////////////////////////////Unity3处理讯息替代方案(以下)///////////////////////////////////////
            if (SocketArray.Count > 0)
            {
                string tData = SocketArray.Dequeue().ToString();
                string[] DataSplit = tData.Split(new char[] { '/' });
                string CodeDecrytion;
                string CodeEncrytion;
                string CodeLegft;

                for (int i = 0; i < DataSplit.Length; i++)
                {
                    foreach (char c in DataSplit[i])
                    {
                        if (Convert.ToInt32(c) == 0)
                        {
                        }
                        else
                        {
                            PacketQueue = PacketQueue + c.ToString();
                        }
                    }

                    while (PacketQueue.IndexOf('|') > -1)
                    {
                        string OldPacket = PacketQueue.Substring(0, PacketQueue.IndexOf('|'));
                        if (PacketQueue.IndexOf('|') == 0)
                        {
                            PacketQueue = "";
                        }
                        else
                        {
                            PacketQueue = PacketQueue.Substring(PacketQueue.IndexOf('|') + 1);
                        }
                        CodeEncrytion = OldPacket.Substring(0, 4);
                        CodeLegft = OldPacket.Substring(4, OldPacket.Length - 4);
                        CodeDecrytion = CodeEncrytion;
                        SplitPack(CodeDecrytion + CodeLegft.Split('@')[0]);
                        StartCoroutine(StopIdle(0, CodeEncrytion));
                        CheckSendString = "";
                    }
                }
            }
            ////////////////////////////////////////////////////Unity3处理讯息替代方案(以上)///////////////////////////////////////

            if (IsException && !IsKickOff)
            {
                ReconnectTimer += Time.deltaTime;
                if (ReconnectTimer > 3 * (ReconnectCount + 1))
                {
                    UIManager.instance.OpenPanel("LoadWindow");
                    ReconnectTimer = 0.1f;
                    OnReconnect();
                }
            }
        }
    }

    public void StartIdle()
    {
        IsIdle = true;
        if (IsIdle)
        {
            if (GameObject.Find("ReconnectWindow") == null)
            {
                if (GameObject.Find("LoadWindow") == null)
                {
                    IdleCount++;
                    UIManager.instance.OpenSinglePanel("LoadWindow", false);
                }
            }
        }

    }

    public IEnumerator StopIdle(float DelayTimer, String Code)
    {
        if (Code != "1902" && Code != "1018" && Code != "7001" && Code != "7002" && Code != "1018" && Code != "1004" && Code != "1005" && Code != "3001" && Code != "5007")
        {
            IdleCount = 0;
            IsIdle = false;
            DestroyImmediate(GameObject.Find("LoadWindow"));
            yield return new WaitForSeconds(DelayTimer);
        }
    }

    public void GameSocketConnect()
    {
        try
        {
            if (ConnectFlag == false)
            {
                if (GameClient == null)
                {
                    GameClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        Debug.Log("NetworkHandler Try Connect:" + GameHost + "  " + GamePort);
                        GameClient.Connect(GameHost, int.Parse(GamePort));
                        GameClient.ReceiveBufferSize = 8192;
                        GameClient.BeginReceive(Data, 0, 1, SocketFlags.None, new AsyncCallback(GameBeginRecieved), GameClient);
                        ConnectFlag = true;
                        IsPrefetch = true;
                        Debug.Log("Connect Successfully");

                        if (GameObject.Find("ReconnectWindow") != null)
                        {
                            Destroy(GameObject.Find("ReconnectWindow"));
                            SendProcess("9801#;");
                        }
                    }
                    catch (SocketException e)
                    {
                        //GameObject.Find("gameDebugTracer").GetComponent<DebugTracer>().CreateDebug("NetworkHandler", "GameSocket:" + e.ToString());
                        //GameClient.Shutdown(SocketShutdown.Both);
                        //GameClient.Close();
                        //ConnectFlag = false;
                        //IsPrefetch = false;
                        if (GameObject.Find("ReconnectWindow") == null && GameObject.Find("LoadWindow") == null)
                        {
                            GameObject panel = (GameObject)Instantiate(Resources.Load("GUI/ReconnectWindow"));
                            panel.name = "ReconnectWindow";
                            Transform tt = panel.transform;
                            tt.parent = GameObject.Find("UIRoot").transform;
                            tt.localPosition = Vector3.zero;
                            tt.localRotation = Quaternion.identity;
                            tt.localScale = Vector3.one;
                        }
                    }
                }
                else
                {
                    Debug.Log("Reconnect!!!");
                    GameSocketDisconnect();
                    GameSocketConnect();
                    if (Application.loadedLevelName != "Downloader")
                    {
                        SendProcess("1001#" + ObscuredPrefs.GetString("Account") + ";" + ObscuredPrefs.GetString("Password") + ";" + PlayerPrefs.GetString("ServerID") + ";");
                    }
                    //foreach (string s in ResendString)
                    //{
                    //    SendProcess(s);
                    //}
                    //ResendString.Clear();
                }
            }
        }
        catch (Exception ex)
        {
            IsException = true;
            Debug.Log(ex.ToString());
            Debug.Log(ex.Message.ToString());
            ErrorLog = ex.Message.ToString();
        }
    }

    public void GameSocketDisconnect()
    {
        //Debug.Log("GameSocketDisconnect");
        //if (GameClient.Connected)
        //{
        //    GameClient.Shutdown(SocketShutdown.Both);
        //}
        GameClient.Close();
        GameClient = null;
    }

    void GameBeginRecieved(IAsyncResult ar)
    {
        Socket Client = (Socket)ar.AsyncState;
        try
        {
            if (IsPrefetch)
            {
                int nb = Client.EndReceive(ar); //关闭接收连线

                if (nb > 0) //如果接收值大于0
                {
                    if (Data[0] == 64 && nb == 1)
                    {
                        GameClient.BeginReceive(Data, 0, 4, SocketFlags.None, new AsyncCallback(GameBeginRecieved), Client);
                    }
                    else if (nb == 4)
                    {
                        PacketLength = BitConverter.ToInt32(Data, 0);
                        GameClient.BeginReceive(Data, 0, PacketLength, SocketFlags.None, new AsyncCallback(GameBeginRecieved), Client);
                    }
                    else
                    {
                        if (nb >= 5)
                        {
                            if (nb < PacketLength)
                            {
                                PacketLength = PacketLength - nb;
                                String ReceiveString = Encoding.UTF8.GetString(Data, 0, nb); //转换成字串
                                SocketArray.Enqueue(ReceiveString);
                                GameClient.BeginReceive(Data, 0, PacketLength, SocketFlags.None, new AsyncCallback(GameBeginRecieved), Client);
                            }
                            else
                            {
                                String ReceiveString = Encoding.UTF8.GetString(Data, 0, nb); //转换成字串
                                ReceiveString += "|";
                                SocketArray.Enqueue(ReceiveString);
                                GameClient.BeginReceive(Data, 0, 1, SocketFlags.None, new AsyncCallback(GameBeginRecieved), Client);
                            }
                        }
                        else
                        {
                            GameClient.BeginReceive(Data, 0, 1, SocketFlags.None, new AsyncCallback(GameBeginRecieved), Client);
                        }
                    }
                }
                else
                {
                    Debug.Log("Game Disconnect");
                    ConnectFlag = false;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            Debug.Log(ex.Message.ToString());
            IsException = true;
            ErrorLog = ex.Message.ToString();
        }
    }

    public static void LuaSendProcess(string SendString)//传送前处理程序7001
    {
        Debug.Log("SendString:" + SendString);
        string CodeNumber = SendString.Substring(0, 4);
        string CodeContent = SendString.Substring(5, SendString.Length - 5);

        try
        {
            int ReturnValue = 0;
            while (ReturnValue <= 0)
            {
                ////////////////////////ProtoBuf(以下)/////////////////////////
                //MyModel myNewModel = new MyModel();
                //myNewModel.Task = EncrytionCode;
                //myNewModel.Operation = int.Parse(CodeNumber.Substring(1, CodeNumber.Length - 1));
                //myNewModel.Body = CodeContent;
                //myNewModel.Cid = 1;

                //MySerializer mySerializer = new MySerializer();
                //MemoryStream file = new MemoryStream();

                //mySerializer.Serialize(file, myNewModel);
                //byte[] SendMessage = file.ToArray();


                Byte[] SendTest1 = Encoding.UTF8.GetBytes(SendString);
                Byte[] RetureByte = new Byte[SendTest1.Length + 5];
                Byte[] TotalLengthByte = BitConverter.GetBytes(SendTest1.Length);
                Byte[] SendTest = Encoding.UTF8.GetBytes("@");


                SendTest.CopyTo(RetureByte, 0);
                TotalLengthByte.CopyTo(RetureByte, 1);
                SendTest1.CopyTo(RetureByte, 5);

                ReturnValue = GameClient.Send(RetureByte);
                ////////////////////////ProtoBuf(以上)/////////////////////////                
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message.ToString());
            ErrorLog = ex.Message.ToString();
            IsException = true;
        }
    }

    public void SendProcess(string SendString)//传送前处理程序
    {
        //Debug.Log("userid" + PlayerPrefs.GetInt("UserID") + "bbbb" + PlayerPrefs.GetString("ServerID"));
#if UNITY_EDITOR
        Debug.Log("SendString:" + SendString);
#endif
        string CodeNumber = SendString.Substring(0, 4);
        string CodeContent = SendString.Substring(5, SendString.Length - 5);

        if (CodeNumber != "1902" && CodeNumber != "1004" && CodeNumber != "1005" && CodeNumber != "3001" && CodeNumber != "7001" && CodeNumber != "7002")
        {
            StartIdle();
            CheckSendString = SendString;
        }

        try
        {
            int ReturnValue = 0;
            while (ReturnValue <= 0)
            {
                ////////////////////////ProtoBuf(以下)/////////////////////////
                //MyModel myNewModel = new MyModel();
                //myNewModel.Task = EncrytionCode;
                //myNewModel.Operation = int.Parse(CodeNumber.Substring(1, CodeNumber.Length - 1));
                //myNewModel.Body = CodeContent;
                //myNewModel.Cid = 1;

                //MySerializer mySerializer = new MySerializer();
                //MemoryStream file = new MemoryStream();

                //mySerializer.Serialize(file, myNewModel);
                //byte[] SendMessage = file.ToArray();


                Byte[] SendTest1 = Encoding.UTF8.GetBytes(SendString);
                Byte[] RetureByte = new Byte[SendTest1.Length + 5];
                Byte[] TotalLengthByte = BitConverter.GetBytes(SendTest1.Length);
                Byte[] SendTest = Encoding.UTF8.GetBytes("@");


                SendTest.CopyTo(RetureByte, 0);
                TotalLengthByte.CopyTo(RetureByte, 1);
                SendTest1.CopyTo(RetureByte, 5);

                ReturnValue = GameClient.Send(RetureByte);
                ////////////////////////ProtoBuf(以上)/////////////////////////                
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message.ToString());
            ErrorLog = ex.Message.ToString();
            ResendString.Add(SendString);
            IsException = true;
        }
    }

    public void LuaSplitPack(string Code, string Content)
    {
        ReconnectTimer = 0.1f;
        IsException = false;
        switch (Code)
        {
            case "1001":
                Process_1001(Content);
                break;
            case "1002":
                Process_1002(Content);
                break;
            case "1003":
                Process_1003(Content);
                break;
            case "1004":
                Process_1004(Content);
                break;
            case "1005":
                Process_1005(Content);
                break;
            case "1006":
                Process_1006(Content);
                break;
            case "1009":
                Process_1009(Content);
                break;
            case "1010":
                Process_1010(Content);
                break;
            case "1011":
                Process_1011(Content);
                break;
            case "1013":
                Process_1013(Content);
                break;
            case "1017":
                Process_1017(Content);
                break;
            case "1018":
                Process_1018(Content);
                break;
            case "1021":
                Process_1021(Content);
                break;
            case "1022":
                Process_1022(Content);
                break;
            case "1024":
                Process_1024(Content);
                break;
            case "1025":
                Process_1025(Content);
                break;
            case "1026":
                Process_1026(Content);
                break;
            case "1027":
                Process_1027(Content);
                break;
            case "1029":
                Process_1029(Content);
                break;
            case "1031":
                Process_1031(Content);
                break;
            case "1032":
                Process_1032(Content);
                break;
            case "1041":
                Process_1041(Content);
                break;
            case "1042":
                Process_1042(Content);
                break;
            case "1091":
                Process_1091(Content);
                break;
            case "1092":
                Process_1092(Content);
                break;
            case "1093":
                Process_1093(Content);
                break;
            case "1101":
                Process_1101(Content);
                break;
            case "1102":
                Process_1102(Content);
                break;
            case "1103":
                Process_1103(Content);
                break;
            case "1104":
                Process_1104(Content);
                break;
            //case "1105":
            //    Process_1105(Content);
            //    break;
            //case "1106":
            //    Process_1106(Content);
            //    break;
            case "1107":
                Process_1107(Content);
                break;
            case "1141":
                Process_1141(Content);
                break;
            case "1201":
                Process_1201(Content);
                break;
            case "1202":
                Process_1202(Content);
                break;
            case "1204":
                Process_1204(Content);
                break;
            case "1205":
                Process_1205(Content);
                break;
            case "2001":
                Process_2001(Content);
                break;
            case "2002":
                Process_2002(Content);
                break;
            case "2004":
                Process_2004(Content);
                break;
            case "2009":
                Process_2009(Content);
                break;
            case "2010":
                Process_2010(Content);
                break;
            case "2011":
                Process_2011(Content);
                break;
            case "2012":
                Process_2012(Content);
                break;
            case "2013":
                Process_2013(Content);
                break;
            case "2014":
                Process_2014(Content);
                break;
            case "2015":
                Process_2015(Content);
                break;
            case "2016":
                Process_2016(Content);
                break;
            case "2017":
                Process_2017(Content);
                break;
            case "2018":
                Process_2018(Content);
                break;
            case "2101":
                Process_2101(Content);
                break;
            case "2102":
                Process_2102(Content);
                break;
            case "2103":
                Process_2103(Content);
                break;
            case "2104":
                Process_2104(Content);
                break;
            case "3001":
                Process_3001(Content);
                break;
            case "3004":
                Process_3004(Content);
                break;
            case "3005":
                Process_3005(Content);
                break;
            case "3006":
                Process_3006(Content);
                break;
            case "3007":
                Process_3007(Content);
                break;
            case "3009":
                Process_3009(Content);
                break;
            case "3010":
                Process_3010(Content);
                break;
            case "3016":
                Process_3016(Content);
                break;
            case "3017":
                Process_3017(Content);
                break;
            case "3018":
                Process_3018(Content);
                break;
            case "3019":
                Process_3019(Content);
                break;
            case "3020":
                Process_3020(Content);
                break;
            case "3021":
                Process_3021(Content);
                break;
            case "3101":
                Process_3101(Content);
                break;
            case "3102":
                Process_3102(Content);
                break;
            case "3103":
                Process_3103(Content);
                break;
            case "3104":
                Process_3104(Content);
                break;
            case "3201":
                Process_3201(Content);
                break;
            case "3202":
                Process_3202(Content);
                break;
            case "3301":
                Process_3301(Content);
                break;
            case "3302":
                Process_3302(Content);
                break;
            case "3303":
                Process_3303(Content);
                break;
            case "3304":
                Process_3304(Content);
                break;
            case "3305":
                Process_3305(Content);
                break;
            case "3306":
                Process_3306(Content);
                break;
            case "3307":
                Process_3307(Content);
                break;
            case "3308":
                Process_3308(Content);
                break;
            case "3401":
                Process_3401(Content);
                break;
            case "3402":
                Process_3402(Content);
                break;
            case "5001":
                Process_5001(Content);
                break;
            case "5003":
                Process_5003(Content);
                break;
            case "5004":
                Process_5004(Content);
                break;
            case "5015":
                Process_5015(Content);
                break;
            case "5005":
                Process_5005(Content);
                break;
            case "5007":
                Process_5007(Content);
                break;
            case "5008":
                Process_5008(Content);
                break;
            case "5006":
                Process_5006(Content);
                break;
            case "5009":
                Process_5009(Content);
                break;
            case "5010":
                Process_5010(Content);
                break;
            case "5011":
                Process_5011(Content);
                break;
            case "5102":
                Process_5102(Content);
                break;
            case "5103":
                Process_5103(Content);
                break;
            case "5105":
                Process_5105(Content);
                break;
            case "5201":
                Process_5201(Content);
                break;
            case "5202":
                Process_5202(Content);
                break;
            case "6001":
                Process_6001(Content);
                break;
            case "6002":
                Process_6002(Content);
                break;
            case "6003":
                Process_6003(Content);
                break;
            case "6004":
                Process_6004(Content);
                break;
            case "6005":
                Process_6005(Content);
                break;
            case "6006":
                Process_6006(Content);
                break;
            case "6007":
                Process_6007(Content);
                break;
            case "6008":
                Process_6008(Content);
                break;
            case "6301":
                Process_6301(Content);
                break;
            case "6302":
                Process_6302(Content);
                break;
            case "6303":
                Process_6303(Content);
                break;
            case "6304":
                Process_6304(Content);
                break;
            case "6305":
                Process_6305(Content);
                break;
            case "6306":
                Process_6306(Content);
                break;
            case "6307":
                Process_6307(Content);
                break;
            case "6308":
                Process_6308(Content);
                break;
            case "6501":
                Process_6501(Content);
                break;
            case "6502":
                Process_6502(Content);
                break;
            case "6503":
                Process_6503(Content);
                break;
            case "6504":
                Process_6504(Content);
                break;
            case "6505":
                Process_6505(Content);
                break;
            case "8001":
                Process_8001(Content);
                break;
            case "8002":
                Process_8002(Content);
                break;
            case "8003":
                Process_8003(Content);
                break;
            case "8004":
                Process_8004(Content);
                break;
            case "8005":
                Process_8005(Content);
                break;
            case "8006":
                Process_8006(Content);
                break;
            case "8007":
                Process_8007(Content);
                break;
            case "8008":
                Process_8008(Content);
                break;
            case "8009":
                Process_8009(Content);
                break;
            case "8010":
                Process_8010(Content);
                break;
            case "8011":
                Process_8011(Content);
                break;
            case "8012":
                Process_8012(Content);
                break;
            case "8013":
                Process_8013(Content);
                break;
            case "8014":
                Process_8014(Content);
                break;
            case "8015":
                Process_8015(Content);
                break;
            case "8016":
                Process_8016(Content);
                break;
            case "8017":
                Process_8017(Content);
                break;
            case "8018":
                Process_8018(Content);
                break;
            case "8019":
                Process_8019(Content);
                break;
            case "8020":
                Process_8020(Content);
                break;
            case "8021":
                Process_8021(Content);
                break;
            case "8022":
                Process_8022(Content);
                break;
            case "8023":
                Process_8023(Content);
                break;
            case "8102":
                Process_8102(Content);
                break;
            case "8103":
                Process_8103(Content);
                break;
            case "8104":
                Process_8104(Content);
                break;
            case "8105":
                Process_8105(Content);
                break;
            case "8201":
                Process_8201(Content);
                break;
            case "8202":
                Process_8202(Content);
                break;
            case "8303":
                Process_8303(Content);
                break;
            case "8304":
                Process_8304(Content);
                break;
            case "8305":
                Process_8305(Content);
                break;
            case "8306":
                Process_8306(Content);
                break;
            case "8307":
                Process_8307(Content);
                break;
            case "8308":
                Process_8308(Content);
                break;
            case "8309":
                Process_8309(Content);
                break;
            case "8310":
                Process_8310(Content);
                break;
            case "8401":
                Process_8401(Content);
                break;
            case "8402":
                Process_8402(Content);
                break;
            case "8403":
                Process_8403(Content);
                break;
            case "8501":
                Process_8501(Content);
                break;
            case "8502":
                Process_8502(Content);
                break;
            case "8503":
                Process_8503(Content);
                break;
            case "8504":
                Process_8504(Content);
                break;
            case "8506":
                Process_8506(Content);
                break;
            case "8601":
                Process_8601(Content);
                break;
            case "8602":
                Process_8602(Content);
                break;
            case "8603":
                Process_8603(Content);
                break;
            case "8604":
                Process_8604(Content);
                break;
            case "8605":
                Process_8605(Content);
                break;
            case "8606":
                Process_8606(Content);
                break;
            case "8607":
                Process_8607(Content);
                break;
            case "8608":
                Process_8608(Content);
                break;
            case "8609":
                Process_8609(Content);
                break;
            case "8610":
                Process_8610(Content);
                break;
            case "8611":
                Process_8611(Content);
                break;
            case "8612":
                Process_8612(Content);
                break;
            case "8613":
                Process_8613(Content);
                break;
            case "8614":
                Process_8614(Content);
                break;
            case "8615":
                Process_8615(Content);
                break;
            case "8616":
                Process_8616(Content);
                break;
            case "8617":
                Process_8617(Content);
                break;
            case "8618":
                Process_8618(Content);
                break;
            case "8619":
                Process_8619(Content);
                break;
            case "8620":
                Process_8620(Content);
                break;
            case "8621":
                Process_8621(Content);
                break;
            case "8622":
                Process_8622(Content);
                break;
            case "8623":
                Process_8623(Content);
                break;
            case "8624":
                Process_8624(Content);
                break;
            case "8625":
                Process_8625(Content);
                break;
            case "8626":
                Process_8626(Content);
                break;
            case "8627":
                Process_8627(Content);
                break;
            case "8628":
                Process_8628(Content);
                break;
            case "8629":
                Process_8629(Content);
                break;
            case "8630":
                Process_8630(Content);
                break;
            case "8631":
                Process_8631(Content);
                break;
            case "8632":
                Process_8632(Content);
                break;
            case "8633":
                Process_8633(Content);
                break;
            case "8634":
                Process_8634(Content);
                break;
            case "8635":
                Process_8635(Content);
                break;
            case "8636":
                Process_8636(Content);
                break;
            case "8637":
                Process_8637(Content);
                break;
            case "8638":
                Process_8638(Content);
                break;
            case "8639":
                Process_8639(Content);
                break;
            case "8640":
                Process_8640(Content);
                break;
            case "8641":
                Process_8641(Content);
                break;
            case "8701":
                Process_8701(Content);
                break;
            case "8702":
                Process_8702(Content);
                break;
            case "8703":
                Process_8703(Content);
                break;
            case "8704":
                Process_8704(Content);
                break;
            case "8705":
                Process_8705(Content);
                break;
            case "8706":
                Process_8706(Content);
                break;
            case "8707":
                Process_8707(Content);
                break;
            case "8708":
                Process_8708(Content);
                break;
            case "8709":
                Process_8709(Content);
                break;
            case "8710":
                Process_8710(Content);
                break;
            case "8711":
                Process_8711(Content);
                break;
            case "8712":
                Process_8712(Content);
                break;
            case "8713":
                Process_8713(Content);
                break;
            case "9001":
                Process_9001(Content);
                break;
            case "9002":
                Process_9002(Content);
                break;
            case "9003":
                Process_9003(Content);
                break;
            case "9004":
                Process_9004(Content);
                break;
            case "9005":
                Process_9005(Content);
                break;
            case "9006":
                Process_9006(Content);
                break;
            case "9007":
                Process_9007(Content);
                break;
            case "9101":
                Process_9101(Content);
                break;
            case "9102":
                Process_9102(Content);
                break;
            case "9103":
                Process_9103(Content);
                break;
            case "9131":
                Process_9131(Content);
                break;
            case "9132":
                Process_9132(Content);
                break;
            case "9133":
                Process_9133(Content);
                break;
            case "9134":
                Process_9134(Content);
                break;
            case "9151":
                Process_9151(Content);
                break;
            case "9152":
                Process_9152(Content);
                break;
            case "9153":
                Process_9153(Content);
                break;
            case "9201":
                Process_9201(Content);
                break;
            case "9202":
                Process_9202(Content);
                break;
            case "9301":
                Process_9301(Content);
                break;
            case "9511":
                Process_9511(Content);
                break;
            case "9701":
                Process_9701(Content);
                break;
            case "9702":
                Process_9702(Content);
                break;
            case "9711":
                Process_9711(Content);
                break;
            case "9712":
                Process_9712(Content);
                break;
            case "9713":
                Process_9713(Content);
                break;
            case "9721":
                Process_9721(Content);
                break;
            case "9722":
                Process_9722(Content);
                break;
            case "9723":
                Process_9723(Content);
                break;
            case "2201":
                Process_2201(Content);
                break;
            case "2202":
                Process_2202(Content);
                break;
            case "2203":
                Process_2203(Content);
                break;
            case "2204":
                Process_2204(Content);
                break;
            case "2205":
                Process_2205(Content);
                break;
            case "2206":
                Process_2206(Content);
                break;
            case "1131":
                Process_1131(Content);
                break;
            case "1132":
                Process_1132(Content);
                break;
            case "1133":
                Process_1133(Content);
                break;
            case "1134":
                Process_1134(Content);
                break;
            case "1135":
                Process_1135(Content);
                break;
            case "1016":
                Process_1016(Content);
                break;
            case "1401":
                Process_1401(Content);
                break;
            case "1402":
                Process_1402(Content);
                break;
            case "1403":
                Process_1403(Content);
                break;
            case "1404":
                Process_1404(Content);
                break;
            case "1405":
                Process_1405(Content);
                break;
            case "1406":
                Process_1406(Content);
                break;
            case "1407":
                Process_1407(Content);
                break;
            case "1408":
                Process_1408(Content);
                break;
            case "1409":
                Process_1409(Content);
                break;
            case "1410":
                Process_1410(Content);
                break;
            case "1501":
                Process_1501(Content);
                break;
            case "1502":
                Process_1502(Content);
                break;
            case "1503":
                Process_1503(Content);
                break;
            case "1504":
                Process_1504(Content);
                break;
            case "1505":
                Process_1505(Content);
                break;
            case "1506":
                Process_1506(Content);
                break;
            case "1507":
                Process_1507(Content);
                break;
            case "1508":
                Process_1508(Content);
                break;
            case "1509":
                Process_1509(Content);
                break;
            case "1510":
                Process_1510(Content);
                break;
            case "1511":
                Process_1511(Content);
                break;
            case "1512":
                Process_1512(Content);
                break;
            case "1601":
                Process_1601(Content);
                break;
            case "1602":
                Process_1602(Content);
                break;
            case "1603":
                Process_1603(Content);
                break;
            case "1611":
                Process_1611(Content);
                break;
            case "1612":
                Process_1612(Content);
                break;
            case "1621":
                Process_1621(Content);
                break;
            case "1622":
                Process_1622(Content);
                break;
            case "1623":
                Process_1623(Content);
                break;
            case "1624":
                Process_1624(Content);
                break;
            case "1701":
                Process_1701(Content);
                break;
            case "1702":
                Process_1702(Content);
                break;
            case "1703":
                Process_1703(Content);
                break;
            case "1704":
                Process_1704(Content);
                break;
            case "1801":
                Process_1801(Content);
                break;
            case "1802":
                Process_1802(Content);
                break;
            case "1803":
                Process_1803(Content);
                break;
            case "1804":
                Process_1804(Content);
                break;
            case "1901":
                Process_1901(Content);
                break;
            case "1902":
                Process_1902(Content);
                break;
            case "1903":
                Process_1903(Content);
                break;
            case "1904":
                Process_1904(Content);
                break;
            case "1911":
                Process_1911(Content);
                break;
            case "1912":
                Process_1912(Content);
                break;
            case "1921":
                Process_1921(Content);
                break;
            case "1922":
                Process_1922(Content);
                break;
            case "1705":
                Process_1705(Content);
                break;
            case "6009":
                Process_6009(Content);
                break;
            case "6010":
                Process_6010(Content);
                break;
            case "6011":
                Process_6011(Content);
                break;
            case "6012":
                Process_6012(Content);
                break;
            case "7001":
                Process_7001(Content);
                break;
            case "7003":
                Process_7003(Content);
                break;
            case "7004":
                Process_7004(Content);
                break;
            case "7101":
                Process_7101(Content);
                break;
            case "7102":
                Process_7102(Content);
                break;
            case "7103":
                Process_7103(Content);
                break;
            case "7104":
                Process_7104(Content);
                break;
            case "7105":
                Process_7105(Content);
                break;
            case "7106":
                Process_7106(Content);
                break;
            case "7107":
                Process_7107(Content);
                break;
            case "7108":
                Process_7108(Content);
                break;
            case "7109":
                Process_7109(Content);
                break;
            case "7110":
                Process_7110(Content);
                break;
            case "7111":
                Process_7111(Content);
                break;
            case "5204":
                Process_5204(Content);
                break;
            case "6013":
                Process_6013(Content);
                break;
            case "6014":
                Process_6014(Content);
                break;
            case "9401":
                Process_9401(Content);
                break;
            case "9402":
                Process_9402(Content);
                break;
            case "9411":
                Process_9411(Content);
                break;
            case "9501":
                Process_9501(Content);
                break;
            case "9521":
                Process_9521(Content);
                break;
            case "6015":
                Process_6015(Content);
                break;
            case "6016":
                Process_6016(Content);
                break;
            case "6017":
                Process_6017(Content);
                break;
            case "9121":
                Process_9121(Content);
                break;
            case "9122":
                Process_9122(Content);
                break;
            case "9123":
                Process_9123(Content);
                break;
            case "5012":
                Process_5012(Content);
                break;
            case "5013":
                Process_5013(Content);
                break;
            case "5014":
                Process_5014(Content);
                break;
            case "5301":
                Process_5301(Content);
                break;
            case "5302":
                Process_5302(Content);
                break;
            case "5303":
                Process_5303(Content);
                break;
            case "5401":
                Process_5401(Content);
                break;
            case "1019":
                Process_1019(Content);
                break;
            case "1020":
                Process_1020(Content);
                break;
            case "5002":
                Process_5002(Content);
                break;
            case "9902":
                Process_9902(Content);
                break;
            case "7002":
                Process_7002(Content);
                break;
            case "9998":
                Process_9998(Content);
                break;
            case "6101":
                Process_6101(Content);
                break;
            case "6102":
                Process_6102(Content);
                break;
            case "6103":
                Process_6103(Content);
                break;
            case "6104":
                Process_6104(Content);
                break;
            case "6105":
                Process_6105(Content);
                break;
            case "6106":
                Process_6106(Content);
                break;
            case "6107":
                Process_6107(Content);
                break;
            case "6108":
                Process_6108(Content);
                break;
            case "6109":
                Process_6109(Content);
                break;
            case "6110":
                Process_6110(Content);
                break;
            case "6111":
                Process_6111(Content);
                break;
            case "6112":
                Process_6112(Content);
                break;
            case "6113":
                Process_6113(Content);
                break;
            case "6201":
                Process_6201(Content);
                break;
            case "6202":
                Process_6202(Content);
                break;
            case "6203":
                Process_6203(Content);
                break;
            case "6204":
                Process_6204(Content);
                break;
            case "6205":
                Process_6205(Content);
                break;
            case "6206":
                Process_6206(Content);
                break;
            case "6207":
                Process_6207(Content);
                break;
            case "6208":
                Process_6208(Content);
                break;
            case "6209":
                Process_6209(Content);
                break;
            case "6210":
                Process_6210(Content);
                break;
            case "6211":
                Process_6211(Content);
                break;
            case "6212":
                Process_6212(Content);
                break;
            case "6401":
                Process_6401(Content);
                break;
            case "6402":
                Process_6402(Content);
                break;
            case "6403":
                Process_6403(Content);
                break;
            case "6404":
                Process_6404(Content);
                break;
            case "6405":
                Process_6405(Content);
                break;
            case "6601":
                Process_6601(Content);
                break;
            case "6602":
                Process_6602(Content);
                break;
            case "6603":
                Process_6603(Content);
                break;
            case "6604":
                Process_6604(Content);
                break;
            case "9607":
                Process_9607(Content);
                break;
            case "9608":
                Process_9608(Content);
                break;
            case "9609":
                Process_9609(Content);
                break;
            case "9610":
                Process_9610(Content);
                break;
            case "9611":
                Process_9611(Content);
                break;
            case "9612":
                Process_9612(Content);
                break;
            case "9613":
                Process_9613(Content);
                break;
            case "9731":
                Process_9731(Content);
                break;
            case "9732":
                Process_9732(Content);
                break;
            case "9733":
                Process_9733(Content);
                break;
            case "9734":
                Process_9734(Content);
                break;
            default:
                break;
        }
    }

    public void SplitPack(string InputString)
    {
#if UNITY_EDITOR
        Debug.Log("RecvString:" + InputString);
#endif
        string[] DataSplit = InputString.Split(new char[] { '#' });

        switch (DataSplit[0])
        {
            case "9801":
                string[] Version = DataSplit[1].Split(';');
                Downloader.instance.SetVersion(int.Parse(Version[0]), int.Parse(Version[1]), Version[2], Version[3]);
                break;
            default:
                //Debug.Log(DataSplit[0] + "----" + DataSplit[1]);
                LuaDeliver.instance.func.Call(DataSplit[0], DataSplit[1]);
                break;
        }
    }
    ////////////////////////////////////////////////////传送讯息(以上)////////////////////////////////////////////////////


    public void Process_1001(string RecvString)
    {
        string[] DataSplit = RecvString.Split(';');
        TextTranslator.instance.ServerInfo = DataSplit[2];
        PlayerPrefs.SetInt("Relogin", 0);
        IsSDKLogin = true;
        if (DataSplit[0] == "1")
        {
            ObscuredPrefs.SetInt("CharacterID", int.Parse(DataSplit[1]));
            TextTranslator.instance.ServerInfo = DataSplit[2];
            string[] NowServerID = DataSplit[2].Split('$');
            for (int i = NowServerID.Length - 1; i >= 0; i--)
            {
                if (NowServerID[i] != "")
                {
                    PlayerPrefs.SetString("ServerID", NowServerID[i]);
                }
            }

            if (Account != "")
            {
                ObscuredPrefs.SetString("Account", Account);
                ObscuredPrefs.SetString("Password", Password);
            }
            if (!IsReconnect || Application.loadedLevelName == "Downloader")
            {
                if (Application.loadedLevelName != "Downloader")
                {
                    if (CharacterRecorder.instance.userId == 0)
                    {
                        SendProcess("1003#");
                        IsFirst = false;
                        UIManager.instance.OpenPanel("LoadingWindow", false);
                    }
                    //if (Application.loadedLevelName != "Downloader")
                    //IsFirst = true;
                    //UIManager.instance.OpenPanel("LoginWindow", true);
                }
                else
                {
                    UIManager.instance.OpenPanel("LoginWindow", true);
                }
            }
            else
            {
                IsException = false;
                IsReconnect = false;
                ReconnectCount = 0;
                UIManager.instance.OpenPromptWindow("断线重连成功", PromptWindow.PromptType.Hint, null, null);
                //新手引导断线重连刷新摄像机
                if (GameObject.Find("MapUiWindow") != null)
                {
                    NetworkHandler.instance.SendProcess("2201#;");
                    NetworkHandler.instance.SendProcess("2016#0;");
                    NetworkHandler.instance.SendProcess("6017#;");
                    NetworkHandler.instance.SendProcess("2001#;");
                }
                //if (GameObject.Find("FightWindow") != null)
                //{                    
                //    if (PictureCreater.instance.NowSequence > 1)
                //    {

                //    }
                //    else
                //    {
                //        PictureCreater.instance.StopFight(false);

                //        if (PictureCreater.instance.IsPVP)
                //        {
                //            PictureCreater.instance.StartPVP(PictureCreater.instance.PVPString);
                //        }
                //        else
                //        {
                //            PictureCreater.instance.StartFight();
                //        }
                //    }
                //}
            }
            ObscuredPrefs.SetString("LastChatItemInfoOnTeam", "");//登录时清除组队聊天邀请按钮上的信息
        }
        else
        {
            switch (DataSplit[1])
            {
                case "1":
                    break;
                case "2":
                    PlayerPrefs.SetInt("UserID", int.Parse(DataSplit[2]));

                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 0);
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 0);

                    if (Application.loadedLevelName == "Downloader")
                    {
                        LuaDeliver.instance.ResetGuide();
                        ObscuredPrefs.SetString("Account", Account);
                        ObscuredPrefs.SetString("Password", Password);

                        if (!IsAuto)
                        {
                            ObscuredPrefs.SetInt("CharacterID", int.Parse(DataSplit[2]));
                            //UIManager.instance.OpenPanel("RegisterWindow", true);
                            //SendProcess("1002#" + ObscuredPrefs.GetInt("CharacterID").ToString() + ";" + Account + ";");
                        }
                        else
                        {
                            ObscuredPrefs.SetInt("CharacterID", int.Parse(DataSplit[2]));
                            //SendProcess("1002#" + ObscuredPrefs.GetInt("CharacterID").ToString() + ";" + Account + ";");
                        }

                        IsCreate = true;
                        PlayerPrefs.SetInt("Speed", 0);
                        PlayerPrefs.SetInt("Hand", 0);
                        //PlayerPrefs.SetInt("PvpRankPodition", 0);//竞技场布阵红点
                        //PlayerPrefs.SetInt("ActivityFinalReward", 0);//大放送最终奖励
                        //PlayerPrefs.SetInt("QuestionState", 0);//问卷调查
                        PlayerPrefs.SetString("ServerID", "0");
                        UIManager.instance.OpenPanel("LoginWindow", true);
                    }
                    else
                    {
                        //IsCreate = true;
                        //UIManager.instance.OpenPanel("LoadingWindow", false);
                        //LuaDeliver.instance.ResetGuide();
                        Destroy(GameObject.Find("StartWindow"));
                        UIManager.instance.OpenPanel("LoadingWindow", false);
                        GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().IsAuto = true;
                        GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().ShowHero(0, true);

                        //////////////////////////////////
                        XMLParser.instance.ShowWeiJia();
                        //////////////////////////////////


                        //UIManager.instance.OpenPanel("StartNameWindow", true);

                        if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0)
                        {
                            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_201);
                        }
                        //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
                        //StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Item"));
                        //TextAsset LuaText = (TextAsset)Resources.Load("CN/Item/Item");
                        //XMLParser.instance.ParseXMLItemScript(LuaText.text);
                        //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
                    }
                    break;
                case "3":
                    break;
                case "9":
                    UIManager.instance.OpenPromptWindow("您的帐号已被冻结，如有问题请咨询客服!", PromptWindow.PromptType.Alert, null, null);
                    break;
            }
        }
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForEndOfFrame();
        if (!IsLogin)
        {
            IsLogin = true;
            AsyncOperation async = Application.LoadLevelAsync("MainScene");
            yield return async;
        }
    }

    public void Process_1002(string RecvString)
    {
        string[] DataSplit = RecvString.Split(';');
        if (DataSplit[0] == "1")
        {
            if (Account != "")
            {
                ObscuredPrefs.SetString("Account", Account);
                ObscuredPrefs.SetString("Password", Password);
            }
            ObscuredPrefs.SetInt("CharacterID", int.Parse(DataSplit[1]));
            IsPrefetch = false;
            IsCreate = false;
            PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 2);
            //PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 18);
            if (ObscuredPrefs.GetString("Account").IndexOf("test_") > -1)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 18);
            }
            PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideSubStateName(), 1);
            //Application.LoadLevel("LoadScene");
            StartCoroutine(LoadScene());
            UIManager.instance.DestroyAllPanel();
            UIManager.instance.OpenPanel("LoadingWindow", true);
            UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_500);
            CharacterRecorder.instance.SetNewPlayerPrefsKey();//建立新号时创建唯一playerfabs,防止换号清除
            PlayerPrefs.SetInt("BulletInfo" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 0);
            PlayerPrefs.SetInt("StormBuff" + "_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 0);
            PlayerPrefs.SetInt("ThreatBuff" + "_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 0);


#if JINLI
        APaymentHelperDemo.instance.SetGameDate(DataSplit[1],
                DataSplit[2], "1",
                PlayerPrefs.GetString("ServerID"), PlayerPrefs.GetString("ServerName"), "createrole");
#elif KY
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            APaymentHelperDemo.instance.SetGameDate(DataSplit[1],
                DataSplit[2], "1",
                PlayerPrefs.GetString("ServerID"), PlayerPrefs.GetString("ServerName"), "createrole");
        }
#elif HOLA || QIHOO360 || QIANHUAN
        
        Dictionary<string, string> mDic = new Dictionary<string, string>();
        mDic.Add("roleId", DataSplit[1]);
        mDic.Add("roleName", DataSplit[2]);
        mDic.Add("roleLevel", "1");
        mDic.Add("zoneId", PlayerPrefs.GetString("ServerID"));
        mDic.Add("zoneName", PlayerPrefs.GetString("ServerName"));
        mDic.Add("roleCTime", CharacterRecorder.CTime);
        mDic.Add("roleLevelMTime", Utils.GetNowTimeUTC().ToString());
        mDic.Add("vip", "0");
        HolagamesSDK.getInstance().loginGameRole("create", HolagamesSDK.dictionaryToString(mDic));
#endif
        }
        else
        {
            switch (DataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("名称非法", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("名称重复", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("名称非法", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }

    public void Process_1003(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.Vip = int.Parse(dataSplit[14]);
        CharacterRecorder.instance.legionID = int.Parse(dataSplit[19]);
        CharacterRecorder.instance.IsOpenMapGate = false;
        if (CharacterRecorder.instance.legionID != 0)
        {
            NetworkHandler.instance.SendProcess(string.Format("8004#{0};", CharacterRecorder.instance.legionID));
            NetworkHandler.instance.SendProcess("8201#;");
        }
        PlayerPrefs.SetInt("RedHeroIsOpen", 0);
        CharacterRecorder.instance.Landingdays = int.Parse(dataSplit[25]);

        /////////////////////////////////商城红点/////////////////////////////////
        if (PlayerPrefs.GetInt("MallCount", 0) != CharacterRecorder.instance.Landingdays)
        {
            PlayerPrefs.SetInt("MallCount", CharacterRecorder.instance.Landingdays);
            PlayerPrefs.SetInt("ShopBuy", 1);
        }
        /////////////////////////////////商城红点/////////////////////////////////

        CharacterRecorder.instance.legionCountryID = int.Parse(dataSplit[26]);
        CharacterRecorder.instance.NationID = int.Parse(dataSplit[27]);
        CharacterRecorder.instance.TrialCurrency = int.Parse(dataSplit[20]);
        CharacterRecorder.instance.ArmyGroup = int.Parse(dataSplit[21]);
        CharacterRecorder.instance.HonerValue = int.Parse(dataSplit[22]);
        CharacterRecorder.instance.headIcon = int.Parse(dataSplit[23]);
        CharacterRecorder.CTime = dataSplit[dataSplit.Length - 2];
        CharacterRecorder.instance.lastCreamGateID = int.Parse(dataSplit[24]);
        CharacterRecorder.instance.GoldBar = int.Parse(dataSplit[28]);
        CharacterRecorder.instance.SetCharacter(dataSplit[0], int.Parse(dataSplit[1]),
           int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]),
           int.Parse(dataSplit[12]), int.Parse(dataSplit[5]), int.Parse(dataSplit[13]), 0, 0, int.Parse(dataSplit[6]), int.Parse(dataSplit[7]), int.Parse(dataSplit[10]), int.Parse(dataSplit[8]), int.Parse(dataSplit[9]), int.Parse(dataSplit[11]));
        //TaskDesigner.instance.Init(int.Parse(dataSplit[10]), int.Parse(dataSplit[11]), int.Parse(dataSplit[12]));        
        //SceneTransformer.instance.CreateMainScene();
        if (PlayerPrefs.HasKey("MainScenceNum") == false) //记录主场景切换到第几个了
        {
            PlayerPrefs.SetInt("MainScenceNum", 1);
        }

        AudioEditer.instance.PlayLoop("Scene");
        if (ObscuredPrefs.GetString("Account").IndexOf("test_") > -1)
        {
            PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 18);
        }
        else if (SceneTransformer.instance.CheckGuideIsFinish() == false)
        {

            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 7 || PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 11 || PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 13)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + 1);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 8)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 6);
            }


            //////////////////////紧急防卡死机制(以下)//////////////////////
            if (CharacterRecorder.instance.level > 5)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 18);
            }
            Debug.LogError("SSSSS   " + PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) + "   " + PlayerPrefs.GetInt
            (LuaDeliver.instance.GetGuideSubStateName()));
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 0)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 2);
            }
            if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 4 && CharacterRecorder.instance.lastGateID != 10002)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 3);
            }
            else if ((PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 8 && CharacterRecorder.instance.lastGateID != 10003) || CharacterRecorder.instance.level == 2)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 6);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 12 && CharacterRecorder.instance.lastGateID != 10004 || CharacterRecorder.instance.level == 3)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 10);
            }
            else if (PlayerPrefs.GetInt(LuaDeliver.instance.GetGuideStateName()) == 14 && CharacterRecorder.instance.lastGateID != 10005 || CharacterRecorder.instance.level == 4)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 12);
            }
            //////////////////////紧急防卡死机制(以上)//////////////////////
        }
#if UC || HUAWEI || WDJ
        
        Dictionary<string, string> mDic = new Dictionary<string, string>();
        mDic.Add("roleId", CharacterRecorder.instance.userId.ToString());
        mDic.Add("roleName", CharacterRecorder.instance.characterName);
        mDic.Add("roleLevel", CharacterRecorder.instance.level.ToString());
        mDic.Add("zoneId", PlayerPrefs.GetString("ServerID"));
        mDic.Add("zoneName", PlayerPrefs.GetString("ServerName"));
        mDic.Add("roleCTime", CharacterRecorder.CTime);
        mDic.Add("roleLevelMTime", Utils.GetNowTimeUTC().ToString());
        HolagamesSDK.getInstance().loginGameRole("loginGameRole", HolagamesSDK.dictionaryToString(mDic));

        HolagamesSDK.getInstance().showToolBar();

#elif HOLA || QIHOO360 || QIANHUAN
        
        Dictionary<string, string> mDic = new Dictionary<string, string>();
        mDic.Add("roleId", CharacterRecorder.instance.userId.ToString());
        mDic.Add("roleName", CharacterRecorder.instance.characterName);
        mDic.Add("roleLevel", CharacterRecorder.instance.level.ToString());
        mDic.Add("zoneId", PlayerPrefs.GetString("ServerID"));
        mDic.Add("zoneName", PlayerPrefs.GetString("ServerName"));
        mDic.Add("roleCTime", CharacterRecorder.CTime);
        mDic.Add("roleLevelMTime", Utils.GetNowTimeUTC().ToString());
        mDic.Add("vip", CharacterRecorder.instance.Vip.ToString());
        HolagamesSDK.getInstance().loginGameRole("enter", HolagamesSDK.dictionaryToString(mDic));
        
#elif QQ
                using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        jo.Call("CheckBill", CharacterRecorder.instance.userId.ToString() + "-" + PlayerPrefs.GetString("ServerID")  + "-1-6");
                    }
                }
#elif JINLI
        APaymentHelperDemo.instance.SetGameDate(CharacterRecorder.instance.userId.ToString(),
                CharacterRecorder.instance.characterName, CharacterRecorder.instance.level.ToString(),
                PlayerPrefs.GetString("ServerID"), PlayerPrefs.GetString("ServerName"), "enterServer");
#elif KY
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            APaymentHelperDemo.instance.SetGameDate(CharacterRecorder.instance.userId.ToString(),
                CharacterRecorder.instance.characterName, CharacterRecorder.instance.level.ToString(),
                PlayerPrefs.GetString("ServerID"), PlayerPrefs.GetString("ServerName"), "enterServer");
        }
#endif
        //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
        StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Item"));
        //TextAsset LuaText = (TextAsset)Resources.Load("CN/Item/Item");
        //XMLParser.instance.ParseXMLItemScript(LuaText.text);
        //////////////////////////////////////////////////////XML///////////////////////////////////////////////////
    }

    public void Process_1004(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        for (int i = 0; i < CharacterRecorder.instance.ownedHeroList.size; i++)
        {
            if (CharacterRecorder.instance.ownedHeroList[i].characterRoleID == -1)
            {
                CharacterRecorder.instance.ownedHeroList.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] heroSplit = dataSplit[i].Split('$');
            bool IsRepeat = false;

            foreach (var r in CharacterRecorder.instance.ownedHeroList)
            {
                if (r.characterRoleID == int.Parse(heroSplit[0]))
                {
                    IsRepeat = true;
                    break;
                }
            }

            if (!IsRepeat)
            {
                CharacterRecorder.instance.AddOwnedHeroList(new Hero(int.Parse(heroSplit[0]), int.Parse(heroSplit[1]), 0, 0, 0, 0, 0, 0));
                /* string[] fateSplit = heroSplit[2].Split('!');
                 for (int j = 0; j < fateSplit.Length - 1; j++)
                 {
                     CharacterRecorder.instance.ListRoleFateId.Add(int.Parse(fateSplit[j]));
                 }*/
            }
            string[] fateSplit = heroSplit[2].Split('!');
            for (int j = 0; j < fateSplit.Length - 1; j++)
            {
                if (!CharacterRecorder.instance.ListRoleFateId.Contains(int.Parse(fateSplit[j])))
                {
                    CharacterRecorder.instance.ListRoleFateId.Add(int.Parse(fateSplit[j]));
                }
            }
        }
        /////////////////////////****新手引导防卡死****/////////////////////////////
        if (GameObject.Find("CardIntroduceWindow") == null && GameObject.Find("GaChaGetWindow") == null && GameObject.Find("GachaWindow") == null && SceneTransformer.instance.CheckGuideIsFinish() == false)
        {
            if (CharacterRecorder.instance.level == 1)
            {
                if (CharacterRecorder.instance.ownedHeroList.size < 2)
                {
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 2);
                }
                else
                {
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 3);
                }
            }
            else if (CharacterRecorder.instance.level == 2)
            {
                if (CharacterRecorder.instance.ownedHeroList.size < 3)
                {
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 4);
                }
                else
                {
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 6);
                }
            }
            if (ObscuredPrefs.GetString("Account").IndexOf("test_") > -1)
            {
                PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 18);
            }
        }
        /////////////////////////****新手引导防卡死****/////////////////////////////
    }

    public void Process_1005(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit.Length > 1)
        {
            string[] heroSplit = dataSplit[0].Split('$');

            if (isFirstUpHero && CharacterRecorder.instance.ownedHeroList.size != 1)
            {
                CharacterRecorder.instance.upDateOwnenHeroList.Clear();
                foreach (var item in CharacterRecorder.instance.ownedHeroList)
                {
                    CharacterRecorder.instance.upDateOwnenHeroList.Add(item);
                }
            }
            isFirstUpHero = false;
            foreach (Hero h in CharacterRecorder.instance.ownedHeroList)
            {
                if (h.characterRoleID == int.Parse(heroSplit[0]))
                {
                    h.SetHeroProperty(int.Parse(heroSplit[1]), int.Parse(heroSplit[2]), int.Parse(heroSplit[3]), int.Parse(heroSplit[4]), int.Parse(heroSplit[5]),
                        int.Parse(heroSplit[6]), int.Parse(heroSplit[7]), int.Parse(heroSplit[8]), int.Parse(heroSplit[9]), int.Parse(heroSplit[10]),
                        int.Parse(heroSplit[11]), int.Parse(heroSplit[12]), float.Parse(heroSplit[13]), float.Parse(heroSplit[14]), float.Parse(heroSplit[15]),
                        float.Parse(heroSplit[16]), float.Parse(heroSplit[17]), float.Parse(heroSplit[18]), int.Parse(heroSplit[19]), int.Parse(heroSplit[20]),
                        int.Parse(heroSplit[21]), int.Parse(heroSplit[22]), int.Parse(heroSplit[23]), float.Parse(heroSplit[24]), float.Parse(heroSplit[25]), float.Parse(heroSplit[26]), float.Parse(heroSplit[27]), float.Parse(heroSplit[28]), float.Parse(heroSplit[29]), int.Parse(heroSplit[30]));

                    TextTranslator.instance.heroNow = h;
                    //存入核武器信息
                    BetterList<Hero.WeaponInfo> weaponList = new BetterList<Hero.WeaponInfo>();
                    Hero.WeaponInfo superCarInfo;
                    if (SceneTransformer.instance.CheckGuideIsFinish())
                    {
                        superCarInfo = new Hero.WeaponInfo(int.Parse(heroSplit[31]), int.Parse(heroSplit[32]), int.Parse(heroSplit[33]), h.cardID);
                    }
                    else
                    {
                        superCarInfo = new Hero.WeaponInfo(0, 0, 0, h.cardID);
                    }
                    weaponList.Add(superCarInfo);
                    h.SetWeaponInfo(weaponList);
                    //
                    if (GameObject.Find("RoleWindow") != null)
                    {
                        RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
                        rw.UpDateDownHeroIcon();
                        if (rw.HeroLevelUpPart.activeSelf)
                        {
                            HeroLevelUpPart hlup = rw.HeroLevelUpPart.GetComponent<HeroLevelUpPart>();
                            hlup.UpdateState();
                            //rw.UpDateDownHeroIcon();
                        }
                        if (rw.HeroAdvancedPart.activeSelf)
                        {
                            HeroAdvancedPart hap = rw.HeroAdvancedPart.GetComponent<HeroAdvancedPart>();
                            hap.UpdateState();
                            //rw.UpDateDownHeroIcon();
                        }
                        if (rw.HeroBreakUpPart.activeSelf)
                        {
                            HeroBreakUpPart hbup = rw.HeroBreakUpPart.GetComponent<HeroBreakUpPart>();
                            //SendProcess("1024#" + heroSplit[0] + ";");
                            //hbup.SetOldState();
                            //rw.UpDateDownHeroIcon();
                        }
                    }
                    if (GameObject.Find("StrengEquipWindow") != null)
                    {
                        GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().InitHeroList();
                    }
                    GameObject rebirthObj = GameObject.Find("RebirthWindow");
                    if (rebirthObj != null)
                    {
                        //Debug.LogError("1005协议返回!");
                        rebirthObj.GetComponent<RebirthWindow>().SetWindowInfo(rebirthObj.GetComponent<RebirthWindow>().presentHeroId);
                    }
                }
            }
            // SortHeroListByForce();
            /*  Hero hero = CharacterRecorder.instance.GetHeroByCharacterRoleID(charaterRoleId);
              hero.score = GetScoreOfOneHero(hero);*/
        }
    }
    int GetScoreOfOneHero(Hero mHero)
    {
        float heroNum = 0;
        switch (mHero.rare)
        {
            case 1: heroNum = 1; break;
            case 2: heroNum = 1.1f; break;
            case 3: heroNum = 1.2f; break;
            case 4: heroNum = 1.4f; break;
            case 5: heroNum = 1.8f; break;
        }
        int score1 = (int)(mHero.level * TextTranslator.instance.GetLabsPointByID(1).RankPoint * heroNum);
        int score2 = (int)(mHero.classNumber * TextTranslator.instance.GetLabsPointByID(2).RankPoint * heroNum);
        int score3 = (int)((mHero.rank - 1) * TextTranslator.instance.GetLabsPointByID(3).RankPoint * heroNum);
        int score4 = 0;
        int _rank4Point = TextTranslator.instance.GetLabsPointByID(4).RankPoint;
        int score5 = 0;
        int _rank5Point = TextTranslator.instance.GetLabsPointByID(5).RankPoint;
        int score6 = 0;
        int _rank6Point = TextTranslator.instance.GetLabsPointByID(6).RankPoint;
        int score7 = 0;
        int _rank7Point = TextTranslator.instance.GetLabsPointByID(7).RankPoint;
        for (int i = 0; i < mHero.equipList.size; i++)
        {
            if (i < 4)
            {
                score4 += mHero.equipList[i].equipColorNum * _rank4Point;
                score5 += mHero.equipList[i].equipClass * _rank5Point;
            }
            else
            {
                int grade = TextTranslator.instance.GetItemByItemCode(mHero.equipList[i].equipCode).itemGrade;
                int myNum = 0;
                switch (grade)
                {
                    case 1: myNum = 0; break;
                    case 2: myNum = 1; break;
                    case 3: myNum = 2; break;
                    case 4: myNum = 3; break;
                    case 5: myNum = 4; break;
                    case 6: myNum = 5; break;
                }
                score6 += mHero.equipList[i].equipLevel * _rank6Point * myNum;
                score7 += mHero.equipList[i].equipClass * _rank7Point * myNum;
            }
        }
        int score8 = (int)(mHero.skillNumber * TextTranslator.instance.GetLabsPointByID(8).RankPoint * heroNum);
        int score9 = 0;
        int _rank9Point = TextTranslator.instance.GetLabsPointByID(9).RankPoint;
        for (int i = 0; i < mHero.rareStoneList.size; i++)
        {
            int grade = TextTranslator.instance.GetItemByItemCode(mHero.rareStoneList[i].stoneId).itemGrade;
            int myNum = 0;
            switch (grade)
            {
                case 1: myNum = 1; break;
                case 2: myNum = 2; break;
                case 3: myNum = 3; break;
                case 4: myNum = 4; break;
                case 5: myNum = 5; break;
                case 6: myNum = 6; break;
            }
            score9 += mHero.rareStoneList[i].stoneLevel * _rank9Point * myNum;
        }
        return score1 + score2 + score3 + score4 + score5 + score6 + score7 + score8 + score9;
    }
    //战力降序排序
    void SortHeroListByForce()
    {
        int listSize = CharacterRecorder.instance.ownedHeroList.size;
        for (int i = 0; i < listSize; i++)
        {
            for (var j = listSize - 1; j > i; j--)
            {
                Hero heroA = CharacterRecorder.instance.ownedHeroList[i];
                Hero heroB = CharacterRecorder.instance.ownedHeroList[j];
                if (heroA.force < heroB.force)
                {
                    var temp = CharacterRecorder.instance.ownedHeroList[i];
                    CharacterRecorder.instance.ownedHeroList[i] = CharacterRecorder.instance.ownedHeroList[j];
                    CharacterRecorder.instance.ownedHeroList[j] = temp;
                }
            }
        }

    }
    public void Process_1006(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            TextTranslator.instance.isUpdateBag = true;
            HeroNewData heroNewData = new HeroNewData();
            heroNewData.force = int.Parse(dataSplit[6]);
            heroNewData.classNumber = int.Parse(dataSplit[2]);
            GameObject.Find("HeroAdvancedPart").GetComponent<HeroAdvancedPart>().PlayShengPinEffect(heroNewData);
            SendProcess("1005#" + dataSplit[1] + ";");
            /* GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/JueSeShengPin", typeof(GameObject)), PictureCreater.instance.ListRolePicture[0].RoleObject.transform.position, Quaternion.identity) as GameObject;
             AudioEditer.instance.PlayOneShot("ui_levelup");*/
            CharacterRecorder.instance.IsOpen = true;
            TextTranslator.instance.isUpdateBag = true;
            CharacterRecorder.instance.AddMoney(-int.Parse(dataSplit[3]));//升级刷新Top金币
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();

            if (int.Parse(dataSplit[2]) >= 9 && int.Parse(dataSplit[2]) < 14) //紫色
            {
                string heroName = CharacterRecorder.instance.GetHeroByCharacterRoleID(int.Parse(dataSplit[1])).name;//TextTranslator.instance.GetItemNameByItemCode(int.Parse(dataSplit[4]));
                string colorName = SetNameColor(int.Parse(dataSplit[2]));
                SendProcess(string.Format("7002#{0};{1};{2};{3}", 29, CharacterRecorder.instance.characterName, heroName, colorName));
            }
            else if (int.Parse(dataSplit[2]) >= 14 && int.Parse(dataSplit[2]) <= 15)
            {
                string heroName = CharacterRecorder.instance.GetHeroByCharacterRoleID(int.Parse(dataSplit[1])).name;//橙色
                string colorName = SetNameColor(int.Parse(dataSplit[2]));
                SendProcess(string.Format("7002#{0};{1};{2};{3}", 30, CharacterRecorder.instance.characterName, heroName, colorName));
            }

        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("角色不存在", 11, false, PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("角色品阶达到上限", 11, false, PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("角色等级不足", 11, false, PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "4")
            {
                UIManager.instance.OpenPromptWindow("升品材料不足，点击材料图标可查看获取途径", 11, false, PromptWindow.PromptType.Hint, null, null);
                if (TextTranslator.instance.isUpdateBag)
                {
                    NetworkHandler.instance.SendProcess("5001#;");
                }
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("金币不足", 11, false, PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    string SetNameColor(int ColorNum)
    {
        string colorName = "";
        switch (ColorNum)
        {
            case 9:
                colorName = "紫色";
                break;
            case 10:
                colorName = "紫色+1";
                break;
            case 11:
                colorName = "紫色+2";
                break;
            case 12:
                colorName = "紫色+3";
                break;
            case 13:
                colorName = "紫色+4";
                break;
            case 14:
                colorName = "橙色";
                break;
            case 15:
                colorName = "橙色+1";
                break;
            default:
                break;
        }
        return colorName;
    }

    public void Process_1009(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.TaskAddExp = true;
        CharacterRecorder.instance.IsOpen = true;
        CharacterRecorder.instance.sprite = int.Parse(dataSplit[5]);
        CharacterRecorder.instance.spriteCap = int.Parse(dataSplit[6]);
        if (GameObject.Find("FightWindow") != null)
        {
            StartCoroutine(DelayOpenLevelUPwindow(dataSplit));
        }
        else
        {
            StartCoroutine(OpenLevelUPwindowAfterAdvance(dataSplit));
        }



#if UC || WDJ
        Dictionary<string, string> mDic = new Dictionary<string, string>();
        mDic.Add("roleId", CharacterRecorder.instance.userId.ToString());
        mDic.Add("roleName", CharacterRecorder.instance.characterName);
        mDic.Add("roleLevel", dataSplit[0]);
        mDic.Add("zoneId", PlayerPrefs.GetString("ServerID"));
        mDic.Add("zoneName", PlayerPrefs.GetString("ServerName"));
        mDic.Add("roleCTime", CharacterRecorder.CTime);
        mDic.Add("roleLevelMTime", Utils.GetNowTimeUTC().ToString());
        HolagamesSDK.getInstance().loginGameRole("loginGameRole", HolagamesSDK.dictionaryToString(mDic));

#elif HOLA || QIHOO360 || QIANHUAN
        Dictionary<string, string> mDic = new Dictionary<string, string>();
        mDic.Add("roleId", CharacterRecorder.instance.userId.ToString());
        mDic.Add("roleName", CharacterRecorder.instance.characterName);
        mDic.Add("roleLevel", dataSplit[0]);
        mDic.Add("zoneId", PlayerPrefs.GetString("ServerID"));
        mDic.Add("zoneName", PlayerPrefs.GetString("ServerName"));
        mDic.Add("roleCTime", CharacterRecorder.CTime);
        mDic.Add("roleLevelMTime", Utils.GetNowTimeUTC().ToString());
        mDic.Add("vip", CharacterRecorder.instance.Vip.ToString());
        HolagamesSDK.getInstance().loginGameRole("levelup", HolagamesSDK.dictionaryToString(mDic));                
#endif

        UIManager.instance.UmengCountLevel(int.Parse(dataSplit[0]));

    }
    IEnumerator DelayOpenLevelUPwindow(string[] dataSplit)
    {
        //Debug.LogError("体力。。。" + CharacterRecorder.instance.stamina);
        int lestGateID = CharacterRecorder.instance.lastGateID;
        yield return new WaitForSeconds(1f);
        while (GameObject.Find("ResultWindow") != null)
        {
            yield return new WaitForSeconds(0.2f);
        }
        UIManager.instance.OpenPanel("LevelUpWindow", false);
        AudioEditer.instance.PlayOneShot("ui_levelup");
        CharacterRecorder.instance.staminaOld = CharacterRecorder.instance.stamina;
        if (GameObject.Find("LevelUpWindow") != null)
        {
            LevelUpWindow luw = GameObject.Find("LevelUpWindow").GetComponent<LevelUpWindow>();
            luw.SetInfo(lestGateID, dataSplit[0], dataSplit[1], dataSplit[2]);
            CharacterRecorder.instance.exp = int.Parse(dataSplit[3]);
            CharacterRecorder.instance.expMax = int.Parse(dataSplit[4]);
        }
        StartCoroutine(SceneTransformer.instance.NewbieGuide());// 新手引导;
        if (GameObject.Find("TaskWindow") != null)
        {
            GameObject.Find("TaskWindow").GetComponent<TaskWindow>().SetLevelUpRefresh();
        }
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().ReSetRole();
        }
    }
    IEnumerator OpenLevelUPwindowAfterAdvance(string[] dataSplit)
    {
        int lestGateID = CharacterRecorder.instance.lastGateID;
        yield return new WaitForSeconds(1f);
        //if (GameObject.Find("TaskWindow") != null)
        //{
        //    yield return new WaitForSeconds(1f);
        //}
        while (GameObject.Find("ResultWindow") != null)
        {
            yield return new WaitForSeconds(0.2f);
        }
        while (GameObject.Find("AdvanceWindow") != null)
        {
            yield return new WaitForSeconds(0.2f);
        }
        CharacterRecorder.instance.staminaOld = CharacterRecorder.instance.stamina;
        UIManager.instance.OpenPanel("LevelUpWindow", false);
        AudioEditer.instance.PlayOneShot("ui_levelup");
        if (GameObject.Find("LevelUpWindow") != null)
        {
            LevelUpWindow luw = GameObject.Find("LevelUpWindow").GetComponent<LevelUpWindow>();
            luw.SetInfo(lestGateID, dataSplit[0], dataSplit[1], dataSplit[2]);
            CharacterRecorder.instance.exp = int.Parse(dataSplit[3]);
            CharacterRecorder.instance.expMax = int.Parse(dataSplit[4]);
        }
        StartCoroutine(SceneTransformer.instance.NewbieGuide());// 新手引导;
        if (GameObject.Find("TaskWindow") != null)
        {
            GameObject.Find("TaskWindow").GetComponent<TaskWindow>().SetLevelUpRefresh();
        }
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().ReSetRole();
        }
        yield return 0;
    }

    public void Process_1010(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
            HeroSkillPart hsp = rw.HeroSkillPart.GetComponent<HeroSkillPart>();
            hsp.UpDateState(true, int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
            SendProcess("1005#" + dataSplit[1] + ";");
            //SendProcess("1005#0;");
            if (int.Parse(dataSplit[3]) > 3)
            {
                SendProcess(string.Format("7002#{0};{1};{2};{3}", 3, CharacterRecorder.instance.characterName, CharacterRecorder.instance.GetHeroByCharacterRoleID(int.Parse(dataSplit[1])).name, dataSplit[3]));
            }

        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("技能等级达上限", PromptWindow.PromptType.Hint, null, null); break;
                case "1":
                    //UIManager.instance.OpenPromptWindow("当前等级不能天命", PromptWindow.PromptType.Hint, null, null);
                    //UIManager.instance.OpenPromptWindow(String.Format("{0}级开放技能突破", 29), PromptWindow.PromptType.Hint, null, null);
                    // UIManager.instance.OpenPromptWindow(String.Format("{0}级开放技能突破", 29), PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2": UIManager.instance.OpenPromptWindow("脑白金不足", PromptWindow.PromptType.Hint, null, null); break;
                case "9":
                    RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
                    HeroSkillPart hsp = rw.HeroSkillPart.GetComponent<HeroSkillPart>();
                    hsp.UpDateState(false, int.Parse(dataSplit[2]), -1, int.Parse(dataSplit[3]));
                    break;
                    //default: UIManager.instance.OpenPromptWindow(String.Format("服务器错误码{0}", dataSplit[1]), PromptWindow.PromptType.Hint, null, null); break;
            }

        }
    }

    public void Process_1011(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "")
        {
            string[] positionSplit = dataSplit[0].Split('!');
            CharacterRecorder.instance.PositionString = positionSplit[0];
            for (int i = 0; i < positionSplit.Length - 1; i++)
            {
                int CharacterRoleID = int.Parse(positionSplit[i].Split('$')[0]);
                int Position = int.Parse(positionSplit[i].Split('$')[1]);
                foreach (var h in CharacterRecorder.instance.ownedHeroList)
                {
                    if (h.characterRoleID == CharacterRoleID)
                    {
                        h.position = Position;
                    }
                }
            }
        }
    }

    public void Process_1012(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
    }
    public void Process_1013(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.headIcon = int.Parse(dataSplit[1]);
            if (GameObject.Find("MainWindow") != null)
            {
                MainWindow _MainWindow = GameObject.Find("MainWindow").GetComponent<MainWindow>();
                _MainWindow.headIcon.mainTexture = Resources.Load(string.Format("Head/{0}", CharacterRecorder.instance.headIcon), typeof(Texture)) as Texture;
            }

            if (GameObject.Find("InfoWindow") != null)
            {
                InfoWindow _InfoWindow = GameObject.Find("InfoWindow").GetComponent<InfoWindow>();
                _InfoWindow.headIcon.mainTexture = Resources.Load(string.Format("Head/{0}", CharacterRecorder.instance.headIcon), typeof(Texture)) as Texture;
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_1041(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.characterName = dataSplit[1];
            if (GameObject.Find("MainWindow") != null)
            {
                MainWindow _MainWindow = GameObject.Find("MainWindow").GetComponent<MainWindow>();
                _MainWindow.LabelName.text = dataSplit[1];
            }

            if (GameObject.Find("InfoWindow") != null)
            {
                InfoWindow _InfoWindow = GameObject.Find("InfoWindow").GetComponent<InfoWindow>();
                _InfoWindow.labelName.text = dataSplit[1];
            }
            CharacterRecorder.instance.AddLunaGem(-100);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("名称格式不对", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("名称重复", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("非法字符", PromptWindow.PromptType.Hint, null, null);
                    break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_1042(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {

            if (GameObject.Find("InfoWindow") != null)
            {
                InfoWindow _InfoWindow = GameObject.Find("InfoWindow").GetComponent<InfoWindow>();
                _InfoWindow.InfoSign.text = dataSplit[1];
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }

    }

    public void Process_1017(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] secSplit = dataSplit[0].Split('$');
        CharacterRecorder.instance.IsOpen = true;
        RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
        HeroLevelUpPart hlup = rw.HeroLevelUpPart.GetComponent<HeroLevelUpPart>();
        hlup.AddFlyLabel(int.Parse(secSplit[1]), int.Parse(secSplit[2]), int.Parse(secSplit[3]));
    }

    public void Process_1018(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        CharacterRecorder.instance.IsOpen = true;
        CharacterRecorder.instance.FightOld = CharacterRecorder.instance.Fight;
        CharacterRecorder.instance.Fight = int.Parse(dataSplit[0]);

        //if (CharacterRecorder.instance.IsNeedOpenFight && GameObject.Find("AdvanceWindow") == null && CharacterRecorder.instance.IsOpenFight && CharacterRecorder.instance.FightOld < CharacterRecorder.instance.Fight)
        //Debug.LogError(CharacterRecorder.instance.IsNeedOpenFight + " " + CharacterRecorder.instance.IsOpenFight + " " + CharacterRecorder.instance.FightOld + " " + CharacterRecorder.instance.Fight);
        if (CharacterRecorder.instance.IsNeedOpenFight && CharacterRecorder.instance.IsOpenFight && CharacterRecorder.instance.FightOld < CharacterRecorder.instance.Fight)
        {
            StartCoroutine(OpenForceChangesWindowLater(int.Parse(dataSplit[0])));
        }
    }

    IEnumerator OpenForceChangesWindowLater(int FightMax)
    {
        CharacterRecorder.instance.IsOpenFight = false;
        while (GameObject.Find("MainWindow") == null && GameObject.Find("RoleWindow") == null && GameObject.Find("MapUiWindow") == null && GameObject.Find("TechWindow") == null && GameObject.Find("StrengEquipWindow") == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        UIManager.instance.CreateForceChange(CharacterRecorder.instance.FightOld, CharacterRecorder.instance.Fight);
        yield return new WaitForSeconds(0.1f);
    }

    public void Process_1019(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (Application.loadedLevelName != "Downloader")
        {
            CharacterRecorder.instance.stamina = int.Parse(dataSplit[0]);
            CharacterRecorder.instance.staminaCap = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.sprite = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.spriteCap = int.Parse(dataSplit[3]);
        }
    }
    public void Process_1020(string RecvString)//战队信息
    {
        string[] dataSplit = RecvString.Split(';');

        int uId = int.Parse(dataSplit[0]);
        int headIcon = int.Parse(dataSplit[1]);
        string name = dataSplit[2];
        int level = int.Parse(dataSplit[3]);
        int vip = int.Parse(dataSplit[4]);
        int fight = int.Parse(dataSplit[5]);
        int pvpRank = int.Parse(dataSplit[6]);
        string legionName = dataSplit[7];
        int legionPosition = int.Parse(dataSplit[8]);
        int contribute = int.Parse(dataSplit[9]);
        string[] dataSplit2 = dataSplit[10].Split('!');
        BetterList<RoleInfoOfTargetPlayer> roleList = new BetterList<RoleInfoOfTargetPlayer>();
        roleList.Clear();
        for (int i = 0; i < dataSplit2.Length - 1; i++)
        {
            string[] secSplit = dataSplit2[i].Split('$');
            int roleId = int.Parse(secSplit[0]);
            int roleLevel = int.Parse(secSplit[1]);
            int roleJunXian = int.Parse(secSplit[2]); ;
            int color = int.Parse(secSplit[3]);
            int character = int.Parse(secSplit[4]);
            roleList.Add(new RoleInfoOfTargetPlayer(roleId, roleLevel, roleJunXian, color, character));
        }
        TextTranslator.instance.targetPlayerInfo = new TargetPlayerInfo(uId, headIcon, name, level, vip, fight, pvpRank, legionName, legionPosition, contribute, roleList);
        if (GameObject.Find("LegionMemberItemDetail") != null)
        {
            LegionMemberItemDetail _LegionMemberItemDetail = GameObject.Find("LegionMemberItemDetail").GetComponent<LegionMemberItemDetail>();
            _LegionMemberItemDetail.SetRoleInfoOfPlayer();
        }
        if (GameObject.Find("SmuggleHeroInfo") != null)
        {
            GameObject.Find("SmuggleHeroInfo").GetComponent<SmuggleHeroInfo>().HeroInfo(dataSplit[10]);
        }
    }
    public void Process_1021(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.IsOpen = true;
            if (GameObject.Find("BagWindow") != null)
            {
                //Debug.LogError("合成成功！");
                BagWindow bw = GameObject.Find("BagWindow").GetComponent<BagWindow>();
                bw.UpDataBag();
                AudioEditer.instance.PlayOneShot("ui_levelup");
                BagWindow rw = GameObject.Find("BagWindow").GetComponent<BagWindow>();
                rw.SetCardInfo(int.Parse(dataSplit[1]));
                //rw.NewHeroBoard.SetActive(true);
                //rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Icon").GetComponent<UISprite>().spriteName = dataSplit[1];
                //UIEventListener.Get(rw.NewHeroBoard.transform.FindChild("ResultMask").gameObject).onClick = delegate(GameObject go)
                //{
                //    rw.NewHeroBoard.SetActive(false);
                //};

                //HeroInfo heroinfo = TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(dataSplit[1]));
                ////CareerInfo _careerInfo = TextTranslator.instance.GetHeroCareerByHeroToSort(heroinfo.heroToSort);
                //rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Name").GetComponent<UILabel>().text = heroinfo.heroName;
                //rw.NewHeroBoard.transform.FindChild("HeroType").GetComponent<UILabel>().text = heroinfo.heroCarrer;
                //rw.NewHeroBoard.transform.FindChild("HeroName").GetComponent<UILabel>().text = heroinfo.heroDescription;

                //switch (heroinfo.heroAi)
                //{
                //    case 1:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("AI").GetComponent<UISprite>().spriteName = "ui1_gaiicon5";
                //        break;
                //    case 2:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("AI").GetComponent<UISprite>().spriteName = "ui1_gaiicon2";
                //        break;
                //    case 3:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("AI").GetComponent<UISprite>().spriteName = "ui1_gaiicon4";
                //        break;
                //    case 4:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("AI").GetComponent<UISprite>().spriteName = "ui1_gaiicon3";
                //        break;
                //    case 5:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("AI").GetComponent<UISprite>().spriteName = "ui1_gaiicon1";
                //        break;
                //    default:
                //        break;
                //}
                ////Hero mhero=new Hero(int.Parse(dataSplit[0]),int.Parse(dataSplit[0]));
                ////CharacterRecorder.instance.AddOwnedHeroList(mhero);

                //switch (heroinfo.heroClass)
                //{
                //    case 1:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").GetComponent<UISprite>().spriteName = "ui27_di3";
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star1").gameObject.SetActive(true);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star2").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star3").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star4").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star5").gameObject.SetActive(false);
                //        break;
                //    case 2:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").GetComponent<UISprite>().spriteName = "ui27_di3";
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star1").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star2").gameObject.SetActive(true);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star3").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star4").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star5").gameObject.SetActive(false);
                //        break;
                //    case 3:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").GetComponent<UISprite>().spriteName = "ui27_di3";
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star1").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star2").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star3").gameObject.SetActive(true);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star4").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star5").gameObject.SetActive(false);
                //        break;
                //    case 4:
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").GetComponent<UISprite>().spriteName = "ui27_di3";
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star1").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star2").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star3").gameObject.SetActive(false);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star4").gameObject.SetActive(true);
                //        rw.NewHeroBoard.transform.FindChild("HeroBoard").FindChild("Scroll View").FindChild("Star5").gameObject.SetActive(false);
                //        break;
                //    default:
                //        break;
                //}


                ////////////////////////
                if (dataSplit[0].Substring(0, 1) == "6")
                {
                    //SendProcess("1004#" + dataSplit[1] + ";");
                    //SendProcess("1005#" + dataSplit[1] + ";");
                    //SendProcess("3001#" + dataSplit[1] + ";");
                }
                if (int.Parse(dataSplit[1]) > 60000 && int.Parse(dataSplit[1]) < 70000 && TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(dataSplit[1])).heroRarity >= 4)
                {
                    Debug.LogError("-------------");
                    string heroName = TextTranslator.instance.GetItemNameByItemCode(int.Parse(dataSplit[1]));
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 6, CharacterRecorder.instance.characterName, heroName, TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(dataSplit[1])).heroRarity));
                }
            }
            else if (GameObject.Find("HeroMapWindow") != null)
            {
                HeroMapWindow hmw = GameObject.Find("HeroMapWindow").GetComponent<HeroMapWindow>();
                hmw.SetCardInfo(int.Parse(dataSplit[1]));
                ////////////////////////
                if (dataSplit[0].Substring(0, 1) == "6")
                {
                    //SendProcess("1004#" + dataSplit[1] + ";");
                    //SendProcess("1005#" + dataSplit[1] + ";");
                    //SendProcess("3001#" + dataSplit[1] + ";");
                }

                if (int.Parse(dataSplit[1]) > 60000 && int.Parse(dataSplit[1]) < 70000 && TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(dataSplit[1])).heroRarity >= 4)
                {
                    Debug.LogError("-------------");
                    string heroName = TextTranslator.instance.GetItemNameByItemCode(int.Parse(dataSplit[1]));
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 6, CharacterRecorder.instance.characterName, heroName, TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(dataSplit[1])).heroRarity));
                }
            }
        }
        else
        {
            if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("没有对应角色码", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("已有此角色", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("碎片不足", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "4")
            {
                UIManager.instance.OpenPromptWindow("星币不足", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_1022(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            TextTranslator.instance.isUpdateBag = true;
            HeroNewData heroNewData = new HeroNewData();
            foreach (Hero _hero in CharacterRecorder.instance.ownedHeroList)
            {
                if (_hero.characterRoleID == int.Parse(dataSplit[2]))
                {
                    heroNewData.classNumber = _hero.classNumber;
                    // heroNewData.force = _hero.force;
                }
            }
            CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);//现有金币
            TopContent topC = GameObject.Find("TopContent").GetComponent<TopContent>();
            topC.Reset();
            heroNewData.rank = int.Parse(dataSplit[4]);//改变数据
            heroNewData.force = int.Parse(dataSplit[5]);
            heroNewData.HP = int.Parse(dataSplit[6]);
            heroNewData.strength = int.Parse(dataSplit[7]);
            heroNewData.physicalDefense = int.Parse(dataSplit[8]);
            GameObject.Find("HeroBreakUpPart").GetComponent<HeroBreakUpPart>().ScuccedJinJieJunXian(heroNewData);
            SendProcess("1005#" + dataSplit[2] + ";");
            SendProcess("1621#" + dataSplit[2] + ";");

            if (int.Parse(dataSplit[4]) > 4) //5星及以上通告
            {
                string heroName = TextTranslator.instance.GetItemNameByItemCode(int.Parse(dataSplit[1]));
                SendProcess(string.Format("7002#{0};{1};{2};{3}", 28, CharacterRecorder.instance.characterName, heroName, dataSplit[4]));
            }
            //SendProcess("1005#0;");
            //SendProcess("7002#28;"+CharacterRecorder.instance.characterName+";"+TextTranslator.instance)
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("达到上限", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("[ff0000]战功不足无法晋升", PromptWindow.PromptType.Hint, null, null);//突破石不足
                if (TextTranslator.instance.isUpdateBag)
                {
                    NetworkHandler.instance.SendProcess("5001#;");
                }
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("碎片不足", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_1024(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
            HeroBreakUpPart hbup = rw.HeroBreakUpPart.GetComponent<HeroBreakUpPart>();
            hbup.SetNewState(int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), int.Parse(dataSplit[5]));
        }
        else
        {

        }
    }

    public void Process_1025(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject roleWindow = GameObject.Find("RoleWindow");
            if (roleWindow != null)
            {
                RoleWindow rw = roleWindow.GetComponent<RoleWindow>();
                HeroTrainPart htp = rw.HeroTrainPart.GetComponent<HeroTrainPart>();
                htp.SetTrainState(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), float.Parse(dataSplit[4]), float.Parse(dataSplit[5]));
                htp.UpdateTrainAdd(int.Parse(dataSplit[6]), int.Parse(dataSplit[7]), int.Parse(dataSplit[8]), float.Parse(dataSplit[9]), float.Parse(dataSplit[10]));
            }
            GameObject rebirthWindow = GameObject.Find("RebirthWindow");
            if (rebirthWindow != null)
            {
                if (dataSplit[11] != null)
                {
                    rebirthWindow.GetComponent<RebirthWindow>().TrainRebirth(int.Parse(dataSplit[11]));
                }
            }
            GameObject litterHelper = GameObject.Find("LittleHelperWindow");
            if (litterHelper != null)
            {
                LittleHelperWindow htp = litterHelper.GetComponent<LittleHelperWindow>();
                htp.SetTrainState(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), float.Parse(dataSplit[4]), float.Parse(dataSplit[5]));
            }
        }
        else
        {

        }

    }

    public void Process_1026(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //SendProcess("1003#");
            CharacterRecorder.instance.IsOpen = true;
            CharacterRecorder.instance.gold = int.Parse(dataSplit[6]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[7]);
            TopContent topC = GameObject.Find("TopContent").GetComponent<TopContent>();
            topC.Reset();
            RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
            HeroTrainPart htp = rw.HeroTrainPart.GetComponent<HeroTrainPart>();
            htp.UpdateTrainAdd(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), float.Parse(dataSplit[4]), float.Parse(dataSplit[5]));
        }
        else
        {
            if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("强化药剂不足无法培养", PromptWindow.PromptType.Hint, null, null);//丹药不足
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("金钱不足", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_1027(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
            HeroTrainPart htp = rw.HeroTrainPart.GetComponent<HeroTrainPart>();
            htp.isClean = true;
            htp.SetTrainState(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), float.Parse(dataSplit[4]), float.Parse(dataSplit[5]));
        }
        else
        {
            //UIManager.instance.OpenPromptWindow(string.Format("服务器错误码{0}", dataSplit[1]), PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_1029(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        GameObject about = GameObject.Find("AboutHeroInfoWindow");
        if (about != null)
        {
            about.GetComponent<AboutHeroInfoWindow>().SetHeroInfo_1029(dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], dataSplit[4], dataSplit[5], dataSplit[6]);
        }


    }
    public void Process_1031(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] secSplit = dataSplit[0].Split('$');
        CharacterRecorder.instance.ListManualSkillId.Clear();
        //List<string> myOwnTacticList = new List<string>();
        for (int i = 0; i < secSplit.Length; i++)
        {
            //myOwnTacticList.Add(secSplit[i]);
            CharacterRecorder.instance.ListManualSkillId.Add(int.Parse(secSplit[i]));
        }
        /*TacticsWindow  _TacticsWindow = GameObject.Find("TacticsWindow").GetComponent<TacticsWindow>();
        _TacticsWindow.SetMyOwnTactics(myOwnTacticList);*/
    }
    public void Process_1032(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //Debug.LogError(dataSplit[1]);
            string[] secSplit = dataSplit[1].Split('$');
            //List<string> myOwnTacticList = new List<string>();
            CharacterRecorder.instance.ListManualSkillId.Clear();
            for (int i = 0; i < secSplit.Length; i++)
            {
                //myOwnTacticList.Add(secSplit[i]);
                CharacterRecorder.instance.ListManualSkillId.Add(int.Parse(secSplit[i]));
            }
            TacticsWindow _TacticsWindow = GameObject.Find("TacticsWindow").GetComponent<TacticsWindow>();
            _TacticsWindow.SetMyOwnTactics(CharacterRecorder.instance.ListManualSkillId);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("战术信息错误", PromptWindow.PromptType.Hint, null, null); break;
                default: UIManager.instance.OpenPromptWindow("战术错误", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }

    /// <summary>
    /// 领取体力以下....
    /// </summary>
    /// <param name="RecvString"></param>
    /// 
    public void Process_1091(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("GetPhysicalPowerWindow") != null)
        {
            GameObject.Find("GetPhysicalPowerWindow").GetComponent<GetPhysicalPowerWindow>().SetInfo(dataSplit);
        }
        if (dataSplit[1] == "1" || dataSplit[2] == "1" || dataSplit[3] == "1" || dataSplit[1] == "2" || dataSplit[2] == "2" || dataSplit[3] == "2")
        {
            CharacterRecorder.instance.isPowerRedPoint = true;
            if (GameObject.Find("GetPhysicalPowerWindow") != null)
            {
                GameObject.Find("ActivityLeftButton1").transform.Find("RedMessage").gameObject.SetActive(true);
                //GameObject.Find("EventWindow").transform.Find("left/Scroll View/EventButtonGrid/PowerButton/RedMessage").gameObject.SetActive(true);
            }
        }
        else
        {
            CharacterRecorder.instance.isPowerRedPoint = false;
            if (GameObject.Find("GetPhysicalPowerWindow") != null)
            {
                GameObject.Find("ActivityLeftButton1").transform.Find("RedMessage").gameObject.SetActive(false);
                //GameObject.Find("EventWindow").transform.Find("left/Scroll View/EventButtonGrid/PowerButton/RedMessage").gameObject.SetActive(false);
            }
        }
        if (GameObject.Find("MainWindow") != null)
        {
            if (CharacterRecorder.instance.isPowerRedPoint)
            {
                CharacterRecorder.instance.SetRedPoint(19, true);
                //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(8, true);
            }
            else
            {
                CharacterRecorder.instance.SetRedPoint(19, false);
                //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(8, false);
            }
        }
    }
    public void Process_1092(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.stamina = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            List<Item> itemlist = new List<Item>();
            itemlist.Add(new Item(90007, 60));
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            SendProcess("1091#");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败", PromptWindow.PromptType.Hint, null, null);
        }

    }
    public void Process_1093(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            SendProcess("1091#");
        }
    }
    /// <summary>
    /// 领取体力以上...........
    /// </summary>
    /// <param name="RecvString"></param>
    public void Process_1101(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.IsOpen = true;
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.stamina = int.Parse(dataSplit[2]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            if (GameObject.Find("MainWindow") != null)
            {
                //GameObject.Find("MainWindow").GetComponent<MainWindow>().MainTaskReadPoint();
                CharacterRecorder.instance.MainTaskReadPoint();
                UIManager.instance.OpenPromptWindow("购买成功", PromptWindow.PromptType.Hint, null, null);
            }
            else if (GameObject.Find("TaskWindow") != null)
            {
                NetworkHandler.instance.SendProcess("1201#1");
                NetworkHandler.instance.SendProcess("1201#2");
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("无法购买", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("达到上限", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
        SendProcess("1102#");
    }
    public void Process_1102(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("BuyPropsWindow") != null)
        {
            //GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().GetCostDiamond(int.Parse(dataSplit[0]),int.Parse(dataSplit[1]),10602);
        }
        else
        {
            UIManager.instance.OpenPanel("BuyPropsWindow", false);
            //GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().GetCostDiamond(int.Parse(dataSplit[0]),int.Parse(dataSplit[1]), 10602);
        }

    }
    public void Process_1103(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.IsOpen = true;
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[2]);
            if (GameObject.Find("GlodPointingWindow") == null)
            {
                int Money = int.Parse(dataSplit[2]) - CharacterRecorder.instance.BuyGold;
                int CritNum = int.Parse(dataSplit[3]);
                int PointNum = int.Parse(dataSplit[4]);
                UIManager.instance.OpenPanel("GlodPointingWindow", false);
                GameObject.Find("GlodPointingWindow").GetComponent<GlodPointingWindow>().SetPointInfo(Money, CritNum, PointNum);
            }
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            if (GameObject.Find("MainWindow") != null)
            {
                //GameObject.Find("MainWindow").GetComponent<MainWindow>().MainTaskReadPoint();
                CharacterRecorder.instance.MainTaskReadPoint();
            }
            else if (GameObject.Find("TaskWindow") != null)
            {
                NetworkHandler.instance.SendProcess("1201#1");
                NetworkHandler.instance.SendProcess("1201#2");
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("无法购买", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("达到上限", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
        //if (int.Parse(dataSplit[3]) > 1)
        //{
        //    UIManager.instance.OpenPromptWindow(string.Format("暴击率{0}", dataSplit[3]), PromptWindow.PromptType.Hint, null, null);
        //}
        SendProcess("1104#");
    }
    public void Process_1104(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject.Find("BuyGlodWindow").GetComponent<BuyGlodWindow>().GetCostDiamond(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
    }
    //public void Process_1105(string RecvString)
    //{
    //    string[] dataSplit = RecvString.Split(';');
    //    if (dataSplit[0] == "1")
    //    {
    //        CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
    //        CharacterRecorder.instance.sprite = int.Parse(dataSplit[2]);
    //        if (GameObject.Find("TopContent") != null)
    //        {
    //            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
    //        }
    //        if (GameObject.Find("MainWindow") != null)
    //        {
    //            GameObject.Find("MainWindow").GetComponent<MainWindow>().MainTaskReadPoint();
    //            UIManager.instance.OpenPromptWindow("购买成功", PromptWindow.PromptType.Hint, null, null);
    //        }
    //        else if (GameObject.Find("TaskWindow") != null)
    //        {
    //            NetworkHandler.instance.SendProcess("1201#1");
    //            NetworkHandler.instance.SendProcess("1201#2");
    //        }
    //    }
    //    else
    //    {
    //        switch (dataSplit[1])
    //        {
    //            case "0":
    //                UIManager.instance.OpenPromptWindow("无法购买", PromptWindow.PromptType.Hint, null, null);
    //                break;
    //            case "1":
    //                UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
    //                break;
    //            case "2":
    //                UIManager.instance.OpenPromptWindow("达到上限", PromptWindow.PromptType.Hint, null, null);
    //                break;
    //        }
    //    }
    //    SendProcess("1106#");
    //}
    //public void Process_1106(string RecvString)
    //{
    //    string[] dataSplit = RecvString.Split(';');
    //    if (GameObject.Find("BuyPropsWindow") != null)
    //    {
    //        //GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().GetCostDiamond(int.Parse(dataSplit[0]),int.Parse(dataSplit[1]), 10702);
    //    }
    //    else
    //    {
    //        UIManager.instance.OpenPanel("BuyPropsWindow", false);
    //        //GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().GetCostDiamond(int.Parse(dataSplit[0]),int.Parse(dataSplit[1]), 10702);
    //    }
    //}
    public void Process_1107(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("InfoWindow") != null)
        {
            InfoWindow mw = GameObject.Find("InfoWindow").GetComponent<InfoWindow>();
            mw.SetInfoWindowTime(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        }
        else
        {
            Debug.Log("未找到InfoWindow");
            if (GameObject.Find("MainWindow") != null)
            {
                MainWindow mw = GameObject.Find("MainWindow").GetComponent<MainWindow>();
                mw.SetTopContent(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
            }
        }

    }
    public void Process_1141(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //FightListWindow rw = GameObject.Find("FightListWindow").GetComponent<FightListWindow>();
            //rw.PraiseResult(dataSplit[2]);
            //NetworkHandler.instance.SendProcess(String.Format("6004#{0};", dataSplit[3]));
            TopContent topC = GameObject.Find("TopContent").GetComponent<TopContent>();
            CharacterRecorder.instance.lunaGem += 20;
            topC.Reset();
            UIManager.instance.OpenPromptWindow("点赞获得20钻石", PromptWindow.PromptType.Hint, null, null);
            if (GameObject.Find("PVPWindow") != null)
            {
                List<GameObject> pvpItemList = GameObject.Find("PVPWindow").GetComponent<PVPWindow>().pvpItemList;
                for (int i = 0; i < pvpItemList.Count; i++)
                {
                    if (dataSplit[2] == pvpItemList[i].name)
                    {
                        pvpItemList[i].transform.Find("FightButtonHui").GetComponent<UISprite>().spriteName = "buttonHui";
                        pvpItemList[i].transform.Find("FightButtonHui").GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }
        else if (dataSplit[0] == "0")
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("三次点完", PromptWindow.PromptType.Hint, null, null);
                    //Debug.LogError("三次点完");
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("重复点赞", PromptWindow.PromptType.Hint, null, null);
                    //Debug.LogError("重复点赞");
                    break;
            }

        }
    }
    public void Process_1201(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")//每日任务
        {
            TextTranslator.instance.SetAchievementInfo(1, RecvString);
            if (GameObject.Find("TaskWindow") != null)
            {
                TaskWindow Ta = GameObject.Find("TaskWindow").GetComponent<TaskWindow>();
                if (Ta.curIndex == 1)
                {
                    //Ta.SetPart(1);
                    Ta.SetPartNew(1, RecvString);
                }
            }
        }
        else if (dataSplit[0] == "2")//成就
        {
            TextTranslator.instance.SetAchievementInfo(2, RecvString);
            if (GameObject.Find("TaskWindow") != null)
            {
                TaskWindow Ta = GameObject.Find("TaskWindow").GetComponent<TaskWindow>();
                if (Ta.curIndex == 2)
                {
                    //Ta.SetPart(2);
                    Ta.SetPartNew(2, RecvString);
                }
            }
            //if (GameObject.Find("MainWindow") != null)
            //{
            //    GameObject.Find("MainWindow").GetComponent<MainWindow>().GetTaskRedPoint();
            //}
            CharacterRecorder.instance.GetTaskRedPoint();
        }
    }

    public void Process_1202(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[7].Split('!');
            int stamina = 0;
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
                if (int.Parse(ticSplit[0]) == 90007)
                {
                    stamina += int.Parse(ticSplit[1]);
                }
            }
            if (GameObject.Find("LevelUpWindow") == null)
            {
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            }
            TextTranslator.instance.isUpdateBag = true;
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            CharacterRecorder.instance.exp = int.Parse(dataSplit[3]);
            CharacterRecorder.instance.expMax = int.Parse(dataSplit[4]);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[5]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[6]);
            CharacterRecorder.instance.stamina += stamina;

            //if (dataSplit[2] == "18" || dataSplit[2] == "19")
            //{
            //    string[] picSplit = secSplit[0].Split('$');
            //    CharacterRecorder.instance.stamina += int.Parse(picSplit[1]);
            //}
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("成就ID错误", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("不可领取", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_1203(string RecvString)
    {

    }
    public void Process_1204(string RecvString)
    {
        //string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.HappyBoxInfo = RecvString;
        if (GameObject.Find("HappyBoxItem") != null) //GameObject.Find("TaskWindow").transform.Find("HappyBoxItem").gameObject.activeSelf
        {
            GameObject.Find("HappyBoxItem").GetComponent<HappyBoxItem>().SetHappyBoxInfo();
        }
    }
    public void Process_1621(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        // TextTranslator.instance.HeroInnateDic.Clear();

        for (int i = 0; i < dataSplit.Length - 1; i = i + 2)
        {
            string[] ITe = new string[20];
            string[] her = dataSplit[i + 1].Split('!');


            for (int j = 0; j < her.Length - 1; j++)
            {
                ITe[j] = her[j].Split('$')[1];
               
            }
           
            
            TextTranslator.instance.AddHeroDicv(int.Parse(dataSplit[i]), ITe);

        }
       // Debug.LogError(int.Parse(dataSplit[0])+"  "+int.Parse(dataSplit[2])+" "+ (dataSplit[1]));
        TextTranslator.instance.AddHeroNum(int.Parse(dataSplit[0]), int.Parse(dataSplit[2]));
        if (GameObject.Find("RoleWindow") != null)
        {
            GameObject.Find("RoleWindow").GetComponent<RoleWindow>().UpDateDownHeroIcon();
        }
    }
    public void Process_1622(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            string[] ITe = new string[20];
            string[] her = dataSplit[2].Split('!');
            for (int j = 0; j < her.Length - 1; j++)
            {
                ITe[j] = her[j].Split('$')[1];

            }

            TextTranslator.instance.AddHeroDicv(int.Parse(dataSplit[1]), ITe);
            TextTranslator.instance.AddHeroNum(int.Parse(dataSplit[1]), int.Parse(dataSplit[3]));
            if (GameObject.Find("TypeWindow") != null)
            {
                GameObject.Find("TypeWindow").GetComponent<TypeWindow>().AddDower();
            }
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            if (GameObject.Find("RoleWindow") != null)
            {
                GameObject.Find("RoleWindow").GetComponent<RoleWindow>().UpDateDownHeroIcon();
            }
            //TextTranslator.instance.AddHeroDic(int.Parse(dataSplit[1]), new HeroDower(int.Parse(her[0].Split('$')[1]), int.Parse(her[1].Split('$')[1]), int.Parse(her[2].Split('$')[1]),
            //    int.Parse(her[3].Split('$')[1]), int.Parse(her[4].Split('$')[1]), int.Parse(her[5].Split('$')[1]), int.Parse(her[6].Split('$')[1]),
            //    int.Parse(her[7].Split('$')[1]), int.Parse(her[8].Split('$')[1]), int.Parse(her[9].Split('$')[1]), int.Parse(her[10].Split('$')[1]),
            //    int.Parse(her[11].Split('$')[1]), int.Parse(her[12].Split('$')[1]), int.Parse(her[13].Split('$')[1]), int.Parse(her[14].Split('$')[1]),
            //    int.Parse(her[15].Split('$')[1]), int.Parse(her[16].Split('$')[1]), int.Parse(her[0].Split('$')[17])));
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1":
                    UIManager.instance.OpenPromptWindow("发送数据失败", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("金币不足！", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "4":
                    UIManager.instance.OpenPromptWindow("天赋点不足！", PromptWindow.PromptType.Hint, null, null);
                    break;
                default:
                    break;
            }
        }

    }
    public void Process_1623(string RecvString)//
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            string[] her = ("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0").Split(',');
            TextTranslator.instance.AddHeroNum(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            TextTranslator.instance.AddHeroDicv(int.Parse(dataSplit[1]), her);
            if (GameObject.Find("HeroDowerPart") != null)
            {
                HeroDowerPart.instance.SetItemList(HeroDowerPart.instance.hero);
            }
            if (GameObject.Find("RoleWindow") != null)
            {
                GameObject.Find("RoleWindow").GetComponent<RoleWindow>().UpDateDownHeroIcon();
            }
            //TextTranslator.instance.AddHeroDic(int.Parse(dataSplit[1]),new HeroDower(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0));int.Parse(dataSplit[1]
        }
    }

    public void Process_1624(string RecvString)//
    {
        string[] dataSplit = RecvString.Split(';');
        print(dataSplit[0]);
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
            TextTranslator.instance.AddHeroNum(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            GameObject go = GameObject.Find("ExchangeWindow");
            print(go.name);
            if (go!=null)
            {
                go.GetComponent<ExchangeWindow>().SetWindow();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1":
                    UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("金币或或碎片不足！", PromptWindow.PromptType.Hint, null, null);
                    break;
                default:
                    break;
            }
        }
    }
    public void Process_1205(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> ItemList = new List<Item>();
            string[] RewardSplit = dataSplit[2].Split('!');
            for (int i = 0; i < RewardSplit.Length - 1; i++)
            {
                string[] trcSplit = RewardSplit[i].Split('$');
                ItemList.Add(new Item(int.Parse(trcSplit[0]), int.Parse(trcSplit[1])));
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, ItemList);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[4]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            SendProcess("1204#;");
            SendProcess("1201#1;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_2001(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (GameObject.Find("GateWindow") != null)
        {
            GateWindow gw = GameObject.Find("GateWindow").GetComponent<GateWindow>();
            gw.SetChapterStar(dataSplit[0], dataSplit[1], dataSplit[2]);
            gw.SetGateButton();
        }
        if (GameObject.Find("WayFubenItem1") != null)
        {
            WayFubenItem wfi = GameObject.Find("WayFubenItem1").GetComponent<WayFubenItem>();
            wfi.SetLock(RecvString);
        }
        if (GameObject.Find("WayFubenItem2") != null)
        {
            WayFubenItem wfi = GameObject.Find("WayFubenItem1").GetComponent<WayFubenItem>();
            wfi.SetLock(RecvString);
        }
        if (GameObject.Find("WayFubenItem3") != null)
        {
            WayFubenItem wfi = GameObject.Find("WayFubenItem1").GetComponent<WayFubenItem>();
            wfi.SetLock(RecvString);
        }
        if (GameObject.Find("MapObject") != null)
        {
            CharacterRecorder.instance.lastGateID = int.Parse(dataSplit[0]);
            CharacterRecorder.instance.lastCreamGateID = int.Parse(dataSplit[3]);
            StartCoroutine(SceneTransformer.instance.NewbieGuide());
            //if (CharacterRecorder.instance.lastGateID == 10011 && CharacterRecorder.instance.GuideID[27] < 4)
            //{
            //    CharacterRecorder.instance.cameraGateID = 1;

            //}
            string[] SecString = dataSplit[1].Split('$');

            GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().MapSatrList.Clear();
            GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().CreamSatrList.Clear();
            for (int i = 0; i < SecString.Length - 1; i++)
            {

                GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().MapSatrList.Add(int.Parse(SecString[i]));
            }
            SecString = dataSplit[2].Split('$');
            if (SceneTransformer.instance.NowGateID == 20001 && CharacterRecorder.instance.GuideID[33] < 3)
            {
                StartCoroutine(SceneTransformer.instance.NewbieGuide());
            }
            for (int i = 0; i < SecString.Length - 1; i++)
            {
                GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().CreamSatrList.Add(int.Parse(SecString[i]));
            }
            GameObject mapWindow = GameObject.Find("MapObject");

            if (CharacterRecorder.instance.IsOpenMapGate)
            {
                List<int> star = mapWindow.transform.Find("MapCon").GetComponent<MapWindow>().MapSatrList;
                GameObject go = GameObject.Find("MapUiWindow");
                if (go != null)
                {
                    go.GetComponent<MapUiWindow>().InitWindow_Back(-1);
                }
            }
            GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().SetMapName();
            GameObject.Find("MapObject").transform.FindChild("MapCon").GetComponent<MapWindow>().SetMapTypeUpdate();
        }
        else
        {
            string[] SecString = dataSplit[1].Split('$');
            for (int i = 0; i < SecString.Length - 1; i++)
            {
                SceneTransformer.instance.MainScene.transform.Find("MapCon").GetComponent<MapWindow>().MapSatrList.Add(int.Parse(SecString[i]));
            }
        }
    }

    public void Process_2002(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "0")
        {
        }
        else
        {
            CharacterRecorder.instance.IsOpen = true;
            AudioEditer.instance.PlayOneShot("Win");
            UIManager.instance.OpenPanel("ResultWindow", false);
            ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
            rw.Init(true, int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), dataSplit[5], int.Parse(dataSplit[6]), dataSplit[7], int.Parse(dataSplit[8]));
            SendProcess("5001#;");
        }
    }

    public void Process_2004(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[1] == "0")
        {
        }
        else
        {
            if (GameObject.Find("FightWindow") != null)
            {
                GameObject.Find("FightWindow").GetComponent<FightWindow>().BeginFight();
            }
        }
    }

    public void Process_2009(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
    }

    public void Process_2010(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            GameObject go = GameObject.Find("GateInfoWindow");
            if (go != null)
            {
                go.GetComponent<MapGateInfoWindow>().ChallengeCount = int.Parse(dataSplit[3]);
                go.GetComponent<MapGateInfoWindow>().SetSweepBtn();
                go.GetComponent<MapGateInfoWindow>().SetChallengNum();
            }
            UIManager.instance.OpenPromptWindow("重置成功", PromptWindow.PromptType.Hint, null, null);
            SendProcess("2017#" + dataSplit[1] + ";");
        }
    }

    public void Process_2011(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] secSplit = dataSplit[2].Split(':');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.stamina = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }
            UIManager.instance.OpenPanel("SweptWindow", false);
            if (GameObject.Find("SweptWindow") != null)
            {
                SweptWindow sw = GameObject.Find("SweptWindow").GetComponent<SweptWindow>();
                //sw.SetOneRush(int.Parse(secSplit[1]), int.Parse(secSplit[0]), secSplit[2]);
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("关卡不符合扫荡", PromptWindow.PromptType.Alert, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("体力不足", PromptWindow.PromptType.Alert, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("VIP等级不足", PromptWindow.PromptType.Alert, null, null);
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("无扫荡次数", PromptWindow.PromptType.Alert, null, null);
            }
        }
    }

    public void Process_2012(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.stamina = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.exp = int.Parse(dataSplit[3]);
            if (GameObject.Find("SweptWindow") == null)
            {
                //UIManager.instance.OpenPanel("SweptWindow", false);
                UIManager.instance.OpenSinglePanel("SweptWindow", false);
            }
            CharacterRecorder.instance.stamina = int.Parse(dataSplit[1]);
            if (GameObject.Find("SweptWindow") != null)
            {
                SweptWindow sw = GameObject.Find("SweptWindow").GetComponent<SweptWindow>();
                sw.SetTenRush(RecvString, int.Parse(dataSplit[4]), int.Parse(dataSplit[5]));
            }
            if (GameObject.Find("GateInfoWindow") != null)
            {
                MapGateInfoWindow gt = GameObject.Find("GateInfoWindow").GetComponent<MapGateInfoWindow>();
                if (gt.GateID > 20000)
                {
                    gt.SweepNum = int.Parse(dataSplit[5]);
                    gt.ChallengeCount = gt.ChallengeCount - gt.SweepNum;
                    if (gt.ChallengeCount < 0)
                    {
                        gt.ChallengeCount = 0;
                    }
                    string _colorCode = "";
                    if (gt.ChallengeCount > 0)
                    {
                        _colorCode = "[00FF00]";
                    }
                    else
                    {
                        _colorCode = "[FF0000]";
                    }
                    gt.LabelCount.GetComponent<UILabel>().text = "挑战次数：" + _colorCode + gt.ChallengeCount + "[-]/3";
                    if (gt.ChallengeCount <= 0)
                    {
                        gt.SetSweepBtn();
                    }
                }
            }
            TextTranslator.instance.isUpdateBag = true;
            GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().mTopContent.GetComponent<TopContent>().Reset();
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("关卡不符合扫荡", PromptWindow.PromptType.Alert, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                if (GameObject.Find("SweptWindow") != null)
                {
                    SweptWindow sw = GameObject.Find("SweptWindow").GetComponent<SweptWindow>();
                    sw.GetMessage(1);
                }
                else
                {
                    UIManager.instance.OpenPromptWindow("体力不足", PromptWindow.PromptType.Alert, null, null);
                }
                //UIManager.instance.OpenPromptWindow("体力不足", PromptWindow.PromptType.Alert, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("VIP等级不足", PromptWindow.PromptType.Alert, null, null);
            }
            else if (dataSplit[1] == "3")
            {
                if (GameObject.Find("SweptWindow") != null)
                {
                    SweptWindow sw = GameObject.Find("SweptWindow").GetComponent<SweptWindow>();
                    sw.GetMessage(3);
                }
                else
                {
                    UIManager.instance.OpenPromptWindow("无扫荡次数", PromptWindow.PromptType.Alert, null, null);
                }
                //UIManager.instance.OpenPromptWindow("无扫荡次数", PromptWindow.PromptType.Alert, null, null);
            }
        }
    }


    public void Process_2013(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("GateWindow") != null)
        {
            GateWindow gw = GameObject.Find("GateWindow").GetComponent<GateWindow>();
            gw.SetChapterOpen(dataSplit[0]);

        }
    }

    public void Process_2014(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            MapUiWindow mgiw = GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>();
            mgiw.GetReward(dataSplit[2], int.Parse(dataSplit[1]));
            StartCoroutine(SceneTransformer.instance.NewbieGuide());// 新手引导;
        }
    }

    public void Process_2015(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            MapUiWindow mgiw = GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>();
            mgiw.GetReward(dataSplit[1], 3);
        }
    }

    public void Process_2016(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (RecvString.Length > 50)
        {
            if (GameObject.Find("MapObject") != null)
            {
                GameObject Map = GameObject.Find("MapObject");
                Map.transform.Find("MapCon").GetComponent<MapWindow>().InitTheChestStatue(dataSplit[0], dataSplit[2], dataSplit[1]);
                return;
            }
        }
        for (int i = 0; i < dataSplit.Length - 2; i++)
        {
            if (GameObject.Find("GateInfoWindow") != null)
            {
                GameObject.Find("GateInfoWindow").GetComponent<MapGateInfoWindow>().SetTheChestStatue(i, dataSplit[i]);
            }
        }
    }

    public void Process_2017(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject go = GameObject.Find("GateInfoWindow");
        if (go != null)
        {
            go.GetComponent<MapGateInfoWindow>().ChallengeCount = int.Parse(dataSplit[0]);
            go.GetComponent<MapGateInfoWindow>().ResetChanllage(int.Parse(dataSplit[1]));
            go.GetComponent<MapGateInfoWindow>().SetSweepBtn();
            go.GetComponent<MapGateInfoWindow>().SetChallengNum();
        }
        else
        {
            GameObject _wayWindow = GameObject.Find("WayWindow");
            if (_wayWindow != null)
            {
                string _url = "All/Right/Scroll View/Grid/WayFubenItem1_" + dataSplit[2];
                Transform _watItem = _wayWindow.transform.Find(_url);
                if (_watItem != null)
                    _watItem.GetComponent<WayItem>().UpdateCreamNum(int.Parse(dataSplit[0]));
                else
                    Debug.LogError("2017协议更新次数失败 " + _url + "  " + _watItem);
            }
        }
    }

    public void Process_2018(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject go = GameObject.Find("HeroListWindow");
        if (go != null)
        {
            go.GetComponent<HeroListWindow>().InitWindow(dataSplit[0], dataSplit[1]);
        }
    }
    ///<summary>
    /// 关卡翻牌
    /// <summary>
    public void Process_2101(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenSinglePanel("SelectedRewardsWindow", false);
                //dataSplit[1] 关卡id
                string[] rewardItem = dataSplit[2].Split('!');
                List<Item> _itemList = new List<Item>();
                for (int i = 0; i < rewardItem.Length - 1; i++)
                {

                    string[] Item = rewardItem[i].Split('$');
                    _itemList.Add(new Item(int.Parse(Item[0]), int.Parse(Item[1])));
                }
                GameObject.Find("SelectedRewardsWindow").GetComponent<SelectedRewardsWindow>().SelectInfo(_itemList);
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("关卡ID错误", PromptWindow.PromptType.Hint, null, null);
            }
        }

    }
    //弃牌暂不做
    public void Process_2102(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

    }
    public void Process_2103(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            string[] rewardItem = dataSplit[2].Split('!');
            List<Item> _itemList = new List<Item>();
            string[] Item = rewardItem[0].Split('$');
            _itemList.Add(new Item(int.Parse(Item[0]), int.Parse(Item[1])));
            GameObject.Find("SelectedRewardsWindow").GetComponent<SelectedRewardsWindow>().OpenRewardItem(int.Parse(dataSplit[4]), _itemList, int.Parse(dataSplit[1]));
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("未抽过奖", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("没奖励", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }

    }
    public void Process_2104(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //string[] rewardItem = dataSplit[1].Split('!');
            //List<Item> _itemList = new List<Item>();
            //for (int i = 0; i < rewardItem.Length - 1; i++)
            //{

            //    string[] Item = rewardItem[i].Split('$');
            //    _itemList.Add(new Item(int.Parse(Item[0]), int.Parse(Item[1])));
            //}
            //GameObject.Find("SelectedRewardsWindow").GetComponent<SelectedRewardsWindow>().SelectInfo(_itemList);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("未抽过奖", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("没奖励", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }

    /// <summary>
    /// 取得角色装备资讯
    /// </summary>
    /// <param name="RecvString"></param>
    public void Process_3001(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        BetterList<Hero.EquipInfo> roleEquipList = new BetterList<Hero.EquipInfo>();

        for (int i = 1; i < 7; i++)
        {
            if (dataSplit[i] != "")
            {
                string[] equipSplit = dataSplit[i].Split('$');
                Hero.EquipInfo equipInfo = new Hero.EquipInfo(i, int.Parse(equipSplit[0]), int.Parse(equipSplit[1]), int.Parse(equipSplit[2]), int.Parse(equipSplit[3]), int.Parse(equipSplit[4]), int.Parse(equipSplit[5]));
                roleEquipList.Add(equipInfo);
            }
            else
            {
                Hero.EquipInfo equipInfo = new Hero.EquipInfo(i, 0, 0, 0, 0, 0, 0);
                roleEquipList.Add(equipInfo);
            }
        }
        Hero hero = CharacterRecorder.instance.GetHeroByCharacterRoleID(int.Parse(dataSplit[0]));
        hero.SetHeroEquip(roleEquipList);
        if (GameObject.Find("StrengEquipWindow") != null)
        {
            StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
            esp.SetEuipList(roleEquipList);
        }
        if (GameObject.Find("AwakeWindow") != null)
        {
            AwakeWindow esp = GameObject.Find("AwakeWindow").GetComponent<AwakeWindow>();
            esp.SetEuipList(int.Parse(dataSplit[0]), roleEquipList);
        }
        if (GameObject.Find("RoleWindow") != null)
        {
            RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
            rw.SetHeroClick(-1);
        }
    }

    public void Process_3004(string RecvString)
    {
        //string[] dataSplit = RecvString.Split(';');
        //if (dataSplit[0] == "1")
        //{
        //    RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
        //    rw.SetEquipLevel(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[5]), int.Parse(dataSplit[4]));
        //    SendProcess("1005#" + dataSplit[1] + ";");
        //    SendProcess("3006#" + dataSplit[1] + ";" + dataSplit[2] + ";");
        //    SendProcess("3001#" + dataSplit[1] + ";");
        //    RoleEquipInfoWindow reiw = GameObject.Find("RoleEquipInfoWindow").GetComponent<RoleEquipInfoWindow>();
        //    reiw.Init(int.Parse(dataSplit[2]), int.Parse(dataSplit[1]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), -1, -1);
        //}
        //else
        //{
        //    if (dataSplit[1] == "4")
        //    {
        //        UIManager.instance.OpenPromptWindow("材料不足", PromptWindow.PromptType.Hint, null, null);
        //    }
        //    if (dataSplit[1] == "1")
        //    {
        //        UIManager.instance.OpenPromptWindow("进阶达到上限", PromptWindow.PromptType.Hint, null, null);
        //    }
        //}
    }

    public void Process_3005(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            AudioEditer.instance.PlayOneShot("ui_power");

            StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();//.EquipStrengerPart.GetComponent<EquipStrengerPart>();
            GameObject go = NGUITools.AddChild(esp.ListEquip[int.Parse(dataSplit[2]) - 1], esp.LevelUpEffect);
            go.transform.localScale = new Vector3(500, 500, 500);

            GameObject _ToolEquipUpEffect = GameObject.Instantiate(Resources.Load("Prefab/Effect/ZhuangBei_up", typeof(GameObject))) as GameObject;
            _ToolEquipUpEffect.transform.parent = esp.EquipIcon.transform.parent.transform;
            _ToolEquipUpEffect.transform.localPosition = Vector3.zero;
            _ToolEquipUpEffect.transform.localScale = Vector3.one;

            EquipStrengerPart esp2 = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().equipStrengPart.GetComponent<EquipStrengerPart>();
            //esp2.CheakEquipLevel(int.Parse(dataSplit[6]));
            esp2.CheakEquipLevel(int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            esp.ResetEquipData(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]));
            SendProcess("3001#" + dataSplit[1] + ";");
            SendProcess("1005#" + dataSplit[1] + ";");

            // esp.SetEquipIcon(int.Parse(dataSplit[2]), int.Parse(dataSplit[5]));
            esp.SetEquipIcon(int.Parse(dataSplit[3]));
            CharacterRecorder.instance.SetMoney(int.Parse(dataSplit[4]));
            GameObject.Find("StrengEquipWindow").transform.FindChild("TopContent").GetComponent<TopContent>().Reset();

        }
        else
        {
            if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("数据异常", PromptWindow.PromptType.Hint, null, null);
            }
            if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("装备等级不能超过战队等级", 11, false, PromptWindow.PromptType.Hint, null, null);
            }
            if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("金钱不足", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    /// <summary>
    /// 获取装备强化资讯
    /// </summary>
    public void Process_3006(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject sew = GameObject.Find("StrengEquipWindow");
        if (sew != null)
        {
            string[] proSplit = dataSplit[0].Split('$');
            string[] secSplit = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
            //sew.GetComponent<StrengEquipWindow>().EquipStrengerPart.GetComponent<EquipStrengerPart>().SetEquipInfo();
            sew.GetComponent<StrengEquipWindow>().SetEquipInfo();
        }
    }

    public void Process_3007(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("RoleWindow") != null)
            {
                HeroInfoPart rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>().HeroInfoPart.GetComponent<HeroInfoPart>();
                GameObject go = NGUITools.AddChild(rw.ListEquip[int.Parse(dataSplit[4]) - 1], rw.WearEquipEffect);
                go.name = "WearEquipEffect";
                go.transform.localScale = new Vector3(500, 500, 500);
                //rw.ListEquip[int.Parse(dataSplit[4]) - 1].transform.FindChild("WearEquipEffect").gameObject.SetActive(true);
                rw.SetEquip(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
                rw.SetEquipLevel(int.Parse(dataSplit[1]), int.Parse(dataSplit[4]), int.Parse(dataSplit[3]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]));
                SendProcess("1005#" + dataSplit[1] + ";");
            }
            if (GameObject.Find("RoleEquipInfoWindow") != null)
            {
                RoleEquipInfoWindow reiw = GameObject.Find("RoleEquipInfoWindow").GetComponent<RoleEquipInfoWindow>();
                reiw.Init(int.Parse(dataSplit[4]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            }
            SendProcess("3006#" + dataSplit[1] + ";" + dataSplit[4] + ";");


            foreach (var h in CharacterRecorder.instance.ownedHeroList)
            {
                if (h.characterRoleID == int.Parse(dataSplit[1]))
                {
                    Debug.Log("BBBBBBBBBBBBBBBBBB" + dataSplit[4]);
                    h.equipList[int.Parse(dataSplit[4]) - 1].equipCode = int.Parse(dataSplit[2]);
                    h.equipList[int.Parse(dataSplit[4]) - 1].equipID = int.Parse(dataSplit[3]);
                    h.equipList[int.Parse(dataSplit[4]) - 1].equipClass = int.Parse(dataSplit[5]);
                    h.equipList[int.Parse(dataSplit[4]) - 1].equipLevel = int.Parse(dataSplit[6]);
                }
            }
        }
    }

    //旧的 一键强化 不包含升品
    public void Process_3009(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {

            StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();//.equipStrengPart.GetComponent<EquipStrengerPart>();
            GameObject go = NGUITools.AddChild(esp.ListEquip[int.Parse(dataSplit[2]) - 1], esp.LevelUpEffect);
            go.transform.localScale = new Vector3(500, 500, 500);

            GameObject _ToolEquipUpEffect = GameObject.Instantiate(Resources.Load("Prefab/Effect/ZhuangBei_up", typeof(GameObject))) as GameObject;
            _ToolEquipUpEffect.transform.parent = esp.EquipIcon.transform.parent.transform;
            _ToolEquipUpEffect.transform.localPosition = Vector3.zero;
            _ToolEquipUpEffect.transform.localScale = Vector3.one;

            esp.ResetEquipData(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]));
            SendProcess("3001#" + dataSplit[1] + ";");
            SendProcess("1005#" + dataSplit[1] + ";");
            //SendProcess("3006#" + dataSplit[1] + ";" + dataSplit[2] + ";");

            EquipStrengerPart esp2 = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().equipStrengPart.GetComponent<EquipStrengerPart>();
            esp.SetEquipIcon(int.Parse(dataSplit[2]), int.Parse(dataSplit[5]));
            //esp2.CheakEquipLevel(int.Parse(dataSplit[6].ToString()));
            esp2.CheakEquipLevel(int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            CharacterRecorder.instance.SetMoney(int.Parse(dataSplit[4]));
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            AudioEditer.instance.PlayOneShot("ui_levelup");
        }
        else
        {
            switch (dataSplit[1])
            {

                case "1": UIManager.instance.OpenPromptWindow("数据异常", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("装备等级不能超过战队等级", 11, false, PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("金钱不足", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    //新的 一键强化 包含升品
    /*  public void Process_3009(string RecvString)
      {
          string[] dataSplit = RecvString.Split(';');
          if (dataSplit[0] == "1")
          {
              CharacterRecorder.instance.gold = int.Parse(dataSplit[4]);
              if (GameObject.Find("TopContent") != null)
              {
                  TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                  tc.Reset();
              }
              if (CharacterRecorder.instance.isOneKeyState)
              {
                  NetworkHandler.instance.SendProcess("3019#" + dataSplit[1] + ";" + StrengEquipWindow.ClickIndex + ";");

                  StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();//.equipStrengPart.GetComponent<EquipStrengerPart>();
                  GameObject go = NGUITools.AddChild(esp.ListEquip[StrengEquipWindow.ClickIndex - 1], esp.LevelUpEffect);
                  go.transform.localScale = new Vector3(500, 500, 500);

                  GameObject _ToolEquipUpEffect = GameObject.Instantiate(Resources.Load("Prefab/Effect/ZhuangBei_up", typeof(GameObject))) as GameObject;
                  _ToolEquipUpEffect.transform.parent = esp.EquipIcon.transform.parent.transform;
                  _ToolEquipUpEffect.transform.localPosition = Vector3.zero;
                  _ToolEquipUpEffect.transform.localScale = Vector3.one;
                  AudioEditer.instance.PlayOneShot("ui_levelup");
              }
              else
              {
                  StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();//.equipStrengPart.GetComponent<EquipStrengerPart>();
                  GameObject go = NGUITools.AddChild(esp.ListEquip[int.Parse(dataSplit[2]) - 1], esp.LevelUpEffect);
                  go.transform.localScale = new Vector3(500, 500, 500);

                  GameObject _ToolEquipUpEffect = GameObject.Instantiate(Resources.Load("Prefab/Effect/ZhuangBei_up", typeof(GameObject))) as GameObject;
                  _ToolEquipUpEffect.transform.parent = esp.EquipIcon.transform.parent.transform;
                  _ToolEquipUpEffect.transform.localPosition = Vector3.zero;
                  _ToolEquipUpEffect.transform.localScale = Vector3.one;

                  esp.ResetEquipData(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]));
                  SendProcess("3001#" + dataSplit[1] + ";");
                  SendProcess("1005#" + dataSplit[1] + ";");
                  //SendProcess("3006#" + dataSplit[1] + ";" + dataSplit[2] + ";");

                  EquipStrengerPart esp2 = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().equipStrengPart.GetComponent<EquipStrengerPart>();
                  esp.SetEquipIcon(int.Parse(dataSplit[2]), int.Parse(dataSplit[5]));
                  //esp2.CheakEquipLevel(int.Parse(dataSplit[6].ToString()));
                  esp2.CheakEquipLevel(int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
                  CharacterRecorder.instance.SetMoney(int.Parse(dataSplit[4]));
                  GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
                  AudioEditer.instance.PlayOneShot("ui_levelup");
              }
          }
          else
          {
              if (CharacterRecorder.instance.isOneKeyState)
              {
                  CharacterRecorder.instance.isOneKeyState = false;
                  SendProcess("3001#" + dataSplit[2] + ";");
                  SendProcess("1005#" + dataSplit[2] + ";");
                  StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
                  for (int i = 0; i < esp.ListEquip.Count;i++ )
                  {
                      esp.ListEquip[i].GetComponent<BoxCollider>().enabled = true;
                  }
                  for (int i = 0; i < esp.ListHero.Count; i++)
                  {
                      esp.ListHero[i].GetComponent<BoxCollider>().enabled = true;
                  }
                  EquipStrengerPart esp2 = esp.equipStrengPart.GetComponent<EquipStrengerPart>();
                  esp2.OneKeyButton.GetComponent<UIButton>().isEnabled = true;
                  switch (dataSplit[1])
                  {
                      case "3": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                  }
              }
              else
              {
                  switch (dataSplit[1])
                  {

                      case "1": UIManager.instance.OpenPromptWindow("数据异常", PromptWindow.PromptType.Hint, null, null); break;
                      case "2": UIManager.instance.OpenPromptWindow("装备等级不能超过战队等级", 11, false, PromptWindow.PromptType.Hint, null, null); break;
                      case "3": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                      //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
                  }
              }
          }
      }*/

    public void Process_3010(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            int _charactorID = int.Parse(dataSplit[1]);
            int _equipPos = int.Parse(dataSplit[2]);
            int _refineLv = int.Parse(dataSplit[4]);
            int _equipLevel = int.Parse(dataSplit[5]);
            int _equipExp = int.Parse(dataSplit[6]);
            foreach (var _hero in CharacterRecorder.instance.ownedHeroList)
            {
                if (_charactorID == _hero.characterRoleID)
                {
                    Hero curHero = _hero;
                    curHero.equipList[_equipPos - 1].equipClass = _refineLv;
                    curHero.equipList[_equipPos - 1].equipLevel = _equipLevel;
                    curHero.equipList[_equipPos - 1].equipExp = _equipExp;
                }
            }
            SendProcess("1005#" + dataSplit[1] + ";");
            if (GameObject.Find("StrengEquipWindow") != null)
            {
                //RefineWindow rfw = GameObject.Find("RefinePart").GetComponent<RefineWindow>();
                StrengEquipWindow SEW = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
                //rfw.SetEquipLevel(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
                SEW.SetEquipInfo();
                SEW.PlayEquipRefineDataLabelEffect();
                //rfw.SetInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));//, 0);

                EquipRefinePart _EquipRefinePart = SEW.equipRefinePart.GetComponent<EquipRefinePart>();
                _EquipRefinePart.ScuccedJingLianSliderEffect();
                _EquipRefinePart.SetInfo(_charactorID, _equipPos);
            }
            else if (GameObject.Find("RoleWindow") != null)
            {
                HeroInfoPart rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>().HeroInfoPart.GetComponent<HeroInfoPart>();
                rw.SetEquipLevel(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("该位置无装备", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("等级达上限", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("此物品无法用于精炼", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("背包无此物品", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "4")
            {
                UIManager.instance.OpenPromptWindow("精炼已达上限，请提升战队等级", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "5")
            {
                UIManager.instance.OpenPromptWindow("饰品装备", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "6")
            {
                UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "9")
            {
                EquipRefinePart rfw = GameObject.Find("EquipRefinePart").GetComponent<EquipRefinePart>();
                rfw.SetEquipLevel(int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]));
                //rfw.SetInfo(int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));//, 0);
                StrengEquipWindow SEW = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
                EquipRefinePart _EquipRefinePart = SEW.equipRefinePart.GetComponent<EquipRefinePart>();
                _EquipRefinePart.HalfScuccedJingLianSliderEffect();
                _EquipRefinePart.SetInfo(int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            }
        }
    }
    public void Process_3016(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            TextTranslator.instance.isUpdateBag = true;
            //Debug.LogError(RecvString);
            AudioEditer.instance.PlayOneShot("ui_qianghua");
            StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();//.baoWuStrengPart.GetComponent<BaoWuStrengPart>();
                                                                                                           //esp.CheakEquipLevel(int.Parse(dataSplit[5]));
            esp.RestSetBaoWuLevel(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            CharacterRecorder.instance.SetMoney(int.Parse(dataSplit[7]));
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            SendProcess("3001#" + dataSplit[1] + ";");
            SendProcess("1005#" + dataSplit[1] + ";");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("无此装备", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("不是宝物", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "4":
                    UIManager.instance.OpenPromptWindow("物品不足", PromptWindow.PromptType.Hint, null, null);
                    if (TextTranslator.instance.isUpdateBag)
                    {
                        NetworkHandler.instance.SendProcess("5001#;");
                    }
                    break;
                    //default: UIManager.instance.OpenPromptWindow("失败code" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }

        }
    }
    public void Process_3017(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            int _roleId = int.Parse(dataSplit[1]);
            int _equipPosition = int.Parse(dataSplit[2]);
            foreach (var h in CharacterRecorder.instance.ownedHeroList)
            {
                if (h.characterRoleID == _roleId)
                {
                    //Debug.LogError("_roleId..." + _roleId + ".._equipPosition........." + _equipPosition);
                    h.equipList[_equipPosition - 1].equipClass = int.Parse(dataSplit[4]);
                    h.equipList[_equipPosition - 1].equipLevel = int.Parse(dataSplit[5]);
                    h.equipList[_equipPosition - 1].equipExp = int.Parse(dataSplit[6]);
                    //Debug.LogError("精炼" + h.equipList[_equipPosition - 1].equipClass + "..等级..." + h.equipList[_equipPosition - 1].equipLevel + ".. 精炼经验.." + h.equipList[_equipPosition - 1].equipExp);
                }
            }
            //Debug.LogError("精炼" + dataSplit[4] + "..等级..." + dataSplit[5] + ".. 精炼经验.." + dataSplit[6]);
            StrengEquipWindow _StrengEquipWindow = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
            RefineWindow RFW = _StrengEquipWindow.baoWuRefinePart.GetComponent<RefineWindow>();
            RFW.SetInfo(_roleId, int.Parse(dataSplit[2]));//, int.Parse(dataSplit[3]));//, int.Parse(dataSplit[4]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            _StrengEquipWindow.RefineBaoWuResult();

            CharacterRecorder.instance.gold = int.Parse(dataSplit[7]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }

            SendProcess("1005#" + _roleId + ";");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("无此装备", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("不是宝物", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("精炼等级达上限", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("战队等级不足", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("精炼石不足", PromptWindow.PromptType.Hint, null, null); break;
                case "5": UIManager.instance.OpenPromptWindow("同类不足", PromptWindow.PromptType.Hint, null, null); break;
                case "6": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("失败code" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }

        }
    }
    public void Process_3018(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            int _roleId = int.Parse(dataSplit[1]);
            int _equipPosition = int.Parse(dataSplit[2]);
            int _equipId = int.Parse(dataSplit[3]);
            int _equipClass = int.Parse(dataSplit[4]);
            int _equipLevel = int.Parse(dataSplit[5]);
            int _equipExp = int.Parse(dataSplit[6]);
            foreach (var h in CharacterRecorder.instance.ownedHeroList)
            {
                if (h.characterRoleID == _roleId)
                {
                    //Debug.LogError("_roleId..." + _roleId + ".._equipPosition........." + _equipPosition);
                    h.equipList[_equipPosition - 1].equipCode = _equipId;
                    h.equipList[_equipPosition - 1].equipClass = _equipClass;
                    h.equipList[_equipPosition - 1].equipLevel = _equipLevel;
                    h.equipList[_equipPosition - 1].equipExp = _equipExp;
                    //Debug.LogError("觉醒精炼" + h.equipList[_equipPosition - 1].equipClass + "..等级..." + h.equipList[_equipPosition - 1].equipLevel + ".. 精炼经验.." + h.equipList[_equipPosition - 1].equipExp);
                }
            }
            //Debug.LogError("精炼" + dataSplit[4] + "..等级..." + dataSplit[5] + ".. 精炼经验.." + dataSplit[6]);
            //RefineWindow RFW = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().baoWuRefinePart.GetComponent<RefineWindow>();
            StrengEquipWindow RFW = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
            RFW.ScuccedAwakeResult(_roleId, _equipPosition, _equipId, _equipClass, _equipLevel, _equipExp);

            SendProcess("1004#" + _roleId + ";");
            SendProcess("3001#" + dataSplit[1] + ";");
            SendProcess("1005#" + dataSplit[1] + ";");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("无此装备", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("不是宝物", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("失败code" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }

        }
    }
    //旧的 升品 不包含于 一键升级
    public void Process_3019(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            TextTranslator.instance.isUpdateBag = true;
            SendProcess("3001#" + dataSplit[1] + ";");
            SendProcess("1005#" + dataSplit[1] + ";");
            CharacterRecorder.instance.gold = int.Parse(dataSplit[5]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }
            int _roleId = int.Parse(dataSplit[1]);
            int _equipPosition = int.Parse(dataSplit[3]);
            int _equipId = int.Parse(dataSplit[2]);
            int _equipColorNum = int.Parse(dataSplit[4]);
            int _equipLevel = 0;
            foreach (var h in CharacterRecorder.instance.ownedHeroList)
            {
                if (h.characterRoleID == _roleId)
                {
                    //Debug.LogError("_roleId..." + _roleId + ".._equipPosition........." + _equipPosition);
                    h.equipList[_equipPosition - 1].equipCode = _equipId;
                    h.equipList[_equipPosition - 1].equipColorNum = _equipColorNum;
                    _equipLevel = h.equipList[_equipPosition - 1].equipLevel;
                    //Debug.LogError("装备升颜色" + h.equipList[_equipPosition - 1].equipClass + "..等级..." + h.equipList[_equipPosition - 1].equipLevel);
                }
            }
            StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();//.equipStrengPart.GetComponent<EquipStrengerPart>();
            esp.SetEquipColor(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
            esp.SetEquipIcon(int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
            EquipStrengerPart esp2 = esp.equipStrengPart.GetComponent<EquipStrengerPart>();
            //esp2.CheakEquipLevel(_equipLevel);
            esp2.CheakEquipLevel(_equipLevel, _equipColorNum);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("无此装备", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "4":
                    UIManager.instance.OpenPromptWindow("材料不足", PromptWindow.PromptType.Hint, null, null);
                    if (TextTranslator.instance.isUpdateBag)
                    {
                        NetworkHandler.instance.SendProcess("5001#;");
                    }
                    break;
                    //default: UIManager.instance.OpenPromptWindow("无返回错误提示", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    //新的 升品 包含于 一键升级
    /* public void Process_3019(string RecvString)
     {
         string[] dataSplit = RecvString.Split(';');
         if (dataSplit[0] == "1")
         {
             TextTranslator.instance.isUpdateBag = true;
             CharacterRecorder.instance.gold = int.Parse(dataSplit[5]);
             if (GameObject.Find("TopContent") != null)
             {
                 TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                 tc.Reset();
             }
             if (CharacterRecorder.instance.isOneKeyState)
             {
                 NetworkHandler.instance.SendProcess("3009#" + dataSplit[1] + ";" + StrengEquipWindow.ClickIndex + ";");
             }
             else
             {
                 SendProcess("3001#" + dataSplit[1] + ";");
                 SendProcess("1005#" + dataSplit[1] + ";");

                 int _roleId = int.Parse(dataSplit[1]);
                 int _equipPosition = int.Parse(dataSplit[3]);
                 int _equipId = int.Parse(dataSplit[2]);
                 int _equipColorNum = int.Parse(dataSplit[4]);
                 int _equipLevel = 0;
                 foreach (var h in CharacterRecorder.instance.ownedHeroList)
                 {
                     if (h.characterRoleID == _roleId)
                     {
                         //Debug.LogError("_roleId..." + _roleId + ".._equipPosition........." + _equipPosition);
                         h.equipList[_equipPosition - 1].equipCode = _equipId;
                         h.equipList[_equipPosition - 1].equipColorNum = _equipColorNum;
                         _equipLevel = h.equipList[_equipPosition - 1].equipLevel;
                         //Debug.LogError("装备升颜色" + h.equipList[_equipPosition - 1].equipClass + "..等级..." + h.equipList[_equipPosition - 1].equipLevel);
                     }
                 }
                 StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();//.equipStrengPart.GetComponent<EquipStrengerPart>();
                 esp.SetEquipColor(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
                 esp.SetEquipIcon(int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
                 EquipStrengerPart esp2 = esp.equipStrengPart.GetComponent<EquipStrengerPart>();
                 //esp2.CheakEquipLevel(_equipLevel);
                 esp2.CheakEquipLevel(_equipLevel, _equipColorNum);
             }


         }
         else
         {
             if (CharacterRecorder.instance.isOneKeyState)
             {
                 StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
                 for (int i = 0; i < esp.ListEquip.Count; i++)
                 {
                     esp.ListEquip[i].GetComponent<BoxCollider>().enabled = true;
                 }
                 for (int i = 0; i < esp.ListHero.Count; i++)
                 {
                     esp.ListHero[i].GetComponent<BoxCollider>().enabled = true;
                 }
                 EquipStrengerPart esp2 = esp.equipStrengPart.GetComponent<EquipStrengerPart>();
                 esp2.OneKeyButton.GetComponent<UIButton>().isEnabled = true;
                 SendProcess("3001#" + dataSplit[2] + ";");
                 SendProcess("1005#" + dataSplit[2] + ";");
                 CharacterRecorder.instance.isOneKeyState = false;
             }
             else
             {
                 switch (dataSplit[1])
                 {
                     case "0": UIManager.instance.OpenPromptWindow("无此装备", PromptWindow.PromptType.Hint, null, null); break;
                     case "1": UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null); break;
                     case "2": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                     case "4":
                         UIManager.instance.OpenPromptWindow("材料不足", PromptWindow.PromptType.Hint, null, null);
                         if (TextTranslator.instance.isUpdateBag)
                         {
                             NetworkHandler.instance.SendProcess("5001#;");
                         }
                         break;
                     //default: UIManager.instance.OpenPromptWindow("无返回错误提示", PromptWindow.PromptType.Hint, null, null); break;
                 }
             }

         }
     }*/
    public void Process_3020(string RecvString)//全部升级
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.SetMoney(int.Parse(dataSplit[3]));
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();

            StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();

            GameObject _ToolEquipUpEffect = GameObject.Instantiate(Resources.Load("Prefab/Effect/ZhuangBei_up", typeof(GameObject))) as GameObject;
            _ToolEquipUpEffect.transform.parent = esp.EquipIcon.transform.parent.transform;
            _ToolEquipUpEffect.transform.localPosition = Vector3.zero;
            _ToolEquipUpEffect.transform.localScale = Vector3.one;

            int _CharacterRoleID = int.Parse(dataSplit[1]);
            Hero hero = CharacterRecorder.instance.GetHeroByCharacterRoleID(_CharacterRoleID);
            string[] dataSplit2 = dataSplit[2].Split('!');

            SendProcess("3001#" + dataSplit[1] + ";");
            SendProcess("1005#" + dataSplit[1] + ";");

            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                int position = int.Parse(dataSplit3[0]);
                int _EquipID = int.Parse(dataSplit3[1]);
                int _Class = int.Parse(dataSplit3[2]);
                int level = int.Parse(dataSplit3[3]);
                if (position == StrengEquipWindow.ClickIndex)
                {
                    esp.OnEquipDataUpEffect();


                    EquipStrengerPart esp2 = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().equipStrengPart.GetComponent<EquipStrengerPart>();
                    esp.SetEquipIcon(position, _Class);
                    esp2.CheakEquipLevel(level, _Class);
                }
                GameObject go = NGUITools.AddChild(esp.ListEquip[position - 1], esp.LevelUpEffect);
                go.transform.localScale = new Vector3(500, 500, 500);
                foreach (var e in hero.equipList)
                {
                    if (e.equipID == _EquipID)
                    {
                        e.equipLevel = level;
                        e.equipClass = _Class;
                        //e.equipExp = _Exp;
                        break;
                    }
                }
            }

            AudioEditer.instance.PlayOneShot("ui_levelup");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("金钱不足", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("数据异常", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("装备等级不能超过战队等级", 11, false, PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("金钱不足", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_3021(string RecvString)//全部升品
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }

            TextTranslator.instance.isUpdateBag = true;
            SendProcess("3001#" + dataSplit[1] + ";");
            SendProcess("1005#" + dataSplit[1] + ";");

            int _CharacterRoleID = int.Parse(dataSplit[1]);
            Hero hero = CharacterRecorder.instance.GetHeroByCharacterRoleID(_CharacterRoleID);
            string[] dataSplit2 = dataSplit[2].Split('!');
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                int _equipId = int.Parse(dataSplit3[0]);
                int position = int.Parse(dataSplit3[1]);
                int _Class = int.Parse(dataSplit3[2]);

                hero.equipList[position - 1].equipCode = _equipId;
                hero.equipList[position - 1].equipColorNum = _Class;
                int _equipLevel = hero.equipList[position - 1].equipLevel;
                if (position == StrengEquipWindow.ClickIndex)
                {
                    StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
                    //esp.SetEquipColor(_CharacterRoleID, _equipId, position, _Class);
                    esp.SetEquipColorForAll(_CharacterRoleID, _equipId, position, _Class);
                    esp.SetEquipIcon(position, _Class);
                    EquipStrengerPart esp2 = esp.equipStrengPart.GetComponent<EquipStrengerPart>();
                    esp2.CheakEquipLevel(_equipLevel, _Class);
                }
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("材料不足", PromptWindow.PromptType.Hint, null, null);
                    if (TextTranslator.instance.isUpdateBag)
                    {
                        NetworkHandler.instance.SendProcess("5001#;");
                    }
                    break;
                case "1": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null); break;
                case "4":
                    UIManager.instance.OpenPromptWindow("材料不足", PromptWindow.PromptType.Hint, null, null);
                    if (TextTranslator.instance.isUpdateBag)
                    {
                        NetworkHandler.instance.SendProcess("5001#;");
                    }
                    break;
                    //default: UIManager.instance.OpenPromptWindow("无返回错误提示", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_3101(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        int charaterRoleId = int.Parse(dataSplit[0]);
        string[] dataSplit2 = dataSplit[1].Split('!');
        /* for (int i = 0; i < dataSplit2.Length - 1; i++)
         {
             string[] dataSplit3 = dataSplit2[i].Split('$');
             int position = int.Parse(dataSplit3[0]);
             int stoneId = int.Parse(dataSplit3[1]);
             int stoneLevel = int.Parse(dataSplit3[2]);
             int stoneExp = int.Parse(dataSplit3[3]);
             TextTranslator.instance.RareTreasureOpenDic[position].RemoveRareTreasure(position);
             TextTranslator.instance.RareTreasureOpenDic[position].SetRareTreasureOpen(stoneId, stoneLevel, stoneExp);
         }*/
        BetterList<Hero.RareStoneInfo> roleRareStoneList = new BetterList<Hero.RareStoneInfo>();
        for (int i = 0; i < dataSplit2.Length - 1; i++)
        {
            if (dataSplit2[i] != "")
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                int position = int.Parse(dataSplit3[0]);
                int stoneId = int.Parse(dataSplit3[1]);
                int stoneLevel = int.Parse(dataSplit3[2]);
                int stoneExp = int.Parse(dataSplit3[3]);
                TextTranslator.instance.RareTreasureOpenDic[position].RemoveRareTreasure(position);
                if (stoneId != 0)
                {
                    CharacterRecorder.instance.isHadFriteStone = true;
                }
                TextTranslator.instance.RareTreasureOpenDic[position].SetRareTreasureOpen(stoneId, stoneLevel, stoneExp);

                Hero.RareStoneInfo equipInfo = new Hero.RareStoneInfo(position, stoneId, stoneLevel, stoneExp);
                roleRareStoneList.Add(equipInfo);
            }
            else
            {
                Hero.RareStoneInfo equipInfo = new Hero.RareStoneInfo(i + 1, 0, 0, 0);
                roleRareStoneList.Add(equipInfo);
            }
        }
        Hero hero = CharacterRecorder.instance.GetHeroByCharacterRoleID(charaterRoleId);
        hero.SetHeroRareStone(roleRareStoneList);

        //hero.score = GetScoreOfOneHero(hero);

        GameObject _SecretStoneWindow = GameObject.Find("SecretStoneWindow");
        if (_SecretStoneWindow != null)
        {
            _SecretStoneWindow.GetComponent<SecretStoneWindow>().InitEquipItemOpenState();
        }

        if (GameObject.Find("StrengEquipWindow") != null)
        {
            StrengEquipWindow esp = GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>();
            esp.SetRedPointRareStone(hero);
        }
    }
    public void Process_3102(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            int charaterRoleId = int.Parse(dataSplit[1]);
            int position = int.Parse(dataSplit[2]);
            int stoneId = int.Parse(dataSplit[3]);
            int stoneLevel = int.Parse(dataSplit[4]);
            int stoneExp = int.Parse(dataSplit[5]);
            TextTranslator.instance.RareTreasureOpenDic[position].SetRareTreasureOpen(stoneId, stoneLevel, stoneExp);

            SendProcess("1005#" + charaterRoleId + ";");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("非秘宝", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("无此角色", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("位置未开放", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("此位置有秘宝", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("同一类型秘宝只能装一个", PromptWindow.PromptType.Hint, null, null); break;
                default: UIManager.instance.OpenPromptWindow("请更换更高品质秘宝", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
        GameObject _SecretStoneWindow = GameObject.Find("SecretStoneWindow");
        if (_SecretStoneWindow != null)
        {
            _SecretStoneWindow.GetComponent<SecretStoneWindow>().InitEquipItemOpenState();
        }
    }
    public void Process_3103(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            int charaterRoleId = int.Parse(dataSplit[1]);
            int position = int.Parse(dataSplit[2]);
            //TextTranslator.instance.RareTreasureOpenDic.Remove(position);
            TextTranslator.instance.RareTreasureOpenDic[position].RemoveRareTreasure(position);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }

            SendProcess("1005#" + charaterRoleId + ";");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("没有此角色", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("没有此秘宝", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
                default: UIManager.instance.OpenPromptWindow("无返回错误提示", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
        GameObject _SecretStoneWindow = GameObject.Find("SecretStoneWindow");
        if (_SecretStoneWindow != null)
        {
            _SecretStoneWindow.GetComponent<SecretStoneWindow>().InitEquipItemOpenState();
        }
    }
    public void Process_3104(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            int charaterRoleId = int.Parse(dataSplit[1]);
            int position = int.Parse(dataSplit[2]);
            int stoneId = int.Parse(dataSplit[3]);
            int stoneLevel = int.Parse(dataSplit[4]);
            int stoneExp = int.Parse(dataSplit[5]);
            TextTranslator.instance.RareTreasureOpenDic[position].SetRareTreasureOpen(stoneId, stoneLevel, stoneExp);

            StoneInfoPart esp = GameObject.Find("StoneInfoPart").GetComponent<StoneInfoPart>();
            GameObject _ToolEquipUpEffect = GameObject.Instantiate(Resources.Load("Prefab/Effect/ZhuangBei_up", typeof(GameObject))) as GameObject;
            _ToolEquipUpEffect.transform.parent = esp.stoneIcon.transform.parent.transform;
            _ToolEquipUpEffect.transform.localPosition = Vector3.zero;
            _ToolEquipUpEffect.transform.localScale = Vector3.one;

            SendProcess("1005#" + charaterRoleId + ";");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("没有此角色", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("没有此秘宝", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("消耗物品不足", PromptWindow.PromptType.Hint, null, null); break;
                default: UIManager.instance.OpenPromptWindow("无返回错误提示", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
        GameObject _SecretStoneWindow = GameObject.Find("SecretStoneWindow");
        if (_SecretStoneWindow != null)
        {
            _SecretStoneWindow.GetComponent<SecretStoneWindow>().InitEquipItemOpenState();
        }
    }

    //豪车
    public void Process_3201(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LuxuryCarWindow") != null)
        {
            GameObject.Find("LuxuryCarWindow").GetComponent<LuxuryCarWindow>().SetCarInfo(dataSplit);

        }
        if (GameObject.Find("StrengEquipWindow") != null)
        {
            GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().SuperCarRedPoint(dataSplit);
        }
        GameObject rebirth = GameObject.Find("RebirthWindow");
        BetterList<Hero.SuperCarInfo> superCarList = new BetterList<Hero.SuperCarInfo>();
        Hero.SuperCarInfo superCarInfo = new Hero.SuperCarInfo(int.Parse(dataSplit[0].Split('$')[0]), int.Parse(dataSplit[0].Split('$')[1]),
                                                             int.Parse(dataSplit[1].Split('$')[0]), int.Parse(dataSplit[1].Split('$')[1]),
                                                             int.Parse(dataSplit[2].Split('$')[0]), int.Parse(dataSplit[2].Split('$')[1]),
                                                             int.Parse(dataSplit[3].Split('$')[0]), int.Parse(dataSplit[3].Split('$')[1]),
                                                             int.Parse(dataSplit[4].Split('$')[0]), int.Parse(dataSplit[4].Split('$')[1]),
                                                             int.Parse(dataSplit[5].Split('$')[0]), int.Parse(dataSplit[5].Split('$')[1])
            );
        Hero hero = CharacterRecorder.instance.GetHeroByCharacterRoleID(int.Parse(dataSplit[6]));
        superCarList.Add(superCarInfo);
        hero.SetSuperCarInfo(superCarList);
        if (rebirth != null)
        {
            rebirth.GetComponent<RebirthWindow>().LuxuryCarRebirth(dataSplit);
        }
    }
    public void Process_3202(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 1)
        {
            string[] strSplit = dataSplit[1].Split('$');
            int id = 42000 + int.Parse(strSplit[0]);
            GameObject.Find(id.ToString()).GetComponent<SuperCarItem>().CarInfo(id, int.Parse(strSplit[1]));
            SendProcess("1005#" + dataSplit[2] + ";");
        }
    }
    //神器
    public void Process_3301(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject nw = GameObject.Find("NuclearWeaponWindow");
        if (nw != null)
        {
            nw.GetComponent<NuclearWeaponWindow>().SetInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
        GameObject litter = GameObject.Find("LittleHelperWindow");
        if (litter != null)
        {
            litter.GetComponent<LittleHelperWindow>().ReceiverMsg_3301(dataSplit);
        }
    }
    public void Process_3302(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 1)
        {
            NuclearWeaponWindow WeaponWindow = GameObject.Find("NuclearWeaponWindow").GetComponent<NuclearWeaponWindow>();
            WeaponWindow.UpStarWindow.SetActive(true);
            WeaponWindow.UpClassWindow.SetActive(false);
            WeaponWindow.SynthesisWindow.SetActive(false);
            WeaponWindow.UpStarWindowInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            GameObject aw = UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            aw.GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.WeaponUpStart, int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), dataSplit[4], dataSplit[5]);
            SendProcess("1005#" + dataSplit[6]);
            GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().WeaponRedPoint();

            string ItemName = TextTranslator.instance.GetItemNameByItemCode(int.Parse(dataSplit[1]));
            switch (int.Parse(dataSplit[2]))
            {
                case 1:
                    ItemName = "[00ff3c]" + ItemName + "[-]";
                    break;
                case 2:
                    ItemName = "[009cff]" + ItemName + "[-]";
                    break;
                case 3:
                    ItemName = "[b500ff]" + ItemName + "[-]";
                    break;
                case 4:
                    ItemName = "[ff6c00]" + ItemName + "[-]";
                    break;
                case 5:
                    ItemName = "[ff0000]" + ItemName + "[-]";
                    break;
                default:
                    break;
            }


            SendProcess(string.Format("7002#{0};{1};{2};{3}", 22, CharacterRecorder.instance.characterName, ItemName, 0));
        }
        else
        {
            switch (int.Parse(dataSplit[0]))
            {
                case 0:
                    UIManager.instance.OpenPromptWindow("核武器碎片不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case 1:
                    UIManager.instance.OpenPromptWindow("神器ID错误", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_3303(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 1)
        {
            NuclearWeaponWindow WeaponWindow = GameObject.Find("NuclearWeaponWindow").GetComponent<NuclearWeaponWindow>();
            WeaponWindow.UpStarWindow.SetActive(true);
            WeaponWindow.UpClassWindow.SetActive(false);
            WeaponWindow.SynthesisWindow.SetActive(false);
            WeaponWindow.UpStarWindowInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            GameObject aw = UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            aw.GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.WeaponUpStart, int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), dataSplit[4], dataSplit[5]);
            SendProcess("1005#" + dataSplit[6]);
            GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().WeaponRedPoint();

            string colorStr = "";
            switch (int.Parse(dataSplit[2]))
            {
                case 1:
                    colorStr = "[00ff3c]绿[-]";
                    break;
                case 2:
                    colorStr = "[009cff]蓝[-]";
                    break;
                case 3:
                    colorStr = "[b500ff]紫[-]";
                    break;
                case 4:
                    colorStr = "[ff6c00]橙[-]";
                    break;
                case 5:
                    colorStr = "[ff0000]红[-]";
                    break;
            }
            SendProcess(string.Format("7002#{0};{1};{2};{3}", 23, CharacterRecorder.instance.characterName, TextTranslator.instance.GetItemNameByItemCode(int.Parse(dataSplit[1])), colorStr));
        }
        else
        {
            switch (int.Parse(dataSplit[1]))
            {
                case 0:
                    UIManager.instance.OpenPromptWindow("星级不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case 1:
                    UIManager.instance.OpenPromptWindow("核武器已升到顶阶ID错误", PromptWindow.PromptType.Hint, null, null);
                    break;
                case 2:
                    UIManager.instance.OpenPromptWindow("升阶材料不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_3304(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 1)
        {
            NuclearWeaponWindow WeaponWindow = GameObject.Find("NuclearWeaponWindow").GetComponent<NuclearWeaponWindow>();
            WeaponWindow.UpStarWindow.SetActive(true);
            WeaponWindow.UpClassWindow.SetActive(false);
            WeaponWindow.SynthesisWindow.SetActive(false);
            WeaponWindow.UpStarWindowInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            if (dataSplit[2] != "5" && dataSplit[3] != "5")
            {
                GameObject aw = UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                aw.GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.WeaponUpStart, int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), dataSplit[4], dataSplit[5]);
            }
            SendProcess("1005#" + dataSplit[6]);
            GameObject.Find("StrengEquipWindow").GetComponent<StrengEquipWindow>().WeaponRedPoint();
        }
        else
        {
            switch (int.Parse(dataSplit[1]))
            {
                case 0:
                    UIManager.instance.OpenPromptWindow("升星材料不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case 1:
                    //UIManager.instance.OpenPromptWindow("升星失败", PromptWindow.PromptType.Hint, null, null);
                    GameObject.Find("NuclearWeaponWindow").GetComponent<NuclearWeaponWindow>().ResetItem10105();
                    GameObject.Find("NuclearWeaponWindow").GetComponent<NuclearWeaponWindow>().ShowShengXingFailed();
                    break;
                case 2:
                    UIManager.instance.OpenPromptWindow("满星", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_3305(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject tw = GameObject.Find("TurntableWindow");
        if (tw != null)
        {
            tw.GetComponent<TurntableWindow>().SetInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            tw.GetComponent<TurntableWindow>().FramgeSetInfo(CharacterRecorder.instance.heroPresentWeapon);
        }
        if (int.Parse(dataSplit[2]) != 0)
        {
            CharacterRecorder.instance.isWeaponGachaFree = true;
        }
        else
        {
            CharacterRecorder.instance.isWeaponGachaFree = false;
        }

    }
    public void Process_3306(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1" && GameObject.Find("TurntableWindow") != null)
        {
            GameObject.Find("TurntableWindow").GetComponent<TurntableWindow>().FramgeSetInfo(int.Parse(dataSplit[1]));
        }
    }
    public void Process_3307(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {

            TurntableWindow tw = GameObject.Find("TurntableWindow").GetComponent<TurntableWindow>();
            tw.EffectBG.SetActive(false);
            string[] RewardItem = dataSplit[1].Split('!');
            List<Item> itemList = new List<Item>();
            List<int> RewardID = new List<int>();
            for (int i = 0; i < RewardItem.Length; i++)
            {
                string[] itemSplit = RewardItem[i].Split('$');
                itemList.Add(new Item(int.Parse(itemSplit[1]), int.Parse(itemSplit[2])));
                RewardID.Add(int.Parse(itemSplit[0]));
            }
            tw.OnceGaChaInfo(itemList, int.Parse(dataSplit[2]), RewardID, int.Parse(dataSplit[4]));
            UpDateTopContentData(itemList);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            GameObject.Find("TurntableWindow").GetComponent<TurntableWindow>().IsButtonFinish = false;
            switch (int.Parse(dataSplit[1]))
            {
                case 0:
                    UIManager.instance.OpenPromptWindow("奖励错误", PromptWindow.PromptType.Hint, null, null);

                    break;
                case 1:
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_3308(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            TurntableWindow tw = GameObject.Find("TurntableWindow").GetComponent<TurntableWindow>();
            tw.EffectBG.SetActive(false);
            string[] RewardItem = dataSplit[1].Split('!');
            List<Item> itemList = new List<Item>();
            List<int> RewardID = new List<int>();
            for (int i = 0; i < RewardItem.Length - 1; i++)
            {
                string[] itemSplit = RewardItem[i].Split('$');
                itemList.Add(new Item(int.Parse(itemSplit[1]), int.Parse(itemSplit[2])));
                RewardID.Add(int.Parse(itemSplit[0]));
            }
            Item temp = itemList[0];
            for (int i = 1; i < itemList.Count; i++)
            {
                if (itemList[i].itemCode >= 85000 && itemList[i].itemCode < 86000)
                {
                    itemList[0] = itemList[i];
                    itemList[i] = temp;
                    break;
                }
            }
            for (int i = 0; i < itemList.Count; i++)
            {
                Debug.LogError("sss  " + itemList[i].itemCode);
            }
            tw.TenGaChaInfo(itemList, int.Parse(dataSplit[2]), RewardID);
            UpDateTopContentData(itemList);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            GameObject.Find("TurntableWindow").GetComponent<TurntableWindow>().IsButtonFinish = false;
            switch (int.Parse(dataSplit[1]))
            {
                case 0:
                    UIManager.instance.OpenPromptWindow("奖励错误", PromptWindow.PromptType.Hint, null, null);

                    break;
                case 1:
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }


    private void Process_3401(string RecvString)
    {
        GameObject _obj = GameObject.Find("StrengthenMasterWindow");
        if (_obj != null)
        {
            _obj.GetComponent<StrengthenMasterWindow>().SerHeroInfo(RecvString);
        }
    }

    private void Process_3402(string RecvString)
    {
        GameObject _obj = GameObject.Find("StrengEquipWindow");
        if (_obj != null)
        {
            //_obj.GetComponent<StrengEquipWindow>().MasterUpgrade(RecvString);
        }
    }
    public void Process_5001(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        BetterList<Item> itemList = new BetterList<Item>();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] itemSplit = dataSplit[i].Split('$');
            itemList.Add(new Item(int.Parse(itemSplit[0]), int.Parse(itemSplit[1])));
        }
        TextTranslator.instance.SetBagItemList(itemList);
        if (TextTranslator.instance.isUpdateBag)
        {
            if (GameObject.Find("BagWindow") != null)
            {
                BagWindow bg = GameObject.Find("BagWindow").GetComponent<BagWindow>();
                bg.SetTab(1);
                TextTranslator.instance.isUpdateBag = false;
            }
        }


        //////////////////////紧急防卡死机制(以下)//////////////////////
        if (CharacterRecorder.instance.level == 5)
        {
            if (TextTranslator.instance.GetItemCountByID(70028) >= 40)
            {
                if (GameObject.Find("MainWindow") != null && !SceneTransformer.instance.CheckGuideIsFinish())
                {
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 15);
                }
            }
            else
            {
                if (CharacterRecorder.instance.ownedHeroList.size < 4 && !SceneTransformer.instance.CheckGuideIsFinish())
                {
                    PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 14);
                }
                else
                {
                    if (CharacterRecorder.instance.mapID > 1)
                    {
                        PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 18);
                    }
                    else
                    {
                        PlayerPrefs.SetInt(LuaDeliver.instance.GetGuideStateName(), 16);
                    }
                }
            }
        }
        //////////////////////紧急防卡死机制(以上)//////////////////////
    }


    int IndexCode = 0;
    public void Process_5002(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //CharacterRecorder.instance.BagItemCode = 0;
        if (dataSplit[0] == "1")
        {
            string[] dataSplit2 = dataSplit[1].Split('!');
            if (GameObject.Find("BuyPropsWindow") != null)
            {
                GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().UsePropsSucess();
            }
            List<Item> _itemList = new List<Item>();
            int BagItemCode = 0;
            string itemName = "";
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                int itemCode = int.Parse(dataSplit3[0]);
                if (IsAwardAreadyContains(_itemList, itemCode))
                {
                    _itemList[IndexCode] = new Item(itemCode, _itemList[IndexCode].itemCount + int.Parse(dataSplit3[1]));
                }
                else
                {
                    _itemList.Add(new Item(itemCode, int.Parse(dataSplit3[1])));
                }
                if (CharacterRecorder.instance.BagItemCode == 20002)//橙色饰品宝箱
                {
                    if (TextTranslator.instance.GetItemByItemCode(itemCode).itemGrade > 4)
                    {
                        itemName = TextTranslator.instance.GetItemNameByItemCode(itemCode);
                        SendProcess(string.Format("7002#{0};{1};{2};{3}", 8, CharacterRecorder.instance.characterName, itemName, TextTranslator.instance.GetItemByItemCode(itemCode).itemGrade));
                    }
                }
                else if (CharacterRecorder.instance.BagItemCode == 20003)//橙色人物饰品宝箱
                {
                    itemName = TextTranslator.instance.GetItemNameByItemCode(itemCode);
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 9, CharacterRecorder.instance.characterName, itemName, 0));
                }
                else if (CharacterRecorder.instance.BagItemCode == 20009 || CharacterRecorder.instance.BagItemCode == 20010 || CharacterRecorder.instance.BagItemCode == 20011)//boss宝箱
                {
                    if (TextTranslator.instance.GetItemByItemCode(itemCode).itemGrade > 5)
                    {
                        itemName = TextTranslator.instance.GetItemNameByItemCode(itemCode);
                        SendProcess(string.Format("7002#{0};{1};{2};{3}", 8, CharacterRecorder.instance.characterName, itemName, 0));
                    }
                }
                else if (itemCode > 60000 && itemCode < 70000)
                {
                    string heroName = TextTranslator.instance.GetItemNameByItemCode(itemCode);
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 9, CharacterRecorder.instance.characterName, heroName, 0));
                }
                //if (itemCode > 60000 && itemCode < 70000)
                //{
                //    Debug.LogError("-------------");
                //    string heroName = TextTranslator.instance.GetItemNameByItemCode(itemCode);
                //    SendProcess(string.Format("7002#{0};{1};{2};{3}", 9, CharacterRecorder.instance.characterName, heroName, 0));
                //}
                if (itemCode == 90013) //vip物品，呼叫9201取得钻石充值总数和vip等级
                {
                    SendProcess("9201#;");
                }
            }

            CharacterRecorder.instance.BagItemCode = 0;
            bool isonekey = false;
            if (GameObject.Find("GoodsItemObj") != null)
            {
                if (GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().isOneKey == true)
                {
                    isonekey = true;
                }
            }
            if (GameObject.Find("GrabResult") == null && isonekey == false)
            {
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, _itemList);
            }
            UpDateTopContentData(_itemList);
            if (GameObject.Find("BagWindow") != null)
            {
                GameObject bag = GameObject.Find("BagWindow");
                BagWindow bw = bag.GetComponent<BagWindow>();
                bw.UpDataBag();
            }
            //一键夺宝
            if (GameObject.Find("GrabResult") != null)
            {
                GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().OneKeyEvent();
            }
            else if (GameObject.Find("GoodsItemObj") != null)
            {
                if (GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().isOneKey)
                {
                    GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().OneKeyEvent();
                }
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("使用失败", PromptWindow.PromptType.Hint, null, null);
        }

    }

    bool IsAwardAreadyContains(List<Item> _itemList, int _code)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].itemCode == _code)
            {
                IndexCode = i;
                return true;
            }
        }
        return false;
    }

    public void Process_5003(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            if (GameObject.Find("TopContent") != null)
            {
                TopContent tc = GameObject.Find("TopContent").GetComponent<TopContent>();
                tc.Reset();
            }
            if (GameObject.Find("BagWindow") != null)
            {
                BagWindow bw = GameObject.Find("BagWindow").GetComponent<BagWindow>();
                //Item _item = TextTranslator.instance.GetItemByID(bw.curItemID);
                //if (dataSplit[3] == "0")
                //{
                //    TextTranslator.instance.bagItemList.Remove(_item);
                //}
                bw.UpDataBag();
            }
        }
        else
        {
            if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("出售失败", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }
    /*void OnMouseUp()
    {
        isShowErrorCodeTip = false;
    }*/
    public void Process_5004(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("RoleWindow") != null)
            {
                RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
                HeroLevelUpPart hlup = rw.HeroLevelUpPart.GetComponent<HeroLevelUpPart>();
                if (int.Parse(dataSplit[3]) > 0)
                {
                    hlup.IsLevelUp = true;
                }
                SendProcess("1005#" + dataSplit[2] + ";");
                hlup.UpdateExp(int.Parse(dataSplit[1]), int.Parse(dataSplit[3]), float.Parse(dataSplit[4]), float.Parse(dataSplit[5]));
            }

        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("等级不足无法使用", 11, false, PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("长官，这是什么啊？？", 11, false, PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("这不是经验药水哦", 11, false, PromptWindow.PromptType.Hint, null, null); break;
                case "3":
                    break;
                case "4": //UIManager.instance.OpenPromptWindow("英雄等级不能超过战队等级", 11, false, PromptWindow.PromptType.Hint, null, null);
                    break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_5015(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("RoleWindow") != null)
            {
                RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
                HeroLevelUpPart hlup = rw.HeroLevelUpPart.GetComponent<HeroLevelUpPart>();
                if (int.Parse(dataSplit[3]) > 0)
                {
                    hlup.IsLevelUp = true;
                }
                SendProcess("1005#" + dataSplit[2] + ";");
                if (dataSplit[1].Contains("$"))
                {
                    List<int> ExpItemID = new List<int>();
                    string[] dataSplit2 = dataSplit[1].Split('$');
                    for (int i = 0; i < dataSplit2.Length - 1; i++)
                    {
                        ExpItemID.Add(int.Parse(dataSplit2[i]));
                    }
                    hlup.UpdateExp(ExpItemID, int.Parse(dataSplit[3]), float.Parse(dataSplit[4]), float.Parse(dataSplit[5]));
                }
                else
                {
                    hlup.UpdateExp(int.Parse(dataSplit[1]), int.Parse(dataSplit[3]), float.Parse(dataSplit[4]), float.Parse(dataSplit[5]));
                }
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("角色唯一ID不对", 11, false, PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_5005(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<Item> list = new List<Item>();

        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] itemSplit = dataSplit[i].Split('$');
            if (itemSplit[0] == "0") //减包包
            {
                TextTranslator.instance.IsNeedUpdateItemInBag = true;
                TextTranslator.instance.SetItemCountReduceByID(int.Parse(itemSplit[1]), int.Parse(itemSplit[2]));
                //GameObject _BagWindowObj =  GameObject.Find("BagWindow");
                //if (_BagWindowObj != null)
                //{
                //    BagWindow bw = _BagWindowObj.GetComponent<BagWindow>();
                //    bw.UpDataBag();
                //}
                Debug.LogError("捡包包");
            }
            else if (itemSplit[0] == "1") //加包包
            {
                TextTranslator.instance.IsNeedUpdateItemInBag = true;
                TextTranslator.instance.SetItemCountAddByID(int.Parse(itemSplit[1]), int.Parse(itemSplit[2]));
                Item item = new Item(int.Parse(itemSplit[1]), int.Parse(itemSplit[2]));
                list.Add(item);
            }
        }
        //if (GameObject.Find("BagWindow") != null)
        //{
        //    GameObject bag = GameObject.Find("BagWindow");
        //    BagWindow bw = bag.GetComponent<BagWindow>();
        //    bw.UpDataBag();
        //    if (list.Count > 0)
        //    {
        //        UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
        //        GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, list);
        //    }
        //}
    }

    public void Process_5006(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.ghost = int.Parse(dataSplit[0]);
            foreach (var h in CharacterRecorder.instance.ownedHeroList)
            {
                if (h.characterRoleID == int.Parse(dataSplit[1]))
                {
                    h.level = int.Parse(dataSplit[3]);
                    h.exp = int.Parse(dataSplit[4]);
                    h.maxExp = int.Parse(dataSplit[5]);

                    if (GameObject.Find("RoleWindow") != null)
                    {
                        RoleWindow rw = GameObject.Find("RoleWindow").GetComponent<RoleWindow>();
                        CharacterRecorder.instance.ghost = int.Parse(dataSplit[2]);
                        //rw.LabelGhost.GetComponent<UILabel>().text = CharacterRecorder.instance.ghost.ToString();
                        //rw.SetHeroClick(-1);



                        GameObject go = GameObject.Instantiate(Resources.Load("Prefab/Effect/JueSeShengJi", typeof(GameObject)), PictureCreater.instance.ListRolePicture[0].RoleObject.transform.position, Quaternion.identity) as GameObject;
                        AudioEditer.instance.PlayOneShot("ui_levelup");

                        SendProcess("1005#" + dataSplit[1] + ";");
                    }
                    break;
                }
            }
        }
    }

    public void Process_5007(string RecvString)
    {
        CharacterRecorder.instance.ownedHeroList.Clear();
        NetworkHandler.instance.SendProcess("1004#0;");
        NetworkHandler.instance.SendProcess("1005#0;");
        NetworkHandler.instance.SendProcess("3001#0;");
        NetworkHandler.instance.SendProcess("1621#0;");
        //string[] dataSplit = RecvString.Split(';');
        //SendProcess("1004#" + dataSplit[0] + ";");
        //SendProcess("1005#" + dataSplit[0] + ";");
        //SendProcess("3001#" + dataSplit[0] + ";");
    }

    public void Process_5008(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("WayWindow") != null)
            {
                WayWindow ww = GameObject.Find("WayWindow").GetComponent<WayWindow>();
                ww.UpdateItemInfo();
            }
            if (GameObject.Find("BagWindow") != null)
            {
                BagWindow bw = GameObject.Find("BagWindow").GetComponent<BagWindow>();
                bw.UpDataBag();
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("此为无法合成的物品", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("背包无足够物品", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[2] == "2")
            {
                UIManager.instance.OpenPromptWindow("合成失败", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_5009(string RecvString)
    {
        if (GameObject.Find("WayWindow") != null)
        {
            WayWindow ww = GameObject.Find("WayWindow").GetComponent<WayWindow>();
            ww.SetOpenFuben(RecvString);
        }
    }
    public void Process_5010(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            GameObject _HeroLevelUpPart = GameObject.Find("HeroLevelUpPart");
            if (_HeroLevelUpPart != null)
            {
                _HeroLevelUpPart.GetComponent<HeroLevelUpPart>().UpDataItemStateAfterBuy();
            }
            //GameObject _GrabItemWindow = GameObject.Find("GrabItemWindow");
            //if (_GrabItemWindow != null)
            //{
            //    _GrabItemWindow.GetComponent<GrabWindow>().ShoppingSuccess(1);
            //    if (_GrabItemWindow.GetComponent<GrabWindow>().ProtectFixedTime == 1)
            //    {
            //        GameObject.Find("NowNum").GetComponent<UILabel>().text = TextTranslator.instance.GetItemCountByID(10901).ToString() + "个";
            //    }
            //    else
            //    {
            //        GameObject.Find("NowNum").GetComponent<UILabel>().text = TextTranslator.instance.GetItemCountByID(10902).ToString() + "个";
            //    }
            //}

            //if (GameObject.Find("BuyPropsWindow") != null)
            //{
            //    GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().BuyPropsSucess();
            //}
            UIManager.instance.OpenPromptWindow("购买成功", PromptWindow.PromptType.Hint, null, null);
            SendProcess("5012#0;");
            if (GameObject.Find("TaskWindow") != null)
            {
                NetworkHandler.instance.SendProcess("1201#1");
                NetworkHandler.instance.SendProcess("1201#2");
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("只能用钻石买", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
            if (GameObject.Find("GrabItemWindow") != null)
            {
                GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().ShoppingSuccess(2);
            }
        }
    }
    public void Process_5011(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject _HeroLevelUpPart = GameObject.Find("HeroBreakUpPart");
            if (_HeroLevelUpPart != null)
            {
                _HeroLevelUpPart.GetComponent<HeroBreakUpPart>().UpDataItemStateAfterExChange();
            }
            if (GameObject.Find("NuclearWeaponWindow") != null)
            {
                GameObject.Find("NuclearWeaponWindow").GetComponent<NuclearWeaponWindow>().UpDataItemStateAfterExChange();
            }
        }
        else
        {
            //UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[0], PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_5012(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<DiamondShopItemData> _shopItemList = new List<DiamondShopItemData>();
        PlayerPrefs.SetInt("ShopBuy", 0);
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] itemSplit = dataSplit[i].Split('$');
            _shopItemList.Add(new DiamondShopItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), int.Parse(itemSplit[2])));
            if (int.Parse(itemSplit[0]) > 31 && int.Parse(itemSplit[0]) < 48)
            {
                if (int.Parse(itemSplit[2]) == 1)
                {
                    PlayerPrefs.SetInt("ShopBuy", 1);
                }
            }
        }

        if (dataSplit.Length == 2)
        {
            //int ItemID = 0;
            //Dictionary<int, ShopCenter>.ValueCollection valueColl = TextTranslator.instance.ShopCenterDic.Values;
            //foreach (ShopCenter _Shop in valueColl)//获取当前窗口位置的物品id;
            //{
            //    if (_Shop.WindowID == _shopItemList[0].index)
            //    {
            //        ItemID = _Shop.ItemID;//物品id
            //        break;
            //    }
            //}

            //if (ItemID == 10602)//体力特殊窗口
            //{
            //    if (GameObject.Find("BuyEnergyWindow") == null)
            //    {
            //        UIManager.instance.OpenPanel("BuyEnergyWindow", false);
            //        string[] trcSplit = dataSplit[0].Split('$');
            //        GameObject.Find("BuyEnergyWindow").GetComponent<BuyEnergyWindow>().SetSpecialInfo(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]), int.Parse(trcSplit[2]));
            //    }
            //    else
            //    {
            //        string[] trcSplit = dataSplit[0].Split('$');
            //        GameObject.Find("BuyEnergyWindow").GetComponent<BuyEnergyWindow>().SetSpecialInfo(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]), int.Parse(trcSplit[2]));
            //    }
            //}
            //else
            //{
            //    if (GameObject.Find("EmployPropsWindow") == null)
            //    {
            //        UIManager.instance.OpenSinglePanel("EmployPropsWindow", false);
            //        string[] trcSplit = dataSplit[0].Split('$');
            //        GameObject.Find("EmployPropsWindow").GetComponent<EmployPropsWindow>().SetSpecialInfo(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]), int.Parse(trcSplit[2]));
            //    }
            //    else
            //    {
            //        string[] trcSplit = dataSplit[0].Split('$');
            //        GameObject.Find("EmployPropsWindow").GetComponent<EmployPropsWindow>().SetSpecialInfo(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]), int.Parse(trcSplit[2]));
            //    }
            //}

            if (GameObject.Find("LegionTeamWindow") != null)   //复活石，军团战用   11.3
            {
                int ItemID = 0;
                int Sellprice = 0;
                Dictionary<int, ShopCenter>.ValueCollection valueColl = TextTranslator.instance.ShopCenterDic.Values;
                foreach (ShopCenter _Shop in valueColl)//获取当前窗口位置的物品id;
                {
                    if (_Shop.WindowID == _shopItemList[0].index)
                    {
                        ItemID = _Shop.ItemID;//物品id
                        break;
                    }
                }

                Dictionary<int, ShopCenterPeculiar>.ValueCollection valueColl2 = TextTranslator.instance.ShopCenterPeculiarDic.Values;
                foreach (ShopCenterPeculiar _Shop2 in valueColl2)//获取当前价格段位
                {
                    if (_Shop2.PeculiarID == _shopItemList[0].index)
                    {
                        if (_Shop2.BuyCount >= _shopItemList[0].buyCount + 1)
                        {
                            Sellprice = _Shop2.PriceDiamond;
                            break;
                        }
                    }
                }

                if (ItemID == 10801 && _shopItemList[0].canBuyCount > 0) //复活石，军团战用
                {
                    UIManager.instance.OpenSinglePanel("UsePropsWindow", false);
                    GameObject.Find("UsePropsWindow").GetComponent<UsePropsWindow>().SetPropsInfo(ItemID, _shopItemList[0].buyCount, _shopItemList[0].canBuyCount, _shopItemList[0].index, Sellprice);
                }
                else if (_shopItemList[0].canBuyCount == 0)
                {
                    UIManager.instance.OpenPromptWindow("今天已达最高购买次数，明天再来哦!", PromptWindow.PromptType.Hint, null, null);
                }
            }
            else if (GameObject.Find("EmployPropsWindow") == null)
            {
                UIManager.instance.OpenSinglePanel("EmployPropsWindow", false);
                string[] trcSplit = dataSplit[0].Split('$');
                GameObject.Find("EmployPropsWindow").GetComponent<EmployPropsWindow>().SetSpecialInfo(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]), int.Parse(trcSplit[2]));
            }
            else
            {
                string[] trcSplit = dataSplit[0].Split('$');
                GameObject.Find("EmployPropsWindow").GetComponent<EmployPropsWindow>().SetSpecialInfo(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]), int.Parse(trcSplit[2]));
            }
        }
        else
        {
            if (GameObject.Find("DiamondShopWindow") != null)
            {
                DiamondShopWindow rw = GameObject.Find("DiamondShopWindow").GetComponent<DiamondShopWindow>();
                rw.SetShopWindow(_shopItemList);
            }
        }
    }

    public void Process_5013(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            GameObject _HeroLevelUpPart = GameObject.Find("HeroLevelUpPart");
            if (_HeroLevelUpPart != null)
            {
                _HeroLevelUpPart.GetComponent<HeroLevelUpPart>().UpDataItemStateAfterBuy();
            }
            GameObject _GrabItemWindow = GameObject.Find("GrabItemWindow");
            if (_GrabItemWindow != null)
            {
                _GrabItemWindow.GetComponent<GrabWindow>().ShoppingSuccess(1);
                if (_GrabItemWindow.GetComponent<GrabWindow>().ProtectFixedTime == 1)
                {
                    GameObject.Find("NowNum").GetComponent<UILabel>().text = TextTranslator.instance.GetItemCountByID(10901).ToString() + "个";
                }
                else
                {
                    GameObject.Find("NowNum").GetComponent<UILabel>().text = TextTranslator.instance.GetItemCountByID(10902).ToString() + "个";
                }
            }

            if (GameObject.Find("BuyPropsWindow") != null)
            {
                GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().BuyPropsSucess();
            }
            if (GameObject.Find("VIPShopWindow") != null)
            {
                GameObject.Find("VIPShopWindow").transform.Find("TopContent").GetComponent<TopContent>().Reset();
                GameObject.Find("VIPShopWindow").GetComponent<VipShopWindow>().RefreshItem();
            }


            if (GameObject.Find("FirstRechargeWindow") != null)
            {
                //GameObject.Find("FirstRechargeWindow").GetComponent<FirstRechargeWindow>().SetVipOneState();
                NetworkHandler.instance.SendProcess("9201#;");
                GameObject.Find("FirstRechargeWindow").GetComponent<FirstRechargeWindow>().RefreshItem();
                //NetworkHandler.instance.SendProcess("9201#;");
            }

            if (GameObject.Find("VipRechargeWindow") != null)
            {
                //GameObject.Find("FirstRechargeWindow").GetComponent<FirstRechargeWindow>().SetVipOneState();
                NetworkHandler.instance.SendProcess("9201#;");
                GameObject.Find("VipRechargeWindow").GetComponent<VIPRechargeWindow>().RefreshItem();
                //NetworkHandler.instance.SendProcess("9201#;");
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("只能用钻石买", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
            if (GameObject.Find("GrabItemWindow") != null)
            {
                GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().ShoppingSuccess(2);
            }
        }
    }
    public void Process_5014(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            string[] dataSplit2 = dataSplit[1].Split('!');
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            List<Item> _itemList = new List<Item>();
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                int itemCode = int.Parse(dataSplit3[0]);
                if (IsAwardAreadyContains(_itemList, itemCode))
                {
                    _itemList[IndexCode] = new Item(itemCode, _itemList[IndexCode].itemCount + int.Parse(dataSplit3[1]));
                }
                else
                {
                    _itemList.Add(new Item(itemCode, int.Parse(dataSplit3[1])));
                }
            }
            string[] itemStr = dataSplit2[0].Split('$');
            if (int.Parse(itemStr[0]) != 10901 && int.Parse(itemStr[0]) != 10902 && GameObject.Find("GrabResult") == null)
            {
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, _itemList);
            }
            else
            {
                if (int.Parse(itemStr[0]) == 10901)
                {
                    NetworkHandler.instance.SendProcess("1406#1;");
                }
                else if (int.Parse(itemStr[0]) == 10902)
                {
                    NetworkHandler.instance.SendProcess("1406#10;");
                }
            }
            //一键夺宝
            if (GameObject.Find("GrabResult") != null)
            {
                GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().OneKeyEvent();
            }
            if (GameObject.Find("GrabResult") == null)
            {
                UpDateTopContentData(_itemList);
            }

            if (GameObject.Find("TaskWindow") != null)
            {
                GameObject.Find("TaskWindow").GetComponent<TaskWindow>().SetLevelUpRefresh();
            }
            GameObject _mapUiWindow = GameObject.Find("MapUiWindow");
            if (_mapUiWindow != null)
            {
                _mapUiWindow.GetComponent<MapUiWindow>().mTopContent.GetComponent<TopContent>().Reset();
            }

        }

        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("窗口ID错误", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("物品数量不足", PromptWindow.PromptType.Hint, null, null); break;
                default: UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_5102(string RecvString)
    {
        /* string[] dataSplit = RecvString.Split(';');
         if (GameObject.Find("RankShopWindow") != null)
         {
             RankShopWindow rw = GameObject.Find("RankShopWindow").GetComponent<RankShopWindow>();
             for (int i = 1; i < dataSplit.Length - 1; i++)
             {
                 string[] itemSplit = dataSplit[i].Split('$');
                 Debug.Log("AAAA");
                 rw.SetShopItem(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), int.Parse(itemSplit[2]), int.Parse(itemSplit[3]), int.Parse(itemSplit[4]), int.Parse(itemSplit[5]), int.Parse(itemSplit[6]));
             }
         }*/
        string[] dataSplit = RecvString.Split(';');
        List<ShopItemData> _shopItemList = new List<ShopItemData>();
        for (int i = 3; i < dataSplit.Length - 1; i++)
        {
            string[] itemSplit = dataSplit[i].Split('$');
            //Debug.Log("AAAA" + itemSplit.Length);
            _shopItemList.Add(new ShopItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), int.Parse(itemSplit[2]), int.Parse(itemSplit[3]), int.Parse(itemSplit[4]), int.Parse(itemSplit[5])));
        }
        if (GameObject.Find("RankShopWindow") != null)
        {
            RankShopWindow rw = GameObject.Find("RankShopWindow").GetComponent<RankShopWindow>();
            rw.SetShopWindow(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), _shopItemList, int.Parse(dataSplit[2]));
        }
        else if (GameObject.Find("SecretShopWindow") != null)
        {
            SecretShopWindow rw = GameObject.Find("SecretShopWindow").GetComponent<SecretShopWindow>();
            rw.SetShopWindow(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), _shopItemList, int.Parse(dataSplit[2]));
        }
    }

    public void Process_5103(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[4]);
            CharacterRecorder.instance.HonerValue = int.Parse(dataSplit[5]);
            CharacterRecorder.instance.GoldBar = int.Parse(dataSplit[8]);
            if (GameObject.Find("RankShopWindow") != null)
            {
                RankShopWindow rw = GameObject.Find("RankShopWindow").GetComponent<RankShopWindow>();
                Debug.Log("AAAA");
                for (int i = 0; i < rw.ListRankShopItem.Count; i++)
                {
                    if (rw.ListRankShopItem[i].name == dataSplit[2])
                    {
                        rw.ListRankShopItem[i].GetComponent<RankShopItem>().TextureMask.SetActive(true);
                        rw.ListRankShopItem[i].GetComponent<RankShopItem>().SetItemCount();
                        rw.ListRankShopItem[i].GetComponent<RankShopItem>().GreenPoint.SetActive(false);
                        rw.SetMoneyNum(int.Parse(dataSplit[1]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]), int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]), int.Parse(dataSplit[8]), int.Parse(dataSplit[9]));
                        break;
                    }
                }
                //rw.ListRankShopItem[int.Parse(dataSplit[4]) - 1].GetComponent<RankShopItem>().TextureMask.mainTexture = Resources.Load("Game/sq", typeof(Texture)) as Texture;

            }
            else if (GameObject.Find("SecretShopWindow") != null)
            {
                SecretShopWindow ssw = GameObject.Find("SecretShopWindow").GetComponent<SecretShopWindow>();
                for (int i = 0; i < ssw.ListRankShopItem.Count; i++)
                {
                    if (ssw.ListRankShopItem[i].name == dataSplit[2])
                    {
                        ssw.ListRankShopItem[i].GetComponent<RankShopItem>().TextureMask.SetActive(true);
                        break;
                    }
                }
                ssw.SetGoldBarReset();
                //ssw.ListRankShopItem[int.Parse(dataSplit[2]) - 1].GetComponent<RankShopItem>().TextureMask.SetActive(true);
            }
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("商品窗口ID错误", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("荣誉币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("试炼币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "5": UIManager.instance.OpenPromptWindow("军团币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "6": UIManager.instance.OpenPromptWindow("金条不足", PromptWindow.PromptType.Hint, null, null); break;
                case "7": UIManager.instance.OpenPromptWindow("王者币不足", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_5105(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            NetworkHandler.instance.SendProcess(string.Format("5102#{0};", dataSplit[1]));//RankShopWindow.curSelectShopType
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            // CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("没有此商店", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_5201(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            TextTranslator.instance.isUpdateBag = true;
            CharacterRecorder.instance.SetMoney(int.Parse(dataSplit[5]));
            CharacterRecorder.instance.SetLunaGem(int.Parse(dataSplit[4]));
            AudioEditer.instance.PlayOneShot("ui_lottery");
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();

            }
            if (GameObject.Find("GaChaGetWindow") == null || CharacterRecorder.instance.isRedHeroWindowFirst == true)
            {
                UIManager.instance.OpenPanel("GaChaGetWindow", false);
                if (dataSplit[1] == "6" || dataSplit[1] == "7")
                {
                    GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>().MustRoleNum.gameObject.SetActive(true);
                    SendProcess("5204#;");
                }
            }
            if (GameObject.Find("GaChaGetWindow") != null)
            {
                //bool HaveHero = true;
                if (dataSplit[1] == "6" || dataSplit[1] == "7")
                {
                    SendProcess("5204#;");
                }
                GaChaGetWindow GCCW = GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>();
                GCCW.Reset();
                string[] secSplit = dataSplit[3].Split('!');
                string[] triSplit = secSplit[0].Split('$');
                //if (int.Parse(triSplit[0]) > 60000 && int.Parse(triSplit[0]) < 70000)
                //{
                //    foreach (var item in CharacterRecorder.instance.ownedHeroList)
                //    {

                //        if (item.cardID == int.Parse(triSplit[0]))
                //        {
                //            Debug.LogError(item.characterRoleID);
                //            int heroRarity = TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(triSplit[0])).heroRarity;
                //            if (heroRarity == 2)
                //            {
                //                triSplit[1] = "16";
                //            }
                //            else if (heroRarity == 3)
                //            {
                //                triSplit[1] = "32";
                //            }
                //            else if (heroRarity == 4)
                //            {
                //                triSplit[1] = "64";
                //            }
                //            HaveHero = false;
                //            triSplit[0] = (int.Parse(triSplit[0]) + 10000).ToString();
                //        }
                //    }
                //}
                Item _item = new Item(int.Parse(triSplit[0]), int.Parse(triSplit[1]));
                GCCW.SetSoloItem(_item, int.Parse(triSplit[2]), int.Parse(dataSplit[1]));
                if (int.Parse(triSplit[0]) > 60000 && int.Parse(triSplit[0]) < 70000 && TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(triSplit[0])).heroRarity >= 4)
                {
                    Debug.LogError("-------------");
                    string heroName = TextTranslator.instance.GetItemNameByItemCode(int.Parse(triSplit[0]));
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 4, CharacterRecorder.instance.characterName, heroName, TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(triSplit[0])).heroRarity));
                }
            }

        }
        else
        {
            switch (dataSplit[1])
            {
                case "1": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("还在CD时间", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
            if (GameObject.Find("GaChaGetWindow") != null)
            {
                GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>().BoxCollderSize();
            }
        }
    }
    public void Process_5202(string RecvString)
    {

        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("GaChaGetWindow") == null)
            {
                UIManager.instance.OpenPanel("GaChaGetWindow", false);//UIManager.instance.OpenPanel("GaChaGetWindow", false);
            }
            GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>().Reset();
            CharacterRecorder.instance.TenCaChaNumber = int.Parse(dataSplit[6]);
            StartCoroutine(CreatItem(dataSplit[3], dataSplit[1]));
            AudioEditer.instance.PlayOneShot("ui_lottery");
            TextTranslator.instance.isUpdateBag = true;
            CharacterRecorder.instance.SetMoney(int.Parse(dataSplit[5]));
            CharacterRecorder.instance.SetLunaGem(int.Parse(dataSplit[4]));

            GameObject top = GameObject.Find("TopContent");
            if (top != null)
            {
                top.GetComponent<TopContent>().Reset();
            }

        }
        else
        {
            switch (dataSplit[1])
            {
                case "1": UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("还在CD时间", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
            if (GameObject.Find("GaChaGetWindow") != null)
            {
                GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>().BoxCollderSize();
            }
        }
    }
    IEnumerator CreatItem(string dataSplit, string type)
    {
        string[] secSplit = dataSplit.Split('!');
        for (int i = 0; i < secSplit.Length - 1; i++)
        {
            string[] triSplit = secSplit[i].Split('$');
            //bool HaveHero=true;
            //if (int.Parse(triSplit[0]) > 60000 && int.Parse(triSplit[0]) < 70000)
            //{
            //    foreach (var item in CharacterRecorder.instance.ownedHeroList)
            //    {

            //        if (item.cardID == int.Parse(triSplit[0]))
            //        {
            //            Debug.LogError(item.characterRoleID);                      
            //            int heroRarity = TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(triSplit[0])).heroRarity;
            //            if(heroRarity==2)
            //            {
            //                triSplit[1]="16";
            //            }
            //            else if(heroRarity==3)
            //            {
            //                triSplit[1]="32";
            //            }
            //            else if(heroRarity==4)
            //            {
            //                triSplit[1]="64";
            //            }
            //            HaveHero=false;
            //            triSplit[0] = (int.Parse(triSplit[0]) + 10000).ToString();
            //        }
            //    }
            //}
            if (GameObject.Find("GaChaGetWindow") != null)
            {
                GaChaGetWindow GCGW = GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>();
                //GCGW.TryAgainButton.GetComponent<BoxCollider>().size = Vector3.zero;
                Item mitem = new Item(int.Parse(triSplit[0]), int.Parse(triSplit[1]));
                GCGW.SetTenItem(mitem, int.Parse(triSplit[2]), int.Parse(type), i);
                yield return new WaitForSeconds(0.1f);
            }
            if (int.Parse(triSplit[0]) > 60000 && int.Parse(triSplit[0]) < 70000 && TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(triSplit[0])).heroRarity >= 4)
            {
                StartCoroutine(SendItem(int.Parse(triSplit[0])));
                //string heroName = TextTranslator.instance.GetItemNameByItemCode(int.Parse(triSplit[0]));
                //SendProcess(string.Format("7002#{0};{1};{2};{3}",4,CharacterRecorder.instance.characterName, heroName,0));
            }
        }
        if (GameObject.Find("GaChaGetWindow") != null)
        {
            yield return new WaitForSeconds(1f);
            GaChaGetWindow GCGW = GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>();

            //GCGW.TryAgainButton.GetComponent<BoxCollider>().size = new Vector3(191, 78, 0);
        }
    }

    IEnumerator SendItem(int itemCode)
    {
        yield return new WaitForSeconds(3f);
        string heroName = TextTranslator.instance.GetItemNameByItemCode(itemCode);
        SendProcess(string.Format("7002#{0};{1};{2};{3}", 4, CharacterRecorder.instance.characterName, heroName, 0));
    }

    public void Process_5204(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.GachaOnce = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.GachaMore = int.Parse(dataSplit[1]);
        CharacterRecorder.instance.GachaMoreTime = int.Parse(dataSplit[2]);
        CharacterRecorder.instance.GachaLotteryNum = int.Parse(dataSplit[4]);
        CharacterRecorder.instance.IsOpen = true;
        CharacterRecorder.instance.IsOpeGacha = true;
        if (GameObject.Find("GachaWindow") != null)
        {
            //Debug.LogError("1111");
            GachaWindow ga = GameObject.Find("GachaWindow").GetComponent<GachaWindow>();
            ga.SetInfo(int.Parse(dataSplit[2]), int.Parse(dataSplit[0]), int.Parse(dataSplit[3]), int.Parse(dataSplit[1]), int.Parse(dataSplit[4]));
        }
        if (GameObject.Find("GaChaGetWindow") != null)
        {
            GameObject.Find("GaChaGetWindow").GetComponent<GaChaGetWindow>().SetMustRoleNum();
        }
        if (GameObject.Find("MainWindow") != null)
        {
            // GameObject.Find("MainWindow").GetComponent<MainWindow>().GaChaRedPont();
            CharacterRecorder.instance.GaChaRedPont();
        }
    }



    public void Process_5301(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("SuperArmsStoreWindow") != null)
        {
            GameObject.Find("SuperArmsStoreWindow").GetComponent<SuperArmsStoreWindow>().GetarmsItemInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
        }
    }



    public void Process_5302(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("SuperArmsStoreWindow") != null)
            {
                GameObject.Find("SuperArmsStoreWindow").GetComponent<SuperArmsStoreWindow>().GetarmsItemInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
            }
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[5]);

            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }

            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[6].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1":
                    UIManager.instance.OpenPromptWindow("钻石不足！", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }

    public void Process_5303(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.HoldJunhuoTime = int.Parse(dataSplit[0]);
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().SureJunhuokuIsOpen();
        }
    }

    public void Process_5401(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            int num = int.Parse(dataSplit[1]) - CharacterRecorder.instance.GoldBar;
            CharacterRecorder.instance.GoldBar = int.Parse(dataSplit[1]);
            GameObject go = GameObject.Find("TechnologyfurnaceWindow");
            if (go != null)
            {
                go.GetComponent<TechnologyfurnaceWindow>().GetResultItemExchange();
            }

            List<Item> itemlist = new List<Item>();
            Item _item = new Item(90006, num);
            itemlist.Add(_item);
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
        }
        else
        {
            UIManager.instance.OpenPromptWindow("融合失败！", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_6001(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.PvpChallengeNum = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.RankNumber = int.Parse(dataSplit[1]);
        CharacterRecorder.instance.HonerValue = int.Parse(dataSplit[2]);
        CharacterRecorder.instance.PvpPoint = int.Parse(dataSplit[4]);//积分
        CharacterRecorder.instance.MaxRankNumber = int.Parse(dataSplit[5]);//最大排名
        CharacterRecorder.instance.PvpRefreshTime = int.Parse(dataSplit[6]);//竞技场刷新时间

        if (GameObject.Find("PVPWindow") != null)
        {
            PVPWindow rw = GameObject.Find("PVPWindow").GetComponent<PVPWindow>();
            rw.SetInfo(dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], dataSplit[6]);
            //rw.SetLeftTime(int.Parse(dataSplit[6]));
            //rw.Point = int.Parse(dataSplit[4]);
        }
        GameObject cw = GameObject.Find("ChallengeWindow");
        if (cw != null)
        {
            cw.GetComponent<ChallengeWindow>().SetJingJiChangRedPoint_6001(dataSplit[0]);
        }
        //else if (GameObject.Find("GetRankingReward") != null)
        //{
        //    GetRankingReward Re = GameObject.Find("GetRankingReward").GetComponent<GetRankingReward>();
        //    Re.HighRanking.text = dataSplit[5];
        //}
        //else if (GameObject.Find("IntegrationWindow") != null)
        //{
        //    //GameObject.Find("IntegrationWindow/All/SpriteBG/SpriteContent/TopBG/LabelIntegration/LabelNum").GetComponent<UILabel>().text = dataSplit[4];
        //    IntegrationWindow IW = GameObject.Find("IntegrationWindow").GetComponent<IntegrationWindow>();
        //    IW.Integration.GetComponent<UILabel>().text = dataSplit[4];
        //}
        //else if (GameObject.Find("ChallengeWindow") != null)
        //{
        //    ChallengeWindow Cha = GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>();
        //    Cha.Point = int.Parse(dataSplit[4]);
        //}
        //else
        //{
        //    Debug.Log("未找到PVPWindow");
        //}
    }

    public void Process_6002(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<PVPItemData> _PVPItemDataList = new List<PVPItemData>();
        for (int i = 0; i < dataSplit.Length - 2; i++)
        {
            string[] SecSplit = dataSplit[i].Split('$');
            _PVPItemDataList.Add(new PVPItemData(int.Parse(SecSplit[0]), int.Parse(SecSplit[1]), SecSplit[2], int.Parse(SecSplit[3]), int.Parse(SecSplit[4]), int.Parse(SecSplit[5]), SecSplit[6], int.Parse(SecSplit[7])));
        }
        if (GameObject.Find("PVPWindow") != null)
        {
            PVPWindow rw = GameObject.Find("PVPWindow").GetComponent<PVPWindow>();
            rw.SetEnemyInfo(_PVPItemDataList);
            rw.RefreshNum = int.Parse(dataSplit[dataSplit.Length - 2]) + 1;
        }
        else
        {
            Debug.Log("未找到PVPWindow");
        }
        if (GameObject.Find("ConquerListWindow") != null)
        {
            GameObject.Find("ConquerListWindow").GetComponent<ConquerListWindow>().SetInfo(_PVPItemDataList);
        }


        if (CharacterRecorder.instance.PVPComeNum == 0)
        {
            CharacterRecorder.instance.PVPItemList.Clear();
            for (int i = 0; i < dataSplit.Length - 2; i++)
            {
                string[] SecSplit = dataSplit[i].Split('$');
                CharacterRecorder.instance.PVPItemList.Add(new PVPItemData(int.Parse(SecSplit[0]), int.Parse(SecSplit[1]), SecSplit[2], int.Parse(SecSplit[3]), int.Parse(SecSplit[4]), int.Parse(SecSplit[5]), SecSplit[6], int.Parse(SecSplit[7])));
            }
        }
    }

    public void Process_6003(string RecvString)
    {
        Debug.Log("待接数据");
        UIManager.instance.OpenPanel("LoadingWindow", true);
        PictureCreater.instance.StartPVP(RecvString);
    }

    public void Process_6004(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("RankListWindow") != null)
        {
            RankListWindow rw = GameObject.Find("RankListWindow").GetComponent<RankListWindow>();
            CharacterRecorder.instance.RankNumber = 0;
            rw.ShowMyRank(int.Parse(dataSplit[0]));
        }
        BetterList<ActiveAwardItemData> mMyList = new BetterList<ActiveAwardItemData>();
        for (int i = 1; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            if (dataSplit[0] == "3")//战力排行 活动
            {
                mMyList.Add(new ActiveAwardItemData(int.Parse(secSplit[0]), secSplit[2], int.Parse(secSplit[4]), int.Parse(secSplit[5])));
                if (secSplit[0] == "1")
                {
                    CharacterRecorder.instance.FirstPowerName = secSplit[2];//战力第一名玩家
                }
            }
            else if (dataSplit[0] == "1")
            {
                if (secSplit[0] == "1")
                {
                    CharacterRecorder.instance.FirstPvpName = secSplit[2];//pvp第一名玩家
                }
            }
            if (secSplit[2] == CharacterRecorder.instance.characterName)
            {
                CharacterRecorder.instance.RankNumber = int.Parse(secSplit[0]);
            }
            if (GameObject.Find("PVPListWindow") != null)
            {
                PVPListWindow pw = GameObject.Find("PVPListWindow").GetComponent<PVPListWindow>();
                pw.SetListItem(int.Parse(secSplit[0]), int.Parse(secSplit[4]), secSplit[2], int.Parse(secSplit[1]));
            }
            else if (GameObject.Find("RankListWindow") != null)
            {
                RankListWindow rw = GameObject.Find("RankListWindow").GetComponent<RankListWindow>();
                rw.CreatItem(dataSplit[0], secSplit[0], secSplit[1], secSplit[2], secSplit[3], secSplit[4], secSplit[5], secSplit[6], secSplit[7], secSplit[8]);
                //Debug.LogError(secSplit[1]);
                if (secSplit[2] == CharacterRecorder.instance.characterName)
                {
                    CharacterRecorder.instance.RankNumber = int.Parse(secSplit[0]);
                    rw.ShowMyRank(int.Parse(dataSplit[0]));
                }

                if (i == 1)
                {
                    rw.SetFirstInfo(secSplit[0], secSplit[1], secSplit[2], secSplit[3], secSplit[4], secSplit[5], secSplit[6], secSplit[7], secSplit[8]);
                }
            }
            else if (GameObject.Find("FightListWindow") != null)
            {
                if (secSplit[0] == "1" || secSplit[0] == "2" || secSplit[0] == "3")
                {
                    FightListWindow flw = GameObject.Find("FightListWindow").GetComponent<FightListWindow>();
                    flw.SetInfo(dataSplit[0], secSplit[0], secSplit[1], secSplit[2], secSplit[3], secSplit[4], secSplit[5], secSplit[6], secSplit[7]);
                }
            }
            else if (GameObject.Find("EmploymentWindow") != null)
            {
                EmploymentWindow ew = GameObject.Find("EmploymentWindow").GetComponent<EmploymentWindow>();
                if (secSplit[2] != CharacterRecorder.instance.characterName)
                {
                    ew.SetInfo(dataSplit[0], secSplit[0], secSplit[1], secSplit[2], secSplit[3], secSplit[4], secSplit[5], secSplit[6], secSplit[7], secSplit[8]);
                }
            }
        }

        if (GameObject.Find("ActiveAwardWindow") != null)
        {
            GameObject.Find("ActiveAwardWindow").GetComponent<ActiveAwardWindow>().GetItemListData(mMyList);
        }
    }

    public void Process_6005(string RecvString)
    {
        Debug.Log("待接数据");
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.IsOpen = true;
        if (dataSplit[0] == "1")
        {
            AudioEditer.instance.PlayLoop("Win");
            UIManager.instance.OpenPanel("ResultPVPWindow", false);
            ResultPVPWindow rw = GameObject.Find("ResultPVPWindow").GetComponent<ResultPVPWindow>();
            if (dataSplit[1] == "1")
            {
                rw.Init(true);
            }
            else
            {
                rw.Init(false);
            }
        }
    }

    public void Process_6006(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "6")
        {
            if (GameObject.Find("PVPWindow") != null)
            {
                PVPWindow rw = GameObject.Find("PVPWindow").GetComponent<PVPWindow>();
                rw.mTeamPosition.Clear();
                string[] dataSplit1 = dataSplit[1].Split('!');

                for (int i = 0; i < dataSplit1.Length - 1; i++)
                {
                    string[] secSplit = dataSplit1[i].Split('$');
                    TeamPosition mPosition = new TeamPosition();
                    mPosition._CharacterID = int.Parse(secSplit[0]);
                    mPosition._CharacterPosition = int.Parse(secSplit[1]);
                    rw.mTeamPosition.Add(mPosition);
                    //rw.SetTeamInfo(i, int.Parse(secSplit[0]), int.Parse(secSplit[1]));
                }
                rw.SetTeamInfo(rw.mTeamPosition);
            }
            PictureCreater.instance.PVPPosition = dataSplit[1];
        }
        else if (dataSplit[0] == "1")
        {
            //if (GameObject.Find("MainWindow") != null)
            //{
            //    MainWindow mw = GameObject.Find("MainWindow").GetComponent<MainWindow>();
            //    mw.PositionCount = 0;
            //    mw.SetTeamInfo();
            //}
            PictureCreater.instance.PVEPosition = dataSplit[1];
        }
        else if (dataSplit[0] == "2")
        {
            PictureCreater.instance.WoodPosition = dataSplit[1];
        }
        else if (dataSplit[0] == "3")
        {
            PictureCreater.instance.LegionPosition = dataSplit[1];
        }
        else if (dataSplit[0] == "4")
        {
            PictureCreater.instance.InstancePosition = dataSplit[1];
        }
    }

    public void Process_6007(string RecvSrting)
    {
        string[] dataSplit = RecvSrting.Split(';');
        if (dataSplit[0] == "1")
        {
            if (dataSplit[1] == "6")
            {
                if (GameObject.Find("PVPWindow") != null)
                {
                    PVPWindow rw = GameObject.Find("PVPWindow").GetComponent<PVPWindow>();
                    rw.mTeamPosition.Clear();
                    string[] dataSplit1 = dataSplit[2].Split('!');

                    for (int i = 0; i < dataSplit1.Length - 1; i++)
                    {
                        string[] secSplit = dataSplit1[i].Split('$');
                        TeamPosition mPosition = new TeamPosition();
                        mPosition._CharacterID = int.Parse(secSplit[0]);
                        mPosition._CharacterPosition = int.Parse(secSplit[1]);
                        rw.mTeamPosition.Add(mPosition);
                    }
                    rw.SetTeamInfo(rw.mTeamPosition);
                }
                else
                {
                    Debug.Log("未找到PVPWindow");
                }
                PictureCreater.instance.PVPPosition = dataSplit[2];
            }
            else if (dataSplit[1] == "1")
            {
                PictureCreater.instance.PVEPosition = dataSplit[2];
            }
            else if (dataSplit[1] == "2")
            {
                PictureCreater.instance.WoodPosition = dataSplit[2];
            }
            else if (dataSplit[0] == "3")
            {
                PictureCreater.instance.LegionPosition = dataSplit[2];
            }
            else if (dataSplit[0] == "4")
            {
                PictureCreater.instance.InstancePosition = dataSplit[2];
            }
        }
        else if (dataSplit[0] == "0")
        {
            Debug.Log("编队失败");
        }
    }
    public void Process_6008(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<RoleInfoForRank> _roleList = new List<RoleInfoForRank>();
        for (int i = 7; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('!');
            for (int j = 0; j < secSplit.Length - 1; j++)
            {
                string[] secSplit2 = secSplit[j].Split('$');
                _roleList.Add(new RoleInfoForRank(secSplit2[0], secSplit2[1], secSplit2[2], secSplit2[3]));
            }
        }
        if (GameObject.Find("LookInfoWindow") != null)
        {
            LookInfoWindow lIW = GameObject.Find("LookInfoWindow").GetComponent<LookInfoWindow>();
            lIW.SetLookInfoWindow(dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], dataSplit[4], dataSplit[5], dataSplit[6], dataSplit[7], dataSplit[8], dataSplit[9], _roleList);
        }
        else
        {
            Debug.Log("未找到LookInfoWindow");
        }
    }
    public void Process_6009(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.GetPointLayer = int.Parse(dataSplit[0]);

        GameObject cw = GameObject.Find("ChallengeWindow");
        if (cw != null)
        {
            cw.GetComponent<ChallengeWindow>().SetJingJiChangRedPoint_6009();
        }
        //if (GameObject.Find("IntegrationWindow") != null)
        //{
        //    IntegrationWindow IW = GameObject.Find("IntegrationWindow").GetComponent<IntegrationWindow>();
        //    IW.GetInfo(int.Parse(dataSplit[0]));
        //}
        //if (GameObject.Find("ChallengeWindow") != null)
        //{
        //    ChallengeWindow Cha = GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>();
        //    Cha.GetPointLayer = int.Parse(dataSplit[0]);
        //}
        //if (GameObject.Find("PVPWindow") != null)
        //{
        //    PVPWindow rw = GameObject.Find("PVPWindow").GetComponent<PVPWindow>();
        //    rw.GetPointLayer = int.Parse(dataSplit[0]);
        //}
        CharacterRecorder.instance.Collision();
    }
    public void Process_6010(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            // CharacterRecorder.instance.GetPointLayer = int.Parse(dataSplit[1]);
            if (GameObject.Find("IntegrationWindow") != null)
            {
                IntegrationWindow IW = GameObject.Find("IntegrationWindow").GetComponent<IntegrationWindow>();
                IW.updateItemlistShow(int.Parse(dataSplit[1]), dataSplit[2], 1);
            }
        }
        else
        {
            //if (GameObject.Find("IntegrationWindow") != null)
            //{
            //    UIManager.instance.OpenPromptWindow("领取失败!", PromptWindow.PromptType.Hint, null, null);
            //}
        }
    }
    public void Process_6011(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.GetRankLayer = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.HaveRankLayer = int.Parse(dataSplit[1]);
        //if (GameObject.Find("GetRankingReward") != null)
        //{
        //    GetRankingReward Re = GameObject.Find("GetRankingReward").GetComponent<GetRankingReward>();
        //    Re.GetPVPRank(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        //    //NetworkHandler.instance.SendProcess("6001#;");
        //}
        //if (GameObject.Find("ChallengeWindow") != null)
        //{
        //    ChallengeWindow Cha = GameObject.Find("ChallengeWindow").GetComponent<ChallengeWindow>();
        //    Cha.GetRankLayer = int.Parse(dataSplit[0]);
        //    Cha.HaveRankLayer = int.Parse(dataSplit[1]);
        //}
        //if (GameObject.Find("PVPWindow") != null)
        //{
        //    PVPWindow rw = GameObject.Find("PVPWindow").GetComponent<PVPWindow>();
        //    rw.GetRankLayer = int.Parse(dataSplit[0]);
        //    rw.HaveRankLayer = int.Parse(dataSplit[1]);
        //}
        if (GameObject.Find("PVPWindow") != null)
        {
            GameObject.Find("PVPWindow").GetComponent<PVPWindow>().GetResultWindow();
        }

        GameObject cw = GameObject.Find("ChallengeWindow");
        if (cw != null)
        {
            cw.GetComponent<ChallengeWindow>().SetJingJiChangRedPoint_6011();
        }
    }

    public void Process_6012(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("GetRankingReward") != null)
            {
                List<Item> itemlist = new List<Item>();
                string[] secSplit = dataSplit[2].Split('!');
                for (int i = 0; i < secSplit.Length - 1; i++)
                {
                    //UIManager.instance.OpenPromptWindow("领取成功！", PromptWindow.PromptType.Hint, null, null);
                    string[] ticSplit = secSplit[i].Split('$');
                    Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                    itemlist.Add(_item);
                }
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
                GetRankingReward Re = GameObject.Find("GetRankingReward").GetComponent<GetRankingReward>();
                Re.updateItemlistShow(int.Parse(dataSplit[1]));
            }
        }
        else
        {
            if (GameObject.Find("GetRankingReward") != null)
            {
                UIManager.instance.OpenPromptWindow("领取失败！", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_6013(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("BuyChanceWindow") == null)
        {
            UIManager.instance.OpenPanel("BuyChanceWindow");
            GameObject.Find("BuyChanceWindow").GetComponent<BuyChanceWindow>().GetCostDiamond(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        }
        else
        {
            GameObject.Find("BuyChanceWindow").GetComponent<BuyChanceWindow>().GetCostDiamond(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        }
    }

    public void Process_6014(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("PVPWindow") != null)
            {
                GameObject.Find("PVPWindow").GetComponent<PVPWindow>().ChanceCount.text = string.Format("挑战次数:{0}{1}/{2}", "[f8911d]", int.Parse(dataSplit[1]), 5);
                GameObject.Find("PVPWindow").GetComponent<PVPWindow>().leftCount = int.Parse(dataSplit[1]);
                GameObject.Find("PVPWindow/ALL/Bottom/ChangeButton/Label").GetComponent<UILabel>().text = "换一批";
                GameObject.Find("RefreshCostType").transform.GetComponent<UISprite>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
                GameObject.Find("PVPWindow").GetComponent<PVPWindow>().GetRefreshNum();
            }
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.PvpChallengeNum = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.PvpRefreshTime = 0;
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            //NetworkHandler.instance.SendProcess("6013#");
            if (GameObject.Find("BuyChanceWindow") != null)
            {
                DestroyImmediate(GameObject.Find("BuyChanceWindow"));
            }
        }
        else
        {
            if (GameObject.Find("PVPWindow") != null)
            {
                UIManager.instance.OpenPromptWindow("领取失败！", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }
    public void Process_6015(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.PVPComeNum = 0;
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            if (GameObject.Find("PVPWindow") != null)
            {
                PVPWindow pvpWindow = GameObject.Find("PVPWindow").GetComponent<PVPWindow>();
                pvpWindow.RefreshNum = int.Parse(dataSplit[1]) + 1;
                pvpWindow.GetRefreshNum();
            }
            NetworkHandler.instance.SendProcess("6002#;");
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("数据表问题！", PromptWindow.PromptType.Hint, null, null);
            }
            else
            {
                UIManager.instance.OpenPromptWindow("钻石不足！", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_6016(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.PvpRefreshTime = 0;
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            GameObject.Find("PVPWindow").GetComponent<PVPWindow>().leftTime = 0;
        }
        else
        {
            UIManager.instance.OpenPromptWindow("钻石不足！", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_6017(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "")
        {
            CharacterRecorder.instance.FirstPowerName = dataSplit[0];
        }
        if (dataSplit[1] != "")
        {
            CharacterRecorder.instance.FirstPvpName = dataSplit[1];
        }
        if (dataSplit[2] != "")
        {
            CharacterRecorder.instance.FirstWoodsName = dataSplit[2];
        }
        if (dataSplit[3] != "")
        {
            CharacterRecorder.instance.FirstLegionName = dataSplit[3];
        }

        if (GameObject.Find("MapUiWindow") != null)
        {
            GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().CupShowInMainWindow();
        }
    }
    public void Process_6301(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.SmuggleNum = int.Parse(dataSplit[1]);
        CharacterRecorder.instance.SmuggleTime = int.Parse(dataSplit[2]);
        //if (GameObject.Find("MainWindow") != null)
        //{
        //    GameObject.Find("MainWindow").GetComponent<MainWindow>().Collision();
        //}
        CharacterRecorder.instance.Collision();
        {
            if (GameObject.Find("SmuggleWindow") != null)
            {
                GameObject.Find("SmuggleWindow").GetComponent<SmuggleWindow>().PlayerListInfo(dataSplit);
            }
        }
        GameObject ch = GameObject.Find("ChallengeWindow");
        if (ch != null)
        {
            ch.GetComponent<ChallengeWindow>().SetZouSiRedPoint_6301(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
        CharacterRecorder.instance.Collision();
    }
    public void Process_6302(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("SmuggleWindow") != null)
        {
            GameObject.Find("SmuggleWindow").GetComponent<SmuggleWindow>().EnemyListInfo(dataSplit[0]);
        }
    }
    public void Process_6303(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
    }
    public void Process_6304(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("SmuggleSelfList") != null)
        {
            GameObject.Find("SmuggleWindow").GetComponent<SmuggleWindow>().SmuggleCarInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        }
    }

    public void Process_6305(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("SmuggleSelfList") != null)
            {
                GameObject.Find("SmuggleWindow").GetComponent<SmuggleWindow>().SmuggleCarInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            }
            GameObject _TopContent = GameObject.Find("TopContent");
            if (_TopContent != null)
            {
                CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
                _TopContent.GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("刷新次数不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_6306(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject _TopContent = GameObject.Find("TopContent");
        if (_TopContent != null)
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            _TopContent.GetComponent<TopContent>().Reset();
        }

        if (dataSplit[0] == "1") //飞机装运货物
        {
            NetworkHandler.instance.SendProcess("7002#16;" + CharacterRecorder.instance.characterName + ";" + "0;0;");
        }
    }
    public void Process_6307(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
    }
    public void Process_6308(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<FragmentItemData> _fragmentItemList = new List<FragmentItemData>();
        UIManager.instance.OpenPanel("GrabFinishWindow", false);
        if (dataSplit[0] == "0")
        {

            _fragmentItemList.Add(new FragmentItemData(90006, 0, 0, 0, 0));
            GameObject.Find("GrabFinishWindow").GetComponent<GrabFinishWindow>().GrabInfo(0, _fragmentItemList);
            GameObject.Find("GrabFinishWindow").transform.Find("AwardObj/Label").GetComponent<UILabel>().text = "被人捷足先登了!";
        }
        else
        {
            if (dataSplit[3] != "")
            {
                string[] awardSplit = dataSplit[3].Split('!');
                string[] itemSplit;
                if (awardSplit[1] != "")
                {
                    itemSplit = awardSplit[0].Split('$');
                    string[] awardItem = awardSplit[1].Split('$');
                    //CharacterRecorder.instance.isFailed = false;
                    _fragmentItemList.Add(new FragmentItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), 0, int.Parse(awardItem[0]), int.Parse(awardItem[1])));
                }
                else
                {
                    //CharacterRecorder.instance.isFailed = true;
                    itemSplit = awardSplit[0].Split('$');
                    _fragmentItemList.Add(new FragmentItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), 0, 0, 0));
                }
            }
            GameObject.Find("GrabFinishWindow").transform.Find("AwardObj/Label").GetComponent<UILabel>().text = "额外奖励";
            GameObject.Find("GrabFinishWindow").GetComponent<GrabFinishWindow>().GrabInfo(int.Parse(dataSplit[1]), _fragmentItemList);
        }
    }

    public void Process_6401(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[4] != "")
        {
            CharacterRecorder.instance.KingChallengeNum = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.KingBuyCount = int.Parse(dataSplit[2]);
            if (GameObject.Find("KingRoadWindow") != null)
            {
                GameObject.Find("KingRoadWindow").GetComponent<KingRoadWindow>().GetKingInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), dataSplit[4], dataSplit[5]);
            }
        }
        else
        {
            if (GameObject.Find("KingRoadWindow") != null)
            {
                GameObject.Find("KingRoadWindow").GetComponent<KingRoadWindow>().GetKingInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            }
        }

        GameObject ch = GameObject.Find("ChallengeWindow");
        if (ch != null)
        {
            ch.GetComponent<ChallengeWindow>().SetWangZheRedPoint_6401(int.Parse(dataSplit[1]), int.Parse(dataSplit[3]));
        }
        CharacterRecorder.instance.Collision();
    }

    public void Process_6402(string RecvString)
    {
        if (GameObject.Find("KingRoadWindow") != null)
        {
            GameObject.Find("KingRoadWindow").GetComponent<KingRoadWindow>().GetKingRank(RecvString);
        }
    }

    public void Process_6403(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "0")
        {
            UIManager.instance.OpenSinglePanel("KingRoadFightWindow", true);
            UIManager.instance.OpenPanel("KingRoadFightWindow", true);
            GameObject.Find("KingRoadFightWindow").GetComponent<KingRoadFightWindow>().GetKingFight(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2]);
        }
        else
        {

        }
    }

    public void Process_6404(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            if (GameObject.Find("KingRoadWindow") != null)
            {
                GameObject.Find("KingRoadWindow").GetComponent<KingRoadWindow>().KingBuyCount(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            }
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
        else
        {
            UIManager.instance.OpenPromptWindow("购买失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_6405(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("KingRoadWindow") != null)
            {
                GameObject.Find("KingRoadWindow").GetComponent<KingRoadWindow>().LookChangeResultWindow();
            }
        }
    }

    public void Process_6501(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[5]) == 0)
        {
            CharacterRecorder.instance.isConquerRedPoint = false;
        }
        else
        {
            CharacterRecorder.instance.isConquerRedPoint = true;
        }
        if (CharacterRecorder.instance.ChallengeToZhengFu == 0)
        {
            CharacterRecorder.instance.ChallengeToZhengFu = -1;
            GameObject ch = GameObject.Find("ChallengeWindow");
            if (ch != null)
            {
                ch.GetComponent<ChallengeWindow>().SetZhenFuRedPoint_6501(int.Parse(dataSplit[2]));
            }
        }
        else
        {
            if (CharacterRecorder.instance.TabeID == 0)
            {
                //if (GameObject.Find("ConquerWindow") == null)
                {
                    UIManager.instance.OpenPanel("ConquerWindow", true);
                }
            }
            GameObject cw = GameObject.Find("ConquerWindow");
            if (cw != null)
            {
                ConquerWindow conquer = cw.GetComponent<ConquerWindow>();
                if (dataSplit[0] != "")
                {

                    conquer.ShowHeroInfo(dataSplit[0]);
                }
                if (dataSplit[1] != "")
                {
                    conquer.HeroSelfUpInfo(dataSplit[1]);
                }
                conquer.CapturedNumberInfo(int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), dataSplit[4], int.Parse(dataSplit[5]));
            }
        }
        CharacterRecorder.instance.Collision();
    }
    public void Process_6502(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject.Find("HarvestWindow").GetComponent<HarvestWindow>().SetInfo(dataSplit);
        SendProcess("6501#");
    }
    public void Process_6503(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 1)
        {
            string[] rewarditem = dataSplit[1].Split('!');
            List<Item> itemlist = new List<Item>();
            for (int i = 0; i < rewarditem.Length - 1; i++)
            {
                string[] ticSplit = rewarditem[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            if (GameObject.Find("HarvestWindow") != null)
            {
                SendProcess("6502#" + CharacterRecorder.instance.TabeID + ";");
            }
            if (GameObject.Find("CheckGateWindow") != null)
            {
                SendProcess("6501#;");
                ConquerWindow conquer = GameObject.Find("ConquerWindow").GetComponent<ConquerWindow>();
                conquer.RewardOne.transform.Find("Label").GetComponent<UILabel>().text = "0";
                conquer.RewardTwo.transform.Find("Label").GetComponent<UILabel>().text = "0";
                conquer.RewardThree.transform.Find("Label").GetComponent<UILabel>().text = "0";
                for (int i = 0; i < itemlist.Count; i++)
                {
                    switch (itemlist[i].itemCode)
                    {
                        case 90001:
                            conquer.Gate1Effect.SetActive(true);
                            break;
                        case 90006:
                            conquer.Gate2Effect.SetActive(true);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                SendProcess("6501#;");
            }
            string message = "";
            for (int i = 0; i < itemlist.Count; i++)
            {
                switch (itemlist[i].itemCode)
                {
                    case 90001:
                        message += "金币" + itemlist[i].itemCount;
                        break;
                    case 90006:
                        if (itemlist[i].itemCount != 0)
                        {
                            message += "金条" + itemlist[i].itemCount;
                        }
                        break;
                    default:
                        break;
                }
            }
            StartCoroutine(DelayAdvanceWindow(itemlist));

            UpDateTopContentData(itemlist);
            CharacterRecorder.instance.isConquerRedPoint = false;
        }
        else
        {
            UIManager.instance.OpenPromptWindow("暂时没有可以领取的收益", PromptWindow.PromptType.Hint, null, null);
        }
    }

    IEnumerator DelayAdvanceWindow(List<Item> itemlist)
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
        GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
    }
    public void Process_6504(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("FightWindow") != null)
            {
                PictureCreater.instance.StopFight(true);
                UIManager.instance.BackTwoUI("ConquerWindow");
                if (GameObject.Find("ConquerWindow") != null)
                {
                    if (GameObject.Find("ConquerWindow") != null && CharacterRecorder.instance.TabeID != 0)
                    {
                        GameObject.Find("ConquerWindow").GetComponent<ConquerWindow>().CheckGateWindow.SetActive(false);
                        GameObject.Find("ConquerWindow").GetComponent<ConquerWindow>().HarvestWindow.SetActive(true);
                    }
                }
            }
            //SendProcess("6502#" + CharacterRecorder.instance.TabeID + ";");
            if (GameObject.Find("CheckGateWindow") == null && CharacterRecorder.instance.TabeID != 0)
            {
                SendProcess("6502#" + CharacterRecorder.instance.TabeID + ";");
                GameObject.Find("HarvestWindow").GetComponent<HarvestWindow>().ShowEffect(CharacterRecorder.instance.KengID);
            }
            else
            {
                SendProcess("6501#;");
            }
            UIManager.instance.OpenPromptWindow("上阵成功", PromptWindow.PromptType.Hint, null, null);
        }
        else
        {

        }
    }
    public void Process_6505(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject.Find("HarvestWindow").GetComponent<HarvestWindow>().LevelUpInfo(int.Parse(dataSplit[1]), true);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[2]);
            GameObject _TopContent = GameObject.Find("TopContent");
            if (_TopContent != null)
            {
                _TopContent.GetComponent<TopContent>().Reset();
            }
        }
        else
        {

        }
    }


    public void Process_6601(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("NuclearwarWindow") != null)
        {
            GameObject.Find("NuclearwarWindow").GetComponent<NuclearwarWindow>().SetState(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), dataSplit[2]);
        }
        if (dataSplit[2] == "1")
        {
            CharacterRecorder.instance.HaveNuclear = true;
        }
        else
        {
            CharacterRecorder.instance.HaveNuclear = false;
        }


    }
    public void Process_6602(string RecvString)
    {

        string[] dataSplit = RecvString.Split(';');
        AudioEditer.instance.PlayOneShot("Win");
        UIManager.instance.OpenPanel("EveryResultWindow", false);
        GameObject.Find("EveryResultWindow").GetComponent<EveryResultWindow>().SetWorldEventResult("", 1);
        if (dataSplit[3] == "1")
        {
            CharacterRecorder.instance.HaveNuclear = true;
        }
        else
        {
            CharacterRecorder.instance.HaveNuclear = false;
        }
        //GameObject.Find("NuclearwarWindow").GetComponent<NuclearwarWindow>().SetLastState(dataSplit[0], int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), dataSplit[3]);
    }
    public void Process_6603(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject.Find("NuclearwarWindow").GetComponent<NuclearwarWindow>().BuyVigor(dataSplit[0], int.Parse(dataSplit[1]));
    }
    public void Process_6604(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject.Find("NuclearwarWindow").GetComponent<NuclearwarWindow>().SetLastItem(dataSplit[0]);
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.HaveNuclear = false;
            List<Item> itemlist = new List<Item>();
            for (int i = 0; i < dataSplit[1].Split('!').Length - 1; i++)
            {
                itemlist.Add(new Item(int.Parse((dataSplit[1].Split('!'))[i].Split('$')[0]), int.Parse((dataSplit[1].Split('!'))[i].Split('$')[1])));
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            // UpDateTopContentData(itemlist);
            // SendProcess("9712#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败", PromptWindow.PromptType.Hint, null, null);
        }
    }



    public void Process_7001(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "0")
        {
            string[] dataSplit2 = dataSplit[3].Split('$');
            if (Application.loadedLevelName != "Downloader")
            {
                if (dataSplit[0] == "1" || dataSplit[0] == "2" || dataSplit[0] == "3" || dataSplit[0] == "4" || dataSplit[0] == "5") //系统，世界，军团，国家，好友
                {
                    ChatItemData _ChatItemData = new ChatItemData(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                    if (TextTranslator.instance.ChatItemDataList.size > 0)
                    {
                        if (_ChatItemData.textWords != TextTranslator.instance.ChatItemDataList[TextTranslator.instance.ChatItemDataList.size - 1].textWords ||
                            _ChatItemData.name != TextTranslator.instance.ChatItemDataList[TextTranslator.instance.ChatItemDataList.size - 1].name)             //去除重复信息
                        {
                            while (TextTranslator.instance.ChatItemDataList.size > 20)//最多20条信息
                            {
                                TextTranslator.instance.ChatItemDataList.RemoveAt(0);
                            }
                            if (_ChatItemData.channel == 3)//军团频道
                            {
                                if (CharacterRecorder.instance.legionID != 0 && int.Parse(dataSplit2[2]) == CharacterRecorder.instance.legionID)                //同一个军团
                                {
                                    TextTranslator.instance.AddChatItemData(_ChatItemData);
                                    ObscuredPrefs.SetString("LastChatItemInfo" + CharacterRecorder.instance.userId.ToString(), RecvString);
                                    GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
                                    if (_ChatWindowNew != null)
                                    {
                                        _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                                    }
                                }
                            }
                            else if (_ChatItemData.channel == 4) //国家频道
                            {
                                if (_ChatItemData.legionCountryID != 0 && _ChatItemData.legionCountryID == CharacterRecorder.instance.legionCountryID) //同一国家
                                {
                                    TextTranslator.instance.AddChatItemData(_ChatItemData);
                                    ObscuredPrefs.SetString("LastChatItemInfo" + CharacterRecorder.instance.userId.ToString(), RecvString);
                                    GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
                                    if (_ChatWindowNew != null)
                                    {
                                        _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                                    }
                                }
                            }
                            else
                            {
                                TextTranslator.instance.AddChatItemData(_ChatItemData);
                                //if (_ChatItemData.channel != 1) //小窗口不显示系统信息 //kino
                                {
                                    ObscuredPrefs.SetString("LastChatItemInfo" + CharacterRecorder.instance.userId.ToString(), RecvString);
                                }
                                GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
                                if (_ChatWindowNew != null)
                                {
                                    _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_ChatItemData.channel == 3)//军团频道
                        {
                            if (CharacterRecorder.instance.legionID != 0 && int.Parse(dataSplit2[2]) == CharacterRecorder.instance.legionID)//同一个军团
                            {
                                TextTranslator.instance.AddChatItemData(_ChatItemData);
                                ObscuredPrefs.SetString("LastChatItemInfo" + CharacterRecorder.instance.userId.ToString(), RecvString);
                                GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
                                if (_ChatWindowNew != null)
                                {
                                    _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                                }
                            }
                        }
                        else if (_ChatItemData.channel == 4) //国家频道
                        {
                            if (_ChatItemData.legionCountryID != 0 && _ChatItemData.legionCountryID == CharacterRecorder.instance.legionCountryID) //同一国家
                            {
                                TextTranslator.instance.AddChatItemData(_ChatItemData);
                                ObscuredPrefs.SetString("LastChatItemInfo" + CharacterRecorder.instance.userId.ToString(), RecvString);
                                GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
                                if (_ChatWindowNew != null)
                                {
                                    _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                                }
                            }
                        }
                        else
                        {
                            TextTranslator.instance.AddChatItemData(_ChatItemData);
                            if (_ChatItemData.channel != 1) //小窗口不显示系统信息
                            {
                                ObscuredPrefs.SetString("LastChatItemInfo" + CharacterRecorder.instance.userId.ToString(), RecvString);
                            }
                            GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
                            if (_ChatWindowNew != null)
                            {
                                _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                            }
                        }
                    }
                }
                else if (dataSplit[0] == "6") //组队
                {
                    TextTranslator.instance.ChatItemOnTeamCopy = new ChatItemData(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                    ObscuredPrefs.SetString("LastChatItemInfoOnTeam", RecvString);
                }
                else if (dataSplit[0] == "7") //军团邀请
                {
                    TextTranslator.instance.ChatItemOnTeamCopy = new ChatItemData(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                    ObscuredPrefs.SetString("LastChatItemInfoOnTeam", RecvString);
                }
                else if (dataSplit[0] == "9") //私聊
                {
                    while (TextTranslator.instance.PrivateChatItemDataList.size >= 100)
                    {
                        TextTranslator.instance.PrivateChatItemDataList.RemoveAt(0);
                    }
                    PrivateChatItemData _PrivateChatItemData = new PrivateChatItemData(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                    TextTranslator.instance.PrivateChatItemDataList.Add(_PrivateChatItemData);
                    GameObject _PrivateChatWindow = GameObject.Find("PrivateChatWindow");
                    if (_PrivateChatWindow != null)
                    {
                        _PrivateChatWindow.GetComponent<PrivateChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
                    }

                    if (int.Parse(dataSplit2[2]) != CharacterRecorder.instance.userId) //发送信息者不是我,表有新信息
                    {
                        CharacterRecorder.instance.HaveNewPrivateChatInfo = true;
                    }
                    GameObject _mainObj = GameObject.Find("MainWindow");
                    if (_mainObj != null)
                    {
                        MainWindow _main = GameObject.Find("MainWindow").GetComponent<MainWindow>();
                        _main.SetPrivateChatButtonIsOpen();
                    }
                }
                GameObject _mainObj2 = GameObject.Find("MainWindow");
                if (_mainObj2 != null)
                {
                    MainWindow _main = GameObject.Find("MainWindow").GetComponent<MainWindow>();
                    _main.SetNewChatButtonInfo();
                }
            }

            #region 旧的7001，暂不用
            //if (dataSplit2.Length > 3)
            //{
            //    if (Application.loadedLevelName != "Downloader" && dataSplit[0] != "9")
            //    {
            //        ChatItemData _ChatItemData = new ChatItemData(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]));
            //        if (TextTranslator.instance.ChatItemDataList.size > 0)
            //        {
            //            if (_ChatItemData.textWords != TextTranslator.instance.ChatItemDataList[TextTranslator.instance.ChatItemDataList.size - 1].textWords ||
            //                _ChatItemData.name != TextTranslator.instance.ChatItemDataList[TextTranslator.instance.ChatItemDataList.size - 1].name)//去除重复信息
            //            {
            //                if (_ChatItemData.channel == 3)//军团频道
            //                {
            //                    if (CharacterRecorder.instance.legionID != 0 && int.Parse(dataSplit2[2]) == CharacterRecorder.instance.legionID)//同一个军团
            //                    {
            //                        TextTranslator.instance.AddChatItemData(_ChatItemData);
            //                    }
            //                }
            //                else
            //                {
            //                    TextTranslator.instance.AddChatItemData(_ChatItemData);
            //                }

            //                while (TextTranslator.instance.ChatItemDataList.size > 20)//最多20条信息
            //                {
            //                    Debug.Log("进入20条判断:" + TextTranslator.instance.ChatItemDataList.size);
            //                    TextTranslator.instance.ChatItemDataList.RemoveAt(0);
            //                    Debug.Log("进入20条判断,删除后:" + TextTranslator.instance.ChatItemDataList.size);
            //                }
            //                GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
            //                if (_ChatWindowNew != null)
            //                {
            //                    _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]));
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (_ChatItemData.channel == 3)//军团频道
            //            {
            //                if (CharacterRecorder.instance.legionID != 0 && int.Parse(dataSplit2[2]) == CharacterRecorder.instance.legionID)//同一个军团
            //                {
            //                    TextTranslator.instance.AddChatItemData(_ChatItemData);
            //                }
            //            }
            //            else
            //            {
            //                TextTranslator.instance.AddChatItemData(_ChatItemData);
            //            }
            //            GameObject _ChatWindowNew = GameObject.Find("ChatWindowNew");
            //            if (_ChatWindowNew != null)
            //            {
            //                // _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]));
            //                if (_ChatItemData.channel == 3)//军团频道
            //                {
            //                    if (CharacterRecorder.instance.legionID != 0 && int.Parse(dataSplit2[2]) == CharacterRecorder.instance.legionID)//同一个军团
            //                    {
            //                        _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]));
            //                    }
            //                }
            //                else
            //                {
            //                    _ChatWindowNew.GetComponent<ChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]));
            //                }
            //            }
            //        }
            //        switch (_ChatItemData.channel)
            //        {
            //            case 1:
            //                CharacterRecorder.instance.Tab_Channel1++;
            //                break;
            //            case 2:
            //                CharacterRecorder.instance.Tab_Channel2++;
            //                break;
            //            case 3:
            //                CharacterRecorder.instance.Tab_Channel3++;
            //                break;
            //            case 4:
            //                CharacterRecorder.instance.Tab_Channel4++;
            //                break;
            //        }
            //        GameObject _mainObj = GameObject.Find("MainWindow");
            //        if (_mainObj != null)
            //        {
            //            MainWindow _main = GameObject.Find("MainWindow").GetComponent<MainWindow>();
            //            _main.SetNewChatButtonInfo();
            //        }
            //    }
            //    else if (Application.loadedLevelName != "Downloader" && dataSplit[0] == "9")
            //    {
            //        while (TextTranslator.instance.PrivateChatItemDataList.size >= 100)
            //        {
            //            TextTranslator.instance.PrivateChatItemDataList.RemoveAt(0);
            //        }
            //        PrivateChatItemData _PrivateChatItemData = new PrivateChatItemData(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
            //        TextTranslator.instance.PrivateChatItemDataList.Add(_PrivateChatItemData);
            //        GameObject _PrivateChatWindow = GameObject.Find("PrivateChatWindow");
            //        if (_PrivateChatWindow != null)
            //        {
            //            _PrivateChatWindow.GetComponent<PrivateChatWindow>().CreatOneChatItem(int.Parse(dataSplit[0]), dataSplit[1], dataSplit[2], int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]), int.Parse(dataSplit2[2]), int.Parse(dataSplit2[3]), int.Parse(dataSplit2[4]), int.Parse(dataSplit2[5]));
            //        }
            //        GameObject _mainObj = GameObject.Find("MainWindow");
            //        if (_mainObj != null)
            //        {
            //            MainWindow _main = GameObject.Find("MainWindow").GetComponent<MainWindow>();
            //            _main.SetPrivateChatButtonIsOpen();
            //        }
            //    }
            //}

            #endregion
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("对方不在线", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }


    public void Process_7002(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');//ObscuredPrefs.GetInt("PoupsCount") == TextTranslator.instance.PopupMessage.Count-1
        string trSplit = dataSplit[0];

        if (Application.loadedLevelName != "Downloader" && GameObject.Find("StartNameWindow") == null)
        {
            switch (dataSplit[0])
            {
                case "0":
                    trSplit = dataSplit[2];
                    break;
                case "1":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]任命战功赫赫的[ffff00]" + dataSplit[2] + "[-]为[ffff00]" + dataSplit[3] + "[-]";
                    break;
                case "2":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]为扩充军备撒出充值礼包,[ffff00]抢[-]";
                    break;
                case "3":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]为研发人体实验成功,将" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]的技能提升至等级[ffff00]" + dataSplit[3] + "[-]";
                    break;
                case "4":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]威名远播,将" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]招募到旗下效命";
                    break;
                case "5":
                    trSplit = "号外!号外![ffff00]" + dataSplit[1] + "[-]称霸世界啦!达到[ffff00]竞技场[-]第一名,大家快去瞻仰吧";
                    break;
                case "6":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]千辛万苦收集碎片，将" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-],合成成功";
                    break;
                case "7":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]三顾茅庐苦苦守候，终于盼到" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]加入效命";
                    break;
                case "8":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]人品大爆发，在宝箱中发现" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]";
                    break;
                case "9":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]打开宝箱，发现" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]，将其纳入旗下";
                    break;
                case "10":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]" + dataSplit[2];
                    break;
                case "11":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]用心良苦,对军团进行了高级捐献,大家掌声鼓励";
                    break;
                case "12":
                    trSplit = "经历九九八十一难,[ffff00]" + dataSplit[1] + "[-]在丛林冒险通关50层啦";
                    break;
                case "13":
                    if (dataSplit[2] == "40104")
                    {
                        Item item = new Item(int.Parse(dataSplit[2]), 0);
                        trSplit = "不可思议,[ffff00]" + dataSplit[1] + "[-]在组队副本中,打到[9900cc]" + item.itemName + "[-]秘宝啦";
                    }
                    else if (dataSplit[2] == "40105")
                    {
                        Item item = new Item(int.Parse(dataSplit[2]), 0);
                        trSplit = "不可思议,[ffff00]" + dataSplit[1] + "[-]在组队副本中,打到[ff9900]" + item.itemName + "[-]秘宝啦";
                    }
                    else if (dataSplit[2] == "40106")
                    {
                        Item item = new Item(int.Parse(dataSplit[2]), 0);
                        trSplit = "不可思议,[ffff00]" + dataSplit[1] + "[-]在组队副本中,打到[ff0000]" + item.itemName + "[-]秘宝啦";
                    }
                    break;
                case "14":
                    trSplit = "拨云见日,[ffff00]" + dataSplit[1] + "[-]通关第" + dataSplit[2] + "片迷雾发现新大陆";
                    break;
                case "15":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]世界事件偶遇雷电,相谈甚欢得到[ff0000]雷电[-]碎片";
                    break;
                case "16":
                    trSplit = "真土豪![ffff00]" + dataSplit[1] + "[-]用飞机装运货物,让人眼红啊";
                    break;
                case "17":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]展现强大的凝聚力,打通军团副本[ffff00]" + dataSplit[2] + "关[-]";
                    break;
                case "18":
                    trSplit = "[ffff00]" + dataSplit[2] + "[-]军团的" + dataSplit[1] + "浴血奋战,成功占领[ffff00]" + dataSplit[3] + "[-]";
                    break;
                case "19":
                    trSplit = "[ffff00]" + dataSplit[2] + "[-]军团邀请战队加入,一起称霸世界,军团ID:" + dataSplit[3];
                    break;
                case "20":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]在竟技场经过猛烈交火击败" + dataSplit[2] + "成为第[ffff00]" + dataSplit[3] + "[-]名";
                    break;
                case "21":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]在军团副本中不可思议的抽中一等奖[ffff00]" + dataSplit[2] + "奖励" + dataSplit[3] + "个[-]";
                    break;

                case "22":
                    trSplit = "经过精密的研发,[ffff00]" + dataSplit[1] + "[-]合成出" + dataSplit[2] + "核武器,战力大爆发";
                    break;
                case "23":
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]把[ffff00]" + dataSplit[2] + "[-]核武器大改造,提升到" + dataSplit[3] + "色,全身闪亮亮";
                    break;
                case "28":
                    int num = int.Parse(dataSplit[3]);
                    if (num < 7)
                    {
                        trSplit = "恭喜[ffff00]" + dataSplit[1] + "[-]成功把英雄" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]升到了[ffff00]" + dataSplit[3] + "[-]星,小队战力得到了大幅度的提升!";
                    }
                    else
                    {
                        trSplit = "恭喜[ffff00]" + dataSplit[1] + "[-]成功把英雄" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]升到了[fb2d50]红" + (num - 6) + "星[-],小队战力得到了大幅度的提升!";
                    }
                    break;
                case "29":
                    trSplit = "恭喜[ffff00]" + dataSplit[1] + "[-]成功把英雄" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]升品到了[bb44ff]" + dataSplit[3] + "[-],小队战力又提升一大截!";
                    break;
                case "30":
                    trSplit = "恭喜[ffff00]" + dataSplit[1] + "[-]成功把英雄" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]升品到了[ff8c04]" + dataSplit[3] + "[-],战斗力突飞猛进!";
                    break;
                case "31":
                    trSplit = "恭喜[ffff00]" + dataSplit[1] + "[-]成功把英雄" + TextTranslator.instance.GetNameColorByName(dataSplit[2]) + dataSplit[2] + "[-]升品到了[fb2d50]" + dataSplit[3] + "[-],总体势力已经突破天际了!";
                    break;
                case "32":
                    string CityName = TextTranslator.instance.GetLegionCityByID(int.Parse(dataSplit[3])).CityName;
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]在军团战场中[ffff00]" + CityName + "[-]城市,[ffff00]大杀特杀[-]!";
                    break;
                case "33":
                    string CityName1 = TextTranslator.instance.GetLegionCityByID(int.Parse(dataSplit[3])).CityName;
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]在军团战场中[ffff00]" + CityName1 + "[-]城市,[ffff00]完成五杀了[-]!";
                    break;
                case "34":
                    string CityName2 = TextTranslator.instance.GetLegionCityByID(int.Parse(dataSplit[3])).CityName;
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]在军团战场中[ffff00]" + CityName2 + "[-]城市,[ffff00]已经超神[-]!";
                    break;
                case "35":
                    string CityName3 = TextTranslator.instance.GetLegionCityByID(int.Parse(dataSplit[3])).CityName;
                    trSplit = "[ffff00]" + dataSplit[1] + "[-]在军团战场中[ffff00]" + CityName3 + "[-]城市,[ffff00]无人可挡[-]!";
                    break;
                case "39":
                    trSplit = "恭喜[ffff00]" + dataSplit[1] + "[-]在夺宝奇兵中获得了[ffff00]万能碎片[-],运气简直爆棚了!";
                    break;
                case "40":
                    trSplit = string.Format("恭喜[ffff00] {0} [-]在砸金蛋活动中获得了[ffff00]幸运大奖[-],运气简直爆棚了!", dataSplit[1], dataSplit[2], dataSplit[3]);
                    break;
                case "41":
                    int yuanNum = TextTranslator.instance.GetExchangeById(int.Parse(dataSplit[3])).cash;
                    trSplit = string.Format("土豪[ffff00]{0}[-]发送[ffff00]{1}元[-]充值红包,速度领啊!", dataSplit[1], yuanNum);
                    break;
                default:
                    trSplit = "";
                    break;
            }


            if (trSplit != "" && GameObject.Find("LoadingWindow") == null)
            {
                string doc = string.Format("{0};{1};{2};{3};{4};", dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], trSplit);

                GameObject _mainObj = GameObject.Find("MainWindow");
                if (_mainObj != null)
                {
                    MainWindow _main = GameObject.Find("MainWindow").GetComponent<MainWindow>();
                    _main.SetNewChatButtonInfo();
                }
                if (dataSplit[0] == "41")
                {
                    if (TextTranslator.instance.docQueue.Count > 0)
                    {
                        bool IsChange = false;
                        for (int i = 0; i < TextTranslator.instance.docQueue.Count; i++)
                        {
                            string poc = TextTranslator.instance.docQueue[i].Split(';')[0];
                            if (poc != "41")
                            {
                                TextTranslator.instance.docQueue[i] = doc;
                                IsChange = true;
                                break;
                            }
                        }
                        if (!IsChange)
                        {
                            TextTranslator.instance.AddQueuedoc(doc);
                        }
                    }
                    else
                    {
                        TextTranslator.instance.AddQueuedoc(doc);
                    }
                }
                else if (TextTranslator.instance.docQueue.Count < 5)
                {
                    TextTranslator.instance.AddQueuedoc(doc);
                }
                //TextTranslator.instance.AddQueuedoc(doc);
                //if (TextTranslator.instance.docQueue.Count > 5)
                //{
                //    TextTranslator.instance.ReadQueuedoc();
                //}
                if (GameObject.Find("PopupWindow") == null && TextTranslator.instance.IsQueueAvailable)
                {
                    UIManager.instance.OpenPromptWindow("0", PromptWindow.PromptType.Popup, null, null);
                }
            }
        }
    }

    IEnumerator StatyPopupWindow()
    {
        while (true)
        {
            if (GameObject.Find("PopupWindow") == null && TextTranslator.instance.IsQueueAvailable)
            {
                UIManager.instance.OpenPromptWindow("0", PromptWindow.PromptType.Popup, null, null);
                break;
            }
            //UIManager.instance.OpenPromptWindow("0", PromptWindow.PromptType.Popup, null, null);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void Process_7003(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject fw = GameObject.Find("FightWindow");
            if (fw != null)
            {
                fw.GetComponent<FightWindow>().SetSelfBulletInfo(dataSplit[1]);
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("不能发送敏感字体", PromptWindow.PromptType.Hint, null, null);
        }

    }

    public void Process_7004(string RecvString)
    {
        if (GameObject.Find("PrivateChatWindow") != null)
        {
            GameObject.Find("PrivateChatWindow").GetComponent<PrivateChatWindow>().GetPrivateDataListOnLine(RecvString);
        }
    }

    public void Process_7101(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] dataSplitItem = dataSplit[0].Split('!');
        List<FriendItemData> _FriendItemDataList = new List<FriendItemData>();
        int tabIndex = 0;
        CharacterRecorder.instance.MyFriendUIDList.Clear();
        CharacterRecorder.instance.MyFriendList.Clear();
        for (int i = 0; i < dataSplitItem.Length - 1; i++)
        {
            string[] dataSplitItemData = dataSplitItem[i].Split('$');
            int userId = int.Parse(dataSplitItemData[0]);
            string name = dataSplitItemData[1];
            int level = int.Parse(dataSplitItemData[2]);
            int fight = int.Parse(dataSplitItemData[3]);
            string icon = dataSplitItemData[4];
            int vipLv = int.Parse(dataSplitItemData[5]);
            int lastLoginTime = int.Parse(dataSplitItemData[6]);

            int canGetSpriteState = int.Parse(dataSplitItemData[7]);
            int giveSpriteState = int.Parse(dataSplitItemData[8]);
            FriendItemData _FriendItemData = new FriendItemData(tabIndex, userId, name, level, fight, icon, vipLv, lastLoginTime, canGetSpriteState, giveSpriteState, (dataSplitItemData[9]), int.Parse(dataSplitItemData[10]));
            _FriendItemDataList.Add(_FriendItemData);
            CharacterRecorder.instance.MyFriendUIDList.Add(userId);
            CharacterRecorder.instance.MyFriendList.Add(_FriendItemData);
        }

        GameObject _FriendWindow = GameObject.Find("FriendWindow");
        if (_FriendWindow != null)
        {
            _FriendWindow.GetComponent<FriendWindow>().SetFriendWindow(tabIndex, _FriendItemDataList);
        }
        GameObject _LegionFriendWindow = GameObject.Find("LegionFriendWindow");
        if (_LegionFriendWindow != null)
        {
            _LegionFriendWindow.GetComponent<LegionFriendWindow>().SetFriendWindow(_FriendItemDataList);
        }
        if (GameObject.Find("FriendListWindow") != null)
        {
            GameObject.Find("FriendListWindow").GetComponent<FriendListWindow>().GetFriendList(_FriendItemDataList);
        }
        if (GameObject.Find("FriendInfoWindow") != null)
        {
            GameObject.Find("FriendInfoWindow").GetComponent<FriendInfoWindow>().SetInfo(_FriendItemDataList);
        }
    }
    public void Process_7102(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] dataSplitItem = dataSplit[0].Split('!');
        List<FriendItemData> _FriendItemDataList = new List<FriendItemData>();
        int tabIndex = 1;
        for (int i = 0; i < dataSplitItem.Length - 1; i++)
        {
            string[] dataSplitItemData = dataSplitItem[i].Split('$');
            int userId = int.Parse(dataSplitItemData[0]);
            string name = dataSplitItemData[1];
            int level = int.Parse(dataSplitItemData[2]);
            int fight = int.Parse(dataSplitItemData[3]);
            string icon = dataSplitItemData[4];
            int vipLv = int.Parse(dataSplitItemData[5]);
            int lastLoginTime = int.Parse(dataSplitItemData[6]);

            FriendItemData _FriendItemData = new FriendItemData(tabIndex, userId, name, level, fight, icon, vipLv, lastLoginTime);
            _FriendItemDataList.Add(_FriendItemData);
        }

        GameObject _FriendWindow = GameObject.Find("FriendWindow");
        if (_FriendWindow != null)
        {
            _FriendWindow.GetComponent<FriendWindow>().SetFriendWindow(tabIndex, _FriendItemDataList);
        }
    }
    public void Process_7103(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] dataSplitItem = dataSplit[0].Split('!');
        List<FriendItemData> _FriendItemDataList = new List<FriendItemData>();
        int tabIndex = 2;
        for (int i = 0; i < dataSplitItem.Length - 1; i++)
        {
            string[] dataSplitItemData = dataSplitItem[i].Split('$');
            int userId = int.Parse(dataSplitItemData[0]);
            string name = dataSplitItemData[1];
            int level = int.Parse(dataSplitItemData[2]);
            int fight = int.Parse(dataSplitItemData[3]);
            string icon = dataSplitItemData[4];
            int vipLv = int.Parse(dataSplitItemData[5]);
            int lastLoginTime = int.Parse(dataSplitItemData[6]);

            FriendItemData _FriendItemData = new FriendItemData(tabIndex, userId, name, level, fight, icon, vipLv, lastLoginTime);
            _FriendItemDataList.Add(_FriendItemData);
        }

        GameObject _FriendWindow = GameObject.Find("FriendWindow");
        if (_FriendWindow != null)
        {
            _FriendWindow.GetComponent<FriendWindow>().SetFriendWindow(tabIndex, _FriendItemDataList);
        }
        CharacterRecorder.instance.applayFriendListCount = _FriendItemDataList.Count;
        if (CharacterRecorder.instance.applayFriendListCount == 0)
        {
            CharacterRecorder.instance.SetRedPoint(9, false);
        }
        GameObject _MainWindowObj = GameObject.Find("MainWindow");
        //if (_MainWindowObj != null)
        //{
        //    _MainWindowObj.GetComponent<MainWindow>().SetFriendRedPoint();
        //}
        CharacterRecorder.instance.SetFriendRedPoint();
    }
    public void Process_7104(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject _FriendWindow = GameObject.Find("FriendWindow");
            if (_FriendWindow != null)
            {
                _FriendWindow.GetComponent<FriendWindow>().ResetFriendWindow(7104, dataSplit[1]);
            }
            GameObject _LegionMemberItemDetail = GameObject.Find("LegionMemberItemDetail");
            if (_LegionMemberItemDetail != null)
            {
                UIManager.instance.OpenPromptWindow("已申请，等待对方验证", PromptWindow.PromptType.Hint, null, null);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("好友达上限", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("客户端数据异常", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("重复申请", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("已是好友", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("对方好友已达上限", PromptWindow.PromptType.Hint, null, null); break;
                case "5": UIManager.instance.OpenPromptWindow("不可添加自己为好友", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_7105(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.applayFriendListCount -= 1;
            GameObject _FriendWindow = GameObject.Find("FriendWindow");
            if (_FriendWindow != null)
            {
                _FriendWindow.GetComponent<FriendWindow>().ResetFriendWindow(7105, dataSplit[1]);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("非等待列表战队", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("对方好友超出上限", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("自己好友超出上限", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("已是好友", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_7106(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.applayFriendListCount -= 1;
            GameObject _FriendWindow = GameObject.Find("FriendWindow");
            if (_FriendWindow != null)
            {
                _FriendWindow.GetComponent<FriendWindow>().ResetFriendWindow(7106, dataSplit[1]);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("拒绝失败", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_7107(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject _FriendWindow = GameObject.Find("FriendWindow");
            if (_FriendWindow != null)
            {
                _FriendWindow.GetComponent<FriendWindow>().ResetFriendWindow(7107, dataSplit[1]);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("不是好友", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("已赠送", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_7108(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.sprite = int.Parse(dataSplit[2]);
            GameObject _TopContent = GameObject.Find("TopContent");
            if (_TopContent != null)
            {
                _TopContent.GetComponent<TopContent>().Reset();
            }
            GameObject _FriendWindow = GameObject.Find("FriendWindow");
            if (_FriendWindow != null)
            {
                _FriendWindow.GetComponent<FriendWindow>().ResetFriendWindow(7108, dataSplit[1]);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("不是好友", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_7109(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            NetworkHandler.instance.SendProcess("7101#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("删除好友失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_7110(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //string[] dataSplitItem = dataSplit[1].Split('!');
            List<FriendItemData> _FriendItemDataList = new List<FriendItemData>();
            int tabIndex = 1;
            //for (int i = 0; i < dataSplitItem.Length - 1; i++)
            {
                string[] dataSplitItemData = dataSplit[1].Split('$');
                int userId = int.Parse(dataSplitItemData[0]);
                string name = dataSplitItemData[1];
                int level = int.Parse(dataSplitItemData[2]);
                int fight = int.Parse(dataSplitItemData[3]);
                string icon = dataSplitItemData[4];
                int vipLv = int.Parse(dataSplitItemData[5]);
                int lastLoginTime = int.Parse(dataSplitItemData[6]);

                FriendItemData _FriendItemData = new FriendItemData(tabIndex, userId, name, level, fight, icon, vipLv, lastLoginTime);
                _FriendItemDataList.Add(_FriendItemData);
            }
            GameObject _FriendWindow = GameObject.Find("FriendWindow");
            if (_FriendWindow != null)
            {
                _FriendWindow.GetComponent<FriendWindow>().SetFriendWindow(tabIndex, _FriendItemDataList);
            }
            GameObject _LegionFriendWindow = GameObject.Find("LegionFriendWindow");
            if (_LegionFriendWindow != null)
            {
                _LegionFriendWindow.GetComponent<LegionFriendWindow>().SetFriendWindow(_FriendItemDataList);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("未找到", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }

    }
    public void Process_7111(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("高级赠送成功", PromptWindow.PromptType.Hint, null, null);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            switch (int.Parse(dataSplit[1]))
            {
                case 1:
                    UIManager.instance.OpenPromptWindow("VIP等级不足.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case 2:
                    UIManager.instance.OpenPromptWindow("钻石不足.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case 3:
                    UIManager.instance.OpenPromptWindow("当日赠送已达到上限.", PromptWindow.PromptType.Hint, null, null);
                    break;
                default:
                    break;
            }
            //UIManager.instance.OpenPromptWindow("高级赠送失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_8001(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //UIManager.instance.OpenPromptWindow("创建军团成功", PromptWindow.PromptType.Hint, null, null);
            CharacterRecorder.instance.legionID = int.Parse(dataSplit[1]);
            NetworkHandler.instance.SendProcess("8201#;");
            if (GameObject.Find("LegionCreatWindow") != null)
            {
                LegionCreatWindow _LegionCreatWindow = GameObject.Find("LegionCreatWindow").GetComponent<LegionCreatWindow>();
                _LegionCreatWindow.AfterSucceedCreatLegion();
            }
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            SendProcess("8017#1_0;");//初始设定自动加入
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("军团名称非法", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("军团名称重复", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("已是军团成员", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("军团人数达到上限", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("离开军团未满24小时", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_8002(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("解散军团成功", PromptWindow.PromptType.Hint, null, null);
            CharacterRecorder.instance.legionID = 0;
            UIManager.instance.OpenPanel("MainWindow", true);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("无此军团", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("不是军团长", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_8003(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("任命官职成功", PromptWindow.PromptType.Hint, null, null);
            int UID = int.Parse(dataSplit[1]);
            string officialPosition = dataSplit[2];
            if (officialPosition == "3")
            {
                CharacterRecorder.instance.isLegionChairman = false;
            }
            if (GameObject.Find("LegionSecondWindow") != null)
            {
                LegionSecondWindow _LegionSecondWindow = GameObject.Find("LegionSecondWindow").GetComponent<LegionSecondWindow>();
                _LegionSecondWindow.partsList[0].GetComponent<LegionBasicInfoPart>().ResetLegionBasicInfoPart(UID, officialPosition);
                _LegionSecondWindow.ShowCheckPartOrNot();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("不能设定军团长", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("非军团成员", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("非本军团成员", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("副军团长人数达上限", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("精英人数达上限", PromptWindow.PromptType.Hint, null, null); break;
                //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
                case "8":
                    UIManager.instance.OpenPromptWindow("权限不够 ", PromptWindow.PromptType.Hint, null, null);
                    SendProcess(string.Format("8004#{0};", CharacterRecorder.instance.legionID));
                    SendProcess(string.Format("8005#{0};", CharacterRecorder.instance.legionID));
                    break;
            }
        }
    }
    public void Process_8004(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //UIManager.instance.OpenPromptWindow("取得军团成功", PromptWindow.PromptType.Hint, null, null);
        int legionID = int.Parse(dataSplit[0]);
        string legionName = dataSplit[1];
        string legionChairmanName = dataSplit[2];
        int legionLevel = int.Parse(dataSplit[3]);
        int menberCount = int.Parse(dataSplit[4]);
        int legionFlag = int.Parse(dataSplit[5]);
        CharacterRecorder.instance.myLegionPosition = int.Parse(dataSplit[6]);
        LegionItemData _LegionItemData = new LegionItemData(legionID, legionName, legionChairmanName, legionLevel, menberCount);
        if (legionID == CharacterRecorder.instance.legionID)
        {
            CharacterRecorder.instance.legionName = legionName;
            CharacterRecorder.instance.legionFlag = legionFlag;
            CharacterRecorder.instance.myLegionData = _LegionItemData;
        }
        if (_LegionItemData.legionChairmanName == CharacterRecorder.instance.characterName)
        {
            CharacterRecorder.instance.isLegionChairman = true;
        }

        if (GameObject.Find("LegionMainHaveWindow") != null)
        {
            LegionMainHaveWindow _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow").GetComponent<LegionMainHaveWindow>();
            _LegionMainHaveWindow.SetLegionBasicInfoPart(_LegionItemData);
        }
        else if (GameObject.Find("LegionBasicInfoPart") != null)
        {
            LegionBasicInfoPart _LegionMainHaveWindow = GameObject.Find("LegionBasicInfoPart").GetComponent<LegionBasicInfoPart>();
            _LegionMainHaveWindow.SetLegionBasicInfoPart(_LegionItemData);
        }
    }
    public void Process_8005(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //UIManager.instance.OpenPromptWindow("取得军团成员成功", PromptWindow.PromptType.Hint, null, null);
        List<LegionMemberData> _mlList = new List<LegionMemberData>();
        _mlList.Clear();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            int uId = int.Parse(secSplit[0]);
            string name = secSplit[1];
            int level = int.Parse(secSplit[2]);
            int vip = int.Parse(secSplit[3]);
            int fight = int.Parse(secSplit[4]);
            int contribute = int.Parse(secSplit[5]);
            int officialPosition = int.Parse(secSplit[6]);
            int lastLoginTime = int.Parse(secSplit[7]);
            int iconHead = int.Parse(secSplit[8]);
            int _todayContribution = int.Parse(secSplit[9]);
            _mlList.Add(new LegionMemberData(uId, name, level, vip, fight, contribute, officialPosition, lastLoginTime, iconHead, _todayContribution));
        }
        if (GameObject.Find("LegionSecondWindow") != null)
        {
            LegionSecondWindow _LegionSecondWindow = GameObject.Find("LegionSecondWindow").GetComponent<LegionSecondWindow>();
            _LegionSecondWindow.partsList[0].GetComponent<LegionBasicInfoPart>().SetLegionBasicInfoPart(_mlList);
        }
    }
    public void Process_8006(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //UIManager.instance.OpenPromptWindow("取得军团捐献成功", PromptWindow.PromptType.Hint, null, null);
        int contributeLeftTimes = int.Parse(dataSplit[0]);
        int legionSumContribute = int.Parse(dataSplit[1]);
        int upGradeContribute = int.Parse(dataSplit[2]);
        int contributeProgress = int.Parse(dataSplit[3]);
        int memberNumContribute = int.Parse(dataSplit[4]);
        if (dataSplit[5].Contains("$"))
        {
            List<string> _mlList = new List<string>();//前5次高级捐献姓名
            _mlList.Clear();
            string[] secSplit = dataSplit[5].Split('$');
            for (int i = 0; i < secSplit.Length; i++)
            {
                _mlList.Add(secSplit[i]);
                NetworkHandler.instance.SendProcess("7002#11;" + secSplit[i] + ";" + "0;0;");
            }
        }

        List<int> boxGetSateList = new List<int>();
        string[] secSplitState = dataSplit[6].Split('$');
        for (int i = 0; i < secSplitState.Length - 1; i++)
        {
            boxGetSateList.Add(int.Parse(secSplitState[i]));
        }
        if (GameObject.Find("LegionContributeWindow") != null)
        {
            LegionContributeWindow _LegionContributeWindow = GameObject.Find("LegionContributeWindow").GetComponent<LegionContributeWindow>();
            _LegionContributeWindow.SetLegionContributeWindow(contributeLeftTimes, legionSumContribute, upGradeContribute, contributeProgress, memberNumContribute, boxGetSateList);
        }

        if (GameObject.Find("LegionMainHaveWindow") != null)
        {
            if (boxGetSateList.Count > 0 && contributeLeftTimes <= 0)
            {
                for (int i = 0; i < boxGetSateList.Count; i++)
                {
                    if (boxGetSateList[i] == 1)
                    {
                        contributeLeftTimes = 1;
                    }
                }
            }
            LegionMainHaveWindow _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow").GetComponent<LegionMainHaveWindow>();
            _LegionMainHaveWindow.SetContributeRedPoint(contributeLeftTimes);
        }
    }
    public void Process_8007(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //UIManager.instance.OpenPromptWindow("军团捐献成功", PromptWindow.PromptType.Hint, null, null);
            int contributeType = int.Parse(dataSplit[1]);
            int contribute = int.Parse(dataSplit[2]);
            int gold = int.Parse(dataSplit[3]);
            int diomand = int.Parse(dataSplit[4]);
            CharacterRecorder.instance.gold = gold;
            CharacterRecorder.instance.lunaGem = diomand;
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            if (GameObject.Find("LegionContributeWindow") != null)
            {
                LegionContributeWindow _LegionContributeWindow = GameObject.Find("LegionContributeWindow").GetComponent<LegionContributeWindow>();
                _LegionContributeWindow.ResetContributeInfo();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("不是军团成员", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("货币不足", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_8008(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("申请军团成功", PromptWindow.PromptType.Hint, null, null);
            if (GameObject.Find("LegionMainNoneWindow") != null)
            {
                NetworkHandler.instance.SendProcess(string.Format("8010#{0};", CharacterRecorder.instance.legionCountryID));

                //LegionMainNoneWindow _LegionMainNoneWindow = GameObject.Find("LegionMainNoneWindow").GetComponent<LegionMainNoneWindow>();
                //_LegionMainNoneWindow.partsList[0].GetComponent<LegionRankListPart>().ScueedApplayingResult();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("已申请过", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("无此军团", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("已是军团成员", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("军团人数达到上限", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("离开军团未满24小时", PromptWindow.PromptType.Hint, null, null); break;
                case "5": UIManager.instance.OpenPromptWindow("不能加入敌国军团", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_8009(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //UIManager.instance.OpenPromptWindow("离开军团成功", PromptWindow.PromptType.Hint, null, null);
            if (dataSplit[1] == "1")//自己离开
            {
                CharacterRecorder.instance.legionID = 0;
                UIManager.instance.OpenPromptWindow("离开军团成功", PromptWindow.PromptType.Hint, null, null);
                UIManager.instance.OpenPanel("MainWindow", true);
            }
            else if (dataSplit[1] == "2")//自己被踢
            {
                CharacterRecorder.instance.legionID = 0;
                UIManager.instance.OpenPromptWindow("您被踢出军团", PromptWindow.PromptType.Hint, null, null);
                GameObject legionMain = GameObject.Find("LegionMainHaveWindow");
                if (legionMain != null)
                {
                    UIManager.instance.OpenPromptWindow("您已经被踢出该军团.", PromptWindow.PromptType.Confirm, () =>
                    {
                        UIManager.instance.OpenPanel("MainWindow", true);
                    }, () =>
                    {
                        UIManager.instance.OpenPanel("MainWindow", true);
                    });
                }
            }
            else
            {
                NetworkHandler.instance.SendProcess(string.Format("8005#{0};", CharacterRecorder.instance.legionID));//踢出别人
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("不是军团成员 ", PromptWindow.PromptType.Hint, null, null); break;
                //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
                case "1":
                    UIManager.instance.OpenPromptWindow("权限不够 ", PromptWindow.PromptType.Hint, null, null);
                    SendProcess(string.Format("8004#{0};", CharacterRecorder.instance.legionID));
                    SendProcess(string.Format("8005#{0};", CharacterRecorder.instance.legionID));
                    break;
            }
        }
    }
    public void Process_8010(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<LegionItemData> _mlList = new List<LegionItemData>();
        _mlList.Clear();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            if (secSplit[5].Contains("_"))
            {
                string[] settingStateSplit = secSplit[5].Split('_');
                _mlList.Add(new LegionItemData(int.Parse(secSplit[0]), secSplit[1], secSplit[2], int.Parse(secSplit[3]), int.Parse(secSplit[4]), int.Parse(settingStateSplit[0]), int.Parse(settingStateSplit[1]), int.Parse(secSplit[6]), int.Parse(secSplit[7]), i + 1));
            }
            else
            {
                _mlList.Add(new LegionItemData(int.Parse(secSplit[0]), secSplit[1], secSplit[2], int.Parse(secSplit[3]), int.Parse(secSplit[4]), int.Parse(secSplit[6]), int.Parse(secSplit[7]), i + 1));
            }
            if (i == 0 && secSplit[0] != "")
            {
                CharacterRecorder.instance.FirstLegionName = secSplit[1];
            }
        }
        /* if (GameObject.Find("LegionJoinWindow") != null)
         {
             LegionJoinWindow _LegionJoinWindow = GameObject.Find("LegionJoinWindow").GetComponent<LegionJoinWindow>();
             Debug.Log("军团Count.." + _mlList.Count);
             _LegionJoinWindow.SetLegionJoinWindow(_mlList);
         } */
        GameObject legionHave = GameObject.Find("LegionMainHaveWindow");
        if (legionHave != null)
        {
            legionHave.GetComponent<LegionMainHaveWindow>().SetLegionRank(_mlList);
        }
        if (GameObject.Find("LegionMainNoneWindow") != null)
        {
            LegionMainNoneWindow _LegionMainNoneWindow = GameObject.Find("LegionMainNoneWindow").GetComponent<LegionMainNoneWindow>();
            Debug.Log("军团Count.." + _mlList.Count);
            _LegionMainNoneWindow.SetLegionSecondWindow(_mlList);
        }
        GameObject ls = GameObject.Find("LegionSecondWindow");
        if (ls != null)
        {
            LegionSecondWindow _LegionSecondWindow = ls.GetComponent<LegionSecondWindow>();
            if (_LegionSecondWindow.partsList[1].gameObject.activeSelf)
            {
                _LegionSecondWindow.partsList[1].GetComponent<LegionRankListPart>().SetLegionRankListPart(LegionDepth.LegionSecond, _mlList);
            }
            else if (_LegionSecondWindow.partsList[0].gameObject.activeSelf)
            {
                //Debug.LogError("长度：：：： " + _mlList.Count);
                _LegionSecondWindow.partsList[0].GetComponent<LegionBasicInfoPart>().SetLegionRankListPart(_mlList);
            }
        }
        if (GameObject.Find("RankListWindow") != null)
        {
            RankListWindow _LegionSecondWindow = GameObject.Find("RankListWindow").GetComponent<RankListWindow>();
            _LegionSecondWindow.partsList[1].GetComponent<LegionRankListPart>().SetLegionRankListPart(LegionDepth.LegionSecond, _mlList);
        }
        if (GameObject.Find("LegionRankListWindow") != null)
        {
            LegionRankListPart _LegionRankListPart = GameObject.Find("LegionRankListWindow").GetComponent<LegionRankListPart>();
            _LegionRankListPart.SetLegionRankListPart(LegionDepth.LegionSecond, _mlList);
        }
    }
    public void Process_8011(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "0")
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("没有此军团", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
        else
        {
            List<LegionItemData> _mlList = new List<LegionItemData>();
            _mlList.Clear();

            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                string[] secSplit = dataSplit[i].Split('$');
                if (secSplit[5].Contains("_"))
                {
                    string[] settingStateSplit = secSplit[5].Split('_');
                    _mlList.Add(new LegionItemData(int.Parse(secSplit[0]), secSplit[1], secSplit[2], int.Parse(secSplit[3]), int.Parse(secSplit[4]), int.Parse(settingStateSplit[0]), int.Parse(settingStateSplit[1]), int.Parse(secSplit[6]), int.Parse(secSplit[7]), i + 1));
                }
                else
                {
                    _mlList.Add(new LegionItemData(int.Parse(secSplit[0]), secSplit[1], secSplit[2], int.Parse(secSplit[3]), int.Parse(secSplit[4]), int.Parse(secSplit[6]), int.Parse(secSplit[7]), i + 1));
                }
            }
            if (GameObject.Find("LegionJoinWindow") != null)
            {
                LegionJoinWindow _LegionJoinWindow = GameObject.Find("LegionJoinWindow").GetComponent<LegionJoinWindow>();
                _LegionJoinWindow.SetLegionJoinWindow(_mlList);
            }
        }
    }
    public void Process_8012(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //UIManager.instance.OpenPromptWindow("军团申请列表成功", PromptWindow.PromptType.Hint, null, null);
        List<LegionMemberData> _mlList = new List<LegionMemberData>();//军团申请列表
        _mlList.Clear();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            int uId = int.Parse(secSplit[0]);
            string name = secSplit[1];
            int level = int.Parse(secSplit[2]);
            int vip = int.Parse(secSplit[3]);
            int fight = int.Parse(secSplit[4]);
            //int contribute = int.Parse(secSplit[5]);
            //string officialPosition = secSplit[6];
            int lastLoginTime = int.Parse(secSplit[5]);
            int iconHead = int.Parse(secSplit[6]);
            _mlList.Add(new LegionMemberData(uId, name, level, vip, fight, lastLoginTime, iconHead));
        }
        CharacterRecorder.instance.needChairmanDealCount = _mlList.Count;
        /* GameObject _MainWindowObj = GameObject.Find("MainWindow");
         if (_MainWindowObj != null)
         {
             _MainWindowObj.GetComponent<MainWindow>().SetLegionRedPoint(_mlList.Count);
         }*/
        if (GameObject.Find("LegionSecondWindow") != null)
        {
            LegionSecondWindow _LegionSecondWindow = GameObject.Find("LegionSecondWindow").GetComponent<LegionSecondWindow>();
            _LegionSecondWindow.partsList[2].GetComponent<CheckPart>().SetCheckListPart(_mlList);
            _LegionSecondWindow.SetRedPointOfCheckTab();
        }
    }
    public void Process_8013(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //UIManager.instance.OpenPromptWindow("军团通过申请成功", PromptWindow.PromptType.Hint, null, null);
            int uId = int.Parse(dataSplit[1]);
            int status = int.Parse(dataSplit[2]);//1 通过 2 拒绝
            int legionId = int.Parse(dataSplit[3]);//军团Id
            if (status == 1)
            {
                CharacterRecorder.instance.legionID = legionId;
                if (CharacterRecorder.instance.legionID != 0)
                {
                    NetworkHandler.instance.SendProcess("8201#;");
                }
                if (CharacterRecorder.instance.isOnlyApplayToJoinIn && GameObject.Find("LegionMainNoneWindow") != null)//此处10.18+
                {
                    UIManager.instance.BackUI();            //此处10.14注销，原军团同意后界面消失
                    UIManager.instance.OpenPanel("LegionMainHaveWindow", true);
                    CharacterRecorder.instance.isOnlyApplayToJoinIn = false;
                }
            }
            NetworkHandler.instance.SendProcess("8012#;");
        }
        else
        {
            NetworkHandler.instance.SendProcess("8012#;");
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("已是军团成员", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("职位太低", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("不是申请战队", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("军团人数达到上限", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("退出军团需24小时才能再加入(开服第一天只需1小时)", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_8014(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] dataSplit2 = dataSplit[0].Split('!');
        List<LegionLogItemData> _mlList = new List<LegionLogItemData>();//军团日志
        _mlList.Clear();
        for (int i = 0; i < dataSplit2.Length - 1; i++)
        {
            string[] secSplit = dataSplit2[i].Split('$');
            string name = secSplit[0];
            int logType = int.Parse(secSplit[1]);
            _mlList.Add(new LegionLogItemData(name, logType));
        }
        if (GameObject.Find("LegionSecondWindow") != null)
        {
            _mlList.Reverse();
            LegionSecondWindow _LegionSecondWindow = GameObject.Find("LegionSecondWindow").GetComponent<LegionSecondWindow>();
            _LegionSecondWindow.partsList[3].GetComponent<LegionLogPart>().SetLegionLogPart(_mlList);
        }
    }
    public void Process_8015(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            string[] dataSplit2 = dataSplit[1].Split('!');
            List<Item> _itemList = new List<Item>();
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, _itemList);
            UpDateTopContentData(_itemList);
            NetworkHandler.instance.SendProcess("8006#;");
        }
        else
        {
            //UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[0], PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_8016(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0].Contains("_"))
        {
            string[] dataSplit2 = dataSplit[0].Split('_');
            if (GameObject.Find("LegionSecondWindow") != null)
            {
                LegionSecondWindow _LegionSecondWindow = GameObject.Find("LegionSecondWindow").GetComponent<LegionSecondWindow>();
                _LegionSecondWindow.partsList[2].GetComponent<CheckPart>().SetSettingStates(int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]));
            }
        }
        else
        {
            if (GameObject.Find("LegionSecondWindow") != null)
            {
                LegionSecondWindow _LegionSecondWindow = GameObject.Find("LegionSecondWindow").GetComponent<LegionSecondWindow>();
                _LegionSecondWindow.partsList[2].GetComponent<CheckPart>().SetSettingStates(1, 0);
            }
        }
    }
    public void Process_8017(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0].Contains("_"))
        {
            string[] dataSplit2 = dataSplit[0].Split('_');
            if (GameObject.Find("LegionSecondWindow") != null)
            {
                LegionSecondWindow _LegionSecondWindow = GameObject.Find("LegionSecondWindow").GetComponent<LegionSecondWindow>();
                _LegionSecondWindow.partsList[2].GetComponent<CheckPart>().SetSettingStates(int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]));
            }
        }
    }
    public void Process_8018(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //List<LegionTrain> _mlList = new List<LegionTrain>();//军团训练场
        //_mlList.Clear();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            if (!dataSplit[i].Contains("$"))
            {
                int trainID = i + 1;
                int status = int.Parse(dataSplit[i]);
                // _mlList.Add(new LegionTrain(status, i + 1, 0, 0));
                TextTranslator.instance.GetLegionTrainByID(trainID).state = status;
            }
            else
            {
                int trainID = i + 1;
                string[] secSplit = dataSplit[i].Split('$');
                int status = 2;
                // _mlList.Add(new LegionTrain(status, i + 1, int.Parse(secSplit[0]), int.Parse(secSplit[1])));
                LegionTrain _LegionTrain = TextTranslator.instance.GetLegionTrainByID(trainID);
                _LegionTrain.SetLegionTrainSeverData(status, int.Parse(secSplit[0]), int.Parse(secSplit[1]), int.Parse(secSplit[2]));
            }
        }
        for (int i = 0; i < TextTranslator.instance.LegionTrainList.Count; i++)
        {
            if (TextTranslator.instance.LegionTrainList[i].mHero != null)
            {
                if (!LegionTrainingGroundWindow.mOnLineTrainingHeroList.Contains(TextTranslator.instance.LegionTrainList[i].mHero))
                {
                    LegionTrainingGroundWindow.mOnLineTrainingHeroList.Add(TextTranslator.instance.LegionTrainList[i].mHero);
                }
            }
        }
        if (GameObject.Find("LegionTrainingGroundWindow") != null)
        {
            LegionTrainingGroundWindow _LegionSecondWindow = GameObject.Find("LegionTrainingGroundWindow").GetComponent<LegionTrainingGroundWindow>();
            _LegionSecondWindow.SetLegionTrainingGroundWindow(TextTranslator.instance.LegionTrainList);
        }
        if (GameObject.Find("LegionMainHaveWindow") != null)
        {
            LegionMainHaveWindow _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow").GetComponent<LegionMainHaveWindow>();
            _LegionMainHaveWindow.SetTrainRedPoint();
        }
    }
    public void Process_8019(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            string[] dataSplit2 = dataSplit[2].Split('$');
            /* List<Item> _itemList = new List<Item>();
             for (int i = 0; i < dataSplit2.Length - 1; i++)
             {
                 string[] dataSplit3 = dataSplit2[i].Split('$');
                 _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
             }*/
            GameObject.Find("LegionTrainingGroundWindow").GetComponent<LegionTrainingGroundWindow>().ResetLegionTrainingGroundWindow(int.Parse(dataSplit[1]), int.Parse(dataSplit2[0]), int.Parse(dataSplit2[1]));
        }
        else
        {
            //UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[0], PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_8020(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            GameObject.Find("LegionTrainingGroundWindow").GetComponent<LegionTrainingGroundWindow>().ResetLegionTrainingGroundWindow(int.Parse(dataSplit[1]));
        }
        else
        {
            //UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null);
            switch (dataSplit[1])
            {
                //case "0": UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
                //case "1": UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_8021(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.legionFlag = int.Parse(dataSplit[1]);
            GameObject _LegionFlagSettingWindow = GameObject.Find("LegionFlagSettingWindow");
            if (_LegionFlagSettingWindow != null)
            {
                _LegionFlagSettingWindow.GetComponent<LegionFlagSettingWindow>().ResetFlagResult();
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("修改旗帜失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_8022(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.LegionAnouncement = dataSplit[0];
        GameObject _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow");
        if (_LegionMainHaveWindow != null)
        {
            _LegionMainHaveWindow.GetComponent<LegionMainHaveWindow>().SetAnouncedInfo(dataSplit[0]);
        }
    }
    public void Process_8023(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.LegionAnouncement = dataSplit[1];
            GameObject _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow");
            if (_LegionMainHaveWindow != null)
            {
                _LegionMainHaveWindow.GetComponent<LegionMainHaveWindow>().ResetAnouncedInfo(dataSplit[1]);
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("修改军团公告失败.", PromptWindow.PromptType.Hint, null, null);
        }
    }


    public void Process_8102(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            int diamondnum = CharacterRecorder.instance.lunaGem;
            CharacterRecorder.instance.legionCountryID = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            TopContent tp = GameObject.Find("TopContent").GetComponent<TopContent>();
            if (tp != null)
            {
                tp.Reset();
            }
            if (GameObject.Find("CountryWarWindow") != null)
            {
                DestroyImmediate(GameObject.Find("CountryWarWindow"));
            }
            UIManager.instance.OpenSinglePanel("JoinCountryWindow", false);

            if (int.Parse(dataSplit[2]) - diamondnum > 0)
            {
                List<Item> _itemList = new List<Item>();
                _itemList.Add(new Item(90002, int.Parse(dataSplit[2]) - diamondnum));
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, _itemList);
            }
        }
        else
        {
            Debug.Log("失败");
        }
    }
    public void Process_8103(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPanel("HarassResultWindow", false);
            GameObject.Find("HarassResultWindow").GetComponent<HarassResultWindow>().MilitaryRankEffect(true, int.Parse(dataSplit[2]));
        }
        else
        {
            UIManager.instance.OpenPanel("HarassResultWindow", false);
            GameObject.Find("HarassResultWindow").GetComponent<HarassResultWindow>().MilitaryRankEffect(false, 0);
        }
    }
    public void Process_8104(string RecvString)
    {
        //string[] dataSplit = RecvString.Split(';');
        MilitaryRankWindow Mw = GameObject.Find("MilitaryRankWindow").GetComponent<MilitaryRankWindow>();
        if (Mw != null)
        {
            Mw.AddNationType(RecvString);
        }
    }

    public void Process_8105(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.legionCountryID = int.Parse(dataSplit[1]);
            if (GameObject.Find("BagWindow") != null)
            {
                GameObject bag = GameObject.Find("BagWindow");
                BagWindow bw = bag.GetComponent<BagWindow>();
                bw.UpDataBag();
            }
            UIManager.instance.OpenPromptWindow("成功叛国", PromptWindow.PromptType.Hint, null, null);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1": UIManager.instance.OpenPromptWindow("未加入国家", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("无道具", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("请先退出军团", PromptWindow.PromptType.Hint, null, null); break;
                default:
                    break;
            }
        }
    }

    public void Process_8201(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        PlayerPrefs.SetInt("LegionGroupID", int.Parse(dataSplit[1]));//当前大关  yy 11.22
        CharacterRecorder.instance.legionFightLeftTimes = int.Parse(dataSplit[0]);
        GameObject _LegionCopyWindow = GameObject.Find("LegionCopyWindow");
        if (_LegionCopyWindow != null)
        {
            _LegionCopyWindow.GetComponent<LegionCopyWindow>().SetLegionCopyWindow(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
        GameObject _LegionCopyWindowNew = GameObject.Find("LegionCopyWindowNew");
        if (_LegionCopyWindowNew != null)
        {
            _LegionCopyWindowNew.GetComponent<LegionCopyWindowNew>().SetLegionCopyWindow(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
        GameObject _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow");
        if (_LegionMainHaveWindow != null)
        {
            _LegionMainHaveWindow.GetComponent<LegionMainHaveWindow>().GetBigState(int.Parse(dataSplit[1]));
        }
    }
    public void Process_8202(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        GameObject _LegionGachaEnterWindow = GameObject.Find("LegionGachaEnterWindow");
        if (_LegionGachaEnterWindow != null)
        {
            BetterList<int> percentList = new BetterList<int>();
            percentList.Add(int.Parse(dataSplit[2]));
            percentList.Add(int.Parse(dataSplit[3]));
            percentList.Add(int.Parse(dataSplit[4]));
            percentList.Add(int.Parse(dataSplit[5]));
            percentList.Add(int.Parse(dataSplit[6]));
            _LegionGachaEnterWindow.GetComponent<LegionGachaEnterWindow>().SetLegionGachaEnterWindow(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), percentList);
        }
        GameObject _LegionCopyWindowNew = GameObject.Find("LegionCopyWindowNew");
        if (_LegionCopyWindowNew != null)
        {
            BetterList<int> percentList = new BetterList<int>();
            percentList.Add(int.Parse(dataSplit[2]));
            percentList.Add(int.Parse(dataSplit[3]));
            percentList.Add(int.Parse(dataSplit[4]));
            percentList.Add(int.Parse(dataSplit[5]));
            percentList.Add(int.Parse(dataSplit[6]));
            _LegionCopyWindowNew.GetComponent<LegionCopyWindowNew>().SetLegionGachaEnterWindow(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), percentList);
        }
        GameObject _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow");
        if (_LegionMainHaveWindow != null)
        {
            BetterList<int> percentList = new BetterList<int>();
            percentList.Add(int.Parse(dataSplit[2]));
            percentList.Add(int.Parse(dataSplit[3]));
            percentList.Add(int.Parse(dataSplit[4]));
            percentList.Add(int.Parse(dataSplit[5]));
            percentList.Add(int.Parse(dataSplit[6]));
            _LegionMainHaveWindow.GetComponent<LegionMainHaveWindow>().GetAllNodeState(int.Parse(dataSplit[1]), percentList);
        }
        if (int.Parse(dataSplit[1]) == PlayerPrefs.GetInt("LegionGroupID")) //yy  11.22
        {
            int num = 0;
            if (int.Parse(dataSplit[2]) == 100) num++;
            if (int.Parse(dataSplit[3]) == 100) num++;
            if (int.Parse(dataSplit[4]) == 100) num++;
            if (int.Parse(dataSplit[5]) == 100) num++;
            if (int.Parse(dataSplit[6]) == 100) num++;

            PlayerPrefs.SetInt("LegionGateIDNum", num); //当前大关小关通关数量
        }
    }
    public void Process_8303(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            string[] dataSplit1 = dataSplit[1].Split('$');
            BetterList<int> gachaState = new BetterList<int>();//0不可领1可领2已领
            for (int i = 0; i < dataSplit1.Length - 1; i++)
            {
                gachaState.Add(int.Parse(dataSplit1[i]));
            }
            string[] dataSplit2 = dataSplit[2].Split('!');
            BetterList<LegionGachaItemData> gachaAwardList = new BetterList<LegionGachaItemData>();//已刮开奖励
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit22 = dataSplit2[i].Split('$');
                gachaAwardList.Add(new LegionGachaItemData(int.Parse(dataSplit22[0]), dataSplit22[1], int.Parse(dataSplit22[2])));
            }
            GameObject _LegionGachaWindow = GameObject.Find("LegionGachaWindow");
            if (_LegionGachaWindow != null)
            {
                _LegionGachaWindow.GetComponent<LegionGachaWindow>().SetLegionGachaWindow(gachaState, gachaAwardList);
            }

            GameObject _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow");
            int count = 0;
            if (_LegionMainHaveWindow != null)
            {
                for (int i = 0; i < gachaState.size; i++)
                {
                    if (gachaState[i] == 1)//表示有可以刮奖的
                    {
                        count++;
                        CharacterRecorder.instance.SetRedPoint(52, true);
                        _LegionMainHaveWindow.GetComponent<LegionMainHaveWindow>().SetRedPoint(true);
                        break;
                    }
                }
                if (count == 0)
                {
                    CharacterRecorder.instance.SetRedPoint(52, false);
                    _LegionMainHaveWindow.GetComponent<LegionMainHaveWindow>().SetRedPoint(false);
                }
            }
        }
        else
        {
            //UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[0], PromptWindow.PromptType.Hint, null, null);
            /* switch (dataSplit[1])
             {
                 default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
             }*/
        }
    }

    public void Process_8304(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        int bigGate = int.Parse(dataSplit[0]);
        int smallGate = int.Parse(dataSplit[1]);
        int position = int.Parse(dataSplit[2]);
        int awardNum = int.Parse(dataSplit[3]);
        GameObject _LegionGachaWindow = GameObject.Find("LegionGachaWindow");
        if (_LegionGachaWindow != null)
        {
            _LegionGachaWindow.GetComponent<LegionGachaWindow>().ResetLegionGachaWindow(bigGate, smallGate, position, awardNum);
        }

    }
    public void Process_8305(string RecvString)
    {
        UIManager.instance.OpenPanel("LoadingWindow", true);
        PictureCreater.instance.FightStyle = 12;
        PictureCreater.instance.StartLegionGate(RecvString);
    }
    public void Process_8306(string RecvString)//1705花式军演
    {
        string[] dataSplit = RecvString.Split(';');
        //CharacterRecorder.instance.IsOpen = true;
        if (dataSplit[0] == "0")
        {
            AudioEditer.instance.PlayLoop("Lose");
            SendProcess("2101#1;");
            //UIManager.instance.OpenPanel("ResultWindow", false);
            //ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
            //rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
        }
        else
        {
            AudioEditer.instance.PlayLoop("Win");
            SendProcess("2101#1;");
            //UIManager.instance.OpenPanel("ResultWindow", false);
            //ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
            //rw.Init(true, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
        }
    }
    public void Process_8307(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<LegionMemberData> _mlList = new List<LegionMemberData>();
        _mlList.Clear();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            if (dataSplit[i].Contains('$'))
            {
                string[] secSplit = dataSplit[i].Split('$');
                int uId = int.Parse(secSplit[0]);
                string name = secSplit[1];
                int level = int.Parse(secSplit[2]);
                int vip = int.Parse(secSplit[3]);
                int fight = int.Parse(secSplit[4]);
                int contribute = int.Parse(secSplit[5]);
                int officialPosition = int.Parse(secSplit[6]);
                int sumHert = int.Parse(secSplit[7]);
                int iconHead = int.Parse(secSplit[8]);
                _mlList.Add(new LegionMemberData(i + 1, uId, name, level, vip, fight, contribute, officialPosition, sumHert, iconHead));
            }
        }
        if (GameObject.Find("LegionHertRankWindow") != null)
        {
            LegionHertRankWindow _LegionHertRankWindow = GameObject.Find("LegionHertRankWindow").GetComponent<LegionHertRankWindow>();
            _LegionHertRankWindow.GetComponent<LegionHertRankWindow>().SetLegionHertRankWindow(_mlList);
        }
    }
    public void Process_8308(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("购买成功", PromptWindow.PromptType.Hint, null, null);
            CharacterRecorder.instance.legionFightLeftTimes = int.Parse(dataSplit[0]);
            GameObject _LegionCopyWindow = GameObject.Find("LegionCopyWindow");
            if (_LegionCopyWindow != null)
            {
                _LegionCopyWindow.GetComponent<LegionCopyWindow>().ResetLegionFightTimes(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            }
            GameObject _LegionCopyWindowNew = GameObject.Find("LegionCopyWindowNew");
            if (_LegionCopyWindowNew != null)
            {
                _LegionCopyWindowNew.GetComponent<LegionCopyWindowNew>().ResetLegionFightTimes(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            }
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("购买次数不足,提升VIP可增加购买次数", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("钻石不足，无法购买军团副本挑战次数", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow(string.Format("服务器错误码{0}", dataSplit[1]), PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_8309(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<LegionPassData> _mList = new List<LegionPassData>();

        int awardStatus = int.Parse(dataSplit[0]);
        for (int i = 1; i < dataSplit.Length - 1; i++)
        {
            if (dataSplit[i].Contains('$'))
            {
                string[] secSplit = dataSplit[i].Split('$');
                //公会ID$公会名称$公会旗$击杀时间
                _mList.Add(new LegionPassData(i, int.Parse(secSplit[0]), secSplit[1], int.Parse(secSplit[2]), int.Parse(secSplit[3])));
            }
        }
        GameObject lr = GameObject.Find("LegionHertRankWindow");
        if (lr != null)
        {
            LegionHertRankWindow _LegionHertRankWindow = lr.GetComponent<LegionHertRankWindow>();
            if (_LegionHertRankWindow != null)
            {
                //Debug.LogError("LegionHertRankWindow:::" + awardStatus);
                if (!_LegionHertRankWindow.ListTabs[1].value)
                {
                    _LegionHertRankWindow.SetLegionPassRankWindow(awardStatus, _mList);
                }
                _LegionHertRankWindow.SetRedPoint(awardStatus);
            }

        }
        //设置排行的红点
        GameObject lc = GameObject.Find("LegionCopyWindowNew");
        if (lc != null)
        {
            lc.GetComponent<LegionCopyWindowNew>().SetHertRankButtonRedPoint(awardStatus, _mList);
        }
    }


    private void Process_8310(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        List<Item> _itemList = new List<Item>();
        string[] dataSplit1 = dataSplit[1].Split('!');
        for (int i = 0; i < dataSplit1.Length - 1; i++)
        {
            string[] trcSplit = dataSplit1[i].Split('$');
            Item _item = new Item(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]));
            if (_item.itemCount > 0)
            {
                _itemList.Add(_item);
            }
        }

        if (GameObject.Find("LegionHertRankWindow") != null)
        {
            LegionHertRankWindow _LegionHertRankWindow = GameObject.Find("LegionHertRankWindow").GetComponent<LegionHertRankWindow>();
            _LegionHertRankWindow.GetComponent<LegionHertRankWindow>().GetLegionPassAwardResult(int.Parse(dataSplit[0]), _itemList);
        }
    }
    public void Process_8401(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        int todayLeftTimes = int.Parse(dataSplit[0]);
        int refreshTimes = int.Parse(dataSplit[1]);
        BetterList<int> taskIdList = new BetterList<int>();
        string[] dataSplitID = dataSplit[2].Split('!');
        for (int i = 0; i < dataSplitID.Length - 1; i++)
        {
            taskIdList.Add(int.Parse(dataSplitID[i]));
        }
        if (GameObject.Find("LegionTaskWindow") != null)
        {
            LegionTaskWindow _LegionHertRankWindow = GameObject.Find("LegionTaskWindow").GetComponent<LegionTaskWindow>();
            _LegionHertRankWindow.GetComponent<LegionTaskWindow>().SetLegionTaskWindow(todayLeftTimes, refreshTimes, taskIdList);
        }
        if (GameObject.Find("LegionMainHaveWindow") != null)
        {
            LegionMainHaveWindow _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow").GetComponent<LegionMainHaveWindow>();
            _LegionMainHaveWindow.SetTaskRedPoint(todayLeftTimes);
        }
    }
    public void Process_8402(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            NetworkHandler.instance.SendProcess("8401#;");
            string[] dataSplit2 = dataSplit[1].Split('!');
            List<Item> _itemList = new List<Item>();
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
            }
            UpDateTopContentData(_itemList);
            GameObject _QuestionWindow = GameObject.Find("LegionTaskWindow");
            if (_QuestionWindow != null)
            {
                _QuestionWindow.GetComponent<LegionTaskWindow>().OpenGainWindow(_itemList);
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("今日任务次数已用完", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_8403(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            NetworkHandler.instance.SendProcess("8401#;");
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
        else
        {
            UIManager.instance.OpenPromptWindow("刷新任务失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_8501(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LegionDiceWindow") != null)
        {
            GameObject.Find("LegionDiceWindow").GetComponent<LegionDiceWindow>().LegionGetDice(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
        }
        if (GameObject.Find("LegionMainHaveWindow") != null)
        {
            LegionMainHaveWindow _LegionMainHaveWindow = GameObject.Find("LegionMainHaveWindow").GetComponent<LegionMainHaveWindow>();
            _LegionMainHaveWindow.SetDiceRedPoint(int.Parse(dataSplit[1]));
        }
    }
    public void Process_8502(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "0")
        {
            if (GameObject.Find("LegionDiceWindow") != null)
            {
                GameObject.Find("LegionDiceWindow").GetComponent<LegionDiceWindow>().LegionSetDice(RecvString);
            }
        }
        else if (dataSplit[0] == "0")
        {
            UIManager.instance.OpenPromptWindow("次数不足", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_8503(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "0")
        {
            if (GameObject.Find("LegionDiceWindow") != null)
            {
                GameObject.Find("LegionDiceWindow").GetComponent<LegionDiceWindow>().LegionChangeDice(RecvString);
            }
        }
        else if (dataSplit[0] == "0")
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("VIP等级不足", PromptWindow.PromptType.Hint, null, null);
            }
        }

    }

    public void Process_8504(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> _itemList = new List<Item>();
            string[] dataSplit1 = dataSplit[1].Split('!');
            for (int i = 0; i < dataSplit1.Length - 1; i++)
            {
                string[] trcSplit = dataSplit1[i].Split('$');
                Item _item = new Item(int.Parse(trcSplit[0]), int.Parse(trcSplit[1]));
                _itemList.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, _itemList);
            if (GameObject.Find("LegionDiceWindow") != null)
            {
                GameObject.Find("LegionDiceWindow").GetComponent<LegionDiceWindow>().ChangeDicAfterGetAward();
            }

            CharacterRecorder.instance.gold = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            Debug.Log("领取失败");
        }
    }

    public void Process_8506(string RecvString)
    {
        if (GameObject.Find("LegionDiceRankWindow") != null)
        {
            GameObject.Find("LegionDiceRankWindow").GetComponent<LegionDiceRankWindow>().LegionGetDicelist(RecvString);
        }
        else if (GameObject.Find("LegionDiceWindow") != null)
        {
            GameObject.Find("LegionDiceWindow").GetComponent<LegionDiceWindow>().EverydayFirst(RecvString);
        }
    }


    public void Process_8601(string RecvString)
    {
        //string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LegionWarWindow") != null)
        {
            GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarGetScene(RecvString);
        }
    }

    public void Process_8602(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        bool Ishave = false;
        for (int i = 0; i < CharacterRecorder.instance.LegionwarGetnodeList.Count; i++)  //刷新单个城市点信息
        {
            if (CharacterRecorder.instance.LegionwarGetnodeList[i].LegionPoint == int.Parse(dataSplit[0]))
            {
                Ishave = true;
                CharacterRecorder.instance.LegionwarGetnodeList[i] = new LegionwarGetnode(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), dataSplit[2], int.Parse(dataSplit[3]), dataSplit[4], int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]), int.Parse(dataSplit[8]), dataSplit[9], int.Parse(dataSplit[10]), int.Parse(dataSplit[11]), int.Parse(dataSplit[12]));
                break;
            }
        }

        if (Ishave == false)
        {
            CharacterRecorder.instance.LegionwarGetnodeList.Add(new LegionwarGetnode(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), dataSplit[2], int.Parse(dataSplit[3]), dataSplit[4], int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]), int.Parse(dataSplit[8]), dataSplit[9], int.Parse(dataSplit[10]), int.Parse(dataSplit[11]), int.Parse(dataSplit[12])));
        }
        GameObject Lw = GameObject.Find("LegionWarWindow");
        if (Lw != null)
        {
            //GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarGetNode(RecvString);
            if (CharacterRecorder.instance.AutomaticbrushCityID == int.Parse(dataSplit[0]) && CharacterRecorder.instance.AutomaticbrushCity)//自动移动点
            {
                Lw.GetComponent<LegionWarWindow>().AutoClickTeamHero1();
            }
            else //非自动移动点，弹出圆形选择窗口
            {
                Lw.GetComponent<LegionWarWindow>().LegionWarGetNode(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), dataSplit[2], int.Parse(dataSplit[3]), dataSplit[4], int.Parse(dataSplit[5]), int.Parse(dataSplit[6]), int.Parse(dataSplit[7]), int.Parse(dataSplit[8]), dataSplit[9], int.Parse(dataSplit[10]), int.Parse(dataSplit[11]), int.Parse(dataSplit[12]), dataSplit[13]);
            }

        }
    }

    public void Process_8603(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.LegionLeftOrRight = int.Parse(dataSplit[2]);
            if (GameObject.Find("LegionWarWindow") != null && dataSplit[2] == "0")
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarStartWar(true, int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            }
            else
            {
                UIManager.instance.OpenPromptWindow("宣战成功！20：30开始军团战！", PromptWindow.PromptType.Hint, null, null);
            }
        }
        else
        {
            if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("您所在的军团已经宣战", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "4")
            {
                UIManager.instance.OpenPromptWindow("其他军团已经宣战", PromptWindow.PromptType.Hint, null, null);
            }
        }

    }

    public void Process_8604(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("LegionFightWindow") != null)
            {
                GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarAutofight(int.Parse(dataSplit[1]));
            }
        }
    }

    public void Process_8605(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("LegionFightWindow") != null)
            {
                GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarAutorevive(int.Parse(dataSplit[1]));
            }
        }
    }

    public void Process_8606(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("出兵成功", PromptWindow.PromptType.Hint, null, null);
            SendProcess("8608#" + CharacterRecorder.instance.LegionHarasPoint + ";");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("出兵失败", PromptWindow.PromptType.Hint, null, null);
        }

    }

    public void Process_8607(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("复活成功", PromptWindow.PromptType.Hint, null, null);
            if (GameObject.Find("LegionPositionWindow") != null)
            {
                GameObject.Find("LegionPositionWindow").GetComponent<LegionPositionWindow>().LegionWarRevive(int.Parse(dataSplit[1]));
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("复活失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_8608(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (GameObject.Find("LegionFightWindow") != null)
        {
            if (CharacterRecorder.instance.IsNoFightEntLegion)
            {
                GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarGetWarNoFight(RecvString);
            }
            else
            {
                if (CharacterRecorder.instance.IsfirstEntLegion)
                {
                    GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarGetWar(RecvString);
                }
                else
                {
                    GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarGetWarMoce(RecvString);
                }
            }
        }
    }

    public void Process_8609(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LegionFightWindow") != null)
        {
            if (int.Parse(dataSplit[3]) == CharacterRecorder.instance.LegionHarasPoint) //战斗结果是否为对应的点
            {
                GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarGetWarResult(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            }
            //GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarGetWarResult(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
    }

    public void Process_8610(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[1].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            if (GameObject.Find("LegionWarWindow") != null)
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().GetRewardList(";");
            }
        }
        else
        {

        }
    }

    public void Process_8611(string RecvString)
    {
        if (GameObject.Find("LegionWarWindow") != null)
        {
            GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().GetRewardList(RecvString);
        }
    }

    public void Process_8612(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //SendProcess("8613#" + CharacterRecorder.instance.LegionHarasPoint + ";");
            if (GameObject.Find("LegionWarWindow") != null)
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().GetActionPoint(int.Parse(dataSplit[1]));
            }

            SendProcess("8613#" + CharacterRecorder.instance.LegionHarasPoint + ";");
        }
        else
        {
            CharacterRecorder.instance.AutomaticbrushCityID = 0;
            if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("每天只可占领10个D级城市", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("布阵成功", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("您不可以骚扰同属国家军团所占领的据点", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_8613(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) > 0)
        {
            CharacterRecorder.instance.ReinforcementNum = int.Parse(dataSplit[0]);
            UIManager.instance.OpenPanel("LoadingWindow", true);
            PictureCreater.instance.FightStyle = 14;
            SceneTransformer.instance.NowGateID = 10001;
            PictureCreater.instance.StartLegionWar();
        }
        else
        {
            UIManager.instance.OpenPromptWindow("守军数量为0,不可骚扰!", PromptWindow.PromptType.Hint, null, null);
            SendProcess("8601#;");
        }
    }

    public void Process_8614(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("LoadingWindow") == null)
            {
                UIManager.instance.OpenSinglePanel("HarassResultWindow", false);
                GameObject.Find("HarassResultWindow").GetComponent<HarassResultWindow>().NextEnemy();
            }
            CharacterRecorder.instance.ReinforcementNum = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.LegionFestPosition = "";
            //for (int i = 3; i < dataSplit.Length - 1; i++)
            //{
            //    CharacterRecorder.instance.LegionFestPosition += dataSplit[i] + ";";
            //}

            if (dataSplit[3] != "")
            {
                for (int i = 3; i < dataSplit.Length - 2; i++)
                {
                    CharacterRecorder.instance.LegionFestPosition += dataSplit[i] + ";";
                }
                //CharacterRecorder.instance.LegionFestPosition += dataSplit[3] + ";";
            }
            Debug.LogError(CharacterRecorder.instance.LegionFestPosition);
            if (CharacterRecorder.instance.LegionFestPosition != "")
            {
                Debug.LogError("创建玩家数据 " + dataSplit[dataSplit.Length - 2]);
                PictureCreater.instance.CreateLegionWar(CharacterRecorder.instance.LegionFestPosition, dataSplit[dataSplit.Length - 2]);
            }
            else
            {
                Debug.Log("AAAAAAAAAAA");
                string LegionWar = "";
                int MaxLevel = int.Parse(dataSplit[2]);
                LegionCity LC = TextTranslator.instance.GetLegionCityByID(CharacterRecorder.instance.LegionHarasPoint);
                LegionWar += "60009$29$" + LC.MonsterHp * MaxLevel + "$" + LC.MonsterAtk * MaxLevel + "$" + LC.MonsterDef * MaxLevel + "$" + LC.MonsterCrit + "$" + LC.MonsterNoCrit + "$" + LC.MonsterNoHit + "$" + (1000 + MaxLevel * 10).ToString() + "$" + LC.MonsterDmgBonus + "$" + LC.MonsterDmgReduction + "$" + dataSplit[2] + "$0$1$0$" + LC.MonsterHp * MaxLevel + "$0$114$0$0;";
                LegionWar += "60007$28$" + LC.MonsterHp * MaxLevel + "$" + LC.MonsterAtk * MaxLevel + "$" + LC.MonsterDef * MaxLevel + "$" + LC.MonsterCrit + "$" + LC.MonsterNoCrit + "$" + LC.MonsterNoHit + "$" + (1000 + MaxLevel * 10).ToString() + "$" + LC.MonsterDmgBonus + "$" + LC.MonsterDmgReduction + "$" + dataSplit[2] + "$0$1$0$" + LC.MonsterHp * MaxLevel + "$0$108$0$0;";
                LegionWar += "60001$31$" + LC.MonsterHp * MaxLevel + "$" + LC.MonsterAtk * MaxLevel + "$" + LC.MonsterDef * MaxLevel + "$" + LC.MonsterCrit + "$" + LC.MonsterNoCrit + "$" + LC.MonsterNoHit + "$" + (1000 + MaxLevel * 10).ToString() + "$" + LC.MonsterDmgBonus + "$" + LC.MonsterDmgReduction + "$" + dataSplit[2] + "$0$1$0$" + LC.MonsterHp * MaxLevel + "$0$116$0$0;";
                LegionWar += "60002$32$" + LC.MonsterHp * MaxLevel + "$" + LC.MonsterAtk * MaxLevel + "$" + LC.MonsterDef * MaxLevel + "$" + LC.MonsterCrit + "$" + LC.MonsterNoCrit + "$" + LC.MonsterNoHit + "$" + (1000 + MaxLevel * 10).ToString() + "$" + LC.MonsterDmgBonus + "$" + LC.MonsterDmgReduction + "$" + dataSplit[2] + "$0$1$0$" + LC.MonsterHp * MaxLevel + "$0$95$0$0;";
                LegionWar += "60008$25$" + LC.MonsterHp * MaxLevel + "$" + LC.MonsterAtk * MaxLevel + "$" + LC.MonsterDef * MaxLevel + "$" + LC.MonsterCrit + "$" + LC.MonsterNoCrit + "$" + LC.MonsterNoHit + "$" + (1000 + MaxLevel * 10).ToString() + "$" + LC.MonsterDmgBonus + "$" + LC.MonsterDmgReduction + "$" + dataSplit[2] + "$0$1$0$" + LC.MonsterHp * MaxLevel + "$0$100$0$0;";
                LegionWar += "60004$33$" + LC.MonsterHp * MaxLevel + "$" + LC.MonsterAtk * MaxLevel + "$" + LC.MonsterDef * MaxLevel + "$" + LC.MonsterCrit + "$" + LC.MonsterNoCrit + "$" + LC.MonsterNoHit + "$" + (1000 + MaxLevel * 10).ToString() + "$" + LC.MonsterDmgBonus + "$" + LC.MonsterDmgReduction + "$" + dataSplit[2] + "$0$1$0$" + LC.MonsterHp * MaxLevel + "$0$102$0$0;";
                PictureCreater.instance.CreateLegionWar(LegionWar, "守卫军");
                Debug.LogError("创建守卫军数据 ");
                Debug.Log("AAAAAAAAAAABBBBBBb");
            }
            //SendProcess("8615#" + +CharacterRecorder.instance.LegionHarasPoint + ";" + "0;");
        }
        else
        {

        }
    }

    public void Process_8615(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (dataSplit[1] == "1") //已占领
            {
                Debug.LogError("已占领");
                UIManager.instance.OpenSinglePanel("HarassResultWindow", false);
                GameObject HR = GameObject.Find("HarassResultWindow");
                if (HR != null)
                {
                    if (int.Parse(dataSplit[3]) <= 10 && CharacterRecorder.instance.LegionHarasPoint != 40)
                    {
                        HR.GetComponent<HarassResultWindow>().LegionWarStartWar(true);
                    }
                    else
                    {
                        HR.GetComponent<HarassResultWindow>().TakeTenCityResultWithWin();
                    }
                }
                SendProcess("7002#18;" + CharacterRecorder.instance.characterName + ";" + CharacterRecorder.instance.myLegionData.legionName + ";" + TextTranslator.instance.GetLegionCityByID(CharacterRecorder.instance.LegionHarasPoint).CityName + ";");
            }
            else
            {
                if (dataSplit[2] == "0")//打输
                {
                    Debug.LogError("已打输");
                    //PictureCreater.instance.StopFight(true);
                    //UIManager.instance.BackUI();
                    UIManager.instance.OpenSinglePanel("HarassResultWindow", false);
                    GameObject HR = GameObject.Find("HarassResultWindow");
                    if (HR != null)
                    {
                        HR.GetComponent<HarassResultWindow>().LegionWarStartWar(false);
                    }
                }
                else
                {
                    PictureCreater.instance.DestroyEnemyComponent();
                    NetworkHandler.instance.SendProcess("8614#" + CharacterRecorder.instance.LegionHarasPoint + ";");
                }
            }
        }
        else
        {
            Debug.LogError("已占领");
            UIManager.instance.OpenSinglePanel("HarassResultWindow", false);
            GameObject HR = GameObject.Find("HarassResultWindow");
            if (HR != null)
            {
                if (dataSplit[1] == "0")
                {
                    HR.GetComponent<HarassResultWindow>().LegionWarStartWar();  //过了时间,占领失败
                }
                else
                {
                    HR.GetComponent<HarassResultWindow>().LegionWarStartWar(true);
                }
            }
        }
    }

    public void Process_8616(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("LegionWarWindow") != null)
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarSetDefend(int.Parse(dataSplit[1]));
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("行动力不足,无法布阵防守", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_8617(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "")
        {
            CharacterRecorder.instance.LegionPositonStr = dataSplit[0];
            //if (GameObject.Find("LegionWarWindow") != null)
            //{
            //    LegionWarWindow lw = GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>();
            //    lw.mTeamPosition.Clear();
            //    string[] dataSplit1 = dataSplit[1].Split('!');

            //    for (int i = 0; i < dataSplit1.Length - 1; i++)
            //    {
            //        string[] secSplit = dataSplit1[i].Split('$');
            //        LegionTeamPosition mPosition = new LegionTeamPosition();
            //        mPosition._CharacterID = int.Parse(secSplit[0]);
            //        mPosition._CharacterPosition = int.Parse(secSplit[1]);
            //        lw.mTeamPosition.Add(mPosition);
            //    }
            //    UIManager.instance.OpenPanel("RankPositionWindow", false);
            //}

            UIManager.instance.OpenSinglePanel("LegionPositionWindow", false);
            //if (GameObject.Find("LegionFightWindow") != null)
            //{
            //    UIManager.instance.OpenSinglePanel("LegionPositionWindow", false);
            //    //GameObject.Find("LegionPositionWindow")
            //}

        }
        else
        {
            CharacterRecorder.instance.LegionPositonStr = dataSplit[0];
            UIManager.instance.OpenSinglePanel("LegionPositionWindow", false);
        }
    }
    public void Process_8618(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("所有在该点驻守的英雄已全部撤离!", PromptWindow.PromptType.Hint, null, null);
            SendProcess("8601#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("所有在该点驻守的英雄撤离失败!", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_8619(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
    }

    public void Process_8620(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
    }

    public void Process_8621(string RecvString)
    {
        //string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LegionWarWindow") != null)
        {
            GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarGetJunGong(RecvString);
        }
    }

    public void Process_8622(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //UIManager.instance.OpenPromptWindow("领取成功", PromptWindow.PromptType.Hint, null, null);
            if (GameObject.Find("LegionWarWindow") != null)
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarGetJunGong(int.Parse(dataSplit[2]));
            }

            List<Item> itemlist = new List<Item>();
            Item _item = new Item(int.Parse(dataSplit[1]), 1);
            itemlist.Add(_item);
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            SendProcess("8621#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("兑换失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_8623(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "")
        {
            CharacterRecorder.instance.LegionPositonStr = dataSplit[0];
            UIManager.instance.OpenSinglePanel("LegionPositionWindow", false);
        }
        else
        {
            CharacterRecorder.instance.LegionPositonStr = dataSplit[0];
            UIManager.instance.OpenSinglePanel("LegionPositionWindow", false);
        }
    }

    public void Process_8624(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.LegionActionPoint = int.Parse(dataSplit[1]);
            if (GameObject.Find("LegionWarWindow") != null)
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().GetActionPoint(int.Parse(dataSplit[1]));
            }
        }
        else
        {

        }

    }
    public void Process_8625(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("LegionWarWindow") != null)
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarMarktalarm(dataSplit[1] + ";");
            }
        }
        else
        {

        }
    }
    public void Process_8626(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //PlayerPrefs.SetString("LegionMarktar", dataSplit[0]);

        //PlayerPrefs.SetString("LegionMarktar",dataSplit[0]);


        //Debug.LogError("LegionMarktar2 " + PlayerPrefs.GetString("LegionMarktar"));

        CharacterRecorder.instance.LegionMarktarStr = dataSplit[0];
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().LegionWarInfo();
        }
    }

    public void Process_8627(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("RankListWindow") != null)
        {
            RankListWindow rw = GameObject.Find("RankListWindow").GetComponent<RankListWindow>();
            CharacterRecorder.instance.RankNumber = 0;
            rw.ShowMyRank(5);
            rw.SetFirstInfo();

        }
        else if (GameObject.Find("LegionWarWindow") != null)
        {
            GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().ShowFirstKillRank(RecvString);
        }
        BetterList<ActiveAwardItemData> mMyList = new BetterList<ActiveAwardItemData>();
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            mMyList.Add(new ActiveAwardItemData(int.Parse(secSplit[0]), secSplit[2], int.Parse(secSplit[6]), int.Parse(secSplit[5])));
            if (secSplit[0] == "1")
            {
                CharacterRecorder.instance.FirstPowerName = secSplit[2];//战力第一名玩家
            }

            if (secSplit[2] == CharacterRecorder.instance.characterName)
            {
                CharacterRecorder.instance.RankNumber = int.Parse(secSplit[0]);
            }

            if (GameObject.Find("RankListWindow") != null)
            {
                RankListWindow rw = GameObject.Find("RankListWindow").GetComponent<RankListWindow>();
                //这里的secSplit[6]为杀敌数，使用level等级字段
                rw.CreatItem("5", secSplit[0], secSplit[1], secSplit[2], secSplit[3], secSplit[6], secSplit[5], secSplit[6], "0", secSplit[7]);

                if (secSplit[2] == CharacterRecorder.instance.characterName)
                {
                    CharacterRecorder.instance.RankNumber = int.Parse(secSplit[0]);
                    rw.ShowMyRank(5);
                }

                if (i == 0)
                {
                    rw.SetFirstInfo(secSplit[0], secSplit[1], secSplit[2], secSplit[3], secSplit[4], secSplit[5], secSplit[6], "0", secSplit[7]);
                }
            }

        }

    }
    public void Process_8628(string RecvString)
    {
        //string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LegionFightWindow") != null)
        {
            GameObject.Find("LegionFightWindow").GetComponent<LegionFightWindow>().LegionWarinfo(RecvString);
        }

    }
    public void Process_8629(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("已放弃该城市的所有权!", PromptWindow.PromptType.Hint, null, null);
            SendProcess("8601#;");
        }
        else
        {
            if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("该城市守卫军不足10队，无法弃城!", PromptWindow.PromptType.Hint, null, null);
            }
            else
            {
                UIManager.instance.OpenPromptWindow("弃城失败!", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_8630(string RecvString)
    {
        CharacterRecorder.instance.MilitaryExploitInfo = RecvString;
        //MeritAwardWindow MW = GameObject.Find("MeritAwardWindow").GetComponent<MeritAwardWindow>();
        if (GameObject.Find("MeritAwardWindow") != null)
        {
            GameObject.Find("MeritAwardWindow").GetComponent<MeritAwardWindow>().SetHappyBoxInfo();
        }

        if (GameObject.Find("LegionWarWindow") != null)
        {
            GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().JiangliButtonRedPoint();
        }
    }

    public void Process_8631(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[1].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            SendProcess("8630#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败！", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_8632(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<string> dataList = new List<string>();
        if (string.IsNullOrEmpty(dataSplit[0]))
        {
            UIManager.instance.OpenPromptWindow("没有战报.", PromptWindow.PromptType.Hint, null, null);
            return;
        }
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            dataList.Add(dataSplit[i]);
        }
        if (dataList.Count > 0)
        {
            GameObject br = GameObject.Find("BattlefieldReportWindow");
            if (br != null)
            {
                br.GetComponent<BattlefieldReportWindow>().ReseiverMsg_8632(dataList);
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("没有战报", PromptWindow.PromptType.Hint, null, null);
        }
    }


    public void Process_8633(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            SendProcess("8624#;1;");
            SendProcess("8636#;");
        }
        else
        {
            Debug.Log("编队失败");
        }
    }

    public void Process_8634(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            SendProcess("8636#;");
        }
        else
        {
            Debug.Log("解散战队失败");
        }
    }

    public void Process_8635(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //SendProcess("8636#;");
            if (dataSplit[1] == "1")
            {
                if (CharacterRecorder.instance.MarinesInfomation1 != null)
                {
                    CharacterRecorder.instance.MarinesInfomation1.CityId = int.Parse(dataSplit[2]);
                    string[] TrcSplit = CharacterRecorder.instance.MarinesInfomation1.TeamInformation.Split('!');
                    string[] BrcSplit = CharacterRecorder.instance.MarinesInfomation1.BloodNumber.Split('!');
                    CharacterRecorder.instance.HarassformationList.Clear();
                    for (int i = 0; i < TrcSplit.Length - 1; i++)
                    {
                        string[] tTrcSplit = TrcSplit[i].Split('$');
                        string[] tBrcSplit = BrcSplit[i].Split('$');
                        CharacterRecorder.instance.HarassformationList.Add(new Harassformation(int.Parse(tTrcSplit[0]), int.Parse(tTrcSplit[1]), CharacterRecorder.instance.MarinesInfomation1.WeakPoint, int.Parse(tBrcSplit[1]), int.Parse(tBrcSplit[2])));
                    }
                }
            }
            else if (dataSplit[1] == "2")
            {
                if (CharacterRecorder.instance.MarinesInfomation2 != null)
                {
                    CharacterRecorder.instance.MarinesInfomation2.CityId = int.Parse(dataSplit[2]);
                    string[] TrcSplit = CharacterRecorder.instance.MarinesInfomation2.TeamInformation.Split('!');
                    string[] BrcSplit = CharacterRecorder.instance.MarinesInfomation2.BloodNumber.Split('!');
                    CharacterRecorder.instance.HarassformationList.Clear();
                    for (int i = 0; i < TrcSplit.Length - 1; i++)
                    {
                        string[] tTrcSplit = TrcSplit[i].Split('$');
                        string[] tBrcSplit = BrcSplit[i].Split('$');
                        CharacterRecorder.instance.HarassformationList.Add(new Harassformation(int.Parse(tTrcSplit[0]), int.Parse(tTrcSplit[1]), CharacterRecorder.instance.MarinesInfomation2.WeakPoint, int.Parse(tBrcSplit[1]), int.Parse(tBrcSplit[2])));
                    }
                }
            }
            else if (dataSplit[1] == "3")
            {
                if (CharacterRecorder.instance.MarinesInfomation3 != null)
                {
                    CharacterRecorder.instance.MarinesInfomation3.CityId = int.Parse(dataSplit[2]);
                    string[] TrcSplit = CharacterRecorder.instance.MarinesInfomation3.TeamInformation.Split('!');
                    string[] BrcSplit = CharacterRecorder.instance.MarinesInfomation3.BloodNumber.Split('!');
                    CharacterRecorder.instance.HarassformationList.Clear();
                    for (int i = 0; i < TrcSplit.Length - 1; i++)
                    {
                        string[] tTrcSplit = TrcSplit[i].Split('$');
                        string[] tBrcSplit = BrcSplit[i].Split('$');
                        CharacterRecorder.instance.HarassformationList.Add(new Harassformation(int.Parse(tTrcSplit[0]), int.Parse(tTrcSplit[1]), CharacterRecorder.instance.MarinesInfomation3.WeakPoint, int.Parse(tBrcSplit[1]), int.Parse(tBrcSplit[2])));
                    }
                }
            }
            CharacterRecorder.instance.LegionHarasPoint = int.Parse(dataSplit[2]);

            LegionwarGetnode LG = null;
            for (int i = 0; i < CharacterRecorder.instance.LegionwarGetnodeList.Count; i++)
            {
                if (CharacterRecorder.instance.LegionwarGetnodeList[i].LegionPoint == int.Parse(dataSplit[2]))
                {
                    LG = CharacterRecorder.instance.LegionwarGetnodeList[i];
                    break;
                }
            }

            int cityType = TextTranslator.instance.GetLegionCityByID(int.Parse(dataSplit[2])).CityType;
            if (cityType < 5)
            {
                if (LG != null)
                {
                    if (LG.LegionID != 0 && LG.LegionID == CharacterRecorder.instance.legionID) //城市点为我占领，上阵防守
                    {
                        UIManager.instance.OpenPromptWindow("防守成功", PromptWindow.PromptType.Hint, null, null);
                        //Debug.Log("进入防守");
                    }
                    else if (LG.DeclareLegionName != CharacterRecorder.instance.myLegionData.legionName)
                    {
                        UIManager.instance.OpenPromptWindow("您的军团未对该城市点宣战", PromptWindow.PromptType.Hint, null, null);
                    }
                    else if (LG.IsAvoidWar != 3)
                    {
                        UIManager.instance.OpenPromptWindow("每日20:30开始军团战斗", PromptWindow.PromptType.Hint, null, null);
                    }
                    else if (LG.LegionID == 0 && LG.DeclareHaveNum > 0)
                    {
                        UIManager.instance.OpenPromptWindow("当前城市可直接占领", PromptWindow.PromptType.Hint, null, null);
                    }
                    else
                    {
                        SendProcess("8612#" + CharacterRecorder.instance.LegionHarasPoint + ";");//发起骚扰
                    }
                }
            }
            else
            {
                if (LG != null)
                {
                    //testyy  2017/4/11
                    //if (LG.LegionID != 0 && LG.LegionID != CharacterRecorder.instance.legionID && LG.CountryID == CharacterRecorder.instance.legionCountryID) //同属国家的据点
                    //{
                    //    UIManager.instance.OpenPromptWindow("您不可以骚扰同属国家军团所占领的据点", PromptWindow.PromptType.Hint, null, null);
                    //    CharacterRecorder.instance.AutomaticbrushCityID = 0;
                    //}
                    //else
                    //{
                    //    if (CharacterRecorder.instance.AutomaticbrushCityID != CharacterRecorder.instance.LegionHarasPoint)//返回的点与自动选择的点不同,非自动点，战斗结果不自动跳出
                    //    {
                    //        CharacterRecorder.instance.AutomaticbrushCityID = 0;
                    //    }
                    //    SendProcess("8612#" + CharacterRecorder.instance.LegionHarasPoint + ";");//发起骚扰    
                    //}

                    if (LG.LegionID != 0)
                    {
                        if (CharacterRecorder.instance.AutomaticbrushCityID != CharacterRecorder.instance.LegionHarasPoint)//返回的点与自动选择的点不同,非自动点，战斗结果不自动跳出
                        {
                            CharacterRecorder.instance.AutomaticbrushCityID = 0;
                        }
                        SendProcess("8612#" + CharacterRecorder.instance.LegionHarasPoint + ";");//发起骚扰    
                    }
                }
            }

            if (GameObject.Find("LegionDeclareWarWindow") != null) //上阵成功刷新战场信息
            {
                NetworkHandler.instance.SendProcess("8640#" + CharacterRecorder.instance.LegionHarasPoint + ";");
                Debug.Log("上阵刷新8640");
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("有玩家正在向你发起进攻，请稍后行动", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("您没有参加军团,无法上阵!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("军团等级不足,无法上阵", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("该城市点已被其它军团宣战,无法上阵", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }

    public void Process_8636(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (CharacterRecorder.instance.MarinesInfomation1 != null)
        {
            if (CharacterRecorder.instance.MarinesInfomation1.TeamHero != null)
            {
                DestroyImmediate(CharacterRecorder.instance.MarinesInfomation1.TeamHero);
            }
        }
        if (CharacterRecorder.instance.MarinesInfomation2 != null)
        {
            if (CharacterRecorder.instance.MarinesInfomation2.TeamHero != null)
            {
                DestroyImmediate(CharacterRecorder.instance.MarinesInfomation2.TeamHero);
            }
        }

        if (CharacterRecorder.instance.MarinesInfomation3 != null)
        {
            if (CharacterRecorder.instance.MarinesInfomation3.TeamHero != null)
            {
                DestroyImmediate(CharacterRecorder.instance.MarinesInfomation3.TeamHero);
            }
        }

        CharacterRecorder.instance.MarinesInfomation1 = new MarinesInfomation(int.Parse(dataSplit[0]), float.Parse(dataSplit[1]), int.Parse(dataSplit[2]), dataSplit[3], dataSplit[4], int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
        CharacterRecorder.instance.MarinesInfomation2 = new MarinesInfomation(int.Parse(dataSplit[7]), float.Parse(dataSplit[8]), int.Parse(dataSplit[9]), dataSplit[10], dataSplit[11], int.Parse(dataSplit[12]), int.Parse(dataSplit[13]));
        CharacterRecorder.instance.MarinesInfomation3 = new MarinesInfomation(int.Parse(dataSplit[14]), float.Parse(dataSplit[15]), int.Parse(dataSplit[16]), dataSplit[17], dataSplit[18], int.Parse(dataSplit[19]), int.Parse(dataSplit[20]));


        if (GameObject.Find("LegionWarWindow") != null)
        {
            GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().LegionWarGetteam();
        }

        if (GameObject.Find("LegionTeamWindow") != null)
        {
            GameObject.Find("LegionTeamWindow").GetComponent<LegionTeamWindow>().SetTeamItemButtonInfo();
        }
        if (GameObject.Find("LegionDeclareWarWindow") != null)
        {
            GameObject.Find("LegionDeclareWarWindow").GetComponent<LegionDeclareWarWindow>().SetEveryTeamInfo();
        }
    }

    public void Process_8637(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.LegionActionPoint = int.Parse(dataSplit[5]);
            if (GameObject.Find("LegionWarWindow") != null)
            {
                GameObject.Find("LegionWarWindow").GetComponent<LegionWarWindow>().GetActionPoint(int.Parse(dataSplit[5]));
            }
            SendProcess("8636#;");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("满血类型错误", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("行动力不足!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("复活石不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }

    public void Process_8638(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            Debug.Log("8638设定我方战队成功");
            string[] TrcSplit = dataSplit[4].Split('!');
            string[] BrcSplit = dataSplit[5].Split('!');
            CharacterRecorder.instance.HarassformationList.Clear();
            for (int i = 0; i < TrcSplit.Length - 1; i++)
            {
                string[] tTrcSplit = TrcSplit[i].Split('$');
                string[] tBrcSplit = BrcSplit[i].Split('$');
                CharacterRecorder.instance.HarassformationList.Add(new Harassformation(int.Parse(tTrcSplit[0]), int.Parse(tTrcSplit[1]), int.Parse(dataSplit[2]), int.Parse(tBrcSplit[1]), int.Parse(tBrcSplit[2])));
            }
        }
        else
        {
            Debug.Log("8638设定我方战队失败");
        }
    }

    public void Process_8639(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //if (GameObject.Find("LegionWarWindow") != null)
        {
            if (dataSplit[0] == "1")
            {
                Debug.Log("8639是战场重刷");
                SendProcess("8601#;");
            }
            else if (dataSplit[0] == "2")
            {
                Debug.Log("8639是战队重刷");
                SendProcess("8636#;");
            }
        }
    }

    public void Process_8640(string RecvString)
    {
        if (GameObject.Find("LegionDeclareWarWindow") != null)
        {
            GameObject.Find("LegionDeclareWarWindow").GetComponent<LegionDeclareWarWindow>().GetSoldiersInfo(RecvString);
            SendProcess("8641#0;");
        }
    }

    public void Process_8641(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            PlayerPrefs.SetInt("LegionTeamChooseNum", int.Parse(dataSplit[1]));
            if (GameObject.Find("LegionDeclareWarWindow") != null)
            {
                GameObject.Find("LegionDeclareWarWindow").GetComponent<LegionDeclareWarWindow>().ClickOneToggleButton();
            }
        }
    }


    public void Process_8701(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        bool redPoint = false;
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] trcSplit = dataSplit[i].Split('$');
            if (trcSplit[3] == "1")
            {
                redPoint = true;
                break;
            }
        }
        CharacterRecorder.instance.isLegionRedPoint = redPoint;

        if (GameObject.Find("LegionRedPacketWindow") != null)
        {
            GameObject.Find("LegionRedPacketWindow").GetComponent<LegionRedPacketWindow>().JuntuanRedPacket(RecvString);
        }
    }

    public void Process_8702(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[3].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            SendProcess("8701#;");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1": //UIManager.instance.OpenPromptWindow("!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("你已经抢过该红包!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":// UIManager.instance.OpenPromptWindow("!", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_8703(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == dataSplit[1])
        {
            CharacterRecorder.instance.isRichRedneckPoint = false;
        }
        else
        {
            CharacterRecorder.instance.isRichRedneckPoint = true;
        }


        if (GameObject.Find("LegionRedPacketWindow") != null)
        {
            GameObject.Find("LegionRedPacketWindow").GetComponent<LegionRedPacketWindow>().TuhaoRedPacket(RecvString);
        }
    }

    public void Process_8704(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[3].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);

            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            SendProcess("8703#;");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1":
                    UIManager.instance.OpenPromptWindow("等级不足!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("可发次数不足!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("钻石不足!", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }

    public void Process_8705(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LegionRedPacketWindow") != null)
        {
            GameObject.Find("LegionRedPacketWindow").GetComponent<LegionRedPacketWindow>().ChongzhiRedPacket(RecvString);
        }


        bool redPoint = false;
        for (int i = 0; i < dataSplit.Length - 5; i++)
        {
            string[] trcSplit = dataSplit[i].Split('$');
            if (trcSplit[1] == "1")
            {
                redPoint = true;
                break;
            }
        }
        CharacterRecorder.instance.isRechargeRedPoint = redPoint;

        long StartTime = long.Parse(dataSplit[dataSplit.Length - 5]);
        long NowTime = long.Parse(dataSplit[dataSplit.Length - 4]);
        long EndTime = long.Parse(dataSplit[dataSplit.Length - 3]);

        if (NowTime >= StartTime && NowTime < EndTime) //活动开始
        {
            CharacterRecorder.instance.isOpenRechargeRed = true;
        }
        else
        {
            CharacterRecorder.instance.isOpenRechargeRed = false;
        }
    }

    public void Process_8706(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[3].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);

            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            SendProcess("8705#;");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1":
                    UIManager.instance.OpenPromptWindow("该档红包不可发!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("以发过该当红包!", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3": //UIManager.instance.OpenPromptWindow("!", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }

    public void Process_8707(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        //if (dataSplit[0] == "0") 
        //{
        //    CharacterRecorder.instance.isGrabRedPoint = false;
        //}


        if (GameObject.Find("LegionRedPacketWindow") != null)
        {
            GameObject.Find("LegionRedPacketWindow").GetComponent<LegionRedPacketWindow>().GetGiftCharacterInfo(RecvString);
        }
    }

    public void Process_8708(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (GameObject.Find("LegionRedPacketWindow") != null)
        {
            GameObject.Find("LegionRedPacketWindow").GetComponent<LegionRedPacketWindow>().QiangRedPacket(RecvString);
        }

        if (dataSplit[0] != "" && dataSplit[0] != "0")
        {
            bool redPoint = false;
            int length = dataSplit.Length;
            for (int i = 0; i < length - 1; i++)
            {
                if (i + 100 >= length) //100条数据
                {
                    string[] trcSplit = dataSplit[i].Split('$');
                    if (trcSplit[4] == "1")
                    {
                        redPoint = true;
                        break;
                    }
                }
            }

            CharacterRecorder.instance.isGrabRedPoint = redPoint;
        }
        else
        {
            CharacterRecorder.instance.isGrabRedPoint = false;
        }
    }

    public void Process_8709(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[4].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();

            LegionRedPacketWindow LR = GameObject.Find("LegionRedPacketWindow").GetComponent<LegionRedPacketWindow>();
            if (LR != null)
            {
                LR.SetGiftMoney(dataSplit[1]);
            }

            SendProcess("8707#;");
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("未加入军团.", PromptWindow.PromptType.Hint, null, null);
            }
            if (dataSplit[1] == "1")
            {
                //UIManager.instance.OpenPromptWindow(".", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("该红包已被抢光.", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("您已抢过该红包.", PromptWindow.PromptType.Hint, null, null);
            }

            //LegionRedPacketWindow LR = GameObject.Find("LegionRedPacketWindow").GetComponent<LegionRedPacketWindow>();
            //if (LR != null)
            //{
            //    LR.SetGiftMoney(dataSplit[2]);
            //}
        }
    }


    public void Process_8710(string RecvString)
    {

        UIManager.instance.OpenPanel("RedPacketsRankWindow", false);
        RedPacketsRankWindow RR = GameObject.Find("RedPacketsRankWindow").GetComponent<RedPacketsRankWindow>();
        if (RR != null)
        {
            RR.SetTuhaoGiveRedPart(RecvString);
        }
    }

    public void Process_8711(string RecvString)
    {

        UIManager.instance.OpenPanel("RedPacketsRankWindow", false);
        RedPacketsRankWindow RR = GameObject.Find("RedPacketsRankWindow").GetComponent<RedPacketsRankWindow>();
        if (RR != null)
        {
            RR.SetChongzhiGiveRedPart(RecvString);
        }
    }

    public void Process_8712(string RecvString)
    {

        UIManager.instance.OpenPanel("RedPacketsRankWindow", false);
        RedPacketsRankWindow RR = GameObject.Find("RedPacketsRankWindow").GetComponent<RedPacketsRankWindow>();
        if (RR != null)
        {
            RR.SetGetRedPart(RecvString);
        }
    }

    public void Process_8713(string RecvString)
    {

        UIManager.instance.OpenPanel("RedPacketsRankWindow", false);
        RedPacketsRankWindow RR = GameObject.Find("RedPacketsRankWindow").GetComponent<RedPacketsRankWindow>();
        if (RR != null)
        {
            RR.SetRankListPart(RecvString);
        }
    }


    public void Process_9001(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<MailItemData> _mailList = new List<MailItemData>();
        _mailList.Clear();
        CharacterRecorder.instance.MailCount = 0;
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            _mailList.Add(new MailItemData(int.Parse(secSplit[0]), secSplit[1], secSplit[2], secSplit[3], int.Parse(secSplit[4]), int.Parse(secSplit[5]), int.Parse(secSplit[6])));
            if (secSplit[5] == "0")
            {
                CharacterRecorder.instance.MailCount += 1;
            }
        }
        if (CharacterRecorder.instance.MailButtonOnClick)
        {
            UIManager.instance.OpenPanel("MailWindow", false);
            CharacterRecorder.instance.MailButtonOnClick = false;
        }
        //CharacterRecorder.instance.MailCount = _mailList.Count;
        if (GameObject.Find("MailWindow") != null)
        {
            MailWindow mw = GameObject.Find("MailWindow").GetComponent<MailWindow>();
            mw.SetMailWindowInfo(_mailList);
        }
        else
        {
        }
        if (CharacterRecorder.instance.MailCount > 0)
        {
            CharacterRecorder.instance.SetRedPoint(8, true);
        }
        else
        {
            CharacterRecorder.instance.SetRedPoint(8, false);
        }
    }

    public void Process_9002(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("MailInfoBoard") != null)
        {
            List<AwardItem> ListAwardItem = new List<AwardItem>();
            if (dataSplit[2].Contains("!"))
            {
                string[] secSplit = dataSplit[2].Split('!');
                for (int i = 0; i < secSplit.Length - 1; i++)
                {
                    string[] tirSplit = secSplit[i].Split('$');
                    ListAwardItem.Add(new AwardItem(int.Parse(tirSplit[0]), int.Parse(tirSplit[1])));
                }
            }
            MailInfoBoard mb = GameObject.Find("MailInfoBoard").GetComponent<MailInfoBoard>();
            mb.SetMailBoardInfo(dataSplit[1], ListAwardItem);
        }
        else
        {
            Debug.Log("未找到MailWindow");
        }
    }

    public void Process_9003(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            Debug.Log("领取成功!!");
            GameObject _MailInfoBoard = GameObject.Find("MailInfoBoard");
            if (_MailInfoBoard != null)
            {
                _MailInfoBoard.GetComponent<MailInfoBoard>().ResetSetMailBoardInfo();
            }
            string[] dataSplit2 = dataSplit[2].Split('!');
            List<Item> _itemList = new List<Item>();
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
            }
            UpDateTopContentData(_itemList);
            if (GameObject.Find("MailWindow") != null)
            {
                MailWindow mw = GameObject.Find("MailWindow").GetComponent<MailWindow>();
                mw.OpenGainWindow(_itemList);
                mw.UpdateMail();
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("接收的邮件ID数据异常", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("已领取", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("无附件", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }
    int _index = 0;
    public void Process_9004(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            Debug.Log("一键领取成功!!");
            string[] dataSplit2 = dataSplit[1].Split('!');
            List<Item> _itemList = new List<Item>();
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                int itemCode = int.Parse(dataSplit3[0]);
                //  _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));     
                if (IsAwardListAreadyContainsThisCode(_itemList, itemCode))
                {
                    _itemList[_index] = new Item(itemCode, _itemList[_index].itemCount + int.Parse(dataSplit3[1]));
                }
                else
                {
                    _itemList.Add(new Item(itemCode, int.Parse(dataSplit3[1])));
                }
            }
            UpDateTopContentData(_itemList);
            if (GameObject.Find("MailWindow") != null)
            {
                MailWindow mw = GameObject.Find("MailWindow").GetComponent<MailWindow>();
                mw.OpenGainWindow(_itemList);
                mw.UpdateMail();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("无附件", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow(string.Format("服务器错误码{0}", dataSplit[1]), PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    bool IsAwardListAreadyContainsThisCode(List<Item> _itemList, int _code)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].itemCode == _code)
            {
                _index = i;
                return true;
            }
        }
        return false;
    }
    public void Process_9005(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.MailCount += 1;//新邮件主动通知

        GameObject _mainWindow = GameObject.Find("MainWindow");
        //if (_mainWindow != null)
        //{
        //    _mainWindow.GetComponent<MainWindow>().MailRedPont();
        //}
        CharacterRecorder.instance.MailRedPont();
    }
    public void Process_9006(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("发送成功", PromptWindow.PromptType.Hint, null, null);
        }
        else
        {
            UIManager.instance.OpenPromptWindow("发送失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_9007(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            UIManager.instance.OpenPromptWindow("发送成功", PromptWindow.PromptType.Hint, null, null);
        }
        else
        {
            UIManager.instance.OpenPromptWindow("发送失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_9101(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject _SignWindowObj = GameObject.Find("SignWindow");
        if (_SignWindowObj != null)
        {
            TextTranslator.instance.SignExtraIDHadGet = int.Parse(dataSplit[2]);
            _SignWindowObj.GetComponent<SignWindow>().SetSignData(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
        }
        GameObject _MainWindowObj = GameObject.Find("MainWindow");
        if (_MainWindowObj != null)
        {
            switch (dataSplit[1])
            {
                case "0": CharacterRecorder.instance.signRedPointState = true; CharacterRecorder.instance.SetRedPoint(7, CharacterRecorder.instance.signRedPointState); break;
                default: CharacterRecorder.instance.signRedPointState = false; CharacterRecorder.instance.SetRedPoint(7, CharacterRecorder.instance.signRedPointState); break;
            }
        }
    }
    public void Process_9102(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> _itemList = new List<Item>();
            string[] dataSplit2 = dataSplit[1].Split('!');
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
            }
            UpDateTopContentData(_itemList);
            GameObject _SignWindowObj = GameObject.Find("SignWindow").gameObject;
            if (_SignWindowObj != null)
            {
                _SignWindowObj.GetComponent<SignWindow>().ResetSignData(_itemList);
            }
            GameObject _MainWindowObj = GameObject.Find("MainWindow");
            if (_MainWindowObj != null)
            {
                CharacterRecorder.instance.signRedPointState = false;
                CharacterRecorder.instance.SetRedPoint(7, CharacterRecorder.instance.signRedPointState);
                //_MainWindowObj.GetComponent<MainWindow>().SetSignRedPoint();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("已领", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_9103(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> _itemList = new List<Item>();
            string[] dataSplit2 = dataSplit[1].Split('!');
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
            }
            UpDateTopContentData(_itemList);
            TextTranslator.instance.SignExtraIDHadGet += 1;
            GameObject _SignWindowObj = GameObject.Find("SignWindow").gameObject;
            if (_SignWindowObj != null)
            {
                _SignWindowObj.GetComponent<SignWindow>().ResetSignExtraData(_itemList);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("已领", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("签到未领取", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("最后一个不可领", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void UpDateTopContentData(List<Item> _itemList)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            switch (_itemList[i].itemCode)
            {
                case 90001: CharacterRecorder.instance.gold += _itemList[i].itemCount; break;
                case 90002: CharacterRecorder.instance.lunaGem += _itemList[i].itemCount; break;
                case 90003: CharacterRecorder.instance.HonerValue += _itemList[i].itemCount; break;
                case 90004: break;
                case 90005: break;
                case 90006: CharacterRecorder.instance.GoldBar += _itemList[i].itemCount; break;
                case 90007: CharacterRecorder.instance.stamina += _itemList[i].itemCount; break;
                case 90008:
                    CharacterRecorder.instance.sprite += _itemList[i].itemCount;
                    if (GameObject.Find("RobberyHeroList") != null)
                    {
                        GameObject.Find("RobberyHeroList").GetComponent<RobberyHeroList>().NowValue.GetComponent<UILabel>().text = CharacterRecorder.instance.sprite.ToString();
                    }
                    break;
                default: //TextTranslator.instance.SetItemCountAddByID(_itemList[i].itemCode, _itemList[i].itemCount);
                    break;
            }
        }
        if (GameObject.Find("TopContent") != null)
        {
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
    }
    public void Process_9121(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.ActivityTime = int.Parse(dataSplit[1]);
        CharacterRecorder.instance.OpenServiceFinalAward = int.Parse(dataSplit[dataSplit.Length - 2]);
        TextTranslator.instance.SetActivitySevenInfo(RecvString);
        if (GameObject.Find("OpenServiceWindow") != null)
        {
            GameObject.Find("OpenServiceWindow").GetComponent<OpenServiceWindow>().SetInfo(RecvString);
        }
        if (GameObject.Find("ActiveAwardWindow") != null)
        {
            GameObject.Find("ActiveAwardWindow").GetComponent<ActiveAwardWindow>().SetInfo(RecvString);
        }
    }
    public void Process_9122(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[4].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            //TextTranslator.instance.isUpdateBag = true;
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            //CharacterRecorder.instance.IsOpen = true;
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            if (GameObject.Find("OpenServiceWindow") != null)
            {
                GameObject.Find("OpenServiceWindow").GetComponent<OpenServiceWindow>().SetUpdate(int.Parse(dataSplit[3]));
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("成就ID错误", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("不可领取", PromptWindow.PromptType.Hint, null, null);
            }
            else
            {
                UIManager.instance.OpenPromptWindow("已过时间", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }
    public void Process_9123(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[1].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            TextTranslator.instance.isUpdateBag = true;
            PlayerPrefs.SetInt("ActivityFinalReward_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 1);
            if (GameObject.Find("OpenServiceWindow") != null)
            {
                GameObject.Find("OpenServiceWindow").GetComponent<OpenServiceWindow>().FinallRewardRed(2);
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("已领", PromptWindow.PromptType.Hint, null, null);
            }
            else
            {
                UIManager.instance.OpenPromptWindow("未到领取时间", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }
    public void Process_9131(string RecvString)
    {
        if (CharacterRecorder.instance.IsOpenloginSign) //活动是否结束
        {
            CharacterRecorder.instance.loginSignCount = 0;
            int num = 0;
            string[] dataSplit = RecvString.Split(';');
            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit[i].Split('$');
                TextTranslator.instance.GetActivitySevenLoginByDay(int.Parse(dataSplit3[0])).status = int.Parse(dataSplit3[1]);
                if (dataSplit3[1] == "1")
                {
                    CharacterRecorder.instance.loginSignCount += 1;
                }
                if (dataSplit3[1] == "2")
                {
                    num++;
                }
            }
            GameObject _LoginSignWindowObj = GameObject.Find("LoginSignWindow");
            if (_LoginSignWindowObj != null)
            {
                _LoginSignWindowObj.GetComponent<LoginSignWindow>().SetLoginSignWindow(TextTranslator.instance.ActivitySevenLoginList);

            }
            GameObject _MainWindowObj = GameObject.Find("MainWindow");
            if (num == 7)
            {
                CharacterRecorder.instance.IsOpenloginSign = false;
            }
            if (_MainWindowObj != null)
            {
                CharacterRecorder.instance.SetLoginSignRedPoint();
                _MainWindowObj.GetComponent<MainWindow>().LoginSignIsOpen();
            }
        }

    }
    public void Process_9132(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.loginSignCount -= 1;
            List<Item> _itemList = new List<Item>();
            string[] dataSplit2 = dataSplit[1].Split('!');
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
                if (int.Parse(dataSplit3[0]) > 60000 && int.Parse(dataSplit3[0]) < 70000 && TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(dataSplit3[0])).heroRarity >= 3)
                {
                    Debug.LogError("-------------");
                    string heroName = TextTranslator.instance.GetItemNameByItemCode(int.Parse(dataSplit3[0]));
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 7, CharacterRecorder.instance.characterName, heroName, TextTranslator.instance.GetHeroInfoByHeroID(int.Parse(dataSplit3[0])).heroRarity));
                }
            }

            UpDateTopContentData(_itemList);
            GameObject _LoginSignWindowObj = GameObject.Find("LoginSignWindow");
            if (_LoginSignWindowObj != null)
            {
                _LoginSignWindowObj.GetComponent<LoginSignWindow>().ResetLoginSignData(_itemList);
                _LoginSignWindowObj.GetComponent<LoginSignWindow>().SetLoginSignWindow(TextTranslator.instance.ActivitySevenLoginList);
            }
            GameObject _MainWindowObj = GameObject.Find("MainWindow");
            if (_MainWindowObj != null)
            {
                CharacterRecorder.instance.SetLoginSignRedPoint();
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("已领", PromptWindow.PromptType.Hint, null, null); break;
                    //default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }

    public void Process_9133(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[1] != "0")
        {
            GameObject.Find("EventWindow").GetComponent<EventWindow>().SevendayWindow.SetActive(true);
            GameObject.Find("EventWindow").GetComponent<EventWindow>().DiscountButton.SetActive(true);
            GameObject.Find("SevenDayWindow").GetComponent<SevenDayWindow>().SetSevendayShow(dataSplit);
        }
        else
        {
            GameObject.Find("EventWindow").GetComponent<EventWindow>().SevendayWindow.SetActive(false);
            GameObject.Find("EventWindow").GetComponent<EventWindow>().DiscountButton.SetActive(false);
            GameObject.Find("EventWindow").GetComponent<EventWindow>().NoActivity.SetActive(true);
        }
    }
    public void Process_9134(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[3].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                if (ticSplit[0] == "90001")
                {
                    CharacterRecorder.instance.AddMoney(int.Parse(ticSplit[1]));
                    GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
                }
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1": UIManager.instance.OpenPromptWindow("时间过期", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("VIP不足", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("不在开启时间", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("已领取", PromptWindow.PromptType.Hint, null, null); break;
                case "5": UIManager.instance.OpenPromptWindow("已售完", PromptWindow.PromptType.Hint, null, null); break;
                case "6":
                    UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
                    UIManager.instance.OpenPanel("VIPShopWindow", true); break;
            }
        }
        SendProcess("9133#");
    }
    public void Process_9151(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject _QuestionWindow = GameObject.Find("QuestionWindow");
        if (_QuestionWindow != null)
        {
            _QuestionWindow.GetComponent<QuestionWindow>().SetQuestionWindow(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        }

        if (dataSplit[0] != "2")
        {
            PlayerPrefs.SetInt("QuestionState_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 0);
        }
        else
        {
            PlayerPrefs.SetInt("QuestionState_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 1);
        }
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().QuestionWindowIsOpen();
        }
    }
    public void Process_9152(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject _QuestionWindow = GameObject.Find("QuestionWindow");
            if (_QuestionWindow != null)
            {
                _QuestionWindow.GetComponent<QuestionWindow>().SetQuestionWindow(int.Parse(dataSplit[1]) + 1);
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("提交失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_9153(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            string[] dataSplit2 = dataSplit[1].Split('!');
            List<Item> _itemList = new List<Item>();
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] dataSplit3 = dataSplit2[i].Split('$');
                _itemList.Add(new Item(int.Parse(dataSplit3[0]), int.Parse(dataSplit3[1])));
            }
            UpDateTopContentData(_itemList);
            GameObject _QuestionWindow = GameObject.Find("QuestionWindow");
            if (_QuestionWindow != null)
            {
                _QuestionWindow.GetComponent<QuestionWindow>().OpenGainWindow(_itemList);
            }

            PlayerPrefs.SetInt("QuestionState_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 1);//问卷调查
            if (GameObject.Find("MainWindow") != null)
            {
                GameObject.Find("MainWindow").GetComponent<MainWindow>().QuestionWindowIsOpen();
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取答卷奖励失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_9902(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (Application.loadedLevelName != "Downloader")
        {
            if (dataSplit[0] == "1")
            {
                CharacterRecorder.instance.IsOpen = true;
                CharacterRecorder.instance.IsOpeGacha = true;
                StartCoroutine(SyncHeroListFormServer());
            }
            else
            {
                UIManager.instance.OpenPromptWindow("刷新失败", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    IEnumerator SyncHeroListFormServer()  //重要
    {
        NetworkHandler.instance.SendProcess("1031#");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("2016#0;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("2201#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1204#;");
        yield return new WaitForSeconds(0.2f);
        NetworkHandler.instance.SendProcess("1201#1;");
        yield return new WaitForSeconds(0.2f);
        NetworkHandler.instance.SendProcess("1201#2;");
        yield return new WaitForSeconds(0.2f);
        NetworkHandler.instance.SendProcess("6001#;");
        NetworkHandler.instance.SendProcess("6006#2;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("6009#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("6011#;");
        yield return new WaitForSeconds(0.2f);
        NetworkHandler.instance.SendProcess("5204#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("1601#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9101#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9001#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("7103#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("7101#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9121#;");
        yield return new WaitForSeconds(0.1f);
        NetworkHandler.instance.SendProcess("9131#;");
    }
    public void Process_1131(string RecvString)
    {
        Debug.Log("asdasd");
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            AudioEditer.instance.PlayLoop("Win");
            UIManager.instance.OpenPanel("ResultWindow", false);
            ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
            rw.Init(true, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, dataSplit[3], 0, dataSplit[2], 3);
        }
        else
        {
            Debug.Log("失败");
        }
    }
    public void Process_1134(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject _mapUiWindow = GameObject.Find("MapUiWindow");
        if (int.Parse(dataSplit[3]) == 0 && int.Parse(dataSplit[4]) == 0)
        {
            //没占过
            if (_mapUiWindow != null)
            {
                _mapUiWindow.GetComponent<MapUiWindow>().ChoiceWindow(1);
                _mapUiWindow.transform.Find("All").transform.Find("GrabTerritory").GetComponent<GrabTerritory>().SetInfo(int.Parse(dataSplit[2]), dataSplit[8], dataSplit[9]);
            }
        }
        if (int.Parse(dataSplit[3]) == 0 && int.Parse(dataSplit[4]) != 0)
        {
            //可以领取
            if (_mapUiWindow != null)
            {
                _mapUiWindow.GetComponent<MapUiWindow>().ChoiceWindow(3);
                _mapUiWindow.transform.Find("All").Find("PatrolItem").GetComponent<PatrolItem>().SetInfo(int.Parse(dataSplit[2]), CharacterRecorder.instance.GetHeroByCharacterRoleID(int.Parse(dataSplit[4])).cardID, dataSplit[7], int.Parse(dataSplit[6]), int.Parse(dataSplit[5]), dataSplit[8], dataSplit[9]);
            }
        }
        if (int.Parse(dataSplit[3]) != 0 && int.Parse(dataSplit[4]) != 0)
        {
            //正在巡逻
            if (_mapUiWindow != null)
            {
                _mapUiWindow.GetComponent<MapUiWindow>().ChoiceWindow(2);
                if (_mapUiWindow.transform.Find("All").Find("PatrolLong") != null)
                {
                    Debug.Log("asdasdasdasd" + int.Parse(dataSplit[4]));
                    _mapUiWindow.transform.Find("All").Find("PatrolLong").GetComponent<PatrolLong>().setInfo(int.Parse(dataSplit[4]), int.Parse(dataSplit[3]), int.Parse(dataSplit[2]), dataSplit[8], dataSplit[9]);
                }
                else
                {
                    Debug.Log("asdasdsdsdwwwww");
                }
            }
        }
    }
    public void Process_1132(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[2]) != 0)
        {
            if (GameObject.Find("MapObject") != null)
            {
                GameObject Map = GameObject.Find("MapObject");
                //for(int i =0;i<Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList.Count;i++){
                //    Debug.Log("asdasdasdasd" + Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList[i] + "        " + int.Parse(dataSplit[2])+"     "+i);
                //    if (Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList[i] == int.Parse(dataSplit[2]))
                //    {
                if (!Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList.Contains(int.Parse(dataSplit[2])))
                {
                    Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList.Add(int.Parse(dataSplit[2]));
                }
                //  }
                //}

                SendProcess("1135#;");
            }
            //for (int i = 0; i < GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList.Count; i++)
            //{
            //    Debug.Log("111111111111111111" + GameObject.Find("MapObject").transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList[i]);
            //}

        }
    }
    public void Process_1133(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.IsOpen = true;
        Debug.Log("aaasssss" + int.Parse(dataSplit[2]));
        if (int.Parse(dataSplit[2]) != 0)
        {
            if (GameObject.Find("MapObject") != null)
            {
                GameObject Map = GameObject.Find("MapObject");


                Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList.Remove(int.Parse(dataSplit[2]));


            }

        }
        if (dataSplit[0] == "1")
        {
            List<Item> list = new List<Item>();
            if (dataSplit[3] != "" && dataSplit[3] != "0")
            {
                Item _temp = new Item(90002, int.Parse(dataSplit[3]));
                CharacterRecorder.instance.AddLunaGem(int.Parse(dataSplit[3]));
                list.Add(_temp);
            }
            if (dataSplit[4] != "" && dataSplit[4] != "0")
            {
                Item _temp = new Item(90001, int.Parse(dataSplit[4]));
                CharacterRecorder.instance.AddMoney(int.Parse(dataSplit[4]));
                list.Add(_temp);
            }
            string[] itemSplit = dataSplit[5].Split('!');
            for (int i = 0; i < itemSplit.Length - 1; i++)
            {
                string[] _tempSplit = itemSplit[i].Split('$');
                if (_tempSplit[1] != "" && _tempSplit[1] != "0")
                {
                    Item _temp = new Item(int.Parse(_tempSplit[0]), int.Parse(_tempSplit[1]));
                    list.Add(_temp);
                }
            }
            if (list.Count > 0)
            {
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, list);
                GameObject _mapUiWindow = GameObject.Find("MapUiWindow");
                if (_mapUiWindow != null)
                {
                    _mapUiWindow.GetComponent<MapUiWindow>().mTopContent.GetComponent<TopContent>().Reset();
                }
                NetworkHandler.instance.SendProcess("1135#;");
            }
            SendProcess("1135#;");
        }
    }
    public void Process_1135(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("MapUiWindow/All/ResourcesBoard") != null)
        {
            List<Mapgetreslist> mpList = new List<Mapgetreslist>();
            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                string[] dataSplitTwo = dataSplit[i].Split('$');
                Mapgetreslist mg = new Mapgetreslist();
                for (int j = 0; j < dataSplitTwo.Length; j++)
                {
                    if (j == 0)
                    {
                        mg.GetresId = int.Parse(dataSplitTwo[j]);
                    }
                    else if (j == 1)
                    {
                        mg.HeroId = int.Parse(dataSplitTwo[j]);
                    }
                    else if (j == 2)
                    {
                        mg.Timer = int.Parse(dataSplitTwo[j]);
                    }
                }
                mpList.Add(mg);
            }
            GameObject.Find("MapUiWindow/All/ResourcesBoard").GetComponent<ResourceBoard>().InitList(mpList);
        }
        else if (GameObject.Find("MapObject") != null)
        {
            GameObject Map = GameObject.Find("MapObject");
            Map.transform.Find("MapCon").GetComponent<MapWindow>().getreslist.Clear();
            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                string[] dataSplitTwo = dataSplit[i].Split('$');
                Mapgetreslist mg = new Mapgetreslist();
                for (int j = 0; j < dataSplitTwo.Length; j++)
                {
                    if (j == 0)
                    {
                        mg.GetresId = int.Parse(dataSplitTwo[j]);
                    }
                    else if (j == 1)
                    {
                        mg.HeroId = int.Parse(dataSplitTwo[j]);
                    }
                    else if (j == 2)
                    {
                        mg.Timer = int.Parse(dataSplitTwo[j]);
                    }
                }
                if (!Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList.Contains(mg.HeroId))
                {
                    Map.transform.Find("MapCon").GetComponent<MapWindow>().HeroIdList.Add(mg.HeroId);
                }
                Map.transform.Find("MapCon").GetComponent<MapWindow>().getreslist.Add(mg);

            }
            //StartCoroutine(SceneTransformer.instance.NewbieGuide());
            Map.transform.Find("MapCon").GetComponent<MapWindow>().ShowPatrol();
        }
    }
    public void Process_2201(string RecvString)
    {
        CharacterRecorder.instance.worldEventList = RecvString;
        GameObject worldFight = GameObject.Find("WorldEvenWindow");
        if (worldFight != null)
        {
            worldFight.GetComponent<WordEventFight>().SetFightTime();
            return;
        }

        string[] dataSplit = CharacterRecorder.instance.worldEventList.Split(';');
        if (dataSplit[0] == "1")
        {
            string[] secSplit = dataSplit[2].Split('!');
            CharacterRecorder.instance.WorldEventFightCount = int.Parse(dataSplit[4]);
            CharacterRecorder.instance.WorldEventRefreshCost = TextTranslator.instance.GetMarketByID(TextTranslator.instance.GetMarketIDByBuyCount(int.Parse(dataSplit[5]))).WorldEventRefreshCost;
            GameObject _mapUiWindow = GameObject.Find("MapUiWindow");
            if (_mapUiWindow != null)
            {
                //_mapUiWindow.GetComponent<MapUiWindow>().ChoiceWindow(6);
                if (_mapUiWindow.transform.Find("All").Find("WordEvent") != null)
                {
                    //GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().worldList = word;
                    if (dataSplit[3] != "" && dataSplit[3] != null)
                    {
                        string[] dataInfo = dataSplit[3].Split('$');
                        Debug.Log("千里走单骑ID：" + int.Parse(dataInfo[0]));
                        if (GameObject.Find("MainCamera").GetComponent<MouseClick>().IsAction)
                        {
                            CharacterRecorder.instance.gotoGateID = TextTranslator.instance.GetGateByTypeGroup(1, TextTranslator.instance.GetActionEventById(int.Parse(dataInfo[0])).ForGateID).id;
                            GameObject.Find("MapCon").GetComponent<MapWindow>().WorldEventId = int.Parse(dataInfo[0]);
                        }

                    }
                }
            }
        }

        GameObject go = GameObject.Find("MapUiWindow");
        if (go != null)
        {
            //if (CharacterRecorder.instance.IsOpenEventList)
            {
                go.GetComponent<MapUiWindow>().InitWorldEvenList();
            }
            go.GetComponent<MapUiWindow>().ShowWorldEventIcon();
        }
        //string[] dataSplit = RecvString.Split(';');
        //if (dataSplit[0] == "1")
        //{

        //    WorldEventList word = new WorldEventList();
        //    word.TimerNumber = int.Parse(dataSplit[1]);
        //    string[] secSplit = dataSplit[2].Split('!');
        //    if (GameObject.Find("MapUiWindow") != null)
        //    {
        //        if (!GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().IsWorldBtnClick)
        //        {
        //            GameObject _WordButtonRedPoint = GameObject.Find("WordButton").transform.FindChild("SpriteRedDian").gameObject;
        //            if (secSplit.Length > 1)
        //            {
        //                _WordButtonRedPoint.SetActive(true);
        //                return;
        //            }
        //            else
        //            {
        //                _WordButtonRedPoint.SetActive(false);
        //                return;
        //            }
        //        }
        //    }
        //    for (int i = 0; i < secSplit.Length - 1; i++)
        //    {
        //        string[] tirSplit = secSplit[i].Split('$');
        //        WorldEventInfo worldInfo = new WorldEventInfo();
        //        worldInfo.WorldId = int.Parse(tirSplit[0]);
        //        worldInfo.WorldType = int.Parse(tirSplit[1]);
        //        worldInfo.WorldColor = int.Parse(tirSplit[2]);
        //        worldInfo.WorldItem = int.Parse(tirSplit[3]);
        //        word.WorldEventInfo.Add(worldInfo);

        //    }
        //    if (GameObject.Find("MapUiWindow") != null)
        //    {
        //        GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().ChoiceWindow(6);
        //        if (GameObject.Find("MapUiWindow").transform.Find("All").Find("WordEvent") != null)
        //        {
        //            //GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().worldList = word;
        //            GameObject.Find("MapUiWindow").transform.Find("All").Find("WordEvent").GetComponent<WordEvent>().setInfo(word);
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.Log("获取世界时间列表失败");
        //}
    }
    public void Process_2202(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("WordEvent") != null)
            {
                GameObject _mapUiWindow = GameObject.Find("MapUiWindow");
                if (_mapUiWindow != null)
                    _mapUiWindow.GetComponent<MapUiWindow>().ChoiceWindow(6);
                GameObject.Find("WordEvent").GetComponent<WordEvent>().WordEventIdObj.SetActive(true);
                string[] secSplit = dataSplit[7].Split('!');
                List<WordItem> Item = new List<WordItem>();
                for (int i = 0; i < secSplit.Length; i++)
                {

                    string[] twoSplit = secSplit[i].Split('$');
                    WordItem It = new WordItem();
                    It.ItemId = int.Parse(twoSplit[0]);
                    //It.ItemNumber = int.Parse(twoSplit[1]);

                    Item.Add(It);
                }
                GameObject.Find("WordEvent").GetComponent<WordEvent>().WordEventIdObj.GetComponent<WordEventFight>().SetInfo(int.Parse(dataSplit[1]), Item, int.Parse(dataSplit[3]));
            }
        }
        else
        {
        }
    }
    public void Process_2204(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem -= int.Parse(dataSplit[1]);
            CharacterRecorder.instance.IsOpenEventList = true;
            NetworkHandler.instance.SendProcess("2201#;");
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("您的钻石不足！", PromptWindow.PromptType.Hint, null, null);
            }
        }

    }
    public void Process_2205(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "")
        {
            StartCoroutine(DelayForOpenWindow(int.Parse(dataSplit[0])));
        }
    }

    IEnumerator Handle_2205(int id)
    {
        GameObject go = GameObject.Find("MapUiWindow");
        if (go == null)
        {
            CharacterRecorder.instance.enterMapFromMain = true;
            PictureCreater.instance.StopFight(true);
            GameObject _CloudWindow = Resources.Load("GUI/CloudWindow") as GameObject;
            GameObject obj = Instantiate(_CloudWindow) as GameObject;
            if (GameObject.Find("UIRoot") != null)
            {
                obj.transform.parent = GameObject.Find("UIRoot").transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                obj.name = "CloudWindow";
            }
            Invoke("OpenMapWindow", 0.4f);
            AudioEditer.instance.PlayOneShot("Jet");
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                if (GameObject.Find("SweptWindow") == null)
                {
                    go.GetComponent<MapUiWindow>().ChoiceWindow(7);
                    CharacterRecorder.instance.IsOpenEventList = false;
                    SendProcess("2201#;");
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        //UIManager.instance.OpenPanel("ActionEventWindow", false);
        GameObject _mapUi = GameObject.Find("MapUiWindow");
        if (_mapUi != null)
        {
            //_mapUi.GetComponent<MapUiWindow>().ChoiceWindow(7);
            //_mapUi.transform.Find("All/ActionEventWindow").gameObject.SetActive(true);
            //SceneTransformer.instance.SetEventNumber(TextTranslator.instance.GetTalkId(id), 1);
            _mapUi.GetComponent<MapUiWindow>().ChoiceWindow(7);
            _mapUi.transform.Find("All/ActionEventWindow").gameObject.SetActive(true);
            SceneTransformer.instance.SetEventNumber(TextTranslator.instance.GetTalkId(id), 1);
            GameObject.Find("ActionEventWindow").GetComponent<ActionEventWindow>().InitActionEvent(id);
            CharacterRecorder.instance.gotoGateID = TextTranslator.instance.GetGateByTypeGroup(1, TextTranslator.instance.GetActionEventById(id).ForGateID).id;
            GameObject.Find("MainCamera").GetComponent<MouseClick>().IsAction = true;
            GameObject _map = GameObject.Find("MapObject");
            if (_map != null)
            {
                SceneTransformer.instance.SetEventNumber(TextTranslator.instance.GetTalkId(id), 1);
                _map.transform.FindChild("MapCon").GetComponent<MapWindow>().WorldEventId = id;
                _map.transform.FindChild("MapCon").GetComponent<MapWindow>().SetMapTypeUpdate();
            }
            //_mapUi.transform.Find("All/ActionEventWindow").gameObject.SetActive(true);
            //GameObject.Find("ActionEventWindow").GetComponent<ActionEventWindow>().InitActionEvent(obj.GetComponent<WordEventItem>().WorldId);
        }
        else
        {
            Debug.Log("MapUiWindow没有打开！---Handle_2205");
        }

    }

    void OpenMapWindow()
    {
        UIManager.instance.OpenPanel("MapUiWindow", true);
    }
    IEnumerator DelayForOpenWindow(int id)
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {

            if (GameObject.Find("LevelUpWindow") == null)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(Handle_2205(id));
    }
    public void Process_2206(string RecvString)
    {
        CharacterRecorder.instance.EnemyInfoStr = RecvString;
    }
    public void Process_2203(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.IsOpen = true;
        if (dataSplit[0] == "0")
        {
            AudioEditer.instance.PlayOneShot("Lose");
            UIManager.instance.OpenPanel("ResultWindow", false);
            ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
            rw.Init(false, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, "", 0, "", 0);
        }
        else
        {
            AudioEditer.instance.PlayOneShot("Win");
            UIManager.instance.OpenPanel("EveryResultWindow", false);
            GameObject.Find("EveryResultWindow").GetComponent<EveryResultWindow>().SetWorldEventResult(dataSplit[2], int.Parse(dataSplit[5]));
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.sprite -= 2;
            SendProcess("5001#;");
        }
    }
    public void Process_1016(string RecvString)
    {
        GameObject Map = GameObject.Find("MapObject");
        if (Map != null)
        {
            Map.transform.Find("MapCon").GetComponent<MapWindow>().IsClickLeiDaLock = false;
        }
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lastGateID = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.mapID = int.Parse(dataSplit[1]);
            SceneTransformer.instance.SetEventNumber(100 + CharacterRecorder.instance.mapID, 1);
            if (Map != null)
            {
                Map.transform.Find("MapCon").GetComponent<MapWindow>().SetCloud(int.Parse(dataSplit[1]));
                CharacterRecorder.instance.mapID = int.Parse(dataSplit[1]);
                //Map.transform.Find("MapCon").GetComponent<MapWindow>().SetMapIdShowName(int.Parse(dataSplit[1]));
                if (SceneTransformer.instance.CheckGuideIsFinish() == false)
                {
                    SceneTransformer.instance.NewGuideButtonClick();
                }
                StartCoroutine(Award_1016(dataSplit[3], int.Parse(dataSplit[1])));
                NetworkHandler.instance.SendProcess("2001#;");
                //GameObject.Find("MapObject").transform.FindChild("MapCon").GetComponent<MapWindow>().SetMapTypeUpdate();
                DestroyImmediate(GameObject.Find("wf_LeiDa_icon_eff"));
                AudioEditer.instance.PlayOneShot("ui_unlockcloud");
                GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().DelClickOpenInfo();
            }

            if (int.Parse(dataSplit[1]) >= 12)
            {
                SendProcess(string.Format("7002#{0};{1};{2};{3}", 14, CharacterRecorder.instance.characterName, dataSplit[1], 0));
            }
        }

    }

    IEnumerator Award_1016(string _awardList, int Chapter)
    {
        string[] _award = _awardList.Split('!');
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (!SceneTransformer.instance.NewGuideObj.GetComponent<NewGuidWindow>().EventTalk)
            {
                break;
            }
        }
        List<Item> itemList = new List<Item>();
        foreach (var item in _award)
        {
            string[] _awardInfo = item.Split('$');
            if (_awardInfo.Length > 1)
            {
                itemList.Add(new Item(int.Parse(_awardInfo[0]), int.Parse(_awardInfo[1])));
                if (_awardInfo[0] == "90007")
                {
                    CharacterRecorder.instance.stamina += int.Parse(_awardInfo[1]);
                }
            }
        }
        CharacterRecorder.instance.Vitality += 40;
        GameObject topConten = GameObject.Find("TopContent");
        if (topConten != null)
        {
            topConten.GetComponent<TopContent>().Reset();
        }

        if (Chapter != 2)
        {
            GameObject go = UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            go.transform.Find("GainResultPart").GetComponent<GainResultPart>().ChangeAwardWindowTitle();
            go.GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemList);
            GameObject.Find("MapUiWindow").GetComponent<MapUiWindow>().mTopContent.GetComponent<TopContent>().Reset();
        }
    }

    public void Process_1401(string RecvString)
    {
        if (GameObject.Find("GrabItemWindow") != null)
        {
            GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().Getgemlist.Clear();
        }
        string[] dataSplit = RecvString.Split(';');
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            string[] SecSplit = dataSplit[i].Split('$');
            if (GameObject.Find("GrabItemWindow") != null)
            {

                Getgemlist GrabList = new Getgemlist();
                GrabList.Id = int.Parse(SecSplit[0]);
                GrabList.Name = SecSplit[1];
                GrabList.level = int.Parse(SecSplit[2]);
                GrabList.FightValue = int.Parse(SecSplit[3]);
                if (int.Parse(SecSplit[4]) == 0)
                {
                    GrabList.IsRole = true;
                }
                else if (int.Parse(SecSplit[4]) == 1)
                {
                    GrabList.IsRole = false;
                }
                string[] IdSplit = SecSplit[5].Split('!');
                for (int j = 0; j < IdSplit.Length - 1; j++)
                {
                    if (int.Parse(IdSplit[j]) != 0)
                    {
                        GrabList.RoleId.Add(int.Parse(IdSplit[j]));
                    }
                }
                GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().Getgemlist.Add(GrabList);
            }
        }
        if (GameObject.Find("GoodsItemObj") != null)
        {
            GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().SetRobberyHeroList();
        }
        //RobberyHeroListObj.SetActive(true);
    }
    public void Process_1402(string RecvString)
    {
        UIManager.instance.OpenPanel("LoadingWindow", true);
        PictureCreater.instance.FightStyle = 3;
        PictureCreater.instance.StartPVP(RecvString);
    }
    public void Process_1403(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
        CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
        CharacterRecorder.instance.sprite = int.Parse(dataSplit[4]);
        CharacterRecorder.instance.GrabIntegrationPoint = int.Parse(dataSplit[5]);
        if (CharacterRecorder.instance.GrabIntegrationGetPointLayer != 10)
        {
            if (TextTranslator.instance.IndianaPointDic[CharacterRecorder.instance.GrabIntegrationGetPointLayer + 1] != null)
            {
                if (TextTranslator.instance.IndianaPointDic[CharacterRecorder.instance.GrabIntegrationGetPointLayer + 1].Point <= int.Parse(dataSplit[5]))
                {
                    GameObject.Find("GrabItemWindow").transform.Find("ALL/IntegralButton/RedPoint").gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Find("GrabItemWindow").transform.Find("ALL/IntegralButton/RedPoint").gameObject.SetActive(false);
                }
            }
        }
        else
        {
            GameObject.Find("GrabItemWindow").transform.Find("ALL/IntegralButton/RedPoint").gameObject.SetActive(false);
        }
        List<FragmentItemData> _fragmentItemList = new List<FragmentItemData>();
        if (GameObject.Find("GrabItemWindow") != null)
        {
            GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().SetPopGrabResultObj();
        }
        if (GameObject.Find("GoodsItemObj") != null)
        {
            GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().SetReset();
        }
        if (GameObject.Find("TopContent") != null)
        {
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
        for (int i = 6; i < dataSplit.Length - 1; i++)
        {
            string[] awardSplit = dataSplit[i].Split('!');
            string[] itemSplit;
            itemSplit = awardSplit[0].Split('$');
            if (awardSplit[1] != "")
            {
                string[] awardItem = awardSplit[1].Split('$');
                _fragmentItemList.Add(new FragmentItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), i - 1, int.Parse(awardItem[0]), int.Parse(awardItem[1])));
                if (itemSplit[0] == "70000")
                {
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 39, CharacterRecorder.instance.characterName, 0, 0));
                }
            }
            else
            {
                _fragmentItemList.Add(new FragmentItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), i - 1, 0, 0));
                if (itemSplit[0] == "70000")
                {
                    SendProcess(string.Format("7002#{0};{1};{2};{3}", 39, CharacterRecorder.instance.characterName, 0, 0));
                }
            }

        }
        GameObject.Find("GrabResult").GetComponent<GrabResult>().SetInfo(_fragmentItemList);
        GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().UpdateList();
        if (dataSplit[0] == "0")
        {
            CharacterRecorder.instance.isFailed = true;
        }
    }
    public void Process_1404(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 0)
        {
            if (int.Parse(dataSplit[1]) == 0)
            {
                UIManager.instance.OpenPromptWindow("此宝物无法合成", PromptWindow.PromptType.Hint, null, null);
            }
            else if (int.Parse(dataSplit[1]) == 1)
            {
                UIManager.instance.OpenPromptWindow("碎片不足无法合成", PromptWindow.PromptType.Hint, null, null);
            }
        }
        else if (int.Parse(dataSplit[0]) == 1)
        {

            string[] trcSplit = dataSplit[1].Split('$');
            UIManager.instance.OpenPromptWindow("获得 " + trcSplit[1] + " 个" + GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().color + GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().itemName + "[-]", PromptWindow.PromptType.Hint, null, null);
            TextTranslator.ItemInfo _ItemInfo = TextTranslator.instance.GetItemByItemCode(int.Parse(trcSplit[0]));
            if (_ItemInfo.itemGrade >= 5)
            {
                SendProcess(string.Format("7002#{0};{1};{2};{3}", 6, CharacterRecorder.instance.characterName, _ItemInfo.itemName, _ItemInfo.itemGrade));//用来区分颜色
            }
            CharacterRecorder.instance.IsOpen = true;
            GameObject.Find("GoodsItemObj").GetComponent<GoodsItemWindow>().ShowItemEffect();

            NetworkHandler.instance.SendProcess("5001#;");
        }
    }
    public void Process_1405(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        Debug.Log("asdadad" + RecvString);
        List<FragmentItemData> _fragmentItemList = new List<FragmentItemData>();
        CharacterRecorder.instance.gold = int.Parse(dataSplit[4]);
        CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
        CharacterRecorder.instance.sprite = int.Parse(dataSplit[5]);
        CharacterRecorder.instance.GrabIntegrationPoint = int.Parse(dataSplit[7]);
        if (dataSplit[0] == "0")
        {
        }
        else
        {
            UIManager.instance.OpenPanel("GrabFinishWindow", false);
            if (dataSplit[6] != "")
            {
                string[] awardSplit = dataSplit[6].Split('!');
                string[] itemSplit;
                if (awardSplit[1] != "")
                {
                    itemSplit = awardSplit[0].Split('$');
                    string[] awardItem = awardSplit[1].Split('$');
                    CharacterRecorder.instance.isFailed = false;
                    _fragmentItemList.Add(new FragmentItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), 0, int.Parse(awardItem[0]), int.Parse(awardItem[1])));
                }
                else
                {
                    CharacterRecorder.instance.isFailed = true;
                    itemSplit = awardSplit[0].Split('$');
                    _fragmentItemList.Add(new FragmentItemData(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]), 0, 0, 0));
                }
            }
            GameObject.Find("GrabFinishWindow").GetComponent<GrabFinishWindow>().GrabInfo(int.Parse(dataSplit[1]), _fragmentItemList);
        }

    }

    public void Process_1406(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().AllProtectTime <= 0)
        {
            GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().SetProtectTimeInfo(int.Parse(dataSplit[1]));
        }
        if (GameObject.Find("BuyPropsWindow") != null)
        {
            GameObject.Find("BuyPropsWindow").GetComponent<BuyPropsWindow>().UsePropsSucess();
        }
    }
    public void Process_1407(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        int Time = int.Parse(dataSplit[0]);
        if (Time > 0)
        {
            GameObject.Find("GrabItemWindow").GetComponent<GrabWindow>().SetProtectTimeInfo(Time);
        }
    }
    public void Process_1408(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("LoseGrabWindow") != null)
        {
            GameObject.Find("LoseGrabWindow").GetComponent<LoseGrabWindow>().LoseMessage(dataSplit[0]);
        }
    }
    public void Process_1409(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.GrabIntegrationGetPointLayer = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.GrabIntegrationPoint = int.Parse(dataSplit[1]);
        if (CharacterRecorder.instance.GrabIntegrationGetPointLayer != 10)
        {
            if (TextTranslator.instance.IndianaPointDic[CharacterRecorder.instance.GrabIntegrationGetPointLayer + 1] != null)
            {
                if (TextTranslator.instance.IndianaPointDic[int.Parse(dataSplit[0]) + 1].Point <= int.Parse(dataSplit[1]))
                {
                    CharacterRecorder.instance.GrabIntegrationRedPoint = true;
                    if (GameObject.Find("GrabItemWindow") != null)
                    {
                        GameObject.Find("GrabItemWindow").transform.Find("ALL/IntegralButton/RedPoint").gameObject.SetActive(true);
                    }
                }
                else
                {
                    CharacterRecorder.instance.GrabIntegrationRedPoint = false;
                    if (GameObject.Find("GrabItemWindow") != null)
                    {
                        GameObject.Find("GrabItemWindow").transform.Find("ALL/IntegralButton/RedPoint").gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            CharacterRecorder.instance.GrabIntegrationRedPoint = false;
            if (GameObject.Find("GrabItemWindow") != null)
            {
                GameObject.Find("GrabItemWindow").transform.Find("ALL/IntegralButton/RedPoint").gameObject.SetActive(false);
            }
        }
    }
    public void Process_1410(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("IntegrationWindow") != null)
            {
                IntegrationWindow IW = GameObject.Find("IntegrationWindow").GetComponent<IntegrationWindow>();
                IW.updateItemlistShow(int.Parse(dataSplit[1]), dataSplit[2], 2);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("奖励不能领取", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("请先领取前一档奖励", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_1501(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        CharacterRecorder.instance.HadRewardID = int.Parse(dataSplit[5]);
        if (GameObject.Find("MainWindow") != null && GameObject.Find("OpenServiceWindow") == null)
        {
            List<TowerShop> ShopList = new List<TowerShop>();
            ShopList = TextTranslator.instance.getAllTowerShop();
            for (int i = 0; i < ShopList.Count; i++)
            {
                if (int.Parse(dataSplit[2]) >= ShopList[i].Point)
                {
                    CharacterRecorder.instance.CanGetRewardID++;
                }
                else
                {
                    break;
                }
            }
            //GameObject.Find("MainWindow").GetComponent<MainWindow>().Collision();
            CharacterRecorder.instance.Collision();
        }
        else
        {
            if (CharacterRecorder.instance.lastGateID >= 10058)
            {
                #region  //最大可跳楼层
                CharacterRecorder.instance.HistoryFloor = int.Parse(dataSplit[4]);
                if (CharacterRecorder.instance.HistoryFloor == 50)
                {
                    CharacterRecorder.instance.CanMoveLayer = 30;
                    if (CharacterRecorder.instance.Vip >= 5)
                    {
                        CharacterRecorder.instance.CanMoveLayer = 40;
                    }
                    if (CharacterRecorder.instance.Vip >= 9)
                    {
                        CharacterRecorder.instance.CanMoveLayer = 45;
                    }
                }
                else
                {
                    CharacterRecorder.instance.CanMoveLayer = CharacterRecorder.instance.HistoryFloor / 2;
                }
                if (CharacterRecorder.instance.CanMoveLayer < 10)
                {
                    CharacterRecorder.instance.CanMoveLayer = 10;
                }

                if (dataSplit[6] == "1")
                {
                    CharacterRecorder.instance.isSkip = true;
                }
                else
                {
                    CharacterRecorder.instance.isSkip = false;
                }

                CharacterRecorder.instance.AutomaticOpenBoxNum = PlayerPrefs.GetInt("AutomaticOpenBoxNum" + CharacterRecorder.instance.userId.ToString() + PlayerPrefs.GetString("ServerID"));
                #endregion

                UIManager.instance.OpenPanel("WoodsTheExpendables", true);
                if (GameObject.Find("WoodsTheExpendables") != null)
                {
                    if (int.Parse(dataSplit[0]) > CharacterRecorder.instance.NowFloor && CharacterRecorder.instance.NowFloor != 0)
                    {
                        GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().isUpFloor = true;
                    }
                    GameObject.Find("WoodsTheExpendables").transform.Find("TopContent").GetComponent<TopContent>().isWoods = true;
                    GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().SetInfo(RecvString);
                    GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().MoveToFloor(int.Parse(dataSplit[0]));
                    PictureCreater.instance.EasyWood = dataSplit[7];
                    PictureCreater.instance.NormalWood = dataSplit[8];
                    PictureCreater.instance.HardWood = dataSplit[9];
                    PictureCreater.instance.WoodBuff = dataSplit[3];
                    CharacterRecorder.instance.HistoryFloor = int.Parse(dataSplit[4]);

                    GameObject.Find("WoodsTheExpendables").transform.Find("TopContent").GetComponent<TopContent>().LabelTrial.text = dataSplit[10];
                    CharacterRecorder.instance.WoodsRankID = int.Parse(dataSplit[11]);
                    if (dataSplit[6] == "1")
                    {
                        CharacterRecorder.instance.isSkip = true;
                    }
                    List<TowerShop> ShopList = new List<TowerShop>();
                    ShopList = TextTranslator.instance.getAllTowerShop();
                    for (int i = 0; i < ShopList.Count; i++)
                    {
                        if (int.Parse(dataSplit[2]) >= ShopList[i].Point)
                        {
                            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().CanRewardID++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    CharacterRecorder.instance.CanGetRewardID = GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().CanRewardID;
                    GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().ShowButtonMessage(int.Parse(dataSplit[5]));
                }
            }
        }
    }
    public void Process_1502(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        //BetterList<Item> itemlist = new BetterList<Item>();
        List<Item> itemlist = new List<Item>();
        string[] secSplit = dataSplit[2].Split('!');
        for (int i = 0; i < secSplit.Length - 1; i++)
        {
            string[] ticSplit = secSplit[i].Split('$');
            Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
            itemlist.Add(_item);
        }
        if (GameObject.Find("WoodsTheExpendables") != null)
        {
            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().SetCurFloor(int.Parse(dataSplit[1]));


            //UIManager.instance.OpenAwardWindow(itemlist, GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().OpenTreasureWindow);
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);

            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().OpenIENum = 9;//第一次打开宝箱
            StartCoroutine(GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().AutomaticbrushOnEveryWindow(WoodsTheExpendablesWindowType.AdvanceWindow, SomeConditionCloseWindow.Nothing));
        }
        CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
        CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[4]);
    }
    public void Process_1503(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.IsOpen = true;
        if (int.Parse(dataSplit[0]) != 0)
        {
            if (GameObject.Find("WoodsTheExpendables") != null)
            {
                SendProcess("1501#");
            }
            else
            {
                AudioEditer.instance.PlayLoop("Win");
                UIManager.instance.OpenPanel("ResultWindow", false);
                ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                rw.SetWood(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
                PlayerPrefs.SetInt("ThreatBuff" + "_" + PlayerPrefs.GetString("ServerID") + "_" + PlayerPrefs.GetInt("UserID"), 0);
            }

            if (int.Parse(dataSplit[1]) == 49) //50层敢死队通告
            {
                SendProcess(string.Format("7002#{0};{1};{2};{3}", 12, CharacterRecorder.instance.characterName, 0, 0));
            }
            //if (GameObject.Find("WoodsTheExpendables") != null)
            //{
            //    GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateDate(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[4]));
            //    GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateWindow(WoodsTheExpendablesWindowType.Right);
            //    GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().MoveToFloor(int.Parse(dataSplit[1]));
            //}
        }
        else
        {
            if (dataSplit[1] == "1")
            {
                PlayerPrefs.SetInt("Automaticbrushfield" + CharacterRecorder.instance.userId.ToString() + PlayerPrefs.GetString("ServerID"), 0);
                if (GameObject.Find("WoodsTheExpendables") != null)
                {
                    GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().AutomaticbrushButton.transform.Find("Checkmark").gameObject.SetActive(false);
                }
            }
        }

    }
    public void Process_1504(string RecvString)
    {
        GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateWindow(WoodsTheExpendablesWindowType.BuffWindow);
        GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().BuffWindow.GetComponent<TowerBuff>().SetInfo(RecvString);
    }
    public void Process_1505(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) != 0)
        {
            if (GameObject.Find("WoodsTheExpendables") != null)
            {
                if (GameObject.Find("BuffWindow") != null)
                {
                    GameObject.Find("BuffWindow").GetComponent<TowerBuff>().NowStar = int.Parse(dataSplit[2]);
                }
                GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateBuffList(dataSplit[3]);
                GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().SetCurFloor(int.Parse(dataSplit[4]));
                if (GameObject.Find("BuffWindow") != null)
                {
                    GameObject.Find("WoodsTheExpendables").transform.Find("Center").transform.Find("BuffWindow").GetComponent<TowerBuff>().SetInfoUpate(RecvString);
                }
                GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().MoveToFloor(int.Parse(dataSplit[4]));
                if (GameObject.Find("DownBg") != null)
                {
                    GameObject.Find("DownBg").transform.Find("LabelNumber").transform.Find("Label").GetComponent<UILabel>().text = dataSplit[2];
                }
            }
        }
        else
        {
            if (int.Parse(dataSplit[1]) == 0)
            {
                UIManager.instance.OpenPromptWindow("跳层", PromptWindow.PromptType.Hint, null, null);
            }
            else if (int.Parse(dataSplit[1]) == 1)
            {
                UIManager.instance.OpenPromptWindow("星数不足", PromptWindow.PromptType.Hint, null, null);
                GameObject.Find("BuffWindow").GetComponent<TowerBuff>().isOpenMessage = true;
            }
            else if (int.Parse(dataSplit[1]) == 2)
            {
                UIManager.instance.OpenPromptWindow("已设置buff", PromptWindow.PromptType.Hint, null, null);
            }
            else if (int.Parse(dataSplit[1]) == 3)
            {
                UIManager.instance.OpenPromptWindow("层数达到最大", PromptWindow.PromptType.Hint, null, null);

            }
        }

    }
    public void Process_1506(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 0)
        {
            if (int.Parse(dataSplit[1]) == 1)
            {
                UIManager.instance.OpenPromptWindow("已经购买", PromptWindow.PromptType.Hint, null, null);
            }
            else if (int.Parse(dataSplit[1]) == 2)
            {
                UIManager.instance.OpenPromptWindow("无法获得当前积分", PromptWindow.PromptType.Hint, null, null);
            }
            else if (int.Parse(dataSplit[1]) == 3)
            {
                UIManager.instance.OpenPromptWindow("当前积分不足", PromptWindow.PromptType.Hint, null, null);
            }

        }
        else
        {
            if (GameObject.Find("WoodsTheExpendables") != null)
            {
                GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateIntegralId(int.Parse(dataSplit[1]));
            }
            CharacterRecorder.instance.HadRewardID = int.Parse(dataSplit[1]);
            string[] trcSplit = dataSplit[2].Split('!');
            string[] Split = trcSplit[2].Split('$');
            CharacterRecorder.instance.AddMoney(int.Parse(Split[1]));
            List<Item> itemlist = new List<Item>();
            for (int i = 0; i < trcSplit.Length - 1; i++)
            {
                string[] item = trcSplit[i].Split('$');
                Item _item = new Item(int.Parse(item[0]), int.Parse(item[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            if (GameObject.Find("AdvanceWindow") != null)
            {
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            }
        }

    }
    public void Process_1507(string RecvString)
    {

    }
    public void Process_1508(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (int.Parse(dataSplit[0]) == 0)
        {
            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().OpenIENum = 7;
            if (int.Parse(dataSplit[1]) == 1)
            {
                UIManager.instance.OpenPromptWindow("钻石不足请充值", PromptWindow.PromptType.Hint, null, null);
                StartCoroutine(GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().AutomaticbrushOnEveryWindow(WoodsTheExpendablesWindowType.TreasureOpenWindow, SomeConditionCloseWindow.Diamondproblem));
            }
            else
            {
                StartCoroutine(GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().AutomaticbrushOnEveryWindow(WoodsTheExpendablesWindowType.TreasureOpenWindow, SomeConditionCloseWindow.Diamondproblem));
            }
        }
        else
        {
            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateWindow(WoodsTheExpendablesWindowType.Right);
            if (GameObject.Find("treasureOpendWindow") != null)
            {
                GameObject.Find("treasureOpendWindow").GetComponent<TowerTreasureOpen>().setInfo(RecvString);
            }
            else
            {
                GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateWindow(WoodsTheExpendablesWindowType.TreasureOpenWindow);
                GameObject.Find("treasureOpendWindow").GetComponent<TowerTreasureOpen>().setInfo(RecvString);
            }
            string[] trcSplit = dataSplit[3].Split('!');
            List<Item> itemlist = new List<Item>();
            for (int i = 0; i < trcSplit.Length - 1; i++)
            {
                string[] item = trcSplit[i].Split('$');
                Item _item = new Item(int.Parse(item[0]), int.Parse(item[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[4]);

            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().OpenIENum = 7;
            if (CharacterRecorder.instance.OpenTreasureNum == CharacterRecorder.instance.AutomaticOpenBoxNum) //宝箱数量够了
            {
                Debug.LogError("宝箱是否满足" + CharacterRecorder.instance.OpenTreasureNum + "  " + CharacterRecorder.instance.AutomaticOpenBoxNum);
                StartCoroutine(GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().AutomaticbrushOnEveryWindow(WoodsTheExpendablesWindowType.TreasureOpenWindow, SomeConditionCloseWindow.OpenTreasurefull));
            }
            else
            {
                StartCoroutine(GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().AutomaticbrushOnEveryWindow(WoodsTheExpendablesWindowType.TreasureOpenWindow, SomeConditionCloseWindow.Nothing));
            }
        }
    }
    public void Process_1509(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (GameObject.Find("WoodsTheExpendables") != null)
        {
            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateWindow(WoodsTheExpendablesWindowType.Right);
            GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().MoveToFloor(int.Parse(dataSplit[1]));
            CharacterRecorder.instance.isOpenAdvance = false;
        }
        SendProcess("1501#;");
    }
    public void Process_1510(string RecvString)
    {
        GameObject fw = GameObject.Find("FightWindow");
        if (fw != null)
        {
            fw.GetComponent<FightWindow>().SetWoodFight(RecvString);
        }
    }
    public void Process_1511(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

    }
    public void Process_1512(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] != "")
        {
            if (GameObject.Find("WoodsTheExpendables") != null)
            {
                GameObject.Find("WoodsTheExpendables").transform.Find("ExpendableslistWindow").gameObject.SetActive(true);
            }
            //GameObject.Find("WoodsTheExpendables").transform.Find("ExpendableslistWindow").gameObject.SetActive(true);
            if (GameObject.Find("ExpendableslistWindow") != null)
            {
                ExpendableslistWindow ExpW = GameObject.Find("ExpendableslistWindow").GetComponent<ExpendableslistWindow>();
                ExpW.ShowList(dataSplit);
            }

            string[] trcSplit = dataSplit[0].Split('$');
            if (trcSplit[0] == "1")
            {
                CharacterRecorder.instance.FirstWoodsName = trcSplit[2];
            }
        }
        else
        {
            Debug.Log("WoodsTheExpendables111");
            if (GameObject.Find("MapUiWindow") == null)
            {
                Debug.Log("WoodsTheExpendables222");
                GameObject.Find("WoodsTheExpendables").GetComponent<WoodsTheExpendables>().UpdateWindow(WoodsTheExpendablesWindowType.Right);
                UIManager.instance.OpenPromptWindow("排行榜第一天不开放", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }
    //TechTree
    public void Process_1601(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.TechRedPoint = false;
        CharacterRecorder.instance.techPoint = int.Parse(dataSplit[3]);

        GameObject litter = GameObject.Find("LittleHelperWindow");
        if (litter != null)
        {
            litter.GetComponent<LittleHelperWindow>().GetIntelligenceMsg_1601(dataSplit);
        }


        if (GameObject.Find("TechWindow"))
        {
            GameObject tw = GameObject.Find("TechWindow").gameObject;
            for (int i = 0; i < dataSplit.Length; i++)
            {
                string[] idSplit = dataSplit[i].Split('$');
                for (int j = 1; j < idSplit.Length; j++)
                {
                    if (idSplit[j - 1] != "")
                    {
                        TechTree TechTreeItem = TextTranslator.instance.GetTechTreeByID(int.Parse(idSplit[j - 1]));
                        //if (TechTreeItem.LevelUpNeedPoint <= int.Parse(dataSplit[3]))
                        //{
                        //    Debug.LogError("需要  " + TechTreeItem.LevelUpNeedPoint);
                        //    CharacterRecorder.instance.TechRedPoint = true;
                        //}
                        GameObject.Find("TechTree").GetComponent<TechTreeList>().ShowItem(TechTreeItem, TechTreeItem.Level);
                    }
                }
            }
            tw.GetComponent<TechWindow>().HavePonitLabel.text = dataSplit[3];

            tw.GetComponent<TechWindow>().CostPonitLabel.text = dataSplit[4];
            if (int.Parse(dataSplit[5]) >= 100)
            {
                tw.GetComponent<TechWindow>().Depth = 2;
                tw.GetComponent<TechWindow>().UnClockButton();
            }
            if (int.Parse(dataSplit[6]) >= 400)
            {
                tw.GetComponent<TechWindow>().Depth = 3;
                tw.GetComponent<TechWindow>().UnClockButton();
            }
        }
        else
        {
            if (CharacterRecorder.instance.techPoint >= 6 && CharacterRecorder.instance.lastGateID >= TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.qingbao).Level + 1)
            {
                CharacterRecorder.instance.SetRedPoint(16, true);
            }
            else
            {
                CharacterRecorder.instance.SetRedPoint(16, false);
            }
            //if (GameObject.Find("MainWindow") != null)
            //{
            //    //for (int i = 0; i < dataSplit.Length; i++)
            //    //{
            //    //    string[] idSplit = dataSplit[i].Split('$');
            //    //    for (int j = 1; j < idSplit.Length; j++)
            //    //    {
            //    //        if (idSplit[j - 1] != "")
            //    //        {
            //    //            TechTree TechTreeItem = TextTranslator.instance.GetTechTreeByID(int.Parse(idSplit[j - 1]));
            //    //            if (TechTreeItem.LevelUpNeedPoint <= int.Parse(dataSplit[3]))
            //    //            {
            //    //                Debug.LogError("需要  " + TechTreeItem.LevelUpNeedPoint);
            //    //                CharacterRecorder.instance.TechRedPoint = true;
            //    //            }
            //    //        }
            //    //    }
            //    //}

            //    if (CharacterRecorder.instance.techPoint >= 6 && CharacterRecorder.instance.lastGateID >= TextTranslator.instance.GetGuildByType((int)TextTranslator.NewGuildIdEnum.jinjizhiliao).Level + 1)
            //    {
            //        //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(13, true);
            //    }
            //    else
            //    {
            //        //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(13, false);
            //    }
            //}
        }
    }
    public void Process_1602(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {

            int itemName;
            TechTree TechTreeDic = TextTranslator.instance.GetTechTreeByID(int.Parse(dataSplit[7]));
            itemName = TechTreeDic.Icon;
            GameObject.Find("RewardWindow").GetComponent<RewardWindow>().EXPlabel.text = TechTreeDic.Level.ToString() + "/10";
            GameObject.Find("TechWindow").GetComponent<TechWindow>().HavePonitLabel.text = dataSplit[4];
            GameObject.Find("RewardWindow").GetComponent<RewardWindow>().GetTreeInfo(false, itemName, TechTreeDic.Level, 1);
            if (TechTreeDic.Level % 10 >= 7)
            {
                GameObject.Find("TechTree").GetComponent<TechTreeList>().ListItemUnLock(TechTreeDic);
            }

            CharacterRecorder.instance.gold = int.Parse(dataSplit[6]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            SendProcess("1601#");
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("长官,等级不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    //UIManager.instance.OpenPromptWindow("长官,重复开启", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("长官,前置情报", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("长官,情报点不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "4":
                    UIManager.instance.OpenPromptWindow("长官,上一层情报点总数不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "5":
                    UIManager.instance.OpenPromptWindow("长官,金币不足", PromptWindow.PromptType.Hint, null, null);
                    break;
            }

        }
    }
    public void Process_1603(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            SendProcess("1601#");
            GameObject.Find("TechTree").GetComponent<TechTreeList>().ResetListItem();
            CharacterRecorder.instance.AddLunaGem(-500);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[5]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
            }
        }

    }
    /// <summary>
    /// 芯片
    /// </summary>
    /// <param name="RecvString"></param>

    public void Process_1611(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        GameObject chip = GameObject.Find("ChipWindow");
        if (chip != null)
        {
            chip.GetComponent<ChipWindow>().SetInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        }
    }
    public void Process_1612(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] != "0")
        {

            SendProcess("1611#");
            CharacterRecorder.instance.ChangeAttribute = true;
        }
        else
        {
            switch (int.Parse(dataSplit[1]))
            {
                case 0:

                    break;
                case 1:

                    break;
            }
        }

    }
    public void Process_1701(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.EveryDayNumberRedPoint[int.Parse(dataSplit[0]) - 1] = int.Parse(dataSplit[1]);
        CharacterRecorder.instance.EveryDayTimerRedPoint[int.Parse(dataSplit[0]) - 1] = int.Parse(dataSplit[3]);
        switch (dataSplit[0])
        {
            case "1":
                GameObject hw1 = GameObject.Find("HunterWindow");
                if (hw1 != null)
                {
                    hw1.GetComponent<HunterWindow>().SetHunterInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
                }
                break;
            case "2":
                GameObject hw2 = GameObject.Find("HunterWindow");
                if (hw2 != null)
                {
                    hw2.GetComponent<HunterWindow>().SetHunterInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
                }
                break;
            case "3":
                GameObject mw1 = GameObject.Find("MilitaryWindow");
                if (mw1 != null)
                {
                    mw1.GetComponent<MilitaryWindow>().SetImpregnableInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
                }
                break;
            case "4":
                GameObject mw2 = GameObject.Find("MilitaryWindow");
                if (mw2 != null)
                {
                    mw2.GetComponent<MilitaryWindow>().SetAttackDefInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
                }
                break;
            case "5":

                GameObject mw3 = GameObject.Find("MilitaryWindow");
                if (mw3 != null)
                {
                    mw3.GetComponent<MilitaryWindow>().SetStormInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
                }
                break;
            case "6":
                GameObject hw3 = GameObject.Find("HunterWindow");
                if (hw3 != null)
                {
                    hw3.GetComponent<HunterWindow>().SetHunterInfo(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
                    hw3.GetComponent<HunterWindow>().IntOpenDay(int.Parse(dataSplit[4]), int.Parse(dataSplit[2]));
                }
                break;
        }
        GameObject cw = GameObject.Find("ChallengeWindow");
        if (cw != null)
        {
            cw.GetComponent<ChallengeWindow>().SetEveryDayRedPoint_1701(int.Parse(dataSplit[1]), int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
        }
        if (int.Parse(dataSplit[0]) == 3)
        {
            if (int.Parse(dataSplit[4]) != 1 || int.Parse(dataSplit[4]) != 3 || int.Parse(dataSplit[4]) != 7)
            {
                CharacterRecorder.instance.EveryDayNumberRedPoint[2] = 0;
            }
        }
        if (int.Parse(dataSplit[0]) == 4)
        {
            if (int.Parse(dataSplit[4]) != 2 || int.Parse(dataSplit[4]) != 4 || int.Parse(dataSplit[4]) != 7)
            {
                CharacterRecorder.instance.EveryDayNumberRedPoint[3] = 0;
            }
        }
        if (int.Parse(dataSplit[0]) == 5)
        {
            if (int.Parse(dataSplit[4]) != 5 || int.Parse(dataSplit[4]) != 6 || int.Parse(dataSplit[4]) != 7)
            {
                CharacterRecorder.instance.EveryDayNumberRedPoint[4] = 0;
            }
        }
        if (int.Parse(dataSplit[0]) == 6)
        {
            if (int.Parse(dataSplit[4]) != 1 || int.Parse(dataSplit[4]) != 6 || int.Parse(dataSplit[4]) != 3 || int.Parse(dataSplit[4]) != 7)
            {
                CharacterRecorder.instance.EveryDayNumberRedPoint[5] = 0;
            }
        }
        CharacterRecorder.instance.Collision();
    }
    public void Process_1702(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.IsOpen = true;
        List<Item> itemlist = new List<Item>();
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("EverydayWindow") != null)
            {
                Item _item = new Item(90001, int.Parse(dataSplit[1]));
                itemlist.Add(_item);
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
                CharacterRecorder.instance.gold += int.Parse(dataSplit[1]);
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
                SendProcess("1701#1");

            }
            else
            {
                UIManager.instance.OpenPanel("EveryResultWindow", false);
                GameObject.Find("EveryResultWindow").GetComponent<EveryResultWindow>().SetHunterInfo(int.Parse(dataSplit[1]));
            }
            CharacterRecorder.instance.gold = int.Parse(dataSplit[2]);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("客户端数据异常", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("次数不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("未开放星期", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "4":
                    UIManager.instance.OpenPromptWindow("不在开放时间", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
        }
    }
    public void Process_1703(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        CharacterRecorder.instance.IsOpen = true;
        List<Item> itemlist = new List<Item>();
        string[] rewardSplit = dataSplit[1].Split('!');
        if (GameObject.Find("EverydayWindow") != null)
        {
            for (int i = 0; i < rewardSplit.Length - 1; i++)
            {
                string[] ticSplit = rewardSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            SendProcess("1701#2");
        }
        else
        {
            UIManager.instance.OpenPanel("EveryResultWindow", false);
            GameObject.Find("EveryResultWindow").GetComponent<EveryResultWindow>().SetThousandInfo(rewardSplit);
        }
    }

    public void Process_1704(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] rewardSplit = dataSplit[1].Split('!');
        if (dataSplit[0] == "1")
        {
            //CharacterRecorder.instance.IsOpen = true;
            //ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
            //UIManager.instance.OpenPanel("ResultWindow", false);
            //rw.Init(true, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, dataSplit[1], 0, "", 0);
            UIManager.instance.OpenPanel("EveryResultWindow", false);
            GameObject.Find("EveryResultWindow").GetComponent<EveryResultWindow>().SetMilitaryInfo(rewardSplit);

        }
    }
    public void Process_1801(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        int roleType = int.Parse(dataSplit[0]);
        int openCount = int.Parse(dataSplit[1]);
        TextTranslator.instance.OneTypeLabsLimitList = TextTranslator.instance.GetOneTypeLabsLimitByRoleType(roleType);
        for (int i = 2; i < dataSplit.Length - 1; i++)
        {
            string[] secSplit = dataSplit[i].Split('$');
            int LabItemPosNum = i - 1;
            ReformLabItemData _targetLabItemData = TextTranslator.instance.GetOneLabsItemByRoleTypeAndPosNum(roleType, LabItemPosNum);
            if (LabItemPosNum > openCount)
            {
                int status = 0;
                _targetLabItemData.state = status;
            }
            else if (secSplit[0] != "0")
            {
                int status = 2;
                _targetLabItemData.state = status;
                _targetLabItemData.SetReformLabItemData(int.Parse(secSplit[0]), int.Parse(secSplit[1]), int.Parse(secSplit[2]), float.Parse(secSplit[3]));
            }
            else
            {
                int status = 1;
                _targetLabItemData.state = status;
                if (CharacterRecorder.instance.IsNeedAddEmpetyCount)
                {
                    CharacterRecorder.instance.empetyCountToOnputHero += 1;
                }
            }

            if (!LabWindow.mOnLineTrainingHeroList.Contains(_targetLabItemData.mHero))
            {
                LabWindow.mOnLineTrainingHeroList.Add(_targetLabItemData.mHero);
            }
        }



        if (GameObject.Find("LabWindow") != null)
        {
            LabWindow _LabWindow = GameObject.Find("LabWindow").GetComponent<LabWindow>();
            _LabWindow.SetReformLabWindow(TextTranslator.instance.OneTypeLabsLimitList);
        }
        if (CharacterRecorder.instance.IsNeedAddEmpetyCount)
        {
            GameObject _MainWindowObj = GameObject.Find("MainWindow");
            CharacterRecorder.instance.SetLabRedPoint();
        }
        GameObject litter = GameObject.Find("LittleHelperWindow");
        if (litter != null)
        {
            litter.GetComponent<LittleHelperWindow>().ReceiverLaboratoryMsg(openCount);
        }
    }
    public void Process_1802(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.empetyCountToOnputHero -= 1;
            GameObject.Find("LabWindow").GetComponent<LabWindow>().ResetLegionTrainingGroundWindow(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
        }
        else
        {
            //UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[0], PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_1803(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');

        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.empetyCountToOnputHero += 1;
            GameObject.Find("LabWindow").GetComponent<LabWindow>().RemoveLabHeroWindow(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
        }
        else
        {
            //UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[0], PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_1804(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[3]);
            GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            GameObject.Find("LabWindow").GetComponent<LabWindow>().ResetLabWindow(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("客户端数据异常", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("格子已全开", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("前一个格子未开", PromptWindow.PromptType.Hint, null, null); break;
                case "3": UIManager.instance.OpenPromptWindow("等级不足", PromptWindow.PromptType.Hint, null, null); break;
                case "4": UIManager.instance.OpenPromptWindow("VIP不足", PromptWindow.PromptType.Hint, null, null); break;
                case "5": UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null); break;
                default: UIManager.instance.OpenPromptWindow("服务器错误码" + dataSplit[1], PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }
    public void Process_1901(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        Debug.LogError("1901..." + RecvString);
        CharacterRecorder.instance.HoldOnLeftTime = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.HoldKillNum = int.Parse(dataSplit[2]);
        if (int.Parse(dataSplit[4]) > 0 && GameObject.Find("MainWindow") != null)
        {
            //NetworkHandler.instance.SendProcess("1902#;");

            List<Item> list = new List<Item>();
            list.Clear();
            if (dataSplit[1] != "")
            {
                string[] dataSplit2 = dataSplit[1].Split('!');
                for (int i = 0; i < dataSplit2.Length - 1; i++)
                {
                    string[] itemSplit = dataSplit2[i].Split('$');
                    Item item = new Item(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]));
                    list.Add(item);
                }
            }
            UIManager.instance.OpenSinglePanel("HoldOnAwardWindow", false);
            GameObject _HoldOnAwardWindowObj = GameObject.Find("HoldOnAwardWindow");
            if (_HoldOnAwardWindowObj != null)
            {
                _HoldOnAwardWindowObj.GetComponent<HoldOnAwardWindow>().SetHoldOnAwardWindow(list, int.Parse(dataSplit[3]), int.Parse(dataSplit[4]));
            }
        }
        else
        {
            CharacterRecorder.instance.HoldOnLeftTime = int.Parse(dataSplit[0]);
            // StartCoroutine(CharacterRecorder.instance.AutoUpdateHoldOnLeftTime());
        }

        //if (dataSplit[0] == "0")
        //{
        //    NetworkHandler.instance.SendProcess("1902#;");
        //}
        //else
        //{
        //    CharacterRecorder.instance.HoldOnLeftTime = int.Parse(dataSplit[0]);
        //    // StartCoroutine(CharacterRecorder.instance.AutoUpdateHoldOnLeftTime());
        //}
    }
    public void Process_1902(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            Debug.LogError("取得挂机奖励");
            CharacterRecorder.instance.HoldKillNum = int.Parse(dataSplit[3]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[5]);
            CharacterRecorder.instance.gold = int.Parse(dataSplit[6]);
            int leftTimes = int.Parse(dataSplit[1]);
            if (leftTimes > 0)
            {
                CharacterRecorder.instance.HoldOnLeftTime = leftTimes;
            }
            string[] dataSplit2 = dataSplit[2].Split('!');
            List<Item> list = new List<Item>();
            for (int i = 0; i < dataSplit2.Length - 1; i++)
            {
                string[] itemSplit = dataSplit2[i].Split('$');
                Item item = new Item(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]));
                list.Add(item);

            }
            UpDateTopContentData(list);
            GameObject _MainWindowObj = GameObject.Find("MainWindow");
            if (_MainWindowObj != null)
            {
                MainWindow Mw = _MainWindowObj.GetComponent<MainWindow>();
                if (list.Count > 0)
                {
                    Mw.SetLabelEffectAwardInfo(list[0].itemCode, list[0].itemCount, int.Parse(dataSplit[7]));
                }
                else
                {
                    Mw.SetLabelEffectAwardInfo(0, 0, int.Parse(dataSplit[7]));
                }

                if (dataSplit[4] == "1")
                {
                    SendProcess("1904#;");
                }

                Mw.UpdateHoldOnLeftTime();
                Mw.ReSetRole();
            }
            GameObject hold = GameObject.Find("HoldOnAwardWindow");
            if (hold != null)
            {
                CharacterRecorder.instance.exp += int.Parse(dataSplit[7]);
                HoldOnAwardWindow.GetExpNum += int.Parse(dataSplit[7]);
                hold.GetComponent<HoldOnAwardWindow>().SetExp_1902(HoldOnAwardWindow.GetExpNum);
            }
        }
        else
        {
            CharacterRecorder.instance.HoldKillNum = int.Parse(dataSplit[1]); ;
        }
    }
    public void Process_1903(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        string[] dataSplit2 = dataSplit[0].Split('!');
        List<Item> list = new List<Item>();
        for (int i = 0; i < dataSplit2.Length - 1; i++)
        {
            string[] itemSplit = dataSplit2[i].Split('$');
            Item item = new Item(int.Parse(itemSplit[0]), int.Parse(itemSplit[1]));
            list.Add(item);
        }
        if (GameObject.Find("MainWindow") != null)
        {
            UIManager.instance.OpenSinglePanel("HoldOnAwardWindow", false);
        }
        GameObject _HoldOnAwardWindowObj = GameObject.Find("HoldOnAwardWindow");
        if (_HoldOnAwardWindowObj != null)
        {
            _HoldOnAwardWindowObj.GetComponent<HoldOnAwardWindow>().SetHoldOnAwardWindow(list, int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), "1903");
        }
    }
    /// <summary>
    /// 获取重生
    /// </summary>
    /// <param name="RecvString">服务器端数据</param>
    public void Process_1911(string RecvString)
    {
        List<string> data = new List<string>();
        string[] dataS = RecvString.Split(';');
        string[] dataSplit = dataS[0].Split('!');
        for (int i = 0; i < dataSplit.Length - 1; i++)
        {
            data.Add(dataSplit[i]);
        }
        if (data.Count > 0)
        {
            GameObject rebirthWindow = GameObject.Find("RebirthWindow");
            if (rebirthWindow != null)
            {
                rebirthWindow.GetComponent<RebirthWindow>().ReceivedAgreement_1911(data, int.Parse(dataS[1]), dataS[2]);
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("重生返回道具为空.", PromptWindow.PromptType.Hint, null, null);
        }
    }
    /// <summary>
    /// 设定重生
    /// </summary>
    /// <param name="RecvString">服务器端数据</param>
    public void Process_1912(string RecvString)
    {
        List<string> data = new List<string>();
        string[] dataSplit = RecvString.Split(';');
        int one = 0;
        bool successInt = int.TryParse(dataSplit[0], out one);


        for (int i = LabWindow.mOnLineTrainingHeroList.Count - 1; i >= 0; i--)
        {
            if (LabWindow.mOnLineTrainingHeroList[i] != null)
            {
                if (LabWindow.mOnLineTrainingHeroList[i].characterRoleID == int.Parse(dataSplit[1]))
                {
                    LabWindow.mOnLineTrainingHeroList.RemoveAt(i);
                }
            }
        }

        GameObject rw = GameObject.Find("RebirthWindow");
        if (successInt)
        {
            if (one == 0)
            {
                if (rw != null)
                {
                    rw.transform.FindChild("MaskSprite").gameObject.SetActive(false);
                    rw.GetComponent<RebirthWindow>().RebirthFailed_1912();
                }
                return;
            }
            string[] dataSplit1 = dataSplit[2].Split('!');
            for (int i = 0; i < dataSplit1.Length - 1; i++)
            {
                //string[] dataSprite2 = dataSplit1[i].Split('$');
                //dataSprite2[0]  是预计返回的道具(item表的ID号)   dataSprite2[1]  是返回的数量
                data.Add(dataSplit1[i]);
            }
            if (data.Count > 0)
            {
                //GameObject rebirthWindow = GameObject.Find("RebirthWindow");
                if (rw != null)
                {
                    rw.GetComponent<RebirthWindow>().ReceivedAgreement_1912(data, int.Parse(dataSplit[3]));
                }
            }
            else
            {
                if (rw != null)
                {
                    rw.transform.FindChild("MaskSprite").gameObject.SetActive(false);
                }
                UIManager.instance.OpenPromptWindow("重生返回道具为空.", PromptWindow.PromptType.Hint, null, null);
            }

        }
        else
        {
            if (rw != null)
            {
                rw.transform.FindChild("MaskSprite").gameObject.SetActive(false);
            }
            Debug.LogError("数据传输错误.  =: " + RecvString);
        }
    }
    public void Process_1904(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.exp = int.Parse(dataSplit[1]);

            GameObject hold = GameObject.Find("HoldOnAwardWindow");
            if (hold != null)
            {
                hold.GetComponent<HoldOnAwardWindow>().SetSliderInfo();
            }
        }
    }

    public void Process_1921(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        PlayerPrefs.SetInt("SmallGoalGetID", int.Parse(dataSplit[0]));
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().ChoseWillOpenTipButton();
        }
    }

    public void Process_1922(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        if (dataSplit[0] == "1")
        {
            PlayerPrefs.SetInt("SmallGoalGetID", int.Parse(dataSplit[1]));
            List<Item> itemlist = new List<Item>();
            string[] rewardSplit = dataSplit[2].Split('!');
            for (int i = 0; i < rewardSplit.Length - 1; i++)
            {
                string[] ticSplit = rewardSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);


            if (GameObject.Find("MainWindow") != null)
            {
                GameObject.Find("MainWindow").GetComponent<MainWindow>().ChoseWillOpenTipButton();
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                //UIManager.instance.OpenPromptWindow("奖励档次错误", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("未达成关卡条件", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("该档奖励已领", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_1705(string RecvString)
    {
        string[] dataSplit = RecvString.Split(';');
        List<Item> itemlist = new List<Item>();
        string[] rewardSplit = dataSplit[1].Split('!');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("EverydayWindow") != null)
            {
                for (int i = 0; i < rewardSplit.Length - 1; i++)
                {
                    string[] ticSplit = rewardSplit[i].Split('$');
                    Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                    itemlist.Add(_item);
                }
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
                if (dataSplit[2] == "3")
                {
                    SendProcess("1701#3");
                }
                else if (dataSplit[2] == "4")
                {
                    SendProcess("1701#4");
                }
                else if (dataSplit[2] == "5")
                {
                    SendProcess("1701#5");
                }
            }
            else
            {
                UIManager.instance.OpenPanel("EveryResultWindow", false);
                GameObject.Find("EveryResultWindow").GetComponent<EveryResultWindow>().SetMilitaryInfo(rewardSplit);
                //CharacterRecorder.instance.IsOpen = true;
                //UIManager.instance.OpenPanel("ResultWindow", false);
                //ResultWindow rw = GameObject.Find("ResultWindow").GetComponent<ResultWindow>();
                //rw.Init(true, CharacterRecorder.instance.stamina, 0, CharacterRecorder.instance.exp, 0, dataSplit[1], 0, "", 0);
            }

        }
    }

    public void Process_6101(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[2] != "")
        {
            string[] trcSplit = dataSplit[2].Split('!');
            List<TeamBrowseItemDate> _TeamBrowseItemDate = new List<TeamBrowseItemDate>();
            for (int i = 0; i < trcSplit.Length - 1; i++)
            {
                string[] prcSplit = trcSplit[i].Split('$');
                if (prcSplit[2] == CharacterRecorder.instance.characterName)
                {
                    SendProcess("6105#" + prcSplit[0] + ";" + "1;");
                }
                _TeamBrowseItemDate.Add(new TeamBrowseItemDate(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]),
                    int.Parse(prcSplit[0]), int.Parse(prcSplit[1]), prcSplit[2], int.Parse(prcSplit[3]), prcSplit[4],
                    int.Parse(prcSplit[5]), int.Parse(prcSplit[6]), int.Parse(prcSplit[7]), prcSplit[8], prcSplit[9], prcSplit[10]));
            }
            if (GameObject.Find("TeamBrowseWindow") != null)
            {
                GameObject.Find("TeamBrowseWindow").GetComponent<TeamBrowseWindow>().SetTeamBrowse(_TeamBrowseItemDate);
            }
        }
        else
        {
            if (GameObject.Find("TeamBrowseWindow") != null)
            {
                GameObject.Find("TeamBrowseWindow").GetComponent<TeamBrowseWindow>().SetNoTeamBrowse();
            }
        }
    }

    public void Process_6102(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("TeamInvitationWindow") == null)
            {
                CharacterRecorder.instance.TeamID = int.Parse(dataSplit[5]);
                CharacterRecorder.instance.TeamPosition = 1;
                CharacterRecorder.instance.IsCaptain = true;
                UIManager.instance.OpenSinglePanel("TeamInvitationWindow", true);
                GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().PlayerslimitSet(int.Parse(dataSplit[1]), dataSplit[2], dataSplit[3], dataSplit[4], int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            }
            else
            {
                GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().PlayerslimitResufh(int.Parse(dataSplit[1]), dataSplit[2], dataSplit[3], dataSplit[4], int.Parse(dataSplit[5]), int.Parse(dataSplit[6]));
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("重置房间失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_6103(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.CopyNumber = int.Parse(dataSplit[1]);
            UIManager.instance.OpenSinglePanel("TeamInvitationWindow", true);
            GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().ChoseBattleHero2(recvString);
        }
        else if (dataSplit[0] == "0")
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("队伍已经解散", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("房间人数已满", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void Process_6104(string recvString)
    {
        //string[] dataSplit = recvString.Split(';'); 
        if (GameObject.Find("TeamInvitationWindow") != null)
        {
            GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().ChoseBattleHero(recvString);
        }
        //GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().ChoseBattleHero(recvString);
    }
    //public void Process_6105(string recvString)
    //{
    //    string[] dataSplit = recvString.Split(';');
    //    if (dataSplit[0] == "1")
    //    {
    //        if (dataSplit[2] == "1" && CharacterRecorder.instance.TeamPosition == int.Parse(dataSplit[2]))
    //        {
    //            if (GameObject.Find("TeamInvitationWindow") != null)
    //            {
    //                //UIManager.instance.BackUI();
    //                UIManager.instance.OpenPanel("MainWindow", true);
    //                UIManager.instance.OpenPromptWindow("解队成功", PromptWindow.PromptType.Hint, null, null);
    //            }
    //        }
    //        else if (dataSplit[2] == "1" && CharacterRecorder.instance.TeamPosition != int.Parse(dataSplit[2]))
    //        {
    //            if (GameObject.Find("TeamInvitationWindow") != null)
    //            {
    //                //UIManager.instance.BackUI();
    //                UIManager.instance.OpenPanel("MainWindow", true);
    //                UIManager.instance.OpenPromptWindow("房间已解散", PromptWindow.PromptType.Hint, null, null);
    //            }
    //        }
    //        else if (dataSplit[2] != "1" && CharacterRecorder.instance.TeamPosition == int.Parse(dataSplit[2]))
    //        {
    //            if (GameObject.Find("TeamInvitationWindow") != null)
    //            {
    //                UIManager.instance.OpenPanel("MainWindow", true);
    //            }

    //            if (GameObject.Find("TeamBrowseWindow") != null)
    //            {
    //                NetworkHandler.instance.SendProcess("6101#" + CharacterRecorder.instance.CopyNumber + ";");
    //                UIManager.instance.OpenPromptWindow("你已离队", PromptWindow.PromptType.Hint, null, null);
    //            }
    //            else
    //            {
    //                UIManager.instance.OpenPromptWindow("你已离队", PromptWindow.PromptType.Hint, null, null);
    //            }
    //        }
    //        else if (dataSplit[2] != "1" && CharacterRecorder.instance.TeamPosition != int.Parse(dataSplit[2]))
    //        {
    //            if (GameObject.Find("TeamInvitationWindow") != null)
    //            {
    //                GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().OneHeroLiveRoom(int.Parse(dataSplit[2]));
    //            }
    //        }
    //    }
    //    else
    //    {
    //        UIManager.instance.OpenPromptWindow("队伍已解散", PromptWindow.PromptType.Hint, null, null);
    //        UIManager.instance.panelList.Clear();
    //        if (GameObject.Find("TeamInvitationWindow") != null)
    //        {
    //            DestroyImmediate(GameObject.Find("TeamInvitationWindow"));
    //        }
    //        if (GameObject.Find("TeamFightChoseWindow") != null)
    //        {
    //            DestroyImmediate(GameObject.Find("TeamFightChoseWindow"));
    //        }
    //        if (GameObject.Find("TeamFightCamer") != null)
    //        {
    //            DestroyImmediate(GameObject.Find("TeamFightCamer"));
    //        }

    //        if (GameObject.Find("CopyScence") != null)
    //        {
    //            DestroyImmediate(GameObject.Find("CopyScence"));
    //        }
    //        UIManager.instance.OpenPanel("MainWindow", true);
    //    }
    //}

    public void Process_6105(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (dataSplit[2] == "1" && CharacterRecorder.instance.TeamPosition == int.Parse(dataSplit[2]))
            {
                if (GameObject.Find("TeamInvitationWindow") != null)
                {
                    UIManager.instance.OpenPromptWindow("解队成功", PromptWindow.PromptType.Hint, null, null);
                    CloseAllTeamCopyWindow();
                    UIManager.instance.OpenSinglePanel("TeamCopyWindow", true);
                }
            }
            else if (dataSplit[2] == "1" && CharacterRecorder.instance.TeamPosition != int.Parse(dataSplit[2]))
            {
                if (GameObject.Find("TeamInvitationWindow") != null)
                {
                    UIManager.instance.OpenPromptWindow("房间已解散", PromptWindow.PromptType.Hint, null, null);
                    CloseAllTeamCopyWindow();
                    UIManager.instance.OpenSinglePanel("TeamCopyWindow", true);
                }
            }
            else if (dataSplit[2] != "1" && CharacterRecorder.instance.TeamPosition == int.Parse(dataSplit[2]))
            {
                if (GameObject.Find("TeamInvitationWindow") != null)
                {
                    CloseAllTeamCopyWindow();
                    UIManager.instance.OpenSinglePanel("TeamCopyWindow", true);
                }

                if (GameObject.Find("TeamBrowseWindow") != null)
                {
                    NetworkHandler.instance.SendProcess("6101#" + CharacterRecorder.instance.CopyNumber + ";");
                    UIManager.instance.OpenPromptWindow("你已离队", PromptWindow.PromptType.Hint, null, null);
                }
                else
                {
                    UIManager.instance.OpenPromptWindow("你已离队", PromptWindow.PromptType.Hint, null, null);
                }
            }
            else if (dataSplit[2] != "1" && CharacterRecorder.instance.TeamPosition != int.Parse(dataSplit[2]))
            {
                if (GameObject.Find("TeamInvitationWindow") != null)
                {
                    GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().OneHeroLiveRoom(int.Parse(dataSplit[2]));
                }
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("队伍已解散", PromptWindow.PromptType.Hint, null, null);
            CloseAllTeamCopyWindow();
            UIManager.instance.OpenSinglePanel("TeamCopyWindow", true);
        }
    }


    void CloseAllTeamCopyWindow()
    {
        PictureCreater.instance.DestroyAllComponent();
        CharacterRecorder.instance.TeamAwardList = null;
        //while (GameObject.Find("TeamInvitationWindow") != null)
        //{
        //    DestroyImmediate(GameObject.Find("TeamInvitationWindow"));
        //}
        //while (GameObject.Find("TeamFightChoseWindow") != null)
        //{
        //    DestroyImmediate(GameObject.Find("TeamFightChoseWindow"));
        //}
        //while (GameObject.Find("TeamFightCamer") != null)
        //{
        //    DestroyImmediate(GameObject.Find("TeamFightCamer"));
        //}
        //while (GameObject.Find("CopyScence") != null)
        //{
        //    DestroyImmediate(GameObject.Find("CopyScence"));
        //}
    }
    public void Process_6106(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        CharacterRecorder.instance.TeamAwardList = null;
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("TeamInvitationWindow") != null || GameObject.Find("TeamFightChoseWindow") != null)
            {
                //if (GameObject.Find("TeamInvitationWindow") != null)
                //{
                //    GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().SetHeroIcon();
                //}
                CharacterRecorder.instance.TeamID = int.Parse(dataSplit[1]);
                CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
                CharacterRecorder.instance.CopyHeroIconList.Clear();
                CharacterRecorder.instance.CopyHeroNameList.Clear();

                string[] trcSplit = dataSplit[2].Split('!');
                for (int i = 0; i < trcSplit.Length - 1; i++)
                {
                    string[] prcSplit = trcSplit[i].Split('$');
                    CharacterRecorder.instance.CopyHeroIconList.Add(int.Parse(prcSplit[1]));
                    CharacterRecorder.instance.CopyHeroNameList.Add(prcSplit[0]);
                }

                UIManager.instance.OpenSinglePanel("TeamFightChoseWindow", true);
                GameObject.Find("TeamFightChoseWindow").GetComponent<TeamFightChoseWindow>().SetTeamFightChoseWindow(int.Parse(dataSplit[1]));

                CharacterRecorder.instance.gold = int.Parse(dataSplit[3]);
                GameObject _TopContent = GameObject.Find("TopContent");
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("进入副本失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_6107(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.TeamAwardList += dataSplit[3];
            Debug.Log("成功");
            //if (GameObject.Find("TeamFightCamer") != null)
            //{
            //    GameObject.Find("TeamFightCamer").GetComponent<TeamFightCamer>().SetKillBoss();
            //    //GameObject.Find("TeamFightCamer").GetComponent<TeamFightCamer>().
            //}
            if (GameObject.Find("TeamFightChoseWindow") != null)
            {
                TeamFightChoseWindow TC = GameObject.Find("TeamFightChoseWindow").GetComponent<TeamFightChoseWindow>();
                TC.SetKillBoss(true, int.Parse(dataSplit[2]));
                TC.SetTeamAward();
                if (dataSplit[2] == "10")
                {
                    TC.SetLastAwardWindow();
                }
                //GameObject.Find("TeamFightChoseWindow").GetComponent<TeamFightChoseWindow>().SetTeamAward();
            }
        }
        else
        {
            Debug.Log("失败");
            if (GameObject.Find("TeamFightChoseWindow") != null)
            {
                TeamFightChoseWindow TC = GameObject.Find("TeamFightChoseWindow").GetComponent<TeamFightChoseWindow>();
                TC.SetKillBoss(false, 1);
                TC.MissionWindow();
            }
        }
    }

    public void Process_6108(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        CharacterRecorder.instance.TeamFightNum = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.TeamHelpTime = int.Parse(dataSplit[2]);
        if (GameObject.Find("TeamCopyWindow") != null)
        {
            GameObject.Find("TeamCopyWindow").GetComponent<TeamCopyWindow>().SetResidueNumber(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
        GameObject ch = GameObject.Find("ChallengeWindow");
        if (ch != null)
        {
            ch.GetComponent<ChallengeWindow>().SetTeamCopyRedPoint_6108(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
        }
        CharacterRecorder.instance.Collision();
    }

    public void Process_6109(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //string[] trcSplit = dataSplit[3].Split('!');
            string[] prcSplit = dataSplit[3].Split('$');
            List<TeamBrowseItemDate> _TeamBrowseItemDate = new List<TeamBrowseItemDate>();
            _TeamBrowseItemDate.Add(new TeamBrowseItemDate(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]),
                int.Parse(prcSplit[0]), int.Parse(prcSplit[1]), prcSplit[2], int.Parse(prcSplit[3]), prcSplit[4],
                int.Parse(prcSplit[5]), int.Parse(prcSplit[6]), int.Parse(prcSplit[7]), prcSplit[8], prcSplit[9], prcSplit[10]));
            if (GameObject.Find("TeamBrowseWindow") != null)
            {
                GameObject.Find("TeamBrowseWindow").GetComponent<TeamBrowseWindow>().SetTeamBrowse(_TeamBrowseItemDate);
            }


            if (GameObject.Find("PopupWindow") != null && GameObject.Find("TeamInvitationWindow") == null)
            {
                GameObject.Find("PopupWindow").GetComponent<PopupWindow>().JoinTeamCondition(_TeamBrowseItemDate[0]);
            }
            else if (GameObject.Find("MainWindow") != null && GameObject.Find("TeamInvitationWindow") == null)
            {
                GameObject.Find("MainWindow").GetComponent<MainWindow>().JoinTeamCondition(_TeamBrowseItemDate[0]);
            }
            else if (GameObject.Find("TeamCopyWindow") != null && GameObject.Find("TeamInvitationWindow") == null)
            {
                GameObject.Find("TeamCopyWindow").GetComponent<TeamCopyWindow>().JoinTeamCondition(_TeamBrowseItemDate[0]);
            }
        }
        else
        {
            if (GameObject.Find("TeamBrowseWindow") != null)
            {
                GameObject.Find("TeamBrowseWindow").GetComponent<TeamBrowseWindow>().SetNoTeamBrowse2();
            }
            if (GameObject.Find("ChatWindowNew") != null && GameObject.Find("TeamInvitationWindow") == null)
            {
                UIManager.instance.OpenPromptWindow("房间已解散", PromptWindow.PromptType.Hint, null, null);
            }
            UIManager.instance.OpenPromptWindow("房间已解散", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_6110(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("TeamInvitationWindow") != null)
            {
                GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().PlayerSetOut(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            }
        }
        else
        {

        }
    }

    public void Process_6111(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("TeamInvitationWindow") != null)
            {
                GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().InstanceTalk(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            }
            else if (GameObject.Find("TeamFightChoseWindow") != null)
            {
                GameObject.Find("TeamFightChoseWindow").GetComponent<TeamFightChoseWindow>().InstanceTalk(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
            }
        }
        else
        {

        }
    }
    public void Process_6112(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            TeamInvitationWindow tw = GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>();
            tw.EmploymentButton.transform.Find("HeroMessage").GetComponent<UILabel>().text = "雇佣:" + dataSplit[1];
            tw.TeamFightLabel.text = (int.Parse(tw.TeamFightLabel.text) + int.Parse(dataSplit[2])).ToString();
        }
        else
        {
            UIManager.instance.OpenPromptWindow("雇佣失败", PromptWindow.PromptType.Hint, null, null);
            GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>().ChangeEmploymentButtonInfo();
        }
    }
    public void Process_6113(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            TeamInvitationWindow tw = GameObject.Find("TeamInvitationWindow").GetComponent<TeamInvitationWindow>();
            tw.TeamFightLabel.text = (int.Parse(tw.TeamFightLabel.text) - int.Parse(dataSplit[1])).ToString();
        }
        else
        {
            UIManager.instance.OpenPromptWindow("取消失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_6201(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit.Length < 6)  //4个
        {
            CharacterRecorder.instance.BossLevel = int.Parse(dataSplit[3]);
            if (dataSplit[3] != "0")
            {
                CharacterRecorder.instance.BossBlood = TextTranslator.instance.GetWorldBossByID(int.Parse(dataSplit[3])).Blood;
            }


            if (dataSplit[2] == "0")
            {
                PlayerPrefs.SetInt("WorldBossIsOpen", 1);
                //if (CharacterRecorder.instance.lastGateID > 10069)
                {
                    if (GameObject.Find("PopupWindow") == null)
                    {
                        string doc = string.Format("{0};{1};{2};{3};{4};", 0, 0, 0, 0, "[ffff00]世界BOSS[-]讨伐已经[ffff00]开始[-]了！各位英雄快来取BOSS首级！");
                        TextTranslator.instance.AddQueuedoc(doc);
                        UIManager.instance.OpenPromptWindow("0", PromptWindow.PromptType.Popup, null, null);
                    }
                }
                if (GameObject.Find("WorldBossWindow") != null)
                {
                    GameObject.Find("WorldBossWindow").GetComponent<WorldBossWindow>().SetWindowState(dataSplit[0], dataSplit[1], dataSplit[2], "0");
                }

                if (GameObject.Find("WorldBossFightWindow") != null) //12 boss开始 主动通知
                {
                    SendProcess("6201#;");
                }
            }
            else
            {
                PlayerPrefs.SetInt("WorldBossIsOpen", 0);
                //if (GameObject.Find("WorldBossFightWindow") != null)
                if (GameObject.Find("WorldBossFightWindow") != null)  //boss结束自动离开
                {
                    GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().LiveWorldBossFightWindow();
                }
            }
        }
        else
        {
            if (dataSplit[1] != "0" && long.Parse(dataSplit[0]) - long.Parse(dataSplit[1]) < 1800)//long.Parse(dataSplit[0]) >= (long.Parse(dataSplit[1]) - 600)
            {
                PlayerPrefs.SetInt("WorldBossIsOpen", 1);
            }
            else
            {
                PlayerPrefs.SetInt("WorldBossIsOpen", 0);
            }

            CharacterRecorder.instance.BossLevel = int.Parse(dataSplit[3]);

            if (dataSplit[3] != "0")
            {
                CharacterRecorder.instance.BossBlood = TextTranslator.instance.GetWorldBossByID(int.Parse(dataSplit[3])).Blood;
            }

            CharacterRecorder.instance.ClearBossNum = int.Parse(dataSplit[4]);

            if (GameObject.Find("WorldBossWindow") != null)
            {
                GameObject.Find("WorldBossWindow").GetComponent<WorldBossWindow>().SetWindowState(dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[5]);
            }

            if (dataSplit[2] != "0") //boss死亡或者结束时全服通告，离开战斗界面
            {
                if (GameObject.Find("WorldBossFightWindow") != null)
                {
                    GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().LiveWorldBossFightWindow();
                }
            }
            else if (dataSplit[2] == "0") //boss刚开始时，刷新战斗场景数据
            {
                if (GameObject.Find("WorldBossFightWindow") != null)
                {
                    CharacterRecorder.instance.IsTimeToOpen = true;
                    GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().IsTimeToOpenBoss();
                }
            }
        }
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().WorldBossIsOpen();
        }
    }

    public void Process_6202(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (Application.loadedLevelName != "Downloader")
        {
            if (GameObject.Find("WorldBossFightWindow") != null)
            {
                GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().GetBossblood(float.Parse(dataSplit[0]));
            }
        }
    }

    public void Process_6203(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (GameObject.Find("WorldBossFightWindow") != null)
        {
            GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().GetBossinSpire(int.Parse(dataSplit[0]), float.Parse(dataSplit[1]), int.Parse(dataSplit[2]), float.Parse(dataSplit[3]));
        }
    }

    public void Process_6204(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.gold = int.Parse(dataSplit[5]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[6]);
            if (GameObject.Find("WorldBossFightWindow") != null)
            {
                GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().SetBossinSpire(int.Parse(dataSplit[1]), float.Parse(dataSplit[2]), int.Parse(dataSplit[3]), float.Parse(dataSplit[4]));
            }
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            if (dataSplit[1] == "2")
            {
                UIManager.instance.OpenPromptWindow("金币不足", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "3")
            {
                UIManager.instance.OpenPromptWindow("钻石不足", PromptWindow.PromptType.Hint, null, null);
            }
            else if (dataSplit[1] == "4")
            {
                UIManager.instance.OpenPromptWindow("请提升VIP等级", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }
    public void Process_6205(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (GameObject.Find("WorldBossFightWindow") != null)
        {
            GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().GetBossCD(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]));
        }
    }

    public void Process_6206(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (GameObject.Find("WorldBossFightWindow") != null)
            {
                GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().BossFight(int.Parse(dataSplit[1]), int.Parse(dataSplit[2]));
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("攻击失败", PromptWindow.PromptType.Hint, null, null);
            SendProcess("6205#;");
        }
    }
    public void Process_6207(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("WorldBossFightWindow") != null)
            {
                GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().BossClearbossCD();
            }
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("清CD失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_6208(string recvString)
    {
        if (Application.loadedLevelName != "Downloader")
        {
            if (GameObject.Find("WorldBossFightWindow") != null)
            {
                GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().GetBossBloodAward(recvString);
            }
        }
    }

    public void Process_6209(string recvString)
    {
        if (GameObject.Find("WorldBossWindow/ListWindow") != null)
        {
            GameObject.Find("WorldBossWindow").GetComponent<WorldBossWindow>().LookStandList(recvString);
        }
        else if (GameObject.Find("WorldBossFightWindow") != null)
        {
            GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().GetBossRank(recvString);
        }
    }

    public void Process_6210(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (Application.loadedLevelName != "Downloader")
        {
            if (CharacterRecorder.instance.characterName == dataSplit[0])
            {
                if (GameObject.Find("WorldBossWindow") != null)
                {
                    UIManager.instance.OpenPanel("WorldBossFightWindow", true);
                    GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().JoinBoss(dataSplit[0], dataSplit[1], int.Parse(dataSplit[2]));
                }
            }
            else
            {
                if (GameObject.Find("WorldBossFightWindow") != null)
                {
                    GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().JoinBoss(dataSplit[0], dataSplit[1], int.Parse(dataSplit[2]));
                }
            }
        }

    }
    public void Process_6211(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (Application.loadedLevelName != "Downloader")
        {
            if (CharacterRecorder.instance.characterName != dataSplit[0])
            {
                if (GameObject.Find("WorldBossFightWindow") != null)
                {
                    GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().LeaveBoss(dataSplit[0], dataSplit[1]);
                }
            }
        }
        //if (GameObject.Find("WorldBossFightWindow") != null)
        //{
        //    GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().LeaveBoss(dataSplit[0], dataSplit[1]);
        //}
        //if (dataSplit[0] == "0")
        //{
        //    if (GameObject.Find("WorldBossFightWindow") != null)
        //    {
        //        GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().LiveWorldBossFightWindow();
        //    }
        //}
    }

    public void Process_6212(string recvString)
    {
        //string[] dataSplit = recvString.Split(';');
        if (Application.loadedLevelName != "Downloader")
        {
            if (GameObject.Find("WorldBossFightWindow") != null)
            {
                GameObject.Find("WorldBossFightWindow").GetComponent<WorldBossFightWindow>().JoinBossPlayer(recvString);
            }
        }
    }

    public void Process_9201(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        CharacterRecorder.instance.Vip = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.PayDiamond = int.Parse(dataSplit[1]);
        CharacterRecorder.instance.BuyGiftBag = dataSplit[2].Split('$');
        CharacterRecorder.instance.MonthCardDay = int.Parse(dataSplit[5]);

        if (dataSplit[4] != null && dataSplit[4] != "")
        {
            string[] trcSplit = dataSplit[4].Split('$');
            for (int i = 0; i < trcSplit.Length - 1; i++)
            {
                Exchange mItem = TextTranslator.instance.GetExchangeById(int.Parse(trcSplit[i]));
                mItem.SetisfristDiamond(true);
            }

            //Debug.LogError("VIPShopWindow1  ");
            //VipShopWindow VS = GameObject.Find("VIPShopWindow").GetComponent<VipShopWindow>();
            //if (VS != null) 
            //{
            //    VS.SetIsFristDiamond();
            //}
            //Debug.LogError("VIPShopWindow2  ");
        }


        if (GameObject.Find("FirstRechargeWindow") != null)
        {
            GameObject.Find("FirstRechargeWindow").GetComponent<FirstRechargeWindow>().SetVipOneState();
        }

        //if (GameObject.Find("MainWindow") != null)
        //{
        //    GameObject.Find("MainWindow").GetComponent<MainWindow>().FirstVipRedpoint();
        //}
        CharacterRecorder.instance.FirstVipRedpoint();
        if (CharacterRecorder.instance.BuyGiftBag[0] == "1")
        {
            PlayerPrefs.SetInt("FirstVipRecharge", 1);
            if (GameObject.Find("MainWindow") != null)
            {
                GameObject.Find("MainWindow").GetComponent<MainWindow>().FirstRechargeWindowIsOpen();
            }
        }
        CharacterRecorder.instance.VipExp = int.Parse(dataSplit[3]);
    }

    public void Process_9202(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        CharacterRecorder.instance.Vip = int.Parse(dataSplit[0]);
        CharacterRecorder.instance.VipExp = int.Parse(dataSplit[1]);
    }
    public void Process_9301(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            if (dataSplit[1] == "1")
            {
                CharacterRecorder.instance.level = int.Parse(dataSplit[2]);
                if (GameObject.Find("LabelLevel") != null)
                {
                    GameObject.Find("LabelLevel").GetComponent<UILabel>().text = string.Format("Lv.{0}", CharacterRecorder.instance.level.ToString());
                }

                if (dataSplit[2] == "12" || dataSplit[2] == "14" || dataSplit[2] == "16" || dataSplit[2] == "18" || dataSplit[2] == "25" || dataSplit[2] == "27" || dataSplit[2] == "33")
                {
                    StartCoroutine(SceneTransformer.instance.NewbieGuide());// 新手引导;
                }
            }
            else if (dataSplit[1] == "2")
            {
                CharacterRecorder.instance.lastGateID = int.Parse(dataSplit[3]);
                CharacterRecorder.instance.mapID = int.Parse(dataSplit[2]);
                if (CharacterRecorder.instance.lastGateID == 10010)
                {
                    StartCoroutine(SceneTransformer.instance.NewbieGuide());// 新手引导;
                }
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("GM指令失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_9511(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject pe = GameObject.Find("PackageExchangeWindow");
            if (pe != null)
            {
                pe.GetComponent<PackageExchangeWindow>().ReceiverMsg_9511(int.Parse(dataSplit[1]),//当前钻石
                                                            int.Parse(dataSplit[2]),//当前金币
                                                            int.Parse(dataSplit[3]),//当前VIP等级
                                                            int.Parse(dataSplit[4]),//当前VIP经验
                                                            dataSplit[5]);//奖励
            }
            //else
            //{
            //    UIManager.instance.OpenPromptWindow("无此面板.", PromptWindow.PromptType.Hint, null, null);
            //}
        }
        else if (dataSplit[0] == "0")
        {
            switch (dataSplit[1])
            {
                case "1":
                    UIManager.instance.OpenPromptWindow("无此Cdkey.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("此Cdkey已无法使用.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("此CKKEY 已过期.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "4":
                    UIManager.instance.OpenPromptWindow("此平台不能使用.", PromptWindow.PromptType.Hint, null, null);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 获取砸金蛋的信息
    /// </summary>
    /// <param name="recvString"></param>
    public void Process_9701(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (int.Parse(dataSplit[2]) > 0)
        {
            CharacterRecorder.instance.IsGoldOpen = true;
            if (int.Parse(dataSplit[4]) > 0)//int.Parse(dataSplit[7]) > 0 && 
            {
                CharacterRecorder.instance.IsGoldEggRedPoint = true;
            }
            else
            {
                CharacterRecorder.instance.IsGoldEggRedPoint = false;
            }
        }
        else
        {
            CharacterRecorder.instance.IsGoldOpen = false;
        }

        GameObject goldEgg = GameObject.Find("GoldenEggActivity");
        if (goldEgg != null)
        {
            string[] data = Utils.GetTime(dataSplit[0]).ToShortDateString().Split('/');
            string[] data1 = Utils.GetTime(dataSplit[1]).ToShortDateString().Split('/');
            string start = data[2] + "年" + data[0] + "月" + data[1] + "日";
            string end = data1[2] + "年" + data1[0] + "月" + data1[1] + "日";
            string time = string.Format("{0}-{1}", start, end);
            goldEgg.GetComponent<GoldenEggActivityWindow>().InitSetWindowInfo(time, int.Parse(dataSplit[2]), int.Parse(dataSplit[4]), int.Parse(dataSplit[3]), int.Parse(dataSplit[5]), dataSplit[6], int.Parse(dataSplit[7]));
        }
        GameObject main = GameObject.Find("MainWindow");
        if (main != null)
        {
            main.GetComponent<MainWindow>().OpenGoldEgg(CharacterRecorder.instance.IsGoldOpen, CharacterRecorder.instance.IsGoldEggRedPoint);
        }
    }
    /// <summary>
    /// 砸金蛋成功
    /// </summary>
    /// <param name="recvString"></param>
    public void Process_9702(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (int.Parse(dataSplit[0]) == 1)
        {
            GameObject goldEgg = GameObject.Find("GoldenEggActivity");
            if (goldEgg != null)
            {
                string[] dataSplit2 = dataSplit[6].Split('!');
                string[] dataSplit1 = dataSplit2[0].Split('$');
                // Debug.LogError("dataSplit1:" + dataSplit1.Length);
                goldEgg.GetComponent<GoldenEggActivityWindow>().GoldEggReceiveMsg_9702(int.Parse(dataSplit1[0]), int.Parse(dataSplit1[1]), int.Parse(dataSplit[5]));
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0":
                    UIManager.instance.OpenPromptWindow("没有该金蛋.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "1":
                    UIManager.instance.OpenPromptWindow("该金蛋已砸过.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("今日砸金蛋次数达到上限.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("锤子数不足.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "4":
                    UIManager.instance.OpenPromptWindow("钻石不足.", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "5":
                    UIManager.instance.OpenPromptWindow("活动未开放.", PromptWindow.PromptType.Hint, null, null);
                    break;
                default:
                    UIManager.instance.OpenPromptWindow("获取奖励失败.", PromptWindow.PromptType.Hint, null, null);
                    break;
            }
            SendProcess("9701#;");
        }
    }
    /// <summary>
    /// 抽神将活动
    /// </summary>
    /// <param name="recvString"></param>
    public void Process_9711(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        GameObject redhero = GameObject.Find("ActivityGachaRedHeroWindow");
        if (redhero != null)
        {
            if (redhero != null)
            {
                redhero.GetComponent<ActivityGachaRedHeroWindow>().RankIntegraInfo(dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], int.Parse(dataSplit[4]));
            }
        }
        if (int.Parse(dataSplit[3]) <= 0)
        {
            PlayerPrefs.SetInt("RedHeroIsOpen", 0);
        }
        else
        {
            PlayerPrefs.SetInt("RedHeroIsOpen", 1);
        }
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().RedHeroIsOpen();
        }
    }
    public void Process_9712(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        for (int i = 0; i < dataSplit.Length; i++)
        {
            if (dataSplit[i] == "1")
            {
                CharacterRecorder.instance.SetRedPoint(36, true);
                break;
            }
            else
            {
                CharacterRecorder.instance.SetRedPoint(36, false);
            }
        }
        GameObject redhero = GameObject.Find("ActivityGachaRedHeroWindow");
        if (redhero != null)
        {
            redhero.GetComponent<ActivityGachaRedHeroWindow>().SetGachaInfo(dataSplit);
        }
    }
    public void Process_9713(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            List<Item> itemlist = new List<Item>();
            for (int i = 0; i < dataSplit[1].Split('!').Length - 1; i++)
            {
                itemlist.Add(new Item(int.Parse((dataSplit[1].Split('!'))[i].Split('$')[0]), int.Parse((dataSplit[1].Split('!'))[i].Split('$')[1])));
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            UpDateTopContentData(itemlist);
            SendProcess("9712#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_9721(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        GameObject wish = GameObject.Find("MyWishWindow");
        if (wish != null)
        {
            wish.GetComponent<MyWishWindow>().ReceiveMsg_9721(dataSplit);
        }
        GameObject main = GameObject.Find("MainWindow");
        if (main != null)
        {
            if (int.Parse(dataSplit[2]) > 0)
            {
                CharacterRecorder.instance.IsWishOpen = true;
                if (int.Parse(dataSplit[5]) > 0 || int.Parse(dataSplit[7]) > 0)
                {
                    CharacterRecorder.instance.IsWishRedpoint = true;
                }
                else
                {
                    CharacterRecorder.instance.IsWishRedpoint = false;
                }
            }
            else
            {
                CharacterRecorder.instance.IsWishOpen = false;
            }
            main.GetComponent<MainWindow>().OpenWishActivity(CharacterRecorder.instance.IsWishOpen, CharacterRecorder.instance.IsWishRedpoint);
        }
    }
    public void Process_9722(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject wish = GameObject.Find("MyWishWindow");
            if (wish != null)
            {
                wish.GetComponent<MyWishWindow>().ReceiveMsg_9722(dataSplit);
            }
        }
        else
        {
            switch (dataSplit[1])
            {
                case "1":
                    UIManager.instance.OpenPromptWindow("许愿次数不足", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "2":
                    UIManager.instance.OpenPromptWindow("该物品已许愿", PromptWindow.PromptType.Hint, null, null);
                    break;
                case "3":
                    UIManager.instance.OpenPromptWindow("请先领取昨日奖励", PromptWindow.PromptType.Hint, null, null);
                    break;
                default:
                    UIManager.instance.OpenPromptWindow("许愿活动明日结束，停止许愿哩", PromptWindow.PromptType.Hint, null, null);
                    break;
            }

        }

    }
    public void Process_9723(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject wish = GameObject.Find("MyWishWindow");
            if (wish != null)
            {
                wish.GetComponent<MyWishWindow>().ReceiveMsg_9723(dataSplit);
            }
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取心愿失败.", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_9401(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "")
        {
            SendProcess("9402#0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$0$;");
            ///Sumarry
            ///sjc(索引)0-14 0-第六关第一段  1-第六关第二段  2-第7-1 3-第7-2 4-第11 5-第18
            ///yangyong(索引）23-12级竞技场引导
            ///Sumarry
        }
        else
        {
            SceneTransformer.instance.AddGuideList(dataSplit[0]);
            //////////////////////紧急防卡死机制(以下)//////////////////////        
            for (int i = 0; i < CharacterRecorder.instance.GuideID.Count; i++)
            {
                if (CharacterRecorder.instance.GuideID[i] > 0)
                {
                    CharacterRecorder.instance.GuideID[i] = 99;
                }
            }
            //////////////////////紧急防卡死机制(以上)//////////////////////
        }
    }
    public void Process_9402(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            SceneTransformer.instance.AddGuideList(dataSplit[1]);
        }
        else
        {
            //Debug.LogError("新手引导失败");
        }
    }
    public void Process_9411(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        CharacterRecorder.instance.HostageName = dataSplit[1]; //俘虏姓名
        CharacterRecorder.instance.HostageRoleID = int.Parse(dataSplit[2]); //俘虏头像
        if (GameObject.Find("MainWindow") != null)
        {
            GameObject.Find("MainWindow").GetComponent<MainWindow>().AllRedPointInfo(dataSplit[0]);
            GameObject.Find("MainWindow").GetComponent<MainWindow>().SetTeamInfo();
        }
    }
    public void Process_9501(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //UIManager.instance.OpenPromptWindow("充值成功", PromptWindow.PromptType.Hint, null, null);
            int lunaGemNum = int.Parse(dataSplit[1]) - CharacterRecorder.instance.lunaGem;
            int vip = int.Parse(dataSplit[2]) - CharacterRecorder.instance.Vip;
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.Vip = int.Parse(dataSplit[2]);
            CharacterRecorder.instance.PayDiamond = int.Parse(dataSplit[3]);
            CharacterRecorder.instance.BuyGiftBag = dataSplit[4].Split('$');
            CharacterRecorder.instance.VipExp = int.Parse(dataSplit[5]);
            CharacterRecorder.instance.MonthCardDay = int.Parse(dataSplit[7]);

            //砸金蛋开启
            if (CharacterRecorder.instance.IsGoldOpen)
            {
                SendProcess("9701#;");
            }

            if (dataSplit[6] != null && dataSplit[6] != "")
            {
                string[] trcSplit = dataSplit[6].Split('$');
                for (int i = 0; i < trcSplit.Length - 1; i++)
                {
                    Exchange mItem = TextTranslator.instance.GetExchangeById(int.Parse(trcSplit[i]));
                    mItem.SetisfristDiamond(true);
                }
            }
            if (GameObject.Find("MainWindow") != null)
            {
                GameObject.Find("MainWindow").GetComponent<MainWindow>().Reset();
            }

            if (GameObject.Find("VIPShopWindow") != null)
            {
                VipShopWindow VS = GameObject.Find("VIPShopWindow").GetComponent<VipShopWindow>();
                VS.SetTopMation();
                VS.SetIsFristDiamond();//充值刷新
            }
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            UIManager.instance.OpenSinglePanel("BuySuccessWindow", false);
            GameObject bs = GameObject.Find("BuySuccessWindow");
            if (bs != null)
            {
                bs.layer = 11;
                BuySuccessWindow aw = bs.GetComponent<BuySuccessWindow>();
                aw.ReceiverMsg_9501(lunaGemNum, vip);
            }
            CharacterRecorder.instance.ActivityRedPointSendPress();
            //V2基金红点
            if (CharacterRecorder.instance.Vip >= 2 && CharacterRecorder.instance.isBuyTheFoundation == false)
            {
                CharacterRecorder.instance.SetRedPoint(38, true);
            }

            SendProcess("5012#0;"); //刷新商城红
                                    //if (dataSplit.Length > 8)
                                    //{
                                    //    SendProcess(string.Format("7002#{0};{1};{2};{3};", 41, CharacterRecorder.instance.characterName, dataSplit[8], dataSplit[9]));//抢红包
                                    //}
        }
        else
        {
            UIManager.instance.OpenPromptWindow("充值失败", PromptWindow.PromptType.Hint, null, null);
        }
    }

    public void Process_9521(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            //int beforelunaGem = CharacterRecorder.instance.lunaGem;
            //int cutlunaGem=int.Parse(dataSplit[1])-beforelunaGem;
            List<Item> itemlist = new List<Item>();
            itemlist.Add(new Item(90002, int.Parse(dataSplit[2])));
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().IsWoodsWindow = true;//yy  11.21
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
        }
        else
        {
            if (dataSplit[1] == "0")
            {
                UIManager.instance.OpenPromptWindow("亲，慢了一步，红包被人抢光啦", PromptWindow.PromptType.Hint, null, null);
            }
            if (dataSplit[1] == "1")
            {
                UIManager.instance.OpenPromptWindow("亲，此红包您已抢过了哦", PromptWindow.PromptType.Hint, null, null);
            }
        }
    }

    public void ResourceTycoonOpenByProcess_9601(string recvString)//yy加，资源大亨判断是否开启
    {
        Debug.Log("进入资源大亨开启条件判断");
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] != "")
        {
            bool ishave = false;
            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                if (dataSplit[i] == "150001")
                {
                    ishave = true;
                    break;
                }
            }
            if (ishave)
            {
                PlayerPrefs.SetInt("IsOpenResourcetycoon", 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsOpenResourcetycoon", 0);
            }
            GameObject Mw = GameObject.Find("MainWindow");
            if (Mw != null)
            {
                Mw.GetComponent<MainWindow>().OpenResourcetycoon();
            }
        }

    }


    public void Process_9607(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            SendProcess("9608#;");
            CharacterRecorder.instance.isBuyTheFoundation = true;
            UIManager.instance.OpenPromptWindow("购买成功", PromptWindow.PromptType.Hint, null, null);
        }
        else
        {
            UIManager.instance.OpenPromptWindow("购买失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void Process_9608(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        List<int> ItemType = new List<int>();
        for (int i = 0; i < dataSplit[2].Split('$').Length - 1; i++)
        {
            ItemType.Add(int.Parse(dataSplit[2].Split('$')[i]));
        }
        int cout = 0;
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.isBuyTheFoundation = true;
        }
        for (int i = 0; i < ItemType.Count; i++)
        {
            if (ItemType[i] == 0 && TextTranslator.instance.GetActivityGrowthFundDicByID(i + 1).Condition <= CharacterRecorder.instance.level && dataSplit[0] == "1")
            {
                if (GameObject.Find("EventWindow") != null)
                {
                    GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftFoundationButton").transform.Find("RedMessage").gameObject.SetActive(true);
                }
                //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(8, true);
                CharacterRecorder.instance.SetRedPoint(35, true);
                CharacterRecorder.instance.isFoundationPoint = true;
                break;
            }
            else
            {
                CharacterRecorder.instance.isFoundationPoint = false;
                if (CharacterRecorder.instance.isBenifPoint == false)
                {
                    if (GameObject.Find("EventWindow") != null)
                    {
                        GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftFoundationButton").transform.Find("RedMessage").gameObject.SetActive(false);
                    }
                    //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(8, false);
                    CharacterRecorder.instance.SetRedPoint(35, false);
                }

            }
        }
        if (CharacterRecorder.instance.Vip >= 2 && CharacterRecorder.instance.isBuyTheFoundation == false)
        {
            if (GameObject.Find("EventWindow") != null)
            {
                GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftFoundationButton").transform.Find("RedMessage").gameObject.SetActive(true);
            }
        }
        else
        {
            CharacterRecorder.instance.SetRedPoint(38, false);
        }
        for (int i = 0; i < ItemType.Count; i++)
        {
            if (ItemType[i] == 1)
            {
                cout++;
            }
        }
        if (cout == ItemType.Count)
        {
            CharacterRecorder.instance.isFoundationRedPoint = true;
        }
        if (GameObject.Find("ActivityFoundationWindow") != null)
        {
            GameObject.Find("ActivityFoundationWindow").GetComponent<ActivityFoundationWindow>().ItemInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), ItemType, 1);
        }


    }
    public void Process_9609(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            if (dataSplit[2] != null)
            {
                List<Item> itemlist = new List<Item>();
                itemlist.Add(new Item(int.Parse((dataSplit[2].Split('!'))[0].Split('$')[0]), int.Parse((dataSplit[2].Split('!'))[0].Split('$')[1])));
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            }
            SendProcess("9608#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败", PromptWindow.PromptType.Hint, null, null);
        }

    }
    public void Process_9610(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        List<int> ItemType = new List<int>();
        for (int i = 0; i < dataSplit[2].Split('$').Length - 1; i++)
        {
            ItemType.Add(int.Parse(dataSplit[2].Split('$')[i]));
        }
        int cout = 0;
        for (int i = 0; i < ItemType.Count; i++)
        {
            if (ItemType[i] == 0 && TextTranslator.instance.GetActivityGrowthFundDicByID(TextTranslator.instance.GetActivityGrowthFundDicLengthByID(1) + i + 1).Condition <= int.Parse(dataSplit[1]))
            {
                CharacterRecorder.instance.isBenifPoint = true;
                if (GameObject.Find("EventWindow") != null)
                {
                    GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftFoundationButton").transform.Find("RedMessage").gameObject.SetActive(true);
                }
                //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(8, true);
                CharacterRecorder.instance.SetRedPoint(37, true);
                break;
            }
            else
            {
                CharacterRecorder.instance.isBenifPoint = false;
                if (CharacterRecorder.instance.isFoundationPoint == false)
                {
                    if (GameObject.Find("EventWindow") != null)
                    {
                        GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftFoundationButton").transform.Find("RedMessage").gameObject.SetActive(false);
                    }
                    //GameObject.Find("MainWindow").GetComponent<MainWindow>().ShowPrompt(8, false);
                    CharacterRecorder.instance.SetRedPoint(37, false);
                }
            }
        }
        if (CharacterRecorder.instance.Vip >= 2 && CharacterRecorder.instance.isBuyTheFoundation == false)
        {
            if (GameObject.Find("EventWindow") != null)
            {
                GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftFoundationButton").transform.Find("RedMessage").gameObject.SetActive(true);
            }
        }
        else
        {
            CharacterRecorder.instance.SetRedPoint(38, false);
        }
        for (int i = 0; i < ItemType.Count; i++)
        {
            if (ItemType[i] == 1)
            {
                cout++;
            }
        }
        if (cout == ItemType.Count)
        {
            CharacterRecorder.instance.isBenifRedPoint = true;
        }
        if (GameObject.Find("ActivityFoundationWindow") != null)
        {
            GameObject.Find("ActivityFoundationWindow").GetComponent<ActivityFoundationWindow>().ItemInfo(int.Parse(dataSplit[0]), int.Parse(dataSplit[1]), ItemType, 2);
        }

    }
    public void Process_9611(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[1]);
            if (GameObject.Find("TopContent") != null)
            {
                GameObject.Find("TopContent").GetComponent<TopContent>().Reset();
            }
            if (dataSplit[2] != null)
            {
                List<Item> itemlist = new List<Item>();
                itemlist.Add(new Item(int.Parse((dataSplit[2].Split('!'))[0].Split('$')[0]), int.Parse((dataSplit[2].Split('!'))[0].Split('$')[1])));
                UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
                GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            }
            SendProcess("9610#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败", PromptWindow.PromptType.Hint, null, null);
        }

    }
    public void Process_9612(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        List<int> ItemType = new List<int>();
        for (int i = 0; i < dataSplit[1].Split('$').Length - 1; i++)
        {
            ItemType.Add(int.Parse(dataSplit[1].Split('$')[i]));
        }
        for (int i = 0; i < ItemType.Count; i++)
        {
            if (ItemType[i] == 1)
            {
                CharacterRecorder.instance.SetRedPoint(33, true);
                if (GameObject.Find("EventWindow") != null)
                {
                    GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftGetVipButton").transform.Find("RedMessage").gameObject.SetActive(true);
                }
                break;
            }
            else
            {
                CharacterRecorder.instance.SetRedPoint(33, false);
                if (GameObject.Find("EventWindow") != null)
                {
                    GameObject.Find("EventWindow/left/Scroll View/EventButtonGrid/LeftGetVipButton").transform.Find("RedMessage").gameObject.SetActive(false);
                }
            }
        }
        if (GameObject.Find("ActivityGetVipWindow") != null)
        {
            GameObject.Find("ActivityGetVipWindow").GetComponent<ActivityGetVipWindow>().SetInfo(int.Parse(dataSplit[0]), ItemType);
        }
    }
    public void Process_9613(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);

            List<Item> itemlist = new List<Item>();
            for (int i = 0; i < dataSplit[3].Split('!').Length - 1; i++)
            {
                itemlist.Add(new Item(int.Parse((dataSplit[3].Split('!'))[i].Split('$')[0]), int.Parse((dataSplit[3].Split('!'))[i].Split('$')[1])));
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
            SendProcess("9612#;");
        }
        else
        {
            UIManager.instance.OpenPromptWindow("领取失败", PromptWindow.PromptType.Hint, null, null);
        }

    }

    public void Process_9731(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        GameObject RT = GameObject.Find("ResourcetycoonWindow");
        if (RT != null)
        {
            RT.GetComponent<ResourcetycoonWindow>().GetMethodBy_9731(dataSplit);
        }
    }

    public void Process_9732(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            GameObject RT = GameObject.Find("ResourcetycoonWindow");
            if (RT != null)
            {
                RT.GetComponent<ResourcetycoonWindow>().GetMethodBy_9732(int.Parse(dataSplit[3]));
            }
            CharacterRecorder.instance.gold = int.Parse(dataSplit[1]);//现有金币
            CharacterRecorder.instance.lunaGem = int.Parse(dataSplit[2]);//现有金币
            TopContent topC = GameObject.Find("TopContent").GetComponent<TopContent>();

            List<Item> itemlist = new List<Item>();
            string[] secSplit = dataSplit[4].Split('!');
            for (int i = 0; i < secSplit.Length - 1; i++)
            {
                string[] ticSplit = secSplit[i].Split('$');
                Item _item = new Item(int.Parse(ticSplit[0]), int.Parse(ticSplit[1]));
                itemlist.Add(_item);
            }
            UIManager.instance.OpenSinglePanel("AdvanceWindow", false);
            GameObject.Find("AdvanceWindow").GetComponent<AdvanceWindow>().SetInfo(AdvanceWindowType.GainResult, null, null, null, null, itemlist);
        }
        else
        {
            switch (dataSplit[1])
            {
                case "0": UIManager.instance.OpenPromptWindow("成就ID错误", PromptWindow.PromptType.Hint, null, null); break;
                case "1": UIManager.instance.OpenPromptWindow("不可领取", PromptWindow.PromptType.Hint, null, null); break;
                case "2": UIManager.instance.OpenPromptWindow("已过时间", PromptWindow.PromptType.Hint, null, null); break;
            }
        }
    }

    public void Process_9733(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        GameObject RT = GameObject.Find("ResourcetycoonWindow");
        if (RT != null && dataSplit[0] != "")
        {
            RT.GetComponent<ResourcetycoonWindow>().GetMethodBy_9733(dataSplit[0], dataSplit[1]);
        }
    }

    public void Process_9734(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        GameObject RT = GameObject.Find("ResourcetycoonWindow");
        if (RT != null && dataSplit[0] != "")
        {
            RT.GetComponent<ResourcetycoonWindow>().GetMethodBy_9734(dataSplit[0], dataSplit[1]);
        }
    }

    public void Process_9998(string recvString)
    {
        string[] dataSplit = recvString.Split(';');
        if (dataSplit[0] == "1")
        {
            IsKickOff = true;
            UIManager.instance.OpenPromptWindow("您的账号在其它设备登陆，确定退出!", PromptWindow.PromptType.Alert, OnQuitGame, null);
        }
        else if (dataSplit[0] == "2")
        {
            IsKickOff = true;
            UIManager.instance.OpenPromptWindow("您的帐号已被冻结，如有问题请咨询客服!", PromptWindow.PromptType.Alert, OnQuitGame, null);
        }
        else
        {
            UIManager.instance.OpenPromptWindow("充值失败", PromptWindow.PromptType.Hint, null, null);
        }
    }
    public void OnQuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        if (Application.loadedLevelName != "Downloader")
        {
            SceneTransformer.instance.SendGuide();
            SceneTransformer.instance.SendBoxGuide();
            NetworkHandler.instance.SendProcess("6105#" + CharacterRecorder.instance.TeamID + ";" + CharacterRecorder.instance.TeamPosition + ";");
        }
        GameSocketDisconnect();
    }

    void OnReconnect()
    {
        IsReconnect = true;
        ConnectFlag = false;
        GameSocketConnect();
        ReconnectCount++;
    }
}
