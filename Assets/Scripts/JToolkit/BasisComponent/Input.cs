using UnityEngine;
using System;
using System.Collections;

namespace JToolkit.BasisComponent {
    using JToolkit.Utility;

    public abstract class Input : Singleton<Input> {
        public enum _Input_Type {
            Mouse_And_Keyboard,
            Controller,
        }

        public enum _Xbox_Controller_Buttons {
            None,
            A,
            B,
            X,
            Y,
            Left_Stick,
            Right_Stick,
            View,
            Menu,
            Left_Bumper,
            Right_Bumper,
        }

        public enum Xbox_Controller_Axes {
            None,
            Left_Stick_Horizontal,
            Left_Stick_Vertical,
            Dpad_Horizontal,
            Dpad_Vertical,
            Right_stick_Horizontal,
            Right_stick_Vertical,
            Left_Trigger,
            Right_Trigger,
        }

        [Serializable]
        public class InputButton {
            public KeyCode key;
            public _Xbox_Controller_Buttons controller_buttons;
            public bool down {
                get; protected set;
            }
            public bool held {
                get; protected set;
            }
            public bool up {
                get; protected set;
            }

            [SerializeField]
            protected bool m_enabled = true;
            public bool enabled {
                get => m_enabled;
            }

            private bool m_getting_input = true;

            private bool m_after_fixed_update_down;
            private bool m_after_fixed_update_held;
            private bool m_after_fixed_update_up;
            private float m_last_click_time = 0f;
            private float m_click_time_from_last_click = 0f;

            protected static readonly EnumDictionary<_Xbox_Controller_Buttons, string> buttons_to_name = new EnumDictionary<_Xbox_Controller_Buttons, string> {
                {_Xbox_Controller_Buttons.A, "A"},
                {_Xbox_Controller_Buttons.B, "B"},
                {_Xbox_Controller_Buttons.X, "X"},
                {_Xbox_Controller_Buttons.Y, "Y"},
                {_Xbox_Controller_Buttons.Left_Stick, "Leftstick"},
                {_Xbox_Controller_Buttons.Right_Stick, "Rightstick"},
                {_Xbox_Controller_Buttons.View, "View"},
                {_Xbox_Controller_Buttons.Menu, "Menu"},
                {_Xbox_Controller_Buttons.Left_Bumper, "Left Bumper"},
                {_Xbox_Controller_Buttons.Right_Bumper, "Right Bumper"},
            };

            public InputButton ( KeyCode key, _Xbox_Controller_Buttons controller_button ) {
                this.key = key;
                this.controller_buttons = controller_button;
            }

            public void get ( bool fixedUpdate_happened, _Input_Type input_type ) {
                if ( !m_enabled ) {
                    down = false;
                    held = false;
                    up = false;
                    return;
                }

                if ( !m_getting_input ) {
                    return;
                }

                switch ( input_type ) {
                    case _Input_Type.Controller:
                        if ( fixedUpdate_happened ) {
                            down = UnityEngine.Input.GetButtonDown ( buttons_to_name[controller_buttons] );
                            held = UnityEngine.Input.GetButton ( buttons_to_name[controller_buttons] );
                            up = UnityEngine.Input.GetButtonUp ( buttons_to_name[controller_buttons] );

                            m_after_fixed_update_down = down;
                            m_after_fixed_update_held = held;
                            m_after_fixed_update_up = up;
                        } else {
                            down = UnityEngine.Input.GetButtonDown ( buttons_to_name[controller_buttons] ) || m_after_fixed_update_down;
                            held = UnityEngine.Input.GetButton ( buttons_to_name[controller_buttons] ) || m_after_fixed_update_held;
                            up = UnityEngine.Input.GetButtonUp ( buttons_to_name[controller_buttons] ) || m_after_fixed_update_up;

                            m_after_fixed_update_down |= down;
                            m_after_fixed_update_held |= held;
                            m_after_fixed_update_up |= up;
                        }
                        break;
                    case _Input_Type.Mouse_And_Keyboard:
                        if ( fixedUpdate_happened ) {
                            down = UnityEngine.Input.GetKeyDown ( key );
                            held = UnityEngine.Input.GetKey ( key );
                            up = UnityEngine.Input.GetKeyUp ( key );

                            m_after_fixed_update_down = down;
                            m_after_fixed_update_held = held;
                            m_after_fixed_update_up = up;
                        } else {
                            down = UnityEngine.Input.GetKeyDown ( key ) || m_after_fixed_update_down;
                            held = UnityEngine.Input.GetKey ( key ) || m_after_fixed_update_held;
                            up = UnityEngine.Input.GetKeyUp ( key ) || m_after_fixed_update_up;

                            m_after_fixed_update_down |= down;
                            m_after_fixed_update_held |= held;
                            m_after_fixed_update_up |= up;
                        }
                        break;
                }

                if ( m_after_fixed_update_down ) {
                    m_click_time_from_last_click = Time.time - m_last_click_time;
                }

                if ( m_after_fixed_update_up ) {
                    m_last_click_time = Time.time;
                }
            }

