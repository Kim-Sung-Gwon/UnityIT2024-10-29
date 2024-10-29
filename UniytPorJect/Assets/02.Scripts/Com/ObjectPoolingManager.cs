using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;

    private GameObject bullet;
    private GameObject Golem;
    private GameObject Necromansor;
    private GameObject bulletPack;

    public List<GameObject> bulletPoolList;
    public List<GameObject> GolemPoolList;
    public List<GameObject> NecromanPoolList;
    public List<GameObject> bulletPackPoolList;

    public List<Transform> SpawnPointList;
    public List<Transform> ItemSpawnList;

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

        CreateBulletPool();
        BulletPackPool();
        CreateGolemPool();
        CreateNcecorPool();
    }

    IEnumerator Start()
    {
        if (poolingManager == null)
            poolingManager = this;
        else if (poolingManager != this)
            Destroy(gameObject);

        var spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null)
            spawnPoint.GetComponentsInChildren<Transform>(SpawnPointList);
        SpawnPointList.RemoveAt(0);
        yield return new WaitForSeconds(3);
        if (SpawnPointList.Count > 0)
            StartCoroutine(CreateGolem());
        yield return new WaitForSeconds(3);
        if (SpawnPointList.Count > 0)
            StartCoroutine(CreateNecorman());

        var Itemspawn = GameObject.Find("ItemSpawnPoint");
        if (Itemspawn != null)
            Itemspawn.GetComponentsInChildren<Transform>(ItemSpawnList);
        ItemSpawnList.RemoveAt(0);
        yield return new WaitForSeconds(3);
        if (ItemSpawnList.Count > 0)
            StartCoroutine(CreateBulletPack());
    }

    void CreateBulletPool()
    {
        GameObject BulletGroup = new GameObject("BulletGroup");
        for (int i = 0; i < maxBulletPool; i++)
        {
            var bulletObj = Instantiate(bullet, BulletGroup.transform);
            bulletObj.name = $"{(i + 1).ToString()} 발";
            bulletObj.SetActive(false);
            bulletPoolList.Add(bulletObj);
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

    void CreateGolemPool()
    {
        GameObject GolemGroup = new GameObject("EnemyGroup");
        for (int i = 0; i < maxGolemPool; i++)
        {
            var GolemObj = Instantiate(Golem, GolemGroup.transform);
            GolemObj.name = $"{(i + 1).ToString()} 명";
            GolemObj.SetActive(false);
            GolemPoolList.Add(GolemObj);
        }
    }

    IEnumerator CreateGolem()
    {
        while (!GameManager.G_Manager.isGameOver)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));

            if (GameManager.G_Manager.isGameOver) yield break;
            foreach (GameObject golem in GolemPoolList)
            {
                if (golem.activeSelf == false)
                {
                    int idx = Random.Range(0, SpawnPointList.Count - 1);
                    golem.transform.rotation = SpawnPointList[idx].rotation;
                    golem.transform.position = SpawnPointList[idx].position;
                    golem.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    void CreateNcecorPool()
    {
        GameObject NecromGroup = new GameObject("NecormansorGroup");
        for (int i = 0; i < maxNecPool; i++)
        {
            var NecroObj = Instantiate(Necromansor, NecromGroup.transform);
            NecroObj.name = $"{(i + 1).ToString()} 명";
            NecroObj.SetActive(false);
            NecromanPoolList.Add(NecroObj);
        }
    }

    IEnumerator CreateNecorman()
    {
        while (!GameManager.G_Manager.isGameOver)
        {
            yield return new WaitForSeconds(Random.Range(4, 10));

            if (GameManager.G_Manager.isGameOver) yield break;
            foreach (GameObject Necor in NecromanPoolList)
            {
                if (Necor.activeSelf == false)
                {
                    int idx = Random.Range(0, SpawnPointList.Count - 1);
                    Necor.transform.rotation = SpawnPointList[idx].rotation;
                    Necor.transform.position = SpawnPointList[idx].position;
                    Necor.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    void BulletPackPool()
    {
        GameObject BulletPackGroup = new GameObject("BulletPackGroup");
        for (int i = 0; i < MaxItemPool; i++)
        {
            var bulletPackObj = Instantiate(bulletPack, BulletPackGroup.transform);
            bulletPackObj.name = $"{(i + 1).ToString()} 개";
            bulletPackObj.SetActive(false);
            bulletPackPoolList.Add(bulletPackObj);
        }
    }

    IEnumerator CreateBulletPack()
    {
        while (!GameManager.G_Manager.isGameOver)
        {
            yield return new WaitForSeconds(Random.Range(2, 7));

            if (GameManager.G_Manager.isGameOver) yield break;
            foreach (GameObject bulletpack in bulletPackPoolList)
            {
                if (bulletpack.activeSelf == false)
                {
                    int idx = Random.Range(0, ItemSpawnList.Count - 1);
                    bulletpack.transform.rotation = ItemSpawnList[idx].rotation;
                    bulletpack.transform.position = ItemSpawnList[idx].position;
                    bulletpack.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}
