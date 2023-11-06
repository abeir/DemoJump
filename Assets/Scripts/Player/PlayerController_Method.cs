using Common.Helper;
using Player.Enums;
using UnityEngine;
using Motion = Player.Enums.Motion;

namespace Player
{
    public partial class PlayerController
    {
        public bool CanMove => motion.Contains(Motion.Move);
        public bool CanJump => motion.Contains(Motion.Jump);
        public bool CanDash => motion.Contains(Motion.Dash);
        public bool CanSlide => motion.Contains(Motion.Slide);
        public bool CanLedge => motion.Contains(Motion.Ledge);
        public bool CanCrouch => motion.Contains(Motion.Crouch);
        public bool CanWall => motion.Contains(Motion.Wall);
        public bool CanWallJump => motion.Contains(Motion.WallJump);

        public bool CanJumpOnGround => jumpMode.Contains(JumpMode.OnGround) && IsOnGround;

        public bool CanJumpOnSlope => jumpMode.Contains(JumpMode.OnSlope) && IsOnSlope;

        public bool CanJumpWhenFalling => jumpMode.Contains(JumpMode.WhenFalling);

        public bool CanDoubleJump => jumpMode.Contains(JumpMode.DoubleJump) && JumpCount == 1 && IsOnAir;


        public bool CheckJumpMode(JumpMode mode)
        {
            return jumpMode.Contains(mode);
        }

        
        public bool Flip()
        {
            if (!facingPositive && MoveDirection.x >= Maths.TinyNum)
            {
                facingPositive = true;
                SpriteRenderer.flipX = false;
                return true;
            }
            if (facingPositive && MoveDirection.x <= -Maths.TinyNum)
            {
                facingPositive = false;
                SpriteRenderer.flipX = true;
                return true;
            }
            return false;
        }

        public void Flip(int direction)
        {
            if (direction == 1)
            {
                facingPositive = true;
                SpriteRenderer.flipX = false;
            }
            else if (direction == -1)
            {
                facingPositive = false;
                SpriteRenderer.flipX = true;
            }
        }


        public void ResetJumpCount()
        {
            JumpCount = 0;
        }

        public void ResetJumpPressedImpulse(bool force = false)
        {
            if (force || (JumpPressedImpulse && Time.time - _jumpCacheTime > jumpCacheDuration))
            {
                JumpPressedImpulse = false;
            }
        }

        public void ResetDashPressed()
        {
            if (DashPressedImpulse && Time.time - _dashCacheTime > dashCacheDuration)
            {
                DashPressedImpulse = false;
            }
        }

        public void SetZeroFriction()
        {
            Rigidbody.sharedMaterial = zeroFriction;
        }

        public void SetDefaultFriction()
        {
            Rigidbody.sharedMaterial = defaultFriction;
        }
        
    }
}