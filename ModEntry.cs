using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace IndoorSprinklers
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
         ** Public methods
         *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        }


        /*********
         ** Private methods
         *********/
        

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.RunSprinklers();
            this.Monitor.Log("Ran sprinklers");
        }

        private IEnumerable<GameLocation> GetIndoorLocations()
        {
            return Game1.locations.Where(
                location => !location.IsOutdoors
            );
        }
    
        private void RunSprinklers()
        {
            foreach (var location in GetIndoorLocations())
            {
                foreach (var sprinkler in location.objects.Values.Where(o => o.IsSprinkler()))
                {
                    foreach (var sprinklerCoveredTile in sprinkler.GetSprinklerTiles())
                    {
                        foreach (var gameObject in location.objects.Pairs.Where(
                                     gameObject => gameObject.Key.Equals(sprinklerCoveredTile)))
                        {
                            gameObject.Value.ApplySprinkler(gameObject.Key);
                        }
                        
                    }
                }
            }
        }
    }
}