using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.UI;

public class RecipeTool : MonoBehaviour {

    List<Transform> inputs = new List<Transform>();
    List<Transform> outputs = new List<Transform>();

    enum OutputMode { Normal, RandomPick, RandomAmount };
    int numberOfRandomOutputs = 0;
    float duration = 5f;
    float durationVariationPercent = 0;
    string name = "Generic Item";

    public Text nameText;
    public Text durationText;

    public Transform inputPrefab;
    public Transform outputPrefab;

    void Start()
    {
        nameText.text = name;
        durationText.text = "Duration: " + duration.ToString() + " seconds";
        AddInput();
        AddOutput();

    }

    public void SetName(string nameIn)
    {
        name = nameIn;
        nameText.text = name;
    }

    public void SetDuration(string durationIn)
    {
        duration = Mathf.Abs( System.Convert.ToSingle( durationIn ) );
        durationText.text = "Duration: " + duration.ToString() + " seconds";
    }

    public void AddInput()
    {
        var input = Instantiate(inputPrefab);        //instantiate an input prefab
        input.SetParent(this.transform);
        var pos = transform.position;
        pos.x = 98f;
        pos.y = Screen.height-150 - inputs.Count * 30f;
        input.transform.position = pos;
        inputs.Add(input);                           //add to list
    }

    public void AddOutput()
    {
        var output = Instantiate(outputPrefab);        //instantiate an output prefab
        output.SetParent(this.transform);
        var pos = transform.position;
        pos.x = 518f;
        pos.y = Screen.height - 150 - outputs.Count * 30f;
        output.transform.position = pos;

        outputs.Add(output);                           //add to list  
    }

}

public class RecipeItem
{
    string name;
    float floatAmount;
    int intAmount;
    float weight;
}

public class RecipeUIItem
{
    Transform prefab;
    string name;
    int intAmount;
    float floatAmount;
    public bool IsContinuous { get; set; }

    //UI Objects
    Text displayNameText;
    Text displayAmountText;

    InputField nameInput;
    InputField amountInput;
    Toggle isContinuousCheckbox;
    Button removeInputButton;

    RecipeUIItem()
    {

    }

    //called when text box for a value is updated
    void UpdateValue(string newValue)
    {
        //if continuous
            //use floatAmount
            //cast newValue to float
            //update display amount text
        //else not continuous
            //use intAmount
            //cast newValue to int
            //update display amount text
    }

    //called when text box for the name is updated
    void UpdateName(string newValue)
    {
        name = newValue;
    }

    //called when the delete button is pressed
    public void DeleteItem()
    {
        //inform parent and delete self
    }
}