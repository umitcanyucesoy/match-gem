using System;
using System.Collections.Generic;
using Gems;
using UnityEngine;
using System.Linq;

namespace BoardGame
{
    public class MatchFinder : MonoBehaviour
    {
        
        [SerializeField] private List<Gem> currentMatchesGems = new List<Gem>();
        
        public static MatchFinder Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        public void FindAllMatches()
        {
            for (var x = 0; x < Board.Instance.width; x++)
            {
                for (var y = 0; y < Board.Instance.height; y++)
                {   
                    var currentGem = Board.Instance.AllGems[x, y];
                    if (!currentGem) continue;
                    
                    if (x > 0 && x < Board.Instance.width - 1)
                    {
                        var leftGem = Board.Instance.AllGems[x - 1, y];
                        var rightGem = Board.Instance.AllGems[x + 1, y];
                        if (leftGem && rightGem)
                        {
                            if(leftGem.gemType == currentGem.gemType && rightGem.gemType == currentGem.gemType)
                            {
                                leftGem.isMatched = true;
                                currentGem.isMatched = true;
                                rightGem.isMatched = true;
                                
                                currentMatchesGems.Add(leftGem);
                                currentMatchesGems.Add(currentGem);
                                currentMatchesGems.Add(rightGem);
                            }
                        }
                    }
                    
                    if (y > 0 && y < Board.Instance.height - 1)
                    {
                        var upGem = Board.Instance.AllGems[x, y + 1];
                        var downGem = Board.Instance.AllGems[x, y - 1];
                        if (upGem && downGem)
                        {
                            if(upGem.gemType == currentGem.gemType && downGem.gemType == currentGem.gemType)
                            {
                                upGem.isMatched = true;
                                currentGem.isMatched = true;
                                downGem.isMatched = true;
                                
                                currentMatchesGems.Add(upGem);
                                currentMatchesGems.Add(currentGem);
                                currentMatchesGems.Add(downGem);
                            }
                        }
                    }
                }
            }
                
            if(currentMatchesGems.Count > 0)
                currentMatchesGems = currentMatchesGems.Distinct().ToList();
        }
        
        
    }
}