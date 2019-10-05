namespace Server.Items
{
  public class TuitionReimbursementForm : Item
  {
    [Constructible]
    public TuitionReimbursementForm() : base(0xE3A) => LootType = LootType.Blessed;

    public TuitionReimbursementForm(Serial serial) : base(serial)
    {
    }

    public override int LabelNumber => 1074610; // Tuition Reimbursement Form (in triplicate)

    public override bool Nontransferable => true;

    public override void AddNameProperties(ObjectPropertyList list)
    {
      base.AddNameProperties(list);
      AddQuestItemProperty(list);
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // Version
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}