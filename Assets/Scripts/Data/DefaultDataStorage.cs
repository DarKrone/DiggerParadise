using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DefaultDataStorage : MonoBehaviour
{
    public static DefaultDataStorage Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Serializable]
    public class MinisListStorage
    {
        public List<NeededResource> Resources;
    }

    public GameData DefaultGameData;
    public List<MinisListStorage> MinisListStorages;

    public GameData GetDefaultGameData
    {
        get
        {
            DefaultGameData.NeededResources = DefaultNeededResources;
            return DefaultGameData;
        }
    }
    private List<List<NeededResource>> DefaultNeededResources
    { 
        get 
        {
            List<List<NeededResource>> values = new List<List<NeededResource>>();
            foreach(var el in MinisListStorages)
            {
                values.Add(el.Resources);
            }
            return values;
        } 
    }
}
