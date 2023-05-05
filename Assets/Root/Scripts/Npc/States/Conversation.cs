// Conversation.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Conversation : PlayerInRange
    {
        private const float BehaviourLimit = 1f;
        private const float BehaviourLimitTolerance = .1f;

        private readonly Vector2 _behaviourDevelopmentRange =
            new(-BehaviourLimit + BehaviourLimitTolerance,
                BehaviourLimit - BehaviourLimitTolerance);

        private Vector2 _currentBehaviourOrientation;

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out ConversationData data)) return;
            base.OnEnterState(stateManager, data.PlayerManager.ToPassableData()); // for PlayerInRange.cs
        }

        internal void OnConversationResponse(InputScore inputScore)
        {
            _currentBehaviourOrientation += new Vector2(inputScore.positivity,
                inputScore.friendliness);
            DecideResponseBehaviour(_currentBehaviourOrientation);
        }

        private void DecideResponseBehaviour(Vector2 orientation)
        {
            bool positive;
            bool friendly;

            if (!orientation.x.IsBetween(_behaviourDevelopmentRange) ||
                !orientation.y.IsBetween(_behaviourDevelopmentRange))
            {
                // is developed behaviour
                positive = orientation.x > 0;
                friendly = orientation.y > 0;
            }
            else
                // is not developed behaviour
                return;


            if (positive & friendly) MyOwner.SetState<FriendlyChase>(); // player is best friend
            if (positive & !friendly) MyOwner.SetState<Ignore>(); // player is helpful stranger
            if (!positive & friendly) MyOwner.SetState<GoHome>(); // player is drunk friend
            if (!positive & !friendly) MyOwner.SetState<HostileChase>(); // player is enemy
        }
    }
}