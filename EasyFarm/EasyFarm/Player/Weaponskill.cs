namespace EasyFarm.PlayerTools
{
    public class WeaponAbility : Ability
    {
        public WeaponAbility() { }
        
        public WeaponAbility(string name, double distance) : base(name) 
        {
            MaxDistance = distance;
        }
        
        /// <summary>
        /// Max distance we cna use a weaponskill at
        /// </summary>
        public double MaxDistance;

        /// <summary>
        /// Can we use the weaponskill?
        /// </summary>
        public bool IsEnabled;
    }
}
