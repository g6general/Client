using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPositions : MonoBehaviour
{
    public Camera mCamera;
    
    void Start()
    {
        //Top panel
        var tCoefX1 = 0.16f;
        var tCoefX2 = 0.47f;
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
        else
        {
            tCoefY = 0.69f;
            tCoefDY = 0.081f;
            tCoefDY1 = 0.51f;
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
        else
        {
            sCoefY1 = 0.65f;
            sCoefY2 = 0.355f;
            sCoefDY = 0.11f;
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
        var cCoefX1 = 0.24f;
        var cCoefX2 = 0.5f;
        var cCoefX3 = 0.78f;
        var cCoefY1 = 0.0f;
        var cCoefY2 = 0.0f;

        if (aspectRatio < 0.5f)
        {
            cCoefY1 = 0.84f;
            cCoefY2 = 0.79f;
        }
        else
        {
            cCoefY1 = 0.86f;
            cCoefY2 = 0.81f;
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
}
