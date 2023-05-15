﻿using System;
using Photon.Pun;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class GenericNetworkManager : MonoBehaviour
    {
        public static GenericNetworkManager Instance;

        [HideInInspector] public string azureAnchorId = "";
        [HideInInspector] public PhotonView localUser;
        private bool isConnected;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    Destroy(Instance.gameObject);
                    Instance = this;
                }
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ConnectToNetwork();
            Debug.Log("GNM started");
        }

        // For future non PUN solutions
        private void StartNetwork(string ipAddress, string port)
        {
            throw new NotImplementedException();
            Debug.Log("GNM started Network");
        }

        private void ConnectToNetwork()
        {
            OnReadyToStartNetwork?.Invoke();
            Debug.Log("GNM started connected To network");
        }

        public static event Action OnReadyToStartNetwork;
    }
}
