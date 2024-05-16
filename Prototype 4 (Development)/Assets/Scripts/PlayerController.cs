using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed = 5.0f;
    public bool isBuffActive = false; // ���� ���¸� ��Ÿ���� ����
    public bool hasBuff2 = false; // ����2 ���¸� ��Ÿ���� ����

    public Buff buff; // Buff ��ũ��Ʈ ���� ����
    public Buff2 buff2; // Buff2 ��ũ��Ʈ ���� ����

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        // Buff �� Buff2 ��ũ��Ʈ �ʱ�ȭ
        buff = GetComponent<Buff>();
        buff2 = GetComponent<Buff2>();
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        // �÷��̾ �ʹ� �Ʒ��� �������� ��
        if (transform.position.y < -10)
        {
            if (hasBuff2)
            {
                RespawnPlayer();
                hasBuff2 = false; // �� �� �������Ǹ� ����2 ���� ����
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // ����� �� ���� �����۰� �浹 �� ó��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeBuff"))
        {
            Destroy(other.gameObject);
            IncreaseRigidbodySizeForEnemies();
        }
        else if (other.CompareTag("DeBuff2"))
        {
            Destroy(other.gameObject);
            IncreaseRigidbodySizeForEnemies();
        }
        else if (other.CompareTag("DeBuff3"))
        {
            Destroy(other.gameObject);
            PushPlayerAway(other.gameObject);
        }
        else if (other.CompareTag("Buff"))
        {
            Destroy(other.gameObject);
            ActivateBuff();
        }
        else if (other.CompareTag("Buff2"))
        {
            Destroy(other.gameObject);
            ActivateBuff2();
        }
    }

    // �ֺ��� ��� ���� Rigidbody ũ�⸦ ������Ű�� �Լ�
    private void IncreaseRigidbodySizeForEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            DeBuff2 deBuff2 = enemy.GetComponent<DeBuff2>();
            if (deBuff2 != null)
            {
                deBuff2.IncreaseRigidbodySize();
            }
        }
    }

    // �÷��̾ �о�� �Լ�
    private void PushPlayerAway(GameObject debuff3Object)
    {
        DeBuff3 deBuff3 = debuff3Object.GetComponent<DeBuff3>();
        if (deBuff3 != null)
        {
            Vector3 awayFromObject = (transform.position - debuff3Object.transform.position).normalized;
            playerRb.AddForce(awayFromObject * deBuff3.pushStrength, ForceMode.Impulse);
            Debug.Log("Player pushed away from " + debuff3Object.name);
        }
    }

    // ������ Ȱ��ȭ�ϴ� �Լ�
    private void ActivateBuff()
    {
        if (buff != null)
        {
            buff.ActivateBuff();
        }
    }

    // ����2�� Ȱ��ȭ�ϴ� �Լ�
    private void ActivateBuff2()
    {
        if (buff2 != null)
        {
            buff2.ActivateBuff();
        }
    }

    // ����2�� ��Ȱ��ȭ�ϴ� �Լ�
    public void DeactivateBuff2()
    {
        hasBuff2 = false;
    }

    // �÷��̾ ���� ��ġ�� �������ϴ� �Լ�
    private void RespawnPlayer()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-9, 9), 0, Random.Range(-9, 9));
        transform.position = spawnPosition;
        playerRb.velocity = Vector3.zero; // ������ �� �ӵ� �ʱ�ȭ
        playerRb.angularVelocity = Vector3.zero; // ������ �� ȸ�� �ӵ� �ʱ�ȭ
    }
}
