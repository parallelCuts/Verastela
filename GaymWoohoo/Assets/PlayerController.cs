using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public class Weapon
    {
        public float damage;
    }
    public class Item
    {
        public string effect;
        public float value;
    }
    public bool choiceMade = false;
    public GameObject playerChoiceMenu;
    public Transform menuSelect;
    public string playerChoice = "Attack";
    public EnemyController eCScript;
    public TurnSystem tSScript;
    public List<Item> Items = new List<Item>();
    public float damage;
    public float maxHealth;
    public Inventory inventory;
    public bool inventoryOn = false;
    public bool attackOn = false;
    // Start is called before the first frame update
    public float health;
    public GameObject enemySelect;
    void Start()
    {
        inventory.gameObject.SetActive(false);
        health = 20;
        maxHealth = health;
        enemySelect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(inventoryOn.ToString());
        if (tSScript.turnNum % (tSScript.enemyCount[tSScript.enemyGroupTurnCount] + 1) == tSScript.enemyCount[tSScript.enemyGroupTurnCount])
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if(choiceMade == false)
                {
                    menuSelect.localPosition = new Vector2(0f, -3.55f);
                    playerChoice = "Item";
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(choiceMade == false)
                {
                    menuSelect.localPosition = new Vector2(-6f, -3.55f);
                    playerChoice = "Attack";
                }
            }
            if (Input.GetKeyUp(KeyCode.Return) && choiceMade == false)
            {
                switch (playerChoice)
                {
                    case "Attack":
                        attackOn = true;
                        enemySelect.SetActive(true);
                        break;
                    case "Item":
                        inventoryOn = true;
                        playerChoiceMenu.SetActive(false);
                        break;
                    default:
                        choiceMade = false;
                        inventoryOn = false;
                        attackOn = false;
                        break;
                }
            }
            if (attackOn == true)
            {
                int i = 0;
                eCScript = tSScript.eCScripts[i];
                enemySelect.transform.localPosition = eCScript.transform.localPosition;
                if (Input.GetKeyUp(KeyCode.A) && i != 0)
                {
                    i--;
                }
                if (Input.GetKeyUp(KeyCode.D) && i != (tSScript.eCScripts.Count - 1))
                {
                    i++;
                }
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    eCScript.Hurt(damage);
                    Debug.Log("attacked!");
                    string dialogue = "You attacked the " + eCScript.lCName + " for " + damage.ToString() + " damage!";
                    tSScript.UpdateTurn(dialogue);
                    choiceMade = true;
                }
            }
            if (inventoryOn == true)
            {
                inventory.gameObject.SetActive(true);
                Transform inventorySelect = inventory.gameObject.transform.GetChild(1).transform;
                if (Input.GetKeyDown(KeyCode.W))
                {
                    inventorySelect.localPosition = new Vector2(inventorySelect.localPosition.x, inventorySelect.localPosition.y + 1.8f);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    inventorySelect.localPosition = new Vector2(inventorySelect.localPosition.x - 1.8f, inventorySelect.localPosition.y);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    inventorySelect.localPosition = new Vector2(inventorySelect.localPosition.x, inventorySelect.localPosition.y - 1.8f);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    inventorySelect.localPosition = new Vector2(inventorySelect.localPosition.x + 1.8f, inventorySelect.localPosition.y);
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            Vector2 iSPosition = inventorySelect.transform.localPosition;
                            if (inventory.inventory[j, i].position == iSPosition)
                            {
                                inventory.UseItem(inventory.inventory[j, i]);
                                inventoryOn = false;
                                choiceMade = true;
                            }
                        }
                    }
                }

                if (inventorySelect.localPosition.x > 8.1f)
                {
                    inventorySelect.localPosition = new Vector2(8.1f, inventorySelect.localPosition.y);
                }
                if (inventorySelect.localPosition.x < -8.1f)
                {
                    inventorySelect.localPosition = new Vector2(-8.1f, inventorySelect.localPosition.y);
                }
                if (inventorySelect.localPosition.y > 2.7f)
                {
                    inventorySelect.localPosition = new Vector2(inventorySelect.localPosition.x, 2.7f);
                }
                if (inventorySelect.localPosition.y < -0.9f)
                {
                    inventorySelect.localPosition = new Vector2(inventorySelect.localPosition.x, -0.9f);
                }
            }
        }
    }
    public void Hurt(float damage)
    {
        health -= damage;
    }
}
