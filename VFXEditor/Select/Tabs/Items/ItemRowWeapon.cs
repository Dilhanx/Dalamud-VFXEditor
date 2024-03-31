using Lumina.Excel.GeneratedSheets2;

namespace VfxEditor.Select.Tabs.Items {
    public class ItemRowWeapon : ItemRow {
        public readonly ItemRowWeapon SubItem;
        public readonly string OverrideImcPath = null;
        private readonly string ModelString;
        private readonly string BodyString;

        public bool HasSubModel => SecondaryIds.Id1 != 0;

        public override string RootPath => $"chara/weapon/{ModelString}/obj/body/{BodyString}/vfx/eff/vw";

        public override string ImcPath => !string.IsNullOrEmpty( OverrideImcPath ) ? OverrideImcPath : $"chara/weapon/{ModelString}/obj/body/{BodyString}/{BodyString}.imc";

        public override int Variant => Ids.WeaponVariant;

        public string PapPath => $"chara/weapon/{ModelString}/animation/a0001/wp_common/resident/weapon.pap";

        public string MdlPath => $"chara/weapon/{ModelString}/obj/body/{BodyString}/model/{ModelString}{BodyString}.mdl";

        public ItemRowWeapon( string name, uint rowId, ushort icon, ItemIds ids, ItemIds secondaryIds, EquipSlotCategory category, string imcPath = "" ) :
            base( name, rowId, icon, ids, secondaryIds, category ) {

            OverrideImcPath = imcPath;
            ModelString = "w" + Ids.Id.ToString().PadLeft( 4, '0' );
            BodyString = "b" + Ids.WeaponBody.ToString().PadLeft( 4, '0' );

        }

        public ItemRowWeapon( Item item, string imcPath = "" ) : base( item ) {
            OverrideImcPath = imcPath;
            ModelString = "w" + Ids.Id.ToString().PadLeft( 4, '0' );
            BodyString = "b" + Ids.WeaponBody.ToString().PadLeft( 4, '0' );

            // ======================================

            if( !HasSubModel ) return;
            var category = item.ItemUICategory.Value.RowId;
            var doubleHand = ( category == 1 || category == 84 || category == 107 ); // MNK, NIN, DNC weapons

            SubItem = new ItemRowWeapon(
                $"{Name} (Offhand)",
                item.RowId,
                Icon,
                doubleHand ? Ids with { Id1 = Ids.Id1 + 50 } : SecondaryIds,
                new( 0 ),
                item.EquipSlotCategory.Value,
                doubleHand ? ImcPath : null );
        }

        public string GetMtrlPath( int id, string suffix ) => $"chara/weapon/{ModelString}/obj/body/{BodyString}/material/v" + id.ToString().PadLeft( 4, '0' ) + $"/mt_{ModelString}{BodyString}_{suffix}.mtrl";

        // chara/weapon/w0101/animation/s0002/body/material.pap
        public string GetMaterialPap( int id ) => $"chara/weapon/{ModelString}/animation/s" + id.ToString().PadLeft( 4, '0' ) + "/body/material.pap";
    }
}