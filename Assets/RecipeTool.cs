using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.UI;

public class RecipeTool : MonoBehaviour
{

    List<Transform> inputs = new List<Transform>();
    List<Transform> outputs = new List<Transform>();

    enum OutputMode { AllOutputs, RandomPick, RandomAmount };
    int numberOfRandomOutputs = 0;
    float duration = 5f;
    float durationVariation = 0;
    string name = "Generic Item";
    OutputMode mode = OutputMode.AllOutputs;

    public Text nameText;
    public Text durationText;

    public Transform inputPrefab;
    public Transform outputPrefab;

    public Transform leftPanel;
    public Transform rightPanel;

    int inputRowHeight = 50;
    int outputRowHeight = 70;

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
        duration = Mathf.Abs(System.Convert.ToSingle(durationIn));
        durationText.text = "Duration: " + duration.ToString() + " seconds";
    }

    public void AddInput()
    {
        var input = Instantiate(inputPrefab);        //instantiate an input prefab
        input.SetParent(leftPanel);
        var pos = transform.position;
        pos.x = 103f;
        pos.y = Screen.height - 120 - inputs.Count * inputRowHeight;
        input.transform.position = pos;
        inputs.Add(input);                           //add to list
        foreach (Transform child in leftPanel)
        {
            if (child.name == "Add Input Button")
            {
                pos = child.transform.position;
                pos.y -= inputRowHeight;
                child.transform.position = pos;
                break;
            }
        }
    }

    public void AddOutput()
    {
        var output = Instantiate(outputPrefab);        //instantiate an output prefab
        output.SetParent(rightPanel);
        var pos = transform.position;
        pos.x = 531f;
        pos.y = Screen.height - 120 - outputs.Count * outputRowHeight;
        output.transform.position = pos;

        outputs.Add(output);                           //add to list  
        foreach (Transform child in rightPanel)
        {
            if (child.name == "Add Output Button")
            {
                pos = child.transform.position;
                pos.y -= outputRowHeight;
                child.transform.position = pos;
                break;
            }
        }
    }

    public void RemoveRecipeItem(Transform itemToRemove)
    {
        bool found = false;
        for (int i = 0; i < inputs.Count; i++)
        {
            if (found)
            {
                var pos = inputs[i].transform.position;
                pos.y += inputRowHeight;
                inputs[i].transform.position = pos;
            }
            else
            {
                if (inputs[i] = itemToRemove)
                {
                    found = inputs.Remove(itemToRemove);
                    foreach (Transform child in leftPanel)
                    {
                        if (child.name == "Add Input Button")
                        {
                            var pos = child.transform.position;
                            pos.y += inputRowHeight;
                            child.transform.position = pos;
                            break;
                        }
                    }
                    i--;
                }
            }
        }
        if (found)
        {
            return;
        }
        for (int i = 0; i < outputs.Count; i++)
        {

            if (found)
            {
                var pos = outputs[i].transform.position;
                pos.y += outputRowHeight;
                outputs[i].transform.position = pos;
            }
            else
            {
                if (outputs[i] = itemToRemove)
                {
                    found = outputs.Remove(itemToRemove);
                    foreach (Transform child in rightPanel)
                    {
                        if (child.name == "Add Output Button")
                        {
                            var pos = child.transform.position;
                            pos.y += outputRowHeight;
                            child.transform.position = pos;
                            break;
                        }
                    }
                    i--;
                }
            }
        }
    }

    public void SaveRecipe()
    {
        Debug.Log("save");

        var settings = new XmlWriterSettings();
	    settings.Indent = true;
        settings.IndentChars = ("\t");
        settings.CloseOutput = true;
        settings.OmitXmlDeclaration = true;

        string filename = "Assets/Data/Recipes/" + nameText.text + ".xml";
        using (XmlWriter writer = XmlWriter.Create(filename,settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Recipe");
            writer.WriteElementString("Name", nameText.text);
            writer.WriteElementString("Duration", duration.ToString());
            writer.WriteElementString("Duration Variation", durationVariation.ToString());
            writer.WriteElementString("Output Mode", mode.ToString());

            foreach (var input in inputs)
            {
                var item = input.GetComponent<RecipeUIItem>() as RecipeUIItem;
                writer.WriteStartElement("InputItem");
                //name
                writer.WriteElementString("Name", item.name);
                //amount
                if (item.intAmount != 0)
                {
                      writer.WriteElementString("Int Amount", item.intAmount.ToString());
                }
                else if (item.floatAmount != 0)
                {
                      writer.WriteElementString("Float Amount", item.floatAmount.ToString());
                }
                //is continuous
                writer.WriteElementString("Is Continuous", item.IsContinuous.ToString());
                //whole number
                writer.WriteElementString("Whole Number", item.UseWholeNumbers.ToString());
                //for failure
                writer.WriteElementString("For Failure", item.ForFailure.ToString());
                //weight
                writer.WriteElementString("Weight", item.weight.ToString());

                writer.WriteEndElement();
                
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();

        }
    }

    public void ClearRecipe()
    {
        Debug.Log("clear");
    }
}

public class RecipeItem
{
    string name;
    float floatAmount;
    int intAmount;
    float weight;
}
