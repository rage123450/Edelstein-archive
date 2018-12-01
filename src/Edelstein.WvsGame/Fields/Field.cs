using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Provider.Fields;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Drops;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Packets;
using Edelstein.WvsGame.Utils;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields
{
    public class Field : IUpdateable
    {
        public int ID { get; set; }
        public FieldTemplate Template { get; }
        private readonly IDictionary<Type, FieldObjPool> _pools;
        public Dictionary<Type, FieldObjPool> Pools => _pools.ToDictionary();

        public Field(int id, FieldTemplate template)
        {
            ID = id;
            Template = template;
            _pools = new Dictionary<Type, FieldObjPool>();
        }

        public async Task Update(DateTime now)
        {
            await Task.WhenAll(Pools.Values
                .SelectMany(p => p.Objects)
                .OfType<IUpdateable>()
                .Select(o => o.Update(now))
            );
        }

        public bool OnPacket(FieldUser user, GameRecvOperations operation, InPacket packet)
        {
            switch (operation)
            {
                case GameRecvOperations.MobMove:
                {
                    var objectID = packet.Decode<int>();
                    var mob = GetObject<FieldMob>(objectID);
                    return mob?.OnPacket(user, operation, packet) ?? true;
                }
                case GameRecvOperations.NpcMove:
                {
                    var objectID = packet.Decode<int>();
                    var npc = GetObject<FieldNPC>(objectID);
                    return npc?.OnPacket(user, operation, packet) ?? true;
                }
                case GameRecvOperations.ReactorHit:
                case GameRecvOperations.ReactorTouch:
                {
                    var objectID = packet.Decode<int>();
                    var reactor = GetObject<FieldReactor>(objectID);
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
            var drop = GetObject<FieldDrop>(objectID);

            drop?.PickUp(user);
            Leave(drop, () => drop?.GetLeaveFieldPacket(0x2, user));
        }

        public void Enter<T>(T obj, Func<OutPacket> getEnterPacket = null) where T : FieldObj
        {
            lock (this)
            {
                obj.Field?.Leave(obj);
                obj.Field = this;

                var pool = GetPool<T>();

                if (pool == null)
                {
                    pool = new FieldObjPool();
                    _pools[typeof(T)] = pool;
                }

                pool.Enter(obj);

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
                    GetObjects()
                        .Where(o => o != user)
                        .ForEach(o => user.SendPacket(o.GetEnterFieldPacket()));
                }
                else BroadcastPacket(getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket());

                UpdateControlledObjects();
            }
        }

        public void Leave<T>(T obj, Func<OutPacket> getLeavePacket = null) where T : FieldObj
        {
            lock (this)
            {
                if (obj is FieldUser user) BroadcastPacket(user, user.GetLeaveFieldPacket());
                else BroadcastPacket(getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket());

                GetPool<T>().Leave(obj);
                UpdateControlledObjects();
            }
        }

        public void UpdateControlledObjects()
        {
            var controllers = GetObjects().OfType<FieldUser>().Shuffle().ToList();
            var controlled = GetObjects().OfType<FieldLifeControlled>().ToList();

            controlled
                .Where(
                    c => c.Controller == null ||
                         !controllers.Contains(c.Controller))
                .ForEach(c => c.ChangeController(controllers.FirstOrDefault()));
        }

        public FieldObjPool GetPool<T>() where T : FieldObj
            => Pools.GetValueOrDefault(typeof(T));

        public T GetObject<T>(int id) where T : FieldObj
            => GetObjects<T>().FirstOrDefault(o => o.ID == id);

        public ICollection<T> GetObjects<T>() where T : FieldObj
            => GetPool<T>()?.Objects.Cast<T>().ToList() ?? new List<T>();

        public IEnumerable<FieldObj> GetObjects()
            => Pools.Values.SelectMany(p => p.Objects).ToList();

        public Task BroadcastPacket(FieldObj source, OutPacket packet)
        {
            return Task.WhenAll(GetObjects<FieldUser>()
                .Where(c => !c.Equals(source))
                .Select(c => c.Socket.SendPacket(packet)));
        }

        public Task BroadcastPacket(OutPacket packet)
        {
            return Task.WhenAll(GetObjects<FieldUser>()
                .Select(c => c.Socket.SendPacket(packet)));
        }
    }
}