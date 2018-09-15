namespace Server.Items
{
  public class Bolt : Item, ICommodity
  {
    [Constructible]
    public Bolt() : this(1)
    {
    }

    [Constructible]
    public Bolt(int amount) : base(0x1BFB)
    {
      Stackable = true;
      Amount = amount;
    }

    public Bolt(Serial serial) : base(serial)
    {
    }

    public override double DefaultWeight => 0.1;
    int ICommodity.DescriptionNumber => LabelNumber;
    bool ICommodity.IsDeedable => true;


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