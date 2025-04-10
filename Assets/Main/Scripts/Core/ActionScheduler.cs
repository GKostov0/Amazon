using UnityEngine;

namespace AMAZON.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction _currentAction;

        public void StartAction(IAction action)
        {
            if (_currentAction == action) return;

            if (_currentAction != null)
            {
                _currentAction.Cancel();
                print($"Canceling {_currentAction}");
            }

            _currentAction = action;
        }

        public void CancelCurrentAction() => StartAction(null);
    }
}