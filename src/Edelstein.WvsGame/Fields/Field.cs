using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Provider.Fields;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Drops;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields
{
    public class Field
    {
        public int ID { get; set; }
        public FieldTemplate Template { get; }
        private int _runningObjectID = 1;
        private readonly List<FieldObject> _objects;
        public IEnumerable<FieldObject> Objects => _objects.AsReadOnly();

        public Field(int id, FieldTemplate template)
        {
            ID = id;
            Template = template;
            _objects = new List<FieldObject>();
        }

        public bool OnPacket(FieldUser user, GameRecvOperations operation, InPacket packet)
        {
            switch (operation)
            {
                case GameRecvOperations.MobMove:
                {
                    var objectID = packet.Decode<int>();
                    var mob = Objects
                        .OfType<FieldMob>()
                        .FirstOrDefault(m => m.ID == objectID);
                    return mob?.OnPacket(user, operation, packet) ?? true;
                }
                case GameRecvOperations.NpcMove:
                {
                    var objectID = packet.Decode<int>();
                    var npc = Objects
                        .OfType<FieldNPC>()
                        .FirstOrDefault(n => n.ID == objectID);
                    return npc?.OnPacket(user, operation, packet) ?? true;
                }
                case GameRecvOperations.ReactorHit:
                case GameRecvOperations.ReactorTouch:
                {
                    var objectID = packet.Decode<int>();
                    var reactor = Objects
                        .OfType<FieldReactor>()
                        .FirstOrDefault(n => n.ID == objectID);
                    return reactor?.OnPacket(user, operation, packet) ?? true;
                }
                case GameRecvOperations.DropPickUpRequest:
                    OnDropPickUpRequest(user, packet);
                    break;
                default:
                    return user.OnPacket(operation, packet);
            }

            return true;
        }

        private void OnDropPickUpRequest(FieldUser user, InPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();
            packet.Decode<short>();
            packet.Decode<short>();
            var objectID = packet.Decode<int>();
            packet.Decode<int>();
            var drop = Objects
                .OfType<FieldDrop>()
                .FirstOrDefault(n => n.ID == objectID);

            drop?.PickUp(user);
            Leave(drop, () => drop?.GetLeaveFieldPacket(0x2, user));
        }

        public void Enter(FieldObject obj, Func<OutPacket> getEnterPacket = null)
        {
            lock (this)
            {
                obj.Field?.Leave(obj);
                obj.Field = this;

                if (obj is FieldUser user)
                {
                    var portal = Template.Portals.Values.FirstOrDefault(p => p.ID == user.Character.FieldPortal) ??
                                 Template.Portals.Values.First(p => p.Type == FieldPortalType.Spawn);

                    user.ID = user.Character.ID;
                    user.Character.FieldID = ID;
                    user.X = (short) portal.X;
                    user.Y = (short) portal.Y;

                    if (portal.Type != FieldPortalType.Spawn)
                    {
                        var foothold = Template.Footholds.Values
                            .Where(f => f.X1 <= portal.X && f.X2 >= portal.X)
                            .First(f => f.X1 < f.X2);

                        user.Foothold = (short) foothold.ID;
                    }

                    user.SendPacket(user.GetSetFieldPacket());
                    BroadcastPacket(user, getEnterPacket?.Invoke() ?? user.GetEnterFieldPacket());

                    if (!user.Socket.IsInstantiated) user.Socket.IsInstantiated = true;
                    user.ResetForcedStats();

                    _objects
                        .Where(o => !o.Equals(obj))
                        .ForEach(o => user.SendPacket(o.GetEnterFieldPacket()));
                }
                else
                {
                    Interlocked.Increment(ref _runningObjectID);
                    if (_runningObjectID == int.MinValue)
                        Interlocked.Exchange(ref _runningObjectID, 1);

                    obj.ID = _runningObjectID;
                    BroadcastPacket(getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket());
                }

                _objects.Add(obj);
                UpdateControlledObjects();
            }
        }

        public void Leave(FieldObject obj, Func<OutPacket> getLeavePacket = null)
        {
            lock (this)
            {
                if (obj is FieldUser user) BroadcastPacket(user, user.GetLeaveFieldPacket());
                else BroadcastPacket(getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket());

                _objects.Remove(obj);
                UpdateControlledObjects();
            }
        }

        public void UpdateControlledObjects()
        {
            var controllers = Objects.OfType<FieldUser>().Shuffle().ToList();
            var controlled = Objects.OfType<FieldObjectControlled>().ToList();

            controlled
                .Where(c => c.Controller == null || !controllers.Contains(c.Controller))
                .ForEach(c => c.ChangeController(controllers.FirstOrDefault()));
        }

        public FieldObject GetObject(int id)
        {
            return Objects
                .Where(o => !(o is FieldUser))
                .SingleOrDefault(o => o.ID == id);
        }

        public FieldUser GetUser(int id)
        {
            return Objects
                .OfType<FieldUser>()
                .SingleOrDefault(o => o.ID == id);
        }

        public Task BroadcastPacket(FieldObject source, OutPacket packet)
        {
            return Task.WhenAll(Objects
                .OfType<FieldUser>()
                .Where(c => !c.Equals(source))
                .Select(c => c.Socket.SendPacket(packet)));
        }

        public Task BroadcastPacket(OutPacket packet)
        {
            return Task.WhenAll(Objects
                .OfType<FieldUser>()
                .Select(c => c.Socket.SendPacket(packet)));
        }
    }
}