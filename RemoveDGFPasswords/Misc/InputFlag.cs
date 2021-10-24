using System;

/// <summary>
/// Remove DGF passwords namespace
/// </summary>
namespace RemoveDGFPasswords
{
    /// <summary>
    /// A structure that describes an input flag
    /// </summary>
    internal readonly struct InputFlag
    {
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription { get; }

        /// <summary>
        /// Inpuit flag
        /// </summary>
        public EInputFlag Flag { get; }

        /// <summary>
        /// Constructs a new input flag
        /// </summary>
        /// <param name="description">Description</param>
        /// <param name="fullDescription">Full description</param>
        /// <param name="flag">Input flag</param>
        public InputFlag(string description, string fullDescription, EInputFlag flag)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            FullDescription = fullDescription ?? throw new ArgumentNullException(nameof(fullDescription));
            Flag = flag;
        }
    }
}
