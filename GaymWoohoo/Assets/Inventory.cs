using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public class Item
    {
        public bool isEquipped = false;
        public string name;
        public string type;
        public string statusType;
        public float value;
        public GameObject sprite;
        public int spriteIndex;
        public Vector2 position;
    }
    public List<GameObject> itemSprites = new List<GameObject>();
    public Item[,] inventory = new Item[10, 3];
    // Start is called before the first frame update
    public Item basicSword = new Item();
    public Item healthPotion = new Item();
    public Item nullItem = new Item();
    public GameObject player;
    public TurnSystem tSScript;
    void Start()
    {
        //null item
        nullItem.name = "";
        nullItem.type = "null";
        nullItem.value = 0f;
        nullItem.spriteIndex = 0;

        //basic sword
        basicSword.name = "Basic Sword";
        basicSword.type = "weapon";
        basicSword.statusType = "basic";
        basicSword.value = 2f;
        basicSword.spriteIndex = 1;

        //health potion
        healthPotion.name = "Health Potion";
        healthPotion.type = "healing";
        healthPotion.statusType = "basic";
        healthPotion.value = 2f;
        healthPotion.spriteIndex = 2;   

        //set all item boxes to null
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 10; j++)
            {
                GenerateNullItem(j, i);
            }
        }

        //gain a health potion
        PutItemInInventory(basicSword);
        PutItemInInventory(healthPotion);
        PutItemInInventory(healthPotion);
    }
    void GenerateNullItem(int j, int i)
    {
        Item item = new Item();
        item.name = nullItem.name;
        item.type = nullItem.type;
        item.value = nullItem.value;
        item.spriteIndex = nullItem.spriteIndex;

        item.sprite = Instantiate(itemSprites[item.spriteIndex]);
        item.sprite.transform.localPosition = new Vector2(-8.1f + (1.8f * j), 2.7f - (1.8f * i));
        item.position = new Vector2(-8.1f + (1.8f * j), 2.7f - (1.8f * i));
        item.sprite.transform.SetParent(this.gameObject.transform);
        inventory[j, i] = item;
    }
    public void UseItem(Item itemToBeUsed)
    {
        string dialogue = "";
        switch (itemToBeUsed.type)
        {
            case "weapon":
                if(itemToBeUsed.isEquipped == true)
                {
                    Debug.Log("This item has already been equipped.");
                }
                else
                {
                    for(int i = 0; i < 3; i++)
                    {
                        for(int j = 0; j < 10; j++)
                        {
                            if ((inventory[j, i].isEquipped == true) && (inventory[j, i].type == "weapon"))
                            {
                                inventory[j, i].isEquipped = false;
                            }
                        }
                    }
                    itemToBeUsed.isEquipped = true;
                    player.GetComponent<PlayerController>().damage = itemToBeUsed.value;
                    dialogue = "The " + itemToBeUsed.name + " has been equipped as your " + itemToBeUsed.type + ".";
                }
                break;
            case "healing":
                if (player.GetComponent<PlayerController>().health == player.GetComponent<PlayerController>().maxHealth)
                {
                    Debug.Log("Your health is already full.");
                }
                else
                {
                    player.GetComponent<PlayerController>().health += itemToBeUsed.value;
                    dialogue = "The " + itemToBeUsed.name + " has been used. You have recovered " + itemToBeUsed.value.ToString() + ".";
                    Destroy(itemToBeUsed.sprite);
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (itemToBeUsed == inventory[j, i])
                            {
                                GenerateNullItem(j, i);
                            }
                        }
                    }
                }
                break;
        }
        if (dialogue != "")
        {
            player.GetComponent<PlayerController>().inventoryOn = false;
            player.GetComponent<PlayerController>().choiceMade = false;
            tSScript.UpdateTurn(dialogue);
            this.gameObject.SetActive(false);
        }
        Debug.Log("Item Used");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void PutItemInInventory(Item itemPreset)
    {
        Item item = new Item();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (inventory[j, i].type == "null")
                {
                    item.name = itemPreset.name;
                    item.type = itemPreset.type;
                    item.statusType = itemPreset.statusType;
                    item.value = itemPreset.value;
                    item.spriteIndex = itemPreset.spriteIndex;

                    item.sprite = Instantiate(itemSprites[itemPreset.spriteIndex]);
                    item.sprite.transform.localPosition = inventory[j, i].position;
                    item.position = inventory[j, i].position;
                    item.sprite.transform.SetParent(this.gameObject.transform);
                    Destroy(inventory[j, i].sprite);
                    inventory[j, i] = item;
                    Debug.Log("found position");
                    goto End;
                }
            }
        }
    End:
        Debug.Log("Finished putting item in inventory");
    }
    void FixedUpdate()
    {
        
    }
}
