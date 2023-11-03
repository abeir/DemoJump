using Common.Helper;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {

        public bool CanJumpOnGround => (jumpMode & JumpMode.OnGround) > 0 && IsOnGround;

        public bool CanJumpOnSlope => (jumpMode & JumpMode.OnSlope) > 0 && IsOnSlope;

        public bool CanJumpWhenFalling => (jumpMode & JumpMode.WhenFalling) > 0;

        public bool CanDoubleJump => (jumpMode & JumpMode.DoubleJump) > 0 && JumpCount == 1 && IsOnAir;


        public bool CheckJumpMode(JumpMode mode)
        {
            return (jumpMode & mode) > 0;
        }
        
        
        public void Flip()
        {
            if (!facingPositive && MoveDirection.x >= Maths.TinyNum)
            {
                facingPositive = true;
                SpriteRenderer.flipX = false;
            }
            else if (facingPositive && MoveDirection.x <= -Maths.TinyNum)
            {
                facingPositive = false;
                SpriteRenderer.flipX = true;
            }
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