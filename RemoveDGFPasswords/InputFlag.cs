/// <summary>
/// Remove DGF passwords namespace
/// </summary>
namespace RemoveDGFPasswords
{
    /// <summary>
    /// Input flag structure
    /// </summary>
    public struct InputFlag
    {
        /// <summary>
        /// Description
        /// </summary>
        private string description;

        /// <summary>
        /// Full description
        /// </summary>
        private string fullDescription;

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                if (description == null)
                {
                    description = string.Empty;
                }
                return description;
            }
        }

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription
        {
            get
            {
                if (fullDescription == null)
                {
                    fullDescription = string.Empty;
                }
                return fullDescription;
            }
        }

        /// <summary>
        /// Inpuit flag
        /// </summary>
        public EInputFlag Flag { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="description">Description</param>
        /// <param name="fullDescription">Full description</param>
        /// <param name="flag">Input flag</param>
        public InputFlag(string description, string fullDescription, EInputFlag flag)
        {
            this.description = ((description == null) ? string.Empty : description);
            this.fullDescription = ((fullDescription == null) ? string.Empty : fullDescription);
            Flag = flag;
        }
    }
}
