using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TextPositions : MonoBehaviour
{
    public Camera mCamera;
    
    void Start()
    {
        setTextPositions();
        setTextFonts();
    }

    private void setTextPositions()
    {
        //Top panel
        var tCoefX1 = 0.16f;
        var tCoefX2 = 0.5f;
        var tCoefX3 = 0.85f;
        var tCoefY = 0.0f;
        var tCoefDY = 0.0f;
        var tCoefDY1 = 0.0f;
        
        var aspectRatio = mCamera.aspect;
        if (aspectRatio < 0.5f)
        {
            tCoefY = 0.655f;
            tCoefDY = 0.068f;
            tCoefDY1 = 0.42f;
        }
        else if (aspectRatio > 0.5f)
        {
            tCoefY = 0.69f;
            tCoefDY = 0.081f;
            tCoefDY1 = 0.51f;
        }
        else
        {
            tCoefY = 0.67f;
            tCoefDY = 0.0735f;
            tCoefDY1 = 0.456f;
        }
        
        var t11 = GameObject.Find("top_num_text_1").GetComponent<RectTransform>();
        var t12 = GameObject.Find("top_nick_text_1").GetComponent<RectTransform>();
        var t13 = GameObject.Find("top_record_text_1").GetComponent<RectTransform>();

        t11.position = new Vector3(Screen.width * tCoefX1, Screen.height * tCoefY, t11.position.z);
        t12.position = new Vector3(Screen.width * tCoefX2, Screen.height * tCoefY, t12.position.z);
        t13.position = new Vector3(Screen.width * tCoefX3, Screen.height * tCoefY, t13.position.z);
        
        var t21 = GameObject.Find("top_num_text_2").GetComponent<RectTransform>();
        var t22 = GameObject.Find("top_nick_text_2").GetComponent<RectTransform>();
        var t23 = GameObject.Find("top_record_text_2").GetComponent<RectTransform>();
        
        t21.position = new Vector3(Screen.width * tCoefX1, Screen.height * (tCoefY - tCoefDY), t21.position.z);
        t22.position = new Vector3(Screen.width * tCoefX2, Screen.height * (tCoefY - tCoefDY), t22.position.z);
        t23.position = new Vector3(Screen.width * tCoefX3, Screen.height * (tCoefY - tCoefDY), t23.position.z);
        
        var t31 = GameObject.Find("top_num_text_3").GetComponent<RectTransform>();
        var t32 = GameObject.Find("top_nick_text_3").GetComponent<RectTransform>();
        var t33 = GameObject.Find("top_record_text_3").GetComponent<RectTransform>();
        
        t31.position = new Vector3(Screen.width * tCoefX1, Screen.height * (tCoefY - 2 * tCoefDY), t31.position.z);
        t32.position = new Vector3(Screen.width * tCoefX2, Screen.height * (tCoefY - 2 * tCoefDY), t32.position.z);
        t33.position = new Vector3(Screen.width * tCoefX3, Screen.height * (tCoefY - 2 * tCoefDY), t33.position.z);
        
        var t41 = GameObject.Find("top_num_text_4").GetComponent<RectTransform>();
        var t42 = GameObject.Find("top_nick_text_4").GetComponent<RectTransform>();
        var t43 = GameObject.Find("top_record_text_4").GetComponent<RectTransform>();
        
        t41.position = new Vector3(Screen.width * tCoefX1, Screen.height * (tCoefY - 3 * tCoefDY), t41.position.z);
        t42.position = new Vector3(Screen.width * tCoefX2, Screen.height * (tCoefY - 3 * tCoefDY), t42.position.z);
        t43.position = new Vector3(Screen.width * tCoefX3, Screen.height * (tCoefY - 3 * tCoefDY), t43.position.z);
        
        var t51 = GameObject.Find("top_num_text_5").GetComponent<RectTransform>();
        var t52 = GameObject.Find("top_nick_text_5").GetComponent<RectTransform>();
        var t53 = GameObject.Find("top_record_text_5").GetComponent<RectTransform>();
        
        t51.position = new Vector3(Screen.width * tCoefX1, Screen.height * (tCoefY - 4 * tCoefDY), t51.position.z);
        t52.position = new Vector3(Screen.width * tCoefX2, Screen.height * (tCoefY - 4 * tCoefDY), t52.position.z);
        t53.position = new Vector3(Screen.width * tCoefX3, Screen.height * (tCoefY - 4 * tCoefDY), t53.position.z);
        
        var t61 = GameObject.Find("top_num_text_you").GetComponent<RectTransform>();
        var t62 = GameObject.Find("top_nick_text_you").GetComponent<RectTransform>();
        var t63 = GameObject.Find("top_record_text_you").GetComponent<RectTransform>();
        
        t61.position = new Vector3(Screen.width * tCoefX1, Screen.height * (tCoefY - tCoefDY1), t61.position.z);
        t62.position = new Vector3(Screen.width * tCoefX2, Screen.height * (tCoefY - tCoefDY1), t62.position.z);
        t63.position = new Vector3(Screen.width * tCoefX3, Screen.height * (tCoefY - tCoefDY1), t63.position.z);
        
        
        //Shop panel
        var sCoefX1 = 0.25f;
        var sCoefX2 = 0.725f;
        var sCoefY1 = 0.0f;
        var sCoefY2 = 0.0f;
        var sCoefDY = 0.0f;

        if (aspectRatio < 0.5f)
        {
            sCoefY1 = 0.625f;
            sCoefY2 = 0.384f;
            sCoefDY = 0.095f;
        }
        else if (aspectRatio > 0.5f)
        {
            sCoefY1 = 0.65f;
            sCoefY2 = 0.355f;
            sCoefDY = 0.11f;
        }
        else
        {
            sCoefY1 = 0.635f;
            sCoefY2 = 0.37f;
            sCoefDY = 0.1f;
        }
        
        var s11Top = GameObject.Find("text_value_1").GetComponent<RectTransform>();
        var s11Bottom = GameObject.Find("text_cost_1").GetComponent<RectTransform>();
        
        s11Top.position = new Vector3(Screen.width * sCoefX1, Screen.height * sCoefY1, s11Top.position.z);
        s11Bottom.position = new Vector3(Screen.width * sCoefX1, Screen.height * (sCoefY1 - sCoefDY), s11Bottom.position.z);
        
        var s12Top = GameObject.Find("text_value_2").GetComponent<RectTransform>();
        var s12Bottom = GameObject.Find("text_cost_2").GetComponent<RectTransform>();
        
        s12Top.position = new Vector3(Screen.width * sCoefX2, Screen.height * sCoefY1, s12Top.position.z);
        s12Bottom.position = new Vector3(Screen.width * sCoefX2, Screen.height * (sCoefY1 - sCoefDY), s12Bottom.position.z);
        
        var s21Top = GameObject.Find("text_value_3").GetComponent<RectTransform>();
        var s21Bottom = GameObject.Find("text_cost_3").GetComponent<RectTransform>();
        
        s21Top.position = new Vector3(Screen.width * sCoefX1, Screen.height * sCoefY2, s21Top.position.z);
        s21Bottom.position = new Vector3(Screen.width * sCoefX1, Screen.height * (sCoefY2 - sCoefDY), s21Bottom.position.z);
        
        var s22Top = GameObject.Find("text_value_4").GetComponent<RectTransform>();
        var s22Bottom = GameObject.Find("text_cost_4").GetComponent<RectTransform>();
        
        s22Top.position = new Vector3(Screen.width * sCoefX2, Screen.height * sCoefY2, s22Top.position.z);
        s22Bottom.position = new Vector3(Screen.width * sCoefX2, Screen.height * (sCoefY2 - sCoefDY), s22Bottom.position.z);
        
        
        //Counters
        var cCoefX1 = 0.2f;
        var cCoefX2 = 0.5f;
        var cCoefX3 = 0.78f;
        var cCoefY1 = 0.0f;
        var cCoefY2 = 0.0f;

        if (aspectRatio < 0.5f)
        {
            cCoefY1 = 0.84f;
            cCoefY2 = 0.79f;
        }
        else if (aspectRatio > 0.5f)
        {
            cCoefY1 = 0.86f;
            cCoefY2 = 0.81f;
        }
        else
        {
            cCoefY1 = 0.84f;
            cCoefY2 = 0.79f;
        }
        
        var text1 = GameObject.Find("steps_text").GetComponent<RectTransform>();
        text1.position = new Vector3(Screen.width * cCoefX1, Screen.height * cCoefY1, text1.position.z);
        
        var counter1 = GameObject.Find("steps_counter").GetComponent<RectTransform>();
        counter1.position = new Vector3(Screen.width * cCoefX1, Screen.height * cCoefY2, counter1.position.z);
        
        var text2 = GameObject.Find("level_text").GetComponent<RectTransform>();
        text2.position = new Vector3(Screen.width * cCoefX2, Screen.height * cCoefY1, text2.position.z);
        
        var counter2 = GameObject.Find("level_counter").GetComponent<RectTransform>();
        counter2.position = new Vector3(Screen.width * cCoefX2, Screen.height * cCoefY2, counter2.position.z);
        
        var text3 = GameObject.Find("record_text").GetComponent<RectTransform>();
        text3.position = new Vector3(Screen.width * cCoefX3, Screen.height * cCoefY1, text3.position.z);
        
        var counter3 = GameObject.Find("record_counter").GetComponent<RectTransform>();
        counter3.position = new Vector3(Screen.width * cCoefX3, Screen.height * cCoefY2, counter3.position.z);
    }

    private void setTextFonts()
    {
        setTopTextFonts();
        setCounterTextFonts();
        setShopTextFonts();
    }

    private void setTopTextFonts()
    {
        int topFont = 0;

        if (Screen.height < 1000)
        {
            topFont = 30;
        }
        else if (Screen.height > 2000)
        {
            topFont = 70;
        }
        else
        {
            topFont = 50;
        }

        var coefX = 0.6f;
        var coefY = 1.2f;

        var go11 = GameObject.Find("top_num_text_1");
        var t11 = go11.GetComponent<Text>();
        var rect11 = go11.GetComponent<RectTransform>();

        t11.fontSize = topFont;
        rect11.sizeDelta = new Vector2(t11.text.Length * t11.fontSize * coefX, t11.fontSize * coefY);
        
        var go12 = GameObject.Find("top_nick_text_1");
        var t12 = go12.GetComponent<Text>();
        var rect12 = go12.GetComponent<RectTransform>();

        t12.fontSize = topFont;
        rect12.sizeDelta = new Vector2(t12.text.Length * t12.fontSize * coefX, t12.fontSize * coefY);
        
        var go13 = GameObject.Find("top_record_text_1");
        var t13 = go13.GetComponent<Text>();
        var rect13 = go13.GetComponent<RectTransform>();

        t13.fontSize = topFont;
        rect13.sizeDelta = new Vector2(t13.text.Length * t13.fontSize * coefX, t13.fontSize * coefY);
        
        //

        var go21 = GameObject.Find("top_num_text_2");
        var t21 = go21.GetComponent<Text>();
        var rect21 = go21.GetComponent<RectTransform>();

        t21.fontSize = topFont;
        rect21.sizeDelta = new Vector2(t21.text.Length * t21.fontSize * coefX, t21.fontSize * coefY);
        
        var go22 = GameObject.Find("top_nick_text_2");
        var t22 = go22.GetComponent<Text>();
        var rect22 = go22.GetComponent<RectTransform>();

        t22.fontSize = topFont;
        rect22.sizeDelta = new Vector2(t22.text.Length * t22.fontSize * coefX, t22.fontSize * coefY);
        
        var go23 = GameObject.Find("top_record_text_2");
        var t23 = go23.GetComponent<Text>();
        var rect23 = go23.GetComponent<RectTransform>();

        t23.fontSize = topFont;
        rect23.sizeDelta = new Vector2(t23.text.Length * t23.fontSize * coefX, t23.fontSize * coefY);
        
        //
        
        var go31 = GameObject.Find("top_num_text_3");
        var t31 = go31.GetComponent<Text>();
        var rect31 = go31.GetComponent<RectTransform>();

        t31.fontSize = topFont;
        rect31.sizeDelta = new Vector2(t31.text.Length * t31.fontSize * coefX, t31.fontSize * coefY);
        
        var go32 = GameObject.Find("top_nick_text_3");
        var t32 = go32.GetComponent<Text>();
        var rect32 = go32.GetComponent<RectTransform>();

        t32.fontSize = topFont;
        rect32.sizeDelta = new Vector2(t32.text.Length * t32.fontSize * coefX, t32.fontSize * coefY);
        
        var go33 = GameObject.Find("top_record_text_3");
        var t33 = go33.GetComponent<Text>();
        var rect33 = go33.GetComponent<RectTransform>();

        t33.fontSize = topFont;
        rect33.sizeDelta = new Vector2(t33.text.Length * t33.fontSize * coefX, t33.fontSize * coefY);
        
        //
        
        var go41 = GameObject.Find("top_num_text_4");
        var t41 = go41.GetComponent<Text>();
        var rect41 = go41.GetComponent<RectTransform>();

        t41.fontSize = topFont;
        rect41.sizeDelta = new Vector2(t41.text.Length * t41.fontSize * coefX, t41.fontSize * coefY);
        
        var go42 = GameObject.Find("top_nick_text_4");
        var t42 = go42.GetComponent<Text>();
        var rect42 = go42.GetComponent<RectTransform>();

        t42.fontSize = topFont;
        rect42.sizeDelta = new Vector2(t42.text.Length * t42.fontSize * coefX, t42.fontSize * coefY);
        
        var go43 = GameObject.Find("top_record_text_4");
        var t43 = go43.GetComponent<Text>();
        var rect43 = go43.GetComponent<RectTransform>();

        t43.fontSize = topFont;
        rect43.sizeDelta = new Vector2(t43.text.Length * t43.fontSize * coefX, t43.fontSize * coefY);
        
        //
        
        var go51 = GameObject.Find("top_num_text_5");
        var t51 = go51.GetComponent<Text>();
        var rect51 = go51.GetComponent<RectTransform>();

        t51.fontSize = topFont;
        rect51.sizeDelta = new Vector2(t51.text.Length * t51.fontSize * coefX, t51.fontSize * coefY);
        
        var go52 = GameObject.Find("top_nick_text_5");
        var t52 = go52.GetComponent<Text>();
        var rect52 = go52.GetComponent<RectTransform>();

        t52.fontSize = topFont;
        rect52.sizeDelta = new Vector2(t52.text.Length * t52.fontSize * coefX, t52.fontSize * coefY);
        
        var go53 = GameObject.Find("top_record_text_5");
        var t53 = go53.GetComponent<Text>();
        var rect53 = go53.GetComponent<RectTransform>();

        t53.fontSize = topFont;
        rect53.sizeDelta = new Vector2(t53.text.Length * t53.fontSize * coefX, t53.fontSize * coefY);
        
        //
        
        var go61 = GameObject.Find("top_num_text_you");
        var t61 = go61.GetComponent<Text>();
        var rect61 = go61.GetComponent<RectTransform>();

        t61.fontSize = topFont;
        rect61.sizeDelta = new Vector2(t61.text.Length * t61.fontSize * coefX, t61.fontSize * coefY);
        
        var go62 = GameObject.Find("top_nick_text_you");
        var t62 = go62.GetComponent<Text>();
        var rect62 = go62.GetComponent<RectTransform>();

        t62.fontSize = topFont;
        rect62.sizeDelta = new Vector2(t62.text.Length * t62.fontSize * coefX, t62.fontSize * coefY);
        
        var go63 = GameObject.Find("top_record_text_you");
        var t63 = go63.GetComponent<Text>();
        var rect63 = go63.GetComponent<RectTransform>();

        t63.fontSize = topFont;
        rect63.sizeDelta = new Vector2(t63.text.Length * t63.fontSize * coefX, t63.fontSize * coefY);
    }

    private void setCounterTextFonts()
    {
        int countersMaxFont = 0;
        int countersMinFont = 0;

        if (Screen.height < 1000)
        {
            countersMaxFont = 45;
            countersMinFont = 35;
        }
        else if (Screen.height > 2000)
        {
            countersMaxFont = 110;
            countersMinFont = 60;
        }
        else
        {
            countersMaxFont = 80;
            countersMinFont = 55;
        }
        
        var coefX = 0.6f;
        var coefY = 1.2f;
        
        var go11 = GameObject.Find("steps_text");
        var t11 = go11.GetComponent<Text>();
        var rect11 = go11.GetComponent<RectTransform>();

        t11.fontSize = countersMinFont;
        rect11.sizeDelta = new Vector2(t11.text.Length * t11.fontSize * coefX, t11.fontSize * coefY);
        
        //
        
        var go21 = GameObject.Find("steps_counter");
        var t21 = go21.GetComponent<Text>();
        var rect21 = go21.GetComponent<RectTransform>();

        t21.fontSize = countersMaxFont;
        rect21.sizeDelta = new Vector2(t21.text.Length * t21.fontSize * coefX, t21.fontSize * coefY);
        
        //
        
        var go12 = GameObject.Find("level_text");
        var t12 = go12.GetComponent<Text>();
        var rect12 = go12.GetComponent<RectTransform>();

        t12.fontSize = countersMinFont;
        rect12.sizeDelta = new Vector2(t12.text.Length * t12.fontSize * coefX, t12.fontSize * coefY);
        
        //
        
        var go22 = GameObject.Find("level_counter");
        var t22 = go22.GetComponent<Text>();
        var rect22 = go22.GetComponent<RectTransform>();

        t22.fontSize = countersMaxFont;
        rect22.sizeDelta = new Vector2(t22.text.Length * t22.fontSize * coefX, t22.fontSize * coefY);
        
        //
        
        var go13 = GameObject.Find("record_text");
        var t13 = go13.GetComponent<Text>();
        var rect13 = go13.GetComponent<RectTransform>();

        t13.fontSize = countersMinFont;
        rect13.sizeDelta = new Vector2(t13.text.Length * t13.fontSize * coefX, t13.fontSize * coefY);
        
        //
        
        var go23 = GameObject.Find("record_counter");
        var t23 = go23.GetComponent<Text>();
        var rect23 = go23.GetComponent<RectTransform>();

        t23.fontSize = countersMaxFont;
        rect23.sizeDelta = new Vector2(t23.text.Length * t23.fontSize * coefX, t23.fontSize * coefY);
    }

    private void setShopTextFonts()
    {
        int shopMaxFont = 0;
        int shopMinFont = 0;

        if (Screen.height < 1000)
        {
            shopMaxFont = 35;
            shopMinFont = 20;
        }
        else if (Screen.height > 2000)
        {
            shopMaxFont = 65;
            shopMinFont = 50;
        }
        else
        {
            shopMaxFont = 45;
            shopMinFont = 30;
        }
        
        var coefX = 0.6f;
        var coefY = 1.2f;
        
        var go11t = GameObject.Find("text_value_1");
        var t11t = go11t.GetComponent<Text>();
        var rect11t = go11t.GetComponent<RectTransform>();

        t11t.fontSize = shopMaxFont;
        rect11t.sizeDelta = new Vector2(t11t.text.Length * t11t.fontSize * coefX, t11t.fontSize * coefY);
        
        //
        
        var go11b = GameObject.Find("text_cost_1");
        var t11b = go11b.GetComponent<Text>();
        var rect11b = go11b.GetComponent<RectTransform>();

        t11b.fontSize = shopMinFont;
        rect11b.sizeDelta = new Vector2(t11b.text.Length * t11b.fontSize * coefX, t11b.fontSize * coefY);
        
        //
        
        var go12t = GameObject.Find("text_value_2");
        var t12t = go12t.GetComponent<Text>();
        var rect12t = go12t.GetComponent<RectTransform>();

        t12t.fontSize = shopMaxFont;
        rect12t.sizeDelta = new Vector2(t12t.text.Length * t12t.fontSize * coefX, t12t.fontSize * coefY);
        
        //
        
        var go12b = GameObject.Find("text_cost_2");
        var t12b = go12b.GetComponent<Text>();
        var rect12b = go12b.GetComponent<RectTransform>();

        t12b.fontSize = shopMinFont;
        rect12b.sizeDelta = new Vector2(t12b.text.Length * t12b.fontSize * coefX, t12b.fontSize * coefY);
        
        //
        
        var go21t = GameObject.Find("text_value_3");
        var t21t = go21t.GetComponent<Text>();
        var rect21t = go21t.GetComponent<RectTransform>();

        t21t.fontSize = shopMaxFont;
        rect21t.sizeDelta = new Vector2(t21t.text.Length * t21t.fontSize * coefX, t21t.fontSize * coefY);
        
        //
        
        var go21b = GameObject.Find("text_cost_3");
        var t21b = go21b.GetComponent<Text>();
        var rect21b = go21b.GetComponent<RectTransform>();

        t21b.fontSize = shopMinFont;
        rect21b.sizeDelta = new Vector2(t21b.text.Length * t21b.fontSize * coefX, t21b.fontSize * coefY);
        
        //
        
        var go22t = GameObject.Find("text_value_4");
        var t22t = go22t.GetComponent<Text>();
        var rect22t = go22t.GetComponent<RectTransform>();

        t22t.fontSize = shopMaxFont;
        rect22t.sizeDelta = new Vector2(t22t.text.Length * t22t.fontSize * coefX, t22t.fontSize * coefY);
        
        //
        
        var go22b = GameObject.Find("text_cost_4");
        var t22b = go22b.GetComponent<Text>();
        var rect22b = go22b.GetComponent<RectTransform>();

        t22b.fontSize = shopMinFont;
        rect22b.sizeDelta = new Vector2(t22b.text.Length * t22b.fontSize * coefX, t22b.fontSize * coefY);
    }
}
