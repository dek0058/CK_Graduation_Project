#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Game.Management;
using JToolkit.Utility;

public class MeunButtonEvent : MonoBehaviour {

    public enum MenuType {
        Main,
        Option,
        Option_Control,
        Option_Audio,
    }

    private EnumDictionary<MenuType, string> menu_list = new EnumDictionary<MenuType, string> {
        { MenuType.Main,            "Canvas_Main"               },
        { MenuType.Option,          "Canvas_Option"             },
        { MenuType.Option_Control,  "Canvas_Option_Control"     },
        { MenuType.Option_Audio,    "Canvas_Option_Audio"       },
    };

    private Dictionary<string, GameObject> menu_objs = new Dictionary<string, GameObject> ( );
    


    public void button_gamestart ( ) {
        TransitionManager.instance.load_scene ( TransitionManager.SceneType.Game_StageJoy );
    }


    public void button_transition ( string typename ) {
        foreach ( var name in menu_objs.Keys ) {
            if ( name.Equals ( typename ) ) {
                menu_objs[name].SetActive ( true );
            } else {
                menu_objs[name].SetActive ( false );
            }
        }
    }



    public void button_quit ( ) {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }


    /// <summary>
    /// MenuButtonEvent를 검증합니다.
    /// </summary>
    private void confirm ( ) {

        if ( !menu_objs .ContainsKey(menu_list[MenuType.Main])) {
            GameObject obj = GameObject.Find ( menu_list[MenuType.Main] );
            if( obj  != null) {
                menu_objs.Add ( menu_list[MenuType.Main], obj );
            } else {
                Debug.Log ( "경고 : Main Canvas Object가 없습니다." );
            }
        }

        if ( !menu_objs.ContainsKey ( menu_list[MenuType.Option] ) ) {
            GameObject obj = GameObject.Find ( menu_list[MenuType.Option] );
            if ( obj != null ) {
                menu_objs.Add ( menu_list[MenuType.Option], obj );
            } else {
                Debug.Log ( "경고 : Option Canvas Object가 없습니다." );
            }
        }

        if ( !menu_objs.ContainsKey ( menu_list[MenuType.Option_Control] ) ) {
            GameObject obj = GameObject.Find ( menu_list[MenuType.Option_Control] );
            if ( obj != null ) {
                menu_objs.Add ( menu_list[MenuType.Option_Control], obj );
            } else {
                Debug.Log ( menu_list[MenuType.Option_Control] );
                Debug.Log ( "경고 : Option Control Canvas Object가 없습니다." );
            }
        }

        if ( !menu_objs.ContainsKey ( menu_list[MenuType.Option_Audio] ) ) {
            GameObject obj = GameObject.Find ( menu_list[MenuType.Option_Audio] );
            if ( obj != null ) {
                menu_objs.Add ( menu_list[MenuType.Option_Audio], obj );
            } else {
                Debug.Log ( "경고 : Option Audio Canvas Object가 없습니다." );
            }
        }


        // 맨처음 씬
        button_transition ( menu_list[MenuType.Main] );
    }

    ////////////////////////////////////////////////////////////////////////////
    ///                               Unity                                  ///
    ////////////////////////////////////////////////////////////////////////////

    private void Awake ( ) {
        confirm ( );
    }
}
