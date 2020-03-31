using UnityEngine;

namespace Game.Animation {
    using Game.Item.Equipment;
    public class CypressAttackSMB : SceneLinkedSMB<WCypressItem> {
        public override void OnSLStateExit ( Animator _animator, AnimatorStateInfo _state_info, int layer_index ) {
            mono_behaviour.smb_attack = false;
        }
    }
}
