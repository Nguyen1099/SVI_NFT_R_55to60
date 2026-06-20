using System;

namespace EqToEq
{
    public class ReceiveVacuumEventArgs : EventArgs
    {
        public EVacuumCommand Command { get; private set; }
        public EPosition Placement { get; private set; }

        public ReceiveVacuumEventArgs(EVacuumCommand command, EPosition placement)
        {
            Command = command;
            Placement = placement;
        }
    }
}