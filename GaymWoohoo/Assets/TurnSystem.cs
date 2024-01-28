using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public List<int> enemyIndex = new List <int>();
    public List<int> enemyCount = new List <int>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public PlayerController pCScript;
    public List<EnemyController> eCScripts = new List<EnemyController>();
    public int turnNum;
    public TextMeshProUGUI text;
    public string curDial;
    public bool nextTurn = false;
    public GameObject playerChoice;
    public GameObject textBox;
    public GameObject inventory;
    public Animator transition;
    public int deathCount = 0;
    public int enemyGroupTurnCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        turnNum = 0;
        curDial = "";
        textBox.SetActive(false);
        playerChoice.SetActive(true);
        UpdateEnemy();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(deathCount == enemyCount[enemyGroupTurnCount])
        {
            UpdateEnemy();
            deathCount = 0;
        }
    }
    public void UpdateEnemy()
    {
        eCScripts.Clear();
        for (int i = 0; i < enemyCount[enemyGroupTurnCount]; i++)
        {
            GameObject enemy = enemyPrefabs[enemyIndex[i]];
            Instantiate(enemy);
            eCScripts.Add(enemy.GetComponent<EnemyController>());
            eCScripts[i].tSScript = this;
            eCScripts[i].pCScript = this.pCScript;
            eCScripts[i].pITW = i;
            eCScripts[i].tW = enemyCount[enemyGroupTurnCount];
        }
        enemyGroupTurnCount++;
    }
    public async void UpdateTurn(string dialogue)
    {
        Debug.Log(dialogue);
        foreach(char s in dialogue)
        {
            textBox.SetActive(true);
            playerChoice.SetActive(false);
            inventory.SetActive(false);
            text.text += s;
            await Task.Delay(50);
        }
        await Task.Delay(150);
        text.text = "";
        textBox.SetActive(false);
        playerChoice.SetActive(true);
        turnNum++;
        Debug.Log(turnNum);
    }
    public async void DeathTurn(string deathMsg)
    {
        Debug.Log(deathMsg);
        foreach (char s in deathMsg)
        {
            textBox.SetActive(true);
            playerChoice.SetActive(false);
            text.text += s;
            await Task.Delay(50);
        }
        Debug.Log("yahoo");
        await Task.Delay(150);
        text.text = "";
        textBox.SetActive(false);
        deathCount++;
        Exit();
    }
    public void Exit()
    {
        //transition.SetBool("EnemyHasDied", true);
    }
    void FixedUpdate()
    {
        
    }
}
