using System;

namespace TwitterDataContract
{
    public class ObjectType
    {
        #region Public Properties

        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the identifier of the time.
        /// </summary>
        /// <value>The identifier of the time.</value>
        /// ------------------------------------------------------------------------------------------------
        public DateTime TimeId { get; set; }

        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        /// ------------------------------------------------------------------------------------------------
        public Object Value { get; set; }

        #endregion Public Properties
    }
}