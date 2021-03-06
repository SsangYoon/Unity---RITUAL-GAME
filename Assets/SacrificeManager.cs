﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using ResourceControll;

namespace SacrificeContoll
{
	public class SacrificeManager : MonoBehaviour
	{
		public List<int> _SacrifceList;
        public GameObject spawnFriendly;
        public CharacterInfo spawnInfo;
        [SerializeField] public Text text;

        public SpawnResponse[] _SpawnComponents;

        private ResourceManager _ResourceManager;

        public GameManager gameManager;
        
        public void Awake()
        {
            _SacrifceList = new List<int>();
            gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
            _ResourceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<ResourceManager>();
        }

		public void SpawnFriendly()
        {
            for (int i = 0; i < _SacrifceList.Count - 1; i++)
            {
                for (int j = 0; j < _SacrifceList.Count - 1; j++)
                {
                    if (_SacrifceList[j] > _SacrifceList[j + 1])
                    {
                        int temp = _SacrifceList[j];
                        _SacrifceList[j] = _SacrifceList[j + 1];
                        _SacrifceList[j + 1] = temp;
                    }
                }
            }

            foreach (int id in _SacrifceList)
            {
                _ResourceManager.EventCheck(id, -1);
            }
            

            while (_SacrifceList.Count < 5)
            {
                _SacrifceList.Add(0);
            }

            spawnInfo = XMLManager.Instance.Load_CharacterData(_SacrifceList[0], _SacrifceList[1], _SacrifceList[2], _SacrifceList[3], _SacrifceList[4]);

            if (spawnInfo == null)
            {
                Reset();
                return;
            }

            spawnFriendly = Instantiate(Resources.Load("Friendly/" + spawnInfo.ID), new Vector3(-3 - gameManager.friendlyList.Count, 0, -6), new Quaternion(0, 0, 0, 1)) as GameObject;
            gameManager.friendlyList.Add(spawnFriendly);

            spawnFriendly.GetComponent<Friendly>().hp = spawnInfo.HP;
            spawnFriendly.GetComponent<Friendly>().ap = spawnInfo.AP;

            text.GetComponent<SummonText>().Activate(spawnInfo.Name);
                         
            Reset();	
		}

		public void Reset()
		{
            for (int i = 0; i < _SpawnComponents.Length; i++)
            {
                _SpawnComponents[i]._Enabled = true;
            }
            // Reset
            _SacrifceList.Clear();
		}
        
        
	}
}

