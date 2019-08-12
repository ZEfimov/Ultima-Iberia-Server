using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using System.Collections;
using Server.Targeting;
using Server.Targets;
using Server.Network;
using Server.Engines.Help;
using Server.Prompts;
using Server.Items;

namespace Server.Items
{
    public enum ArmyFormationEnum
    {
        Latest,
        FullSquare,
        EmptySquare,
        Line,
        Triangle,
        Other,
        Bordel
    }

    public class ArmyController : Item
    {
public SleepingBody m_SleepingBody;
ArrayList sleeping = new ArrayList();
ArrayList sleepingbod = new ArrayList();
        private Mobile m_Owner;
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

        private bool m_War;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool War { get { return m_War; } set { m_War = value; } }

        private bool m_Free;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Free { get { return m_Free; } set { m_Free = value; } }

        private Point3D m_ArmyHome;
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D ArmyHome { get { return m_ArmyHome; } set { m_ArmyHome = value; } }

        private Direction m_ArmyDirection;
        [CommandProperty(AccessLevel.GameMaster)]
        public Direction ArmyDirection { get { return m_ArmyDirection; } set { m_ArmyDirection = value; } }

        private Direction m_ArmyFormationDirection;
        [CommandProperty(AccessLevel.GameMaster)]
        public Direction ArmyFormationDirection { get { return m_ArmyFormationDirection; } set { m_ArmyFormationDirection = value; } }

        private ArmyFormationEnum m_CurrentFormation;
        [CommandProperty(AccessLevel.GameMaster)]
        public ArmyFormationEnum CurrentFormation { get { return m_CurrentFormation; } set { m_CurrentFormation = value; } }

        public ArrayList Soldiers = new ArrayList();

        [CommandProperty(AccessLevel.GameMaster)]
        public int ArmySize { get { return Soldiers.Count; } }

        [Constructable]
        public ArmyController()
            : base(3007)
        {
            Weight = 0.0;
            Name = "Army Controller";
            m_Owner = null;
            m_ArmyDirection = Direction.North;
            m_ArmyFormationDirection = Direction.North;
            m_ArmyDirection = Direction.North;
            m_CurrentFormation = ArmyFormationEnum.FullSquare;
        }

        public void GoThere(Point3D point)
        {
            m_ArmyHome = point;
            int xx = 0;
            int yy = 0;
            int x = 0;
            int y = 0;
            int a = 0;
            int b = 0;
            double c = 0.0;
            a = Soldiers.Count;
            b = (int)Math.Sqrt((double)Soldiers.Count);
            c = ((double)a) / 6.292;

            for (int i = 0; i < a; i++)
            {
                BaseCreature soldier = (BaseCreature)Soldiers[i];
                switch (m_ArmyFormationDirection)
                {
                    case Direction.North: xx = x; yy = y; break;
                    case Direction.Right: xx = x + y; yy = -x + y; break;
                    case Direction.East: xx = y; yy = -x; break;
                    case Direction.Down: xx = -x + y; yy = -x - y; break;
                    case Direction.South: xx = -x; yy = -y; break;
                    case Direction.Left: xx = -x - y; yy = x - y; break;
                    case Direction.West: xx = -y; yy = x; break;
                    case Direction.Up: xx = x - y; yy = x + y; break;
                    default: break;
                }
                if (!(soldier.AIObject is ArmyAI))
                    soldier.ChangeAIType(AIType.AI_Army);
                ((ArmyAI)(soldier.AIObject)).ArmyHome = new Point3D(point.X + xx, point.Y + yy, point.Z);
                switch (m_CurrentFormation)
                {
                    case ArmyFormationEnum.Latest:
                        break;
                    case ArmyFormationEnum.FullSquare:
                        if (x == 0)
                        {
                            x = y + 1;
                            y = 0;
                        }
                        else if (y < x)
                        {
                            y++;
                        }
                        else
                        {
                            x--;
                        }
                        break;
                    case ArmyFormationEnum.EmptySquare:
                        if ((x < a / 4) && (y == 0))
                        {
                            x = x + 1;
                        }
                        else if ((x == a / 4) && (y < a / 4))
                        {
                            y++;
                        }
                        else if ((x > 0) && (y == a / 4))
                        {
                            x--;
                        }
                        else
                        {
                            y--;
                        }
                        break;
                    case ArmyFormationEnum.Line:
                        x++;
                        break;
                    case ArmyFormationEnum.Triangle:
                        if (x == 0)
                        {
                            x = y + 1;
                            y = 0;
                        }
                        else
                        {
                            y = x;
                            x = 0;
                        }
                        break;
                    case ArmyFormationEnum.Other:
                        x = (int)((c + 0.5) * Math.Cos((double)(i + 1) / c) - (c + 0.5));
                        y = (int)((c + 0.5) * Math.Sin((double)(i + 1) / c));
                        break;
                    case ArmyFormationEnum.Bordel:
                        {
                            x = Utility.RandomMinMax(0, b * 2);
                            y = Utility.RandomMinMax(0, b * 2);
                        }
                        break;
                    default:
                        break;
                }
            }
            //CheckSoldiersStatus();
        }

