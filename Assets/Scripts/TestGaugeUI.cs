using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JToolkit.Utility;

public class TestGaugeUI : Singleton<TestGaugeUI>
{
    public Transform target;

    public RectTransform gauge_target;



    private void Update ( ) {
        if(target == null) {
            return;
        }

        Vector2 screen_point = Camera.main.WorldToScreenPoint ( target.position );
        Vector2 to_ui_point = screen_point - new Vector2 ( Screen.currentResolution.width / 2, Screen.currentResolution.height / 2 );
        gauge_target.anchoredPosition = to_ui_point;

    }

}
