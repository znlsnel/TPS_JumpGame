![Typing SVG](https://readme-typing-svg.demolab.com?font=Fira+Code&size=50&pause=1000&width=435&height=70&lines=JUMP!+JUMP!)
---
# 🛠️ Description
- **프로젝트 소개** <br>
  해당 프로젝트는 TPS 플랫폼 게임입니다. <br>
  여러 아이템과 NPC 시스템, 다양한 플랫폼 등 플랫폼 게임의 다양한 기능들을 담았습니다. <br>
  작업간에 코드 재사용성을 높이고 유지보수가 용이한 쪽으로 작업해보는 것을 목표로 개발했습니다.<br>
<br>

- **개발 기간** : 2025.03.05 - 2025.03.11
- **사용 기술** <br>
-언어 : C#<br>
-엔진 : Unity Engine <br>
-개발 환경 : Windows 11 <br>
<br>

---

# 📼 플레이 영상
[![시연 영상](https://github.com/user-attachments/assets/8aa7e540-2031-4566-8f29-03370f165eb8)](https://www.youtube.com/watch?v=WddA5sAjhDg&feature=youtu.be) 


<br><br>
---
 
# 🫀 핵심 기능 

<details>
  <summary>카메라 시스템</summary>
  
  ## 📷 카메라 시스템 [🔗 Camera Controller](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Entity/CameraController.cs)
  <img src="https://github.com/user-attachments/assets/a3585003-bf92-4f98-8c19-0be4dffebb08" alt="카메라 무빙" width="500px"> <br>
- **3인칭 카메라**<br>
  3인칭 카메라를 구현했습니다. 카메라가 플레이어 주변을 회전하도록 구현하였고, <br>
  마우스 휠을 입력받은 후, 카메라 거리를 조절할 수 있도록 했습니다. <br>
  카메라와 플레이어 사이에 물체가 있을 때, Raycast를 통해 플레이어가 항상 보이게끔 하였습니다.
  <br><br>
  
```csharp
private void LateUpdate()
{
  if (GameManager.Instance.IsGameOver)
  { 
    LookAtTarget(); 
    return; 
  }

  MoveCamera();
  SetCameraDist();
}
```
- 플레이어가 먼저 이동 후, 카메라가 이동할 수 있게 LateUpdate에서 카메라의 이동로직을 실행했습니다.
<br><br>

```csharp
void MoveCamera()
{
  transform.position = target.position;

  Vector2 mouseDelta = mouseDir * sensitivity * Time.deltaTime;

  // 좌우 회전 (Y축 회전)
  transform.Rotate(Vector3.up * mouseDelta.x);
  // 상하 회전 (X축 회전) 
  rotationX -= mouseDelta.y;
  rotationX = Mathf.Clamp(rotationX, minXRot, maxXRot); // 카메라가 뒤집히지 않도록 제한

  // Quaternion을 사용하여 회전 적용
  transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y, 0f);
  mouseDir = Vector2.zero;
}
```
- 카메라의 위치를 target의 위치로 옮기고, 마우스 입력값에 따라 회전을 실행합니다.
<br><br>

```csharp
void SetCameraDist() 
{
  float dist = cameraDist;

  // 몸통의 중간에서 부터 시작
  Vector3 startPos = transform.position + Vector3.up * 0.3f;
  Vector3 dir = (Camera.main.transform.position - startPos).normalized;

  // 충돌을 했다면 카메라 거리 조절
  Ray ray = new Ray(startPos + Vector3.up * 0.2f, dir);   
  RaycastHit hit;
  if (Physics.Raycast(ray, out hit, dist, hitLayer))
    dist = (hit.point - startPos).magnitude; 

  // 혹시 모를 예외 상황을 위해 dist와 cameraDist중 최소값을 넣어줌
  Camera.main.transform.localPosition = cameraDir * Mathf.Min(dist, cameraDist);
}  
```
- Raycast를 통해 카메라와 플레이어 사이에 물체가 있는지 감지하고, 해당 거리만큼 카메라를 조정합니다.


<br><br>
    
</details>

<details>
  <summary>캐릭터 로직</summary>
  
  ## 🕹️ 캐릭터 로직 [🔗 Player Controller Link](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Entity/PlayerController.cs) 
<img src="https://github.com/user-attachments/assets/9dd95c67-0407-4fa4-b6c0-0afe0638cbdb" alt="이동" width="500px"> <br>
<img src="https://github.com/user-attachments/assets/90d9e4b6-ed92-4e10-8dd8-3172315679c8" alt="점프" width="500px"> <br>
- 캐릭터 로직 <br>
``` csharp
void Move(Vector2 dir)
{
  Vector3 inputDir = new Vector3(dir.x, 0, dir.y);
  float cameraYaw = Camera.main.transform.eulerAngles.y;

  Quaternion yawRotation = Quaternion.Euler(0, cameraYaw, 0);
  Vector3 rotatedInputDir = yawRotation * inputDir; 

  if (rotatedInputDir != Vector3.zero)
  {
    Quaternion inputRotation = Quaternion.LookRotation(rotatedInputDir);
    targetRot = inputRotation.eulerAngles; // 최종 회전 각도
  } 
  Vector3 direction = rotatedInputDir * statHandler.MoveSpeed;
  if (knockbackDuration > 0.0f)
  {
    direction *= 0.2f; 
    direction += knockback;
  }

  SetVelocity(direction); 
}
```
- 캐릭터가 카메라의 Yaw값을 기준으로 이동하도록 했습니다. <br>
  왼쪽, 오른쪽, 뒤로 가는 키를 입력시, 그만큼 회전을 추가했습니다. <br>
<br><br>

```csharp
void Rotate(Vector3 rot)
{
  if (moveDir.magnitude <= 0f) 
    return;

  Quaternion targetRotation = Quaternion.Euler(rot);
  float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
  float t = Mathf.Clamp01((rotSpeed * Time.deltaTime) / angleDifference);
  transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
}
```
- 캐릭터 회전시 한틱에 회전하면 부자연스럽다고 느꼈습니다. 이에 targetRot 변수에 회전값을 넣어놓고 <br>
  서서히 회전하도록 구현했습니다.
  <br><br>

``` csharp
bool IsGrounded()
{
  Ray[] rays = new Ray[4]
  {
    new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
    new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
    new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
    new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
  };

  for (int i = 0; i < rays.Length; i++)
  {
    if (Physics.Raycast(rays[i], 1f, groundLayerMask))
      return true;
  }
  return false;
}
```
- 캐릭터 점프는 Rigidbody의 AddForce 기능을 통해 간단하게 구현했습니다.
- 현재 땅바닥에 있는지 체크한 후, 바닥에 있는 경우에만 점프할 수 있도록 했습니다.
<br><br><br><br>

<img src="https://github.com/user-attachments/assets/6e63daaf-14eb-4345-941e-95dd1927d0db" alt="벽타기" width="500px"> <br> 
- ClimbHandler 클래스를 통해 벽타기 기능을 구현했습니다. [🔗 ClimbHandler Link](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Handler/ClimbHandler.cs)
``` csharp
void ClimbCheck()
{
  if (!isJump || !isMove || climbTargetPos != null)
    return;

  for (int i = 0; i < 4; i ++) 
  { 
    Vector3 yOffset = new Vector3(0, -0.5f * i, 0);
    Ray ray = new Ray(rayCastPoint.position + yOffset, gameObject.transform.forward);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, 1.5f, climbLayer)) 
    {
      BoxCollider bc = hit.collider as BoxCollider;
      if (bc != null)
      {
        Vector3 targetPos = hit.point;
        targetPos.y = hit.collider.gameObject.transform.position.y + bc.center.y + (bc.size.y * bc.transform.localScale.y) / 2;

        bool isForward = Vector3.Dot(rigid.velocity, (targetPos - transform.position).normalized) > -0.5f;

        Debug.Log((hit.point.y + -0.5f * i) - targetPos.y);
        if (isForward && Mathf.Abs((rayCastPoint.position.y) - targetPos.y) < 1.3f)
        {  
          targetPos += (transform.forward * 0.2f);
          StartClimb(hit.collider.gameObject, targetPos);
          break;
        }
      }
    }
  }
}
```
- ClimbCheck 함수를 통해 벽타기가 가능한지 체크를 했습니다. <br>
  머리에서부터 아래로 4개의 Raycast를 발사하여 충돌을 체크했습니다. <br>
  이후 충돌 대상이 BoxCollider를 가지고 있으며, BoxCollider의 위쪽 부분에 충돌했다면 벽타기 기능을 수행하도록 설계했습니다.
<br><br>

``` csharp
void Climb(Transform climbPos)
{
  if (climbPos == null)
    return;

  //Vector3 dir = climbPos.Value - handTf.position;
  Vector3 dir = climbPos.position - transform.position; 
    
  float dist = dir.magnitude;  
  if (dist > 0.1f)
    dir = dir * dist * Time.fixedDeltaTime * 3.0f;

  rigid.MovePosition(transform.position + dir); 
} 
```
- 업데이트 함수에서 위의 Climb 함수를 호출하여 위로 이동하도록 했습니다. <br>
  처음 구현할 때에는 오른손이 항상 climbPos에 위치하도록 설계를 했지만 <br>
  캐릭터가 끝까지 오르지 않는 문제가 발생했고, 마지막에 위치값을 보정해주는 것도 부자연스러웠습니다. <br>
  결론적으로는 캐릭터의 위치가 서서히 clibPos로 이동하도록 하여 최대한 자연스럽게 오르도록 구현했습니다.
  <br><br><br><br>

  
<img src="https://github.com/user-attachments/assets/5943f507-b4a1-4a26-9621-47ec43830bc2" alt="이미지" width="800px"> <br> 
- 캐릭터 애니메이션의 경우 Animation Handler를 통해 애니메이션을 실행했습니다. [🔗 AnimationHandler Link](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Handler/AnimationHandler.cs) <br>
``` csharp
public void Move(Vector3 moveDir) => animator.SetBool(IsMoving, moveDir.magnitude > 0.5f); 
public void Jump() => animator.SetTrigger(IsJumping);
public void Landing() => animator.SetBool(IsInAir, false);  
public void Falling() => animator.SetBool(IsInAir, true);
public void OnClimb() => animator.SetTrigger(Climb);
public void OnDie(bool active)
{
  animator.SetBool(IsAlive, !active); 
  if (active)  
    animator.SetTrigger(IsDie);  
}
```
- 움직일때, 점프할 때, 착지할 때 등 각각의 상황에 함수들이 호출되도록 하였고 <br>
  애니메이션 관련 로직들은 해당 클래스에서만 실행되도록 설계했습니다.
  <br><br>
</details>

<details>
  <summary>상호작용</summary>
  
  ## 🤝 상호작용 [🔗 Interaction Handler](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Handler/InteractionHandler.cs)
<img src="https://github.com/user-attachments/assets/724601c5-d8c8-47ea-861e-567a6bab121a" alt="벽타기" width="500px"> <br>
``` csharp
void Find()
  {
  Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * xOffset, Screen.height * yOffset, 0));
  RaycastHit hit;
   
  float dist = interactionDistance + (Camera.main.transform.position - transform.position).magnitude;
  bool found = false;
  if (Physics.Raycast(ray, out hit, dist, layer)) 
  {
    var obj = hit.collider.gameObject.GetComponent<IInteractableObject>();
    if (obj != selectObject)
    {
      obj.ShowInfo();
      selectObject = obj;
    }
    found = obj != null;
  }

  if (!found && selectObject != null)
  {
    interactionUI.Init();
    selectObject = null;
  }
} 
```
- InvokeRepeating함수를 통해 0.1초에 한번씩 위의 Find 함수가 호출 되도록 했습니다. <br>
  인스펙터에서 설정한 에임의 위치 ( y, xOffset )을 이용해 Raycast를 하고, IInteractableObject 인터페이스를 상속받은 오브젝트를 찾습니다 <br>
  이후, 인터페이스의 ShowInfo 함수를 통해 정보 UI가 표시되도록 하였으며, <br>
  상호작용을 위해 selectObject 라는 이름으로 오브젝트를 저장했습니다.
<br><br>

```csharp
void InteractionInput(InputAction.CallbackContext context)
{
  if (selectObject == null)
    return;

  selectObject.Interaction(gameObject); 
}
```
- 상호작용키를 입력받을 때, selectObject가 존재한다면 해당 인터페이스의 Interaction 함수를 호출하여 상호작용을 구현했습니다.
<br><br>

 [🔗 Item](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Entity/Item.cs)
```csharp
public void Interaction(GameObject player)
{
  player.GetComponent<PlayerDataHandler>()?.PickupItem(this);
}
```
- Item 클래스의 경우에는 PlayerDataHandler의 PickupItem 함수를 호출하여 아이템이 실행되도록 하였습니다.
<br><br>

 [🔗 Npc Controller](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Entity/NpcController.cs)
```csharp 
public void Interaction(GameObject player)
{
  UIHandler.Instance.DialogUI.OpenUI(npcName, dialog, () => CheckCoin());
}
```
- Npc Controller의 경우 대화창이 켜지는 로직이 실행되도록 했습니다.
<br><br>

  <br><br>
</details>

<details>
  <summary>아이템 소개</summary>
  
  ## 📗 아이템
  ![image](https://github.com/user-attachments/assets/96e92409-fdcd-4066-9f90-afa4ed6f128d)
- Scriptable Object를 통해 아이템의 데이터를 설계했습니다. <br>

```csharp
public enum EItemType
{
	Equipable,
	Consumable,
}

public enum EEquipType
{
	Cloak,
	Body,
	Head,
	Hair,
}
```
- enum을 통해 아이템의 타입을 선택할 수 있게 하였습니다. <br>
  장착형 아이템의 경우 어디에 장착할건지를 Type으로 결정하게 됩니다. <br>
<br><br>

[🔗 Equip Handler](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Handler/EquipHandler.cs)
```csharp
private Dictionary<EEquipType, Transform> equipTf = new Dictionary<EEquipType, Transform>();
private Dictionary<EEquipType, GameObject> equipItems = new Dictionary<EEquipType, GameObject>();
private Dictionary<EEquipType, Action> onUnEquip = new Dictionary<EEquipType, Action>();

public void EquipItem(Item item)
{
  EEquipType type = item.data.equipType;
  Transform ts = equipTf[type];

  GameObject nextItem = item.gameObject;
  GameObject curItem = equipItems[type];

  // 현재 장착중인 아이템 장착 해제
  if (curItem != null && curItem.TryGetComponent(out Item myItem))
  {
    curItem.transform.SetParent(null, false);
    curItem.transform.position = transform.position + transform.forward * 0.3f;
    myItem.data.onUnequip?.Invoke();
    curItem.gameObject.GetComponent<Item>().SetActiveItem(true);
  }
  else
    Destroy(curItem); 
   
  // 새로운 아이템 장착
  nextItem.gameObject.GetComponent<Item>().SetActiveItem(false);
  nextItem.transform.SetParent(ts, false);
  nextItem.transform.localPosition = Vector3.zero;
  equipItems[type] = nextItem; 
}
```
- 아이템의 장착은 Equip Handler에서 담당하도록 설계했습니다. <br>
  장착 아이템을 해당 클래스의 EquipItem 함수를 통해 보내면 위의 Dictionary를 통해서 장착 로직을 수행하게 됩니다 <br>
  우선 equipTf로 장착할 위치를 찾고, equipItems를 통해 해당 위치에 장착중인 아이템을 찾습니다. <br>
  이미 장착중인 아이템은 장착 해제를 하게되는데 이때, 아이템의 수치(스피드, 점프력 등)을 빼주는 함수를 onUnEquip을 통해 해결합니다 <br>
<br><br>

  <br><br>
</details>


<details>
  <summary>Game UI</summary>
  
  ## 💬 Game UI
  
  <br><br>
</details>

<details>
  <summary>플랫폼</summary>
  
  ## 🟫 플랫폼 [🔗 Script Link](https://github.com/znlsnel/TPS_JumpGame/tree/main/Assets/9.%20Scripts/Entity/Platform)
<img src="https://github.com/user-attachments/assets/34e0f688-4009-4010-a5ac-fddfad17e9ed" alt="플랫폼" width="500px"> <br>
<img src="https://github.com/user-attachments/assets/cd576d86-0eb1-4eca-a639-d770c19a49b3" alt="플랫폼 루프" width="500px"> <br>

  <br><br>
</details>


<details>
  <summary>트랩</summary>
  
  ## 🎲 트랩 [🔗 Script Link](https://github.com/znlsnel/TPS_JumpGame/tree/main/Assets/9.%20Scripts/Entity/Trap)
<img src="https://github.com/user-attachments/assets/f242b020-7d72-4f87-87f1-844118848906" alt="트랩" width="500px"> <br>

  <br><br>
</details>
