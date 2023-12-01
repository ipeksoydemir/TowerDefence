namespace Tools.HSM
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IStateMachine
    {
        void EnterState();
        void ChangeState(int newState);
        void UpdateState();
        void ExitState();
    }

}