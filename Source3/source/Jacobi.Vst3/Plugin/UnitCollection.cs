using System.Collections.ObjectModel;

namespace Jacobi.Vst3.Plugin
{
    public class UnitCollection : KeyedCollection<int, Unit>
    {
        public Unit Parent { get; set; }

        public Unit GetAt(int index)
        {
            var baseThis = (Collection<Unit>)this;
            return baseThis[index];
        }

        protected override int GetKeyForItem(Unit item)
        {
            if (item == null) return 0;

            return item.Info.Id;
        }

        protected override void InsertItem(int index, Unit item)
        {
            SetParent(item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Unit item)
        {
            SetParent(item);
            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            // detach from parent
            var unit = GetAt(index);
            unit.Parent = null;

            base.RemoveItem(index);
        }

        void SetParent(Unit item)
        {
            if (Parent != null && item != null && item.Parent == null) item.Parent = Parent;
        }
    }
}
