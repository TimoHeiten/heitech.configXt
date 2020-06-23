namespace heitech.configXt.Models
{
    public class ContextModel
    {
        ///<summary>
        /// The current User Publishing the Context
        ///</summary>
        public AuthModel User { get; set; }

        ///<summary>
        /// The Key of the ConfigurationEntry. Must be Null on ContextType Read All. In Upload this needs to be a parseable Indicator (Json, csv etc.)
        ///</summary>
        public string Key { get; set; }
        ///<summary>
        /// Set/Update/Create Value. On ContextType Read, ReadAll or Delete this must be null. On Uplaod this should be the items as string
        ///</summary>
        public string Value { get; set; }

        ///<summary>
        /// Which operation should be performed
        ///</summary>
        public ContextType Type { get; set; }
    }
}