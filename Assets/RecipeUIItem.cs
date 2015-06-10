using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.UI;

public class RecipeUIItem : MonoBehaviour
{
    Transform prefab;
    public string name;
    public int intAmount;
    public float floatAmount;
    public bool IsContinuous { get; set; }
    public bool ForFailure { get; set; }
    public bool UseWholeNumbers { get; set; }
    public float weight = 1;
    //UI Objects
    Text displayNameText;
    Text displayAmountText;

    InputField nameInput;
    InputField amountInput;
    Toggle isContinuousCheckbox;
    Button removeInputButton;


    //called when text box for a value is updated
    public void UpdateCount(string newCount)
    {
        if(UseWholeNumbers)
        {
            intAmount = System.Convert.ToInt32(newCount);
            floatAmount = 0;
            //TODO update display amount text
        }
        else
        {
            floatAmount = System.Convert.ToSingle(newCount);
            intAmount = 0;
            //TODO update display amount text
        }
    }

    //called when text box for the name is updated
    public void UpdateName(string newValue)
    {
        name = newValue;
    }

    public void UpdateWeight(string newValue)
    {
            weight = System.Convert.ToInt32(newValue);
    }

    public void UpdateContinuousToggle(bool newValue)
    {
        UseWholeNumbers = newValue;
    }

    public void WholeNumberToggle(bool newValue)
    {
        UseWholeNumbers = newValue;
    }
    
    public void ForFailureToggle(bool newValue)
    {
        ForFailure = newValue;
    }


    //called when the delete button is pressed
    public void DeleteItem()
    {
        //inform parent and delete self
        var parent = this.transform.parent.parent.GetComponent<RecipeTool>() as RecipeTool;
        parent.RemoveRecipeItem(this.transform);
        Transform.Destroy(this.gameObject);
    }
}