        public void SetDirection(Direction direction)
        {
            m_ArmyDirection = direction;
            for (int i = Soldiers.Count - 1; i >= 0; i--)
            {
                BaseCreature soldier = (BaseCreature)Soldiers[i];
                if (soldier.Direction != m_ArmyDirection)
                {
                    if (!(soldier.AIObject is ArmyAI))
                        soldier.ChangeAIType(AIType.AI_Army);
                    ((ArmyAI)(soldier.AIObject)).ArmyDirection = m_ArmyDirection;
                }
            }
            UpdateAction();
        }

        public void Recruit(Mobile from, BaseCreature soldier)
        {
            if (!Soldiers.Contains(soldier))
            {
                if (soldier is BaseSoldier)
                {
                    BaseSoldier recruit = soldier as BaseSoldier;
                    if (from == recruit.GetOwner())
                    {
                        //if (Soldiers.Count + 1 <= CalculateMaxFollowers(from))
                        //{
                            Soldiers.Add(soldier);
                            //soldier.ChangeAIType(AIType.AI_Army);
                            ((BaseCreature)soldier).AI = AIType.AI_Army;
                            //soldier.Controlled = false;
                            //soldier.ControlMaster = null;
                            from.SendMessage("Another Recruit has joined the Army.", recruit.Title);
                            SetFormation(ArmyFormationEnum.Latest);
                            SetDirection(m_ArmyDirection);
                        //}
                        //else
                            /*TEMP*/
                            //soldier.Say("You have {0} soldiers already, and can only have {1}!", ArmySize, CalculateMaxFollowers(from));
                            //soldier.Say("Your army is bigger then your head!");
                    }
                    else
                        soldier.Say("You need to hire me first!");
                }
                else
                    soldier.Say("I am not for hire!");
            }
            else
            {
                soldier.Say("Sir, I am already in the Army!");
            }
            //TargetRecruit(from);
        }

        private int[] FameLevels = new int[] { 15000,14000,13000,12000,11000,10000,9000,8000,7000,
            6000,5000,4000,3000,2500,2000,1500,1000,500,250,0 };

        private int[] ArmyLimit = new int[] { 60, 57, 54, 51, 48, 45, 42, 39, 36, 33, 30, 27, 24, 21, 18, 15, 12, 9, 6, 3 };

        //REMOVE ME: private bool TestBool = false; //Use this so I only have to read all the Console lines once.
        //REMOVE ME: [CommandProperty(AccessLevel.Administrator)]
        //REMOVE ME: public bool SetTestBool { get { return TestBool; } set { TestBool = value; } }

        private int CalculateMaxFollowers(Mobile from)
        {
            //REMOVE ME: if (!TestBool) Console.WriteLine("About to calculate Max Army Size.");
            for (int x = 0; x < FameLevels.Length; x++)
            {
                if (from.Fame >= FameLevels[x])
                {
                    #region Test Console Messages
                    //REMOVE ME: if (!TestBool)
                    //REMOVE ME: {
                    //REMOVE ME:     Console.WriteLine("from.Fame - {0} is greater than or equal to FameLevels - {1}.",
                    //REMOVE ME:         from.Fame.ToString(), FameLevels[x].ToString());
                    //REMOVE ME:     Console.WriteLine("from.Followers = {0}.", from.Followers);
                    //REMOVE ME:     Console.WriteLine("6 - from.Followers = {0}.",((int)(6 - from.Followers)).ToString());
                    //REMOVE ME:     Console.WriteLine("ArmyLimit[{0}] = {1}.", x.ToString(), ArmyLimit[x].ToString());
                    //REMOVE ME:     Console.WriteLine("So returning Army Size of {0}.", (ArmyLimit[x] * (6 - from.Followers) / 6));
                    //REMOVE ME: }
                    //REMOVE ME: TestBool = true;
                    //REMOVE ME: Console.WriteLine("Returning Army Size of {0}.", (ArmyLimit[x]*(6 - from.Followers) / 6));
                    #endregion
                    return (ArmyLimit[x] * (6 - from.Followers) / 6);
                }
                //REMOVE ME: if (!TestBool) Console.WriteLine("Iteration {0}: FameLevels[x] is {1}. from.Fame is {2}, so go again.",
                //REMOVE ME:    x.ToString(),FameLevels[x].ToString(),from.Fame.ToString());
            }
            //REMOVE ME: TestBool = true;
            return 0;
        }

