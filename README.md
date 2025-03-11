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
  
  ## 📷 카메라 시스템 [🔗 Script Link](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Entity/CameraController.cs)
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
  
  ## 🕹️ 캐릭터 로직 [🔗 Script Link](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Entity/PlayerController.cs)
<img src="https://github.com/user-attachments/assets/9dd95c67-0407-4fa4-b6c0-0afe0638cbdb" alt="이동" width="500px"> <br>
<img src="https://github.com/user-attachments/assets/90d9e4b6-ed92-4e10-8dd8-3172315679c8" alt="점프" width="500px"> <br>
<img src="https://github.com/user-attachments/assets/6e63daaf-14eb-4345-941e-95dd1927d0db" alt="벽타기" width="500px"> <br>


  
  <br><br>
</details>


<details>
  <summary>상호작용</summary>
  
  ## 🤝 상호작용 [🔗 Script Link](https://github.com/znlsnel/TPS_JumpGame/blob/main/Assets/9.%20Scripts/Handler/InteractionHandler.cs)
<img src="https://github.com/user-attachments/assets/724601c5-d8c8-47ea-861e-567a6bab121a" alt="벽타기" width="500px"> <br>
  
  <br><br>
</details>

<details>
  <summary>아이템 소개</summary>
  
  ## 📗 아이템 소개
  
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
