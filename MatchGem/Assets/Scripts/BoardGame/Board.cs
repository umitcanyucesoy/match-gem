using System;
using System.Collections;
using System.Collections.Generic;
using Gems;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardGame
{
    public class Board : MonoBehaviour
    {
        [Header("-----Board Settings-----")]
        public int width;
        public int height;
        
        [Header("-----Board Elements-----")]
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private List<Gem> gems = new List<Gem>();
        public Gem[,] AllGems;

        public static Board Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            AllGems = new Gem[width, height];
            CreateBoard();
        }

        private void Update()
        {
            MatchFinder.Instance.FindAllMatches();
        }

        private void CreateBoard()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.transform.SetParent(transform);
                    tile.name = $"Tile {x},{y}";
                    
                    var gemToUse = Random.Range(0, gems.Count);
                    SpawnGem(new Vector3(x,y,0), gems[gemToUse]);   
                }
            }
        }

        private void SpawnGem(Vector3 position,Gem gemToSpawn)
        {
            var gem = Instantiate(gemToSpawn, position, Quaternion.identity);
            gem.transform.SetParent(transform);
            gem.name = $"Gem {position.x},{position.y}";
            
            AllGems[(int)position.x, (int)position.y] = gem;
            gem.SetupGem(new Vector2Int((int)position.x, (int)position.y), this);
        }
        
        
    }
    
}