        public void Attack(Mobile from, Mobile mob)
        {
            for (int i = Soldiers.Count - 1; i >= 0; i--)
            {
                BaseCreature soldier = (BaseCreature)Soldiers[i];
                if (soldier.AIObject != null)
                {
                    soldier.FocusMob = mob;
                    soldier.Combatant = mob;
                    soldier.FocusMob = null;
                }
            }
            m_Free = true;
            SetWar();
        }

        public void TargetGoTo(Mobile from)
        {
            //CheckSoldiersStatus();
            from.SendMessage("Select a direction to go.");
            from.Target = new ArmyGoToTarget(this);
        }

        public void TargetAttack(Mobile from)
        {
            from.SendMessage("Select a creature to attack.");
            from.Target = new ArmyAttackTarget(this);
        }

        public void TargetRecruit(Mobile from)
        {
            from.SendMessage("Select a person to recruit.");
            from.Target = new ArmyRecruitTarget(this);
        }

        public void SetFormation(ArmyFormationEnum formation)
        {
            m_Free = false;
            if (formation != ArmyFormationEnum.Latest)
            {
                if (m_CurrentFormation == formation)
                    m_ArmyFormationDirection = (Direction)((1 + (int)m_ArmyFormationDirection) % 8);
                else
                    m_CurrentFormation = formation;
                if (m_CurrentFormation == ArmyFormationEnum.Triangle)
                    SetDirection((Direction)((7 + (8 - (int)m_ArmyFormationDirection)) % 8));
                else
                    SetDirection((Direction)(((8 - (int)m_ArmyFormationDirection)) % 8));
            }
            GoThere(m_ArmyHome);
            UpdateAction();
        }

        public void SetWar()
        {
            m_War = true;
            UpdateAction();
        }

        public void SetPeace()
        {
            m_War = false;
            UpdateAction();
        }

        public void SetFree()
        {
            m_Free = true;
            UpdateAction();
        }

        public void UpdateAction()
        {
            //CheckSoldiersStatus();
            for (int i = Soldiers.Count - 1; i >= 0; i--)
            {
                BaseCreature soldier = (BaseCreature)Soldiers[i];
                if (Free)
                    if (m_War)
                    {
                        soldier.AIObject.Action = ActionType.Combat;
                        if (soldier.AIObject is ArmyAI)
                            ((ArmyAI)(soldier.AIObject)).ArmyFightMode = FightMode.Aggressor;//closest
                        soldier.Team = Serial;
                    }
                    else
                    {
                        soldier.AIObject.Action = ActionType.Wander;
                        if (soldier.AIObject is ArmyAI)
                            ((ArmyAI)(soldier.AIObject)).ArmyFightMode = FightMode.Aggressor;
                        soldier.Team = 0;
                        soldier.Combatant = null;
                    }
                else
                    if (m_War)
                    {
                        soldier.AIObject.Action = ActionType.Guard;
                        if (soldier.AIObject is ArmyAI)
                            ((ArmyAI)(soldier.AIObject)).ArmyFightMode = FightMode.Aggressor; //closest
                        soldier.Team = Serial;
                    }
                    else
                    {
                        soldier.AIObject.Action = ActionType.Backoff;
                        if (soldier.AIObject is ArmyAI)
                            ((ArmyAI)(soldier.AIObject)).ArmyFightMode = FightMode.Aggressor;
                        soldier.Team = 0;
                        soldier.Combatant = null;
                    }
            }
        }
        
