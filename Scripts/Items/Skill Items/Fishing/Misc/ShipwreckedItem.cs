namespace Server.Items
{
  public interface IShipwreckedItem
  {
    bool IsShipwreckedItem{ get; set; }
  }

  public class ShipwreckedItem : Item, IDyable, IShipwreckedItem
  {
    public ShipwreckedItem(int itemID) : base(itemID)
    {
      int weight = ItemData.Weight;

      if (weight >= 255)
        weight = 1;

      Weight = weight;
    }

    public ShipwreckedItem(Serial serial) : base(serial)
    {
    }

    public bool Dye(Mobile from, DyeTub sender)
    {
      if (Deleted)
        return false;

      if (ItemID >= 0x13A4 && ItemID <= 0x13AE)
      {
        Hue = sender.DyedHue;
        return true;
      }

      from.SendLocalizedMessage(sender.FailMessage);
      return false;
    }

    #region IShipwreckedItem Members

    public bool IsShipwreckedItem
    {
      get => true;
      set { }
    }

    #endregion

    public override void OnSingleClick(Mobile from)
    {
      LabelTo(from, 1050039, $"#{LabelNumber}\t#1041645");
    }

    public override void AddNameProperties(ObjectPropertyList list)
    {
      base.AddNameProperties(list);
      list.Add(1041645); // recovered from a shipwreck
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