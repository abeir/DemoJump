# 约定


## 变量命名

* 私有成员变量并且不暴露给 Inspector 的以 `_` 开头
```csharp
private GroundDetector _groundDetector;
```

* 私有成员变量并且需要暴露给 Inspector 的以小写字母开头
```csharp
private Transform playerTransform;
```

* 公共成员变量以小写字母开头
```csharp
public bool debug = false; 
```

* 私有属性以大写字母开头
```csharp
private bool Test { get; set; }
```

* 暴露给 Inspector 的变量若需要提供给其他类调用可以声明为公共成员变量，否则应该声明为私有成员变量
```csharp
[SerializeField]
public float speed = 250;
```

* 非暴露给 Inspector 的并且需要提供给外部调用的成员变量应该设置为属性；若外部修改属性，应该将其 setter 设置为私有
```csharp
public Animator UnarmedAnimator { get; private set; }
```

* 所有暴露给 Inspector 的变量需要显示添加 `[SerializeField]`
```csharp
[SerializeField]
public float jumpForce = 850; 

[SerializeField]
private bool slopeDetectEnable = true;
```

## 注释

### HACK

使用特殊的方式修复BUG时，需要在注释中添加 [HACK]

示例：
```csharp
/*
 * [HACK]
 * 此处修复因快速按以下按键导致的跳跃时变为了Idle状态
 * 按下移动 -> 按住跳跃 -> 松开移动
 */
if (pre.ID == (int)PlayerStateID.Jump)
{
    return PlayerController.PlayerDetector.IsOnGround && Mathf.Abs(PlayerController.Rigidbody.velocity.y) <= Mathf.Epsilon;
}
```