        public void Sleeping()
        {
            for (int i = Soldiers.Count - 1; i >= 0; i--)
            {
                        BaseCreature soldier = (BaseCreature)Soldiers[i];
                        if ( !(soldier.Alive))
                        {
                            return;
                        }
                        else
                        {
                        soldier.Direction = Direction.North;
                        m_SleepingBody = new SleepingBody( soldier, false, false );
			m_SleepingBody.MoveToWorld( ((BaseSoldier)soldier).HouseLocation, ((BaseSoldier)soldier).HouseMap );
                        this.sleeping.Add( soldier );
                        this.sleepingbod.Add( m_SleepingBody );
			soldier.Map = Map.Internal;
                        }
            }
            return;
        }
        
        public void Wakeup()
		{
                        if ( sleeping != null ) // Delete all items needed
			{
				foreach( Mobile i in sleeping )
				{
                                        //this.sleeping.Remove( i );
					((BaseSoldier)i).MoveToWorld( ((BaseSoldier)i).Home, ((BaseSoldier)i).HouseMap );
				}
			}
                        if ( sleepingbod != null ) // Delete all items needed
			{
				foreach( Item i in sleepingbod )
				{
                                        //this.sleepingbod.Remove( i );
					i.Delete();
				}
			}
		}
  
        public void Disband(bool toDelete)
        {
            for (int i = Soldiers.Count - 1; i >= 0; i--)
            {
                BaseCreature soldier = (BaseCreature)Soldiers[i];
                soldier.ChangeAIToDefault();
                soldier.Controlled = false;
                soldier.ControlMaster = null;
                soldier.Team = 0;
                Soldiers.RemoveAt(i);
                soldier.Delete();
            }
            if (toDelete)
                Delete();
        }

        public void PromptSay(Mobile from)
        {
            from.SendMessage("What do you want the Army to say?:");
            from.Prompt = new ArmySayPrompt(this);
        }

        public void ArmySay(string text)
        {
            //CheckSoldiersStatus();
            for (int i = 0; i < Soldiers.Count; i++)
            {
                //if (Utility.RandomDouble() > 0.7)
                    ((BaseCreature)(Soldiers[i])).Say(text);
            }
        }

        /*public void CheckSoldiersStatus()
        {
            bool finnished = false;
            while (!finnished)
            {
                if (Soldiers.Count <= 0)
                    finnished = true;
                else
                    for (int i = 0; i < Soldiers.Count; i++)
                    {
                        BaseCreature soldier = (BaseCreature)Soldiers[i];
                        if (soldier.Deleted || !(soldier.Alive))
                        {
                            Soldiers.RemoveAt(i);
                            i = Soldiers.Count;
                        }
                        else
                        {
                            if (i >= Soldiers.Count - 1)
                                finnished = true;
                        }
                    }
            }
            int delete = 0;
            int y = Soldiers.Count;
            for (int i = 0; i < y; i++)
            {
                BaseCreature soldier = (BaseCreature)Soldiers[i];
                if (ArmySize > CalculateMaxFollowers(this.Owner))
                {
                    delete++;
                    soldier.Say("My commander can no longer lead me.");
                    y--;
                }
            }
            if (delete > 0)
            {
                for (int x = 0; x < delete; x++)
                {
                    Soldiers.RemoveAt(x);
                }
            }
        }*/

        public override void OnDoubleClick(Mobile from)
        {
            //if (from.AccessLevel >= AccessLevel.GameMaster)
                m_Owner = from;
            if (m_Owner == null)
            {
                from.SendMessage("This is not yours. You have been reported.");
                if (m_Owner != null) m_Owner.SendMessage("{0} Tried to illegaly use an ArmyController.", from.Name);
                //Delete();
                PageQueue.Enqueue(new PageEntry(from, "Ce Page a ete envoye par le server: ce joueur a essaye d'utiliser un ArmyControler qui n'etait pas le sien.", PageType.Other));
            }
            else
            {
                if (from == m_Owner)
                {
                    //if(ArmyGump != null)
                    //{
                    from.CloseGump( typeof( ArmyGump ) );
                    //}
                    from.SendGump(new ArmyGump(this));
                }
                else
                {
                    from.SendMessage("This is not yours. You have been reported.");
                    if (m_Owner != null) m_Owner.SendMessage("{0} Tried to illegaly use an ArmyController.", from.Name);
                    //Delete();
                    PageQueue.Enqueue(new PageEntry(from, "Ce Page a ete envoye par le server: ce joueur a essaye d'utiliser un ArmyControler qui n'etait pas le sien.", PageType.Other));
                }
            }
        }

