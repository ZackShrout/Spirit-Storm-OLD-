using UnityEngine;
using System.Collections;
using SpiritStorm.Saving;
using SpiritStorm.Resource;
using SpiritStorm.Core;
using SpiritStorm.Stats;

namespace SpiritStorm.Combat
{   
    public class Fighter : MonoBehaviour
    {
        [SerializeField] GameObject mainHandPrefab = null;
        [SerializeField] GameObject offHandPrefab = null;
        [SerializeField] Transform mainHandTransform = null;
        [SerializeField] Transform offHandTransform = null;
        
        Health target;

        void Start()
        {

        }

        void Update()
        {

        }

        void DealDamage()
        {
            float damage = GetComponent<BaseStats>().GetStat(Stat.Strength);
            target.TakeDamage(gameObject, damage);
        }
    }
}