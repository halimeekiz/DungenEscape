using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Item
    {
        public int Damage { get; set; }
        public int Heal { get; set; }
        public int AttackPower { get; set; }
        public string Name { get; set; }


        public Item(string name, int damage, int heal, int attackPower)
        {
            Name = name;
            Damage = damage;
            AttackPower = attackPower;
            Heal = heal;
        }

    }
}

