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
        [SerializeField] private GameObject roverExplorerPrefab = default;
        [SerializeField] private GameObject newTempObj = default;
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

        public void Update()
        {
            
            if (hasObjInRoom)
            {
                string cloneObjName = newTempObj.name + "(Clone)";
                GameObject cloneObj = GameObject.Find(cloneObjName);
                /*      cloneObj.transform.localPosition = Vector3.Lerp(ImageTarget.transform.localPosition, anchorPosition, Time.deltaTime * 5);
                      cloneObj.transform.localRotation = Quaternion.Lerp(ImageTarget.transform.localRotation, anchorRotation, Time.deltaTime * 5);*/
                cloneObj.transform.localPosition = ImageTarget.transform.localPosition;
                cloneObj.transform.localRotation = ImageTarget.transform.localRotation;

            }
        }

        private void Start()
        {
            pv = ImageTarget.GetComponent<PhotonView>();

            // Allow prefabs not in a Resources folder
            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if (photonUserPrefab != null) pool.ResourceCache.Add(photonUserPrefab.name, photonUserPrefab);
                if (newTempObj != null) pool.ResourceCache.Add(newTempObj.name, newTempObj);
                if (roverExplorerPrefab != null) pool.ResourceCache.Add(roverExplorerPrefab.name, roverExplorerPrefab);
            }

            Debug.Log("PR started");
            if (pv == null) {
                Debug.Log("PR PV is null");
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

            if (!PhotonNetwork.IsMasterClient) return;
            /*
                        if (TableAnchor.Instance != null) CreateInteractableObjects();*/
        }

        private void CreatPlayer()
        {
            var player = PhotonNetwork.Instantiate(photonUserPrefab.name, new Vector3(0f, 0.2f, 0f), Quaternion.identity);
        }

    



        public void MyCreation()
        {
            hasObjInRoom = true;

            trackingImage();


            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if(!pool.ResourceCache.ContainsKey(newTempObj.name))
                    if (newTempObj != null) pool.ResourceCache.Add(newTempObj.name, newTempObj);
            }


            var position = anchorPosition;
            var positionOnTopOfSurface = new Vector3(position.x, position.y,
                position.z);
            Debug.Log(positionOnTopOfSurface + " : " + anchorRotation);
            var go = PhotonNetwork.Instantiate(newTempObj.name, positionOnTopOfSurface,
                anchorRotation);
            /*PhotonView photonView = newTempObj.GetComponent<PhotonView>();
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);*/

        }


        [PunRPC]
        private void UpdateDynamicAnchor(Vector3 position, Quaternion rotation)
        {
            // Update the dynamic anchor's position and rotation
            anchorPosition = position;
            anchorRotation = rotation;
        }
        public void trackingImage()
        {
            anchorPosition = ImageTarget.transform.position;
            anchorRotation = ImageTarget.transform.rotation;

            // Update the dynamic anchor's position and rotation across the network
            if (pv != null)
                pv.RPC("UpdateDynamicAnchor", RpcTarget.Others, anchorPosition, anchorRotation);
            else
                Debug.Log("PV is null");     
        }



        [PunRPC]
        public void DisableObject(int viewId)
        {
            PhotonView targetView = PhotonView.Find(viewId);
            if (targetView != null)
            {
                Debug.Log("found:" + viewId);
                targetView.gameObject.SetActive(false);
            }
        }

        public void MyDeletion() {

            if (hasObjInRoom)
            {
                PhotonView photonView = newTempObj.GetComponent<PhotonView>();

                /*photonView.RPC("DisableObject", RpcTarget.All, photonView.ViewID);*/
                string cloneObjName = newTempObj.name + "(Clone)";
                GameObject cloneObj = GameObject.Find(cloneObjName);
                cloneObj.SetActive(false);
                hasObjInRoom = false;
            }
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
