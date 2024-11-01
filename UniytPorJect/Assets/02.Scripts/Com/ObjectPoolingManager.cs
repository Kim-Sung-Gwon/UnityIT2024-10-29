using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;

    private GameObject bullet;
    private GameObject Golem;
    private GameObject Necromansor;
    private GameObject bulletPack;

    [SerializeField] private List<GameObject> bulletPoolList = new List<GameObject>();
    [SerializeField] private List<GameObject> GolemPoolList = new List<GameObject>();
    [SerializeField] private List<GameObject> NecromanPoolList = new List<GameObject>();
    [SerializeField] private List<GameObject> bulletPackPoolList = new List<GameObject>();

    public List<Transform> SpawnPointList = new List<Transform>();
    public List<Transform> ItemSpawnList = new List<Transform>();

    int maxBulletPool = 15;
    int maxGolemPool = 8;
    int maxNecPool = 7;
    int MaxItemPool = 7;

    void Awake()
    {
        bullet = Resources.Load<GameObject>("Bullet");
        Golem = Resources.Load<GameObject>("Golem");
        Necromansor = Resources.Load<GameObject>("Necromanser");
        bulletPack = Resources.Load<GameObject>("BulletBox");

        CreateObjectPool(bullet, maxBulletPool, bulletPoolList);
        CreateObjectPool(bulletPack, MaxItemPool, bulletPackPoolList);
        CreateObjectPool(Golem, maxGolemPool, GolemPoolList);
        CreateObjectPool(Necromansor, maxNecPool, NecromanPoolList);
    }

    IEnumerator Start()
    {
        if (poolingManager == null)
            poolingManager = this;
        else if (poolingManager != this)
        {
            Destroy(gameObject);
            yield break;
        }

        SpawnPoints("ItemSpawnPoint", ItemSpawnList);
        StartCoroutine(CreateObject(bulletPackPoolList, ItemSpawnList, 2, 7));

        SpawnPoints("SpawnPoint", SpawnPointList);
        StartCoroutine(CreateObject(GolemPoolList, SpawnPointList, 6, 13));
        StartCoroutine(CreateObject(NecromanPoolList, SpawnPointList, 14, 20));

        #region 최적화 하기전
        //var Itemspawn = GameObject.Find("ItemSpawnPoint");
        //if (Itemspawn != null)
        //    Itemspawn.GetComponentsInChildren<Transform>(ItemSpawnList);
        //ItemSpawnList.RemoveAt(0);
        //yield return new WaitForSeconds(3);
        //if (ItemSpawnList.Count > 0)
        //    StartCoroutine(CreateObject(bulletPackPoolList, ItemSpawnList, 2, 7));

        //var spawnPoint = GameObject.Find("SpawnPoint");
        //if (spawnPoint != null)
        //    spawnPoint.GetComponentsInChildren<Transform>(SpawnPointList);
        //SpawnPointList.RemoveAt(0);
        //yield return new WaitForSeconds(3);
        //if (SpawnPointList.Count > 0)
        //    StartCoroutine(CreateObject(GolemPoolList, SpawnPointList , 6, 13));

        //yield return new WaitForSeconds(3);
        //if (SpawnPointList.Count > 0)
        //    StartCoroutine(CreateObject(NecromanPoolList, SpawnPointList , 14, 20));
        #endregion
    }

    void SpawnPoints(string objName, List<Transform> spawnList)
    {
        Transform obj = GameObject.Find(objName)?.transform;
        if (obj != null)
        {
            obj.GetComponentsInChildren(true, spawnList);
            if (spawnList.Count > 0)
                spawnList.RemoveAt(0);
        }
    }

    public GameObject GetBulletPool()
    {
        for (int i = 0; i < bulletPoolList.Count; i++)
        {
            if (bulletPoolList[i].activeSelf == false)
            {
                return bulletPoolList[i];
            }
        }
        return null;
    }

    void CreateObjectPool(GameObject G_Obj, int maxPool, List<GameObject> G_List)
    {
        GameObject ObjGroup = new GameObject($"{G_Obj}");
        for (int i = 0; i < maxPool; i++)
        {
            var Obj_G = Instantiate(G_Obj, ObjGroup.transform);
            Obj_G.name = $"{(i + 1).ToString()}";
            Obj_G.SetActive(false);
            G_List.Add(Obj_G);
        }
    }

    IEnumerator CreateObject(List<GameObject> objList, List<Transform> objListTr, int minTime, int maxTime)
    {
        while (!GameManager.G_Manager.isGameOver)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            if (GameManager.G_Manager.isGameOver) yield break;
            foreach (GameObject Obj in objList)
            {
                if (Obj.activeSelf == false)
                {
                    int idx = Random.Range(0, objListTr.Count - 1);
                    Obj.transform.rotation = objListTr[idx].rotation;
                    Obj.transform.position = objListTr[idx].position;
                    Obj.gameObject.SetActive(true);
                    break;
                }
                else
                    yield return null;
            }
        }
    }

    #region 실제 생성된는 오브젝트 코드를 하나의 코드로 통일 하기전
    //IEnumerator CreateGolem()
    //{
    //    while (!GameManager.G_Manager.isGameOver)
    //    {
    //        yield return new WaitForSeconds(Random.Range(3, 8));

    //        if (GameManager.G_Manager.isGameOver) yield break;
    //        foreach (GameObject golem in GolemPoolList)
    //        {
    //            if (golem.activeSelf == false)
    //            {
    //                int idx = Random.Range(0, SpawnPointList.Count - 1);
    //                golem.transform.rotation = SpawnPointList[idx].rotation;
    //                golem.transform.position = SpawnPointList[idx].position;
    //                golem.gameObject.SetActive(true);
    //                break;
    //            }
    //            else
    //            {
    //                yield return null;
    //            }
    //        }
    //    }
    //}

    //IEnumerator CreateNecorman()
    //{
    //    while (!GameManager.G_Manager.isGameOver)
    //    {
    //        yield return new WaitForSeconds(Random.Range(4, 10));

    //        if (GameManager.G_Manager.isGameOver) yield break;
    //        foreach (GameObject Necor in NecromanPoolList)
    //        {
    //            if (Necor.activeSelf == false)
    //            {
    //                int idx = Random.Range(0, SpawnPointList.Count - 1);
    //                Necor.transform.rotation = SpawnPointList[idx].rotation;
    //                Necor.transform.position = SpawnPointList[idx].position;
    //                Necor.gameObject.SetActive(true);
    //                break;
    //            }
    //            else
    //            {
    //                yield return null;
    //            }
    //        }
    //    }
    //}

    //IEnumerator CreateBulletPack()
    //{
    //    while (!GameManager.G_Manager.isGameOver)
    //    {
    //        yield return new WaitForSeconds(Random.Range(2, 7));

    //        if (GameManager.G_Manager.isGameOver) yield break;
    //        foreach (GameObject bulletpack in bulletPackPoolList)
    //        {
    //            if (bulletpack.activeSelf == false)
    //            {
    //                int idx = Random.Range(0, ItemSpawnList.Count - 1);
    //                bulletpack.transform.rotation = ItemSpawnList[idx].rotation;
    //                bulletpack.transform.position = ItemSpawnList[idx].position;
    //                bulletpack.gameObject.SetActive(true);
    //                break;
    //            }
    //            else
    //            {
    //                yield return null;
    //            }
    //        }
    //    }
    //}
    #endregion

    #region 풀 리스트에 담는 코드를 하나의 코드로 통일 하기전
    //void CreateBulletPool()
    //{
    //    GameObject BulletGroup = new GameObject("BulletGroup");
    //    for (int i = 0; i < maxBulletPool; i++)
    //    {
    //        var bulletObj = Instantiate(bullet, BulletGroup.transform);
    //        bulletObj.name = $"{(i + 1).ToString()} 발";
    //        bulletObj.SetActive(false);
    //        bulletPoolList.Add(bulletObj);
    //    }
    //}

    //void CreateGolemPool()
    //{
    //    GameObject GolemGroup = new GameObject("EnemyGroup");
    //    for (int i = 0; i < maxGolemPool; i++)
    //    {
    //        var GolemObj = Instantiate(Golem, GolemGroup.transform);
    //        GolemObj.name = $"{(i + 1).ToString()} 명";
    //        GolemObj.SetActive(false);
    //        GolemPoolList.Add(GolemObj);
    //    }
    //}

    //void CreateNcecorPool()
    //{
    //    GameObject NecromGroup = new GameObject("NecormansorGroup");
    //    for (int i = 0; i < maxNecPool; i++)
    //    {
    //        var NecroObj = Instantiate(Necromansor, NecromGroup.transform);
    //        NecroObj.name = $"{(i + 1).ToString()} 명";
    //        NecroObj.SetActive(false);
    //        NecromanPoolList.Add(NecroObj);
    //    }
    //}

    //void BulletPackPool()
    //{
    //    GameObject BulletPackGroup = new GameObject("BulletPackGroup");
    //    for (int i = 0; i < MaxItemPool; i++)
    //    {
    //        var bulletPackObj = Instantiate(bulletPack, BulletPackGroup.transform);
    //        bulletPackObj.name = $"{(i + 1).ToString()} 개";
    //        bulletPackObj.SetActive(false);
    //        bulletPackPoolList.Add(bulletPackObj);
    //    }
    //}
    #endregion
}
