using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click : MonoBehaviour
{
    static int [] statesInt = new int[8];
    static bool[] statesBool = new bool[2];
    static bool inited = false;
    static object lockObject = new object();

	// Use this for initialization
	void Start ()
    {
        print(this.name + " started");

        lock (lockObject)
        {
            if (!inited)
            {
                inited = true;
                for (int i = 0; i < 8; i++)
                    OnClick(i, 0);
                BoolOnclick(8, false);
                BoolOnclick(9, false);
            }
        }
	}
	
    public void OnClickPlus()
    {
        var name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        print("clicked plus: " + name);


        int index = ExtractIndex();
        OnClick(index, 1);
        BoolOnclick(index, true);
    }

    public void OnClickMinus()
    {
        var name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        print("clicked minus: " + name);
        
        int index = ExtractIndex();
        OnClick(index, -1);
        BoolOnclick(index, false);
    }

    void OnClick(int index, int val)
    {
        if (index < 0) return;
        if (index > 7) return;

        statesInt[index] += val;
        var field = GameObject.Find(string.Format("InputField1 ({0})", index)).GetComponent<UnityEngine.UI.InputField>();
        field.text = statesInt[index].ToString();
    }

    void BoolOnclick(int index, bool val)
    {
        if (index < 8) return;
        if (index > 9) return;

        index -= 8;
        statesBool[index] = val;
        var field = GameObject.Find(string.Format("InputField1 ({0})", index + 8)).GetComponent<UnityEngine.UI.InputField>();
        field.text = statesBool[index].ToString();
    }

    int ExtractIndex()
    {
        var name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (name.Length < 3) return -1;
        var splitted = name.Split(' ');
        if (splitted == null) return -1;
        var number = splitted[splitted.Length - 1];
        number = number.Replace("(", "").Replace(")", "");
        int index = -1;
        int.TryParse(number, out index);
        return index;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