            public void set_active ( bool _value ) {
                m_enabled = _value;
            }

            public void gain_control ( ) {
                m_getting_input = true;
                m_click_time_from_last_click = 0.0f;
                m_last_click_time = 0.0f;
            }

            public bool double_click ( float _time ) {
                return held && m_click_time_from_last_click > 0f && m_click_time_from_last_click <= _time;
            }

            public IEnumerator release_control ( bool reset_values ) {
                m_getting_input = false;
                m_click_time_from_last_click = 0.0f;
                m_last_click_time = 0.0f;

                if ( !reset_values ) {
                    yield break;
                }

                if ( down ) {
                    up = true;
                }
                down = false;
                held = false;

                m_after_fixed_update_down = false;
                m_after_fixed_update_held = false;
                m_after_fixed_update_up = false;

                yield return null;

                up = false;
            }
        }

        [Serializable]
        public class InputAxis {
            public KeyCode positive;
            public KeyCode negative;
            public Xbox_Controller_Axes controller_axis;
            public float value {
                get; protected set;
            }
            public int count {
                get; protected set;
            }
            public bool receiving_input {
                get; protected set;
            }

            protected bool m_enabled = true;
            public bool enabled {
                get => m_enabled;
            }
            protected bool m_getting_input = true;

            private int m_input_count = 0;
            private float m_last_value = 0.0f;
            private float m_last_input = 0.0f;
            private float m_input_time = 0.0f;

            protected readonly static EnumDictionary<Xbox_Controller_Axes, string> k_axis_to_name = new EnumDictionary<Xbox_Controller_Axes, string> {
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Leftstick Horizontal"},
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Leftstick Vertical"},
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Dpad Horizontal"},
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Dpad Vertical"},
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Rightstick Horizontal"},
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Rightstick Vertical"},
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Left Trigger"},
                {Xbox_Controller_Axes.Left_Stick_Horizontal, "Right Trigger"},
            };

            public InputAxis ( KeyCode positive, KeyCode negative, Xbox_Controller_Axes controller_axis ) {
                this.positive = positive;
                this.negative = negative;
                this.controller_axis = controller_axis;
            }

            public void get ( _Input_Type input_type ) {
                if ( !m_enabled ) {
                    value = 0f;
                    return;
                }

                if ( !m_getting_input ) {
                    return;
                }

                bool positive_held = false;
                bool negative_held = false;


                switch ( input_type ) {
                    case _Input_Type.Controller:
                        float value = UnityEngine.Input.GetAxisRaw ( k_axis_to_name[controller_axis] );
                        positive_held = value > Single.Epsilon;
                        negative_held = value < -Single.Epsilon;
                        break;
                    case _Input_Type.Mouse_And_Keyboard:
                        positive_held = UnityEngine.Input.GetKey ( positive );
                        negative_held = UnityEngine.Input.GetKey ( negative );
                        break;
                }

                if ( positive_held == negative_held ) {
                    value = 0f;
                } else if ( positive_held ) {
                    value = 1f;
                } else {
                    value = -1f;
                }

                receiving_input = positive_held || negative_held;

                if ( receiving_input && !(positive_held == negative_held) && m_last_value == 0f ) {
                    m_input_time = 0.33f;
                    m_input_count++;

                    if ( m_input_count > 0 && m_last_input != value ) {
                        m_input_count = 1;
                    }

                    count = m_input_count;
                    m_last_input = value;
                } else if ( !receiving_input || positive_held == negative_held ) {
                    count = 0;
                }

                if ( m_input_time > 0f ) {
                    m_input_time -= Time.deltaTime;
                } else {
                    m_input_time = 0f;
                    m_input_count = 0;
                    m_last_input = 0f;
                }

                m_last_value = value;
            }

            public void set_active ( bool value ) {
                m_enabled = value;
            }

            public void gain_control ( ) {
                m_getting_input = true;
            }

            public void release_control ( bool reset_values ) {
                m_getting_input = false;
                if ( reset_values ) {
                    value = 0f;
                    receiving_input = false;
                }
            }
        }

        public _Input_Type input_type = _Input_Type.Mouse_And_Keyboard;
        private bool fixed_update_happened;

        protected abstract void get_inpts ( bool fixed_update_happened );
        public abstract void gain_control ( );
        public abstract void release_control ( bool reset_values = true );

        protected void gain_control ( InputButton input_button ) {
            input_button.gain_control ( );
        }

        protected void gain_control ( InputAxis input_axis ) {
            input_axis.gain_control ( );
        }

        protected void release_control ( InputButton input_button, bool reset_values ) {
            StartCoroutine ( input_button.release_control ( reset_values ) );
        }

        protected void release_control ( InputAxis _input_axis, bool _reset_values ) {
            _input_axis.release_control ( _reset_values );
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        protected virtual void Update ( ) {
            get_inpts ( fixed_update_happened || Mathf.Approximately ( Time.timeScale, 0 ) );

            fixed_update_happened = false;
        }


        protected virtual void FixedUpdate ( ) {
            fixed_update_happened = true;
        }
    }
}