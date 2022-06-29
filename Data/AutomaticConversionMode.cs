namespace SettlementCultureChanger.Data
{
    public enum AutomaticConversionMode
    {
        /// <summary>
        /// Conversion takes place over a randomly allotted amount of time.
        /// </summary>
        Timed,
        /// <summary>
        /// Conversion takes place immediately whenever ownership is transferred. 
        /// </summary>
        Immediate,
    }
}