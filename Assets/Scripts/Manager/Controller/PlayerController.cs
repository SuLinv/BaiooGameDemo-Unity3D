using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 回调
    public event Action AfterHit;
    public event Action AfterDie;
    public event Action NotHit;
    public event Action OnWalk;
    public event Action OnRun;
    public event Action<int> OnCoinChange;
    
    // ������
    GameObject go;
    MeshFilter mf;
    MeshRenderer mr;
    Shader shader;


    Rigidbody rig;
    Animator animator;
    public LayerMask groundLayer;

    /*
        ��������
     */
    // public float maxHealth = 10;
    public List<float> maxAttribute; //存储每个属性最大值，0-health
    // float health;
    public List<float> attribute; //存储每个属性，0-health
    HealthBarController healthBar;
    public int coin = 0;

    // �޵�
    bool invincible = false;
    float invincibleTime = 0.5f;
    float invincibleCountDown;


    bool isAttacking = false;
    bool isDead = false;

    /*
        �ƶ����
     */
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    /*
        �������
     */
    float speed;
    bool isGround = true;
    // ������
    public float jumpForce = 12;
    // ����ˮƽ�ٶ�
    float airSpeed = 0;

    /*
        �������
     */
    float attackTimes = 0;
    float lastAttackTime = 0;
    public float damage = 2;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -35, 0);
        attribute[0] = maxAttribute[0]/2;
    }

    private void OnEnable() {
        InputManager.instance.RegisteTo(gameObject);
    }
    private void Start()
    {
        // ע�ᵽ InputManager
        healthBar = HealthBarController.instance;
        healthBar.ChangeLen(attribute[0], maxAttribute[0]);
    }
    private void Update()
    {
        damage = attribute[2];
        // ����cd����
        if(lastAttackTime > 0)
        {
            lastAttackTime -= Time.deltaTime;
        } else
        {
            isAttacking = false;
        }
        
        // �޵�״̬����
        if(invincible)
        {
            invincibleCountDown -= Time.deltaTime;
            if (invincibleCountDown <= 0)
            {
                invincible = false;
            }
        }

        // 播放动画
        animator.SetFloat("Speed", speed);
        animator.SetFloat("VerticalSpeed", rig.velocity.y);
        
    }

    private void FixedUpdate()
    {
        if (!isGround)
        {
            // ����ˮƽ�ٶ�
            rig.MovePosition(rig.position + transform.forward * airSpeed * Time.fixedDeltaTime);
        }

    }

    //�ܻ��ӿ�
    public void Hit(float damage)
    {
        if(!invincible)
        {
            ChangeHealth(-damage);
            animator.SetTrigger("GetHit");

            invincible = true;
            invincibleCountDown = invincibleTime;
        }
        

        if(Mathf.Approximately(attribute[0], 0) && !isDead)
        {
            isDead = true;
            Die();      
        }
    }

    // �����ӿ�
    public void Die()
    {
        animator.SetBool("Dead", true);
        AfterDie?.Invoke();
    }

    // �����ӿ�
    public void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StopMoving();
            animator.SetFloat("AttackTimes", attackTimes);
            animator.SetTrigger("Attack");
            attackTimes++;
            attackTimes %= 3;
            lastAttackTime = 0.55f;
            Transform eTarget = umbrellaAttact(transform, 60, 4);
            // Debug.Log(eTarget.gameObject.layer);

            if (eTarget && eTarget.gameObject.layer != 6)
            {
                Debug.Log("打到了");
                eTarget.gameObject.GetComponent<EnemyController>()?.Hit(damage);
                AfterHit?.Invoke();
            }
            else
            {
                NotHit?.Invoke();
            }
        }
        
    }

    // ֹͣ�ƶ��ӿڣ�ֹͣ�ƶ���������
    public void StopMoving()
    {
        speed = 0;
    }

    // �ƶ��ӿ�,���뷽�򣬽�ɫ�ƶ�
    public void Move(Vector2 moveDirection)
    {
        if (isGround)
        {
            // ���½�ɫ����
            UpdateRotation(moveDirection);
            // ���˶�������ǲ����м�������
            speed = CheckSpeed();
            rig.MovePosition(rig.position + transform.forward * speed * Time.fixedDeltaTime);
        }
    }

    // ��Ծ�ӿ�
    public void Jump(Vector2 moveDirection)
    {
        if (isGround)
        {
            airSpeed = speed;
            rig.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    // ������״̬
    float CheckSpeed()
    {
        // �����û�а�סshift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            OnRun?.Invoke();
            return runSpeed;
        }
        else
        {
            OnWalk?.Invoke();
            return moveSpeed;
        }
        
    }

    // ����ˮƽ������������ʾ��ɫ��Ҫ��ת�ķ���
    void UpdateRotation(Vector2 moveDirection)
    {
        // û�����룬Ĭ����ǰ��
        if (Mathf.Approximately(moveDirection.magnitude, 0))
            moveDirection.y = 1;
        // ����ָ���Ŀ�귽������ڵ�ǰ��Ļ����
        Vector3 targetDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        // ��ǰ��Ļ���򣬼�������
        Quaternion cameraForward = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        // ������Ŀ�귽��
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward * targetDirection);
        Vector3 target = targetRotation * Vector3.forward;

        // ���½�ɫ����
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 800.0f);
        transform.rotation = newRotation;
    }

    public Transform umbrellaAttact(Transform player, float angle, float radius)
    {
        HashSet<Transform> transInAttackRange = PlayerAttackRange.instance.transInAttackRange;
        foreach(Transform target in transInAttackRange){
            Vector3 deltaA = target.position - player.position;
            //Mathf.Rad2Deg : ����ֵ����ת������
            //Mathf.Acos(f) : ���ز���f�ķ�����ֵ
            float tmpAngle = Mathf.Acos(Vector3.Dot(deltaA.normalized, player.forward)) * Mathf.Rad2Deg;
            if (tmpAngle <= angle * 0.5f && deltaA.magnitude < radius)
            {
                return target;
            }
        }
        return null;
    }

    public void ToDrawSectorSolid(Transform t, Vector3 center, float angle, float radius)
    {
        int pointAmmount = 100;
        float eachAngle = angle / pointAmmount;

        Vector3 forward = t.forward;
        List<Vector3> vertices = new List<Vector3>();

        vertices.Add(center);
        for (int i = 0; i < pointAmmount; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angle / 2 + eachAngle * (i - 1), 0f) * forward * radius + center;
            vertices.Add(pos);
        }
        CreateMesh(vertices);
    }

    private GameObject CreateMesh(List<Vector3> vertices)
    {
        int[] triangles;
        Mesh mesh = new Mesh();

        int triangleAmount = vertices.Count - 2;
        triangles = new int[3 * triangleAmount];

        //���������εĸ�������������������εĶ���˳��
        for (int i = 0; i < triangleAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }

        if (go == null)
        {
            go = new GameObject("mesh");
            go.transform.position = new Vector3(0f, 0.1f, 0.5f);

            mf = go.AddComponent<MeshFilter>();
            mr = go.AddComponent<MeshRenderer>();

            shader = Shader.Find("Unlit/Color");
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        mf.mesh = mesh;
        mr.material.shader = shader;
        mr.material.color = Color.red;

        return go;
    }

    void ChangeHealth(float value)
    {
        // �߽���
        attribute[0] = Mathf.Max(0, attribute[0] + value);
        attribute[0] = Mathf.Min(maxAttribute[0], attribute[0]);

        // �޸�Ѫ������
        healthBar.ChangeLen(attribute[0], maxAttribute[0]);
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 6 && !isGround)
        {
            isGround = true;
            animator.SetBool("IsGround", isGround);
        }
    }

    private void OnTriggerExit(Collider other)
    {
          if (other.gameObject.layer == 6 && isGround)
        {
            isGround = false;
            animator.SetBool("IsGround", isGround);
            StopMoving();
        }

    }

    public void ChangeCoin(int changeValue)
    {
        coin = coin + changeValue;
        if(int.MaxValue - coin < changeValue)
        {
            coin = int.MaxValue;
        }
        OnCoinChange?.Invoke(coin);
        Debug.Log("当前金币数：" + coin);
    }
}
