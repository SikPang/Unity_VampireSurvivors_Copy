# Unity_VampireSurvivors_Copy
> 뱀파이어 서바이벌 모작 (2022. 07. 25 ~ 2022. 08. 11) <br/>
> Unity C# <br/>
 
 
<img src="https://user-images.githubusercontent.com/87380790/184260073-ec6a5674-392d-4175-acc6-c79661d91c49.png" width="80%">

### [>> View All Source Codes <<](https://github.com/SikPang/Unity_VampireSurvivors_Copy/tree/main/Assets/Scripts)  <br/>
### [>> Download Game <<](https://drive.google.com/file/d/1tGWBtdpHSdv7wRL8RQ1wvmi3kwXcsKaI/view) <br/>

<br/>
<br/>
<br/>
  
## 1. Object Pooling
> Instantiate와 Destory의 가비지 컬렉팅으로 인한 프레임 드랍 방지 <br/>

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


<img src="https://user-images.githubusercontent.com/87380790/184267717-b6768be3-3d13-42f9-a16b-2f79fd5ac5a2.gif" width="49%">  <img src="https://user-images.githubusercontent.com/87380790/184267731-8ff9c827-278d-41e7-80ec-ae8c340b0033.gif" width="49%">


- Scriptable Objects로 4종류 적의 정보 불러와 stage마다 다르게 Key값으로 사용 <br/> <br/>
- 방향을 랜덤으로 정하고 Object Pooling에서 가져오기 <br/>
                      
<br/>
<br/>
<br/>
  
## 2. 공격, 무기
> 총 6가지의 무기에 따라 각 다른 자동 공격


<img src="https://user-images.githubusercontent.com/87380790/184267824-bcb9e7c2-a101-4cdd-b9b7-3e42b00fbfde.gif" width="49%">  <img src="https://user-images.githubusercontent.com/87380790/184267853-cd707f08-cb9e-405d-a937-358f13e2d9be.gif" width="49%">


- 컴포넌트 패턴에서 아이디어를 얻어 미리 모든 생성기 스크립트를 오브젝트에 넣어두고 획득한 무기만 GetComponent 하여 StartCoroutine <br/><br/>
- Scriptable Objects로 각각의 무기 정보 불러오기<br/><br/>
- 상속으로 각각의 무기 생성기 설계, 생성기의 Coroutine에서 주기마다 무기 이펙트를 ObjectPooling에서 가져와 생성 <br/><br/>
- 무기 이펙트 생성 시, 크기와 각도, AddForce 조절 및 코루틴으로 공격 속도 조절 (악세사리 효과 적용 등)<br/><br/>
- 무기의 레벨에 따라 생성되는 무기 이펙트 개수 조절<br/><br/>
- 무기 생성기의 데미지와 지속 시간 등을 무기 클래스로 파라미터로 넘겨주어 레벨에 따라 데미지 조절<br/>


<br/>
<br/>
<br/>
  
## 3. 크리스탈
> 적 처치 시 일정 확률로 드랍되는 레벨 업 크리스탈


<img src="https://user-images.githubusercontent.com/87380790/184267990-bdba7778-e24a-4185-b046-cd723a1908da.gif" width="80%">


- Scriptable Objects로 각각의 크리스탈 정보 불러오고, ObjectPooling에서 가져와 드랍 <br/><br/>
- 플레이어가 바라보는 방향으로 AddForce 이후 코루틴을 이용해서 0.2초 뒤 플레이어에게 이동 <br/>


<br/>
<br/>
<br/>
  
## 4. 레벨 업
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
  
## 5. 인벤토리
> 현재까지 획득한 아이템 리스트


<img src="https://user-images.githubusercontent.com/87380790/184268158-a06f4cc2-4540-4a17-be1f-1079d7d042c0.png" width="50%">

 
- Dictionary를 사용하여 획득한 무기와 악세사리 추가 value 값으로 레벨을 가짐 <br/><br/>
- 정해진 슬롯 개수를 동적 생성 후 Dictionary를 순회하며 해당 아이템의 정보를 표기 <br/>


<br/>
<br/>
<br/>
  
## 6. 피격, 사망
> OnCollisionEnter로 빨갛게 색 변경, 체력이 깎일 시 출혈 ParticleSystem 재생


<img src="https://user-images.githubusercontent.com/87380790/184268025-637a2edd-7702-4d9f-9a4e-32e48478cc60.gif" width="80%">




<br/>
<br/>
<br/>

 ## Screen Shots
 
<img src="https://user-images.githubusercontent.com/87380790/184273251-eace4ae5-0d05-4b38-86b6-72c23bd97f3e.gif" width="80%">
<img src="https://user-images.githubusercontent.com/87380790/184268592-8edbb618-4eab-4e08-b52c-cece56e9f15d.png" width="80%">
