using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;

namespace Jacobi.Vst3.Plugin
{
    public class ClassRegistration
    {
        // optional
        public ObjectCreatorCallback CreatorCallback { get; set; }

        public Type ClassType { get; set; }

        // GuidAttribute on ClassType
        public Guid ClassTypeId { get; set; }

        // maps to Category
        public ObjectClasses ObjectClass { get; set; }

        public ComponentFlags ClassFlags { get; set; }

        // DisplayName on ClassType
        public string DisplayName { get; set; }

        private PlugType _categories;
        // maps to subCategories
        public PlugType Categories
        {
            get
            {
                if (_categories == null) _categories = new PlugType();
                return _categories;
            }

            set
            {
                if (_categories != null) throw new InvalidOperationException("Categories is already initialized.");
                _categories = value;
            }
        }

        public string Vendor { get; set; }

        public Version Version { get; set; }

        public enum ObjectClasses
        {
            None,
            AudioModuleClass,
            ComponentControllerClass,
            TestClass   // validator (and test host?)
        }
    }
}
