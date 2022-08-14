using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    // 精灵单例池
    Dictionary<string, Sprite> iconPool;

    // 商店中的商品
    public GameObject slot;
    List<GameObject> items;
    // buff池
    BuffManager buffManager;

    // 商品展示窗口
    public GameObject tableView;
    // 展示的最大商品数
    public int ItemNum = 9;

    // 商品组装
    ItemFactory itemFactory;

    GameObject chosenItem;

    PlayerBuff playerBuff;
    PlayerController playerController;

    private void Awake()
    {
        base.Awake();
        iconPool = new Dictionary<string, Sprite>();
        // 加载icon资源
        Sprite[] icons = Resources.LoadAll<Sprite>("Icon");
        foreach(Sprite icon in icons)
        {
            iconPool.Add(icon.name, icon);
        }
    }

    private void Start()
    {
        buffManager = BuffManager.Instance;
        playerBuff = InputManager.instance.player.GetComponent<PlayerBuff>();
        playerController = InputManager.instance.player.GetComponent<PlayerController>();
        // 初始不显示
        gameObject.SetActive(false);
    }

    public void SetChosen(GameObject item)
    {
        foreach(GameObject slot in items)
        {
            ChangeItemColor(slot, Color.black);
        }
        ChangeItemColor(item, Color.grey);
        this.chosenItem = item;
    }

    void ChangeItemColor(GameObject item, Color color)
    {
        Image image = item.GetComponent<Image>();
        image.color = color;
    }

    /*展示商店页面*/
    public void ShowShop()
    {
        // 防止重复打开
        CloseShop();
        GetItem();
        gameObject.SetActive(true);
        AutoGridLayoutSize();
        // 暂停游戏
        InputManager.instance.PauseTheGame();
    }

    /*关闭商店页面*/
    public void CloseShop()
    {
        tableView.transform.DetachChildren();
        gameObject.SetActive(false);
        // 继续游戏
        InputManager.instance.UnPauseTheGame();
    }

    /*随机获取buff*/
    void GetItem()
    {     
        BuffBase[] buffs = buffManager.buffBase.ToArray();
        items = new List<GameObject>();
        int len = buffs.Length;
        if(len > ItemNum)
        {
            len = ItemNum;
            Shuffle(buffs);
        }
        for (int i = 0; i < len; i++)
        {
            // 构造商品
            GameObject item = InitItem(buffs[i]);
            item.transform.SetParent(tableView.transform);
            items.Add(item);
        }
    }

    // 洗牌算法
    void Shuffle(BuffBase[] buffs)
    {
        int n = buffs.Length;
        for(int i = n - 1; i >= 0; i--)
        {
            int swap = Random.Range(0, i + 1);
            BuffBase temp = buffs[i];
            buffs[i] = buffs[swap];
            buffs[swap] = temp;     
        }
    }

    GameObject InitItem(BuffBase buff)
    {
        GameObject item = Instantiate(slot);
        itemFactory = new BuffItemFactory(buff, item, iconPool);
        item = itemFactory.CreateItem();
        return item;
    }

    public void BuyBuff()
    {
        if (chosenItem != null)
        {
            BuffBase chosenBuff = chosenItem.GetComponent<ItemController>().Buff;
            if (playerController.coin >= chosenBuff.buffCost)
            {
                playerController.ChangeCoin(-chosenBuff.buffCost);
                playerBuff.AddBuff(chosenBuff);
                chosenItem = null;
            }
            else
            {
                Debug.Log("钱不够");
            }

            // 购买后重置商店状态
            ResetAll();
        }
    }

    /*网格布局自适应*/
    private void AutoGridLayoutSize()
    {
        GridLayoutGroup grid = tableView.GetComponent<GridLayoutGroup>();
        int num = grid.constraintCount;//每行/列Cell约束的个数
        int k = (grid.transform.childCount + num - 1) / num;
        float value;
        RectTransform.Axis axis;
        switch (grid.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                value = k * grid.cellSize.y;//列数乘以Cell的高度
                value += (k - 1) * grid.spacing.y;//每列之间有间隔
                value += grid.padding.top + grid.padding.bottom;//上下间隔
                axis = RectTransform.Axis.Vertical;
                break;
            case GridLayoutGroup.Constraint.FixedRowCount:
                value = k * grid.cellSize.x;//行数乘以Cell的高度
                value += (k - 1) * grid.spacing.x;//每行之间有间隔
                value += grid.padding.left + grid.padding.right;//左右间隔
                axis = RectTransform.Axis.Horizontal;
                break;
            default:
                Debug.LogError(grid.name + "的约束为：Flexibl！");
                return;
        }
        (grid.transform as RectTransform).SetSizeWithCurrentAnchors(axis, value);
    }

    void ResetAll()
    {
        foreach(GameObject slot in items)
        {
            ChangeItemColor(slot, Color.black);
            chosenItem = null;
        }
    }
}
