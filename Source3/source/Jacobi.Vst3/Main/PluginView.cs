using System;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Plug-In view default implementation.
    /// \ingroup sdkBase
    /// Can be used as base class for an IPlugView implementation.
    /// </summary>
    public class CPluginView : IPlugView
    {
        protected ViewRect rect = new();
        protected IntPtr systemWindow;
        protected IPlugFrame plugFrame;

        public CPluginView(ViewRect? rect = null)
        {
            if (rect != null)
                this.rect = rect.Value;
        }

        /// <summary>
        /// Gets or sets the current frame rectangle.
        /// </summary>
        public ViewRect Rect
        {
            get => rect;
            set => rect = value;
        }

        /// <summary>
        /// Checks if this view is attached to its parent view.
        /// </summary>
        public bool IsAttached => systemWindow != IntPtr.Zero;

        /// <summary>
        /// Calls when this view will be attached to its parent view.
        /// </summary>
        public virtual void AttachedToParent() { }

        /// <summary>
        /// Calls when this view will be removed from its parent view.
        /// </summary>
        public virtual void RemovedFromParent() { }

        #region IPlugView

        public virtual TResult IsPlatformTypeSupported(string type) => kNotImplemented;

        public virtual TResult Attached(IntPtr parent, string type)
        {
            systemWindow = parent;

            AttachedToParent();
            return kResultOk;
        }

        public virtual TResult Removed()
        {
            systemWindow = IntPtr.Zero;

            RemovedFromParent();
            return kResultOk;
        }

        public virtual TResult OnWheel(float distance) => kResultFalse;
        public virtual TResult OnKeyDown(char key, short keyMsg, short modifiers) => kResultFalse;
        public virtual TResult OnKeyUp(char key, short keyMsg, short modifiers) => kResultFalse;

        public virtual TResult OnSize(ref ViewRect newSize) { rect = newSize; return kResultTrue; }
        public virtual TResult GetSize(out ViewRect size) { size = rect; return kResultTrue; }

        public virtual TResult OnFocus(bool state) => kResultFalse;
        public virtual TResult SetFrame(IPlugFrame frame) { plugFrame = frame; return kResultTrue; }

        public virtual TResult CanResize() => kResultFalse;
        public virtual TResult CheckSizeConstraint(ref ViewRect rect) => kResultFalse;

        #endregion
    }
}
