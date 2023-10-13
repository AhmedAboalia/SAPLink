namespace SAPLink.Handler.SAP.Interfaces
{
    public interface IServiceLayerHandler
    {
        #region Login

        /// <summary>
        /// Establishes a connection to the Service Layer.
        /// </summary>
        void Connect();

        /// <summary>
        /// Is Connected With Service Layer
        /// </summary>
        /// <returns>'true' if connected to service layer, and 'false' if disconnected.</returns>
        bool Connected();

        /// <summary>
        /// ReConnect to Service Layer
        /// </summary>
        void ReConnect();

        /// <summary>
        /// Disconnect from Service Layer
        /// </summary>
        void Disconnect();

        #endregion

        #region Session

        /// <summary>
        /// Get Session ID from Service Layer.
        /// </summary>
        string SessionId();

        /// <summary>
        /// Get Session Timeout from Service Layer.
        /// </summary>
        string SessionVersion();

        /// <summary>
        /// Get Session Version from Service Layer.
        /// </summary>
        int SessionTimeout();

        #endregion

    }
}
