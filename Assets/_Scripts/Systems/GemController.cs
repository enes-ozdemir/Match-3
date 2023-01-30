using System.Collections.Generic;
using _Scripts.SO;
using UnityEngine;

namespace _Scripts.Systems
{
    public class GemController : MonoBehaviour
    {
        [SerializeField] private List<Gem> gemList;

        public Gem GetRandomGem()
        {
            var index = Random.Range(0, gemList.Count);
            return gemList[index];
        }
    }
}