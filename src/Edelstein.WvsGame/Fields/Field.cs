using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Provider.Fields;
using Edelstein.WvsGame.Fields.Users;

namespace Edelstein.WvsGame.Fields
{
    public class Field
    {
        public FieldTemplate Template { get; }
        private int _runningObjectID;
        private readonly List<FieldObject> _objects;
        public IEnumerable<FieldObject> Objects => _objects.AsReadOnly();

        public Field(FieldTemplate template)
        {
            this.Template = template;
            this._objects = new List<FieldObject>();
        }

        public void Enter(FieldObject obj)
        {
            obj.Field?.Leave(obj);

            obj.ID = Interlocked.Increment(ref _runningObjectID);
            obj.Field = this;

            if (obj is FieldUser user)
            {
                user.SendPacket(user.GetSetFieldPacket());
                BroadcastPacket(user, user.GetEnterFieldPacket());
            }
            else BroadcastPacket(obj.GetLeaveFieldPacket());

            this._objects.Add(obj);
        }

        public void Leave(FieldObject obj)
        {
            if (obj is FieldUser user) BroadcastPacket(user, user.GetLeaveFieldPacket());
            else BroadcastPacket(obj.GetLeaveFieldPacket());

            this._objects.Remove(obj);
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