        public override void OnDelete()
        {
            Disband(false);
            if (m_Owner != null)
                m_Owner.CloseGump(typeof(ArmyGump));
            if ( sleepingbod != null ) // Delete all items needed
			{
				foreach( Item i in sleepingbod )
				{
					i.Delete();
				}
			}
            base.OnDelete();
        }

        public ArmyController(Serial serial): base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version
            writer.WriteMobileList(sleeping);
            writer.WriteItemList(sleepingbod);
            writer.Write(m_ArmyHome);
            writer.Write(m_Owner);
            writer.Write(m_War);
            writer.Write(m_Free);
            writer.Write((int)m_ArmyDirection);
            writer.Write((int)m_ArmyFormationDirection);
            writer.Write((int)m_CurrentFormation);
            writer.WriteMobileList(Soldiers);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
            case 2:
                    sleeping = reader.ReadMobileList();
                    sleepingbod = reader.ReadItemList();
                    goto case 1;
                case 1:
                    m_ArmyHome = reader.ReadPoint3D();
                    goto case 0;
                case 0:
                    m_Owner = reader.ReadMobile();
                    m_War = reader.ReadBool();
                    m_Free = reader.ReadBool();
                    m_ArmyDirection = (Direction)reader.ReadInt();
                    m_ArmyFormationDirection = (Direction)reader.ReadInt();
                    m_CurrentFormation = (ArmyFormationEnum)reader.ReadInt();
                    Soldiers = reader.ReadMobileList();
                    break;
                default:
                    break;
            }
        }
    }
    ///////////////////////////////////////////////////
    public class ArmySayPrompt : Prompt
    {
        private ArmyController m_ArmyController;

        public ArmySayPrompt(ArmyController controller)
        {
            m_ArmyController = controller;
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (m_ArmyController != null)
                if (!m_ArmyController.Deleted)
                    if (m_ArmyController.Owner != null)
                        if (m_ArmyController.Owner.Alive && !(m_ArmyController.Owner.Deleted))
                            if (m_ArmyController.Owner == from)
                                m_ArmyController.ArmySay(text);
        }
    }
    ////////////////////////////////////////////////////
    public class ArmyGoToTarget : Target
    {
        private ArmyController m_ArmyController;

        public ArmyGoToTarget(ArmyController controller)
            : base(-1, true, TargetFlags.None)
        {
            m_ArmyController = controller;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_ArmyController != null)
                if (!m_ArmyController.Deleted)
                    if (m_ArmyController.Owner != null)
                        if (m_ArmyController.Owner.Alive && !(m_ArmyController.Owner.Deleted))
                            if (m_ArmyController.Owner == from)
                                if (target is IPoint3D)
                                {
                                    IPoint3D p = target as IPoint3D;
                                    Point3D p3d = new Point3D(p);
                                    m_ArmyController.GoThere(p3d);
                                }
        }
    }
    ////////////////////////////////////////////////////
    public class ArmyRecruitTarget : Target
    {
        private ArmyController m_ArmyController;

        public ArmyRecruitTarget(ArmyController controller)
            : base(-1, false, TargetFlags.None)
        {
            m_ArmyController = controller;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_ArmyController != null)
                if (!m_ArmyController.Deleted)
                    if (m_ArmyController.Owner != null)
                        if (m_ArmyController.Owner.Alive && !(m_ArmyController.Owner.Deleted))
                            if (m_ArmyController.Owner == from)
                                if (target is BaseCreature)
                                {
                                    m_ArmyController.Recruit(from, (BaseCreature)target);
                                }
        }
    }
    ////////////////////////////////////////////////////
    public class ArmyAttackTarget : Target
    {
        private ArmyController m_ArmyController;

        public ArmyAttackTarget(ArmyController controller)
            : base(-1, false, TargetFlags.None)
        {
            m_ArmyController = controller;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_ArmyController != null)
                if (!m_ArmyController.Deleted)
                    if (m_ArmyController.Owner != null)
                        if (m_ArmyController.Owner.Alive && !(m_ArmyController.Owner.Deleted))
                            if (m_ArmyController.Owner == from)
                                if (target is Mobile)
                                {
                                    m_ArmyController.Attack(from, (Mobile)target);
                                }
        }
    }
    ////////////////////////////////////////////////////////
}


