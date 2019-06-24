using System.Collections.Generic;
using UnityEngine;
using DragonBones;

namespace dragonboneAnimationController
{
    public class DragonboneAnimationController : MonoBehaviour
    {
        private const string NORMAL_ANIMATION_GROUP = "normal";

        private const float G = -0.005f;
        private const float GROUND = 0.0f;
        private const float JUMP_SPEED = -0.2f;
        private const float NORMALIZE_MOVE_SPEED = 0.03f;
        private const float MAX_MOVE_SPEED_FRONT = NORMALIZE_MOVE_SPEED * 1.4f;

        public KeyCode left = KeyCode.A;
        public KeyCode right = KeyCode.D;
        public KeyCode jump = KeyCode.Space;
        public KeyCode down = KeyCode.S;

        private bool _isJumping = false;
        private bool _isFalling = false;
        private int _faceDir = 1;
        private int _moveDir = 0;

        private UnityArmatureComponent _armatureComponent = null;
        private DragonBones.AnimationState _walkState = null;
        private Vector2 _speed = new Vector2();


        [SerializeField]
        private string dragonboneDataName = "Cubetto";

        [SerializeField]
        private string idleState = "Idle";

        [SerializeField]
        private string jumpState = "Jump";

        [SerializeField]
        private string fallState = "Fall";

        [SerializeField]
        private string glideState = "Glide";

        [SerializeField]
        private string walkState = "Walk";

        [SerializeField]
        private string bounceState = "Bounce";

        [SerializeField]
        private string deathState = "Death";

        //
        [SerializeField]
        private UnityDragonBonesData dragonBoneData;

        [SerializeField]
        private Player player;

        void Start()
        {
            UnityFactory.factory.autoSearch = true;
            UnityFactory.factory.LoadData(dragonBoneData);

            var armatureDisplay = new GameObject(dragonboneDataName);
#if UNITY_5_6_OR_NEWER
            //armatureDisplay.AddComponent<UnityEngine.Rendering.SortingGroup>();
#endif
            _armatureComponent = UnityFactory.factory.BuildArmatureComponent(dragonboneDataName, null, null, null, armatureDisplay);

#if UNITY_5_6_OR_NEWER
            //_armatureComponent.sortingMode = SortingMode.SortByOrder;
#endif
            _armatureComponent.AddEventListener(EventObject.FADE_IN_COMPLETE, _animationEventHandler);
            _armatureComponent.AddEventListener(EventObject.FADE_OUT_COMPLETE, _animationEventHandler);

            _armatureComponent.animation.Reset();
            _armatureComponent.animation.Play(idleState);
            _armatureComponent.armature.flipX = true;

            _updateAnimation();
        }

        void Update()
        {
            if (player.velocity.x == 0)
            {
                _move(0);
            }
            else if (player.velocity.x > 0)
            {
                _move(1);
            }
            else if (player.velocity.x < 0)
            {
                _move(-1);
            }

            if (Input.GetKeyDown(jump))
            {
                _jump();
            }

            //
            _updatePosition();
        }

        private void _animationEventHandler(string type, EventObject eventObject)
        {
            switch (type)
            {
                case EventObject.FADE_IN_COMPLETE:
                    if (eventObject.animationState.name == jumpState)
                    {
                        _isFalling = true;
                        //_armatureComponent.animation.FadeIn("jump_2", -1, -1, 0, NORMAL_ANIMATION_GROUP).resetToPose = false;
                    }
                    else if (eventObject.animationState.name == fallState)
                    {
                        _updateAnimation();
                    }
                    break;
            }
        }

        private void _move(int dir)
        {
            if (_moveDir == dir)
            {
                return;
            }

            _moveDir = dir;
            _updateAnimation();
        }

        private void _jump()
        {
            if (_isJumping)
            {
                return;
            }

            _isJumping = true;
            _armatureComponent.animation.FadeIn(jumpState, -1.0f, -1, 0, NORMAL_ANIMATION_GROUP).resetToPose = false;
            _walkState = null;
        }

        private void _updateAnimation()
        {
            if (_isJumping || _isFalling)
            {
                return;
            }

            if (_moveDir == 0.0f)
            {
                _armatureComponent.animation.FadeIn(idleState, -1.0f, -1, 0, NORMAL_ANIMATION_GROUP).resetToPose = false;
                _walkState = null;
            }
            else
            {
                if (_walkState == null)
                {
                    _walkState = _armatureComponent.animation.FadeIn(walkState, -1.0f, -1, 0, NORMAL_ANIMATION_GROUP);
                    this._walkState.resetToPose = false;
                }

                //if (this._moveDir * this._faceDir > 0.0f)
                //{
                //    this._walkState.timeScale = MAX_MOVE_SPEED_FRONT / NORMALIZE_MOVE_SPEED;
                //}
                //else
                //{
                //    this._walkState.timeScale = -MAX_MOVE_SPEED_FRONT / NORMALIZE_MOVE_SPEED;
                //}
            }
        }

        private void _updatePosition()
        {
            if (player.velocity.x == 0.0f && !_isFalling)
            {
                return;
            }

            var position = this._armatureComponent.transform.localPosition;
            //var position = this.transform.localPosition;

            if (_speed.x != 0.0f)
            {
                //position.x += _speed.x * _armatureComponent.animation.timeScale;
            }

            if (_isFalling)
            {
                if (player.velocity.y < 0f)
                {
                    _armatureComponent.animation.FadeIn(fallState, -1.0f, -1, 0, NORMAL_ANIMATION_GROUP).resetToPose = false;
                }

                if (player.isOnGround)
                {
                    _isJumping = false;
                    _isFalling = false;
                    _armatureComponent.animation.FadeIn(fallState, -1.0f, -1, 0, NORMAL_ANIMATION_GROUP).resetToPose = false;
                }
            }
        }
    }
}