using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Provider.Fields;
using Edelstein.WvsGame.Fields.Objects.Users;
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
            this.ID = id;
            this.Template = template;
            this._objects = new List<FieldObject>();
        }

        public void Enter(FieldObject obj)
        {
            lock (this)
            {
                obj.Field?.Leave(obj);

                obj.ID = Interlocked.Increment(ref _runningObjectID);
                Console.WriteLine(obj.ID);
                obj.Field = this;

                if (obj is FieldUser user)
                {
                    var portal = Template.Portals[user.Character.FieldPortal] ??
                                 Template.Portals.Values.First(p => p.Type == FieldPortalType.Spawn);

                    user.X = (short) portal.X;
                    user.Y = (short) portal.Y;
                    user.SendPacket(user.GetSetFieldPacket());
                    BroadcastPacket(user, user.GetEnterFieldPacket());

                    if (!user.Socket.IsInstantiated) user.Socket.IsInstantiated = true;

                    this._objects
                        .Where(o => !o.Equals(obj))
                        .ForEach(o => user.SendPacket(o.GetEnterFieldPacket()));
                }
                else BroadcastPacket(obj.GetEnterFieldPacket());

                this._objects.Add(obj);
            }
        }

        public void Leave(FieldObject obj)
        {
            lock (this)
            {
                if (obj is FieldUser user) BroadcastPacket(user, user.GetLeaveFieldPacket());
                else BroadcastPacket(obj.GetLeaveFieldPacket());

                this._objects.Remove(obj);
            }
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