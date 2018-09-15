namespace Server.Mobiles
{
  public class Phoenix : BaseCreature
  {
    [Constructible]
    public Phoenix() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
    {
      Body = 5;
      Hue = 0x674;
      BaseSoundID = 0x8F;

      SetStr(504, 700);
      SetDex(202, 300);
      SetInt(504, 700);

      SetHits(340, 383);

      SetDamage(25);

      SetDamageType(ResistanceType.Physical, 50);
      SetDamageType(ResistanceType.Fire, 50);

      SetResistance(ResistanceType.Physical, 45, 55);
      SetResistance(ResistanceType.Fire, 60, 70);
      SetResistance(ResistanceType.Poison, 25, 35);
      SetResistance(ResistanceType.Energy, 40, 50);

      SetSkill(SkillName.EvalInt, 90.2, 100.0);
      SetSkill(SkillName.Magery, 90.2, 100.0);
      SetSkill(SkillName.Meditation, 75.1, 100.0);
      SetSkill(SkillName.MagicResist, 86.0, 135.0);
      SetSkill(SkillName.Tactics, 80.1, 90.0);
      SetSkill(SkillName.Wrestling, 90.1, 100.0);

      Fame = 15000;
      Karma = 0;

      VirtualArmor = 60;
    }

    public Phoenix(Serial serial) : base(serial)
    {
    }

    public override string CorpseName => "a phoenix corpse";
    public override string DefaultName => "a phoenix";

    public override int Meat => 1;
    public override MeatType MeatType => MeatType.Bird;
    public override int Feathers => 36;
    public override bool CanFly => true;

    public override void GenerateLoot()
    {
      AddLoot(LootPack.FilthyRich);
      AddLoot(LootPack.Rich);
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);
      writer.Write(0);
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);
      int version = reader.ReadInt();
    }
  }
}