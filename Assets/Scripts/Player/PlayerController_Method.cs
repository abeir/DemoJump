using Common.Helper;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        
        
        public bool Flip()
        {
            if (!facingPositive && MoveDirection.x >= Maths.TinyNum)
            {
                facingPositive = true;
                SpriteRenderer.flipX = false;
                return true;
            }
            else if (facingPositive && MoveDirection.x <= -Maths.TinyNum)
            {
                facingPositive = false;
                SpriteRenderer.flipX = true;
                return true;
            }
            return false;
        }

        public void ResetJumpCount()
        {
            JumpCount = 0;
        }

        public void ResetJumpPressed()
        {
            if (JumpPressed && Time.time - _jumpCacheTime > jumpCacheDuration)
            {
                JumpPressed = false;
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