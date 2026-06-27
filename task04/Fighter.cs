namespace task04
{
    public class Fighter : ISpaceship
    {
        private int _position;
        private int _angle;
        private int _FireCount;

        public int Speed => 100;
        public int FirePower => 50;

        public int Position => _position;
        public int Angle => _angle;
        public int FireCount => _FireCount;

        public void MoveForward() => _position += Speed;

        public void Rotate(int angle)
        {
            _angle = (_angle + angle) % 360;
            if (_angle < 0)
                _angle += 360;
        }

        public void Fire() => _FireCount++;
    }
}


