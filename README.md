# Unity_VampireSurvivors_Copy
> 뱀파이어 서바이벌 모작 (2022. 07 ~ 2022. 08) <br/>
> Unity2D C# <br/>
 
 
<img src="https://user-images.githubusercontent.com/87380790/184260073-ec6a5674-392d-4175-acc6-c79661d91c49.png" width="80%">

### [>> View All Source Codes](https://github.com/SikPang/Unity_VampireSurvivors_Copy/tree/android/Assets/Scripts)  <br/>
### [>> Download Game [Windows]](https://drive.google.com/file/d/1tGWBtdpHSdv7wRL8RQ1wvmi3kwXcsKaI/view) <br/>
### [>> Download Game [MacOS]](https://drive.google.com/file/d/1PGxx8P-M8Ph8j4mRweQPi6QLz1eX90Ug/view?usp=sharing) <br/>
### [>> Download Game [Android]](https://drive.google.com/file/d/1SiQrK_csoYX4-b9fRdXm9gapInbnVWD_/view?usp=sharing) <br/>
### [>> Download Build(need Xcode) [iOS]](https://drive.google.com/file/d/1eikto2OBk8gGUPl9WagoHgBoyGuL7x-G/view?usp=sharing) <br/>

<br/>
<br/>
<br/>
  
## 1. Object Pooling
> Destory의 가비지 컬렉팅과 Instantiate으로 인한 프레임 드랍 방지 <br/>

- Dictionary<string, Queue>를 활용하여 시작 시 Instantiate하여 Queue에 담아두고, 필요할 때 Queue에서 하나씩 가져오기 <br/><br/>
- 적, 무기 이펙트, 플로팅 데미지 텍스트, 크리스탈 오브젝트 생성을 담당하고 각각의 고유 key값으로 접근 <br/> <br/>
- Generic을 활용하여 모든 타입을 받아 switch-case로 처리 <br/> 


<br/>
<br/>
<br/>
  
## 2. Scriptable Objects
> 플레이어, 적, 무기, 악세사리, 크리스탈 등의 모든 종류별 데이터 관리 <br/>  

- 생명력, 공격력, 이동속도, Image, Animator 등 기본 정보 포함 <br/><br/>
- 해당 데이터들을 모두 가져올 수 있는 싱글톤 패턴의 클래스로 관리 <br/><br/>
- 종류마다 하나씩 상속받는 대신 Scriptable Objects에서 정보를 가져오는 방식 <br/>


<br/>
<br/>
<br/>
  
## 3. 적 생성기
> 사방에서 0.5초마다 적 생성 


<img src="https://user-images.githubusercontent.com/87380790/185332137-f5f9a01d-e783-468e-8713-99cd46f2b39e.gif" width="49%">  <img src="https://user-images.githubusercontent.com/87380790/185332205-c4da0f07-0728-4e7f-baa8-9ed49554321e.gif" width="49%">


- Scriptable Objects로 4종류 적의 정보 불러와 stage마다 다르게 Key값으로 사용 <br/> <br/>
- 방향을 랜덤으로 정하고 Object Pooling에서 가져오기 <br/>
                      
<br/>
<br/>
<br/>
  
## 4. 공격
> 획득한 무기에 따라 공격

<img src="https://user-images.githubusercontent.com/87380790/185332662-cf8900a2-e0d8-47f4-b8d3-dc8d8b6bec34.gif" width="80%">

- 공격 받은 적은 Coroutine으로 0.1초간 하얗게 색 변경 (SpriteRenderer의 Shader를 Text Shader로 일시적으로 변경하여 구현) <br/><br/>
- 공격 받은 적은 AddForce로 적의 이동 방향의 반대 방향으로 넉백 <br/><br/>
- 기본 데미지의 +-20%의 랜덤 데미지를 입힘, 해당 데미지 텍스트를 Object Pooling에서 가져와 플로팅<br/>

<br/>
<br/>
<br/>
  
## 5. 무기
> 총 6가지의 무기에 따라 각 다른 자동 공격

<img src="https://user-images.githubusercontent.com/87380790/185332844-6a26ed01-cf3a-4741-91e9-9163c06061ea.gif" width="80%">

- 컴포넌트 패턴에서 아이디어를 얻어 미리 모든 생성기 스크립트를 오브젝트에 넣어두고 획득한 무기만 GetComponent 하여 StartCoroutine <br/><br/>
- Scriptable Objects로 각각의 무기 정보 불러오기<br/><br/>
- 상속으로 각각의 무기 생성기 설계, 생성기의 Coroutine에서 주기마다 무기 이펙트를 ObjectPooling에서 가져와 생성 <br/><br/>
- 무기 이펙트 생성 시, 크기와 각도, AddForce 조절 및 코루틴으로 공격 속도 조절 (악세사리 효과 적용 등)<br/><br/>
- 무기의 레벨에 따라 생성되는 무기 이펙트 개수 조절<br/><br/>
- 무기 생성기의 데미지와 지속 시간 등을 무기 클래스로 파라미터로 넘겨주어 레벨에 따라 데미지 조절<br/>


