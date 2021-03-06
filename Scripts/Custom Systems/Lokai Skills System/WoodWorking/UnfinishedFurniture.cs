/***************************************************************************
 *   New LokaiSkill System script by Lokai. This program is free software; you 
 *   can redistribute it and/or modify it under the terms of the GNU GPL. 
 ***************************************************************************/
using System;
using System.Reflection;
using Server;

namespace Server.Items
{
    public interface IStainable
    {
        void Stain(CraftResource resource);
    }

    public class UnfinishedFurniture : Item, IStainable
    {
        [Constructable]
        public UnfinishedFurniture(int itemID) : base(itemID) { }
        public UnfinishedFurniture(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }

        #region IStainable Members

        public void Stain(CraftResource resource)
        {
            Hue = CraftResources.GetHue(resource);
        }

        #endregion
    }

    public class LatheWork : UnfinishedFurniture
    {
        [Constructable]
        public LatheWork() : base(0x1E82) { }
        public LatheWork(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) 
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader) 
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class UnfinishedStool : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedStool() : base(0x1E7F) { }
        public UnfinishedStool(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "stool"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class LargeTableLegs : UnfinishedFurniture
    {
        [Constructable]
        public LargeTableLegs() : base(0x1E75) { }
        public LargeTableLegs(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "large table legs"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E76, 0x1E7E)]
    public class UnfinishedShelves : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedShelves() : base(0x1E76) { }
        public UnfinishedShelves(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "shelves"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E74, 0x1E7D)]
    public class UnfinishedTableLeg : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedTableLeg() : base(0x1E74) { }
        public UnfinishedTableLeg(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "table leg"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E73, 0x1E7C)]
    public class UnfinishedTableLegs : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedTableLegs() : base(0x1E73) { }
        public UnfinishedTableLegs(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "table legs"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E72, 0x1E7B)]
    public class UnfinishedTable : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedTable() : base(0x1E72) { }
        public UnfinishedTable(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "table"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E71, 0x1E7A)]
    public class UnfinishedChestOfDrawers : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedChestOfDrawers() : base(0x1E71) { }
        public UnfinishedChestOfDrawers(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "chest of drawers"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E70, 0x1E79)]
    public class UnfinishedDrawer : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedDrawer() : base(0x1E70) { }
        public UnfinishedDrawer(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "drawer"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E6F, 0x1E78)]
    public class UnfinishedChair : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedChair() : base(0x1E6F) { }
        public UnfinishedChair(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "chair"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E6E, 0x1E77)]
    public class UnfinishedBoard : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedBoard() : base(0x1E6E) { }
        public UnfinishedBoard(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "board"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    [Flipable(0x1E80, 0x1E81)]
    public class UnfinishedWoodenBox : UnfinishedFurniture
    {
        [Constructable]
        public UnfinishedWoodenBox() : base(0x1E80) { }
        public UnfinishedWoodenBox(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "wooden box"; } }

        public override void Serialize(GenericWriter writer)
        { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader)
        { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}