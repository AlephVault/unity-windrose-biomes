using AlephVault.Unity.WindRose.Types;


namespace AlephVault.Unity.WindRose.Biomes
{
    namespace Types
    {
        /// <summary>
        ///   This exception is thrown when a biome is not set on behaviours
        ///   that make use of them.
        /// </summary>
        public class MissingBiomeSetException : Exception
        {
            public MissingBiomeSetException() { }
            public MissingBiomeSetException(string message) : base(message) { }
            public MissingBiomeSetException(string message, Exception inner) : base(message, inner) { }
        }
    }
}