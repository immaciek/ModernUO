namespace Server.Items
{
  public class BloodOathScroll : SpellScroll
  {
    [Constructible]
    public BloodOathScroll() : this(1)
    {
    }

    [Constructible]
    public BloodOathScroll(int amount) : base(101, 0x2261, amount)
    {
    }

    public BloodOathScroll(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}