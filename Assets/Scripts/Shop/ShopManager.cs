using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    // ���鵥����
    Dictionary<string, Sprite> iconPool;

    // �̵��е���Ʒ
    public GameObject slot;
    List<GameObject> items;
    // buff��
    BuffManager buffManager;

    // ��Ʒչʾ����
    public GameObject tableView;
    // չʾ�������Ʒ��
    public int ItemNum = 9;

    // ��Ʒ��װ
    ItemFactory itemFactory;

    GameObject chosenItem;

    PlayerBuff playerBuff;
    PlayerController playerController;

    private void Awake()
    {
        base.Awake();
        iconPool = new Dictionary<string, Sprite>();
        // ����icon��Դ
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
        // ��ʼ����ʾ
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

    /*չʾ�̵�ҳ��*/
    public void ShowShop()
    {
        // ��ֹ�ظ���
        CloseShop();
        GetItem();
        gameObject.SetActive(true);
        AutoGridLayoutSize();
        // ��ͣ��Ϸ
        InputManager.instance.PauseTheGame();
    }

    /*�ر��̵�ҳ��*/
    public void CloseShop()
    {
        tableView.transform.DetachChildren();
        gameObject.SetActive(false);
        // ������Ϸ
        InputManager.instance.UnPauseTheGame();
    }

    /*�����ȡbuff*/
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
            // ������Ʒ
            GameObject item = InitItem(buffs[i]);
            item.transform.SetParent(tableView.transform);
            items.Add(item);
        }
    }

    // ϴ���㷨
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
                Debug.Log("Ǯ����");
            }

            // ����������̵�״̬
            ResetAll();
        }
    }

    /*���񲼾�����Ӧ*/
    private void AutoGridLayoutSize()
    {
        GridLayoutGroup grid = tableView.GetComponent<GridLayoutGroup>();
        int num = grid.constraintCount;//ÿ��/��CellԼ���ĸ���
        int k = (grid.transform.childCount + num - 1) / num;
        float value;
        RectTransform.Axis axis;
        switch (grid.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                value = k * grid.cellSize.y;//��������Cell�ĸ߶�
                value += (k - 1) * grid.spacing.y;//ÿ��֮���м��
                value += grid.padding.top + grid.padding.bottom;//���¼��
                axis = RectTransform.Axis.Vertical;
                break;
            case GridLayoutGroup.Constraint.FixedRowCount:
                value = k * grid.cellSize.x;//��������Cell�ĸ߶�
                value += (k - 1) * grid.spacing.x;//ÿ��֮���м��
                value += grid.padding.left + grid.padding.right;//���Ҽ��
                axis = RectTransform.Axis.Horizontal;
                break;
            default:
                Debug.LogError(grid.name + "��Լ��Ϊ��Flexibl��");
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
