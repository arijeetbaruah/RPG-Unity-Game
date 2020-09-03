using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersisranceObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject presistanObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            
            hasSpawned = true;
            SpawnPersisranceObject();
            
        }

        void SpawnPersisranceObject()
        {
            GameObject obj = Instantiate(presistanObjectPrefab);
            DontDestroyOnLoad(obj);
        }
    }
}
