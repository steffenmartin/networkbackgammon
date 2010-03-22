using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Container for collecting data posted via events (asynchronous)
    /// </summary>
    [Serializable]
    public class NetworkBackgammonEventQueueElement
    {
        #region Members

            /// <summary>
            /// Event that has been posted
            /// </summary>
            INetworkBackgammonEvent gameSessionEvent;

            /// <summary>
            /// Notifier (sender) who posted the event
            /// </summary>
            INetworkBackgammonNotifier gameSessionNotifier;

            #endregion

        #region Methods

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_event">Event that has been posted</param>
            /// <param name="_notifier">Notifier (sender) who posted the event</param>
            public NetworkBackgammonEventQueueElement(INetworkBackgammonEvent _event, INetworkBackgammonNotifier _notifier)
            {
                gameSessionEvent = _event;
                gameSessionNotifier = _notifier;
            }

            #endregion

        #region Properties

            /// <summary>
            /// Gets the event that has been posted
            /// </summary>
            public INetworkBackgammonEvent Event
            {
                get
                {
                    return gameSessionEvent;
                }
            }

            /// <summary>
            /// Gets the notifier (sender) who posted the event
            /// </summary>
            public INetworkBackgammonNotifier Notifier
            {
                get
                {
                    return gameSessionNotifier;
                }
            }

            #endregion
    }
}
