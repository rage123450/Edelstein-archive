using System.Collections.Generic;
using System.Threading;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Fields
{
    public class FieldObjPool
    {
        private int _runningObjectID = 1;
        private readonly List<FieldObj> _objects;
        public IEnumerable<FieldObj> Objects => _objects.AsReadOnly();

        public FieldObjPool()
        {
            _objects = new List<FieldObj>();
        }

        public void Enter(FieldObj obj)
        {
            if (obj is FieldUser user) obj.ID = user.Character.ID;
            else
            {
                Interlocked.Increment(ref _runningObjectID);
                if (_runningObjectID == int.MinValue)
                    Interlocked.Exchange(ref _runningObjectID, 1);

                obj.ID = _runningObjectID;
            }

            _objects.Add(obj);
        }

        public void Leave(FieldObj obj) => _objects.Remove(obj);
    }
}