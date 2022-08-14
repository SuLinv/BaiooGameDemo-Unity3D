using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { NOTARGET, CHASE, ATTACK, DIE }

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public List<DoorOpen> connectedDoor;


    public event Action<float, float> onEnemyHealthChange;
    [HideInInspector]
    public GameObject attackTarget;
    public LayerMask searchLayer;
    [HideInInspector]
    public NavMeshAgent agent;

    // ---- ��ʼ��Ϣ ----
    public Vector3 oriPosition;
    public Quaternion oriRotation;
    public float maxHealth = 10;
    float health;
    EnemyState enemyState = EnemyState.NOTARGET;

    // ---- ������� ----
    // ���з�Χ
    public float findRange = 8;
    // ֹͣ���룬����С�����ֵ�϶�Ϊ����
    public float stopDistance = 0.2f;

    // ---- �������� ----
    // ������
    public float damage;
    // ������ȴʱ��
    public float maxCoolDown = 2;

    // ---- ��ɱ���� ----
    public int coin;

    IState state;
    Dictionary<EnemyState, IState> stateMap;

    public Animator ani;

    bool iswalk;
    bool isdead;
    public bool isWalk
    {
        get { return iswalk; }
        set { iswalk = value; }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        oriPosition = transform.position;
        oriRotation = transform.rotation;
        health = maxHealth;

        // ע��״̬
        stateMap = new Dictionary<EnemyState, IState>();
        stateMap.Add(EnemyState.NOTARGET, new NoTargetState(this));
        stateMap.Add(EnemyState.CHASE, new ChaseState(this));
        stateMap.Add(EnemyState.ATTACK, new AttackState(this));
        
        ani = GetComponent<Animator>();

        
    }

    private void Start()
    {
        // 注册到关卡门的开启控件中
        foreach (DoorOpen door in connectedDoor)
        {
            door.enemyCount++; 
        }

        state = stateMap[EnemyState.NOTARGET];
        state.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState != EnemyState.DIE && state != null)
        {
            state.OnUpdate();
        }

        if(!isDePlayerSpeed && health/maxHealth <= 0.8f)    StartCoroutine(dePlayerSpeed(4.0f,5.0f));
        if(!isBreakBack && health/maxHealth <= 0.5f)    StartCoroutine(breakBackPlayer());
        if(!isAreaDamage && health/maxHealth <= 0.3f)   StartCoroutine(areaDamage(gameObject,5,2.0f));
        if(!isStopPlayer && health/maxHealth <= 0.3f)   StartCoroutine(stopPlayer(2));
        if(!isCreateSphere && health/maxHealth <= 0.3f) StartCoroutine(createShpere(50,3.0f));
        
        switchAnistate();
    }

    public void Stop()
    {
        agent.destination = transform.position;
        iswalk = false;
        agent.isStopped = true;
    }

    /*�ܻ�*/
    public void Hit(float damage)
    {
        health = Mathf.Max(0, health - damage);
        // �ܻ��ص�������UI
        onEnemyHealthChange?.Invoke(health, maxHealth);
        if (Mathf.Approximately(health, 0))
        {
            // ????????
            Stop();
            isdead = true;
            enemyState = EnemyState.DIE;
            state = null;
            attackTarget?.GetComponent<PlayerController>().ChangeCoin(coin);
            foreach(DoorOpen door in connectedDoor)
            {
                door.enemyCount--;
            }
            StartCoroutine(Die());
        }
        else
        {
            ani.SetTrigger("Hit");
        }
    }

    /*����*/
    IEnumerator Die()
    {
        // ���Ŷ���
        ani.SetBool("DEAD", true);
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
    }

    void switchAnistate()
    {
        ani.SetBool("WALK", iswalk);
    }

    public void TransitionState(EnemyState state)
    {
        enemyState = state;
        this.state = stateMap[state];
        this.state.OnEnter();
    }

    public void OnPlayerDie()
    {
        attackTarget = null;
        TransitionState(EnemyState.NOTARGET);
    }

    bool isBreakBack = false;
    private IEnumerator breakBackPlayer(){
        isBreakBack = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f);
        Debug.Log(hitColliders.Length);
        foreach(var col in hitColliders){
            if(col.gameObject.layer != 6){
                if(col.gameObject.tag == "Player"){
                    col.gameObject.GetComponent<Rigidbody>().MovePosition(col.transform.position + transform.forward * 5.0f);
                    break;
                }
            }
        }
        yield return null;
    }

    bool isAreaDamage = false;
    private IEnumerator areaDamage(GameObject controller,int duration,float skillDamage)
    {
        isAreaDamage = true;
        float lookAngle = 90f;
        float lookAccurate = 70f;
        for(int t=0;t<duration;t++){
            if (LookAround(controller, Quaternion.identity, Color.green)){
                InputManager.instance.player.GetComponent<PlayerController>().attribute[0] -= skillDamage;
                yield return new WaitForSeconds(1.0f);
                break;
            }   
            float subAngle = (lookAngle / 2) / lookAccurate;
            for (int i = 0; i < lookAccurate; i++)
            {
                if (LookAround(controller, Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), Color.green)
                    || LookAround(controller, Quaternion.Euler(0, subAngle * (i + 1), 0), Color.green)){
                        InputManager.instance.player.GetComponent<PlayerController>().attribute[0] -= skillDamage;
                        HealthBarController.instance.ChangeLen(InputManager.instance.player.GetComponent<PlayerController>().attribute[0]
                        ,InputManager.instance.player.GetComponent<PlayerController>().maxAttribute[0]);
                        yield return new WaitForSeconds(1.0f);
                        break;
                    }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    bool LookAround(GameObject attacker, Quaternion eulerAnger, Color DebugColor)
    {
        float distance;

        RaycastHit hit;

        if (Physics.Raycast(attacker.transform.position, eulerAnger * transform.forward, out hit, 5) && hit.collider.CompareTag("Player"))
        {
            //controller.chaseTarget = hit.transform;
            return true;
           
        }
        if (hit.collider == null)
        {
            distance = 5f;
        }
        else
        {
            distance = Vector3.Distance(attacker.transform.position, hit.collider.gameObject.transform.position);
        }
        Debug.DrawRay(attacker.transform.position, eulerAnger * attacker.transform.forward.normalized * distance, DebugColor);

        return false;
    }

    bool isStopPlayer = false;
    private IEnumerator stopPlayer(int duration){
        isStopPlayer = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f);
        foreach(var col in hitColliders){
            if(col.gameObject.tag == "Player"){
                col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                yield return new WaitForSeconds(duration);
                col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
            }           
        }
        yield return null;
    }

    public GameObject enemySphere;
    bool isCreateSphere = false;
    private IEnumerator createShpere(int snum,float radius){
        isCreateSphere = true;
        for(int i=0;i<snum;i++){
            Vector3 s_pos = new Vector3(UnityEngine.Random.Range(transform.position.x - radius,transform.position.x + radius)
            ,transform.position.y + 10.0f
            ,UnityEngine.Random.Range(transform.position.z - radius,transform.position.z + radius));
            GameObject sphere = Instantiate(enemySphere,s_pos,Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    private void createRay(float radius){
        Vector3[] rayPos = {new Vector3(transform.position.x,transform.position.y+1,transform.position.z + radius)
        ,new Vector3(transform.position.x + radius*Mathf.Cos(30),transform.position.y+1,transform.position.z + radius*Mathf.Sin(30))
        ,new Vector3(transform.position.x - radius*Mathf.Cos(30),transform.position.y+1,transform.position.z + radius*Mathf.Sin(30))
        ,new Vector3(transform.position.x - radius*Mathf.Sin(30),transform.position.y+1,transform.position.z - radius*Mathf.Cos(30))
        ,new Vector3(transform.position.x + radius*Mathf.Sin(30),transform.position.y,transform.position.z - radius*Mathf.Cos(30))};
        foreach(Vector3 pos in rayPos){
            Ray ray = new Ray(transform.position, pos - transform.position);
            Debug.DrawRay(ray.origin ,ray.direction , Color.red,5.0f);
        }
    }

    bool isDePlayerSpeed = false;
    private IEnumerator dePlayerSpeed(float deValue,float duration){
        isDePlayerSpeed = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f);
        foreach(var col in hitColliders){
            if(col.gameObject.GetComponent<PlayerController>()){
                col.gameObject.GetComponent<PlayerController>().moveSpeed -= deValue;
                col.gameObject.GetComponent<PlayerController>().runSpeed -= deValue;
                yield return new WaitForSeconds(duration);
                col.gameObject.GetComponent<PlayerController>().moveSpeed += deValue;
                col.gameObject.GetComponent<PlayerController>().runSpeed += deValue;
                break;
            }           
        }
        yield return null;
    }

}
