using Common.Helper;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        
        
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