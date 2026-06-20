using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SVI_NFT_R.UI
{
    public sealed class SwapPair
    {
        public Control PrimaryControl { get; private set; }
        public Control SecondaryControl { get; private set; }
        public Point PrimaryLocation { get; private set; }
        public Point SecondaryLocation { get; private set; }
        public ESwapOrigin Status { get; private set; } = ESwapOrigin.Begin;
        public event EventHandler<SwapPairEventArgs> SwapStatusChanged;

        public SwapPair(Control primary, Control secondary)
        {
            Debug.Assert(primary != null);
            Debug.Assert(secondary != null);
            PrimaryControl = primary;
            SecondaryControl = secondary;
            PrimaryLocation = PrimaryControl.Location;
            SecondaryLocation = SecondaryControl.Location;
        }

        public void SetSwap(ESwapOrigin target)
        {
            if (Status == target)
            {
                return;
            }

            switch (target)
            {
                case ESwapOrigin.Begin:
                    PrimaryControl.Location = PrimaryLocation;
                    SecondaryControl.Location = SecondaryLocation;
                    break;
                case ESwapOrigin.Swaped:
                    PrimaryControl.Location = SecondaryLocation;
                    SecondaryControl.Location = PrimaryLocation;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            Status = target;
            raiseSwapStatusChangedEvent();
        }

        public void SetOrigin(ESwapOrigin target)
        {
            SetSwap(target);
            PrimaryLocation = PrimaryControl.Location;
            SecondaryLocation = SecondaryControl.Location;
            Status = ESwapOrigin.Begin;
        }

        private void raiseSwapStatusChangedEvent()
        {
            SwapStatusChanged?.Invoke(this, new SwapPairEventArgs(Status));
        }
    }
}
