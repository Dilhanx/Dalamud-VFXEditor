using System.IO;
using System.Linq;
using VfxEditor.AVFXLib;
using VfxEditor.AVFXLib.Particle;

namespace VfxEditor.AVFX.VFX {
    public class UIParticleView : UINodeDropdownView<UIParticle> {
        public UIParticleView( AVFXFile vfxFile, AVFXMain avfx, UINodeGroup<UIParticle> group ) : base( vfxFile, avfx, group, "Particle", true, true, "default_particle.vfxedit" ) { }

        public override void OnDelete( UIParticle item ) => AVFX.RemoveParticle( item.Particle );

        public override void OnExport( BinaryWriter writer, UIParticle item ) => item.Write( writer );

        public override UIParticle OnImport( BinaryReader reader, int size, bool has_dependencies = false ) {
            var item = new AVFXParticle();
            item.Read( reader, size );
            AVFX.AddParticle( item );
            return new UIParticle( item, VfxFile.NodeGroupSet, has_dependencies );
        }

        public override void OnSelect( UIParticle item ) { }
    }
}
