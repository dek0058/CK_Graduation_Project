using UnityEngine;

namespace Game.Animation {
    using Unit;

    public class AnimatorBroadcast : SceneLinkedSMB<UUnit> {

        public float exit_time = 1.0f;

        public override void OnSLStateNoTransitionUpdate ( Animator _animator, AnimatorStateInfo _state_info, int layer_index ) {
            if ( _state_info.normalizedTime >= exit_time ) {
                mono_behaviour.action_doing = false;
            }
        }
    }
}