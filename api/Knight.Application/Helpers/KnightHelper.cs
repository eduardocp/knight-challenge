namespace Knight.Application.Helpers
{
    internal static class KnightHelper
    {
        private record ModMap(int Min, int Max, int Mod);

        private static int GetAttackMod(int keyAttribute)
        {
            var mod = keyAttribute switch
            {
                < 9 => new ModMap(0, 8, -2),
                > 9 and <= 10 => new ModMap(9, 10, -1),
                > 10 and <= 12 => new ModMap(11, 12, 0),
                > 12 and <= 15 => new ModMap(13, 15, 1),
                > 15 and <= 18 => new ModMap(16, 18, 2),
                _ => new ModMap(19, 20, 3)
            };

            return mod.Mod;
        }

        public static int GetKnightAttack(Domain.Models.Entities.Knight knight)
        {
            var keyAttribute = knight.KeyAttribute.Trim().ToLower() switch
            {
                "strength" => knight.Attributes.Strength,
                "dexterity" => knight.Attributes.Dexterity,
                "constitution" => knight.Attributes.Constitution,
                "intelligence" => knight.Attributes.Intelligence,
                "wisdom" => knight.Attributes.Wisdom,
                "charisma" => knight.Attributes.Charisma,
                _ => throw new NotImplementedException(),
            };
            var weaponAttack = knight.Weapons.FirstOrDefault(x => x.Equipped)?.Mod ?? 0;

            return 10 + GetAttackMod(keyAttribute) + weaponAttack;
        }
    }
}