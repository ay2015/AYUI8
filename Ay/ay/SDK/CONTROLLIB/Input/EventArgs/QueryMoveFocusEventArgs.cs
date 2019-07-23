using System.Windows;
using System.Windows.Input;

namespace ay.Controls.Args
{
    public delegate void QueryMoveFocusEventHandler(object sender, QueryMoveFocusEventArgs e);
    public class QueryMoveFocusEventArgs : RoutedEventArgs
    {
        private FocusNavigationDirection m_navigationDirection;

        private bool m_reachedMaxLength;

        private bool m_canMove = true;

        public FocusNavigationDirection FocusNavigationDirection
        {
            get
            {
                return m_navigationDirection;
            }
        }

        public bool ReachedMaxLength
        {
            get
            {
                return m_reachedMaxLength;
            }
        }
        public bool CanMoveFocus
        {
            get
            {
                return m_canMove;
            }
            set
            {
                m_canMove = value;
            }
        }

        private QueryMoveFocusEventArgs()
        {
        }

        internal QueryMoveFocusEventArgs(FocusNavigationDirection direction, bool reachedMaxLength)
            : base(AyTextBoxBase.QueryMoveFocusEvent)
        {
            m_navigationDirection = direction;
            m_reachedMaxLength = reachedMaxLength;
        }
    }
}
