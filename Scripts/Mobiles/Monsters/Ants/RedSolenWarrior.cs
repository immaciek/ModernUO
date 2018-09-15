using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
  public class RedSolenWarrior : BaseCreature
  {
    [Constructible]
    public RedSolenWarrior() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
    {
      Body = 782;
      BaseSoundID = 959;

      SetStr(196, 220);
      SetDex(101, 125);
      SetInt(36, 60);

      SetHits(96, 107);

      SetDamage(5, 15);

      SetDamageType(ResistanceType.Physical, 80);
      SetDamageType(ResistanceType.Poison, 20);

      SetResistance(ResistanceType.Physical, 20, 35);
      SetResistance(ResistanceType.Fire, 20, 35);
      SetResistance(ResistanceType.Cold, 10, 25);
      SetResistance(ResistanceType.Poison, 20, 35);
      SetResistance(ResistanceType.Energy, 10, 25);

      SetSkill(SkillName.MagicResist, 60.0);
      SetSkill(SkillName.Tactics, 80.0);
      SetSkill(SkillName.Wrestling, 80.0);

      Fame = 3000;
      Karma = -3000;

      VirtualArmor = 35;

      SolenHelper.PackPicnicBasket(this);
      PackItem(new ZoogiFungus(0.05 < Utility.RandomDouble() ? 3 : 13));

      if (Utility.RandomDouble() < 0.05)
        PackItem(new BraceletOfBinding());
    }

    public RedSolenWarrior(Serial serial) : base(serial)
    {
    }

    public override string CorpseName => "a solen warrior corpse";
    public bool BurstSac{ get; private set; }

    public override string DefaultName => "a red solen warrior";

    public override int GetAngerSound()
    {
      return 0xB5;
    }

    public override int GetIdleSound()
    {
      return 0xB5;
    }

    public override int GetAttackSound()
    {
      return 0x289;
    }

    public override int GetHurtSound()
    {
      return 0xBC;
    }

    public override int GetDeathSound()
    {
      return 0xE4;
    }

    public override void GenerateLoot()
    {
      AddLoot(LootPack.Rich);
      AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
    }

    public override bool IsEnemy(Mobile m)
    {
      if (SolenHelper.CheckRedFriendship(m))
        return false;
      return base.IsEnemy(m);
    }

    public override void OnDamage(int amount, Mobile from, bool willKill)
    {
      SolenHelper.OnRedDamage(from);

      if (!willKill)
      {
        if (!BurstSac)
        {
          if (Hits < 50)
          {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The solen's acid sac is burst open! *");
            BurstSac = true;
          }
        }
        else if (from != null && from != this && InRange(from, 1))
        {
          SpillAcid(from, 1);
        }
      }

      base.OnDamage(amount, from, willKill);
    }

    public override bool OnBeforeDeath()
    {
      SpillAcid(4);

      return base.OnBeforeDeath();
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);
      writer.Write(1);
      writer.Write(BurstSac);
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);
      int version = reader.ReadInt();

      switch (version)
      {
        case 1:
        {
          BurstSac = reader.ReadBool();
          break;
        }
      }
    }
  }
}