namespace Daw.Vst
{
    public class Note
    {
        bool _pressed = false;
        public bool Pressed
        {
            get => _pressed;
            set
            {
                _pressed = value;
                if (!value) PressedTime = 0;
            }
        }

        public int PressedTime; // Note on time
        public byte Velocity;
    }
}
