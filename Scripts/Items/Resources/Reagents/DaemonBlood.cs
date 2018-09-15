namespace Server.Items
{
  public class DaemonBlood : BaseReagent, ICommodity
  {
    [Constructible]
    public DaemonBlood() : this(1)
    {
    }

    [Constructible]
    public DaemonBlood(int amount) : base(0xF7D, amount)
    {
    }

    public DaemonBlood(Serial serial) : base(serial)
    {
    }

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