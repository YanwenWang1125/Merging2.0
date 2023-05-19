using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

/*namespace MRTK.Tutorials.MultiUserCapabilities*/
public class UserRemove : MonoBehaviour
{

    private PhotonView pv;
    

    private void createPrivatePV() {

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        string userObjName = "User" + playerCount;
        GameObject userObj = GameObject.Find(userObjName);

        if (userObj != null)
        {
            PhotonView photonView = userObj.GetComponent<PhotonView>();
            Debug.Log(userObj.gameObject.name);
            pv = photonView;
        }
        else
            Debug.Log("Current User Obj is Null");
        
     
    }
    [PunRPC]
    public void DisableObject(string removingObjName)
    {
        string cloneObjName = removingObjName + "(Clone)";
        GameObject cloneObj = GameObject.Find(cloneObjName);
        if (cloneObj != null)
        {
            cloneObj.SetActive(false);
            Debug.LogWarning("GameObject " + cloneObjName + " found.");
        }
        else
        {
            Debug.LogWarning("GameObject " + cloneObjName + " not found. It may have been destroyed.");
        }
    }


    private bool checkObjExist(GameObject removingObj) {
        bool results = false;
        if (removingObj == null) {
            Debug.Log("removing Object is null");
        }
        string objectStringName = removingObj.name + "(Clone)";
        if (GameObject.Find(objectStringName) != null)
            results = true;
        return results;
    }


    public void MyDeletion(GameObject removingObj)
    {
        Debug.Log(removingObj.name);
        if (checkObjExist(removingObj))
        {
            createPrivatePV();
            if (pv != null)
            {

                // Check if you are the owner before calling the RPC
                if (pv.IsMine)
                {
                    pv.RPC("DisableObject", RpcTarget.All, removingObj.name);
                    Debug.LogWarning("PhotonView found on ImageTarget.");
                }
                else
                {
                    Debug.LogWarning("Ownership request for PhotonView on ImageTarget was not successful.");
                }
            }
            else
            {
                Debug.LogWarning("PhotonView not found on ImageTarget.");
            }
        }
    }
}