<br/>
<br/>
<br/>
  
## 6. 크리스탈
> 적 처치 시 일정 확률로 드랍되는 레벨 업 크리스탈


<img src="https://user-images.githubusercontent.com/87380790/184267990-bdba7778-e24a-4185-b046-cd723a1908da.gif" width="80%">


- Scriptable Objects로 각각의 크리스탈 정보 불러오고, ObjectPooling에서 가져와 드랍 <br/><br/>
- 플레이어가 바라보는 방향으로 AddForce 이후 코루틴을 이용해서 0.2초 뒤 플레이어에게 이동 <br/>


<br/>
<br/>
<br/>
  
## 7. 레벨 업
>  레벨업 할 때마다 랜덤하게 뽑힌 3가지의 아이템 중 선택하여 획득 가능


<img src="https://user-images.githubusercontent.com/87380790/184268000-1ddc3864-5b8e-4ee4-9904-876384860a2e.gif" width="80%">


- 정해진 슬롯 개수를 동적 생성 후 각각의 정보 오브젝트(아이콘,설명 등)를 배열에 담아둠 <br/><br/>
- 랜덤 아이템을 Scriptable Objects의 관리 클래스에서 정보를 가져와 표기 <br/><br/>
- 아이템을 고를 때마다 Inventory의 Dictionary에 추가 후 효과 적용 (ContainsKey로 이미 있는지 확인 후 획득 or 레벨업) <br/><br/>
- ParticleSystem과 Color.Lerp를 이용한 이펙트 <br/><br/>
- 악세사리는 무기와 다르게 효과만 반복 적용 <br/>


<br/>
<br/>
<br/>
  
## 8. 인벤토리
> 현재까지 획득한 아이템 리스트


<img src="https://user-images.githubusercontent.com/87380790/184268158-a06f4cc2-4540-4a17-be1f-1079d7d042c0.png" width="50%">

 
- Dictionary를 사용하여 획득한 무기와 악세사리 추가 value 값으로 레벨을 가짐 <br/><br/>
- 정해진 슬롯 개수를 동적 생성 후 Dictionary를 순회하며 해당 아이템의 정보를 표기 <br/>


<br/>
<br/>
<br/>
  
## 9. 피격, 사망
> 체력이 깎일 시, Coroutine으로 0.2초간 빨갛게 색 변경, 출혈 ParticleSystem 재생


<img src="https://user-images.githubusercontent.com/87380790/185333082-d4cbb604-a4fa-4837-93fa-13207d2c8678.gif" width="80%">


<br/>
<br/>
<br/>

## 10. 모바일 가상 컨트롤러
> 화면의 원하는 곳을 터치 - 드래그하여 가상 컨트롤러 활성화


<img src="https://user-images.githubusercontent.com/87380790/185814637-4e25291f-7f89-45b8-92ba-3085ec097c3b.jpg" width="80%">
<img src="https://user-images.githubusercontent.com/87380790/185814640-6c252ca3-e90c-410f-bdf5-b7574cd57396.jpg" width="80%">


<br/>
<br/>
<br/>

 ## Screen Shots
 
<img src="https://user-images.githubusercontent.com/87380790/185333253-0ffdd915-c5a2-496c-90ef-467e2fcc9815.gif" width="80%">
<img src="https://user-images.githubusercontent.com/87380790/184268592-8edbb618-4eab-4e08-b52c-cece56e9f15d.png" width="80%">
<img src="https://user-images.githubusercontent.com/87380790/186140219-2336666a-4386-4e55-a310-3575132506dd.jpg" width="80%">
<img src="https://user-images.githubusercontent.com/87380790/186140230-56a2b7df-68ef-4d84-8e4e-96d7c2b2c9af.jpg" width="80%">

<br/>
<br/>
<br/>

## Assets Credit
> Please write down the credit when using this project

### - Graphics
Monsters_Creatures_Fantasy - Luiz Melo - [Unity Asset Store Link](https://assetstore.unity.com/packages/2d/characters/monsters-creatures-fantasy-167949)

Hero Knight - Pixel Art - Sven Thole - [Unity Asset Store Link](https://assetstore.unity.com/packages/2d/characters/hero-knight-pixel-art-165188)

Pixel Art Top Down - Basic - Cainos - [Unity Asset Store Link](https://assetstore.unity.com/packages/2d/environments/pixel-art-top-down-basic-187605)

### - Code
SikPang - https://github.com/SikPang/Unity_VampireSurvivors_Copy
