using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class EffectMaker : MonoBehaviour
{
    public static EffectMaker instance;
    public List<Effect2D> ListEffect2D = new List<Effect2D>();
    public int Effect2DCount = 0;
    Vector3 newEffect2DForwardPosition;
    List<Effect2D> ListCrear = new List<Effect2D>();
    float MoveDistance;

    Font EffectFont;

    public void Start()
    {
        instance = this;
        EffectFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    }

    public class Effect2D
    {
        public GameObject Effect2DObject;
        public Vector3 Effect2DNewPosition;
        public Vector3 Effect2DStartPosition;
        public float Effect2DMoveSpeed;
        public float Effect2DDelayTimer;  //延播计时器
        public float Effect2DDelayTime;   //延播时间
        public float Effect2DContinueTimer;  //续播计时器
        public float Effect2DContinueTime;   //续播时间
        public Vector3 Effect2DScale;
        public Vector3 Effect2DRotation;
        public Vector3 Effect2DMoveDistance;
        public Vector3 Effect2DHalfPosition;
        public float EffectDistance;


        public string EffectHurtType;
        public string EffectName = "";
        public GameObject EffectParentObject;

        public List<Buff> Effect2DTargetBuff1;
        public Buff Effect2DTargetBuff2;
        public FightProjectile EffectFightProjectile;

        public int Effect2DStartIndex;
        public List<int> Effect2DTargetIndex;
        public List<int> Effect2DTargetType;
        public List<int> Effect2DTargetDamige;
        public int Effect2DWidthCount;
        public int Effect2DHeightCount;
        public int EffectNowFrame;
        public bool IsRock;
        public bool IsWith;
        public bool IsUp;
        public bool IsDown;
        public bool IsEnd;
        public bool IsCollision;
        public bool IsFadeOut;
        public bool IsSkill;
        public bool IsTarget;
        public bool IsMonster;
        public bool IsDelay;
    }



    public void Create2DEffect(string Name, string PictureName, GameObject ParentObject, Vector3 StartPosition, Vector3 EndPosition, Vector3 StartScale, float MoveSpeed, float DelayTime, float ContinueTime, int WidthCount, int HeightCount, int StartIndex, List<int> TargetIndex, List<int> TargetType, List<int> TargetDamige, List<Buff> TargetBuff1, Buff TargetBuff2, bool IsRock, bool IsCollision, bool IsWith, bool IsUp, bool IsFadeOut, bool IsBig, bool IsCurve, bool IsSkill, bool IsMonster, string HurtType, FightProjectile fp)
    {

        Effect2D NewEffect2D = new Effect2D();
        if (PictureName == "")
        {
            if (Name.IndexOf("~") == 0)
            {
                Name = Name.Replace("~", "");
                NewEffect2D.EffectName = Name;

                //if (Name == "0")
                //{
                //    return;
                //}

                NewEffect2D.Effect2DObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                DestroyImmediate(NewEffect2D.Effect2DObject.GetComponent("MeshCollider"));
                NewEffect2D.Effect2DObject.renderer.castShadows = false;
                NewEffect2D.Effect2DObject.renderer.receiveShadows = false;
                NewEffect2D.Effect2DObject.name = Name;
                NewEffect2D.Effect2DObject.renderer.material.mainTexture = Resources.Load("Effect/blank", typeof(Texture)) as Texture;
                NewEffect2D.Effect2DObject.renderer.material.shader = Shader.Find("Unlit/Transparent Colored");

                NewEffect2D.Effect2DStartPosition = StartPosition;
                NewEffect2D.Effect2DNewPosition = EndPosition;
                NewEffect2D.Effect2DScale = new Vector3(1f, 1f, 1f);
                NewEffect2D.Effect2DRotation = StartScale;
            }
            else if (Name.IndexOf("+") == -1 && Name.IndexOf("-") == -1)
            {
                //NewEffect2D.Effect2DObject = new GameObject(Name);
                //TextMesh PictureTextMesh = (TextMesh)NewEffect2D.Effect2DObject.AddComponent("TextMesh");
                //MeshRenderer PictureMeshRenderer = (MeshRenderer)NewEffect2D.Effect2DObject.AddComponent("MeshRenderer");
                //PictureTextMesh.text = Name;
                //PictureTextMesh.anchor = TextAnchor.MiddleCenter;
                //PictureTextMesh.font = transform.parent.GetComponent<GameCenter>().gameResourceLoader.GetComponent<ResourceLoader>().GameFont;
                //PictureTextMesh.fontSize = 30;
                //PictureMeshRenderer.material = transform.parent.GetComponent<GameCenter>().gameResourceLoader.GetComponent<ResourceLoader>().GameFont.material;
                //PictureMeshRenderer.material.shader = Shader.Find("GUI/3D Text Shader");
                //if (IsBig)
                //{
                //    PictureMeshRenderer.material.SetColor("_Color", Color.cyan);
                //    NewEffect2D.Effect2DObject.transform.localScale = new Vector3(StartScale.x, StartScale.z, 1); // 可调
                //}
                //else
                //{
                //    PictureMeshRenderer.material.SetColor("_Color", Color.black);
                //    NewEffect2D.Effect2DObject.transform.localScale = new Vector3(StartScale.x, StartScale.z, 1); // 可调
                //}
            }
            else
            {
                //NewEffect2D.Effect2DObject = new GameObject(Name);
                //NewEffect2D.Effect2DObject.AddComponent<UILabel>();
                //NewEffect2D.Effect2DObject.GetComponent<UILabel>().text = "";

                //NewEffect2D.Effect2DObject.GetComponent<UILabel>().SetDimensions(1600, 800);
                //NewEffect2D.Effect2DObject.GetComponent<UILabel>().effectStyle = UILabel.Effect.Outline;
                //NewEffect2D.Effect2DObject.GetComponent<UILabel>().effectDistance = new Vector2(10, 10);

                //if (Name.IndexOf("+") > -1)
                //{
                //    Debug.Log(Name);
                //    NewEffect2D.Effect2DObject.GetComponent<UILabel>().color = Color.green;
                //    NewEffect2D.Effect2DObject.GetComponent<UILabel>().fontSize = 400;
                //}
                //else if (IsBig)
                //{
                //    NewEffect2D.Effect2DObject.GetComponent<UILabel>().color = Color.red;
                //    NewEffect2D.Effect2DObject.GetComponent<UILabel>().fontSize = 400;
                //}
                //else
                //{
                //    NewEffect2D.Effect2DObject.GetComponent<UILabel>().color = Color.yellow;
                //    NewEffect2D.Effect2DObject.GetComponent<UILabel>().fontSize = 300;
                //}
                //NewEffect2D.Effect2DObject.GetComponent<UILabel>().trueTypeFont = EffectFont;
                //NewEffect2D.Effect2DObject.transform.position = StartPosition;
            }





            //foreach (Component c in NewEffect2D.Effect2DObject.GetComponentsInChildren(typeof(Component), true))
            //{
            //    if (c != null)
            //    {
            //        if (c.GetComponent<ParticleSystem>() != null)
            //        {
            //            Debug.Log(c.transform.name);
            //            if (c.gameObject.GetComponent<ParticleScaler>() == null)
            //            {
            //                c.gameObject.AddComponent<ParticleScaler>();
            //                c.gameObject.GetComponent<ParticleScaler>().particleScale = 2;
            //            }
            //        }
            //    }
            //}
        }
        else
        {
            NewEffect2D.Effect2DObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            DestroyImmediate(NewEffect2D.Effect2DObject.GetComponent("MeshCollider"));
            NewEffect2D.Effect2DObject.renderer.castShadows = false;
            NewEffect2D.Effect2DObject.renderer.receiveShadows = false;
            NewEffect2D.Effect2DObject.name = Name;
            //NewEffect2D.Effect2DObject.transform.localScale = new Vector3(StartScale.x, StartScale.z, 1);
            //if (IsSkill)
            //{
                NewEffect2D.Effect2DObject.renderer.material.mainTextureScale = new Vector2(1f / WidthCount, 1f / HeightCount);
                NewEffect2D.Effect2DObject.renderer.material.mainTextureOffset = new Vector2(0, 0);
            //}
            //else
            //{
            //    NewEffect2D.Effect2DObject.renderer.material.mainTextureScale = new Vector2(-1f / WidthCount, 1f / HeightCount);
            //    NewEffect2D.Effect2DObject.renderer.material.mainTextureOffset = new Vector2(1, 0);
            //}
            NewEffect2D.Effect2DObject.renderer.material.mainTexture = Resources.Load("Effect/" + PictureName, typeof(Texture)) as Texture;
            NewEffect2D.Effect2DObject.renderer.material.shader = Shader.Find("Unlit/Transparent Colored");

            if (IsRock) //攻击特效改方向
            {
                NewEffect2D.Effect2DObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                if (StartPosition.x > EndPosition.x)
                {
                    float Offset = Mathf.Atan((StartPosition.z - EndPosition.z) / (StartPosition.x - EndPosition.x)) * 180 / Mathf.PI + 180;
                    NewEffect2D.Effect2DObject.transform.Rotate(0f, 0f, Offset);
                }
                else if (StartPosition.x < EndPosition.x)
                {
                    float Offset = Mathf.Atan((StartPosition.z - EndPosition.z) / (StartPosition.x - EndPosition.x)) * 180 / Mathf.PI;
                    NewEffect2D.Effect2DObject.transform.Rotate(0f, 0f, Offset);
                }
                NewEffect2D.Effect2DObject.transform.Rotate(30, 0, 0);
            }

            NewEffect2D.Effect2DObject.transform.position = new Vector3(-100, -100, -100);

            if (IsUp)
            {
                StartPosition += new Vector3(0, 2, -2);
                EndPosition += new Vector3(0, 2, -2);
            }
            NewEffect2D.Effect2DStartPosition = StartPosition;
            NewEffect2D.Effect2DNewPosition = EndPosition;
            NewEffect2D.Effect2DScale = StartScale;
        }


        NewEffect2D.Effect2DMoveSpeed = MoveSpeed;
        NewEffect2D.Effect2DDelayTimer = 0;
        NewEffect2D.Effect2DDelayTime = DelayTime;
        NewEffect2D.Effect2DContinueTimer = 0;
        NewEffect2D.Effect2DContinueTime = ContinueTime;
        NewEffect2D.Effect2DWidthCount = WidthCount;
        NewEffect2D.Effect2DHeightCount = HeightCount;
        NewEffect2D.Effect2DStartIndex = StartIndex;
        NewEffect2D.IsSkill = IsSkill;
        NewEffect2D.IsDelay = false;
        NewEffect2D.EffectHurtType = HurtType;
        NewEffect2D.EffectFightProjectile = fp;
        if (TargetBuff1 != null)
        {
            NewEffect2D.Effect2DTargetBuff1 = TargetBuff1;
        }
        if (TargetBuff2 != null)
        {
            NewEffect2D.Effect2DTargetBuff2 = new Buff(TargetBuff2);
        }

        if (TargetIndex != null)
        {
            if (TargetIndex.Count > 0)
            {
                NewEffect2D.Effect2DTargetIndex = new List<int>();
                NewEffect2D.Effect2DTargetType = new List<int>();
                NewEffect2D.Effect2DTargetDamige = new List<int>();
                foreach (int t in TargetIndex)
                {
                    NewEffect2D.Effect2DTargetIndex.Add(t);
                }
                foreach (int t in TargetType)
                {
                    NewEffect2D.Effect2DTargetType.Add(t);
                }
                foreach (int t in TargetDamige)
                {
                    NewEffect2D.Effect2DTargetDamige.Add(t);
                    if (StartIndex > -1)
                    {
                        PictureCreater.instance.AttackCount++;
                    }
                }
            }
        }
        NewEffect2D.IsRock = IsRock;
        NewEffect2D.IsWith = IsWith;
        NewEffect2D.IsUp = IsUp;
        NewEffect2D.IsEnd = false;
        NewEffect2D.IsTarget = true;
        NewEffect2D.IsCollision = IsCollision;
        NewEffect2D.IsFadeOut = IsFadeOut;
        NewEffect2D.IsMonster = IsMonster;
        NewEffect2D.EffectNowFrame = 0;
        NewEffect2D.EffectParentObject = ParentObject;

        ListEffect2D.Add(NewEffect2D);
        Effect2DCount++;
    }

    public void Destroy2Deffect()
    {
        for (int i = 0; i < ListEffect2D.Count; i++)
        {
            if (ListEffect2D[i].Effect2DObject != null)
            {
                DestroyImmediate(ListEffect2D[i].Effect2DObject);
            }
        }
        ListEffect2D.Clear();
        Effect2DCount = 0;
    }

    void FixedUpdate()
    {
        //////////////////////判断特效移动(以下)//////////////////////    
        for (int i = 0; i < ListEffect2D.Count; i++)
        {
            Effect2D e = ListEffect2D[i];

            if (e.Effect2DObject != null)
            {
                if (e.Effect2DDelayTimer >= e.Effect2DDelayTime || PictureCreater.instance.IsSkip)
                {
                    e.EffectNowFrame++;
                    if (!e.IsDelay)
                    {
                        e.IsDelay = true;

                        if(PictureCreater.instance.IsSkip)
                        {
                            e.Effect2DObject.transform.position = e.Effect2DNewPosition;
                            e.Effect2DObject.transform.localScale = new Vector3(e.Effect2DScale.x, e.Effect2DScale.z, 1);
                        }
                        if (e.EffectName != "")
                        {
                            Destroy(e.Effect2DObject);
                            if (e.EffectParentObject != null)
                            {
                                e.Effect2DObject = GameObject.Instantiate(Resources.Load("Prefab/Effect/" + e.EffectName, typeof(GameObject)), e.EffectParentObject.transform.position, Quaternion.identity) as GameObject;
                                e.Effect2DObject.transform.Rotate(e.Effect2DRotation);
                                e.Effect2DObject.transform.parent = e.EffectParentObject.transform;
                                e.Effect2DObject.transform.localPosition = Vector3.zero;
                                e.Effect2DObject.transform.localScale = new Vector3(e.Effect2DScale.x, e.Effect2DScale.z, 1);

                                e.Effect2DNewPosition = e.Effect2DObject.transform.position;
                            }
                            else
                            {
                                if (e.EffectName != "0")
                                {
                                    //Debug.LogError(e.EffectName);
                                    e.Effect2DObject = GameObject.Instantiate(Resources.Load("Prefab/Effect/" + e.EffectName, typeof(GameObject)), e.Effect2DStartPosition, Quaternion.Euler(e.Effect2DRotation)) as GameObject;
                                    e.Effect2DObject.transform.localScale = new Vector3(e.Effect2DScale.x, e.Effect2DScale.z, 1);
                                }
                            }
                        }
                        else
                        {
                            e.Effect2DObject.transform.position = e.Effect2DStartPosition;
                            e.Effect2DObject.transform.localScale = new Vector3(e.Effect2DScale.x, e.Effect2DScale.z, 1);
                        }

                        newEffect2DForwardPosition = e.Effect2DNewPosition - e.Effect2DObject.transform.position;
                        MoveDistance = Vector3.Distance(e.Effect2DNewPosition, e.Effect2DObject.transform.position);
                        e.Effect2DMoveDistance = newEffect2DForwardPosition / MoveDistance * e.Effect2DMoveSpeed * Time.deltaTime * 50;

                        if (e.IsRock)
                        {
                            e.EffectDistance = Vector3.Distance(e.Effect2DStartPosition, e.Effect2DNewPosition);
                            e.Effect2DHalfPosition = e.Effect2DNewPosition;
                            e.Effect2DNewPosition = e.Effect2DStartPosition + (e.Effect2DNewPosition - e.Effect2DStartPosition) / 2 + new Vector3(0, e.EffectDistance / 5 * 4, 0);
                            e.IsDown = false;
                        }
                    }

                    newEffect2DForwardPosition = e.Effect2DNewPosition - e.Effect2DObject.transform.position;
                    MoveDistance = Vector3.Distance(e.Effect2DNewPosition, e.Effect2DObject.transform.position);

                    if (e.IsRock) //攻击特效改方向
                    {
                        e.Effect2DObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                        if (e.Effect2DObject.transform.position.x > e.Effect2DNewPosition.x)
                        {
                            e.Effect2DObject.transform.Rotate(0f, 0f, Mathf.Atan((e.Effect2DObject.transform.position.y - e.Effect2DNewPosition.y) / (e.Effect2DObject.transform.position.x - e.Effect2DNewPosition.x)) * 180 / Mathf.PI);
                        }
                        else if (e.Effect2DObject.transform.position.x < e.Effect2DNewPosition.x)
                        {
                            e.Effect2DObject.transform.Rotate(0f, 0f, Mathf.Atan((e.Effect2DObject.transform.position.y - e.Effect2DNewPosition.y) / (e.Effect2DObject.transform.position.x - e.Effect2DNewPosition.x)) * 180 / Mathf.PI + 180);
                        }
                    }

                    if (Vector3.Distance(Vector3.zero, newEffect2DForwardPosition / MoveDistance * e.Effect2DMoveSpeed * Time.deltaTime * 50) < MoveDistance && !e.IsEnd && !PictureCreater.instance.IsSkip)
                    {
                        if (e.EffectParentObject == null)
                        {
                            e.Effect2DObject.transform.position += newEffect2DForwardPosition / MoveDistance * e.Effect2DMoveSpeed * Time.deltaTime * 50;

                            if (e.IsRock)
                            {
                                if (MoveDistance < e.EffectDistance / 5 && !e.IsDown)
                                {
                                    e.Effect2DNewPosition = e.Effect2DHalfPosition;
                                    if (e.Effect2DObject.transform.position.x > e.Effect2DNewPosition.x)
                                    {
                                        e.Effect2DObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                                    }
                                    else
                                    {
                                        e.Effect2DObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.PI);
                                    }
                                    e.IsDown = true;
                                }
                                else
                                {
                                    if (e.IsDown)
                                    {
                                        e.Effect2DObject.transform.position += new Vector3(0, 0.01f, 0);


                                        if (MoveDistance < e.EffectDistance / 2f)
                                        {
                                            e.Effect2DObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                                            if (e.Effect2DObject.transform.position.x > e.Effect2DNewPosition.x)
                                            {
                                                e.Effect2DObject.transform.Rotate(0f, 0f, Mathf.Atan((e.Effect2DObject.transform.position.y - e.Effect2DNewPosition.y - MoveDistance / 10) / (e.Effect2DObject.transform.position.x - e.Effect2DNewPosition.x)) * 180 / Mathf.PI);
                                            }
                                            else
                                            {
                                                e.Effect2DObject.transform.Rotate(0f, 0f, Mathf.Atan((e.Effect2DObject.transform.position.y - e.Effect2DNewPosition.y - MoveDistance / 10) / (e.Effect2DObject.transform.position.x - e.Effect2DNewPosition.x)) * 180 / Mathf.PI + 180);
                                            }
                                        }
                                        else
                                        {
                                            e.Effect2DObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                                            if (e.Effect2DObject.transform.position.x > e.Effect2DNewPosition.x)
                                            {
                                                e.Effect2DObject.transform.Rotate(0f, 0f, Mathf.Atan((e.Effect2DObject.transform.position.y - e.Effect2DNewPosition.y - MoveDistance / 5) / (e.Effect2DObject.transform.position.x - e.Effect2DNewPosition.x)) * 180 / Mathf.PI);
                                            }
                                            else
                                            {
                                                e.Effect2DObject.transform.Rotate(0f, 0f, Mathf.Atan((e.Effect2DObject.transform.position.y - e.Effect2DNewPosition.y - MoveDistance / 5) / (e.Effect2DObject.transform.position.x - e.Effect2DNewPosition.x)) * 180 / Mathf.PI + 180);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        e.Effect2DObject.transform.position += new Vector3(0, MoveDistance / 20, 0);
                                    }
                                }
                            }
                            e.IsEnd = false;
                        }
                        else
                        {
                            e.IsEnd = true;
                        }
                    }
                    else
                    {
                        if (e.EffectParentObject == null)
                        {
                            e.Effect2DObject.transform.position = new Vector3(e.Effect2DNewPosition.x, e.Effect2DObject.transform.position.y, e.Effect2DNewPosition.z);
                        }


                        if (e.Effect2DTargetIndex != null)
                        {
                            if (e.Effect2DTargetIndex.Count > 0)
                            {
                                if (e.IsTarget)
                                {
                                    for (int j = 0; j < e.Effect2DTargetIndex.Count; j++)
                                    {
                                        try
                                        {
                                            StartCoroutine(PictureCreater.instance.SetPictureEffect(e.IsMonster, e.Effect2DStartIndex, e.Effect2DTargetIndex[j], e.Effect2DTargetDamige[j], e.Effect2DTargetBuff1, e.Effect2DTargetBuff2, e.EffectHurtType, e.EffectFightProjectile, e.IsSkill));
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    e.IsTarget = false;
                                }
                            }
                        }

                        //////////////////////特效到位后播放秒数(以下)//////////////////////
                        e.Effect2DContinueTimer += Time.deltaTime;
                        if (e.Effect2DContinueTimer > e.Effect2DContinueTime)
                        {
                            Destroy(e.Effect2DObject);
                            ListCrear.Add(e);
                            continue;
                        }
                        //////////////////////特效到位后播放秒数(以上)//////////////////////
                    }
                }
                else
                {
                    e.Effect2DDelayTimer += Time.deltaTime;
                }
            }
        }
        //////////////////////判断特效移动(以上)//////////////////////

        foreach (Effect2D e in ListCrear)
        {
            ListEffect2D.Remove(e);
            Effect2DCount--;

            if (Effect2DCount < 0)
            {
                Effect2DCount = 0;
            }
        }
        ListCrear.Clear();
    }
}
