using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        public static PhotonRoom Room;

        [SerializeField] private GameObject photonUserPrefab = default;
        [SerializeField] private GameObject skullPrefab = default;
        [SerializeField] private GameObject brainPrefab = default;
        [SerializeField] private Transform roverExplorerLocation = default;
        public GameObject ImageTarget;

        private Vector3 anchorPosition;
        private Quaternion anchorRotation;
        private PhotonView pv;
        private Player[] photonPlayers;
        private int playersInRoom;
        private int myNumberInRoom;
        private bool hasObjInRoom = false;


        // private GameObject module;
        // private Vector3 moduleLocation = Vector3.zero;
/*
        [PunRPC]
        public void PRCAllObj(GameObject cloneObj, Vector3 newVector) {
            
            if (cloneObj != null)
            {
                cloneObj.transform.position = newVector;
                
               
                    
            }
        }*/

        public void update() {
      /*      string cloneObjName = newTempObj.name + "(Clone)";
            GameObject cloneObj = GameObject.Find(cloneObjName);
            cloneObj.transform.position = ImageTarget.transform.position;*/
            /*photonView.RPC("PRCAllObj", RpcTarget.All, cloneObj, ImageTarget.transform);*/

        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom++;
        }

        private void Awake()
        {
            if (Room == null)
            {
                Room = this;
            }
            else
            {
                if (Room != this)
                {
                    Destroy(Room.gameObject);
                    Room = this;
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

   


        private void Start()
        {
            pv = ImageTarget.GetComponent<PhotonView>();

            // Allow prefabs not in a Resources folder
            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if (photonUserPrefab != null) pool.ResourceCache.Add(photonUserPrefab.name, photonUserPrefab);
                if (photonUserPrefab != null) pool.ResourceCache.Add(skullPrefab.name, skullPrefab);
                if (photonUserPrefab != null) pool.ResourceCache.Add(brainPrefab.name, brainPrefab);
            }

            
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;
            PhotonNetwork.NickName = myNumberInRoom.ToString();

            StartGame();
        }

        private void StartGame()
        {
            CreatPlayer();
        /*    createPrivatePV();*/

            /*
                        if (TableAnchor.Instance != null) CreateInteractableObjects();*/
        }

        private void CreatPlayer()
        {
            var player = PhotonNetwork.Instantiate(photonUserPrefab.name, new Vector3(0f, 0.2f, 0f), Quaternion.identity);
        }

    

        

        public void MyCreation(GameObject newTempObj)
        {
            hasObjInRoom = true;

    /*        trackingImage();*/
                                                                 

            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if(!pool.ResourceCache.ContainsKey(newTempObj.name))
                    if (newTempObj != null) pool.ResourceCache.Add(newTempObj.name, newTempObj);
            }


            var position = roverExplorerLocation.transform.position;
            var positionOnTopOfSurface = new Vector3(position.x, position.y+0.5f,
                position.z);


            Debug.Log(positionOnTopOfSurface);
            
            Quaternion rotation = newTempObj.transform.rotation;
            var go = PhotonNetwork.Instantiate(newTempObj.name, positionOnTopOfSurface, rotation);
          
           
            



        }

      


        // private void CreateMainLunarModule()
        // {
        //     module = PhotonNetwork.Instantiate(roverExplorerPrefab.name, Vector3.zero, Quaternion.identity);
        //     pv.RPC("Rpc_SetModuleParent", RpcTarget.AllBuffered);
        // }
        //
        // [PunRPC]
        // private void Rpc_SetModuleParent()
        // {
        //     Debug.Log("Rpc_SetModuleParent- RPC Called");
        //     module.transform.parent = TableAnchor.Instance.transform;
        //     module.transform.localPosition = moduleLocation;
        // }

        /*    private void CreateInteractableObjects()
        {
            var position = roverExplorerLocation.position;
            var positionOnTopOfSurface = new Vector3(position.x, position.y + 0.3f,
                position.z);

            var go = PhotonNetwork.Instantiate(roverExplorerPrefab.name, positionOnTopOfSurface,
                roverExplorerLocation.rotation);

           *//* Debug.Log("init: " + roverExplorerPrefab.GetPhotonView().ViewID);*//*
        }*/
    }
}
